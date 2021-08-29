Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic


Public Enum FTPMode As Integer
    Passive = 1
    Active = 2
End Enum


Public Enum FTPFileTransferType As Integer
    ASCII = 1
    Binary = 2
End Enum


Public Class FTPFile

    Private mstrName As String

    Public Sub New()
    End Sub

    Public Property Name() As String
        Get
            Return mstrName
        End Get
        Set(ByVal value As String)
            mstrName = value
        End Set
    End Property

End Class


Public Class FTPConnection

    Private mobjTcpClient As System.Net.Sockets.TcpClient
    Private Shared mintsBlockSize As Integer = 512
    Private Shared mintsDefaultRemotePort As Integer = 21
    Private Shared mintsDataPortRangeFrom As Integer = 1500
    Private Shared mintsDataPortRangeTo As Integer = 65000
    Private mintFtpMode As FTPMode
    Private mintActiveConnectionsCount As Integer
    Private mstrRemoteHost As String

    Private mcolMessageList As New System.Collections.ArrayList()
    Private mlogMessages As Boolean

    Public Sub New()
        mintActiveConnectionsCount = 0
        mintFtpMode = FTPMode.Active
        mlogMessages = False
    End Sub

    Public ReadOnly Property MessageList() As System.Collections.ArrayList
        Get
            Return mcolMessageList
        End Get
    End Property

    Public Property LogMessages() As Boolean
        Get
            Return mlogMessages
        End Get
        Set(ByVal value As Boolean)
            If Not value Then
                'mcolMessageList = New ArrayList()
                mcolMessageList.Clear()
            End If
            mlogMessages = value
        End Set
    End Property

    Public Overridable Sub Open(ByVal tstrRemoteHost As String, ByVal tstrUser As String, ByVal tstrPassword As String)
        Me.Open(tstrRemoteHost, mintsDefaultRemotePort, tstrUser, tstrPassword, FTPMode.Active)
    End Sub
    Public Overridable Sub Open(ByVal tstrRemoteHost As String, ByVal tstrUser As String, ByVal tstrPassword As String, ByVal tintMode As FTPMode)
        Me.Open(tstrRemoteHost, mintsDefaultRemotePort, tstrUser, tstrPassword, tintMode)
    End Sub
    Public Overridable Sub Open(ByVal tstrRemoteHost As String, ByVal tintRemotePort As Integer, ByVal tstrUser As String, ByVal tstrPassword As String)
        Me.Open(tstrRemoteHost, tintRemotePort, tstrUser, tstrPassword, FTPMode.Active)
    End Sub
    Public Overridable Sub Open(ByVal tstrRemoteHost As String, ByVal tintRemotePort As Integer, ByVal tstrUser As String, ByVal tstrPassword As String, ByVal tintMode As FTPMode)
        Dim lcolTempMessageList As System.Collections.ArrayList
        Dim lintReturn As Integer

        mintFtpMode = tintMode
        mobjTcpClient = New System.Net.Sockets.TcpClient()
        mstrRemoteHost = tstrRemoteHost

        ' As we cannot detect the local address from the TCPClient class, convert "127.0.0.1" and "localhost" to 
        ' the DNS record of this machine; this will ensure that the connection address and the PORT command address 
        ' are identical. This fixes bug 854919. 
        If tstrRemoteHost = "localhost" OrElse tstrRemoteHost = "127.0.0.1" Then
            tstrRemoteHost = Me.GetLocalAddressList(0).ToString()
        End If

        'CONNECT
        Try
            mobjTcpClient.Connect(tstrRemoteHost, tintRemotePort)
        Catch ex As System.Exception
            Throw New System.IO.IOException("Couldn't connect to remote server!", ex)
        End Try
        lcolTempMessageList = Me.Read()
        lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
        If lintReturn <> 220 Then
            Me.Close()
            Throw New System.Exception(CStr(lcolTempMessageList(0)))
        End If

        'SEND USER 
        lcolTempMessageList = Me.SendCommand("USER " & tstrUser)
        lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
        If Not (lintReturn = 331 OrElse lintReturn = 202) Then
            Me.Close()
            Throw New System.Exception(CStr(lcolTempMessageList(0)))
        End If

        'SEND PASSWORD 
        If lintReturn = 331 Then
            lcolTempMessageList = Me.SendCommand("PASS " & tstrPassword)
            lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
            If Not (lintReturn = 230 OrElse lintReturn = 202) Then
                Me.Close()
                Throw New System.Exception(CStr(lcolTempMessageList(0)))
            End If
        End If
    End Sub

    Public Overridable Sub Close()
        If mobjTcpClient IsNot Nothing Then
            Me.SendCommand("QUIT")
            mobjTcpClient.Close()
            mobjTcpClient = Nothing
        End If
    End Sub

    Public Function Dir(ByVal tstrMask As String) As System.Collections.ArrayList
        Dim lcolTmpList As System.Collections.ArrayList = Me.Dir()

        Using lobjTable As New System.Data.DataTable()
            lobjTable.Columns.Add("Name")
            For I As Integer = 0 To lcolTmpList.Count - 1
                Dim lobjRow As System.Data.DataRow = lobjTable.NewRow()
                lobjRow("Name") = CStr(lcolTmpList(I))
                lobjTable.Rows.Add(lobjRow)
            Next
            lcolTmpList.Clear()

            Dim lobjaRowList As System.Data.DataRow() = lobjTable.Select("Name LIKE '" & tstrMask & "'", "", System.Data.DataViewRowState.CurrentRows)
            lcolTmpList = New System.Collections.ArrayList()
            For I As Integer = 0 To lobjaRowList.Length - 1
                lcolTmpList.Add(CStr(lobjaRowList(I)("Name")))
            Next
            ReDim lobjaRowList(-1)
        End Using

        Return lcolTmpList
    End Function

    Public Function Dir() As System.Collections.ArrayList
        Me.LockTcpClient()

        Dim lobjListener As System.Net.Sockets.TcpListener = Nothing
        Dim lobjClient As System.Net.Sockets.TcpClient = Nothing
        Dim lcolTempMessageList As System.Collections.ArrayList
        Dim lintReturn As Integer
        Dim lcolFileList As System.Collections.ArrayList

        Me.SetTransferType(FTPFileTransferType.ASCII)

        If mintFtpMode = FTPMode.Active Then
            lobjListener = Me.CreateDataListner()
            lobjListener.Start()
        Else
            lobjClient = Me.CreateDataClient()
        End If

        lcolTempMessageList = Me.SendCommand("NLST")
        lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
        If Not (lintReturn = 150 OrElse lintReturn = 125 OrElse lintReturn = 550) Then
            Throw New System.Exception(CStr(lcolTempMessageList(0)))
        End If

        If lintReturn = 550 Then
            'No files found 
            Return New System.Collections.ArrayList()
        End If

        If Me.mintFtpMode = FTPMode.Active Then
            lobjClient = lobjListener.AcceptTcpClient()
        End If

        Using lobjNetworkStream As System.Net.Sockets.NetworkStream = lobjClient.GetStream()

            lcolFileList = Me.ReadLines(lobjNetworkStream)

            Dim lstrReturnMessage As String
            If lcolTempMessageList.Count = 1 Then
                lcolTempMessageList = Me.Read()
                lstrReturnMessage = CStr(lcolTempMessageList(0))
                lintReturn = Me.GetMessageReturnValue(lstrReturnMessage)
            Else
                lstrReturnMessage = CStr(lcolTempMessageList(1))
                lintReturn = Me.GetMessageReturnValue(lstrReturnMessage)
            End If

            If lintReturn <> 226 Then
                Throw New System.Exception(lstrReturnMessage)
            End If

            lobjNetworkStream.Close()
        End Using
        lobjClient.Close()

        If Me.mintFtpMode = FTPMode.Active Then
            lobjListener.Stop()
        End If
        Me.UnlockTcpClient()
        Return lcolFileList
    End Function

    Public Function XDir(ByVal tstrMask As String, ByVal tintOnColumnIndex As Integer) As System.Collections.ArrayList
        Dim lcolNewList As New System.Collections.ArrayList()
        Dim lcolTmpList As System.Collections.ArrayList = Me.XDir()

        Using lobjTable As New System.Data.DataTable()
            lobjTable.Columns.Add("Name")
            For I As Integer = 0 To lcolTmpList.Count - 1
                Dim lobjRow As System.Data.DataRow = lobjTable.NewRow()
                lobjRow("Name") = CStr(CType(lcolTmpList(I), System.Collections.ArrayList)(tintOnColumnIndex))
                lobjTable.Rows.Add(lobjRow)
            Next

            Dim lobjaRowList As System.Data.DataRow() = lobjTable.Select("Name LIKE '" & tstrMask & "'", "", System.Data.DataViewRowState.CurrentRows)
            For I As Integer = 0 To lobjaRowList.Length - 1
                For J As Integer = 0 To lcolTmpList.Count - 1
                    If CStr(lobjaRowList(I)("Name")) = CStr(CType(lcolTmpList(J), System.Collections.ArrayList)(tintOnColumnIndex)) Then
                        lcolNewList.Add(CType(lcolTmpList(J), System.Collections.ArrayList))
                    End If
                Next
            Next
            ReDim lobjaRowList(-1)
        End Using

        Return lcolNewList
    End Function

    Public Function XDir() As System.Collections.ArrayList
        Me.LockTcpClient()

        Dim lobjListener As System.Net.Sockets.TcpListener = Nothing
        Dim lobjClient As System.Net.Sockets.TcpClient = Nothing
        Dim lcolTempMessageList As System.Collections.ArrayList
        Dim lintReturn As Integer
        Dim lcolFilesAndFolderList As New System.Collections.ArrayList()
        Dim lcolTmpFilesAndFolderList As System.Collections.ArrayList

        Me.SetTransferType(FTPFileTransferType.ASCII)

        If mintFtpMode = FTPMode.Active Then
            lobjListener = Me.CreateDataListner()
            lobjListener.Start()
        Else
            lobjClient = Me.CreateDataClient()
        End If

        lcolTempMessageList = Me.SendCommand("LIST")
        lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
        If Not (lintReturn = 150 OrElse lintReturn = 125) Then
            Throw New System.Exception(CStr(lcolTempMessageList(0)))
        End If

        If mintFtpMode = FTPMode.Active Then
            lobjClient = lobjListener.AcceptTcpClient()
        End If

        Using lobjNetworkStream As System.Net.Sockets.NetworkStream = lobjClient.GetStream()

            lcolTmpFilesAndFolderList = Me.ReadLines(lobjNetworkStream)

            Dim lstrReturnMessage As String
            If lcolTempMessageList.Count = 1 Then
                lcolTempMessageList = Me.Read()
                lstrReturnMessage = CStr(lcolTempMessageList(0))
                lintReturn = Me.GetMessageReturnValue(lstrReturnMessage)
            Else
                lstrReturnMessage = CStr(lcolTempMessageList(1))
                lintReturn = Me.GetMessageReturnValue(lstrReturnMessage)
            End If

            If lintReturn <> 226 Then
                Throw New System.Exception(lstrReturnMessage)
            End If

            lobjNetworkStream.Close()
        End Using
        lobjClient.Close()

        If mintFtpMode = FTPMode.Active Then
            lobjListener.Stop()
        End If

        Me.UnlockTcpClient()
        For Each lstrItem As String In lcolTmpFilesAndFolderList
            lcolFilesAndFolderList.Add(Me.GetTokens(lstrItem, " "))
        Next
        Return lcolFilesAndFolderList
    End Function

    Public Sub SendStream(ByVal tobjStream As System.IO.Stream, ByVal tstrRemoteFileName As String, ByVal tintType As FTPFileTransferType)
        Me.LockTcpClient()

        Dim lobjListener As System.Net.Sockets.TcpListener = Nothing
        Dim lobjClient As System.Net.Sockets.TcpClient = Nothing
        Dim lcolTempMessageList As New System.Collections.ArrayList()
        Dim lintReturn As Integer

        Me.SetTransferType(tintType)

        If mintFtpMode = FTPMode.Active Then
            lobjListener = Me.CreateDataListner()
            lobjListener.Start()
        Else
            lobjClient = Me.CreateDataClient()
        End If

        lcolTempMessageList = Me.SendCommand("STOR " & tstrRemoteFileName)
        lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
        If Not (lintReturn = 150 OrElse lintReturn = 125) Then
            Throw New System.Exception(CStr(lcolTempMessageList(0)))
        End If

        If mintFtpMode = FTPMode.Active Then
            lobjClient = lobjListener.AcceptTcpClient()
        End If

        Using lobjNetworkStream As System.Net.Sockets.NetworkStream = lobjClient.GetStream()

            Dim lbytaBuffer As Byte() = New Byte(mintsBlockSize - 1) {}
            Dim lintTotalBytes As Integer = 0

            While lintTotalBytes < tobjStream.Length
                Dim lintBytes As Integer = CInt(tobjStream.Read(lbytaBuffer, 0, mintsBlockSize))
                lintTotalBytes += lintBytes
                lobjNetworkStream.Write(lbytaBuffer, 0, lintBytes)
            End While

            lobjNetworkStream.Close()
        End Using
        lobjClient.Close()

        If mintFtpMode = FTPMode.Active Then
            lobjListener.Stop()
        End If

        Dim lstrReturnMessage As String

        If lcolTempMessageList.Count = 1 Then
            lcolTempMessageList = Me.Read()
            lstrReturnMessage = CStr(lcolTempMessageList(0))
            lintReturn = Me.GetMessageReturnValue(lstrReturnMessage)
        Else
            lstrReturnMessage = CStr(lcolTempMessageList(1))
            lintReturn = Me.GetMessageReturnValue(lstrReturnMessage)
        End If

        If lintReturn <> 226 Then
            Throw New System.Exception(lstrReturnMessage)
        End If
        Me.UnlockTcpClient()
    End Sub

    Public Overridable Sub SendFile(ByVal tstrLocalFileName As String, ByVal tintType As FTPFileTransferType)
        Me.SendFile(tstrLocalFileName, System.IO.Path.GetFileName(tstrLocalFileName), tintType)
    End Sub

    Public Overridable Sub SendFile(ByVal tstrLocalFileName As String, ByVal tstrRemoteFileName As String, ByVal tintType As FTPFileTransferType)
        Using lobjFS As New System.IO.FileStream(tstrLocalFileName, System.IO.FileMode.Open)
            Me.SendStream(lobjFS, tstrRemoteFileName, tintType)
            lobjFS.Close()
        End Using
    End Sub

    Public Sub GetStream(ByVal tstrRemoteFileName As String, ByVal tobjStream As System.IO.Stream, ByVal tintType As FTPFileTransferType)
        Dim lobjListener As System.Net.Sockets.TcpListener = Nothing
        Dim lobjClient As System.Net.Sockets.TcpClient = Nothing
        Dim lcolTempMessageList As System.Collections.ArrayList
        Dim lintReturn As Integer

        Me.LockTcpClient()

        Me.SetTransferType(tintType)

        If mintFtpMode = FTPMode.Active Then
            lobjListener = Me.CreateDataListner()
            lobjListener.Start()
        Else
            lobjClient = Me.CreateDataClient()
        End If

        lcolTempMessageList = Me.SendCommand("RETR " & tstrRemoteFileName)
        lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
        If Not (lintReturn = 150 OrElse lintReturn = 125) Then
            Throw New System.Exception(CStr(lcolTempMessageList(0)))
        End If

        If mintFtpMode = FTPMode.Active Then
            lobjClient = lobjListener.AcceptTcpClient()
        End If

        Using lobjNetworkStream As System.Net.Sockets.NetworkStream = lobjClient.GetStream()

            Dim lbytaBuffer As Byte() = New Byte(mintsBlockSize - 1) {}

            Dim llogRead As Boolean = True
            While llogRead
                Dim lintBytes As Integer = CInt(lobjNetworkStream.Read(lbytaBuffer, 0, lbytaBuffer.Length))
                tobjStream.Write(lbytaBuffer, 0, lintBytes)
                If lintBytes = 0 Then
                    llogRead = False
                End If
            End While

            lobjNetworkStream.Close()
        End Using
        lobjClient.Close()

        If mintFtpMode = FTPMode.Active Then
            lobjListener.Stop()
        End If

        Dim lstrReturnMessage As String

        If lcolTempMessageList.Count = 1 Then
            lcolTempMessageList = Me.Read()
            lstrReturnMessage = CStr(lcolTempMessageList(0))
            lintReturn = Me.GetMessageReturnValue(lstrReturnMessage)
        Else
            lstrReturnMessage = CStr(lcolTempMessageList(1))
            lintReturn = Me.GetMessageReturnValue(lstrReturnMessage)
        End If

        If lintReturn <> 226 Then
            Throw New System.Exception(lstrReturnMessage)
        End If

        Me.UnlockTcpClient()
    End Sub

    Public Overridable Sub GetFile(ByVal tstrRemoteFileName As String, ByVal tintType As FTPFileTransferType)
        Me.GetFile(tstrRemoteFileName, System.IO.Path.GetFileName(tstrRemoteFileName), tintType)
    End Sub
    Public Overridable Sub GetFile(ByVal tstrRemoteFileName As String, ByVal tstrLocalFileName As String, ByVal tintType As FTPFileTransferType)
        Using lobjFS As New System.IO.FileStream(tstrLocalFileName, System.IO.FileMode.Create)
            Me.GetStream(tstrRemoteFileName, lobjFS, tintType)
            lobjFS.Close()
        End Using
    End Sub

    Public Overridable Sub DeleteFile(ByVal tstrRemoteFileName As String)
        Me.LockTcpClient()
        Dim lcolTempMessageList As System.Collections.ArrayList
        Dim lintReturn As Integer
        lcolTempMessageList = Me.SendCommand("DELE " & tstrRemoteFileName)
        lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
        If lintReturn <> 250 Then
            Throw New System.Exception(CStr(lcolTempMessageList(0)))
        End If
        Me.UnlockTcpClient()
    End Sub

    Public Overridable Sub MoveFile(ByVal tstrRemoteFileName As String, ByVal tstrToRemotePath As String)
        If tstrToRemotePath.Length > 0 AndAlso Not tstrToRemotePath.EndsWith("/") Then
            tstrToRemotePath &= "/"
        End If
        Me.RenameFile(tstrRemoteFileName, tstrToRemotePath & tstrRemoteFileName)
    End Sub

    Public Overridable Sub RenameFile(ByVal tstrFromRemoteFileName As String, ByVal tstrToRemoteFileName As String)
        Me.LockTcpClient()
        Dim lcolTempMessageList As System.Collections.ArrayList
        Dim lintReturn As Integer
        lcolTempMessageList = Me.SendCommand("RNFR " & tstrFromRemoteFileName)
        lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
        If lintReturn <> 350 Then
            Throw New System.Exception(CStr(lcolTempMessageList(0)))
        End If
        lcolTempMessageList = Me.SendCommand("RNTO " & tstrToRemoteFileName)
        lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
        If lintReturn <> 250 Then
            Throw New System.Exception(CStr(lcolTempMessageList(0)))
        End If
        Me.UnlockTcpClient()
    End Sub

    Public Overridable Sub SetCurrentDirectory(ByVal tstrRemotePath As String)
        Me.LockTcpClient()
        Dim lcolTempMessageList As System.Collections.ArrayList
        Dim lintReturn As Integer
        lcolTempMessageList = Me.SendCommand("CWD " & tstrRemotePath)
        lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
        If lintReturn <> 250 Then
            Throw New System.Exception(CStr(lcolTempMessageList(0)))
        End If
        Me.UnlockTcpClient()
    End Sub

    Private Sub SetTransferType(ByVal tintType As FTPFileTransferType)
        Select Case tintType
            Case FTPFileTransferType.ASCII
                Me.SetMode("TYPE A")
            Case FTPFileTransferType.Binary
                Me.SetMode("TYPE I")
            Case Else
                Throw New System.Exception("Invalid File Transfer Type")
        End Select
    End Sub

    Private Sub SetMode(ByVal tstrMode As String)
        Me.LockTcpClient()
        Dim lcolTempMessageList As System.Collections.ArrayList
        Dim lintReturn As Integer
        lcolTempMessageList = Me.SendCommand(tstrMode)
        lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
        If lintReturn <> 200 Then
            Throw New System.Exception(CStr(lcolTempMessageList(0)))
        End If
        Me.UnlockTcpClient()
    End Sub

    Private Function CreateDataListner() As System.Net.Sockets.TcpListener
        Dim lintPort As Integer = Me.GetPortNumber()
        Me.SetDataPort(lintPort)
        Dim lobjLocalHost As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName())
        Dim lobjListener As New System.Net.Sockets.TcpListener(lobjLocalHost.AddressList(0), lintPort)
        Return lobjListener
    End Function

    Private Function CreateDataClient() As System.Net.Sockets.TcpClient
        Dim lintPort As Integer = Me.GetPortNumber()
        Dim lobjClient As New System.Net.Sockets.TcpClient()
        lobjClient.Connect(Me.mstrRemoteHost, lintPort)
        Return lobjClient
    End Function

    Private Sub SetDataPort(ByVal tintPortNumber As Integer)
        Me.LockTcpClient()

        Dim lcolTempMessageList As System.Collections.ArrayList
        Dim lintReturn As Integer
        Dim lintPortHigh As Integer = tintPortNumber >> 8
        Dim lintPortLow As Integer = tintPortNumber And 255

        lcolTempMessageList = Me.SendCommand((("PORT " & GetLocalAddressList(0).ToString().Replace(".", ",") & ",") & lintPortHigh & ",") & lintPortLow)
        lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
        If lintReturn <> 200 Then
            Throw New System.Exception(CStr(lcolTempMessageList(0)))
        End If
        Me.UnlockTcpClient()
    End Sub

    Public Overridable Sub MakeDir(ByVal tstrDirectoryName As String)
        Me.LockTcpClient()

        Dim lcolTempMessageList As System.Collections.ArrayList
        Dim lintReturn As Integer

        lcolTempMessageList = Me.SendCommand("MKD " & tstrDirectoryName)
        lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
        If lintReturn <> 257 Then
            Throw New System.Exception(CStr(lcolTempMessageList(0)))
        End If

        Me.UnlockTcpClient()
    End Sub

    Public Overridable Sub RemoveDir(ByVal tstrDirectoryName As String)
        Me.LockTcpClient()

        Dim lcolTempMessageList As System.Collections.ArrayList
        Dim lintReturn As Integer

        lcolTempMessageList = Me.SendCommand("RMD " & tstrDirectoryName)
        lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
        If lintReturn <> 250 Then
            Throw New System.Exception(CStr(lcolTempMessageList(0)))
        End If

        Me.UnlockTcpClient()
    End Sub

    Public Function SendCommand(ByVal tstrCommand As String) As System.Collections.ArrayList
        Dim lobjStream As System.Net.Sockets.NetworkStream = mobjTcpClient.GetStream()
        mintActiveConnectionsCount += 1

        Dim lbytaCmdBytes As Byte() = System.Text.Encoding.ASCII.GetBytes((tstrCommand & System.Environment.NewLine).ToCharArray())
        lobjStream.Write(lbytaCmdBytes, 0, lbytaCmdBytes.Length)

        mintActiveConnectionsCount -= 1

        If tstrCommand = "QUIT" Then Return Nothing
        Return Me.Read()
    End Function

    Private Function Read() As System.Collections.ArrayList
        Dim lobjStream As System.Net.Sockets.NetworkStream = mobjTcpClient.GetStream()
        Dim lcolMessageList As New System.Collections.ArrayList()
        Dim lcolTempMessage As System.Collections.ArrayList = Me.ReadLines(lobjStream)

        Dim lintTryCount As Integer = 0
        While lcolTempMessage.Count = 0
            If lintTryCount = 100 Then
                Throw New System.Exception("Server does not return message to the message")
            End If
            System.Threading.Thread.Sleep(100)
            lintTryCount += 1
            lcolTempMessage = Me.ReadLines(lobjStream)
        End While

        While CStr(lcolTempMessage(lcolTempMessage.Count - 1)).Substring(3, 1) = "-"
            lcolMessageList.AddRange(lcolTempMessage)
            lcolTempMessage = Me.ReadLines(lobjStream)
        End While
        lcolMessageList.AddRange(lcolTempMessage)

        Me.AddMessagesToMessageList(lcolMessageList)

        Return lcolMessageList
    End Function

    Private Function ReadLines(ByVal tobjStream As System.Net.Sockets.NetworkStream) As System.Collections.ArrayList
        Dim lcolMessageList As New System.Collections.ArrayList()
        Dim lchraSeperator As Char() = {ControlChars.Lf}
        Dim lchaToRemove As Char() = {ControlChars.Cr}
        Dim lbytaBuffer As Byte() = New Byte(mintsBlockSize - 1) {}
        Dim lintBytes As Integer
        Dim lstrTmpMes As String = ""

        While tobjStream.DataAvailable
            lintBytes = tobjStream.Read(lbytaBuffer, 0, lbytaBuffer.Length)
            lstrTmpMes &= System.Text.Encoding.ASCII.GetString(lbytaBuffer, 0, lintBytes)
        End While

        Dim lstraMess As String() = lstrTmpMes.Split(lchraSeperator)
        For I As Integer = 0 To lstraMess.Length - 1
            If lstraMess(I).Length > 0 Then
                lcolMessageList.Add(lstraMess(I).Trim(lchaToRemove))
            End If
        Next

        Return lcolMessageList
    End Function

    Private Function GetMessageReturnValue(ByVal tstrMessage As String) As Integer
        Return Integer.Parse(tstrMessage.Substring(0, 3))
    End Function

    Private Function GetPortNumber() As Integer
        Me.LockTcpClient()

        Dim lintPort As Integer

        Select Case mintFtpMode
            Case FTPMode.Active
                Dim lobjRnd As New System.Random(CInt(System.DateTime.Now.Ticks))
                lintPort = mintsDataPortRangeFrom + lobjRnd.Next(mintsDataPortRangeTo - mintsDataPortRangeFrom)

            Case FTPMode.Passive
                Dim lcolTempMessageList As System.Collections.ArrayList
                Dim lintReturn As Integer
                lcolTempMessageList = Me.SendCommand("PASV")
                lintReturn = Me.GetMessageReturnValue(CStr(lcolTempMessageList(0)))
                If lintReturn <> 227 Then
                    If CStr(lcolTempMessageList(0)).Length > 4 Then
                        Throw New System.Exception(CStr(lcolTempMessageList(0)))
                    Else
                        Throw New System.Exception(CStr(lcolTempMessageList(0)) & " Passive Mode not implemented")
                    End If
                End If
                Dim lstrMessage As String = CStr(lcolTempMessageList(0))
                Dim lintIndex1 As Integer = lstrMessage.IndexOf(",", 0)
                Dim lintIndex2 As Integer = lstrMessage.IndexOf(",", lintIndex1 + 1)
                Dim lintIndex3 As Integer = lstrMessage.IndexOf(",", lintIndex2 + 1)
                Dim lintIndex4 As Integer = lstrMessage.IndexOf(",", lintIndex3 + 1)
                Dim lintIndex5 As Integer = lstrMessage.IndexOf(",", lintIndex4 + 1)
                Dim lintIndex6 As Integer = lstrMessage.IndexOf(")", lintIndex5 + 1)
                lintPort = 256 * Integer.Parse(lstrMessage.Substring(lintIndex4 + 1, lintIndex5 - lintIndex4 - 1)) + Integer.Parse(lstrMessage.Substring(lintIndex5 + 1, lintIndex6 - lintIndex5 - 1))

        End Select
        Me.UnlockTcpClient()
        Return lintPort
    End Function

    Private Sub AddMessagesToMessageList(ByVal tcolMessages As System.Collections.ArrayList)
        If mlogMessages Then
            mcolMessageList.AddRange(tcolMessages)
        End If
    End Sub

    Private Function GetLocalAddressList() As System.Net.IPAddress()
        Return System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList
    End Function

    Private Sub LockTcpClient()
        System.Threading.Monitor.Enter(mobjTcpClient)
    End Sub

    Private Sub UnlockTcpClient()
        System.Threading.Monitor.Exit(mobjTcpClient)
    End Sub

    Private Function GetTokens(ByVal tstrText As String, ByVal tstrDelimiter As String) As System.Collections.ArrayList
        Dim lintNext As Integer
        Dim lcolTokens As New System.Collections.ArrayList()

        lintNext = tstrText.IndexOf(tstrDelimiter)
        While lintNext <> -1
            Dim lstrItem As String = tstrText.Substring(0, lintNext)
            If lstrItem.Length > 0 Then
                lcolTokens.Add(lstrItem)
            End If

            tstrText = tstrText.Substring(lintNext + 1)
            lintNext = tstrText.IndexOf(tstrDelimiter)
        End While

        If tstrText.Length > 0 Then
            lcolTokens.Add(tstrText)
        End If

        Return lcolTokens
    End Function

End Class
