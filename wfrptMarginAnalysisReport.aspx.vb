
''Created by    : Saylee
''Dated         : 24-June-2010

Partial Class wfrptMarginAnalysisReport
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

#Region " Variable Declaration "
    Public mItemList As ItemList
    ''Public mrptMarginAnalysisPurchaseInvoice As rptMarginAnalysisPurchaseInvoice
    ''Public mrptMarginAnalysisSalesInvoice As rptMarginAnalysisSalesInvoice
    Public mrptMarginAnalysisReport As rptMarginAnalysisReport
    Dim StartDate As String
    Dim EndDate As String
    Dim PartID As String
    Dim PartNo, Description As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mItemList = CType(Session("mItemList"), ItemList)
        mrptMarginAnalysisReport = CType(Session("mrptMarginAnalysisReport"), rptMarginAnalysisReport)

        PartNo = CType(Session("PartNo"), String)
        Description = CType(Session("Description"), String)
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        PartID = CType(Session("PartID"), String)
    End Sub
    Private Sub SetSession()
        Session("mItemList") = mItemList
        Session("mrptMarginAnalysisReport") = mrptMarginAnalysisReport
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemList")
        Session.Remove("mrptMarginAnalysisReport")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("PartID")
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
#End Region

#Region " Helper Methods "
    Private Sub Display()
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
    End Sub
    Private Sub SetValues()
        '''If Not (txtFromDate1.IsDateValue) Then
        '''    StartDate = ""
        '''Else
        '''    StartDate = txtFromDate1.Value.ToString
        '''End If
        '''If Not (txtToDate1.IsDateValue) Then
        '''    EndDate = ""
        '''Else
        '''    EndDate = txtToDate1.Value.ToString
        '''End If
        '''
        If txtFromDate1.Text = "" Then
            StartDate = ""
        Else
            StartDate = txtFromDate1.Text
        End If

        If txtToDate1.Text = "" Then
            EndDate = ""
        Else
            EndDate = txtToDate1.Text
        End If


        PartNo = IIf(PartNo <> "" And Not IsNothing(PartNo), PartNo, "")
        Description = IIf(Description <> "" And Not IsNothing(Description), Description, "")
        PartID = IIf(PartID <> "" And Not IsNothing(PartID), PartID, "{00000000-0000-0000-0000-000000000000}")

        '' PartID = cmbPart.SelectedValue.ToString
        lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
        lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")
    End Sub
    Private Sub ResetValues()
        StartDate = txtFromDate1.Text.ToString
        EndDate = txtToDate1.Text.ToString
        PartID = "{00000000-0000-0000-0000-000000000000}"
    End Sub
    Public Sub NewPage(ByVal s As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dgPartSearch.CurrentPageIndex = e.NewPageIndex
        dgPartSearch.DataSource = mItemList
        Session("mItemList") = mItemList
        dgPartSearch.DataBind()
        setFocus(dgPartSearch)
        lblResult.Text = "List of Part No.s: " & mItemList.Count & " Record(s) found."
    End Sub
    Private Sub SetReport()
        SetValues()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsMarginAnalysisReport
        Dim mCompanyDetail As New CompanyDetail

        myReport = New crMarginAnalysisReport


        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                mCompanyDetail.WebSite, "Margin Analysis Report", New SmartDate(StartDate).FormattedText, New SmartDate(EndDate).FormattedText, PartNo, Description, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))  'Changed By Utkarsh For Report Logo.

        Dim objSearch As rptSearchingCriteria
        objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), StartDate, EndDate, PartNo, "", "", "", "", "", "", "", Description, "")

        mrptMarginAnalysisReport = rptMarginAnalysisReport.GetMarginAnalysis(StartDate, EndDate, PartID)

        If mrptMarginAnalysisReport.Count = 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            msg1.ReplacePage = "wfrptMarginAnalysisReport.aspx?Backpage="
            msg1.Show()
            Exit Sub
        Else

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1187)
        End If
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, mrptMarginAnalysisReport)
        da.Fill(ds, objSearch)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ResetValues()
    End Sub
    Private Sub FindNow(ByVal LookInType As Integer, ByVal ItemName As String, ByVal Description As String)
        mItemList = Nothing
        dgPartSearch.DataSource = Nothing
        mItemList = ItemList.GetItemList(LookInType, ItemName, Description, "", "", "", "", False)
        dgPartSearch.DataSource = mItemList
        dgPartSearch.DataBind()
        Session("mItemList") = mItemList
        lblResult.Text = "List of Part No.s: " & mItemList.Count & " Record(s) found."
    End Sub
    Private Sub ClearControls()
        txtSearchFor.Text = ""
    End Sub
    Private Sub ControlVisibility1(ByVal Index As Int16)
        lblFor.Visible = (Index <> 0)
        txtSearchFor.Visible = (Index <> 0)
    End Sub
    Private Sub ControlVisibility3()
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblDateRangeFrom.Visible = False
        lblDateRangeTo.Visible = False
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        ' ''Dim custValidator As CustomValidator
        ' ''custValidator = CType(s, CustomValidator)
        ' ''If custValidator.ControlToValidate = "cmbAircraft" Then
        ' ''    If cmbAircraft.SelectedIndex = 0 Then
        ' ''        custValidator.ErrorMessage = "Please select the Aircraft"
        ' ''        e.IsValid = False
        ' ''    Else
        ' ''        e.IsValid = True
        ' ''    End If
        ' ''End If
    End Sub
    Private Sub DataFieldBind()
        mItemList = ItemList.GetItemList(0, "", "", "", "", "", "", False)
        dgPartSearch.DataSource = mItemList
        Session("mItemList") = mItemList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            txtFromDate1.Text = Now.Date
            txtToDate1.Text = Now.Date
            DataFieldBind()
        End If
        lblResult.Text = "List of Part Nos. : " & mItemList.Count & " Record(s) found."
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
        ClearControls()
        ControlVisibility1(Index)
        If cmbSearch.Enabled = True Then
            setFocus(cmbSearch)
        End If
    End Sub
    'Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate1.CalendarVisibleChanged
    '    Me.cmbSearch.Visible = Not CType(sender, Boolean)
    'End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Response.End()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartSearch.CurrentPageIndex = 0
        SetValues()
        PartID = "{00000000-0000-0000-0000-000000000000}"
        PartNo = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
        Description = IIf(cmbSearch.SelectedIndex = 2, Trim(txtSearchFor.Text), "")
        Session("PartNo") = PartNo
        Session("Description") = Description
        Session("PartID") = PartID
        FindNow(cmbSearch.SelectedIndex, PartNo, Description)

    End Sub
    Private Sub dgPartSearch_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPartSearch.ItemCommand
        Dim Index As Int16 = e.Item.ItemIndex + dgPartSearch.CurrentPageIndex * dgPartSearch.PageSize
        Select Case e.CommandName
            Case "Select"
                ClearControls()
                PartID = mItemList(Index).ID.ToString
                PartNo = mItemList(Index).Name
                Description = mItemList(Index).Description

                Session("PartID") = PartID
                Session("PartNo") = PartNo
                Session("Description") = Description
                ControlVisibility3()
                SetFocus(dgPartSearch)
        End Select
    End Sub
    Private Sub dgPartSearch_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgPartSearch.SortCommand
        'Added By Rahul 18-June-2009 for grid sorting
        mItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mItemList") = mItemList
        dgPartSearch.DataSource = mItemList
        dgPartSearch.DataBind()
    End Sub
#End Region

End Class
