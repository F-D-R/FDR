<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mnuMain = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCompare = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPublish = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCheck = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSQL = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFTP = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDatabase = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuConnect = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDisconnect = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSep2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuOpt = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuUser = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCat = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAlbum = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPict = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLonelyPictures = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSep3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuCatAlb = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAlbPict = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSep4 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSelect = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSearch = New System.Windows.Forms.ToolStripMenuItem()
        Me.cboConn = New System.Windows.Forms.ComboBox()
        Me.btnRename = New System.Windows.Forms.Button()
        Me.btnCompare = New System.Windows.Forms.Button()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.btnDisconnect = New System.Windows.Forms.Button()
        Me.btnAlbum = New System.Windows.Forms.Button()
        Me.btnPublish = New System.Windows.Forms.Button()
        Me.btnCheck = New System.Windows.Forms.Button()
        Me.mnuMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'mnuMain
        '
        Me.mnuMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuTools, Me.mnuDatabase})
        Me.mnuMain.Location = New System.Drawing.Point(0, 0)
        Me.mnuMain.Name = "mnuMain"
        Me.mnuMain.Size = New System.Drawing.Size(407, 24)
        Me.mnuMain.TabIndex = 0
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(37, 20)
        Me.mnuFile.Text = "&File"
        '
        'mnuExit
        '
        Me.mnuExit.Image = Global.FDRTools.My.Resources.Resources.close
        Me.mnuExit.ImageTransparentColor = System.Drawing.Color.Silver
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(92, 22)
        Me.mnuExit.Text = "&Exit"
        '
        'mnuTools
        '
        Me.mnuTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRename, Me.mnuCompare, Me.mnuPublish, Me.mnuCheck, Me.mnuSep1, Me.mnuSQL, Me.mnuFTP})
        Me.mnuTools.Name = "mnuTools"
        Me.mnuTools.Size = New System.Drawing.Size(48, 20)
        Me.mnuTools.Text = "&Tools"
        '
        'mnuRename
        '
        Me.mnuRename.Name = "mnuRename"
        Me.mnuRename.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D1), System.Windows.Forms.Keys)
        Me.mnuRename.Size = New System.Drawing.Size(163, 22)
        Me.mnuRename.Text = "&Rename"
        '
        'mnuCompare
        '
        Me.mnuCompare.Name = "mnuCompare"
        Me.mnuCompare.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D2), System.Windows.Forms.Keys)
        Me.mnuCompare.Size = New System.Drawing.Size(163, 22)
        Me.mnuCompare.Text = "&Compare"
        '
        'mnuPublish
        '
        Me.mnuPublish.Name = "mnuPublish"
        Me.mnuPublish.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D3), System.Windows.Forms.Keys)
        Me.mnuPublish.Size = New System.Drawing.Size(163, 22)
        Me.mnuPublish.Text = "&Publish"
        '
        'mnuCheck
        '
        Me.mnuCheck.Name = "mnuCheck"
        Me.mnuCheck.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D4), System.Windows.Forms.Keys)
        Me.mnuCheck.Size = New System.Drawing.Size(163, 22)
        Me.mnuCheck.Text = "Check"
        '
        'mnuSep1
        '
        Me.mnuSep1.Name = "mnuSep1"
        Me.mnuSep1.Size = New System.Drawing.Size(160, 6)
        Me.mnuSep1.Visible = False
        '
        'mnuSQL
        '
        Me.mnuSQL.Name = "mnuSQL"
        Me.mnuSQL.Size = New System.Drawing.Size(163, 22)
        Me.mnuSQL.Text = "&SQL"
        Me.mnuSQL.Visible = False
        '
        'mnuFTP
        '
        Me.mnuFTP.Name = "mnuFTP"
        Me.mnuFTP.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D5), System.Windows.Forms.Keys)
        Me.mnuFTP.Size = New System.Drawing.Size(163, 22)
        Me.mnuFTP.Text = "&FTP"
        Me.mnuFTP.Visible = False
        '
        'mnuDatabase
        '
        Me.mnuDatabase.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuConnect, Me.mnuDisconnect, Me.mnuSep2, Me.mnuOpt, Me.mnuUser, Me.mnuCat, Me.mnuAlbum, Me.mnuPict, Me.mnuLonelyPictures, Me.mnuSep3, Me.mnuCatAlb, Me.mnuAlbPict, Me.mnuSep4, Me.mnuSelect, Me.mnuSearch})
        Me.mnuDatabase.Name = "mnuDatabase"
        Me.mnuDatabase.Size = New System.Drawing.Size(67, 20)
        Me.mnuDatabase.Text = "&Database"
        '
        'mnuConnect
        '
        Me.mnuConnect.Name = "mnuConnect"
        Me.mnuConnect.Size = New System.Drawing.Size(164, 22)
        Me.mnuConnect.Text = "&Connect"
        '
        'mnuDisconnect
        '
        Me.mnuDisconnect.Name = "mnuDisconnect"
        Me.mnuDisconnect.Size = New System.Drawing.Size(164, 22)
        Me.mnuDisconnect.Text = "&Disconnect"
        '
        'mnuSep2
        '
        Me.mnuSep2.Name = "mnuSep2"
        Me.mnuSep2.Size = New System.Drawing.Size(161, 6)
        '
        'mnuOpt
        '
        Me.mnuOpt.Name = "mnuOpt"
        Me.mnuOpt.Size = New System.Drawing.Size(164, 22)
        Me.mnuOpt.Text = "&Options"
        '
        'mnuUser
        '
        Me.mnuUser.Name = "mnuUser"
        Me.mnuUser.Size = New System.Drawing.Size(164, 22)
        Me.mnuUser.Text = "&Users"
        '
        'mnuCat
        '
        Me.mnuCat.Name = "mnuCat"
        Me.mnuCat.Size = New System.Drawing.Size(164, 22)
        Me.mnuCat.Text = "Ca&tegories"
        '
        'mnuAlbum
        '
        Me.mnuAlbum.Name = "mnuAlbum"
        Me.mnuAlbum.Size = New System.Drawing.Size(164, 22)
        Me.mnuAlbum.Text = "&Albums"
        '
        'mnuPict
        '
        Me.mnuPict.Name = "mnuPict"
        Me.mnuPict.Size = New System.Drawing.Size(164, 22)
        Me.mnuPict.Text = "&Pictures"
        '
        'mnuLonelyPictures
        '
        Me.mnuLonelyPictures.Name = "mnuLonelyPictures"
        Me.mnuLonelyPictures.Size = New System.Drawing.Size(164, 22)
        Me.mnuLonelyPictures.Text = "Lonely pictures"
        '
        'mnuSep3
        '
        Me.mnuSep3.Name = "mnuSep3"
        Me.mnuSep3.Size = New System.Drawing.Size(161, 6)
        '
        'mnuCatAlb
        '
        Me.mnuCatAlb.Name = "mnuCatAlb"
        Me.mnuCatAlb.Size = New System.Drawing.Size(164, 22)
        Me.mnuCatAlb.Text = "Category albums"
        '
        'mnuAlbPict
        '
        Me.mnuAlbPict.Name = "mnuAlbPict"
        Me.mnuAlbPict.Size = New System.Drawing.Size(164, 22)
        Me.mnuAlbPict.Text = "Album pictures"
        '
        'mnuSep4
        '
        Me.mnuSep4.Name = "mnuSep4"
        Me.mnuSep4.Size = New System.Drawing.Size(161, 6)
        '
        'mnuSelect
        '
        Me.mnuSelect.Name = "mnuSelect"
        Me.mnuSelect.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mnuSelect.Size = New System.Drawing.Size(164, 22)
        Me.mnuSelect.Text = "Select..."
        '
        'mnuSearch
        '
        Me.mnuSearch.Name = "mnuSearch"
        Me.mnuSearch.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.mnuSearch.Size = New System.Drawing.Size(164, 22)
        Me.mnuSearch.Text = "Search..."
        '
        'cboConn
        '
        Me.cboConn.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboConn.FormattingEnabled = True
        Me.cboConn.Items.AddRange(New Object() {"Persist Security Info=False;database=fdr;server=localhost;user id=fdr;Password=eo" & _
                "s10d", "server=localhost;database=fdr", "Persist Security Info=False;database=fdr;server=https://www.dataglobe.eu/mysql/ ;" & _
                "user id=fdr;Password=eos10d", "Persist Security Info=False;database=fdr;server=localhost;user id=;Password="})
        Me.cboConn.Location = New System.Drawing.Point(4, 117)
        Me.cboConn.Name = "cboConn"
        Me.cboConn.Size = New System.Drawing.Size(397, 21)
        Me.cboConn.TabIndex = 10
        '
        'btnRename
        '
        Me.btnRename.Location = New System.Drawing.Point(5, 36)
        Me.btnRename.Name = "btnRename"
        Me.btnRename.Size = New System.Drawing.Size(128, 48)
        Me.btnRename.TabIndex = 1
        Me.btnRename.Text = "Rename"
        '
        'btnCompare
        '
        Me.btnCompare.Location = New System.Drawing.Point(139, 36)
        Me.btnCompare.Name = "btnCompare"
        Me.btnCompare.Size = New System.Drawing.Size(128, 48)
        Me.btnCompare.TabIndex = 2
        Me.btnCompare.Text = "Compare"
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(5, 144)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(128, 48)
        Me.btnConnect.TabIndex = 11
        Me.btnConnect.Text = "Connect"
        '
        'btnDisconnect
        '
        Me.btnDisconnect.Location = New System.Drawing.Point(139, 144)
        Me.btnDisconnect.Name = "btnDisconnect"
        Me.btnDisconnect.Size = New System.Drawing.Size(128, 48)
        Me.btnDisconnect.TabIndex = 12
        Me.btnDisconnect.Text = "Disconnect"
        '
        'btnAlbum
        '
        Me.btnAlbum.Location = New System.Drawing.Point(273, 144)
        Me.btnAlbum.Name = "btnAlbum"
        Me.btnAlbum.Size = New System.Drawing.Size(128, 48)
        Me.btnAlbum.TabIndex = 13
        Me.btnAlbum.Text = "Albums"
        '
        'btnPublish
        '
        Me.btnPublish.Location = New System.Drawing.Point(273, 36)
        Me.btnPublish.Name = "btnPublish"
        Me.btnPublish.Size = New System.Drawing.Size(128, 48)
        Me.btnPublish.TabIndex = 3
        Me.btnPublish.Text = "Publish"
        '
        'btnCheck
        '
        Me.btnCheck.Location = New System.Drawing.Point(4, 88)
        Me.btnCheck.Name = "btnCheck"
        Me.btnCheck.Size = New System.Drawing.Size(128, 24)
        Me.btnCheck.TabIndex = 4
        Me.btnCheck.Text = "Check"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(407, 199)
        Me.Controls.Add(Me.btnPublish)
        Me.Controls.Add(Me.btnAlbum)
        Me.Controls.Add(Me.btnDisconnect)
        Me.Controls.Add(Me.btnConnect)
        Me.Controls.Add(Me.cboConn)
        Me.Controls.Add(Me.btnCompare)
        Me.Controls.Add(Me.btnCheck)
        Me.Controls.Add(Me.btnRename)
        Me.Controls.Add(Me.mnuMain)
        Me.MainMenuStrip = Me.mnuMain
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FDR Tools"
        Me.mnuMain.ResumeLayout(False)
        Me.mnuMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents mnuMain As System.Windows.Forms.MenuStrip
    Private WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuTools As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuRename As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuCompare As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuSQL As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents cboConn As System.Windows.Forms.ComboBox
    Private WithEvents mnuDatabase As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuConnect As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuDisconnect As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuSep2 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents mnuUser As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuCat As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuAlbum As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuPict As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuSep3 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents mnuCatAlb As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuAlbPict As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuSep4 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents mnuSelect As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents btnRename As System.Windows.Forms.Button
    Private WithEvents btnCompare As System.Windows.Forms.Button
    Private WithEvents mnuLonelyPictures As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents btnConnect As System.Windows.Forms.Button
    Private WithEvents btnDisconnect As System.Windows.Forms.Button
    Private WithEvents btnAlbum As System.Windows.Forms.Button
    Private WithEvents mnuSearch As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuFTP As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuPublish As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuSep1 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents btnPublish As System.Windows.Forms.Button
	Private WithEvents mnuOpt As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents mnuCheck As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents btnCheck As System.Windows.Forms.Button

End Class
