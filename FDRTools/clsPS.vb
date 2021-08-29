Option Explicit On
Option Strict Off

Friend Class PS

#Region "Events"

    Public Class ProgressEventArgs
        Inherits System.EventArgs
        Private ReadOnly mintMax As Integer
        Private ReadOnly mintValue As Integer

        Friend Sub New(ByVal tintMax As Integer, Optional ByVal tintValue As Integer = 0)
            If tintValue > tintMax Then tintValue = tintMax
            mintMax = tintMax
            mintValue = tintValue
        End Sub

        Public ReadOnly Property Maximum() As Integer
            Get
                Return mintMax
            End Get
        End Property

        Public ReadOnly Property Value() As Integer
            Get
                Return mintValue
            End Get
        End Property

    End Class

    Public Delegate Sub ProgressEventHandler(ByVal sender As Object, ByVal e As ProgressEventArgs)

    Public Event ProgressInit As ProgressEventHandler
    Public Event ProgressIncrement As System.EventHandler

#End Region

    Public Sub New()
    End Sub

    Public Function WebResize(ByVal tstrSourceDir As String, Optional ByVal tstrSet As String = "FDR", Optional ByVal tstrAction As String = "web resize", Optional ByVal tintMinSize As Integer = 60000) As Boolean
        Dim lobjPS As Object = Nothing
        Dim lobjDoc As Object = Nothing
        Dim lstraFiles() As String
        Dim lstrTmpPictDir As String = App.TmpPictDir
        Dim lstrTmpThumbDir As String = App.TmpThumbDir
        Try
            'Forrás könyvtár ellenõrzése:
            If IsES(tstrSourceDir) Then Throw New System.ArgumentNullException(tstrAction)
            If Not System.IO.Directory.Exists(tstrSourceDir) Then Throw New System.IO.DirectoryNotFoundException()
            'ActionSet nevének ellenõrzése:
            If IsES(tstrSet) Then Throw New System.ArgumentNullException(tstrSet)
            'Action nevének ellenõrzése:
            If IsES(tstrAction) Then Throw New System.ArgumentNullException(tstrAction)

            'Célkönyvtárak törlése:
            If System.IO.Directory.Exists(lstrTmpPictDir) Then System.IO.Directory.Delete(lstrTmpPictDir, True)
            If System.IO.Directory.Exists(lstrTmpThumbDir) Then System.IO.Directory.Delete(lstrTmpThumbDir, True)

            'Célkönyvtárak létrehozása:
            If Not System.IO.Directory.Exists(lstrTmpPictDir) Then System.IO.Directory.CreateDirectory(lstrTmpPictDir)
            If Not System.IO.Directory.Exists(System.IO.Path.Combine(lstrTmpPictDir, "50")) Then System.IO.Directory.CreateDirectory(System.IO.Path.Combine(lstrTmpPictDir, "50"))
            If Not System.IO.Directory.Exists(System.IO.Path.Combine(lstrTmpPictDir, "60")) Then System.IO.Directory.CreateDirectory(System.IO.Path.Combine(lstrTmpPictDir, "60"))
            If Not System.IO.Directory.Exists(System.IO.Path.Combine(lstrTmpPictDir, "70")) Then System.IO.Directory.CreateDirectory(System.IO.Path.Combine(lstrTmpPictDir, "70"))
            If Not System.IO.Directory.Exists(System.IO.Path.Combine(lstrTmpPictDir, "80")) Then System.IO.Directory.CreateDirectory(System.IO.Path.Combine(lstrTmpPictDir, "80"))
            If Not System.IO.Directory.Exists(System.IO.Path.Combine(lstrTmpPictDir, "90")) Then System.IO.Directory.CreateDirectory(System.IO.Path.Combine(lstrTmpPictDir, "90"))
            If Not System.IO.Directory.Exists(lstrTmpThumbDir) Then System.IO.Directory.CreateDirectory(lstrTmpThumbDir)

            'Photoshop alkalmazás objektum létrehozása:
            lobjPS = System.Activator.CreateInstance(System.Type.GetTypeFromProgID("Photoshop.Application"))
            lobjPS.BringToFront()
            lobjPS.DisplayDialogs = 3 'psDisplayNoDialogs

            Dim lstraExcludes() As String = App.GetSetting("FileExcludes").Split("|"c)

            'Photoshop Action végrehajtása a fájlokon:
            lstraFiles = System.IO.Directory.GetFiles(tstrSourceDir, "*.jpg")

            RaiseEvent ProgressInit(Me, New ProgressEventArgs(lstraFiles.Length + 1, 1))
            For Each lstrFile As String In lstraFiles
                Dim lstrName As String = System.IO.Path.GetFileName(lstrFile)
                If Not LikeArray(lstrName, lstraExcludes) Then
                    lobjDoc = lobjPS.Open(lstrFile)
                    lobjPS.DoAction(tstrAction, tstrSet)
                    Try
                        lobjDoc.Close(2) 'psDoNotSaveChanges
                    Catch
                    End Try
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(lobjDoc)
                    System.GC.KeepAlive(lobjDoc)
                    lobjDoc = Nothing

                    Dim lstrSmall As String
                    Dim lobjFI As System.IO.FileInfo

                    lstrSmall = System.IO.Path.Combine(System.IO.Path.Combine(lstrTmpPictDir, "50"), lstrName)
                    lobjFI = New System.IO.FileInfo(lstrSmall)
                    If lobjFI.Length > tintMinSize Then
                        System.IO.File.Move(lstrSmall, System.IO.Path.Combine(lstrTmpPictDir, lstrName))
                    Else
                        lstrSmall = System.IO.Path.Combine(System.IO.Path.Combine(lstrTmpPictDir, "60"), lstrName)
                        lobjFI = New System.IO.FileInfo(lstrSmall)
                        If lobjFI.Length > tintMinSize Then
                            System.IO.File.Move(lstrSmall, System.IO.Path.Combine(lstrTmpPictDir, lstrName))
                        Else
                            lstrSmall = System.IO.Path.Combine(System.IO.Path.Combine(lstrTmpPictDir, "70"), lstrName)
                            lobjFI = New System.IO.FileInfo(lstrSmall)
                            If lobjFI.Length > tintMinSize Then
                                System.IO.File.Move(lstrSmall, System.IO.Path.Combine(lstrTmpPictDir, lstrName))
                            Else
                                lstrSmall = System.IO.Path.Combine(System.IO.Path.Combine(lstrTmpPictDir, "80"), lstrName)
                                lobjFI = New System.IO.FileInfo(lstrSmall)
                                If lobjFI.Length > tintMinSize Then
                                    System.IO.File.Move(lstrSmall, System.IO.Path.Combine(lstrTmpPictDir, lstrName))
                                Else
                                    System.IO.File.Move(System.IO.Path.Combine(System.IO.Path.Combine(lstrTmpPictDir, "90"), lstrName), System.IO.Path.Combine(lstrTmpPictDir, lstrName))
                                End If
                            End If
                        End If
                    End If
                End If
                RaiseEvent ProgressIncrement(Me, New System.EventArgs)
            Next

            'Fölösleges fájlok és könyvtárak törlése:
            System.IO.Directory.Delete(System.IO.Path.Combine(lstrTmpPictDir, "50"), True)
            System.IO.Directory.Delete(System.IO.Path.Combine(lstrTmpPictDir, "60"), True)
            System.IO.Directory.Delete(System.IO.Path.Combine(lstrTmpPictDir, "70"), True)
            System.IO.Directory.Delete(System.IO.Path.Combine(lstrTmpPictDir, "80"), True)
            System.IO.Directory.Delete(System.IO.Path.Combine(lstrTmpPictDir, "90"), True)

            Return True
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            If lobjDoc IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(lobjDoc)
            End If
            If lobjPS IsNot Nothing Then
                lobjPS.Quit()
                System.Runtime.InteropServices.Marshal.ReleaseComObject(lobjPS)
                System.GC.KeepAlive(lobjPS)
            End If
        End Try
    End Function

    Public Function WebResize2(ByVal tstrSourceDir As String, Optional ByVal tstrSet As String = "FDR", Optional ByVal tstrAction As String = "web resize", Optional ByVal tintMinSize As Integer = 60000) As Boolean
        Dim lobjPS As Object = Nothing
        Dim lobjDoc As Object = Nothing
        Dim lobjOptions As Object = Nothing
        Dim lstraFiles() As String
        Dim lstrTmpPictDir As String = App.TmpPictDir
        Dim lstrTmpThumbDir As String = App.TmpThumbDir
        Try
            'Forrás könyvtár ellenõrzése:
            If IsES(tstrSourceDir) Then Throw New System.ArgumentNullException(tstrAction)
            If Not System.IO.Directory.Exists(tstrSourceDir) Then Throw New System.IO.DirectoryNotFoundException()

            ''Action nevének ellenõrzése:
            'If IsES(tstrAction) Then Throw New ArgumentNullException(tstrAction)

            'Célkönyvtárak törlése:
            System.IO.Directory.Delete(lstrTmpPictDir, True)
            System.IO.Directory.Delete(lstrTmpThumbDir, True)

            'Célkönyvtárak létrehozása:
            If Not System.IO.Directory.Exists(lstrTmpPictDir) Then System.IO.Directory.CreateDirectory(lstrTmpPictDir)
            If Not System.IO.Directory.Exists(lstrTmpThumbDir) Then System.IO.Directory.CreateDirectory(lstrTmpThumbDir)

            'Photoshop alkalmazás objektum létrehozása:
            'lobjPS = CreateObject("Photoshop.Application")
            lobjPS = System.Activator.CreateInstance(System.Type.GetTypeFromProgID("Photoshop.Application"))
            lobjPS.BringToFront()
            lobjPS.DisplayDialogs = 3 'psDisplayNoDialogs

            lstraFiles = System.IO.Directory.GetFiles(tstrSourceDir, "*.jpg")

            RaiseEvent ProgressInit(Me, New ProgressEventArgs(lstraFiles.Length + 1, 1))
            'lobjOptions = CreateObject("Photoshop.ExportOptionsSaveForWeb")
            lobjOptions = System.Activator.CreateInstance(System.Type.GetTypeFromProgID("Photoshop.ExportOptionsSaveForWeb"))
            For Each lstrFile As String In lstraFiles
                lobjDoc = lobjPS.Open(lstrFile)
                Dim lstrDestFile As String = System.IO.Path.Combine(lstrTmpPictDir, System.IO.Path.GetFileName(lstrFile))

                Me.FitImage(lobjPS, lobjDoc, 600, 600)



                'For lintQuality As Integer = 50 To 90 Step 10
                '    With lobjOptions
                '        .Format = 6 'psJPEGSave
                '        .Interlaced = False
                '        .Quality = lintQuality
                '        .Optimized = True
                '        .Blur = 0
                '        .IncludeProfile = False
                '        '.MatteColor = System.Drawing.Color.White
                '    End With
                '    'lobjDoc.ExportDocument(lstrDestFile, 2, lobjOptions) '2=psSaveForWeb
                '    lobjDoc.ExportDocument(lstrDestFile, 2, lobjOptions) '2=psSaveForWeb
                '    If IO.File.Exists(lstrDestFile) AndAlso FileLen(lstrDestFile) > tintMinSize Then
                '        Exit For
                '    End If
                'Next

                Try
                    lobjDoc.Close(2) 'psDoNotSaveChanges
                Catch
                End Try
                System.Runtime.InteropServices.Marshal.ReleaseComObject(lobjDoc)
                System.GC.KeepAlive(lobjDoc)
                lobjDoc = Nothing

                RaiseEvent ProgressIncrement(Me, New System.EventArgs)
            Next
            System.Runtime.InteropServices.Marshal.ReleaseComObject(lobjOptions)
            System.GC.KeepAlive(lobjOptions)
            lobjOptions = Nothing

            Return True
        Catch ex As System.Exception
            App.AddErr(ex)
        Finally
            If lobjOptions IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(lobjOptions)
            End If
            If lobjDoc IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(lobjDoc)
            End If
            If lobjPS IsNot Nothing Then
                lobjPS.Quit()
                System.Runtime.InteropServices.Marshal.ReleaseComObject(lobjPS)
                System.GC.KeepAlive(lobjPS)
            End If
        End Try
    End Function

    Private Function FitImage(ByRef tobjPS As Object, ByRef tobjDoc As Object, ByVal tintWidth As Integer, ByVal tintHeight As Integer) As Boolean

        Dim ldblDocWidth As Double
        Dim ldblDocHeight As Double
        Dim ldblDocRatio As Double
        Dim ldblNewWidth As Double
        Dim ldblNewHeight As Double
        Dim lintOldUnit As Integer
        Dim ldblResolution As Double

        If tintWidth < 1 OrElse tintHeight < 1 Then Return False 'error

        If tobjDoc Is Nothing Then tobjDoc = tobjPS.ActiveDocument
        ldblResolution = tobjDoc.Resolution

        lintOldUnit = tobjPS.Preferences.RulerUnits 'save old preferences
        tobjPS.Preferences.RulerUnits = 1 'psPixels

        'original width, height
        ldblDocWidth = (tobjDoc.Width * ldblResolution) / 72.0 'decimal inches assuming 72 dpi (used in docRatio)
        ldblDocHeight = (tobjDoc.Height * ldblResolution) / 72.0 'ditto
        'TODO: ha ez tényleg inch, akkor miért nem lehet kisebb egynél?:
        If ldblDocWidth < 1.0 OrElse ldblDocHeight < 1.0 Then Return False 'error

        ldblDocRatio = ldblDocWidth / ldblDocHeight 'decimal ratio of original width/height

        'NOTE - ccox - 17 Aug 2004 - I added the rounding by 0.5
        'this should solve reported cases of fit image being off by 1 (always under)
        'NOTE - elr - 3 May 2006 - keep original aspect ratio
        ldblNewWidth = tintWidth
        ldblNewHeight = CInt((CDbl(tintWidth) / ldblDocRatio) + 0.5) 'decimal calc

        If ldblNewHeight > tintHeight Then
            ldblNewWidth = CInt(0.5 + ldblDocRatio * tintHeight) 'decimal calc
            ldblNewHeight = tintHeight
        End If

        'resize the image using a good conversion method while keeping the pixel resolution
        'and the aspect ratio the same
        tobjDoc.ResizeImage(ldblNewWidth, ldblNewHeight, ldblResolution, 4) '4=psBicubic (5=psBicubicSharper, 6=psBicubicSmoother)
        tobjPS.Preferences.RulerUnits = lintOldUnit 'restore old prefs

        'isCancelled = False 'if get here, definitely executed
        Return True 'no error
    End Function

End Class
