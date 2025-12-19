'AJAX Conversion by vikrant on 20-May-2015

Imports System.Web.Services
Imports System.Text
Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq

Imports System
Imports System.IO

Public Class wfModelMonitorModList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mAssemblyMonitorModStatus As AssemblyMonitorModStatus
    Public mModelMonitorModList As ModelMonitorModList
    Public mModelMonitorMod As ModelMonitorMod
    ''Added By Saylee on 11th Aug 2007
    Public mDocumentTypeForID As Integer
    Public mAttachToID As Guid
    Public mName As String

    Public mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList
    'Added by vikrant on 27-July-2011
    Dim EventLogID As Guid
    Public mDirectiveDetail As String
    Public mATA As String
    Public mDirectiveType As String
    Public mDirectiveNo As String
    Public mRegNo As String
    Public mModelName As String
    Public mModelMonitorModTypeList As ModelMonitorModTypeList
    Dim mFileAttach As FileAttach

    Dim str As String = ""
    Dim DuplicateModNos As String = ""
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mAssemblyMonitorModStatus = CType(Session("mAssemblyMonitorModStatus"), AssemblyMonitorModStatus)
        mModelMonitorModList = CType(Session("mModelMonitorModList"), ModelMonitorModList)
        mModelMonitorMod = CType(Session("mModelMonitorMod"), ModelMonitorMod)
        mAssemblyMonitorModStatusList = CType(Session("mAssemblyMonitorModStatusList"), tmpAssemblyMonitorModStatusList)
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mModelMonitorModList")
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mModelMonitorModList.CurrentIndex = Index
        Session("mModelMonitorModList") = mModelMonitorModList
    End Sub
    Private Sub NewRecord()
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************


        Dim mModelMonitorMod As ModelMonitorMod
        Dim ID As Guid = Guid.NewGuid 'Revise Activity
        If Session("ModelIDFromModelCreation") = Nothing Then
            mModelMonitorMod = ModelMonitorMod.NewModelMonitorMod(ID, mAssemblyStatus.Assembly.ModelID, mHourType, ID)
        ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
            mModelMonitorMod = ModelMonitorMod.NewModelMonitorMod(ID, New Guid(Session("ModelIDFromModelCreation").ToString), 1, ID)
        End If
        Session("mModelMonitorMod") = mModelMonitorMod
        RemoveSession()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelModMasterWindow", "OpenModelModMasterWindow()", True)
        'Response.Redirect("wfModelMonitorMod_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfModelMonitorModList_Ajax.aspx")
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)

        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************

        Dim mModelMonitorMod As ModelMonitorMod
        If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019 
            mModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(mId, mHourType)
        Else
            mModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(mId, 1)
        End If

        Session("mModelMonitorMod") = mModelMonitorMod
        RemoveSession()

        'Changed by Saylee on 9-aug-2012
        'Added by Vikrant on 27-July-2011
        mDirectiveNo = " Directive No. : " & mModelMonitorMod.Number
        'mDirectiveDetail = "Model : " + mModelName + " Directive Type : " + mDirectiveType + " Directive No. : " + mDirectiveNo
        mDirectiveDetail = "Model : " & mModelMonitorModList(mId).ModelName & " Model Directive Type : " & mModelMonitorMod.ModelMonitorModTypeName & " Directive No: " & mModelMonitorMod.Number & " Description : " & mModelMonitorMod.Description
        MarkLog(Util.Action.Edit, "Model Directive", mDirectiveDetail, Util.ErrorType.NoError, mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelModMasterWindow", "OpenModelModMasterWindow()", True)
        'Response.Redirect("wfModelMonitorMod_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfModelMonitorModList_Ajax.aspx")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            If mModelMonitorModList(mModelMonitorModList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mModelMonitorModList(mModelMonitorModList.CurrentIndex).ID)
                            End If
                            ModelMonitorMod.DeleteModelMonitorMod(mModelMonitorModList.CurrentItem.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            'Added by Vikrant on 27-July-2011
                            mModelName = mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ModelName
                            mDirectiveNo = mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).Number
                            mATA = mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ATAChapter
                            mDirectiveType = mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ModelMonitorModTypeName

                            mDirectiveDetail = "Model : " + mModelName + " Directive Type : " + mDirectiveType + " Directive No. : " + mDirectiveNo
                            MarkLog(Util.Action.Delete, "Model Directive", mDirectiveDetail, Util.ErrorType.NoError, mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ID, EventLogID)
                            'End
                            DataFieldBind()
                            SetGrid()
                            ControlVisibility()
                            upnlGrid.Update()
                            upnlActionBtn.Update()
                            upnlActionBtnTop.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                'Added by Vikrant on 27-July-2011
                                mModelName = mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ModelName
                                mDirectiveNo = mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).Number
                                mATA = mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ATAChapter
                                mDirectiveType = mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ModelMonitorModTypeName

                                mDirectiveDetail = "Model : " + mModelName + " Directive Type : " + mDirectiveType + " Directive No. : " + mDirectiveNo
                                MarkLog(Util.Action.Delete, "Model Directive", "Can't Delete : " & mDirectiveDetail & " is already in use", Util.ErrorType.NoError, mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ID, EventLogID)
                                'End 
                                'Added by saylee on 1-Jun-2016
                                Dim mModelMonitorModConfiguredList As ModelMonitorConfiguredList
                                If Session("ModelIDFromModelCreation") = Nothing Then
                                    mModelMonitorModConfiguredList = ModelMonitorConfiguredList.GetModelMonitorModConfiguredList(mAssemblyStatus.Assembly.ModelID, mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ID.ToString)
                                ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                                    mModelMonitorModConfiguredList = ModelMonitorConfiguredList.GetModelMonitorModConfiguredList(New Guid(Session("ModelIDFromModelCreation").ToString), mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ID.ToString)
                                End If
                                If mModelMonitorModConfiguredList.Count > 0 Then
                                    Dim SerialNos As String = String.Empty

                                    For i As Integer = 0 To mModelMonitorModConfiguredList.Count - 1
                                        If i = mModelMonitorModConfiguredList.Count - 1 Then
                                            SerialNos = SerialNos + mModelMonitorModConfiguredList(i).SerialNo
                                        Else
                                            SerialNos = SerialNos + mModelMonitorModConfiguredList(i).SerialNo + ","
                                        End If
                                    Next

                                    MSGBoxCtrl.show("Deletion Alert!", "Selected Directive is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be deleted", "To delete master record please delete all configured status first", MsgBoxStyle.OkOnly, "")

                                End If
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
    Private Sub SetPage()
        If Session("ModelNameFromModelCreation") = Nothing Then
            lblTitle.Text = "Model Directives List of - [ " & "Model: " & mAssemblyStatus.Assembly.ModelName & "]"
        Else
            lblTitle.Text = "Model Directives List of - [ " & "Model: " & Session("ModelNameFromModelCreation").ToString & "]"
        End If
        '        lblResult.Text = "List Of Model Monitor Directives: " & mModelMonitorModList.Count & " Record(s)"
    End Sub
    Private Sub SetGrid()
        Dim P As Boolean
        Dim NA As String
        Dim chkBox As CheckBox


        For j As Integer = 0 To dgModelMonitorModList.Rows.Count - 1
            P = CType(Me.dgModelMonitorModList.Rows(j).Cells(19).Text, Boolean)
            NA = CType(Me.dgModelMonitorModList.Rows(j).Cells(20).Text, String)
            chkBox = CType(Me.dgModelMonitorModList.Rows(j).FindControl("chkSelectList"), CheckBox)
            If P = False Then
                dgModelMonitorModList.Rows(j).Cells(18).Enabled = False
            End If
            If NA = "No Frequency" And Session("ModelIDFromModelCreation") = Nothing Then 'Session("ModelIDFromModelCreation") = Nothing,'Added by Saylee on 14-Nov-2019
                If Not mAssemblyMonitorModStatusList.Contains(New Guid(dgModelMonitorModList.DataKeys(j).Values("ID").ToString)) Then
                    chkBox.Enabled = True
                Else
                    chkBox.Enabled = False
                    chkBox.Checked = True
                End If

            Else
                chkBox.Visible = False
            End If
        Next

        If Not Session("ModelIDFromModelCreation") = Nothing Then
            dgModelMonitorModList.Columns(0).Visible = False 'Added by Saylee on 14-Nov-2019
        End If
        If Not Session("OpenFromModelCreation") Is Nothing Then
            dgModelMonitorModList.Columns(14).Visible = False
        End If
    End Sub
    Private Sub DisplayControls(ByVal Index As Integer)
        txtFor.Text = IIf(Index = 3 Or Index = 4 Or Index = 5, txtFor.Text, "")
        txtCode.Text = IIf(Index = 2, txtCode.Text, "")
        txtCode.Visible = IIf(Index = 2, True, False)
        txtFor.Visible = IIf(Index = 3 Or Index = 4 Or Index = 5, True, False)
        lblFor.Visible = (Index > 1)
        cmbSearchFor.Visible = (Index = 1)
    End Sub
    Private Sub FindNow()
        dgModelMonitorModList.PageIndex = 0
        Select Case cmbLookIn.SelectedIndex
            Case 0, -1  'All
                If Session("ModelIDFromModelCreation") = Nothing Then
                    mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(mAssemblyStatus.Assembly.ModelID, IsRII:=chkIsRII.Checked)
                ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                    mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(New Guid(Session("ModelIDFromModelCreation").ToString), IsRII:=chkIsRII.Checked)
                End If
            Case 1  'Mod Type ID
                If Session("ModelIDFromModelCreation") = Nothing Then
                    mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(mAssemblyStatus.Assembly.ModelID, CInt(cmbSearchFor.SelectedValue), IsRII:=chkIsRII.Checked)
                ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                    mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(New Guid(Session("ModelIDFromModelCreation").ToString), CInt(cmbSearchFor.SelectedValue), IsRII:=chkIsRII.Checked)
                End If
            Case 2  'ATA Code
                If Session("ModelIDFromModelCreation") = Nothing Then
                    mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(mAssemblyStatus.Assembly.ModelID, , Val(txtCode.Text), IsRII:=chkIsRII.Checked)
                ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                    mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(New Guid(Session("ModelIDFromModelCreation").ToString), , Val(txtCode.Text), IsRII:=chkIsRII.Checked)
                End If
                'Case 3 ' ATA Nomenclature
                '    If Session("ModelIDFromModelCreation") = Nothing Then
                '        mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(mAssemblyStatus.Assembly.ModelID, , , txtFor.Text.Trim)
                '    ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                '        mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(New Guid(Session("ModelIDFromModelCreation").ToString), , , txtFor.Text.Trim)
                '    End If
            Case 3 'Description
                If Session("ModelIDFromModelCreation") = Nothing Then
                    mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(mAssemblyStatus.Assembly.ModelID, , , , txtFor.Text.Trim, IsRII:=chkIsRII.Checked)
                ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                    mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(New Guid(Session("ModelIDFromModelCreation").ToString), , , , txtFor.Text.Trim, IsRII:=chkIsRII.Checked)
                End If
            Case 4  'Reference
                If Session("ModelIDFromModelCreation") = Nothing Then
                    mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(mAssemblyStatus.Assembly.ModelID, , , , , txtFor.Text.Trim, IsRII:=chkIsRII.Checked)
                ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                    mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(New Guid(Session("ModelIDFromModelCreation").ToString), , , , , txtFor.Text.Trim, IsRII:=chkIsRII.Checked)
                End If
            Case 5  'Directive No.
                If Session("ModelIDFromModelCreation") = Nothing Then
                    mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(mAssemblyStatus.Assembly.ModelID, , , , , , txtFor.Text.Trim, IsRII:=chkIsRII.Checked)
                ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                    mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(New Guid(Session("ModelIDFromModelCreation").ToString), , , , , , txtFor.Text.Trim, IsRII:=chkIsRII.Checked)
                End If
        End Select

        dgModelMonitorModList.DataSource = mModelMonitorModList
        dgModelMonitorModList.DataBind()
        Session("mModelMonitorModList") = mModelMonitorModList
        lblResult.Text = "List Of Model Directives: " & mModelMonitorModList.Count & " Record(s)"

        SetGrid()
        ''=====Added By Saylee on 28-th-Jan-2008 for bug-Inspection List (IL2)
        'Session("LookIn") = cmbLookIn.SelectedIndex
        'Session("txtFor") = txtFor.Text
        'Session("txtCode") = txtCode.Text
        'SearchFor = IIf(cmbSearchFor.SelectedIndex <= 0, "", cmbSearchFor.SelectedValue)
        'Session("SearchFor") = SearchFor
        ''============================================================================
    End Sub
    Private Sub ControlVisibility()
        btnPrintTop.Visible = (mModelMonitorModList.Count > 15)
        btnAddNewTop.Visible = (mModelMonitorModList.Count > 15)
        btnBackTop.Visible = (mModelMonitorModList.Count > 15)

        btnPrint.Enabled = (mModelMonitorModList.Count > 0)
        btnPrintTop.Enabled = (mModelMonitorModList.Count > 0)

        'Added by Saylee on 14-Nov-2019
        If Session("ModelIDFromModelCreation") = Nothing Then
            btnConfigure.Visible = True
        Else
            btnConfigure.Visible = False
        End If
        '********************************************************
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Str = ""
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        Dim IsNotSelected As Boolean = True

        Dim chkSelectList As New CheckBox
        For i As Integer = 0 To Me.dgModelMonitorModList.Rows.Count - 1
            chkSelectList = CType(Me.dgModelMonitorModList.Rows(i).FindControl("chkSelectList"), CheckBox)
            If chkSelectList.Checked = True Then
                IsNotSelected = False
            End If
        Next

        Session("str") = str
        Str = Session("str")
        If IsNotSelected = True Then
            custValidator.ErrorMessage = "Please select atleast one item to configure"
            e.IsValid = False
        End If
        upnlValidationSummary.Update()
    End Sub
    Public Function ConfigureAD(ID As String) As Boolean

        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************

        mModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(New Guid(ID), mHourType)
        Session("mModelMonitorMod") = mModelMonitorMod
        If Not mAssemblyMonitorModStatusList.Contains(New Guid(ID)) Then

            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mModelMonitorMod.IssueDate, mAssemblyStatus.Assembly.ModelID, mHourType)
            '--------------------
            mAssemblyMonitorModStatus.ModelMonitorModID(True) = New Guid(ID)
            If mAssemblyMonitorModStatus.IsValid Then
                Try
                    mAssemblyMonitorModStatus.Save()
                Catch ex As Exception
                    Return False
                End Try

                Return True
            End If
        Else
            If DuplicateModNos = "" Then
                DuplicateModNos = mModelMonitorMod.Number
            Else
                DuplicateModNos = DuplicateModNos + ", " + mModelMonitorMod.Number
            End If

        End If
        Return False
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        If Session("ModelIDFromModelCreation") = Nothing Then
            mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(mAssemblyStatus.Assembly.ModelID, IsRII:=chkIsRII.Checked)
        ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
            mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(New Guid(Session("ModelIDFromModelCreation").ToString), IsRII:=chkIsRII.Checked)
        End If
        dgModelMonitorModList.DataSource = mModelMonitorModList
        Session("mModelMonitorModList") = mModelMonitorModList

        mModelMonitorModTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList("(ALL)")
        cmbSearchFor.DataSource = mModelMonitorModTypeList

        dgModelMonitorModList.DataBind()
        cmbSearchFor.DataBind()
        lblResult.Text = "List Of Model Directives: " & mModelMonitorModList.Count & " Record(s)"

        If mAssemblyMonitorModStatusList Is Nothing Then
            mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mMachine.AssemblyStatus.AsOnDate, mMachine.AssemblyStatus.AssemblyID, mMachine.ID, True)
            Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        'Added by vikrant on 27-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            DataFieldBind()
            DisplayControls(0)
            SetPage()
            SetGrid()
            ControlVisibility()
        End If
    End Sub
    Private Sub dgModelMonitorModList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgModelMonitorModList.RowCommand
        Dim Index As Int32
        Dim mID As Guid
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgModelMonitorModList.PageIndex * dgModelMonitorModList.PageSize
                mID = New Guid(dgModelMonitorModList.DataKeys(Index).Value.ToString)
                EditRecord(mID)
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgModelMonitorModList.PageIndex * dgModelMonitorModList.PageSize
                DeleteRecord(Index)
            Case "Select"
                Index = CInt(e.CommandArgument) + dgModelMonitorModList.PageIndex * dgModelMonitorModList.PageSize
                mID = New Guid(dgModelMonitorModList.DataKeys(Index).Value.ToString)
                If Session("NewPage") = "True" Then

                    'Rajnish 14 july 2008
                    mModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(mID, mHourType)
                    Session("mModelMonitorMod") = mModelMonitorMod

                    mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mModelMonitorMod.IssueDate, mAssemblyStatus.Assembly.ModelID, mHourType)
                    '--------------------
                    mAssemblyMonitorModStatus.ModelMonitorModID(True) = mID
                    Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                    RemoveSession()
                    'Code added by Saylee on 1/4/2008 Suggested by Deven sir
                    'Response.Redirect(Request.QueryString("GChildPage3") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))

                    Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
                    Session("mIssueDate") = mModelMonitorMod.IssueDateFormatted
                    Response.Redirect("wfAssemblyMonitorModStatusNew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                    '--------------------------------
                Else
                    mAssemblyMonitorModStatus.ModelMonitorModID(False) = mID
                    Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                    RemoveSession()
                    'Code added by Saylee on 1/4/2008 Suggested by Deven sir
                    'Response.Redirect(Request.QueryString("GChildPage3") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))

                    Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList

                    Response.Redirect("wfAssemblyMonitorModStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                    '--------------------------------
                End If

                '=====================Added By Saylee on 11th Aug 2007=================
            Case "View"
                Index = CInt(e.CommandArgument) + dgModelMonitorModList.PageIndex * dgModelMonitorModList.PageSize
                mID = New Guid(dgModelMonitorModList.DataKeys(Index).Value.ToString)

                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                mFileAttach = FileAttach.GetAttachment(mID)
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    End If
                Else
                    'Dim msg1 As New SIMsgBox(Page, "Attachment!", "No Attach File Present.", "", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfModelMonitorModList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                    'msg1.Show()
                End If
                ' Dim mID As New Guid(e.Item.Cells(0).Text)
                ''If (Not User.IsInRole("FileAttach")) Then
                ''    MarkLog(Util.Action.Attach, "Receipt", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ''    msg.ReplacePage = "wfReceiptList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeId
                ''    msg.Show()
                ''    Exit Sub
                ''End If
                'mDocumentTypeForID = 8   'In csDocumentTypeFor Table, Model Monitor Directive has ID=8  
                'mAttachToID = mID
                'mName = "Model:" & mModelMonitorModList.Item(mID).ModelName
                'Session("mDocumentTypeForID") = mDocumentTypeForID
                'Session("mAttachToID") = mAttachToID
                'Session("mName") = mName
                ''Dim str As String
                ''str = "<script language='javascript'>openledgersame('wfAttachFiles.aspx?MainBackPage=wfModelMonitorModList.aspx');</script>"
                '' ClientScript.RegisterStartupScript(Me.GetType(),"OpenScript", str)

                ''Changed By Saylee on 12th Dec 2007 to solve bug-MMM1 of Aircraft Master given By Pramod
                'If Session("NewPage") = "True" Then
                '    Response.Redirect("wfAttachFiles.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfModelMonitorModList.aspx")
                'Else
                '    Response.Redirect("wfAttachFiles.aspx?MainBackPage=wfModelMonitorModList.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
                'End If
                '=====================================================================
        End Select
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        'Added by Vikrant on 27-July-2011
        MarkLog(Util.Action.[New], "Model Directive", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        NewRecord()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        RemoveSession()

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            'Session.Remove("mManufacturer")
            'Session.Remove("mManufacturerList")
            Session("ActiveTabIndex") = 0
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        'Added by Vikrant on 27-July-2011
        MarkLog(Util.Action.Close, "Model Directive", "Model : " & mAssemblyStatus.Assembly.ModelName & " Serial No. : " & mAssemblyStatus.Assembly.SerialNo, Util.ErrorType.NoError, Guid.Empty, EventLogID)

        If Session("NewPage") = "True" Then
            Session("NewPage") = "False"
            Response.Redirect(Request.QueryString("BackPage"))
        Else
            Response.Redirect(Request.QueryString("GChildPage3") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
        End If
    End Sub
    Private Sub dgModelMonitorModList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgModelMonitorModList.Sorting
        mModelMonitorModList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mModelMonitorModList") = mModelMonitorModList
        dgModelMonitorModList.DataSource = mModelMonitorModList
        dgModelMonitorModList.DataBind()
        SetGrid()
    End Sub
    Private Sub cmbLookIn_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookIn.SelectedIndexChanged
        cmbSearchFor.SelectedIndex = 0
        DisplayControls(cmbLookIn.SelectedIndex)
        If cmbLookIn.Enabled = True Then
            cmbLookIn.Focus()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
        upnlGrid.Update()
        upnlActionBtn.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnModelModMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnModelModMaster.Click
        'DataFieldBind()
        FindNow()
        SetGrid()
        ControlVisibility()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
        upnlGrid.Update()
    End Sub
    Private Sub btnConfigure_Click(sender As Object, e As System.EventArgs) Handles btnConfigure.Click, btnConfigureTop.Click
        If IsValid Then

            'MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select atleast one item to configure", MsgBoxStyle.OkOnly, "")
            'Exit Sub

            Dim IsSuccessfully As Boolean = True
            Dim chkSelectList As New CheckBox
            For i As Integer = 0 To Me.dgModelMonitorModList.Rows.Count - 1
                chkSelectList = CType(Me.dgModelMonitorModList.Rows(i).FindControl("chkSelectList"), CheckBox)
                If chkSelectList.Checked = True And chkSelectList.Enabled = True Then

                    Dim ID As String = dgModelMonitorModList.DataKeys(i).Values("ID").ToString
                    'txtCompRemark = CType(Me.dgDueMonitoringCompList.Rows(i).FindControl("txtCompRemark"), TextBox)

                    Try
                        If ConfigureAD(ID) = True Then
                            IsSuccessfully = True
                        End If

                    Catch ex As Exception

                    Finally

                    End Try

                End If
            Next

            If DuplicateModNos.Length > 0 Then
                MSGBoxCtrl.show("Alert!!", DuplicateModNos & " Directives are already Configured on this Assembly.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            If IsSuccessfully = True Then
                If Session("NewPage") = "True" Then
                    Session("NewPage") = "False"
                    Response.Redirect(Request.QueryString("BackPage"))
                Else
                    Response.Redirect(Request.QueryString("GChildPage3") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                End If
            End If

        End If
    End Sub

#End Region


#Region " Report "
    '    'Created By:- Jyoti
#Region "Report Variable"
    Dim mCompanyDetail As New CompanyDetail
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region "Event"
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        Rpt = New crListModelMonitor
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        'Model Monitor Modification List
        ReportDetails.Add(New rptStatus(, 0, , _
        , , , dgModelMonitorModList.Columns.Item(2).HeaderText, , dgModelMonitorModList.Columns.Item(3).HeaderText, _
        dgModelMonitorModList.Columns.Item(4).HeaderText, dgModelMonitorModList.Columns.Item(5).HeaderText, _
       dgModelMonitorModList.Columns.Item(6).HeaderText, dgModelMonitorModList.Columns.Item(7).HeaderText, _
      dgModelMonitorModList.Columns.Item(8).HeaderText, dgModelMonitorModList.Columns.Item(9).HeaderText))

        Dim TotalCount As Integer
        TotalCount = Me.mModelMonitorModList.Count
        Dim I As Integer

        Dim str(8) As String

        For I = 0 To TotalCount - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""
            If Me.dgModelMonitorModList.Rows(I).Cells(2).Text <> "&nbsp;" Then str(0) = Me.dgModelMonitorModList.Rows(I).Cells(2).Text
            If Me.dgModelMonitorModList.Rows(I).Cells(3).Text <> "&nbsp;" Then str(1) = Me.dgModelMonitorModList.Rows(I).Cells(3).Text
            If Me.dgModelMonitorModList.Rows(I).Cells(4).Text <> "&nbsp;" Then str(2) = Me.dgModelMonitorModList.Rows(I).Cells(4).Text
            If Me.dgModelMonitorModList.Rows(I).Cells(5).Text <> "&nbsp;" Then str(3) = Me.dgModelMonitorModList.Rows(I).Cells(5).Text
            If Me.dgModelMonitorModList.Rows(I).Cells(6).Text <> "&nbsp;" Then str(4) = Me.dgModelMonitorModList.Rows(I).Cells(6).Text
            If Me.dgModelMonitorModList.Rows(I).Cells(7).Text <> "&nbsp;" Then str(5) = Me.dgModelMonitorModList.Rows(I).Cells(7).Text
            If CType(Me.mModelMonitorModList.Item(I).IsRII, String) <> "&nbsp;" Then str(7) = IIf(CType(Me.mModelMonitorModList.Item(I).IsRII, String) = "True", "Yes", "")
            If Me.dgModelMonitorModList.Rows(I).Cells(8).Text <> "&nbsp;" Then str(6) = Me.dgModelMonitorModList.Rows(I).Cells(8).Text
            ReportDetails.Add(New rptStatus(, 1, , _
                     , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), str(7)))
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "Model Directives List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)

        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region

 
  
End Class