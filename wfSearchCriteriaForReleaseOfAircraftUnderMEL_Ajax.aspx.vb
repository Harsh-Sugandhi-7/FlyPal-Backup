Public Class wfSearchCriteriaForReleaseOfAircraftUnderMEL_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mMachineNameValueList As MachineNameValueList 'Changed By Utkarsh On 19-Apr-2011
    Public mATAList As ATAList
    Dim StartDate As String
    Dim EndDate As String
    Dim MachineID, ATAID As String
    Dim Aircraft, ATAChapter As String
    Public mReleaseOfAircraftUnderMEL As ReleaseOfAircraftUnderMEL
    Dim mReleaseOfAircraftUnderMELSearchingCriteria As String = String.Empty
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mReleaseOfAircraftUnderMEL = CType(Session("mReleaseOfAircraftUnderMEL"), ReleaseOfAircraftUnderMEL)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList) 'Changed By Utkarsh On 19-Apr-2011
        mATAList = CType(Session("mATAList"), ATAList)
    End Sub
    Private Sub SetSession()
        Session("mReleaseOfAircraftUnderMEL") = mReleaseOfAircraftUnderMEL
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mATAList") = mATAList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mReleaseOfAircraftUnderMEL")
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
#End Region

#Region " Helper Methods "
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        lblATAChapter1.Visible = True
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
            EndDate = txtToDate.Text.ToString
        End If

        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        ATAChapter = IIf(cmbATAChapter.SelectedIndex > 0, cmbATAChapter.SelectedItem.Text, "")
        MachineID = cmbAircraft.SelectedValue.ToString
        ATAID = cmbATAChapter.SelectedValue.ToString
        lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
        lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        lblATAChapter1.Text = "ATA Chapter : " & IIf(ATAChapter <> "", ATAChapter, "")
        mReleaseOfAircraftUnderMELSearchingCriteria = lblDateRangeFrom.Text.Trim + ", " + lblDateRangeTo.Text.Trim + ", " + lblAircraft1.Text.Trim + ", " + lblATAChapter1.Text.Trim
    End Sub
    Private Sub SetReport()
        SetValues()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsReleaseOfAircraftUnderMEL
        Dim mCompanyDetail As New CompanyDetail

        myReport = New crptReleaseOfAircraftUnderMEL
        If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then
            mReleaseOfAircraftUnderMEL = ReleaseOfAircraftUnderMEL.GetReleaseOfAircraftUnderMEL(StartDate, EndDate, MachineID, ATAID, "HH:mm", SkipIsForInventoryAircarft:=True)
        Else
            mReleaseOfAircraftUnderMEL = ReleaseOfAircraftUnderMEL.GetReleaseOfAircraftUnderMEL(StartDate, EndDate, MachineID, ATAID, SkipIsForInventoryAircarft:=True)
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, IIf(AppSettings("MELSnagNomenclature") = "True", "Release Of Aircraft Under ADD", "Release Of Aircraft Under MEL"), New SmartDate(StartDate).FormattedText, New SmartDate(EndDate).FormattedText, ATAChapter, Aircraft, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mReleaseOfAircraftUnderMEL.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mReleaseOfAircraftUnderMEL)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "ReleaseOfAircraftUnderMEL", mReleaseOfAircraftUnderMELSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbAircraft" Then
            If cmbAircraft.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Please select the Aircraft"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub DataFieldBind()
        'Commented By Utkarsh On 19-Apr-201

        'mMachineNameValueList = tmpMachineList.GetMachineList("", "", "", "", "", "(All)")

        '********************************** 
        'Added By Utkarsh On 19-Apr-2011

        mMachineNameValueList = MachineNameValueList.GetMachineList("", , , , , , , True, "(All)", , True)
        '***********************************
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList

        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack Then
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat").ToString)
            DataFieldBind()
            If cmbAircraft.Enabled = True Then
                setFocus(cmbAircraft)
            End If
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlselection1.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class