<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCompare
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
        Me.grpPath1 = New System.Windows.Forms.GroupBox
        Me.lblCount1 = New System.Windows.Forms.Label
        Me.cboFilter1 = New System.Windows.Forms.ComboBox
        Me.btnLastDir = New System.Windows.Forms.Button
        Me.btnNextDir = New System.Windows.Forms.Button
        Me.btnPrevDir = New System.Windows.Forms.Button
        Me.btnFirstDir = New System.Windows.Forms.Button
        Me.btnPath1 = New System.Windows.Forms.Button
        Me.txtPath1 = New System.Windows.Forms.TextBox
        Me.lblFilter1 = New System.Windows.Forms.Label
        Me.lblPath1 = New System.Windows.Forms.Label
        Me.lstFiles1 = New System.Windows.Forms.ListBox
        Me.grpPath2 = New System.Windows.Forms.GroupBox
        Me.chkSubfolderOfFirst = New System.Windows.Forms.CheckBox
        Me.lblCount2 = New System.Windows.Forms.Label
        Me.cboFilter2 = New System.Windows.Forms.ComboBox
        Me.btnSubFolder2 = New System.Windows.Forms.Button
        Me.btnPath2 = New System.Windows.Forms.Button
        Me.txtPath2 = New System.Windows.Forms.TextBox
        Me.lblFilter2 = New System.Windows.Forms.Label
        Me.lblPath2 = New System.Windows.Forms.Label
        Me.txtSubFolder2 = New System.Windows.Forms.TextBox
        Me.lblSubFolder2 = New System.Windows.Forms.Label
        Me.lstFiles2 = New System.Windows.Forms.ListBox
        Me.grpSelect1 = New System.Windows.Forms.GroupBox
        Me.lblSelected1 = New System.Windows.Forms.Label
        Me.btnInverse1 = New System.Windows.Forms.Button
        Me.btnDeselect1 = New System.Windows.Forms.Button
        Me.btnNotExistsInSecond = New System.Windows.Forms.Button
        Me.btnExistsInSecond = New System.Windows.Forms.Button
        Me.lblSample1 = New System.Windows.Forms.Label
        Me.grpSubString1 = New System.Windows.Forms.GroupBox
        Me.txtLength1 = New System.Windows.Forms.TextBox
        Me.lblLength1 = New System.Windows.Forms.Label
        Me.txtFirst1 = New System.Windows.Forms.TextBox
        Me.lblFirst1 = New System.Windows.Forms.Label
        Me.optPiece1 = New System.Windows.Forms.RadioButton
        Me.txtPostfix1 = New System.Windows.Forms.TextBox
        Me.lblPostfix1 = New System.Windows.Forms.Label
        Me.txtPrefix1 = New System.Windows.Forms.TextBox
        Me.lblSampleCaption1 = New System.Windows.Forms.Label
        Me.lblPrefix1 = New System.Windows.Forms.Label
        Me.optWhole1 = New System.Windows.Forms.RadioButton
        Me.grpSelect2 = New System.Windows.Forms.GroupBox
        Me.lblSelected2 = New System.Windows.Forms.Label
        Me.btnInverse2 = New System.Windows.Forms.Button
        Me.btnDeselect2 = New System.Windows.Forms.Button
        Me.btnNotExistsInFirst = New System.Windows.Forms.Button
        Me.btnExistsInFirst = New System.Windows.Forms.Button
        Me.lblSample2 = New System.Windows.Forms.Label
        Me.grpSubString2 = New System.Windows.Forms.GroupBox
        Me.txtLength2 = New System.Windows.Forms.TextBox
        Me.lblLength2 = New System.Windows.Forms.Label
        Me.txtFirst2 = New System.Windows.Forms.TextBox
        Me.lblFirst2 = New System.Windows.Forms.Label
        Me.optPiece2 = New System.Windows.Forms.RadioButton
        Me.txtPostfix2 = New System.Windows.Forms.TextBox
        Me.lblPostfix2 = New System.Windows.Forms.Label
        Me.txtPrefix2 = New System.Windows.Forms.TextBox
        Me.lblSampleCaption2 = New System.Windows.Forms.Label
        Me.lblPrefix2 = New System.Windows.Forms.Label
        Me.optWhole2 = New System.Windows.Forms.RadioButton
        Me.btnDelete1 = New System.Windows.Forms.Button
        Me.btnDelete2 = New System.Windows.Forms.Button
        Me.dlgFolder = New System.Windows.Forms.FolderBrowserDialog
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.optAuto1 = New System.Windows.Forms.RadioButton
        Me.optAuto2 = New System.Windows.Forms.RadioButton
        Me.grpPath1.SuspendLayout()
        Me.grpPath2.SuspendLayout()
        Me.grpSelect1.SuspendLayout()
        Me.grpSubString1.SuspendLayout()
        Me.grpSelect2.SuspendLayout()
        Me.grpSubString2.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpPath1
        '
        Me.grpPath1.Controls.Add(Me.lblCount1)
        Me.grpPath1.Controls.Add(Me.cboFilter1)
        Me.grpPath1.Controls.Add(Me.btnLastDir)
        Me.grpPath1.Controls.Add(Me.btnNextDir)
        Me.grpPath1.Controls.Add(Me.btnPrevDir)
        Me.grpPath1.Controls.Add(Me.btnFirstDir)
        Me.grpPath1.Controls.Add(Me.btnPath1)
        Me.grpPath1.Controls.Add(Me.txtPath1)
        Me.grpPath1.Controls.Add(Me.lblFilter1)
        Me.grpPath1.Controls.Add(Me.lblPath1)
        Me.grpPath1.Location = New System.Drawing.Point(4, 4)
        Me.grpPath1.Name = "grpPath1"
        Me.grpPath1.Size = New System.Drawing.Size(252, 136)
        Me.grpPath1.TabIndex = 0
        Me.grpPath1.TabStop = False
        Me.grpPath1.Text = "First directory"
        '
        'lblCount1
        '
        Me.lblCount1.Location = New System.Drawing.Point(156, 108)
        Me.lblCount1.Name = "lblCount1"
        Me.lblCount1.Size = New System.Drawing.Size(92, 20)
        Me.lblCount1.TabIndex = 9
        Me.lblCount1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cboFilter1
        '
        Me.cboFilter1.FormattingEnabled = True
        Me.cboFilter1.Location = New System.Drawing.Point(64, 108)
        Me.cboFilter1.Name = "cboFilter1"
        Me.cboFilter1.Size = New System.Drawing.Size(88, 21)
        Me.cboFilter1.TabIndex = 8
        '
        'btnLastDir
        '
        Me.btnLastDir.Location = New System.Drawing.Point(163, 37)
        Me.btnLastDir.Name = "btnLastDir"
        Me.btnLastDir.Size = New System.Drawing.Size(33, 23)
        Me.btnLastDir.TabIndex = 3
        Me.btnLastDir.Text = ">>"
        Me.ToolTip1.SetToolTip(Me.btnLastDir, "Last directory")
        Me.btnLastDir.UseVisualStyleBackColor = True
        '
        'btnNextDir
        '
        Me.btnNextDir.Location = New System.Drawing.Point(130, 37)
        Me.btnNextDir.Name = "btnNextDir"
        Me.btnNextDir.Size = New System.Drawing.Size(33, 23)
        Me.btnNextDir.TabIndex = 2
        Me.btnNextDir.Text = ">"
        Me.ToolTip1.SetToolTip(Me.btnNextDir, "Next directory")
        Me.btnNextDir.UseVisualStyleBackColor = True
        '
        'btnPrevDir
        '
        Me.btnPrevDir.Location = New System.Drawing.Point(97, 37)
        Me.btnPrevDir.Name = "btnPrevDir"
        Me.btnPrevDir.Size = New System.Drawing.Size(33, 23)
        Me.btnPrevDir.TabIndex = 1
        Me.btnPrevDir.Text = "<"
        Me.ToolTip1.SetToolTip(Me.btnPrevDir, "Previous directory")
        Me.btnPrevDir.UseVisualStyleBackColor = True
        '
        'btnFirstDir
        '
        Me.btnFirstDir.Location = New System.Drawing.Point(64, 37)
        Me.btnFirstDir.Name = "btnFirstDir"
        Me.btnFirstDir.Size = New System.Drawing.Size(33, 23)
        Me.btnFirstDir.TabIndex = 0
        Me.btnFirstDir.Text = "<<"
        Me.ToolTip1.SetToolTip(Me.btnFirstDir, "First directory")
        Me.btnFirstDir.UseVisualStyleBackColor = True
        '
        'btnPath1
        '
        Me.btnPath1.Location = New System.Drawing.Point(220, 84)
        Me.btnPath1.Name = "btnPath1"
        Me.btnPath1.Size = New System.Drawing.Size(24, 20)
        Me.btnPath1.TabIndex = 6
        Me.btnPath1.Text = "..."
        Me.btnPath1.UseVisualStyleBackColor = True
        '
        'txtPath1
        '
        Me.txtPath1.Location = New System.Drawing.Point(64, 84)
        Me.txtPath1.Name = "txtPath1"
        Me.txtPath1.ReadOnly = True
        Me.txtPath1.Size = New System.Drawing.Size(152, 20)
        Me.txtPath1.TabIndex = 5
        Me.txtPath1.TabStop = False
        '
        'lblFilter1
        '
        Me.lblFilter1.Location = New System.Drawing.Point(8, 112)
        Me.lblFilter1.Name = "lblFilter1"
        Me.lblFilter1.Size = New System.Drawing.Size(52, 16)
        Me.lblFilter1.TabIndex = 7
        Me.lblFilter1.Text = "&Filter:"
        Me.lblFilter1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPath1
        '
        Me.lblPath1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblPath1.Location = New System.Drawing.Point(8, 88)
        Me.lblPath1.Name = "lblPath1"
        Me.lblPath1.Size = New System.Drawing.Size(52, 16)
        Me.lblPath1.TabIndex = 4
        Me.lblPath1.Text = "&Path:"
        Me.lblPath1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lstFiles1
        '
        Me.lstFiles1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstFiles1.Location = New System.Drawing.Point(4, 144)
        Me.lstFiles1.Name = "lstFiles1"
        Me.lstFiles1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstFiles1.Size = New System.Drawing.Size(252, 199)
        Me.lstFiles1.Sorted = True
        Me.lstFiles1.TabIndex = 2
        Me.lstFiles1.TabStop = False
        '
        'grpPath2
        '
        Me.grpPath2.Controls.Add(Me.chkSubfolderOfFirst)
        Me.grpPath2.Controls.Add(Me.lblCount2)
        Me.grpPath2.Controls.Add(Me.cboFilter2)
        Me.grpPath2.Controls.Add(Me.btnSubFolder2)
        Me.grpPath2.Controls.Add(Me.btnPath2)
        Me.grpPath2.Controls.Add(Me.txtPath2)
        Me.grpPath2.Controls.Add(Me.lblFilter2)
        Me.grpPath2.Controls.Add(Me.lblPath2)
        Me.grpPath2.Controls.Add(Me.txtSubFolder2)
        Me.grpPath2.Controls.Add(Me.lblSubFolder2)
        Me.grpPath2.Location = New System.Drawing.Point(260, 4)
        Me.grpPath2.Name = "grpPath2"
        Me.grpPath2.Size = New System.Drawing.Size(252, 136)
        Me.grpPath2.TabIndex = 1
        Me.grpPath2.TabStop = False
        Me.grpPath2.Text = "Second directory"
        '
        'chkSubfolderOfFirst
        '
        Me.chkSubfolderOfFirst.AutoSize = True
        Me.chkSubfolderOfFirst.Checked = True
        Me.chkSubfolderOfFirst.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSubfolderOfFirst.Location = New System.Drawing.Point(64, 37)
        Me.chkSubfolderOfFirst.Name = "chkSubfolderOfFirst"
        Me.chkSubfolderOfFirst.Size = New System.Drawing.Size(105, 17)
        Me.chkSubfolderOfFirst.TabIndex = 0
        Me.chkSubfolderOfFirst.Text = "Subfolder of First"
        Me.chkSubfolderOfFirst.UseVisualStyleBackColor = True
        '
        'lblCount2
        '
        Me.lblCount2.Location = New System.Drawing.Point(156, 108)
        Me.lblCount2.Name = "lblCount2"
        Me.lblCount2.Size = New System.Drawing.Size(92, 20)
        Me.lblCount2.TabIndex = 9
        Me.lblCount2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cboFilter2
        '
        Me.cboFilter2.FormattingEnabled = True
        Me.cboFilter2.Location = New System.Drawing.Point(64, 108)
        Me.cboFilter2.Name = "cboFilter2"
        Me.cboFilter2.Size = New System.Drawing.Size(88, 21)
        Me.cboFilter2.TabIndex = 8
        '
        'btnSubFolder2
        '
        Me.btnSubFolder2.Location = New System.Drawing.Point(220, 60)
        Me.btnSubFolder2.Name = "btnSubFolder2"
        Me.btnSubFolder2.Size = New System.Drawing.Size(24, 20)
        Me.btnSubFolder2.TabIndex = 3
        Me.btnSubFolder2.Text = "->"
        Me.btnSubFolder2.UseVisualStyleBackColor = True
        '
        'btnPath2
        '
        Me.btnPath2.Location = New System.Drawing.Point(220, 84)
        Me.btnPath2.Name = "btnPath2"
        Me.btnPath2.Size = New System.Drawing.Size(24, 20)
        Me.btnPath2.TabIndex = 6
        Me.btnPath2.Text = "..."
        Me.btnPath2.UseVisualStyleBackColor = True
        '
        'txtPath2
        '
        Me.txtPath2.Location = New System.Drawing.Point(64, 84)
        Me.txtPath2.Name = "txtPath2"
        Me.txtPath2.ReadOnly = True
        Me.txtPath2.Size = New System.Drawing.Size(152, 20)
        Me.txtPath2.TabIndex = 5
        Me.txtPath2.TabStop = False
        '
        'lblFilter2
        '
        Me.lblFilter2.Location = New System.Drawing.Point(8, 112)
        Me.lblFilter2.Name = "lblFilter2"
        Me.lblFilter2.Size = New System.Drawing.Size(52, 16)
        Me.lblFilter2.TabIndex = 7
        Me.lblFilter2.Text = "&Filter:"
        Me.lblFilter2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPath2
        '
        Me.lblPath2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblPath2.Location = New System.Drawing.Point(8, 88)
        Me.lblPath2.Name = "lblPath2"
        Me.lblPath2.Size = New System.Drawing.Size(52, 16)
        Me.lblPath2.TabIndex = 4
        Me.lblPath2.Text = "&Path:"
        Me.lblPath2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSubFolder2
        '
        Me.txtSubFolder2.Location = New System.Drawing.Point(64, 60)
        Me.txtSubFolder2.Name = "txtSubFolder2"
        Me.txtSubFolder2.Size = New System.Drawing.Size(152, 20)
        Me.txtSubFolder2.TabIndex = 2
        Me.txtSubFolder2.Text = "RAW"
        '
        'lblSubFolder2
        '
        Me.lblSubFolder2.Location = New System.Drawing.Point(8, 60)
        Me.lblSubFolder2.Name = "lblSubFolder2"
        Me.lblSubFolder2.Size = New System.Drawing.Size(56, 20)
        Me.lblSubFolder2.TabIndex = 1
        Me.lblSubFolder2.Text = "Subfolder:"
        Me.lblSubFolder2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lstFiles2
        '
        Me.lstFiles2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstFiles2.Location = New System.Drawing.Point(260, 144)
        Me.lstFiles2.Name = "lstFiles2"
        Me.lstFiles2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstFiles2.Size = New System.Drawing.Size(252, 199)
        Me.lstFiles2.Sorted = True
        Me.lstFiles2.TabIndex = 3
        Me.lstFiles2.TabStop = False
        '
        'grpSelect1
        '
        Me.grpSelect1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpSelect1.Controls.Add(Me.optAuto1)
        Me.grpSelect1.Controls.Add(Me.lblSelected1)
        Me.grpSelect1.Controls.Add(Me.btnInverse1)
        Me.grpSelect1.Controls.Add(Me.btnDeselect1)
        Me.grpSelect1.Controls.Add(Me.btnNotExistsInSecond)
        Me.grpSelect1.Controls.Add(Me.btnExistsInSecond)
        Me.grpSelect1.Controls.Add(Me.lblSample1)
        Me.grpSelect1.Controls.Add(Me.grpSubString1)
        Me.grpSelect1.Controls.Add(Me.optPiece1)
        Me.grpSelect1.Controls.Add(Me.txtPostfix1)
        Me.grpSelect1.Controls.Add(Me.lblPostfix1)
        Me.grpSelect1.Controls.Add(Me.txtPrefix1)
        Me.grpSelect1.Controls.Add(Me.lblSampleCaption1)
        Me.grpSelect1.Controls.Add(Me.lblPrefix1)
        Me.grpSelect1.Controls.Add(Me.optWhole1)
        Me.grpSelect1.Location = New System.Drawing.Point(4, 350)
        Me.grpSelect1.Name = "grpSelect1"
        Me.grpSelect1.Size = New System.Drawing.Size(252, 232)
        Me.grpSelect1.TabIndex = 4
        Me.grpSelect1.TabStop = False
        Me.grpSelect1.Text = "Select"
        '
        'lblSelected1
        '
        Me.lblSelected1.Location = New System.Drawing.Point(8, 204)
        Me.lblSelected1.Name = "lblSelected1"
        Me.lblSelected1.Size = New System.Drawing.Size(236, 20)
        Me.lblSelected1.TabIndex = 14
        Me.lblSelected1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnInverse1
        '
        Me.btnInverse1.Location = New System.Drawing.Point(128, 168)
        Me.btnInverse1.Name = "btnInverse1"
        Me.btnInverse1.Size = New System.Drawing.Size(116, 32)
        Me.btnInverse1.TabIndex = 13
        Me.btnInverse1.Text = "Inverse Selection"
        Me.btnInverse1.UseVisualStyleBackColor = True
        '
        'btnDeselect1
        '
        Me.btnDeselect1.Location = New System.Drawing.Point(8, 168)
        Me.btnDeselect1.Name = "btnDeselect1"
        Me.btnDeselect1.Size = New System.Drawing.Size(116, 32)
        Me.btnDeselect1.TabIndex = 12
        Me.btnDeselect1.Text = "Deselect All"
        Me.btnDeselect1.UseVisualStyleBackColor = True
        '
        'btnNotExistsInSecond
        '
        Me.btnNotExistsInSecond.Location = New System.Drawing.Point(128, 132)
        Me.btnNotExistsInSecond.Name = "btnNotExistsInSecond"
        Me.btnNotExistsInSecond.Size = New System.Drawing.Size(116, 32)
        Me.btnNotExistsInSecond.TabIndex = 11
        Me.btnNotExistsInSecond.Text = "Not Exists in Second"
        Me.btnNotExistsInSecond.UseVisualStyleBackColor = True
        '
        'btnExistsInSecond
        '
        Me.btnExistsInSecond.Location = New System.Drawing.Point(8, 132)
        Me.btnExistsInSecond.Name = "btnExistsInSecond"
        Me.btnExistsInSecond.Size = New System.Drawing.Size(116, 32)
        Me.btnExistsInSecond.TabIndex = 10
        Me.btnExistsInSecond.Text = "Exists in Second"
        Me.btnExistsInSecond.UseVisualStyleBackColor = True
        '
        'lblSample1
        '
        Me.lblSample1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSample1.Location = New System.Drawing.Point(64, 108)
        Me.lblSample1.Name = "lblSample1"
        Me.lblSample1.Size = New System.Drawing.Size(180, 20)
        Me.lblSample1.TabIndex = 9
        Me.lblSample1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpSubString1
        '
        Me.grpSubString1.Controls.Add(Me.txtLength1)
        Me.grpSubString1.Controls.Add(Me.lblLength1)
        Me.grpSubString1.Controls.Add(Me.txtFirst1)
        Me.grpSubString1.Controls.Add(Me.lblFirst1)
        Me.grpSubString1.Location = New System.Drawing.Point(140, 16)
        Me.grpSubString1.Name = "grpSubString1"
        Me.grpSubString1.Size = New System.Drawing.Size(104, 64)
        Me.grpSubString1.TabIndex = 3
        Me.grpSubString1.TabStop = False
        Me.grpSubString1.Text = "SubString"
        '
        'txtLength1
        '
        Me.txtLength1.Location = New System.Drawing.Point(56, 36)
        Me.txtLength1.Name = "txtLength1"
        Me.txtLength1.Size = New System.Drawing.Size(40, 20)
        Me.txtLength1.TabIndex = 3
        Me.txtLength1.Text = "10"
        Me.txtLength1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblLength1
        '
        Me.lblLength1.Location = New System.Drawing.Point(8, 36)
        Me.lblLength1.Name = "lblLength1"
        Me.lblLength1.Size = New System.Drawing.Size(44, 20)
        Me.lblLength1.TabIndex = 2
        Me.lblLength1.Text = "Length:"
        Me.lblLength1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFirst1
        '
        Me.txtFirst1.Location = New System.Drawing.Point(56, 16)
        Me.txtFirst1.Name = "txtFirst1"
        Me.txtFirst1.Size = New System.Drawing.Size(40, 20)
        Me.txtFirst1.TabIndex = 1
        Me.txtFirst1.Text = "1"
        Me.txtFirst1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFirst1
        '
        Me.lblFirst1.Location = New System.Drawing.Point(8, 16)
        Me.lblFirst1.Name = "lblFirst1"
        Me.lblFirst1.Size = New System.Drawing.Size(44, 20)
        Me.lblFirst1.TabIndex = 0
        Me.lblFirst1.Text = "First:"
        Me.lblFirst1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'optPiece1
        '
        Me.optPiece1.Location = New System.Drawing.Point(12, 36)
        Me.optPiece1.Name = "optPiece1"
        Me.optPiece1.Size = New System.Drawing.Size(112, 20)
        Me.optPiece1.TabIndex = 1
        Me.optPiece1.Text = "Piece of filename"
        Me.optPiece1.UseVisualStyleBackColor = True
        '
        'txtPostfix1
        '
        Me.txtPostfix1.Location = New System.Drawing.Point(184, 84)
        Me.txtPostfix1.Name = "txtPostfix1"
        Me.txtPostfix1.Size = New System.Drawing.Size(60, 20)
        Me.txtPostfix1.TabIndex = 7
        Me.txtPostfix1.Text = "*.*"
        '
        'lblPostfix1
        '
        Me.lblPostfix1.Location = New System.Drawing.Point(128, 84)
        Me.lblPostfix1.Name = "lblPostfix1"
        Me.lblPostfix1.Size = New System.Drawing.Size(52, 20)
        Me.lblPostfix1.TabIndex = 6
        Me.lblPostfix1.Text = "Postfix:"
        Me.lblPostfix1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtPrefix1
        '
        Me.txtPrefix1.Location = New System.Drawing.Point(64, 84)
        Me.txtPrefix1.Name = "txtPrefix1"
        Me.txtPrefix1.Size = New System.Drawing.Size(60, 20)
        Me.txtPrefix1.TabIndex = 5
        '
        'lblSampleCaption1
        '
        Me.lblSampleCaption1.Location = New System.Drawing.Point(8, 108)
        Me.lblSampleCaption1.Name = "lblSampleCaption1"
        Me.lblSampleCaption1.Size = New System.Drawing.Size(52, 20)
        Me.lblSampleCaption1.TabIndex = 8
        Me.lblSampleCaption1.Text = "Pattern:"
        Me.lblSampleCaption1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPrefix1
        '
        Me.lblPrefix1.Location = New System.Drawing.Point(8, 84)
        Me.lblPrefix1.Name = "lblPrefix1"
        Me.lblPrefix1.Size = New System.Drawing.Size(52, 20)
        Me.lblPrefix1.TabIndex = 4
        Me.lblPrefix1.Text = "Prefix:"
        Me.lblPrefix1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'optWhole1
        '
        Me.optWhole1.Location = New System.Drawing.Point(12, 16)
        Me.optWhole1.Name = "optWhole1"
        Me.optWhole1.Size = New System.Drawing.Size(112, 20)
        Me.optWhole1.TabIndex = 0
        Me.optWhole1.Text = "Whole filename"
        Me.optWhole1.UseVisualStyleBackColor = True
        '
        'grpSelect2
        '
        Me.grpSelect2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpSelect2.Controls.Add(Me.optAuto2)
        Me.grpSelect2.Controls.Add(Me.lblSelected2)
        Me.grpSelect2.Controls.Add(Me.btnInverse2)
        Me.grpSelect2.Controls.Add(Me.btnDeselect2)
        Me.grpSelect2.Controls.Add(Me.btnNotExistsInFirst)
        Me.grpSelect2.Controls.Add(Me.btnExistsInFirst)
        Me.grpSelect2.Controls.Add(Me.lblSample2)
        Me.grpSelect2.Controls.Add(Me.grpSubString2)
        Me.grpSelect2.Controls.Add(Me.optPiece2)
        Me.grpSelect2.Controls.Add(Me.txtPostfix2)
        Me.grpSelect2.Controls.Add(Me.lblPostfix2)
        Me.grpSelect2.Controls.Add(Me.txtPrefix2)
        Me.grpSelect2.Controls.Add(Me.lblSampleCaption2)
        Me.grpSelect2.Controls.Add(Me.lblPrefix2)
        Me.grpSelect2.Controls.Add(Me.optWhole2)
        Me.grpSelect2.Location = New System.Drawing.Point(260, 350)
        Me.grpSelect2.Name = "grpSelect2"
        Me.grpSelect2.Size = New System.Drawing.Size(252, 232)
        Me.grpSelect2.TabIndex = 5
        Me.grpSelect2.TabStop = False
        Me.grpSelect2.Text = "Select"
        '
        'lblSelected2
        '
        Me.lblSelected2.Location = New System.Drawing.Point(8, 204)
        Me.lblSelected2.Name = "lblSelected2"
        Me.lblSelected2.Size = New System.Drawing.Size(236, 20)
        Me.lblSelected2.TabIndex = 14
        Me.lblSelected2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnInverse2
        '
        Me.btnInverse2.Location = New System.Drawing.Point(128, 168)
        Me.btnInverse2.Name = "btnInverse2"
        Me.btnInverse2.Size = New System.Drawing.Size(116, 32)
        Me.btnInverse2.TabIndex = 13
        Me.btnInverse2.Text = "Inverse Selection"
        Me.btnInverse2.UseVisualStyleBackColor = True
        '
        'btnDeselect2
        '
        Me.btnDeselect2.Location = New System.Drawing.Point(8, 168)
        Me.btnDeselect2.Name = "btnDeselect2"
        Me.btnDeselect2.Size = New System.Drawing.Size(116, 32)
        Me.btnDeselect2.TabIndex = 12
        Me.btnDeselect2.Text = "Deselect All"
        Me.btnDeselect2.UseVisualStyleBackColor = True
        '
        'btnNotExistsInFirst
        '
        Me.btnNotExistsInFirst.Location = New System.Drawing.Point(128, 132)
        Me.btnNotExistsInFirst.Name = "btnNotExistsInFirst"
        Me.btnNotExistsInFirst.Size = New System.Drawing.Size(116, 32)
        Me.btnNotExistsInFirst.TabIndex = 11
        Me.btnNotExistsInFirst.Text = "Not Exists in First"
        Me.btnNotExistsInFirst.UseVisualStyleBackColor = True
        '
        'btnExistsInFirst
        '
        Me.btnExistsInFirst.Location = New System.Drawing.Point(8, 132)
        Me.btnExistsInFirst.Name = "btnExistsInFirst"
        Me.btnExistsInFirst.Size = New System.Drawing.Size(116, 32)
        Me.btnExistsInFirst.TabIndex = 10
        Me.btnExistsInFirst.Text = "Exists in First"
        Me.btnExistsInFirst.UseVisualStyleBackColor = True
        '
        'lblSample2
        '
        Me.lblSample2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSample2.Location = New System.Drawing.Point(64, 108)
        Me.lblSample2.Name = "lblSample2"
        Me.lblSample2.Size = New System.Drawing.Size(180, 20)
        Me.lblSample2.TabIndex = 9
        Me.lblSample2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpSubString2
        '
        Me.grpSubString2.Controls.Add(Me.txtLength2)
        Me.grpSubString2.Controls.Add(Me.lblLength2)
        Me.grpSubString2.Controls.Add(Me.txtFirst2)
        Me.grpSubString2.Controls.Add(Me.lblFirst2)
        Me.grpSubString2.Location = New System.Drawing.Point(140, 16)
        Me.grpSubString2.Name = "grpSubString2"
        Me.grpSubString2.Size = New System.Drawing.Size(104, 64)
        Me.grpSubString2.TabIndex = 3
        Me.grpSubString2.TabStop = False
        Me.grpSubString2.Text = "SubString"
        '
        'txtLength2
        '
        Me.txtLength2.Location = New System.Drawing.Point(56, 36)
        Me.txtLength2.Name = "txtLength2"
        Me.txtLength2.Size = New System.Drawing.Size(40, 20)
        Me.txtLength2.TabIndex = 3
        Me.txtLength2.Text = "10"
        Me.txtLength2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblLength2
        '
        Me.lblLength2.Location = New System.Drawing.Point(8, 36)
        Me.lblLength2.Name = "lblLength2"
        Me.lblLength2.Size = New System.Drawing.Size(44, 20)
        Me.lblLength2.TabIndex = 2
        Me.lblLength2.Text = "Length:"
        Me.lblLength2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFirst2
        '
        Me.txtFirst2.Location = New System.Drawing.Point(56, 16)
        Me.txtFirst2.Name = "txtFirst2"
        Me.txtFirst2.Size = New System.Drawing.Size(40, 20)
        Me.txtFirst2.TabIndex = 1
        Me.txtFirst2.Text = "1"
        Me.txtFirst2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFirst2
        '
        Me.lblFirst2.Location = New System.Drawing.Point(8, 16)
        Me.lblFirst2.Name = "lblFirst2"
        Me.lblFirst2.Size = New System.Drawing.Size(44, 20)
        Me.lblFirst2.TabIndex = 0
        Me.lblFirst2.Text = "First:"
        Me.lblFirst2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'optPiece2
        '
        Me.optPiece2.Location = New System.Drawing.Point(12, 36)
        Me.optPiece2.Name = "optPiece2"
        Me.optPiece2.Size = New System.Drawing.Size(112, 20)
        Me.optPiece2.TabIndex = 1
        Me.optPiece2.Text = "Piece of filename"
        Me.optPiece2.UseVisualStyleBackColor = True
        '
        'txtPostfix2
        '
        Me.txtPostfix2.Location = New System.Drawing.Point(184, 84)
        Me.txtPostfix2.Name = "txtPostfix2"
        Me.txtPostfix2.Size = New System.Drawing.Size(60, 20)
        Me.txtPostfix2.TabIndex = 7
        Me.txtPostfix2.Text = "*.*"
        '
        'lblPostfix2
        '
        Me.lblPostfix2.Location = New System.Drawing.Point(128, 84)
        Me.lblPostfix2.Name = "lblPostfix2"
        Me.lblPostfix2.Size = New System.Drawing.Size(52, 20)
        Me.lblPostfix2.TabIndex = 6
        Me.lblPostfix2.Text = "Postfix:"
        Me.lblPostfix2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtPrefix2
        '
        Me.txtPrefix2.Location = New System.Drawing.Point(64, 84)
        Me.txtPrefix2.Name = "txtPrefix2"
        Me.txtPrefix2.Size = New System.Drawing.Size(60, 20)
        Me.txtPrefix2.TabIndex = 5
        '
        'lblSampleCaption2
        '
        Me.lblSampleCaption2.Location = New System.Drawing.Point(8, 108)
        Me.lblSampleCaption2.Name = "lblSampleCaption2"
        Me.lblSampleCaption2.Size = New System.Drawing.Size(52, 20)
        Me.lblSampleCaption2.TabIndex = 8
        Me.lblSampleCaption2.Text = "Pattern:"
        Me.lblSampleCaption2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPrefix2
        '
        Me.lblPrefix2.Location = New System.Drawing.Point(8, 84)
        Me.lblPrefix2.Name = "lblPrefix2"
        Me.lblPrefix2.Size = New System.Drawing.Size(52, 20)
        Me.lblPrefix2.TabIndex = 4
        Me.lblPrefix2.Text = "Prefix:"
        Me.lblPrefix2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'optWhole2
        '
        Me.optWhole2.Checked = True
        Me.optWhole2.Location = New System.Drawing.Point(12, 16)
        Me.optWhole2.Name = "optWhole2"
        Me.optWhole2.Size = New System.Drawing.Size(112, 20)
        Me.optWhole2.TabIndex = 0
        Me.optWhole2.TabStop = True
        Me.optWhole2.Text = "Whole filename"
        Me.optWhole2.UseVisualStyleBackColor = True
        '
        'btnDelete1
        '
        Me.btnDelete1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDelete1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.btnDelete1.Location = New System.Drawing.Point(132, 586)
        Me.btnDelete1.Name = "btnDelete1"
        Me.btnDelete1.Size = New System.Drawing.Size(116, 32)
        Me.btnDelete1.TabIndex = 6
        Me.btnDelete1.Text = "Delete Selected"
        Me.btnDelete1.UseVisualStyleBackColor = True
        '
        'btnDelete2
        '
        Me.btnDelete2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDelete2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.btnDelete2.Location = New System.Drawing.Point(388, 586)
        Me.btnDelete2.Name = "btnDelete2"
        Me.btnDelete2.Size = New System.Drawing.Size(116, 32)
        Me.btnDelete2.TabIndex = 7
        Me.btnDelete2.Text = "Delete Selected"
        Me.btnDelete2.UseVisualStyleBackColor = True
        '
        'dlgFolder
        '
        Me.dlgFolder.RootFolder = System.Environment.SpecialFolder.MyComputer
        Me.dlgFolder.ShowNewFolderButton = False
        '
        'optAuto1
        '
        Me.optAuto1.Checked = True
        Me.optAuto1.Location = New System.Drawing.Point(12, 56)
        Me.optAuto1.Name = "optAuto1"
        Me.optAuto1.Size = New System.Drawing.Size(112, 20)
        Me.optAuto1.TabIndex = 2
        Me.optAuto1.TabStop = True
        Me.optAuto1.Text = "Auto length"
        Me.optAuto1.UseVisualStyleBackColor = True
        '
        'optAuto2
        '
        Me.optAuto2.Location = New System.Drawing.Point(11, 56)
        Me.optAuto2.Name = "optAuto2"
        Me.optAuto2.Size = New System.Drawing.Size(112, 20)
        Me.optAuto2.TabIndex = 2
        Me.optAuto2.Text = "Auto length"
        Me.optAuto2.UseVisualStyleBackColor = True
        '
        'frmCompare
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(516, 623)
        Me.Controls.Add(Me.btnDelete2)
        Me.Controls.Add(Me.btnDelete1)
        Me.Controls.Add(Me.grpSelect2)
        Me.Controls.Add(Me.grpSelect1)
        Me.Controls.Add(Me.grpPath2)
        Me.Controls.Add(Me.grpPath1)
        Me.Controls.Add(Me.lstFiles2)
        Me.Controls.Add(Me.lstFiles1)
        Me.KeyPreview = True
        Me.MaximumSize = New System.Drawing.Size(524, 2000)
        Me.MinimumSize = New System.Drawing.Size(524, 459)
        Me.Name = "frmCompare"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Directory Comparing Tool"
        Me.grpPath1.ResumeLayout(False)
        Me.grpPath1.PerformLayout()
        Me.grpPath2.ResumeLayout(False)
        Me.grpPath2.PerformLayout()
        Me.grpSelect1.ResumeLayout(False)
        Me.grpSelect1.PerformLayout()
        Me.grpSubString1.ResumeLayout(False)
        Me.grpSubString1.PerformLayout()
        Me.grpSelect2.ResumeLayout(False)
        Me.grpSelect2.PerformLayout()
        Me.grpSubString2.ResumeLayout(False)
        Me.grpSubString2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents grpPath1 As System.Windows.Forms.GroupBox
    Private WithEvents lblCount1 As System.Windows.Forms.Label
    Private WithEvents cboFilter1 As System.Windows.Forms.ComboBox
    Private WithEvents btnPath1 As System.Windows.Forms.Button
    Private WithEvents txtPath1 As System.Windows.Forms.TextBox
    Private WithEvents lblFilter1 As System.Windows.Forms.Label
    Private WithEvents lblPath1 As System.Windows.Forms.Label
    Private WithEvents lstFiles1 As System.Windows.Forms.ListBox
    Private WithEvents grpPath2 As System.Windows.Forms.GroupBox
    Private WithEvents lblCount2 As System.Windows.Forms.Label
    Private WithEvents cboFilter2 As System.Windows.Forms.ComboBox
    Private WithEvents btnPath2 As System.Windows.Forms.Button
    Private WithEvents txtPath2 As System.Windows.Forms.TextBox
    Private WithEvents lblFilter2 As System.Windows.Forms.Label
    Private WithEvents lblPath2 As System.Windows.Forms.Label
    Private WithEvents lstFiles2 As System.Windows.Forms.ListBox
    Private WithEvents grpSelect1 As System.Windows.Forms.GroupBox
    Private WithEvents grpSubString1 As System.Windows.Forms.GroupBox
    Private WithEvents optPiece1 As System.Windows.Forms.RadioButton
    Private WithEvents optWhole1 As System.Windows.Forms.RadioButton
    Private WithEvents txtFirst1 As System.Windows.Forms.TextBox
    Private WithEvents lblFirst1 As System.Windows.Forms.Label
    Private WithEvents txtLength1 As System.Windows.Forms.TextBox
    Private WithEvents lblLength1 As System.Windows.Forms.Label
    Private WithEvents txtPrefix1 As System.Windows.Forms.TextBox
    Private WithEvents lblPrefix1 As System.Windows.Forms.Label
    Private WithEvents btnExistsInSecond As System.Windows.Forms.Button
    Private WithEvents lblSample1 As System.Windows.Forms.Label
    Private WithEvents txtPostfix1 As System.Windows.Forms.TextBox
    Private WithEvents lblPostfix1 As System.Windows.Forms.Label
    Private WithEvents lblSampleCaption1 As System.Windows.Forms.Label
    Private WithEvents btnInverse1 As System.Windows.Forms.Button
    Private WithEvents btnDeselect1 As System.Windows.Forms.Button
    Private WithEvents btnNotExistsInSecond As System.Windows.Forms.Button
    Private WithEvents grpSelect2 As System.Windows.Forms.GroupBox
    Private WithEvents btnInverse2 As System.Windows.Forms.Button
    Private WithEvents btnDeselect2 As System.Windows.Forms.Button
    Private WithEvents btnNotExistsInFirst As System.Windows.Forms.Button
    Private WithEvents btnExistsInFirst As System.Windows.Forms.Button
    Private WithEvents lblSample2 As System.Windows.Forms.Label
    Private WithEvents grpSubString2 As System.Windows.Forms.GroupBox
    Private WithEvents txtLength2 As System.Windows.Forms.TextBox
    Private WithEvents lblLength2 As System.Windows.Forms.Label
    Private WithEvents txtFirst2 As System.Windows.Forms.TextBox
    Private WithEvents lblFirst2 As System.Windows.Forms.Label
    Private WithEvents optPiece2 As System.Windows.Forms.RadioButton
    Private WithEvents txtPostfix2 As System.Windows.Forms.TextBox
    Private WithEvents lblPostfix2 As System.Windows.Forms.Label
    Private WithEvents txtPrefix2 As System.Windows.Forms.TextBox
    Private WithEvents lblSampleCaption2 As System.Windows.Forms.Label
    Private WithEvents lblPrefix2 As System.Windows.Forms.Label
    Private WithEvents optWhole2 As System.Windows.Forms.RadioButton
    Private WithEvents btnDelete1 As System.Windows.Forms.Button
    Private WithEvents btnDelete2 As System.Windows.Forms.Button
    Private WithEvents txtSubFolder2 As System.Windows.Forms.TextBox
    Private WithEvents lblSubFolder2 As System.Windows.Forms.Label
    Private WithEvents lblSelected1 As System.Windows.Forms.Label
    Private WithEvents lblSelected2 As System.Windows.Forms.Label
    Private WithEvents dlgFolder As System.Windows.Forms.FolderBrowserDialog
    Private WithEvents btnSubFolder2 As System.Windows.Forms.Button
    Private WithEvents chkSubfolderOfFirst As System.Windows.Forms.CheckBox
    Private WithEvents btnLastDir As System.Windows.Forms.Button
    Private WithEvents btnNextDir As System.Windows.Forms.Button
    Private WithEvents btnPrevDir As System.Windows.Forms.Button
    Private WithEvents btnFirstDir As System.Windows.Forms.Button
    Private WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Private WithEvents optAuto1 As System.Windows.Forms.RadioButton
    Private WithEvents optAuto2 As System.Windows.Forms.RadioButton
End Class
