<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSQL
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
        Me.grpSource = New System.Windows.Forms.GroupBox
        Me.chkRecursive = New System.Windows.Forms.CheckBox
        Me.btnSubDir = New System.Windows.Forms.Button
        Me.txtSubDir = New System.Windows.Forms.TextBox
        Me.lblSubDir = New System.Windows.Forms.Label
        Me.btnLoadPictDirs = New System.Windows.Forms.Button
        Me.lstDirectories = New System.Windows.Forms.ListBox
        Me.cboFilter = New System.Windows.Forms.ComboBox
        Me.btnRoot = New System.Windows.Forms.Button
        Me.txtPictDir = New System.Windows.Forms.TextBox
        Me.lblPictDir = New System.Windows.Forms.Label
        Me.btnCheckDuplicates = New System.Windows.Forms.Button
        Me.btnDeselect = New System.Windows.Forms.Button
        Me.btnInverse = New System.Windows.Forms.Button
        Me.btnSelectAll = New System.Windows.Forms.Button
        Me.txtThumbDir = New System.Windows.Forms.TextBox
        Me.lblThumbDir = New System.Windows.Forms.Label
        Me.txtRoot = New System.Windows.Forms.TextBox
        Me.lblDirectories = New System.Windows.Forms.Label
        Me.lblRoot = New System.Windows.Forms.Label
        Me.lblFilter = New System.Windows.Forms.Label
        Me.grpGenerate = New System.Windows.Forms.GroupBox
        Me.chkAutoOrder = New System.Windows.Forms.CheckBox
        Me.barCurrent = New System.Windows.Forms.ProgressBar
        Me.barAll = New System.Windows.Forms.ProgressBar
        Me.lstFields = New System.Windows.Forms.CheckedListBox
        Me.optUpdate = New System.Windows.Forms.RadioButton
        Me.optInsert = New System.Windows.Forms.RadioButton
        Me.btnGenUpdate = New System.Windows.Forms.Button
        Me.btnGenInsert = New System.Windows.Forms.Button
        Me.txtLocationEN = New System.Windows.Forms.TextBox
        Me.txtLocationHU = New System.Windows.Forms.TextBox
        Me.lblLocationEN = New System.Windows.Forms.Label
        Me.txtCaptured = New System.Windows.Forms.TextBox
        Me.lblLocationHU = New System.Windows.Forms.Label
        Me.lblFields = New System.Windows.Forms.Label
        Me.lblCaptured = New System.Windows.Forms.Label
        Me.cboOwner = New System.Windows.Forms.ComboBox
        Me.txtAlbums = New System.Windows.Forms.TextBox
        Me.lblAlbums = New System.Windows.Forms.Label
        Me.lblOwner = New System.Windows.Forms.Label
        Me.dlgFolder = New System.Windows.Forms.FolderBrowserDialog
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.btnWebResize = New System.Windows.Forms.Button
        Me.grpSource.SuspendLayout()
        Me.grpGenerate.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpSource
        '
        Me.grpSource.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSource.Controls.Add(Me.chkRecursive)
        Me.grpSource.Controls.Add(Me.btnSubDir)
        Me.grpSource.Controls.Add(Me.txtSubDir)
        Me.grpSource.Controls.Add(Me.lblSubDir)
        Me.grpSource.Controls.Add(Me.btnLoadPictDirs)
        Me.grpSource.Controls.Add(Me.lstDirectories)
        Me.grpSource.Controls.Add(Me.cboFilter)
        Me.grpSource.Controls.Add(Me.btnRoot)
        Me.grpSource.Controls.Add(Me.txtPictDir)
        Me.grpSource.Controls.Add(Me.lblPictDir)
        Me.grpSource.Controls.Add(Me.btnCheckDuplicates)
        Me.grpSource.Controls.Add(Me.btnDeselect)
        Me.grpSource.Controls.Add(Me.btnInverse)
        Me.grpSource.Controls.Add(Me.btnSelectAll)
        Me.grpSource.Controls.Add(Me.txtThumbDir)
        Me.grpSource.Controls.Add(Me.lblThumbDir)
        Me.grpSource.Controls.Add(Me.txtRoot)
        Me.grpSource.Controls.Add(Me.lblDirectories)
        Me.grpSource.Controls.Add(Me.lblRoot)
        Me.grpSource.Controls.Add(Me.lblFilter)
        Me.grpSource.Location = New System.Drawing.Point(4, 4)
        Me.grpSource.Name = "grpSource"
        Me.grpSource.Size = New System.Drawing.Size(424, 299)
        Me.grpSource.TabIndex = 0
        Me.grpSource.TabStop = False
        Me.grpSource.Text = "Source"
        '
        'chkRecursive
        '
        Me.chkRecursive.AutoSize = True
        Me.chkRecursive.Checked = True
        Me.chkRecursive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRecursive.Location = New System.Drawing.Point(112, 99)
        Me.chkRecursive.Name = "chkRecursive"
        Me.chkRecursive.Size = New System.Drawing.Size(74, 17)
        Me.chkRecursive.TabIndex = 10
        Me.chkRecursive.Text = "Recursive"
        Me.chkRecursive.UseVisualStyleBackColor = True
        '
        'btnSubDir
        '
        Me.btnSubDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSubDir.Location = New System.Drawing.Point(372, 73)
        Me.btnSubDir.Name = "btnSubDir"
        Me.btnSubDir.Size = New System.Drawing.Size(24, 20)
        Me.btnSubDir.TabIndex = 9
        Me.btnSubDir.Text = "..."
        Me.btnSubDir.UseVisualStyleBackColor = True
        '
        'txtSubDir
        '
        Me.txtSubDir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ErrorProvider1.SetError(Me.txtSubDir, "Invalid directory!")
        Me.ErrorProvider1.SetIconPadding(Me.txtSubDir, 26)
        Me.txtSubDir.Location = New System.Drawing.Point(112, 73)
        Me.txtSubDir.Name = "txtSubDir"
        Me.txtSubDir.Size = New System.Drawing.Size(260, 20)
        Me.txtSubDir.TabIndex = 8
        '
        'lblSubDir
        '
        Me.lblSubDir.Location = New System.Drawing.Point(8, 77)
        Me.lblSubDir.Name = "lblSubDir"
        Me.lblSubDir.Size = New System.Drawing.Size(100, 16)
        Me.lblSubDir.TabIndex = 7
        Me.lblSubDir.Text = "&Subdir:"
        Me.lblSubDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnLoadPictDirs
        '
        Me.btnLoadPictDirs.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoadPictDirs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.btnLoadPictDirs.Location = New System.Drawing.Point(296, 96)
        Me.btnLoadPictDirs.Name = "btnLoadPictDirs"
        Me.btnLoadPictDirs.Size = New System.Drawing.Size(100, 20)
        Me.btnLoadPictDirs.TabIndex = 11
        Me.btnLoadPictDirs.Text = "&Load"
        Me.btnLoadPictDirs.UseVisualStyleBackColor = True
        '
        'lstDirectories
        '
        Me.lstDirectories.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstDirectories.FormattingEnabled = True
        Me.lstDirectories.Location = New System.Drawing.Point(112, 122)
        Me.lstDirectories.Name = "lstDirectories"
        Me.lstDirectories.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstDirectories.Size = New System.Drawing.Size(180, 134)
        Me.lstDirectories.Sorted = True
        Me.lstDirectories.TabIndex = 13
        '
        'cboFilter
        '
        Me.cboFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cboFilter.FormattingEnabled = True
        Me.cboFilter.Location = New System.Drawing.Point(112, 271)
        Me.cboFilter.Name = "cboFilter"
        Me.cboFilter.Size = New System.Drawing.Size(180, 21)
        Me.cboFilter.TabIndex = 18
        '
        'btnRoot
        '
        Me.btnRoot.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRoot.Location = New System.Drawing.Point(372, 16)
        Me.btnRoot.Name = "btnRoot"
        Me.btnRoot.Size = New System.Drawing.Size(24, 20)
        Me.btnRoot.TabIndex = 2
        Me.btnRoot.Text = "..."
        Me.btnRoot.UseVisualStyleBackColor = True
        '
        'txtPictDir
        '
        Me.ErrorProvider1.SetError(Me.txtPictDir, "Invalid directory!")
        Me.ErrorProvider1.SetIconPadding(Me.txtPictDir, 2)
        Me.txtPictDir.Location = New System.Drawing.Point(112, 36)
        Me.txtPictDir.Name = "txtPictDir"
        Me.txtPictDir.Size = New System.Drawing.Size(180, 20)
        Me.txtPictDir.TabIndex = 4
        Me.txtPictDir.Text = "pictures"
        '
        'lblPictDir
        '
        Me.lblPictDir.Location = New System.Drawing.Point(8, 40)
        Me.lblPictDir.Name = "lblPictDir"
        Me.lblPictDir.Size = New System.Drawing.Size(100, 16)
        Me.lblPictDir.TabIndex = 3
        Me.lblPictDir.Text = "&Pictures dir:"
        Me.lblPictDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnCheckDuplicates
        '
        Me.btnCheckDuplicates.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCheckDuplicates.Location = New System.Drawing.Point(296, 259)
        Me.btnCheckDuplicates.Name = "btnCheckDuplicates"
        Me.btnCheckDuplicates.Size = New System.Drawing.Size(120, 32)
        Me.btnCheckDuplicates.TabIndex = 19
        Me.btnCheckDuplicates.Text = "Check Duplicates"
        Me.btnCheckDuplicates.UseVisualStyleBackColor = True
        '
        'btnDeselect
        '
        Me.btnDeselect.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeselect.Location = New System.Drawing.Point(296, 162)
        Me.btnDeselect.Name = "btnDeselect"
        Me.btnDeselect.Size = New System.Drawing.Size(100, 20)
        Me.btnDeselect.TabIndex = 16
        Me.btnDeselect.Text = "Deselect All"
        Me.btnDeselect.UseVisualStyleBackColor = True
        '
        'btnInverse
        '
        Me.btnInverse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInverse.Location = New System.Drawing.Point(296, 142)
        Me.btnInverse.Name = "btnInverse"
        Me.btnInverse.Size = New System.Drawing.Size(100, 20)
        Me.btnInverse.TabIndex = 15
        Me.btnInverse.Text = "Inverse"
        Me.btnInverse.UseVisualStyleBackColor = True
        '
        'btnSelectAll
        '
        Me.btnSelectAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSelectAll.Location = New System.Drawing.Point(296, 122)
        Me.btnSelectAll.Name = "btnSelectAll"
        Me.btnSelectAll.Size = New System.Drawing.Size(100, 20)
        Me.btnSelectAll.TabIndex = 14
        Me.btnSelectAll.Text = "Select All"
        Me.btnSelectAll.UseVisualStyleBackColor = True
        '
        'txtThumbDir
        '
        Me.ErrorProvider1.SetError(Me.txtThumbDir, "Invalid directory!")
        Me.ErrorProvider1.SetIconPadding(Me.txtThumbDir, 2)
        Me.txtThumbDir.Location = New System.Drawing.Point(112, 56)
        Me.txtThumbDir.Name = "txtThumbDir"
        Me.txtThumbDir.Size = New System.Drawing.Size(180, 20)
        Me.txtThumbDir.TabIndex = 6
        Me.txtThumbDir.Text = "thumbnails"
        '
        'lblThumbDir
        '
        Me.lblThumbDir.Location = New System.Drawing.Point(8, 60)
        Me.lblThumbDir.Name = "lblThumbDir"
        Me.lblThumbDir.Size = New System.Drawing.Size(100, 16)
        Me.lblThumbDir.TabIndex = 5
        Me.lblThumbDir.Text = "&Thumbnails dir:"
        Me.lblThumbDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtRoot
        '
        Me.txtRoot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ErrorProvider1.SetError(Me.txtRoot, "Invalid directory!")
        Me.ErrorProvider1.SetIconPadding(Me.txtRoot, 26)
        Me.txtRoot.Location = New System.Drawing.Point(112, 16)
        Me.txtRoot.Name = "txtRoot"
        Me.txtRoot.Size = New System.Drawing.Size(260, 20)
        Me.txtRoot.TabIndex = 1
        '
        'lblDirectories
        '
        Me.lblDirectories.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblDirectories.Location = New System.Drawing.Point(8, 122)
        Me.lblDirectories.Name = "lblDirectories"
        Me.lblDirectories.Size = New System.Drawing.Size(100, 16)
        Me.lblDirectories.TabIndex = 12
        Me.lblDirectories.Text = "&Directories:"
        Me.lblDirectories.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblRoot
        '
        Me.lblRoot.Location = New System.Drawing.Point(8, 20)
        Me.lblRoot.Name = "lblRoot"
        Me.lblRoot.Size = New System.Drawing.Size(100, 16)
        Me.lblRoot.TabIndex = 0
        Me.lblRoot.Text = "&Root:"
        Me.lblRoot.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFilter
        '
        Me.lblFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblFilter.Location = New System.Drawing.Point(8, 275)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(100, 16)
        Me.lblFilter.TabIndex = 17
        Me.lblFilter.Text = "&Filter:"
        Me.lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpGenerate
        '
        Me.grpGenerate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpGenerate.Controls.Add(Me.btnWebResize)
        Me.grpGenerate.Controls.Add(Me.chkAutoOrder)
        Me.grpGenerate.Controls.Add(Me.barCurrent)
        Me.grpGenerate.Controls.Add(Me.barAll)
        Me.grpGenerate.Controls.Add(Me.lstFields)
        Me.grpGenerate.Controls.Add(Me.optUpdate)
        Me.grpGenerate.Controls.Add(Me.optInsert)
        Me.grpGenerate.Controls.Add(Me.btnGenUpdate)
        Me.grpGenerate.Controls.Add(Me.btnGenInsert)
        Me.grpGenerate.Controls.Add(Me.txtLocationEN)
        Me.grpGenerate.Controls.Add(Me.txtLocationHU)
        Me.grpGenerate.Controls.Add(Me.lblLocationEN)
        Me.grpGenerate.Controls.Add(Me.txtCaptured)
        Me.grpGenerate.Controls.Add(Me.lblLocationHU)
        Me.grpGenerate.Controls.Add(Me.lblFields)
        Me.grpGenerate.Controls.Add(Me.lblCaptured)
        Me.grpGenerate.Controls.Add(Me.cboOwner)
        Me.grpGenerate.Controls.Add(Me.txtAlbums)
        Me.grpGenerate.Controls.Add(Me.lblAlbums)
        Me.grpGenerate.Controls.Add(Me.lblOwner)
        Me.grpGenerate.Location = New System.Drawing.Point(4, 309)
        Me.grpGenerate.Name = "grpGenerate"
        Me.grpGenerate.Size = New System.Drawing.Size(424, 336)
        Me.grpGenerate.TabIndex = 1
        Me.grpGenerate.TabStop = False
        Me.grpGenerate.Text = "Generate"
        '
        'chkAutoOrder
        '
        Me.chkAutoOrder.AutoSize = True
        Me.chkAutoOrder.Location = New System.Drawing.Point(112, 122)
        Me.chkAutoOrder.Name = "chkAutoOrder"
        Me.chkAutoOrder.Size = New System.Drawing.Size(124, 17)
        Me.chkAutoOrder.TabIndex = 10
        Me.chkAutoOrder.Text = "A&uto increment order"
        Me.chkAutoOrder.UseVisualStyleBackColor = True
        '
        'barCurrent
        '
        Me.barCurrent.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.barCurrent.Location = New System.Drawing.Point(6, 307)
        Me.barCurrent.Name = "barCurrent"
        Me.barCurrent.Size = New System.Drawing.Size(408, 20)
        Me.barCurrent.Step = 1
        Me.barCurrent.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.barCurrent.TabIndex = 19
        '
        'barAll
        '
        Me.barAll.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.barAll.Location = New System.Drawing.Point(6, 287)
        Me.barAll.Name = "barAll"
        Me.barAll.Size = New System.Drawing.Size(408, 20)
        Me.barAll.Step = 1
        Me.barAll.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.barAll.TabIndex = 18
        '
        'lstFields
        '
        Me.lstFields.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstFields.CheckOnClick = True
        Me.lstFields.Location = New System.Drawing.Point(112, 171)
        Me.lstFields.Name = "lstFields"
        Me.lstFields.Size = New System.Drawing.Size(180, 109)
        Me.lstFields.TabIndex = 14
        '
        'optUpdate
        '
        Me.optUpdate.Location = New System.Drawing.Point(212, 16)
        Me.optUpdate.Name = "optUpdate"
        Me.optUpdate.Size = New System.Drawing.Size(76, 20)
        Me.optUpdate.TabIndex = 1
        Me.optUpdate.TabStop = True
        Me.optUpdate.Text = "&UPDATE"
        Me.optUpdate.UseVisualStyleBackColor = True
        '
        'optInsert
        '
        Me.optInsert.Checked = True
        Me.optInsert.Location = New System.Drawing.Point(112, 16)
        Me.optInsert.Name = "optInsert"
        Me.optInsert.Size = New System.Drawing.Size(76, 20)
        Me.optInsert.TabIndex = 0
        Me.optInsert.TabStop = True
        Me.optInsert.Text = "&INSERT"
        Me.optInsert.UseVisualStyleBackColor = True
        '
        'btnGenUpdate
        '
        Me.btnGenUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGenUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.btnGenUpdate.Location = New System.Drawing.Point(296, 207)
        Me.btnGenUpdate.Name = "btnGenUpdate"
        Me.btnGenUpdate.Size = New System.Drawing.Size(120, 32)
        Me.btnGenUpdate.TabIndex = 16
        Me.btnGenUpdate.Text = "Gen. UPDATE"
        Me.btnGenUpdate.UseVisualStyleBackColor = True
        '
        'btnGenInsert
        '
        Me.btnGenInsert.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGenInsert.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.btnGenInsert.Location = New System.Drawing.Point(296, 171)
        Me.btnGenInsert.Name = "btnGenInsert"
        Me.btnGenInsert.Size = New System.Drawing.Size(120, 32)
        Me.btnGenInsert.TabIndex = 15
        Me.btnGenInsert.Text = "Gen. INSERT"
        Me.btnGenInsert.UseVisualStyleBackColor = True
        '
        'txtLocationEN
        '
        Me.txtLocationEN.Location = New System.Drawing.Point(112, 100)
        Me.txtLocationEN.Name = "txtLocationEN"
        Me.txtLocationEN.Size = New System.Drawing.Size(180, 20)
        Me.txtLocationEN.TabIndex = 9
        '
        'txtLocationHU
        '
        Me.txtLocationHU.Location = New System.Drawing.Point(112, 80)
        Me.txtLocationHU.Name = "txtLocationHU"
        Me.txtLocationHU.Size = New System.Drawing.Size(180, 20)
        Me.txtLocationHU.TabIndex = 7
        '
        'lblLocationEN
        '
        Me.lblLocationEN.Location = New System.Drawing.Point(8, 104)
        Me.lblLocationEN.Name = "lblLocationEN"
        Me.lblLocationEN.Size = New System.Drawing.Size(100, 16)
        Me.lblLocationEN.TabIndex = 8
        Me.lblLocationEN.Text = "Location EN:"
        Me.lblLocationEN.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCaptured
        '
        Me.ErrorProvider1.SetIconPadding(Me.txtCaptured, 2)
        Me.txtCaptured.Location = New System.Drawing.Point(112, 60)
        Me.txtCaptured.Name = "txtCaptured"
        Me.txtCaptured.Size = New System.Drawing.Size(180, 20)
        Me.txtCaptured.TabIndex = 5
        '
        'lblLocationHU
        '
        Me.lblLocationHU.Location = New System.Drawing.Point(8, 84)
        Me.lblLocationHU.Name = "lblLocationHU"
        Me.lblLocationHU.Size = New System.Drawing.Size(100, 16)
        Me.lblLocationHU.TabIndex = 6
        Me.lblLocationHU.Text = "Location HU:"
        Me.lblLocationHU.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFields
        '
        Me.lblFields.Location = New System.Drawing.Point(8, 171)
        Me.lblFields.Name = "lblFields"
        Me.lblFields.Size = New System.Drawing.Size(100, 16)
        Me.lblFields.TabIndex = 13
        Me.lblFields.Text = "&Fields to update:"
        Me.lblFields.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCaptured
        '
        Me.lblCaptured.Location = New System.Drawing.Point(8, 64)
        Me.lblCaptured.Name = "lblCaptured"
        Me.lblCaptured.Size = New System.Drawing.Size(100, 16)
        Me.lblCaptured.TabIndex = 4
        Me.lblCaptured.Text = "&Captured [auto]:"
        Me.lblCaptured.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboOwner
        '
        Me.cboOwner.FormattingEnabled = True
        Me.cboOwner.Location = New System.Drawing.Point(112, 36)
        Me.cboOwner.Name = "cboOwner"
        Me.cboOwner.Size = New System.Drawing.Size(180, 21)
        Me.cboOwner.TabIndex = 3
        '
        'txtAlbums
        '
        Me.txtAlbums.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAlbums.Location = New System.Drawing.Point(112, 145)
        Me.txtAlbums.Name = "txtAlbums"
        Me.txtAlbums.Size = New System.Drawing.Size(304, 20)
        Me.txtAlbums.TabIndex = 12
        '
        'lblAlbums
        '
        Me.lblAlbums.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblAlbums.Location = New System.Drawing.Point(8, 149)
        Me.lblAlbums.Name = "lblAlbums"
        Me.lblAlbums.Size = New System.Drawing.Size(100, 16)
        Me.lblAlbums.TabIndex = 11
        Me.lblAlbums.Text = "&Albums:"
        Me.lblAlbums.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblOwner
        '
        Me.lblOwner.Location = New System.Drawing.Point(8, 40)
        Me.lblOwner.Name = "lblOwner"
        Me.lblOwner.Size = New System.Drawing.Size(100, 16)
        Me.lblOwner.TabIndex = 2
        Me.lblOwner.Text = "&Owner:"
        Me.lblOwner.TextAlign = System.Drawing.ContentAlignment.MiddleRight
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
        'btnWebResize
        '
        Me.btnWebResize.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWebResize.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.btnWebResize.Location = New System.Drawing.Point(296, 245)
        Me.btnWebResize.Name = "btnWebResize"
        Me.btnWebResize.Size = New System.Drawing.Size(120, 32)
        Me.btnWebResize.TabIndex = 17
        Me.btnWebResize.Text = "&Web Resize"
        Me.btnWebResize.Visible = False
        '
        'frmSQL
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(432, 648)
        Me.Controls.Add(Me.grpGenerate)
        Me.Controls.Add(Me.grpSource)
        Me.KeyPreview = True
        Me.Name = "frmSQL"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "SQL Generating Tool"
        Me.grpSource.ResumeLayout(False)
        Me.grpSource.PerformLayout()
        Me.grpGenerate.ResumeLayout(False)
        Me.grpGenerate.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents grpSource As System.Windows.Forms.GroupBox
    Private WithEvents cboFilter As System.Windows.Forms.ComboBox
    Private WithEvents btnRoot As System.Windows.Forms.Button
    Private WithEvents txtRoot As System.Windows.Forms.TextBox
    Private WithEvents lblRoot As System.Windows.Forms.Label
    Private WithEvents lblFilter As System.Windows.Forms.Label
    Private WithEvents grpGenerate As System.Windows.Forms.GroupBox
    Private WithEvents txtThumbDir As System.Windows.Forms.TextBox
    Private WithEvents lblThumbDir As System.Windows.Forms.Label
    Private WithEvents cboOwner As System.Windows.Forms.ComboBox
    Private WithEvents lblOwner As System.Windows.Forms.Label
    Private WithEvents txtCaptured As System.Windows.Forms.TextBox
    Private WithEvents lblCaptured As System.Windows.Forms.Label
    Private WithEvents txtAlbums As System.Windows.Forms.TextBox
    Private WithEvents lblAlbums As System.Windows.Forms.Label
    Private WithEvents btnGenInsert As System.Windows.Forms.Button
    Private WithEvents dlgFolder As System.Windows.Forms.FolderBrowserDialog
    Private WithEvents btnGenUpdate As System.Windows.Forms.Button
    Private WithEvents txtPictDir As System.Windows.Forms.TextBox
    Private WithEvents lblPictDir As System.Windows.Forms.Label
    Private WithEvents btnLoadPictDirs As System.Windows.Forms.Button
    Private WithEvents lstDirectories As System.Windows.Forms.ListBox
    Private WithEvents lblDirectories As System.Windows.Forms.Label
    Private WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Private WithEvents optUpdate As System.Windows.Forms.RadioButton
    Private WithEvents optInsert As System.Windows.Forms.RadioButton
    Private WithEvents lstFields As System.Windows.Forms.CheckedListBox
    Private WithEvents lblFields As System.Windows.Forms.Label
    Private WithEvents barAll As System.Windows.Forms.ProgressBar
    Private WithEvents barCurrent As System.Windows.Forms.ProgressBar
    Private WithEvents btnSelectAll As System.Windows.Forms.Button
    Private WithEvents btnDeselect As System.Windows.Forms.Button
    Private WithEvents btnInverse As System.Windows.Forms.Button
    Private WithEvents btnCheckDuplicates As System.Windows.Forms.Button
    Private WithEvents txtLocationEN As System.Windows.Forms.TextBox
    Private WithEvents txtLocationHU As System.Windows.Forms.TextBox
    Private WithEvents lblLocationEN As System.Windows.Forms.Label
    Private WithEvents lblLocationHU As System.Windows.Forms.Label
    Private WithEvents chkAutoOrder As System.Windows.Forms.CheckBox
    Private WithEvents btnSubDir As System.Windows.Forms.Button
    Private WithEvents txtSubDir As System.Windows.Forms.TextBox
    Private WithEvents lblSubDir As System.Windows.Forms.Label
    Private WithEvents chkRecursive As System.Windows.Forms.CheckBox
    Private WithEvents btnWebResize As System.Windows.Forms.Button
End Class
