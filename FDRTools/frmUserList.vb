Option Explicit On
Option Strict On

Friend Class frmUserList

    Private mobjDA As MySql.Data.MySqlClient.MySqlDataAdapter
    Private mobjCB As MySql.Data.MySqlClient.MySqlCommandBuilder
    Private mobjTable As Data.DataTable

    Private Sub frmUserList_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            If mobjTable IsNot Nothing Then mobjTable.Dispose()
            mobjTable = Nothing
            If mobjCB IsNot Nothing Then mobjCB.Dispose()
            mobjCB = Nothing
            If mobjDA IsNot Nothing Then mobjDA.Dispose()
            mobjDA = Nothing
        Catch
        End Try
    End Sub

	Private Sub frmUserList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Try
            mobjDA = New MySql.Data.MySqlClient.MySqlDataAdapter("select * from fdr_user", App.Connection)
            mobjCB = New MySql.Data.MySqlClient.MySqlCommandBuilder(mobjDA)
            mobjTable = New Data.DataTable()
            mobjTable.Locale = System.Globalization.CultureInfo.InvariantCulture
            mobjDA.Fill(mobjTable)
            Me.grdList.DataSource = mobjTable
            Me.grdList.AutoResizeColumns()
        Catch ex As Exception
            App.AddErr(ex)
        End Try
	End Sub

	Private Sub frmUserList_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		Try
			If e.KeyCode = Keys.Escape Then
				Me.Close()
			End If
		Catch
		End Try
	End Sub

End Class
