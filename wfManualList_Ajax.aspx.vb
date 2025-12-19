Imports System.Web.UI.WebControls
Public Class wfManualList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mManual As Manual
    Dim mManualList As ManualList
    Dim mCategorySelection As CategorySelection
    Protected mCategoryListForManualList As CategoryNameValueList
    Dim EventLogID As Guid
#End Region

#Region " Methods "
    Private Sub GetSession()
        mManualList = Session("mManualList")
        mManual = Session("mManual")
    End Sub
    Public Property dir() As SortDirection
        Get
            If ViewState("dirState") Is Nothing Then
                ViewState("dirState") = SortDirection.Ascending
            End If
            Return DirectCast(ViewState("dirState"), SortDirection)
        End Get
        Set(value As SortDirection)
            ViewState("dirState") = value
        End Set
    End Property
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfManualList_Ajax.aspx" Then
            Session("BackPage") = Nothing
            Session.Remove("BackPage")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub NewRecord()
        mManual = Manual.NewManual()
        Session("mManual") = mManual
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mManual = Manual.GetManual(mId)
        Session("mManual") = mManual
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        GridBind()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mManual = Manual.GetManual(mId)
        Session("mManual") = mManual
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Manual.DeleteManual(mManual.ID)
                            MarkLog(Util.Action.Delete, "Manual", mManual.Name + " Category : " + mManual.MCategoryName, Util.ErrorType.NoError, mManual.ID, EventLogID)
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.DatabaseException, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        Finally
                            DataFieldBind()

                            mManualList = ManualList.GetManualList(txtManualName.Text.Trim, New Guid(cmbCategory.SelectedValue), Trim(txtManualNo.Text))
                            dgManualList.DataSource = mManualList
                            dgManualList.DataBind()

                            ClearControls()
                            UpdatePanel()
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        DataFieldBind()
                    End If
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Sub GridBind()
        dgManualList.DataSource = mManualList
        dgManualList.DataBind()
        upnlManualList.Update()
    End Sub
    Private Sub UpdatePanel()
        upnlResult.Update()
        upnlActionBtnTop.Update()
        upnlManualList.Update()
        upnlActionBtnBottom.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mManualList = ManualList.GetManualList("")
        dgManualList.DataSource = mManualList
        DataBind()
        Session("mManualList") = mManualList
        lblCount.Text = "List of Manuals as per criteria : " & mManualList.Count & " Record(s) found."
        If mManualList.Count = 0 Then
            btnPrint.Enabled = False
            btnPrintTop.Enabled = False
        Else
            btnPrint.Enabled = True
            btnPrintTop.Enabled = True
        End If
    End Sub
    Private Sub DataFieldBindForDropDownList()
        mCategoryListForManualList = CategoryNameValueList.GetCategoryNameValueList("(SELECT)")
        cmbCategory.DataSource = mCategoryListForManualList
        cmbCategory.DataBind()
    End Sub
    Private Sub ClearControls()
        txtManualName.Text = ""
        cmbCategory.ClearSelection()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If txtManualName.Enabled = True Then
                setFocus(txtManualName)
            End If
            Session("MiddleFrame") = "wfManualList_Ajax.aspx"
            DataFieldBind()
            DataFieldBindForDropDownList()
        End If
    End Sub
    Private Sub btnAddNewTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        If (Not User.IsInRole("ManualNew")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim ID As Guid = Guid.Empty
        MarkLog(Util.Action.[New], "Manual", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)   'Added By Prashant 20-Jul-2011
        NewRecord()
        Dim str As String
        str = "openledgersame('wfManual_Ajax.aspx?BackPage=wfManualList_Ajax.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindNow.Click
        mManualList = ManualList.GetManualList(txtManualName.Text.Trim, New Guid(cmbCategory.SelectedValue), Trim(txtManualNo.Text))
        dgManualList.DataSource = mManualList
        dgManualList.DataBind()
        Session("mManualList") = mManualList
        lblCount.Text = "List of Manuals as per criteria : " & mManualList.Count & " Record(s) found."
        If mManualList.Count = 0 Then
            btnPrint.Enabled = False
            btnPrintTop.Enabled = False
        Else
            btnPrint.Enabled = True
            btnPrintTop.Enabled = True
        End If
        UpdatePanel()
    End Sub
    Private Sub dgManualList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgManualList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim index As Integer = CInt(e.CommandArgument) + dgManualList.PageIndex * dgManualList.PageSize
                Dim mID As Guid = mManualList(index).ID
                If (Not User.IsInRole("ManualEdit") And Not User.IsInRole("ManualView")) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                EditRecord(mID)
                GridBind()
                Dim mManualDetail As String = mManualList(index).Name + " Category : " + mManualList(index).MCategoryName
                MarkLog(Util.Action.Edit, "Manual", mManualDetail, Util.ErrorType.NoError, mID, EventLogID)
                Dim str As String
                str = "openledgersame('wfManual_Ajax.aspx?BackPage=wfManualList_Ajax.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRecord"
                If (Not User.IsInRole("ManualDelete")) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                Dim index As Integer = CInt(e.CommandArgument) + dgManualList.PageIndex * dgManualList.PageSize
                Dim mID As Guid = mManualList(index).ID
                DeleteRecord(mID)
        End Select
    End Sub
    Protected Sub dgManualList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Dim sortingDirection As String = String.Empty
        If dir = SortDirection.Ascending Then
            dir = SortDirection.Descending
            sortingDirection = "Desc"
        Else
            dir = SortDirection.Ascending
            sortingDirection = "Asc"
        End If
        Dim sortedView As New DataView(BindGridView())
        sortedView.Sort = Convert.ToString(e.SortExpression) & " " & sortingDirection
        dgManualList.DataSource = sortedView
        dgManualList.DataBind()
        upnlManualList.Update()
    End Sub
    Private Sub dgManualList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgManualList.PageIndexChanging
        dgManualList.PageIndex = e.NewPageIndex
        GridBind()
    End Sub
    Private Function BindGridView() As DataTable
        Dim dtGrid As New DataTable()
        Dim dAdapter As New CSLA10.Data.ObjectAdapter
        Dim Obj As ManualList
        Obj = ManualList.GetManualList(txtManualName.Text.Trim, New Guid(cmbCategory.SelectedValue), Trim(txtManualNo.Text))
        dAdapter.Fill(dtGrid, Obj)
        Return dtGrid
    End Function
    Private Sub btnPrintTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click, btnPrint.Click
        Dim mCompanyDetail As New CompanyDetail
        Dim Rpt As New crManualList
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim ds As New Flypal.dsCommon
        Dim Obj As ManualList

        Obj = ManualList.GetManualList(txtManualName.Text.Trim, New Guid(cmbCategory.SelectedValue), Trim(txtManualNo.Text))
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Manual List Report", txtManualName.Text.Trim, IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, ""), "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 27-Feb-2012
        da.Fill(ds, Obj)
        da.Fill(ds, mrptImage)  'Added by Shweta on 27-Feb-2012
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)

        Session("CrystalReport") = Rpt

        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

   


End Class