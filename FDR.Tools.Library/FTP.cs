using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;

namespace FDR.Tools.Library.FTP
{
    public enum FTPMode
    {
        Passive = 1,
        Active = 2
    }

    public enum FTPFileTransferType
    {
        ASCII = 1,
        Binary = 2
    }

    public class FTPFile
    {
        public string? Name { get; set; }
    }

    public sealed class FTPConnection
    {
        private TcpClient? mobjTcpClient;
        private static int mintsBlockSize = 512;
        private static int mintsDefaultRemotePort = 21;
        private static int mintsDataPortRangeFrom = 1500;
        private static int mintsDataPortRangeTo = 65000;
        private FTPMode mintFtpMode;
        private int mintActiveConnectionsCount;
        private string? mstrRemoteHost;

        private readonly List<string> mcolMessageList = new List<string>();
        private bool mlogMessages;

        public FTPConnection()
        {
            mintActiveConnectionsCount = 0;
            mintFtpMode = FTPMode.Active;
            mlogMessages = false;
        }

        public List<string> MessageList
        {
            get { return mcolMessageList; }
        }

        public bool LogMessages
        {
            get { return mlogMessages; }
            set
            {
                if (!value) mcolMessageList.Clear();
                mlogMessages = value;
            }
        }

        public void Open(string tstrRemoteHost, string tstrUser, string tstrPassword)
        {
            Open(tstrRemoteHost, mintsDefaultRemotePort, tstrUser, tstrPassword, FTPMode.Active);
        }
        public void Open(string tstrRemoteHost, string tstrUser, string tstrPassword, FTPMode tintMode)
        {
            Open(tstrRemoteHost, mintsDefaultRemotePort, tstrUser, tstrPassword, tintMode);
        }
        public void Open(string tstrRemoteHost, int tintRemotePort, string tstrUser, string tstrPassword)
        {
            Open(tstrRemoteHost, tintRemotePort, tstrUser, tstrPassword, FTPMode.Active);
        }
        public void Open(string tstrRemoteHost, int tintRemotePort, string tstrUser, string tstrPassword, FTPMode tintMode)
        {
            List<string> lcolTempMessageList;
            int lintReturn;

            mintFtpMode = tintMode;
            mobjTcpClient = new TcpClient();
            mstrRemoteHost = tstrRemoteHost;

            // As we cannot detect the local address from the TCPClient class, convert "127.0.0.1" and "localhost" to 
            // the DNS record of this machine; this will ensure that the connection address and the PORT command address 
            // are identical. This fixes bug 854919. 
            if (tstrRemoteHost == "localhost" || tstrRemoteHost == "127.0.0.1")
                tstrRemoteHost = GetLocalAddressList()[0].ToString();

            // CONNECT
            try
            {
                mobjTcpClient.Connect(tstrRemoteHost, tintRemotePort);
            }
            catch (Exception ex)
            {
                throw new IOException("Couldn't connect to remote server!", ex);
            }
            lcolTempMessageList = Read();
            lintReturn = GetMessageReturnValue(lcolTempMessageList);
            if (lintReturn != 220)
            {
                Close();
                throw new Exception(lcolTempMessageList[0]);
            }

            // SEND USER 
            lcolTempMessageList = SendCommand("USER " + tstrUser);
            lintReturn = GetMessageReturnValue(lcolTempMessageList);
            if (!(lintReturn == 331 || lintReturn == 202))
            {
                Close();
                throw new Exception(lcolTempMessageList[0]);
            }

            // SEND PASSWORD 
            if (lintReturn == 331)
            {
                lcolTempMessageList = SendCommand("PASS " + tstrPassword);
                lintReturn = GetMessageReturnValue(lcolTempMessageList);
                if (!(lintReturn == 230 || lintReturn == 202))
                {
                    Close();
                    throw new Exception(lcolTempMessageList[0]);
                }
            }
        }

        public void Close()
        {
            if (mobjTcpClient != null)
            {
                SendCommand("QUIT");
                mobjTcpClient.Close();
                mobjTcpClient = null;
            }
        }

        public List<string> Dir(string tstrMask)
        {
            List<string> lcolTmpList = Dir();

            using (System.Data.DataTable lobjTable = new System.Data.DataTable())
            {
                lobjTable.Columns.Add("Name");
                for (int i = 0; i <= lcolTmpList.Count - 1; i++)
                {
                    System.Data.DataRow lobjRow = lobjTable.NewRow();
                    lobjRow["Name"] = lcolTmpList[i];
                    lobjTable.Rows.Add(lobjRow);
                }
                lcolTmpList.Clear();

                System.Data.DataRow[] lobjaRowList = lobjTable.Select("Name LIKE '" + tstrMask + "'", "", System.Data.DataViewRowState.CurrentRows);
                for (int I = 0; I <= lobjaRowList.Length - 1; I++)
                    lcolTmpList.Add((string)lobjaRowList[I]["Name"]);
                lobjaRowList = new System.Data.DataRow[0];
            }

            return lcolTmpList;
        }

        public List<string> Dir()
        {
            LockTcpClient();

            TcpListener? lobjListener = null;
            TcpClient? lobjClient = null;
            List<string> lcolTempMessageList;
            int lintReturn;
            List<string>? lcolFileList;

            SetTransferType(FTPFileTransferType.ASCII);

            if (mintFtpMode == FTPMode.Active)
            {
                lobjListener = CreateDataListner();
                lobjListener.Start();
            }
            else
                lobjClient = CreateDataClient();

            lcolTempMessageList = SendCommand("NLST");
            lintReturn = GetMessageReturnValue(lcolTempMessageList);
            if (!(lintReturn == 150 || lintReturn == 125 || lintReturn == 550))
                throw new Exception(lcolTempMessageList[0]);

            if (lintReturn == 550)
                // No files found 
                return new List<string>();

            if (mintFtpMode == FTPMode.Active)
                lobjClient = lobjListener?.AcceptTcpClient();

            using (NetworkStream lobjNetworkStream = lobjClient!.GetStream())
            {
                lcolFileList = ReadLines(lobjNetworkStream);

                string lstrReturnMessage = "";
                if (lcolTempMessageList?.Count == 1)
                {
                    lcolTempMessageList = Read();
                    lstrReturnMessage = lcolTempMessageList[0];
                    lintReturn = GetMessageReturnValue(lstrReturnMessage);
                }
                else if (lcolTempMessageList?.Count > 1)
                {
                    lstrReturnMessage = lcolTempMessageList[1];
                    lintReturn = GetMessageReturnValue(lstrReturnMessage);
                }

                if (lintReturn != 226)
                    throw new Exception(lstrReturnMessage);

                lobjNetworkStream.Close();
            }
            lobjClient.Close();

            if (mintFtpMode == FTPMode.Active)
                lobjListener?.Stop();
            UnlockTcpClient();
            return lcolFileList;
        }

        public void SendStream(Stream tobjStream, string tstrRemoteFileName, FTPFileTransferType tintType)
        {
            LockTcpClient();

            TcpListener? lobjListener = null;
            TcpClient? lobjClient = null;
            List<string> lcolTempMessageList = new List<string>();
            int lintReturn;

            SetTransferType(tintType);

            if (mintFtpMode == FTPMode.Active)
            {
                lobjListener = CreateDataListner();
                lobjListener.Start();
            }
            else
                lobjClient = CreateDataClient();

            lcolTempMessageList = SendCommand("STOR " + tstrRemoteFileName);
            lintReturn = GetMessageReturnValue(lcolTempMessageList);
            if (!(lintReturn == 150 || lintReturn == 125))
                throw new Exception(lcolTempMessageList[0]);

            if (mintFtpMode == FTPMode.Active)
                lobjClient = lobjListener?.AcceptTcpClient();

            using (NetworkStream lobjNetworkStream = lobjClient!.GetStream())
            {
                byte[] lbytaBuffer = new byte[mintsBlockSize - 1 + 1];
                int lintTotalBytes = 0;

                while (lintTotalBytes < tobjStream.Length)
                {
                    int lintBytes = Convert.ToInt32(tobjStream.Read(lbytaBuffer, 0, mintsBlockSize));
                    lintTotalBytes += lintBytes;
                    lobjNetworkStream.Write(lbytaBuffer, 0, lintBytes);
                }

                lobjNetworkStream.Close();
            }
            lobjClient.Close();

            if (mintFtpMode == FTPMode.Active)
                lobjListener?.Stop();

            string lstrReturnMessage = "";
            if (lcolTempMessageList.Count == 1)
            {
                lcolTempMessageList = Read();
                lstrReturnMessage = lcolTempMessageList[0];
                lintReturn = GetMessageReturnValue(lstrReturnMessage);
            }
            else if (lcolTempMessageList.Count > 1)
            {
                lstrReturnMessage = lcolTempMessageList[1];
                lintReturn = GetMessageReturnValue(lstrReturnMessage);
            }

            if (lintReturn != 226)
                throw new Exception(lstrReturnMessage);
            UnlockTcpClient();
        }

        public void SendFile(string tstrLocalFileName, FTPFileTransferType tintType)
        {
            SendFile(tstrLocalFileName, Path.GetFileName(tstrLocalFileName), tintType);
        }

        public void SendFile(string tstrLocalFileName, string tstrRemoteFileName, FTPFileTransferType tintType)
        {
            using (FileStream lobjFS = new FileStream(tstrLocalFileName, FileMode.Open))
            {
                SendStream(lobjFS, tstrRemoteFileName, tintType);
                lobjFS.Close();
            }
        }

        public void GetStream(string tstrRemoteFileName, Stream tobjStream, FTPFileTransferType tintType)
        {
            TcpListener? lobjListener = null;
            TcpClient? lobjClient = null;
            List<string> lcolTempMessageList;
            int lintReturn;

            LockTcpClient();

            SetTransferType(tintType);

            if (mintFtpMode == FTPMode.Active)
            {
                lobjListener = CreateDataListner();
                lobjListener.Start();
            }
            else
                lobjClient = CreateDataClient();

            lcolTempMessageList = SendCommand("RETR " + tstrRemoteFileName);
            lintReturn = GetMessageReturnValue(lcolTempMessageList);
            if (!(lintReturn == 150 || lintReturn == 125))
                throw new Exception(lcolTempMessageList[0]);

            if (mintFtpMode == FTPMode.Active)
                lobjClient = lobjListener?.AcceptTcpClient();

            using (NetworkStream lobjNetworkStream = lobjClient!.GetStream())
            {
                byte[] lbytaBuffer = new byte[mintsBlockSize - 1 + 1];

                bool llogRead = true;
                while (llogRead)
                {
                    int lintBytes = Convert.ToInt32(lobjNetworkStream.Read(lbytaBuffer, 0, lbytaBuffer.Length));
                    tobjStream.Write(lbytaBuffer, 0, lintBytes);
                    if (lintBytes == 0)
                        llogRead = false;
                }

                lobjNetworkStream.Close();
            }
            lobjClient.Close();

            if (mintFtpMode == FTPMode.Active)
                lobjListener?.Stop();

            string lstrReturnMessage = "";
            if (lcolTempMessageList.Count == 1)
            {
                lcolTempMessageList = Read();
                lstrReturnMessage = lcolTempMessageList[0];
                lintReturn = GetMessageReturnValue(lstrReturnMessage);
            }
            else if (lcolTempMessageList.Count > 1)
            {
                lstrReturnMessage = lcolTempMessageList[1];
                lintReturn = GetMessageReturnValue(lstrReturnMessage);
            }

            if (lintReturn != 226)
                throw new Exception(lstrReturnMessage);

            UnlockTcpClient();
        }

        public void GetFile(string tstrRemoteFileName, FTPFileTransferType tintType)
        {
            GetFile(tstrRemoteFileName, Path.GetFileName(tstrRemoteFileName), tintType);
        }
        public void GetFile(string tstrRemoteFileName, string tstrLocalFileName, FTPFileTransferType tintType)
        {
            using (FileStream lobjFS = new FileStream(tstrLocalFileName, FileMode.Create))
            {
                GetStream(tstrRemoteFileName, lobjFS, tintType);
                lobjFS.Close();
            }
        }

        public void DeleteFile(string tstrRemoteFileName)
        {
            LockTcpClient();
            List<string> lcolTempMessageList;
            int lintReturn;
            lcolTempMessageList = SendCommand("DELE " + tstrRemoteFileName);
            lintReturn = GetMessageReturnValue(lcolTempMessageList);
            if (lintReturn != 250)
                throw new Exception(lcolTempMessageList[0]);
            UnlockTcpClient();
        }

        public void MoveFile(string tstrRemoteFileName, string tstrToRemotePath)
        {
            if (tstrToRemotePath.Length > 0 && !tstrToRemotePath.EndsWith("/"))
                tstrToRemotePath += "/";
            RenameFile(tstrRemoteFileName, tstrToRemotePath + tstrRemoteFileName);
        }

        public void RenameFile(string tstrFromRemoteFileName, string tstrToRemoteFileName)
        {
            LockTcpClient();
            List<string> lcolTempMessageList;
            int lintReturn;
            lcolTempMessageList = SendCommand("RNFR " + tstrFromRemoteFileName);
            lintReturn = GetMessageReturnValue(lcolTempMessageList);
            if (lintReturn != 350)
                throw new Exception(lcolTempMessageList[0]);
            lcolTempMessageList = SendCommand("RNTO " + tstrToRemoteFileName);
            lintReturn = GetMessageReturnValue(lcolTempMessageList);
            if (lintReturn != 250)
                throw new Exception(lcolTempMessageList[0]);
            UnlockTcpClient();
        }

        public void SetCurrentDirectory(string tstrRemotePath)
        {
            LockTcpClient();
            List<string> lcolTempMessageList;
            int lintReturn;
            lcolTempMessageList = SendCommand("CWD " + tstrRemotePath);
            lintReturn = GetMessageReturnValue(lcolTempMessageList);
            if (lintReturn != 250)
                throw new Exception(lcolTempMessageList[0]);
            UnlockTcpClient();
        }

        private void SetTransferType(FTPFileTransferType tintType)
        {
            switch (tintType)
            {
                case FTPFileTransferType.ASCII:
                    SetMode("TYPE A");
                    break;

                case FTPFileTransferType.Binary:
                    SetMode("TYPE I");
                    break;

                default:
                    throw new Exception("Invalid File Transfer Type");
            }
        }

        private void SetMode(string tstrMode)
        {
            LockTcpClient();
            List<string> lcolTempMessageList;
            int lintReturn;
            lcolTempMessageList = SendCommand(tstrMode);
            lintReturn = GetMessageReturnValue(lcolTempMessageList);
            if (lintReturn != 200)
                throw new Exception(lcolTempMessageList[0]);
            UnlockTcpClient();
        }

        private TcpListener CreateDataListner()
        {
            int lintPort = GetPortNumber();
            SetDataPort(lintPort);
            IPHostEntry lobjLocalHost = Dns.GetHostEntry(Dns.GetHostName());
            TcpListener lobjListener = new TcpListener(lobjLocalHost.AddressList[0], lintPort);
            return lobjListener;
        }

        private TcpClient CreateDataClient()
        {
            if (mstrRemoteHost == null)
                throw new ArgumentNullException(nameof(mstrRemoteHost));
            int lintPort = GetPortNumber();
            TcpClient lobjClient = new TcpClient();
            lobjClient.Connect(mstrRemoteHost, lintPort);
            return lobjClient;
        }

        private void SetDataPort(int tintPortNumber)
        {
            LockTcpClient();

            List<string> lcolTempMessageList;
            int lintReturn;
            int lintPortHigh = tintPortNumber >> 8;
            int lintPortLow = tintPortNumber & 255;

            lcolTempMessageList = SendCommand((("PORT " + GetLocalAddressList()[0].ToString().Replace(".", ",") + ",") + lintPortHigh + ",") + lintPortLow);
            lintReturn = GetMessageReturnValue(lcolTempMessageList);
            if (lintReturn != 200)
                throw new Exception(lcolTempMessageList[0]);
            UnlockTcpClient();
        }

        public void MakeDir(string tstrDirectoryName)
        {
            LockTcpClient();

            List<string> lcolTempMessageList;
            int lintReturn;

            lcolTempMessageList = SendCommand("MKD " + tstrDirectoryName);
            lintReturn = GetMessageReturnValue(lcolTempMessageList);
            if (lintReturn != 257)
                throw new Exception(lcolTempMessageList[0]);

            UnlockTcpClient();
        }

        public void RemoveDir(string tstrDirectoryName)
        {
            LockTcpClient();

            List<string> lcolTempMessageList;
            int lintReturn;

            lcolTempMessageList = SendCommand("RMD " + tstrDirectoryName);
            lintReturn = GetMessageReturnValue(lcolTempMessageList);
            if (lintReturn != 250)
                throw new Exception(lcolTempMessageList[0]);

            UnlockTcpClient();
        }

        public List<string> SendCommand(string tstrCommand)
        {
            NetworkStream? lobjStream = mobjTcpClient?.GetStream();
            mintActiveConnectionsCount += 1;

            byte[] lbytaCmdBytes = Encoding.ASCII.GetBytes((tstrCommand + Environment.NewLine).ToCharArray());
            lobjStream?.Write(lbytaCmdBytes, 0, lbytaCmdBytes.Length);

            mintActiveConnectionsCount -= 1;

            if (tstrCommand == "QUIT")
                return new List<string>();
            return Read();
        }

        private List<string> Read()
        {
            NetworkStream lobjStream = mobjTcpClient!.GetStream();
            List<string> lcolMessageList = new List<string>();
            List<string> lcolTempMessage = ReadLines(lobjStream);

            int lintTryCount = 0;
            while (lcolTempMessage.Count == 0)
            {
                if (lintTryCount == 100)
                    throw new Exception("Server does not return message to the message");
                Thread.Sleep(100);
                lintTryCount += 1;
                lcolTempMessage = ReadLines(lobjStream);
            }

            while (lcolTempMessage[lcolTempMessage.Count - 1].ToString().Substring(3, 1) == "-")
            {
                lcolMessageList.AddRange(lcolTempMessage);
                lcolTempMessage = ReadLines(lobjStream);
            }
            lcolMessageList.AddRange(lcolTempMessage);

            AddMessagesToMessageList(lcolMessageList);

            return lcolMessageList;
        }

        private List<string> ReadLines(NetworkStream tobjStream)
        {
            List<string> lcolMessageList = new List<string>();
            char[] lchraSeperator = new[] { (char)10 };
            char[] lchaToRemove = new[] { (char)13 };
            byte[] lbytaBuffer = new byte[mintsBlockSize - 1 + 1];
            int lintBytes;
            string lstrTmpMes = "";

            while (tobjStream.DataAvailable)
            {
                lintBytes = tobjStream.Read(lbytaBuffer, 0, lbytaBuffer.Length);
                lstrTmpMes += Encoding.ASCII.GetString(lbytaBuffer, 0, lintBytes);
            }

            string[] lstraMess = lstrTmpMes.Split(lchraSeperator);
            for (int I = 0; I <= lstraMess.Length - 1; I++)
            {
                if (lstraMess[I].Length > 0)
                    lcolMessageList.Add(lstraMess[I].Trim(lchaToRemove));
            }

            return lcolMessageList;
        }

        private int GetMessageReturnValue(List<string>? tcolMessages)
        {
            if (tcolMessages == null || tcolMessages.Count == 0)
                throw new ArgumentNullException(nameof(tcolMessages));
            return GetMessageReturnValue(tcolMessages[0]);
        }
        private int GetMessageReturnValue(string? tstrMessage)
        {
            if (string.IsNullOrWhiteSpace(tstrMessage))
                throw new ArgumentException(nameof(tstrMessage));
            return int.Parse(tstrMessage.Substring(0, 3));
        }

        private int GetPortNumber()
        {
            LockTcpClient();

            int lintPort = 0;
            try
            {
                switch (mintFtpMode)
                {
                    case FTPMode.Active:
                        Random lobjRnd = new Random((int)DateTime.Now.Ticks);
                        lintPort = mintsDataPortRangeFrom + lobjRnd.Next(mintsDataPortRangeTo - mintsDataPortRangeFrom);
                        break;

                    case FTPMode.Passive:
                        List<string> lcolTempMessageList;
                        int lintReturn;
                        lcolTempMessageList = SendCommand("PASV");
                        lintReturn = GetMessageReturnValue(lcolTempMessageList);
                        if (lintReturn != 227)
                        {
                            if (lcolTempMessageList[0].ToString().Length > 4)
                                throw new Exception(lcolTempMessageList[0]);
                            else
                                throw new Exception(lcolTempMessageList[0] + " Passive Mode not implemented");
                        }
                        string lstrMessage = lcolTempMessageList[0].ToString();
                        int lintIndex1 = lstrMessage.IndexOf(",", 0);
                        int lintIndex2 = lstrMessage.IndexOf(",", lintIndex1 + 1);
                        int lintIndex3 = lstrMessage.IndexOf(",", lintIndex2 + 1);
                        int lintIndex4 = lstrMessage.IndexOf(",", lintIndex3 + 1);
                        int lintIndex5 = lstrMessage.IndexOf(",", lintIndex4 + 1);
                        int lintIndex6 = lstrMessage.IndexOf(")", lintIndex5 + 1);
                        lintPort = 256 * int.Parse(lstrMessage.Substring(lintIndex4 + 1, lintIndex5 - lintIndex4 - 1)) + int.Parse(lstrMessage.Substring(lintIndex5 + 1, lintIndex6 - lintIndex5 - 1));
                        break;
                }
            }
            finally
            {
                UnlockTcpClient();
            }
            return lintPort;
        }

        private void AddMessagesToMessageList(List<string> tcolMessages)
        {
            if (mlogMessages)
                mcolMessageList.AddRange(tcolMessages);
        }

        private IPAddress[] GetLocalAddressList()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        }

        private void LockTcpClient()
        {
            if (mobjTcpClient != null)
                Monitor.Enter(mobjTcpClient);
        }

        private void UnlockTcpClient()
        {
            if (mobjTcpClient != null)
                Monitor.Exit(mobjTcpClient);
        }
    }
}