

'AJAX Created By Saylee On 15-May-2015

Imports System.Web.Services
Imports System.Text
Imports System.Collections.Generic

Public Class wfSelectTaskCardList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mTaskCard As TaskCard
    Dim mTaskCardList As TaskCardList
    Dim mModelID, ModelName As String
    Dim mModelList As ModelList


    Public mMaintenanceTask As MaintenanceTask
    Dim mTaskCardNo, mInspInterval As String

    Private checkedIds As New List(Of String)()
    'Dim mMROJobMaster As MROJobMaster
    Dim mnWOJob As nWOJob

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mTaskCardList = Session("mSelectTaskCardList")
        mTaskCard = Session("mTaskCard")
        mModelList = Session("mModelList")
        'ModelID = Session("ModelID")
        'ModelName = Session("ModelName")

        mMaintenanceTask = CType(Session("mMaintenanceTask"), MaintenanceTask) 'Added By Saylee on 7-Nov-2013 for ALL07112013

        mTaskCardNo = Session("mTaskCardNo")
        mInspInterval = Session("mInspInterval")
        mModelID = Session("mModelID")

    End Sub
    Private Sub SetSession()
        Session("mTaskCard") = mTaskCard
        Session("mSelectTaskCardList") = mTaskCardList
        Session("mModelList") = mModelList
        'Session("ModelID") = ModelID
        'Session("ModelName") = ModelName


    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub AddTaskCards()
        Dim checkString = Request.Form("chkSelect")

        If checkString Is Nothing Then
            'MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            ' we'll need a split to get the individual ids
            Dim values = checkString.Split(","c)
            For Each value As String In values
                checkedIds.Add(value)
                mTaskCardList(New Guid(value)).IsSelect = True
            Next

            For j As Integer = 0 To mTaskCardList.Count - 1
                If mTaskCardList(j).IsSelect = True And Array.IndexOf(values, mTaskCardList(j).ID.ToString) = -1 Then
                    mTaskCardList(j).IsSelect = False
                End If
            Next
            values = ""
            checkString = Nothing
        End If
        'Dim item As GridViewRow
        'Dim chkBox As CheckBox
        'Dim Recordno, PageItems As Integer
        'Dim i As Integer
        'PageItems = dgTaskCardList.Rows.Count - 1
        '' Set Selected Notes value  
        'For i = 0 To PageItems
        '    Recordno = i + dgTaskCardList.PageSize * dgTaskCardList.PageIndex
        '    item = dgTaskCardList.Rows(i)
        '    chkBox = CType(item.FindControl("chkSelect"), CheckBox)
        '    mTaskCardList(Recordno).IsSelect = chkBox.Checked
        'Next
        SetSession()
    End Sub
    Private Sub CheckTaskCards()
        'Added By Saylee on 7-Nov-2013 for ALL07112013
        Dim item As GridViewRow
        Dim chkBox As CheckBox
        Dim Recordno, PageItems As Integer
        Dim i As Integer
        PageItems = dgTaskCardList.Rows.Count - 1
        ' Set Selected Notes value  
        For i = 0 To PageItems
            Recordno = i + dgTaskCardList.PageSize * dgTaskCardList.PageIndex
            item = dgTaskCardList.Rows(i)
            chkBox = CType(item.FindControl("chkSelect"), CheckBox)
            If mMaintenanceTask.MaintenanceTaskDetails.Contains(mTaskCardList(Recordno).ID, "") Or mTaskCardList(Recordno).IsSelect = True Then
                chkBox.Checked = True
                If mMaintenanceTask.MaintenanceTaskDetails.Contains(mTaskCardList(Recordno).ID, "") Then mTaskCardList(Recordno).IsSelect = True
            End If
        Next
        SetSession()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        GetSession()
        'txtModel.Text = ModelName 'mTaskCardList.CurrentItem.ModelName 

        If Session("IsOpenFrom") = "MRO" Then
            'mMROJobMaster = Session("mMROJobMaster")
            'If Not mTaskCardList Is Nothing Then
            '    For i As Integer = 0 To mTaskCardList.Count - 1
            '        mTaskCardList(i).IsSelect = mMROJobMaster.MROJobMasterTasks.Contains(mTaskCardList(i).ID)
            '        If mTaskCardList(i).IsSelect Then
            '            checkedIds.Add(mTaskCardList(i).ID.ToString)
            '        End If
            '    Next
            'End If
        ElseIf Session("IsOpenFrom") = "WorkOrder" Then
            mnWOJob = Session("mnWOJob")
            If Not mTaskCardList Is Nothing Then
                For i As Integer = 0 To mTaskCardList.Count - 1
                    mTaskCardList(i).IsSelect = mnWOJob.WOJobTasks.Contains(mTaskCardList(i).ID, "")
                    If mTaskCardList(i).IsSelect Then
                        checkedIds.Add(mTaskCardList(i).ID.ToString)
                    End If
                Next
            End If
            btnAddWOJobTask.Visible = True
        Else
            'Added By Saylee on 7-Nov-2013 for ALL07112013
            If Not mTaskCardList Is Nothing Then
                For i As Integer = 0 To mTaskCardList.Count - 1
                    mTaskCardList(i).IsSelect = mMaintenanceTask.MaintenanceTaskDetails.Contains(mTaskCardList(i).ID, "")
                    If mTaskCardList(i).IsSelect Then
                        checkedIds.Add(mTaskCardList(i).ID.ToString)
                    End If
                Next
            End If
            btnAddWOJobTask.Visible = False
        End If

        'Dim item As GridViewRow
        'Dim chkBox As CheckBox
        'Dim Recordno, PageItems As Integer
        'Dim i As Integer
        'PageItems = dgTaskCardList.Rows.Count - 1
        '' Set Selected Notes value  
        'For i = 0 To PageItems
        '    Recordno = i + dgTaskCardList.PageSize * dgTaskCardList.PageIndex
        '    item = dgTaskCardList.Rows(i)
        '    chkBox = CType(item.FindControl("chkSelect"), CheckBox)
        '    If mMaintenanceTask.MaintenanceTaskDetails.Contains(mTaskCardList(Recordno).ID, "") Then
        '        mTaskCardList(Recordno).IsSelect = True
        '        chkBox.Checked = True
        '    End If
        'Next
        dgTaskCardList.DataSource = mTaskCardList
        Session("mSelectTaskCardList") = mTaskCardList
        dgTaskCardList.DataBind()
        'End


        mModelList = ModelList.GetModelList(0, , , , "(All)")
        Session("mModelList") = mModelList
        cmbModelList.DataSource = mModelList

        DataBind()




        txtTaskCardNo.Text = Session("mTaskCardNo")
        txtInspTypeIntervalSearch.Text = Session("mInspInterval")
        If mModelID <> "" Then cmbModelList.SelectedValue = mModelID.ToString
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        setFocus(txtTaskCardNo)
        If Not IsPostBack And Session("sender") = "" Then
            'ModelID = ((New Guid(Request.QueryString("ModelID"))).ToString)
            'ModelName = Request.QueryString("ModelName")

            'Now we are not getting Task Card as per Model
            'mTaskCardList = mTaskCardList.GetTaskCardList(" ", "", "", "", ModelID, "", "")
            If mTaskCardList Is Nothing Then
                mTaskCardList = TaskCardList.GetTaskCardList(" ", "", "", "", Guid.Empty.ToString, "", "")
                '-------------------
            End If
            SetSession()
            DataFieldBind()
            lblResult.InnerText = "List of Task Cards as per Model:" & mTaskCardList.Count & " Record(s) found."
            btnDone1.Visible = mTaskCardList.Count > 25
        End If
    End Sub
    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click, btnDone1.Click
        AddTaskCards()
        Session("AddTaskCards") = "True"
        mTaskCardNo = ""
        mInspInterval = ""
        mModelID = ""

        Session("mTaskCardNo") = mTaskCardNo
        Session("mInspInterval") = mInspInterval
        Session("mModelID") = mModelID
        Session.Remove("IsOpenFrom")
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If


        Response.Redirect(Request.QueryString("BackPage5") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4"))
    End Sub
    'Private Sub imgbtnTaskCard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgbtnTaskCard.Click
    '    AddTaskCards()
    '    Response.Redirect("wfTaskCardList.aspx?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=wfSelectTaskCardList.aspx")
    'End Sub
    Private Sub imgTaskCard_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgTaskCard.Click
        AddTaskCards()
        Session.Remove("mTaskCardList")
        Session.Remove("TaskCardNo")
        Session.Remove("TaskDesc")
        Session.Remove("InspTypeIntervalSearch")
        Session.Remove("ModelID")
        Session("POPUpPage") = "wfSelectTaskCardList_Ajax.aspx"
        Session("GChildPage7") = "wfSelectTaskCardList_Ajax.aspx"
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskMasterWindow", "OpenTaskMasterWindow();", True)
        'Response.Redirect("wfTaskCardList_Ajax.aspx?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=wfSelectTaskCardList_Ajax.aspx")
    End Sub
    Private Sub imgFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgFindNow.Click, hdnBtnTaskMaster.Click
        mTaskCardList = TaskCardList.GetTaskCardList("", "", "", txtTaskCardNo.Text, IIf(cmbModelList.SelectedIndex <= 0, Guid.Empty.ToString, cmbModelList.SelectedValue), "", "", txtInspTypeIntervalSearch.Text.Trim)
        SetSession()
        If Session("IsOpenFrom") = "MRO" Or Session("IsOpenFrom") = "WorkOrder" Then
            DataFieldBind()
        Else
            dgTaskCardList.DataSource = mTaskCardList
            Session("mSelectTaskCardList") = mTaskCardList
            dgTaskCardList.DataBind()
        End If


        lblResult.InnerText = "List of Task Cards as per Model:" & mTaskCardList.Count & " Record(s) found."
        btnDone1.Visible = mTaskCardList.Count > 25

        Session("mTaskCardNo") = Trim(txtTaskCardNo.Text)
        Session("mInspInterval") = Trim(txtInspTypeIntervalSearch.Text)
        Session("mModelID") = IIf(cmbModelList.SelectedIndex <= 0, Guid.Empty.ToString, cmbModelList.SelectedValue)

        mTaskCardNo = Trim(txtTaskCardNo.Text)
        mInspInterval = Trim(txtInspTypeIntervalSearch.Text)
        mModelID = IIf(cmbModelList.SelectedIndex <= 0, Guid.Empty.ToString, cmbModelList.SelectedValue)
        upnlTaskCardInfo.Update()
        upnlTaskCardOnfo.Update()
    End Sub
    Private Sub btnAddWOJobTask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddWOJobTask.Click
        Dim Index As Integer = -1
        Session("mIndex") = "-1"
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToAddJobTaskDetail", "OpenToAddJobTaskDetail('" + Index.ToString + "');", True)
    End Sub
#End Region

#Region "Checked Selection"
    Public Function NumeroChequeInclus(ByVal numero As String) As String
        If (checkedIds.Contains(numero)) Then
            Return "checked"
        Else
            Return String.Empty
        End If
    End Function
#End Region

End Class