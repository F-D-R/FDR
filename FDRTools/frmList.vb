Option Explicit On
Option Strict On

Friend Class frmList

    Private mstrCmd As String
    Private mlogAutoStart As Boolean
    Private mobjDA As MySql.Data.MySqlClient.MySqlDataAdapter
    Private mobjCB As MySql.Data.MySqlClient.MySqlCommandBuilder
    Private mobjTable As System.Data.DataTable

    Private mstrPictSource As String
    Private mstrPictRoot As String
    Private mstrPictCol As String = "pict"

    Private mlogSelect As Boolean
    Private mstrSelectedID As String

    Public Sub New(ByVal tstrCmd As String, Optional ByVal tstrTitle As String = Nothing, Optional ByVal tlogAutoStart As Boolean = True, Optional ByVal tstrPictSource As String = Nothing, Optional ByVal tstrPictRoot As String = Nothing, Optional ByVal tstrPictCol As String = "pict")
        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        mstrCmd = tstrCmd
        If Not IsES(tstrTitle) Then Me.Text = tstrTitle
        mlogAutoStart = tlogAutoStart

        mstrPictSource = tstrPictSource
        mstrPictRoot = tstrPictRoot
        mstrPictCol = tstrPictCol
    End Sub

    Public Function SelectID(ByRef tstrID As String, Optional ByVal tobjOwner As System.Windows.Forms.IWin32Window = Nothing) As Boolean
        Try
            tstrID = Nothing
            mlogSelect = True
            If Me.ShowDialog(tobjOwner) = System.Windows.Forms.DialogResult.OK Then
                tstrID = mstrSelectedID
                Return True
            End If
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Function

    Private Sub Gombok()
        Try
            Dim llogChanged As Boolean = False

        Catch
        End Try
    End Sub

    Private Sub frmList_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            Me.Clear()
        Catch
        End Try
    End Sub

    Private Sub frmList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If mlogAutoStart Then Me.Fill()
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub frmList_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Try
            Select Case e.KeyCode
                Case System.Windows.Forms.Keys.Escape
                    Me.Close()

                Case System.Windows.Forms.Keys.Enter, System.Windows.Forms.Keys.Return
                    If mlogSelect Then
                        mstrSelectedID = Me.GetField()
                        Me.DialogResult = System.Windows.Forms.DialogResult.OK
                    End If

            End Select
        Catch
        End Try
    End Sub

    Private Sub frmList_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Try
            cboWhere.Width = tlsToolStrip.ClientSize.Width - tsbRefresh.Width - 25
        Catch
        End Try
    End Sub

    Private Sub Clear()
        Try
            If mobjTable IsNot Nothing Then mobjTable.Dispose()
            mobjTable = Nothing
            If mobjCB IsNot Nothing Then mobjCB.Dispose()
            mobjCB = Nothing
            If mobjDA IsNot Nothing Then mobjDA.Dispose()
            mobjDA = Nothing
            lblMsg.Text = Nothing
            lblRows.Text = Nothing
        Catch
        End Try
    End Sub

    Public Function Fill() As Boolean
        Dim lstrSQL As String
        Dim lstrWhere As String
        Dim lintPos1 As Integer
        Dim lintPos2 As Integer
        Try
            Me.Clear()

            lstrSQL = ES(mstrCmd)
            lstrWhere = cboWhere.Text
            If Not IsES(lstrWhere) Then
                If Not cboWhere.Items.Contains(lstrWhere) Then
                    cboWhere.Items.Add(lstrWhere)
                End If

                lintPos1 = lstrSQL.ToLower.IndexOf(" where ")

                lintPos2 = lstrSQL.ToLower.IndexOf(" group by ")
                If lintPos2 < 0 Then lintPos2 = lstrSQL.ToLower.IndexOf(" having ")
                If lintPos2 < 0 Then lintPos2 = lstrSQL.ToLower.IndexOf(" order by ")
                If lintPos2 < 0 Then lintPos2 = lstrSQL.ToLower.IndexOf(" limit ")
                If lintPos2 < 0 Then lintPos2 = lstrSQL.ToLower.IndexOf(" into ")
                If lintPos2 < 0 Then lintPos2 = lstrSQL.ToLower.IndexOf(" for update ")
                If lintPos2 < 0 Then lintPos2 = lstrSQL.Length

                If lintPos1 < 0 Then
                    'Ha nincs WHERE:
                    lstrSQL = lstrSQL.Insert(lintPos2, " where " & lstrWhere)
                Else
                    'Ha van WHERE:
                    lintPos1 += " where ".Length
                    lstrSQL = lstrSQL.Insert(lintPos2, ") and (" & lstrWhere & ")")
                    lstrSQL = lstrSQL.Insert(lintPos1, "(")
                End If
            End If
            lblMsg.Text = lstrSQL

            'Kép oszlop hozzáadása:
            Me.AddPictCol()

            mobjDA = New MySql.Data.MySqlClient.MySqlDataAdapter(lstrSQL, App.Connection)
            mobjCB = New MySql.Data.MySqlClient.MySqlCommandBuilder(mobjDA)
            mobjTable = New System.Data.DataTable()
            mobjTable.Locale = System.Globalization.CultureInfo.InvariantCulture
            mobjDA.Fill(mobjTable)
            grdList.DataSource = mobjTable

            'Képek feltöltése:
            If Not IsES(mstrPictSource) AndAlso Not IsES(mstrPictCol) AndAlso grdList.Columns.Contains(mstrPictCol) AndAlso mobjTable.Columns.Contains(mstrPictSource) Then
                For I As Integer = 0 To mobjTable.Rows.Count - 1
                    Dim lstrPict As String
                    lstrPict = ES(mobjTable.Rows(I).Item(mstrPictSource))
                    If Not IsES(lstrPict) Then
                        lstrPict = System.IO.Path.Combine(mstrPictRoot, lstrPict)
                        If System.IO.File.Exists(lstrPict) Then
                            grdList.Item(mstrPictCol, I).Value = New System.Drawing.Bitmap(lstrPict)
                        End If
                    End If
                Next
                grdList.AutoResizeRows()
            End If

            grdList.AutoResizeColumns()
            lblRows.Text = grdList.Rows.Count & " rows"

            Return True
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Function

    Private Sub Refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
     Handles mnuRefresh.Click, tsbRefresh.Click
        Try
            Me.Fill()
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub mnuClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuClose.Click
        Try
            Me.Close()
        Catch
        End Try
    End Sub

    Private Sub mnuSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSave.Click
        Try
            mobjDA.Update(mobjTable)
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub mnuCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCancel.Click
        Try
            mobjDA.Fill(mobjTable)
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub mnuNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNew.Click
        Try
            mobjTable.NewRow()
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub mnuDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDelete.Click
        Try

        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub AddPictCol()
        Try
            If Not IsES(mstrPictSource) AndAlso Not IsES(mstrPictCol) AndAlso Not grdList.Columns.Contains(mstrPictCol) Then
                Dim lobjColumn As New System.Windows.Forms.DataGridViewImageColumn()
                With lobjColumn
                    .Name = mstrPictCol
                    .HeaderText = ""
                    .ReadOnly = True
                    .Width = 20
                    .Frozen = True
                End With
                grdList.Columns.Insert(0, lobjColumn)
            End If
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Private Sub grdList_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdList.CellDoubleClick
        If mlogSelect Then
            mstrSelectedID = Me.GetField(, e.RowIndex)
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub grdList_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles grdList.DataError
        e.Cancel = True
    End Sub

    Public Sub AddFunction(ByVal tstrCaption As String, Optional ByRef tobjEventHandler As System.EventHandler = Nothing, Optional ByVal tintKeys As System.Windows.Forms.Keys = 0)
        Try
            Dim lobjItem As System.Windows.Forms.ToolStripItem = mnuFunctions.DropDownItems.Add(tstrCaption, Nothing, tobjEventHandler)
            If TypeOf lobjItem Is System.Windows.Forms.ToolStripMenuItem Then
                CType(lobjItem, System.Windows.Forms.ToolStripMenuItem).ShortcutKeys = tintKeys
            End If
            mnuFunctions.Visible = True
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Public Function GetField(Optional ByVal tstrField As String = "id", Optional ByVal tintRow As Integer = -1) As String
        If tintRow < 0 Then
            If grdList.CurrentRow IsNot Nothing AndAlso mobjTable.Columns.Contains(tstrField) Then
                Return ES(mobjTable.Rows(grdList.CurrentRow.Index).Item(tstrField))
            End If
        Else
            If tintRow < grdList.Rows.Count AndAlso mobjTable.Columns.Contains(tstrField) Then
                Return ES(mobjTable.Rows(tintRow).Item(tstrField))
            End If
        End If
        Return String.Empty
    End Function

    Public Function GetSelection() As Integer()
        Dim lintaRows() As Integer
        Dim lcolRows As System.Windows.Forms.DataGridViewSelectedRowCollection = grdList.SelectedRows
        If lcolRows Is Nothing Then
            ReDim lintaRows(-1)
        ElseIf lcolRows.Count = 0 AndAlso grdList.CurrentRow IsNot Nothing Then
            ReDim lintaRows(0)
            lintaRows(0) = grdList.CurrentRow.Index
        Else
            ReDim lintaRows(lcolRows.Count - 1)
            For I As Integer = 0 To lcolRows.Count - 1
                lintaRows(I) = lcolRows(I).Index
            Next
        End If
        Return lintaRows
    End Function

End Class
