Imports System.Linq
Imports System.Collections.Generic
Imports System.Text
Public Class wfADSBReviewRegisterReport
    Inherits System.Web.UI.Page

#Region " Variable Declaration "



    Dim EventLogDetail As String
    Dim StartDate As String
    Dim EndDate As String
    Dim Text, No, ADSBNo As String
    Dim mDistinctADSBText As DistinctADSBText

    Private mID As Guid
    Private mADSBTechRecordingDate As SmartDate
    Private mIssueDate As SmartDate
    Private mNumber As String
    Private mNo As Decimal
    Private mDescription As String
    Private mADSBNo As String
    Private mSrNo As Integer
    Private mADSBStep As String
    Public mADSBR As ADSBReviewRegisterReport

    Dim EventLogID As Guid
    Public mEventLog As EventLog
    Public mUser As User

    Dim DateIndex, FromDate, ToDate As String





#End Region

#Region " Methods "

    Private Sub GetSession()
        mUser = Session("mUser")
        mDistinctADSBText = Session("mDistinctADSBText")
        Text = Session("Text")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
 
    End Sub
    Private Sub SetSession()
        Session("mDistinctADSBText") = mDistinctADSBText
        Session("Text") = Text
        Session("No") = No
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mUser")
        Session.Remove("mDistinctADSBText")
        Session.Remove("Text")
        Session.Remove("No")
    End Sub
    Private Sub ControlVisibility2()
 
        lblDateRangeFrom.Visible = True
        lblNo.Visible = True
        lblADSBNo.Visible = True

    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfADSBReviewRegisterReport.aspx?" Then
            Session.Remove("mUser")
        End If


    End Sub
    Private Sub SetValues()
        If Not IsDate(txtFromDate.Text) Then
            StartDate = ""
        Else
            StartDate = txtFromDate.Text
        End If
        If Not IsDate(txtToDate.Text) Then
            EndDate = ""
        Else
            EndDate = txtToDate.Text
        End If
        'If Not IsDate(TextIssueDate.Text) Then
        '    EndDate = ""
        'Else
        '    EndDate = TextIssueDate.Text
        'End If


        Text = IIf(cmbNo.SelectedItem.Text = "" Or cmbNo.SelectedItem.Text = "(ALL)", "", cmbNo.SelectedItem.Text)
        No = Val(txtNo.Text.Trim)
        ADSBNo = txtADSBNo.Text.Trim

        'Name = IIf(cmbCustomerList.SelectedItem.Text = "" Or cmbCustomerList.SelectedItem.Text = "(ALL)", "", cmbCustomerList.SelectedItem.Text)



        Dim RegIDForLogo As Guid
        Dim tmpRegIDs As New StringBuilder
        Session("tmpRegIDs") = tmpRegIDs.ToString.TrimEnd(",")
        Session("RegIDForLogo") = RegIDForLogo
        'End
        ' EventLogDetail = StartDate + ", " + EndDate + ",    Status : " + cmbStatus.SelectedItem.Text //====

        'Search Criteria
        FromDate = txtFromDate.Text.ToString
        ToDate = txtToDate.Text.ToString
        lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText
        lblADSBNo.Text = "AD/SB No. : " & txtADSBNo.Text.Trim
        lblNo.Text = "No. : " & IIf(Text = "", "(ALL)", Text + IIf(txtNo.Text.Trim = "0" Or txtNo.Text = "", "", "-" + txtNo.Text.Trim))





    End Sub

    Public Sub DataFieldBind()
        txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
        txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
        'TextIssueDate.Text = Now.Date.ToString(AppSettings("DateFormat"))

        mUser = CType(Session("mUser"), User)
        mEventLog = Session("mEventLog")

        mDistinctADSBText = DistinctADSBText.GetDistinctTextList(True, "(ALL)")
        cmbNo.DataSource = mDistinctADSBText
        Session("mDistinctADSBText") = mDistinctADSBText
 
        DataBind()
    End Sub
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        ClearAll()
        GetSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfADSBReviewRegisterReport.aspx?"
            mEventLog = EventLog.GetEventLog(CType(Session("EventLogID"), Guid))
            Session("mEventLog") = mEventLog

            DataFieldBind()

        End If
    End Sub

    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid Then
            ControlVisibility2()
            SetValues()

            upnlDisplaySearchCriteria.Update()
        End If
    End Sub
    Private Sub btnDisplay_Click(sender As Object, e As System.EventArgs) Handles btnDisplay.Click

        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim rpt As ADSBReviewRegisterReport
        Dim ds As New dsADSBReviewRegisterReport
        myReport = New crptADSBReviewRegisterReport




        SetValues()
        Dim MNo As String
        If Trim(txtNo.Text) = "0" Or txtNo.Text = "" Then
            MNo = ""
        Else
            MNo = "-" + txtNo.Text
        End If

        rpt = ADSBReviewRegisterReport.GetADSBReviewRegisterList(FromDate:=StartDate, ToDate:=EndDate, Text:=Text, No:=No, ADSBNo:=ADSBNo)
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1366)
            MarkLog(Util.Action.Print, "ADSBReviewRegister", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
              mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
              mCompanyDetail.WebSite, "", New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, "", _
              "", "", ProductVersion:=AppSettings("Product Version"), SearchStr10:=AppSettings("Logo"), SearchStr11:=AppSettings("MROISONo"), _
              SearchStr12:="TELEFAX:" & mCompanyDetail.Fax & " " & mCompanyDetail.Email, SearchStr13:=txtADSBNo.Text.ToString, SearchStr14:="", SearchStr16:="", SearchStr15:="", _
              SearchStr17:="", SearchStr18:="", SearchStr19:="", SearchStr20:="", _
              SearchStr21:="", SearchStr22:="", SearchStr23:="", SearchStr24:="", _
              SearchStr25:="", SearchStr6:="", SearchStr7:=IIf(Text = "", "(ALL)", cmbNo.SelectedItem.ToString + MNo), SearchStr8:="", SearchStr9:="", SINote:=AppSettings("SINote"))



        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)


        da.Fill(ds, rpt)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub


    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

End Class