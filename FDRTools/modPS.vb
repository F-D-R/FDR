Option Explicit On
Option Strict Off

Friend Module modPS

    Function WebResize(ByVal tstrSourceDir As String, Optional ByVal tstrAction As String = "web resize", Optional ByVal tintMinSize As Integer = 60000) As Boolean
        Dim lobjPS As Object = Nothing
        Dim lobjDoc As Object = Nothing
        Dim lstraFiles() As String
        Try
            'Forrás könyvtár ellenõrzése:
            If IsES(tstrSourceDir) Then Throw New ArgumentNullException(tstrAction)
            If Not IO.Directory.Exists(tstrSourceDir) Then Throw New IO.DirectoryNotFoundException()
            'Action nevének ellenõrzése:
            If IsES(tstrAction) Then Throw New ArgumentNullException(tstrAction)

            'Célkönyvtárak törlése:
            IO.Directory.Delete(gstrTmpPictDir, True)
            IO.Directory.Delete(gstrTmpThumbDir, True)

            'Célkönyvtárak létrehozása:
            If Not IO.Directory.Exists(gstrTmpPictDir) Then IO.Directory.CreateDirectory(gstrTmpPictDir)
            If Not IO.Directory.Exists(IO.Path.Combine(gstrTmpPictDir, "50")) Then IO.Directory.CreateDirectory(IO.Path.Combine(gstrTmpPictDir, "50"))
            If Not IO.Directory.Exists(IO.Path.Combine(gstrTmpPictDir, "60")) Then IO.Directory.CreateDirectory(IO.Path.Combine(gstrTmpPictDir, "60"))
            If Not IO.Directory.Exists(IO.Path.Combine(gstrTmpPictDir, "70")) Then IO.Directory.CreateDirectory(IO.Path.Combine(gstrTmpPictDir, "70"))
            If Not IO.Directory.Exists(IO.Path.Combine(gstrTmpPictDir, "80")) Then IO.Directory.CreateDirectory(IO.Path.Combine(gstrTmpPictDir, "80"))
            If Not IO.Directory.Exists(IO.Path.Combine(gstrTmpPictDir, "90")) Then IO.Directory.CreateDirectory(IO.Path.Combine(gstrTmpPictDir, "90"))
            If Not IO.Directory.Exists(gstrTmpThumbDir) Then IO.Directory.CreateDirectory(gstrTmpThumbDir)

            'Photoshop alkalmazás objektum létrehozása:
            lobjPS = CreateObject("Photoshop.Application")
            lobjPS.BringToFront()
            lobjPS.DisplayDialogs = 3 'psDisplayNoDialogs

            'Photoshop Action végrehajtása a fájlokon:
            lstraFiles = IO.Directory.GetFiles(tstrSourceDir, "*.jpg")
            For Each lstrFile As String In lstraFiles
                lobjDoc = lobjPS.Open(lstrFile)
                lobjPS.DoAction(tstrAction, "FDR")
                Try
                    lobjDoc.Close(2) 'psDoNotSaveChanges
                Catch
                End Try
                System.Runtime.InteropServices.Marshal.ReleaseComObject(lobjDoc)
                GC.KeepAlive(lobjDoc)
                lobjDoc = Nothing
            Next

            'A megfelelõ méretû fájlok kigyûjtése:
            For Each lstrFile As String In lstraFiles
                Dim lstrName As String = IO.Path.GetFileName(lstrFile)
                If FileLen(IO.Path.Combine(IO.Path.Combine(gstrTmpPictDir, "50"), lstrName)) > tintMinSize Then
                    IO.File.Move(IO.Path.Combine(IO.Path.Combine(gstrTmpPictDir, "50"), lstrName), IO.Path.Combine(gstrTmpPictDir, lstrName))
                ElseIf FileLen(IO.Path.Combine(IO.Path.Combine(gstrTmpPictDir, "60"), lstrName)) > tintMinSize Then
                    IO.File.Move(IO.Path.Combine(IO.Path.Combine(gstrTmpPictDir, "60"), lstrName), IO.Path.Combine(gstrTmpPictDir, lstrName))
                ElseIf FileLen(IO.Path.Combine(IO.Path.Combine(gstrTmpPictDir, "70"), lstrName)) > tintMinSize Then
                    IO.File.Move(IO.Path.Combine(IO.Path.Combine(gstrTmpPictDir, "70"), lstrName), IO.Path.Combine(gstrTmpPictDir, lstrName))
                ElseIf FileLen(IO.Path.Combine(IO.Path.Combine(gstrTmpPictDir, "80"), lstrName)) > tintMinSize Then
                    IO.File.Move(IO.Path.Combine(IO.Path.Combine(gstrTmpPictDir, "80"), lstrName), IO.Path.Combine(gstrTmpPictDir, lstrName))
                Else
                    IO.File.Move(IO.Path.Combine(IO.Path.Combine(gstrTmpPictDir, "90"), lstrName), IO.Path.Combine(gstrTmpPictDir, lstrName))
                End If
            Next

            'Fölösleges fájlok és könyvtárak törlése:
            IO.Directory.Delete(IO.Path.Combine(gstrTmpPictDir, "50"), True)
            IO.Directory.Delete(IO.Path.Combine(gstrTmpPictDir, "60"), True)
            IO.Directory.Delete(IO.Path.Combine(gstrTmpPictDir, "70"), True)
            IO.Directory.Delete(IO.Path.Combine(gstrTmpPictDir, "80"), True)
            IO.Directory.Delete(IO.Path.Combine(gstrTmpPictDir, "90"), True)

            Return True
        Catch ex As Exception
            App.AddErr(ex)
        Finally
            If lobjDoc IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(lobjDoc)
            End If
            If lobjPS IsNot Nothing Then
                lobjPS.Quit()
                System.Runtime.InteropServices.Marshal.ReleaseComObject(lobjPS)
                GC.KeepAlive(lobjPS)
            End If
        End Try
    End Function

End Module
