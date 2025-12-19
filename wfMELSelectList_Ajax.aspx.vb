Imports System.Linq

Public Class wfMELSelectList_Ajax
    Inherits Page


#Region "Enumaration"

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

#Region "Variable Declaration"

    Dim mSubATAList As SubATAList
    Public mMELList As MELList
    Public mATAList As ATAList
    Public mMEL As MEL
    Dim MELDetail As String
    Dim Code As String = String.Empty
    Dim PartName As String = String.Empty
    Dim ModelName As String = String.Empty
    Dim Reference As String = String.Empty
    Public ModelID As String = "{00000000-0000-0000-0000-000000000000}"
    Dim EventLogID As Guid

    Dim mAssemblylist As AssemblyList
    Dim mMELSnagCorrectiveAction As MELSnagCorrectiveAction

#End Region

#Region "Business Methods"

    Private Sub GetSession()
        mMELList = Session("mMELList")
        mAssemblylist = Session("mAssemblylist")
        mMELSnagCorrectiveAction = Session("mMELSnagCorrectiveAction")
    End Sub

    Private Sub SetSession()
        Session("mMEL") = mMEL
        Session("mMELList") = mMELList
        Session("mAssemblylist") = mAssemblylist
        Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
    End Sub

    Private Sub RemoveSession()
        Session.Remove("mMEL")
        Session.Remove("mMELList")
    End Sub

    Private Sub SetObject(ByVal mId As Guid)

        Try

            mMEL = MEL.GetMEL(mId)
            mMELSnagCorrectiveAction.MELID = mMEL.ID
            mMELSnagCorrectiveAction.MELCategoryID = mMEL.MELCategoryID
            mMELSnagCorrectiveAction.MELCategoryName = mMEL.MELCategoryName

            If AppSettings("ShowNewDiscrepancyFlow") = "True" Then
                mMELSnagCorrectiveAction.IsMEL = True
            Else
                mMELSnagCorrectiveAction.Defect = mMEL.MELDescription + vbCrLf + mMEL.Remark
            End If

            mMELSnagCorrectiveAction.ATAChapterID = mMEL.ATAID
            mMELSnagCorrectiveAction.SubATAID = mMEL.SubATAID
            mMELSnagCorrectiveAction.FrequencyInDays = mMEL.FrequencyInDays
            mMELSnagCorrectiveAction.FrequencyInHours = mMEL.FrequencyInHours
            mMELSnagCorrectiveAction.FrequencyInCycles = mMEL.FrequencyInCycles

            mMELSnagCorrectiveAction.IsHours = mMEL.IsHours
            mMELSnagCorrectiveAction.MELDescription = mMEL.MELDescription
            mMELSnagCorrectiveAction.ItemSequenceNo = mMEL.ItemNo
            Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
            Session("mDiscrepancyCorrectiveAction") = mMELSnagCorrectiveAction

        Catch ex As Exception
            ex.GetBaseException()
        End Try

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
                            mMEL = CType(Session("mMEL"), MEL)
                            mMEL.Delete()
                            mMEL.Save()
                            DataFieldBind()
                            SetControl()
                            SetGrid()

                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MELDetail = mMEL.ModelName + "," + " ATA : " + mMEL.ATACode.ToString + "," + " SubATA : " + mMEL.SubATACode.ToString
                                MarkLog(Util.Action.Delete, "MEL", "Can't delete : " & MELDetail & " is Currently in use", Util.ErrorType.HandledError, mMEL.ID, EventLogID, "MEL")
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            ErrorsCount = ex.Errors.Count
                        Finally
                            If ErrorsCount = 0 Then
                                MELDetail = mMEL.ModelName + "," + " ATA : " + mMEL.ATACode.ToString + "," + " SubATA : " + mMEL.SubATACode.ToString
                                MarkLog(Util.Action.Delete, "MEL", MELDetail, Util.ErrorType.NoError, mMEL.ID, EventLogID, "MEL")
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
    Private Sub FindNow(Optional ModelID As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional ATAID As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional SubATA As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional ItemSequenceNo As String = "",
                        Optional Description As String = "",
                        Optional MELCategoryID As Integer = -1,
                        Optional RevisionNo As String = "")
        mMELList = Nothing
        dgMELList.DataSource = Nothing
		'Get List From the Database as per Criteria             
		''mMELList = MELList.GetListOfMELPart(ModelID:=mAssemblylist(0).ModelID.ToString,
		''                                    ATAID:=ATAID, SubATA:=SubATA,
		''                                    ItemSequenceNo:=ItemSequenceNo,
		''                                    Description:=Description,
		''                                    MELCategoryID:=MELCategoryID,
		''                                    RevisionNo:=RevisionNo)
		mMELList = MELList.GetListOfMELPart(PrimaryModelID:=mAssemblylist(0).PrimaryModelID.ToString,
											ATAID:=ATAID, SubATA:=SubATA,
											ItemSequenceNo:=ItemSequenceNo,
											Description:=Description,
											MELCategoryID:=MELCategoryID,
											RevisionNo:=RevisionNo)
		'Set DataSource of the Grid
		Session("mMELList") = mMELList
        dgMELList.DataSource = mMELList
        dgMELList.DataBind()
        SetTitle() 'For lblResult
        upnlMELList.Update()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetTitle()
        lblResult.Text = "As per criteria : " & mMELList.Count.ToString & " Record(s) found." 'AppSettings Added By Vikrant On 07-Sep-2020 For ALL07092020
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        IsInRoleString = "MEL"
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

    Private Sub SetIDs()
        If hdnModelId.Value <> String.Empty Then
            ModelID = hdnModelId.Value.ToString
        End If
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
        cmbMELCategory.DataSource = MELCategoryList.GetMELCategoryList("ALL")
        DataBind()
    End Sub
    Public Sub GridBind()
        dgMELList.DataSource = mMELList
        dgMELList.DataBind()
        upnlMELList.Update()
    End Sub
    Private Sub SetGrid()

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtModel.Focus()
            txtModel.Text = mAssemblylist(0).ModelName
            DataFieldBind()
            SetControl()
        End If
        SetGrid()
        SetTitle()
    End Sub
    Private Sub dgMELList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgMELList.RowCommand
        Select Case e.CommandName
            Case "SelectRec"
                Dim Index As Integer
                Index = CInt(e.CommandArgument) + dgMELList.PageIndex * dgMELList.PageSize
                Dim mID As Guid = mMELList(Index).ID
                SetObject(mID)
                Dim mOpenAs As String = Request.QueryString("Type")
                If mOpenAs IsNot Nothing AndAlso mOpenAs = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, [GetType], "onClose", "CallParentCallback();", True)
                    Exit Sub
                End If
        End Select
        SetGrid()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        SetIDs()
        FindNow(ModelID:=ModelID, ATAID:=cmbATAChapter.SelectedValue.ToString, SubATA:=cmbSubATAList.SelectedValue.ToString, ItemSequenceNo:=txtItemSequenceNo.Text.Trim, Description:=txtDescription.Text.Trim, MELCategoryID:=IIf(cmbMELCategory.SelectedIndex > 0, cmbMELCategory.SelectedValue, -1), RevisionNo:=txtRevisionNo.Text)
        SetGrid()

    End Sub
    Private Sub btnTopClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomClose.Click
        RemoveSession()

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        ' Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgMELList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgMELList.PageIndexChanging
        dgMELList.PageIndex = e.NewPageIndex
        dgMELList.DataSource = mMELList
        Session("mMELList") = mMELList
        GridBind()
        SetGrid()
    End Sub
    Private Sub dgMELList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMELList.Sorting
        mMELList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMELList") = mMELList
        dgMELList.DataSource = mMELList
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
    Private Sub cmbSubATAList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbSubATAList.SelectedIndexChanged, txtItemSequenceNo.TextChanged, txtDescription.TextChanged, cmbMELCategory.SelectedIndexChanged, txtRevisionNo.TextChanged, txtDescription.TextChanged
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