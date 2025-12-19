'Added by vikrant For New Requisition
Partial Class wfRequisitionItemNew
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblRequisitionInfo As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrderTerms As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Description "
    Public mRequisitionNew As RequisitionNew
    Public mPriorityList As PriorityList
    Public mMachineList As MachineList
    Public mRequisitionItemTypeList As RequisitionItemTypeList
    Dim Type As Integer
    Public OpeningFor As Integer
    Public RegNo As String = ""
    Public WONo As Integer = 0
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mRequisitionNew = Session("mRequisitionNew")
        mPriorityList = Session("mPriorityList")
        mRequisitionItemTypeList = Session("mRequisitionItemTypeList")
        Type = Session("Type")
        OpeningFor = Session("OpeningFor")
        mMachineList = Session("mMachineList")
        'mItemList = Session("mItemList")
    End Sub
    Private Sub setSession()
        Session("mRequisitionNew") = mRequisitionNew
        Session("mPriorityList") = mPriorityList
        Session("mRequisitionItemTypeList") = mRequisitionItemTypeList
        Session("mMachineList") = mMachineList
        'Session("mItemList") = mItemList
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub addAttributes()
        txtQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQty').value,event)")
        txtDays.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtDays').value,event)")
        'txtWONo.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtWONo').value)")
    End Sub
    Private Sub SetPage()
        If Session("Edit") Then
            lblTitle.Text = "Requisition Item [" & mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo & "]"
            txtPartNo.BackColor = Color.Silver
        End If
    End Sub
    Private Function setObject() As Boolean
        mRequisitionNew.RequisitionItemsNew.CurrentItem.SrNo = mRequisitionNew.RequisitionItemsNew.CurrentIndex + 1
        'mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID = Guid.Empty 'check
        mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo = Trim(txtWONo.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.NRCNo = Trim(txtNRCNo.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID = New Guid(cmbMachine.SelectedValue)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo = IIf(cmbMachine.SelectedIndex > 0, cmbMachine.SelectedItem.Text, "")
        mRequisitionNew.RequisitionItemsNew.CurrentItem.ReasonForRequest = Trim(txtReasonForRequest.Text)
        If Session("ItemID") <> "True" Then
            Session("ItemID") = "True"
            Dim mFetchItemByName As FetchItemByName = FetchItemByName.GetItemByName(txtPartNo.Text.Trim)
            If mFetchItemByName.Count > 0 Then
                If Not (mFetchItemByName(0).ID.Equals(Guid.Empty)) Then
                    mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID = mFetchItemByName(0).ID
                End If
            End If
        End If
        'End
        mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo = txtPartNo.Text
        mRequisitionNew.RequisitionItemsNew.CurrentItem.Description = txtDescription.Text
        mRequisitionNew.RequisitionItemsNew.CurrentItem.IPCReference = Trim(txtReference.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.RequestedQty = CDec(Val(txtQty.Text))
        mRequisitionNew.RequisitionItemsNew.CurrentItem.PriorityID = CInt(cmbPriority.SelectedValue)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.ReasonForPurchase = Trim(txtReasonForPurchase.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.Remark = Trim(txtRemark.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.Note = Trim(txtNote.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.Days = Val(txtDays.Text)
        Dim mtmpItem As Item = Item.GetItem(mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID)    'Added by Saylee on 24-Jul-2012

        If mRequisitionNew.RequisitionItemsNew.Contains(mRequisitionNew.RequisitionItemsNew.CurrentItem) Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Requisition Item", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfRequisitionItemNew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            msg1.Show()
            mRequisitionNew.CancelEdit()
            Exit Function
        ElseIf mtmpItem.NotInUse = True Then 'Added by Saylee on 24-Jul-2012
            If CDate(mtmpItem.NotInUseDate) <= CDate(mRequisitionNew.ReqDate) Then
                Dim msg1 As New SIMsgBox(Page, "Save Alert!", "Part is not applicable since " + mtmpItem.NotInUseDateFormatted + " <br><br> Select another Part from list & try again", "", MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfRequisitionItemNew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
                Exit Function
            End If
        Else
            mRequisitionNew.ApplyEdit()
        End If
        Return True
    End Function
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Delete" Then
                        Try
                            'Session("Sender") = ""
                            'Dim mQuotation As Quotation
                            'mQuotation = CType(Session("mQuotation"), Quotation)
                            'mQuotation.QuotationItems.RemoveAt(mQuotation.QuotationItems.CurrentIndex)
                            'Session("mQuotation") = mQuotation
                            'Response.Redirect("wfQuotationItem.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfRequisitionItemNew.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfRequisitionItemNew.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfRequisitionItemNew.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                    Response.Redirect("wfRequisitionItemNew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK ' And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfRequisitionItemNew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
                    Response.Redirect("wfRequisitionItemNew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfRequisitionItemNew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mPriorityList = PriorityList.GetPriorityList(, , "")
        Session("mPriorityList") = mPriorityList
        cmbPriority.DataSource = mPriorityList

        mMachineList = MachineList.GetMachineList(, , , , , , , , , , True, "(SELECT)")
        Session("mMachineList") = mMachineList
        cmbMachine.DataSource = mMachineList

        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtQty" Then
            If Val(txtQty.Text) <= 0 Then
                custValidator.ErrorMessage = "Quantity must be greater than zero."
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "cmbMachine" Then
            If (mRequisitionNew.ReqTypeID = 1 And (mRequisitionNew.RequisitionEngineeringBrancheID = 1 Or mRequisitionNew.RequisitionEngineeringBrancheID = 2)) Then
                If cmbMachine.SelectedIndex <= 0 Then
                    custValidator.ErrorMessage = "Aircraft Required."
                    e.IsValid = False
                End If
            End If
        End If
    End Sub
    Private Sub AddSingleParts()
        With mRequisitionNew.RequisitionItemsNew.CurrentItem
            If Session("ItemName") <> "" Then
                'If Not mRequisitionNew.RequisitionItemsNew.Contains((CType(Session("ItemName"), RequisitionItemNew).ItemID), mRequisitionNew.RequisitionItemsNew.CurrentItem.ID) Then
                .ItemID = Guid.Empty
                ''.ReqItemID = Guid.Empty
                Try
                    ''.ReqPartNo = Session("ItemName")
                    .PartNo = Session("ItemName")

                Catch ex As Exception
                End Try
                .Description = Session("Description")
                ''.PartNo = Session("ItemName")
                ''.ReqDescription = Session("Description")
                ''.ReqPartNo = Session("ItemName")
                ''.IPCReference = ""
                ''.RequestedQty = 0
                'End 'If
            Else
                If Not mRequisitionNew.RequisitionItemsNew.Contains(CType(Session("SelectedRequisitionItem"), RequisitionItemNew).ItemID) Then
                    If OpeningFor = 1 Then
                        .ItemID = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).ItemID
                        .PartNo = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).PartNo
                        .Description = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).Description
                        ''.ReqItemID = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).ItemID
                        ''.ReqPartNo = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).PartNo
                        ''.ReqDescription = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).Description
                        '.JobDescription = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).JobDescription
                        .IPCReference = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).IPCReference
                        .RequestedQty = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).RequestedQty
                        '.UnitName = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).UnitName
                    ElseIf OpeningFor = 2 Then
                        .ItemID = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).ItemID
                        .PartNo = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).PartNo
                        .Description = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).Description
                        '.JobDescription = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).JobDescription
                        .IPCReference = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).IPCReference
                        '.UnitName = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).UnitName
                    End If
                Else
                    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.ValidationAlert, SIMsgBox.Message_text.ValidationAlert, "Part : '" + mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo.ToString + "' already taken for Requisition.", MsgBoxStyle.OkOnly)
                    msg.ReplacePage = "wfRequisitionItemNew.aspx?BackPage=" & Request.QueryString("BackPage")
                    Session("sender") = "Authorization"
                    msg.Show()
                    Exit Sub
                End If
            End If
        End With
    End Sub
    Private Sub controlvisibility()
        If mRequisitionNew.ReqTypeID <> 1 Then
            lblAircraft.Visible = False
            cmbMachine.Visible = False
            lblWONo.Visible = False
            txtWONo.Visible = False
            btnSelectWONo.Visible = False
            lblAircraftStar.Visible = False
        End If
        If Not mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID.Equals(Guid.Empty) Then
            cmbMachine.Enabled = False
        End If
        If Not mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID.Equals(Guid.Empty) Then
            txtWONo.Enabled = False
        End If

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        addAttributes()
        If CType(Session("AddSingleParts"), String) = "True" Then
            AddSingleParts()
            Session("AddSingleParts") = "False"
        Else
            Session("AddSingleParts") = "False"
        End If

        If Not Session("ID") Is Nothing Then
            mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID = New Guid(Session("ID").ToString)
            mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo = Session("No")
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID = Session("WOMachineID")
            Session.Remove("ID")
            Session.Remove("No")
            Session.Remove("WOMachineID")
        End If


        If Not IsPostBack Then
            If txtPartNo.Enabled = True Then
                setFocus(txtPartNo)
            End If
            Type = Request.QueryString("Type")
            Session("Type") = Type
            DataFieldBind()
        End If
        controlvisibility()
        SetPage()
        MessageBoxResult()
        If Session("Edit") Or (Not mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty)) Then
            If ((Session("Edit") And (Not mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty))) Or (Not mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty))) Then
                txtPartNo.BackColor = System.Drawing.Color.Gainsboro
                txtDescription.BackColor = System.Drawing.Color.Gainsboro
            Else
                txtPartNo.BackColor = System.Drawing.Color.White
                txtDescription.BackColor = System.Drawing.Color.White
            End If
        End If
    End Sub
    Private Sub imgbtnPartNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnPartNo.Click
        Session("AddMultipleParts") = "False"
        Session("Add") = True
        setObject()
        If mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty) Then 'v'
            ''Session("ItemName") = mRequisitionNew.RequisitionItemsNew.CurrentItem.ReqPartNo '.ItemName
            ''Session("Description") = mRequisitionNew.RequisitionItemsNew.CurrentItem.ReqDescription '.ItemDescription
            Session("ItemName") = mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo '.ItemName
            Session("Description") = mRequisitionNew.RequisitionItemsNew.CurrentItem.Description '.ItemDescription
        Else
            Session("ItemName") = ""
            Session("Description") = ""
        End If
        Session("mRequisitionNew") = mRequisitionNew
        Session("mPriorityList") = mPriorityList
        Session("PartNo") = txtPartNo.Text
        Session("mRequisitionItemTypeList") = mRequisitionItemTypeList
        Response.Redirect("wfRequisitionItemSearch.aspx?BackPage=wfRequisitionItemNew.aspx&ChildPage=wfRequisitionNew.aspx")
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'If (Not User.IsInRole("NewRequisitionNew") And mRequisitionNew.IsNew) Or (Not User.IsInRole("NewRequisitionEdit") And Not mRequisitionNew.IsNew) Then
        '    setObject()
        '    setSession()
        '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfRequisitionItemNew.aspx?BackPage=" & Request.QueryString("BackPage")
        '    Session("sender") = "Authorization"
        '    msg.Show()
        '    Exit Sub
        'End If
        If IsValid Then
            If setObject() Then
                Session("mRequisitionNew") = mRequisitionNew
                Session.Remove("mModelList")
                Session.Remove("mPriorityList")
                Session.Remove("mMachineList")
                Session.Remove("mRequisitionItemTypeList")
                Session.Remove("ItemID")
                Session.Remove("Edit")
                Response.Redirect(Request.QueryString("BackPage"))
            End If
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mRequisitionNew.RequisitionItemsNew.CurrentItem.IsNew And Not Session("Edit") = True Then mRequisitionNew.RequisitionItemsNew.Remove(mRequisitionNew.RequisitionItemsNew.CurrentItem)
        Session.Remove("Edit")
        Session.Remove("mModelList")
        Session.Remove("mPriorityList")
        Session.Remove("mMachineList")
        Session.Remove("mRequisitionItemTypeList")
        Session.Remove("ItemID")
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnSelectWONo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectWONo.Click
        RegNo = IIf(cmbMachine.SelectedIndex > 0, cmbMachine.SelectedItem.Text, "")
        'WONo = IIf(txtWONo.Text = "", 0, Trim(txtWONo.Text))
        Session("RegNo") = RegNo
        Session("WONo") = WONo

        setObject()
        ''setComboDetails()
        Session("mRequisitionNew") = mRequisitionNew
        'Response.Redirect("wfSelectListForNewRequisition.aspx")
        Response.Redirect("wfSelectListForNewRequisition.aspx?BackPage=wfRequisitionItemNew.aspx&ChildPage=wfRequisitionNew.aspx")
    End Sub
    Private Sub cmbPriority_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPriority.SelectedIndexChanged
        If cmbPriority.SelectedIndex = 5 Then
            If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
                txtDays.Enabled = True
            Else
                txtDays.Enabled = False
            End If
        Else
            txtDays.Enabled = False
            txtDays.Text = "0"
        End If

    End Sub
#End Region

End Class
