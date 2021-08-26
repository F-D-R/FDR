Option Explicit On
Option Strict On

'TODO: VB!!!
Imports Microsoft.VisualBasic

Friend Class frmMain

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Text &= " - v" & System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
            cboConn.Text = App.DBConn
            Me.Gombok()
        Catch
        End Try
    End Sub

    Private Sub frmMain_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            App.Disconnect()
        Catch
        End Try
    End Sub

    Private Sub Gombok()
        Try
            Dim llogConn As Boolean = App.IsConnected
            If True Then
                mnuConnect.Enabled = Not llogConn
                btnConnect.Enabled = Not llogConn
                mnuDisconnect.Enabled = llogConn
                btnDisconnect.Enabled = llogConn
                mnuOpt.Enabled = llogConn
                mnuUser.Enabled = llogConn
                mnuCat.Enabled = llogConn
                mnuAlbum.Enabled = llogConn
                btnAlbum.Enabled = llogConn
                mnuPict.Enabled = llogConn
                mnuLonelyPictures.Enabled = llogConn
                mnuCatAlb.Enabled = llogConn
                mnuAlbPict.Enabled = llogConn
                mnuSelect.Enabled = llogConn
                mnuSearch.Enabled = llogConn
            End If
        Catch
        End Try
    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        Try
            Me.Close()
        Catch
        End Try
    End Sub

	Private Sub Rename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
	 Handles mnuRename.Click, btnRename.Click
		Try
            Dim lobjForm As New frmRename()
			lobjForm.Show()
			Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
		End Try
	End Sub

	Private Sub Check_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
	 Handles mnuCheck.Click, btnCheck.Click
		Try
            Dim lobjForm As New frmCheck()
			lobjForm.Show()
			Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
		End Try
	End Sub

	Private Sub Compare_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
	 Handles mnuCompare.Click, btnCompare.Click
		Try
            Dim lobjForm As New frmCompare()
			lobjForm.Show()
			Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
		End Try
	End Sub

    Private Sub SQL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles mnuSQL.Click
        Try
            Dim lobjForm As New frmSQL()
            lobjForm.Show()
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub Publish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles mnuPublish.Click, btnPublish.Click
        Try
            Dim lobjForm As New frmPublish()
            lobjForm.Show()
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub


    Private Sub Connect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles mnuConnect.Click, btnConnect.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            App.Connect(cboConn.Text)
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub Disconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles mnuDisconnect.Click, btnDisconnect.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            App.Disconnect()
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub


    Private Sub ShowOpt(Optional ByVal tstrID As String = Nothing)
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Dim lstrSQL As String = "select * from fdr_opt"
            If Not IsES(tstrID) Then
                If lstrSQL.EndsWith("fdr_opt") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "id='" & tstrID & "'"
            End If
            lstrSQL &= " order by id"
            Dim lobjForm As New frmList(lstrSQL, "Options")
            lobjForm.Show()
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub mnuOpt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOpt.Click
        Me.ShowOpt()
    End Sub


    Private Sub ShowUser(Optional ByVal tstrID As String = Nothing)
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Dim lstrSQL As String = "select * from fdr_user"
            If Not IsES(tstrID) Then
                If lstrSQL.EndsWith("fdr_user") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "id='" & tstrID & "'"
            End If
            lstrSQL &= " order by id"
            Dim lobjForm As New frmList(lstrSQL, "Users")
            lobjForm.Show()
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub mnuUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUser.Click
        Me.ShowUser()
    End Sub


    Private Sub Cat_CatAlb(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender)
        If Not IsES(lstrID) Then Me.ShowCatAlb(lstrID)
    End Sub

    Private Sub ShowCat(Optional ByVal tstrID As String = Nothing)
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Dim lstrSQL As String = "select * from fdr_cat"
            If Not IsES(tstrID) Then
                If lstrSQL.EndsWith("fdr_cat") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "id='" & tstrID & "'"
            End If
            lstrSQL &= " order by cat_order, id"
            Dim lobjForm As New frmList(lstrSQL, "Categories")
            lobjForm.AddFunction("Cat_Al&b", AddressOf Me.Cat_CatAlb, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B)
            lobjForm.Show()
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub mnuCat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCat.Click
        Me.ShowCat()
    End Sub


    Private Sub Album_Pict(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender)
        If Not IsES(lstrID) Then Me.ShowPict(, lstrID)
    End Sub

    Private Sub Album_LargePict(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender)
        If Not IsES(lstrID) Then Me.ShowPict(, lstrID, , App.DestPictDir)
    End Sub

    Private Sub Album_AlbPict(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender)
        If Not IsES(lstrID) Then Me.ShowAlbPict(lstrID)
    End Sub

    Private Sub Album_AlbPictThumbs(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender)
        If Not IsES(lstrID) Then Me.ShowAlbPictWithThumbs(lstrID)
    End Sub

    Private Sub Album_AlbPictLargePict(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender)
        If Not IsES(lstrID) Then Me.ShowAlbPictWithThumbs(lstrID, , App.DestPictDir)
    End Sub

    Private Sub Album_CatAlb(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender)
        If Not IsES(lstrID) Then Me.ShowCatAlb(, lstrID)
    End Sub

    Private Sub Album_Thumb(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender, "thumb")
        If Not IsES(lstrID) Then Me.ShowPict(lstrID)
    End Sub

    Private Sub Album_InsertCatAlb(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim lintaRows() As Integer = Me.GetSelectionBySender(sender)
            If lintaRows Is Nothing OrElse lintaRows.Length = 0 Then Exit Sub

            Dim lstrCatID As String = InputBox("Category ID:")
            If IsES(lstrCatID) Then Exit Sub

            For I As Integer = 0 To lintaRows.Length - 1
                Dim lstrAlbID As String = Me.GetFieldBySender(sender, "id", lintaRows(I))
                If Not IsES(lstrCatID) AndAlso Not IsES(lstrAlbID) Then
                    Dim lstrSQL As String = "insert into fdr_cat_alb (cat_id, album_id, album_order) values ('" & lstrCatID & "', '" & lstrAlbID & "', 0)"
                    Using lobjCmd As New MySql.Data.MySqlClient.MySqlCommand(lstrSQL, App.Connection)
                        Try
                            lobjCmd.ExecuteNonQuery()
                        Catch ex As System.Exception
                            App.AddErr(ex)
                        End Try
                    End Using
                End If
            Next
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub ShowAlbum(Optional ByVal tstrID As String = Nothing)
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Dim lstrSQL As String = "select * from fdr_album"
            If Not IsES(tstrID) Then
                If lstrSQL.EndsWith("fdr_album") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "id='" & tstrID & "'"
            End If
            lstrSQL &= " order by id"
            Dim lobjForm As New frmList(lstrSQL, "Albums")
            lobjForm.AddFunction("&Pict", AddressOf Me.Album_Pict, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P)
            lobjForm.AddFunction("&Large Pict", AddressOf Me.Album_LargePict, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L)
            lobjForm.AddFunction("Al&b_Pict", AddressOf Me.Album_AlbPict, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B)
            lobjForm.AddFunction("Alb_Pict+&Thumbs", AddressOf Me.Album_AlbPictThumbs, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.T)
            lobjForm.AddFunction("Alb_Pict+P&ict", AddressOf Me.Album_AlbPictLargePict, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.I)
            lobjForm.AddFunction("Cat_Alb", AddressOf Me.Album_CatAlb)
            lobjForm.AddFunction("Thumb", AddressOf Me.Album_Thumb)
            lobjForm.AddFunction("-")
            lobjForm.AddFunction("Insert CatAlb", AddressOf Me.Album_InsertCatAlb, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Insert)
            lobjForm.Show()
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub Album_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles mnuAlbum.Click, btnAlbum.Click
        Me.ShowAlbum()
    End Sub


    Private Sub Pict_AlbPict(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender)
        If Not IsES(lstrID) Then Me.ShowAlbPict(, lstrID)
    End Sub

    Private Sub Pict_InsertAlbPict(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim lintaRows() As Integer = Me.GetSelectionBySender(sender)
            If lintaRows Is Nothing OrElse lintaRows.Length = 0 Then Exit Sub

            Dim lstrAlbID As String = InputBox("Album ID:")
            If IsES(lstrAlbID) Then Exit Sub

            For I As Integer = 0 To lintaRows.Length - 1
                Dim lstrPictID As String = Me.GetFieldBySender(sender, "id", lintaRows(I))
                If Not IsES(lstrAlbID) AndAlso Not IsES(lstrPictID) Then
                    Dim lstrSQL As String = "insert into fdr_alb_pict (album_id, pict_id, pict_order) values ('" & lstrAlbID & "', '" & lstrPictID & "', 0)"
                    Using lobjCmd As New MySql.Data.MySqlClient.MySqlCommand(lstrSQL, App.Connection)
                        Try
                            lobjCmd.ExecuteNonQuery()
                        Catch ex As System.Exception
                            App.AddErr(ex)
                        End Try
                    End Using
                End If
            Next
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub Pict_LargePict(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender)
        If Not IsES(lstrID) Then Me.ShowPict(lstrID, , , App.DestPictDir)
    End Sub

    Private Sub ShowPict(Optional ByVal tstrID As String = Nothing, Optional ByVal tstrAlbID As String = Nothing, Optional ByVal tlogLonely As Boolean = False, Optional ByVal tstrPictRoot As String = Nothing, Optional ByVal tstrSearch As String = Nothing, Optional ByVal tstrWhere As String = Nothing)
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Dim lstrSQL As String = "select * from fdr_pict"
            If Not IsES(tstrID) Then
                If lstrSQL.EndsWith("fdr_pict") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "id='" & tstrID & "'"
            End If
            If Not IsES(tstrAlbID) Then
                If lstrSQL.EndsWith("fdr_pict") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "id in (select pict_id from fdr_alb_pict where album_id='" & tstrAlbID & "')"
            End If
            If tlogLonely Then
                If lstrSQL.EndsWith("fdr_pict") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "id not in (select pict_id from fdr_alb_pict)"
            End If
            If Not IsES(tstrSearch) Then
                If lstrSQL.EndsWith("fdr_pict") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "("
                lstrSQL &= "id like '%" & tstrSearch & "%'"
                lstrSQL &= " or pictfile like '%" & tstrSearch & "%'"
                lstrSQL &= " or title_hu like '%" & tstrSearch & "%'"
                lstrSQL &= " or title_en like '%" & tstrSearch & "%'"
                lstrSQL &= " or description_hu like '%" & tstrSearch & "%'"
                lstrSQL &= " or description_en like '%" & tstrSearch & "%'"
                lstrSQL &= " or location_hu like '%" & tstrSearch & "%'"
                lstrSQL &= " or location_en like '%" & tstrSearch & "%'"
                lstrSQL &= " or keywords_hu like '%" & tstrSearch & "%'"
                lstrSQL &= " or keywords_en like '%" & tstrSearch & "%'"
                lstrSQL &= ")"
            End If
            If Not IsES(tstrWhere) Then
                If lstrSQL.EndsWith("fdr_pict") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "(" & tstrWhere & ")"
            End If
            lstrSQL &= " order by id"
            If Not lstrSQL.Contains("where") Then
                lstrSQL &= " limit 0,1000"
            End If
            If IsES(tstrPictRoot) Then tstrPictRoot = App.DestThumbDir
            Dim lobjForm As New frmList(lstrSQL, "Pictures", lstrSQL.Contains("where"), "pictfile", tstrPictRoot)
            lobjForm.AddFunction("Al&b_Pict", AddressOf Me.Pict_AlbPict, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B)
            lobjForm.AddFunction("&Large Pict", AddressOf Me.Pict_LargePict, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L)
            lobjForm.AddFunction("-")
            lobjForm.AddFunction("Add to album", AddressOf Me.Pict_InsertAlbPict, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Insert)
            lobjForm.Show()
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub mnuPict_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPict.Click
        Me.ShowPict()
    End Sub

    Private Sub mnuLonelyPictures_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLonelyPictures.Click
        Me.ShowPict(, , True)
    End Sub


    Private Sub CatAlb_Cat(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender, "cat_id")
        If Not IsES(lstrID) Then Me.ShowCat(lstrID)
    End Sub

    Private Sub CatAlb_Album(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender, "album_id")
        If Not IsES(lstrID) Then Me.ShowAlbum(lstrID)
    End Sub

    Private Sub ShowCatAlb(Optional ByVal tstrCatID As String = Nothing, Optional ByVal tstrAlbID As String = Nothing)
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Dim lstrSQL As String = "select * from fdr_cat_alb"
            If Not IsES(tstrCatID) Then
                If lstrSQL.EndsWith("fdr_cat_alb") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "cat_id='" & tstrCatID & "'"
            End If
            If Not IsES(tstrAlbID) Then
                If lstrSQL.EndsWith("fdr_cat_alb") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "album_id='" & tstrAlbID & "'"
            End If
            lstrSQL &= " order by cat_id, album_order, album_id"
            Dim lobjForm As New frmList(lstrSQL, "Category albums")
            lobjForm.AddFunction("Cat", AddressOf Me.CatAlb_Cat)
            lobjForm.AddFunction("Al&bum", AddressOf Me.CatAlb_Album, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B)
            lobjForm.Show()
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub mnuCatAlb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCatAlb.Click
        Me.ShowCatAlb()
    End Sub


    Private Sub AlbPict_Album(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender, "album_id")
        If Not IsES(lstrID) Then Me.ShowAlbum(lstrID)
    End Sub

    Private Sub AlbPict_Pict(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender, "pict_id")
        If Not IsES(lstrID) Then Me.ShowPict(lstrID)
    End Sub

    Private Sub AlbPict_LargePict(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstrID As String = Me.GetFieldBySender(sender, "pict_id")
        If Not IsES(lstrID) Then Me.ShowPict(lstrID, , , App.DestPictDir)
    End Sub

    Private Sub AlbPict_Toggle(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim lintaRows() As Integer = Me.GetSelectionBySender(sender)
            If lintaRows Is Nothing OrElse lintaRows.Length = 0 Then Exit Sub
            For I As Integer = 0 To lintaRows.Length - 1
                Dim lstrAlbID As String = Me.GetFieldBySender(sender, "album_id", lintaRows(I))
                Dim lstrPictID As String = Me.GetFieldBySender(sender, "pict_id", lintaRows(I))
                Dim lstrVisible As String = Me.GetFieldBySender(sender, "visible", lintaRows(I))
                If Not IsES(lstrAlbID) AndAlso Not IsES(lstrPictID) AndAlso Not IsES(lstrVisible) Then
                    If IsTrue(lstrVisible) Then
                        lstrVisible = "0"
                    Else
                        lstrVisible = "1"
                    End If
                    Dim lstrSQL As String = "update fdr_alb_pict set visible=" & lstrVisible & " where album_id='" & lstrAlbID & "' and pict_id='" & lstrPictID & "'"
                    Using lobjCmd As New MySql.Data.MySqlClient.MySqlCommand(lstrSQL, App.Connection)
                        Try
                            lobjCmd.ExecuteNonQuery()
                        Catch ex As System.Exception
                            App.AddErr(ex)
                        End Try
                    End Using
                End If
            Next
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub AlbPict_Delete(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim lintaRows() As Integer = Me.GetSelectionBySender(sender)
            If lintaRows Is Nothing OrElse lintaRows.Length = 0 Then Exit Sub
            If System.Windows.Forms.MessageBox.Show("Are you sure to delete these rows?", "Deleting confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then Exit Sub

            For I As Integer = 0 To lintaRows.Length - 1
                Dim lstrAlbID As String = Me.GetFieldBySender(sender, "album_id", lintaRows(I))
                Dim lstrPictID As String = Me.GetFieldBySender(sender, "pict_id", lintaRows(I))
                If Not IsES(lstrAlbID) AndAlso Not IsES(lstrPictID) Then
                    Dim lstrSQL As String = "delete from fdr_alb_pict where album_id='" & lstrAlbID & "' and pict_id='" & lstrPictID & "'"
                    Using lobjCmd As New MySql.Data.MySqlClient.MySqlCommand(lstrSQL, App.Connection)
                        Try
                            lobjCmd.ExecuteNonQuery()
                        Catch ex As System.Exception
                            App.AddErr(ex)
                        End Try
                    End Using
                End If
            Next
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub AlbPict_InsertAlbPict(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim lintaRows() As Integer = Me.GetSelectionBySender(sender)
            If lintaRows Is Nothing OrElse lintaRows.Length = 0 Then Exit Sub

            Dim lstrAlbID As String = InputBox("Album ID:")
            If IsES(lstrAlbID) Then Exit Sub

            For I As Integer = 0 To lintaRows.Length - 1
                Dim lstrPictID As String = Me.GetFieldBySender(sender, "pict_id", lintaRows(I))
                If Not IsES(lstrAlbID) AndAlso Not IsES(lstrPictID) Then
                    Dim lstrSQL As String = "insert into fdr_alb_pict (album_id, pict_id, pict_order) values ('" & lstrAlbID & "', '" & lstrPictID & "', 0)"
                    Using lobjCmd As New MySql.Data.MySqlClient.MySqlCommand(lstrSQL, App.Connection)
                        Try
                            lobjCmd.ExecuteNonQuery()
                        Catch ex As System.Exception
                            App.AddErr(ex)
                        End Try
                    End Using
                End If
            Next
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub ShowAlbPict(Optional ByVal tstrAlbID As String = Nothing, Optional ByVal tstrPictID As String = Nothing)
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Dim lstrSQL As String = "select * from fdr_alb_pict"
            If Not IsES(tstrAlbID) Then
                If lstrSQL.EndsWith("fdr_alb_pict") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "album_id='" & tstrAlbID & "'"
            End If
            If Not IsES(tstrPictID) Then
                If lstrSQL.EndsWith("fdr_alb_pict") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "pict_id='" & tstrPictID & "'"
            End If
            lstrSQL &= " order by album_id, pict_order, pict_id"
            If Not lstrSQL.Contains("where") Then
                lstrSQL &= " limit 0,1000"
            End If
            Dim lobjForm As New frmList(lstrSQL, "Album pictures", lstrSQL.Contains("where"))
            lobjForm.AddFunction("Al&bum", AddressOf Me.AlbPict_Album, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B)
            lobjForm.AddFunction("&Pict", AddressOf Me.AlbPict_Pict, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P)
            lobjForm.AddFunction("&Large Pict", AddressOf Me.AlbPict_LargePict, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L)
            lobjForm.Show()
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub ShowAlbPictWithThumbs(Optional ByVal tstrAlbID As String = Nothing, Optional ByVal tstrPictID As String = Nothing, Optional ByVal tstrPictRoot As String = Nothing)
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Dim lstrSQL As String = "select ap.*, p.pictfile from fdr_alb_pict ap left outer join fdr_pict p on ap.pict_id=p.id"
            If Not IsES(tstrAlbID) Then
                If lstrSQL.EndsWith("on ap.pict_id=p.id") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "ap.album_id='" & tstrAlbID & "'"
            End If
            If Not IsES(tstrPictID) Then
                If lstrSQL.EndsWith("on ap.pict_id=p.id") Then
                    lstrSQL &= " where "
                Else
                    lstrSQL &= " and "
                End If
                lstrSQL &= "ap.pict_id='" & tstrPictID & "'"
            End If
            lstrSQL &= " order by ap.album_id, ap.pict_order, ap.pict_id limit 0,1000"
            If IsES(tstrPictRoot) Then tstrPictRoot = App.DestThumbDir
            Dim lobjForm As New frmList(lstrSQL, "Album pictures", lstrSQL.Contains("where"), "pictfile", tstrPictRoot)
            lobjForm.AddFunction("Al&bum", AddressOf Me.AlbPict_Album, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B)
            lobjForm.AddFunction("&Pict", AddressOf Me.AlbPict_Pict, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P)
            lobjForm.AddFunction("&Large Pict", AddressOf Me.AlbPict_LargePict, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L)
            lobjForm.AddFunction("-")
            lobjForm.AddFunction("&Toggle visible", AddressOf Me.AlbPict_Toggle, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.T)
            'lobjForm.AddFunction("&Delete", AddressOf Me.AlbPict_Delete, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Delete)
            lobjForm.AddFunction("&Delete", AddressOf Me.AlbPict_Delete, System.Windows.Forms.Keys.Delete)
            lobjForm.AddFunction("-")
            lobjForm.AddFunction("Add to album", AddressOf Me.AlbPict_InsertAlbPict, System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Insert)
            lobjForm.Show()
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub mnuAlbPict_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAlbPict.Click
        Me.ShowAlbPict()
    End Sub


    Private Sub mnuSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSearch.Click
        Try
            Dim lstrSearch As String = InputBox("Search:")
            If Not IsES(lstrSearch) Then Me.ShowPict(, , , , lstrSearch)
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub


    Private Sub mnuSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelect.Click
        Dim lstrSQL As String
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            lstrSQL = InputBox("SQL")
            If Not IsES(lstrSQL) Then
                Dim lobjForm As New frmList(lstrSQL, "SQL")
                lobjForm.Show()
            End If
            Me.Gombok()
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Function GetFormBySender(ByVal sender As System.Object) As System.Windows.Forms.Form
        Try
            If TypeOf sender Is System.Windows.Forms.ToolStripMenuItem Then
                Return CType(sender, System.Windows.Forms.ToolStripMenuItem).OwnerItem.Owner.FindForm()
            End If
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
        Return Nothing
    End Function

    Private Function GetFieldBySender(ByVal sender As System.Object, Optional ByVal tstrField As String = "id", Optional ByVal tintRow As Integer = -1) As String
        Try
            Dim lobjForm As System.Windows.Forms.Form = Me.GetFormBySender(sender)
            If lobjForm IsNot Nothing AndAlso TypeOf lobjForm Is frmList Then
                Return CType(lobjForm, frmList).GetField(tstrField, tintRow)
            End If
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
        Return String.Empty
    End Function

    Private Function GetSelectionBySender(ByVal sender As System.Object, Optional ByVal tstrField As String = "id") As Integer()
        Try
            Dim lobjForm As System.Windows.Forms.Form = Me.GetFormBySender(sender)
            If lobjForm IsNot Nothing AndAlso TypeOf lobjForm Is frmList Then
                Return CType(lobjForm, frmList).GetSelection()
            End If
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
        Return Nothing
    End Function


    Private Sub mnuFTP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFTP.Click
        Dim lobjConn As FTPConnection = Nothing
        Dim lstrMsg As String = Nothing
        Dim llngBegin As Long
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
			llngBegin = Date.Now.Ticks

            lobjConn = New FTPConnection()
            With lobjConn
                If IsTrue(App.GetSetting("RemotePassive")) Then
                    .Open(App.GetSetting("RemoteHost"), App.GetSetting("RemoteLogin"), App.GetSetting("RemotePwd"), FTPMode.Passive)
                Else
                    .Open(App.GetSetting("RemoteHost"), App.GetSetting("RemoteLogin"), App.GetSetting("RemotePwd"), FTPMode.Active)
                End If
                lstrMsg &= "Open: " & (Date.Now.Ticks - llngBegin) \ 10000 & "ms" & System.Environment.NewLine
				llngBegin = Date.Now.Ticks


                If False Then
                    If False Then
                        .SetCurrentDirectory("/FtpClient")
                        lstrMsg &= "SetCurrentDirectory: " & (Date.Now.Ticks - llngBegin) \ 10000 & "ms" & System.Environment.NewLine
						llngBegin = Date.Now.Ticks
                    End If
					Dim lcolTmp As System.Collections.ArrayList = .Dir()
                    lstrMsg &= "Dir: " & (Date.Now.Ticks - llngBegin) \ 10000 & "ms" & System.Environment.NewLine
					llngBegin = Date.Now.Ticks
                    For Each lobjTmp As Object In lcolTmp
                        lstrMsg &= lobjTmp.ToString & System.Environment.NewLine
                    Next
                    lstrMsg &= "List: " & (Date.Now.Ticks - llngBegin) \ 10000 & "ms" & System.Environment.NewLine
					llngBegin = Date.Now.Ticks
                ElseIf False Then
                    If False Then
                        .SetCurrentDirectory("/FtpClient")
                        lstrMsg &= "SetCurrentDirectory: " & (Date.Now.Ticks - llngBegin) \ 10000 & "ms" & System.Environment.NewLine
						llngBegin = Date.Now.Ticks
                    End If
					Dim lcolTmp As System.Collections.ArrayList = .XDir()
                    lstrMsg &= "XDir: " & (Date.Now.Ticks - llngBegin) \ 10000 & "ms" & System.Environment.NewLine
					llngBegin = Date.Now.Ticks
                    For Each lobjTmp As Object In lcolTmp
						If TypeOf lobjTmp Is System.Collections.ArrayList Then
							For Each lobjCol As Object In CType(lobjTmp, System.Collections.ArrayList)
								lstrMsg &= lobjCol.ToString & "  "
							Next
						End If
                        lstrMsg &= System.Environment.NewLine
                    Next
                    lstrMsg &= "List: " & (Date.Now.Ticks - llngBegin) \ 10000 & "ms" & System.Environment.NewLine
					llngBegin = Date.Now.Ticks
                ElseIf True Then
                    If True Then
                        Try
                            .MakeDir("/proba1/tmp") 'Csak akkor tudja létrehozni, ha a proba1 könyvtár már létezik!!!
                            lstrMsg &= "MakeDir: " & (Date.Now.Ticks - llngBegin) \ 10000 & "ms" & System.Environment.NewLine
							llngBegin = Date.Now.Ticks
                        Catch
                        End Try
                    End If
                    If True Then
                        Try
                            .MakeDir("/proba1")
                            lstrMsg &= "MakeDir: " & (Date.Now.Ticks - llngBegin) \ 10000 & "ms" & System.Environment.NewLine
							llngBegin = Date.Now.Ticks
                        Catch
                        End Try
                    End If
                    If False Then
                        .SetCurrentDirectory("/proba1")
                        lstrMsg &= "SetCurrentDirectory: " & (Date.Now.Ticks - llngBegin) \ 10000 & "ms" & System.Environment.NewLine
						llngBegin = Date.Now.Ticks
                    End If
                    .SendFile("c:\proba1.jpg", "/proba1/proba1.jpg", FTPFileTransferType.Binary)
                    lstrMsg &= "SendFile: " & (Date.Now.Ticks - llngBegin) \ 10000 & "ms" & System.Environment.NewLine
					llngBegin = Date.Now.Ticks

                    If True Then
                        Try
                            .MakeDir("/proba2")
                            lstrMsg &= "MakeDir: " & (Date.Now.Ticks - llngBegin) \ 10000 & "ms" & System.Environment.NewLine
							llngBegin = Date.Now.Ticks
                        Catch
                        End Try
                    End If
                    If False Then
                        .SetCurrentDirectory("/proba2")
                        lstrMsg &= "SetCurrentDirectory: " & (Date.Now.Ticks - llngBegin) \ 10000 & "ms" & System.Environment.NewLine
						llngBegin = Date.Now.Ticks
                    End If
                    .SendFile("c:\proba2.jpg", "/proba2/proba2.jpg", FTPFileTransferType.Binary)
                    lstrMsg &= "SendFile: " & (Date.Now.Ticks - llngBegin) \ 10000 & "ms" & System.Environment.NewLine
					llngBegin = Date.Now.Ticks
                End If


                .Close()
            End With
            lobjConn = Nothing

            System.Windows.Forms.MessageBox.Show(lstrMsg, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
        Catch ex As System.Exception
            System.Windows.Forms.MessageBox.Show(ex.ToString, "Error!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation)
		Finally
			If lobjConn IsNot Nothing Then
				Try
					lobjConn.Close()
				Catch
				End Try
			End If
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

End Class
