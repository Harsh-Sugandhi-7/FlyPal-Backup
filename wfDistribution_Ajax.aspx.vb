Public Class wfDistribution_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mModelList As ModelList
    Public mDistributionList As DistributionList
    Public mDistribution As Distribution
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub SetSession()
        Session("mDistribution") = mDistribution
        Session("mDistributionList") = mDistributionList
        Session("mModelList") = mModelList
    End Sub
    Private Sub GetSession()
        mDistribution = Session("mDistribution")
        mDistributionList = Session("mDistributionList")
        mModelList = Session("mModelList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mDistribution")
        Session.Remove("mDistributionList")
        Session.Remove("mModelList")
    End Sub
    Private Sub NewRecord()
        mDistribution = Distribution.NewDistribution(Guid.NewGuid, Guid.Empty, "", 0)
        Session("mDistribution") = mDistribution
        lbltitle.Text = "Distribution [New]"
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mDistribution = Distribution.GetDistribution(mId, Guid.Empty)
        Session("mDistribution") = mDistribution
        SetFocus(cmbModelList)
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mDistribution = Distribution.GetDistribution(mId, Guid.Empty)
        Session("mDistribution") = mDistribution
    End Sub
    Private Sub SetObject()
        mDistribution.ModelID = New Guid(cmbModelList.SelectedValue)
        mDistribution.Name = Trim(txtName.Text)
        'Added By Vikrant On 03-Sept-2013 For ALL02092013-2
        Dim Count As Integer = DistributionList.GetDistributionList(New Guid(cmbModelList.SelectedValue)).Count
        mDistribution.SrNo = IIf(Count > 0, Count + 1, 1)
        'End

        If cmbCategory.SelectedValue = "(SELECT)" Then
            mDistribution.CategoryName = ""
        Else

            mDistribution.CategoryName = cmbCategory.SelectedItem.ToString
        End If
        
        mDistribution.Remark = txtRemark.Text
        Session("mDistribution") = mDistribution
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        'If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
        '    Result1 = -1
        'Else
        '    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        'End If
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim TempModelID As Guid = Guid.Empty
                        Try
                            Session("sender") = ""
                            mDistribution = Session("mDistribution")
                            TempModelID = mDistribution.ModelID
                            Distribution.DeleteDistribution(mDistribution.ID)
                            MarkLog(Action.Delete, "DistributionList", "Distribution Name : " + mDistribution.Name + ", " + "Model : " + mDistribution.ModelName, ErrorType.NoError, mDistribution.ID, EventLogID)
                            mDistributionList = DistributionList.GetDistributionList(TempModelID)
                            If mDistributionList.Count > 0 Then
                                Dim J As Integer = 1
                                For i As Integer = 0 To mDistributionList.Count - 1
                                    mDistribution.SrNo = J
                                    mDistribution.DistributionListSrNoUpdate(mDistributionList(i).ID, mDistributionList(i).Name, mDistributionList(i).ModelID, J, mDistributionList(i).CategoryName, mDistributionList(i).Remark)
                                    J = J + 1
                                Next
                            End If
                            'Response.Redirect("wfDistribution.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
                            NewRecord()
                            mDistributionList = DistributionList.GetDistributionList(Guid.Empty, "")
                            Session("mDistributionList") = mDistributionList
                            dgDistribution.DataSource = mDistributionList
                            DataBind()
                            cmbSearchModelList.SelectedIndex = CInt(0)
                            lblResult.Text = "Distribution List : " & mDistributionList.Count & " Record(s) Found."
                            upnlDistributionList.Update()

                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfDistribution.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfDistribution.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfDistribution.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        NewRecord()
                        upnlDistributionList.Update()
                    End If
                Case MsgBoxResult.OK ' And Session("sender") = ""        'Code Added
                    'Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfDistribution.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    'Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfDistribution.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
            End Select
        ElseIf Result1 = -1 Then
            'Session("sender") = ""
            'DataFieldBind()
            'Response.Redirect("wfDistribution.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            'Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()

        '''mModelList = ModelList.GetModelList(1, "", Guid.Empty.ToString, Guid.Empty.ToString, "(SELECT)")
        mModelList = ModelList.GetAirframeModelList("(SELECT)")
        cmbModelList.DataSource = mModelList
        Session("mModelList") = mModelList

        cmbSearchModelList.DataSource = mModelList

        mDistributionList = DistributionList.GetDistributionList(Guid.Empty, "")
        Session("mDistributionList") = mDistributionList
        dgDistribution.DataSource = mDistributionList
        DataBind()

        lblResult.Text = "Distribution List : " & mDistributionList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("Sender"), String) = "" Then
            Session("MiddleFrame") = "wfDistribution_Ajax.aspx"
            If cmbModelList.Enabled = True Then
                SetFocus(cmbModelList)
            End If
            NewRecord()
            DataFieldBind()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            Try
                SetObject()
                mDistribution.Save()
                If cmbModelList.Enabled = True Then
                    SetFocus(cmbModelList)
                End If
                MarkLog(Util.Action.Save, "DistributionList", mDistribution.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                NewRecord()
                mDistribution = Session("mDistribution")
                DataFieldBind()
                SetSession()
                txtName.Text = ""
                txtSearchDesc.Text = ""
                txtRemark.Text = ""
                cmbCategory.SelectedIndex = 0
            Catch ex As SqlException
                If ex.Number = 8145 Then
                      MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 2627 Then
                      MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 547 Then
                     MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                End If
            End Try
        End If
    End Sub
   Private Sub dgDistribution_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDistribution.RowCommand
        Dim Idx As Int32
        Dim mId As Guid
        Select e.CommandName
            Case "EditRec"
                Idx = CInt(e.CommandArgument) + dgDistribution.PageIndex * dgDistribution.PageSize
                mId = mDistributionList(Idx).ID
                EditRecord(mId)
                cmbModelList.DataBind()
                txtName.DataBind()

                If mDistribution.CategoryName = "" Then
                    cmbCategory.SelectedValue = "(SELECT)"
                Else
                    cmbCategory.SelectedValue = mDistribution.CategoryName
                End If

                cmbCategory.DataBind()
                txtRemark.DataBind()
                MarkLog(Util.Action.Edit, "DistributionList", mDistribution.Name, Util.ErrorType.NoError, mDistribution.ID, EventLogID)
                If Len(mDistribution.Name) > 15 Then
                    lbltitle.Text = "Distribution[" & mDistribution.Name.Substring(0, 15) & "...]"
                Else
                    lbltitle.Text = "Distribution[" & mDistribution.Name & "]"
                End If
            Case "DeleteRec"
                Idx = CInt(e.CommandArgument) + dgDistribution.PageIndex * dgDistribution.PageSize
                mId = mDistributionList(Idx).ID
                DeleteRecord(mId)
        End Select

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
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        mDistributionList = DistributionList.GetDistributionList(IIf(cmbSearchModelList.SelectedIndex > 0, New Guid(cmbSearchModelList.SelectedValue), Guid.Empty), txtSearchDesc.Text.Trim)
        dgDistribution.DataSource = mDistributionList
        Session("mDistributionList") = mDistributionList
        DataBind()
        lblResult.Text = "Distribution List : " & mDistributionList.Count & " Record(s) Found."
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
        MarkLog(Util.Action.[New], "DistributionList", "", Util.ErrorType.NoError, mDistribution.ID, EventLogID)
        cmbModelList.DataBind()
        txtName.DataBind()
        cmbSearchModelList.DataBind()
        txtSearchDesc.DataBind()
        cmbCategory.ClearSelection()
        cmbCategory.SelectedIndex = 0
        txtRemark.DataBind()
        If cmbModelList.Enabled Then
            SetFocus(cmbModelList)
        End If
    End Sub
     Private Sub lnkCopyDistribution_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCopyDistribution.Click
        Dim str As String
        str = "openledgersame('wfDistributionCopy_Ajax.aspx?BackPage=Index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

   
End Class