'AJAX Conversion By Vikrant

Public Class wfIssueItem_Ajax
    Inherits System.Web.UI.Page

#Region " Enumeration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
    End Enum
#End Region

#Region " Variable Declaration "
    Public mIssue As Issue
    Public mTransTypeID As Trans
    Public mItemName As String   'Added By Saylee on 19-Sep-2007
    Public mPendingToIssueItemList As PendingToIssueItemList    'Added By Saylee on 19-Sep-2007
    Public mIssueItemRequisitionItems As IssueItemRequisitionItems
    'Added By Prashant 17-Dec-2008
    Public mRequisitionItemTypeList As RequisitionItemTypeList
    '-----------------------------
    Public mUnitConverterList As UnitConverterList
    Public mRequisitionItemIssueItems As RequisitionItemIssueItems  'Added by vikrant For New Requisition
    Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mIssue = Session("mIssue")
        'Added New
        mTransTypeID = Session("mTransTypeID")
    End Sub
    Private Sub SetSession()
        Session("mIssue") = mIssue
        'Added New
        Session("mTransTypeID") = mTransTypeID
    End Sub
    Private Sub addAttributes()
        txtQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQty').value,event)")
        txtDiscardAmt.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtDiscardAmt').value,event)")
    End Sub
    Private Sub SetPage()
        If mIssue.IssueItems.CurrentItem.IsNew Then
            lblTitle.Text = "Issue Item [New]"
            btnAddCombo.Enabled = True
            cmbAdd.Enabled = True
            If mIssue.ToTypeID = 18 Then
                cmbAdd.Enabled = False
                btnAddCombo.Enabled = False
            End If
        ElseIf Session("Edit") Then
            lblTitle.Text = "Issue Item [" & mIssue.IssueItems.CurrentItem.ItemName & "]"
            txtPartNo.BackColor = Color.LightGray
            btnAddCombo.Enabled = False
            cmbAdd.Enabled = False
        End If
        'If Session("Edit") Then
        '    lblTitle.Text = "Issue Item [" & mIssue.IssueItems.CurrentItem.ItemName & "]"
        '    'imgBtnPartNo.BackColor = Color.Silver
        '    txtPartNo.BackColor = Color.Silver
        'Else
        '    lblTitle.Text = "Issue Item [New]"
        'End If
        dgRequisitionItemList.Columns(5).Visible = (mIssue.IssueItems.CurrentItem.IssueItemRequisitionItems.Count > 0) And CType(mIssue.TransTypeID, Trans) = Util.Trans.IssueToAircraft And mIssue.StatusID = 1

        'Added By Vikrant on 21-May-2012 FOR ALL21052012-05
        If mIssue.TransTypeID = 25 Then
            chkIsReturnableFromAircraft.Text = "Part Expected Back From Customer"
        End If
        'End
    End Sub
    Private Function setObject() As Boolean
        mIssue.BeginEdit()
        mIssue.IssueItems.CurrentItem.SRNo = mIssue.IssueItems.CurrentIndex + 1
        '.mIssue.IssueItems.CurrentItem.Qty = Val(txtQty.Text)
        mIssue.IssueItems.CurrentItem.DisplayUnitID = New Guid(cmbUnitConverterList.SelectedValue)  'Added By Prashant 12-May-2010
        mIssue.IssueItems.CurrentItem.DisplayUnitName = IIf(cmbUnitConverterList.SelectedIndex > 0, cmbUnitConverterList.SelectedItem.Text, "")     'Added By Prashant 12-May-2010
        mIssue.IssueItems.CurrentItem.DisplayQty = Val(txtQty.Text)
        'Added By Prashant 12-May-2010
        ' mIssue.IssueItems.CurrentItem.Returnable = chkReturnable.Checked
        'If chkReturnable.Checked Then
        mIssue.IssueItems.CurrentItem.ReceiptBalanceQty = Val(txtQty.Text)
        'Else
        '    mIssue.IssueItems.CurrentItem.ReceiptBalanceQty = 0D
        'End If
        mIssue.IssueItems.CurrentItem.Remark = Trim(txtRemark.Text)
        mIssue.IssueItems.CurrentItem.Note = Trim(txtNote.Text)
        mIssue.IssueItems.CurrentItem.OutGoingReleaseNoteNo = Trim(txtOGReleaseNoteNo.Text)
        'Added By Prashant 17-Dec-2008
        mIssue.IssueItems.CurrentItem.RequisitionItemTypeID = CInt(cmbRequisitionItemTypeList.SelectedValue)
        Dim mCEffRateValue As Decimal
        mCEffRateValue = Session("mCEffRateValue")
        'Commented and added By Prashant 'Added By Prashant 4-Nov-2014  ALL04112014
        If mIssue.TransTypeID = 19 Then
            'mIssue.IssueItems.CurrentItem.DiscardAmt = (mIssue.IssueItems.CurrentItem.CEffRate * Val(txtQty.Text)) 'Qty multiplication Added By Prashant 3-Apr-2014 'ALL03042014   'Added by Vikrant on 05-July-2011
            mIssue.IssueItems.CurrentItem.DiscardAmt = (mIssue.IssueItems.CurrentItem.EffRate * Val(txtQty.Text))
       Else
            mIssue.IssueItems.CurrentItem.DiscardAmt = 0
        End If
        'End
        mIssue.IssueItems.CurrentItem.IsReturnableFromAircraft = chkIsReturnableFromAircraft.Checked   'Added by Vikrant on 7.3.12 FORALL03052012
        '-----------------------------
        mIssue.IssueItems.CurrentItem.IsCapitalize = chkIsCapitalize.Checked 'Added By Prashant 25-Apr-2014 'ALL25042014
        mIssue.IssueItems.CurrentItem.BarcodeNo = Trim(txtBarcodeNo.Text)
        Dim mIssueItemRequisitionItem As IssueItemRequisitionItem
        Dim txtValue As TextBox
        Dim i As Integer = 0
        For Each mIssueItemRequisitionItem In mIssue.IssueItems.CurrentItem.IssueItemRequisitionItems
            With mIssueItemRequisitionItem
                txtValue = CType(Me.dgRequisitionItemList.Rows(i).FindControl("txtReqQty"), TextBox)
                .Qty = CDec(Val(txtValue.Text))
                'End of Added Code
            End With
            i = i + 1
        Next
        txtQty.DataBind()
        If mIssue.IssueItems.Contains(mIssue.IssueItems.CurrentItem) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Issue Item", MsgBoxStyle.OkOnly, "")
            mIssue.CancelEdit()
            Exit Function
            'ElseIf AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Then
            '    If (mIssue.IssueItems.CurrentItem.PrimaryCategoryID = 1 And (chkIsReturnableFromAircraft.Checked = False And chkIsCapitalize.Checked = False) And (mIssue.TransTypeID = 14 Or mIssue.TransTypeID = 44)) Then
            '        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Select Unserviceable/Serviceable Part Expected Back Or Is Capitalize", MsgBoxStyle.OkOnly, "")
            '        mIssue.CancelEdit()
            '        Exit Function
            '    Else
            '        mIssue.ApplyEdit()
            '        mIssue.CalculateTotal()
            '    End If
        Else
            mIssue.ApplyEdit()
            mIssue.CalculateTotal()            'Added By Saylee on 7-July-2011
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
                            Session("Sender") = ""
                            Dim mIssue As Issue
                            mIssue = CType(Session("mIssue"), Issue)
                            mIssue.IssueItems.CurrentItem.IssueItemRequisitionItems.Remove(mIssue.IssueItems.CurrentItem.IssueItemRequisitionItems.CurrentItem.ID)
                            Session("mIssue") = mIssue
                            Response.Redirect("wfIssueItem_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mIssue.IssueItems.CurrentItem.IssueItemRequisitionItems.CurrentIndex = Index
        Session("mIssue") = mIssue
    End Sub
    Private Sub AddRequisitionParts(ByVal mRequisitionItems As RequisitionItems)
        Dim mRequisitionItem As RequisitionItem

        If mRequisitionItems Is Nothing Then Exit Sub
        For Each mRequisitionItem In mRequisitionItems
            'If mRequisitionItem.IsSelect Then
            With mIssue.IssueItems.CurrentItem
                'Check is Requisition Part is present ?
                If Not .IssueItemRequisitionItems.Contains(mRequisitionItem.ID) Then
                    'if NOT then add
                    If mIssue.ToTypeID = 17 Then
                        mIssue.WOID = mRequisitionItem.WOID 'Old WO ID
                        mIssue.nWOID = mRequisitionItem.WOID 'New WO ID
                    End If
                    .IssueItemRequisitionItems.Add(.ID, mRequisitionItem.ID, mRequisitionItem.IssueBalQty, mRequisitionItem.RequisitionNo)
                Else
                    'if YES fire Message
                    MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Requisition Part already taken for Issue.", MsgBoxStyle.OkOnly, "Close")
                    Exit Sub
                End If
            End With
        Next
    End Sub
    Private Sub AddRequisitionPart()
        Dim mRequisitionItem As RequisitionItem
        Dim mRequisitionItems As RequisitionItems = Session("mRequisitionItems")
        If mRequisitionItems Is Nothing Then Exit Sub

        For Each mRequisitionItem In mRequisitionItems
            If mRequisitionItem.IsSelect Then
                With mIssue.IssueItems.CurrentItem
                    'Check is Requisition Part is present ?
                    If Not .IssueItemRequisitionItems.Contains(mRequisitionItem.ID) Then
                        'if NOT then add
                        .IssueItemRequisitionItems.Add(.ID, mRequisitionItem.ID, mRequisitionItem.IssueBalQty, mRequisitionItem.RequisitionNo)
                    Else
                        'if YES fire Message
                        MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Requisition Part already taken for Issue.", MsgBoxStyle.OkOnly, "Close")
                        Exit Sub
                    End If
                End With
            End If
        Next
    End Sub
    Private Sub AddSalesOrderParts()
        GetSession()
        mItemName = Session("mItemName")
        txtPartNo.Text = mItemName

        Session("mItemName") = mItemName
        SetSession()
        'mPendingToIssueItemList = PendingToIssueItemList.GetPendingItemList(mIssue.StoreID, txtPartNo.Text, mIssue.IDate)
        'If mPendingToIssueItemList.Count = 0 Then
        '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "", MsgBoxStyle.OKOnly)
        '    msg1.ReplacePage = "wfIssueItem_Ajax.aspx?MsgResult=0&MainBackPage=" & Request.QueryString("MainBackPage")
        '    '   FindNow()
        '    DataFieldBind()
        '    'EnableDisableButtons()
        '    msg1.Show()
        '    Exit Sub
        'End If
        'Response.Redirect("wfPendingToIssueItemList.aspx?BackPage=wfIssue_Ajax.aspx&ChildPage=wfIssueItem_Ajax.aspx&PartName=" & txtPartNo.Text)
        Response.Redirect("wfPendingToIssueItemList.aspx?BackPage=wfIssue_Ajax.aspx&ChildPage1=wfIssueItem_Ajax.aspx&Name=" & HttpUtility.UrlEncode(txtPartNo.Text))

    End Sub
    'Added by vikrant For New Requisition
    Private Sub AddPartForNewRequisition()
        'Dim mRequisitionItemNew As RequisitionItemNew
        Dim mRequisitionItemsNew As RequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForList(mIssue.IDate.ToString, mIssue.IssueItems.CurrentItem.ItemName, mIssue.IssueItems.CurrentItem.ItemID, 0, , , mIssue.MachineID.ToString, , mIssue.RequisitionID.ToString)
        'If mRequisitionItemsNew Is Nothing Then Exit Sub

        'For Each mRequisitionItemNew In mRequisitionItemsNew
        'If mRequisitionItemNew.IsSelect Then
        With mIssue.IssueItems.CurrentItem
            'Check is Requisition Part is present ?
            Dim mRequisitionNew As RequisitionNew
            mRequisitionNew = RequisitionNew.GetRequisition(mIssue.RequisitionID)
            If Not .RequisitionItemIssueItems.Contains(.RequisitionItemID) Then
                'if NOT then add
                'mIssue.RequisitionID = mRequisitionItemNew.ReqID
                'mIssue.MachineID = mRequisitionItemNew.MachineID ''
                .RequisitionItemIssueItems.Add(.ID, .RequisitionItemID, .DisplayQty, mRequisitionNew.RequisitionNo)
                Dim Factor As Decimal
                Dim mUnitConverterList As UnitConverterList = UnitConverterList.GetUnitConverterList(mIssue.IssueItems.CurrentItem.ItemID)
                If Not mUnitConverterList Is Nothing Then
                    Factor = mUnitConverterList.UnitConverterFactor(mIssue.IssueItems.CurrentItem.BaseUnitID, mIssue.IssueItems.CurrentItem.DisplayUnitID)
                End If
                If Factor = 0 Then
                    mIssue.IssueItems.CurrentItem.Qty = mIssue.IssueItems.CurrentItem.DisplayQty
                Else
                    mIssue.IssueItems.CurrentItem.Qty = mIssue.IssueItems.CurrentItem.DisplayQty / Factor
                End If
                mRequisitionNew = Nothing
            Else
                'if YES fire Message
                MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Requisition Part already taken for Issue.", MsgBoxStyle.OkOnly, "Close")
                Exit Sub
            End If
        End With
        'End If
        'Next
    End Sub
    'End
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mIssueItemRequisitionItems = mIssue.IssueItems.CurrentItem.IssueItemRequisitionItems
        dgRequisitionItemList.DataSource = mIssueItemRequisitionItems
        'mnuSalesOrder.Enabled = (mTransTypeID = Flypal.Util.Trans.IssueToCustomer)
        'mnuIssueApprovedPartList.Enabled = (mTransTypeID = Flypal.Util.Trans.IssueToAircraft)

        'Added By Prashant 17-Dec-2008
        mRequisitionItemTypeList = RequisitionItemTypeList.GetRequisitionItemTypeList()
        Session("mRequisitionItemTypeList") = mRequisitionItemTypeList
        cmbRequisitionItemTypeList.DataSource = mRequisitionItemTypeList
        '-----------------------------
        mUnitConverterList = UnitConverterList.GetUnitConverterList(mIssue.IssueItems.CurrentItem.ItemID, "(SELECT)")
        cmbUnitConverterList.DataSource = mUnitConverterList
        Session("mUnitConverterList") = mUnitConverterList

        DataBind()
        If Session("Edit") = True Then
            txtPartNo.ToolTip = "Part No."
        Else
            txtPartNo.ToolTip = "Enter Part No."
        End If

        If txtDiscardAmt.Enabled = True Then
            txtDiscardAmt.ToolTip = "Enter Discard Amount"
        Else
            txtDiscardAmt.ToolTip = "Discard Amount"
        End If
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtQty" Then
            If Session("CheckQty") = "True" Then Exit Sub
            Dim mAvailableQty As Decimal = 0D
            If Session("Edit") Then
                mAvailableQty = mIssue.IssueItems.CurrentItem.Qty + CType(Session("AvailableQuantity"), Decimal)
            Else
                mAvailableQty = CType(Session("AvailableQuantity"), Decimal)
            End If
            If Val(txtQty.Text) <= 0 Then
                custValidator.ErrorMessage = "Quantity must be greater than zero."
                e.IsValid = False
            ElseIf Val(txtQty.Text) > mAvailableQty Then
                custValidator.ErrorMessage = "Quantity must not be greater than receipt balance quantity."
                e.IsValid = False
            End If
        End If
    End Sub
    Private Function CustomValidate1() As Boolean
        Dim strMSG As String = ""
        If Not mIssue.IsValid Then
            For i As Integer = 0 To mIssue.IssueItems.CurrentItem.GetBrokenRulesCollection.Count - 1
                strMSG = strMSG + mIssue.IssueItems.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If

        If strMSG.Trim <> "" Then
            cvQty.ErrorMessage = strMSG
            cvQty.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub ControlVisibility()

        If AppSettings("NewRequisition") = "True" Then
            lblGridHeader.Visible = False
            dgRequisitionItemList.Visible = False
        Else
            lblGridHeader.Visible = (mIssue.TransTypeID = Flypal.Util.Trans.IssueToAircraft)
            dgRequisitionItemList.Visible = (mIssue.TransTypeID = Flypal.Util.Trans.IssueToAircraft)
        End If

        If mIssue.StatusID > 1 Then
            Dim mIssueItemRequisitionItem As IssueItemRequisitionItem
            Dim txtValue As TextBox
            Dim i As Integer = 0
            For Each mIssueItemRequisitionItem In mIssue.IssueItems.CurrentItem.IssueItemRequisitionItems
                With mIssueItemRequisitionItem
                    txtValue = CType(Me.dgRequisitionItemList.Rows(i).FindControl("txtReqQty"), TextBox)
                    txtValue.Enabled = False
                End With
                i = i + 1
            Next
        End If
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "TAAL" Then
            lblBatchNo.Text = "RNN No."
        End If
        'Added By Prashant 3-Jun-2010
        If mIssue.TransTypeID = Util.Trans.IssueToAircraft And mIssue.StatusID = 1 And mIssue.IssueItems.CurrentItem.IssueItemRequisitionItems.Count = 0 Then
            cmbUnitConverterList.Enabled = True
        ElseIf mIssue.TransTypeID = Util.Trans.IssueToAircraft And mIssue.StatusID = 1 And mIssue.IssueItems.CurrentItem.IssueItemRequisitionItems.Count > 0 Then
            cmbUnitConverterList.Enabled = False
        End If
        '----------------------------
        'If chkIsCapitalize.Checked = True Then
        '    chkIsReturnableFromAircraft.Checked = False
        '    chkIsReturnableFromAircraft.Enabled = False
        'Else
        '    chkIsReturnableFromAircraft.Enabled = True
        'End If
        If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then
            txtBarcodeNo.Visible = True
        End If
       
    End Sub
    Private Sub ReceiptItemAttachment(Optional ByVal Visibility As Integer = 0)

     
            mFileAttach = FileAttach.GetAttachment(mIssue.IssueItems.CurrentItem.ReceiptItemID)
            If (mFileAttach.Size = 0 And Visibility = 1) Then
                ImageButton1.Visible = False
                lblReeiptItemView.Visible = False
        ElseIf (mFileAttach.Size > 0 And Visibility = 2) Then
            'Added by Shital on 29-Jun-2020
            Dim mFileAttachments As FileAttachments
            mFileAttachments = FileAttachments.GetChildFileAttachments(mIssue.IssueItems.CurrentItem.ReceiptItemID)
            Dim AttachmentCount As Integer = mFileAttachments.Count
            If AttachmentCount > 1 Then

                Session("mFileAttachments") = mFileAttachments
                Session("TransactionNameMarkLog") = "Receipt Cum Invoice Item"
                Session("TransactionName") = "Receipt Cum Invoice No.and Date"
                Session("TransactionDetails") = mIssue.IssueItems.CurrentItem.ReceiptTextNo + " & " + mIssue.IssueItems.CurrentItem.ReceiptDateFormatted.ToString
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAttachWindow", "OpenAttachWindow();", True)

            Else
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
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
        End If

      
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()

        If Not IsPostBack Then
            If CType(Session("AddRequisitionParts"), String) = "True" Then

                '' mIssue.IssueItems.CurrentItem.IssueItemRequisitionItems.Clear() ''Commented By Rajnish On 22-02-2008
                Dim mRequisitionItems As RequisitionItems


                mRequisitionItems = RequisitionItems.GetRequisitionItems(Requisition.RequisitionLevel.ForEngIssueApproval, mIssue.IDate.ToString, "", mIssue.IssueItems.CurrentItem.ItemID, 0, , , mIssue.MachineID.ToString, mIssue.WOID.ToString)     'Added By Prashant 18/12/2007
                AddRequisitionParts(mRequisitionItems)
                Session("AddRequisitionParts") = "False"
            Else
                Session("AddRequisitionParts") = "False"
            End If
            If CType(Session("AddRequisitionPart"), String) = "True" Then
                'Add selected part(s) to Enquiry Items
                AddRequisitionPart()
                Session("AddRequisitionPart") = "False"
            Else
                Session("AddRequisitionPart") = "False"
            End If

            '===========Added By Saylee on 19-Sep-2007======================
            If CType(Session("AddSalesOrderParts"), String) = "True" Then
                Session("AddSalesOrderParts") = "False" 'Made false before func,as thru func other form is called. 
                AddSalesOrderParts()
            Else
                Session("AddSalesOrderParts") = "False"
            End If
            '==============================================================
            'Added by vikrant For New Requisition
            If ((mTransTypeID = Util.Trans.IssueToAircraft Or mTransTypeID = Util.Trans.IssueToWorkShop Or mTransTypeID = Util.Trans.IssueToWorkOrderAsSpares) And Session("NewRequisition") = "True") Then
                AddPartForNewRequisition()
                Session("NewRequisition") = "False"
            Else
                Session("NewRequisition") = "False"
            End If
            'End

            If txtPartNo.Enabled = True Then
                txtPartNo.Focus()
            End If
            DataFieldBind()
            '===========Added By Saylee on 20-Sep-2007===============
            If ((CType(mIssue.TransTypeID, Trans) = Util.Trans.IssueToCustomer)) Then
                cmbAdd.Items.Add(New ListItem("Sales Order", "1"))
            ElseIf ((CType(mIssue.TransTypeID, Trans) = Util.Trans.IssueToAircraft)) Then
                'cmbAdd.Items.Add(New ListItem("Issue Approved Part List", "2"))
                cmbAdd.Items.Add(New ListItem("Items Removed as Returnable From Aircraft", "3")) 'Added By Vikrant On 16-July-2013 For ALL10072013
            End If
            '========================================================
            'Added By Vikrant On 18-Aug-2016
            Dim ExcludeTransTypeID() As Integer = {18, 16, 49, 55, 51, 58, 59, 60}
            Dim IssueToPartDiscard As String = CType(Session("IssueToDiscardAsExpired"), String)
            If (Not (mIssue.ToTypeID = 18)) Or (Array.IndexOf(ExcludeTransTypeID, mIssue.TransTypeID) <> -1) Then
                If IssueToPartDiscard = "0" Then
                    cmbAdd.Items.Add(New ListItem("All Receipt Items", "4"))
                End If
            End If
            'End
            ControlVisibility()
            SetPage()
        End If
        ReceiptItemAttachment(Visibility:=1)
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ''If (Not User.IsInRole(Rights.[New]) And mIssue.IsNew) Or (Not User.IsInRole(Rights.Edit) And Not mIssue.IsNew) Then
        ''    setObject()
        ''    SetSession()
        ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        ''    msg.ReplacePage = "wfIssueItem_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
        ''    Session("sender") = "Authorization"
        ''    msg.Show()
        ''    Exit Sub
        ''End If
        setObject()
        Session("mIssue") = mIssue
        If Not CustomValidate1() Then upnlvalidationSummary.Update() : Exit Sub
        If mIssue.IssueItems.CurrentItem.IsValid Then
            If setObject() Then
                Session("mIssue") = mIssue
                Session.Remove("Edit")
                Session.Remove("mCEffRateValue")
                Response.Redirect("wfIssue_Ajax.aspx")
            End If
        Else
            upnlvalidationSummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mIssue.IssueItems.CurrentItem.IsNew And Not Session("Edit") = True Then mIssue.IssueItems.Remove(mIssue.IssueItems.CurrentItem)
        Session("mIssue") = mIssue
        Session.Remove("Edit")
        Session.Remove("mCEffRateValue")
        'Response.Redirect(Request.QueryString("BackPage"))
        Response.Redirect("wfIssue_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        setObject()
        SetSession()
        'If mTransTypeID = Util.Trans.IssueToAircraft Then
        '    Session("TransDate") = mIssue.IDate.ToString
        '    Session("ItemID") = mIssue.IssueItems.CurrentItem.ItemID
        '    Response.Redirect("wfPendingIssueApprovedItemList.aspx?BackPage=wfIssue_Ajax.aspx&ChildPage=wfIssueItem_Ajax.aspx&Name=" & txtPartNo.Text)
        'End If
        Session("TransDate") = mIssue.IDate.ToString
        Session("ItemID") = mIssue.IssueItems.CurrentItem.ItemID
        If mTransTypeID = 16 Then
            Response.Redirect("wfPendingToReturnForExchangeRepair_Ajax.aspx?BackPage=wfIssueItem_Ajax.aspx")
        ElseIf mTransTypeID = 18 Then
            Response.Redirect("wfPendingLoanToReturn_Ajax.aspx?BackPage=wfIssueItem_Ajax.aspx")
        ElseIf mTransTypeID = 49 Then
            Response.Redirect("wfPendingLoanToReturn_Ajax.aspx?BackPage=wfIssueItem_Ajax.aspx")
        ElseIf mTransTypeID = 51 Or mTransTypeID = 58 Then '58 Added By Prashant 21-May-2010
            Response.Redirect("wfPendingLoanToReturn_Ajax.aspx?BackPage=wfIssueItem_Ajax.aspx")
        ElseIf mTransTypeID = 55 Then               'Added By Prashant 6-Jan-2010
            Response.Redirect("wfPendingLoanToReturn_Ajax.aspx?BackPage=wfIssueItem_Ajax.aspx")
        ElseIf mIssue.TransTypeID = 59 Then    'Added By Saylee 9-Dec-2010
            Response.Redirect("wfnPendingWOListForIssueSpares_Ajax.aspx?BackPage=wfIssue_Ajax.aspx")
        ElseIf mIssue.TransTypeID = 60 Then    'Added By Saylee 9-Dec-2010
            Response.Redirect("wfnPendingWOListForIssueTools_Ajax.aspx?BackPage=wfIssue_Ajax.aspx")
        Else
            Response.Redirect("wfIssue_Ajax.aspx?BackPage=wfIssueItem_Ajax.aspx")
        End If
    End Sub
    Private Sub dgRequisitionItemList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRequisitionItemList.RowCommand
        Dim Index As Int32 = CInt(e.CommandArgument) + dgRequisitionItemList.PageIndex * dgRequisitionItemList.PageSize
        Select Case e.CommandName
            Case "Remove"
                DeleteRecord(Index)
        End Select
    End Sub
    Private Sub btnAddCombo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddCombo.Click
        If cmbAdd.SelectedValue = 0 Then   'Part List
            btnAddCombo.Enabled = True
            If mIssue.TransTypeID = Util.Trans.LoanReturnToStore Then
                setObject()
                SetSession()
                Session("Back") = True
                Response.Redirect("wfPendingLoanToReturn_Ajax.aspx?BackPage=wfIssue_Ajax.aspx&ChildPage=wfIssueItem_Ajax.aspx&Name=" & HttpUtility.UrlEncode(txtPartNo.Text))
            ElseIf (mIssue.TransTypeID = Util.Trans.IssueforLoanReturntoSupplier) Or mIssue.TransTypeID = Util.Trans.IssueforLoanReturntoCustomer Or mIssue.TransTypeID = Util.Trans.IssueToCustomerAsRepairedReturn Then
                setObject()
                SetSession()
                Session("Back") = True
                Response.Redirect("wfPendingLoanToReturn_Ajax.aspx?BackPage=wfIssue_Ajax.aspx&ChildPage=wfIssueItem_Ajax.aspx&Name=" & HttpUtility.UrlEncode(txtPartNo.Text))
            ElseIf mIssue.TransTypeID = Util.Trans.ExchangeRepairIssueToVendor Then
                setObject()
                SetSession()
                Response.Redirect("wfPendingToReturnForExchangeRepair_Ajax.aspx?BackPage=wfIssue_Ajax.aspx&ChildPage=wfIssueItem_Ajax.aspx&Name=" & HttpUtility.UrlEncode(txtPartNo.Text))
            Else
                setObject()
                SetSession()
                If (mIssue.TransTypeID = 59 Or mIssue.TransTypeID = 60) Then Session("PartNo") = txtPartNo.Text
                Session.Remove("mPendingItemList")
                Response.Redirect("wfPartStockStatus_Ajax.aspx?BackPage=wfIssue_Ajax.aspx&ChildPage=wfIssueItem_Ajax.aspx&Name=" & HttpUtility.UrlEncode(txtPartNo.Text))
            End If
        End If
        If cmbAdd.SelectedValue = 1 Then    'Sales Order
            btnAddCombo.Enabled = True
            If mIssue.TransTypeID = Util.Trans.IssueToAircraft Then
                setObject()
                SetSession()
                Session.Remove("mPendingItemList")
                Response.Redirect("wfIssueApprovalList_Ajax.aspx?BackPage=wfIssue_Ajax.aspx&ChildPage=wfIssueItem_Ajax.aspx&Name=" & HttpUtility.UrlEncode(txtPartNo.Text))
            Else
                If mIssue.TransTypeID = Util.Trans.IssueToCustomer Then
                    setObject()
                    SetSession()
                    Session.Remove("mPendingItemList")
                    Response.Redirect("wfPendingSalesOrderList_Ajax.aspx?BackPage=wfIssue_Ajax.aspx&ChildPage=wfIssueItem_Ajax.aspx&Name=" & HttpUtility.UrlEncode(txtPartNo.Text))
                End If
            End If
        End If
        If cmbAdd.SelectedValue = 2 Then   'Issue Approved Part List
            btnAddCombo.Enabled = True
            If mIssue.TransTypeID = Util.Trans.IssueToAircraft Then ' Or mIssue.TransTypeID = Util.Trans.IssueToWorkShop Then
                setObject()
                SetSession()
                Session.Remove("mPendingItemList")
                Response.Redirect("wfIssueApprovalList_Ajax.aspx?BackPage=wfIssue_Ajax.aspx&ChildPage=wfIssueItem_Ajax.aspx&Name=" & HttpUtility.UrlEncode(txtPartNo.Text))
            End If
        End If
        'Added By Vikrant On 16-July-2013 For ALL10072013
        If cmbAdd.SelectedValue = 3 Then   'Items Removed As Returnable From Aircraft
            setObject()
            SetSession()
            If mIssue.TransTypeID = 14 Then Session("IsRemovedAsReturnableFromAircraft") = True
            Session.Remove("mPendingItemList")
            Response.Redirect("wfPartStockStatus_Ajax.aspx?BackPage=wfIssue_Ajax.aspx&ChildPage=wfIssueItem_Ajax.aspx&Name=" & HttpUtility.UrlEncode(txtPartNo.Text))
        End If
        'End
        'Added By Vikrant On 18-Aug-2016
        If cmbAdd.SelectedValue = 4 Then
            setObject()
            SetSession()
            Response.Redirect("wfPartStockStatusByReceipt_Ajax.aspx?BackPage=wfIssue_Ajax.aspx&ChildPage=wfIssueItem_Ajax.aspx")
        End If
        'End    
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'Private Sub chkIsCapitalize_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkIsCapitalize.CheckedChanged
    '    If chkIsCapitalize.Checked = True Then
    '        chkIsReturnableFromAircraft.Checked = False
    '        chkIsReturnableFromAircraft.Enabled = False
    '    Else
    '        chkIsReturnableFromAircraft.Enabled = True
    '    End If
    'End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ReceiptItemAttachment(Visibility:=2)
    End Sub
#End Region

End Class