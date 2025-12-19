'Added by vikrant on 22-Jun-2015
Imports System.Web.UI.WebControls
Public Class wfUpdateTaskCardAMPRevNoByModel_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mTaskCard As TaskCard
    Dim mTaskCardList As TaskCardList
    Dim mModelList As ModelList
    Dim EventLogID As Guid 'Added by Vikrant on 20-July-2011
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mTaskCardList = Session("mTaskCardList")
        mModelList = Session("mModelList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mTaskCardList")
        Session.Remove("mModelList")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "SaveAMPNo" Then
                        Try
                            Try
                                Save()
                                FindNow()
                                upnlDetails.Update()
                            Catch ex As Exception

                            End Try

                        Catch ex As Exception


                        Finally

                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok

                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added

            End Select
        ElseIf Result1 = -1 Then

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added

        End If
    End Sub
    Private Sub FindNow(Optional ByVal ModelID As String = "")
        mTaskCardList = Nothing
        dgTaskCardList.DataSource = Nothing
        mTaskCardList = TaskCardList.GetTaskCardList(, , , , cmbModelList.SelectedValue.ToString, , "", )

        dgTaskCardList.DataSource = mTaskCardList
        dgTaskCardList.DataBind()
        If mTaskCardList.Count > 0 Then
            txtExistingAMPNo.Text = mTaskCardList(0).AMPIssueRev
        Else
            txtExistingAMPNo.Text = ""
        End If
        txtExistingAMPNo.DataBind()
        Session("mTaskCardList") = mTaskCardList
        lblResult.Text = "List of Task Cards as per criteria : " & "" & mTaskCardList.Count & " Record(s) found."
    End Sub
    Public Property dir() As SortDirection
        Get
            If ViewState("dirState") Is Nothing Then
                ViewState("dirState") = SortDirection.Ascending
            End If
            Return DirectCast(ViewState("dirState"), SortDirection)
        End Get
        Set(ByVal value As SortDirection)
            ViewState("dirState") = value
        End Set
    End Property
    Private Function BindGridView() As DataTable
        Dim dtGrid As New DataTable()
        Dim dAdapter As New CSLA10.Data.ObjectAdapter
        dAdapter.Fill(dtGrid, mTaskCardList)
        Return dtGrid
    End Function
    Private Sub Save()
        Try
            mTaskCard = New TaskCard
            mTaskCard.UpdateTaskCardAMPRevNoByModel(New Guid(cmbModelList.SelectedValue.ToString), txtNewAMPNo.Text.Trim)
        Catch ex As Exception
            MSGBoxCtrl.show("Alert", "Error In Updating AMP Issue/Rev No.", ex.InnerException.ToString, MsgBoxStyle.OkOnly, "")
            Exit Sub
        End Try
        MarkLog(Util.Action.Save, "UpdateAMPIssue/RevNo", "User Name : " + HttpContext.Current.User.Identity.Name + ";" + " Date Time : " + Now.ToString + ";" + " Old AMP Issue/Rev No. : " + txtExistingAMPNo.Text + ";" + " New AMP Issue/Rev No. : " + txtNewAMPNo.Text, ErrorType.NoError, Guid.Empty, EventLogID)
        ClearAll()
    End Sub
    Private Sub ClearAll()
        txtNewAMPNo.Text = ""
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mModelList = ModelList.GetModelList(0, , , , )
        Session("mModelList") = mModelList
        cmbModelList.DataSource = mModelList
        cmbModelList.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            FindNow()
        End If
    End Sub
    Private Sub btnUpdateAMPNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateAMPNo.Click
        If dgTaskCardList.Rows.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record to Update for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Confirmation, "AMP Issue/Rev No. " + "<b>" + txtNewAMPNo.Text + "</b>" + " will get updated to All the Task Cards of Model " + cmbModelList.SelectedItem.ToString + ".<BR><BR>Do you want to continue ?", MsgBoxStyle.YesNo, "SaveAMPNo")
    End Sub
    Private Sub cmbModelList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbModelList.SelectedIndexChanged
        FindNow()
        ClearAll()
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub dgTaskCardList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
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
        dgTaskCardList.DataSource = sortedView
        dgTaskCardList.DataBind()
    End Sub
#End Region



End Class