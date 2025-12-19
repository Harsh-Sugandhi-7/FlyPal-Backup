Imports System.Collections.Generic

Public Class wfPartStockStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mIssue As Issue
    Public mStockItemList As PendingToIssueItemList
    Public mPendingItemList As PendingToIssueList
    Public mOrder As Order
    Public PartNo As String
    Public mItemName As String
    Dim mIndex2 As Int32
    Dim LinkID As String
    Public mAlternateStockList As AlternateStockItemList    'Added By Utkarsh ON 30-Apr-2012 FOR ALLIssue30042012
    Public mName As String = String.Empty                   'Added By Utkarsh ON 11-Oct-2012 FOR ALL11102012
    Public mPendingToReturnItemsRemovedFromAircraft As PendingToReturnItemsRemovedFromAircraft 'Added By Vikrant On 16-July-2013 For ALL10072013
    Dim ReceiptItemStoreCollection As Dictionary(Of Guid, Guid)
    Dim IssueToDiscardAsExpired As String = "0"
    Dim mFileAttach As FileAttach
    Dim ItemPrimaryCategory As Integer = 0 'Added By Vikrant For Issue Tools Transaction
    Public mUserHasNoStoreRights As UserHasNoStoreRights
    Public mCategoryList As CategoryList 'Added By Vikrant On 26-Nov-2018 For APFT26112018
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mIssue = CType(Session("mIssue"), Issue)
        mStockItemList = CType(Session("mStockItemList"), PendingToIssueItemList)
        mPendingItemList = CType(Session("mPendingItemList"), PendingToIssueList)
        mAlternateStockList = Session("mAlternateStockList") 'Added By Utkarsh ON 30-Apr-2012 FOR ALLIssue30042012
        mOrder = CType(Session("mOrder"), Order)
        PartNo = Session("PartNo")
        If mIssue Is Nothing Then
            'do nothing
        Else
            If mIssue.TransTypeID = 18 Then
                LinkID = Session("mLinkID").ToString
            End If
        End If
        'Added By Vikrant On 16-July-2013 For ALL10072013
        mPendingToReturnItemsRemovedFromAircraft = CType(Session("mPendingToReturnItemsRemovedFromAircraft"), PendingToReturnItemsRemovedFromAircraft)
        'End
        ReceiptItemStoreCollection = Session("ReceiptItemStoreCollection")
        IssueToDiscardAsExpired = Session("IssueToDiscardAsExpired")
        ItemPrimaryCategory = IIf(Session("ItemPrimaryCategory") Is Nothing, 0, Session("ItemPrimaryCategory")) 'Added By Vikrant For Issue Tools Transaction
        mCategoryList = Session("mCategoryList") 'Added By Vikrant On 26-Nov-2018 For APFT26112018
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub setLandingRate() 'Added By Vikrant On 27-Oct-2014 For ALL27102014-2	
        Dim Report As New ReportData("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", )
        If mPendingItemList.Count > 0 Then
            dgPendingItemList.Columns(15).HeaderText = "Landing Rate(" + Report.CurrencySymbol + ")" '18=>15
        Else
            dgPendingItemList.Columns(15).HeaderText = "Landing Rate" '18=>15
        End If
        If Not mAlternateStockList Is Nothing Then
            If mAlternateStockList.Count > 0 Then
                dgAlternateStockList.Columns(14).HeaderText = "Landing Rate(" + Report.CurrencySymbol + ")" '17=>14
            Else
                dgAlternateStockList.Columns(14).HeaderText = "Landing Rate" '17=>14
            End If
        End If
    End Sub 'End
    Private Sub Method()
        Session("CheckQty") = "False"
        Session.Remove("mStockItemList")
        Session.Remove("mPendingItemList")
        Session.Remove("mAlternateStockList")
        Session.Remove("PartNo")
        Session("Edit") = False
        Session("IsRemovedAsReturnableFromAircraft") = False 'Added By Vikrant On 16-July-2013 For ALL10072013
        Session.Remove("ItemPrimaryCategory") 'Added By Vikrant For Issue Tools Transaction
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Expired" Then
                        Try
                            Session("Sender") = ""
                            mIndex2 = Session("Index2")
                            If Session("IsAlternatePart") = "True" Then
                                If mAlternateStockList.Count > 0 Then
                                    If mAlternateStockList(mIndex2).CountOfComponentReservationItem > 0 Then 'Added By Prashant 2-Dec-2021 BA29112021 Then
                                        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "This component is reserved for Aircraft " + mAlternateStockList(mIndex2).ReservedComponentRegNo + " Dated " + mAlternateStockList(mIndex2).ReservedComponentDateFormatted + " as per schedule allocation. " + "<BR>Are you issuing it as per allocation?", MsgBoxStyle.YesNo, "ReservedComponent")
                                        Session("Index2") = mIndex2
                                        Session("ItemName") = mAlternateStockList(mIndex2).ItemName
                                        Session("Toshowsecondmessageboxonce") = "Toshowsecondmessageboxonce"
                                        MarkLog(Util.Action.Planned, "IssueToAircraft", "User has clicked on yes " + " component is reserved for Aircraft " + mAlternateStockList(mIndex2).ReservedComponentRegNo + " Dated " + mAlternateStockList(mIndex2).ReservedComponentDateFormatted, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                                        Exit Sub
                                    End If
                                End If
                            End If
                            If Session("IsAlternatePart") <> "True" Then
                                If mPendingItemList.Count > 0 Then
                                    If mPendingItemList(mIndex2).CountOfComponentReservationItem > 0 Then 'Added By Prashant 2-Dec-2021 BA29112021
                                        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "This component is reserved for Aircraft " + mPendingItemList(mIndex2).ReservedComponentRegNo + " Dated " + mPendingItemList(mIndex2).ReservedComponentDateFormatted + " as per schedule allocation. " + "<BR>Are you issuing it as per allocation?", MsgBoxStyle.YesNo, "ReservedComponent")
                                        Session("Index2") = mIndex2
                                        Session("ItemName") = mPendingItemList(mIndex2).ItemName
                                        Session("Toshowsecondmessageboxonce") = "Toshowsecondmessageboxonce"
                                        MarkLog(Util.Action.Planned, "IssueToAircraft", "User has clicked on yes " + " component is reserved for Aircraft " + mPendingItemList(mIndex2).ReservedComponentRegNo + " Dated " + mPendingItemList(mIndex2).ReservedComponentDateFormatted, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                                        Exit Sub
                                    End If
                                End If
                            End If

                            If Session("IsAlternatePart") = "True" Then
                                SetObject(mIndex2, True, AsPerAllocation:="Yes")
                            Else
                                SetObject(mIndex2, AsPerAllocation:="Yes")
                            End If
                            Method()
                            Session.Remove("Index2")
                            Session.Remove("ItemName")
                            Session.Remove("IsAlternatePart")
                            'Or mIssue.TransTypeID = Trans.IssueToolsToEmployee 'Added By Prashant on 17-May-2021 ALL17052021
                            If ((mIssue.TransTypeID = Trans.IssueToAircraft Or mIssue.TransTypeID = Trans.IssueToWorkShop Or _
                            mIssue.TransTypeID = Trans.IssueToolsToEmployee Or mIssue.TransTypeID = Trans.IssueToWorkOrderAsSpares) _
                            And mIssue.ToTypeID = 18) Then
                                Session("NewRequisition") = "True"
                            End If
                            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "ReservedComponent" Or MSGBoxCtrl.Sender = "SecondTimeMessage" Then
                        Try
                            Session("Sender") = ""
                            mIndex2 = Session("Index2")
                            If Session("IsAlternatePart") = "True" Then
                                If mAlternateStockList.Count > 0 And Session("Toshowsecondmessageboxonce") = "Toshowsecondmessageboxonce" Then
                                    If mIssue.IssueTo.Trim <> mAlternateStockList(mIndex2).ReservedComponentRegNo.Trim And mAlternateStockList(mIndex2).CountOfComponentReservationItem > 0 Then
                                        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "You are issuing it to " + mIssue.IssueTo.Trim + "<BR>Do you want to continue?", MsgBoxStyle.YesNo, "SecondTimeMessage")
                                        Session.Remove("Toshowsecondmessageboxonce")
                                        Session("Index2") = mIndex2
                                        Session("ItemName") = mAlternateStockList(mIndex2).ItemName
                                        MarkLog(Util.Action.Planned, "IssueToAircraft", "User has clicked on yes " + " component is reserved for Aircraft " + mAlternateStockList(mIndex2).ReservedComponentRegNo + " Dated " + mAlternateStockList(mIndex2).ReservedComponentDateFormatted + " Issued to:- " + mIssue.IssueTo.Trim, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                                        Exit Sub
                                    End If
                                End If
                            End If
                            If Session("IsAlternatePart") <> "True" Then
                                If mPendingItemList.Count > 0 And Session("Toshowsecondmessageboxonce") = "Toshowsecondmessageboxonce" Then
                                    If mIssue.IssueTo.Trim <> mPendingItemList(mIndex2).ReservedComponentRegNo.Trim And mPendingItemList(mIndex2).CountOfComponentReservationItem > 0 Then
                                        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "You are issuing it to " + mIssue.IssueTo.Trim + "<BR>Do you want to continue?", MsgBoxStyle.YesNo, "SecondTimeMessage")
                                        Session.Remove("Toshowsecondmessageboxonce")
                                        Session("Index2") = mIndex2
                                        Session("ItemName") = mPendingItemList(mIndex2).ItemName
                                        MarkLog(Util.Action.Planned, "IssueToAircraft", "User has clicked on yes " + " component is reserved for Aircraft " + mPendingItemList(mIndex2).ReservedComponentRegNo + " Dated " + mPendingItemList(mIndex2).ReservedComponentDateFormatted + " Issued to:- " + mIssue.IssueTo.Trim, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                                        Exit Sub
                                    End If
                                End If
                            End If
                            If Session("IsAlternatePart") = "True" Then
                                SetObject(mIndex2, True, AsPerAllocation:="Yes")
                            Else
                                SetObject(mIndex2, AsPerAllocation:="Yes")
                            End If
                            Method()
                            Session.Remove("Index2")
                            Session.Remove("ItemName")
                            Session.Remove("IsAlternatePart")
                            'Or mIssue.TransTypeID = Trans.IssueToolsToEmployee 'Added By Prashant on 17-May-2021 ALL17052021
                            If ((mIssue.TransTypeID = Trans.IssueToAircraft Or mIssue.TransTypeID = Trans.IssueToWorkShop Or _
                            mIssue.TransTypeID = Trans.IssueToolsToEmployee Or mIssue.TransTypeID = Trans.IssueToWorkOrderAsSpares) _
                            And mIssue.ToTypeID = 18) Then
                                Session("NewRequisition") = "True"
                            End If
                            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                        'Added By Vikrant On 03-Feb-2016 For ALL03022016
                    ElseIf MSGBoxCtrl.Sender = "SelectAllParts" Then
                        Session("IsAllPartsSelected") = True
                        Dim FirstItem As Integer = 0
                        mPendingItemList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, , , , , , mIssue.IDate.ToString, mIssue.TransTypeID, , , _
                                                                                    chkShowBERPart.Checked, IsAllPartsRequired:=True, _
                                                                                    IssueToDiscardAsExpired:=CInt(IssueToDiscardAsExpired), _
                                                                                    ItemPrimaryCategory:=ItemPrimaryCategory, CodeNo:=Trim(txtGSENo.Text), _
                                                                                    ToTypeIDOfIssue:=mIssue.ToTypeID, CategoryID:=cmbCategory.SelectedValue) ' ItemPrimaryCategory Added By Vikrant For Issue Tools Transaction
                        For i As Integer = 0 To mPendingItemList.Count - 1
                            If Not mIssue.IssueItems.Contains(mPendingItemList(i).ReceiptItemID) Then
                                If FirstItem < 500 Then
                                    If FirstItem = 0 Then 'For First Item directly SetObject
                                        SetObjectForAllParts(i)
                                    Else 'For All Other Items First Add New Child then SetObject
                                        mIssue.IssueItems.Add(mIssue.ID, mIssue.TransTypeID)
                                        mIssue.IssueItems.CurrentIndex = mIssue.IssueItems.Count - 1
                                        mIssue.IssueItems.CurrentItem.SRNo = mIssue.IssueItems.CurrentIndex + 1
                                        SetObjectForAllParts(i)
                                    End If
                                    FirstItem = FirstItem + 1
                                Else
                                    Exit For
                                End If
                            End If
                        Next
                        Method()
                        Session("mIssue") = mIssue
                        mIssue.CalculateTotal()
                        Response.Redirect("wfIssue_Ajax.aspx")
                        'End
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Expired" Or MSGBoxCtrl.Sender = "SecondTimeMessage" Then
                        Session("sender") = ""
                        Response.Redirect("wfPartStockStatus_Ajax.aspx?ChildPage=" & Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
                    End If
                    If MSGBoxCtrl.Sender = "ReservedComponent" Then
                        Try
                            Session("Sender") = ""
                            mIndex2 = Session("Index2")
                            If Session("IsAlternatePart") = "True" Then
                                SetObject(mIndex2, True, AsPerAllocation:="No")
                                MarkLog(Util.Action.Planned, "IssueToAircraft", "User has clicked on No " + " component is reserved for Aircraft " + mAlternateStockList(mIndex2).ReservedComponentRegNo + " Dated " + mAlternateStockList(mIndex2).ReservedComponentDateFormatted + " Issued to:- " + mIssue.IssueTo.Trim, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                            Else
                                SetObject(mIndex2, AsPerAllocation:="No")
                                MarkLog(Util.Action.Planned, "IssueToAircraft", "User has clicked on No " + " component is reserved for Aircraft " + mPendingItemList(mIndex2).ReservedComponentRegNo + " Dated " + mPendingItemList(mIndex2).ReservedComponentDateFormatted + " Issued to:- " + mIssue.IssueTo.Trim, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                            End If
                            Method()
                            Session.Remove("Index2")
                            Session.Remove("ItemName")
                            Session.Remove("IsAlternatePart")
                            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
            End Select
        End If
    End Sub
    Private Sub SetObjectForAllParts(ByVal Index As Int32) 'Added By Vikrant On 03-Feb-2016 For ALL03022016
        mIssue.IssueItems.CurrentItem.ReceiptItemID = mPendingItemList(Index).ReceiptItemID
        If mPendingItemList(Index).IsSerialized Or Request.QueryString("ChildPage") = "wfToolsCheckOut_Ajax.aspx" Then ' or condition Added By Vikrant For Issue Tools Transaction
            mIssue.IssueItems.CurrentItem.DisplayQty = 1   'Added By Prashant  12-May-2010     
            Session("AvailableQuantity") = 1
            Session("SerialNo") = mPendingItemList(Index).SerialNo
        Else
            mIssue.IssueItems.CurrentItem.DisplayQty = mPendingItemList(Index).AvailableQuantity 'Added By Prashant  12-May-2010   
            Session("SerialNo") = mPendingItemList(Index).SerialNo
        End If
        'Added By Prashant 3-July-2011 Once StoreID set for Issue for transaction 49,51,58 then do not set agin
        If mIssue.TransTypeID = Flypal.Util.Trans.IssuetoSupplierasRentalLease Or _
            mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoCustomer Or _
            mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoSupplier Or _
            mIssue.TransTypeID = Flypal.Util.Trans.IssueToWorkOrderAsSpares Or _
            mIssue.TransTypeID = Flypal.Util.Trans.IssueToWorkOrderAsTools Or _
            mIssue.TransTypeID = Flypal.Util.Trans.LoanReturnToStore Or _
            mIssue.TransTypeID = Flypal.Util.Trans.IssueToCustomerAsRepairedReturn Or _
            ((mIssue.TransTypeID = Flypal.Util.Trans.IssueToAircraft Or mIssue.TransTypeID = Util.Trans.IssueToWorkShop Or mIssue.TransTypeID = Util.Trans.IssueToolsToEmployee Or mIssue.TransTypeID = Util.Trans.IssueToWorkOrderAsSpares) And mIssue.ToTypeID = 18) Then   'Added By Saylee 27-Jan-2010  'Trans.IssueToRequisition Added by vikrant For New Requisition
            If mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoSupplier Or mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoCustomer Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToCustomerAsRepairedReturn Then
                If mIssue.StoreID.Equals(Guid.Empty) Then
                    mIssue.StoreID = mPendingItemList.Item(Index).StoreID
                End If
            Else
                mIssue.StoreID = mPendingItemList.Item(Index).StoreID
            End If
        End If

        If mIssue.ToTypeID = 18 Or mIssue.TransTypeID = 59 Or mIssue.TransTypeID = 60 Then 'Requisition,Issue To WO As Spares/Tools Transaction 
            mIssue.IssueItems.CurrentItem.DisplayUnitID = New Guid(Session("PendingIssuedQtyUnit").ToString)
            mIssue.IssueItems.CurrentItem.DisplayUnitName = mPendingItemList(Index).UnitName
        Else
            'Commented and added by Prashant 18-Sep-2019
            'mIssue.IssueItems.CurrentItem.DisplayUnitID = mPendingItemList(Index).UnitID
            mIssue.IssueItems.CurrentItem.DisplayUnitID = mPendingItemList(Index).DisplayUnitID
            mIssue.IssueItems.CurrentItem.DisplayUnitName = mPendingItemList(Index).DisplayUnitName
        End If

        If (mIssue.TransTypeID = 19) Then 'Added By Prashant On 18-Jul-2016
            mIssue.IssueItems.CurrentItem.DiscardAmt = mPendingItemList(index:=Index).EffRate
        End If
        mIssue.IssueItems.CurrentItem.ItemTagID = mPendingItemList(index:=Index).ItemTagID
        mIssue.IssueItems.CurrentItem.ItemTagName = mPendingItemList(index:=Index).ItemTagName
        mIssue.IssueItems.CurrentItem.StatusKit = mPendingItemList(index:=Index).StatusKit
        Session("mIssue") = mIssue
    End Sub 'End
    Private Sub SetObject(ByVal Index As Int32, Optional ByVal IsAlternatePart As Boolean = False, Optional ByVal AsPerAllocation As String = "") 'Changed By Utkarsh On 02-May-2012 FOR ALLIssue30042012
        If IsAlternatePart = False Then                                                             'Changed By Utkarsh On 02-May-2012 FOR ALLIssue30042012
            If Not mIssue Is Nothing Then
                mIssue.IssueItems.CurrentItem.ReceiptItemID = mPendingItemList(Index).ReceiptItemID
                If AsPerAllocation = "Yes" Then 'Added By Prashant 2-Dec-2021 BA29112021
                    mIssue.IssueItems.CurrentItem.IsAsPerAllocation = True
                ElseIf AsPerAllocation = "No" Then
                    mIssue.IssueItems.CurrentItem.IsAsPerAllocation = False
                End If
                If mPendingItemList(Index).IsSerialized Or Request.QueryString("ChildPage") = "wfToolsCheckOut_Ajax.aspx" Then 'Added By Vikrant For Issue Tools Transaction
                    mIssue.IssueItems.CurrentItem.DisplayQty = 1   'Added By Prashant  12-May-2010     
                    Session("AvailableQuantity") = 1
                    Session("SerialNo") = mPendingItemList(Index).SerialNo
                    'Added By Vikrant On 21-Dec-2016 For ALL21122016-1
                    If mPendingItemList(Index).CalibrationDueDateFormatted.ToString <> "" Then
                        mIssue.IssueItems.CurrentItem.CalibrationDueDate = mPendingItemList(Index).CalibrationDueDateFormatted.ToString
                    Else
                        mIssue.IssueItems.CurrentItem.CalibrationDueDate = System.DBNull.Value
                    End If
                    'End

                    ' '--Added by Saylee on 9-Mar-2021 for Heligo10032021
                    If mPendingItemList(Index).ManufacturingDateFormatted.ToString <> "" Then
                        mIssue.IssueItems.CurrentItem.ManufacturingDate = mPendingItemList(Index).ManufacturingDateFormatted.ToString
                    Else
                        mIssue.IssueItems.CurrentItem.ManufacturingDate = System.DBNull.Value
                    End If
                    '*****************************
                Else
                    If mIssue.ToTypeID = 18 Or mIssue.TransTypeID = 59 Or mIssue.TransTypeID = 60 Then 'Requisition,Issue To WO As Spares/Tools Transaction  'New Code
                        If mPendingItemList(Index).UnitID.Equals(New Guid(Session("PendingIssuedQtyUnit").ToString)) Then 'Existing Code
                            If CType(Session("PendingIssuedQty"), Decimal) > mPendingItemList(Index).AvailableQuantity Then
                                mIssue.IssueItems.CurrentItem.DisplayQty = mPendingItemList(Index).AvailableQuantity 'Added By Prashant  12-May-2010   
                                Session("AvailableQuantity") = mPendingItemList(Index).AvailableQuantity
                            Else
                                'Added by Saylee on 8-Dec-2010
                                mIssue.IssueItems.CurrentItem.DisplayQty = CType(Session("PendingIssuedQty"), Decimal)
                                Session("AvailableQuantity") = CType(Session("RequiredQty"), Decimal)
                            End If
                        Else  'Added By Vikrant On 08-May-2019 For BA07052019 
                            Dim mUnitConverterList As UnitConverterList = UnitConverterList.GetUnitConverterList(mPendingItemList(Index).ItemID)
                            Dim Factor As Decimal = 0

                            If Not mUnitConverterList Is Nothing Then
                                Factor = mUnitConverterList.UnitConverterFactor(mPendingItemList(Index).UnitID, New Guid(Session("PendingIssuedQtyUnit").ToString))
                            End If
                            If CType(Session("PendingIssuedQty"), Decimal) > (IIf(Factor > 0, mPendingItemList(Index).AvailableQuantity * Factor, mPendingItemList(Index).AvailableQuantity)) Then
                                mIssue.IssueItems.CurrentItem.DisplayQty = (IIf(Factor > 0, mPendingItemList(Index).AvailableQuantity * Factor, mPendingItemList(Index).AvailableQuantity)) 'Added By Prashant  12-May-2010   
                                Session("AvailableQuantity") = mPendingItemList(Index).AvailableQuantity
                            Else
                                'Added by Saylee on 8-Dec-2010
                                mIssue.IssueItems.CurrentItem.DisplayQty = CType(Session("PendingIssuedQty"), Decimal)
                                Session("AvailableQuantity") = CType(Session("RequiredQty"), Decimal)
                            End If
                        End If
                    Else 'Existing Code
                        If CType(Session("PendingIssuedQty"), Decimal) > mPendingItemList(Index).AvailableQuantity Then
                            mIssue.IssueItems.CurrentItem.DisplayQty = mPendingItemList(Index).AvailableQuantity 'Added By Prashant  12-May-2010   
                            Session("AvailableQuantity") = mPendingItemList(Index).AvailableQuantity
                        Else
                            'Added by Saylee on 8-Dec-2010
                            mIssue.IssueItems.CurrentItem.DisplayQty = CType(Session("PendingIssuedQty"), Decimal)
                            Session("AvailableQuantity") = CType(Session("RequiredQty"), Decimal)
                        End If
                    End If


                    Session("SerialNo") = mPendingItemList(Index).SerialNo
                End If
                'Added By Prashant 3-July-2011 Once StoreID set for Issue for transaction 49,51,58 then do not set agin
                If mIssue.TransTypeID = Flypal.Util.Trans.IssuetoSupplierasRentalLease Or _
                    mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoCustomer Or _
                    mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoSupplier Or _
                    mIssue.TransTypeID = Flypal.Util.Trans.IssueToWorkOrderAsSpares Or _
                    mIssue.TransTypeID = Flypal.Util.Trans.IssueToWorkOrderAsTools Or _
                    mIssue.TransTypeID = Flypal.Util.Trans.LoanReturnToStore Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToCustomerAsRepairedReturn Or _
                    ((mIssue.TransTypeID = Flypal.Util.Trans.IssueToAircraft Or mIssue.TransTypeID = Util.Trans.IssueToWorkShop Or mIssue.TransTypeID = Util.Trans.IssueToolsToEmployee Or mIssue.TransTypeID = Util.Trans.IssueToWorkOrderAsSpares) And mIssue.ToTypeID = 18) Then   'Added By Saylee 27-Jan-2010  'Trans.IssueToRequisition Added by vikrant For New Requisition
                    If mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoSupplier Or mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoCustomer Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToCustomerAsRepairedReturn Then
                        If mIssue.StoreID.Equals(Guid.Empty) Then
                            mIssue.StoreID = mPendingItemList.Item(Index).StoreID
                        End If
                    Else
                        mIssue.StoreID = mPendingItemList.Item(Index).StoreID
                    End If
                End If
                If mIssue.ToTypeID = 18 Or mIssue.TransTypeID = 59 Or mIssue.TransTypeID = 60 Then 'Requisition,Issue To WO As Spares/Tools Transaction 
                    mIssue.IssueItems.CurrentItem.DisplayUnitID = New Guid(Session("PendingIssuedQtyUnit").ToString)
                Else
                    'Commented and added by Prashant 18-Sep-2019
                    'mIssue.IssueItems.CurrentItem.DisplayUnitID = mPendingItemList(Index).UnitID
                    mIssue.IssueItems.CurrentItem.DisplayUnitID = mPendingItemList(Index).DisplayUnitID
                    mIssue.IssueItems.CurrentItem.DisplayUnitName = mPendingItemList(Index).DisplayUnitName
                End If

                'If (mIssue.TransTypeID = 14 And mPendingItemList(Index).PrimaryCategoryID = 1) Then 'Added By Prashant On 12-Apr-2016 For ALL12042016
                '    mIssue.IssueItems.CurrentItem.IsReturnableFromAircraft = True
                'End If
                If (mIssue.TransTypeID = 19) Then 'Added By Prashant On 18-Jul-2016
                    mIssue.IssueItems.CurrentItem.DiscardAmt = mPendingItemList(index:=Index).EffRate
                End If
                mIssue.IssueItems.CurrentItem.ItemTagID = mPendingItemList(index:=Index).ItemTagID
                mIssue.IssueItems.CurrentItem.ItemTagName = mPendingItemList(index:=Index).ItemTagName
                mIssue.IssueItems.CurrentItem.StatusKit = mPendingItemList(index:=Index).StatusKit
                mIssue.IssueItems.CurrentItem.CodeNo = mPendingItemList(index:=Index).CodeNo 'Added By Vikrant On 21-Dec-2016 For ALL21122016-1
                mIssue.IssueItems.CurrentItem.Location = mPendingItemList(index:=Index).ReceiptItemBinLocation   'Added by Shital   on 25-Sep-2020s
                Session("mIssue") = mIssue
            End If
            If Not mOrder Is Nothing Then
                mOrder.OrderItems.CurrentItem.ReceiptItemID = mPendingItemList(Index).ReceiptItemID
                mOrder.OrderItems.CurrentItem.ItemFrom = FromOrder.PreviousTrans.FromStock
                mOrder.OrderItems.CurrentItem.FromNo = mOrder.OrderItems.CurrentItem.ReceiptItemDetailForOrder.ReceiptTextNo
                mOrder.OrderItems.CurrentItem.FromDate = mOrder.OrderItems.CurrentItem.ReceiptItemDetailForOrder.ReceiptDate.ToString
                'Added By Prashant 29-Aug-2013  ALL29082013
                mOrder.OrderItems.CurrentItem.Qty = mPendingItemList(Index).AvailableQuantity
                mOrder.OrderItems.CurrentItem.SerialNo = mPendingItemList(Index).SerialNo
                If mOrder.TransTypeID = Flypal.Util.Trans.PurchaseOrderForExchangeRepair Then
                    mOrder.OrderItems.CurrentItem.IsInWarranty = mPendingItemList(Index).IsWarranty
                    mOrder.OrderItems.CurrentItem.WarrantyInDays = mPendingItemList(Index).WarrantyInDays
                    mOrder.OrderItems.CurrentItem.WarrantyStartDate = mPendingItemList(Index).WarrantyStartDate.ToString
                    mOrder.OrderItems.CurrentItem.WarrantyExpiryDate = mPendingItemList(Index).WarrantyExpiryDate.ToString
                End If
                ''Added By Saylee on 17-Oct-2012    'ALL15102012
                Session("IsSelectedForAutoIssue") = True 'Used in PurchaseOrder for Auto Creation of Issue for Repair/OH/Exchange
                Session("StoreIDForAutoIssue") = mPendingItemList(Index).StoreID
                mOrder.OrderItems.CurrentItem.StoreID = mPendingItemList(Index).StoreID   'Added By Prashant 19-Dec-2013 For Auto Issue Creation
                If (mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mPendingItemList(Index).TechDirectionCount <> 0 Then
                    mOrder.OrderItems.CurrentItem.CompStatusID = mPendingItemList(Index).CompStatusID
                    mOrder.OrderItems.CurrentItem.MachineID = mPendingItemList(Index).MachineID
                    mOrder.OrderItems.CurrentItem.TechDirectionDate = mPendingItemList(Index).TechDirectionDate
                    mOrder.OrderItems.CurrentItem.TechDirectionCount = mPendingItemList(Index).TechDirectionCount
                End If
                Session("mOrder") = mOrder
            End If
        Else
            If Not mIssue Is Nothing Then
                mIssue.IssueItems.CurrentItem.ReceiptItemID = mAlternateStockList(Index).ReceiptItemID
                If AsPerAllocation = "Yes" Then 'Added By Prashant 2-Dec-2021 BA29112021
                    mIssue.IssueItems.CurrentItem.IsAsPerAllocation = True
                ElseIf AsPerAllocation = "No" Then
                    mIssue.IssueItems.CurrentItem.IsAsPerAllocation = False
                End If
                If mAlternateStockList(Index).IsSerialized Or Request.QueryString("ChildPage") = "wfToolsCheckOut_Ajax.aspx" Then 'Added By Vikrant For Issue Tools Transaction
                    mIssue.IssueItems.CurrentItem.DisplayQty = 1
                    Session("AvailableQuantity") = 1
                    Session("SerialNo") = mAlternateStockList(Index).SerialNo
                    'Added By Vikrant On 21-Dec-2016 For ALL21122016-1
                    If mAlternateStockList(Index).CalibrationDueDateFormatted.ToString <> "" Then
                        mIssue.IssueItems.CurrentItem.CalibrationDueDate = mAlternateStockList(Index).CalibrationDueDateFormatted.ToString
                    Else
                        mIssue.IssueItems.CurrentItem.CalibrationDueDate = System.DBNull.Value
                    End If
                    'End

                    ' '--Added by Saylee on 9-Mar-2021 for Heligo10032021
                    If mAlternateStockList(Index).ManufacturingDateFormatted.ToString <> "" Then
                        mIssue.IssueItems.CurrentItem.ManufacturingDate = mAlternateStockList(Index).ManufacturingDateFormatted.ToString
                    Else
                        mIssue.IssueItems.CurrentItem.ManufacturingDate = System.DBNull.Value
                    End If
                    '*****************************

                Else
                    If mIssue.ToTypeID = 18 Or mIssue.TransTypeID = 59 Or mIssue.TransTypeID = 60 Then 'Requisition,Issue To WO As Spares/Tools Transaction 
                        If mAlternateStockList(Index).UnitID.Equals(New Guid(Session("PendingIssuedQtyUnit").ToString)) Then 'Existing Code
                            If CType(Session("PendingIssuedQty"), Decimal) > mAlternateStockList(Index).AvailableQuantity Then
                                mIssue.IssueItems.CurrentItem.DisplayQty = mAlternateStockList(Index).AvailableQuantity 'Added By Prashant  12-May-2010   
                                Session("AvailableQuantity") = mAlternateStockList(Index).AvailableQuantity
                            Else
                                'Added by Saylee on 8-Dec-2010
                                mIssue.IssueItems.CurrentItem.DisplayQty = CType(Session("PendingIssuedQty"), Decimal)
                                Session("AvailableQuantity") = CType(Session("RequiredQty"), Decimal)
                            End If
                        Else  'Added By Vikrant On 08-May-2019 For BA07052019 
                            Dim mUnitConverterList As UnitConverterList = UnitConverterList.GetUnitConverterList(mAlternateStockList(Index).ItemID)
                            Dim Factor As Decimal = 0

                            If Not mUnitConverterList Is Nothing Then
                                Factor = mUnitConverterList.UnitConverterFactor(mAlternateStockList(Index).UnitID, New Guid(Session("PendingIssuedQtyUnit").ToString))
                            End If
                            If CType(Session("PendingIssuedQty"), Decimal) > (IIf(Factor > 0, mAlternateStockList(Index).AvailableQuantity * Factor, mAlternateStockList(Index).AvailableQuantity)) Then
                                mIssue.IssueItems.CurrentItem.DisplayQty = (IIf(Factor > 0, mAlternateStockList(Index).AvailableQuantity * Factor, mAlternateStockList(Index).AvailableQuantity)) 'Added By Prashant  12-May-2010   
                                Session("AvailableQuantity") = mAlternateStockList(Index).AvailableQuantity
                            Else
                                'Added by Saylee on 8-Dec-2010
                                mIssue.IssueItems.CurrentItem.DisplayQty = CType(Session("PendingIssuedQty"), Decimal)
                                Session("AvailableQuantity") = CType(Session("RequiredQty"), Decimal)
                            End If
                        End If
                    Else 'Existing Code
                        If CType(Session("PendingIssuedQty"), Decimal) > mAlternateStockList(Index).AvailableQuantity Then
                            mIssue.IssueItems.CurrentItem.DisplayQty = mAlternateStockList(Index).AvailableQuantity 'Added By Prashant  12-May-2010   
                            Session("AvailableQuantity") = mAlternateStockList(Index).AvailableQuantity
                        Else
                            'Added by Saylee on 8-Dec-2010
                            mIssue.IssueItems.CurrentItem.DisplayQty = CType(Session("PendingIssuedQty"), Decimal)
                            Session("AvailableQuantity") = CType(Session("RequiredQty"), Decimal)
                        End If
                    End If
                    'Added By Saylee 1-Feb-2010

                    'End
                    Session("SerialNo") = mAlternateStockList(Index).SerialNo
                End If
                'Added By Prashant 3-July-2011 Once StoreID set for Issue for transaction 49,51,58 then do not set agin
                If mIssue.TransTypeID = Flypal.Util.Trans.IssuetoSupplierasRentalLease Or _
                    mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoCustomer Or _
                    mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoSupplier Or _
                    mIssue.TransTypeID = Flypal.Util.Trans.IssueToWorkOrderAsSpares Or _
                    mIssue.TransTypeID = Flypal.Util.Trans.IssueToWorkOrderAsTools Or _
                    mIssue.TransTypeID = Flypal.Util.Trans.LoanReturnToStore Or _
                    mIssue.TransTypeID = Flypal.Util.Trans.IssueToCustomerAsRepairedReturn Or _
                    ((mIssue.TransTypeID = Flypal.Util.Trans.IssueToAircraft Or mIssue.TransTypeID = Util.Trans.IssueToWorkShop Or mIssue.TransTypeID = Util.Trans.IssueToolsToEmployee Or mIssue.TransTypeID = Util.Trans.IssueToWorkOrderAsSpares) And mIssue.ToTypeID = 18) Then   'Added By Saylee 27-Jan-2010
                    If mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoSupplier Or mIssue.TransTypeID = Flypal.Util.Trans.IssueforLoanReturntoCustomer Or mIssue.TransTypeID = Flypal.Util.Trans.IssueToCustomerAsRepairedReturn Then
                        If mIssue.StoreID.Equals(Guid.Empty) Then
                            mIssue.StoreID = mAlternateStockList.Item(Index).StoreID
                        End If
                    Else
                        mIssue.StoreID = mAlternateStockList.Item(Index).StoreID
                    End If
                End If
                If mIssue.ToTypeID = 18 Or mIssue.TransTypeID = 59 Or mIssue.TransTypeID = 60 Then 'Requisition,Issue To WO As Spares/Tools Transaction 
                    mIssue.IssueItems.CurrentItem.DisplayUnitID = New Guid(Session("PendingIssuedQtyUnit").ToString)
                Else
                    'mIssue.IssueItems.CurrentItem.DisplayUnitID = mAlternateStockList(Index).UnitID
                    mIssue.IssueItems.CurrentItem.DisplayUnitID = mAlternateStockList(Index).DisplayUnitID
                    mIssue.IssueItems.CurrentItem.DisplayUnitName = mAlternateStockList(Index).DisplayUnitName
                End If

                'If (mIssue.TransTypeID = 14 And mAlternateStockList(Index).PrimaryCategoryID = 1) Then 'Added By Prashant On 12-Apr-2016 For ALL12042016
                '    mIssue.IssueItems.CurrentItem.IsReturnableFromAircraft = True
                'End If
                If (mIssue.TransTypeID = 19) Then 'Added By Prashant On 18-Jul-2016
                    mIssue.IssueItems.CurrentItem.DiscardAmt = mAlternateStockList(index:=Index).EffRate
                End If
                mIssue.IssueItems.CurrentItem.ItemTagID = mAlternateStockList(index:=Index).ItemTagID
                mIssue.IssueItems.CurrentItem.ItemTagName = mAlternateStockList(index:=Index).ItemTagName
                mIssue.IssueItems.CurrentItem.StatusKit = mAlternateStockList(index:=Index).StatusKit
                mIssue.IssueItems.CurrentItem.CodeNo = mAlternateStockList(index:=Index).CodeNo 'Added By Vikrant On 21-Dec-2016 For ALL21122016-1
                mIssue.IssueItems.CurrentItem.Location = mAlternateStockList(index:=Index).ReceiptItemBinLocation   'Added by Shital   on 25-Sep-2020s
                Session("mIssue") = mIssue
            End If
            If Not mOrder Is Nothing Then
                mOrder.OrderItems.CurrentItem.ReceiptItemID = mAlternateStockList(Index).ReceiptItemID
                mOrder.OrderItems.CurrentItem.ItemFrom = FromOrder.PreviousTrans.FromStock
                mOrder.OrderItems.CurrentItem.FromNo = mOrder.OrderItems.CurrentItem.ReceiptItemDetailForOrder.ReceiptTextNo
                mOrder.OrderItems.CurrentItem.FromDate = mOrder.OrderItems.CurrentItem.ReceiptItemDetailForOrder.ReceiptDate.ToString
                'Added By Prashant 29-Aug-2013  ALL29082013
                mOrder.OrderItems.CurrentItem.Qty = mAlternateStockList(Index).AvailableQuantity
                mOrder.OrderItems.CurrentItem.SerialNo = mAlternateStockList(Index).SerialNo
                If mOrder.TransTypeID = Flypal.Util.Trans.PurchaseOrderForExchangeRepair Then
                    mOrder.OrderItems.CurrentItem.IsInWarranty = mAlternateStockList(Index).IsWarranty
                    mOrder.OrderItems.CurrentItem.WarrantyInDays = mAlternateStockList(Index).WarrantyInDays
                    mOrder.OrderItems.CurrentItem.WarrantyStartDate = mAlternateStockList(Index).WarrantyStartDate.ToString
                    mOrder.OrderItems.CurrentItem.WarrantyExpiryDate = mAlternateStockList(Index).WarrantyExpiryDate.ToString
                End If
                ''Added By Saylee on 17-Oct-2012    'ALL15102012
                Session("IsSelectedForAutoIssue") = True 'Used in PurchaseOrder for Auto Creation of Issue for Repair/OH/Exchange
                Session("StoreIDForAutoIssue") = mAlternateStockList(Index).StoreID
                mOrder.OrderItems.CurrentItem.StoreID = mAlternateStockList(Index).StoreID   'Added By Prashant 19-Dec-2013 For Auto Issue Creation
                Session("mOrder") = mOrder
            End If
        End If
    End Sub
    'Added By Utkarsh On 02-May-2012 FOR ALLIssue30042012
    Private Sub ControlVisibility()
        If Not mAlternateStockList Is Nothing Then
            If mAlternateStockList.Count = 0 Then
                dgAlternateStockList.Visible = False
                lblResult2.Visible = False
                'dgAlternateStockList.Columns(15).HeaderText = "Landing Rate" 'Added By Vikrant On 27-Oct-2014 For ALL27102014-2	
            Else
                dgAlternateStockList.Visible = True
                lblResult2.Visible = True
                'dgAlternateStockList.Columns(15).HeaderText = mAlternateStockList(0).Symbol 'Added By Vikrant On 27-Oct-2014 For ALL27102014-2	
            End If
        Else
            dgAlternateStockList.Visible = False
            lblResult2.Visible = False
            'dgAlternateStockList.Columns(15).HeaderText = mAlternateStockList(0).Symbol 'Added By Vikrant On 27-Oct-2014 For ALL27102014-2	
        End If
        'Added By Vikrant On 16-July-2013 For ALL10072013
        If Not mIssue Is Nothing Then
            ControlVisibilityForAircraftRemovedReturnableItems()
        End If
        'End

        'Store Transfer and Discard Transaction
        'Added By Vikrant On 03-Feb-2016 For ALL03022016
        If Not mIssue Is Nothing Then
            btnSelectAllParts.Visible = IIf((mIssue.TransTypeID = 15 Or mIssue.TransTypeID = 19 Or mIssue.TransTypeID = 63), True, False)
            btnSelectAllParts.Enabled = IIf(Session("IsAllPartsSelected") = True, False, True)
        Else
            btnSelectAllParts.Visible = False
        End If
        'End
        If Not mIssue Is Nothing Then
            lblCodeNo.Visible = IIf(mIssue.TransTypeID = 79 And (AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA"), True, False)
            txtGSENo.Visible = IIf(mIssue.TransTypeID = 79 And (AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA"), True, False)
        Else
            lblCodeNo.Visible = False
            txtGSENo.Visible = False
        End If
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        If Not mIssue Is Nothing Then
            lblCategory.Visible = True
            cmbCategory.Visible = True
        Else
            lblCategory.Visible = False
            cmbCategory.Visible = False
        End If
        'End
    End Sub
    'End 
    'Added By Vikrant On 16-July-2013 For ALL10072013
    Private Sub ControlVisibilityForAircraftRemovedReturnableItems()
        If ((mIssue.TransTypeID = 14) And (Session("IsRemovedAsReturnableFromAircraft") = True)) Then
            dgRemovedAsReturnableFromAircraft.Visible = True
            lblResult3.Visible = True
            dgStockItemList.Visible = False
            lblResult.Visible = False
            'Added By Vikrant On 30-July-2013 For ALL10072013
            If Not mPendingToReturnItemsRemovedFromAircraft Is Nothing Then
                lblResult3.Text = "Removed As Returnable From Aircraft Items : " & mPendingToReturnItemsRemovedFromAircraft.Count & " Record(s) found."
            Else
                lblResult3.Text = "Removed As Returnable From Aircraft Items : 0 Record(s) found."
            End If
            'End
        Else
            dgStockItemList.Visible = True
            lblResult.Visible = True
            dgRemovedAsReturnableFromAircraft.Visible = False
            lblResult3.Visible = False
        End If
    End Sub
    'End
    Private Sub ReceiptItemAttachment(Optional ByVal ReceiptItemID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal Visibility As Integer = 0)
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
    End Sub
    'Added By Prashant 6-Jul-2020 All06072020
    Private Sub dgPendingItemList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgPendingItemList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).BackColor = System.Drawing.ColorTranslator.FromHtml("#" & e.Row.Cells(22).Text) '27=>22
            If CInt(e.Row.Cells(23).Text) > 0 Then 'CountOfComponentReservationItem '28=>23
                For Each cell As TableCell In e.Row.Cells
                    cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#9ae6ac")
                    lblGreen.Visible = True
                    lblGreenInfo.Visible = True
                Next
            End If
            e.Row.Cells(20).Enabled = CBool(e.Row.Cells(24).Text)  '25=>20  29=>24
            upnlColor.Update()
        End If
    End Sub
    Private Sub dgAlternateStockList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgAlternateStockList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).BackColor = System.Drawing.ColorTranslator.FromHtml("#" & e.Row.Cells(21).Text) '26=>21
            If CInt(e.Row.Cells(22).Text) > 0 Then 'CountOfComponentReservationItem 27=> 22
                For Each cell As TableCell In e.Row.Cells
                    cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#9ae6ac")
                    lblGreen.Visible = True
                    lblGreenInfo.Visible = True
                Next
            End If
            upnlColor.Update()
        End If
    End Sub
    'End of Added By Prashant 6-Jul-2020 All06072020
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgStockItemList.DataSource = mStockItemList
        dgPendingItemList.DataSource = mPendingItemList
        dgRemovedAsReturnableFromAircraft.DataSource = mPendingToReturnItemsRemovedFromAircraft 'Added By Vikrant On 16-July-2013 For ALL10072013
        setLandingRate()
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        If Not mIssue Is Nothing Then
            If mIssue.TransTypeID = Util.Trans.IssueToolsToEmployee Then
                mCategoryList = CategoryList.GetCategoryList("(All)", True)
                cmbCategory.DataSource = mCategoryList
            Else
                mCategoryList = CategoryList.GetCategoryList("(All)")
                cmbCategory.DataSource = mCategoryList
            End If
            Session("mCategoryLists") = mCategoryList
        End If
        'End
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then

            If txtSearch.Enabled = True Then
                setFocus(txtSearch)
            End If
            'Added By Utkarsh ON 11-Oct-2012 FOR ALL11102012
            If Not mIssue Is Nothing Then
                mName = Request.QueryString("Name")
                txtSearch.Text = mName
                If Request.QueryString("Name") Is Nothing And Session("ItemName") <> "" Then 'Added By Prashant 13-Nov-2019 as form is redirecting on same page in case of No click of message box
                    txtSearch.Text = Session("ItemName")
                End If
            ElseIf Not mOrder Is Nothing Then
                txtSearch.Text = PartNo
            End If

            'End
            If Not mIssue Is Nothing Then
                If mIssue.TransTypeID = Util.Trans.IssueToolsToEmployee Then 'If Open From Tools CheckOut Link then SHow Parts With Tools Primary Category  'Added By Vikrant For Issue Tools Transaction
                    ItemPrimaryCategory = 2
                    Session("ItemPrimaryCategory") = ItemPrimaryCategory
                End If

                txtSearch.Visible = True
                btnFindNow.Visible = True
                lblPartNo.Visible = True
                chkShowBERPart.Visible = IIf(mIssue.TransTypeID = Util.Trans.IssueToolsToEmployee, False, True) 'Condition Added By Vikrant For Issue Tools Transaction
                If (mIssue.TransTypeID = 59 Or mIssue.TransTypeID = 60 Or mIssue.TransTypeID = 18 Or mIssue.TransTypeID = 49 Or mIssue.TransTypeID = 55 Or mIssue.TransTypeID = 51 Or mIssue.TransTypeID = 58 Or ((mIssue.TransTypeID = 14 Or mIssue.TransTypeID = 44 Or mIssue.TransTypeID = 79) And mIssue.ToTypeID = 18)) Then 'Added By Prashant 20-Aug-2014 ALL20082014
                    txtSearch.Enabled = False
                    btnFindNow.Enabled = False
                    chkShowBERPart.Enabled = False
                    DueAtMessage.Visible = False
                End If
                'Added By Utkarsh ON 11-Oct-2012 FOR ALL11102012
                If txtSearch.Text.Trim.Length = 0 Then
                    mPendingItemList = PendingToIssueList.NewPendingToIssueList
                    mStockItemList = PendingToIssueItemList.NewPendingItemList
                    Session("mStockItemList") = mStockItemList
                    Session("mPendingItemList") = mPendingItemList
                    'Added By Vikrant On 16-July-2013 For ALL10072013
                    mPendingToReturnItemsRemovedFromAircraft = PendingToReturnItemsRemovedFromAircraft.NewPendingToReturnItemsRemovedFromAircraft
                    Session("mPendingToReturnItemsRemovedFromAircraft") = mPendingToReturnItemsRemovedFromAircraft
                    'lblResult3.Text = "Removed As Reurnable From Aircraft Items : " & mPendingToReturnItemsRemovedFromAircraft.Count & " Record(s) found."
                    'End
                    DataFieldBind()
                    lblResult.Text = "Part Stock Status List : " & mStockItemList.Count & " Record(s) found."
                    lblResult1.Text = "Stock Details : " & mPendingItemList.Count & " Record(s) found."
                    GoTo Visibility
                    Exit Sub
                End If
                'Added By Prashant 3-July-2011 to Show only respective Store records
                mStockItemList = PendingToIssueItemList.GetPendingItemList(mIssue.StoreID, txtSearch.Text, mIssue.IDate.ToString, mIssue.TransTypeID, chkShowBERPart.Checked, IssueToDiscardAsExpired:=CInt(IssueToDiscardAsExpired), ItemPrimaryCategory:=ItemPrimaryCategory, CodeNo:=Trim(txtGSENo.Text)) 'Added By Vikrant For Issue Tools Transaction
                'Added By Vikrant On 30-July-2013 For ALL10072013
                If ((mIssue.TransTypeID = 14) And (Session("IsRemovedAsReturnableFromAircraft") = True)) Then
                    mPendingToReturnItemsRemovedFromAircraft = PendingToReturnItemsRemovedFromAircraft.GetPendingToReturnItemsRemovedFromAircraft(mIssue.StoreID, mIssue.MachineID, txtSearch.Text.Trim, mIssue.IDate.ToString, mIssue.TransTypeID, chkShowBERPart.Checked)
                    Session("mPendingToReturnItemsRemovedFromAircraft") = mPendingToReturnItemsRemovedFromAircraft
                End If
                'End
                If mIssue.TransTypeID = 18 Then    'TransTypeID = 18 Added By Prashant 26-Sep-2011
                    mPendingItemList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, txtSearch.Text, , , , , mIssue.IDate.ToString, _
                                                                                CType(mIssue.TransTypeID, Flypal.Util.Trans), LinkID, , _
                                                                                chkShowBERPart.Checked, ItemPrimaryCategory:=ItemPrimaryCategory, _
                                                                                CodeNo:=Trim(txtGSENo.Text), ToTypeIDOfIssue:=mIssue.ToTypeID) 'Added By Vikrant For Issue Tools Transaction
                ElseIf ((mIssue.TransTypeID = 14) And (Session("IsRemovedAsReturnableFromAircraft") = True)) Then 'Added By Vikrant On 30-July-2013 For ALL10072013
                    mPendingItemList = PendingToIssueList.NewPendingToIssueList
                Else
                    mPendingItemList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, txtSearch.Text, , , , , mIssue.IDate.ToString, _
                                                                                CType(mIssue.TransTypeID, Flypal.Util.Trans), , , _
                                                                                chkShowBERPart.Checked, _
                                                                                IssueToDiscardAsExpired:=CInt(IssueToDiscardAsExpired), _
                                                                                ItemPrimaryCategory:=ItemPrimaryCategory, CodeNo:=Trim(txtGSENo.Text), _
                                                                                ToTypeIDOfIssue:=mIssue.ToTypeID) 'Added By Vikrant For Issue Tools Transaction
                End If
                Session("mStockItemList") = mStockItemList
                Session("mPendingItemList") = mPendingItemList
                DataFieldBind()
                lblResult.Text = "Part Stock Status List : " & mStockItemList.Count & " Record(s) found."
                lblResult1.Text = "Stock Details : " & mPendingItemList.Count & " Record(s) found."
            End If
            If Not mOrder Is Nothing Then
                txtSearch.Visible = False
                btnFindNow.Visible = False
                lblPartNo.Visible = False
                chkShowBERPart.Visible = False

                If Not mOrder.OrderItems.CurrentItem.ItemID.Equals(Guid.Empty) Then
                    mPendingItemList = PendingToIssueList.GetPendingToIssueList(Guid.Empty, txtSearch.Text, , , , , mOrder.OrderDate.ToString, _
                                                                                mOrder.TransTypeID, mOrder.OrderItems.CurrentItem.ItemID.ToString, , _
                                                                                chkShowBERPart.Checked, ItemPrimaryCategory:=ItemPrimaryCategory, _
                                                                                CodeNo:=Trim(txtGSENo.Text)) 'Added By Vikrant For Issue Tools Transaction
                    Session("mPendingItemList") = mPendingItemList
                    lblResult.Visible = False

                Else
                    mStockItemList = PendingToIssueItemList.GetPendingItemList(Guid.Empty, txtSearch.Text, mOrder.OrderDate.ToString, mOrder.TransTypeID, chkShowBERPart.Checked, ItemPrimaryCategory:=ItemPrimaryCategory, CodeNo:=Trim(txtGSENo.Text)) 'Added By Vikrant For Issue Tools Transaction
                    mPendingItemList = PendingToIssueList.GetPendingToIssueList(Guid.Empty, txtSearch.Text, , , , , mOrder.OrderDate.ToString, _
                                                                                CType(mOrder.TransTypeID, Flypal.Util.Trans), , , _
                                                                                chkShowBERPart.Checked, ItemPrimaryCategory:=ItemPrimaryCategory, _
                                                                                CodeNo:=Trim(txtGSENo.Text)) 'Added By Vikrant For Issue Tools Transaction
                End If
                Session("mStockItemList") = mStockItemList
                Session("mPendingItemList") = mPendingItemList
                DataFieldBind()
                lblResult1.Text = "Stock Details : " & mPendingItemList.Count & " Record(s) found."
            End If
            'If Trying to issue expired part to Aircraft, user will be pointed for confirmation. And user selects 'No' 
            If Not mIssue Is Nothing Then
                If (mIssue.TransTypeID = 14) And (Not Session("Index2") Is Nothing) Then
                    mItemName = Session("ItemName")
                    'Added By Utkarsh ON 11-Oct-2012 FOR ALL11102012
                    If txtSearch.Text.Trim.Length = 0 Then
                        mPendingItemList = PendingToIssueList.NewPendingToIssueList
                        mStockItemList = PendingToIssueItemList.NewPendingItemList
                        Session("mStockItemList") = mStockItemList
                        Session("mPendingItemList") = mPendingItemList
                        'Added By Vikrant On 16-July-2013 For ALL10072013
                        mPendingToReturnItemsRemovedFromAircraft = PendingToReturnItemsRemovedFromAircraft.NewPendingToReturnItemsRemovedFromAircraft
                        Session("mPendingToReturnItemsRemovedFromAircraft") = mPendingToReturnItemsRemovedFromAircraft
                        'lblResult3.Text = "Removed As Reurnable From Aircraft Items : " & mPendingToReturnItemsRemovedFromAircraft.Count & " Record(s) found."
                        'End
                        DataFieldBind()
                        lblResult.Text = "Part Stock Status List : " & mStockItemList.Count & " Record(s) found."
                        lblResult1.Text = "Stock Details : " & mPendingItemList.Count & " Record(s) found."
                        GoTo Visibility
                        Exit Sub
                    End If
                    'End
                    If Not mIssue Is Nothing Then
                        mPendingItemList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, mItemName, , , , , mIssue.IDate.ToString, _
                                                                                    mIssue.TransTypeID, , , chkShowBERPart.Checked, _
                                                                                    IssueToDiscardAsExpired:=CInt(IssueToDiscardAsExpired), _
                                                                                    ItemPrimaryCategory:=ItemPrimaryCategory, CodeNo:=Trim(txtGSENo.Text), _
                                                                                    ToTypeIDOfIssue:=mIssue.ToTypeID) 'Added By Vikrant For Issue Tools Transaction
                    End If
                    If Not mOrder Is Nothing Then
                        mPendingItemList = PendingToIssueList.GetPendingToIssueList(Guid.Empty, mItemName, , , , , mOrder.OrderDate.ToString, _
                                                                                    mOrder.TransTypeID, , , chkShowBERPart.Checked, _
                                                                                    ItemPrimaryCategory:=ItemPrimaryCategory, _
                                                                                    CodeNo:=Trim(txtGSENo.Text)) 'Added By Vikrant For Issue Tools Transaction
                    End If
                    Session("mPendingItemList") = mPendingItemList
                    DataFieldBind()
                    lblResult.Text = "Part Stock Status List : " & mStockItemList.Count & " Record(s) found."
                    lblResult1.Text = "Stock Details : " & mPendingItemList.Count & " Record(s) found."
                End If
            End If


        End If
Visibility:
        ControlVisibility()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If Not mIssue Is Nothing Then
            If ((mIssue.TransTypeID = 14) And (Session("IsRemovedAsReturnableFromAircraft") = True)) Then 'Added By Vikrant On 16-July-2013 For ALL10072013
                mPendingToReturnItemsRemovedFromAircraft = PendingToReturnItemsRemovedFromAircraft.GetPendingToReturnItemsRemovedFromAircraft(mIssue.StoreID, mIssue.MachineID, txtSearch.Text.Trim, mIssue.IDate.ToString, mIssue.TransTypeID, chkShowBERPart.Checked)
            Else 'Existing Condition As It Is 'Added By Prashant 3-July-2011 to Show only respective Store records
                mStockItemList = PendingToIssueItemList.GetPendingItemList(mIssue.StoreID, txtSearch.Text.Trim, mIssue.IDate.ToString, mIssue.TransTypeID, chkShowBERPart.Checked, IssueToDiscardAsExpired:=CInt(IssueToDiscardAsExpired), ItemPrimaryCategory:=ItemPrimaryCategory, CodeNo:=Trim(txtGSENo.Text), CategoryID:=cmbCategory.SelectedValue.ToString) 'Added By Vikrant For Issue Tools Transaction
            End If

            If mIssue.TransTypeID = 18 Then    'TransTypeID = 18 Added By Prashant 26-Sep-2011
                mPendingItemList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, "", , , , , mIssue.IDate.ToString, mIssue.TransTypeID, _
                                                                            LinkID, , chkShowBERPart.Checked, ItemPrimaryCategory:=ItemPrimaryCategory, _
                                                                            CodeNo:=Trim(txtGSENo.Text), ToTypeIDOfIssue:=mIssue.ToTypeID)
            Else
                mPendingItemList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, "", , , , , mIssue.IDate.ToString, mIssue.TransTypeID, , , _
                                                                            chkShowBERPart.Checked, IssueToDiscardAsExpired:=CInt(IssueToDiscardAsExpired), _
                                                                            ItemPrimaryCategory:=ItemPrimaryCategory, CodeNo:=Trim(txtGSENo.Text), _
                                                                            ToTypeIDOfIssue:=mIssue.ToTypeID) 'Added By Vikrant For Issue Tools Transaction
            End If
        End If
        If Not mOrder Is Nothing Then
            mStockItemList = PendingToIssueItemList.GetPendingItemList(Guid.Empty, txtSearch.Text.Trim, mOrder.OrderDate.ToString, mOrder.TransTypeID, chkShowBERPart.Checked, _
                                                                       ItemPrimaryCategory:=ItemPrimaryCategory, CodeNo:=Trim(txtGSENo.Text)) 'Added By Vikrant For Issue Tools Transaction
            mPendingItemList = PendingToIssueList.GetPendingToIssueList(Guid.Empty, "", , , , , mOrder.OrderDate.ToString, mOrder.TransTypeID, , , _
                                                                        chkShowBERPart.Checked, ItemPrimaryCategory:=ItemPrimaryCategory, _
                                                                        CodeNo:=Trim(txtGSENo.Text)) 'Added By Vikrant For Issue Tools Transaction
        End If
        Session("mStockItemList") = mStockItemList
        Session("mPendingItemList") = mPendingItemList
        Session("mPendingToReturnItemsRemovedFromAircraft") = mPendingToReturnItemsRemovedFromAircraft 'Added By Vikrant On 16-July-2013 For ALL10072013
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        Dim SelectedCategoryValue As String
        If Not mIssue Is Nothing Then
            SelectedCategoryValue = cmbCategory.SelectedValue
        End If
        'End
        DataFieldBind()
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        If Not mIssue Is Nothing Then
            cmbCategory.SelectedValue = SelectedCategoryValue
        End If
        'End


        lblResult.Text = "Part Stock Status List : " & mStockItemList.Count & " Record(s) found."
        lblResult1.Text = "Stock Details : " & mPendingItemList.Count & " Record(s) found."
        'Added By Utkarsh ON 03-May-2012 FOR ALLIssue30042012
        dgAlternateStockList.Visible = False
        lblResult2.Visible = False
        'End
        ControlVisibilityForAircraftRemovedReturnableItems() 'Added By Vikrant On 15-July-2013 For ALL10072013
        upnlStockItemList.Update()
        upnlPendingItemList.Update()
        upnlRemovedAsReturnableFromAircraft.Update()
        upnlAlternateStockList.Update() 'ALL21122016-1
    End Sub
    Private Sub dgStockItemList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgStockItemList.RowCommand
        DueAtMessage.Visible = False
        Select Case e.CommandName
            Case "SelectRecord"
                Dim Index1 As Int32 = CInt(e.CommandArgument) + dgStockItemList.PageIndex * dgStockItemList.PageSize
                If Not mIssue Is Nothing Then
                    'Added By Prashant 3-July-2011 to Show only respective Store records
                    'Added By Prashant 26-Sep-2011
                    If mIssue.TransTypeID = 18 Then  'Loan Return To Store
                        mPendingItemList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, mStockItemList(Index1).ItemName, , , , , _
                                                                                    mIssue.IDate.ToString, mIssue.TransTypeID, _
                                                                                    mStockItemList(Index1).LinkID.ToString, , chkShowBERPart.Checked, _
                                                                                    ItemPrimaryCategory:=ItemPrimaryCategory, CodeNo:=Trim(txtGSENo.Text), _
                                                                                    ToTypeIDOfIssue:=mIssue.ToTypeID) 'Added By Vikrant For Issue Tools Transaction
                        'Added By Utkarsh ON 30-Apr-2012 FOR ALLIssue30042012
                        mAlternateStockList = AlternateStockItemList.GetAlternateStockItemList(mIssue.StoreID, mStockItemList(Index1).ItemName, , , , , _
                                                                                               mIssue.IDate.ToString, mIssue.TransTypeID, _
                                                                                               mStockItemList(Index1).LinkID.ToString, , _
                                                                                               chkShowBERPart.Checked, ToTypeIDOfIssue:=mIssue.ToTypeID)
                        'End 
                        lblResult2.Text = "Alternate Stock Item List : " & mAlternateStockList.Count & " Record(s) found."
                    Else '-----------------------------
                        mPendingItemList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, mStockItemList(Index1).ItemName, , , , , _
                                                                                    mIssue.IDate.ToString, mIssue.TransTypeID, , , _
                                                                                    chkShowBERPart.Checked, _
                                                                                    IssueToDiscardAsExpired:=CInt(IssueToDiscardAsExpired), _
                                                                                    ItemPrimaryCategory:=ItemPrimaryCategory, CodeNo:=Trim(txtGSENo.Text), _
                                                                                    ToTypeIDOfIssue:=mIssue.ToTypeID) 'Added By Vikrant For Issue Tools Transaction
                        'Added By Utkarsh ON 30-Apr-2012 FOR ALLIssue30042012
                        If mIssue.TransTypeID = 16 Or mIssue.TransTypeID = 19 Or mIssue.TransTypeID = 58 Then
                            'Do nothing
                        Else
                            mAlternateStockList = AlternateStockItemList.GetAlternateStockItemList(mIssue.StoreID, mStockItemList(Index1).ItemName, , , , , _
                                                                                                   mIssue.IDate.ToString, mIssue.TransTypeID, _
                                                                                                   mStockItemList(Index1).ItemID.ToString, , _
                                                                                                   chkShowBERPart.Checked, ToTypeIDOfIssue:=mIssue.ToTypeID)
                            lblResult2.Text = "Alternate Stock Item List : " & mAlternateStockList.Count & " Record(s) found."
                        End If
                        'End 
                    End If '-------------------------------------------------------------------
                End If
                If Not mOrder Is Nothing Then
                    mPendingItemList = PendingToIssueList.GetPendingToIssueList(Guid.Empty, mStockItemList(Index1).ItemName, , , , , mOrder.OrderDate.ToString, mOrder.TransTypeID, , , chkShowBERPart.Checked, ItemPrimaryCategory:=ItemPrimaryCategory, CodeNo:=Trim(txtGSENo.Text)) 'Added By Vikrant For Issue Tools Transaction
                    'Added By Utkarsh ON 30-Apr-2012 FOR ALLIssue30042012
                    mAlternateStockList = AlternateStockItemList.GetAlternateStockItemList(Guid.Empty, mStockItemList(Index1).ItemName, , , , , mOrder.OrderDate.ToString, mOrder.TransTypeID, mStockItemList(Index1).ItemID.ToString, , chkShowBERPart.Checked)
                    'End 
                    lblResult2.Text = "Alternate Stock Item List : " & mAlternateStockList.Count & " Record(s) found."
                End If
                Session("mPendingItemList") = mPendingItemList
                Session("mAlternateStockList") = mAlternateStockList 'Added By Utkarsh ON 30-Apr-2012 FOR ALLIssue30042012
                dgAlternateStockList.DataSource = mAlternateStockList
                DataFieldBind()
                lblResult.Text = "Part Stock Status List : " & mStockItemList.Count & " Record(s) found."
                lblResult1.Text = "Stock Details : " & mPendingItemList.Count & " Record(s) found."
                ControlVisibility() 'Added By Utkarsh ON 02-May-2012 FOR ALLIssue30042012
                upnlPendingItemList.Update()
                upnlAlternateStockList.Update()
        End Select
    End Sub
    Private Sub dgPendingItemList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingItemList.RowCommand

        DueAtMessage.Visible = False
        Select Case e.CommandName
            Case "SelectRecord"

                Dim Index2 As Int32 = CInt(e.CommandArgument) + dgPendingItemList.PageIndex * dgPendingItemList.PageSize

                mUserHasNoStoreRights = UserHasNoStoreRights.GetUserHasNoStoreRights(User.Identity.Name, mPendingItemList(Index2).StoreID.ToString) 'Added By Prashant 31-Oct-2018 ALL30102018

                If mUserHasNoStoreRights.Count > 0 Then

                    MSGBoxCtrl.Show("Alert!", "Sorry you do not have rights to select this store. Please contact with admin.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub

                End If

                If mIssue IsNot Nothing Then

                    If IsDBNull(mPendingItemList(Index2).Expirydate) = False Then

                        If mPendingItemList(Index2).Expirydate <= Today.Date Then

                            MSGBoxCtrl.show(MSGBox.Message_title.PartExpired,
                                            MSGBox.Message_text.PartExpired,
                                            "<BR> <BR> Do you want to continue",
                                            MsgBoxStyle.YesNo,
                                            "Expired")
                            Session("Index2") = Index2
                            Session("ItemName") = mPendingItemList(Index2).ItemName
                            Exit Sub

                        End If

                    ElseIf mPendingItemList(Index2).ExpiryQtrs <> "" Then

                        If mPendingItemList(Index2).ExpiryQtrDate <= Today.Date Then

                            MSGBoxCtrl.show(MSGBox.Message_title.PartExpired,
                                            MSGBox.Message_text.PartExpired,
                                            "<BR> <BR> Do you want to continue",
                                            MsgBoxStyle.YesNo,
                                            "Expired")
                            Session("Index2") = Index2
                            Session("ItemName") = mPendingItemList(Index2).ItemName
                            Exit Sub

                        End If

                    End If

                    If mPendingItemList(Index2).CountOfComponentReservationItem > 0 Then 'Added By Prashant 2-Dec-2021 BA29112021

                        MSGBoxCtrl.show(MSGBox.Message_title.Alert,
                                            MSGBox.Message_text.Alert,
                                            "This component is reserved for Aircraft " + mPendingItemList(Index2).ReservedComponentRegNo +
                                            " Dated " + mPendingItemList(Index2).ReservedComponentDateFormatted + " as per schedule allocation. " +
                                            "<BR>Are you issuing it as per allocation?",
                                            MsgBoxStyle.YesNo,
                                            "ReservedComponent")
                        Session("Index2") = Index2
                        Session("ItemName") = mPendingItemList(Index2).ItemName
                        Session("Toshowsecondmessageboxonce") = "Toshowsecondmessageboxonce"
                        Exit Sub

                    End If

                    If Not (mIssue.TransTypeID = 14 And mIssue.ToTypeID <> 18) Then

                        'Added By Vikrant For Issue Tools Transaction
                        If mIssue.TransTypeID = Util.Trans.IssueToolsToEmployee Then

                            If mPendingItemList(Index2).CalibrationDueDateFormatted.ToString <> "" Then 'Added by Prashant 29-Aug-2019 BA29082019-1

                                If CDate(mPendingItemList(Index2).CalibrationDueDate) <= CDate(mIssue.IDate) Then

                                    MSGBoxCtrl.Show("Alert!",
                                                "Calibration due date of Part No. " + mPendingItemList(Index2).ItemName +
                                                " have expired.",
                                                "",
                                                MsgBoxStyle.OkOnly, "")
                                    Exit Sub

                                End If

                            End If 'End of Added by Prashant 29-Aug-2019 BA29082019-1

                            If mIssue.IssueItems.Contains(mPendingItemList(Index2).ReceiptItemID) Then

                                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate,
                                            MSGBox.Message_text.Duplicate,
                                            "Issue Item",
                                            MsgBoxStyle.OkOnly,
                                            "")
                                Exit Sub

                            End If

                        End If

                        'Added by vikrant For New Requisition

                        If ((mIssue.TransTypeID = Trans.IssueToAircraft Or mIssue.TransTypeID = Trans.IssueToWorkShop Or
                             mIssue.TransTypeID = Trans.IssueToolsToEmployee Or mIssue.TransTypeID = Trans.IssueToWorkOrderAsSpares) _
                            And mIssue.ToTypeID = 18) Then
                            Session("NewRequisition") = "True"  'Or mIssue.TransTypeID = Trans.IssueToolsToEmployee 'Added By Prashant on 17-May-2021 ALL17052021
                        End If
                        'End

                    End If

                End If

                SetObject(Index2)
                Method()
                Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))

            Case "ViewRec"
                Dim Index2 As Int32 = CInt(e.CommandArgument) + dgPendingItemList.PageIndex * dgPendingItemList.PageSize
                ReceiptItemAttachment(ReceiptItemID:=mPendingItemList(Index2).ReceiptItemID.ToString)
        End Select

    End Sub
    Private Sub dgAlternateStockList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAlternateStockList.RowCommand
        DueAtMessage.Visible = False
        Select Case e.CommandName
            Case "SelectRecord"
                Dim Index2 As Int32 = CInt(e.CommandArgument) + dgAlternateStockList.PageIndex * dgAlternateStockList.PageSize

                mUserHasNoStoreRights = UserHasNoStoreRights.GetUserHasNoStoreRights(User.Identity.Name, mAlternateStockList(Index2).StoreID.ToString) 'Added By Prashant 31-Oct-2018 ALL30102018
                If mUserHasNoStoreRights.Count > 0 Then
                    MSGBoxCtrl.show("Alert!", "Sorry you do not have rights to select this store. Please contact with admin.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                If Not mIssue Is Nothing Then
                    If IsDBNull(mAlternateStockList(Index2).Expirydate) = False Then
                        If mAlternateStockList(Index2).Expirydate <= Today.Date Then
                            MSGBoxCtrl.show(MSGBox.Message_title.PartExpired, MSGBox.Message_text.PartExpired, "<BR> <BR> Do you want to continue", MsgBoxStyle.YesNo, "Expired")
                            Session("Index2") = Index2
                            Session("ItemName") = mAlternateStockList(Index2).ItemName
                            Session("IsAlternatePart") = "True"
                            Exit Sub
                        End If
                    ElseIf mAlternateStockList(Index2).ExpiryQtrs <> "" Then
                        If mAlternateStockList(Index2).ExpiryQtrDate <= Today.Date Then
                            MSGBoxCtrl.show(MSGBox.Message_title.PartExpired, MSGBox.Message_text.PartExpired, "<BR> <BR> Do you want to continue", MsgBoxStyle.YesNo, "Expired")
                            Session("Index2") = Index2
                            Session("ItemName") = mAlternateStockList(Index2).ItemName
                            Session("IsAlternatePart") = "True"
                            Exit Sub
                        End If
                    End If
                    If mAlternateStockList(Index2).CountOfComponentReservationItem > 0 Then 'Added By Prashant 2-Dec-2021 BA29112021
                        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "This component is reserved for Aircraft " + mAlternateStockList(Index2).ReservedComponentRegNo + " Dated " + mPendingItemList(Index2).ReservedComponentDateFormatted + " as per schedule allocation. " + "<BR>Are you issuing it as per allocation?", MsgBoxStyle.YesNo, "ReservedComponent")
                        Session("Index2") = Index2
                        Session("ItemName") = mAlternateStockList(Index2).ItemName
                        Session("IsAlternatePart") = "True"
                        Session("Toshowsecondmessageboxonce") = "Toshowsecondmessageboxonce"
                        Exit Sub
                    End If
                    If mIssue.TransTypeID = 14 And mIssue.ToTypeID <> 18 Then
                        SetObject(Index2, True)
                        Method()
                        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
                    Else
                        'Added By Vikrant For Issue Tools Transaction
                        If mIssue.TransTypeID = Util.Trans.IssueToolsToEmployee Then
                            If mAlternateStockList(Index2).CalibrationDueDateFormatted.ToString <> "" Then 'Added by Prashant 29-Aug-2019 BA29082019-1
                                If CDate(mAlternateStockList(Index2).CalibrationDueDate) <= CDate(mIssue.IDate) Then
                                    MSGBoxCtrl.show("Alert!", "Calibration due date of Part No. " + mAlternateStockList(Index2).ItemName + " have expired.", "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                End If
                            End If 'End of Added by Prashant 29-Aug-2019 BA29082019-1
                            If mIssue.IssueItems.Contains(mAlternateStockList(Index2).ReceiptItemID) Then
                                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Issue Item", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        End If
                        'End
                        SetObject(Index2, True)
                        Method()
                        'Added by vikrant For New Requisition
                        If ((mIssue.TransTypeID = Trans.IssueToAircraft Or mIssue.TransTypeID = Trans.IssueToWorkShop Or _
                            mIssue.TransTypeID = Trans.IssueToolsToEmployee Or mIssue.TransTypeID = Trans.IssueToWorkOrderAsSpares) _
                           And mIssue.ToTypeID = 18) Then
                            Session("NewRequisition") = "True"  'Or mIssue.TransTypeID = Trans.IssueToolsToEmployee 'Added By Prashant on 17-May-2021 ALL17052021
                        End If
                        'End
                        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
                    End If
                Else
                    SetObject(Index2, True)
                    Method()
                    Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
                End If
            Case "ViewRec"
                Dim Index2 As Int32 = CInt(e.CommandArgument) + dgAlternateStockList.PageIndex * dgAlternateStockList.PageSize
                ReceiptItemAttachment(ReceiptItemID:=mAlternateStockList(Index2).ReceiptItemID.ToString)
        End Select
    End Sub
    Private Sub dgRemovedAsReturnableFromAircraft_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRemovedAsReturnableFromAircraft.RowCommand 'Added By Vikrant On 15-July-2013 For ALL10072013
        DueAtMessage.Visible = False
        Select Case e.CommandName
            Case "SelectRecord"
                Dim Index1 As Int32 = CInt(e.CommandArgument) + dgRemovedAsReturnableFromAircraft.PageIndex * dgRemovedAsReturnableFromAircraft.PageSize
                If Not mIssue Is Nothing Then
                    mPendingItemList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, mPendingToReturnItemsRemovedFromAircraft(Index1).ItemName, , , , , _
                                                                                mIssue.IDate.ToString, mIssue.TransTypeID, , , chkShowBERPart.Checked, _
                                                                                ItemPrimaryCategory:=ItemPrimaryCategory, _
                                                                                CodeNo:=Trim(txtGSENo.Text), ToTypeIDOfIssue:=mIssue.ToTypeID) 'Added By Vikrant For Issue Tools Transaction
                    mAlternateStockList = AlternateStockItemList.GetAlternateStockItemList(mIssue.StoreID, _
                                                                                           mPendingToReturnItemsRemovedFromAircraft(Index1).ItemName, , , , , _
                                                                                           mIssue.IDate.ToString, mIssue.TransTypeID, _
                                                                                           mPendingToReturnItemsRemovedFromAircraft(Index1).ItemID.ToString, , _
                                                                                           chkShowBERPart.Checked, ToTypeIDOfIssue:=mIssue.ToTypeID)
                End If
                mIssue.IssueItems.CurrentItem.RemovalReceiptItemID = New Guid(dgRemovedAsReturnableFromAircraft.Rows.Item(CInt(e.CommandArgument)).Cells(0).Text)
                Session("mIssue") = mIssue
                Session("mPendingItemList") = mPendingItemList
                Session("mAlternateStockList") = mAlternateStockList
                dgAlternateStockList.DataSource = mAlternateStockList
                DataFieldBind()
                lblResult.Text = "Part Stock Status List : " & mStockItemList.Count & " Record(s) found."
                lblResult1.Text = "Stock Details : " & mPendingItemList.Count & " Record(s) found."
                lblResult2.Text = "Alternate Stock Item List : " & mAlternateStockList.Count & " Record(s) found."
                lblResult3.Text = "Removed As Reurnable From Aircraft Items : " & mPendingToReturnItemsRemovedFromAircraft.Count & " Record(s) found."
                ControlVisibility()
                upnlAlternateStockList.Update()
                upnlPendingItemList.Update()
        End Select
    End Sub
    Private Sub dgStockItemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgStockItemList.PageIndexChanging
        DueAtMessage.Visible = False
        dgStockItemList.PageIndex = e.NewPageIndex
        dgStockItemList.DataSource = mStockItemList
        dgStockItemList.DataBind()
        upnlStockItemList.Update()
        Session("mStockItemList") = mStockItemList
    End Sub
    Private Sub dgPendingItemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingItemList.PageIndexChanging
        DueAtMessage.Visible = False
        dgPendingItemList.PageIndex = e.NewPageIndex
        dgPendingItemList.DataSource = mPendingItemList
        dgPendingItemList.DataBind()
        upnlPendingItemList.Update()
        Session("mPendingItemList") = mPendingItemList
    End Sub
    Private Sub dgAlternateStockList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgAlternateStockList.PageIndexChanging
        DueAtMessage.Visible = False
        dgAlternateStockList.PageIndex = e.NewPageIndex
        dgAlternateStockList.DataSource = mAlternateStockList
        dgAlternateStockList.DataBind()
        upnlAlternateStockList.Update()
        Session("mAlternateStockList") = mAlternateStockList
    End Sub
    Private Sub dgRemovedAsReturnableFromAircraft_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgRemovedAsReturnableFromAircraft.PageIndexChanging
        DueAtMessage.Visible = False
        dgRemovedAsReturnableFromAircraft.PageIndex = e.NewPageIndex
        dgRemovedAsReturnableFromAircraft.DataSource = mPendingToReturnItemsRemovedFromAircraft
        dgRemovedAsReturnableFromAircraft.DataBind()
        upnlRemovedAsReturnableFromAircraft.Update()
        Session("mPendingToReturnItemsRemovedFromAircraft") = mPendingToReturnItemsRemovedFromAircraft
    End Sub
    Private Sub dgStockItemList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgStockItemList.Sorting
        DueAtMessage.Visible = False
        mStockItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mStockItemList") = mStockItemList
        dgStockItemList.DataSource = mStockItemList
        dgStockItemList.DataBind()
        upnlStockItemList.Update()
    End Sub
    Private Sub dgPendingItemList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPendingItemList.Sorting
        DueAtMessage.Visible = False
        mPendingItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingItemList") = mPendingItemList
        dgPendingItemList.DataSource = mPendingItemList
        dgPendingItemList.DataBind()
        upnlPendingItemList.Update()
    End Sub
    Private Sub dgAlternateStockList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAlternateStockList.Sorting
        DueAtMessage.Visible = False
        mAlternateStockList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAlternateStockList") = mAlternateStockList
        dgAlternateStockList.DataSource = mAlternateStockList
        dgAlternateStockList.DataBind()
        upnlAlternateStockList.Update()
    End Sub
    Private Sub dgRemovedAsReturnableFromAircraft_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRemovedAsReturnableFromAircraft.Sorting
        mPendingToReturnItemsRemovedFromAircraft.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingToReturnItemsRemovedFromAircraft") = mPendingToReturnItemsRemovedFromAircraft
        dgRemovedAsReturnableFromAircraft.DataSource = mPendingToReturnItemsRemovedFromAircraft
        dgRemovedAsReturnableFromAircraft.DataBind()
        upnlRemovedAsReturnableFromAircraft.Update()
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
        Session("IsRemovedAsReturnableFromAircraft") = False 'Added By Vikrant On 16-July-2013 For ALL10072013
        Session.Remove("ItemPrimaryCategory") 'Added By Vikrant For Issue Tools Transaction
        If Request.QueryString("ChildPage1") = "wfnPendingWOListForIssueSpares_Ajax.aspx" Or Request.QueryString("ChildPage1") = "wfnPendingWOListForIssueTools_Ajax.aspx" Or Request.QueryString("ChildPage1") = "wfRequisitionItemListForIssue_Ajax.aspx" Then
            Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        Else
            If Request.QueryString("ChildPage") = "wfToolsCheckOut_Ajax.aspx" Then 'Added By Vikrant For Issue Tools Transaction
                If mIssue.IssueItems.CurrentItem.ItemID.Equals(Guid.Empty) Then
                    mIssue.IssueItems.Remove(mIssue.IssueItems.CurrentItem)
                End If
                Session("mIssue") = mIssue
            End If 'End
            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        If Not mIssue Is Nothing Then
            'If mIssue.TransTypeID = 14 Then
            MessageBoxResult()
            'End If
        End If
    End Sub
    'Added By Vikrant On 03-Feb-2016 For ALL03022016
    Private Sub btnSelectAllParts_Click(sender As Object, e As System.EventArgs) Handles btnSelectAllParts.Click
        mStockItemList = PendingToIssueItemList.GetPendingItemList(mIssue.StoreID, "", mIssue.IDate.ToString, mIssue.TransTypeID, chkShowBERPart.Checked, True, IssueToDiscardAsExpired:=CInt(IssueToDiscardAsExpired), ItemPrimaryCategory:=ItemPrimaryCategory, CodeNo:=Trim(txtGSENo.Text), CategoryID:=cmbCategory.SelectedValue) 'Added By Vikrant For Issue Tools Transaction
        If mStockItemList(0).TotalRecords > 0 Then
            If mStockItemList(0).TotalRecords > 500 Then
                MSGBoxCtrl.show("Alert!", "There are " & mStockItemList(0).TotalRecords.ToString & " Part(s) available in Store.You can Issue 500 Part(s) at a time.For other Parts you need to create seperate Issue(s). " & "<BR><BR>You are about to Issue first 500 Part(s). " & "<BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo, "SelectAllParts")
            Else
                MSGBoxCtrl.show("Alert!", "You are about to Issue all " & mStockItemList(0).TotalRecords.ToString & " Part(s) available in Store." & "<BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo, "SelectAllParts")
            End If
        Else
            MSGBoxCtrl.show("Alert!", "There are no Part(s) in the Store to be Issued.", "", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    'End
#End Region

End Class