Option Explicit On
Option Strict On

Public Class frmCompare

    Private mintFileCount1 As Integer
    Private mintFileCount2 As Integer
    Private Const mintcMsgLength As Integer = 200

    Private Sub frmCompare_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            With cboFilter1
                .Items.AddRange(App.GetSetting("CompareFilter1Items", "*.*|*.CRW|*.CR2|*.THM|*.JPG").Split("|"c))
                .SelectedIndex = CInt(App.GetSetting("CompareFilter1Index", "4"))
            End With
            With cboFilter2
                .Items.AddRange(App.GetSetting("CompareFilter2Items", "*.*|*.CRW|*.CR2|*.THM|*.JPG").Split("|"c))
                .SelectedIndex = CInt(App.GetSetting("CompareFilter2Index", "0"))
            End With

            txtPath1.Text = App.GetSetting("ComparePath1", App.SourceRoot)
            txtPath2.Text = App.GetSetting("ComparePath2", "")
            chkSubfolderOfFirst.Checked = IsTrue(App.GetSetting("CompareSubfolderOfFirst", "True"))
            txtSubFolder2.Text = App.GetSetting("CompareSubfolder", "RAW")

            Me.LoadFiles(1)
            Me.LoadFiles(2)

            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub frmCompare_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Try
            Select Case e.KeyCode
                Case System.Windows.Forms.Keys.Escape
                    Me.Close()

                Case System.Windows.Forms.Keys.F5
                    Me.LoadFiles(1)
                    If chkSubfolderOfFirst.Checked Then
                        btnSubFolder2.PerformClick()
                    End If

                Case System.Windows.Forms.Keys.Left, System.Windows.Forms.Keys.Up
                    If e.Control AndAlso btnPrevDir.Enabled Then btnPrevDir.PerformClick()

                Case System.Windows.Forms.Keys.Right, System.Windows.Forms.Keys.Down
                    If e.Control AndAlso btnNextDir.Enabled Then btnNextDir.PerformClick()

                Case System.Windows.Forms.Keys.Home
                    If e.Control AndAlso btnFirstDir.Enabled Then btnFirstDir.PerformClick()

                Case System.Windows.Forms.Keys.End
                    If e.Control AndAlso btnLastDir.Enabled Then btnLastDir.PerformClick()

                Case System.Windows.Forms.Keys.Delete
                    If btnDelete1.Enabled AndAlso Not btnDelete2.Enabled Then
                        btnDelete1.PerformClick()
                    ElseIf btnDelete2.Enabled AndAlso Not btnDelete1.Enabled Then
                        btnDelete2.PerformClick()
                    End If

                Case System.Windows.Forms.Keys.F2
                    If e.Shift Then
                        btnNotExistsInSecond.PerformClick()
                        btnNotExistsInFirst.PerformClick()
                    End If

            End Select
        Catch
        End Try
    End Sub

    Private Sub Gombok()
        Dim lintCount1 As Integer
        Dim lintCount2 As Integer
        Dim llogTmp As Boolean
        Try
            llogTmp = Me.chkSubfolderOfFirst.Checked
            lblSubFolder2.Enabled = llogTmp
            txtSubFolder2.Enabled = llogTmp
            btnSubFolder2.Visible = llogTmp
            'btnPath2.Visible = Not llogTmp

            grpSubString1.Visible = Not optWhole1.Checked
            grpSubString2.Visible = Not optWhole2.Checked
            grpSubString1.Enabled = optPiece1.Checked
            grpSubString2.Enabled = optPiece2.Checked

            btnDelete1.Enabled = lstFiles1.SelectedItems.Count > 0
            btnDelete2.Enabled = lstFiles2.SelectedItems.Count > 0

            'Fájlok számának kiírása:
            Try
                If IsES(txtPath1.Text) Then
                    lblCount1.Text = Nothing
                ElseIf mintFileCount1 = 0 Then
                    lblCount1.Text = "No such files..."
                ElseIf mintFileCount1 = 1 Then
                    lblCount1.Text = "1 file"
                Else
                    lblCount1.Text = CStr(mintFileCount1) & " files"
                End If
                If IsES(txtPath2.Text) Then
                    lblCount2.Text = Nothing
                ElseIf mintFileCount2 = 0 Then
                    lblCount2.Text = "No such files..."
                ElseIf mintFileCount2 = 1 Then
                    lblCount2.Text = "1 file"
                Else
                    lblCount2.Text = CStr(mintFileCount2) & " files"
                End If
            Catch
            End Try

            'Fájlok kiválasztási maszkja:
            Try
                If lstFiles1.Items.Count > 0 Then
                    lblSample1.Text = Me.GetFilter1(System.IO.Path.GetFileNameWithoutExtension(CStr(lstFiles1.Items(0))))
                Else
                    lblSample1.Text = Nothing
                End If
                If lstFiles2.Items.Count > 0 Then
                    lblSample2.Text = Me.GetFilter2(System.IO.Path.GetFileNameWithoutExtension(CStr(lstFiles2.Items(0))))
                Else
                    lblSample2.Text = Nothing
                End If

                If lstFiles1.Items.Count = lstFiles2.Items.Count Then
                    lblCount1.Font = New System.Drawing.Font(lblCount1.Font, System.Drawing.FontStyle.Regular)
                    lblCount2.Font = New System.Drawing.Font(lblCount2.Font, System.Drawing.FontStyle.Regular)
                ElseIf lstFiles1.Items.Count > lstFiles2.Items.Count Then
                    lblCount1.Font = New System.Drawing.Font(lblCount1.Font, System.Drawing.FontStyle.Bold)
                    lblCount2.Font = New System.Drawing.Font(lblCount2.Font, System.Drawing.FontStyle.Regular)
                ElseIf lstFiles1.Items.Count < lstFiles2.Items.Count Then
                    lblCount1.Font = New System.Drawing.Font(lblCount1.Font, System.Drawing.FontStyle.Regular)
                    lblCount2.Font = New System.Drawing.Font(lblCount2.Font, System.Drawing.FontStyle.Bold)
                End If
            Catch
            End Try

            'Kiválasztott fájlok számának kiírása:
            Try
                lintCount1 = Me.lstFiles1.SelectedItems.Count
                If lintCount1 = 0 Then
                    lblSelected1.Text = "No file selected..."
                    lblSelected1.Font = New System.Drawing.Font(lblSelected1.Font, System.Drawing.FontStyle.Regular)
                ElseIf lintCount1 = 1 Then
                    lblSelected1.Text = "1 file selected"
                    lblSelected1.Font = New System.Drawing.Font(lblSelected1.Font, System.Drawing.FontStyle.Bold)
                Else
                    lblSelected1.Text = CStr(lintCount1) & " files selected"
                    lblSelected1.Font = New System.Drawing.Font(lblSelected1.Font, System.Drawing.FontStyle.Bold)
                End If

                lintCount2 = Me.lstFiles2.SelectedItems.Count
                If lintCount2 = 0 Then
                    lblSelected2.Text = "No file selected..."
                    lblSelected2.Font = New System.Drawing.Font(lblSelected2.Font, System.Drawing.FontStyle.Regular)
                ElseIf lintCount2 = 1 Then
                    lblSelected2.Text = "1 file selected"
                    lblSelected2.Font = New System.Drawing.Font(lblSelected2.Font, System.Drawing.FontStyle.Bold)
                Else
                    lblSelected2.Text = CStr(lintCount2) & " files selected"
                    lblSelected2.Font = New System.Drawing.Font(lblSelected2.Font, System.Drawing.FontStyle.Bold)
                End If
            Catch
            End Try

        Catch
        End Try
    End Sub

    Private Sub btnDelete1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete1.Click
        Dim lstrTmp As String = Nothing
        Dim lintCount As Integer
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            lintCount = lstFiles1.SelectedItems.Count
            If lintCount > 0 Then
                For I As Integer = 0 To lstFiles1.SelectedItems.Count - 1
                    lstrTmp &= CStr(lstFiles1.SelectedItems(I)) & System.Environment.NewLine
                Next
                If ES(lstrTmp).Length > mintcMsgLength Then
                    lstrTmp = ES(lstrTmp).Substring(0, mintcMsgLength) & "..."
                End If
                If System.Windows.Forms.MessageBox.Show("Are you sure to delete the selected files?  (" & lblSelected1.Text & "):" & System.Environment.NewLine & System.Environment.NewLine & lstrTmp, "Delete files", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                    For I As Integer = 0 To lstFiles1.SelectedItems.Count - 1
                        System.IO.File.Delete(System.IO.Path.Combine(txtPath1.Text, CStr(lstFiles1.SelectedItems(I))))
                    Next
                    'MessageBox.Show("Files deleted successfully...", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.LoadFiles(1)
                End If
            End If
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub btnDelete2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete2.Click
        Dim lstrTmp As String = Nothing
        Dim lintCount As Integer
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            lintCount = lstFiles2.SelectedItems.Count
            If lintCount > 0 Then
                For I As Integer = 0 To lstFiles2.SelectedItems.Count - 1
                    lstrTmp &= CStr(lstFiles2.SelectedItems(I)) & System.Environment.NewLine
                Next
                If ES(lstrTmp).Length > mintcMsgLength Then
                    lstrTmp = ES(lstrTmp).Substring(0, mintcMsgLength) & "..."
                End If
                If System.Windows.Forms.MessageBox.Show("Are you sure to delete the selected files?  (" & lblSelected2.Text & "):" & System.Environment.NewLine & System.Environment.NewLine & lstrTmp, "Delete files", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                    For I As Integer = 0 To lstFiles2.SelectedItems.Count - 1
                        System.IO.File.Delete(System.IO.Path.Combine(txtPath2.Text, CStr(lstFiles2.SelectedItems(I))))
                    Next
                    'MessageBox.Show("Files deleted successfully...", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.LoadFiles(2)
                End If
            End If
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub btnExistsInSecond_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExistsInSecond.Click
        Dim lstrFileName As String
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            lstFiles1.BeginUpdate()
            btnDeselect1.PerformClick()
            For I As Integer = 0 To lstFiles1.Items.Count - 1
                lstrFileName = System.IO.Path.GetFileNameWithoutExtension(CStr(lstFiles1.Items(I)))
                For J As Integer = 0 To lstFiles2.Items.Count - 1
                    If CStr(lstFiles2.Items(J)) Like Me.GetFilter1(lstrFileName) Then
                        lstFiles1.SelectedIndices.Add(I)
                        Exit For
                    End If
                Next J
            Next I
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            lstFiles1.EndUpdate()
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub btnExistsInFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExistsInFirst.Click
        Dim lstrFileName As String
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            lstFiles2.BeginUpdate()
            btnDeselect2.PerformClick()
            For I As Integer = 0 To lstFiles2.Items.Count - 1
                lstrFileName = System.IO.Path.GetFileNameWithoutExtension(CStr(lstFiles2.Items(I)))
                For J As Integer = 0 To lstFiles1.Items.Count - 1
                    If CStr(lstFiles1.Items(J)) Like Me.GetFilter2(lstrFileName) Then
                        lstFiles2.SelectedIndices.Add(I)
                        Exit For
                    End If
                Next J
            Next I
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            lstFiles2.EndUpdate()
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub btnNotExists_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles btnNotExistsInSecond.Click, btnNotExistsInFirst.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            If sender Is btnNotExistsInSecond Then
                btnExistsInSecond.PerformClick()
                btnInverse1.PerformClick()
            Else
                btnExistsInFirst.PerformClick()
                btnInverse2.PerformClick()
            End If
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub btnDeselect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles btnDeselect1.Click, btnDeselect2.Click
        Try
            If sender Is Me.btnDeselect1 Then
                lstFiles1.SelectedIndices.Clear()
            Else
                lstFiles2.SelectedIndices.Clear()
            End If
            Me.Gombok()
        Catch
        End Try
    End Sub

    Private Sub btnInverse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles btnInverse1.Click, btnInverse2.Click
        Dim lobjList As System.Windows.Forms.ListBox = Nothing
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            If sender Is btnInverse1 Then
                lobjList = lstFiles1
            Else
                lobjList = lstFiles2
            End If
            lobjList.BeginUpdate()
            For I As Integer = 0 To lobjList.Items.Count - 1
                lobjList.SetSelected(I, Not lobjList.GetSelected(I))
            Next I
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            If lobjList IsNot Nothing Then lobjList.EndUpdate()
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub Reload(ByVal sender As Object, ByVal e As System.EventArgs) _
     Handles lblCount1.DoubleClick, lblCount2.DoubleClick, cboFilter1.SelectedIndexChanged, cboFilter2.SelectedIndexChanged
        If sender Is Me.lblCount1 OrElse sender Is Me.cboFilter1 Then
            Me.LoadFiles(1)
        Else
            Me.LoadFiles(2)
        End If
    End Sub

    Private Sub btnPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles btnPath1.Click, btnPath2.Click
        Try
            With dlgFolder
                If sender Is Me.btnPath1 Then
                    If Not IsES(txtPath1.Text) AndAlso System.IO.Directory.Exists(txtPath1.Text) Then
                        .SelectedPath = txtPath1.Text
                    End If
                    If .ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                        txtPath1.Text = .SelectedPath
                        If Not txtPath1.Text.EndsWith(System.IO.Path.DirectorySeparatorChar) Then txtPath1.Text &= System.IO.Path.DirectorySeparatorChar
                        Me.LoadFiles(1)
                        If chkSubfolderOfFirst.Checked Then
                            btnSubFolder2.PerformClick()
                        End If
                    End If
                Else
                    If Not IsES(txtPath2.Text) AndAlso System.IO.Directory.Exists(txtPath2.Text) Then
                        .SelectedPath = txtPath2.Text
                    End If
                    If .ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                        txtPath2.Text = .SelectedPath
                        If Not txtPath2.Text.EndsWith(System.IO.Path.DirectorySeparatorChar) Then txtPath2.Text &= System.IO.Path.DirectorySeparatorChar
                        Me.LoadFiles(2)
                    End If
                End If
            End With
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Function GetFilter1(ByVal tstrFile As String) As String
        Try
            If optWhole1.Checked Then
                Return txtPrefix1.Text & System.IO.Path.GetFileNameWithoutExtension(tstrFile) & txtPostfix1.Text
            Else
                Return txtPrefix1.Text & ES(tstrFile).Substring(CInt(txtFirst1.Text) - 1, CInt(txtLength1.Text)) & txtPostfix1.Text
            End If
        Catch
            Return String.Empty
        End Try
    End Function

    Private Function GetFilter2(ByVal tstrFile As String) As String
        Try
            If optWhole2.Checked Then
                Return txtPrefix2.Text & System.IO.Path.GetFileNameWithoutExtension(tstrFile) & txtPostfix2.Text
            Else
                Return txtPrefix2.Text & ES(tstrFile).Substring(CInt(txtFirst2.Text) - 1, CInt(txtLength2.Text)) & txtPostfix2.Text
            End If
        Catch
            Return String.Empty
        End Try
    End Function

    Private Sub Number_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles txtFirst1.TextChanged, txtFirst2.TextChanged, txtLength1.TextChanged, txtLength2.TextChanged
        Dim lintTmp As Integer
        Try
            With CType(sender, System.Windows.Forms.TextBox)
                Try
                    lintTmp = CInt(.Text)
                Catch
                End Try
                If lintTmp < 1 Then lintTmp = 1
                .Text = CStr(lintTmp)
            End With
            Me.Gombok()
        Catch
        End Try
    End Sub

    Private Sub LoadFiles(ByVal tintIndex As Integer)
        Dim lobjFiles As System.Windows.Forms.ListBox
        Dim lobjPath As System.Windows.Forms.TextBox
        Dim lobjFilter As System.Windows.Forms.ComboBox
        Dim llogAuto As Boolean
        Dim lintLength As Integer
        Dim lintTmp As Integer
        Dim lstraFiles() As String
        Try
            If tintIndex <> 1 AndAlso tintIndex <> 2 Then Exit Sub

            'Homokóra:
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            If tintIndex = 1 Then
                lobjFiles = lstFiles1
                lobjPath = txtPath1
                lobjFilter = cboFilter1
                llogAuto = optAuto1.Checked
            Else
                lobjFiles = lstFiles2
                lobjPath = txtPath2
                lobjFilter = cboFilter2
                llogAuto = optAuto2.Checked
            End If

            'ListBox elemeinek törlése:
            lobjFiles.Items.Clear()

            'Ha az útvonal üresvagy ha a könyvtár nem létezik, kilép:
            If IsES(lobjPath.Text) OrElse Not System.IO.Directory.Exists(lobjPath.Text) Then
                If tintIndex = 1 Then
                    mintFileCount1 = 0
                Else
                    mintFileCount2 = 0
                End If

                Exit Sub
            End If

            lstraFiles = System.IO.Directory.GetFiles(lobjPath.Text, lobjFilter.Text)
            For Each lstrFile As String In lstraFiles
                lobjFiles.Items.Add(System.IO.Path.GetFileName(lstrFile))
                If llogAuto Then
                    lintTmp = System.IO.Path.GetFileNameWithoutExtension(lstrFile).Length
                    If lintLength = 0 OrElse lintTmp < lintLength Then lintLength = lintTmp
                End If
            Next

            If tintIndex = 1 Then
                mintFileCount1 = lstraFiles.Length
                If llogAuto Then
                    txtFirst1.Text = "1"
                    txtLength1.Text = CStr(lintLength)
                End If
            Else
                mintFileCount2 = lstraFiles.Length
                If llogAuto Then
                    txtFirst2.Text = "1"
                    txtLength2.Text = CStr(lintLength)
                End If
            End If
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Gombok()
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub Frissites(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles optWhole1.CheckedChanged, optWhole2.CheckedChanged, optPiece1.CheckedChanged, optPiece2.CheckedChanged, _
     optAuto1.CheckedChanged, optAuto2.CheckedChanged, _
     txtFirst1.TextChanged, txtFirst2.TextChanged, txtLength1.TextChanged, txtLength2.TextChanged, _
     lstFiles1.SelectedIndexChanged, lstFiles2.SelectedIndexChanged, chkSubfolderOfFirst.CheckedChanged, _
     txtPrefix1.TextChanged, txtPrefix2.TextChanged, txtPostfix1.TextChanged, txtPostfix2.TextChanged
        Me.Gombok()
    End Sub

    Private Sub btnSubFolder2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubFolder2.Click
        Dim lstrPath As String
        Try
            If Not IsES(Me.txtPath1.Text) Then
                lstrPath = System.IO.Path.Combine(Me.txtPath1.Text, Me.txtSubFolder2.Text)
                If Not lstrPath.EndsWith(System.IO.Path.DirectorySeparatorChar) Then lstrPath &= System.IO.Path.DirectorySeparatorChar
                Me.txtPath2.Text = lstrPath
                Me.LoadFiles(2)
            End If
        Catch
        End Try
    End Sub

    Private Sub ChangeDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles btnFirstDir.Click, btnPrevDir.Click, btnNextDir.Click, btnLastDir.Click
        Dim lstraDirs() As String
        Dim lintIndex As Integer
        Try
            If IsES(txtPath1.Text) Then Exit Sub
            If Not System.IO.Directory.Exists(txtPath1.Text) Then Exit Sub

            lstraDirs = System.IO.Directory.GetDirectories(System.IO.Directory.GetParent(txtPath1.Text).Parent.FullName)
            System.Array.Sort(lstraDirs)

            'Dim lstrMsg As String = Nothing
            'For I As Integer = 0 To lstraDirs.Length - 1
            '    lstrMsg &= lstraDirs(I) & vbCrLf
            'Next
            'MsgBox("Directories:" & vbCrLf & vbCrLf & lstrMsg)

            For I As Integer = 0 To lstraDirs.Length - 1
                If lstraDirs(I) & System.IO.Path.DirectorySeparatorChar = txtPath1.Text Then
                    lintIndex = I
                    Exit For
                End If
            Next

            If sender Is btnFirstDir Then
                lintIndex = 0
            ElseIf sender Is btnPrevDir Then
                lintIndex -= 1
            ElseIf sender Is btnNextDir Then
                lintIndex += 1
            ElseIf sender Is btnLastDir Then
                lintIndex = lstraDirs.Length - 1
            Else
                Exit Sub
            End If

            If lintIndex < 0 Then lintIndex = 0
            If lintIndex > lstraDirs.Length - 1 Then lintIndex = lstraDirs.Length - 1
            'Ha ugyanaz maradt a könyvtár, kilép:
            If txtPath1.Text = lstraDirs(lintIndex) & System.IO.Path.DirectorySeparatorChar Then Exit Sub
            'Beállítja az új könyvtárat:
            txtPath1.Text = lstraDirs(lintIndex) & System.IO.Path.DirectorySeparatorChar
            'Betölti a fájlokat:
            Me.LoadFiles(1)
            If chkSubfolderOfFirst.Checked Then
                btnSubFolder2.PerformClick()
            End If
        Catch
        End Try
    End Sub

    Private Sub txtPath1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPath1.TextChanged
        ToolTip1.SetToolTip(txtPath1, txtPath1.Text)
    End Sub

    Private Sub txtPath2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPath2.TextChanged
        ToolTip1.SetToolTip(txtPath2, txtPath2.Text)
    End Sub

End Class
