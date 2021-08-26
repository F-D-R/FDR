Option Explicit On
Option Strict On

Imports System
Imports System.Windows.Forms
'TODO: VB!!!
Imports Microsoft.VisualBasic

Public Class frmSQL

	Private mintItemPath As Integer
	Private mintItemPictSize As Integer
	Private mintItemThumbSize As Integer
	Private mintItemCaptured As Integer
	Private mintItemOwner As Integer
	Private mintItemLocationHU As Integer
	Private mintItemLocationEN As Integer

	Private Sub frmSQL_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		Try
			Select Case e.KeyCode
				Case Keys.Escape
					Me.Close()
				Case Keys.L
					If e.Control Then
						btnLoadPictDirs.Focus()
						btnLoadPictDirs.PerformClick()
					End If
				Case Keys.A
					If e.Control Then
						btnSelectAll.Focus()
						btnSelectAll.PerformClick()
					End If
				Case Keys.D
					If e.Control Then
						btnDeselect.Focus()
						btnDeselect.PerformClick()
					End If
				Case Keys.I
					If e.Control Then
						btnInverse.Focus()
						btnInverse.PerformClick()
					End If
			End Select
		Catch
		End Try
	End Sub

	Private Sub frmSQL_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Try
			With cboFilter
				.Items.Add("*.*")
				.Items.Add("*.jpg")
				.Items.Add("*.gif")
				.Items.Add("*.png")
				.SelectedIndex = 1
			End With
			With cboOwner
				.Items.Add("fdr")
				.Items.Add("mse")
				.SelectedIndex = 0
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

			Me.Gombok()
		Catch ex As Exception
			App.AddErr(ex)
		End Try
	End Sub

	Private Sub Gombok()
		Const lstrcInvDir As String = "Invalid directory!"
		'Const lstrcInvDate As String = "Invalid date!"
		Try
			Dim llogInsert As Boolean = optInsert.Checked
			Dim llogUpdate As Boolean = optUpdate.Checked

			ErrorProvider1.SetError(txtRoot, CStr(IIf(Not IsES(txtRoot.Text) AndAlso IO.Directory.Exists(txtRoot.Text), Nothing, lstrcInvDir)))
			ErrorProvider1.SetError(txtPictDir, CStr(IIf(Not IsES(txtRoot.Text) AndAlso IO.Directory.Exists(IO.Path.Combine(txtRoot.Text, txtPictDir.Text)), Nothing, lstrcInvDir)))
			ErrorProvider1.SetError(txtThumbDir, CStr(IIf(Not IsES(txtRoot.Text) AndAlso IO.Directory.Exists(IO.Path.Combine(txtRoot.Text, txtThumbDir.Text)), Nothing, lstrcInvDir)))
			ErrorProvider1.SetError(txtSubDir, CStr(IIf(Not IsES(txtRoot.Text) AndAlso IO.Directory.Exists(IO.Path.Combine(IO.Path.Combine(txtRoot.Text, txtPictDir.Text), txtSubDir.Text)), Nothing, lstrcInvDir)))

			btnGenInsert.Enabled = llogInsert AndAlso lstDirectories.SelectedItems.Count > 0
			btnGenUpdate.Enabled = llogUpdate AndAlso lstDirectories.SelectedItems.Count > 0
			btnWebResize.Enabled = lstDirectories.SelectedItems.Count > 0
			'btnCheckDuplicates.Enabled = lstDirectories.SelectedItems.Count = 1
			lblFields.Visible = llogUpdate
			lstFields.Visible = llogUpdate
		Catch
		End Try
	End Sub

	Private Sub Frissites(ByVal sender As System.Object, ByVal e As System.EventArgs) _
	 Handles txtRoot.Validated, txtPictDir.Validated, txtThumbDir.Validated, _
	 lstDirectories.SelectedIndexChanged, cboFilter.SelectedIndexChanged, optInsert.CheckedChanged, optUpdate.CheckedChanged
		Me.Gombok()
	End Sub

	Private Sub lstFields_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstFields.Leave
		Try
			'Törli a mezõkiválasztásokat:
			lstFields.SelectedItems.Clear()
		Catch
		End Try
	End Sub

	Private Sub btnRoot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRoot.Click
		Try

			'itt kéne valami könyvtár kiválasztás...

			'Törli a könyvtárak listáját:
			lstDirectories.Items.Clear()
			Me.Gombok()
		Catch ex As Exception
			App.AddErr(ex)
		End Try
	End Sub

	Private Sub btnSubDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubDir.Click
		Try
			Dim lstrFolder As String = Nothing
			Dim lstrRoot As String = IO.Path.Combine(txtRoot.Text, txtPictDir.Text)
			If Not IO.Directory.Exists(lstrRoot) Then Exit Sub
			Using ldlgFolder As New FolderBrowserDialog
				ldlgFolder.ShowNewFolderButton = False
				If IO.Directory.Exists(IO.Path.Combine(lstrRoot, txtSubDir.Text)) Then
					ldlgFolder.SelectedPath = IO.Path.Combine(lstrRoot, txtSubDir.Text)
				Else
					ldlgFolder.SelectedPath = lstrRoot
				End If
				If Not ldlgFolder.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then Exit Sub
				lstrFolder = ldlgFolder.SelectedPath
			End Using
			If Not lstrFolder.StartsWith(lstrRoot) Then Exit Sub
			txtSubDir.Text = lstrFolder.Substring(lstrRoot.Length + 1)
			'Törli a könyvtárak listáját:
			lstDirectories.Items.Clear()
			Me.Gombok()
		Catch ex As Exception
			App.AddErr(ex)
		End Try
	End Sub

	Private Sub btnLoadPictDirs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadPictDirs.Click
		Dim lstraDirs() As String
		Try
			Me.Cursor = Cursors.WaitCursor
			Me.SuspendLayout()

			'ListBox elemeinek törlése:
			lstDirectories.Items.Clear()

			'Ha az útvonal üres, kilép:
			If IsES(txtRoot.Text) OrElse IsES(txtPictDir.Text) Then Exit Sub
			Dim lstrFolder As String = IO.Path.Combine(IO.Path.Combine(txtRoot.Text, txtPictDir.Text), txtSubDir.Text)
			'Ha az útvonal nem létezik, kilép:
			If Not IO.Directory.Exists(lstrFolder) Then Exit Sub

			'Könyvtárak betöltése:
			lstraDirs = IO.Directory.GetDirectories(lstrFolder)
			For Each lstrDir As String In lstraDirs
				lstrDir = IO.Path.GetFileName(lstrDir)
				lstDirectories.Items.Add(lstrDir)
			Next

			Me.Gombok()
		Catch ex As Exception
			App.AddErr(ex)
		Finally
			Me.Cursor = Cursors.Default
			Me.ResumeLayout()
		End Try
	End Sub

	Private Sub txtCaptured_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtCaptured.Validating, txtLocationEN.Validating, txtLocationHU.Validating
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

	Private Sub btnGenInsert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenInsert.Click
		Dim lstrSQL As String
		Dim ldatNow As Date
		Dim lstrTmp As String
		Dim lstraAlbums() As String
		Dim lstrDir As String
		Dim lstraFiles() As String
		Dim lobjImage As System.Drawing.Image = Nothing
		Dim lstrScript As String = Nothing
		Dim lstrError As String = Nothing
		Try
			Me.Cursor = Cursors.WaitCursor

			ldatNow = Date.Now
			lstraAlbums = Split(txtAlbums.Text, ",")

			IO.File.Delete(IO.Path.Combine(txtRoot.Text, "insert.log"))
			IO.File.Delete(IO.Path.Combine(txtRoot.Text, "insert.sql"))

			'Progressbar:
			barAll.Value = 0
			barAll.Maximum = lstDirectories.SelectedItems.Count

			For I As Integer = 0 To lstDirectories.SelectedItems.Count - 1
				lstrDir = CStr(lstDirectories.SelectedItems(I))
				lstraFiles = IO.Directory.GetFiles(IO.Path.Combine(IO.Path.Combine(IO.Path.Combine(txtRoot.Text, txtPictDir.Text), txtSubDir.Text), lstrDir), cboFilter.Text)

				'Progressbar:
				barCurrent.Value = 0
				barCurrent.Maximum = lstraFiles.Length

				Dim lintIndex As Integer = 0
				For Each lstrFile As String In lstraFiles
					Dim lstrID As String
					Dim lintPWidth As Integer
					Dim lintPHeight As Integer
					Dim lstrThumbFile As String
					Dim lintTWidth As Integer
					Dim lintTHeight As Integer

					lstrID = IO.Path.GetFileNameWithoutExtension(lstrFile)

					'Kép méretének kiolvasása:
					Try
						lobjImage = System.Drawing.Image.FromFile(lstrFile, False)
						With lobjImage
							lintPWidth = .Width
							lintPHeight = .Height
						End With
					Catch
						lstrError &= "Picture: " & lstrFile & vbCrLf
					Finally
						If lobjImage IsNot Nothing Then lobjImage.Dispose()
						lobjImage = Nothing
					End Try

					'Thumbnail fájl útvonala:
					lintTWidth = 0
					lintTHeight = 0
					lstrThumbFile = IO.Path.Combine(IO.Path.Combine(IO.Path.Combine(IO.Path.Combine(txtRoot.Text, txtThumbDir.Text), txtSubDir.Text), lstrDir), IO.Path.GetFileName(lstrFile))
					If IO.File.Exists(lstrThumbFile) Then
						'Thumbnail méretének kiolvasása, ha létezik:
						Try
							lobjImage = System.Drawing.Image.FromFile(lstrThumbFile, False)
							With lobjImage
								lintTWidth = .Width
								lintTHeight = .Height
							End With
						Catch
							lstrError &= "Thumbnail: " & lstrThumbFile & vbCrLf
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
					If IsES(txtSubDir.Text) Then
						lstrSQL &= "'" & lstrDir & "/" & IO.Path.GetFileName(lstrFile).ToLower & "', "
					Else
						lstrSQL &= "'" & txtSubDir.Text & "/" & lstrDir & "/" & IO.Path.GetFileName(lstrFile).ToLower & "', "
					End If
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
						lstrTmp &= ES(lstrID).Substring(0, 2) & "-" & Mid(lstrID, 3, 2) & "-" & Mid(lstrID, 5, 2)
						lstrSQL &= "'" & lstrTmp & "', "
					Else
						lstrSQL &= "NULL, "
					End If
					lstrSQL &= "'" & Format(ldatNow, "yyyy-MM-dd HH\:mm\:ss") & "', "
					lstrSQL &= CStr(lintPWidth) & ", "
					lstrSQL &= CStr(lintPHeight) & ", "
					lstrSQL &= CStr(lintTWidth) & ", "
					lstrSQL &= CStr(lintTHeight) & ");"

					lstrScript &= lstrSQL & vbCrLf

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

							lstrScript &= lstrSQL & vbCrLf
						End If
					Next
					lintIndex += 1

					'Progressbar léptetése:
					barCurrent.Value += 1
				Next
				'Progressbar léptetése:
				barAll.Value += 1
			Next

			If Not IsES(lstrScript) Then
				Using lobjWriter As New IO.StreamWriter(IO.Path.Combine(txtRoot.Text, "insert.sql"))
					lobjWriter.WriteLine(lstrScript)
					lobjWriter.Close()
				End Using
			End If
			If Not IsES(lstrError) Then
				Using lobjWriter As New IO.StreamWriter(IO.Path.Combine(txtRoot.Text, "insert.log"))
					lobjWriter.WriteLine(lstrError)
					lobjWriter.Close()
				End Using
			End If

			If IsES(lstrError) Then
				MessageBox.Show(Me, "Successfully finished...", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Else
				MessageBox.Show(Me, "Finished with errors!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
			End If
			Me.Gombok()
		Catch ex As Exception
			App.AddErr(ex)
			MessageBox.Show(Me, "Generating SQL failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		Finally
			Me.Cursor = Cursors.Default
			'Progressbar törlése:
			barAll.Value = 0
			barCurrent.Value = 0
		End Try
	End Sub

	Private Sub btnGenUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenUpdate.Click
		Dim lstrSQL As String
		Dim lstrDir As String
		Dim lstraFiles() As String
		Dim lobjImage As System.Drawing.Image = Nothing
		Dim lstrScript As String = Nothing
		Dim lstrError As String = Nothing
		Try
			Me.Cursor = Cursors.WaitCursor

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

			IO.File.Delete(IO.Path.Combine(txtRoot.Text, "update.log"))
			IO.File.Delete(IO.Path.Combine(txtRoot.Text, "update.sql"))

			'Progressbar:
			barAll.Value = 0
			barAll.Maximum = lstDirectories.SelectedItems.Count

			For I As Integer = 0 To lstDirectories.SelectedItems.Count - 1
				lstrDir = CStr(lstDirectories.SelectedItems(I))
				lstraFiles = IO.Directory.GetFiles(IO.Path.Combine(IO.Path.Combine(IO.Path.Combine(txtRoot.Text, txtPictDir.Text), txtSubDir.Text), lstrDir), cboFilter.Text)

				'Progressbar:
				barCurrent.Value = 0
				barCurrent.Maximum = lstraFiles.Length

				For Each lstrFile As String In lstraFiles
					Dim lstrID As String
					Dim lintPWidth As Integer
					Dim lintPHeight As Integer
					Dim lstrThumbFile As String
					Dim lintTWidth As Integer
					Dim lintTHeight As Integer

					lstrSQL = Nothing
					lstrID = IO.Path.GetFileNameWithoutExtension(lstrFile)

					If llogPictSize Then
						'Kép méretének kiolvasása:
						Try
							lobjImage = System.Drawing.Image.FromFile(lstrFile, False)
							With lobjImage
								lintPWidth = .Width
								lintPHeight = .Height
							End With

							If Not IsES(lstrSQL) Then lstrSQL &= ", "
							lstrSQL &= "pwidth=" & CStr(lintPWidth)
							If Not IsES(lstrSQL) Then lstrSQL &= ", "
							lstrSQL &= "pheight=" & CStr(lintPHeight)
						Catch
							lstrError &= "Picture: " & lstrFile & vbCrLf
						Finally
							If lobjImage IsNot Nothing Then lobjImage.Dispose()
							lobjImage = Nothing
						End Try
					End If

					If llogThumbSize Then
						lintTWidth = 0
						lintTHeight = 0

						'Thumbnail fájl útvonala:
						lstrThumbFile = IO.Path.Combine(IO.Path.Combine(IO.Path.Combine(IO.Path.Combine(txtRoot.Text, txtThumbDir.Text), txtSubDir.Text), lstrDir), IO.Path.GetFileName(lstrFile))
						If IO.File.Exists(lstrThumbFile) Then
							'Thumbnail méretének kiolvasása, ha létezik:
							Try
								lobjImage = System.Drawing.Image.FromFile(lstrThumbFile, False)
								With lobjImage
									lintTWidth = .Width
									lintTHeight = .Height
								End With
							Catch
								lstrError &= "Thumbnail: " & lstrThumbFile & vbCrLf
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

						If Not IsES(lstrSQL) Then lstrSQL &= ", "
						lstrSQL &= "twidth=" & CStr(lintTWidth)
						If Not IsES(lstrSQL) Then lstrSQL &= ", "
						lstrSQL &= "theight=" & CStr(lintTHeight)
					End If

					If llogPath Then
						If Not IsES(lstrSQL) Then lstrSQL &= ", "
						If IsES(txtSubDir.Text) Then
							lstrSQL &= "pictfile='" & lstrDir & "/" & IO.Path.GetFileName(lstrFile) & "'"
						Else
							lstrSQL &= "pictfile='" & txtSubDir.Text & "/" & lstrDir & "/" & IO.Path.GetFileName(lstrFile) & "'"
						End If
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
					lstrScript &= lstrSQL & vbCrLf

					'Progressbar léptetése:
					barCurrent.Value += 1
				Next
				'Progressbar léptetése:
				barAll.Value += 1
			Next

			If Not IsES(lstrScript) Then
				Using lobjWriter As New IO.StreamWriter(IO.Path.Combine(txtRoot.Text, "update.sql"))
					lobjWriter.WriteLine(lstrScript)
					lobjWriter.Close()
				End Using
			End If
			If Not IsES(lstrError) Then
				Using lobjWriter As New IO.StreamWriter(IO.Path.Combine(txtRoot.Text, "update.log"))
					lobjWriter.WriteLine(lstrError)
					lobjWriter.Close()
				End Using
			End If

			If IsES(lstrError) Then
				MessageBox.Show(Me, "Successfully finished...", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Else
				MessageBox.Show(Me, "Finished with errors!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
			End If
			Me.Gombok()
		Catch ex As Exception
			App.AddErr(ex)
			MessageBox.Show(Me, "Generating SQL failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		Finally
			Me.Cursor = Cursors.Default
			'Progressbar törlése:
			barAll.Value = 0
			barCurrent.Value = 0
		End Try
	End Sub

	Private Sub btnWebResize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWebResize.Click
		Dim lstraAlbums() As String
		Dim lstrDir As String
		Dim lstrPath As String
		'Dim lstraFiles() As String
		Dim lstrError As String = Nothing
		Try
			Me.Cursor = Cursors.WaitCursor

			'ldatNow = Date.Now
			lstraAlbums = Split(txtAlbums.Text, ",")

			IO.File.Delete(IO.Path.Combine(txtRoot.Text, "webresize.log"))

			'Progressbar:
			barAll.Value = 0
			barAll.Maximum = lstDirectories.SelectedItems.Count
			'Progressbar:
			barCurrent.Value = 0
			barCurrent.Maximum = lstDirectories.SelectedItems.Count

			For I As Integer = 0 To lstDirectories.SelectedItems.Count - 1
				lstrDir = CStr(lstDirectories.SelectedItems(I))
				lstrPath = IO.Path.Combine(IO.Path.Combine(IO.Path.Combine(txtRoot.Text, txtPictDir.Text), txtSubDir.Text), lstrDir)


				''Átméretezés:
				'WebResize(lstrPath)

				''A kész fájlok végleges helyükre mozgatása:
				'lstraFiles = IO.Directory.GetFiles("d:\temp\pictures", "*.jpg")
				'For Each lstrFile As String In lstraFiles
				'    IO.File.Move(lstrFile, "")
				'Next


				'Progressbar léptetése:
				barCurrent.Value += 1
				'Progressbar léptetése:
				barAll.Value += 1
			Next

			If Not IsES(lstrError) Then
				Using lobjWriter As New IO.StreamWriter(IO.Path.Combine(txtRoot.Text, "webresize.log"))
					lobjWriter.WriteLine(lstrError)
					lobjWriter.Close()
				End Using
			End If

			If IsES(lstrError) Then
				MessageBox.Show(Me, "Successfully finished...", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Else
				MessageBox.Show(Me, "Finished with errors!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
			End If
			Me.Gombok()
		Catch ex As Exception
			App.AddErr(ex)
			MessageBox.Show(Me, "Resizing for the web failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		Finally
			Me.Cursor = Cursors.Default
			'Progressbar törlése:
			barAll.Value = 0
			barCurrent.Value = 0
		End Try
	End Sub

	Private Sub btnSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click
		Try
			For I As Integer = 0 To lstDirectories.Items.Count - 1
				lstDirectories.SetSelected(I, True)
			Next
			Me.Gombok()
		Catch
		End Try
	End Sub

	Private Sub btnInverse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInverse.Click
		Try
			For I As Integer = 0 To lstDirectories.Items.Count - 1
				lstDirectories.SetSelected(I, Not lstDirectories.GetSelected(I))
			Next
			Me.Gombok()
		Catch
		End Try
	End Sub

	Private Sub btnDeselect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeselect.Click
		Try
			lstDirectories.SelectedItems.Clear()
			Me.Gombok()
		Catch
		End Try
	End Sub

	Private Function CheckDuplicates(ByVal tstrRoot As String, ByVal tstrDir As String, Optional ByVal tstrFilter As String = "*.*") As Boolean
		Dim lstraFiles() As String
		Dim lstrFile As String
		Dim lstrFiles As String = String.Empty
		Dim lstrDuplicates As String = Nothing
		Try
			Me.Cursor = Cursors.WaitCursor

			IO.File.Delete(IO.Path.Combine(txtRoot.Text, "duplicates.log"))

			'Progressbar:
			barAll.Value = 0
			barAll.Maximum = 3
			barCurrent.Value = 0

			If Not IO.Directory.Exists(IO.Path.Combine(tstrRoot, tstrDir)) Then
				App.AddErr(Nothing, "Invalid directory: " & tstrRoot)
				Return False
			End If

			lstraFiles = IO.Directory.GetFiles(IO.Path.Combine(tstrRoot, tstrDir), tstrFilter, IO.SearchOption.AllDirectories)
			barAll.Value = 1

			'Progressbar:
			barCurrent.Value = 0
			barCurrent.Maximum = lstraFiles.Length

			For Each lstrPath As String In lstraFiles
				lstrFile = IO.Path.GetFileName(lstrPath).ToLower

				If lstrFiles.IndexOf(lstrFile) >= 0 Then
					lstrDuplicates &= lstrPath & vbCrLf
				End If

				lstrFiles &= lstrFile & vbCrLf

				'Progressbar léptetése:
				barCurrent.Value += 1
			Next
			barAll.Value = 2

			If Not IsES(lstrDuplicates) Then
				Using lobjWriter As New IO.StreamWriter(IO.Path.Combine(txtRoot.Text, "duplicates.log"))
					lobjWriter.WriteLine(lstrDuplicates)
					lobjWriter.Close()
				End Using
			End If
			barAll.Value = 3

			If IsES(lstrDuplicates) Then
				MessageBox.Show(Me, "No duplicate files...", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Else
				MessageBox.Show(Me, "There are duplicate files!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
			End If

			Me.Gombok()

			Return True
		Catch ex As Exception
			App.AddErr(ex)
			MessageBox.Show(Me, "Checking duplicates failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		Finally
			Me.Cursor = Cursors.Default
			'Progressbar törlése:
			barAll.Value = 0
			barCurrent.Value = 0
		End Try
	End Function

	Private Sub btnCheckDuplicates_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckDuplicates.Click
		Try
			'If lstDirectories.SelectedItems.Count <> 1 Then Exit Sub
			'Me.CheckDuplicates(txtRoot.Text, CStr(lstDirectories.SelectedValue), cboFilter.Text)

			IO.File.Delete(IO.Path.Combine(txtRoot.Text, "duplicate_pictures.log"))
			IO.File.Delete(IO.Path.Combine(txtRoot.Text, "duplicate_thumbnails.log"))

			Me.CheckDuplicates(txtRoot.Text, txtPictDir.Text, cboFilter.Text)
			If IO.File.Exists(IO.Path.Combine(txtRoot.Text, "duplicates.log")) Then
				IO.File.Move(IO.Path.Combine(txtRoot.Text, "duplicates.log"), IO.Path.Combine(txtRoot.Text, "duplicate_pictures.log"))
			End If
			Me.CheckDuplicates(txtRoot.Text, txtThumbDir.Text, cboFilter.Text)
			If IO.File.Exists(IO.Path.Combine(txtRoot.Text, "duplicates.log")) Then
				IO.File.Move(IO.Path.Combine(txtRoot.Text, "duplicates.log"), IO.Path.Combine(txtRoot.Text, "duplicate_thumbnails.log"))
			End If
		Catch ex As Exception
			App.AddErr(ex)
		End Try
	End Sub

End Class
