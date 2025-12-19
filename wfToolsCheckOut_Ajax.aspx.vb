Imports System.Linq
Imports System.Linq.Enumerable


Public Class wfToolsCheckOut_Ajax
    Inherits Page

#Region " Enumeration "

    Private Enum Rights

        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        Authorized = 7 'Added By Prashant 17-Aug-2011

    End Enum

    Private Enum RequestFor

        Supplier = 0
        Customer = 1

    End Enum

#End Region

#Region " Variable Declaration "

    Public mEmployeeStatus As EmployeeStatus
    Private mEmployeeListForCombo As EmployeeListForCombo
    Public mIssue As Issue
    Dim mStoreList As StoreList
    Dim mTypeList1 As TypeList1
    Dim mVendorList As VendorList
    Dim mMachineNameValueList As MachineNameValueList  'Dim mMachineNameValueList As tmpMachineList
    Dim mStatusList As StatusList
    Public mTransTypeID As Trans
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Public mWorkShopList As WorkShopList
    ''Public mWOList As FlyPal22.Maintain.WOList                 'Added Code
    Public mnWOListForCombo As nWOListForCombo
    Dim mIsForWOReturn As Boolean = False
    Dim mIssueDetail As String
    Public ModuleName As String
    Public mIssueTo As String
    Dim mVendorTerms As VendorTerms          'Added By Prashant 26-Apr-2010
    Public Flag As Integer
    'Added By Utkarsh ON 22-Jul-2013 FOR ALL19072013
    Dim mReceiptCumInvoice As ReceiptCumInvoice = Nothing
    Dim mPendingToReceiveTransItemList As PendingToReceiveTransItemList = Nothing
    'End
    Dim BaseCurrencysymbol As String = ""
    Dim mOpenFrom As String 'Added By Prashant 3-Apr-2014 ALL03042014
    Dim mLastWarrantyInformation As LastWarrantyInformation
    'Dim mMachineNameValueList As MachineNameValueList
    Public mEmployeeList As EmployeeList
    Public mUserHasNoStoreRights As UserHasNoStoreRights

#End Region

#Region " Business Methods "

    Private Sub GetSession()

        mIssue = Session("mIssue")
        mStoreList = Session("mStoreList")
        mTypeList1 = Session("mTypeList1")
        mVendorList = Session("mVendorList")
        mMachineNameValueList = Session("mMachineNameValueList")
        mStatusList = Session("mStatusList")
        mTransTypeID = Session("mTransTypeID")
        ModuleName = Session("ModuleName")
        mnWOListForCombo = Session("mnWOListForCombo")
        mWorkShopList = Session("mWorkShopList")
        mVendorTerms = Session("mVendorTerms")
        mIsForWOReturn = CType(Session("IsForWOReturn"), Boolean) 'Added by Saylee on 13-Dec-2010
        mEmployeeListForCombo = Session("mEmployeeListForCombo")

    End Sub

    Private Sub SetSession()

        Session("mIssue") = mIssue
        Session("mStoreList") = mStoreList
        Session("mTypeList1") = mTypeList1
        Session("mVendorList") = mVendorList
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mStatusList") = mStatusList
        Session("ModuleName") = ModuleName
        Session("ModuleName") = ModuleName
        Session("mnWOListForCombo") = mnWOListForCombo
        Session("mWorkShopList") = mWorkShopList
        Session("mVendorTerms") = mVendorTerms
        Session("IsForWOReturn") = mIsForWOReturn 'Added by Saylee on 13-Dec-2010

    End Sub

    Private Sub RemoveSession()

        Session.Remove("mIssue")
        Session.Remove("mStoreList")
        Session.Remove("mTypeList1")
        Session.Remove("mVendorList")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mStatusList")
        Session.Remove("mnWOListForCombo")
        Session.Remove("Edit")
        Session.Remove("mWorkShopList")
        Session.Remove("mVendorTerms")
        Session.Remove("mEmployeeListForCombo")

    End Sub

    Private Sub SetPage()

        If mIssue.No > 0 Then
            lblTitle.Text = " " + Session("ModuleName") + " [ " + mIssue.Text + "-" + CType(mIssue.No, String) + " ]"
        Else
            lblTitle.Text = " " + Session("ModuleName") + " [ New ]"
        End If

    End Sub

    Private Sub SaveIsSerializedExists()

        'Authentication
        If Not mIssue.IDate Is DBNull.Value Then

            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))

            If mCheck.WebAuthentication = True Then

                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------

                If DateDiff(DateInterval.Day, CDate(mIssue.IDate), maxAllowableDate) < 0 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Issue. <br> Issue Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub

                End If

                'Added By Vikrant On 28-July-2014 For BA24072014
                If AppSettings("LockBackDatedTransaction") = "True" Then

                    If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then

                        'Do nothing
                    Else

                        If mIssue.StatusID <> 2 Then

                            If CheckDateForTransactionLock(mIssue.IDate) Then

                                MSGBoxCtrl.Show("Save Alert!", "Previous Months transactions can only be saved until " & DateSerial(Year(CDate(mIssue.IDate).AddMonths(1)), Month(CDate(mIssue.IDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "Kindly book this transaction in current month to reflect in Valuation.", MsgBoxStyle.OkOnly, "")
                                Exit Sub

                            End If

                        End If

                    End If

                End If

            End If

        End If

        'Authentication
        Dim IssueClone As Issue
        IssueClone = mIssue.Clone

        Try

            'check whether min. one item is present while saving
            If Not mIssue.IssueItems.Count = 0 Then

                SetObject()

                If mIssue.IsValid Then

                    mIssue.Save()
                    Session("ToMakeAuthorizeButtonInvisibel") = ""

                Else

                    Dim strMSG As String = ""

                    If Not mIssue.IsValid Then

                        For i As Integer = 0 To mIssue.GetBrokenRulesCollection.Count - 1

                            strMSG = strMSG + mIssue.GetBrokenRulesCollection(i).Description + "<Br>"

                        Next

                    End If

                    'Added by vikrant on 26-aug-2011-----------------------------------
                    Dim mIssueItem As IssueItem

                    If Not mIssue.IssueItems.IsValid Then

                        For Each mIssueItem In mIssue.IssueItems

                            For i As Integer = 0 To mIssueItem.GetBrokenRulesCollection.Count - 1

                                strMSG = strMSG + mIssueItem.ItemName + " : " + mIssueItem.GetBrokenRulesCollection(i).Description + "<Br>"

                            Next

                        Next

                    End If

                    If strMSG.Trim <> "" Then
                        Session("strMSG") = strMSG
                        cvControlValidator.ErrorMessage = strMSG
                        cvControlValidator.IsValid = False
                    End If

                End If

                mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + IIf(mIssueTo <> "", " to " + mIssueTo, "")

                If mIssue.StatusID = 2 Then
                    MarkLog(Action.Authorize, ModuleName, mIssueDetail, ErrorType.NoError, mIssue.ID, EventLogID)
                ElseIf mIssue.StatusID = 3 Then
                    MarkLog(Action.Amend, ModuleName, mIssueDetail, ErrorType.NoError, mIssue.ID, EventLogID)
                ElseIf mIssue.StatusID = 4 Then
                    MarkLog(Action.Cancel, ModuleName, mIssueDetail, ErrorType.NoError, mIssue.ID, EventLogID)
                Else
                    MarkLog(Action.Save, ModuleName, mIssueDetail, ErrorType.NoError, mIssue.ID, EventLogID)
                End If

                mIssue.MarkClean()
                Session("mIssue") = mIssue
                SetPage()
                ControlVisibility()
                DataFieldBind()
                upnlActionBtn.Update()
                upnlIssueDetails.Update()
                upnlIssueItem.Update()
                upnlTitle.Update()

            Else

                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
                                MSGBox.Message_text.saveAlert,
                                "Issue can not be saved without Item.",
                                MsgBoxStyle.OkOnly,
                                "")
                Exit Sub

            End If

        Catch ex As SqlException

            Session("IssueClone") = IssueClone

            If ex.Number = 8114 Or ex.Number = 8115 Then

                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow,
                                MSGBox.Message_text.NumericOverFlow,
                                " Rate or Qty or Conversion Factor. ",
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf ex.Number = 8145 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.ProcedureError,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf ex.Number = 2627 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.Duplicate,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf ex.Number = 547 Then

                If InStr(ex.Message, "CCtabRequisitionItemIssueBalQty", CompareMethod.Text) Or
                   InStr(ex.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Or
                   InStr(ex.Message, "CCtabReceiptItemLoanQty", CompareMethod.Text) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                    MSGBox.Message_text.PendingQty,
                                    "Issue Qty can not be greater than Receipt Qty.",
                                    MsgBoxStyle.OkOnly,
                                    "Status")

                    mIssue = IssueClone
                    SetObject()
                    Session("mIssue") = mIssue
                    DataFieldBind()

                ElseIf InStr(ex.Message, "CCtabnWOSpareFromWOJobSparePendingIssuedQty", CompareMethod.Text) Or
                    InStr(ex.Message, "CCtabnWOToolsPendingIssuedQty", CompareMethod.Text) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                    MSGBox.Message_text.PendingQty,
                                    "Issue Qty can not be greater than Required Qty.",
                                    MsgBoxStyle.OkOnly,
                                    "Status")

                    mIssue = IssueClone
                    SetObject()
                    Session("mIssue") = mIssue
                    DataFieldBind()

                ElseIf InStr(ex.Message, "FKtabIssueTermtabTerm", CompareMethod.Text) Then

                    MSGBoxCtrl.Show("Term Deleted! ",
                                    "Term Not Available<Br><BR>Selected Term is no longer exist in the Database
                                               <BR><BR> Remove Term and try Again",
                                    " ",
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf ex.Number = 8144 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                    MSGBox.Message_text.ReferenceDelete,
                                    ex.Procedure + "," + ex.Message,
                                    MsgBoxStyle.OkOnly,
                                    "")

                End If

            End If

        Catch ex1 As Exception

            If InStr(ex1.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Then

                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                MSGBox.Message_text.PendingQty,
                                ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) +
                                           "Issue Qty can not be greater than Stock Qty.",
                                MsgBoxStyle.OkOnly,
                                "Status")

                mIssue = IssueClone
                SetObject()
                Session("mIssue") = mIssue
                DataFieldBind()

            ElseIf InStr(ex1.Message, "CCtabnWOSpareFromWOJobSparePendingIssuedQty", CompareMethod.Text) Or
                   InStr(ex1.Message, "CCtabnWOToolsPendingIssuedQty", CompareMethod.Text) Then

                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                MSGBox.Message_text.PendingQty,
                                "Issue Qty can not be greater than Required Qty.",
                                MsgBoxStyle.OkOnly,
                                "Status")

                mIssue = IssueClone
                SetObject()
                Session("mIssue") = mIssue
                DataFieldBind()

            ElseIf InStr(ex1.Message, "CCtabReceiptItemLoanQty", CompareMethod.Text) Then

                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                MSGBox.Message_text.PendingQty,
                                ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) +
                                           "Issue Qty can not be greater than Receipt Qty.",
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf InStr(ex1.Message, "CCtabSalesOrderItemIssueBalQty", CompareMethod.Text) Then

                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                MSGBox.Message_text.PendingQty,
                                ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) +
                                           "Issue Qty can not be greater than Sales Order Qty.",
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf InStr(ex1.Message, "CCtabOrderItemEROQty", CompareMethod.Text) Then

                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                MSGBox.Message_text.PendingQty,
                                ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) +
                                           "Issue Qty can not be greater than Exchange/Repair/Overhaul Qty.",
                                MsgBoxStyle.OkOnly,
                                "")

            End If

        Finally

            IssueClone = Nothing

        End Try

    End Sub

    Private Sub SetMsgBoxContent()
        For i As Integer = 0 To mIssue.IssueItems.Count - 1

        Next
    End Sub

    Private Sub Save()

        'Authentication
        If Not mIssue.IDate Is DBNull.Value Then

            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))

            If mCheck.WebAuthentication = True Then

                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------

                If DateDiff(DateInterval.Day, CDate(mIssue.IDate), maxAllowableDate) < 0 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Issue. <br> Issue Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub

                End If
                'Added By Vikrant On 05-Nov-2015 For All05112015
                If mIssue.TransTypeID = 14 Or mIssue.TransTypeID = 20 Then 'Aircraft Related Transactions

                    mMachineNameValueList = MachineNameValueList.GetMachineList(mIssue.IDateFormatted.ToString, , , , , , , True, "(SELECT)".Trim, True)
                    If mMachineNameValueList(mIssue.MachineID).IsReadOnly Then

                        MSGBoxCtrl.Show("Alert!", "", "As <b>" & cmbAircraftList.SelectedItem.ToString & "</b> is marked as ReadOnly,You can not save Issue.", MsgBoxStyle.OkOnly, "")
                        Exit Sub

                    End If

                End If
                'End
                'Added By Vikrant On 28-July-2014 For BA24072014
                If AppSettings("LockBackDatedTransaction") = "True" Then

                    If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then

                        'Do nothing
                    Else

                        If mIssue.StatusID <> 2 Then

                            If CheckDateForTransactionLock(mIssue.IDate) Then

                                MSGBoxCtrl.Show("Save Alert!", "Previous Months transactions can only be saved until " & DateSerial(Year(CDate(mIssue.IDate).AddMonths(1)), Month(CDate(mIssue.IDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "Kindly book this transaction in current month to reflect in Valuation.", MsgBoxStyle.OkOnly, "")
                                Exit Sub

                            End If

                        End If

                    End If

                End If

                'End
            End If

        End If
        'Authentication
        Dim IssueClone As Issue
        IssueClone = mIssue.Clone
        Try
            'check whether min. one item is present while saving
            If Not mIssue.IssueItems.Count = 0 Then

                SetObject()

                If mIssue.IsValid Then
                    'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries
                    'Check if IssueText is blank then call TransTextSeries UI

                    If (mIssue.IsNew) And (mIssue.Text = "") Then

                        Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.
                                                                            GetTransTextPreviousSeries(mIssue.TransTypeID,
                                                                                                       mIssue.IDateFormatted)

                        If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And
                            (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mIssue.TransTypeID) = False) Or
                            (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mIssue.TransTypeID) = True AndAlso
                            mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mIssue.TransTypeID).TransText = "")) Then

                            Dim str = "<script language='javascript'>openledgersame('wfToolsCheckOut_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

                            Session("BackPagestr_ForTransSeries") = str

                            Session("TransName_ForTransSeries") = "Issue"
                            Session("TransTypeID_ForTransSeries") = mIssue.TransTypeID
                            Session("TransDate_ForTransSeries") = mIssue.IDateFormatted
                            Session("AddTransTextSeries") = "True"

                            If mIssue.StatusID = 2 Then

                                mIssue.StatusID = 1
                                Session("mIssue") = mIssue

                            End If

                            Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")

                            Exit Sub

                        Else

                            Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.
                                                                                            RenewIt(mPreviousTransTextSeries)

                            If mAutoRenewTransTextSeries.IsRenewed Then

                                With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.
                                                                                            ItemByTransTypeID(mIssue.TransTypeID)
                                    mIssue.Text = .TransText
                                    mIssue.No = .StartingTransNo
                                End With

                            Else

                                Dim str = "<script language='javascript'>openledgersame('wfToolsCheckOut_Ajax.aspx?BackPage=" &
                                            Request.QueryString("BackPage") & "');</script>"

                                Session("BackPagestr_ForTransSeries") = str

                                Session("TransName_ForTransSeries") = "Issue"
                                Session("TransTypeID_ForTransSeries") = mIssue.TransTypeID
                                Session("TransDate_ForTransSeries") = mIssue.IDateFormatted
                                Session("AddTransTextSeries") = "True"

                                If mIssue.StatusID = 2 Then

                                    mIssue.StatusID = 1
                                    Session("mIssue") = mIssue

                                End If

                                Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")

                                Exit Sub

                            End If

                        End If

                    End If

                    'Added By Saylee on 21-July-2016 for store validation regarding NotInUse
                    ''Check whether Issue Date is greater than NotInUse of Store 
                    If mStoreList(mIssue.StoreID).NotInUse = True Then

                        If CDate(mStoreList(mIssue.StoreID).NotInUseDate) <= CDate(mIssue.IDate) Then

                            MSGBoxCtrl.Show("Save Alert!",
                                            "Store is not applicable since " + mStoreList(mIssue.StoreID).NotInUseDateFormatted,
                                            "Select another Store from list or select date before " +
                                                        mStoreList(mIssue.StoreID).NotInUseDateFormatted + " & try again",
                                            MsgBoxStyle.OkOnly,
                                            "")

                            Exit Sub

                        End If

                    End If

                    ''Check whether Issue Date is greater than NotInUse of ToStore 
                    If mStoreList(mIssue.ToStoreID).NotInUse = True Then

                        If CDate(mStoreList(mIssue.ToStoreID).NotInUseDate) <= CDate(mIssue.IDate) Then

                            MSGBoxCtrl.Show("Save Alert!",
                                            "Destination Store is not applicable since " +
                                                       mStoreList(mIssue.ToStoreID).NotInUseDateFormatted,
                                            "Select another Store from list or select date before " +
                                                       mStoreList(mIssue.ToStoreID).NotInUseDateFormatted + " & try again",
                                            MsgBoxStyle.OkOnly,
                                            "")

                            Exit Sub

                        End If

                    End If
                    '---------------------------------------------

                    'End
                    mIssue.Save()
                    Session("ToMakeAuthorizeButtonInvisibel") = ""
                Else

                    ValidationCode()
                    upnlValidationSummary.Update()
                    Exit Sub

                End If

                mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted +
                               IIf(mIssueTo <> "", " to " + mIssueTo, "")

                If mIssue.StatusID = 2 Then

                    MarkLog(Action.Authorize, ModuleName,
                            mIssueDetail,
                            ErrorType.NoError,
                            mIssue.ID,
                            EventLogID)

                ElseIf mIssue.StatusID = 3 Then

                    MarkLog(Action.Amend, ModuleName,
                            mIssueDetail,
                            ErrorType.NoError,
                            mIssue.ID,
                            EventLogID)

                ElseIf mIssue.StatusID = 4 Then

                    MarkLog(Action.Cancel, ModuleName,
                            mIssueDetail,
                            ErrorType.NoError,
                            mIssue.ID,
                            EventLogID)

                Else

                    MarkLog(Action.Save, ModuleName,
                            mIssueDetail,
                            ErrorType.NoError,
                            mIssue.ID,
                            EventLogID)

                End If

                mIssue.MarkClean()
                Session("mIssue") = mIssue
                SetPage()
                ControlVisibility()
                DataFieldBind()
                upnlActionBtn.Update()
                upnlIssueDetails.Update()
                upnlIssueItem.Update()
                upnlTitle.Update()

            Else

                If mIssue.StatusID = 2 Then

                    mIssue.StatusID = 1

                End If

                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
                                MSGBox.Message_text.saveAlert,
                                "Issue can not be saved without Item.",
                                MsgBoxStyle.OkOnly,
                                "")

            End If

        Catch ex As SqlException

            Session("IssueClone") = IssueClone

            If ex.Number = 8114 Or ex.Number = 8115 Then

                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow,
                                MSGBox.Message_text.NumericOverFlow,
                                " Rate or Qty or Conversion Factor. ",
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf ex.Number = 8145 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.ProcedureError,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf ex.Number = 2627 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.Duplicate,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf ex.Number = 547 Then

                If InStr(ex.Message, "CCtabRequisitionItemIssueBalQty", CompareMethod.Text) Or
                   InStr(ex.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Or
                   InStr(ex.Message, "CCtabReceiptItemLoanQty", CompareMethod.Text) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                    MSGBox.Message_text.PendingQty,
                                    "Issue Qty can not be greater than Receipt Qty.",
                                    MsgBoxStyle.OkOnly,
                                    "Status")

                    mIssue = IssueClone
                    SetObject()
                    Session("mIssue") = mIssue
                    DataFieldBind()

                ElseIf InStr(ex.Message, "CCtabnWOSpareFromWOJobSparePendingIssuedQty", CompareMethod.Text) Or
                    InStr(ex.Message, "CCtabnWOToolsPendingIssuedQty", CompareMethod.Text) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                    MSGBox.Message_text.PendingQty,
                                    "Issue Qty can not be greater than Required Qty.",
                                    MsgBoxStyle.OkOnly,
                                    "Status")

                    mIssue = IssueClone
                    SetObject()
                    Session("mIssue") = mIssue
                    DataFieldBind()

                ElseIf InStr(ex.Message, "FKtabIssueTermtabTerm", CompareMethod.Text) Then

                    MSGBoxCtrl.Show("Term Deleted! ",
                                    "Term Not Available<Br><BR>Selected Term is no longer exist in the Database 
                                               <BR><BR> Remove Term and try Again",
                                    " ",
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf ex.Number = 8144 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                    MSGBox.Message_text.ReferenceDelete,
                                    ex.Procedure + "," + ex.Message,
                                    MsgBoxStyle.OkOnly,
                                    "")

                End If

            End If

        Catch ex1 As Exception

            mIssue = IssueClone
            SetObject()
            Session("mIssue") = mIssue
            DataFieldBind()

            If InStr(ex1.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Then

                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                MSGBox.Message_text.PendingQty,
                                ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) +
                                           "Issue Qty can not be greater than Stock Qty.",
                                MsgBoxStyle.OkOnly,
                                "Status")

            ElseIf InStr(ex1.Message, "CCtabnWOSpareFromWOJobSparePendingIssuedQty", CompareMethod.Text) Or
                   InStr(ex1.Message, "CCtabnWOToolsPendingIssuedQty", CompareMethod.Text) Then

                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                MSGBox.Message_text.PendingQty,
                                "Issue Qty can not be greater than Required Qty.",
                                MsgBoxStyle.OkOnly,
                                "Status")

            ElseIf InStr(ex1.Message, "CCtabReceiptItemLoanQty", CompareMethod.Text) Then

                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                MSGBox.Message_text.PendingQty,
                                ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) +
                                           "Issue Qty can not be greater than Receipt Qty.",
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf InStr(ex1.Message, "CCtabSalesOrderItemIssueBalQty", CompareMethod.Text) Then

                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                MSGBox.Message_text.PendingQty,
                                ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) +
                                           "Issue Qty can not be greater than Sales Order Qty.",
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf InStr(ex1.Message, "CCtabOrderItemEROQty", CompareMethod.Text) Then

                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                MSGBox.Message_text.PendingQty,
                                ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) +
                                           "Issue Qty can not be greater than Exchange/Repair/Overhaul Qty.",
                                MsgBoxStyle.OkOnly,
                                "")

            Else

                MSGBoxCtrl.show(MSGBox.Message_title.CheckQty,
                                MSGBox.Message_text.CheckQty,
                                ex1.Message,
                                MsgBoxStyle.OkOnly,
                                "")

            End If

        Finally
            IssueClone = Nothing
        End Try

    End Sub

    'Added By Vikrant On 24-July-2014 For BA24072014
    Private Function CheckDateForTransactionLock(TransDate As Date) As Boolean
        Dim FirstDayofLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
        Dim FirstDayofMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)
        If (TransDate >= FirstDayofLastMonth) Then
            If (TransDate < FirstDayofMonth) And (Day(Today.Date) > 10) Then
                If mIssue.StatusID = 4 Then
                    mIssue.StatusID = 2
                    Session("mIssue") = mIssue
                End If
                Return True
            Else
                Return False
            End If
        Else
            If mIssue.StatusID = 4 Then
                mIssue.StatusID = 2
                Session("mIssue") = mIssue
            End If
            Return True
        End If
    End Function
    'End

    Private Sub ValidationCode()
        Dim strMSG As String = ""
        If Not mIssue.IsValid Then
            For i As Integer = 0 To mIssue.GetBrokenRulesCollection.Count - 1
                strMSG = strMSG + mIssue.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        'Added by vikrant on 26-aug-2011-----------------------------------
        Dim mIssueItem As IssueItem
        If Not mIssue.IssueItems.IsValid Then
            For Each mIssueItem In mIssue.IssueItems
                For i As Integer = 0 To mIssueItem.GetBrokenRulesCollection.Count - 1
                    strMSG = strMSG + mIssueItem.ItemName + " : " + mIssueItem.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If
        '-------------------------------------------------------------------
        If strMSG.Trim <> "" Then
            Session("strMSG") = strMSG
            cvControlValidator.ErrorMessage = strMSG
            cvControlValidator.IsValid = False
        End If
    End Sub

    Private Sub SetObject()
        mIssue.IDate = CDate(txtIssueDate.Text)
        mIssue.StoreID = New Guid(cmbStoreList.SelectedValue)
        mIssue.nWOID = New Guid(cmbWorkOrder.SelectedValue)
        mIssue.Remark = Trim(txtRemark.Text)
        mIssue.Text = txtText.Text
        mIssue.No = Val(txtNo.Text)
        mIssue.MachineID = New Guid(cmbAircraftList.SelectedValue)
        mIssue.ReferenceNo = Trim(txtRequisitionRef.Text)  'Here Reference No. Is Requisition Ref No. user will add. Added by Prashant 28-Jan-2019 'ALL28012019
        mIssue.UserName = User.Identity.Name
        'mIssue.ToolsIssuedToEmployeeID = New Guid(cmbIssuedToEmployee.SelectedValue)
        'mIssue.ToolsIssuedToEmployeeName = Trim(txtIssueToEmpName.Text)
        'mIssue.ToolsReceivedByEmployeeID = New Guid(cmbReceivedByEmployee.SelectedValue)
        'mIssue.ToolsReceivedByEmployeeName = Trim(txtIssuedByEmployee.Text)
    End Sub

    Private Sub DeleteRecord(Index As Int32)

        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Remove")
        mIssue.IssueItems.CurrentIndex = Index
        Session("mIssue") = mIssue

    End Sub

    Private Sub ReturnWOQty()
        Dim mtmpIssueItem As IssueItem
        For Each mtmpIssueItem In mIssue.IssueItems
            mtmpIssueItem.DisplayQty = mtmpIssueItem.DisplayQty - mtmpIssueItem.WOReturnQty
            mtmpIssueItem.WOReturnQty = 0

        Next
        mIssue.RemoveQtyZeroItems()
        'Added By Vikrant On 21-Dec-2016 For ALL21122016-1
        dgIssueItems.Columns(1).Visible = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", True, False)
        dgIssueItems.Columns(1).HeaderText = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo", "Code No.", "GSE No.")
        'End
        dgIssueItems.DataSource = mIssue.IssueItems
        dgIssueItems.DataBind()
        Session("mIssue") = mIssue
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Remove" Then
                        Try
                            Session("Sender") = ""
                            Dim mIssue As Issue
                            mIssue = CType(Session("mIssue"), Issue)
                            mIssue.IssueItems.Remove(mIssue.IssueItems.CurrentItem)
                            mIssue.CalculateTotal()
                            Session("mIssue") = mIssue
                            'Added By Vikrant On 21-Dec-2016 For ALL21122016-1
                            dgIssueItems.Columns(1).Visible = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", True, False)
                            dgIssueItems.Columns(1).HeaderText = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo", "Code No.", "GSE No.")
                            'End
                            dgIssueItems.DataSource = mIssue.IssueItems
                            dgIssueItems.DataBind()
                            ControlVisibility()

                            upnlIssueItem.Update()
                            upnlIssueDetails.Update()
                            upnlActionBtn.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
                        'Added Code
                        Session("sender") = ""
                        Page.Validate("1")
                        If Page.IsValid Then
                            DataFieldBind()
                            'mIssue.StatusID = 2
                            Session("mIssue") = mIssue
                            If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                                Exit Sub
                            End If
                            Session.Remove("IsValid")
                            If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
                            mIssue.StatusID = 2
                            Session("mIssue") = mIssue
                            Save()
                        Else
                            Session.Remove("IsValid")
                            upnlValidationSummary.Update()
                            'Response.Redirect("wfToolsCheckOut_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        Page.Validate("1")
                        If Page.IsValid Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            mIssue.StatusID = 2
                            Session("mIssue") = mIssue
                            Save()
                        Else
                            Session.Remove("IsValid")
                            upnlValidationSummary.Update()
                            'Response.Redirect("wfToolsCheckOut_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        If mIssue.IsNew Then
                            Session.Remove("mIssue")
                        End If
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        If mIssue.StatusID = 2 Then
                            mIssue.StatusID = 1
                        End If
                        Session("mIssue") = mIssue
                        SetControlStatus(mIssue.StatusID, mIsForWOReturn)
                        upnlIssueDetails.Update()
                        'Response.Redirect("wfToolsCheckOut_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        '------------------------------------------------------------------------------------------
                        '-----------------Added by Vikrant on 26-aug-2011--------------------------
                    Else
                        Session("Sender") = ""
                        'Response.Redirect("wfToolsCheckOut_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "ResetIssuedToEmployee" Then
                        txtIssuedToEmployee.Text = ""
                        txtIssuedToEmployee.DataBind()
                        mIssue.ToolsIssuedToEmployeeID = Guid.Empty
                        mIssue.ToolsIssuedToEmployeeName = ""
                        hdnIssuedToEmployeeId.Value = ""
                        upnlIssueDetails.Update()
                    ElseIf MSGBoxCtrl.Sender = "ResetIssuedByEmployee" Then
                        txtIssuedByEmployee.Text = ""
                        txtIssuedByEmployee.DataBind()
                        hdnIssuedByEmployeeId.Value = ""
                        mIssue.ToolsReceivedByEmployeeID = Guid.Empty
                        mIssue.ToolsReceivedByEmployeeName = ""
                        upnlIssueDetails.Update()
                    ElseIf MSGBoxCtrl.Sender = "ResetCollectedByEmployee" Then
                        txtCollectedByEmployee.Text = ""
                        txtCollectedByEmployee.DataBind()
                        hdnCollectedByEmployeeId.Value = ""
                        mIssue.ToolsCollectedByEmployeeID = Guid.Empty
                        mIssue.ToolsCollectedByEmployeeName = ""
                        upnlIssueDetails.Update()
                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        If mIssue.StatusID = 2 Then
                            mIssue.StatusID = 1
                        End If
                        Session("sender") = ""
                        Session("mIssue") = mIssue
                        ''========================================
                        DataFieldBind()
                        upnlIssueDetails.Update()

                        'Response.Redirect("wfToolsCheckOut_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    ElseIf MSGBoxCtrl.Sender = "ResetFromStore" Then
                        cmbStoreList.ClearSelection()
                        upnlIssueDetails.Update()
                    ElseIf MSGBoxCtrl.Sender = "DuplicateBarcode" Then
                        txtBarcodeItem.Text = ""
                        txtBarcodeItem.DataBind()
                        upnlIssueItem.Update()
                    Else
                        Session("sender") = ""
                    End If
            End Select
        ElseIf Result1 = -1 Then
            If mIssue.StatusID = 2 And Session("sender") <> "Close" Then
                mIssue.StatusID = 1
            End If
            Session("mIssue") = mIssue
            Session("sender") = ""
            upnlIssueDetails.Update()
            'Response.Redirect("wfToolsCheckOut_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub

    Private Sub SetControlStatus(StatusId As Int16, Optional IsForWOReturn As Boolean = False)
        If mOpenFrom = "FromwfStockCard" Or mOpenFrom = "FromReqItemStatusReport" Then 'Added By Prashant 3-Apr-2014 ALL03042014
            btnAddItem.Enabled = False
            txtRemark.Enabled = False
            txtText.Enabled = False
            txtNo.Enabled = False
        Else
            btnAddItem.Enabled = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True)
            txtRemark.Enabled = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True)
            txtText.Enabled = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True)
            txtNo.Enabled = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True)
        End If
    End Sub

    Private Sub ControlVisibility()

        dgIssueItems.Columns(10).Visible = IIf(mIssue.StatusID = 1, True, False)
        'Added By Prashant 17-Aug-2011
        If Not IsInRole(Rights.Authorized) Then
            btnAuthorized.Enabled = False
            btnAuthorized.ToolTip = "You are not authorized user "
        End If

        txtText.Enabled = (CType(IIf(mIssue.StatusID >= 2, False, True), Boolean))
        txtNo.Enabled = (CType(IIf(mIssue.StatusID >= 2, False, True), Boolean))
        cmbAircraftList.Enabled = (CType(IIf(mIssue.StatusID >= 2, False, True), Boolean))
        cmbWorkOrder.Enabled = (CType(IIf(mIssue.StatusID >= 2, False, True), Boolean))
        txtIssueDate.Enabled = (CType(IIf(mIssue.StatusID >= 2, False, True), Boolean) And mIssue.IssueItems.Count = 0) Or (mIssue.IssueItems.Count = 0)
        'Commneted by Prashant on 6-Sep-2021 as star air need to select Employee them self
        'If mIssue.TransTypeID = 79 And mIssue.ToTypeID = 18 Then 'mIssue.ToTypeID = 18 against requisition 'Added By Prashant on 17-May-2021 ALL17052021
        '    txtIssuedToEmployee.Enabled = (CType(IIf(mIssue.StatusID >= 2, False, True), Boolean) And mIssue.IssueItems.Count = 0) Or (mIssue.IssueItems.Count = 0)
        'Else
        txtIssuedToEmployee.Enabled = (CType(IIf(mIssue.StatusID >= 2, False, True), Boolean))
        'End If
        txtIssuedByEmployee.Enabled = (CType(IIf(mIssue.StatusID >= 2, False, True), Boolean))
        cmbStoreList.Enabled = (CType(IIf(mIssue.StatusID >= 2, False, True), Boolean) And mIssue.IssueItems.Count = 0) Or (mIssue.IssueItems.Count = 0)
        txtCollectedByEmployee.Enabled = (CType(IIf(mIssue.StatusID >= 2, False, True), Boolean))
    End Sub

    'Added by Saylee on 15-July-2010
    Private Function ISInDate() As Boolean
        Dim dueDay As Integer = CType(AppSettings("dueDay"), Integer)
        Dim tmpDate As Date = New Date(Year(mIssue.IDate), Month(mIssue.IDate) + 1, dueDay)   ''New Date(2010, 7,  7)
        Dim PrevtmpDate As Date = New Date(Year(mIssue.IDate), Month(mIssue.IDate), 1)

        If Today.Date >= PrevtmpDate And (Today.Date <= tmpDate) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function IsInRole(CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "ToolsCheckOut"
        Select Case CheckFor
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function

    Private Sub AddAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub

    Private Function VendorStatus(TransTypeID As Integer, Type As RequestFor) As Boolean

        If Type = RequestFor.Supplier Then 'Issue

            Select Case CType(TransTypeID, Trans)
                Case Trans.ExchangeRepairIssueToVendor
                    Return True
                Case Trans.LoanIssueToVendor
                    Return True
                Case Trans.IssuetoSupplierNone
                    Return True
                Case Else
                    Return False
            End Select

        ElseIf Type = RequestFor.Customer Then 'Issue

            Select Case CType(TransTypeID, Trans)
                Case Trans.IssueToCustomer
                    Return True
                Case Trans.LoanIssueToCustomer
                    Return True
                Case Trans.IssueToCustomerAsRepairedReturn
                    Return True
                Case Else
                    Return False
            End Select

        End If

    End Function

    Public Sub SetReport()
        Dim objIssue As rptIssues
        Dim objChilds As rptIssueChields
        Dim letter As rptLetterHead
        Dim ds As New dsIssue
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

        If AppSettings("ClientCode") = "STR" Then
            myReport = New crptToolsCheckOutStarAir  'Added by Shital on 25-Sep-2019
        Else
            myReport = New crptToolsCheckOut
        End If

        objIssue = rptIssues.GetIssues(mIssue.ID)
        objChilds = rptIssueChields.GetIssuechilds(mIssue.ID)

        '---------- 'Addded by vikrant on 7-sept-2011------------
        Dim mSearchstring As String
        If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then
            mSearchstring = "True"
        Else
            mSearchstring = "False"
        End If
        '--------------------------------------------------------
        letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "",
                                                 mSearchstring, AppSettings("Logo"), Today.Date.ToString(AppSettings("DateFormat")),
                                                 ClientCode:=AppSettings("ClientCode"))
        If letter.Count > 0 Then
            BaseCurrencysymbol = letter(0).BaseCurrencysymbol
            Session("BaseCurrencysymbol") = BaseCurrencysymbol
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, objIssue)
        da.Fill(ds, objChilds)
        da.Fill(ds, letter)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
    End Sub

    Private Sub AddPartForNewRequisition() 'Added By Prashant on 17-May-2021 ALL17052021
        Dim mRequisitionItemsNew As RequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForList(mIssue.IDate.ToString, "", mIssue.IssueItems.CurrentItem.ItemID, 0, , , mIssue.MachineID.ToString, , mIssue.RequisitionID.ToString)
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
    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()
        mStoreList = StoreList.GetStoreList(0, , True)
        mTypeList1 = TypeList1.GetTypeList("3", mIssue.TransTypeID)             'For Issue


        If mIssue.TransTypeID = 19 Then 'Discard 'Added By Prashant 5-July-2011
            mVendorList = VendorList.GetVendortList(0, , , , , , True)
        Else
            mVendorList = VendorList.GetVendortList(0, , , , , , True, VendorStatus(mIssue.TransTypeID, RequestFor.Customer), VendorStatus(mIssue.TransTypeID, RequestFor.Supplier))
        End If

        mMachineNameValueList = MachineNameValueList.GetMachineList(mIssue.IDateFormatted.ToString, IsTagRequired:=True, TagText:="(SELECT)", ForInventory:=True)
        mStatusList = StatusList.GetStatusList(mIssue.StatusID, True)
        mnWOListForCombo = nWOListForCombo.GetnWOListForCombo("(SELECT)", , , New SmartDate("01-01-1800").FormattedText, New SmartDate(mIssue.IDate.ToString).FormattedText, , , 2)
        mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(SELECT)")

        cmbStoreList.DataSource = mStoreList
        cmbAircraftList.DataSource = mMachineNameValueList
        cmbWorkOrder.DataSource = mnWOListForCombo
        dgIssueItems.DataSource = mIssue.IssueItems
        txtIssueDate.Text = mIssue.IDateFormatted

        SetSession()

        cmbWorkOrder.DataSource = mnWOListForCombo
        txtIssuedToEmployee.Text = mIssue.ToolsIssuedToEmployeeName
        txtIssuedByEmployee.Text = mIssue.ToolsReceivedByEmployeeName
        txtCollectedByEmployee.Text = mIssue.ToolsCollectedByEmployeeName
        'Added By Vikrant On 21-Dec-2016 For ALL21122016-1
        dgIssueItems.Columns(1).Visible = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", True, False)
        dgIssueItems.Columns(1).HeaderText = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo", "Code No.", "GSE No.")
        'End
        DataBind()

    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GetSession()
        AddAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        mOpenFrom = Request.QueryString("Type")  'Added By Prashant 3-Apr-2014 ALL03042014
        SetControlStatus(mIssue.StatusID, mIsForWOReturn)
        If Not IsPostBack And Session("sender") = "" Then
            'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If Session("sender") = "ReceiptCumInvoiceCreation" Then
                    '
                Else
                    If mIssue.IsNew Then
                        mIssue.Text = Session("TransText_ForTransSeries")
                        txtText.Text = mIssue.Text
                        Session("mIssue") = mIssue
                        Session("AddTransTextSeries") = "False"
                        Session.Remove("TransName_ForTransSeries")
                        Session.Remove("TransText_ForTransSeries")
                        Session.Remove("TransNo_ForTransSeries")
                    End If
                End If

            End If
            'End
            If AppSettings("AutoCompleteTransText") = "False" Then 'Added By Utkarsh ON 23-May-2012 FOR 23052012 
                If txtText.Enabled = True Then
                    txtText.Focus()
                End If
            End If
            If ((mIssue.TransTypeID = 79 And mIssue.ToTypeID = 18) And Session("NewRequisition") = "True") Then 'Added By Prashant on 17-May-2021 ALL17052021
                AddPartForNewRequisition()
                Session("NewRequisition") = "False"
            Else
                Session("NewRequisition") = "False"
            End If
            DataFieldBind()
            SetPage()
            ControlVisibility()
        End If

    End Sub

    'The Change made in the Date will effect to Issue Text and No.
    Private Sub IssueDateChanged(sender As Object, e As EventArgs) Handles txtIssueDate.TextChanged

        mIssue.IDate = txtIssueDate.Text
        txtText.Text = mIssue.Text 'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries
        mnWOListForCombo = nWOListForCombo.GetnWOListForCombo("(SELECT)", , , New SmartDate("01-01-1800").FormattedText, New SmartDate(mIssue.IDate.ToString).FormattedText, , , 2)
        cmbWorkOrder.DataSource = mnWOListForCombo
        cmbWorkOrder.DataBind()
        Session("mWOList") = mnWOListForCombo

    End Sub

    Private Sub AddItem(sender As Object, e As EventArgs) Handles btnAddItem.Click

        If IsValid Then
            SetObject()
            mIssue.IssueItems.Add(mIssue.ID, mIssue.TransTypeID)
            Session("mIssue") = mIssue
            If (mIssue.TransTypeID = 79 And mIssue.ToTypeID = 18) Then 'Added By Prashant on 17-May-2021 ALL17052021
                Response.Redirect("wfRequisitionItemListForIssue_Ajax.aspx?BackPage=index.aspx&ChildPage=wfToolsCheckOut_Ajax.aspx&Name=")
            Else
                Response.Redirect("wfPartStockStatus_Ajax.aspx?BackPage=index.aspx&ChildPage=wfToolsCheckOut_Ajax.aspx&Name=")
            End If
        Else
            upnlValidationSummary.Update()
        End If

    End Sub

    Private Sub GridViewRowCommand(source As Object, e As Web.UI.WebControls.GridViewCommandEventArgs) Handles dgIssueItems.RowCommand

        Select Case e.CommandName
            Case "DeleteRec"
                Dim Index As Int32 = CInt(e.CommandArgument) - 1
                DeleteRecord(Index)
        End Select

    End Sub

    Private Sub Back(sender As Object, e As EventArgs) Handles btnBack.Click

        'MarkLog(Util.Action.Close, ModuleName, "", Util.ErrorType.NoError, Guid.Empty)
        SetObject()
        mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + IIf(mIssueTo <> "", " to " + mIssueTo, "")
        MarkLog(Action.Close, ModuleName, mIssueDetail, ErrorType.NoError, mIssue.ID, EventLogID)

        If Not mOpenFrom Is Nothing AndAlso mOpenFrom = "FromwfStockCard" Then  'Added By Prashant 3-Apr-2014 ALL03042014
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        ElseIf Not mOpenFrom Is Nothing AndAlso mOpenFrom = "FromReqItemStatusReport" Then 'Added By Vikrant on 13-Oct-2014 For Req Item Status Report
            RemoveSession()
            Session.Remove("ModuleName")
            Response.Redirect("Index.aspx")
        End If

        Session("IsValid") = IsValid
        If mIssue.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
            If IsValid Then
                SetObject()
            End If
        Else
            RemoveSession()
            Session.Remove("ModuleName")
            Response.Redirect("Index.aspx")
        End If

    End Sub

    Private Sub Print(sender As Object, e As EventArgs) Handles btnPrint.Click

        If Not IsInRole(Rights.Print) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"), True)
            Exit Sub
        End If
        SetReport() 'Added By Prashant 16-Sep-2013 ALL16092013
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)

    End Sub

    Private Sub StoreChanged(sender As Object, e As EventArgs) Handles cmbStoreList.SelectedIndexChanged

        mUserHasNoStoreRights = UserHasNoStoreRights.GetUserHasNoStoreRights(User.Identity.Name, cmbStoreList.SelectedValue.ToString) 'Added By Prashant 31-Oct-2018 ALL30102018
        If mUserHasNoStoreRights.Count > 0 Then
            MSGBoxCtrl.Show("Alert!", "Sorry you do not have rights to select this store. Please contact with admin.", "", MsgBoxStyle.OkOnly, "ResetFromStore")
        End If

        cmbStoreList.Focus()

    End Sub

    Private Sub AircraftChanged(sender As Object, e As EventArgs) Handles cmbAircraftList.SelectedIndexChanged

        mnWOListForCombo = nWOListForCombo.GetnWOListForCombo("(SELECT)", , , New SmartDate("01-01-1900").FormattedText, New SmartDate(mIssue.IDate.ToString).FormattedText, IIf(cmbAircraftList.SelectedIndex <= 0, "", cmbAircraftList.SelectedItem.ToString), , 2)
        cmbWorkOrder.DataSource = mnWOListForCombo
        cmbWorkOrder.DataBind()

    End Sub

    Protected Sub IssuedToEmployee(sender As Object, e As EventArgs)

        Dim message As String = ""

        If IsNumeric(txtIssuedToEmployee.Text) Then

            Dim mEmployeeListForCombo As EmployeeListForCombo
            mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo(BarcodeNo:=txtIssuedToEmployee.Text)

            If mEmployeeListForCombo.Count > 0 Then

                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeListForCombo(0).ID.ToString, mIssue.IDateFormatted.ToString)

                If mEmployeeStatus.Count > 0 Then

                    If (mEmployeeStatus(0).Information <> "") Then

                        message = mEmployeeStatus(0).Information
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "ResetIssuedToEmployee")
                        Exit Sub

                    End If

                    txtIssuedToEmployee.Text = mEmployeeListForCombo(0).LicenceNoName
                    txtIssuedToEmployee.DataBind()
                    mIssue.ToolsIssuedToEmployeeID = New Guid(mEmployeeListForCombo(0).ID.ToString)
                    mIssue.ToolsIssuedToEmployeeName = mEmployeeListForCombo(0).LicenceNoName
                    Session("mIssue") = mIssue

                End If

                Exit Sub

            End If

        End If

        If hdnIssuedToEmployeeId.Value <> "" Then

            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(hdnIssuedToEmployeeId.Value.ToString, mIssue.IDateFormatted.ToString)

            If mEmployeeStatus.Count > 0 Then

                If (mEmployeeStatus(0).Information <> "") Then

                    message = mEmployeeStatus(0).Information
                    MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.SaveAlert,
                                    MessageText:=MSGBox.Message_text.Custom,
                                    ExtraMessage:=message,
                                    ButtonToShow:=MsgBoxStyle.OkOnly,
                                    Sender:="ResetIssuedToEmployee")
                    Exit Sub

                End If

                mIssue.ToolsIssuedToEmployeeID = New Guid(hdnIssuedToEmployeeId.Value)
                mIssue.ToolsIssuedToEmployeeName = txtIssuedToEmployee.Text

            Else

                txtIssuedToEmployee.Text = ""
                mIssue.ToolsIssuedToEmployeeID = Guid.Empty
                mIssue.ToolsIssuedToEmployeeName = ""

            End If

        Else

            If txtIssuedToEmployee.Text <> "" Then

                mEmployeeList = EmployeeList.GetEmployeeList()
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(EmployeeID:=mEmployeeList(EmpNoName:=txtIssuedToEmployee.Text, "").ID.ToString,
                                                                          EDate:=mIssue.IDateFormatted.ToString)

                If mEmployeeStatus.Count > 0 Then

                    If (mEmployeeStatus(0).Information <> "") Then

                        message = mEmployeeStatus(0).Information
                        MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.SaveAlert,
                                        MessageText:=MSGBox.Message_text.Custom,
                                        ExtraMessage:=message,
                                        ButtonToShow:=MsgBoxStyle.OkOnly,
                                        Sender:="ResetIssuedToEmployee")

                        Exit Sub

                    End If

                    mIssue.ToolsIssuedToEmployeeID = mEmployeeList(txtIssuedToEmployee.Text, "").ID
                    mIssue.ToolsIssuedToEmployeeName = txtIssuedToEmployee.Text

                Else

                    txtIssuedToEmployee.Text = ""
                    mIssue.ToolsIssuedToEmployeeID = Guid.Empty
                    mIssue.ToolsIssuedToEmployeeName = ""

                End If

            End If

        End If

        Session("mIssue") = mIssue

    End Sub

    Protected Sub IssuedByEmployee(sender As Object, e As EventArgs)

        Dim message As String = ""

        If IsNumeric(txtIssuedByEmployee.Text) Then

            Dim mEmployeeListForCombo As EmployeeListForCombo
            mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo(BarcodeNo:=txtIssuedByEmployee.Text)

            If mEmployeeListForCombo.Count > 0 Then

                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(EmployeeID:=mEmployeeListForCombo(0).ID.ToString,
                                                                          EDate:=mIssue.IDateFormatted.ToString)
                If mEmployeeStatus.Count > 0 Then

                    If (mEmployeeStatus(0).Information <> "") Then

                        message = mEmployeeStatus(0).Information
                        MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.SaveAlert,
                                        MessageText:=MSGBox.Message_text.Custom,
                                        ExtraMessage:=message,
                                        ButtonToShow:=MsgBoxStyle.OkOnly,
                                        Sender:="ResetIssuedByEmployee")
                        Exit Sub

                    End If

                    txtIssuedByEmployee.Text = mEmployeeListForCombo(0).LicenceNoName
                    mIssue.ToolsReceivedByEmployeeID = New Guid(mEmployeeListForCombo(0).ID.ToString)
                    mIssue.ToolsReceivedByEmployeeName = mEmployeeListForCombo(0).LicenceNoName
                    Session("mIssue") = mIssue

                End If

                Exit Sub

            End If

        End If

        If hdnIssuedByEmployeeId.Value <> "" Then

            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(EmployeeID:=hdnIssuedByEmployeeId.Value.ToString,
                                                                      EDate:=mIssue.IDateFormatted.ToString)
            If mEmployeeStatus.Count > 0 Then

                If (mEmployeeStatus(0).Information <> "") Then

                    message = mEmployeeStatus(0).Information
                    MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.SaveAlert,
                                    MessageText:=MSGBox.Message_text.Custom,
                                    ExtraMessage:=message,
                                    ButtonToShow:=MsgBoxStyle.OkOnly,
                                    Sender:="ResetIssuedByEmployee")
                    Exit Sub

                End If

                mIssue.ToolsReceivedByEmployeeID = New Guid(hdnIssuedByEmployeeId.Value)
                mIssue.ToolsReceivedByEmployeeName = txtIssuedByEmployee.Text

            Else

                txtIssuedByEmployee.Text = ""
                mIssue.ToolsReceivedByEmployeeID = Guid.Empty
                mIssue.ToolsReceivedByEmployeeName = ""

            End If

        Else

            If txtIssuedByEmployee.Text <> "" Then

                mEmployeeList = EmployeeList.GetEmployeeList()
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(EmployeeID:=mEmployeeList(EmpNoName:=txtIssuedByEmployee.Text, "").ID.ToString,
                                                                          EDate:=mIssue.IDateFormatted.ToString)
                If mEmployeeStatus.Count > 0 Then

                    If (mEmployeeStatus(0).Information <> "") Then

                        message = mEmployeeStatus(0).Information
                        MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.SaveAlert,
                                        MessageText:=MSGBox.Message_text.Custom,
                                        ExtraMessage:=message,
                                        ButtonToShow:=MsgBoxStyle.OkOnly,
                                        Sender:="ResetIssuedByEmployee")

                        Exit Sub

                    End If

                    mIssue.ToolsReceivedByEmployeeID = mEmployeeList(txtIssuedByEmployee.Text, "").ID
                    mIssue.ToolsReceivedByEmployeeName = txtIssuedByEmployee.Text

                Else

                    txtIssuedByEmployee.Text = ""
                    mIssue.ToolsReceivedByEmployeeID = Guid.Empty
                    mIssue.ToolsReceivedByEmployeeName = ""

                End If

            End If

        End If

        Session("mIssue") = mIssue

    End Sub

    Protected Sub CollectedByEmployee(sender As Object, e As EventArgs)

        Dim message As String = ""

        If IsNumeric(txtCollectedByEmployee.Text) Then

            Dim mEmployeeListForCombo As EmployeeListForCombo
            mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo(BarcodeNo:=txtCollectedByEmployee.Text)

            If mEmployeeListForCombo.Count > 0 Then

                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(EmployeeID:=mEmployeeListForCombo(0).ID.ToString,
                                                                          EDate:=mIssue.IDateFormatted.ToString)
                If mEmployeeStatus.Count > 0 Then

                    If (mEmployeeStatus(0).Information <> "") Then

                        message = mEmployeeStatus(0).Information
                        MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.SaveAlert,
                                        MessageText:=MSGBox.Message_text.Custom,
                                        ExtraMessage:=message,
                                        ButtonToShow:=MsgBoxStyle.OkOnly,
                                        Sender:="ResetCollectedByEmployee")

                        Exit Sub

                    End If

                    txtCollectedByEmployee.Text = mEmployeeListForCombo(0).LicenceNoName
                    mIssue.ToolsCollectedByEmployeeID = New Guid(mEmployeeListForCombo(0).ID.ToString)
                    mIssue.ToolsCollectedByEmployeeName = mEmployeeListForCombo(0).LicenceNoName
                    Session("mIssue") = mIssue

                End If

                Exit Sub

            End If

        End If

        If hdnCollectedByEmployeeId.Value <> "" Then

            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(EmployeeID:=hdnCollectedByEmployeeId.Value.ToString,
                                                                      EDate:=mIssue.IDateFormatted.ToString)
            If mEmployeeStatus.Count > 0 Then

                If (mEmployeeStatus(0).Information <> "") Then

                    message = mEmployeeStatus(0).Information
                    MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.SaveAlert,
                                    MessageText:=MSGBox.Message_text.Custom,
                                    ExtraMessage:=message,
                                    ButtonToShow:=MsgBoxStyle.OkOnly,
                                    Sender:="ResetCollectedByEmployee")

                    Exit Sub

                End If

                mIssue.ToolsCollectedByEmployeeID = New Guid(hdnCollectedByEmployeeId.Value)
                mIssue.ToolsCollectedByEmployeeName = txtCollectedByEmployee.Text

            Else

                txtCollectedByEmployee.Text = ""
                mIssue.ToolsCollectedByEmployeeID = Guid.Empty
                mIssue.ToolsCollectedByEmployeeName = ""

            End If
        Else

            If txtCollectedByEmployee.Text <> "" Then

                mEmployeeList = EmployeeList.GetEmployeeList()
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(EmployeeID:=mEmployeeList(EmpNoName:=txtCollectedByEmployee.Text, str:="").ID.ToString,
                                                                          EDate:=mIssue.IDateFormatted.ToString)
                If mEmployeeStatus.Count > 0 Then

                    If (mEmployeeStatus(0).Information <> "") Then

                        message = mEmployeeStatus(0).Information
                        MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.SaveAlert,
                                        MessageText:=MSGBox.Message_text.Custom,
                                        ExtraMessage:=message,
                                        ButtonToShow:=MsgBoxStyle.OkOnly,
                                        Sender:="ResetCollectedByEmployee")
                        Exit Sub

                    End If

                    mIssue.ToolsCollectedByEmployeeID = mEmployeeList(txtCollectedByEmployee.Text, "").ID
                    mIssue.ToolsCollectedByEmployeeName = txtCollectedByEmployee.Text

                Else

                    txtCollectedByEmployee.Text = ""
                    mIssue.ToolsCollectedByEmployeeID = Guid.Empty
                    mIssue.ToolsCollectedByEmployeeName = ""

                End If

            End If

        End If

        Session("mIssue") = mIssue

    End Sub

#End Region

#Region " Show BrokenRules "

    Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)

        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        If custValidator.ControlToValidate = "txtIssuedToEmployee" Then

            If txtIssuedToEmployee.Text = "" Or mIssue.ToolsIssuedToEmployeeID.Equals(Guid.Empty) Then
                e.IsValid = False
                custValidator.ErrorMessage = "Select Employee to which Tools needs to be issued"
            Else
                e.IsValid = True
            End If

        End If

    End Sub

    Public Sub CustomValidate1(s As Object, e As ServerValidateEventArgs)

        If Flag = 1 Then Exit Sub

        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        Dim strMsg As String = ""
        SetObject()

        If Not mIssue.IsValid Then

            For i As Integer = 0 To mIssue.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mIssue.GetBrokenRulesCollection(i).Description + "<Br>"
            Next

        End If

        Dim mIssueItem As IssueItem

        If Not mIssue.IssueItems.IsValid Then

            For Each mIssueItem In mIssue.IssueItems

                For i As Integer = 0 To mIssueItem.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mIssueItem.ItemName + " : " + mIssueItem.GetBrokenRulesCollection(i).Description + "<Br>"
                Next

            Next

        End If

        If strMsg.Trim <> "" Then

            CustValidator.ErrorMessage = strMsg
            e.IsValid = False

        End If

        Flag = 1

    End Sub

    Public Function CustomValidate2() As Boolean

        Dim strMsg As String = ""

        If txtIssuedToEmployee.Text = "" Or mIssue.ToolsIssuedToEmployeeID.Equals(Guid.Empty) Then

            strMsg = "Select Employee to which Tools needs to be issued"

        End If

        If strMsg <> "" Then

            cvControlValidator.ErrorMessage = strMsg
            cvControlValidator.IsValid = False
            Return False

        End If

        Return True

    End Function

#End Region

#Region " Status "

    'Authorize
    Private Sub Authorized(sender As Object, e As EventArgs) Handles btnAuthorized.Click

        SetObject()
        If Not mIssue.IsValid Then

            ValidationCode()
            upnlValidationSummary.Update()
            Exit Sub

        End If

        If IsValid Then

            If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub

            MSGBoxCtrl.Show("Save Alert!", "You are about to Issue Tool(s).</br></br>Do you want to continue?", "", MsgBoxStyle.YesNo, "Status")
            Session("IsValid") = IsValid

        Else
            upnlValidationSummary.Update()
        End If

    End Sub

#End Region

#Region " Barcode "

    Private Sub BarcodeItem(sender As Object, e As EventArgs) Handles btnAddBarcodeItem.Click

        If IsValid Then

            If txtBarcodeItem.Text <> "" Then

                Dim mPendingItemList As PendingToIssueList
                Dim mItemListByBarcode As ItemListByBarcode
                mItemListByBarcode = ItemListByBarcode.GetItemListByBarcode(txtBarcodeItem.Text)

                If mItemListByBarcode.Count > 0 Then

                    mPendingItemList = PendingToIssueList.GetPendingToIssueListForBarcode(New Guid(cmbStoreList.SelectedValue),
                                                                                          mItemListByBarcode(0).ItemName, "", "", "",
                                                                                          cmbStoreList.SelectedItem.Text, txtIssueDate.Text,
                                                                                          mIssue.TransTypeID, mItemListByBarcode(0).ItemID.ToString, , ,
                                                                                          txtBarcodeItem.Text,
                                                                                          mItemListByBarcode(0).ReceiptItemID.ToString, 2,
                                                                                          ToTypeIDOfIssue:=mIssue.ToTypeID)
                Else

                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Invalid Barcode Number.", False), True)
                    txtBarcodeItem.Text = ""
                    Exit Sub

                End If

                If mPendingItemList.Count > 0 Then '3

                    If mIssue.IssueItems.Contains(mPendingItemList(0).ReceiptItemID) Then '4

                        MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Issue Item", MsgBoxStyle.OkOnly, "DuplicateBarcode")
                        Exit Sub

                    Else '4
                        AddItemByBarcode(mPendingItemList)
                    End If '5

                Else

                    txtBarcodeItem.Text = ""
                    MSGBoxCtrl.Show("Add alert !", "Tool can not be added <br> Tool not present in Stock or Wrong Store selected", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub

                End If '4

            Else

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Invalid Barcode Number.", False), True)
                txtBarcodeItem.Text = ""
                Exit Sub

            End If

        Else

            upnlValidationSummary.Update()

        End If

    End Sub
    Public Sub AddItemByBarcode(mPendingItemList As PendingToIssueList)

        mIssue.IssueItems.Add(mIssue.ID, mIssue.TransTypeID)
        mIssue.IssueItems.CurrentItem.ReceiptItemID = mPendingItemList(0).ReceiptItemID
        mIssue.IssueItems.CurrentItem.DisplayQty = 1
        mIssue.IssueItems.CurrentItem.DisplayUnitID = mPendingItemList(0).UnitID
        mIssue.IssueItems.CurrentItem.DisplayUnitName = mPendingItemList(0).UnitName
        mIssue.StoreID = mPendingItemList(0).StoreID
        mIssue.IssueItems.CurrentItem.CodeNo = mPendingItemList(0).CodeNo

        If mPendingItemList(0).CalibrationDueDateFormatted.ToString <> "" Then
            mIssue.IssueItems.CurrentItem.CalibrationDueDate = mPendingItemList(0).CalibrationDueDateFormatted.ToString
        Else
            mIssue.IssueItems.CurrentItem.CalibrationDueDate = DBNull.Value
        End If

        Session("mIssue") = mIssue
        'Added By Vikrant On 21-Dec-2016 For ALL21122016-1
        dgIssueItems.Columns(1).Visible = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", True, False)
        dgIssueItems.Columns(1).HeaderText = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo", "Code No.", "GSE No.")
        'End
        dgIssueItems.DataSource = mIssue.IssueItems
        dgIssueItems.DataBind()
        cmbStoreList.SelectedValue = mPendingItemList(0).StoreID.ToString
        txtBarcodeItem.Text = ""
        txtBarcodeItem.Focus()
        ControlVisibility()

        If mIssue.IssueItems.Count = 1 Then

            txtIssuedToEmployee.Enabled = True
            txtIssuedByEmployee.Enabled = True
            txtCollectedByEmployee.Enabled = False

        End If

        upnlIssueDetails.Update()

    End Sub
    Private Sub MSGBoxCtrl_UserControl(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked

        MessageBoxResult()

    End Sub

#End Region

#Region "Service Methods"

    <Services.WebMethod(), Script.Services.ScriptMethod()>
    Public Shared Function GetEmployeeList(prefixText As String, count As Integer, contextKey As String) As String()

        Dim itemList As EmpNoNameAutoComplete
        itemList = EmpNoNameAutoComplete.GeEmpNoNameList(prefixText)
        If count = 0 Then
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemList
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).ToArray
        Else
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemList
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).Take(count).ToArray
        End If

    End Function

#End Region

End Class