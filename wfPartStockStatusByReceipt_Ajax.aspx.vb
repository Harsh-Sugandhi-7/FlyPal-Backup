'Added By Vikrant On 18-Aug-2016
Imports System.Linq
Imports System.Collections.Generic

Public Class wfPartStockStatusByReceipt_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mIssue As Issue
    Public mStockItemList As PendingToIssueItemListByReceipt
    Public mPendingItemList As PendingToIssueList
    Public PartNo As String
    Public mItemName As String
    Dim mIndex2 As Int32
    Dim LinkID As String
    'Public mAlternateStockList As AlternateStockItemList
    Dim IssueToDiscardAsExpired As String = "0"
    Dim mFileAttach As FileAttach
#End Region

    '#Region " Business Methods "
    Private Sub GetSession()
        mIssue = CType(Session("mIssue"), Issue)
        mStockItemList = CType(Session("mStockItemList"), PendingToIssueItemListByReceipt)
        mPendingItemList = CType(Session("mPendingItemList"), PendingToIssueList)
        'mAlternateStockList = Session("mAlternateStockList") 'Added By Utkarsh ON 30-Apr-2012 FOR ALLIssue30042012
        PartNo = Session("PartNo")
        If mIssue Is Nothing Then
            'do nothing
        Else
            If mIssue.TransTypeID = 18 Then
                LinkID = Session("mLinkID").ToString
            End If
        End If
        IssueToDiscardAsExpired = Session("IssueToDiscardAsExpired")
    End Sub
    Private Sub AddIssueItems(ByVal Idx As Integer)
        If Not mIssue Is Nothing Then
            If mIssue.TransTypeID = 14 And mIssue.ToTypeID <> 18 Then
                'Do Nothing 
            Else
                If ((mIssue.TransTypeID = Trans.IssueToAircraft Or mIssue.TransTypeID = Trans.IssueToWorkShop) And mIssue.ToTypeID = 18) Then
                    Session("NewRequisition") = "True"
                End If
            End If
            SetObject(Idx)
        End If
    End Sub
    '    Private Sub MessageBoxResult()
    '        Dim Result1 As MsgBoxResult
    '        Result1 = MSGBoxCtrl.Result
    '        If Result1 > 0 Then
    '            Select Case Result1
    '                Case MsgBoxResult.Yes
    '                    If MSGBoxCtrl.Sender = "Expired" Then
    '                        Try
    '                            Session("Sender") = ""
    '                            mIndex2 = Session("Index2")
    '                            If Session("IsAlternatePart") = "True" Then
    '                                SetObject(mIndex2, True)
    '                            Else
    '                                SetObject(mIndex2)
    '                            End If
    '                            Session("CheckQty") = "False"
    '                            Session.Remove("mStockItemList")
    '                            Session.Remove("mPendingItemList")
    '                            Session.Remove("mAlternateStockList")
    '                            Session.Remove("PartNo")
    '                            Session("Edit") = False
    '                            Session.Remove("Index2")
    '                            Session.Remove("ItemName")
    '                            Session.Remove("IsAlternatePart")
    '                            Session("IsRemovedAsReturnableFromAircraft") = False 'Added By Vikrant On 16-July-2013 For ALL10072013
    '                            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
    '                        Catch ex As SqlException
    '                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
    '                            Exit Sub
    '                        End Try
    '                        'Added By Vikrant On 03-Feb-2016 For ALL03022016
    '                    ElseIf MSGBoxCtrl.Sender = "SelectAllParts" Then
    '                        Session("IsAllPartsSelected") = True
    '                        Dim FirstItem As Integer = 0
    '                        mPendingItemList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, , , , , , mIssue.IDate.ToString, mIssue.TransTypeID, , , chkShowBERPart.Checked, IsAllPartsRequired:=True, IssueToDiscardAsExpired:=CInt(IssueToDiscardAsExpired))
    '                        For i As Integer = 0 To mPendingItemList.Count - 1
    '                            If Not mIssue.IssueItems.Contains(mPendingItemList(i).ReceiptItemID) Then
    '                                If FirstItem < 500 Then
    '                                    If FirstItem = 0 Then 'For First Item directly SetObject
    '                                        SetObjectForAllParts(i)
    '                                    Else 'For All Other Items First Add New Child then SetObject
    '                                        mIssue.IssueItems.Add(mIssue.ID, mIssue.TransTypeID)
    '                                        mIssue.IssueItems.CurrentIndex = mIssue.IssueItems.Count - 1
    '                                        mIssue.IssueItems.CurrentItem.SRNo = mIssue.IssueItems.CurrentIndex + 1
    '                                        SetObjectForAllParts(i)
    '                                    End If
    '                                    FirstItem = FirstItem + 1
    '                                Else
    '                                    Exit For
    '                                End If
    '                            End If
    '                        Next
    '                        Session("CheckQty") = "False"
    '                        Session.Remove("mStockItemList")
    '                        Session.Remove("mPendingItemList")
    '                        Session.Remove("mAlternateStockList")
    '                        Session.Remove("PartNo")
    '                        Session.Remove("PendingIssuedQty")
    '                        Session("Edit") = False
    '                        Session("IsRemovedAsReturnableFromAircraft") = False 'Added By Vikrant On 16-July-2013 For ALL10072013
    '                        Session("mIssue") = mIssue
    '                        mIssue.CalculateTotal()
    '                        Response.Redirect("wfIssue_Ajax.aspx")
    '                        'End
    '                    End If
    '                Case MsgBoxResult.No
    '                    If MSGBoxCtrl.Sender = "Expired" Then
    '                        Session("sender") = ""
    '                        Response.Redirect("wfPartStockStatus_Ajax.aspx?ChildPage=" & Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
    '                    End If
    '            End Select
    '        End If
    '    End Sub
    '    Private Sub SetObjectForAllParts(ByVal Index As Int32) 'Added By Vikrant On 03-Feb-2016 For ALL03022016
    '        mIssue.IssueItems.CurrentItem.ReceiptItemID = mPendingItemList(index).ReceiptItemID
    '        If mPendingItemList(index).IsSerialized Then
    '            mIssue.IssueItems.CurrentItem.DisplayQty = 1   'Added By Prashant  12-May-2010     
    '            Session("AvailableQuantity") = 1
    '            Session("SerialNo") = mPendingItemList(index).SerialNo
    '        Else
    '            mIssue.IssueItems.CurrentItem.DisplayQty = mPendingItemList(Index).AvailableQuantity 'Added By Prashant  12-May-2010   
    '            Session("SerialNo") = mPendingItemList(Index).SerialNo
    '        End If
    '        'Added By Prashant 3-July-2011 Once StoreID set for Issue for transaction 49,51,58 then do not set agin
    '        If mIssue.TransTypeID = Flypal.Util.Trans.IssuetoSupplierasRentalLease Or mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoCustomer Or mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoSupplier Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToWorkOrderAsSpares Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToWorkOrderAsTools Or mIssue.TransTypeID = Flypal.Util.Trans.LoanReturnToStore Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToCustomerAsRepairedReturn Or ((mIssue.TransTypeID = Flypal.Util.Trans.IssueToAircraft Or mIssue.TransTypeID = Util.Trans.IssueToWorkShop) And mIssue.ToTypeID = 18) Then   'Added By Saylee 27-Jan-2010  'Trans.IssueToRequisition Added by vikrant For New Requisition
    '            If mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoSupplier Or mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoCustomer Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToCustomerAsRepairedReturn Then
    '                If mIssue.StoreID.Equals(Guid.Empty) Then
    '                    mIssue.StoreID = mPendingItemList.Item(index).StoreID
    '                End If
    '            Else
    '                mIssue.StoreID = mPendingItemList.Item(index).StoreID
    '            End If
    '        End If
    '        mIssue.IssueItems.CurrentItem.DisplayUnitID = mPendingItemList(index).UnitID
    '        mIssue.IssueItems.CurrentItem.DisplayUnitName = mPendingItemList(Index).UnitName
    '        mIssue.IssueItems.CurrentItem.DiscardAmt = mPendingItemList(index:=Index).EffRate 'Added By Prashant On 18-Jul-2016
    '        Session("mIssue") = mIssue
    '    End Sub 'End
    Private Sub SetObject(ByVal Index As Int32, Optional ByVal IsAlternatePart As Boolean = False) 'Changed By Utkarsh On 02-May-2012 FOR ALLIssue30042012
        If IsAlternatePart = False Then                                                             'Changed By Utkarsh On 02-May-2012 FOR ALLIssue30042012
            If Not mIssue Is Nothing Then
                mIssue.IssueItems.CurrentItem.ReceiptItemID = mPendingItemList(Index).ReceiptItemID
                If mPendingItemList(Index).IsSerialized Then
                    mIssue.IssueItems.CurrentItem.DisplayQty = 1   'Added By Prashant  12-May-2010     
                    Session("AvailableQuantity") = 1
                    Session("SerialNo") = mPendingItemList(Index).SerialNo
                Else
                    Dim mUnitConverterList As UnitConverterList = UnitConverterList.GetUnitConverterList(mPendingItemList(Index).ItemID)
                    Dim Factor As Decimal = 0

                    If Not mUnitConverterList Is Nothing Then
                        Factor = mUnitConverterList.UnitConverterFactor(mPendingItemList(Index).UnitID, mPendingItemList(Index).DisplayUnitID)
                    End If
                    'mIssue.IssueItems.CurrentItem.DisplayQty = mPendingItemList(Index).AvailableQuantity
                    'Session("AvailableQuantity") = mPendingItemList(Index).AvailableQuantity
                    mIssue.IssueItems.CurrentItem.DisplayQty = IIf(Factor > 0, mPendingItemList(Index).AvailableQuantity * Factor, mPendingItemList(Index).AvailableQuantity)
                    Session("AvailableQuantity") = mPendingItemList(Index).AvailableQuantity
                    Session("SerialNo") = mPendingItemList(Index).SerialNo
                End If
                'Added By Prashant 3-July-2011 Once StoreID set for Issue for transaction 49,51,58 then do not set agin
                If mIssue.TransTypeID = Flypal.Util.Trans.IssuetoSupplierasRentalLease Or mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoCustomer Or mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoSupplier Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToWorkOrderAsSpares Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToWorkOrderAsTools Or mIssue.TransTypeID = Flypal.Util.Trans.LoanReturnToStore Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToCustomerAsRepairedReturn Or ((mIssue.TransTypeID = Flypal.Util.Trans.IssueToAircraft Or mIssue.TransTypeID = Util.Trans.IssueToWorkShop) And mIssue.ToTypeID = 18) Then   'Added By Saylee 27-Jan-2010  'Trans.IssueToRequisition Added by vikrant For New Requisition
                    If mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoSupplier Or mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoCustomer Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToCustomerAsRepairedReturn Then
                        If mIssue.StoreID.Equals(Guid.Empty) Then
                            mIssue.StoreID = mPendingItemList.Item(Index).StoreID
                        End If
                    Else
                        mIssue.StoreID = mPendingItemList.Item(Index).StoreID
                    End If
                End If
                'Commented and added by Prashant 18-Sep-2019
                'mIssue.IssueItems.CurrentItem.DisplayUnitID = mPendingItemList(Index).UnitID
                'mIssue.IssueItems.CurrentItem.DisplayUnitName = mPendingItemList(Index).UnitName
                mIssue.IssueItems.CurrentItem.DisplayUnitID = mPendingItemList(Index).DisplayUnitID
                mIssue.IssueItems.CurrentItem.DisplayUnitName = mPendingItemList(Index).DisplayUnitName
                'If (mIssue.TransTypeID = 14 And mPendingItemList(Index).PrimaryCategoryID = 1) Then 'Added By Prashant On 12-Apr-2016 For ALL12042016
                '    mIssue.IssueItems.CurrentItem.IsReturnableFromAircraft = True
                'End If
                If (mIssue.TransTypeID = 19) Then 'Added By Prashant On 18-Jul-2016
                    mIssue.IssueItems.CurrentItem.DiscardAmt = mPendingItemList(index:=Index).EffRate
                End If
                mIssue.IssueItems.CurrentItem.ItemTagID = mPendingItemList(index:=Index).ItemTagID
                mIssue.IssueItems.CurrentItem.ItemTagName = mPendingItemList(index:=Index).ItemTagName
                mIssue.IssueItems.CurrentItem.StatusKit = mPendingItemList(index:=Index).StatusKit
                Session("mIssue") = mIssue
            End If
        Else
            'If Not mIssue Is Nothing Then
            '    mIssue.IssueItems.CurrentItem.ReceiptItemID = mAlternateStockList(Index).ReceiptItemID
            '    If mAlternateStockList(Index).IsSerialized Then
            '        mIssue.IssueItems.CurrentItem.DisplayQty = 1
            '        Session("AvailableQuantity") = 1
            '        Session("SerialNo") = mAlternateStockList(Index).SerialNo
            '    Else
            '        'Added By Saylee 1-Feb-2010
            '        If CType(Session("PendingIssuedQty"), Decimal) > mAlternateStockList(Index).AvailableQuantity Then
            '            mIssue.IssueItems.CurrentItem.DisplayQty = mAlternateStockList(Index).AvailableQuantity 'Added By Prashant  12-May-2010   
            '            Session("AvailableQuantity") = mAlternateStockList(Index).AvailableQuantity
            '        Else
            '            'Added by Saylee on 8-Dec-2010
            '            mIssue.IssueItems.CurrentItem.DisplayQty = CType(Session("PendingIssuedQty"), Decimal)
            '            Session("AvailableQuantity") = CType(Session("RequiredQty"), Decimal)
            '        End If
            '        Session("SerialNo") = mAlternateStockList(Index).SerialNo
            '    End If
            '    'Added By Prashant 3-July-2011 Once StoreID set for Issue for transaction 49,51,58 then do not set agin
            '    If mIssue.TransTypeID = Flypal.Util.Trans.IssuetoSupplierasRentalLease Or mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoCustomer Or mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoSupplier Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToWorkOrderAsSpares Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToWorkOrderAsTools Or mIssue.TransTypeID = Flypal.Util.Trans.LoanReturnToStore Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToCustomerAsRepairedReturn Then   'Added By Saylee 27-Jan-2010
            '        If mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoSupplier Or mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoCustomer Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToCustomerAsRepairedReturn Then
            '            If mIssue.StoreID.Equals(Guid.Empty) Then
            '                mIssue.StoreID = mAlternateStockList.Item(Index).StoreID
            '            End If
            '        Else
            '            mIssue.StoreID = mAlternateStockList.Item(Index).StoreID
            '        End If
            '    End If
            '    mIssue.IssueItems.CurrentItem.DisplayUnitID = mAlternateStockList(Index).UnitID
            '    'If (mIssue.TransTypeID = 14 And mAlternateStockList(Index).PrimaryCategoryID = 1) Then 'Added By Prashant On 12-Apr-2016 For ALL12042016
            '    '    mIssue.IssueItems.CurrentItem.IsReturnableFromAircraft = True
            '    'End If
            '    If (mIssue.TransTypeID = 19) Then 'Added By Prashant On 18-Jul-2016
            '        mIssue.IssueItems.CurrentItem.DiscardAmt = mPendingItemList(index:=Index).EffRate
            '    End If
            '    Session("mIssue") = mIssue
            'End If
        End If
    End Sub
#Region " Data Binding "
    Private Sub DataFieldBind()
        dgStockItemList.DataSource = mStockItemList
        dgPendingItemList.DataSource = mPendingItemList
        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        Dim Count As Integer = 0
        If custValidator.ControlToValidate = "txtText" Then
            Dim chkReceiptItemSelected As CheckBox
            Dim ReceiptItemID As Guid
            Dim ItemName As String
            For i As Integer = 0 To dgPendingItemList.Rows.Count - 1
                chkReceiptItemSelected = CType(dgPendingItemList.Rows(i).FindControl("chkSelect"), CheckBox)
                ReceiptItemID = New Guid(dgPendingItemList.DataKeys(i).Values("ReceiptItemID").ToString)
                ItemName = dgPendingItemList.DataKeys(i).Values("ItemName").ToString
                If chkReceiptItemSelected.Checked Then
                    Count += 1
                    If mIssue.IssueItems.Contains(ReceiptItemID) Then
                        e.IsValid = False
                        custValidator.ErrorMessage = "Please select different Receipt Item." + ItemName + " is already added in list."
                        Exit Sub
                    End If
                End If
            Next
            If Count = 0 Then
                e.IsValid = False
                custValidator.ErrorMessage = "Please select at least one Stock Item"
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub ReceiptItemAttachment(Optional ByVal ReceiptItemID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal Visibility As Integer = 0)

        ' If condition Added by Shital on 29-Jun-2020
        Dim mFileAttachments As FileAttachments
        mFileAttachments = FileAttachments.GetChildFileAttachments(New Guid(ReceiptItemID))
        Dim AttachmentCount As Integer = mFileAttachments.Count
        If AttachmentCount > 1 Then

            Session("mFileAttachments") = mFileAttachments
            Session("TransactionNameMarkLog") = "Receipt Cum Invoice Item"
            Session("TransactionName") = "Receipt Cum Invoice No.and Date"

            Session("TransactionDetails") = mPendingItemList(Visibility).OriginalReceiptTextNo + " & " + mPendingItemList(Visibility).OriginalReceiptDate
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAttachWindow", "OpenAttachWindow();", True)

        Else
            mFileAttach = FileAttach.GetAttachment(New Guid(ReceiptItemID))
            If (mFileAttach.Size > 0) Then
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
    'Added By Prashant 6-Jul-2020 All06072020
    Protected Sub OnRowDataBoundPendingItemList(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(2).BackColor = System.Drawing.ColorTranslator.FromHtml("#" & e.Row.Cells(20).Text) '25=>20 Ajay
        End If
    End Sub
   'End of Added By Prashant 6-Jul-2020 All06072020
#End Region

    '#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            'txtText.Focus()
            mPendingItemList = PendingToIssueList.NewPendingToIssueList
            mStockItemList = PendingToIssueItemListByReceipt.NewPendingItemList
            Session("mStockItemList") = mStockItemList
            Session("mPendingItemList") = mPendingItemList
            DataFieldBind()
            lblResult.Text = "Receipt List : " & mStockItemList.Count & " Record(s) found."
            lblResult1.Text = "Stock Details : " & mPendingItemList.Count & " Record(s) found."
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If Not mIssue Is Nothing Then
            mStockItemList = PendingToIssueItemListByReceipt.GetPendingItemList(StoreID:=mIssue.StoreID, IssueDate:=mIssue.IDate.ToString, TransTypeID:=mIssue.TransTypeID, IsBERPart:=False, IssueToDiscardAsExpired:=CInt(IssueToDiscardAsExpired), Text:=Trim(txtText.Text), No:=CInt(Val(txtNo.Text)))
        End If
        Session("mStockItemList") = mStockItemList
        dgStockItemList.DataSource = mStockItemList
        dgStockItemList.DataBind()

        lblResult.Text = "Receipt List : " & mStockItemList.Count & " Record(s) found."
        'Added By Utkarsh ON 03-May-2012 FOR ALLIssue30042012
        'dgAlternateStockList.Visible = False
        'lblResult2.Visible = False
        'End
        upnlStockItemList.Update()
    End Sub
    Private Sub dgStockItemList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgStockItemList.RowCommand
        DueAtMessage.Visible = False
        Select Case e.CommandName
            Case "SelectRecord"
                Dim Index1 As Int32 = CInt(e.CommandArgument) + dgStockItemList.PageIndex * dgStockItemList.PageSize
                Dim ReceID As Guid = New Guid(dgStockItemList.DataKeys(CInt(e.CommandArgument)).Values(0).ToString)
                If Not mIssue Is Nothing Then
                    'Added By Prashant 3-July-2011 to Show only respective Store records
                    'Added By Prashant 26-Sep-2011
                    If mIssue.TransTypeID = 18 Then  'Loan Return To Store
                        mPendingItemList = PendingToIssueList.GetPendingToIssueList(StoreID:=mIssue.StoreID, IssueDate:=mIssue.IDate.ToString, _
                                                                                    TransTypeID:=mIssue.TransTypeID, ReceiptID:=ReceID.ToString, _
                                                                                    IsAllPartsRequired:=True, ToTypeIDOfIssue:=mIssue.ToTypeID)
                        'Added By Utkarsh ON 30-Apr-2012 FOR ALLIssue30042012
                        'mAlternateStockList = AlternateStockItemList.GetAlternateStockItemList(mIssue.StoreID, mStockItemList(Index1).ItemName, , , , , mIssue.IDate.ToString, mIssue.TransTypeID, mStockItemList(Index1).LinkID.ToString)
                        'End 
                        'lblResult2.Text = "Alternate Stock Item List : " & mAlternateStockList.Count & " Record(s) found."
                    Else '-----------------------------
                        mPendingItemList = PendingToIssueList.GetPendingToIssueList(StoreID:=mIssue.StoreID, IssueDate:=mIssue.IDate.ToString, _
                                                                                    TransTypeID:=mIssue.TransTypeID, ReceiptID:=ReceID.ToString, _
                                                                                    IsAllPartsRequired:=True, ToTypeIDOfIssue:=mIssue.ToTypeID)
                        'Added By Utkarsh ON 30-Apr-2012 FOR ALLIssue30042012
                        If mIssue.TransTypeID = 16 Or mIssue.TransTypeID = 19 Or mIssue.TransTypeID = 58 Then
                            'Do nothing
                        Else
                            'mAlternateStockList = AlternateStockItemList.GetAlternateStockItemList(mIssue.StoreID, mStockItemList(Index1).ItemName, , , , , mIssue.IDate.ToString, mIssue.TransTypeID, mStockItemList(Index1).ItemID.ToString)
                            'lblResult2.Text = "Alternate Stock Item List : " & mAlternateStockList.Count & " Record(s) found."
                        End If
                        'End 
                    End If '-------------------------------------------------------------------
                End If
                Session("mPendingItemList") = mPendingItemList
                'Session("mAlternateStockList") = mAlternateStockList 'Added By Utkarsh ON 30-Apr-2012 FOR ALLIssue30042012
                'dgAlternateStockList.DataSource = mAlternateStockList
                DataFieldBind()
                lblResult.Text = "Receipt List : " & mStockItemList.Count & " Record(s) found."
                lblResult1.Text = "Stock Details : " & mPendingItemList.Count & " Record(s) found."
                upnlPendingItemList.Update()
                'upnlAlternateStockList.Update()
        End Select
    End Sub
    Private Sub btnOk_Click(sender As Object, e As System.EventArgs) Handles btnOk.Click
        If IsValid Then
            DueAtMessage.Visible = False
            Dim chk As CheckBox
            Dim IsFirstItem As Boolean = True
            For i As Integer = 0 To mPendingItemList.Count - 1
                chk = CType(dgPendingItemList.Rows(i).FindControl("chkSelect"), CheckBox)
                If chk.Checked Then
                    If Not mIssue.IssueItems.Contains(mPendingItemList(i).ReceiptItemID) Then
                        If IsFirstItem Then
                            AddIssueItems(i)
                            IsFirstItem = False
                        Else
                            mIssue.IssueItems.Add(mIssue.ID, mIssue.TransTypeID)
                            mIssue.IssueItems.CurrentIndex = mIssue.IssueItems.Count - 1
                            AddIssueItems(i)
                        End If
                    End If
                End If
            Next
            If IsFirstItem Then
                mIssue.IssueItems.Remove(mIssue.IssueItems.CurrentItem)
            End If

            Session("CheckQty") = "False"
            Session.Remove("mStockItemList")
            Session.Remove("mPendingItemList")
            Session.Remove("mAlternateStockList")
            Session.Remove("PartNo")
            Session.Remove("PendingIssuedQty")
            Session("Edit") = False
            Session("IsRemovedAsReturnableFromAircraft") = False 'Added By Vikrant On 16-July-2013 For ALL10072013
            Response.Redirect(Request.QueryString("BackPage"))
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub dgStockItemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgStockItemList.PageIndexChanging
        DueAtMessage.Visible = False
        dgStockItemList.PageIndex = e.NewPageIndex
        dgStockItemList.DataSource = mStockItemList
        dgStockItemList.DataBind()
        Session("mStockItemList") = mStockItemList
    End Sub
    Private Sub dgPendingItemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingItemList.PageIndexChanging
        DueAtMessage.Visible = False
        dgPendingItemList.PageIndex = e.NewPageIndex
        dgPendingItemList.DataSource = mPendingItemList
        dgPendingItemList.DataBind()
        Session("mPendingItemList") = mPendingItemList
    End Sub
    Private Sub dgStockItemList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgStockItemList.Sorting
        DueAtMessage.Visible = False
        mStockItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mStockItemList") = mStockItemList
        dgStockItemList.DataSource = mStockItemList
        dgStockItemList.DataBind()
    End Sub
    Private Sub dgPendingItemList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingItemList.RowCommand
        Select Case e.CommandName
            Case "ViewRec"
                Dim Index1 As Int32 = CInt(e.CommandArgument) + dgPendingItemList.PageIndex * dgPendingItemList.PageSize
                ReceiptItemAttachment(ReceiptItemID:=mPendingItemList(Index1).ReceiptItemID.ToString, Visibility:=Index1)
        End Select
    End Sub
    Private Sub dgPendingItemList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPendingItemList.Sorting
        DueAtMessage.Visible = False
        mPendingItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingItemList") = mPendingItemList
        dgPendingItemList.DataSource = mPendingItemList
        dgPendingItemList.DataBind()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session.Remove("mStockItemList")
        Session.Remove("mPendingItemList")
        Session.Remove("mAlternateStockList")
        Session.Remove("PartNo")
        Session.Remove("PendingIssuedQty")
        Session.Remove("Index2")
        Session.Remove("ItemName")
        Session("Edit") = False
        Session.Remove("mLinkID")
        If Request.QueryString("ChildPage1") = "wfnPendingWOListForIssueSpares_Ajax.aspx" Or Request.QueryString("ChildPage1") = "wfnPendingWOListForIssueTools_Ajax.aspx" Or Request.QueryString("ChildPage1") = "wfRequisitionItemListForIssue_Ajax.aspx" Then
            Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        Else
            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetDistinctTextListAutoComplete(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mDistinctTextAutoComplete As DistinctTextListAutoComplete
        Dim str As String() = contextKey.Split("¿")
        Dim mTransTypeID As Integer = CInt(str(0).Substring(str(0).IndexOf("=") + 1))
        Dim mIssueDate As String = str(1).Substring(str(1).IndexOf("=") + 1)
        mDistinctTextAutoComplete = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, "AllRec", True, ToDate:=mIssueDate)
        If count = 0 Then
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In mDistinctTextAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
        Else
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In mDistinctTextAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
        End If
    End Function
#End Region

End Class