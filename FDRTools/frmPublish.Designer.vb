<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPublish
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPublish))
        Me.grpSource = New System.Windows.Forms.GroupBox
        Me.optDest = New System.Windows.Forms.RadioButton
        Me.optSource = New System.Windows.Forms.RadioButton
        Me.cboFilter = New System.Windows.Forms.ComboBox
        Me.txtRoot = New System.Windows.Forms.TextBox
        Me.lblDirectories = New System.Windows.Forms.Label
        Me.lblRoot = New System.Windows.Forms.Label
        Me.lblFilter = New System.Windows.Forms.Label
        Me.grpGenerate = New System.Windows.Forms.GroupBox
        Me.chkExport = New System.Windows.Forms.CheckBox
        Me.grpExport = New System.Windows.Forms.GroupBox
        Me.txtRowLimit = New System.Windows.Forms.TextBox
        Me.lblRowLimit = New System.Windows.Forms.Label
        Me.pnlDatabase = New System.Windows.Forms.Panel
        Me.txtLocationHU = New System.Windows.Forms.TextBox
        Me.lblOwner = New System.Windows.Forms.Label
        Me.cboOwner = New System.Windows.Forms.ComboBox
        Me.lblCaptured = New System.Windows.Forms.Label
        Me.lblLocationHU = New System.Windows.Forms.Label
        Me.txtCaptured = New System.Windows.Forms.TextBox
        Me.chkAutoOrder = New System.Windows.Forms.CheckBox
        Me.lblLocationEN = New System.Windows.Forms.Label
        Me.txtLocationEN = New System.Windows.Forms.TextBox
        Me.chkInsert = New System.Windows.Forms.CheckBox
        Me.chkUpdate = New System.Windows.Forms.CheckBox
        Me.grpInsert = New System.Windows.Forms.GroupBox
        Me.btnAddAlbum = New System.Windows.Forms.Button
        Me.txtAlbums = New System.Windows.Forms.TextBox
        Me.lblAlbums = New System.Windows.Forms.Label
        Me.grpUpdate = New System.Windows.Forms.GroupBox
        Me.lstFields = New System.Windows.Forms.CheckedListBox
        Me.lblFields = New System.Windows.Forms.Label
        Me.barCurrent = New System.Windows.Forms.ProgressBar
        Me.barAll = New System.Windows.Forms.ProgressBar
        Me.dlgFolder = New System.Windows.Forms.FolderBrowserDialog
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.grpResize = New System.Windows.Forms.GroupBox
        Me.txtActionName = New System.Windows.Forms.TextBox
        Me.lblActionName = New System.Windows.Forms.Label
        Me.txtActionSet = New System.Windows.Forms.TextBox
        Me.lblActionSet = New System.Windows.Forms.Label
        Me.chkOverwrite = New System.Windows.Forms.CheckBox
        Me.lblKB = New System.Windows.Forms.Label
        Me.txtMinSize = New System.Windows.Forms.TextBox
        Me.lblMinSize = New System.Windows.Forms.Label
        Me.txtRemoteHost = New System.Windows.Forms.TextBox
        Me.txtRemoteLogin = New System.Windows.Forms.TextBox
        Me.txtRemotePwd = New System.Windows.Forms.TextBox
        Me.chkResize = New System.Windows.Forms.CheckBox
        Me.grpFTP = New System.Windows.Forms.GroupBox
        Me.chkRemotePassive = New System.Windows.Forms.CheckBox
        Me.lblRemotePwd = New System.Windows.Forms.Label
        Me.lblRemoteLogin = New System.Windows.Forms.Label
        Me.lblRemoteHost = New System.Windows.Forms.Label
        Me.chkFTP = New System.Windows.Forms.CheckBox
        Me.btnStart = New System.Windows.Forms.Button
        Me.treDirectories = New FDRTools.TreeViewMS
        Me.grpSource.SuspendLayout()
        Me.grpGenerate.SuspendLayout()
        Me.grpExport.SuspendLayout()
        Me.pnlDatabase.SuspendLayout()
        Me.grpInsert.SuspendLayout()
        Me.grpUpdate.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpResize.SuspendLayout()
        Me.grpFTP.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpSource
        '
        Me.grpSource.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSource.Controls.Add(Me.optDest)
        Me.grpSource.Controls.Add(Me.optSource)
        Me.grpSource.Controls.Add(Me.treDirectories)
        Me.grpSource.Controls.Add(Me.cboFilter)
        Me.grpSource.Controls.Add(Me.txtRoot)
        Me.grpSource.Controls.Add(Me.lblDirectories)
        Me.grpSource.Controls.Add(Me.lblRoot)
        Me.grpSource.Controls.Add(Me.lblFilter)
        Me.grpSource.Location = New System.Drawing.Point(4, 4)
        Me.grpSource.Name = "grpSource"
        Me.grpSource.Size = New System.Drawing.Size(534, 249)
        Me.grpSource.TabIndex = 0
        Me.grpSource.TabStop = False
        Me.grpSource.Text = "Source"
        '
        'optDest
        '
        Me.optDest.AutoSize = True
        Me.optDest.Location = New System.Drawing.Point(212, 14)
        Me.optDest.Name = "optDest"
        Me.optDest.Size = New System.Drawing.Size(78, 17)
        Me.optDest.TabIndex = 1
        Me.optDest.Text = "&Destination"
        Me.optDest.UseVisualStyleBackColor = True
        '
        'optSource
        '
        Me.optSource.AutoSize = True
        Me.optSource.Checked = True
        Me.optSource.Location = New System.Drawing.Point(112, 14)
        Me.optSource.Name = "optSource"
        Me.optSource.Size = New System.Drawing.Size(59, 17)
        Me.optSource.TabIndex = 0
        Me.optSource.TabStop = True
        Me.optSource.Text = "Sour&ce"
        '
        'cboFilter
        '
        Me.cboFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cboFilter.FormattingEnabled = True
        Me.cboFilter.Location = New System.Drawing.Point(112, 221)
        Me.cboFilter.Name = "cboFilter"
        Me.cboFilter.Size = New System.Drawing.Size(150, 21)
        Me.cboFilter.TabIndex = 7
        '
        'txtRoot
        '
        Me.txtRoot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRoot.Location = New System.Drawing.Point(112, 33)
        Me.txtRoot.Name = "txtRoot"
        Me.txtRoot.ReadOnly = True
        Me.txtRoot.Size = New System.Drawing.Size(410, 20)
        Me.txtRoot.TabIndex = 3
        Me.txtRoot.TabStop = False
        '
        'lblDirectories
        '
        Me.lblDirectories.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblDirectories.Location = New System.Drawing.Point(8, 53)
        Me.lblDirectories.Name = "lblDirectories"
        Me.lblDirectories.Size = New System.Drawing.Size(100, 16)
        Me.lblDirectories.TabIndex = 4
        Me.lblDirectories.Text = "Directories:"
        Me.lblDirectories.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblRoot
        '
        Me.lblRoot.Location = New System.Drawing.Point(8, 37)
        Me.lblRoot.Name = "lblRoot"
        Me.lblRoot.Size = New System.Drawing.Size(100, 16)
        Me.lblRoot.TabIndex = 2
        Me.lblRoot.Text = "Root:"
        Me.lblRoot.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFilter
        '
        Me.lblFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblFilter.Location = New System.Drawing.Point(8, 222)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(100, 16)
        Me.lblFilter.TabIndex = 6
        Me.lblFilter.Text = "Filter:"
        Me.lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpGenerate
        '
        Me.grpGenerate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpGenerate.Controls.Add(Me.chkExport)
        Me.grpGenerate.Controls.Add(Me.grpExport)
        Me.grpGenerate.Controls.Add(Me.pnlDatabase)
        Me.grpGenerate.Controls.Add(Me.chkInsert)
        Me.grpGenerate.Controls.Add(Me.chkUpdate)
        Me.grpGenerate.Controls.Add(Me.grpInsert)
        Me.grpGenerate.Controls.Add(Me.grpUpdate)
        Me.grpGenerate.Location = New System.Drawing.Point(4, 311)
        Me.grpGenerate.Name = "grpGenerate"
        Me.grpGenerate.Size = New System.Drawing.Size(534, 273)
        Me.grpGenerate.TabIndex = 3
        Me.grpGenerate.TabStop = False
        Me.grpGenerate.Text = "Database"
        '
        'chkExport
        '
        Me.chkExport.AutoSize = True
        Me.chkExport.Checked = True
        Me.chkExport.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.chkExport.Location = New System.Drawing.Point(14, 220)
        Me.chkExport.Name = "chkExport"
        Me.chkExport.Size = New System.Drawing.Size(62, 17)
        Me.chkExport.TabIndex = 5
        Me.chkExport.Text = "&Export"
        Me.chkExport.UseVisualStyleBackColor = True
        '
        'grpExport
        '
        Me.grpExport.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpExport.Controls.Add(Me.txtRowLimit)
        Me.grpExport.Controls.Add(Me.lblRowLimit)
        Me.grpExport.Location = New System.Drawing.Point(6, 221)
        Me.grpExport.Name = "grpExport"
        Me.grpExport.Size = New System.Drawing.Size(522, 46)
        Me.grpExport.TabIndex = 6
        Me.grpExport.TabStop = False
        '
        'txtRowLimit
        '
        Me.ErrorProvider1.SetIconPadding(Me.txtRowLimit, 4)
        Me.txtRowLimit.Location = New System.Drawing.Point(106, 19)
        Me.txtRowLimit.Name = "txtRowLimit"
        Me.txtRowLimit.Size = New System.Drawing.Size(47, 20)
        Me.txtRowLimit.TabIndex = 1
        Me.txtRowLimit.Text = "500"
        Me.txtRowLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblRowLimit
        '
        Me.lblRowLimit.Location = New System.Drawing.Point(2, 20)
        Me.lblRowLimit.Name = "lblRowLimit"
        Me.lblRowLimit.Size = New System.Drawing.Size(100, 16)
        Me.lblRowLimit.TabIndex = 0
        Me.lblRowLimit.Text = "Row limit:"
        Me.lblRowLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlDatabase
        '
        Me.pnlDatabase.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlDatabase.Controls.Add(Me.txtLocationHU)
        Me.pnlDatabase.Controls.Add(Me.lblOwner)
        Me.pnlDatabase.Controls.Add(Me.cboOwner)
        Me.pnlDatabase.Controls.Add(Me.lblCaptured)
        Me.pnlDatabase.Controls.Add(Me.lblLocationHU)
        Me.pnlDatabase.Controls.Add(Me.txtCaptured)
        Me.pnlDatabase.Controls.Add(Me.chkAutoOrder)
        Me.pnlDatabase.Controls.Add(Me.lblLocationEN)
        Me.pnlDatabase.Controls.Add(Me.txtLocationEN)
        Me.pnlDatabase.Location = New System.Drawing.Point(6, 12)
        Me.pnlDatabase.Name = "pnlDatabase"
        Me.pnlDatabase.Size = New System.Drawing.Size(522, 71)
        Me.pnlDatabase.TabIndex = 0
        '
        'txtLocationHU
        '
        Me.txtLocationHU.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLocationHU.Location = New System.Drawing.Point(348, 0)
        Me.txtLocationHU.Name = "txtLocationHU"
        Me.txtLocationHU.Size = New System.Drawing.Size(168, 20)
        Me.txtLocationHU.TabIndex = 5
        '
        'lblOwner
        '
        Me.lblOwner.Location = New System.Drawing.Point(2, 1)
        Me.lblOwner.Name = "lblOwner"
        Me.lblOwner.Size = New System.Drawing.Size(100, 16)
        Me.lblOwner.TabIndex = 0
        Me.lblOwner.Text = "O&wner:"
        Me.lblOwner.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboOwner
        '
        Me.cboOwner.FormattingEnabled = True
        Me.cboOwner.Location = New System.Drawing.Point(106, 0)
        Me.cboOwner.Name = "cboOwner"
        Me.cboOwner.Size = New System.Drawing.Size(150, 21)
        Me.cboOwner.TabIndex = 1
        '
        'lblCaptured
        '
        Me.lblCaptured.Location = New System.Drawing.Point(1, 30)
        Me.lblCaptured.Name = "lblCaptured"
        Me.lblCaptured.Size = New System.Drawing.Size(100, 16)
        Me.lblCaptured.TabIndex = 2
        Me.lblCaptured.Text = "&Captured [auto]:"
        Me.lblCaptured.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblLocationHU
        '
        Me.lblLocationHU.Location = New System.Drawing.Point(263, 1)
        Me.lblLocationHU.Name = "lblLocationHU"
        Me.lblLocationHU.Size = New System.Drawing.Size(79, 16)
        Me.lblLocationHU.TabIndex = 4
        Me.lblLocationHU.Text = "Location &HU:"
        Me.lblLocationHU.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCaptured
        '
        Me.txtCaptured.Location = New System.Drawing.Point(106, 29)
        Me.txtCaptured.Name = "txtCaptured"
        Me.txtCaptured.Size = New System.Drawing.Size(150, 20)
        Me.txtCaptured.TabIndex = 3
        '
        'chkAutoOrder
        '
        Me.chkAutoOrder.AutoSize = True
        Me.chkAutoOrder.Location = New System.Drawing.Point(106, 53)
        Me.chkAutoOrder.Name = "chkAutoOrder"
        Me.chkAutoOrder.Size = New System.Drawing.Size(124, 17)
        Me.chkAutoOrder.TabIndex = 8
        Me.chkAutoOrder.Text = "Auto increment order"
        Me.chkAutoOrder.UseVisualStyleBackColor = True
        '
        'lblLocationEN
        '
        Me.lblLocationEN.Location = New System.Drawing.Point(263, 29)
        Me.lblLocationEN.Name = "lblLocationEN"
        Me.lblLocationEN.Size = New System.Drawing.Size(79, 16)
        Me.lblLocationEN.TabIndex = 6
        Me.lblLocationEN.Text = "Location E&N:"
        Me.lblLocationEN.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtLocationEN
        '
        Me.txtLocationEN.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLocationEN.Location = New System.Drawing.Point(348, 28)
        Me.txtLocationEN.Name = "txtLocationEN"
        Me.txtLocationEN.Size = New System.Drawing.Size(168, 20)
        Me.txtLocationEN.TabIndex = 7
        '
        'chkInsert
        '
        Me.chkInsert.AutoSize = True
        Me.chkInsert.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.chkInsert.Location = New System.Drawing.Point(14, 168)
        Me.chkInsert.Name = "chkInsert"
        Me.chkInsert.Size = New System.Drawing.Size(58, 17)
        Me.chkInsert.TabIndex = 3
        Me.chkInsert.Text = "&Insert"
        Me.chkInsert.UseVisualStyleBackColor = True
        '
        'chkUpdate
        '
        Me.chkUpdate.AutoSize = True
        Me.chkUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.chkUpdate.Location = New System.Drawing.Point(14, 88)
        Me.chkUpdate.Name = "chkUpdate"
        Me.chkUpdate.Size = New System.Drawing.Size(67, 17)
        Me.chkUpdate.TabIndex = 1
        Me.chkUpdate.Text = "&Update"
        Me.chkUpdate.UseVisualStyleBackColor = True
        '
        'grpInsert
        '
        Me.grpInsert.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpInsert.Controls.Add(Me.btnAddAlbum)
        Me.grpInsert.Controls.Add(Me.txtAlbums)
        Me.grpInsert.Controls.Add(Me.lblAlbums)
        Me.grpInsert.Location = New System.Drawing.Point(6, 169)
        Me.grpInsert.Name = "grpInsert"
        Me.grpInsert.Size = New System.Drawing.Size(522, 46)
        Me.grpInsert.TabIndex = 4
        Me.grpInsert.TabStop = False
        '
        'btnAddAlbum
        '
        Me.btnAddAlbum.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddAlbum.Location = New System.Drawing.Point(495, 20)
        Me.btnAddAlbum.Name = "btnAddAlbum"
        Me.btnAddAlbum.Size = New System.Drawing.Size(24, 20)
        Me.btnAddAlbum.TabIndex = 2
        Me.btnAddAlbum.Text = "..."
        Me.btnAddAlbum.UseVisualStyleBackColor = True
        '
        'txtAlbums
        '
        Me.txtAlbums.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAlbums.Location = New System.Drawing.Point(106, 19)
        Me.txtAlbums.Name = "txtAlbums"
        Me.txtAlbums.Size = New System.Drawing.Size(383, 20)
        Me.txtAlbums.TabIndex = 1
        '
        'lblAlbums
        '
        Me.lblAlbums.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblAlbums.Location = New System.Drawing.Point(2, 20)
        Me.lblAlbums.Name = "lblAlbums"
        Me.lblAlbums.Size = New System.Drawing.Size(100, 16)
        Me.lblAlbums.TabIndex = 0
        Me.lblAlbums.Text = "&Albums:"
        Me.lblAlbums.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpUpdate
        '
        Me.grpUpdate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpUpdate.Controls.Add(Me.lstFields)
        Me.grpUpdate.Controls.Add(Me.lblFields)
        Me.grpUpdate.Location = New System.Drawing.Point(6, 89)
        Me.grpUpdate.Name = "grpUpdate"
        Me.grpUpdate.Size = New System.Drawing.Size(522, 74)
        Me.grpUpdate.TabIndex = 2
        Me.grpUpdate.TabStop = False
        '
        'lstFields
        '
        Me.lstFields.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstFields.CheckOnClick = True
        Me.lstFields.Location = New System.Drawing.Point(106, 19)
        Me.lstFields.MultiColumn = True
        Me.lstFields.Name = "lstFields"
        Me.lstFields.Size = New System.Drawing.Size(410, 49)
        Me.lstFields.TabIndex = 1
        '
        'lblFields
        '
        Me.lblFields.Location = New System.Drawing.Point(5, 19)
        Me.lblFields.Name = "lblFields"
        Me.lblFields.Size = New System.Drawing.Size(97, 16)
        Me.lblFields.TabIndex = 0
        Me.lblFields.Text = "&Fields to update:"
        Me.lblFields.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'barCurrent
        '
        Me.barCurrent.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.barCurrent.Location = New System.Drawing.Point(4, 660)
        Me.barCurrent.Name = "barCurrent"
        Me.barCurrent.Size = New System.Drawing.Size(412, 10)
        Me.barCurrent.Step = 1
        Me.barCurrent.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.barCurrent.TabIndex = 21
        '
        'barAll
        '
        Me.barAll.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.barAll.Location = New System.Drawing.Point(4, 644)
        Me.barAll.Name = "barAll"
        Me.barAll.Size = New System.Drawing.Size(412, 10)
        Me.barAll.Step = 1
        Me.barAll.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.barAll.TabIndex = 20
        '
        'dlgFolder
        '
        Me.dlgFolder.RootFolder = System.Environment.SpecialFolder.MyComputer
        Me.dlgFolder.ShowNewFolderButton = False
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'grpResize
        '
        Me.grpResize.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpResize.Controls.Add(Me.txtActionName)
        Me.grpResize.Controls.Add(Me.lblActionName)
        Me.grpResize.Controls.Add(Me.txtActionSet)
        Me.grpResize.Controls.Add(Me.lblActionSet)
        Me.grpResize.Controls.Add(Me.chkOverwrite)
        Me.grpResize.Controls.Add(Me.lblKB)
        Me.grpResize.Controls.Add(Me.txtMinSize)
        Me.grpResize.Controls.Add(Me.lblMinSize)
        Me.ErrorProvider1.SetIconPadding(Me.grpResize, 20)
        Me.grpResize.Location = New System.Drawing.Point(4, 259)
        Me.grpResize.Name = "grpResize"
        Me.grpResize.Size = New System.Drawing.Size(534, 46)
        Me.grpResize.TabIndex = 2
        Me.grpResize.TabStop = False
        '
        'txtActionName
        '
        Me.ErrorProvider1.SetIconPadding(Me.txtActionName, 20)
        Me.txtActionName.Location = New System.Drawing.Point(456, 19)
        Me.txtActionName.Name = "txtActionName"
        Me.txtActionName.Size = New System.Drawing.Size(67, 20)
        Me.txtActionName.TabIndex = 7
        Me.txtActionName.Text = "web resize"
        '
        'lblActionName
        '
        Me.lblActionName.Location = New System.Drawing.Point(407, 20)
        Me.lblActionName.Name = "lblActionName"
        Me.lblActionName.Size = New System.Drawing.Size(51, 16)
        Me.lblActionName.TabIndex = 6
        Me.lblActionName.Text = "Action:"
        Me.lblActionName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtActionSet
        '
        Me.ErrorProvider1.SetIconPadding(Me.txtActionSet, 20)
        Me.txtActionSet.Location = New System.Drawing.Point(354, 19)
        Me.txtActionSet.Name = "txtActionSet"
        Me.txtActionSet.Size = New System.Drawing.Size(47, 20)
        Me.txtActionSet.TabIndex = 5
        Me.txtActionSet.Text = "FDR"
        '
        'lblActionSet
        '
        Me.lblActionSet.Location = New System.Drawing.Point(269, 20)
        Me.lblActionSet.Name = "lblActionSet"
        Me.lblActionSet.Size = New System.Drawing.Size(79, 16)
        Me.lblActionSet.TabIndex = 4
        Me.lblActionSet.Text = "Set:"
        Me.lblActionSet.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chkOverwrite
        '
        Me.chkOverwrite.AutoSize = True
        Me.chkOverwrite.Checked = True
        Me.chkOverwrite.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOverwrite.Location = New System.Drawing.Point(191, 21)
        Me.chkOverwrite.Name = "chkOverwrite"
        Me.chkOverwrite.Size = New System.Drawing.Size(71, 17)
        Me.chkOverwrite.TabIndex = 3
        Me.chkOverwrite.Text = "Overwrite"
        Me.chkOverwrite.UseVisualStyleBackColor = True
        '
        'lblKB
        '
        Me.lblKB.AutoSize = True
        Me.lblKB.Location = New System.Drawing.Point(160, 22)
        Me.lblKB.Name = "lblKB"
        Me.lblKB.Size = New System.Drawing.Size(20, 13)
        Me.lblKB.TabIndex = 2
        Me.lblKB.Text = "kB"
        Me.lblKB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMinSize
        '
        Me.ErrorProvider1.SetIconPadding(Me.txtMinSize, 20)
        Me.txtMinSize.Location = New System.Drawing.Point(112, 19)
        Me.txtMinSize.Name = "txtMinSize"
        Me.txtMinSize.Size = New System.Drawing.Size(47, 20)
        Me.txtMinSize.TabIndex = 1
        Me.txtMinSize.Text = "60"
        Me.txtMinSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMinSize
        '
        Me.lblMinSize.Location = New System.Drawing.Point(7, 20)
        Me.lblMinSize.Name = "lblMinSize"
        Me.lblMinSize.Size = New System.Drawing.Size(100, 16)
        Me.lblMinSize.TabIndex = 0
        Me.lblMinSize.Text = "Min. size:"
        Me.lblMinSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtRemoteHost
        '
        Me.ErrorProvider1.SetIconPadding(Me.txtRemoteHost, 20)
        Me.txtRemoteHost.Location = New System.Drawing.Point(112, 21)
        Me.txtRemoteHost.Name = "txtRemoteHost"
        Me.txtRemoteHost.Size = New System.Drawing.Size(114, 20)
        Me.txtRemoteHost.TabIndex = 1
        Me.txtRemoteHost.Text = "ftp.dataglobe.hu"
        '
        'txtRemoteLogin
        '
        Me.ErrorProvider1.SetIconPadding(Me.txtRemoteLogin, 20)
        Me.txtRemoteLogin.Location = New System.Drawing.Point(276, 22)
        Me.txtRemoteLogin.Name = "txtRemoteLogin"
        Me.txtRemoteLogin.Size = New System.Drawing.Size(57, 20)
        Me.txtRemoteLogin.TabIndex = 3
        Me.txtRemoteLogin.Text = "fdr"
        '
        'txtRemotePwd
        '
        Me.ErrorProvider1.SetIconPadding(Me.txtRemotePwd, 20)
        Me.txtRemotePwd.Location = New System.Drawing.Point(385, 22)
        Me.txtRemotePwd.Name = "txtRemotePwd"
        Me.txtRemotePwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtRemotePwd.Size = New System.Drawing.Size(57, 20)
        Me.txtRemotePwd.TabIndex = 5
        Me.txtRemotePwd.Text = "eos10d    "
        '
        'chkResize
        '
        Me.chkResize.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkResize.AutoSize = True
        Me.chkResize.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.chkResize.Location = New System.Drawing.Point(18, 258)
        Me.chkResize.Name = "chkResize"
        Me.chkResize.Size = New System.Drawing.Size(64, 17)
        Me.chkResize.TabIndex = 1
        Me.chkResize.Text = "&Resize"
        Me.chkResize.UseVisualStyleBackColor = True
        '
        'grpFTP
        '
        Me.grpFTP.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpFTP.Controls.Add(Me.chkRemotePassive)
        Me.grpFTP.Controls.Add(Me.txtRemotePwd)
        Me.grpFTP.Controls.Add(Me.lblRemotePwd)
        Me.grpFTP.Controls.Add(Me.txtRemoteLogin)
        Me.grpFTP.Controls.Add(Me.lblRemoteLogin)
        Me.grpFTP.Controls.Add(Me.txtRemoteHost)
        Me.grpFTP.Controls.Add(Me.lblRemoteHost)
        Me.grpFTP.Location = New System.Drawing.Point(4, 590)
        Me.grpFTP.Name = "grpFTP"
        Me.grpFTP.Size = New System.Drawing.Size(534, 46)
        Me.grpFTP.TabIndex = 5
        Me.grpFTP.TabStop = False
        '
        'chkRemotePassive
        '
        Me.chkRemotePassive.AutoSize = True
        Me.chkRemotePassive.Checked = True
        Me.chkRemotePassive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRemotePassive.Location = New System.Drawing.Point(462, 24)
        Me.chkRemotePassive.Name = "chkRemotePassive"
        Me.chkRemotePassive.Size = New System.Drawing.Size(63, 17)
        Me.chkRemotePassive.TabIndex = 6
        Me.chkRemotePassive.Text = "Passive"
        Me.chkRemotePassive.UseVisualStyleBackColor = True
        '
        'lblRemotePwd
        '
        Me.lblRemotePwd.Location = New System.Drawing.Point(338, 23)
        Me.lblRemotePwd.Name = "lblRemotePwd"
        Me.lblRemotePwd.Size = New System.Drawing.Size(41, 16)
        Me.lblRemotePwd.TabIndex = 4
        Me.lblRemotePwd.Text = "Pwd:"
        Me.lblRemotePwd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblRemoteLogin
        '
        Me.lblRemoteLogin.Location = New System.Drawing.Point(232, 23)
        Me.lblRemoteLogin.Name = "lblRemoteLogin"
        Me.lblRemoteLogin.Size = New System.Drawing.Size(41, 16)
        Me.lblRemoteLogin.TabIndex = 2
        Me.lblRemoteLogin.Text = "Login:"
        Me.lblRemoteLogin.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblRemoteHost
        '
        Me.lblRemoteHost.Location = New System.Drawing.Point(65, 22)
        Me.lblRemoteHost.Name = "lblRemoteHost"
        Me.lblRemoteHost.Size = New System.Drawing.Size(41, 16)
        Me.lblRemoteHost.TabIndex = 0
        Me.lblRemoteHost.Text = "Host:"
        Me.lblRemoteHost.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chkFTP
        '
        Me.chkFTP.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkFTP.AutoSize = True
        Me.chkFTP.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.chkFTP.Location = New System.Drawing.Point(18, 588)
        Me.chkFTP.Name = "chkFTP"
        Me.chkFTP.Size = New System.Drawing.Size(93, 17)
        Me.chkFTP.TabIndex = 4
        Me.chkFTP.Text = "&FTP Upload"
        Me.chkFTP.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.btnStart.Location = New System.Drawing.Point(422, 642)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(116, 28)
        Me.btnStart.TabIndex = 19
        Me.btnStart.Text = "&Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'treDirectories
        '
        Me.treDirectories.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.treDirectories.Location = New System.Drawing.Point(112, 53)
        Me.treDirectories.Name = "treDirectories"
        Me.treDirectories.SelectedNodes = CType(resources.GetObject("treDirectories.SelectedNodes"), System.Collections.ArrayList)
        Me.treDirectories.Size = New System.Drawing.Size(410, 162)
        Me.treDirectories.TabIndex = 5
        '
        'frmPublish
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(542, 673)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.chkFTP)
        Me.Controls.Add(Me.grpFTP)
        Me.Controls.Add(Me.chkResize)
        Me.Controls.Add(Me.grpResize)
        Me.Controls.Add(Me.grpGenerate)
        Me.Controls.Add(Me.grpSource)
        Me.Controls.Add(Me.barCurrent)
        Me.Controls.Add(Me.barAll)
        Me.KeyPreview = True
        Me.Name = "frmPublish"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Web Publishing Tool"
        Me.grpSource.ResumeLayout(False)
        Me.grpSource.PerformLayout()
        Me.grpGenerate.ResumeLayout(False)
        Me.grpGenerate.PerformLayout()
        Me.grpExport.ResumeLayout(False)
        Me.grpExport.PerformLayout()
        Me.pnlDatabase.ResumeLayout(False)
        Me.pnlDatabase.PerformLayout()
        Me.grpInsert.ResumeLayout(False)
        Me.grpInsert.PerformLayout()
        Me.grpUpdate.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpResize.ResumeLayout(False)
        Me.grpResize.PerformLayout()
        Me.grpFTP.ResumeLayout(False)
        Me.grpFTP.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents grpSource As System.Windows.Forms.GroupBox
    Private WithEvents cboFilter As System.Windows.Forms.ComboBox
    Private WithEvents txtRoot As System.Windows.Forms.TextBox
    Private WithEvents lblRoot As System.Windows.Forms.Label
    Private WithEvents lblFilter As System.Windows.Forms.Label
    Private WithEvents grpGenerate As System.Windows.Forms.GroupBox
    Private WithEvents cboOwner As System.Windows.Forms.ComboBox
    Private WithEvents lblOwner As System.Windows.Forms.Label
    Private WithEvents txtCaptured As System.Windows.Forms.TextBox
    Private WithEvents lblCaptured As System.Windows.Forms.Label
    Private WithEvents txtAlbums As System.Windows.Forms.TextBox
    Private WithEvents lblAlbums As System.Windows.Forms.Label
    Private WithEvents dlgFolder As System.Windows.Forms.FolderBrowserDialog
    Private WithEvents lblDirectories As System.Windows.Forms.Label
    Private WithEvents lstFields As System.Windows.Forms.CheckedListBox
    Private WithEvents lblFields As System.Windows.Forms.Label
    Private WithEvents barAll As System.Windows.Forms.ProgressBar
    Private WithEvents barCurrent As System.Windows.Forms.ProgressBar
    Private WithEvents txtLocationEN As System.Windows.Forms.TextBox
    Private WithEvents txtLocationHU As System.Windows.Forms.TextBox
    Private WithEvents lblLocationEN As System.Windows.Forms.Label
    Private WithEvents lblLocationHU As System.Windows.Forms.Label
    Private WithEvents chkAutoOrder As System.Windows.Forms.CheckBox
    Private WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Private WithEvents treDirectories As FDRTools.TreeViewMS
    Private WithEvents optDest As System.Windows.Forms.RadioButton
    Private WithEvents optSource As System.Windows.Forms.RadioButton
    Private WithEvents grpUpdate As System.Windows.Forms.GroupBox
    Private WithEvents chkUpdate As System.Windows.Forms.CheckBox
    Private WithEvents grpInsert As System.Windows.Forms.GroupBox
    Private WithEvents chkInsert As System.Windows.Forms.CheckBox
    Private WithEvents grpResize As System.Windows.Forms.GroupBox
    Private WithEvents chkResize As System.Windows.Forms.CheckBox
    Private WithEvents chkExport As System.Windows.Forms.CheckBox
    Private WithEvents grpFTP As System.Windows.Forms.GroupBox
    Private WithEvents chkFTP As System.Windows.Forms.CheckBox
    Private WithEvents btnStart As System.Windows.Forms.Button
    Private WithEvents lblKB As System.Windows.Forms.Label
    Private WithEvents pnlDatabase As System.Windows.Forms.Panel
    Private WithEvents chkOverwrite As System.Windows.Forms.CheckBox
    Private WithEvents btnAddAlbum As System.Windows.Forms.Button
    Private WithEvents txtActionSet As System.Windows.Forms.TextBox
    Private WithEvents lblActionSet As System.Windows.Forms.Label
    Private WithEvents txtActionName As System.Windows.Forms.TextBox
    Private WithEvents lblActionName As System.Windows.Forms.Label
    Private WithEvents grpExport As System.Windows.Forms.GroupBox
    Private WithEvents txtRowLimit As System.Windows.Forms.TextBox
    Private WithEvents txtMinSize As System.Windows.Forms.TextBox
    Private WithEvents lblMinSize As System.Windows.Forms.Label
    Private WithEvents lblRowLimit As System.Windows.Forms.Label
    Private WithEvents txtRemoteHost As System.Windows.Forms.TextBox
    Private WithEvents lblRemoteHost As System.Windows.Forms.Label
    Private WithEvents txtRemotePwd As System.Windows.Forms.TextBox
    Private WithEvents lblRemotePwd As System.Windows.Forms.Label
    Private WithEvents txtRemoteLogin As System.Windows.Forms.TextBox
    Private WithEvents lblRemoteLogin As System.Windows.Forms.Label
    Private WithEvents chkRemotePassive As System.Windows.Forms.CheckBox
End Class
