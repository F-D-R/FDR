Option Explicit On
Option Strict On

Public Class frmFtpProba

    Private mstrHost As String = "ftp.dataglobe.hu"
    Private mstrLogin As String = "fdr"
    Private mstrPwd As String = "eos10d"

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        Dim lobjConn As FTPConnection = Nothing
        Dim lstrMsg As String = Nothing
        Dim llngBegin As Long
        Try
            Me.Cursor = Cursors.WaitCursor
            llngBegin = Now.Ticks

            lobjConn = New FTPConnection()
            With lobjConn
                .Open(mstrHost, mstrLogin, mstrPwd, FTPMode.Passive)
                lstrMsg &= "Open: " & (Now.Ticks - llngBegin) \ 10000 & "ms" & vbCrLf
                llngBegin = Now.Ticks


                If False Then
                    If False Then
                        .SetCurrentDirectory("/FtpClient")
                        lstrMsg &= "SetCurrentDirectory: " & (Now.Ticks - llngBegin) \ 10000 & "ms" & vbCrLf
                        llngBegin = Now.Ticks
                    End If
                    Dim lcolTmp As ArrayList = .Dir()
                    lstrMsg &= "Dir: " & (Now.Ticks - llngBegin) \ 10000 & "ms" & vbCrLf
                    llngBegin = Now.Ticks
                    For Each lobjTmp As Object In lcolTmp
                        lstrMsg &= lobjTmp.ToString & vbCrLf
                    Next
                    lstrMsg &= "List: " & (Now.Ticks - llngBegin) \ 10000 & "ms" & vbCrLf
                    llngBegin = Now.Ticks
                ElseIf False Then
                    If False Then
                        .SetCurrentDirectory("/FtpClient")
                        lstrMsg &= "SetCurrentDirectory: " & (Now.Ticks - llngBegin) \ 10000 & "ms" & vbCrLf
                        llngBegin = Now.Ticks
                    End If
                    Dim lcolTmp As ArrayList = .XDir()
                    lstrMsg &= "XDir: " & (Now.Ticks - llngBegin) \ 10000 & "ms" & vbCrLf
                    llngBegin = Now.Ticks
                    For Each lobjTmp As Object In lcolTmp
                        If TypeOf lobjTmp Is ArrayList Then
                            For Each lobjCol As Object In CType(lobjTmp, ArrayList)
                                lstrMsg &= lobjCol.ToString & "  "
                            Next
                        End If
                        lstrMsg &= vbCrLf
                    Next
                    lstrMsg &= "List: " & (Now.Ticks - llngBegin) \ 10000 & "ms" & vbCrLf
                    llngBegin = Now.Ticks
                ElseIf True Then
                    If False Then
                        .SetCurrentDirectory("/FtpClient/proba")
                        lstrMsg &= "SetCurrentDirectory: " & (Now.Ticks - llngBegin) \ 10000 & "ms" & vbCrLf
                        llngBegin = Now.Ticks
                    End If
                    .SendFile("c:\proba1.jpg", FTPFileTransferType.Binary)
                    lstrMsg &= "SendFile: " & (Now.Ticks - llngBegin) \ 10000 & "ms" & vbCrLf
                    llngBegin = Now.Ticks

                    If False Then
                        .SetCurrentDirectory("/FtpClient/proba2")
                        lstrMsg &= "SetCurrentDirectory: " & (Now.Ticks - llngBegin) \ 10000 & "ms" & vbCrLf
                        llngBegin = Now.Ticks
                    End If
                    .SendFile("c:\proba2.jpg", FTPFileTransferType.Binary)
                    lstrMsg &= "SendFile: " & (Now.Ticks - llngBegin) \ 10000 & "ms" & vbCrLf
                    llngBegin = Now.Ticks
                End If


                .Close()
            End With
            lobjConn = Nothing

            MsgBox(lstrMsg, MsgBoxStyle.Information)
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Exclamation)
        Finally
            If lobjConn IsNot Nothing Then
                Try
                    lobjConn.Close()
                Catch
                End Try
            End If
            Me.Cursor = Cursors.Default
        End Try
    End Sub

End Class
