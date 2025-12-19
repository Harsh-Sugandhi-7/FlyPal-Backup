Public Class wfrptApprovedPartList_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mCategoryLists As CategoryList
    Public PartNo As String = ""
    Public Description As String = ""
    Public mCategory As Category
    Public mCategoryID As Guid
    Public StrCategory As String
    Dim mApprovedPartListSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid 'Added by Prashant on 04-Dec-2013
    Public mAltTypeList As AltTypeList
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        PartNo = Session("PartNo")
        Description = Session("Description")
        mCategoryLists = CType(Session("mCategoryLists"), CategoryList)
    End Sub
    Private Sub DataFieldBind()
        mCategoryLists = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryLists
        Session("mCategoryLists") = mCategoryLists

        mAltTypeList = AltTypeList.GetAltTypeList(IsSelectTagRequired:=True, AddTopItem:="(All)")
        cmbAltType.DataSource = mAltTypeList

        DataBind()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mCategoryLists")
    End Sub
    Private Sub ResetValues()
        PartNo = ""
        Description = ""
        Session("PartNo") = ""
        Session("Description") = ""
        Session("mCategoryID") = ""
    End Sub
    Private Sub SetValues()
        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text.Trim)
            Description = Trim(txtSearch.Text.Trim)
        End If

        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        If cmbCategory.SelectedIndex <= 0 Then
            StrCategory = ""
            mCategoryID = Guid.Empty
            lblCategoryName.Text = "Category Name : All"
        Else
            StrCategory = cmbCategory.SelectedItem.Text
            mCategoryID = New Guid(cmbCategory.SelectedValue)
            lblCategoryName.Text = "Category Name : " & StrCategory
        End If

        lblSeralizedstatus.Text = "Serialized Status : " & IIf(chkserializedtatus.Checked = True, "Yes", "No")
        lblgroundEquipment.Text = "Ground Equipment : " & IIf(chkGroundequipmentstatus.Checked = True, "Yes", "No")
        lblPartType.Text = "Part Type : " & IIf(cmbAltType.SelectedIndex = 0, "", cmbAltType.SelectedItem.Text)
        mApprovedPartListSearchingCriteria = lblCategoryName.Text + ", " + lblSeralizedstatus.Text + ", " + lblgroundEquipment.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + lblPartType.Text

    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean)
        Session("IsExcel") = IsExcel
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rpt As ApprovedPartList
        SetValues()
        Dim ReportName As String = "Approved Part List Report"
        Dim ds As New dsApprovedPartList

        Dim mCompanyDetail As New CompanyDetail
        Dim ReportData As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, _
        mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, ReportName, PartNo, Description, StrCategory, _
        IIf(cmbAltType.SelectedIndex = 0, "", cmbAltType.SelectedItem.Text), "", "", AppSettings("ProductVersion"), AppSettings("SINote"), AppSettings("Logo")) 'Str7=Logo,str1=pno,str2=desc

        rpt = ApprovedPartList.GetApprovedPartList(PartNo, Description, StrCategory, , chkserializedtatus.Checked, chkGroundequipmentstatus.Checked, _
                                                   Val(cmbAltType.SelectedValue))
        myReport = New crptApprovedPartList

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1242)
        End If
        ds.Clear()
        da.Fill(ds, rpt)
        If IsExcel = False Then
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
        End If
        da.Fill(ds, ReportData)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "ApprovedPartList", mApprovedPartListSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub ControlVisibility2()
        lblCategoryName.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblSeralizedstatus.Visible = True
        lblgroundEquipment.Visible = True
        lblPartType.Visible = True
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

#Region " Events "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant on 04-Dec-2013
        If Not IsPostBack Then
            GetSession()
            DataFieldBind()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport(False)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
        upnlSelection.Update()
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetReport(True)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class