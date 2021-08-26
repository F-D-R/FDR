Option Explicit On
Option Strict On

Friend Class App

    Private Shared mstrConn As String
    Private Shared mobjConn As MySql.Data.MySqlClient.MySqlConnection
    Private Shared mxmlConfig As System.Xml.XmlDocument

    Public Sub New()
    End Sub
    Shared Sub New()
        Try
            mxmlConfig = New System.Xml.XmlDocument()
            If System.IO.File.Exists(App.ConfigFile) Then mxmlConfig.Load(App.ConfigFile)
        Catch
        End Try
    End Sub

    Public Shared ReadOnly Property ConfigFile() As String
        Get
            Return System.Reflection.Assembly.GetExecutingAssembly.Location & ".config"
        End Get
    End Property

    Public Shared Function GetSetting(ByVal tstrName As String, Optional ByVal tstrDefaultValue As String = Nothing) As String
        Try
            Dim lxmlNode As System.Xml.XmlNode
            Dim lstrSection As String = Nothing
            Dim lstrValue As String

            lxmlNode = mxmlConfig.SelectSingleNode("configuration/appSettings/add[@key='Section']")
            If lxmlNode IsNot Nothing Then
                lstrSection = lxmlNode.Attributes("value").Value
                If ES(lstrSection).ToUpper = "AUTO" Then lstrSection = System.Environment.MachineName
            End If
            If IsES(lstrSection) Then lstrSection = "appSettings"

            lxmlNode = mxmlConfig.SelectSingleNode("configuration/" & lstrSection & "/add[@key='" & tstrName & "']")
            If lxmlNode Is Nothing Then
                lxmlNode = mxmlConfig.SelectSingleNode("configuration/appSettings/add[@key='" & tstrName & "']")
            End If
            If lxmlNode IsNot Nothing Then
                lstrValue = lxmlNode.Attributes("value").Value
                Return ES(lstrValue)
            Else
                Return ES(tstrDefaultValue)
            End If
        Catch ex As System.Exception
            System.Windows.Forms.MessageBox.Show(ex.ToString)
            Return ES(tstrDefaultValue)
        End Try
    End Function

    Public Shared ReadOnly Property DBConn() As String
        Get
            Return App.GetSetting("DBConn", "server=localhost;database=fdr")
        End Get
    End Property

    Public Shared ReadOnly Property Connection() As MySql.Data.MySqlClient.MySqlConnection
        Get
            Return mobjConn
        End Get
    End Property

    Public Shared ReadOnly Property IsConnected() As Boolean
        Get
            Return mobjConn IsNot Nothing AndAlso mobjConn.State = System.Data.ConnectionState.Open
        End Get
    End Property

    Public Shared Function Connect(Optional ByVal tstrConn As String = Nothing) As Boolean
        Try
            App.Disconnect()

            If IsES(tstrConn) Then
                mstrConn = App.DBConn
            Else
                mstrConn = tstrConn
            End If
            mobjConn = New MySql.Data.MySqlClient.MySqlConnection(mstrConn)
            mobjConn.Open()

            Return True
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Function

    Public Shared Sub Disconnect()
        Try
            If mobjConn IsNot Nothing Then
                mobjConn.Close()
                mobjConn.Dispose()
            End If
            mobjConn = Nothing
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Public Shared Sub AddErr(ByVal tobjEx As System.Exception, Optional ByVal tstrInfo As String = Nothing, Optional ByVal tlogMsg As Boolean = True)
        Try
            Dim lstrMsg As String = tobjEx.ToString
            If Not String.IsNullOrEmpty(tstrInfo) Then lstrMsg &= System.Environment.NewLine & System.Environment.NewLine & tstrInfo
            If tlogMsg Then System.Windows.Forms.MessageBox.Show(lstrMsg, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation)
        Catch
        End Try
    End Sub

    Public Shared ReadOnly Property LogDir() As String
        Get
            Return App.GetSetting("LogDir", "D:\Temp\")
        End Get
    End Property

    Public Shared ReadOnly Property SourceRoot() As String
        Get
            Return App.GetSetting("SourceRoot", "D:\FDR\")
        End Get
    End Property

    Public Shared ReadOnly Property DestRoot() As String
        Get
            Return App.GetSetting("DestRoot", "D:\WEB\fdr_hu\www\")
        End Get
    End Property

    Public Shared ReadOnly Property RemoteRoot() As String
        Get
            Return App.GetSetting("RemoteRoot", "/fdr_hu/www/")
        End Get
    End Property

    Public Shared ReadOnly Property PictDirName() As String
        Get
            Return App.GetSetting("PictDirName", "pictures")
        End Get
    End Property

    Public Shared ReadOnly Property ThumbDirName() As String
        Get
            Return App.GetSetting("ThumbDirName", "thumbnails")
        End Get
    End Property

    Public Shared ReadOnly Property DestPictDir() As String
        Get
            Return System.IO.Path.Combine(App.DestRoot, App.PictDirName)
        End Get
    End Property

    Public Shared ReadOnly Property DestThumbDir() As String
        Get
            Return System.IO.Path.Combine(App.DestRoot, App.ThumbDirName)
        End Get
    End Property

    Public Shared ReadOnly Property TmpPictDir() As String
        Get
            Return App.GetSetting("TmpPictDir", "D:\Temp\pictures\")
        End Get
    End Property

    Public Shared ReadOnly Property TmpThumbDir() As String
        Get
            Return App.GetSetting("TmpThumbDir", "D:\Temp\thumbnails\")
        End Get
    End Property

End Class


Friend Module Core

    'Public Sub Main()
    '    Try
    '        Using lobjForm As New frmLicense
    '            If lobjForm.ShowDialog() <> DialogResult.OK Then End
    '        End Using
    '        Using lobjForm As New frmMBJK
    '            lobjForm.ShowDialog()
    '        End Using
    '    Catch ex As Exception
    '        App.AddErr(ex)
    '    End Try
    'End Sub

    Public Function ES(ByRef tstrString As String) As String
        If tstrString Is Nothing Then Return String.Empty
        Return tstrString
    End Function
    Public Function ES(ByVal tobjString As Object) As String
        If tobjString Is System.DBNull.Value OrElse tobjString Is Nothing Then Return String.Empty
        Return tobjString.ToString
    End Function

    Public Function IsES(ByVal tstrString As String, Optional ByVal tlogTrim As Boolean = True) As Boolean
        If tstrString Is Nothing Then Return True
        If tstrString = Nothing Then Return True
        If tlogTrim AndAlso tstrString.Trim = Nothing Then Return True
        Return False
    End Function
    Public Function IsES(ByVal tobjString As Object, Optional ByVal tlogTrim As Boolean = True) As Boolean
        If tobjString Is System.DBNull.Value Then Return True
        Return IsES(CStr(tobjString), tlogTrim)
    End Function

    Public Sub ClearAllControls(ByVal tobjControl As System.Windows.Forms.Control, Optional ByVal tlogRecursive As Boolean = True)
        Dim lobjControl As System.Windows.Forms.Control
        Try
            If TypeOf tobjControl Is System.Windows.Forms.TextBoxBase Then CType(tobjControl, System.Windows.Forms.TextBoxBase).Clear()
            If TypeOf tobjControl Is System.Windows.Forms.ComboBox Then CType(tobjControl, System.Windows.Forms.ComboBox).SelectedIndex = -1
            'Változás okoz a formokon!
            'If TypeOf tobjControl Is System.Windows.Forms.CheckBox Then CType(tobjControl, System.Windows.Forms.CheckBox).Checked = False
            If tlogRecursive Then
                For Each lobjControl In tobjControl.Controls
                    Core.ClearAllControls(lobjControl, True)
                Next
            End If
        Catch
        End Try
    End Sub

    Public Sub AssignEnterEvent(ByVal tobjControl As System.Windows.Forms.Control, ByVal tobjEventHandler As System.EventHandler, Optional ByVal tlogRecursive As Boolean = True)
        Dim lobjControl As System.Windows.Forms.Control
        Try
            If TypeOf tobjControl Is System.Windows.Forms.TextBoxBase Then AddHandler tobjControl.Enter, tobjEventHandler
            If tlogRecursive Then
                For Each lobjControl In tobjControl.Controls
                    AssignEnterEvent(lobjControl, tobjEventHandler, True)
                Next
            End If
        Catch
        End Try
    End Sub

    Public Sub EnterEvent(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If TypeOf sender Is System.Windows.Forms.TextBoxBase Then
                CType(sender, System.Windows.Forms.TextBoxBase).SelectAll()
            End If
        Catch
        End Try
    End Sub

    Public Sub AssignTextChangedEvent(ByVal tobjControl As System.Windows.Forms.Control, ByVal tobjEventHandler As System.EventHandler, Optional ByVal tlogRecursive As Boolean = True)
        Dim lobjControl As System.Windows.Forms.Control
        Try
            If TypeOf tobjControl Is System.Windows.Forms.TextBoxBase Then AddHandler tobjControl.TextChanged, tobjEventHandler
            If tlogRecursive Then
                For Each lobjControl In tobjControl.Controls
                    AssignTextChangedEvent(lobjControl, tobjEventHandler, True)
                Next
            End If
        Catch
        End Try
    End Sub

    Public Sub FillCombo(ByVal tobjComboBox As System.Windows.Forms.ComboBox, ByVal tobjDataSource As System.Collections.IList)
        Try
            With tobjComboBox
                .DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
                If IsES(.DisplayMember) Then .DisplayMember = "ToString"
                If IsES(.ValueMember) Then .ValueMember = "Value"
                .DataSource = tobjDataSource
                .SelectedIndex = -1
            End With
        Catch ex As System.Exception
            App.AddErr(ex)
        End Try
    End Sub

    Public Function IsTrue(ByVal tstrText As String) As Boolean
		Try
			If IsES(tstrText) Then Return False
			'If IsNumeric(tstrText) Then Return CInt(tstrText) <> 0
			Dim lintTmp As Integer
			If Integer.TryParse(tstrText, lintTmp) Then
				Return lintTmp <> 0
			End If
			'Yes:
            If tstrText.StartsWith("Y", System.StringComparison.OrdinalIgnoreCase) Then Return True
			'True:
            If tstrText.StartsWith("T", System.StringComparison.OrdinalIgnoreCase) Then Return True
			'Igen, Igaz:
            If tstrText.StartsWith("I", System.StringComparison.OrdinalIgnoreCase) Then Return True
			Return False
		Catch
		End Try
    End Function

    Public Function LikeArray(ByRef tstrText As String, ByRef tstraSamples() As String, Optional ByVal tlogCaseSensitive As Boolean = False) As Boolean
        If tlogCaseSensitive Then
            For Each lstrSample As String In tstraSamples
                If Not IsES(lstrSample) AndAlso ES(tstrText) Like lstrSample Then
                    Return True
                End If
            Next
        Else
            For Each lstrSample As String In tstraSamples
                If Not IsES(lstrSample) AndAlso ES(tstrText).ToLower Like lstrSample.ToLower Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

End Module


Friend Class ListItem
    Private mobjValue As Object
    Private mstrCaption As String

    Public Sub New(ByVal tobjValue As Object, ByVal tstrCaption As String)
        mobjValue = tobjValue
        mstrCaption = tstrCaption
    End Sub

    Public ReadOnly Property Value() As Object
        Get
            Return mobjValue
        End Get
    End Property

    Public ReadOnly Property Caption() As String
        Get
            Return mstrCaption
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return CStr(mobjValue) & ": " & mstrCaption
    End Function

End Class
