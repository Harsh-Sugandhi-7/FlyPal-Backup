
'AJAX Conversion By     :   Saylee
'Dated                  :   1-Feb-2015



Public Class wfrptAuditSchedule_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAuditSchedulAuditNoList As AuditSchedulAuditNoList
    Public mAuditTypeList As AuditTypeList
    Dim AuditNo, AuditType As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mAuditSchedulAuditNoList = Session("mAuditSchedulAuditNoList")
        mAuditTypeList = Session("mAuditTypeList")
        AuditNo = Session("AuditNo")
        AuditType = Session("AuditType")
    End Sub
    Private Sub SetSession()
        Session("mAuditSchedulAuditNoList") = mAuditSchedulAuditNoList
        Session("mAuditTypeList") = mAuditTypeList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAuditSchedulAuditNoList")
        Session.Remove("mAuditTypeList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptAuditSchedule_Ajax.aspx?" Then
            Session.Remove("mAuditSchedule")
            Session.Remove("mAuditSchedulAuditNoList")
            Session.Remove("mAuditTypeList")
            Session.Remove("AuditNo")
            Session.Remove("AuditType")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "FocusScript", str)
    End Sub

#End Region

#Region " DataBinding "
    Public Sub DataFieldBind()
        mAuditSchedulAuditNoList = AuditSchedulAuditNoList.GetAuditSchedulAuditNoList("(All)")
        cmbAuditNo.DataSource = mAuditSchedulAuditNoList
        mAuditSchedulAuditNoList = Session("mAuditSchedulAuditNoList")

        mAuditTypeList = AuditTypeList.GetAuditTypeList("(All)")
        cmbAuditType.DataSource = mAuditTypeList
        mAuditTypeList = Session("mAuditTypeList")

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfrptAuditSchedule_Ajax.aspx?"
            txtAsOnDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            txtAsOnDate.DataBind()
            DataFieldBind()
            setFocus(cmbAuditNo)
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Not IsValid Then Exit Sub
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mrptAuditSchedule As rptAuditSchedule
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mdsAuditSchedule As New dsAuditSchedule
        myReport = New crptAuditSchedule
        If cmbAuditNo.SelectedIndex > 0 Then
            AuditNo = cmbAuditNo.SelectedValue
        Else
            AuditNo = ""
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, "Audit Schedule Report", New SmartDate(txtAsOnDate.Text.ToString).FormattedText, "", AuditNo, "", cmbAuditType.SelectedItem.Text, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

        mrptAuditSchedule = rptAuditSchedule.GetrptAuditScheduleList(txtAsOnDate.Text.ToString, AuditNo, cmbAuditType.SelectedValue)

        If mrptAuditSchedule.Count <= 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfrptAuditSchedule_Ajax.aspx?"
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(mdsAuditSchedule)
        '----------------------------------------------------------
        da.Fill(mdsAuditSchedule, mrptAuditSchedule)
        da.Fill(mdsAuditSchedule, Report)
        da.Fill(mdsAuditSchedule, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(mdsAuditSchedule)
        Session("CrystalReport") = myReport

        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblAsOnDate1.Visible = True
        lblAuditNo1.Visible = True
        lblAuditType1.Visible = True
        lblAsOnDate1.Text = "As On Date : " & New SmartDate(txtAsOnDate.Text.ToString).FormattedText

        If cmbAuditNo.SelectedIndex > 0 Then
            lblAuditNo1.Text = "Audit No. : " & cmbAuditNo.SelectedItem.Text
        Else
            lblAuditNo1.Text = "Audit No. : All"
        End If


        If cmbAuditType.SelectedIndex > 0 Then
            lblAuditType1.Text = "Audit Type : " & cmbAuditType.SelectedItem.Text
        Else
            lblAuditType1.Text = "Audit Type : All"
        End If
        upnlCurrentCriteria.Update()
    End Sub
#End Region

End Class