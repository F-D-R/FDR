<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRename
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
        Me.grpSource = New System.Windows.Forms.GroupBox
        Me.lblCount = New System.Windows.Forms.Label
        Me.lstFiles = New System.Windows.Forms.ListBox
        Me.cboFilter = New System.Windows.Forms.ComboBox
        Me.btnPath = New System.Windows.Forms.Button
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.lblFiles = New System.Windows.Forms.Label
        Me.lblFilter = New System.Windows.Forms.Label
        Me.lblPath = New System.Windows.Forms.Label
        Me.grpAdditional = New System.Windows.Forms.GroupBox
        Me.chkThird = New System.Windows.Forms.CheckBox
        Me.chkSecond = New System.Windows.Forms.CheckBox
        Me.cboThird = New System.Windows.Forms.ComboBox
        Me.cboSecond = New System.Windows.Forms.ComboBox
        Me.lblThird = New System.Windows.Forms.Label
        Me.lblSecond = New System.Windows.Forms.Label
        Me.grpRename = New System.Windows.Forms.GroupBox
        Me.btnRename = New System.Windows.Forms.Button
        Me.chkAutoPrefix = New System.Windows.Forms.CheckBox
        Me.lblSample = New System.Windows.Forms.Label
        Me.cboExtensionCase = New System.Windows.Forms.ComboBox
        Me.cboFileNameCase = New System.Windows.Forms.ComboBox
        Me.txtFirst = New System.Windows.Forms.TextBox
        Me.lblFirst = New System.Windows.Forms.Label
        Me.lblExtensionCase = New System.Windows.Forms.Label
        Me.txtDigits = New System.Windows.Forms.TextBox
        Me.lblFileNameCase = New System.Windows.Forms.Label
        Me.lblDigits = New System.Windows.Forms.Label
        Me.txtPostfix = New System.Windows.Forms.TextBox
        Me.lblPostfix = New System.Windows.Forms.Label
        Me.txtPrefix = New System.Windows.Forms.TextBox
        Me.lblPrefix = New System.Windows.Forms.Label
        Me.dlgFolder = New System.Windows.Forms.FolderBrowserDialog
        Me.grpSource.SuspendLayout()
        Me.grpAdditional.SuspendLayout()
        Me.grpRename.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpSource
        '
        Me.grpSource.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSource.Controls.Add(Me.lblCount)
        Me.grpSource.Controls.Add(Me.lstFiles)
        Me.grpSource.Controls.Add(Me.cboFilter)
        Me.grpSource.Controls.Add(Me.btnPath)
        Me.grpSource.Controls.Add(Me.txtPath)
        Me.grpSource.Controls.Add(Me.lblFiles)
        Me.grpSource.Controls.Add(Me.lblFilter)
        Me.grpSource.Controls.Add(Me.lblPath)
        Me.grpSource.Location = New System.Drawing.Point(4, 4)
        Me.grpSource.Name = "grpSource"
        Me.grpSource.Size = New System.Drawing.Size(424, 236)
        Me.grpSource.TabIndex = 0
        Me.grpSource.TabStop = False
        Me.grpSource.Text = "Source"
        '
        'lblCount
        '
        Me.lblCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCount.Location = New System.Drawing.Point(296, 212)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(120, 16)
        Me.lblCount.TabIndex = 7
        Me.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lstFiles
        '
        Me.lstFiles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstFiles.FormattingEnabled = True
        Me.lstFiles.Location = New System.Drawing.Point(112, 68)
        Me.lstFiles.Name = "lstFiles"
        Me.lstFiles.Size = New System.Drawing.Size(180, 160)
        Me.lstFiles.Sorted = True
        Me.lstFiles.TabIndex = 6
        Me.lstFiles.TabStop = False
        '
        'cboFilter
        '
        Me.cboFilter.FormattingEnabled = True
        Me.cboFilter.Location = New System.Drawing.Point(112, 44)
        Me.cboFilter.Name = "cboFilter"
        Me.cboFilter.Size = New System.Drawing.Size(180, 21)
        Me.cboFilter.TabIndex = 4
        '
        'btnPath
        '
        Me.btnPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPath.Location = New System.Drawing.Point(392, 20)
        Me.btnPath.Name = "btnPath"
        Me.btnPath.Size = New System.Drawing.Size(24, 20)
        Me.btnPath.TabIndex = 2
        Me.btnPath.Text = "..."
        Me.btnPath.UseVisualStyleBackColor = True
        '
        'txtPath
        '
        Me.txtPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPath.Location = New System.Drawing.Point(112, 20)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.ReadOnly = True
        Me.txtPath.Size = New System.Drawing.Size(280, 20)
        Me.txtPath.TabIndex = 1
        Me.txtPath.TabStop = False
        '
        'lblFiles
        '
        Me.lblFiles.Location = New System.Drawing.Point(8, 72)
        Me.lblFiles.Name = "lblFiles"
        Me.lblFiles.Size = New System.Drawing.Size(100, 16)
        Me.lblFiles.TabIndex = 5
        Me.lblFiles.Text = "File &list:"
        Me.lblFiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFilter
        '
        Me.lblFilter.Location = New System.Drawing.Point(8, 48)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(100, 16)
        Me.lblFilter.TabIndex = 3
        Me.lblFilter.Text = "&Filter:"
        Me.lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPath
        '
        Me.lblPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblPath.Location = New System.Drawing.Point(8, 24)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.Size = New System.Drawing.Size(100, 16)
        Me.lblPath.TabIndex = 0
        Me.lblPath.Text = "&Path:"
        Me.lblPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpAdditional
        '
        Me.grpAdditional.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAdditional.Controls.Add(Me.chkThird)
        Me.grpAdditional.Controls.Add(Me.chkSecond)
        Me.grpAdditional.Controls.Add(Me.cboThird)
        Me.grpAdditional.Controls.Add(Me.cboSecond)
        Me.grpAdditional.Controls.Add(Me.lblThird)
        Me.grpAdditional.Controls.Add(Me.lblSecond)
        Me.grpAdditional.Location = New System.Drawing.Point(4, 244)
        Me.grpAdditional.Name = "grpAdditional"
        Me.grpAdditional.Size = New System.Drawing.Size(424, 68)
        Me.grpAdditional.TabIndex = 1
        Me.grpAdditional.TabStop = False
        Me.grpAdditional.Text = "Additional files"
        '
        'chkThird
        '
        Me.chkThird.Location = New System.Drawing.Point(112, 44)
        Me.chkThird.Name = "chkThird"
        Me.chkThird.Size = New System.Drawing.Size(80, 16)
        Me.chkThird.TabIndex = 3
        Me.chkThird.Text = "&Third"
        Me.chkThird.UseVisualStyleBackColor = True
        '
        'chkSecond
        '
        Me.chkSecond.Location = New System.Drawing.Point(112, 20)
        Me.chkSecond.Name = "chkSecond"
        Me.chkSecond.Size = New System.Drawing.Size(80, 16)
        Me.chkSecond.TabIndex = 0
        Me.chkSecond.Text = "&Second"
        Me.chkSecond.UseVisualStyleBackColor = True
        '
        'cboThird
        '
        Me.cboThird.FormattingEnabled = True
        Me.cboThird.Location = New System.Drawing.Point(296, 40)
        Me.cboThird.Name = "cboThird"
        Me.cboThird.Size = New System.Drawing.Size(120, 21)
        Me.cboThird.TabIndex = 5
        '
        'cboSecond
        '
        Me.cboSecond.FormattingEnabled = True
        Me.cboSecond.Location = New System.Drawing.Point(296, 16)
        Me.cboSecond.Name = "cboSecond"
        Me.cboSecond.Size = New System.Drawing.Size(120, 21)
        Me.cboSecond.TabIndex = 2
        '
        'lblThird
        '
        Me.lblThird.Location = New System.Drawing.Point(196, 44)
        Me.lblThird.Name = "lblThird"
        Me.lblThird.Size = New System.Drawing.Size(96, 16)
        Me.lblThird.TabIndex = 4
        Me.lblThird.Text = "Third file type:"
        Me.lblThird.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblSecond
        '
        Me.lblSecond.Location = New System.Drawing.Point(196, 20)
        Me.lblSecond.Name = "lblSecond"
        Me.lblSecond.Size = New System.Drawing.Size(96, 16)
        Me.lblSecond.TabIndex = 1
        Me.lblSecond.Text = "Second file type:"
        Me.lblSecond.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpRename
        '
        Me.grpRename.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpRename.Controls.Add(Me.btnRename)
        Me.grpRename.Controls.Add(Me.chkAutoPrefix)
        Me.grpRename.Controls.Add(Me.lblSample)
        Me.grpRename.Controls.Add(Me.cboExtensionCase)
        Me.grpRename.Controls.Add(Me.cboFileNameCase)
        Me.grpRename.Controls.Add(Me.txtFirst)
        Me.grpRename.Controls.Add(Me.lblFirst)
        Me.grpRename.Controls.Add(Me.lblExtensionCase)
        Me.grpRename.Controls.Add(Me.txtDigits)
        Me.grpRename.Controls.Add(Me.lblFileNameCase)
        Me.grpRename.Controls.Add(Me.lblDigits)
        Me.grpRename.Controls.Add(Me.txtPostfix)
        Me.grpRename.Controls.Add(Me.lblPostfix)
        Me.grpRename.Controls.Add(Me.txtPrefix)
        Me.grpRename.Controls.Add(Me.lblPrefix)
        Me.grpRename.Location = New System.Drawing.Point(4, 316)
        Me.grpRename.Name = "grpRename"
        Me.grpRename.Size = New System.Drawing.Size(424, 155)
        Me.grpRename.TabIndex = 2
        Me.grpRename.TabStop = False
        Me.grpRename.Text = "Rename"
        '
        'btnRename
        '
        Me.btnRename.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.btnRename.Location = New System.Drawing.Point(296, 108)
        Me.btnRename.Name = "btnRename"
        Me.btnRename.Size = New System.Drawing.Size(120, 40)
        Me.btnRename.TabIndex = 14
        Me.btnRename.Text = "&Rename"
        Me.btnRename.UseVisualStyleBackColor = True
        '
        'chkAutoPrefix
        '
        Me.chkAutoPrefix.Checked = True
        Me.chkAutoPrefix.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAutoPrefix.Location = New System.Drawing.Point(296, 24)
        Me.chkAutoPrefix.Name = "chkAutoPrefix"
        Me.chkAutoPrefix.Size = New System.Drawing.Size(120, 16)
        Me.chkAutoPrefix.TabIndex = 2
        Me.chkAutoPrefix.Text = "Auto prefix"
        Me.chkAutoPrefix.UseVisualStyleBackColor = True
        '
        'lblSample
        '
        Me.lblSample.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSample.Location = New System.Drawing.Point(112, 132)
        Me.lblSample.Name = "lblSample"
        Me.lblSample.Size = New System.Drawing.Size(180, 16)
        Me.lblSample.TabIndex = 13
        Me.lblSample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cboExtensionCase
        '
        Me.cboExtensionCase.DisplayMember = "Caption"
        Me.cboExtensionCase.FormattingEnabled = True
        Me.cboExtensionCase.Location = New System.Drawing.Point(112, 108)
        Me.cboExtensionCase.Name = "cboExtensionCase"
        Me.cboExtensionCase.Size = New System.Drawing.Size(180, 21)
        Me.cboExtensionCase.TabIndex = 12
        '
        'cboFileNameCase
        '
        Me.cboFileNameCase.DisplayMember = "Caption"
        Me.cboFileNameCase.FormattingEnabled = True
        Me.cboFileNameCase.Location = New System.Drawing.Point(112, 88)
        Me.cboFileNameCase.Name = "cboFileNameCase"
        Me.cboFileNameCase.Size = New System.Drawing.Size(180, 21)
        Me.cboFileNameCase.TabIndex = 10
        '
        'txtFirst
        '
        Me.txtFirst.Location = New System.Drawing.Point(252, 64)
        Me.txtFirst.Name = "txtFirst"
        Me.txtFirst.Size = New System.Drawing.Size(40, 20)
        Me.txtFirst.TabIndex = 8
        Me.txtFirst.Text = "1"
        Me.txtFirst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFirst
        '
        Me.lblFirst.Location = New System.Drawing.Point(156, 68)
        Me.lblFirst.Name = "lblFirst"
        Me.lblFirst.Size = New System.Drawing.Size(92, 16)
        Me.lblFirst.TabIndex = 7
        Me.lblFirst.Text = "Firs&t index:"
        Me.lblFirst.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblExtensionCase
        '
        Me.lblExtensionCase.Location = New System.Drawing.Point(8, 112)
        Me.lblExtensionCase.Name = "lblExtensionCase"
        Me.lblExtensionCase.Size = New System.Drawing.Size(100, 16)
        Me.lblExtensionCase.TabIndex = 11
        Me.lblExtensionCase.Text = "Extensio&n case:"
        Me.lblExtensionCase.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDigits
        '
        Me.txtDigits.Location = New System.Drawing.Point(112, 64)
        Me.txtDigits.Name = "txtDigits"
        Me.txtDigits.Size = New System.Drawing.Size(40, 20)
        Me.txtDigits.TabIndex = 6
        Me.txtDigits.Text = "1"
        Me.txtDigits.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFileNameCase
        '
        Me.lblFileNameCase.Location = New System.Drawing.Point(8, 92)
        Me.lblFileNameCase.Name = "lblFileNameCase"
        Me.lblFileNameCase.Size = New System.Drawing.Size(100, 16)
        Me.lblFileNameCase.TabIndex = 9
        Me.lblFileNameCase.Text = "Filename &case:"
        Me.lblFileNameCase.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDigits
        '
        Me.lblDigits.Location = New System.Drawing.Point(8, 68)
        Me.lblDigits.Name = "lblDigits"
        Me.lblDigits.Size = New System.Drawing.Size(100, 16)
        Me.lblDigits.TabIndex = 5
        Me.lblDigits.Text = "&Digits:"
        Me.lblDigits.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtPostfix
        '
        Me.txtPostfix.Location = New System.Drawing.Point(112, 40)
        Me.txtPostfix.Name = "txtPostfix"
        Me.txtPostfix.Size = New System.Drawing.Size(180, 20)
        Me.txtPostfix.TabIndex = 4
        '
        'lblPostfix
        '
        Me.lblPostfix.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblPostfix.Location = New System.Drawing.Point(8, 44)
        Me.lblPostfix.Name = "lblPostfix"
        Me.lblPostfix.Size = New System.Drawing.Size(100, 16)
        Me.lblPostfix.TabIndex = 3
        Me.lblPostfix.Text = "P&ostfix:"
        Me.lblPostfix.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtPrefix
        '
        Me.txtPrefix.Location = New System.Drawing.Point(112, 20)
        Me.txtPrefix.Name = "txtPrefix"
        Me.txtPrefix.Size = New System.Drawing.Size(180, 20)
        Me.txtPrefix.TabIndex = 1
        '
        'lblPrefix
        '
        Me.lblPrefix.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblPrefix.Location = New System.Drawing.Point(8, 24)
        Me.lblPrefix.Name = "lblPrefix"
        Me.lblPrefix.Size = New System.Drawing.Size(100, 16)
        Me.lblPrefix.TabIndex = 0
        Me.lblPrefix.Text = "Prefi&x:"
        Me.lblPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dlgFolder
        '
        Me.dlgFolder.RootFolder = System.Environment.SpecialFolder.MyComputer
        Me.dlgFolder.ShowNewFolderButton = False
        '
        'frmRename
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(432, 474)
        Me.Controls.Add(Me.grpRename)
        Me.Controls.Add(Me.grpAdditional)
        Me.Controls.Add(Me.grpSource)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(440, 358)
        Me.Name = "frmRename"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "File Renaming Tool"
        Me.grpSource.ResumeLayout(False)
        Me.grpSource.PerformLayout()
        Me.grpAdditional.ResumeLayout(False)
        Me.grpRename.ResumeLayout(False)
        Me.grpRename.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents grpSource As System.Windows.Forms.GroupBox
    Private WithEvents btnPath As System.Windows.Forms.Button
    Private WithEvents txtPath As System.Windows.Forms.TextBox
    Private WithEvents lblPath As System.Windows.Forms.Label
    Private WithEvents cboFilter As System.Windows.Forms.ComboBox
    Private WithEvents lblFilter As System.Windows.Forms.Label
    Private WithEvents lstFiles As System.Windows.Forms.ListBox
    Private WithEvents lblFiles As System.Windows.Forms.Label
    Private WithEvents lblCount As System.Windows.Forms.Label
    Private WithEvents grpAdditional As System.Windows.Forms.GroupBox
    Private WithEvents chkThird As System.Windows.Forms.CheckBox
    Private WithEvents chkSecond As System.Windows.Forms.CheckBox
    Private WithEvents cboThird As System.Windows.Forms.ComboBox
    Private WithEvents cboSecond As System.Windows.Forms.ComboBox
    Private WithEvents lblThird As System.Windows.Forms.Label
    Private WithEvents lblSecond As System.Windows.Forms.Label
    Private WithEvents grpRename As System.Windows.Forms.GroupBox
    Private WithEvents txtPrefix As System.Windows.Forms.TextBox
    Private WithEvents lblPrefix As System.Windows.Forms.Label
    Private WithEvents txtPostfix As System.Windows.Forms.TextBox
    Private WithEvents lblPostfix As System.Windows.Forms.Label
    Private WithEvents txtFirst As System.Windows.Forms.TextBox
    Private WithEvents lblFirst As System.Windows.Forms.Label
    Private WithEvents txtDigits As System.Windows.Forms.TextBox
    Private WithEvents lblDigits As System.Windows.Forms.Label
    Private WithEvents lblSample As System.Windows.Forms.Label
    Private WithEvents btnRename As System.Windows.Forms.Button
    Private WithEvents dlgFolder As System.Windows.Forms.FolderBrowserDialog
    Private WithEvents chkAutoPrefix As System.Windows.Forms.CheckBox
    Private WithEvents cboFileNameCase As System.Windows.Forms.ComboBox
    Private WithEvents lblFileNameCase As System.Windows.Forms.Label
    Private WithEvents cboExtensionCase As System.Windows.Forms.ComboBox
    Private WithEvents lblExtensionCase As System.Windows.Forms.Label
End Class
