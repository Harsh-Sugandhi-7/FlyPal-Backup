Partial Class footer
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        lblDateFormat.Text = "DateTime Format: (" & Flypal.Util.WebDateFormat & " " & Flypal.Util.WebTimeFormat & ")"  'AppSettings("DateFormat")
        'Most Recent Item Menu:
        ''If (Now.Date = DateSerial(Year(Now.Date), 8, 12)) Or (Now.Date = DateSerial(Year(Now.Date), 8, 13)) Or (Now.Date = DateSerial(Year(Now.Date), 8, 14)) Then
        ''    FlagImage.Visible = True
        ''    FlagImage.ToolTip = "Happy Indapendance Week"
        ''ElseIf (Now.Date = DateSerial(Year(Now.Date), 8, 15)) Then
        ''    FlagImage.Visible = True
        ''    FlagImage.ToolTip = "Happy Indapendance Day"
        ''End If

        'If (Now.Date >= DateSerial(Year(Now.Date), 8, 14)) And (Now.Date <= DateSerial(Year(Now.Date), 8, 20)) Then
        '    FlagImage.Visible = True
        '    If (Now.Date = DateSerial(Year(Now.Date), 8, 15)) Then
        '        FlagImage.ToolTip = "Happy Indapendance Day"
        '    Else
        '        FlagImage.ToolTip = "Happy Indapendance Week"
        '    End If
        'Else
        '    FlagImage.Visible = False
        'End If


        If AppSettings("ClientCode") <> "7AR" Then
            If ((Now.Date >= DateSerial(Year(Now.Date), 8, 14)) And (Now.Date <= DateSerial(Year(Now.Date), 8, 16)) Or (Now.Date >= DateSerial(Year(Now.Date), 1, 25)) And (Now.Date <= DateSerial(Year(Now.Date), 1, 27))) Then
                FlagImage.Visible = True
                If (Now.Date = DateSerial(Year(Now.Date), 8, 15)) Then
                    FlagImage.ToolTip = "Happy Independance Day"
                Else
                    If (Now.Date >= DateSerial(Year(Now.Date), 8, 14)) And (Now.Date <= DateSerial(Year(Now.Date), 8, 16)) Then
                        FlagImage.ToolTip = "Happy Independance Week"
                    Else
                        If (Now.Date = DateSerial(Year(Now.Date), 1, 26)) Then
                            FlagImage.ToolTip = "Happy Republic Day"
                        Else
                            If (Now.Date >= DateSerial(Year(Now.Date), 1, 25)) And (Now.Date <= DateSerial(Year(Now.Date), 1, 27)) Then
                                FlagImage.ToolTip = "Happy Republic Week"
                            End If
                        End If
                    End If
                End If
            Else
                FlagImage.Visible = False
            End If
        Else
            FlagImage.Visible = False
        End If
    End Sub
    Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Error
        Session("Message") = Context.Server.GetLastError.Message
        Session("Source") = Context.Server.GetLastError.Source
        Session("Trace") = Context.Server.GetLastError.StackTrace
    End Sub
#End Region

End Class
