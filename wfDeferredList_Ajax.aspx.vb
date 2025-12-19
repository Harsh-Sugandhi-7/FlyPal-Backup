'Created By: Prashant
'Dated:  22-Feb-2024

Imports System.Linq
Public Class wfDeferredList_Ajax
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
    End Enum
#End Region

#Region " Variable Declaration "
    Public mDeviationCategoryList As DeviationCategoryList
    Dim mSubATAList As SubATAList
    Public mDeviationLists As DeviationLists
    Public mATAList As ATAList
    Public mDeviationList As DeviationList
    Dim DeviationListDetail As String
    Dim Code As String = String.Empty
    Dim PartName As String = String.Empty
    Dim ModelName As String = String.Empty
    Dim Reference As String = String.Empty
    Public ModelID As String = "{00000000-0000-0000-0000-000000000000}"
    Dim EventLogID As Guid
    Public mModelList As ModelList
    Public PrimaryModelID As String = "{00000000-0000-0000-0000-000000000000}"
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mDeviationLists = Session("mDeviationLists")
    End Sub
    Private Sub SetSession()
        Session("mDeviationList") = mDeviationList
        Session("mDeviationLists") = mDeviationLists
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mDeviationList")
        Session.Remove("mDeviationLists")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfDeferredList_Ajax.aspx" Then
            Session.Remove("mDeviationLists")
        End If
    End Sub
    Private Sub NewRecord()
        mDeviationList = DeviationList.NewDeviationList()
        mDeviationList.MarkClean()
        Session("mDeviationList") = mDeviationList
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mDeviationList = DeviationList.GetDeviationList(mId)
        mDeviationList.MarkClean()
        Session("mDeviationList") = mDeviationList
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mDeviationList = DeviationList.GetDeviationList(mId)
        Session("mDeviationList") = mDeviationList
        GridBind()
    End Sub
    Private Sub SetControl()
        FindNow()
        txtModel.Text = Code
        SetTitle()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim ErrorsCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mDeviationList = CType(Session("mDeviationList"), DeviationList)
                            mDeviationList.Delete()
                            mDeviationList.Save()
                            DataFieldBind()
                            SetControl()

                            BottomActionButtonVisibility()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                DeviationListDetail = mDeviationList.ModelName + "," + " ATA : " + mDeviationList.ATACode.ToString + "," + " SubATA : " + mDeviationList.SubATACode.ToString
                                MarkLog(Util.Action.Delete, "DeferredListMaster", "Can't delete : " & DeviationListDetail & " is Currently in use", Util.ErrorType.HandledError, mDeviationList.ID, EventLogID, "DeviationList")
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            ErrorsCount = ex.Errors.Count
                        Finally
                            If ErrorsCount = 0 Then
                                DeviationListDetail = mDeviationList.ModelName + "," + " ATA : " + mDeviationList.ATACode.ToString + "," + " SubATA : " + mDeviationList.SubATACode.ToString
                                MarkLog(Util.Action.Delete, "DeferredListMaster", DeviationListDetail, Util.ErrorType.NoError, mDeviationList.ID, EventLogID, "DeviationList")
                            End If
                            Session("ForEventLog") = "For Event Log"
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        DataFieldBind()
                        SetControl()

                    End If
                Case MsgBoxResult.Ok
                    DataFieldBind()
                    SetControl()

            End Select
        End If
    End Sub
    Private Sub FindNow(Optional ByVal ModelID As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional ByVal ATAID As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional ByVal SubATA As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional ByVal ItemSequenceNo As String = "", Optional ByVal Description As String = "",
                        Optional ByVal DeviationCategoryID As Integer = -1, Optional RevisionNo As String = "",
                        Optional PrimaryModelID As String = "{00000000-0000-0000-0000-000000000000}")
        mDeviationLists = Nothing
        dgDeviationLists.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mDeviationLists = DeviationLists.GetDeviationLists(ModelID:=ModelID, ATAID:=ATAID, SubATA:=SubATA, Description:=Description,
                                                           DeviationCategoryID:=DeviationCategoryID, ItemSequenceNo:=ItemSequenceNo,
                                                           RevisionNo:=RevisionNo, PrimaryModelID:=PrimaryModelID)
        'Set DataSource of the Grid
        Session("mDeviationLists") = mDeviationLists
        dgDeviationLists.DataSource = mDeviationLists
        dgDeviationLists.DataBind()
        SetTitle() 'For lblResult

        upnlDeviationLists.Update()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ClearControls()
        txtModel.Text = ""
    End Sub
    Private Sub setVariables()
        Code = txtModel.Text.Trim
        Session("Code") = Code
    End Sub
    Private Sub SetTitle()
        lblResult.Text = "Deferred List as per criteria : " & mDeviationLists.Count.ToString & " Record(s) found."
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        IsInRoleString = "DeferredListMaster"
        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.FindNow
                Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
        End Select
    End Function
    Private Sub BottomActionButtonVisibility()
        btnBottomAdd.Visible = IIf(mDeviationLists.Count > 25, True, False)
        btnBottomClose.Visible = IIf(mDeviationLists.Count > 25, True, False)
        upnlBottomActionButton.Update()
    End Sub
    Private Sub SetIDs()
        If hdnModelId.Value <> String.Empty Then
            ModelID = hdnModelId.Value.ToString
        End If
        If hdnModelId.Value = "" Then 'This is for Microsoft\Edge Browser
            mModelList = ModelList.GetModelList(0, "", , , "(All)")
            If txtModel.Text.Trim <> "" Then
                ModelID = mModelList.Item(txtModel.Text.Trim).ID.ToString
                PrimaryModelID = mModelList.Item(txtModel.Text.Trim, "").PrimaryModelID.ToString
            End If
        End If
    End Sub
    Public Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim obj As DeviationLists
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA10.Data.ObjectAdapter
        'Dim ds As New dsEmployeeTrainningRegister
        Dim ds As New dsMEL

        myReport = New crDeviationList

        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim SearchStr3 As String
        Dim SearchStr4 As String



        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                                     mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                                     mCompanyDetail.WebSite, "", SearchStr1, SearchStr2, SearchStr3,
                                     SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"),
                                     "", "", "", "", AppSettings("Logo"))

        obj = mDeviationLists

        If obj.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
        da.Fill(ds, obj)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("myReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
    End Sub
#End Region

#Region " DataFieldBind "
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList("", "ALL")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList

        cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
        mSubATAList = SubATAList.GetSubATAList(Guid.Empty, "", "ALL")
        cmbSubATAList.DataSource = mSubATAList

        mDeviationCategoryList = DeviationCategoryList.GetDeviationCategoryList("ALL")
        cmbDeviationCategory.DataSource = mDeviationCategoryList

        DataBind()
    End Sub
    Public Sub GridBind()
        dgDeviationLists.DataSource = mDeviationLists
        dgDeviationLists.DataBind()
        upnlDeviationLists.Update()
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If txtModel.Enabled = True Then
                setFocus(txtModel)
            End If
            Session("MiddleFrame") = "wfDeferredList_Ajax.aspx"
            DataFieldBind()
            SetControl()
            BottomActionButtonVisibility()
        End If

        SetTitle()
    End Sub
    Private Sub dgDeviationLists_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDeviationLists.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                EditRecord(mID)
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                GridBind()
                SetTitle()
                DeviationListDetail = mDeviationList.ModelName + "," + " ATA : " + mDeviationList.ATACode.ToString + "," + " SubATA : " + mDeviationList.SubATACode.ToString
                MarkLog(Util.Action.Edit, "DeferredListMaster", DeviationListDetail, Util.ErrorType.NoError, mDeviationList.ID, EventLogID, "DeviationList")
                Dim str As String
                str = "openledgersame('wfDeferredDetail_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not IsInRole(Rights.Delete)) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                DeleteRecord(mID)
        End Select

    End Sub
    Private Sub txtModel_TextChanged(sender As Object, e As System.EventArgs) Handles txtModel.TextChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        SetIDs()
        FindNow(ModelID:=ModelID, ATAID:=cmbATAChapter.SelectedValue.ToString, SubATA:=cmbSubATAList.SelectedValue.ToString,
                ItemSequenceNo:=txtItemSequenceNo.Text.Trim, Description:=txtDescription.Text.Trim,
                DeviationCategoryID:=IIf(cmbDeviationCategory.SelectedIndex > 0, cmbDeviationCategory.SelectedValue, -1),
                RevisionNo:=txtRevisionNo.Text, PrimaryModelID:=PrimaryModelID)

        BottomActionButtonVisibility()
    End Sub
    Private Sub btnTopAdd_Click(sender As Object, e As System.EventArgs) Handles btnTopAdd.Click, btnBottomAdd.Click
        NewRecord()
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        MarkLog(Util.Action.[New], "DeferredListMaster", "", Util.ErrorType.NoError, mDeviationList.ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfDeferredDetail_Ajax.aspx?BackPage=index.aspx');", True)
    End Sub
    Private Sub btnTopClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomClose.Click, btnTopClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgDeviationLists_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgDeviationLists.PageIndexChanging
        dgDeviationLists.PageIndex = e.NewPageIndex
        dgDeviationLists.DataSource = mDeviationLists
        Session("mDeviationLists") = mDeviationLists
        GridBind()

    End Sub
    Private Sub dgDeviationLists_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDeviationLists.Sorting
        mDeviationLists.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mDeviationLists") = mDeviationLists
        dgDeviationLists.DataSource = mDeviationLists
        GridBind()

    End Sub
    Private Sub cmbATAChapter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbATAChapter.SelectedIndexChanged
        cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
        mSubATAList = SubATAList.GetSubATAList(New Guid(cmbATAChapter.SelectedValue), "", "ALL")
        cmbSubATAList.DataSource = mSubATAList
        cmbSubATAList.DataBind()
        Session("mSubATAList") = mSubATAList
        upnlSubATA.Update()
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub cmbSubATAList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbSubATAList.SelectedIndexChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub txtItemSequenceNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtItemSequenceNo.TextChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub txtDescription_TextChanged(sender As Object, e As System.EventArgs) Handles txtDescription.TextChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub cmbDeviationCategory_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbDeviationCategory.SelectedIndexChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub txtRevisionNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtRevisionNo.TextChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        SetReport()
    End Sub
#End Region

#Region " Web Services "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetModelList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim list As ModelListAutoComplete
        list = ModelListAutoComplete.GetModelList(prefixText, 1)
        If count = 0 Then
            Return (From c As ModelListAutoComplete.ModelListAutoCompleteInfo In list
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.ToString())).ToArray
        Else
            Return (From c As ModelListAutoComplete.ModelListAutoCompleteInfo In list
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region

End Class