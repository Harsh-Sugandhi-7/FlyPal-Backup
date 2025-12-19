

'Created By : Saylee
'Dated		 : 30-May-2022

Public Class _wfCustomerContract_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCustomerContract As CustomerContract
    Public mCustomerContractList As CustomerContractList
    Public mCustomerList As VendorList
    Public mCurrencyList As CurrencyList
    Dim EventLogID As Guid
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False

    Dim mMachineNameValueList As MachineNameValueList
    Public mModelList As ModelList
    Dim Flag As Int16
#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mCustomerContract = Session("mCustomerContract")
        mCustomerContractList = Session("mCustomerContractList")
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mCurrencyList = Session("mCurrencyList")
        mMachineNameValueList = Session("mMachineNameValueList")
        mModelList = Session("mModelList")
    End Sub
    Private Sub SetSession()
        Session("mCustomerContract") = mCustomerContract
        Session("mCustomerContractList") = mCustomerContractList
        Session("mCurrencyList") = mCurrencyList
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mModelList") = mModelList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCustomerContract")
        Session.Remove("mCustomerContractList")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("mCurrencyList")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mModelList")
    End Sub
    Private Sub Controlvisibility()
        'dgContractItems.Columns(6).Visible = (mCustomerContract.StatusID = 1)

        If Not User.IsInRole("CustomerContractAuthorized") Then
            btnAuthorized.Enabled = False
            btnAuthorized.ToolTip = "You are not authorized user "
           
            btnCancel.Enabled = False
            btnCancel.ToolTip = "You are not authorized user "
        End If

        btnSelectFile.Disabled = Not (mCustomerContract.StatusID = 1)
    End Sub
    Private Sub DataFieldBind()
        mCustomerList = VendorList.GetVendortList(0, "", "", "", "", "", True, True, False)
        cmbCustomer.DataSource = mCustomerList
        Session("mCustomerList") = mCustomerList

        mCurrencyList = CurrencyList.GetCurrencyList(, , True)
        cmbCurrencyList.DataSource = mCurrencyList
        Session("mCurrencyList") = mCurrencyList

       

        mMachineNameValueList = MachineNameValueList.GetMachineList(mCustomerContract.ContractDate.ToString, IsTagRequired:=True, TagText:="(SELECT)", ForInventory:=True)
        cmbAircraftList.DataSource = mMachineNameValueList
       
        mModelList = ModelList.GetModelList(0, "", , , "(SELECT)")
        cmbModel.DataSource = mModelList
       

        dgContractItems.DataSource = mCustomerContract.CustomerContractTasks
        dgContractTerms.DataSource = mCustomerContract.CustomerContractTerms

        DataBind()
        txtContractDate.Text = mCustomerContract.ContractDateFormatted
        txtFromDate.Text = mCustomerContract.FromDateFormatted.ToString
        txtToDate.Text = mCustomerContract.ToDateFormatted.ToString
        cmbCurrencyList.SelectedValue = mCustomerContract.CurrencyID.ToString
        txtConversionFactor.Text = mCustomerContract.ConversionFactor.ToString
        cmbModel.SelectedValue = mCustomerContract.ModelID.ToString
        cmbAircraftList.SelectedValue = mCustomerContract.MachineID.ToString
        cmbCustomer.SelectedValue = mCustomerContract.CustomerID.ToString
    End Sub
    Public Sub Save()
        SetObject()
        If Not mCustomerContract.IsValid Then
            If Not customvalidate1() Then
                upnlValidationsummary.Update()
                Exit Sub
            End If
        End If

        If mCustomerContract.CustomerContractTasks.Count = 0 Then
            MSGBoxCtrl.show("Alert..!!!", "Contract cannot be saved without Tasks", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Try
            'If (mCustomerContractList.Contains(mCustomerContract.DocumentID, mCustomerContract.ID, mIsRenew.ToString)) Then
            '    MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the duplicate entry.", "You can not add duplicate entry. ", MsgBoxStyle.OkOnly, "")
            'Else
            mCustomerContract.Save()
            SetSession()
            Controlvisibility()
            ControlVisibilityForAttachment()
            lblTitle.Text = "Contract Information"
            DataFieldBind()
            MarkLog(Flypal.Util.Action.Save, "CompanyDocument", "Contract No. : " + mCustomerContract.ContractNumber, Flypal.Util.ErrorType.NoError, mCustomerContract.ID, EventLogID)
            Session.Remove("mCustomerContractList")
            If mCustomerContract.StatusID = 2 Then
                MSGBoxCtrl.show(MSGBox.Message_title.AuthorizedSuccessFully, MSGBox.Message_text.AuthorizedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            ElseIf mCustomerContract.StatusID = 4 Then
                MSGBoxCtrl.show(MSGBox.Message_title.CanceledSuccessFully, MSGBox.Message_text.CanceledSuccessFully, "", MsgBoxStyle.OkOnly, "")
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            End If


            upnlContractDetail.Update()
            upnlContractTasks.Update()
            upnlContractTerms.Update()
            upnlActionBtn.Update()
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Or ex.Number = 2601 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            End If
        End Try
    End Sub

    Private Sub SetTitle()
        If mCustomerContract.IsNew Then
            lblTitle.Text = "Contract Information [New]"
        Else

            lblTitle.Text = "Contract Information"

        End If
        upnlTitle.Update()
    End Sub
    Public Function customvalidate1() As Boolean
        If Flag = 1 Then Exit Function
        'Dim custValidator As CustomValidator
        'custValidator = CType(s, CustomValidator)
        SetObject()

        Dim str As String = ""
        Dim txtValue As TextBox

        If Not mCustomerContract.IsValid Then
            For i As Integer = 0 To mCustomerContract.GetBrokenRulesCollection.Count - 1
                str = str + mCustomerContract.GetBrokenRulesCollection(i).Description + "<Br>"
            Next

            For Each mCustomerContractTask As CustomerContractTask In mCustomerContract.CustomerContractTasks
                For i As Integer = 0 To mCustomerContractTask.GetBrokenRulesCollection.Count - 1
                    str = str + mCustomerContractTask.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next

            For Each mCustomerContractTerm As CustomerContractTerm In mCustomerContract.CustomerContractTerms
                For i As Integer = 0 To mCustomerContractTerm.GetBrokenRulesCollection.Count - 1
                    str = str + mCustomerContractTerm.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If


        If str <> "" Then
            cvConDate.ErrorMessage = str
            cvConDate.IsValid = False
            Return False
        End If
        Flag = 1

        Return True
    End Function
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)
        If CustValid.ControlToValidate = "cmbCustomer" Then
            If cmbCustomer.SelectedIndex = 0 Then
                CustValid.ErrorMessage = "Customer required."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValid.ControlToValidate = "cmbModel" Then
            If cmbModel.SelectedIndex = 0 Then
                CustValid.ErrorMessage = "Model required."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValid.ControlToValidate = "cmbCurrencyList" Then
            If cmbCurrencyList.SelectedIndex = 0 Then
                CustValid.ErrorMessage = "Currency required."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValid.ControlToValidate = "txtFromDate" Then
            If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                If CDate(txtFromDate.Text.ToString) > CDate(txtToDate.Text.ToString) Then
                    CustValid.ErrorMessage = "To Date should be greater than or Equal to From Date."
                    e.IsValid = False
                End If
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub SetObject()
        If txtContractDate.Text <> "" Then
            mCustomerContract.ContractDate = txtContractDate.Text
        Else
            mCustomerContract.ContractDate = System.DBNull.Value
        End If



        mCustomerContract.Text = txtText.Text.ToString.Trim
        mCustomerContract.No = Val(txtNo.Text)
        mCustomerContract.CustomerID = New Guid(cmbCustomer.SelectedValue)

        mCustomerContract.CurrencyID = New Guid(cmbCurrencyList.SelectedValue)
        mCustomerContract.ConversionFactor = Val(txtConversionFactor.Text)

        mCustomerContract.MachineID = New Guid(cmbAircraftList.SelectedValue)
        mCustomerContract.ModelID = New Guid(cmbModel.SelectedValue)

        If txtFromDate.Text <> "" Then
            mCustomerContract.FromDate = txtFromDate.Text
        Else
            mCustomerContract.FromDate = System.DBNull.Value
        End If

        If txtToDate.Text <> "" Then
            mCustomerContract.ToDate = txtToDate.Text
        Else
            mCustomerContract.ToDate = System.DBNull.Value
        End If

        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mCustomerContract.IsAttachmentAdded = True
            Else
                mCustomerContract.IsAttachmentAdded = False
            End If
        End If
        mCustomerContract.UserName = User.Identity.Name
        Session("mCustomerContract") = mCustomerContract
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mCustomerContract.IsAttachmentAdded Then
            ImageButton1.Visible = True
            If mCustomerContract.StatusID = 1 Then btnDelAttach.Enabled = True Else btnDelAttach.Enabled = False
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub SaveAttachment() '
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mCustomerContract.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mCustomerContract.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString

        If mCustomerContract.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCustomerContract.ID)
        End If
        If mFileAttach.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
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
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "RemoveTask" Then
                        Try
                            Session("sender") = ""
                            mCustomerContract = Session("mCustomerContract")
                            mCustomerContract.CustomerContractTasks.Remove(mCustomerContract.CustomerContractTasks.CurrentIndex)
                            Session("mCustomerContract") = mCustomerContract
                            dgContractItems.DataSource = mCustomerContract.CustomerContractTasks
                            dgContractItems.DataBind()
                            upnlContractTasks.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "CustomerContract", "Can't delete : " + mCustomerContract.ContractNumber + "  is Currently in use", Flypal.Util.ErrorType.NoError, mCustomerContract.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            upnlContractTasks.Update()
                            upnlContractTasks.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then

                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "Status" Or MSGBoxCtrl.Sender = "Close" Then
                        Session("sender") = ""
                        Save()
                        If MSGBoxCtrl.Sender = "Close" Then
                            Session.Remove("mFileAttach")
                            Response.Redirect("index.aspx")
                        End If
                    ElseIf MSGBoxCtrl.Sender = "RemoveTerms" Then
                        Try
                            Session("sender") = ""
                            mCustomerContract = Session("mCustomerContract")
                            mCustomerContract.CustomerContractTerms.Remove(mCustomerContract.CustomerContractTerms.CurrentItem.ID)
                            Session("mCustomerContract") = mCustomerContract
                            dgContractTerms.DataSource = mCustomerContract.CustomerContractTerms
                            dgContractTerms.DataBind()
                            upnlContractTerms.Update()
                        Catch ex As SqlException
                            upnlContractTerms.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then

                            End If
                        End Try

                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        Session.Remove("IsValid")
                        If mCustomerContract.StatusID = 2 And MSGBoxCtrl.Sender <> "Close" Then
                            mCustomerContract.StatusID = 1
                        ElseIf mCustomerContract.StatusID = 4 Then
                            mCustomerContract.StatusID = 2
                        ElseIf mCustomerContract.StatusID = 1 Then
                            mCustomerContract.StatusID = 2
                        End If
                        Session("mCustomerContract") = mCustomerContract
                        DataFieldBind()
                        Controlvisibility()
                    ElseIf MSGBoxCtrl.Sender = "Close" Then
                        Response.Redirect("Index.aspx")
                    End If
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""

        End If
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveTask")
        mCustomerContract.CustomerContractTasks.CurrentIndex = Index
        Session("mCustomerContract") = mCustomerContract
    End Sub


#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        SetTitle()

        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            Controlvisibility()
            ControlVisibilityForAttachment()
            '' If mCustomerContract.IsNew Then cmbCurrencyList.SelectedValue = mCurrencyList(3).ID.ToString
            ''If mCustomerContract.IsNew Then txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not customvalidate1() Then upnlValidationsummary.Update() : Exit Sub

        If Page.IsValid Then
            Save()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub

    Private Sub btnAuthorized_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
        If Not customvalidate1() Then upnlValidationsummary.Update() : Exit Sub
        If Page.IsValid Then
            'Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
            mCustomerContract.StatusID = 2

            mCustomerContract.AuthorizedBy = User.Identity.Name
            Session("mCustomerContract") = mCustomerContract
            MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<strong>Contract</strong>", MsgBoxStyle.YesNo, "Status")
        Else
            upnlValidationsummary.Update()
        End If

    End Sub
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If IsValid Then
            'Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
            mCustomerContract.StatusID = 4


            Session("mCustomerContract") = mCustomerContract
            MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<strong>Contract</strong>", MsgBoxStyle.YesNo, "Status")
        Else
            upnlValidationsummary.Update()
        End If
    End Sub

    Private Sub cmbCurrencyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCurrencyList.SelectedIndexChanged
        txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
        If cmbCurrencyList.Enabled = True Then
            SetFocus(cmbCurrencyList)
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        Session.Remove("mCustomerContractList")

        If mCustomerContract.IsDirty Then
            Session("IsValid") = "True"
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            Session.Remove("mFileAttach")
            Response.Redirect("index.aspx")
        End If
        '' Response.Redirect(Request.QueryString("BackPage") & "?ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked

        MessageBoxResult()

    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mCustomerContract.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        If mCustomerContract.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCustomerContract.ID)
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mCustomerContract.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        Session("mFileAttach") = mFileAttach
        Session("mCustomerContract") = mCustomerContract
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mCustomerContract.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mCustomerContract.ID)
        Else
            If IsAttachmentDeleted Then
                If (Not mCustomerContract.IsNew) Then
                    mFileAttach = FileAttach.GetAttachment(mCustomerContract.ID)
                    If Not mFileAttach Is Nothing Then
                        Dim fileSize1 As Integer = 0
                        Dim file1(fileSize1) As Byte

                        mFileAttach.ImageFile = file1
                        mFileAttach.Size = 0
                        GoTo CodeBlock
                    End If
                End If
            End If
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mCustomerContract.ID)
        End If
CodeBlock:
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
    End Sub


    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Page.IsValid Then

            If Not customvalidate1() Then upnlValidationsummary.Update() : Exit Sub

            SetObject()
            mCustomerContract.CustomerContractTasks.Add(mCustomerContract.ID)
            Session("mCustomerContract") = mCustomerContract
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCustomerContractTasksWindow", "OpenCustomerContractTasksWindow();", True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub

    Private Sub dgContractItems_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgContractItems.PageIndexChanging
        dgContractItems.PageIndex = e.NewPageIndex
        dgContractItems.DataSource = mCustomerContract.CustomerContractTasks
        Session("mCWPList") = mCustomerContract.CustomerContractTasks
        dgContractItems.DataBind()

    End Sub
    Private Sub dgContractItems_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgContractItems.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgContractItems.PageSize * dgContractItems.PageIndex
                mCustomerContract.CustomerContractTasks.CurrentIndex = Index - 1
                Session("mCustomerContract") = mCustomerContract

                Session("Edit") = True
                Dim mCustomerContractClone As CustomerContract
                mCustomerContractClone = mCustomerContract.Clone
                Session("mCustomerContractClone") = mCustomerContractClone

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCustomerContractTasksWindow", "OpenCustomerContractTasksWindow();", True)
            Case "Remove"
                Dim Index As Integer = CInt(e.CommandArgument) + dgContractItems.PageSize * dgContractItems.PageIndex
                DeleteRecord(Index - 1)
        End Select
    End Sub
    Private Sub hdnBtnCustomerContractTasks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnCustomerContractTasks.Click
        dgContractItems.DataSource = mCustomerContract.CustomerContractTasks
        dgContractItems.DataBind()
        upnlContractTasks.Update()
    End Sub
    Private Sub btnAddTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddTerm.Click
        If IsValid Then
            SetObject()
            Session("mCustomerContract") = mCustomerContract
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub hdnimgBtnCustomerTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnCustomerTerm.Click
        DataFieldBind()
        upnlContractTerms.Update()
    End Sub
    Private Sub dgContractTerms_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgContractTerms.RowCommand
        Select Case e.CommandName
            Case "Remove"

                Dim Index As Int32 = CInt(e.CommandArgument)
                MSGBoxCtrl.show(MSGBox.Message_title.RemoveTerm, MSGBox.Message_text.RemoveTerm, "", MsgBoxStyle.YesNo, "RemoveTerms")
                mCustomerContract.CustomerContractTerms.CurrentIndex = Index
                Session("mCustomerContract") = mCustomerContract

                'Dim Index As Int32 = CInt(e.CommandArgument)
                'mCustomerContract.CustomerContractTerms.CurrentIndex = Index
                'mCustomerContract.CustomerContractTerms.Remove(mCustomerContract.CustomerContractTerms.CurrentItem.ID)
                'Session("mCustomerContract") = mCustomerContract
                'dgContractTerms.DataSource = mCustomerContract.CustomerContractTerms
                'dgContractTerms.DataBind()
                'upnlContractTerms.Update()
        End Select
    End Sub
#End Region




End Class