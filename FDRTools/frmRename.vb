Option Explicit On
Option Strict On

Imports System
Imports System.Windows.Forms
'TODO: VB!!!
Imports Microsoft.VisualBasic

Public Class frmRename

	Private mintFileCount As Integer

	Private mintcNoChange As Integer = 1
	Private mintcLowerCase As Integer = 2
	Private mintcUpperCase As Integer = 3

	Private Sub frmRename_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		Try
			Select Case e.KeyCode
				Case Keys.Escape
					Me.Close()

				Case Keys.F5
					Me.LoadFiles()

			End Select
		Catch
		End Try
	End Sub

	Private Sub frmRename_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			With cboFilter
				.Items.AddRange(App.GetSetting("RenameFilterItems", "*.*|*.CRW|CRW*.CRW|CRW*.THM|*.CR2|IMG*.CR2|_MG*.CR2|?MG*.CR2|*.JPG|IMG*.JPG|_MG*.JPG|?MG*.JPG").Split("|"c))
				.SelectedIndex = CInt(App.GetSetting("RenameFilterIndex", "7"))
			End With
			With cboSecond
				.Items.AddRange(App.GetSetting("RenameSecondItems", ".JPG|.THM").Split("|"c))
				.SelectedIndex = CInt(App.GetSetting("RenameSecondIndex", "0"))
			End With
			With cboThird
				.Items.AddRange(App.GetSetting("RenameThirdItems", ".JPG|.THM").Split("|"c))
				.SelectedIndex = CInt(App.GetSetting("RenameThirdIndex", "1"))
			End With
			txtDigits.Text = "1"

			Dim lcolItems As System.Collections.ArrayList

			lcolItems = New System.Collections.ArrayList
			With lcolItems
				.Add(New ListItem(mintcNoChange, "NoChange"))
				.Add(New ListItem(mintcLowerCase, "lowercase"))
				.Add(New ListItem(mintcUpperCase, "UPPERCASE"))
			End With
			FillCombo(Me.cboFileNameCase, lcolItems)
			Me.cboFileNameCase.SelectedIndex = CInt(App.GetSetting("RenameFileCaseIndex", "0"))

			lcolItems = New System.Collections.ArrayList
			With lcolItems
				.Add(New ListItem(mintcNoChange, "NoChange"))
				.Add(New ListItem(mintcLowerCase, "lowercase"))
				.Add(New ListItem(mintcUpperCase, "UPPERCASE"))
			End With
			FillCombo(Me.cboExtensionCase, lcolItems)
			Me.cboExtensionCase.SelectedIndex = CInt(App.GetSetting("RenameExtCaseIndex", "0"))

			txtPath.Text = App.GetSetting("RenamePath", App.SourceRoot)
			chkSecond.Checked = IsTrue(App.GetSetting("RenameSecond", "False"))
			chkThird.Checked = IsTrue(App.GetSetting("RenameThird", "False"))
			txtPrefix.Text = App.GetSetting("RenamePrefix", Format(Date.Now, "yyMMdd_"))
			chkAutoPrefix.Checked = IsTrue(App.GetSetting("RenameAutoPrefix", "True"))
			txtPostfix.Text = App.GetSetting("RenamePostfix")

			Me.Gombok()
		Catch ex As Exception
			App.AddErr(ex)
		End Try
	End Sub

	Private Sub btnPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPath.Click
		Try
			With dlgFolder
				If Not IsES(txtPath.Text) AndAlso IO.Directory.Exists(txtPath.Text) Then
					.SelectedPath = txtPath.Text
				End If
				If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
					txtPath.Text = .SelectedPath
					If Not txtPath.Text.EndsWith(IO.Path.DirectorySeparatorChar) Then txtPath.Text &= IO.Path.DirectorySeparatorChar
					Me.LoadFiles()
				End If
			End With
		Catch ex As Exception
			App.AddErr(ex)
		End Try
	End Sub

	Private Sub cboFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFilter.SelectedIndexChanged
		Me.LoadFiles()
	End Sub

	Private Sub LoadFiles()
		Dim lstraFiles() As String
		Try
			'Homokóra:
			Me.Cursor = Cursors.WaitCursor

			'ListBox elemeinek törlése:
			lstFiles.Items.Clear()
			lblCount.Text = Nothing

			'Ha az útvonal üres, kilép:
			If IsES(txtPath.Text) Then Exit Sub

			lstraFiles = IO.Directory.GetFiles(txtPath.Text, cboFilter.Text)
			For Each lstrFile As String In lstraFiles
				lstFiles.Items.Add(IO.Path.GetFileName(lstrFile))
			Next
			mintFileCount = lstraFiles.Length

			If mintFileCount = 0 Then
				lblCount.Text = "No such files..."
			ElseIf mintFileCount = 1 Then
				lblCount.Text = "1 file"
			Else
				lblCount.Text = CStr(mintFileCount) & " files"
			End If
			Try
				If CStr(mintFileCount).Length > CInt(txtDigits.Text) Then
					txtDigits.Text = CStr(CStr(mintFileCount).Length)
				End If
			Catch
			End Try

			'Elsõ elem kiválasztása:
			'lstFiles.SelectedIndex = 0

			Me.FormatSample()
			Me.Gombok()

			'Index visszaállítása:
			txtFirst.Text = "1"
			'Prefix beállítása, ha szükséges:
			If chkAutoPrefix.Checked Then
				Dim lstrDir As String
				Dim lstraDir() As String = txtPath.Text.TrimEnd(IO.Path.DirectorySeparatorChar).Split(IO.Path.DirectorySeparatorChar)
				lstrDir = lstraDir(lstraDir.Length - 1)
				If lstrDir.ToUpper = "RAW" Then
					lstrDir = lstraDir(lstraDir.Length - 2)
				End If
				If lstrDir Like "########*" Then
					txtPrefix.Text = lstrDir.Substring(2, 6) & "_"
				ElseIf lstrDir Like "######*" Then
					txtPrefix.Text = lstrDir.Substring(0, 6) & "_"
				End If
			End If
		Catch ex As Exception
			App.AddErr(ex)
		Finally
			Me.Cursor = Cursors.Default
		End Try
	End Sub

	Private Sub Additional_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
	 Handles chkSecond.CheckedChanged, chkThird.CheckedChanged, chkAutoPrefix.CheckedChanged
		Me.Gombok()
	End Sub

	Private Sub txtDigits_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDigits.TextChanged
		Try
			txtDigits.Text = CStr(CInt(txtDigits.Text))
			If CStr(mintFileCount).Length > CInt(txtDigits.Text) Then
				txtDigits.Text = CStr(CStr(mintFileCount).Length)
			End If
			Me.FormatSample()
		Catch
		End Try
	End Sub

	Private Sub txtFirst_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFirst.TextChanged
		Try
			txtFirst.Text = CStr(CInt(txtFirst.Text))
			If CInt(txtFirst.Text) < 1 Then txtFirst.Text = "1"
			Me.FormatSample()
		Catch
		End Try
	End Sub

	Private Sub FormattingSample(ByVal sender As Object, ByVal e As System.EventArgs) _
	 Handles txtPrefix.TextChanged, txtPostfix.TextChanged, cboFilter.TextChanged, cboFileNameCase.SelectedIndexChanged, cboExtensionCase.SelectedIndexChanged
		Me.FormatSample()
	End Sub

	Private Sub FormatSample()
		Try
			'Kiterjesztés:
			Dim lstrExt As String
			If Me.lstFiles.Items.Count > 0 Then
				lstrExt = IO.Path.GetExtension(CStr(Me.lstFiles.Items(0)))
			Else
				lstrExt = IO.Path.GetExtension(Me.cboFilter.Text)
			End If
			If Me.cboExtensionCase.SelectedIndex >= 0 Then
				Select Case CInt(Me.cboExtensionCase.SelectedValue)
					Case mintcLowerCase	'all lowercase
						lstrExt = ES(lstrExt).ToLower
					Case mintcUpperCase	'ALL UPPERCASE
						lstrExt = ES(lstrExt).ToUpper
				End Select
			End If

			'Fájlnév:
			Dim lstrFile As String = txtPrefix.Text & CInt(txtFirst.Text).ToString(New String("0"c, CInt(txtDigits.Text))) & txtPostfix.Text
			If Me.cboFileNameCase.SelectedIndex >= 0 Then
				Select Case CInt(Me.cboFileNameCase.SelectedValue)
					Case mintcLowerCase	'all lowercase
						lstrFile = ES(lstrFile).ToLower
					Case mintcUpperCase	'ALL UPPERCASE
						lstrFile = ES(lstrFile).ToUpper
				End Select
			End If

			lblSample.Text = lstrFile & lstrExt
		Catch
		End Try
	End Sub

	Private Sub btnRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRename.Click
		Dim lstrFile As String
		Dim lstrExt As String
		Dim lstrOldName As String
		Dim lstrNewName As String
		Dim lintIndex As Integer
		Dim lintFileNameCase As Integer
		Dim lintExtensionCase As Integer
		Try
			If lstFiles.Items.Count = 0 OrElse IsES(txtPath.Text) Then Exit Sub
			Me.Cursor = Cursors.WaitCursor

			If Me.cboFileNameCase.SelectedIndex >= 0 Then
				lintFileNameCase = CInt(Me.cboFileNameCase.SelectedValue)
			End If
			If Me.cboExtensionCase.SelectedIndex >= 0 Then
				lintExtensionCase = CInt(Me.cboExtensionCase.SelectedValue)
			End If

			lintIndex = CInt(txtFirst.Text)
			For J As Integer = 0 To lstFiles.Items.Count - 1
				lstrFile = CStr(lstFiles.Items(J))
				lstrOldName = IO.Path.GetFileNameWithoutExtension(lstrFile)

				lstrNewName = txtPrefix.Text & lintIndex.ToString(New String("0"c, CInt(txtDigits.Text))) & txtPostfix.Text
				Select Case lintFileNameCase
					Case mintcLowerCase	'all lowercase
						lstrNewName = ES(lstrNewName).ToLower
					Case mintcUpperCase	'ALL UPPERCASE
						lstrNewName = ES(lstrNewName).ToUpper
				End Select

				lstrExt = IO.Path.GetExtension(lstrFile)
				Select Case lintExtensionCase
					Case mintcLowerCase	'all lowercase
						lstrExt = ES(lstrExt).ToLower
					Case mintcUpperCase	'ALL UPPERCASE
						lstrExt = ES(lstrExt).ToUpper
				End Select

				IO.File.Move(IO.Path.Combine(txtPath.Text, lstrFile), IO.Path.Combine(txtPath.Text, lstrNewName & lstrExt))

				lstrExt = cboSecond.Text
				If chkSecond.Checked AndAlso IO.File.Exists(IO.Path.Combine(txtPath.Text, lstrOldName & lstrExt)) Then
					Select Case lintExtensionCase
						Case mintcLowerCase	'all lowercase
							lstrExt = ES(lstrExt).ToLower
						Case mintcUpperCase	'ALL UPPERCASE
							lstrExt = ES(lstrExt).ToUpper
					End Select
					IO.File.Move(IO.Path.Combine(txtPath.Text, lstrOldName & cboSecond.Text), IO.Path.Combine(txtPath.Text, lstrNewName & lstrExt))
				End If
				lstrExt = cboThird.Text
				If chkThird.Checked AndAlso IO.File.Exists(IO.Path.Combine(txtPath.Text, lstrOldName & lstrExt)) Then
					Select Case lintExtensionCase
						Case mintcLowerCase	'all lowercase
							lstrExt = ES(lstrExt).ToLower
						Case mintcUpperCase	'ALL UPPERCASE
							lstrExt = ES(lstrExt).ToUpper
					End Select
					IO.File.Move(IO.Path.Combine(txtPath.Text, lstrOldName & cboThird.Text), IO.Path.Combine(txtPath.Text, lstrNewName & lstrExt))
				End If

				lintIndex += 1
				txtFirst.Text = CStr(lintIndex)
			Next J

			MessageBox.Show("Successfully finished...", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Me.LoadFiles()
		Catch ex As Exception
			App.AddErr(ex)
			MessageBox.Show("Renaming files failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
		Finally
			Me.Cursor = Cursors.Default
		End Try
	End Sub

	Private Sub Gombok()
		Try
			lblSecond.Visible = chkSecond.Checked
			cboSecond.Visible = chkSecond.Checked
			lblThird.Visible = chkThird.Checked
			cboThird.Visible = chkThird.Checked

			btnRename.Enabled = lstFiles.Items.Count > 0
		Catch
		End Try
	End Sub

End Class