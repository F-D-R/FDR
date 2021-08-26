<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmList
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.mnuMain = New System.Windows.Forms.MenuStrip
        Me.mnuList = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRefresh = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSep1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuClose = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuData = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSave = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuCancel = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSep2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuNew = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFunctions = New System.Windows.Forms.ToolStripMenuItem
        Me.grdList = New System.Windows.Forms.DataGridView
        Me.stsStatus = New System.Windows.Forms.StatusStrip
        Me.lblMsg = New System.Windows.Forms.ToolStripStatusLabel
        Me.lblRows = New System.Windows.Forms.ToolStripStatusLabel
        Me.tlsToolStrip = New System.Windows.Forms.ToolStrip
        Me.tsbRefresh = New System.Windows.Forms.ToolStripButton
        Me.cboWhere = New System.Windows.Forms.ToolStripComboBox
        Me.mnuMain.SuspendLayout()
        CType(Me.grdList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.stsStatus.SuspendLayout()
        Me.tlsToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'mnuMain
        '
        Me.mnuMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuList, Me.mnuData, Me.mnuFunctions})
        Me.mnuMain.Location = New System.Drawing.Point(0, 0)
        Me.mnuMain.Name = "mnuMain"
        Me.mnuMain.Size = New System.Drawing.Size(672, 24)
        Me.mnuMain.TabIndex = 0
        Me.mnuMain.Text = "MenuStrip1"
        '
        'mnuList
        '
        Me.mnuList.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRefresh, Me.mnuSep1, Me.mnuClose})
        Me.mnuList.Name = "mnuList"
        Me.mnuList.Size = New System.Drawing.Size(35, 20)
        Me.mnuList.Text = "&List"
        '
        'mnuRefresh
        '
        Me.mnuRefresh.Image = Global.FDRTools.My.Resources.Resources.find
        Me.mnuRefresh.ImageTransparentColor = System.Drawing.Color.Silver
        Me.mnuRefresh.Name = "mnuRefresh"
        Me.mnuRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.mnuRefresh.Size = New System.Drawing.Size(131, 22)
        Me.mnuRefresh.Text = "&Refresh"
        '
        'mnuSep1
        '
        Me.mnuSep1.Name = "mnuSep1"
        Me.mnuSep1.Size = New System.Drawing.Size(128, 6)
        '
        'mnuClose
        '
        Me.mnuClose.Image = Global.FDRTools.My.Resources.Resources.close
        Me.mnuClose.ImageTransparentColor = System.Drawing.Color.Silver
        Me.mnuClose.Name = "mnuClose"
        Me.mnuClose.Size = New System.Drawing.Size(131, 22)
        Me.mnuClose.Text = "&Close"
        '
        'mnuData
        '
        Me.mnuData.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSave, Me.mnuCancel, Me.mnuSep2, Me.mnuNew, Me.mnuDelete})
        Me.mnuData.Name = "mnuData"
        Me.mnuData.Size = New System.Drawing.Size(42, 20)
        Me.mnuData.Text = "&Data"
        '
        'mnuSave
        '
        Me.mnuSave.Image = Global.FDRTools.My.Resources.Resources.save
        Me.mnuSave.ImageTransparentColor = System.Drawing.Color.Silver
        Me.mnuSave.Name = "mnuSave"
        Me.mnuSave.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mnuSave.Size = New System.Drawing.Size(173, 22)
        Me.mnuSave.Text = "&Save"
        '
        'mnuCancel
        '
        Me.mnuCancel.Image = Global.FDRTools.My.Resources.Resources.cancel
        Me.mnuCancel.ImageTransparentColor = System.Drawing.Color.Silver
        Me.mnuCancel.Name = "mnuCancel"
        Me.mnuCancel.Size = New System.Drawing.Size(173, 22)
        Me.mnuCancel.Text = "&Cancel"
        '
        'mnuSep2
        '
        Me.mnuSep2.Name = "mnuSep2"
        Me.mnuSep2.Size = New System.Drawing.Size(170, 6)
        Me.mnuSep2.Visible = False
        '
        'mnuNew
        '
        Me.mnuNew.Image = Global.FDRTools.My.Resources.Resources._new
        Me.mnuNew.ImageTransparentColor = System.Drawing.Color.Silver
        Me.mnuNew.Name = "mnuNew"
        Me.mnuNew.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.mnuNew.Size = New System.Drawing.Size(173, 22)
        Me.mnuNew.Text = "&New row"
        Me.mnuNew.Visible = False
        '
        'mnuDelete
        '
        Me.mnuDelete.Image = Global.FDRTools.My.Resources.Resources.delete
        Me.mnuDelete.ImageTransparentColor = System.Drawing.Color.Silver
        Me.mnuDelete.Name = "mnuDelete"
        Me.mnuDelete.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Delete), System.Windows.Forms.Keys)
        Me.mnuDelete.Size = New System.Drawing.Size(173, 22)
        Me.mnuDelete.Text = "&Delete row"
        Me.mnuDelete.Visible = False
        '
        'mnuFunctions
        '
        Me.mnuFunctions.Name = "mnuFunctions"
        Me.mnuFunctions.Size = New System.Drawing.Size(65, 20)
        Me.mnuFunctions.Text = "&Functions"
        Me.mnuFunctions.Visible = False
        '
        'grdList
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.grdList.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.grdList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdList.Location = New System.Drawing.Point(0, 49)
        Me.grdList.Name = "grdList"
        Me.grdList.Size = New System.Drawing.Size(672, 652)
        Me.grdList.TabIndex = 2
        '
        'stsStatus
        '
        Me.stsStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblMsg, Me.lblRows})
        Me.stsStatus.Location = New System.Drawing.Point(0, 701)
        Me.stsStatus.Name = "stsStatus"
        Me.stsStatus.Size = New System.Drawing.Size(672, 22)
        Me.stsStatus.TabIndex = 3
        Me.stsStatus.Text = "StatusStrip1"
        '
        'lblMsg
        '
        Me.lblMsg.Name = "lblMsg"
        Me.lblMsg.Size = New System.Drawing.Size(647, 17)
        Me.lblMsg.Spring = True
        Me.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblRows
        '
        Me.lblRows.Name = "lblRows"
        Me.lblRows.Size = New System.Drawing.Size(10, 17)
        Me.lblRows.Text = " "
        '
        'tlsToolStrip
        '
        Me.tlsToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbRefresh, Me.cboWhere})
        Me.tlsToolStrip.Location = New System.Drawing.Point(0, 24)
        Me.tlsToolStrip.Name = "tlsToolStrip"
        Me.tlsToolStrip.Size = New System.Drawing.Size(672, 25)
        Me.tlsToolStrip.TabIndex = 1
        Me.tlsToolStrip.Text = "ToolStrip1"
        '
        'tsbRefresh
        '
        Me.tsbRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbRefresh.Image = Global.FDRTools.My.Resources.Resources.find
        Me.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Silver
        Me.tsbRefresh.Name = "tsbRefresh"
        Me.tsbRefresh.Size = New System.Drawing.Size(23, 22)
        Me.tsbRefresh.Text = "Refresh"
        '
        'cboWhere
        '
        Me.cboWhere.AutoSize = False
        Me.cboWhere.Name = "cboWhere"
        Me.cboWhere.Size = New System.Drawing.Size(121, 21)
        '
        'frmList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(672, 723)
        Me.Controls.Add(Me.grdList)
        Me.Controls.Add(Me.stsStatus)
        Me.Controls.Add(Me.tlsToolStrip)
        Me.Controls.Add(Me.mnuMain)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.mnuMain
        Me.MinimumSize = New System.Drawing.Size(300, 150)
        Me.Name = "frmList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "List"
        Me.mnuMain.ResumeLayout(False)
        Me.mnuMain.PerformLayout()
        CType(Me.grdList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.stsStatus.ResumeLayout(False)
        Me.stsStatus.PerformLayout()
        Me.tlsToolStrip.ResumeLayout(False)
        Me.tlsToolStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents mnuMain As System.Windows.Forms.MenuStrip
    Private WithEvents grdList As System.Windows.Forms.DataGridView
    Private WithEvents mnuList As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuRefresh As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuSep1 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents mnuClose As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuData As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuSave As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuCancel As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuSep2 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents mnuNew As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuDelete As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents stsStatus As System.Windows.Forms.StatusStrip
    Private WithEvents tsbRefresh As System.Windows.Forms.ToolStripButton
    Private WithEvents tlsToolStrip As System.Windows.Forms.ToolStrip
    Private WithEvents lblMsg As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents lblRows As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents cboWhere As System.Windows.Forms.ToolStripComboBox
    Private WithEvents mnuFunctions As System.Windows.Forms.ToolStripMenuItem
End Class
