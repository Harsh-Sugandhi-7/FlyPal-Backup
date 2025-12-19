Public Class wfDistributionCopy_Ajax
    Inherits System.Web.UI.Page

    
#Region " Variable Declaration "
    Public mDistributionList As DistributionList
    Public mCopyModelList As ModelList
    Public mSourceModel As String
    Public mDestinationModel As String
    Public mSourceModelID As Guid
    Public mDestinationModelID As Guid
    Public mSourceModelName As String
    Public mDestinationModelName As String
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mCopyModelList = CType(Session("mCopyModelList"), ModelList)
        mSourceModel = Session("mSourceModel")
        mDestinationModel = Session("mDestinationModel")
        mSourceModelID = CType(Session("mSourceModelID"), Guid)
        mDestinationModelID = CType(Session("mDestinationModelID"), Guid)
        mDistributionList = Session("mDistributionList")
        mSourceModelName = Session("mSourceModelName")
        mDestinationModelName = Session("mDestinationModelName")
    End Sub
    Private Sub SetSession()
        Session("mCopyModelList") = mCopyModelList
        Session("mSourceModel") = mSourceModel
        Session("mDestinationModel") = mDestinationModel
        Session("mSourceModelID") = mSourceModelID
        Session("mDestinationModelID") = mDestinationModelID
        Session("mSourceModelName") = mSourceModelName
        Session("mDestinationModelName") = mDestinationModelName
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCopyModelList")
        Session.Remove("mSourceModel")
        Session.Remove("mDestinationModel")
        Session.Remove("mSourceModelID")
        Session.Remove("mDestinationModelID")
        Session.Remove("Copied")
        Session.Remove("mSourceModelName")
        Session.Remove("mDestinationModelName")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Continue1" Then
                        Try
                            Session("sender") = ""
                            DeleteAllRecordsForModel()
                            CopyRecords()
                             cmbFromModel.SelectedIndex = CInt(0)
                            cmbToModel.SelectedIndex = CInt(0)
                            upnlDistributionCopy.Update()
                        Catch ex As SqlException

                        Finally

                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Cancel

                Case MsgBoxResult.OK
                    Session("sender") = ""
             End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetVariables()

        If Not mCopyModelList.Item(cmbFromModel.SelectedIndex).ID.Equals(Guid.Empty) Then
            mSourceModelID = New Guid(cmbFromModel.SelectedValue.ToString)
            mSourceModelName = cmbFromModel.SelectedItem.ToString
        Else
            mSourceModelID = Guid.Empty
            mSourceModelName = ""
        End If
        If Not mCopyModelList.Item(cmbToModel.SelectedIndex).ID.Equals(Guid.Empty) Then
            mDestinationModelID = New Guid(cmbToModel.SelectedValue.ToString)
            mDestinationModelName = cmbToModel.SelectedItem.ToString
        Else
            mDestinationModelID = Guid.Empty
            mDestinationModelName = ""
        End If
        SetSession()
    End Sub
    Private Sub DeleteAllRecordsForModel()
        mDistributionList = DistributionList.GetDistributionList(mDestinationModelID)
        For i As Integer = 0 To mDistributionList.Count - 1
            Distribution.DeleteDistribution(mDistributionList(i).ID)
        Next
    End Sub
    Private Sub CopyRecords()
        mDistributionList = Session("mDistributionList")
        Dim J As Integer = 1 'Added By Prashant On 03-Sept-2013 For ALL02092013-2
        For i As Integer = 0 To mDistributionList.Count - 1
            Dim mOldDistribution As Distribution
            Dim mNewDistribution As Distribution
            mOldDistribution = Distribution.GetDistribution(mDistributionList(i).ID, Guid.Empty)

            mNewDistribution = Distribution.NewDistribution(Guid.NewGuid, mDestinationModelID, "", J)
            mNewDistribution.Name = mOldDistribution.Name
            mNewDistribution.CategoryName = mOldDistribution.CategoryName 'Added By Prashant 9-Feb-2022 ALL09022022
            mNewDistribution.Remark = mOldDistribution.Remark 'Added By Prashant 9-Feb-2022 ALL09022022
            Try

                If mNewDistribution.IsValid Then
                    mNewDistribution = CType(mNewDistribution.Save(), Distribution)
                    J = J + 1 'Added By Prashant On 03-Sept-2013 For ALL02092013-2
                    Session("Copied") = "True"
                End If
            Catch ex As SqlException
                If ex.Number = 2627 Or ex.Number = 50000 Then
                    ''mErrorString = mErrorString + vbNewLine + mNewDistribution.Number + " - Duplicate"
                    Session("Copied") = "False"
                Else
                    ''mErrorString = mErrorString + vbNewLine + mNewDistribution.Number + " - " + ex.Message
                End If
            Finally

            End Try
            mNewDistribution = Nothing

        Next i
        MarkLog(Util.Action.Save, "Copy Distribution", "Copied Distribution(s) From " + mSourceModelName + " To " + mDestinationModelName, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        If Session("Copied") = "True" Then
            MSGBoxCtrl.show("Copied Successfully", "Success!!!", "Distribution(s) has been copied successfully.", MsgBoxStyle.OkOnly, "")
        ElseIf Session("Copied") = "False" Then
            MSGBoxCtrl.show("", "Failure!!!", "Error In Copying Distribution(s).", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    Private Sub ControlVisibility()
        btnCopy.Enabled = IIf(dgDistribution.Rows.Count > 0, True, False)
    End Sub
#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()
        mSourceModel = Session("mSourceModel")
        mDestinationModel = Session("mDestinationModel")
        mSourceModelID = IIf(IsNothing(mSourceModelID), Guid.Empty, mSourceModelID)
        mDestinationModelID = IIf(IsNothing(mDestinationModelID), Guid.Empty, mDestinationModelID)
        mSourceModelName = IIf(IsNothing(mSourceModelName), "", mSourceModelName)
        mDestinationModelName = IIf(IsNothing(mDestinationModelName), "", mDestinationModelName)

        mCopyModelList = ModelList.GetModelList(1, "", , , "(SELECT)")

        If mCopyModelList.Count > 0 Then
            cmbFromModel.DataSource = mCopyModelList
            cmbToModel.DataSource = mCopyModelList
            Session("mCopyModelList") = mCopyModelList
            DataBind()
            setFocus(cmbFromModel)

        End If

        If cmbFromModel.SelectedIndex > 0 Then
            mDistributionList = DistributionList.GetDistributionList(New Guid(cmbFromModel.SelectedValue))
            lblHeader.Text = "List of Distribution(s) For Model " + cmbFromModel.SelectedItem.Text
            dgDistribution.DataSource = mDistributionList
            Session("mDistributionList") = mDistributionList
        Else
            mDistributionList = DistributionList.GetDistributionList(New Guid("{11111111-1111-1111-1111-111111111111}"))
            lblHeader.Text = "List of Distribution(s)"
            dgDistribution.DataSource = mDistributionList
            Session("mDistributionList") = mDistributionList
        End If
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If cmbFromModel.Enabled = True Then
                SetFocus(cmbFromModel)
            End If
            DataFieldBind()
            cmbFromModel.SelectedValue = mSourceModelID.ToString
            cmbToModel.SelectedValue = mDestinationModelID.ToString
            DataBind()
        End If
        ControlVisibility()
    End Sub
    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        SetVariables()

        If Not IsValid Then Exit Sub
        Session("Copied") = ""

        If Not mCopyModelList.Item(cmbFromModel.SelectedIndex).ID.Equals(mCopyModelList.Item(cmbToModel.SelectedIndex).ID) Then
            mDistributionList = DistributionList.GetDistributionList(New Guid(cmbFromModel.SelectedValue.ToString))
            Session("mDistributionList") = mDistributionList

            If mDistributionList.Count > 0 Then
              MSGBoxCtrl.show("Alert!", "You are copying all Distribution(s) from " + cmbFromModel.SelectedItem.ToString + " to " + cmbToModel.SelectedItem.ToString + ".<BR>" + "All Previous Distribution(s) of " + cmbToModel.SelectedItem.ToString + " will get deleted.<BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo, "Continue1")
            End If
        End If
    End Sub
    Private Sub cmbFromModel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFromModel.SelectedIndexChanged
        If cmbFromModel.SelectedIndex > 0 Then
            mDistributionList = DistributionList.GetDistributionList(New Guid(cmbFromModel.SelectedValue))
            lblHeader.Text = "List of Distribution(s) For Model " + cmbFromModel.SelectedItem.Text
        Else
            mDistributionList = DistributionList.GetDistributionList(New Guid("{11111111-1111-1111-1111-111111111111}"))
            lblHeader.Text = "List of Distribution(s)"
        End If
        dgDistribution.DataSource = mDistributionList
        dgDistribution.DataBind()
        Session("mDistributionList") = mDistributionList
        ControlVisibility()
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "Copy Distribution", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Response.Redirect(Request.QueryString("Backpage"))
    End Sub
    Private Sub dgDistribution_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgDistribution.PageIndexChanging
        dgDistribution.PageIndex = e.NewPageIndex
        dgDistribution.DataSource = mDistributionList
        Session("mDistributionList") = mDistributionList
        dgDistribution.DataBind()
    End Sub
    Private Sub dgDistribution_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDistribution.Sorting
        mDistributionList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mDistributionList") = mDistributionList
        dgDistribution.DataSource = mDistributionList
        dgDistribution.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region
End Class