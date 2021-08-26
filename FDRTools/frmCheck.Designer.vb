<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCheck
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
		Me.grpSource = New System.Windows.Forms.GroupBox()
		Me.cboFilter = New System.Windows.Forms.ComboBox()
		Me.btnPath = New System.Windows.Forms.Button()
		Me.txtPath = New System.Windows.Forms.TextBox()
		Me.lblFilter = New System.Windows.Forms.Label()
		Me.lblPath = New System.Windows.Forms.Label()
		Me.grpCheck = New System.Windows.Forms.GroupBox()
		Me.lstFaulty = New System.Windows.Forms.ListBox()
		Me.btnCheck = New System.Windows.Forms.Button()
		Me.dlgFolder = New System.Windows.Forms.FolderBrowserDialog()
		Me.txtFrom = New System.Windows.Forms.Label()
		Me.TextBox1 = New System.Windows.Forms.TextBox()
		Me.btnGenerate = New System.Windows.Forms.Button()
		Me.lblCount = New System.Windows.Forms.Label()
		Me.grpSource.SuspendLayout()
		Me.grpCheck.SuspendLayout()
		Me.SuspendLayout()
		'
		'grpSource
		'
		Me.grpSource.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grpSource.Controls.Add(Me.cboFilter)
		Me.grpSource.Controls.Add(Me.btnPath)
		Me.grpSource.Controls.Add(Me.txtPath)
		Me.grpSource.Controls.Add(Me.lblFilter)
		Me.grpSource.Controls.Add(Me.lblPath)
		Me.grpSource.Location = New System.Drawing.Point(4, 4)
		Me.grpSource.Name = "grpSource"
		Me.grpSource.Size = New System.Drawing.Size(424, 72)
		Me.grpSource.TabIndex = 0
		Me.grpSource.TabStop = False
		Me.grpSource.Text = "Source"
		'
		'cboFilter
		'
		Me.cboFilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
		'grpCheck
		'
		Me.grpCheck.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
				  Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grpCheck.Controls.Add(Me.lblCount)
		Me.grpCheck.Controls.Add(Me.btnGenerate)
		Me.grpCheck.Controls.Add(Me.lstFaulty)
		Me.grpCheck.Controls.Add(Me.btnCheck)
		Me.grpCheck.Controls.Add(Me.TextBox1)
		Me.grpCheck.Controls.Add(Me.txtFrom)
		Me.grpCheck.Location = New System.Drawing.Point(4, 80)
		Me.grpCheck.Name = "grpCheck"
		Me.grpCheck.Size = New System.Drawing.Size(424, 374)
		Me.grpCheck.TabIndex = 1
		Me.grpCheck.TabStop = False
		Me.grpCheck.Text = "Check"
		'
		'lstFaulty
		'
		Me.lstFaulty.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
				  Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lstFaulty.FormattingEnabled = True
		Me.lstFaulty.Location = New System.Drawing.Point(8, 40)
		Me.lstFaulty.Name = "lstFaulty"
		Me.lstFaulty.Size = New System.Drawing.Size(408, 264)
		Me.lstFaulty.Sorted = True
		Me.lstFaulty.TabIndex = 2
		Me.lstFaulty.TabStop = False
		'
		'btnCheck
		'
		Me.btnCheck.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnCheck.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
		Me.btnCheck.Location = New System.Drawing.Point(8, 12)
		Me.btnCheck.Name = "btnCheck"
		Me.btnCheck.Size = New System.Drawing.Size(408, 24)
		Me.btnCheck.TabIndex = 1
		Me.btnCheck.Text = "Check"
		Me.btnCheck.UseVisualStyleBackColor = True
		'
		'dlgFolder
		'
		Me.dlgFolder.RootFolder = System.Environment.SpecialFolder.MyComputer
		Me.dlgFolder.ShowNewFolderButton = False
		'
		'txtFrom
		'
		Me.txtFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.txtFrom.Location = New System.Drawing.Point(8, 332)
		Me.txtFrom.Name = "txtFrom"
		Me.txtFrom.Size = New System.Drawing.Size(100, 16)
		Me.txtFrom.TabIndex = 3
		Me.txtFrom.Text = "From:"
		Me.txtFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'TextBox1
		'
		Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.TextBox1.Location = New System.Drawing.Point(112, 328)
		Me.TextBox1.Name = "TextBox1"
		Me.TextBox1.Size = New System.Drawing.Size(304, 20)
		Me.TextBox1.TabIndex = 4
		'
		'btnGenerate
		'
		Me.btnGenerate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnGenerate.Location = New System.Drawing.Point(112, 348)
		Me.btnGenerate.Name = "btnGenerate"
		Me.btnGenerate.Size = New System.Drawing.Size(304, 20)
		Me.btnGenerate.TabIndex = 5
		Me.btnGenerate.Text = "Generate Copy Script"
		Me.btnGenerate.UseVisualStyleBackColor = True
		'
		'lblCount
		'
		Me.lblCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lblCount.Location = New System.Drawing.Point(268, 308)
		Me.lblCount.Name = "lblCount"
		Me.lblCount.Size = New System.Drawing.Size(148, 16)
		Me.lblCount.TabIndex = 6
		Me.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'frmCheck
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(432, 460)
		Me.Controls.Add(Me.grpCheck)
		Me.Controls.Add(Me.grpSource)
		Me.KeyPreview = True
		Me.MinimumSize = New System.Drawing.Size(440, 358)
		Me.Name = "frmCheck"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Check Images"
		Me.grpSource.ResumeLayout(False)
		Me.grpSource.PerformLayout()
		Me.grpCheck.ResumeLayout(False)
		Me.grpCheck.PerformLayout()
		Me.ResumeLayout(False)

	End Sub
	Private WithEvents grpSource As System.Windows.Forms.GroupBox
	Private WithEvents btnPath As System.Windows.Forms.Button
	Private WithEvents txtPath As System.Windows.Forms.TextBox
	Private WithEvents lblPath As System.Windows.Forms.Label
	Private WithEvents cboFilter As System.Windows.Forms.ComboBox
	Private WithEvents lblFilter As System.Windows.Forms.Label
	Private WithEvents grpCheck As System.Windows.Forms.GroupBox
	Private WithEvents btnCheck As System.Windows.Forms.Button
	Private WithEvents dlgFolder As System.Windows.Forms.FolderBrowserDialog
	Private WithEvents lstFaulty As System.Windows.Forms.ListBox
	Private WithEvents TextBox1 As System.Windows.Forms.TextBox
	Private WithEvents txtFrom As System.Windows.Forms.Label
	Private WithEvents btnGenerate As System.Windows.Forms.Button
	Private WithEvents lblCount As System.Windows.Forms.Label
End Class
