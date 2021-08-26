Option Explicit On
Option Strict On


Public Class frmCheck

	Private mintFileCount As Integer

	Private Sub frmCheck_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		Try
			Select Case e.KeyCode
				Case System.Windows.Forms.Keys.Escape
					Me.Close()

				Case System.Windows.Forms.Keys.F5
					Me.LoadFiles()

			End Select
		Catch
		End Try
	End Sub

	Private Sub frmCheck_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			With cboFilter
				.Items.AddRange(App.GetSetting("CheckFilterItems", "*.*|*.JPG|IMG*.JPG|_MG*.JPG|?MG*.JPG").Split("|"c))
				.SelectedIndex = CInt(App.GetSetting("CheckFilterIndex", "1"))
			End With

			txtPath.Text = App.GetSetting("CheckPath", App.SourceRoot)
		Catch ex As System.Exception
			App.AddErr(ex)
		End Try
	End Sub

	Private Sub btnPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPath.Click
		Try
			With dlgFolder
				If Not IsES(txtPath.Text) AndAlso System.IO.Directory.Exists(txtPath.Text) Then
					.SelectedPath = txtPath.Text
				End If
				If .ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
					txtPath.Text = .SelectedPath
					If Not txtPath.Text.EndsWith(System.IO.Path.DirectorySeparatorChar) Then txtPath.Text &= System.IO.Path.DirectorySeparatorChar
					Me.LoadFiles()
				End If
			End With
		Catch ex As System.Exception
			App.AddErr(ex)
		End Try
	End Sub

	Private Sub cboFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFilter.SelectedIndexChanged
		Me.LoadFiles()
	End Sub

	Private Sub LoadFiles()
		Try
			'ListBox elemeinek törlése:
			lblCount.Text = Nothing
			lstFaulty.Items.Clear()
		Catch ex As System.Exception
			App.AddErr(ex)
		End Try
	End Sub

	Private Sub CheckFiles(ByVal tstrDir As String, ByRef tintCount As Integer, ByRef tintFaulty As Integer)
		Dim lstraFiles() As String = System.IO.Directory.GetFiles(tstrDir, cboFilter.Text)
		For Each lstrFile As String In lstraFiles
			tintCount += 1

			Try
				Using lobjBitmap As New System.Drawing.Bitmap(lstrFile)
				End Using
			Catch
				tintFaulty += 1
				lstFaulty.Items.Add(lstrFile)
				lstFaulty.Refresh()
			End Try

			lblCount.Text = tintFaulty & " / " & tintCount
			lblCount.Refresh()
			System.Windows.Forms.Application.DoEvents()
		Next

		Dim lstraDirs() As String = System.IO.Directory.GetDirectories(tstrDir)
		For Each lstrDir As String In lstraDirs
			Me.CheckFiles(lstrDir, tintCount, tintFaulty)
		Next
	End Sub

	Private Sub btnCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheck.Click
		Try
			Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

			lstFaulty.Items.Clear()

			'Ha az útvonal üres, kilép:
			If IsES(txtPath.Text) Then Exit Sub

			Dim lintCount As Integer
			Dim lintFaulty As Integer

			Me.CheckFiles(txtPath.Text, lintCount, lintFaulty)

			lblCount.Text = lintFaulty & " / " & lintCount

			System.Windows.Forms.MessageBox.Show("Finished...", "OK", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
		Catch ex As System.Exception
			App.AddErr(ex)
			System.Windows.Forms.MessageBox.Show("Checking files failed!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation)
		Finally
			Me.Cursor = System.Windows.Forms.Cursors.Default
		End Try
	End Sub

	Private Sub btnGenerate_Click(sender As System.Object, e As System.EventArgs) Handles btnGenerate.Click
		Try
			Dim lstrFiles As String = Nothing
			For I As Integer = 0 To lstFaulty.Items.Count - 1
				Dim lstrFile As String = CStr(lstFaulty.Items(I))
				lstrFiles &= lstrFile & System.Environment.NewLine
			Next
			System.Windows.Forms.Clipboard.SetText(lstrFiles)
		Catch ex As System.Exception
			App.AddErr(ex)
		End Try
	End Sub

End Class
