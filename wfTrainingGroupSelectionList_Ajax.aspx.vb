

Imports System.Web.Services
Imports System.Text
Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq
Imports System
Imports System.IO

Public Class wfTrainingGroupSelectionList_Ajax
    Inherits System.Web.UI.Page



#Region " Variable Declaration "
    Dim mGroupTraining As GroupTraining
    Dim mGroupTrainingList As GroupTrainingList
    Dim mGroupTrainingForCombo As GroupTrainingList
    Public mTrainingList As TrainingList
    Dim Index, Text As String

    Dim EventLogID As Guid

    Public mEmployee As Employee
#End Region

#Region " Business Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Try
            Dim str As String
            str = "try{document.getElementById('" + cntrl.ClientID + "').focus();}catch (Error) {}"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
        Catch ex As Exception
            '
        End Try
    End Sub
    Private Sub GetSession()
        mGroupTraining = Session("mGroupTraining")
        mGroupTrainingList = Session("mGroupTrainingList")
        mGroupTrainingForCombo = Session("mGroupTrainingForCombo")
        Index = Session("Index")
        mEmployee = Session("mEmployee")
    End Sub
    Private Sub SetSession()
        Session("mGroupTraining") = mGroupTraining
        Session("mGroupTrainingList") = mGroupTrainingList
        Session("mGroupTrainingForCombo") = mGroupTrainingForCombo
        Session("Index") = Index
        Session("mEmployee") = mEmployee
    End Sub
    Private Sub RemoveSession()
        Session.Remove("Index")

        Session.Remove("mGroupTraining")
        Session.Remove("mGroupTrainingList")
    End Sub
  
   
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult

        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Dim mGroupTraining As Kit
                            Session("Sender") = ""
                            mGroupTraining = CType(Session("mGroupTraining"), Kit)
                            'mGroupTraining.DeleteKit(mGroupTraining.ID)
                            mGroupTraining.Delete()
                            mGroupTraining.Save()
                            DataFieldBind()

                            upnlTrainingList.Update()
                            upnlGridViewTitle.Update()
                           
                            MarkLog(Util.Action.Delete, "Inspection Kit", mGroupTraining.KitName, Util.ErrorType.NoError, mGroupTraining.ID, EventLogID)
                            'Response.Redirect("wfKitList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'Added by Vikrant
                                MarkLog(Util.Action.Delete, "Currency", "Can't delete :" & mGroupTraining.GroupName & " is Currently in use", Util.ErrorType.NoError, mGroupTraining.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                    'Response.Redirect("wfKitList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfKitList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
                    'Response.Redirect("wfKitList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfKitList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    ''======Added By Prashant on 10-Dec-2007==========
    'Private Sub FindNow(Optional ByVal cmbGroupType As Int16 = 1, Optional ByVal KitName As String = "", Optional ByVal ItemName As String = "")
    '    If cmbGroupType = -1 Then
    '        cmbGroupType = 0   ' This step is IMP when details form  is opened directly.
    '    End If
    '    If cmbGroupType = 0 Then
    '        mGroupTrainingList = KitList.GetKitList(0, "", "")
    '    Else
    '        mGroupTrainingList = KitList.GetKitList(cmbGroupType, KitName, ItemName)
    '    End If
    '    dgTrainingList.DataSource = mGroupTrainingList
    '    Session("mGroupTrainingList") = mGroupTrainingList
    '    dgTrainingList.DataBind()
    '    lblResult.Text = "List of Inspection Kit as per criteria :" + CType(mGroupTrainingList.Count, String) + " Record(s) found."
    '    upnlGrid.Update()
    '    upnlGridViewTitle.Update()
    'End Sub
    ''================================================
    Private Sub SetControl()
        '======Added By Prashant on 10-Dec-2007==========
        Index = Session("Index")
        Text = Session("Text")
        'FindNow(Index, Text, Text)

        cmbGroup.SelectedIndex = Index
        ChklistTrainingList.DataBind()
        '=============================================
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mTrainingList = TrainingList.GetTrainingList()
        Session("mTrainingList") = mTrainingList

       

        Index = IIf(IsNothing(Index), 0, Index)
        Session("Index") = Index
        mGroupTrainingForCombo = GroupTrainingList.GetGroupTrainingList(TrainingsRequired:=False, DesignationID:=mEmployee.DesignationID.ToString, AddTopItem:="(SELECT)")
        cmbGroup.DataSource = mGroupTrainingForCombo
        Session("mGroupTrainingForCombo") = mGroupTrainingForCombo

        cmbGroup.DataBind()
        lblResult.Text = "List of Training(s) as per criteria :" + CType(mTrainingList.Count, String) + " Record(s) found."
        mGroupTrainingList = GroupTrainingList.GetGroupTrainingList(Name:=mGroupTrainingForCombo(0).GroupName.ToString, TrainingsRequired:=True, DesignationID:=mEmployee.DesignationID.ToString)
        Session("mGroupTrainingList") = mGroupTrainingList

      
        ChklistTrainingList.DataSource = mTrainingList
        ChklistTrainingList.DataBind()
        lblResult.DataBind()
        upnlTrainingList.Update()
        upnlGridViewTitle.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 27-July-2011
        If Not IsPostBack Then
            If cmbGroup.Enabled = True Then
                setFocus(cmbGroup)
            End If
            DataFieldBind()
        End If
    End Sub
    Private Function CheckForcheckedValues() As Boolean
        For i As Integer = 0 To ChklistTrainingList.Items.Count - 1

            If ChklistTrainingList.Items(i).Selected Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub btnDONE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click, btnDoneTop.Click



        If CheckForcheckedValues() = False Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select atleast one Training", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim mEmployeeTrainingList As EmployeeTrainingList
        Dim DuplicateTraining As String = String.Empty
        For i As Integer = 0 To ChklistTrainingList.Items.Count - 1

            If ChklistTrainingList.Items(i).Selected Then
                Dim mEmployeeTraining As EmployeeTraining
                mEmployeeTraining = EmployeeTraining.NewEmployeeTraining
                Dim mTraining As Training = Training.GetTraining(New Guid(ChklistTrainingList.Items(i).Value))
                mEmployeeTraining.EmployeeID = mEmployee.ID
                mEmployeeTraining.TrainingID = mTraining.ID

                mEmployeeTraining.IsNOTApplicable = False
                mEmployeeTraining.RecurringStatus = mTraining.RecurringStatus

                mEmployeeTraining.FreqInMonths = mTraining.FreqInMonths
                mEmployeeTraining.WarningDays = mTraining.WarningDays

                mEmployeeTrainingList = EmployeeTrainingList.GetEmployeeTrainingList(mEmployee.ID)

                If ((mEmployeeTrainingList.Contains(mEmployeeTraining.EmployeeID, mEmployeeTraining.TrainingID, mEmployeeTraining.ReferenceID)) And mEmployeeTraining.IsNew) Then
                    If DuplicateTraining = "" Then
                        DuplicateTraining = mTraining.Name
                    Else
                        DuplicateTraining = DuplicateTraining + ", " + mTraining.Name
                    End If

                Else
                    If mEmployeeTraining.IsValid Then
                        mEmployeeTraining.Save()
                    End If
                End If
            End If
        Next


        If DuplicateTraining <> "" Then
            MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the duplicate Training(s).", DuplicateTraining, MsgBoxStyle.OkOnly, "")

        End If


        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            Session.Remove("checkedIds")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click ', btnCloseTop.Click
        RemoveSession()
        Session.Remove("checkedIds")
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        Index = cmbGroup.SelectedIndex
        Session("Index") = Index
        'FindNow(cmbGroup.SelectedIndex)
    End Sub
    'Private Sub cmbGroup_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbGroup.SelectedIndexChanged
    '    dgTrainingList.DataSource = mTrainingList
    '    dgTrainingList.DataBind()
    '    lblResult.Text = "List of Training(s) as per criteria :" + CType(mTrainingList.Count, String) + " Record(s) found."
    '    mGroupTrainingList = GroupTrainingList.GetGroupTrainingList(GroupID:=cmbGroup.SelectedValue.ToString)
    '    Session("mGroupTrainingList") = mGroupTrainingList
    'End Sub

    'Private Sub dgTrainingList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgTrainingList.RowDataBound
    '    If (e.Row.RowType = DataControlRowType.DataRow) Then
    '        Dim TrainingID As Guid = (DataBinder.Eval(e.Row.DataItem, "ID"))
    '        If mGroupTrainingList.Contains(TrainingID, "", "") Then
    '            Dim chkSelect As CheckBox = CType(e.Row.FindControl("chkSelect"), CheckBox)
    '            chkSelect.Checked = True
    '            chkSelect.DataBind()
    '        End If
    '    End If
    'End Sub

   
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub cmbGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbGroup.SelectedIndexChanged
        mGroupTrainingList = GroupTrainingList.GetGroupTrainingList(Name:=cmbGroup.SelectedValue.ToString, TrainingsRequired:=True, DesignationID:=mEmployee.DesignationID.ToString)
        Session("mGroupTrainingList") = mGroupTrainingList
        mTrainingList = Session("mTrainingList")
        Session.Remove("checkedIds")

        For i As Integer = 0 To ChklistTrainingList.Items.Count - 1
            If mGroupTrainingList.Contains(New Guid(ChklistTrainingList.Items(i).Value), "", "") Then
                ChklistTrainingList.Items(i).Selected = True
            Else
                ChklistTrainingList.Items(i).Selected = False
            End If
        Next
   
        lblResult.Text = "List of Training(s) as per criteria :" + CType(mTrainingList.Count, String) + " Record(s) found."
        upnlTrainingList.Update()
    End Sub
    Private Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAll.CheckedChanged
        If chkAll.Checked Then
            For i As Integer = 0 To ChklistTrainingList.Items.Count - 1
                ChklistTrainingList.Items(i).Selected = True
            Next
        Else
            For i As Integer = 0 To ChklistTrainingList.Items.Count - 1
                ChklistTrainingList.Items(i).Selected = False
            Next
        End If
    End Sub
#End Region





  
   
End Class