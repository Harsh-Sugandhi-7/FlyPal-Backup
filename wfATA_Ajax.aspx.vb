'Added By Vikrant

Partial Class wfATA_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mATA As ATA
    Public mATAList As ATAList
    Dim EventLogID As Guid 'Added By Utkarsh On 19-Jul-2011 For All19072011
    Dim totcnt As Integer 'shweta 
    Private mSubATAS As SubATAs
    Dim SubCode As Integer?
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mATA = CType(Session("mATA"), ATA)
        mATAList = CType(Session("mATAList"), ATAList)
    End Sub
    Private Sub SetSession()
        Session("mATA") = mATA
        Session("mATAList") = mATAList
    End Sub
    'AJAX
    Private Sub RemoveSession()
        Session.Remove("mATA")
        Session.Remove("mATAList")
    End Sub
    Private Sub NewRecord()
        mATA = ATA.NewATA(Guid.NewGuid)
        Session("mATA") = mATA
        txtATANomenclature.Enabled = True
        txtATACode.Enabled = True
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mATA = ATA.GetATA(mId)
        Session("mATA") = mATA
        'New Addition By Yogita on 10-Dec-2007 to solve Bug No:-ATA4 given by Pramod
        setFocus(txtATACode)
    End Sub
    Private Sub EditRecordSubATA(ByVal mId As Guid)
        txtSubATACode.Text = mATA.SubATAs.Item(mId).DispSubATACode
        txtSubATAChapter.Text = mATA.SubATAs.Item(mId).SubATANomenclature
        txtDescription.Text = mATA.SubATAs.Item(mId).SubATADescription
        'Added By Vikrant On 17-Dec-2018 For ALL17122018
        If mATA.SubATAs.Item(mId).SubCode.HasValue Then
            txtSubCode.Text = Format(mATA.SubATAs.Item(mId).SubCode, "0#")
        End If
        'End

    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mATA = ATA.GetATA(mId)
        Session("mATA") = mATA
    End Sub
    Private Sub setObject()
        mATA.ATACode = Val(Trim(txtATACode.Text))
        mATA.ATANomenclature = Trim(txtATANomenclature.Text)
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0

        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mATA = CType(Session("mATA"), ATA)
                            ATA.DeleteATA(mATA.ID)
                            
                            'upnlValidationSummary.Update()

                            'Response.Redirect("wfATA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&BackPage3=" & Request.QueryString("BackPage3"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Util.Action.Delete, "ATA", "Can't delete : " & mATAList(mATA.ID).ATAChapter & " is Currently in use", Util.ErrorType.NoError, mATA.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            'DataFieldBind()
                            msgCount = ex.Errors.Count

                        Finally
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            upnlATADetails.Update()
                            upnlGridView.Update()
                            upnlSubATALink.Update()
                            If msgCount = 0 Then
                                'Changed By Utkarsh On 19-Jul-2011 For All19072011
                                MarkLog(Util.Action.Delete, "ATA", mATAList(mATA.ID).ATAChapter, Util.ErrorType.NoError, mATA.ID, EventLogID)
                                'End
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "DeleteSubATA" Then 'AJAX
                        Try
                            Session("sender") = ""
                            mATA.SubATAs.Remove(mATA.SubATAs(mATA.SubATAs.CurrentIndex))
                            mATA.Save()
                            Session("mATA") = mATA
                            Session("SubATAEdit") = False
                            DataFieldBindForSubATA()
                            SetTitleForSubATA()
                            ClearControls()
                            lnkSubATACount_ModalPopupExtender.Show()
                            upnlSubATA.Update()
                            'Response.Redirect("wfSubATA.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage3=" & Request.QueryString("BackPage3"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "RefDeleteSubATA")
                            End If
                            'Added To Retain mATA Session Value after SubATA Reference Delete
                            txtATAChapter.Text = mATA.ATAChapter
                            Dim mTempATA As ATA = ATA.GetATA(mATA.ID)
                            dgSubATAList.DataSource = mTempATA.SubATAs
                            upnlSubATA.DataBind()
                            Session("mATA") = mTempATA
                            'End
                            msgCount = ex.Errors.Count
                            lnkSubATACount_ModalPopupExtender.Show()
                            upnlSubATA.Update()
                        Finally
                            If msgCount = 0 Then
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "DeleteSubATA" Or MSGBoxCtrl.Sender = "RefDeleteSubATA" Then 'AJAX
                        lnkSubATACount_ModalPopupExtender.Show()
                        upnlSubATA.Update()
                    End If
                    If MSGBoxCtrl.Sender = "Delete" Then
                        NewRecord()
                        SetTitle()
                        lnkSubATACount.Visible = False
                        lblSubATA.Visible = False
                        upnlATADetails.DataBind()
                        upnlSubATALink.Update()
                        upnlATADetails.Update()
                    End If
                    Session("sender") = ""
                    'Response.Redirect("wfATA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&BackPage3=" & Request.QueryString("BackPage3"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    If MSGBoxCtrl.Sender = "DeleteSubATA" Or MSGBoxCtrl.Sender = "RefDeleteSubATA" Then 'AJAX
                        DataFieldBindForSubATA()
                        lnkSubATACount_ModalPopupExtender.Show()
                        upnlSubATA.Update()
                    Else
                        NewRecord()
                        DataFieldBind()
                    End If
                    Session("sender") = ""
                    'Response.Redirect("wfATA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&BackPage3=" & Request.QueryString("BackPage3"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    NewRecord()
                    DataFieldBind()
                    'Response.Redirect("wfATA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&BackPage3=" & Request.QueryString("BackPage3"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            'Response.Redirect("wfATA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&BackPage3=" & Request.QueryString("BackPage3"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If

    End Sub
    Private Sub addAttributes()
        txtATACode.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtATACode').value,event)")
    End Sub
    Private Sub SetTitle()
        If mATA.IsNew Then
            lbltitle.Text = "ATA [ New ]"
        Else
            If Len(mATA.ATANomenclature) > 15 Then
                lbltitle.Text = "ATA [ " & mATA.ATANomenclature.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "ATA [ " & mATA.ATANomenclature & " ]"
            End If
        End If
        upnlTitle.Update() 'AJAX
    End Sub
    'AJAX
    Private Sub ShowSubATAs()
        lnkSubATACount_ModalPopupExtender.Show()
        txtSubATACode.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtSubATACode').value,event)")
        txtSubCode.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtSubCode').value,event)") 'Added By Vikrant On 17-Dec-2018 For ALL17122018
        DataFieldBindForSubATA()
        SetSubATATitle()
        upnlSubATA.Update()
    End Sub

    Private Sub DisableATAName(ByVal mId As Guid) 'Added by : Saylee 19-Jun-2020, ALL16062020
        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerATA(mId)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtATANomenclature.Enabled = mTransCountAsPerMasters.Count = 0
            txtATACode.Enabled = mTransCountAsPerMasters.Count = 0
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList("", "")
        Session("mATAList") = mATAList
        Session("totcnt") = totcnt
        dgATAList.DataSource = mATAList
        lblResult.Text = "ATA List : " & mATAList.Count & " Record(s) Found."
        totcnt = mATA.SubATAs.Count 'Shweta
        Session("totcnt") = totcnt 'shweta

        DataBind()
        'AJAX
        upnlGridView.Update()
        'ENd
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        If CustValidator.ControlToValidate = "txtATACode" Then
            If IsNumeric(txtATACode.Text) Then
                If Val(txtATACode.Text) < 0 Then
                    CustValidator.ErrorMessage = "Code should be Numeric and should not be negative.."
                    e.IsValid = False
                End If
            Else
                If (txtATACode.Text) = "" Then
                    CustValidator.ErrorMessage = "Code Required."
                    e.IsValid = False
                Else

                    CustValidator.ErrorMessage = "Code should be Numeric."
                    e.IsValid = False
                End If
               
            End If
        End If
        If CustValidator.ControlToValidate = "txtATANomenclature" Then
            If txtATANomenclature.Text = "" Then 'AJAX
                CustValidator.ErrorMessage = "Chapter Required."
                e.IsValid = False
            ElseIf txtATANomenclature.Text.Trim.Length > 50 Then
                CustValidator.ErrorMessage = "Chapter Should not be greater than 50 characters."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 19-Jul-2011 For All19072011

        addAttributes()
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtATACode.Enabled = True Then
                setFocus(txtATACode)
            End If
            If IsNothing(Request.QueryString("BackPage3")) Or Request.QueryString("BackPage3") = "" Then
                'Added by utkarsh on 6-nov-2013 for ata popup
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                Else
                    Session("MiddleFrame") = "wfATA_Ajax.aspx?"
                End If
                'End

            End If
            NewRecord()
            DataFieldBind()
            SetTitle()

        End If
        'MessageBoxResult()
        'SetTitle() AJAX
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("ATANew") And mATA.IsNew) Or (Not User.IsInRole("ATAEdit") And Not mATA.IsNew) Then
            setObject()
            SetSession()
            'Changed By Utkarsh On 19-Jul-2011 For All19072011
            MarkLog(Util.Action.Save, "ATA", User.Identity.Name & " is not Authorized User to Save " & mATAList(mATA.ID).ATAChapter, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'End
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Try
                setObject()
                mATA.Save()
                If txtATACode.Enabled = True Then
                    setFocus(txtATACode)
                End If
                DataFieldBind()
                'Changed By Utkarsh On 19-Jul-2011 For All19072011
                MarkLog(Util.Action.Save, "ATA", mATAList(mATA.ID).ATAChapter, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                'End
                NewRecord()

                SetSession()
                SetTitle()

            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
            txtATACode.Text = ""                'Added By Prashant 11-Jan-2013 'ALL10012013
            txtATANomenclature.Text = ""        'Added By Prashant 11-Jan-2013 'ALL10012013

            lnkSubATACount.Visible = False
            lblSubATA.Visible = False
            'AJAX
            upnlATADetails.Update()
            upnlGridView.Update()
            upnlSubATALink.Update()
            'End
        Else 'AJAX
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub dgATAList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgATAList.RowCommand
        Dim Idx As Int32
        Dim mId As Guid
        Select Case e.CommandName
            Case "EditRec"
                Idx = CInt(e.CommandArgument) + dgATAList.PageIndex * dgATAList.PageSize
                mId = mATAList(Idx).ID

                If (Not User.IsInRole("ATAView") And Not User.IsInRole("ATAEdit")) Then
                    setObject()
                    SetSession()
                    'Changed By Utkarsh On 19-Jul-2011 For All19072011
                    MarkLog(Util.Action.Edit, "ATA", User.Identity.Name & " is not Authorized User to Edit " & mATAList(mId).ATAChapter, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If

                EditRecord(mId)
                upnlATADetails.DataBind()

                If mATA.SubATAs.Count > 0 Then
                    lnkSubATACount.Visible = True 'Shweta
                    lblSubATA.Visible = True
                    lnkSubATACount.Text = "( " + mATA.SubATAs.Count.ToString + " ) Records"
                Else 'AJAX
                    lnkSubATACount.Visible = False
                    lblSubATA.Visible = False
                End If
                'AJAX
                upnlATADetails.Update()
                upnlSubATALink.Update()
                'End

                'Changed By Utkarsh On 19-Jul-2011 For All19072011
                MarkLog(Util.Action.Edit, "ATA", mATAList(mATA.ID).ATAChapter, Util.ErrorType.NoError, mATA.ID, EventLogID)
                'End
                SetTitle()
                DisableATAName(mId) 'Added by : Saylee 19-Jun-2020, ALL16062020
            Case "DeleteRec"
                Idx = CInt(e.CommandArgument) + dgATAList.PageIndex * dgATAList.PageSize
                mId = mATAList(Idx).ID

                If (Not User.IsInRole("ATADelete")) Then
                    setObject()
                    SetSession()
                    'Changed By Utkarsh On 19-Jul-2011 For All19072011
                    MarkLog(Util.Action.Delete, "ATA", User.Identity.Name & " is not Authorized User to Delete " & mATAList(mId).ATAChapter, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If

                DeleteRecord(mId)
            Case "SubATA"
                Idx = CInt(e.CommandArgument) + dgATAList.PageIndex * dgATAList.PageSize
                mId = mATAList(Idx).ID
                mATA = ATA.GetATA(mId)
                Session("mATA") = mATA
                Session("SubATAEditFromGrid") = True

                DataFieldBind()

                ShowSubATAs()
                upnlSubATA.Update()
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        mATAList = ATAList.GetATAList(txtSearch.Text)
        dgATAList.DataSource = mATAList
        dgATAList.DataBind()
        Session("mATAList") = mATAList
        lblResult.Text = "ATA List : " & mATAList.Count & " Record(s) Found."
        'AJAX
        upnlGridView.Update()
        'ENd
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'New Addition By Yogita on 10-Dec-2007 to solve Bug No:-ATA5 given by Pramod

        'MarkLog(Util.Action.[New], "ATA", "", Util.ErrorType.NoError, mATA.ID)
        'NewRecord()
        'DataFieldBind()
        NewRecord()
        txtATACode.Text = "" 'Added By Rajnish on 28-12-2007
        txtATANomenclature.Text = ""  'Added By Rajnish on 28-12-2007

        'AJAX
        lblSubATA.Visible = False
        lnkSubATACount.Visible = False
        upnlATADetails.Update()
        upnlValidationSummary.Update()
        upnlSubATALink.Update()
        'End

        'Changed By Utkarsh On 19-Jul-2011 For All19072011
        MarkLog(Util.Action.[New], "ATA", "", Util.ErrorType.NoError, mATA.ID, EventLogID)
        'End
        'DataFieldBind()
        If txtATACode.Enabled = True Then
            setFocus(txtATACode)
        End If
        SetTitle()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Changed By Utkarsh On 19-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, "ATA", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        RemoveSession()
        'Added by utkarsh on 6-nov-2013 for ata popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        Session("sender") = ""
        If IsNothing(Request.QueryString("BackPage3")) Or Request.QueryString("BackPage3") = "" Then
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        Else
            Response.Redirect(Request.QueryString("BackPage3") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6"))
        End If
    End Sub

    Private Sub lnkSubATACount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkSubATACount.Click
        Session("mATA") = mATA
        ShowSubATAs()
    End Sub

    Private Sub dgATAList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgATAList.PageIndexChanging
        dgATAList.PageIndex = e.NewPageIndex
        dgATAList.DataSource = mATAList
        Session("mATAList") = mATAList
        dgATAList.DataBind()
    End Sub
    Private Sub dgATAList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgATAList.Sorting
        mATAList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mATAList") = mATAList
        dgATAList.DataSource = mATAList
        dgATAList.DataBind()
    End Sub
    'AJAX
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

#Region "Sub ATA"
    Private Sub DataFieldBindForSubATA()
        txtATAChapter.Text = mATA.ATAChapter
        Dim mTempATA As ATA = ATA.GetATA(mATA.ID)
        dgSubATAList.DataSource = mTempATA.SubATAs
        dgSubATAList.PageIndex = 0
        Session("mATA") = mTempATA
        upnlSubATA.DataBind()
    End Sub
    Private Sub SetSubATATitle()
        lblResultSubATA.Text = "Sub ATA List : " & mATA.SubATAs.Count & " Record(s) Found."
    End Sub
    Private Sub btnSaveSubATA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSubATA.Click
        If (Not User.IsInRole("ATANew") And mATA.IsNew) Or (Not User.IsInRole("ATAEdit") And Not mATA.IsNew) Then
            SetSessionForSubATA()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Try
            If CustomValidate1() Then
                If Session("SubATAEdit") = False Then
                    If Trim(txtSubCode.Text) <> "" Then
                        SubCode = CInt(Trim(txtSubCode.Text))
                    End If
                    mATA.SubATAs.Add(Guid.NewGuid, mATA.ID, mATA.ATACode, mATA.ATANomenclature, CInt(txtSubATACode.Text), Trim(txtSubATAChapter.Text), Trim(txtDescription.Text), SubCode)
                    Session("mATA") = mATA
                    setFocus(txtSubATACode)
                Else
                    setObjectForSubATA()
                    If Not IsValid Then
                        Exit Sub
                    End If
                    Session("mATA") = mATA
                    setFocus(txtSubATACode)
                    Session("SubATAEdit") = False
                End If
                Try
                    mATA.Save()
                    DataFieldBindForSubATA()
                    SetTitleForSubATA()
                    ClearControls()
                    lnkSubATACount_ModalPopupExtender.Show()
                    'Response.Redirect("wfSubATA.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage3=" & Request.QueryString("BackPage3"))
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        mATA.SubATAs.Remove(mATA.SubATAs.CurrentItem)
                        DataFieldBindForSubATA()
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "RefDeleteSubATA")
                    ElseIf ex.Number = 2601 Then
                        mATA.SubATAs.Remove(mATA.SubATAs.CurrentItem)
                        DataFieldBindForSubATA()
                        MSGBoxCtrl.show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Duplicate, "Sub ATA Chapter Should Not Be Same For Same ATA.", MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    lnkSubATACount_ModalPopupExtender.Show()
                End Try
            Else
                lnkSubATACount_ModalPopupExtender.Show()
                upnlSubATA.Update()
            End If
        Catch ex As Exception
            Throw ex
        Finally

        End Try
    End Sub
    Private Sub SetSessionForSubATA()
        Session("mATA") = mATA
    End Sub
    Public Function CustomValidate1() As Boolean
        Dim strMSG As String = ""

        If IsNumeric(txtSubATACode.Text) Then
            If Val(txtSubATACode.Text) < 0 Then
                strMSG = "Sub ATA Code should be Numeric." + "<BR>"
            ElseIf txtSubATACode.Text = "" Then 'AJAX
                strMSG += "Sub ATA Code Required." + "<BR>"
            End If
        Else
            If txtSubATACode.Text = "" Then 'AJAX
                strMSG += "Sub ATA Code Required." + "<BR>"
            Else
                strMSG = "Sub ATA Code should be Numeric." + "<BR>"
            End If

        End If

        If txtSubATAChapter.Text = "" Then 'AJAX
            strMSG += "Sub ATA Chapter Required." + "<BR>"
        ElseIf txtSubATAChapter.Text.Trim.Length > 50 Then
            strMSG += "Sub ATA Chapter Should Not Greater than 50 Characters." + "<BR>"
        End If

        If Len(txtDescription.Text.Trim) > 1000 Then
            strMSG += "Sub ATA Description Should Not Greater than 1000 Characters." + "<BR>"
        End If
        upnlSubATA.Update()
        If strMSG.Trim <> "" Then
            cvDescription.ErrorMessage = strMSG
            cvDescription.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub setObjectForSubATA()
        mATA.SubATAs.Item(mATA.SubATAs.CurrentIndex).SubATACode = Trim(txtSubATACode.Text)
        mATA.SubATAs.Item(mATA.SubATAs.CurrentIndex).SubATANomenclature = Trim(txtSubATAChapter.Text)
        mATA.SubATAs.Item(mATA.SubATAs.CurrentIndex).SubATADescription = Trim(txtDescription.Text)
        'Added By Vikrant On 17-Dec-2018 For ALL17122018
        If Trim(txtSubCode.Text) <> "" Then
            mATA.SubATAs.Item(mATA.SubATAs.CurrentIndex).SubCode = Trim(txtSubCode.Text)
        End If
        'End
    End Sub
    Private Sub SetTitleForSubATA()
        lblResultSubATA.Text = "Sub ATA List : " & mATA.SubATAs.Count & " Record(s) Found."
    End Sub

    Private Sub dgSubATAList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSubATAList.RowCommand
        Dim Idx As Int32
        Select Case e.CommandName
            Case "DeleteRec"
                lnkSubATACount_ModalPopupExtender.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteSubATA")
                Idx = CInt(e.CommandArgument) + dgSubATAList.PageIndex * dgSubATAList.PageSize
                mATA.SubATAs.CurrentIndex = Idx
                Session("mATA") = mATA
            Case "EditRec"
                Idx = CInt(e.CommandArgument) + dgSubATAList.PageIndex * dgSubATAList.PageSize
                mATA.SubATAs.CurrentIndex = Idx
                If Len(mATA.SubATAs.Item(Idx).SubATANomenclature) > 15 Then
                    lblTitleSubATA.Text = "Sub ATA [ " & mATA.SubATAs.Item(Idx).SubATANomenclature.Substring(0, 15) & "...]"
                Else
                    lblTitleSubATA.Text = "Sub ATA [ " & mATA.SubATAs.Item(Idx).SubATANomenclature & "]"
                End If
                Dim mID As Guid = mATA.SubATAs(Idx).ID
                EditRecordSubATA(mID)
                setFocus(txtSubATACode)
                dgSubATAList.DataSource = mATA.SubATAs
                upnlSubATA.DataBind()
                Session("SubATAEdit") = True
                Session("mATA") = mATA
                lnkSubATACount_ModalPopupExtender.Show()
        End Select
    End Sub
    Private Sub dgSubATAList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgSubATAList.PageIndexChanging
        dgSubATAList.PageIndex = e.NewPageIndex
        dgSubATAList.DataSource = mATA.SubATAs
        Session("mATA") = mATA
        dgSubATAList.DataBind()
    End Sub
    Private Sub btnCloseSubATA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseSubATA.Click
        lnkSubATACount_ModalPopupExtender.Hide()
        ClearControls()
        DataFieldBind()
        If mATA.SubATAs.Count > 0 Then
            lnkSubATACount.Visible = True
            lblSubATA.Visible = True
            lnkSubATACount.Text = "( " + mATA.SubATAs.Count.ToString + " ) Records"
        Else
            lnkSubATACount.Visible = False
            lblSubATA.Visible = False
        End If
        'AJAX
        upnlSubATALink.Update()

        If Session("SubATAEditFromGrid") = True Then
            lbltitle.Text = "ATA [ New ]"
            lnkSubATACount.Text = ""
            lnkSubATACount.Visible = False
            lblSubATA.Visible = False

            upnlTitle.Update() 'AJAX
            upnlSubATA.Update()
            Session("SubATAEditFromGrid") = False
        End If
    End Sub
    Private Sub ClearControls()
        txtSubATACode.Text = ""
        txtSubATAChapter.Text = ""
        txtDescription.Text = ""
        txtSubCode.Text = "" 'Added By Vikrant On 17-Dec-2018 For ALL17122018
        lblTitleSubATA.Text = "Sub ATA [ New ]"
    End Sub
#End Region
End Class
