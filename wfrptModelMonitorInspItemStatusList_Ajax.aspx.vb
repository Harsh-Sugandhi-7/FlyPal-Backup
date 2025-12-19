Public Class wfrptModelMonitorInspItemStatusList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "

    Public mModelMonitorInspSearchList As ModelMonitorInspSearchList
    Public mModelList As ModelList
    Public mAssemblyTypeList As AssemblyTypeList
    Dim mInspectionTypeList As ModelMonitorInspTypeList
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mModelMonitorInspSearchList = Session("mModelMonitorInspSearchList")
        mModelList = Session("mModelList")
        mInspectionTypeList = Session("mInspectionTypeList")
        mAssemblyTypeList = Session("mAssemblyTypeList")
    End Sub
    Private Sub DataFieldBind()

        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeList("(SELECT)")
        cmbAssemblyTypeList.DataSource = mAssemblyTypeList
        Session("mAssemblyTypeList") = mAssemblyTypeList
        cmbAssemblyTypeList.DataBind()

        '''mModelList = ModelList.GetModelList(0, ModelList.IsSelectTagRequired.True)
        '''cmbModelList.DataSource = mModelList
        '''Session("mModelList") = mModelList
        '''cmbModelList.DataBind()

        mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList("(ALL)")
        cmbInspType.DataSource = mInspectionTypeList
        Session("mInspectionTypeList") = mInspectionTypeList
        cmbInspType.DataBind()

        cmbModelList.Enabled = False
        upnlModel.Update()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        Dim IsSelect As Boolean = True

        If custValidator.ControlToValidate = "cmbModelList" Then
            If cmbModelList.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Select atleast one Model"
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbAssemblyTypeList" Then
            If cmbAssemblyTypeList.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Select atleast one Assembly Type"
                e.IsValid = False
            End If

        End If
    End Sub
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""

        If cmbModelList.SelectedIndex = 0 Then
            strMsg = "Select atleast one Model"
        ElseIf cmbAssemblyTypeList.SelectedIndex = 0 Then
            strMsg = "Select atleast one Assembly Type"
        End If

        If strMsg <> "" Then
            cvCustomValidate.ErrorMessage = strMsg
            cvCustomValidate.IsValid = False
            Return False
        End If

        Return True
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
        End If
    End Sub

    Private Sub cmbAssemblyTypeList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAssemblyTypeList.SelectedIndexChanged
        If cmbAssemblyTypeList.SelectedIndex > 0 Then
            mModelList = ModelList.GetModelList(CType(cmbAssemblyTypeList.SelectedValue, Short), ModelList.IsSelectTagRequired.True)
            cmbModelList.DataSource = mModelList
            Session("mModelList") = mModelList
            cmbModelList.DataBind()
            cmbModelList.Enabled = True
        Else
            cmbModelList.SelectedIndex = 0
            cmbModelList.DataSource = Nothing
            cmbModelList.Enabled = False
            cmbModelList.DataBind()
        End If

        dgModelMonitorInsp.DataSource = Nothing
        dgModelMonitorInsp.DataBind()
        lblResult.Text = ""
        Session("mModelMonitorInspSearchList") = mModelMonitorInspSearchList

        upnlModel.Update()
        upnlValidationSummary.Update()
        upnlGrid.Update()
        upnlResult.Update()
    End Sub
    Private Sub cmbModelList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbModelList.SelectedIndexChanged
        If cmbModelList.SelectedIndex > 0 Then
            mModelMonitorInspSearchList = ModelMonitorInspSearchList.GetModelMonitorInspSearchList(New Guid(cmbModelList.SelectedValue.ToString), cmbInspType.SelectedValue, txtSearchFor.Text, txtSearchFor.Text)
            dgModelMonitorInsp.DataSource = mModelMonitorInspSearchList
            dgModelMonitorInsp.DataBind()
            lblResult.Text = "List of Model Inspections as per criteria: " & mModelMonitorInspSearchList.Count.ToString & " Record(s) found."
            Session("mModelMonitorInspSearchList") = mModelMonitorInspSearchList
        Else
            dgModelMonitorInsp.DataSource = Nothing
            dgModelMonitorInsp.DataBind()
            lblResult.Text = "List of Model Inspections as per criteria:" & " 0 Record(s) found."
            Session("mModelMonitorInspSearchList") = mModelMonitorInspSearchList
        End If

        upnlValidationSummary.Update()

        upnlGrid.Update()
        upnlResult.Update()
    End Sub

    Private Sub cmbInspType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbInspType.SelectedIndexChanged
        If CustomValidate1() = False Then upnlValidationSummary.Update() : Exit Sub

        mModelMonitorInspSearchList = ModelMonitorInspSearchList.GetModelMonitorInspSearchList(New Guid(cmbModelList.SelectedValue.ToString), cmbInspType.SelectedValue, txtSearchFor.Text, txtSearchFor.Text)
        dgModelMonitorInsp.DataSource = mModelMonitorInspSearchList
        dgModelMonitorInsp.DataBind()
        lblResult.Text = "List of Model Inspections as per criteria:" & mModelMonitorInspSearchList.Count.ToString & " Record(s) found."
        Session("mModelMonitorInspSearchList") = mModelMonitorInspSearchList
        upnlGrid.Update()
        upnlResult.Update()
    End Sub

    Private Sub cmbSearchFor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSearchFor.SelectedIndexChanged

        If cmbSearchFor.SelectedIndex = 0 Then
            txtSearchFor.Visible = False
            txtSearchFor.Text = ""
        ElseIf cmbSearchFor.SelectedIndex = 1 Then
            txtSearchFor.Visible = True
            txtSearchFor.Text = ""
        ElseIf cmbSearchFor.SelectedIndex = 2 Then
            txtSearchFor.Visible = True
            txtSearchFor.Text = ""
        End If

        upnlTextSearchFor.Update()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        If IsValid = False Then upnlValidationSummary.Update() : Exit Sub

        If cmbSearchFor.SelectedIndex = 0 Then
            txtSearchFor.Visible = False
            txtSearchFor.Text = ""
            mModelMonitorInspSearchList = ModelMonitorInspSearchList.GetModelMonitorInspSearchList(New Guid(cmbModelList.SelectedValue.ToString), cmbInspType.SelectedValue)
        ElseIf cmbSearchFor.SelectedIndex = 1 Then
            mModelMonitorInspSearchList = ModelMonitorInspSearchList.GetModelMonitorInspSearchList(New Guid(cmbModelList.SelectedValue.ToString), cmbInspType.SelectedValue, "", txtSearchFor.Text)
            txtSearchFor.Visible = True
        Else : cmbSearchFor.SelectedIndex = 2
            mModelMonitorInspSearchList = ModelMonitorInspSearchList.GetModelMonitorInspSearchList(New Guid(cmbModelList.SelectedValue.ToString), cmbInspType.SelectedValue, txtSearchFor.Text, "")
            txtSearchFor.Visible = True
        End If

        dgModelMonitorInsp.DataSource = mModelMonitorInspSearchList
        Session("mModelMonitorInspSearchList") = mModelMonitorInspSearchList
        dgModelMonitorInsp.DataBind()
        lblResult.Text = "List of Model Inspections as per criteria:" & mModelMonitorInspSearchList.Count.ToString & " Record(s) found."
        'txtSearchFor.Text = ""
        upnlGrid.Update()
        upnlResult.Update()
    End Sub

    Private Sub dgModelMonitorInsp_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgModelMonitorInsp.PageIndexChanged

    End Sub
    Private Sub dgModelMonitorInsp_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgModelMonitorInsp.PageIndexChanging
        dgModelMonitorInsp.PageIndex = e.NewPageIndex

        dgModelMonitorInsp.DataSource = mModelMonitorInspSearchList
        Session("mModelMonitorInspSearchList") = mModelMonitorInspSearchList
        dgModelMonitorInsp.DataBind()
        upnlGrid.Update()
    End Sub

    Private Sub dgModelMonitorInsp_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgModelMonitorInsp.RowCommand
        Dim mrptModelMonitorInspItemStatusList As rptModelMonitorInspItemStatusList
        Select Case e.CommandName
            Case "Select"
                Dim mCompanyDetail As New CompanyDetail
                Dim da As New CSLA.Data.ObjectAdapter
                Dim dsrptModelMonitorInspItemStatusList As New dsrptModelMonitorInspItemStatusList
                Dim SearchStr1 As String
                Dim SearchStr2 As String
                mModelMonitorInspSearchList = Session("mModelMonitorInspSearchList")

                Dim Index As Integer = CInt(e.CommandArgument) + dgModelMonitorInsp.PageIndex * dgModelMonitorInsp.PageSize
                mrptModelMonitorInspItemStatusList = rptModelMonitorInspItemStatusList.GetModelMonitorInspItemStatusList(New Guid(cmbModelList.SelectedValue.ToString), mModelMonitorInspSearchList(Index).ID, mModelMonitorInspSearchList(Index).ModelMonitorInspTypeID, mModelMonitorInspSearchList(Index).MonitorTypeID)

                Session("mrptModelMonitorInspItemStatusList") = mrptModelMonitorInspItemStatusList

                If mrptModelMonitorInspItemStatusList.Count <= 0 Then
                    ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
                    ''msg1.ReplacePage = "wfrptSectorProfileList.aspx?"
                    ''msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else

                    'Dim rme As New RecentMenuEvent
                    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1281)
                End If

                SearchStr1 = IIf(cmbModelList.SelectedIndex > 0, cmbModelList.SelectedItem.ToString, "ALL")
                SearchStr2 = mModelMonitorInspSearchList(Index).Description

                mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
                Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                  mCompanyDetail.WebSite, "", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))



                Dim myReport = New crptModelMonitorInspItemStatusList

                Dim mrptImage As rptImage = rptImage.GetImage(dsrptModelMonitorInspItemStatusList)
                da.Fill(dsrptModelMonitorInspItemStatusList, mrptModelMonitorInspItemStatusList)
                da.Fill(dsrptModelMonitorInspItemStatusList, Report)
                da.Fill(dsrptModelMonitorInspItemStatusList, mrptImage)
                myReport.SetDataSource(dsrptModelMonitorInspItemStatusList)

                Session("CrystalReport") = myReport

                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        End Select

    End Sub
    Private Sub dgModelMonitorInsp_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgModelMonitorInsp.Sorting
        mModelMonitorInspSearchList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mModelMonitorInspSearchList") = mModelMonitorInspSearchList
        dgModelMonitorInsp.DataSource = mModelMonitorInspSearchList
        dgModelMonitorInsp.DataBind()
        upnlGrid.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region



   
End Class