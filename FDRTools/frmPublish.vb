Option Explicit On
Option Strict On

Imports System
Imports System.Windows.Forms
'TODO: VB!!!
Imports Microsoft.VisualBasic

Public Class frmPublish

    Private mintItemPath As Integer
    Private mintItemPictSize As Integer
    Private mintItemThumbSize As Integer
    Private mintItemCaptured As Integer
    Private mintItemOwner As Integer
    Private mintItemLocationHU As Integer
    Private mintItemLocationEN As Integer
    Private WithEvents mobjPS As New PS

    Private Sub frmPublish_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles MyBase.Load
        Dim lstrTmp As String = Nothing
        Try
            lstrTmp = App.GetSetting("PublishSourceChecked", "True")
            If IsTrue(lstrTmp) Then optSource.Checked = True
            lstrTmp = App.GetSetting("PublishDestChecked", "False")
            If IsTrue(lstrTmp) Then optDest.Checked = True

            With cboFilter
                .Items.AddRange(App.GetSetting("PublishFilterItems", "*.*|*.jpg|*.gif|*.png").Split("|"c))
                .SelectedIndex = CInt(App.GetSetting("PublishFilterIndex", "1"))
            End With
            With cboOwner
                .Items.AddRange(App.GetSetting("PublishOwnerItems", "fdr|mse").Split("|"c))
                .SelectedIndex = CInt(App.GetSetting("PublishOwnerIndex", "0"))
            End With

            With lstFields
                mintItemPath = .Items.Add("Path", True)
                mintItemPictSize = .Items.Add("Picture size", True)
                mintItemThumbSize = .Items.Add("Thumbnail size", True)
                mintItemCaptured = .Items.Add("Capture date", False)
                mintItemOwner = .Items.Add("Owner", False)
                mintItemLocationHU = .Items.Add("Location HU", False)
                mintItemLocationEN = .Items.Add("Location EN", False)
            End With

            chkResize.Checked = IsTrue(App.GetSetting("Resize", "False"))
            txtMinSize.Text = App.GetSetting("ResizeMinSize", "60")
            chkOverwrite.Checked = IsTrue(App.GetSetting("ResizeOverwrite", "True"))
            txtActionSet.Text = App.GetSetting("ResizeActionSet", "FDR")
            txtActionName.Text = App.GetSetting("ResizeActionName", "web resize")

            chkUpdate.Checked = IsTrue(App.GetSetting("Update", "False"))

            chkInsert.Checked = IsTrue(App.GetSetting("Insert", "False"))

            chkExport.Checked = IsTrue(App.GetSetting("Export", "False"))
            txtRowLimit.Text = App.GetSetting("ExportRowLimit", "500")

            chkFTP.Checked = IsTrue(App.GetSetting("RemoteUpload", "False"))
            txtRemoteHost.Text = App.GetSetting("RemoteHost", "ftp.dataglobe.hu")
            txtRemoteLogin.Text = App.GetSetting("RemoteLogin", "fdr")
            lstrTmp = App.GetSetting("RemotePwd")
            If Not IsES(lstrTmp) Then txtRemotePwd.Text = lstrTmp
            chkRemotePassive.Checked = IsTrue(App.GetSetting("RemotePassive", "True"))

            Me.LoadDirs()

            Me.Gombok()
        Catch ex As Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub frmPublish_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) _
     Handles Me.KeyDown
        Try
            Select Case e.KeyCode
                Case Keys.Escape
                    Me.Close()

                Case Keys.F5
                    Me.LoadDirs()

                Case Keys.D
                    If e.Control Then
                        treDirectories.SelectedNodes.Clear()
                        Me.Gombok()
                    End If

            End Select
        Catch
        End Try
    End Sub

    Private Sub Gombok()
        Try
            grpResize.Enabled = chkResize.Checked
            pnlDatabase.Enabled = chkUpdate.Checked OrElse chkInsert.Checked
            grpUpdate.Enabled = chkUpdate.Checked
            grpInsert.Enabled = chkInsert.Checked
            grpExport.Enabled = chkExport.Checked
            grpFTP.Enabled = chkFTP.Checked

            chkResize.Enabled = Not optDest.Checked

            'btnStart.Enabled = (chkResize.Checked OrElse chkUpdate.Checked OrElse chkInsert.Checked OrElse chkFTP.Checked) AndAlso (treDirectories.SelectedNodes.Count > 0)
        Catch
        End Try
    End Sub

    Private Sub Frissites(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles cboFilter.SelectedIndexChanged, treDirectories.Click,
     chkResize.CheckedChanged, chkUpdate.CheckedChanged, chkInsert.CheckedChanged, chkExport.CheckedChanged, chkFTP.CheckedChanged
        Me.Gombok()
    End Sub
    Private Sub Frissites2(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) _
     Handles treDirectories.AfterSelect
        Me.Gombok()
    End Sub

    Private Sub lstFields_Leave(ByVal sender As Object, ByVal e As System.EventArgs) _
     Handles lstFields.Leave
        Try
            'Törli a mezõk kijelölt színét (a check-ek megmaradnak):
            lstFields.SelectedItems.Clear()
        Catch
        End Try
    End Sub

    Private Sub LoadDirs()
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.SuspendLayout()

            'TreeView elemeinek törlése:
            treDirectories.SelectedNodes.Clear()
            treDirectories.Nodes.Clear()

            Dim lstraExcludes() As String = App.GetSetting("DirExcludes").Split("|"c)
            Dim lstrFolder As String = txtRoot.Text
            'Ha az útvonal üres, vagy nem létezik, kilép:
            If IsES(lstrFolder) OrElse Not IO.Directory.Exists(lstrFolder) Then Exit Sub

            'Refkurzív feltöltés:
            Me.LoadDir(lstrFolder, Nothing, lstraExcludes)

            Me.Gombok()
        Catch ex As Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = Cursors.Default
            Me.ResumeLayout()
        End Try
    End Sub

    Private Sub LoadDir(ByVal tstrDir As String, ByVal tobjNode As TreeNode, ByRef tstraExcludes() As String)
        Dim lstraDirs() As String
        Dim lstrName As String
        Dim lobjNode As TreeNode
        'Könyvtárak betöltése:
        lstraDirs = IO.Directory.GetDirectories(tstrDir)
        For Each lstrPath As String In lstraDirs
            lstrName = IO.Path.GetFileName(lstrPath)
            Dim llogOK As Boolean = True
            For Each lstrEx As String In tstraExcludes
                If lstrName.ToLower Like lstrEx.ToLower Then
                    llogOK = False
                    Exit For
                End If
            Next
            'Ha a könyvtár neve nincs a kivétellistán, feltöltjük:
            If llogOK Then
                If tobjNode Is Nothing Then
                    lobjNode = treDirectories.Nodes.Add(lstrPath, lstrName)
                Else
                    lobjNode = tobjNode.Nodes.Add(lstrPath, lstrName)
                End If
                'Rekurzív hívás:
                Me.LoadDir(lstrPath, lobjNode, tstraExcludes)
            End If
        Next
    End Sub

    Private Sub Root_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles optSource.CheckedChanged, optDest.CheckedChanged
        Try
            If optDest.Checked Then
                txtRoot.Text = App.DestRoot
                chkResize.Checked = False
            Else
                txtRoot.Text = App.SourceRoot
            End If
            Me.LoadDirs()
        Catch ex As Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub txtCaptured_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) _
     Handles txtCaptured.Validating
        Dim ldatTmp As Date
        Try
            If Not IsES(txtCaptured.Text) Then
                ldatTmp = CDate(txtCaptured.Text)
                txtCaptured.Text = ldatTmp.ToString("yyyy-MM-dd")
            End If
            ErrorProvider1.SetError(txtCaptured, Nothing)
        Catch
            ErrorProvider1.SetError(txtCaptured, "Invalid date!")
            e.Cancel = True
        End Try
    End Sub

    Private Sub txtMinSize_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) _
     Handles txtMinSize.Validating
        Dim lintTmp As Integer
        Try
            If Not IsES(txtMinSize.Text) Then
                lintTmp = CInt(txtMinSize.Text)
                txtMinSize.Text = CStr(lintTmp)
                If lintTmp < 1 OrElse lintTmp > 1000 Then
                    ErrorProvider1.SetError(txtMinSize, "Invalid value! (Must be between 1 and 1000)")
                    e.Cancel = True
                    Exit Sub
                End If
            End If
            ErrorProvider1.SetError(txtMinSize, Nothing)
        Catch
            ErrorProvider1.SetError(txtMinSize, "Invalid number!")
            e.Cancel = True
        End Try
    End Sub

    Private Sub txtRowLimit_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) _
     Handles txtRowLimit.Validating
        Dim lintTmp As Integer
        Try
            If Not IsES(txtRowLimit.Text) Then
                lintTmp = CInt(txtRowLimit.Text)
                txtRowLimit.Text = CStr(lintTmp)
                If lintTmp < 1 OrElse lintTmp > 1000 Then
                    ErrorProvider1.SetError(txtRowLimit, "Invalid value! (Must be between 1 and 1000)")
                    e.Cancel = True
                    Exit Sub
                End If
            End If
            ErrorProvider1.SetError(txtRowLimit, Nothing)
        Catch
            ErrorProvider1.SetError(txtRowLimit, "Invalid number!")
            e.Cancel = True
        End Try
    End Sub

    Private Sub btnAddAlbum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles btnAddAlbum.Click
        Dim lstrID As String = Nothing
        Try
            If Not App.IsConnected Then App.Connect()
            Using lobjForm As New frmList("select * from fdr_album order by id", "Albums")
                If lobjForm.SelectID(lstrID) Then
                    If Not IsES(txtAlbums.Text) Then txtAlbums.Text &= ","
                    txtAlbums.Text &= lstrID
                End If
            End Using
            Me.Gombok()
        Catch ex As Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles btnStart.Click
        Dim lstrMsg As String = Nothing
        Dim lstrError As String = Nothing
        Dim lobjNode As TreeNode
        Dim lstrPath As String
        Try
            Me.Cursor = Cursors.WaitCursor

            'Progressbar:
            Me.AllMax(treDirectories.SelectedNodes.Count * 4 + 2, 1)

            'Elõzõ logfájl törlése:
            IO.File.Delete(IO.Path.Combine(App.LogDir, "publish.log"))

            For I As Integer = 0 To treDirectories.SelectedNodes.Count - 1
                If TypeOf treDirectories.SelectedNodes(I) IsNot TreeNode Then
                    lstrError &= "Not a TreeNode! (I=" & I & ")" & Environment.NewLine
                    Exit For
                End If
                lobjNode = CType(treDirectories.SelectedNodes(I), TreeNode)
                lstrPath = lobjNode.Name

                'RESIZE: ==========================================================================================
                If chkResize.Checked Then
                    Dim lstrSubDir As String
                    Dim lstraFiles() As String
                    Dim lstrName As String
                    Dim lstrWebPictDir As String
                    Dim lstrWebThumbDir As String
                    Dim lstrDestFile As String
                    Dim lstrDestRoot As String = App.DestRoot
                    Dim lstraExcludes() As String = App.GetSetting("FileExcludes").Split("|"c)

                    'Átméretezés Photoshop-pal:
                    If Not mobjPS.WebResize(lstrPath, txtActionSet.Text, txtActionName.Text, CInt(txtMinSize.Text)) Then Exit Sub
                    'If Not mobjPS.WebResize2(lstrPath, txtActionSet.Text, txtActionName.Text, CInt(txtMinSize.Text)) Then Exit Sub

                    lstrSubDir = lstrPath.Remove(0, App.SourceRoot.Length)
                    lstrWebPictDir = IO.Path.Combine(App.DestPictDir, lstrSubDir)
                    lstrWebThumbDir = IO.Path.Combine(App.DestThumbDir, lstrSubDir)
                    'Web könyvtárak létrehozása:
                    IO.Directory.CreateDirectory(lstrWebPictDir)
                    IO.Directory.CreateDirectory(lstrWebThumbDir)
                    'Átméretezett fájok átmozgatása a végleges helyükre:
                    lstraFiles = IO.Directory.GetFiles(App.TmpPictDir, "*.jpg")
                    For Each lstrFile As String In lstraFiles
                        lstrName = IO.Path.GetFileName(lstrFile).ToLower
                        If Not LikeArray(lstrName, lstraExcludes) Then
                            lstrDestFile = IO.Path.Combine(lstrWebPictDir, lstrName)
                            If IO.File.Exists(lstrDestFile) Then
                                If chkOverwrite.Checked Then
                                    IO.File.Delete(lstrDestFile)
                                    IO.File.Move(lstrFile, lstrDestFile)
                                End If
                            Else
                                IO.File.Move(lstrFile, lstrDestFile)
                            End If
                        End If
                    Next
                    lstraFiles = IO.Directory.GetFiles(App.TmpThumbDir, "*.jpg")
                    For Each lstrFile As String In lstraFiles
                        lstrName = IO.Path.GetFileName(lstrFile).ToLower
                        If Not LikeArray(lstrName, lstraExcludes) Then
                            lstrDestFile = IO.Path.Combine(lstrWebThumbDir, lstrName)
                            If IO.File.Exists(lstrDestFile) Then
                                If chkOverwrite.Checked Then
                                    IO.File.Delete(lstrDestFile)
                                    IO.File.Move(lstrFile, lstrDestFile)
                                End If
                            Else
                                IO.File.Move(lstrFile, lstrDestFile)
                            End If
                        End If
                    Next
                End If
                'Progressbar léptetése:
                Me.CurrMax()
                Me.AllInc()

                'UPDATE ==========================================================================================
                If chkUpdate.Checked Then
                    Dim lstrSQL As String = Nothing

                    Me.GenUpdate(lstrPath, lstrSQL, lstrError)

                    If Not IsES(lstrSQL) Then
                        If Not App.IsConnected AndAlso Not App.Connect Then
                            lstrError &= "Connect failed!" & Environment.NewLine
                        Else
                            Using lobjCmd As New MySql.Data.MySqlClient.MySqlCommand(lstrSQL, App.Connection)
                                Try
                                    lobjCmd.ExecuteNonQuery()
                                Catch ex As Exception
                                    lstrError &= ex.ToString & Environment.NewLine
                                    App.AddErr(ex)
                                End Try
                            End Using
                        End If
                    End If
                End If
                'Progressbar léptetése:
                Me.CurrMax()
                Me.AllInc()

                'INSERT ==========================================================================================
                If chkInsert.Checked Then
                    Dim lstrSQL As String = Nothing

                    Me.GenInsert(lstrPath, lstrSQL, lstrError)

                    If Not IsES(lstrSQL) Then
                        If Not App.IsConnected AndAlso Not App.Connect Then
                            lstrError &= "Connect failed!" & Environment.NewLine
                        Else
                            Using lobjCmd As New MySql.Data.MySqlClient.MySqlCommand(lstrSQL, App.Connection)
                                Try
                                    lobjCmd.ExecuteNonQuery()
                                Catch ex As Exception
                                    lstrError &= ex.ToString & Environment.NewLine
                                    App.AddErr(ex)
                                End Try
                            End Using
                        End If
                    End If
                End If
                'Progressbar léptetése:
                Me.CurrMax()
                Me.AllInc()

                'FTP ==========================================================================================
                If chkFTP.Checked Then
                    Me.FtpUpload(lstrPath, lstrError)
                End If
                'Progressbar léptetése:
                Me.CurrMax()
                Me.AllInc()
            Next

            'EXPORT ==========================================================================================
            If chkExport.Checked Then
                If False Then
                    Dim lobjProcess As System.Diagnostics.Process
                    Dim lobjInfo As New System.Diagnostics.ProcessStartInfo()
                    With lobjInfo
                        .FileName = App.GetSetting("ExportBatch", "D:\WEB\export_fdr.bat")
                        .CreateNoWindow = True
                        .UseShellExecute = False
                    End With
                    lobjProcess = System.Diagnostics.Process.Start(lobjInfo)
                    lobjProcess.WaitForExit(10000)
                Else
                    Me.CurrMax(11, 1)

                    Dim lstrFile As String = "fdr_" & Date.Now.ToString("yyyyMMdd") & ".sql"
                    lstrFile = IO.Path.Combine(App.GetSetting("ExportDir", "D:\WEB\"), lstrFile)

                    Dim lstrLogFile As String = "fdr_" & Date.Now.ToString("yyyyMMdd") & ".log"
                    lstrLogFile = IO.Path.Combine(App.GetSetting("ExportDir", "D:\WEB\"), lstrLogFile)

                    IO.File.Delete(lstrFile)

                    If Not App.IsConnected Then
                        App.Connect()
                    End If

                    Dim lobjProcess As System.Diagnostics.Process
                    Dim lobjInfo As New System.Diagnostics.ProcessStartInfo()
                    With lobjInfo
                        .FileName = "C:\Program Files\MySQL\MySQL Server 5.5\bin\mysqldump.exe"
                        .Arguments = "--user=fdr --password=eos10d --skip-opt --create-options --compact --comments --no-data --result-file=""" & lstrFile & """ --log-error=""" & lstrLogFile & """ fdr fdr_film fdr_user fdr_pict fdr_album fdr_alb_pict fdr_cat fdr_cat_alb fdr_opt"
                        .CreateNoWindow = True
                        .UseShellExecute = False
                    End With
                    lobjProcess = System.Diagnostics.Process.Start(lobjInfo)
                    lobjProcess.WaitForExit(60000)
                    Me.CurrInc()

                    Dim lstrTmp As String
                    Using lobjReader As New IO.StreamReader(lstrFile, System.Text.Encoding.UTF8)
                        lstrTmp = lobjReader.ReadToEnd()
                    End Using

                    '                    lstrTmp = "" _
                    '& "SET @saved_cs_client     = @@character_set_client;" _
                    '& "SET character_set_client = utf8;" _
                    '& "" _
                    '& "-- Table structure for table `fdr_film`" _
                    '& "CREATE TABLE `fdr_film` (" _
                    '& "  `film` int(4) NOT NULL," _
                    '& "  `datum` date NOT NULL," _
                    '& "  PRIMARY KEY  (`film`)" _
                    '& ") ENGINE=InnoDB DEFAULT CHARSET=utf8;" _
                    '& "" _
                    '& "-- Table structure for table `fdr_user`" _
                    '& "CREATE TABLE `fdr_user` (" _
                    '& "  `id` varchar(20) NOT NULL," _
                    '& "  `name` varchar(50) NOT NULL," _
                    '& "  PRIMARY KEY  (`id`)" _
                    '& ") ENGINE=InnoDB DEFAULT CHARSET=utf8;" _
                    '& "" _
                    '& "-- Table structure for table `fdr_pict`" _
                    '& "CREATE TABLE `fdr_pict` (" _
                    '& "  `id` varchar(40) NOT NULL," _
                    '& "  `owner` varchar(20) NOT NULL," _
                    '& "  `pictfile` varchar(100) default NULL," _
                    '& "  `title_hu` varchar(100) default NULL," _
                    '& "  `title_en` varchar(100) default NULL," _
                    '& "  `description_hu` varchar(255) default NULL," _
                    '& "  `description_en` varchar(255) default NULL," _
                    '& "  `location_hu` varchar(255) default NULL," _
                    '& "  `location_en` varchar(255) default NULL," _
                    '& "  `keywords_hu` varchar(255) default NULL," _
                    '& "  `keywords_en` varchar(255) default NULL," _
                    '& "  `captured` date default NULL," _
                    '& "  `uploaded` datetime NOT NULL," _
                    '& "  `pwidth` smallint(6) NOT NULL," _
                    '& "  `pheight` smallint(6) NOT NULL," _
                    '& "  `twidth` smallint(6) default NULL," _
                    '& "  `theight` smallint(6) default NULL," _
                    '& "  PRIMARY KEY  (`id`)," _
                    '& "  KEY `pict_owner_fki` USING BTREE (`owner`)," _
                    '& "  CONSTRAINT `pict_owner_fk` FOREIGN KEY (`owner`) REFERENCES `fdr_user` (`id`) ON UPDATE CASCADE" _
                    '& ") ENGINE=InnoDB DEFAULT CHARSET=utf8;" _
                    '& "" _
                    '& "-- Table structure for table `fdr_album`" _
                    '& "CREATE TABLE `fdr_album` (" _
                    '& "  `id` varchar(20) NOT NULL," _
                    '& "  `owner` varchar(20) NOT NULL," _
                    '& "  `title_hu` varchar(100) default NULL," _
                    '& "  `title_en` varchar(100) default NULL," _
                    '& "  `description_hu` varchar(255) default NULL," _
                    '& "  `description_en` varchar(255) default NULL," _
                    '& "  `keywords_hu` varchar(255) default NULL," _
                    '& "  `keywords_en` varchar(255) default NULL," _
                    '& "  `thumb` varchar(40) default NULL," _
                    '& "  `public` tinyint(1) NOT NULL default '1'," _
                    '& "  PRIMARY KEY  (`id`)," _
                    '& "  KEY `album_owner_fki` USING BTREE (`owner`)," _
                    '& "  KEY `album_thumb_fki` USING BTREE (`thumb`)," _
                    '& "  CONSTRAINT `album_owner_fk` FOREIGN KEY (`owner`) REFERENCES `fdr_user` (`id`) ON UPDATE CASCADE," _
                    '& "  CONSTRAINT `album_thumb_fk` FOREIGN KEY (`thumb`) REFERENCES `fdr_pict` (`id`) ON UPDATE CASCADE" _
                    '& ") ENGINE=InnoDB DEFAULT CHARSET=utf8;" _
                    '& "" _
                    '& "-- Table structure for table `fdr_alb_pict`" _
                    '& "CREATE TABLE `fdr_alb_pict` (" _
                    '& "  `album_id` varchar(20) NOT NULL," _
                    '& "  `pict_id` varchar(40) NOT NULL," _
                    '& "  `pict_order` int(11) NOT NULL default '0'," _
                    '& "  `visible` tinyint(1) NOT NULL default '1'," _
                    '& "  PRIMARY KEY  (`album_id`,`pict_id`)," _
                    '& "  KEY `alb_pict_pict_order_i` (`pict_order`)," _
                    '& "  KEY `alb_pict_pict_id_fki` USING BTREE (`pict_id`)," _
                    '& "  CONSTRAINT `alb_pict_album_id_fk` FOREIGN KEY (`album_id`) REFERENCES `fdr_album` (`id`) ON DELETE CASCADE ON UPDATE CASCADE," _
                    '& "  CONSTRAINT `alb_pict_pict_id_fk` FOREIGN KEY (`pict_id`) REFERENCES `fdr_pict` (`id`) ON DELETE CASCADE ON UPDATE CASCADE" _
                    '& ") ENGINE=InnoDB DEFAULT CHARSET=utf8;" _
                    '& "" _
                    '& "-- Table structure for table `fdr_cat`" _
                    '& "CREATE TABLE `fdr_cat` (" _
                    '& "  `id` varchar(20) NOT NULL," _
                    '& "  `name_hu` varchar(100) NOT NULL," _
                    '& "  `name_en` varchar(100) default NULL," _
                    '& "  `description_hu` varchar(255) default NULL," _
                    '& "  `description_en` varchar(255) default NULL," _
                    '& "  `cat_order` int(11) default NULL," _
                    '& "  `visible` tinyint(1) NOT NULL default '1'," _
                    '& "  `public` tinyint(1) NOT NULL default '1'," _
                    '& "  `pict` varchar(40) default NULL," _
                    '& "  PRIMARY KEY  (`id`)," _
                    '& "  KEY `cat_cat_order_i` USING BTREE (`cat_order`)," _
                    '& "  KEY `cat_pict_fk` (`pict`)," _
                    '& "  CONSTRAINT `cat_pict_fk` FOREIGN KEY (`pict`) REFERENCES `fdr_pict` (`id`) ON UPDATE CASCADE" _
                    '& ") ENGINE=InnoDB DEFAULT CHARSET=utf8;" _
                    '& "" _
                    '& "-- Table structure for table `fdr_cat_alb`" _
                    '& "CREATE TABLE `fdr_cat_alb` (" _
                    '& "  `cat_id` varchar(20) NOT NULL," _
                    '& "  `album_id` varchar(20) NOT NULL," _
                    '& "  `album_order` int(11) NOT NULL default '0'," _
                    '& "  PRIMARY KEY  (`cat_id`,`album_id`)," _
                    '& "  KEY `cat_alb_album_order_i` USING BTREE (`album_order`)," _
                    '& "  KEY `cat_alb_album_id_fki` USING BTREE (`album_id`)," _
                    '& "  CONSTRAINT `cat_alb_album_id_fk` FOREIGN KEY (`album_id`) REFERENCES `fdr_album` (`id`) ON DELETE CASCADE ON UPDATE CASCADE," _
                    '& "  CONSTRAINT `cat_alb_cat_id_fk` FOREIGN KEY (`cat_id`) REFERENCES `fdr_cat` (`id`) ON DELETE CASCADE ON UPDATE CASCADE" _
                    '& ") ENGINE=InnoDB DEFAULT CHARSET=utf8;" _
                    '& "" _
                    '& "-- Table structure for table `fdr_opt`" _
                    '& "CREATE TABLE `fdr_opt` (" _
                    '& "  `id` varchar(20) NOT NULL," _
                    '& "  `main_pict` varchar(40) default NULL," _
                    '& "  PRIMARY KEY  (`id`)," _
                    '& "  KEY `opt_main_pict_fk` (`main_pict`)," _
                    '& "  CONSTRAINT `opt_main_pict_fk` FOREIGN KEY (`main_pict`) REFERENCES `fdr_pict` (`id`) ON UPDATE CASCADE" _
                    '& ") ENGINE=InnoDB DEFAULT CHARSET=utf8;" _
                    '& "" _
                    '& "SET character_set_client = @saved_cs_client;"


                    Using lobjWriter As New IO.StreamWriter(lstrFile, False, System.Text.Encoding.UTF8)
                        lobjWriter.WriteLine("-- Drop tables to eliminate foreign keys:")
                        lobjWriter.WriteLine("DROP TABLE IF EXISTS `fdr_opt`;")
                        lobjWriter.WriteLine("DROP TABLE IF EXISTS `fdr_cat_alb`;")
                        lobjWriter.WriteLine("DROP TABLE IF EXISTS `fdr_cat`;")
                        lobjWriter.WriteLine("DROP TABLE IF EXISTS `fdr_alb_pict`;")
                        lobjWriter.WriteLine("DROP TABLE IF EXISTS `fdr_album`;")
                        lobjWriter.WriteLine("DROP TABLE IF EXISTS `fdr_pict`;")
                        lobjWriter.WriteLine("DROP TABLE IF EXISTS `fdr_user`;")
                        lobjWriter.WriteLine("DROP TABLE IF EXISTS `fdr_film`;")
                        lobjWriter.WriteLine("")
                        lobjWriter.WriteLine(lstrTmp)
                        lobjWriter.Close()
                    End Using
                    Me.CurrInc()

                    Me.ExportTable("fdr_film", lstrFile, CInt(txtRowLimit.Text))
                    Me.CurrInc()
                    Me.ExportTable("fdr_user", lstrFile, CInt(txtRowLimit.Text))
                    Me.CurrInc()
                    Me.ExportTable("fdr_pict", lstrFile, CInt(txtRowLimit.Text))
                    Me.CurrInc()
                    Me.ExportTable("fdr_album", lstrFile, CInt(txtRowLimit.Text))
                    Me.CurrInc()
                    Me.ExportTable("fdr_alb_pict", lstrFile, CInt(txtRowLimit.Text))
                    Me.CurrInc()
                    Me.ExportTable("fdr_cat", lstrFile, CInt(txtRowLimit.Text))
                    Me.CurrInc()
                    Me.ExportTable("fdr_cat_alb", lstrFile, CInt(txtRowLimit.Text))
                    Me.CurrInc()
                    Me.ExportTable("fdr_opt", lstrFile, CInt(txtRowLimit.Text))
                    Me.CurrInc()
                End If
            End If
            'Progressbar léptetése:
            Me.CurrMax()
            Me.AllInc()

            If IsES(lstrError) Then
                MessageBox.Show(Me, "Successfully finished...", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show(Me, "Finished with errors!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
            Me.Gombok()
        Catch ex As Exception
            App.AddErr(ex)
            lstrError &= ex.ToString & Environment.NewLine
        Finally
            Me.Cursor = Cursors.Default
            'Progressbar törlése:
            Me.AllMax()
            Me.CurrMax()
            'Hibalog kiírása:
            If Not IsES(lstrError) Then
                Try
                    Using lobjWriter As New IO.StreamWriter(IO.Path.Combine(App.LogDir, "publish.log"))
                        lobjWriter.WriteLine(lstrError)
                        lobjWriter.Close()
                    End Using
                Catch
                End Try
            End If
        End Try
    End Sub

    Private Sub AllMax(Optional ByVal tintMax As Integer = 100, Optional ByVal tintValue As Integer = 0)
        Try
            barAll.SuspendLayout()
            If tintValue > tintMax Then tintValue = tintMax
            barAll.Value = 0
            barAll.Maximum = tintMax
            barAll.Value = tintValue
        Catch
        Finally
            barAll.ResumeLayout()
        End Try
    End Sub

    Private Sub AllInc()
        Try
            barAll.Value += 1
        Catch
        End Try
    End Sub

    Private Sub CurrMax(Optional ByVal tintMax As Integer = 100, Optional ByVal tintValue As Integer = 0)
        Try
            barCurrent.SuspendLayout()
            If tintValue > tintMax Then tintValue = tintMax
            barCurrent.Value = 0
            barCurrent.Maximum = tintMax
            barCurrent.Value = tintValue
        Catch
        Finally
            barCurrent.ResumeLayout()
        End Try
    End Sub

    Private Sub CurrInc()
        Try
            barCurrent.Value += 1
        Catch
        End Try
    End Sub

    Private Sub mobjPS_ProgressInit(ByVal sender As Object, ByVal e As PS.ProgressEventArgs) _
     Handles mobjPS.ProgressInit
        Me.CurrMax(e.Maximum, e.Value)
    End Sub

    Private Sub mobjPS_ProgressIncrement(ByVal sender As Object, ByVal e As System.EventArgs) _
     Handles mobjPS.ProgressIncrement
        Me.CurrInc()
    End Sub

    Private Sub GenUpdate(ByVal tstrDir As String, ByRef tstrScript As String, ByRef tstrError As String)
        Dim lstraFiles() As String
        Dim lobjImage As System.Drawing.Image = Nothing
        Dim lstrSubDir As String
        Dim lstrPictDir As String
        Dim lstrSourceRoot As String = App.SourceRoot
        Dim lstrDestRoot As String = App.DestRoot
        Dim lstraExcludes() As String = App.GetSetting("FileExcludes").Split("|"c)

        If IsES(tstrDir) Then
            tstrError &= "UPDATE Missing parameter! (tstrDir)" & Environment.NewLine
            Exit Sub
        End If

        If ES(tstrDir).StartsWith(lstrSourceRoot, StringComparison.CurrentCultureIgnoreCase) Then
            lstrSubDir = tstrDir.Remove(0, lstrSourceRoot.Length)
        ElseIf ES(tstrDir).StartsWith(App.DestPictDir, StringComparison.CurrentCultureIgnoreCase) Then
            lstrSubDir = tstrDir.Remove(0, App.DestPictDir.Length)
        ElseIf ES(tstrDir).StartsWith(App.DestThumbDir, StringComparison.CurrentCultureIgnoreCase) Then
            lstrSubDir = tstrDir.Remove(0, App.DestThumbDir.Length)
        Else
            tstrError &= "UPDATE Invalid root directory! (" & tstrDir & ")" & Environment.NewLine
            Exit Sub
        End If
        lstrSubDir = lstrSubDir.TrimStart(IO.Path.DirectorySeparatorChar, IO.Path.AltDirectorySeparatorChar)

        Dim llogPath As Boolean = lstFields.GetItemChecked(mintItemPath)
        Dim llogPictSize As Boolean = lstFields.GetItemChecked(mintItemPictSize)
        Dim llogThumbSize As Boolean = lstFields.GetItemChecked(mintItemThumbSize)
        Dim llogCaptured As Boolean = lstFields.GetItemChecked(mintItemCaptured)
        Dim llogOwner As Boolean = lstFields.GetItemChecked(mintItemOwner)
        Dim llogLocationHU As Boolean = lstFields.GetItemChecked(mintItemLocationHU)
        Dim llogLocationEN As Boolean = lstFields.GetItemChecked(mintItemLocationEN)

        Dim lstrCaptured As String = txtCaptured.Text.Trim
        Dim lstrOwner As String = cboOwner.Text.Trim
        Dim lstrLocationHU As String = txtLocationHU.Text.Trim
        Dim lstrLocationEN As String = txtLocationEN.Text.Trim

        lstrPictDir = IO.Path.Combine(App.DestPictDir, lstrSubDir)
        If Not IO.Directory.Exists(lstrPictDir) Then
            tstrError &= "UPDATE Missing directory: " & lstrPictDir & Environment.NewLine
            Exit Sub
        End If

        lstraFiles = IO.Directory.GetFiles(tstrDir, cboFilter.Text)

        Me.CurrMax(lstraFiles.Length)

        For Each lstrFile As String In lstraFiles
            Dim lstrSQL As String
            Dim lstrID As String
            Dim lintPWidth As Integer
            Dim lintPHeight As Integer
            Dim lstrThumbFile As String
            Dim lintTWidth As Integer
            Dim lintTHeight As Integer
            Dim lstrPictFile As String
            Dim lstrName As String = IO.Path.GetFileName(lstrFile)
            If Not LikeArray(lstrName, lstraExcludes) Then
                lstrSQL = Nothing
                lstrID = IO.Path.GetFileNameWithoutExtension(lstrFile)
                lstrPictFile = IO.Path.Combine(lstrSubDir, IO.Path.GetFileName(lstrFile).ToLower).Replace("\"c, "/"c)
                If lstrPictFile.StartsWith("/") Then lstrPictFile = lstrPictFile.Remove(0, 1)

                If llogPictSize Then
                    'Kép méretének kiolvasása:
                    Try
                        lobjImage = System.Drawing.Image.FromFile(IO.Path.Combine(App.DestPictDir, lstrPictFile), False)
                        With lobjImage
                            lintPWidth = .Width
                            lintPHeight = .Height
                        End With

                        If Not IsES(lstrSQL) Then lstrSQL &= ", "
                        lstrSQL &= "pwidth=" & CStr(lintPWidth)
                        If Not IsES(lstrSQL) Then lstrSQL &= ", "
                        lstrSQL &= "pheight=" & CStr(lintPHeight)
                    Catch
                        tstrError &= "UPDATE Picture: " & IO.Path.Combine(App.DestPictDir, lstrPictFile) & Environment.NewLine
                    Finally
                        If lobjImage IsNot Nothing Then lobjImage.Dispose()
                        lobjImage = Nothing
                    End Try
                End If

                If llogThumbSize Then
                    lintTWidth = 0
                    lintTHeight = 0

                    'Thumbnail fájl útvonala:
                    lstrThumbFile = IO.Path.Combine(App.DestThumbDir, lstrPictFile)
                    If IO.File.Exists(lstrThumbFile) Then
                        'Thumbnail méretének kiolvasása, ha létezik:
                        Try
                            lobjImage = System.Drawing.Image.FromFile(lstrThumbFile, False)
                            With lobjImage
                                lintTWidth = .Width
                                lintTHeight = .Height
                            End With
                        Catch
                            tstrError &= "UPDATE Thumbnail: " & lstrThumbFile & Environment.NewLine
                        Finally
                            If lobjImage IsNot Nothing Then lobjImage.Dispose()
                            lobjImage = Nothing
                        End Try
                    End If

                    If lintPWidth <> 0 AndAlso lintPHeight <> 0 AndAlso (lintTWidth = 0 OrElse lintTHeight = 0) Then
                        'Thumbnail méretének kiszámolása, he nem létezik:
                        If lintPWidth >= lintPHeight Then
                            lintTWidth = 100
                            lintTHeight = CInt((100 * lintPHeight) / lintPWidth)
                        Else
                            lintTHeight = 100
                            lintTWidth = CInt((100L * lintPWidth) / lintPHeight)
                        End If
                    End If

                    If Not IsES(lstrSQL) Then lstrSQL &= ", "
                    lstrSQL &= "twidth=" & CStr(lintTWidth)
                    If Not IsES(lstrSQL) Then lstrSQL &= ", "
                    lstrSQL &= "theight=" & CStr(lintTHeight)
                End If

                If llogPath Then
                    If Not IsES(lstrSQL) Then lstrSQL &= ", "
                    lstrSQL &= "pictfile='" & lstrPictFile & "'"
                End If

                If llogCaptured Then
                    If Not IsES(lstrSQL) Then lstrSQL &= ", "
                    lstrSQL &= "captured='" & lstrCaptured & "'"
                End If

                If llogOwner Then
                    If Not IsES(lstrSQL) Then lstrSQL &= ", "
                    lstrSQL &= "owner='" & lstrOwner & "'"
                End If

                If llogLocationHU Then
                    If Not IsES(lstrSQL) Then lstrSQL &= ", "
                    lstrSQL &= "location_hu='" & lstrLocationHU & "'"
                End If

                If llogLocationEN Then
                    If Not IsES(lstrSQL) Then lstrSQL &= ", "
                    lstrSQL &= "location_en='" & lstrLocationEN & "'"
                End If

                lstrSQL = "UPDATE fdr_pict SET " & lstrSQL & " WHERE id='" & lstrID & "';"
                tstrScript &= lstrSQL & Environment.NewLine
            End If
            Me.CurrInc()
        Next
    End Sub

    Private Sub GenInsert(ByVal tstrDir As String, ByRef tstrScript As String, ByRef tstrError As String)
        Dim ldatNow As Date
        Dim lstrTmp As String
        Dim lstraAlbums() As String
        Dim lstraFiles() As String
        Dim lobjImage As System.Drawing.Image = Nothing
        Dim lstrSubDir As String
        Dim lstrPictDir As String
        Dim lstrSourceRoot As String = App.SourceRoot
        Dim lstrDestRoot As String = App.DestRoot
        Dim lstraExcludes() As String = App.GetSetting("FileExcludes").Split("|"c)

        If IsES(tstrDir) Then
            tstrError &= "INSERT Missing parameter! (tstrDir)" & Environment.NewLine
            Exit Sub
        End If

        If ES(tstrDir).StartsWith(lstrSourceRoot, StringComparison.CurrentCultureIgnoreCase) Then
            lstrSubDir = tstrDir.Remove(0, lstrSourceRoot.Length)
        ElseIf ES(tstrDir).StartsWith(App.DestPictDir, StringComparison.CurrentCultureIgnoreCase) Then
            lstrSubDir = tstrDir.Remove(0, App.DestPictDir.Length)
        ElseIf ES(tstrDir).StartsWith(App.DestThumbDir, StringComparison.CurrentCultureIgnoreCase) Then
            lstrSubDir = tstrDir.Remove(0, App.DestThumbDir.Length)
        Else
            tstrError &= "INSERT Invalid root directory! (" & tstrDir & ")" & Environment.NewLine
            Exit Sub
        End If
        lstrSubDir = lstrSubDir.TrimStart(IO.Path.DirectorySeparatorChar, IO.Path.AltDirectorySeparatorChar)

        ldatNow = Date.Now
        'lstraAlbums = Split(txtAlbums.Text, ",")
        lstraAlbums = ES(txtAlbums.Text).Split(","c)

        lstrPictDir = IO.Path.Combine(App.DestPictDir, lstrSubDir)
        If Not IO.Directory.Exists(lstrPictDir) Then
            tstrError &= "INSERT Missing directory: " & lstrPictDir & Environment.NewLine
            Exit Sub
        End If

        lstraFiles = IO.Directory.GetFiles(tstrDir, cboFilter.Text)

        Me.CurrMax(lstraFiles.Length)

        Dim lintIndex As Integer = 0
        For Each lstrFile As String In lstraFiles
            Dim lstrSQL As String
            Dim lstrID As String
            Dim lintPWidth As Integer
            Dim lintPHeight As Integer
            Dim lstrThumbFile As String
            Dim lintTWidth As Integer
            Dim lintTHeight As Integer
            Dim lstrPictFile As String
            Dim lstrName As String = IO.Path.GetFileName(lstrFile)
            If Not LikeArray(lstrName, lstraExcludes) Then
                lstrID = IO.Path.GetFileNameWithoutExtension(lstrFile)
                lstrPictFile = IO.Path.Combine(lstrSubDir, IO.Path.GetFileName(lstrFile).ToLower).Replace("\"c, "/"c)
                If lstrPictFile.StartsWith("/") Then lstrPictFile = lstrPictFile.Remove(0, 1)

                'Kép méretének kiolvasása:
                Try
                    lobjImage = System.Drawing.Image.FromFile(IO.Path.Combine(App.DestPictDir, lstrPictFile), False)
                    With lobjImage
                        lintPWidth = .Width
                        lintPHeight = .Height
                    End With
                Catch
                    tstrError &= "INSERT Picture: " & IO.Path.Combine(App.DestPictDir, lstrPictFile) & Environment.NewLine
                Finally
                    If lobjImage IsNot Nothing Then lobjImage.Dispose()
                    lobjImage = Nothing
                End Try

                'Thumbnail fájl útvonala:
                lintTWidth = 0
                lintTHeight = 0
                lstrThumbFile = IO.Path.Combine(App.DestThumbDir, lstrPictFile)
                If IO.File.Exists(lstrThumbFile) Then
                    'Thumbnail méretének kiolvasása, ha létezik:
                    Try
                        lobjImage = System.Drawing.Image.FromFile(lstrThumbFile, False)
                        With lobjImage
                            lintTWidth = .Width
                            lintTHeight = .Height
                        End With
                    Catch
                        tstrError &= "INSERT Thumbnail: " & lstrThumbFile & Environment.NewLine
                    Finally
                        If lobjImage IsNot Nothing Then lobjImage.Dispose()
                        lobjImage = Nothing
                    End Try
                End If

                If lintTWidth = 0 OrElse lintTHeight = 0 Then
                    'Thumbnail méretének kiszámolása, he nem létezik:
                    If lintPWidth >= lintPHeight Then
                        lintTWidth = 100
                        lintTHeight = CInt((100 * lintPHeight) / lintPWidth)
                    Else
                        lintTHeight = 100
                        lintTWidth = CInt((100 * lintPWidth) / lintPHeight)
                    End If
                End If

                lstrSQL = "INSERT IGNORE INTO fdr_pict (id, owner, pictfile, location_hu, location_en, captured, uploaded, pwidth, pheight, twidth, theight) VALUES ("
                lstrSQL &= "'" & lstrID & "', "
                lstrSQL &= "'" & cboOwner.Text & "', "
                lstrSQL &= "'" & lstrPictFile & "', "
                If Not IsES(txtLocationHU.Text) Then
                    lstrSQL &= "'" & ES(txtLocationHU.Text).Trim & "', "
                Else
                    lstrSQL &= "NULL, "
                End If
                If Not IsES(txtLocationEN.Text) Then
                    lstrSQL &= "'" & ES(txtLocationEN.Text).Trim & "', "
                Else
                    lstrSQL &= "NULL, "
                End If
                If Not IsES(txtCaptured.Text) And txtCaptured.Text Like "####-##-##" Then
                    lstrSQL &= "'" & txtCaptured.Text & "', "
                ElseIf lstrID Like "######_#*" Then
                    If CInt(ES(lstrID).Substring(0, 2)) > 80 Then
                        lstrTmp = "19"
                    Else
                        lstrTmp = "20"
                    End If
                    'lstrTmp &= ES(lstrID).Substring(0, 2) & "-" & Mid(lstrID, 3, 2) & "-" & Mid(lstrID, 5, 2)
                    lstrTmp &= ES(lstrID).Substring(0, 2) & "-" & ES(lstrID).Substring(2, 2) & "-" & ES(lstrID).Substring(4, 2)
                    lstrSQL &= "'" & lstrTmp & "', "
                Else
                    lstrSQL &= "NULL, "
                End If
                lstrSQL &= "'" & Format(ldatNow, "yyyy-MM-dd HH\:mm\:ss") & "', "
                lstrSQL &= CStr(lintPWidth) & ", "
                lstrSQL &= CStr(lintPHeight) & ", "
                lstrSQL &= CStr(lintTWidth) & ", "
                lstrSQL &= CStr(lintTHeight) & ");"

                tstrScript &= lstrSQL & Environment.NewLine

                Dim llogAutoOrder As Boolean = Me.chkAutoOrder.Checked
                For Each lstrAlbum As String In lstraAlbums
                    If Not IsES(lstrAlbum) Then
                        lstrSQL = "INSERT IGNORE INTO fdr_alb_pict (album_id, pict_id, pict_order) VALUES ("
                        lstrSQL &= "'" & ES(lstrAlbum).Trim & "', "
                        lstrSQL &= "'" & lstrID & "', "
                        If llogAutoOrder Then
                            lstrSQL &= CStr((lintIndex + 1) * 100) & ");"
                        Else
                            lstrSQL &= "0);"
                        End If

                        tstrScript &= lstrSQL & Environment.NewLine
                    End If
                Next
                lintIndex += 1
            End If
            Me.CurrInc()
        Next
    End Sub

    Private Sub FtpUpload(ByVal tstrDir As String, ByRef tstrError As String)
        Dim lstraFiles() As String
        Dim lstrSubDir As String
        Dim lobjConn As FTPConnection = Nothing
        Dim lstrSource As String
        Dim lstrPictPath As String
        Dim lstrThumbPath As String
        Dim lintIndex As Integer
        Dim lstrSourceRoot As String = App.SourceRoot
        Dim lstrDestRoot As String = App.DestRoot
        Dim lstrRemoteRoot As String = App.RemoteRoot

        If IsES(tstrDir) Then
            tstrError &= "FTP Missing parameter! (tstrDir)" & Environment.NewLine
            Exit Sub
        End If

        If ES(tstrDir).StartsWith(lstrSourceRoot, StringComparison.CurrentCultureIgnoreCase) Then
            lstrSubDir = tstrDir.Remove(0, lstrSourceRoot.Length)
        ElseIf ES(tstrDir).StartsWith(App.DestPictDir, StringComparison.CurrentCultureIgnoreCase) Then
            lstrSubDir = tstrDir.Remove(0, App.DestPictDir.Length)
        ElseIf ES(tstrDir).StartsWith(App.DestThumbDir, StringComparison.CurrentCultureIgnoreCase) Then
            lstrSubDir = tstrDir.Remove(0, App.DestThumbDir.Length)
        Else
            tstrError &= "FTP Invalid root directory! (" & tstrDir & ")" & Environment.NewLine
            Exit Sub
        End If
        lstrSubDir = lstrSubDir.TrimStart(IO.Path.DirectorySeparatorChar, IO.Path.AltDirectorySeparatorChar)
        lstrSubDir = lstrSubDir.Replace("\", "/")

        lstraFiles = IO.Directory.GetFiles(tstrDir, cboFilter.Text)

        Me.CurrMax(lstraFiles.Length * 2 + 4, 1)

        Try
            lobjConn = New FTPConnection()
            With lobjConn
                If chkRemotePassive.Checked Then
                    .Open(txtRemoteHost.Text, txtRemoteLogin.Text, txtRemotePwd.Text.Trim, FTPMode.Passive)
                Else
                    .Open(txtRemoteHost.Text, txtRemoteLogin.Text, txtRemotePwd.Text.Trim, FTPMode.Active)
                End If
                Me.CurrInc()

                'Könyvtárstruktúra létrehozása:
                lstrPictPath = lstrRemoteRoot & App.PictDirName & "/" & lstrSubDir
                lintIndex = 1
                Do
                    lintIndex = lstrPictPath.IndexOf("/"c, lintIndex + 1)
                    If lintIndex >= 0 Then
                        Try
                            .MakeDir(lstrPictPath.Substring(0, lintIndex))
                        Catch
                        End Try
                    End If
                Loop Until lintIndex < 0
                Try
                    .MakeDir(lstrPictPath)
                Catch
                End Try
                Me.CurrInc()

                'Könyvtárstruktúra létrehozása:
                lstrThumbPath = lstrRemoteRoot & App.ThumbDirName & "/" & lstrSubDir
                lintIndex = 1
                Do
                    lintIndex = lstrThumbPath.IndexOf("/"c, lintIndex + 1)
                    If lintIndex >= 0 Then
                        Try
                            .MakeDir(lstrThumbPath.Substring(0, lintIndex))
                        Catch
                        End Try
                    End If
                Loop Until lintIndex < 0
                Try
                    .MakeDir(lstrThumbPath)
                Catch
                End Try
                Me.CurrInc()

                Dim lstraExcludes() As String = App.GetSetting("FileExcludes").Split("|"c)
                For Each lstrFile As String In lstraFiles
                    Dim lstrFileName As String = IO.Path.GetFileName(lstrFile).ToLower
                    If Not LikeArray(lstrFileName, lstraExcludes) Then
                        lstrSource = IO.Path.Combine(IO.Path.Combine(App.DestPictDir, lstrSubDir), lstrFileName)
                        If IO.File.Exists(lstrSource) Then
                            .SendFile(lstrSource, lstrPictPath & "/" & lstrFileName, FTPFileTransferType.Binary)
                        Else
                            tstrError &= "FTP Missing picture: " & lstrSource & Environment.NewLine
                        End If
                        Me.CurrInc()

                        lstrSource = IO.Path.Combine(IO.Path.Combine(App.DestThumbDir, lstrSubDir), lstrFileName)
                        If IO.File.Exists(lstrSource) Then
                            .SendFile(lstrSource, lstrThumbPath & "/" & lstrFileName, FTPFileTransferType.Binary)
                        Else
                            tstrError &= "FTP Missing thumbnail: " & lstrSource & Environment.NewLine
                        End If
                    End If
                    Me.CurrInc()
                Next
            End With
        Finally
            If lobjConn IsNot Nothing Then
                Try
                    lobjConn.Close()
                Catch
                End Try
                lobjConn = Nothing
            End If
        End Try
    End Sub

    Private Sub ExportTable(ByVal tstrTable As String, ByVal tstrFile As String, Optional ByVal tintRowLimit As Integer = 500)
        Dim lstrInsert As String = Nothing
        Dim lstrSQL As String = Nothing
        Dim lstrScript As String = Nothing
        Dim lobjDA As MySql.Data.MySqlClient.MySqlDataAdapter
        Dim lobjCB As MySql.Data.MySqlClient.MySqlCommandBuilder
        Dim lobjTable As Data.DataTable
        Dim ldatDate As Date

        lobjDA = New MySql.Data.MySqlClient.MySqlDataAdapter("select * from " & tstrTable, App.Connection)
        lobjCB = New MySql.Data.MySqlClient.MySqlCommandBuilder(lobjDA)
        lobjTable = New Data.DataTable With {
            .Locale = System.Globalization.CultureInfo.InvariantCulture
        }
        lobjDA.Fill(lobjTable)

        For I As Integer = 0 To lobjTable.Columns.Count - 1
            If Not IsES(lstrInsert) Then lstrInsert &= ", "
            lstrInsert &= "`" & lobjTable.Columns(I).ColumnName & "`"
        Next
        lstrInsert = "INSERT INTO `" & tstrTable & "` (" & lstrInsert & ") VALUES "

        For I As Integer = 0 To lobjTable.Rows.Count - 1
            Dim lstrValues As String = Nothing
            For J As Integer = 0 To lobjTable.Columns.Count - 1
                If Not IsES(lstrValues) Then lstrValues &= ", "
                If lobjTable.Rows(I).Item(J) Is DBNull.Value Then
                    lstrValues &= "NULL"
                Else
                    Select Case lobjTable.Columns(J).DataType.Name
                        Case "String"
                            lstrValues &= "'" & lobjTable.Rows(I).Item(J).ToString.Replace("'", "\'") & "'"

                        Case "DateTime"
                            ldatDate = CDate(lobjTable.Rows(I).Item(J))
                            If ldatDate = ldatDate.Date Then
                                lstrValues &= "'" & ldatDate.ToString("yyyy-MM-dd") & "'"
                            Else
                                lstrValues &= "'" & ldatDate.ToString("yyyy-MM-dd HH:mm:ss") & "'"
                            End If

                        Case "Boolean"
                            'lstrValues &= CStr(IIf(IsTrue(lobjTable.Rows(I).Item(J).ToString), "1", "0"))
                            If IsTrue(lobjTable.Rows(I).Item(J).ToString) Then
                                lstrValues &= "1"
                            Else
                                lstrValues &= "0"
                            End If

                        Case Else
                            lstrValues &= lobjTable.Rows(I).Item(J).ToString

                    End Select
                End If
            Next
            If Not IsES(lstrSQL) Then lstrSQL &= ", "
            lstrSQL &= "(" & lstrValues & ")"

            If (I + 1) Mod tintRowLimit = 0 Then
                lstrScript &= lstrInsert & lstrSQL & ";" & Environment.NewLine
                lstrSQL = Nothing
            End If
        Next
        If Not IsES(lstrSQL) Then lstrScript &= lstrInsert & lstrSQL & ";" & Environment.NewLine

        Using lobjWriter As New IO.StreamWriter(tstrFile, True, System.Text.Encoding.UTF8)
            lobjWriter.WriteLine(lstrScript)
            lobjWriter.Close()
        End Using
    End Sub

End Class
