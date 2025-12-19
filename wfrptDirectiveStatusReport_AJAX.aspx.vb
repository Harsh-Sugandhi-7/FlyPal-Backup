

'CREATED : SAYLEE
'DATED   : 24-Jan-2014


Public Class wfrptDirectiveStatusReport_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim Directive As String
    Dim WithCompliance As String

    Private mDistinctModificationNumberList As DistinctModificationNumber
    Dim EventLog As Guid
    Public DirCriteria As String = String.Empty
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        Directive = IIf(IsNothing(Directive), "", Directive)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptDirectiveStatusReport_AJAX.aspx" Then
            Session.Remove("Directive")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub Display()
        ''lblDateRangeFrom.Visible = True
        ''lblDateRangeTo.Visible = True
        lblDirective1.Visible = True
    End Sub
    Private Sub SetValues()
        ''If Not (txtFromDate.IsDateValue) Then
        ''    StartDate = ""
        ''Else
        ''    StartDate = txtFromDate.Value.ToString
        ''End If
        ''If Not (txtToDate.IsDateValue) Then
        ''    EndDate = ""
        ''Else
        ''    EndDate = txtToDate.Value.ToString
        ''End If

        Directive = IIf(cmbModificationNumber.SelectedIndex > 0, cmbModificationNumber.SelectedItem.Text, "") 'mDistinctModificationNumberList((cmbModificationNumber.SelectedIndex)).ModificationNumber 'txtDirective.Text

        'lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
        'lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
        lblDirective1.Text = "Directive : " & IIf(cmbModificationNumber.SelectedIndex > 0, cmbModificationNumber.SelectedItem.Text, "All")
        WithCompliance = IIf(chkComplHist.Checked = True, " ( With Compliance History )", "")

        DirCriteria = lblDirective1.Text + WithCompliance
    End Sub
    Private Sub ResetValues()
        Directive = ""
    End Sub
    Private Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsrptDirectiveStatusReport
        Dim Obj As rptDirectiveStatusReport

        Dim mCompanyDetail As New CompanyDetail
        SetValues()

        Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Directive Status Report", Directive, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        'Obj = rptDirectiveStatusReport.GetrptDirectiveStatusReport(Directive)
        Obj = rptDirectiveStatusReport.GetrptDirectiveStatusList(Directive)

        If Obj.Count <= 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfrptDirectiveStatusReport.aspx?Backpage="
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")

            Exit Sub
        Else

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1133)
        End If
        ds.Clear()
        da.Fill(ds, Obj)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        '************************Report Show ***************************

        If chkComplHist.Checked = True Then
            myReport = New crptDirectiveStatusReportWithComplianceHist
        Else
            myReport = New crptDirectiveStatusReport
        End If


        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "DirectiveStatusReport", DirCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)

    End Sub

#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        'Dim custValidator As CustomValidator
        'custValidator = CType(s, CustomValidator)
        'If custValidator.ControlToValidate = "cmbModificationNumber" Then
        '    If cmbModificationNumber.SelectedIndex = 0 Then
        '        custValidator.ErrorMessage = "Please select the Directive"
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'End If
    End Sub
    Private Sub DataFieldBind()
        mDistinctModificationNumberList = DistinctModificationNumber.GetDistinctModificationNumberList("(All)")
        cmbModificationNumber.DataSource = mDistinctModificationNumberList
        cmbModificationNumber.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptDirectiveStatusReport_AJAX.aspx"
            DataFieldBind()
            setFocus(cmbModificationNumber)
            ResetValues()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("Directive")
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

#End Region

End Class