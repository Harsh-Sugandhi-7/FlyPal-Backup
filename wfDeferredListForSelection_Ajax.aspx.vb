'Created by : Prashant 
'Dated      : 22-Feb-2024

Imports System.Linq
Public Class wfDeferredListForSelection_Ajax
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
    Dim mMELSnagCorrectiveAction As MELSnagCorrectiveAction
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mDeviationLists = Session("mDeviationLists")
        mMELSnagCorrectiveAction = Session("mMELSnagCorrectiveAction")
    End Sub
    Private Sub SetSession()
        Session("mDeviationList") = mDeviationList
        Session("mDeviationLists") = mDeviationLists
        Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mDeviationList")
        Session.Remove("mDeviationLists")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfDeferredListForSelection_Ajax.aspx" Then
            Session.Remove("mDeviationLists")
        End If
    End Sub
    Private Sub SetObject(ByVal mId As Guid)
        mDeviationList = DeviationList.GetDeviationList(mId)
        mMELSnagCorrectiveAction.IsDeviationList = True
        mMELSnagCorrectiveAction.DeviationListID = mDeviationList.ID
        mMELSnagCorrectiveAction.DeviationDescription = mDeviationList.Description
        mMELSnagCorrectiveAction.IsHours = False
        mMELSnagCorrectiveAction.CauseOfDefect = mDeviationList.DeviationCategoryName
        mMELSnagCorrectiveAction.ATAChapterID = mDeviationList.ATAID
        mMELSnagCorrectiveAction.SubATAID = mDeviationList.SubATAID
        mMELSnagCorrectiveAction.ItemSequenceNo = mDeviationList.ItemNo
        Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
		Session("DiscrepancyCorrectiveAction") = mMELSnagCorrectiveAction
	End Sub

    Private Sub SetControl()
        FindNow()
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
                            SetGrid()
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
                        SetGrid()
                    End If
                Case MsgBoxResult.Ok
                    DataFieldBind()
                    SetControl()
                    SetGrid()
            End Select
        End If
    End Sub
    Private Sub FindNow(Optional ByVal ModelID As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional ByVal ATAID As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional ByVal SubATA As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional ByVal ItemSequenceNo As String = "", Optional ByVal Description As String = "",
                        Optional ByVal DeviationCategoryID As Integer = -1)
        mDeviationLists = Nothing
        dgDeviationLists.DataSource = Nothing
        Dim mMachine As Machine
		mMachine = Machine.GetMachine(mMELSnagCorrectiveAction.MachineID)
		'Get List From the Database as per Criteria
		'Comment by Sankalp 08-10-25
		'mDeviationLists = DeviationLists.GetDeviationLists(ModelID:=mMachine.AssemblyStatus.Assembly.ModelID.ToString, ATAID:=ATAID, SubATA:=SubATA, Description:=Description,
		'DeviationCategoryID:=DeviationCategoryID)
		'Set DataSource of the Grid
		'Changes from Sankalp 08-10-2025 As Required To get list as per Primary List
		mDeviationLists = DeviationLists.GetDeviationLists(ModelID:=Guid.Empty.ToString, ATAID:=ATAID, SubATA:=SubATA,
														   Description:=Description,
														   DeviationCategoryID:=DeviationCategoryID,
														   PrimaryModelID:=mMachine.AssemblyStatus.Assembly.Model.PrimaryModelID.ToString)
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
    End Sub
    Private Sub setVariables()
    End Sub
    Private Sub SetTitle()
        lblResult.Text = "Deferred List as per criteria : " & mDeviationLists.Count.ToString & " Record(s) found."
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        IsInRoleString = "DeviationList"
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
		'btnBottomAdd.Visible = IIf(mDeviationLists.Count > 25, True, False)
		btnBottomClose.Visible = IIf(mDeviationLists.Count > 25, True, False)
        upnlBottomActionButton.Update()
    End Sub
    Private Sub SetIDs()

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
        Session("mDeviationLists") = mDeviationLists
        upnlDeviationLists.Update()
    End Sub
    Private Sub SetGrid()

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
            SetControl()
            BottomActionButtonVisibility()
        End If
        SetGrid()
        SetTitle()
    End Sub
    Private Sub dgDeviationLists_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDeviationLists.RowCommand
        Dim index As Int32
        Select Case e.CommandName
            Case "SelectRecord"
                index = (CInt(e.CommandArgument) + (dgDeviationLists.PageSize * dgDeviationLists.PageIndex))
                Dim mID As Guid = New Guid(dgDeviationLists.DataKeys(CInt(e.CommandArgument)).Values(0).ToString)
                SetObject(mID)
                SetTitle()
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
        End Select
        SetGrid()
    End Sub
	Private Sub btnTopBack_Click(sender As Object, e As EventArgs) Handles btnTopBack.Click, btnBottomClose.Click
		Dim mopenas As String = Request.QueryString("Type")
		If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        SetIDs()
        FindNow(ATAID:=cmbATAChapter.SelectedValue.ToString, SubATA:=cmbSubATAList.SelectedValue.ToString,
                ItemSequenceNo:="", Description:=txtDescription.Text.Trim,
                DeviationCategoryID:=IIf(cmbDeviationCategory.SelectedIndex > 0, cmbDeviationCategory.SelectedValue, -1)
                )
        SetGrid()
        BottomActionButtonVisibility()
    End Sub
    Private Sub dgDeviationLists_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgDeviationLists.PageIndexChanging
        dgDeviationLists.PageIndex = e.NewPageIndex
        dgDeviationLists.DataSource = mDeviationLists
        Session("mDeviationLists") = mDeviationLists
        GridBind()
        SetGrid()
    End Sub
    Private Sub dgDeviationLists_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDeviationLists.Sorting
        mDeviationLists.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mDeviationLists") = mDeviationLists
        dgDeviationLists.DataSource = mDeviationLists
        GridBind()
        SetGrid()
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
    Private Sub txtDescription_TextChanged(sender As Object, e As System.EventArgs) Handles txtDescription.TextChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub cmbDeviationCategory_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbDeviationCategory.SelectedIndexChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
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