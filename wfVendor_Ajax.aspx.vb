Imports System.Text.RegularExpressions

'Added by Vikrant

Partial Class wfVendor_Ajax
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnContactInfo As System.Web.UI.WebControls.Button
    Protected WithEvents btnBankInfo As System.Web.UI.WebControls.Button
    Protected WithEvents btnTaxInfo As System.Web.UI.WebControls.Button
    'Protected WithEvents txtNotInUseDate As SIControls.SICalendar
    Protected WithEvents cvNotInUse As System.Web.UI.WebControls.CustomValidator

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Public mVendor As Vendor
    Public mCityList As CityInvList
    Public mCity As CityInv
    Public mState As State
    Public BackPage As String
    Public Type As Int16 = 0
    Public flag As Boolean = False
    Dim EventLogID As Guid 'Added by Saylee on 19-July-2011
    Public mVendorTypeList As VendorTypeList
    Public mVendorApprovals As VendorApprovals
    Public mVendorApproval As VendorApproval
    Public mRenewVendorApproval As VendorApproval
    Dim mFileAttach As FileAttach
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mVendor = Session("mVendor")
        mCityList = Session("mCityList")
        Type = Val(Request.QueryString("Type"))
        mVendorTypeList = Session("mVendorTypeList")
    End Sub
    Private Sub SetSession()
        Session("mVendor") = mVendor
        Session("mCitylist") = mCityList
        Session("Type") = Type
        Session("mVendorTypeList") = mVendorTypeList
    End Sub
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
    Private Sub SetObject()
        mVendor.Name = Trim(txtName.Text)
        mVendor.IsSupplier = chkSupplier.Checked
        mVendor.IsCustomer = chkCustomer.Checked
        mVendor.IsServiceProvider = chkIsServiceProvider.Checked  'Added by Prashant 17/07/07
        mVendor.Address = Trim(txtAddress.Text)
        'Added Code
        'Try
        mVendor.CityID = New Guid(cmbCity.SelectedValue)
        ' Catch ex As Exception
        ' mVendor.CityID = Guid.Empty
        ' End Try
        'End of Added Code
        mVendor.Zip = txtZipCode.Text
        mVendor.Phone1 = txtPhone1.Text
        mVendor.Phone2 = Trim(txtPhone2.Text)
        mVendor.Phone3 = Trim(txtPhone3.Text)
        mVendor.Fax = Trim(txtFax.Text)
        mVendor.Email = Trim(txtEmail.Text.Trim)
        mVendor.ContactPerson = txtContactPerson.Text

        If txtNotInUseDate.Text = "" Then
            mVendor.NotInUseDate = System.DBNull.Value
        Else
            mVendor.NotInUseDate = txtNotInUseDate.Text.ToString
        End If
        mVendor.NotInUse = chkNotInUse.Checked
        mVendor.VendorTypeID = cmbVendorTypeList.SelectedValue
        mVendor.IsApprovalRequired = chkIsApprovalRequired.Checked
        mVendor.Code = Trim(txtVendorCode.Text)
        mVendor.NatureOfVendor = Trim(txtNatureOfVendor.Text)
        mVendor.RepairStationCertificate = txtRepairStationCertificate.Text.Trim
        mVendor.VendorID = txtVendorID.Text.Trim
        mVendor.GSTIN = Trim(txtFirstTwoDigits.Text.Trim) + Trim(txt10Characters.Text.Trim) + Trim(txtThirteen.Text.Trim) + Trim(txtFourteen.Text.Trim) + Trim(txtFifteen.Text.Trim)
        Session("mVendor") = mVendor
    End Sub
    Private Sub SetCity()

        If mVendor.IsNew Then
            txtState.Text = mCityList.Item(cmbCity.SelectedIndex).State.ToString
            txtCountry.Text = mCityList.Item(cmbCity.SelectedIndex).Country.ToString
        Else
            cmbCity.SelectedValue = mVendor.CityID.ToString
            If mVendor.CityID.Equals(Guid.Empty) Then
                'do nothing
                txtState.Text = ""
                txtCountry.Text = ""
            Else

                mCity = CityInv.GetCity(mVendor.CityID)
                mState = State.GetState(mCity.StateID)
                txtState.Text = mState.Name
                txtCountry.Text = mState.CountryName
            End If

        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteVendorApproval" Then
                        Try
                            mVendorApproval = Session("mVendorApproval")
                            If mVendorApproval.IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mVendorApproval.ID)
                            End If
                            VendorApproval.DeleteVendorApproval(mVendorApproval.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            VendorApprovalsGridBind()
                            SetVendorApprovalsGrid()
                        Catch ex As SqlException
                        Finally
                            MarkLog(Util.Action.Delete, "Vendor", "Approval No. : " & mVendorApproval.ApprovalNo & " Name : " & mVendorApproval.Name, Util.ErrorType.NoError, mVendorApproval.ID, EventLogID)
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    If Not MSGBoxCtrl.Sender = "Delete" Then
                        DataFieldBind()
                    End If
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Sub ControlVisibility() 'Added by Saylee for SYMCO on 28-July-2010
        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer") Then
            ''txtName.Enabled = False
            ''chkSupplier.Enabled = False
            ''chkCustomer.Enabled = False
            ''chkIsServiceProvider.Enabled = False
            ''txtAddress.Enabled = False
            ''cmbCity.Enabled = False
            ''txtZipCode.Enabled = False
            ''txtPhone1.Enabled = False
            ''txtPhone2.Enabled = False
            ''txtPhone3.Enabled = False
            ''txtFax.Enabled = False
            ''txtEmail.Enabled = False
            ''txtContactPerson.Enabled = False

            ''btnSave.Visible = False
        End If
        txtNotInUseDate.Enabled = chkNotInUse.Checked
        chkIsApprovalRequired.Enabled = (Not mVendor.IsNew)
        btnAddNewApproval.Enabled = (chkIsApprovalRequired.Checked And Not mVendor.IsNew)
        DisableName(mVendor.ID) 'Added by : Shital 19-Jun-2020, ALL16062020
        If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "RED" Then
            txtVendorCode.Enabled = False
        End If
    End Sub
    Private Sub DisableName(ByVal mId As Guid) 'Added by : Shital 19-Jun-2020, ALL16062020
        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerVendor(mId)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtName.Enabled = mTransCountAsPerMasters.Count = 0
        End If
    End Sub
    Private Sub addAttributes()
        txtFirstTwoDigits.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtFirstTwoDigits').value,event)")
        'txtFourteen.Attributes.Add("onKeyPress", "validateText(('Alphabets'),document.getElementById('txtFourteen').value)")
        'txtFifteen.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtFifteen').value)")
    End Sub
#End Region

#Region " DataBinding "
    Private Sub DataFieldBind()
        mCityList = CityInvList.GetCityList(0, "", "", True)
        cmbCity.DataSource = mCityList
        Session("mCityList") = mCityList

        mVendorTypeList = VendorTypeList.GetVendorTypeList()
        cmbVendorTypeList.DataSource = mVendorTypeList

        'Added Code
        'If Not mCityList.Contains(mVendor.CityName) Then
        '    mVendor.CityID = Guid.Empty
        'End If
        If Not mCityList.Contains(mVendor.CityID) Then
            mVendor.CityID = Guid.Empty
        End If
        'End of Added Code
        If mVendor.NotInUseDate.ToString = "" Then
            txtNotInUseDate.Text = ""
        Else
            txtNotInUseDate.Text = Format(CDate(mVendor.NotInUseDate), AppSettings("DateFormat"))
        End If

        DataBind()
        SetCity()
    End Sub
    Public Sub Customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)
        If CustValid.ControlToValidate = "txtName" Then
            If Trim(txtName.Text) = "" Then
                CustValid.ErrorMessage = "Name Required "
                e.IsValid = False
            ElseIf Len(Trim(txtName.Text)) > 100 Then
                CustValid.ErrorMessage = "Name is too long "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If


        If CustValid.ControlToValidate = "txtAddress" Then
            If Trim(txtAddress.Text) = "" Then
                CustValid.ErrorMessage = "Address Required "
                e.IsValid = False
            ElseIf Len(txtAddress.Text) > 500 Then
                CustValid.ErrorMessage = " Address is too long "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        If CustValid.ControlToValidate = "cmbCity" Then
            If cmbCity.SelectedIndex = 0 Then
                CustValid.ErrorMessage = "Please select the City "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        If CustValid.ControlToValidate = "txtName" Then
            If flag = False Then
                If chkSupplier.Checked = False And chkIsServiceProvider.Checked = False And chkCustomer.Checked = False Then
                    CustValid.ErrorMessage = "Select at least one Category."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
                flag = True
            End If
        ElseIf CustValid.ControlToValidate = "txtNotInUseDate" Then
            If chkNotInUse.Checked = True And txtNotInUseDate.Text.ToString = "" Then
                CustValid.ErrorMessage = "Not In Use Date should not be Blank."
                e.IsValid = False
            ElseIf chkIsApprovalRequired.Checked = True And dgApprovalList.Rows.Count = 0 Then
                CustValid.ErrorMessage = "At least one approval entry required."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        If CustValid.ControlToValidate = "txtFirstTwoDigits" Then
            'If txtCountry.Text.ToUpper = "INDIA" Then
            If Len(txtFirstTwoDigits.Text) > 0 Then
                If Len(txtFirstTwoDigits.Text) = 2 And Regex.IsMatch(txtFirstTwoDigits.Text, "^[0-9]{2}$") = True Then
                    e.IsValid = True
                Else
                    CustValid.ErrorMessage = "Enter Valid GSTIN First Should be 2 Digist Numbers. E.g 22"
                    e.IsValid = False
                End If
            End If
            'End If
        End If
        If CustValid.ControlToValidate = "txt10Characters" Then
            'If Len(txt10Characters.Text) <> 10 And Not Regex.IsMatch(txt10Characters.Text, "^([a-zA-Z0-9]+$") Then
            'If txtCountry.Text.ToUpper = "INDIA" Then
            If Len(txt10Characters.Text) > 0 Then
                If Len(txt10Characters.Text) = 10 And Regex.Match(txt10Characters.Text, "^[a-zA-Z0-9]+$", RegexOptions.IgnoreCase).Success = True Then
                    e.IsValid = True
                Else
                    CustValid.ErrorMessage = "Enter Valid GSTIN, Enter Alphabets And Numbers Only. E.g. AAAAA0000A "
                    e.IsValid = False
                End If
            End If
            'End If
        End If
        If CustValid.ControlToValidate = "txtThirteen" Then
            'If txtCountry.Text.ToUpper = "INDIA" Then
            If Len(txtThirteen.Text) > 0 Then
                If Regex.Match(txtThirteen.Text, "^[a-zA-Z0-9]+$", RegexOptions.IgnoreCase).Success = False Then
                    CustValid.ErrorMessage = "Enter Valid GSTIN, Enter Single Alphabet OR Number Only. E.g. 1 Or B"
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            End If
            'End If
        End If
        If CustValid.ControlToValidate = "txtFourteen" Then
            'If txtCountry.Text.ToUpper = "INDIA" Then
            If Len(txtFourteen.Text) > 0 Then
                If Regex.Match(txtFourteen.Text, "^[a-zA-Z]+$", RegexOptions.IgnoreCase).Success = False Then
                    CustValid.ErrorMessage = "Enter Valid GSTIN, Enter Single Alphabet Only. E.g. B"
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
                'End If
            End If
        End If
        If CustValid.ControlToValidate = "txtFifteen" Then
            'If txtCountry.Text.ToUpper = "INDIA" Then
            'If Regex.Match(txtFifteen.Text, "^[0-9]{1}$", RegexOptions.IgnoreCase).Success = False Then
            If Len(txtFifteen.Text) > 0 Then
                If Regex.Match(txtFifteen.Text, "^[a-zA-Z0-9]+$", RegexOptions.IgnoreCase).Success = False Then
                    CustValid.ErrorMessage = "Enter Valid GSTIN, Enter Single Alphabet OR Number Only. E.g. 5/X "
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
                'End If
            End If
        End If
    End Sub
#End Region

#Region "Vendor Approvals"
    Private Sub VendorApprovalsGridBind()
        mVendorApprovals = VendorApprovals.GetVendorApprovalList(mVendor.ID)
        dgApprovalList.DataSource = mVendorApprovals
        DataBind()
        upnlApprovalRequired.Update()
    End Sub
    Private Sub SetVendorApprovalsGrid()
        Dim P As Boolean
        Dim C As Integer
        Dim D As Boolean
        'For j As Integer = 0 To dgApprovalList.Rows.Count - 1
        '    P = CType(Me.dgApprovalList.Rows.Item(j).Cells(10).Text, Boolean)
        '    C = CType(Me.dgApprovalList.Rows.Item(j).Cells(11).Text, Integer)
        '    D = CType(Me.dgApprovalList.Rows.Item(j).Cells(12).Text, Boolean)
        '    If P = False Then
        '        dgApprovalList.Rows.Item(j).Cells(7).Enabled = False
        '    End If
        '    If C = 1 Then
        '        dgApprovalList.Rows.Item(j).Cells(9).Enabled = False
        '    End If
        '    If D = True Then
        '        dgApprovalList.Rows.Item(j).Cells(8).Enabled = False
        '    End If
        'Next
    End Sub
    Private Sub SetVendorApprovalsHistoryGrid()
        Dim P As Boolean
        For j As Integer = 0 To dgApprovalHistoryList.Rows.Count - 1
            P = CType(Me.dgApprovalHistoryList.Rows.Item(j).Cells(6).Text, Boolean)
            If P = False Then
                dgApprovalHistoryList.Rows.Item(j).Cells(5).Enabled = False
            End If
        Next
    End Sub
    Private Sub DeleteRecord(ByVal ID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteVendorApproval")
        mVendorApproval = VendorApproval.GetVendorApproval(ID)
        Session("mVendorApproval") = mVendorApproval
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 19-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            If txtName.Enabled = True Then
                setFocus(txtName)
            End If
            BackPage = Request.QueryString("BackPage")
            Session("BackPage") = BackPage
            DataFieldBind()
            VendorApprovalsGridBind()
            SetVendorApprovalsGrid()
        End If
        If mVendor.IsNew Then
            lblVendorInfo.Text = " Vendor Information[New] "
        Else
            If Len(mVendor.Name) > 15 Then
                lblVendorInfo.Text = "Vendor Information[ " & mVendor.Name.Substring(0, 15) & "...]"
            Else
                lblVendorInfo.Text = "Vendor Information[ " & mVendor.Name & "]"
            End If
        End If
        upnlTitle.Update()
        ControlVisibility()
        If AppSettings("ClientCode") = "7AR" Then 'You can enable or disable the validator based on some condition in your code-behind:
            rfvCode.Enabled = False 'RequiredFieldValidator
            rfvID.Enabled = True    'RequiredFieldValidator
        Else
            rfvCode.Enabled = True   'RequiredFieldValidator
            rfvID.Enabled = False    'RequiredFieldValidator
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("VendorNew") And mVendor.IsNew) Or (Not User.IsInRole("VendorEdit") And Not mVendor.IsNew) Then
            SetObject()
            SetSession()
            MarkLog(Util.Action.Save, "Vendor", User.Identity.Name & " is not Authorized User to save " & mVendor.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID) 'Added by Saylee on 19-July-2011
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If IsValid Then
            Try
                SetObject()
                SetSession()
                If mVendor.IsValid Then
                    mVendor.Save()
                    chkIsApprovalRequired.Enabled = True
                    upnlApprovalButtons.Update()
                Else
                    Dim strMsg As String = ""
                    If Not mVendor.IsValid Then
                        For j As Integer = 0 To mVendor.GetBrokenRulesCollection.Count - 1
                            strMsg = strMsg + mVendor.GetBrokenRulesCollection(j).Description + "<BR>"
                        Next
                    End If

                    If strMsg.Trim <> "" Then
                        cvDate.ErrorMessage = strMsg
                        cvDate.IsValid = mVendor.IsValid
                    End If
                End If

                If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "RED" Then
                    mVendor = Vendor.GetVendor(mVendor.ID)
                    txtVendorCode.DataBind()
                    upnlVendorDetails.Update()
                End If

                MarkLog(Util.Action.Save, "Vendor", mVendor.Name, Util.ErrorType.NoError, mVendor.ID, EventLogID) 'Added by Saylee on 19-July-2011
                lblVendorInfo.Text = "Vendor Information[ " & mVendor.Name & "]"
                lblVendorInfo.DataBind()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 2627 Then
                    If AppSettings("ClientCode") = "7AR" Then
                        MSGBoxCtrl.Show("Alert!", "You are trying to add duplicate record. Only unique record is allowed.", "ID should be unique.", MsgBoxStyle.OkOnly, "Delete")
                    Else
                        MSGBoxCtrl.Show("Alert!", "You are trying to add duplicate record. Only unique record is allowed.", "Code should be unique.", MsgBoxStyle.OkOnly, "Delete")
                    End If
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 50000 Then
                    MSGBoxCtrl.Show("Cancel Vendor!", "<BR><BR>" + ex.Message, "", MsgBoxStyle.OkOnly, "")
                End If
            End Try
        Else 'AJAX
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, "Vendor", "", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Saylee on 19-July-2011
        Session("sender") = ""
        Response.Redirect(Request.QueryString("BackPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Request.QueryString("Type"))
    End Sub
    Private Sub imgCity_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgCity.Click
        SetObject()    'Added Code
        SetSession()
        '   Response.Redirect("wfCityInv_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&Type=" & Request.QueryString("Type") & "&BackPage3=wfVendor_Ajax.aspx")
    End Sub
    Private Sub cmbCity_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCity.SelectedIndexChanged
        Dim indx As Integer
        indx = cmbCity.SelectedIndex
        txtState.Text = mCityList.Item(indx).State.ToString
        txtCountry.Text = mCityList.Item(indx).Country.ToString
        If cmbCity.Enabled = True Then
            setFocus(cmbCity)
        End If
        'upnlContactDetails.Update()
    End Sub
    Private Sub chkNotInUse_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotInUse.CheckedChanged
        txtNotInUseDate.Enabled = chkNotInUse.Checked
        If chkNotInUse.Checked = False Then
            txtNotInUseDate.Text = ""
        End If
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Protected Sub btnAddNewApproval_Click(sender As Object, e As EventArgs) Handles btnAddNewApproval.Click
        SetObject()
        SetSession()
        mVendorApproval = VendorApproval.NewVendorApproval(Guid.NewGuid, mVendor.ID)
        Session("mVendorApproval") = mVendorApproval
        Session("VendorName") = txtName.Text.Trim
        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mVendorApproval.ID)
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenVendorApprovalWindow", "OpenVendorApprovalWindow()", True)
    End Sub
    Private Sub chkIsApprovalRequired_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkIsApprovalRequired.CheckedChanged
        If chkIsApprovalRequired.Checked = True Then
            btnAddNewApproval.Enabled = True
        Else
            btnAddNewApproval.Enabled = False
        End If
    End Sub
    Private Sub dgApprovalList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgApprovalList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                'Dim index As Integer = CInt(e.CommandArgument) + dgApprovalList.PageIndex * dgApprovalList.PageSize
                'Dim ID As Guid = New Guid(Me.dgApprovalList.Rows.Item(index).Cells(0).Text)
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                mVendorApproval = VendorApproval.GetVendorApproval(ID)
                Session("mVendorApproval") = mVendorApproval
                If mVendorApproval.IsAttachmentAdded = True Then
                    mFileAttach = FileAttach.GetAttachment(mVendorApproval.ID)
                Else
                    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mVendorApproval.ID)
                End If
                Session("mFileAttach") = mFileAttach
                Session("VendorName") = txtName.Text.Trim
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenVendorApprovalWindow", "OpenVendorApprovalWindow()", True)
            Case "DeleteRec"
                'Dim index As Integer = CInt(e.CommandArgument) + dgApprovalList.PageIndex * dgApprovalList.PageSize
                'Dim ID As Guid = New Guid(Me.dgApprovalList.Rows.Item(index).Cells(0).Text)
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("VendorDelete")) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    Exit Sub
                End If
                DeleteRecord(ID)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                'Dim index As Integer = CInt(e.CommandArgument) + dgApprovalList.PageIndex * dgApprovalList.PageSize
                'Dim ID As Guid = New Guid(Me.dgApprovalList.Rows.Item(index).Cells(0).Text)
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                mFileAttach = FileAttach.GetAttachment(ID)
                Session("mFileAttach") = mFileAttach
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
                        Dim Str As String
                        Str = "openFile();"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
                    End If
                End If
            Case "RenewRec"
                'Dim index As Integer = CInt(e.CommandArgument) + dgApprovalList.PageIndex * dgApprovalList.PageSize
                'Dim ID As Guid = New Guid(Me.dgApprovalList.Rows.Item(index).Cells(0).Text)
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                mVendorApproval = VendorApproval.GetVendorApproval(ID)
                mRenewVendorApproval = VendorApproval.NewVendorApproval(Guid.NewGuid, mVendor.ID, IsRenew:=True, ApprovalNo:=mVendorApproval.ApprovalNo, Name:=mVendorApproval.Name, FromDate:=mVendorApproval.FromDate.ToString, ToDate:=mVendorApproval.ToDate.ToString, IsOneTime:=mVendorApproval.IsOneTime, IsApplicable:=mVendorApproval.IsApplicable, SortNo:=mVendorApproval.SortNo + 1, ReferenceID:=mVendorApproval.ReferenceID.ToString, Remark:=mVendorApproval.Remark)
                mVendorApproval.IsRenew = True
                Session("mVendorApproval") = mRenewVendorApproval
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mRenewVendorApproval.ID)
                Session("mFileAttach") = mFileAttach
                Session("VendorName") = txtName.Text.Trim
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenVendorApprovalWindow", "OpenVendorApprovalWindow()", True)
            Case "HistoryRec"
                'Dim index As Integer = CInt(e.CommandArgument) + dgApprovalList.PageIndex * dgApprovalList.PageSize
                'Dim ID As Guid = New Guid(Me.dgApprovalList.Rows.Item(index).Cells(0).Text)
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                mVendorApprovals = VendorApprovals.GetVendorApprovalList(mVendor.ID, True, ID.ToString)
                dgApprovalHistoryList.DataSource = mVendorApprovals
                dgApprovalHistoryList.DataBind()
                SetVendorApprovalsHistoryGrid()
                upnlApprovalHistory.Update()
                mdeApprovalHistory.Show()
        End Select
    End Sub
    Private Sub dgApprovalHistoryList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgApprovalHistoryList.RowCommand
        Select Case e.CommandName
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                'Dim index As Integer = CInt(e.CommandArgument) + dgApprovalHistoryList.PageIndex * dgApprovalHistoryList.PageSize
                'Dim ID As Guid = New Guid(Me.dgApprovalHistoryList.Rows.Item(index).Cells(0).Text)
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                mFileAttach = FileAttach.GetAttachment(ID)
                Session("mFileAttach") = mFileAttach
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
                        Dim Str As String
                        Str = "openFile();"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
                    End If
                End If
        End Select
    End Sub
    Private Sub hdnBtnVendorApproval_Click(sender As Object, e As System.EventArgs) Handles hdnBtnVendorApproval.Click
        VendorApprovalsGridBind()
        SetVendorApprovalsGrid()
    End Sub
    Private Sub btnApprovalHistoryClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprovalHistoryClose.Click
        mdeApprovalHistory.Hide()
    End Sub
    Private Sub hdnimgBtnCity_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnCity.Click
        mCityList = CityInvList.GetCityList(0, "", "", True)
        cmbCity.DataSource = mCityList
        Session("mCityList") = mCityList
        cmbCity.DataBind()
        SetCity()
        upnlVendorDetails.Update()
    End Sub
#End Region

End Class
