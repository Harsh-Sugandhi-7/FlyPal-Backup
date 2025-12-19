Public Class wfGroupTrainingList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mTrainingListForEmployeeGrouping As TrainingListForEmployeeGrouping
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mTrainingListForEmployeeGrouping = CType(Session("mTrainingListForEmployeeGrouping"), TrainingListForEmployeeGrouping)
    End Sub
    Private Sub SetSession()
        Session("mTrainingListForEmployeeGrouping") = mTrainingListForEmployeeGrouping
    End Sub
    'Private Sub ClearAll()
    '    If Session("MiddleFrame") <> "wfGroupTrainingList_Ajax.aspx" Then
    '        Session.Remove("mTrainingListForEmployeeGrouping")
    '    End If
    'End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, , , , )
        dgTrainingList.DataSource = mTrainingListForEmployeeGrouping
        Session("mTrainingListForEmployeeGrouping") = mTrainingListForEmployeeGrouping
        DataBind()

        lblResult.Text = "Training List: " & mTrainingListForEmployeeGrouping.Count & " Record(s) Found."
    End Sub
    Public Sub ControlVisibility(ByVal index As Integer)
        txtFor.Visible = IIf(index > 0, True, False)
        lblFor.Visible = IIf(index > 0, True, False)
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ''ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            'Session("MiddleFrame") = "wfGroupTrainingList_Ajax.aspx"
            DataFieldBind()
        End If
    End Sub
    Private Sub dgTrainingList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTrainingList.PageIndexChanging
        dgTrainingList.PageIndex = e.NewPageIndex
        dgTrainingList.DataSource = mTrainingListForEmployeeGrouping
        Session("mTrainingListForEmployeeGrouping") = mTrainingListForEmployeeGrouping
        dgTrainingList.DataBind()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If cmbSearchType.SelectedValue = 0 Then
            mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, , , , )
        ElseIf cmbSearchType.SelectedValue = 1 Then
            mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, Trim(txtFor.Text), , , )
        ElseIf cmbSearchType.SelectedValue = 2 Then
            mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, , txtFor.Text, , )
        End If
        dgTrainingList.DataSource = mTrainingListForEmployeeGrouping
        dgTrainingList.DataBind()
        Session("mTrainingListForEmployeeGrouping") = mTrainingListForEmployeeGrouping

        lblResult.Text = "Training List: " & mTrainingListForEmployeeGrouping.Count & " Record(s) Found."
        'AJAX
        upnlSearchCriteria.Update()
        upnlValidationSummary.Update()
        upnlGridView.Update()
        'End
    End Sub
    Private Sub cmbSearchType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearchType.SelectedIndexChanged
        Dim index As Integer
        txtFor.Text = ""
        index = cmbSearchType.SelectedIndex
        ControlVisibility(index)

        If cmbSearchType.SelectedValue = 0 Then
            mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, , , , )
        ElseIf cmbSearchType.SelectedValue = 1 Then
            mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, Trim(txtFor.Text), , , )
        ElseIf cmbSearchType.SelectedValue = 2 Then
            mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, , txtFor.Text, , )
        End If
        dgTrainingList.DataSource = mTrainingListForEmployeeGrouping
        dgTrainingList.DataBind()
        Session("mTrainingListForEmployeeGrouping") = mTrainingListForEmployeeGrouping

        lblResult.Text = "Training List: " & mTrainingListForEmployeeGrouping.Count & " Record(s) Found."
        'AJAX
        upnlSearchCriteria.Update()
        upnlValidationSummary.Update()
        upnlGridView.Update()
        'End
    End Sub
    Protected Sub txtFor_TextChanged(sender As Object, e As EventArgs) Handles txtFor.TextChanged
        If cmbSearchType.SelectedValue = 0 Then
            mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, , , , )
        ElseIf cmbSearchType.SelectedValue = 1 Then
            mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, Trim(txtFor.Text), , , )
        ElseIf cmbSearchType.SelectedValue = 2 Then
            mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, , txtFor.Text, , )
        End If
        dgTrainingList.DataSource = mTrainingListForEmployeeGrouping
        dgTrainingList.DataBind()
        Session("mTrainingListForEmployeeGrouping") = mTrainingListForEmployeeGrouping

        lblResult.Text = "Training List: " & mTrainingListForEmployeeGrouping.Count & " Record(s) Found."
        'AJAX
        upnlSearchCriteria.Update()
        upnlValidationSummary.Update()
        upnlGridView.Update()
        'End
    End Sub
    Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click
        'Session("MiddleFrame") = ""
        Session.Remove("mTrainingListForEmployeeGrouping")
        Response.Redirect("index.aspx")
    End Sub
    Private Sub dgTrainingList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgTrainingList.Sorting
        mTrainingListForEmployeeGrouping.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mTrainingListForEmployeeGrouping") = mTrainingListForEmployeeGrouping
        dgTrainingList.DataSource = mTrainingListForEmployeeGrouping
        dgTrainingList.DataBind()
    End Sub
    Private Sub dgTrainingList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTrainingList.RowCommand
        Select Case e.CommandName
            Case "Allocate"
                Dim mID As Guid = mTrainingListForEmployeeGrouping(CType(e.CommandArgument.ToString, Integer) + dgTrainingList.PageIndex * dgTrainingList.PageSize).ID
                Session("mTrainingID") = mID
                ' ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenGroupEmpTrainingWindow", "OpenGroupEmpTrainingWindow()", True)
                Session("SkipNames") = mTrainingListForEmployeeGrouping(CType(e.CommandArgument.ToString, Integer) + dgTrainingList.PageIndex * dgTrainingList.PageSize).ApplicableToEmployees
                Dim str As String
                str = "openledgersame('wfGroupTrainingConfiguration_Ajax.aspx?" & "');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)

        End Select
    End Sub
    Private Sub hdnBtnEmployeeTraining_Click(sender As Object, e As System.EventArgs) Handles hdnBtnEmployeeTraining.Click
        If cmbSearchType.SelectedValue = 0 Then
            mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, , , , )
        ElseIf cmbSearchType.SelectedValue = 1 Then
            mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, Trim(txtFor.Text), , , )
        ElseIf cmbSearchType.SelectedValue = 2 Then
            mTrainingListForEmployeeGrouping = TrainingListForEmployeeGrouping.GetTrainingListForEmployeeGrouping(, , txtFor.Text, , )
        End If
        dgTrainingList.DataSource = mTrainingListForEmployeeGrouping
        dgTrainingList.DataBind()
        Session("mTrainingListForEmployeeGrouping") = mTrainingListForEmployeeGrouping

        lblResult.Text = "Training List: " & mTrainingListForEmployeeGrouping.Count & " Record(s) Found."
        'AJAX
        SetSession()
        upnlSearchCriteria.Update()
        upnlValidationSummary.Update()
        upnlGridView.Update()
        'End
    End Sub
#End Region



End Class