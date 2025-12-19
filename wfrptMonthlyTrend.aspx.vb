Partial Class wfrptMonthlyTrend
    Inherits System.Web.UI.Page

#Region "Web Form Designer Generated Code"
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Enum "
    Enum GraphType
        MonthlyTrend = 1
        ForcastedDemand = 2
    End Enum
#End Region

#Region " Variable Declaration "
    Public mItem As Item
    Public mItemList As ItemList
    Dim ToDate As String
    Dim PartNo As String
    Dim Description As String
    Dim mGraph As Int16 = 1
    Dim gID As Guid
    Dim Trend As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mItemList = CType(Session("mItemList"), ItemList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        gID = Session("gID")
        mGraph = CType(Session("mGraph"), Int16)
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("mItemList") = mItemList
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)

    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        If cmbSearch.SelectedIndex = 0 Then
            lblFor.Visible = False
            txtSearchFor.Visible = False
        Else
            lblFor.Visible = True
            txtSearchFor.Visible = True
        End If
    End Sub
    Private Sub ControlVisibility1(ByVal Index As Int16)
        lblFor.Visible = (Index <> 0)
        txtSearchFor.Visible = (Index <> 0)
    End Sub
    Private Sub ClearControls()
        txtSearchFor.Text = ""
    End Sub
    Private Sub SetPage()
        txtTrendType.Visible = IIf(mGraph = 1, True, False)
        lblTrendType.Visible = IIf(mGraph = 1, True, False)
    End Sub
    Private Sub SetValues()
        If cmbYear.Items.Count <> 0 Then
            lblDispYear.Text = "Year : " & cmbYear.SelectedItem.ToString
        End If

        If cmbYear.Items.Count <> 0 Then
            ToDate = cmbYear.SelectedValue
        End If
        If mGraph = 1 Then
            If txtTrendType.Text = "" Then
                txtTrendType.Text = "3"
            Else
                Trend = txtTrendType.Text
                lblDispTrendType.Text = "Trend Type   :" & Trend
            End If
        Else
            txtTrendType.Visible = False
            lblTrendType.Visible = False
            lblDispTrendType.Visible = False
        End If
        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")

    End Sub
    Private Sub ResetValues()
        ToDate = Format(CDate(Today.Date).Year, "")
        txtTrendType.Text = ""
        PartNo = ""
        Description = ""
    End Sub

    Private Sub FindNow(ByVal LookInType As Integer, ByVal ItemName As String, ByVal Description As String)
        'dereference the objects
        mItemList = Nothing
        dgPartSearch.DataSource = Nothing
        mItemList = ItemList.GetItemList(LookInType, ItemName, Description, "", "", "", "", False)
        dgPartSearch.DataSource = mItemList
        dgPartSearch.DataBind()
        Session("mItemList") = mItemList
        lblResult.Text = "List of Part No.s: " & mItemList.Count & " Record(s) found."
    End Sub
    Private Sub setHeading()
        If mGraph = 1 Then
            lbltitle.Text = "Monthly Trend Report"
        ElseIf mGraph = 2 Then
            lbltitle.Text = "Forecasted Demand Report"
        End If
    End Sub
    Private Sub ControlVisible()
        'lblDispYear.Visible = IIf(mGraph = 1, True, False)

        'Added Code By Girish May,04,2007
        If mGraph = 1 Then
            lblDispYear.Visible = IIf(mGraph = 1, True, False)
        ElseIf mGraph = 2 Then
            lblDispYear.Visible = IIf(mGraph = 2, True, False)
        End If
        'End of Code

        lblDispTrendType.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
    End Sub
    Private Sub ControlInVisible()
        lblDispYear.Visible = False
        lblDispTrendType.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mItemList = ItemList.GetItemList(0, "", "", "", "", "", "", False)
        dgPartSearch.DataSource = mItemList
        Session("mItemList") = mItemList
        DataBind()
    End Sub
    Public Sub NewPage(ByVal s As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dgPartSearch.CurrentPageIndex = e.NewPageIndex
        mItemList = ItemList.GetItemList(0, "", "", "", "", "", "", False)
        dgPartSearch.DataSource = mItemList
        Session("mItemList") = mItemList
        dgPartSearch.DataBind()
    End Sub
    'Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
    '    Dim custValidator As CustomValidator
    '    custValidator = CType(s, CustomValidator)
    '    If custValidator.ControlToValidate = "txtTrendType" Then
    '        If Val(txtTrendType.Text) < 0 Then
    '            custValidator.ErrorMessage = "Enter Numeric number only."
    '            e.IsValid = False
    '        Else
    '        End If
    '    End If
    'End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Integer
        If cmbYear.Items.Count = 0 Then 'Or cmbYear.SelectedValue = "" Then
            For i = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today).Year)
            Next
            cmbYear.SelectedIndex = 10
        End If
        GetSession()
        If Not IsPostBack Then
            gID = Guid.Empty
            mGraph = CType(Request.QueryString("GraphType"), Int16)
            Session("mGraph") = mGraph
            If cmbYear.Enabled = True Then
                SetFocus(cmbYear)
            End If
            DataFieldBind()
            ControlVisibility(2)
        End If
        SetPage()
        'SetValues()
        lblResult.Text = "List of Part No(s): " & mItemList.Count & " Record(s) found."
        setHeading()
    End Sub

    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
        ClearControls()
        ControlVisibility1(Index)
        If cmbSearch.Enabled = True Then
            SetFocus(cmbSearch)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisible()
        SetValues()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartSearch.CurrentPageIndex = 0
        SetValues()
        PartNo = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
        Description = IIf(cmbSearch.SelectedIndex = 2, Trim(txtSearchFor.Text), "")
        Session("PartNo") = PartNo
        Session("Description") = Description
        FindNow(cmbSearch.SelectedIndex, PartNo, Description)
        ControlInVisible()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub dgPartSearch_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPartSearch.ItemCommand
        '  Dim Index As Int16 = e.Item.ItemIndex + dgPartSearch.CurrentPageIndex * dgPartSearch.PageSize
        Select Case e.CommandName
            Case "Select"
                Dim gID As New Guid(e.Item.Cells(0).Text)
                ClearControls()
                PartNo = mItemList(gID).Name
                Description = mItemList(gID).Description
                Session("PartNo") = PartNo
                Session("Description") = Description
                Session("gID") = gID
                ControlInVisible()
        End Select
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As DataSet
        Dim rptSearch As rptGraphSearchingCriteria
        Dim str As String
        rptSearch = rptGraphSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", 0, 0, 0, 0, 0, 0, 0, 0.0, 0, 0, 0, 0.0, 0.0, 0.0, 0, 0, 0, 0.0, 0.0, 0.0, AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.  '{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}

        If mGraph = 1 Then
            ds = New dsMonthlyTrendList
            Dim rpt As MonthlyTrendList
            myReport = New crptMonthlyTrendList
            SetValues()
            rpt = MonthlyTrendList.GetMonthlyTrendList(ToDate, gID, PartNo, Description, Val(txtTrendType.Text))
            If rpt.Count <= 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfrptMonthlyTrend.aspx?Backpage="
                msg1.Show()
                Exit Sub
            Else
                 RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 902)
            End If
            ds.Clear()
            '-----------Added by Utkarsh for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            '----------------------------------------------------------
            da.Fill(ds, rpt)
            da.Fill(ds, rptSearch)
            da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport
            str = "<script language=Javascript>openTranDetail();</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", str)
            ResetValues()
        ElseIf mGraph = 2 Then
            ds = New dsForecastedDemand
            Dim rpt As ForcastedDemandList
            myReport = New crptForecastedDemand
            SetValues()
            rpt = ForcastedDemandList.GetForcastedDemandList(ToDate, gID, PartNo, Description, Val(txtTrendType.Text))
            If rpt.Count <= 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfrptMonthlyTrend.aspx?GraphType=" & mGraph
                msg1.Show()
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 901)
            End If
            ds.Clear()
            '-----------Added by Utkarsh for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            '----------------------------------------------------------
            da.Fill(ds, rpt)
            da.Fill(ds, rptSearch)
            da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            str = "<script language=Javascript>openTranDetail();</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", str)
            ResetValues()
        End If
    End Sub
    'Added By Prashant 22-June-2009 for grid sorting
    Private Sub dgPartSearch_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgPartSearch.SortCommand
        mItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mItemList") = mItemList
        dgPartSearch.DataSource = mItemList
        dgPartSearch.DataBind()
    End Sub
    '-------------------------------------------------
#End Region

End Class
