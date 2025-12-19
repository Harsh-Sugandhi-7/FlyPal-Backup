Public Class wfGroupTrainingConfigList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAllocatedTrainingGroupedByDoneOnList As AllocatedTrainingGroupedByDoneOnList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAllocatedTrainingGroupedByDoneOnList = CType(Session("mAllocatedTrainingGroupedByDoneOnList"), AllocatedTrainingGroupedByDoneOnList)
    End Sub
    Private Sub SetSession()
        Session("mAllocatedTrainingGroupedByDoneOnList") = mAllocatedTrainingGroupedByDoneOnList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfGroupTrainingConfigList_Ajax.aspx" Then
            Session.Remove("mAllocatedTrainingGroupedByDoneOnList")
        End If
    End Sub
    Private Sub SetGrid()
        Dim P As Integer
        For j As Integer = 0 To dgTrainingList.Rows.Count - 1
            P = CType(Me.dgTrainingList.Rows.Item(j).Cells(10).Text, Boolean)
            If P Then
                ''dgTrainingList.Rows.Item(j).Cells(8).Enabled = False ''EditView
                dgTrainingList.Rows.Item(j).Cells(9).Enabled = False ''Delete
            End If
        Next
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCntrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCntrl.Sender = "Delete" Then
                        Dim mEmployeeTrainningRegister As EmployeeTrainningRegister
                        Dim DoneDate As String = Session("DoneDate")
                        Dim ApplicableNames As String() = Session("ApplicableNames")
                        Dim mID As Guid = Session("mID")

                        mEmployeeTrainningRegister = EmployeeTrainningRegister.GetEmployeeTrainningRegister(TrainningID:=mID.ToString)
                        For i As Integer = 0 To mEmployeeTrainningRegister.Count - 1
                            If mEmployeeTrainningRegister(i).Date.ToString.Equals(DoneDate) And Array.IndexOf(ApplicableNames, mEmployeeTrainningRegister(i).EmployeeName) >= 0 Then
                                EmployeeTraining.DeleteEmployeeTraining(mEmployeeTrainningRegister(i).ID)
                                MarkLog(Flypal.Util.Action.Delete, "EmployeeGroupTrainingAllocation", "Emp : " + mEmployeeTrainningRegister(i).EmployeeName + " Training : " + mEmployeeTrainningRegister(i).TrainingName + " Delete From Group Training", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                        Next
                        mAllocatedTrainingGroupedByDoneOnList = AllocatedTrainingGroupedByDoneOnList.GetTrainingListForEmployeeGrouping(Name:=txtName.Text)
                        dgTrainingList.DataSource = mAllocatedTrainingGroupedByDoneOnList
                        Session("mAllocatedTrainingGroupedByDoneOnList") = mAllocatedTrainingGroupedByDoneOnList
                        dgTrainingList.DataBind()
                        lblResult.Text = "Training List: " & mAllocatedTrainingGroupedByDoneOnList.Count & " Record(s) Found."
                        SetGrid()
                        upnlGridView.Update()
                        upnlTitle.Update()
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCntrl.Sender = "Delete" Then

                    End If
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAllocatedTrainingGroupedByDoneOnList = AllocatedTrainingGroupedByDoneOnList.GetTrainingListForEmployeeGrouping(Name:=txtName.Text)
        dgTrainingList.DataSource = mAllocatedTrainingGroupedByDoneOnList
        Session("mAllocatedTrainingGroupedByDoneOnList") = mAllocatedTrainingGroupedByDoneOnList
        DataBind()

        lblResult.Text = "Training List: " & mAllocatedTrainingGroupedByDoneOnList.Count & " Record(s) Found."
    End Sub
    Public Sub ControlVisibility(ByVal index As Integer)
        txtName.Visible = IIf(index > 0, True, False)
        lblFor.Visible = IIf(index > 0, True, False)
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfGroupTrainingConfigList_Ajax.aspx"
            DataFieldBind()
            SetGrid()
        End If
    End Sub
    Private Sub dgTrainingList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTrainingList.PageIndexChanging
        dgTrainingList.PageIndex = e.NewPageIndex
        dgTrainingList.DataSource = mAllocatedTrainingGroupedByDoneOnList
        Session("mAllocatedTrainingGroupedByDoneOnList") = mAllocatedTrainingGroupedByDoneOnList
        dgTrainingList.DataBind()
        SetGrid()
    End Sub
    Private Sub cmbSearchType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearchType.SelectedIndexChanged
        'Dim index As Integer
        'txtName.Text = ""
        'index = cmbSearchType.SelectedIndex
        'ControlVisibility(index)

        'If cmbSearchType.SelectedValue = 0 Then
        '    mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, , , , )
        'ElseIf cmbSearchType.SelectedValue = 1 Then
        '    mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, Trim(txtName.Text), , , )
        'ElseIf cmbSearchType.SelectedValue = 2 Then
        '    mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, , txtName.Text, , )
        'End If
        'dgTrainingList.DataSource = mTrainingListForEmployeeGrouping
        'dgTrainingList.DataBind()
        'Session("mTrainingListForEmployeeGrouping") = mTrainingListForEmployeeGrouping

        'lblResult.Text = "Training List: " & mTrainingListForEmployeeGrouping.Count & " Record(s) Found."
        ''AJAX
        'upnlSearchCriteria.Update()
        'upnlValidationSummary.Update()
        'upnlGridView.Update()
        ''End
    End Sub
    Protected Sub txtFor_TextChanged(sender As Object, e As EventArgs) Handles txtName.TextChanged
        mAllocatedTrainingGroupedByDoneOnList = AllocatedTrainingGroupedByDoneOnList.GetTrainingListForEmployeeGrouping(Name:=txtName.Text.Trim)
        dgTrainingList.DataSource = mAllocatedTrainingGroupedByDoneOnList
        dgTrainingList.DataBind()
        Session("mAllocatedTrainingGroupedByDoneOnList") = mAllocatedTrainingGroupedByDoneOnList

        lblResult.Text = "Training List: " & mAllocatedTrainingGroupedByDoneOnList.Count & " Record(s) Found."
        'AJAX
        upnlSearchCriteria.Update()
        upnlValidationSummary.Update()
        upnlGridView.Update()
        'End
        SetGrid()
    End Sub
    Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgTrainingList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgTrainingList.Sorting
        mAllocatedTrainingGroupedByDoneOnList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAllocatedTrainingGroupedByDoneOnList") = mAllocatedTrainingGroupedByDoneOnList
        dgTrainingList.DataSource = mAllocatedTrainingGroupedByDoneOnList
        dgTrainingList.DataBind()
        SetGrid()
    End Sub
    Private Sub dgTrainingList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTrainingList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim DoneDate As String
                Dim mID As Guid = mAllocatedTrainingGroupedByDoneOnList(CType(e.CommandArgument.ToString, Integer) + dgTrainingList.PageIndex * dgTrainingList.PageSize).ID
                DoneDate = mAllocatedTrainingGroupedByDoneOnList(CType(e.CommandArgument.ToString, Integer) + dgTrainingList.PageIndex * dgTrainingList.PageSize).Date.ToString
                Session("mTrainingID") = mID
                ' ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenGroupEmpTrainingWindow", "OpenGroupEmpTrainingWindow()", True)
                Dim str As String
                str = "openledgersame('wfGroupTrainingConfiguration_Ajax.aspx?" & "');"
                Session("SkipNames") = mAllocatedTrainingGroupedByDoneOnList(mID, DoneDate)
                Session("EditTrainingGroup") = "True"
                Session("DoneDate") = DoneDate
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRecord"
                MSGBoxCntrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                Dim DoneDate As String
                Dim ApplicableNames As String()

                Dim mID As Guid = mAllocatedTrainingGroupedByDoneOnList(CType(e.CommandArgument.ToString, Integer) + dgTrainingList.PageIndex * dgTrainingList.PageSize).ID
                DoneDate = mAllocatedTrainingGroupedByDoneOnList(CType(e.CommandArgument.ToString, Integer) + dgTrainingList.PageIndex * dgTrainingList.PageSize).Date.ToString
                ApplicableNames = mAllocatedTrainingGroupedByDoneOnList(CType(e.CommandArgument.ToString, Integer) + dgTrainingList.PageIndex * dgTrainingList.PageSize).ApplicableToEmployees.Split(",")
                Session("mID") = mID
                Session("DoneDate") = DoneDate
                Session("ApplicableNames") = ApplicableNames
               
        End Select
    End Sub
    Private Sub btnAddNew_Click(sender As Object, e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        Dim str As String
        Str = "openledgersame('wfGroupTrainingList_Ajax.aspx" & "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", Str, True)
    End Sub
    Private Sub hdnBtnEmployeeTraining_Click(sender As Object, e As System.EventArgs) Handles hdnBtnEmployeeTraining.Click
        'If cmbSearchType.SelectedValue = 0 Then
        '    mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, , , , )
        'ElseIf cmbSearchType.SelectedValue = 1 Then
        '    mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, Trim(txtName.Text), , , )
        'ElseIf cmbSearchType.SelectedValue = 2 Then
        '    mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, , txtName.Text, , )
        'End If
        'dgTrainingList.DataSource = mTrainingListForEmployeeGrouping
        'dgTrainingList.DataBind()
        'Session("mTrainingListForEmployeeGrouping") = mTrainingListForEmployeeGrouping

        'lblResult.Text = "Training List: " & mTrainingListForEmployeeGrouping.Count & " Record(s) Found."
        ''AJAX
        'SetSession()
        'upnlSearchCriteria.Update()
        'upnlValidationSummary.Update()
        'upnlGridView.Update()
        ''End
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCntrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region



    
End Class