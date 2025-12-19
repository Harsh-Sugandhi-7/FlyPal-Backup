'AJAX Conversion by Vikrant
Imports System.Text
Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Imports System.ComponentModel
Public Class wfIssue_Ajax
    Inherits System.Web.UI.Page

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
    Private Enum RequstFor
        Supplier = 0
        Customer = 1
    End Enum
#End Region

#Region " Variable Declaration "
    Public mIssue As Issue
    Dim mStoreList As StoreList
    Dim mTypeList1 As TypeList1
    Public mVendorList As VendorList
    'Dim mMachineList As FlyPal22.Maintain.SelectList 'Dim mMachineList As tmpMachineList
    'Dim mMachineNameValueList As MachineNameValueList
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
    Public mRequisitionItemTypeList As RequisitionItemTypeList
    Dim mMachineNameValueList As MachineNameValueList
    Dim email As Thread
    Public mUserHasNoStoreRights As UserHasNoStoreRights
    Public mTransactionList As TransactionList   'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Public mEmployeeList As EmployeeList
    Public mEmployeeStatus As EmployeeStatus
    Public S As String
    Dim mFileAttachments As FileAttachments = New FileAttachments() 'Sankalp 29-09-25
    Dim IsAttachmentDeleted As Boolean = False
    Public mIsAttachmentNotSave As Boolean = True
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
        'mWOList = Session("mWOList")
        mnWOListForCombo = Session("mnWOListForCombo")
        mWorkShopList = Session("mWorkShopList")
        mVendorTerms = Session("mVendorTerms")
        mIsForWOReturn = CType(Session("IsForWOReturn"), Boolean) 'Added by Saylee on 13-Dec-2010
        mTransactionList = Session("mTransactionList")      'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 

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
        'Session("mWOList") = mWOList
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
        'Session.Remove("mWOList")
        Session.Remove("mnWOListForCombo")
        Session.Remove("Edit")
        Session.Remove("mWorkShopList")
        Session.Remove("mVendorTerms")
    End Sub
    Private Sub SetPage()
        If mIssue.No > 0 Then
            lblTitle.Text = Session("ModuleName") + " [" + mIssue.Text + "-" + CType(mIssue.No, String) + "]"
        Else
            lblTitle.Text = Session("ModuleName") + " [ New ]"
        End If
    End Sub
    Private Sub Enable()
        txtIssueDate.Enabled = True
        cmbStoreList.Enabled = True
        cmbLocationStore.Enabled = True
        cmbVendorList.Enabled = True
        cmbAircraftList.Enabled = True
        txtIssueDate.Enabled = True
        cmbWorkShop.Enabled = True
        cmbWorkOrder.Enabled = True
    End Sub
    Private Sub Disable()
        txtIssueDate.Enabled = False
        cmbStoreList.Enabled = False
        cmbLocationStore.Enabled = False
        cmbVendorList.Enabled = False
        cmbAircraftList.Enabled = False
        txtIssueDate.Enabled = False
        cmbWorkShop.Enabled = False
        cmbWorkOrder.Enabled = False
    End Sub
    Private Sub SaveIsSerializedExists()
        'Authentication
        If Not mIssue.IDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                'Changes by Kalpesh in 13-3-2013
                'These lines commented
                '
                'Dim strOutString As String = ReadXMLFile()
                'strOutString = strOutString.Split(CChar("$"))(1)
                'Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, CInt(strOutString), mCheck.SubscriptionDate)

                'Changes by Kalpesh in 13-3-2013
                'These lines commented
                '
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------

                If DateDiff(DateInterval.Day, CDate(mIssue.IDate), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert, MSGBox.Message_Text.saveAlert, " Your subscription has been expired. can not save Issue. <br> Issue Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
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
                    '-------------------------------------------------------------------

                    If strMSG.Trim <> "" Then
                        Session("strMSG") = strMSG
                        cvControlValidator.ErrorMessage = strMSG
                        cvControlValidator.IsValid = False
                    End If
                End If
                'MarkLog(Util.Action.Save, ModuleName, mIssue.IssueNo, Util.ErrorType.NoError, mIssue.ID)
                IssueTo()
                mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + IIf(mIssueTo <> "", " to " + mIssueTo, "")

                If mIssue.StatusID = 2 Then
                    MarkLog(Util.Action.Authorize, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                ElseIf mIssue.StatusID = 3 Then
                    MarkLog(Util.Action.Amend, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                ElseIf mIssue.StatusID = 4 Then
                    MarkLog(Util.Action.Cancel, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                Else
                    MarkLog(Util.Action.Save, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                End If

                mIssue.MarkClean()
                Session("mIssue") = mIssue
                'Added By Utkarsh ON 22-Jul-2013 FOR ALL19072013
                If mIssue.StatusID = 2 Then
                    If mIssue.TransTypeID = 19 Then  'Issue to discard   Added By Prashant 16-Sep-2013 ALL16092013 
                        SendMail()
                    End If
                    'Else
                    'Response.Redirect("wfIssue.aspx?BackPage=" & Request.QueryString("BackPage"), False)   
                End If
                'End

                SetPage()
                ControlVisibility()
                If mIssue.IsNew Then
                    lblStatus.Text = "OPEN"
                End If
                If mIssue.IssueItems.Count > 0 Then
                    Disable()
                Else
                    Enable()
                End If
                'ControlVisibilityForFileAttachment()
                DataFieldBind()
                upnlActionBtn.Update()
                upnlIssueDetails.Update()
                upnlIssueItem.Update()
                upnlIssueTerms.Update()
                upnlTitle.Update()

                'TextChanged(sender, e)
            Else
                MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert, MSGBox.Message_Text.saveAlert, "Issue can not be saved without Item.", MsgBoxStyle.OkOnly, "")
                Exit Sub
                'mIssue = IssueClone
                'SetObject()
                'Session("mIssue") = mIssue
                'DataFieldBind()
            End If
        Catch ex As SqlException
            Session("IssueClone") = IssueClone
            If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.NumericOverFlow, MSGBox.Message_Text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                If InStr(ex.Message, "CCtabRequisitionItemIssueBalQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabReceiptItemLoanQty", CompareMethod.Text) Then
                    MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, "Issue Qty can not be greater than Receipt Qty.", MsgBoxStyle.OkOnly, "Status")
                    mIssue = IssueClone
                    SetObject()
                    Session("mIssue") = mIssue
                    DataFieldBind()
                ElseIf InStr(ex.Message, "CCtabnWOSpareFromWOJobSparePendingIssuedQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabnWOToolsPendingIssuedQty", CompareMethod.Text) Then
                    MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, "Issue Qty can not be greater than Required Qty.", MsgBoxStyle.OkOnly, "Status")
                    mIssue = IssueClone
                    SetObject()
                    Session("mIssue") = mIssue
                    DataFieldBind()
                ElseIf InStr(ex.Message, "FKtabIssueTermtabTerm", CompareMethod.Text) Then
                    MSGBoxCtrl.Show("Term Deleted! ", "Term Not Avalable<Br><BR>Selected Term is no longer exist in the Database <BR><BR> Remove Term and try Again", " ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8144 Then
                    MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete, MSGBox.Message_Text.ReferenceDelete, ex.Procedure + "," + ex.Message, MsgBoxStyle.OkOnly, "")
                End If
            End If
        Catch ex1 As Exception

            If InStr(ex1.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Stock Qty.", MsgBoxStyle.OkOnly, "Status")
                mIssue = IssueClone
                SetObject()
                Session("mIssue") = mIssue
                DataFieldBind()
            ElseIf InStr(ex1.Message, "CCtabnWOSpareFromWOJobSparePendingIssuedQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabnWOToolsPendingIssuedQty", CompareMethod.Text) Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, "Issue Qty can not be greater than Required Qty.", MsgBoxStyle.OkOnly, "Status")
                mIssue = IssueClone
                SetObject()
                Session("mIssue") = mIssue
                DataFieldBind()
            ElseIf InStr(ex1.Message, "CCtabReceiptItemLoanQty", CompareMethod.Text) Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Receipt Qty.", MsgBoxStyle.OkOnly, "")
            ElseIf InStr(ex1.Message, "CCtabSalesOrderItemIssueBalQty", CompareMethod.Text) Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Sales Order Qty.", MsgBoxStyle.OkOnly, "")
            ElseIf InStr(ex1.Message, "CCtabOrderItemEROQty", CompareMethod.Text) Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Exchange/Repair/Overhaul Qty.", MsgBoxStyle.OkOnly, "")
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
        If Not mIssue.IDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                'Changes by Kalpesh in 13-3-2013
                'These lines commented
                '
                'Dim strOutString As String = ReadXMLFile()
                'strOutString = strOutString.Split(CChar("$"))(1)
                'Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, CInt(strOutString), mCheck.SubscriptionDate)

                'Changes by Kalpesh in 13-3-2013
                'These lines commented
                '
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------

                If DateDiff(DateInterval.Day, CDate(mIssue.IDate), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert, MSGBox.Message_Text.saveAlert, " Your subscription has been expired. can not save Issue. <br> Issue Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
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
                    '------------------------------------------------------------------------------------------------------------
                    If mIssue.TransTypeID = 19 And mIssue.IssueItems.IsSerializedExists = True Then
                        Dim str1 As String = ""
                        Session("IsValid") = mIssue.IsValid
                        str1 = str1 + ("<TABLE width =""300px"" BORDER=0 CELLSPACING=0 CELLPADING=0 ID=""Table2"">")
                        'str1 = str1 + ("<tr>" & "<td WIDTH=60px align=""left"">" & "<font face=""Calibri""><b>Sr. No. </b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Part No.</b>" & "</font>" & "</td><td WIDTH=100px align=""right"">" & "<font face=""Calibri""><b>Qty.</b>" & "</font>" & "</td></tr>")

                        str1 = str1 + ("<TR>")
                        str1 = str1 + ("<TD class=""TextBreak"" style=""width:300px"">")
                        str1 = str1 + "Part No(s). : " & mIssue.IssueItems.SerializedPartNoList
                        str1 = str1 + ("</font>")
                        str1 = str1 + ("</TD>")
                        str1 = str1 + ("</TR>")

                        str1 = str1 + ("</TABLE>")
                        MSGBoxCtrl.Show(MSGBox.Message_Title.Discard, MSGBox.Message_Text.Discard, str1, MsgBoxStyle.YesNo, "IsSerializedExists")
                        Exit Sub
                    End If
                    '------------------------------------------------------------------------------------------------------------

                    'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries
                    'Check if IssueText is blank then call TransTextSeries UI

                    If (mIssue.IsNew) And (mIssue.Text = "") Then

                        Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mIssue.TransTypeID, mIssue.IDateFormatted)

                        If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mIssue.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mIssue.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mIssue.TransTypeID).TransText = "")) Then

                            Dim str = "<script language='javascript'>openledgersame('wfIssue_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

                            Session("BackPagestr_ForTransSeries") = str

                            Session("TransName_ForTransSeries") = "Issue"
                            Session("TransTypeID_ForTransSeries") = mIssue.TransTypeID
                            Session("TransDate_ForTransSeries") = mIssue.IDateFormatted
                            Session("AddTransTextSeries") = "True"

                            Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")

                        Else
                            Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                            If mAutoRenewTransTextSeries.IsRenewed Then
                                With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mIssue.TransTypeID)
                                    mIssue.Text = .TransText
                                    mIssue.No = .StartingTransNo
                                End With
                            Else
                                Dim str = "<script language='javascript'>openledgersame('wfIssue_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

                                Session("BackPagestr_ForTransSeries") = str

                                Session("TransName_ForTransSeries") = "Issue"
                                Session("TransTypeID_ForTransSeries") = mIssue.TransTypeID
                                Session("TransDate_ForTransSeries") = mIssue.IDateFormatted
                                Session("AddTransTextSeries") = "True"

                                Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                            End If
                        End If

                    End If

                    'Added By Saylee on 21-July-2016 for store validation regarding NotInUse
                    ''Check whether Issue Date is greater than NotInUse of Store 
                    If mStoreList(mIssue.StoreID).NotInUse = True Then
                        If CDate(mStoreList(mIssue.StoreID).NotInUseDate) <= CDate(mIssue.IDate) Then
                            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Store is not applicable since " + mStoreList(mIssue.StoreID).NotInUseDateFormatted + "\n" + "Select another Store from list or select date before " + mStoreList(mIssue.StoreID).NotInUseDateFormatted + " & try again", False), True)
                            MSGBoxCtrl.Show("Save Alert!", "Store is not applicable since " + mStoreList(mIssue.StoreID).NotInUseDateFormatted, "Select another Store from list or select date before " + mStoreList(mIssue.StoreID).NotInUseDateFormatted + " & try again", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                    End If

                    ''Check whether Issue Date is greater than NotInUse of ToStore 
                    If mStoreList(mIssue.ToStoreID).NotInUse = True Then
                        If CDate(mStoreList(mIssue.ToStoreID).NotInUseDate) <= CDate(mIssue.IDate) Then
                            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Destination Store is not applicable since " + mStoreList(mIssue.ToStoreID).NotInUseDateFormatted + "\n" + "Select another Store from list or select date before " + mStoreList(mIssue.ToStoreID).NotInUseDateFormatted + " & try again", False), True)
                            MSGBoxCtrl.Show("Save Alert!", "Destination Store is not applicable since " + mStoreList(mIssue.ToStoreID).NotInUseDateFormatted, "Select another Store from list or select date before " + mStoreList(mIssue.ToStoreID).NotInUseDateFormatted + " & try again", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                    End If
                    '---------------------------------------------

                    'End
                    mIssue.Save()
                    'Added By Utkarsh ON 19-Oct-2012 FOR ALL18102012
                    'ControlVisibilityForFileAttachment()
                    'End
                    Session("ToMakeAuthorizeButtonInvisibel") = ""
                Else
                    ValidationCode()
                    upnlValidationSummary.Update()
                    Exit Sub
                End If
                'MarkLog(Util.Action.Save, ModuleName, mIssue.IssueNo, Util.ErrorType.NoError, mIssue.ID)
                IssueTo()
                mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + IIf(mIssueTo <> "", " to " + mIssueTo, "")

                If mIssue.StatusID = 2 Then
                    MarkLog(Util.Action.Authorize, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                ElseIf mIssue.StatusID = 3 Then
                    MarkLog(Util.Action.Amend, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                ElseIf mIssue.StatusID = 4 Then
                    MarkLog(Util.Action.Cancel, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                Else
                    MarkLog(Util.Action.Save, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                End If

                mIssue.MarkClean()
                Session("mIssue") = mIssue
                'Added By Utkarsh ON 22-Jul-2013 FOR ALL19072013
                If mIssue.StatusID = 2 Then
                    If (AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA") Then
                        SendMail()
                    ElseIf AppSettings("ClientCode") = "IND" And mIssue.TransTypeID = 14 And mIssue.ToTypeID = 18 Then   'ToTypeID=18 Issue To Aircraft as Req^n
                        SendMail()
                    Else
                        If mIssue.TransTypeID = 19 Then  'Issue to discard   Added By Prashant 16-Sep-2013 ALL16092013 
                            SendMail()
                        End If
                    End If
                    'Else
                    'Response.Redirect("wfIssue.aspx?BackPage=" & Request.QueryString("BackPage"), False)   'Commented For Barcode Implementation by Vikrant
                End If
                'End

                SetPage()
                'ControlVisibility()
                If mIssue.IsNew Then
                    lblStatus.Text = "OPEN"
                End If
                If mIssue.IssueItems.Count > 0 Then
                    Disable()
                Else
                    Enable()
                End If
                'ControlVisibilityForFileAttachment()
                DataFieldBind()
                ControlVisibility()
                upnlActionBtn.Update()
                upnlIssueDetails.Update()
                upnlIssueItem.Update()
                upnlIssueTerms.Update()
                upnlTitle.Update()
                If mIssue.StatusID = 2 Then
                    MSGBoxCtrl.Show(MSGBox.Message_Title.AuthorizedSuccessFully, MSGBox.Message_Text.AuthorizedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                ElseIf mIssue.StatusID = 4 Then
                    MSGBoxCtrl.Show(MSGBox.Message_Title.CanceledSuccessFully, MSGBox.Message_Text.CanceledSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Else
                    MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully, MSGBox.Message_Text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                End If
            Else
                MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert, MSGBox.Message_Text.saveAlert, "Issue can not be saved without Item.", MsgBoxStyle.OkOnly, "")
                'mIssue = IssueClone
                'SetObject()
                'Session("mIssue") = mIssue
                'DataFieldBind()
            End If
        Catch ex As SqlClient.SqlException
            Session("IssueClone") = IssueClone
            If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.NumericOverFlow, MSGBox.Message_Text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                If InStr(ex.Message, "CCtabRequisitionItemIssueBalQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabReceiptItemLoanQty", CompareMethod.Text) Then
                    MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, "Issue Qty can not be greater than Receipt Qty.", MsgBoxStyle.OkOnly, "Status")
                    mIssue = IssueClone
                    SetObject()
                    Session("mIssue") = mIssue
                    DataFieldBind()
                ElseIf InStr(ex.Message, "CCtabnWOSpareFromWOJobSparePendingIssuedQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabnWOToolsPendingIssuedQty", CompareMethod.Text) Then
                    MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, "Issue Qty can not be greater than Required Qty.", MsgBoxStyle.OkOnly, "Status")
                    mIssue = IssueClone
                    SetObject()
                    Session("mIssue") = mIssue
                    DataFieldBind()
                ElseIf InStr(ex.Message, "FKtabIssueTermtabTerm", CompareMethod.Text) Then
                    MSGBoxCtrl.Show("Term Deleted! ", "Term Not Avalable<Br><BR>Selected Term is no longer exist in the Database <BR><BR> Remove Term and try Again", " ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8144 Then
                    MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete, MSGBox.Message_Text.ReferenceDelete, ex.Procedure + "," + ex.Message, MsgBoxStyle.OkOnly, "")
                End If
            End If
        Catch ex1 As Exception
            mIssue = IssueClone
            SetObject()
            Session("mIssue") = mIssue
            DataFieldBind()
            If InStr(ex1.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Stock Qty.", MsgBoxStyle.OkOnly, "Status")
            ElseIf InStr(ex1.Message, "CCtabnWOSpareFromWOJobSparePendingIssuedQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabnWOToolsPendingIssuedQty", CompareMethod.Text) Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, "Issue Qty can not be greater than Required Qty.", MsgBoxStyle.OkOnly, "Status")
            ElseIf InStr(ex1.Message, "CCtabReceiptItemLoanQty", CompareMethod.Text) Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Receipt Qty.", MsgBoxStyle.OkOnly, "")
            ElseIf InStr(ex1.Message, "CCtabSalesOrderItemIssueBalQty", CompareMethod.Text) Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Sales Order Qty.", MsgBoxStyle.OkOnly, "")
            ElseIf InStr(ex1.Message, "CCtabOrderItemEROQty", CompareMethod.Text) Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Exchange/Repair/Overhaul Qty.", MsgBoxStyle.OkOnly, "")
            ElseIf InStr(ex1.Message, "CCtabIssueItemQty", CompareMethod.Text) Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.CheckQty, MSGBox.Message_Text.CheckQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Check Qty./Change Qty. according to unit", MsgBoxStyle.OkOnly, "")
            Else
                MSGBoxCtrl.Show(MSGBox.Message_Title.CheckQty, MSGBox.Message_Text.CheckQty, ex1.Message, MsgBoxStyle.OkOnly, "")
            End If
        Finally
            IssueClone = Nothing
        End Try
    End Sub
    'Added By Vikrant On 24-July-2014 For BA24072014
    Private Function CheckDateForTransactionLock(ByVal TransDate As Date) As Boolean
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
        mIssue.ToStoreID = New Guid(cmbLocationStore.SelectedValue)
        mIssue.StoreID = New Guid(cmbStoreList.SelectedValue)
        mIssue.nWOID = New Guid(cmbWorkOrder.SelectedValue)
        mIssue.ToTypeID = Val(cmbToType.SelectedValue)
        mIssue.Person = Trim(txtPerson.Text)
        mIssue.Remark = Trim(txtRemark.Text)
        mIssue.RegNo = Trim(txtRegNo.Text)
        mIssue.Text = txtText.Text
        mIssue.No = Val(txtNo.Text)
        mIssue.AWBNo = Trim(txtAWBNo.Text)
        mIssue.VoucherNo = Trim(txtVoucherNo.Text)

        mIssue.ReferenceNo = txtReferenceNo.Text.Trim 'Added By Utkarsh On 15-May-2012 FOR 15052012-17

        Select Case mIssue.ToTypeID
            Case 1, 7  '7 Added By Prashant 5-July-2011 for Discard
                mIssue.VendorID = New Guid(cmbVendorList.SelectedValue)
                If mIssue.TransTypeID = 19 Then '19 Part Discard
                    mIssue.IssueTo = "Part Discard"
                Else
                    mIssue.IssueTo = IIf(cmbVendorList.SelectedIndex > 0, cmbVendorList.SelectedItem.Text, "")
                End If
                mIssue.MachineID = Guid.Empty
                mIssue.ToStoreID = Guid.Empty
                mIssue.WorkShopID = Guid.Empty
                mIssue.nWOID = Guid.Empty
            Case 2
                mIssue.VendorID = Guid.Empty
                mIssue.MachineID = New Guid(cmbAircraftList.SelectedValue)
                mIssue.IssueTo = IIf(cmbAircraftList.SelectedIndex > 0, cmbAircraftList.SelectedItem.Text, "")
                mIssue.ToStoreID = Guid.Empty
                mIssue.WorkShopID = Guid.Empty
            Case 8
                mIssue.VendorID = Guid.Empty
                mIssue.MachineID = Guid.Empty
                mIssue.ToStoreID = New Guid(cmbLocationStore.SelectedValue)
                mIssue.IssueTo = IIf(cmbLocationStore.SelectedIndex > 0, cmbLocationStore.SelectedItem.Text, "")
                mIssue.WorkShopID = Guid.Empty
                mIssue.nWOID = Guid.Empty
            Case 16
                mIssue.VendorID = Guid.Empty
                mIssue.MachineID = Guid.Empty
                mIssue.ToStoreID = Guid.Empty
                mIssue.WorkShopID = New Guid(cmbWorkShop.SelectedValue)
                mIssue.IssueTo = IIf(cmbWorkShop.SelectedIndex > 0, cmbWorkShop.SelectedItem.Text, "")
                mIssue.nWOID = Guid.Empty
            Case 17
                mIssue.VendorID = Guid.Empty
                ' Commented by Utkarsh On 05-Jul-2012 FOR ALL04072012
                ' mIssue.MachineID = Guid.Empty
                'End
                mIssue.ToStoreID = Guid.Empty
                mIssue.WorkShopID = Guid.Empty
                mIssue.nWOID = New Guid(cmbWorkOrder.SelectedValue)
                mIssue.IssueTo = IIf(cmbWorkOrder.SelectedIndex > 0, cmbWorkOrder.SelectedItem.Text, "")
            Case 18
                mIssue.VendorID = Guid.Empty
                If mIssue.TransTypeID = 14 Then
                    mIssue.MachineID = New Guid(cmbAircraftList.SelectedValue)
                    mIssue.IssueTo = IIf(cmbAircraftList.SelectedIndex > 0, cmbAircraftList.SelectedItem.Text, "")
                ElseIf mIssue.TransTypeID = 44 Then
                    mIssue.WorkShopID = New Guid(cmbWorkShop.SelectedValue)
                    mIssue.IssueTo = IIf(cmbWorkShop.SelectedIndex > 0, cmbWorkShop.SelectedItem.Text, "")
                ElseIf mIssue.TransTypeID = 59 Then 'Issue to work order as spare assembly/Component requisition Added By Prashant on 25-Jun-2021 STR25062021
                    mIssue.nWOID = New Guid(cmbWorkOrder.SelectedValue)
                    mIssue.IssueTo = IIf(cmbWorkOrder.SelectedIndex > 0, cmbWorkOrder.SelectedItem.Text, "")
                End If
                mIssue.ToStoreID = Guid.Empty

        End Select
        mIssue.UserName = User.Identity.Name
        mIssue.CalculateTotal()            'Added By Saylee on 7-July-2011

        'Added by vikrant on 25-AUG-2011
        Dim txtValue As TextBox
        Dim cmbValue As DropDownList
        Dim mIssueItem As IssueItem
        Dim i As Integer = 0
        For Each mIssueItem In mIssue.IssueItems
            With mIssueItem
                Try
                    txtValue = CType(Me.dgIssueItems.Rows(i).FindControl("txtQty"), TextBox)
                    .DisplayQty = CDec(Val(txtValue.Text))
                    If mIssue.TransTypeID = 19 Then
                        .DiscardAmt = .EffRate * .DisplayQty 'Added By Prashant On 18-Jul-2016
                    End If
                    cmbValue = CType(Me.dgIssueItems.Rows(i).FindControl("cmbRequisitionItemTypeList"), DropDownList) 'Added By Prashant 13-Apr-2015 ALL13042015
                    .RequisitionItemTypeID = CInt(cmbValue.SelectedValue)
                Catch ex As Exception

                End Try
            End With
            i = i + 1
        Next
        If Not mIssue.FileAttachments Is Nothing Then 'Added By Sankalp on 29-09-25
            If mIssue.FileAttachments.Count > 0 Then
                mIssue.Size = 1
            Else
                mIssue.Size = 0
            End If
        End If
    End Sub
    'DONE
    Private Sub SetcmbRequisitionItemTypeList() 'Added By Prashant 13-Apr-2015 ALL13042015
        Dim cmbValue As DropDownList
        Dim mIssueItem As IssueItem
        Dim i As Integer = 0
        If dgIssueItems.Rows.Count > 0 Then
            For Each mIssueItem In mIssue.IssueItems
                With mIssueItem
                    Try
                        cmbValue = CType(Me.dgIssueItems.Rows(i).FindControl("cmbRequisitionItemTypeList"), DropDownList)
                        .RequisitionItemTypeID = CInt(cmbValue.SelectedValue)
                    Catch ex As Exception

                    End Try
                End With
                i = i + 1
            Next
        End If
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.Show(MSGBox.Message_Title.RemoveItem, MSGBox.Message_Text.RemoveItem, "", MsgBoxStyle.YesNo, "Remove")
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
        mRequisitionItemTypeList = RequisitionItemTypeList.GetRequisitionItemTypeList()  'Added By Prashant 13-Apr-2015 ALL13042015
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
                            mIssueDetail = mIssue.IssueNo + " Dated: " + mIssue.IDateFormatted + " Part No. " + mIssue.IssueItems.CurrentItem.ItemName + " Category " + mIssue.IssueItems.CurrentItem.Category + " Qty:- " + mIssue.IssueItems.CurrentItem.DisplayQty.ToString + " Rate:- " + mIssue.IssueItems.CurrentItem.EffRate.ToString + " Receipt No:- " + mIssue.IssueItems.CurrentItem.ReceiptTextNo.ToString
                            mIssue.IssueItems.Remove(mIssue.IssueItems.CurrentItem)
                            mIssue.CalculateTotal()            'Added By Saylee on 7-July-2011
                            Session("mIssue") = mIssue
                            mRequisitionItemTypeList = RequisitionItemTypeList.GetRequisitionItemTypeList()  'Added By Prashant 13-Apr-2015 ALL13042015
                            dgIssueItems.DataSource = mIssue.IssueItems
                            dgIssueItems.DataBind()
                            ControlVisibility()
                            If mIssue.IssueItems.Count > 0 Then
                                Disable()
                            Else
                                Enable()
                            End If
                            upnlIssueItem.Update()
                            upnlIssueDetails.Update()
                            upnlActionBtn.Update()
                            upnlIssueTerms.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete, MSGBox.Message_Text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        Finally
                            MarkLog(Util.Action.Remove, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
                        'Added Code
                        Session("sender") = ""
                        Page.Validate("1")
                        If Page.IsValid Then
                            DataFieldBind()
                            If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
                                MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            '------------------------------------------------------------------------------------------------------------
                            If mIssue.TransTypeID = 19 And mIssue.IssueItems.IsSerializedExists = True Then
                                MSGBoxCtrl.Show(MSGBox.Message_Title.Discard, MSGBox.Message_Text.Discard, "Part No(s). : " & mIssue.IssueItems.SerializedPartNoList, MsgBoxStyle.YesNo, "IsSerializedExists")
                                Exit Sub
                            End If
                            '------------------------------------------------------------------------------------------------------------

                            Session.Remove("IsValid")
                            If Not CustomValidate2() Then Exit Sub
                            Save()
                        Else
                            Session.Remove("IsValid")
                            upnlValidationSummary.Update()
                            'Response.Redirect("wfIssue_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                        '-----------------Added by Vikrant on 26-aug-2011--------------------------
                    ElseIf MSGBoxCtrl.Sender = "Expired" Then  '' Expired confirmation
                        Session("sender") = ""
                        DataFieldBind()
                        AddExpiredItemByBarcode()
                        '--------------------------------------------------------------------------
                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        Page.Validate("1")
                        If Page.IsValid Then
                            If CType(Session("ReturnWO"), String) = "ReturnWO" And CType(Session("IsForWOReturn"), Boolean) = True Then
                                ReturnWOQty()
                                Session.Remove("ReturnWO")
                                If mIssue.IssueItems.Count = 0 Then
                                    mIssue.Delete()
                                    mIssue.Save()
                                    Session.Remove("IsAllPartsSelected") 'Added By Vikrant On 03-Feb-2016 For ALL03022016
                                    Response.Redirect("Index.aspx")
                                End If
                            End If
                            Session.Remove("IsValid")
                            DataFieldBind()
                            Save()
                            'Added By Utkarsh ON 22-Jul-2013 FOR ALL19072013
                            If mIssue.StatusID = 2 And Not mIssue.IsDirty Then 'Not mIssue.IsDirty is added by vikrant on 08-Feb-2016 because SqlException was not getting captured
                                ToCreateRCI()
                            Else
                                mIssue.StatusID = 1
                                Session("mIssue") = mIssue
                            End If
                            'End
                            'Response.Redirect("Index.aspx")
                        Else
                            Session.Remove("IsValid")
                            upnlValidationSummary.Update()
                            'Response.Redirect("wfIssue_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                        'Added By Utkarsh ON 22-Jul-2013 FOR ALL19072013
                    ElseIf MSGBoxCtrl.Sender = "ReceiptCumInvoiceCreation" Then
                        DataFieldBind()
                        CreateAutoReceiptCumInvoice()  'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries
                    ElseIf MSGBoxCtrl.Sender = "IsSerializedExists" Then  '' Close confirmation
                        'Added Code
                        Session("sender") = ""
                        Page.Validate("1")
                        If Page.IsValid Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If Not CustomValidate2() Then Exit Sub
                            SaveIsSerializedExists()
                        Else
                            Session.Remove("IsValid")
                            upnlValidationSummary.Update()
                            'Response.Redirect("wfIssue_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                        '

                    ElseIf MSGBoxCtrl.Sender = "IsSerializedExistsStatus" Then
                        Session("sender") = ""
                        Page.Validate("1")
                        If Page.IsValid Then
                            If CType(Session("ReturnWO"), String) = "ReturnWO" And CType(Session("IsForWOReturn"), Boolean) = True Then
                                ReturnWOQty()
                                Session.Remove("ReturnWO")
                                If mIssue.IssueItems.Count = 0 Then
                                    mIssue.Delete()
                                    mIssue.Save()
                                    Session.Remove("IsAllPartsSelected") 'Added By Vikrant On 03-Feb-2016 For ALL03022016
                                    Response.Redirect("Index.aspx")
                                End If
                            End If
                            Session.Remove("IsValid")
                            DataFieldBind()
                            SaveIsSerializedExists()
                        Else
                            Session.Remove("IsValid")
                            upnlValidationSummary.Update()
                            'Response.Redirect("wfIssue_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                        '------------------------------------------------------------------------------------------------------
                    ElseIf MSGBoxCtrl.Sender = "StatusCancel" Then
                        Session("sender") = ""
                        mIssue.StatusID = 4
                        DataFieldBind()
                        Save()
                    End If
                    'Sankalp 25-05-25
                    If MSGBoxCtrl.Sender = "RemoveAttachment" Then
                        Try
                            Session("Sender") = ""
                            mIssue = CType(Session("mIssue"), Issue)
                            mIssue.FileAttachments.Remove(mIssue.FileAttachments.CurrentItem)
                            dgItemAttachment.DataSource = mIssue.FileAttachments
                            dgItemAttachment.DataBind()
                            upnldgItemAttachment.Update()
                            upnlItemAttachment.Update()
                            Session("mIssue") = mIssue
                        Catch ex As SqlException

                        End Try
                    End If

                    If MSGBoxCtrl.Sender = "SaveAttachment" Then
						mIssue.SaveUpdatedAttachment(mIssue.ID)
						mIsAttachmentNotSave = False
						Session("IsAttachmentNotSave") = mIsAttachmentNotSave
						Session("mIssue") = mIssue
					End If

                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session.Remove("mTypeList1")
                        If mIssue.IsNew Then
                            Session.Remove("mIssue")
                        End If
                        Session("Sender") = ""
                        Session.Remove("IsAllPartsSelected") 'Added By Vikrant On 03-Feb-2016 For ALL03022016
                        Response.Redirect("Index.aspx")
                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        Session.Remove("mTypeList1")

                        If CType(Session("ReturnWO"), String) = "ReturnWO" And CType(Session("IsForWOReturn"), Boolean) = True Then
                            ''ReturnWOQty()
                            Session.Remove("ReturnWO")
                        End If
                        If mIssue.StatusID = 2 Then
                            mIssue.StatusID = 1
                        ElseIf mIssue.StatusID = 4 Then
                            mIssue.StatusID = 2
                        ElseIf mIssue.StatusID = 1 Then      'Added By Prashant 27-Apr-2010
                            mIssue.StatusID = 2
                        End If
                        Session("mIssue") = mIssue
                        SetControlStatus(mIssue.StatusID, mIsForWOReturn)
                        upnlIssueDetails.Update()
                        'Response.Redirect("wfIssue_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        '------------------------------------------------------------------------------------------
                    ElseIf MSGBoxCtrl.Sender = "IsSerializedExistsStatus" Then
                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        Session.Remove("mTypeList1")

                        If CType(Session("ReturnWO"), String) = "ReturnWO" And CType(Session("IsForWOReturn"), Boolean) = True Then
                            ''ReturnWOQty()
                            Session.Remove("ReturnWO")
                        End If
                        If mIssue.StatusID = 2 Then
                            mIssue.StatusID = 1
                        ElseIf mIssue.StatusID = 4 Then
                            mIssue.StatusID = 2
                        ElseIf mIssue.StatusID = 1 Then      'Added By Prashant 27-Apr-2010
                            mIssue.StatusID = 2
                        End If
                        Session("mIssue") = mIssue
                        upnlIssueDetails.Update()
                        'Response.Redirect("wfIssue_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

                        '-----------------Added by Vikrant on 26-aug-2011--------------------------
                    ElseIf MSGBoxCtrl.Sender = "Expired" Then  '' Expired confirmation
                        Session("sender") = ""
                        Session.Remove("mPendingItemList")
                        'DataFieldBind()
                        'Added By Utkarsh ON 22-Jul-2013 FOR ALL19072013
                    ElseIf CType(Session("sender"), String) = "ReceiptCumInvoiceCreation" Then
                        Session("sender") = ""
                        Session("ReceiptCumInvoiceCreate") = ""
                        'Response.Redirect("wfIssue_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        'End
                    ElseIf MSGBoxCtrl.Sender = "SaveAttachment" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    Else
                        Session("Sender") = ""
                        'Response.Redirect("wfIssue_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
                    If MSGBoxCtrl.Sender = "Status" Then
                        ''==========================================WO - 2006-2007-1-17.doc
                        If mIssue.StatusID = 2 And Session("sender") <> "Close" Then
                            mIssue.StatusID = 1
                        ElseIf mIssue.StatusID = 4 Then
                            mIssue.StatusID = 2
                        End If
                        Session("sender") = ""
                        Session("mIssue") = mIssue
                        ''========================================
                        DataFieldBind()
                        upnlIssueDetails.Update()

                        'Response.Redirect("wfIssue_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries
                    ElseIf MSGBoxCtrl.Sender = "ReceiptCumInvoiceTransTextSeriesAlert" Then
                        Session("AddTransTextSeries") = "True"
                        Session("sender") = "ReceiptCumInvoiceCreation" 'Need to set again
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    ElseIf MSGBoxCtrl.Sender = "ResetFromStore" Then
                        Session("sender") = ""
                        cmbStoreList.ClearSelection()
                        upnlIssueDetails.Update()
                    ElseIf MSGBoxCtrl.Sender = "ResetToStore" Then
                        Session("sender") = ""
                        cmbLocationStore.ClearSelection()
                        upnlIssueDetails.Update()
                    Else
                        Session("sender") = ""
                        'DataFieldBind()
                        'upnlIssueDetails.Update()
                        'Response.Redirect("wfIssue_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
            End Select
        ElseIf Result1 = -1 Then
            If mIssue.StatusID = 2 And Session("sender") <> "Close" Then
                mIssue.StatusID = 1
            ElseIf mIssue.StatusID = 4 Then
                mIssue.StatusID = 2
            ElseIf mIssue.StatusID = 1 Then  'Added By Prashant 27-Apr-2010
                mIssue.StatusID = 2
            End If
            Session("mIssue") = mIssue
            Session("sender") = ""
            upnlIssueDetails.Update()
            'Response.Redirect("wfIssue_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries
        ElseIf Result1 = 0 And MSGBoxCtrl.Sender = "ReceiptCumInvoiceCreation" Then
            Session("ReceiptCumInvoiceCreate") = ""
            DataFieldBind()
            CreateAutoReceiptCumInvoice()
        End If
    End Sub
    Private Sub SetControlStatus(ByVal StatusId As Int16, Optional ByVal IsForWOReturn As Boolean = False)
        If mOpenFrom = "FromwfStockCard" Or mOpenFrom = "FromReqItemStatusReport" Then 'Added By Prashant 3-Apr-2014 ALL03042014
            btnAddItem.Enabled = False
            btnAddTerm.Enabled = False
            btnAddSupplierSpecificTerms.Enabled = False
            txtRemark.Enabled = False
            txtText.Enabled = False
            txtNo.Enabled = False
            txtPerson.Enabled = False
            btnSave.Visible = False
            dgIssueItems.Columns(17).Visible = False '22=>21 21=>20 20=>18 18=>17
            dgIssueItems.Columns(18).Visible = False '23=>22 22=>21 21=>19  19=>18
            'dgIssueItems.Columns(24).Visible = False '' Ajay 28-02-2023
            'btnSelectFile.Disabled = True  'Comment by Sankalp 29-09-25
            btnSelectFiles.Enabled = False 'Sankalp 29-09-25
        Else
            btnAddItem.Enabled = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True)
            btnAddTerm.Enabled = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True)
            btnAddSupplierSpecificTerms.Enabled = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True)
            txtRemark.Enabled = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True)
            txtText.Enabled = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True)
            txtNo.Enabled = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True)
            txtPerson.Enabled = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True)
            btnSave.Visible = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True)
            '' Ajay 28-02-2023
            'dgIssueItems.Columns(24).Visible = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True) 'Delete
            'dgIssueItems.Columns(23).Visible = IIf(IsForWOReturn = True, False, True)
            dgIssueItems.Columns(18).Visible = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True) 'Delete 23=>22 22=>21 21=>19 19=>18
            dgIssueItems.Columns(18).Visible = IIf(IsForWOReturn = True, False, True) '23=>22 22=>21 21=>19 19=>18
            '--------------
            'btnSelectFile.Disabled = IIf(StatusId > 1, True, False) 'Comment 29-09-25
            'btnSelectFiles.Enabled = IIf(StatusId > 1, False, True) 'Sankalp 29-09-25
        End If
        dgIssueTerms.Columns(2).Visible = IIf(StatusId > 1, False, True) And IIf(IsForWOReturn = True, False, True)
        If StatusId > 1 Then dgIssueItems.Columns(18).HeaderText = "Action" '23=>22 22=>21 21=>19 19=>18
        dgIssueItems.Columns(10).Visible = IIf(CType(mIssue.TransTypeID, Flypal.Util.Trans) = Flypal.Util.Trans.DisacrdPart, True, False) '13=>12 12=>11 11=>10
    End Sub
    Private Sub ControlVisibility()
        If mIssue.StatusID = 1 Then
            txtIssueDate.Enabled = False
        Else
            txtIssueDate.Enabled = False
        End If
        If mOpenFrom = "FromwfStockCard" Or mOpenFrom = "FromReqItemStatusReport" Then 'Added By Prashant 3-Apr-2014 ALL03042014
            btnAuthorized.Enabled = False
            'btnSelectFile.Disabled = True 'Comment Sankalp 29-09-25
            btnSelectFiles.Enabled = False   'Sankalp 29-09-25
        Else
            btnAuthorized.Visible = (Not mIssue.IssueItems.Count = 0) And (Not mIssue.IsNew) And (mIssue.StatusID = 1) And Session("ToMakeAuthorizeButtonInvisibel") = ""
        End If
        btnCancel.Visible = (Not mIssue.IsNew) And (mIssue.StatusID = 2) And (mIssue.IsSync = 0)  'One Condition Added by Saylee on 2-June-2010
        btnLineMaintenanceReturn.Visible = (mIssue.TransTypeID = 14 Or mIssue.TransTypeID = 44 Or mIssue.TransTypeID = 52) And (mIssue.StatusID = 2)

        'Added By Saylee 3-Jun-2010
        btnSentToBill.Visible = (mIssue.ToTypeID = 1 Or mIssue.ToTypeID = 2 Or mIssue.ToTypeID = 17) And (Not mIssue.IsNew) And (mIssue.StatusID = 2) And ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer") 'Added by Saylee on 2-June-2010
        btnSentToBill.Enabled = (mIssue.ToTypeID = 1 Or mIssue.ToTypeID = 2 Or mIssue.ToTypeID = 17) And (Not mIssue.IsNew) And (mIssue.IsSync = 0) And ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer") 'Added by Saylee on 2-June-2010

        dgIssueItems.Columns(16).Visible = IIf(CType(Session("IsForWOReturn"), Boolean) = True, True, False) '21=20 20=>19 19=>17 17=>16
        btnReturnAuthorized.Visible = (Not mIssue.IsNew) And (mIssue.StatusID = 2) And (IsInRole(Rights.[New]) And IsInRole(Rights.Edit) And IsInRole(Rights.Delete) And IsInRole(Rights.View) And IsInRole(Rights.Print)) And (mIssue.IsSync = 0) And IIf(CType(Session("IsForWOReturn"), Boolean) = True, True, False)

        'Added By Prashant 3-Jun-2010
        If User.IsInRole("IssueEditAfterAuthorizationView") = True Then
            btnLineMaintenanceReturn.Enabled = (mIssue.IsSync = 0)    'One Condition Added by Saylee on 3-June-2010
        Else
            btnLineMaintenanceReturn.Enabled = False
        End If
        '-----------------------------

        '--------------Added by vikrant on 26-aug-2011----------------
        If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then
            'txtBarcodeIssue.Visible = True
            'lblBarcodeNo.Visible = True
            If ((mIssue.TransTypeID = 14) Or (mIssue.TransTypeID = 20) Or (mIssue.TransTypeID = 15) Or (mIssue.TransTypeID = 17) Or (mIssue.TransTypeID = 24) Or (mIssue.TransTypeID = 63) Or (mIssue.TransTypeID = 19) Or (mIssue.TransTypeID = 25) Or (mIssue.TransTypeID = 26) Or (mIssue.TransTypeID = 44) Or (mIssue.TransTypeID = 45) Or (mIssue.TransTypeID = 52)) Then
                txtBarcodeItem.Visible = True
                btnAddBarcodeItem.Visible = True
                lblBarcodeNos.Visible = True
                txtBarcodeItem.Enabled = (mIssue.StatusID = 1)
                btnAddBarcodeItem.Enabled = (mIssue.StatusID = 1)
            Else
                ControlVisibilityForQty()
            End If
            If Session("strMSG") <> "" Then
                Dim S As String
                S = Session("strMSG")
                cvControlValidator.ErrorMessage = S
                cvControlValidator.IsValid = False
                Session.Remove("strMSG")
            End If
        Else
            If Not (mIssue.TransTypeID = 15 Or mIssue.TransTypeID = 19) Then 'Added By Vikrant On 03-Feb-2016 For ALL03022016
                ControlVisibilityForQty()
            End If
        End If
        If (mIssue.IssueItems.Count = 0) Then
            btnPrint.Enabled = False
            btnReleaseNoteNo.Enabled = False
        ElseIf mIssue.IsNew Then
            btnPrint.Enabled = False
            btnReleaseNoteNo.Enabled = False
        End If

        '--------------------------------------------------------------
        'Added By Prashant 17-Aug-2011
        If Not IsInRole(Rights.Authorized) Then
            btnAuthorized.Enabled = False
            btnAuthorized.ToolTip = "You are not authorized user "
            btnReturnAuthorized.Enabled = False
            btnReturnAuthorized.ToolTip = "You are not authorized user "
            btnCancel.Enabled = False
            btnCancel.ToolTip = "You are not authorized user "
            'Sankalp 29-09-25
            btnSaveAttachment.Enabled = False
            btnSaveAttachment.ToolTip = "You are not authorized user "
        End If

        'Comment Sankalp 29-09-25
        'If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
        '    btnSelectFile.Disabled = True
        '    btnDelAttach.Enabled = False
        '    btnDelAttach.ToolTip = "You are not authorized user "
        '    ImageButton1.Enabled = False
        '    ImageButton1.ToolTip = "You are not authorized user "
        'End If
        If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            btnSelectFiles.Enabled = False
            btnSelectFiles.ToolTip = "You are not authorized user "
        End If
        'Added By Utkarsh On 15-May-2012 FOR 15052012-17
        If AppSettings("ReferenceNo") = "True" Then
            lblReferenceNo.Visible = True
            txtReferenceNo.Visible = True
        End If
        'End
        'Sankalp 29-09-25
        If mIssue.StatusID = 2 Then
            btnSaveAttachment.Visible = True
        Else
            btnSaveAttachment.Visible = False
        End If

    End Sub
    'Added by vikrant on 31-aug-2011
    'DONE
    Public Sub ControlVisibilityForQty()
        Dim txtValue As TextBox
        Dim mIssueItem As IssueItem
        Dim i As Integer = 0
        For Each mIssueItem In mIssue.IssueItems
            With mIssueItem
                Try
                    txtValue = CType(Me.dgIssueItems.Rows(i).FindControl("txtQty"), TextBox)
                    'txtValue.ReadOnly = False
                    txtValue.Enabled = False
                Catch ex As Exception

                End Try
            End With
            i = i + 1
        Next
    End Sub
    'Added by Saylee on 15-July-2010
    'DONE
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
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        'Select Case mTransTypeID       'Commented By Prashant 17-Aug-2011
        Select Case mIssue.TransTypeID  'Added By Prashant 17-Aug-2011
            Case Util.Trans.IssueToAircraft
                IsInRoleString = "IssueToAircraft"
            Case Util.Trans.IssueToStore
                IsInRoleString = "IssueToStore"
            Case Util.Trans.ExchangeRepairIssueToVendor
                IsInRoleString = "IssueToVendorForExchange"
            Case Util.Trans.LoanIssueToStore
                IsInRoleString = "IssueLoanToStore"
            Case Util.Trans.LoanIssuedToAircraft
                IsInRoleString = "IssueLoanToAircraft"
            Case Util.Trans.LoanReturnToStore
                IsInRoleString = "IssueLoanReturnToStore"
            Case Util.Trans.DisacrdPart
                IsInRoleString = "IssueToDiscard"
            Case Util.Trans.IssueToCustomer
                IsInRoleString = "IssueToCustomer"
            Case Util.Trans.LoanIssueToCustomer
                IsInRoleString = "LoanIssueToCustomer"
            Case Util.Trans.LoanIssueToVendor
                IsInRoleString = "LoanIssueToVendor"
            Case Util.Trans.IssueToWorkShop
                IsInRoleString = "IssueToWorkShop"
            Case Util.Trans.LoanIssueToWorkShop
                IsInRoleString = "IssueLoanToWorkShop"
            Case Util.Trans.IssueforLoanReturntoSupplier
                IsInRoleString = "IssueforLoanReturntoSupplier"
            Case Util.Trans.IssueforLoanReturntoCustomer
                IsInRoleString = "IssueforLoanReturntoCustomer"
            Case Util.Trans.IssueToWorkOrder
                IsInRoleString = "IssueToWorkOrder"
            Case Util.Trans.IssuetoSupplierasRentalLease
                IsInRoleString = "IssuetoSupplierasRentalLease"
            Case Util.Trans.IssueToCustomerAsRepairedReturn
                IsInRoleString = "IssueToCustomerAsRepairedReturn"
            Case Util.Trans.IssueToWorkOrderAsSpares
                If mIssue.ToTypeID = 18 Then 'Issue to work order as Material Requisition 
                    IsInRoleString = "IssuetoworkorderasSparerequisition"
                Else
                    IsInRoleString = "IssueToWorkOrderAsSpares"
                End If
                'IsInRoleString = "IssueToWorkOrderAsSpares"
            Case Util.Trans.IssueToWorkOrderAsTools
                IsInRoleString = "IssueToWorkOrderAsTools"
            Case Util.Trans.IssuetoSupplierNone
                IsInRoleString = "IssuetoSupplierNone"
                'Case Util.Trans.IssueToRequisition  'Added by vikrant For New Requisition
                'IsInRoleString = "IssueToRequisition"
            Case Util.Trans.IssueToCustomerAsNone
                IsInRoleString = "IssueToCustomerAsNone"
            Case Util.Trans.IssuetoSupplierasReturn
                IsInRoleString = "IssuetoSupplierasReturn"
        End Select
        'IsInRoleString = "Issue"
        'Depending upon decided IsInRole String; checkign Rights of the User
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
                'Added By Prashant 17-Aug-2011
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
                '-----------------------------
        End Select
    End Function
    Private Sub addattributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Public Sub TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtValue As TextBox
        Dim mIssueItem As IssueItem
        Dim i As Integer = 0
        For Each mIssueItem In mIssue.IssueItems
            With mIssueItem
                Try
                    txtValue = CType(Me.dgIssueItems.Rows(i).FindControl("txtQty"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")
                Catch ex As Exception
                End Try
            End With
            i = i + 1
        Next
    End Sub
    Private Function getVendorStatus(ByVal TransTypeID As Integer, ByVal Type As RequstFor) As Boolean
        If Type = RequstFor.Supplier Then                                 'Issue
            Select Case CType(TransTypeID, Trans)
                Case Util.Trans.ExchangeRepairIssueToVendor
                    Return True
                Case Util.Trans.LoanIssueToVendor
                    Return True
                Case Util.Trans.IssuetoSupplierNone Or Util.Trans.IssuetoSupplierasReturn
                    Return True
                Case Else
                    Return False
            End Select
        ElseIf Type = RequstFor.Customer Then                              'Issue    
            Select Case CType(TransTypeID, Trans)
                Case Util.Trans.IssueToCustomer
                    Return True
                Case Util.Trans.LoanIssueToCustomer
                    Return True
                Case Util.Trans.IssueToCustomerAsRepairedReturn
                    Return True
                Case Util.Trans.IssueToCustomerAsNone
                    Return True

                Case Else
                    Return False
            End Select
        End If
    End Function
    Private Sub IssueTo()
        If mIssue.TransTypeID = Trans.ExchangeRepairIssueToVendor Or mIssue.TransTypeID = Trans.LoanIssueToVendor Or mIssue.TransTypeID = Trans.IssueToCustomer Or mIssue.TransTypeID = Trans.LoanIssueToCustomer Or mIssue.TransTypeID = Trans.IssuetoSupplierNone Or mIssue.TransTypeID = Trans.DisacrdPart Or mIssue.TransTypeID = Trans.IssuetoSupplierasRentalLease Or mIssue.TransTypeID = Trans.IssueToCustomerAsRepairedReturn Or mIssue.TransTypeID = Trans.IssueforLoanReturntoCustomer Or mIssue.TransTypeID = Trans.IssueforLoanReturntoSupplier Then
            mIssueTo = IIf(cmbVendorList.SelectedIndex > 0, cmbVendorList.SelectedItem.Text, "")
        ElseIf mIssue.TransTypeID = Trans.IssueToAircraft Or mIssue.TransTypeID = Trans.LoanIssuedToAircraft Then 'Or mIssue.TransTypeID = Trans.IssueToRequisition
            mIssueTo = IIf(cmbAircraftList.SelectedIndex > 0, cmbAircraftList.SelectedItem.Text, "")
        ElseIf mIssue.TransTypeID = Trans.IssueToStore Or mIssue.TransTypeID = Trans.LoanIssueToStore Or mIssue.TransTypeID = Trans.LoanReturnToStore Then
            mIssueTo = IIf(cmbLocationStore.SelectedIndex > 0, cmbLocationStore.SelectedItem.Text, "")
        ElseIf mIssue.TransTypeID = Trans.IssueToWorkShop Or mIssue.TransTypeID = Trans.LoanIssueToWorkShop Then            'Added By Prashant 7/4/2008
            mIssueTo = IIf(cmbWorkShop.SelectedIndex > 0, cmbWorkShop.SelectedItem.Text, "")
        ElseIf mIssue.TransTypeID = Trans.IssueToWorkOrder Or mIssue.TransTypeID = Trans.IssueToWorkOrderAsSpares Or mIssue.TransTypeID = Trans.IssueToWorkOrderAsTools Then           'Added By Prashant 7/4/2008
            mIssueTo = IIf(cmbWorkOrder.SelectedIndex > 0, cmbWorkOrder.SelectedItem.Text, "")
        End If
    End Sub
    'Comment Sankalp 29-09-25
    'Added By Utkarsh ON 18-Oct-2012 FOR ALL18102012
    'Private Sub AttachMyFile()
    '    Try
    '        mIssue.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
    '        mIssue.Size = Session("FileUpload.FileSize")
    '        mIssue.Extension = Session("FileUpload.FileExtension")
    '        Session("mIssue") = mIssue
    '        Session.Remove("FileUpload.FileSize")
    '        Session.Remove("FileUpload.FileContent")
    '        Session.Remove("FileUpload.FileExtension")
    '        ControlVisibilityForFileAttachment()
    '    Catch ex As Exception
    '        MSGBoxCtrl.show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
    '    End Try
    'End Sub
    'End
    'Private Sub ControlVisibilityForFileAttachment()
    '    If mIssue.Size > 0 Then
    '        'ImageButton1.Visible = True
    '        If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
    '            'Sankalp 29-09-25
    '            'btnSelectFile.Disabled = True
    '            'btnDelAttach.Enabled = False
    '            'btnDelAttach.ToolTip = "You are not authorized user "
    '            'ImageButton1.Enabled = False
    '            'ImageButton1.ToolTip = "You are not authorized user "
    '            btnSelectFiles.Enabled = False
    '            btnSelectFiles.ToolTip = "You are not authorized user "
    '        Else
    '            btnSelectFiles.Enabled = True
    '            'btnDelAttach.Enabled = True
    '        End If
    '    Else
    '        'ImageButton1.Visible = False
    '        'btnDelAttach.Enabled = False
    '    End If
    'End Sub
    'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries
    Public Sub CreateAutoReceiptCumInvoice()
        Session("sender") = ""
        DataFieldBind()
        If User.IsInRole("RCIFromStoreNew") Then
            CreateRCI()
        Else
            lblAlertTitle.Text = "Save Alert !"
            lblAlertMessage.Text = "You are not authorized user to create RCI."
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", "OpenAlert();", True)
            Exit Sub
        End If
    End Sub
    'End
    'Added By Utkarsh ON 22-Jul-2013 FOR ALL19072013
    Private Sub ToCreateRCI()
        If mIssue.TransTypeID = 15 Then
            MSGBoxCtrl.Show("RCI Creation !", "Do you want to create RCI for this issue ? ", "", MsgBoxStyle.YesNo, "ReceiptCumInvoiceCreation")
            Session("ReceiptCumInvoiceCreate") = "ReceiptCumInvoiceCreation"
            Exit Sub
        End If
    End Sub
    Private Sub CreateRCI()
        mReceiptCumInvoice = ReceiptCumInvoice.NewReceiptCumInvoice(Util.Trans.ReceivedFromOtherStore)
        mReceiptCumInvoice.RecCumInvDate = mIssue.IDate
        mReceiptCumInvoice.FromTypeID = 8  'Store 
        mReceiptCumInvoice.StoreID = mIssue.StoreID
        mReceiptCumInvoice.StoreName = mStoreList.Item(mIssue.StoreID).Name

        Dim mBaseCurrency As Currency
        mBaseCurrency = Currency.GetBaseCurrency()
        mReceiptCumInvoice.CurrencyID = mBaseCurrency.ID
        mReceiptCumInvoice.ConversionFactor = mBaseCurrency.ConversionFactor
        mReceiptCumInvoice.StatusID = 2
        mReceiptCumInvoice.UserName = User.Identity.Name
        mReceiptCumInvoice.AuthorizedBy = User.Identity.Name
        Dim mPendingToReceiveTransItemList As PendingToReceiveTransItemList
        mPendingToReceiveTransItemList = PendingToReceiveTransItemList.GetPendingToReceiveTransItemList(8, Guid.Empty, 0, mIssue.IDate.ToString, mIssue.ID, IsFromIssueBERParts:=False)

        Session("mPendingToReceiveTransItemList") = mPendingToReceiveTransItemList

        If mPendingToReceiveTransItemList.Count > 0 Then
            For i As Integer = 0 To mPendingToReceiveTransItemList.Count - 1
                mReceiptCumInvoice.ReceiptCumInvoiceItems.Add(mReceiptCumInvoice.ID)
                ItemForRCI(i)
            Next
            mReceiptCumInvoice.Invoice.CalculateTotal()
            Dim strMsg As String = ""

            'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries
            If Session("AddTransTextSeries") = "True" Then

                mReceiptCumInvoice.RecText = Session("TransText_ForTransSeries")
                mReceiptCumInvoice.RecNo = Session("TransNo_ForTransSeries")

                Session("AddTransTextSeries") = "False"

                Session.Remove("TransName_ForTransSeries")
                Session.Remove("TransText_ForTransSeries")
                Session.Remove("TransNo_ForTransSeries")

            End If
            'End

            If mReceiptCumInvoice.IsValid Then
                'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries
                If (mReceiptCumInvoice.IsNew) And (mReceiptCumInvoice.RecText = "") Then

                    Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mReceiptCumInvoice.TransTypeID, mReceiptCumInvoice.RecCumInvDateFormatted)

                    If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mReceiptCumInvoice.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mReceiptCumInvoice.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mReceiptCumInvoice.TransTypeID).TransText = "")) Then
                        Dim str = "<script language='javascript'>openledgersame('" + "wfIssue_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") + "'); </script>"

                        Session("BackPagestr_ForTransSeries") = str
                        Session("TransName_ForTransSeries") = "ReceiptCumInvoice"
                        Session("TransTypeID_ForTransSeries") = mReceiptCumInvoice.TransTypeID
                        Session("TransDate_ForTransSeries") = mReceiptCumInvoice.RecCumInvDateFormatted

                        MSGBoxCtrl.Show("Goods Receipt Transaction Series", "You have requested to create Goods Receipt against this Issue. But, system does not find transaction series for new Goods Receipt. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "ReceiptCumInvoiceTransTextSeriesAlert")
                        SetObject()
                        Session("mIssue") = mIssue
                        Exit Sub
                    Else
                        Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                        If mAutoRenewTransTextSeries.IsRenewed Then
                            With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mReceiptCumInvoice.TransTypeID)
                                mReceiptCumInvoice.RecText = .TransText
                                mReceiptCumInvoice.RecNo = .StartingTransNo
                            End With
                        Else
                            Dim str = "<script language='javascript'>openledgersame('wfIssue_Ajax.aspx');</script>"

                            Session("BackPagestr_ForTransSeries") = str

                            Session("TransName_ForTransSeries") = "ReceiptCumInvoice"
                            Session("TransTypeID_ForTransSeries") = mReceiptCumInvoice.TransTypeID
                            Session("TransDate_ForTransSeries") = mReceiptCumInvoice.RecCumInvDateFormatted
                            Session("AddTransTextSeries") = "True"

                            Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                        End If
                    End If

                    'Session("AddTransTextSeries") = "True"
                    'Session("sender") = "IssueCreate" 'Need to set again
                    'Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                End If
                'End
                mReceiptCumInvoice.Save()

                Session("ReceiptCumInvoiceCreate") = "" 'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries

                Dim mRCIDetail As String = "RCI No . : <b>" & mReceiptCumInvoice.ReceiptNo + " </b>" + " Dated : <b>" & mReceiptCumInvoice.RecCumInvDateFormatted & "</b> From : <b>" & mStoreList(mReceiptCumInvoice.StoreID).LocationStore & "</b>"
                lblAlertTitle.Text = "RCI created successfully !"
                lblAlertMessage.Text = mRCIDetail

                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "OpenAlertMessage", "OpenAlert();", True)

                mRCIDetail = mRCIDetail.Replace("<b>", "").Replace("</b>", "")
                MarkLog(Util.Action.Authorize, "RCIFromStore", mRCIDetail, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
            Else
                If mReceiptCumInvoice.GetBrokenRulesCollection.Count > 0 Then
                    For i As Integer = 0 To mReceiptCumInvoice.GetBrokenRulesCollection.Count - 1
                        strMsg = strMsg + mReceiptCumInvoice.GetBrokenRulesCollection(i).Description + "<Br>"
                    Next
                    lblAlertTitle.Text = "Save Alert !"
                    lblAlertMessage.Text = strMsg
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", "OpenAlert();", True)
                End If
            End If
            upnlMessBox.Update()
        End If
    End Sub
    Private Sub ItemForRCI(ByVal Index As Integer)
        mPendingToReceiveTransItemList = Session("mPendingToReceiveTransItemList")

        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = mPendingToReceiveTransItemList(Index).Type
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mPendingToReceiveTransItemList(Index).IssueItemID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = False

        'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemList(Index).UnitID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemList(Index).IssueItemUnitID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = CDec(mPendingToReceiveTransItemList(Index).IssueItemDisplayQty) 'CDec(mPendingToReceiveTransItemList(Index).PendingItemQty)
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InvoiceItemRateForRCI 'Added by Prashant 30-Aug-2013 ALL30082013-1
        'Added By Prashant On 27-Apr-2021 ALL26042021
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RateEffRateDiffrenceForRCI * CDec(mPendingToReceiveTransItemList(Index).IssueItemDisplayQty))
        'End of Added By Prashant On 27-Apr-2021 ALL26042021 
        'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RateEffRateDiffrenceForRCI 'Added by Prashant 30-Aug-2013 ALL30082013-1
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.SerialNo
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteNo = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ReleaseNoteNo
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteDate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ReleaseNoteDate
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTypeID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ItemTypeID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ExpiryDate
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpQtrs = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ExpQtrs
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpYear = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ExpYear
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureQtrs = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.CureQtrs
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureYear = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.CureYear
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BatchNo = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.BatchNo
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.StartDate
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ToStoreID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptDate = mPendingToReceiveTransItemList(Index).OriginalReceiptDate
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptTextNo = mPendingToReceiveTransItemList(Index).OriginalReceiptTextNo
        'PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP
        If mPendingToReceiveTransItemList(Index).IsSerialized = True Then
            mLastWarrantyInformation = LastWarrantyInformation.GetLastWarrantyInformation(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.ToString, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo)
            If mLastWarrantyInformation.Count > 0 Then
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsWarranty = mLastWarrantyInformation(0).IsWarranty
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays = mLastWarrantyInformation(0).WarrantyInDays
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate = mLastWarrantyInformation(0).WarrantyStartDateFormatted.ToString
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate = mLastWarrantyInformation(0).WarrantyExpiryDateFormatted.ToString
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CodeNo = mLastWarrantyInformation(0).CodeNo
                mLastWarrantyInformation = Nothing
            End If
        End If
        'PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP
    End Sub
    'End
    'Added By Prashant 16-Sep-2013 ALL16092013
    Public Sub SetReport(Optional ByVal IsForIssueTag As Boolean = False, Optional ByVal ByMail As Boolean = False)
        Dim objIssueReceiptItemPeriodChildList As New rptIssueReceiptItemPeriodChildList
        Dim objIssue As rptIssues
        Dim objChilds As rptIssueChields
        Dim letter As rptLetterHead
        Dim ds As New dsIssue
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim _ReportHelper As New ReportHelper
        If IsForIssueTag = False Then
            Dim result = _ReportHelper.GetIssueDetailedReport(Id:=mIssue.ID, False, ByMail)
            myReport = result.Item1
            Session("myReport") = myReport
            If ByMail = True Then
                Dim Array() As String = result.Item3.Split({", "}, StringSplitOptions.RemoveEmptyEntries)
                Dim Info As String = String.Empty
                If Array.Length >= 1 Then
                    Info = Array(0)
                End If
                SendMailFile.SendMailFile(Session("myReport"), Thread.CurrentPrincipal.Identity.Name,
                                      "Requested Parts/Components has been Issued/Dispatched.",
                                      mIssue.IssueNo,
                                      Info:=Info,
                                      VendorEmailID:="",
                                      ToMailID:=Session("ToSendMailIDs"),
                                      CCMailID:=Session("CcSendMailIDs"),
                                      ReportPath:="", ReportByMail:=False,
                                      Remark:=Session("SendMailRemark"),
                                      ReportGeneratedBy:=Session("ReportGenratedBy"),
                                      SmtpHost:=Session("SmtpHost"),
                                      SmtpPort:=Session("SmtpPort"),
                                      SmtpUser:=Session("SmtpUser"),
                                      SmtpPassword:=Session("SmtpPassword"))
            End If
            Exit Sub
            'If AppSettings("ClientCode") = "IRM" Then 'Added By Vikrant On 08-Nov-2021 For IRM08112021-1
            '    myReport = New crptIssueDetailPotraitIRM
            'Else 'Existing Code
            '    If mIssue.TransTypeID = Flypal.Util.Trans.DisacrdPart Then
            '        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
            '            myReport = New crptIssueLandScapeDiscard
            '        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            '            myReport = New crptIssueDetailPotraitTAALDiscard
            '        ElseIf AppSettings("ClientCode") = "HSC" Then 'HeliStar Added by Prashant HSC22082019
            '            myReport = New crptIssueDetailPotraitDiscardForHeliStar
            '        Else
            '            myReport = New crptIssueDetailPotraitDiscard
            '        End If
            '    Else
            '        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
            '            myReport = New crptIssueLandScape
            '        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            '            myReport = New crptIssueDetailPotraitTAAL
            '        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "YA") Then

            '            If mIssue.TransTypeID = 16 Then
            '                myReport = New crptIssueDetailPotraitYA
            '            Else
            '                myReport = New crptIssueDetailPotrait
            '            End If
            '            objIssueReceiptItemPeriodChildList = rptIssueReceiptItemPeriodChildList.GetPeriodChildList(mIssue.ID)
            '        ElseIf AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ" Then ' SPZ Code added by Saylee on 13-Jun-2022  'Added By Vikrant On 18-Jun-2014 For Deccan18062014 -1
            '            myReport = New crptIssueDetailPotraitDeccan
            '        ElseIf AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
            '            'myReport = New crptIssueDetailPotraitForBA
            '            If mIssue.TransTypeID = Flypal.Util.Trans.LoanIssueToWorkShop And AppSettings("ClientCode") = "BA" Then
            '                myReport = New crptIssueDetailPotraitForBAWithLoanIssueWorkshop
            '            Else
            '                myReport = New crptIssueDetailPotraitForBA
            '            End If

            '        ElseIf AppSettings("ClientCode") = "HSC" Then 'HeliStar Added by Prashant HSC22082019
            '            myReport = New crptIssueDetailPotraitForHeliStar
            '        ElseIf AppSettings("ClientCode") = "STR" Then 'Added by Prashant on 29-Jan-2021 STR29012021 As Per Mail
            '            If mIssue.TransTypeID = 60 Then 'Issue To work order As Tools
            '                myReport = New crptToolsCheckOutStarAir
            '            Else
            '                myReport = New crptIssueDetailPotrait
            '            End If
            '        Else
            '            myReport = New crptIssueDetailPotrait
            '        End If
            '    End If
            'End If
            'objIssue = rptIssues.GetIssues(mIssue.ID)
            'objChilds = rptIssueChields.GetIssuechilds(mIssue.ID)
        Else
            'myReport = New TagIssue
            If AppSettings("ClientCode") = "IND" And mIssue.TransTypeID = 19 Then '19 Issue to discard Added By Prashant 19082020
                myReport = New TagIssueForIND
            ElseIf AppSettings("ClientCode") = "IRM1" And mIssue.TransTypeID = 19 Then '19 Issue to discard Added By Shital 09102021
                myReport = New TagIssueForIRM
            Else
                myReport = New TagIssue
            End If
            objIssue = rptIssues.GetIssues(mIssue.ID)
            objChilds = rptIssueChields.GetIssuechilds(mIssue.ID, IsForIssueTag:=True)
        End If
        '---------- 'Addded by vikrant on 7-sept-2011------------
        Dim mSearchstring As String
        If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then
            mSearchstring = "True"
        Else
            mSearchstring = "False"
        End If
        '--------------------------------------------------------

        'Added By Vikrant On 08-Nov-2021 For IRM08112021-1
        Dim ReqDate As String = ""
        Dim ReqNo As String = ""
        If Not mIssue.ReqDateFormatted Is Nothing Then
            ReqDate = mIssue.ReqDateFormatted.ToString
        End If
        If Not mIssue.ReqTextNo Is Nothing Then
            ReqNo = mIssue.ReqTextNo
        End If
        'End
        letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "",
                                                 mSearchstring, AppSettings("Logo"), AppSettings("PrintBarCodeOnItemDetail"),
                                                 ClientCode:=AppSettings("ClientCode"), SearchString4:=mTransactionList.Item(mIssue.TransTypeID).FormRevisionNo,
                                             SearchString5:=mTransactionList.Item(mIssue.TransTypeID).FormRevisionDate, SearchString6:=ReqNo,
                                             SearchString7:=ReqDate, SearchString8:=mIssue.nWO.WONumber)
        If letter.Count > 0 Then
            BaseCurrencysymbol = letter(0).BaseCurrencysymbol
            Session("BaseCurrencysymbol") = BaseCurrencysymbol
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)

        Dim mCompanyDetail As New CompanyDetail
        SetUserMailIDs()
        Dim mReport As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2,
                                     mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, ReportName:="", SearchStr1:=AppSettings("FormNumberOnIssue"),
                                     SearchStr2:=AppSettings("IssueNumber"), SearchStr3:=Session("FormRevisionNo"), SearchStr4:=AppSettings("IssueDate"),
                                     SearchStr5:=Session("FormRevisionDate"), ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"),
                                     SearchStr6:="")
        da.Fill(ds, objIssue)
        da.Fill(ds, objChilds)
        da.Fill(ds, letter)
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "YA") Then
            da.Fill(ds, objIssueReceiptItemPeriodChildList)
        End If
        da.Fill(ds, mrptImage)
        da.Fill(ds, mReport)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        If ByMail = True Then
            Dim str As String
            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri""> Requested Parts/Components has been Issued/Dispatched." + " </font></P> ")
            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Issue No.: <b> " & mIssue.IssueNo.ToString & "</b> Issue Date: <b> " + mIssue.IDateFormatted + "</b> Issued By: <b> " + Thread.CurrentPrincipal.Identity.Name + " </b> on: <b> " + New SmartDate(Today.Date).FormattedText + "</b>.</font></P> ")
            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri""> Please View Parts/Components Details in Attached File. </font></P>")
            str = str + ("</body></html>")
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Requested Parts/Components has been Issued/Dispatched.", mIssue.IssueNo, Info:=str, VendorEmailID:="", ToMailID:=Session("ToSendMailIDs"),
                                      CCMailID:=Session("CcSendMailIDs"), ReportPath:="", ReportByMail:=False, Remark:=Session("SendMailRemark"),
                                      ReportGeneratedBy:=Session("ReportGenratedBy"),
                                      SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
        End If
    End Sub
    Public Sub SendMail()
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        SetUserMailIDs()
        'If AppSettings("MailsRequire") = "True" Then
        If Session("MailsRequire") = "True" Then

            If Thread.CurrentPrincipal.Identity.Name.ToUpper = "BTPLADMIN" Or Thread.CurrentPrincipal.Identity.Name.ToUpper = "BYTZADMIN" Then ' BYTZADMIN For Deccan 'Added by Prashant 15-Oct-2019 
                'Do nothing
                Exit Sub
            End If
            SetReport()
            Dim str As String
            If mIssue.TransTypeID = 19 Then
                str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Issue To Part Discard No. <b> " & mIssue.IssueNo & "</b> Dated: <b> " + mIssue.IDateFormatted + IIf(cmbVendorList.SelectedIndex > 0, "</b> To Supplier: <b> " + cmbVendorList.SelectedItem.Text, "") + "</b> From Store: <b> " + cmbStoreList.SelectedItem.Text + "</b> of Value: <b> " + "(" + Session("BaseCurrencysymbol") + ") " + mIssue.TotalDiscardAmt.ToString + "</b> has been Authorized By User: <b> " + Thread.CurrentPrincipal.Identity.Name + " </b> on: <b> " + New SmartDate(Today.Date).FormattedText + "</b>,</font></P> ")
            Else
                str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri""><b> " & mIssue.IssueNo & "</b> Dated: <b> " + mIssue.IDateFormatted + " From Store: " + cmbStoreList.SelectedItem.Text + "has been Authorized By User: <b> " + Thread.CurrentPrincipal.Identity.Name + " </b> on: <b> " + New SmartDate(Today.Date).FormattedText + "</b>,</font></P> ")
            End If
            str = str + ("</body></html>")
            Session.Remove("BaseCurrencysymbol")
            Dim EmployeeMailID As String
            If AppSettings("ClientCode") = "IND" And mIssue.ToTypeID = 18 And mIssue.TransTypeID = 14 Then 'Issue To Aircraft As Requisition
                EmployeeMailID = Employee.GetEmployee(mIssue.ReqEmployeeID).Email
                Session("UserEmailID") = EmployeeMailID
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Issue", mIssue.IssueNo, Info:=str, ToMailID:=EmployeeMailID.ToString, Remark:="", ReportGeneratedBy:="",
                                          SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Issue", mIssue.IssueNo, Info:=str, Remark:="", ReportGeneratedBy:="",
                                          SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
            End If

        End If
    End Sub '-------------------------------------------------------------
    Public Sub SetRequistionReport(Optional ByVal ByMail As Boolean = False)
        Dim ReqNos As New StringBuilder
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As DataSet
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim AircraftList As New StringBuilder


        If AppSettings("ClientCode") = "STR" Then
            myReport = New crptIssueAgainstRequisitionItemStarAir
        Else
            myReport = New crptIssueAgainstRequisitionItem
        End If
        ds = New dsIssueAgainstRequisitionItem
        'Commented and added by Prashant After Discussion with Deven Sir 19-Jul-2018 ALL18072018
        'Dim mIssueAgainstRequisitionItem As IssueAgainstRequisitionItem = IssueAgainstRequisitionItem.GetIssueAgainstRequisitionItem(Guid.Empty, True, mIssue.ID.ToString)
        Dim mIssueAgainstRequisitionItem As IssueAgainstRequisitionItem = IssueAgainstRequisitionItem.GetIssueAgainstRequisitionItem(mIssue.RequisitionID, True, ClientCode:=AppSettings("ClientCode"))
        'End of Comment
        Dim mTemp As New Hashtable
        Dim mTemp1 As New Hashtable
        For i As Integer = 0 To mIssueAgainstRequisitionItem.Count - 1
            If Not mTemp.ContainsValue(mIssueAgainstRequisitionItem(i).RequisitionNumber) Then
                mTemp.Add(i, mIssueAgainstRequisitionItem(i).RequisitionNumber)
                ReqNos.Append(mTemp(i) + ",")
            End If
            If Not mTemp1.ContainsValue(mIssueAgainstRequisitionItem(i).RegNo) Then
                mTemp1.Add(i, mIssueAgainstRequisitionItem(i).RegNo)
                AircraftList.Append(mTemp1(i) + ",")
            End If
        Next

        If ReqNos.Length > 0 Then
            ReqNos.Replace(",", "", ReqNos.Length - 1, 1)
        End If
        If AircraftList.Length > 0 Then
            AircraftList.Replace(",", "", AircraftList.Length - 1, 1)
        End If
        da.Fill(ds, mIssueAgainstRequisitionItem)

        Dim mCompanyDetail As New CompanyDetail

        Dim mReport As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2,
                                      mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, "",
                                       ReqNos.ToString, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"),
                                       IIf(mIssue.TransTypeID = 14 Or mIssue.TransTypeID = 59, "65", "72"), , ,
                                       SearchStr9:=IIf(mIssue.TransTypeID = 14, AircraftList.ToString + "/" + mIssueAgainstRequisitionItem(0).RequisitionEngineeringBranch, mIssueAgainstRequisitionItem(0).RequisitionEngineeringBranch),
                                       SearchStr10:=AppSettings("Logo"), SearchStr11:=AppSettings("ClientCode"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mReport)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        If ByMail = True Then
            Dim str As String
            str = str + ("<html><head></head><body >" & "<P><font face=""Calibri""> Requested Parts/Components with Reference to </font></P>")
            str = str + ("<P><font face=""Calibri""> Requisition No.: <b> " + ReqNos.ToString + "</b></font></P> ")
            str = str + ("<P><font face=""Calibri""> For Aircraft: <b> " + mIssue.RegNo + "</b></font></P> ")

            If mIssueAgainstRequisitionItem(0).WONo = "" Then
                'do nothing
            Else
                str = str + ("<P><font face=""Calibri""> For Work order: <b> " + mIssueAgainstRequisitionItem(0).WONo + "</b></font></P> ")
            End If
            str = str + ("<P><font face=""Calibri""> Requested by: <b> " + mIssueAgainstRequisitionItem(0).EmployeeName + "</b></font></P> ")
            str = str + ("<P><font face=""Calibri""> Dated: <b> " + mIssueAgainstRequisitionItem(0).RequisitionDateFormatted + "</b></font></P> ")
            str = str + ("<P><font face=""Calibri""> has been Issued/Dispatched. </font></P> ")
            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri""> Please View Parts/Components Details in Attached File. </font></P>")
            str = str + ("</body></html>")
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Parts/Components has been Issued/Dispatched with Reference to " + ReqNos.ToString,
                                      mIssue.IssueNo, Info:=str, VendorEmailID:="", ToMailID:=Session("ToSendMailIDs"), CCMailID:=Session("CcSendMailIDs"), ReportPath:="",
                                      ReportByMail:=False, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
                                      SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If
    End Sub
    'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Public Sub SetUserMailIDs()
        Session("UserEmailID") = mTransactionList.Item(mIssue.TransTypeID).SendToMailID
        Session("MailsRequire") = mTransactionList.Item(mIssue.TransTypeID).MailsRequire
        Session("SmtpHost") = mTransactionList.Item(mIssue.TransTypeID).SmtpHost
        Session("SmtpPort") = mTransactionList.Item(mIssue.TransTypeID).SmtpPort
        Session("SmtpUser") = mTransactionList.Item(mIssue.TransTypeID).SmtpUser
        Session("SmtpPassword") = mTransactionList.Item(mIssue.TransTypeID).SmtpPassword
        Session("FormRevisionNo") = mTransactionList.Item(mIssue.TransTypeID).FormRevisionNo
        Session("FormRevisionDate") = mTransactionList.Item(mIssue.TransTypeID).FormRevisionDate
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mStoreList = StoreList.GetStoreList(0, , True)
        mTypeList1 = TypeList1.GetTypeList("3", mIssue.TransTypeID)             'For Issue
        'mVendorList = VendorList.GetVendortList(0, , , , , , True, getVendorStatus(mIssue.TransTypeID, RequstFor.Customer), getVendorStatus(mIssue.TransTypeID, RequstFor.Supplier))
        '--------------------------------------
        If mIssue.TransTypeID = 19 Then 'Discard 'Added By Prashant 5-July-2011
            mVendorList = VendorList.GetVendortList(0, , , , , , True)
        Else
            '' mVendorList = VendorList.GetVendortList(0, , , , , , True, getVendorStatus(mIssue.TransTypeID, RequstFor.Customer), getVendorStatus(mIssue.TransTypeID, RequstFor.Supplier))
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "7AR") Then 'Added By Saylee 14-Oct-2024
                mVendorList = VendorList.GetVendortList(0, , , , , , True, getVendorStatus(mIssue.TransTypeID, RequstFor.Customer), getVendorStatus(mIssue.TransTypeID, RequstFor.Supplier), IsServiceProvider:=getVendorStatus(mIssue.TransTypeID, RequstFor.Supplier))
            Else
                mVendorList = VendorList.GetVendortList(0, , , , , , True, getVendorStatus(mIssue.TransTypeID, RequstFor.Customer), getVendorStatus(mIssue.TransTypeID, RequstFor.Supplier))
            End If
        End If
        '--------------------------------------
        'mMachineList = FlyPal22.Maintain.SelectList.GetMachineList(New SmartDate(mIssue.IDate.ToString).FormattedText, mIssue.IsNew, "<SELECT>")
        mMachineNameValueList = MachineNameValueList.GetMachineList(mIssue.IDateFormatted.ToString, IsTagRequired:=True, TagText:="(SELECT)", ForInventory:=True)

        mStatusList = StatusList.GetStatusList(mIssue.StatusID, True)
        ''OLD WO Object ===Commented by Saylee on 8-Dec-2010
        '' mWOList = FlyPal22.Maintain.WOList.GetWOList(, , 0, New SmartDate("01-01-1800").FormattedText, New SmartDate(mIssue.IDate.ToString).FormattedText, , , , , , , 2, , "<SELECT>")
        mnWOListForCombo = nWOListForCombo.GetnWOListForCombo("(SELECT)", , , New SmartDate("01-01-1800").FormattedText, New SmartDate(mIssue.IDate.ToString).FormattedText, , , )
        mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(SELECT)")

        cmbWorkShop.DataSource = mWorkShopList
        cmbStoreList.DataSource = mStoreList
        cmbToType.DataSource = mTypeList1
        cmbVendorList.DataSource = mVendorList
        cmbAircraftList.DataSource = mMachineNameValueList
        cmbLocationStore.DataSource = mStoreList
        ''cmbWO.DataSource = mWOList
        ''cmbWorkOrder.DataSource = mWOList

        cmbWorkOrder.DataSource = mnWOListForCombo
        dgIssueItems.DataSource = mIssue.IssueItems
        dgIssueTerms.DataSource = mIssue.IssueTerms
        txtIssueDate.Text = mIssue.IDateFormatted
        txtReqEmployeeName.Text = mIssue.ToolsIssuedToEmployeeName
        dgItemAttachment.DataSource = mIssue.FileAttachments     'Sankalp 29-09-25
        upnldgItemAttachment.DataBind() 'Sankalp 29-09-25
        SetSession()


        ''cmbWO.DataSource = mWOList
        ''cmbWorkOrder.DataSource = mWOList

        cmbWorkOrder.DataSource = mnWOListForCombo

        mRequisitionItemTypeList = RequisitionItemTypeList.GetRequisitionItemTypeList()  'Added By Prashant 13-Apr-2015 ALL13042015

        DataBind()
        Select Case mIssue.ToTypeID
            Case 1  'Vendor
                cmbVendorList.Visible = True
                btnAddSupplierSpecificTerms.Visible = True
                mIssue.IssueTo = IIf(cmbVendorList.SelectedIndex > 0, cmbVendorList.SelectedItem.Text, "")
            Case 2
                cmbAircraftList.Visible = True
                mIssue.IssueTo = IIf(cmbAircraftList.SelectedIndex > 0, cmbAircraftList.SelectedItem.Text, "")
            Case 7  'Discard
                cmbVendorList.Visible = True
                'lblSelectDetails.Visible = False
                lblSelectDetails.Visible = True 'Added By Prashant 5-July-2011
                lblSelectDetailsStar1.Visible = False
                mIssue.IssueTo = "Part Discard"
            Case 8  'Store
                cmbLocationStore.Visible = True
                mIssue.IssueTo = IIf(cmbLocationStore.SelectedIndex > 0, cmbLocationStore.SelectedItem.Text, "")
            Case 16 'Work Shop
                cmbWorkShop.Visible = True
                mIssue.IssueTo = IIf(cmbWorkShop.SelectedIndex > 0, cmbWorkShop.SelectedItem.Text, "")
            Case 17 'Work Order 
                cmbWorkOrder.Visible = True
            Case 18 'Issue Against Requisition
                If mIssue.TransTypeID = 14 Then
                    cmbAircraftList.Visible = True
                    mIssue.IssueTo = IIf(cmbAircraftList.SelectedIndex > 0, cmbAircraftList.SelectedItem.Text, "")
                ElseIf mIssue.TransTypeID = 44 Then
                    cmbWorkShop.Visible = True
                    mIssue.IssueTo = IIf(cmbWorkShop.SelectedIndex > 0, cmbWorkShop.SelectedItem.Text, "")
                ElseIf mIssue.TransTypeID = 59 Then 'Issue to work order as spare assembly/Component requisition Added By Prashant on 25-Jun-2021 STR25062021
                    cmbWorkOrder.Visible = True
                    mIssue.IssueTo = IIf(cmbWorkOrder.SelectedIndex > 0, cmbWorkOrder.SelectedItem.Text, "")
                End If

        End Select
        If mIssue.TransTypeID = 16 Or mIssue.TransTypeID = 24 Or mIssue.TransTypeID = 55 Or mIssue.TransTypeID = 49 Then
            btnAddSupplierSpecificTerms.Text = "Add Supplier Specific Terms"
            btnAddSupplierSpecificTerms.ToolTip = "Click To Add Supplier Specific Terms"
        ElseIf mIssue.TransTypeID = 25 Or mIssue.TransTypeID = 26 Then
            btnAddSupplierSpecificTerms.Text = "Add Customer Specific Terms"
            btnAddSupplierSpecificTerms.ToolTip = "Click To Add Customer Specific Terms"
        End If
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        Dim ToTypeId As Int16 = Val(cmbToType.SelectedValue)
        If custValidator.ControlToValidate = "txtIssueDate" Then
            If txtIssueDate.Text = "" Then
                custValidator.ErrorMessage = "Select Issue Date."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbStoreList" Then
            If cmbStoreList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select the store name from the list"
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbWorkShop" Then
            If cmbWorkShop.SelectedIndex <= 0 And ToTypeId = 16 Then
                custValidator.ErrorMessage = "Select WorkShop from the list."
                e.IsValid = False
            End If
            'ElseIf custValidator.ControlToValidate = "cmbWorkOrder" Then
            '    If cmbWorkOrder.SelectedIndex <= 0 And ToTypeId = 17 Then
            '        custValidator.ErrorMessage = "Select WorkOrder from the list."
            '        e.IsValid = False
            '    End If
        ElseIf custValidator.ControlToValidate = "cmbVendorList" Then
            If cmbVendorList.SelectedIndex <= 0 And ToTypeId = 1 Then
                custValidator.ErrorMessage = IIf(mIssue.TransTypeID = 26 Or mIssue.TransTypeID = 25, "Select Customer from Customer List.", "Select Supplier from Supplier List.")
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbAircraftList" Then
            If cmbAircraftList.SelectedIndex <= 0 And ToTypeId = 2 Then
                custValidator.ErrorMessage = "Select aircraft from aircraft list"
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbLocationStore" Then
            If cmbLocationStore.SelectedIndex <= 0 And ToTypeId = 8 Then
                custValidator.ErrorMessage = " Issue To - Select Store from the store list."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtPerson" Then
            If Len(txtPerson.Text.Trim) > 50 Then
                txtPerson.Text = txtPerson.Text.Trim.Substring(0, 46) + "..."
                custValidator.ErrorMessage = "Person field length must not be greater than 50 Character."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text.Trim) > 150 Then
                txtRemark.Text = txtRemark.Text.Trim.Substring(0, 146) + "..."
                custValidator.ErrorMessage = "Remark field length must not be greater than 150 Character."
                e.IsValid = False
            End If


        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addattributes()
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
                        'mIssue.No = Session("TransNo_ForTransSeries")
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
            DataFieldBind()
            SetPage()
            ControlVisibility()
            If mIssue.IsNew Then
                lblStatus.Text = "OPEN"
            End If
            If mIssue.IssueItems.Count > 0 Then
                Disable()
            Else
                Enable()
            End If
            'ControlVisibilityForFileAttachment()
            TextChanged(sender, e)
        End If
    End Sub
    'The Change made in the Date will effect to Issue Text and No.
    'DONE
    Private Sub txtIssueDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIssueDate.TextChanged
        mIssue.IDate = txtIssueDate.Text
        txtText.Text = mIssue.Text 'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries
        ''mWOList = FlyPal22.Maintain.WOList.GetWOList(, , 0, New SmartDate("01-01-1800").FormattedText, New SmartDate(mIssue.IDate.ToString).FormattedText, , , , , , , 2, , "<SELECT>")
        mnWOListForCombo = nWOListForCombo.GetnWOListForCombo("(SELECT)", , , New SmartDate("01-01-1800").FormattedText, New SmartDate(mIssue.IDate.ToString).FormattedText, , , )
        cmbWorkOrder.DataSource = mnWOListForCombo
        cmbWorkOrder.DataBind()
        Session("mWOList") = mnWOListForCombo
    End Sub
    'DONE
    Private Sub btnAddItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddItem.Click
        If IsValid Then
            SetObject()
            If (mIssue.IssueItems.Count = 0 And (mIssue.TransTypeID = 15 Or mIssue.TransTypeID = 19 Or mIssue.TransTypeID = 63)) Then Session("IsAllPartsSelected") = False
            mIssue.IssueItems.Add(mIssue.ID, mIssue.TransTypeID)
            Session("mIssue") = mIssue
            If mIssue.TransTypeID = 16 Then
                Response.Redirect("wfPendingToReturnForExchangeRepair_Ajax.aspx?BackPage=wfIssue_Ajax.aspx")
            ElseIf mIssue.TransTypeID = 18 Or mIssue.TransTypeID = 55 Then
                Response.Redirect("wfPendingLoanToReturn_Ajax.aspx?BackPage=wfIssue_Ajax.aspx")
            ElseIf mIssue.TransTypeID = 59 Then    'Added By Prashant 9-Dec-2010
                If mIssue.ToTypeID = 18 Then 'Issue to work order as Material Requisition Added By Prashant on 25-Jun-2021 STR25062021
                    Response.Redirect("wfRequisitionItemListForIssue_Ajax.aspx?BackPage=wfIssue_Ajax.aspx")
                Else
                    Response.Redirect("wfnPendingWOListForIssueSpares_Ajax.aspx?BackPage=wfIssue_Ajax.aspx")
                End If
            ElseIf mIssue.TransTypeID = 60 Then    'Added By Prashant 9-Dec-2010
                Response.Redirect("wfnPendingWOListForIssueTools_Ajax.aspx?BackPage=wfIssue_Ajax.aspx")
            ElseIf ((mIssue.TransTypeID = 14 Or mIssue.TransTypeID = 44) And mIssue.ToTypeID = 18) Then    'Added By Prashant 9-Dec-2010
                Response.Redirect("wfRequisitionItemListForIssue_Ajax.aspx?BackPage=wfIssue_Ajax.aspx")
            Else
                Session("Edit") = False
                Response.Redirect("wfIssueItem_Ajax.aspx?BackPage=wfIssue_Ajax.aspx")
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    'DONE
    Private Sub btnAddTerm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTerm.Click
        If IsValid Then
            SetObject()
            Session("mIssue") = mIssue
            Response.Redirect("wfIssueTerm_Ajax.aspx?BackPage=wfIssue_Ajax.aspx&Type=2")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    'DONE
    Private Sub btnAddSupplierSpecificTerms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSupplierSpecificTerms.Click
        mVendorTerms = VendorTerms.GetVendorTerms(New Guid(cmbVendorList.SelectedValue), mIssue.TransTypeID, mIssue.ID.ToString, 2)
        Dim i As Integer = 0
        While i < mVendorTerms.Count
            If mIssue.IssueTerms.Contains(mVendorTerms.Item(i).TermID) = False Then
                mIssue.IssueTerms.Add(mIssue.ID)
                mIssue.IssueTerms.CurrentItem.Terms = mVendorTerms.Item(i).Terms
                mIssue.IssueTerms.CurrentItem.TermID = mVendorTerms.Item(i).TermID
            End If
            i = i + 1
        End While
        dgIssueTerms.DataSource = mVendorTerms
        dgIssueTerms.DataBind()
    End Sub
    'DONE
    Private Sub dgIssueItems_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgIssueItems.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                'Dim Index As Int32 = CInt(e.CommandArgument) ''Ajay 28-02-2023
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim Index As Int32 = gvr.RowIndex
                Session("Edit") = True
                SetObject()
                mIssue.IssueItems.CurrentIndex = Index
                Dim mPendingItemList As PendingToIssueList
                mPendingItemList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, mIssue.IssueItems(Index).ItemName, , , , ,
                                                                            mIssue.IDate.ToString, ToTypeIDOfIssue:=mIssue.ToTypeID)
                Session("AvailableQuantity") = 0
                For i As Integer = 0 To mPendingItemList.Count - 1
                    If mPendingItemList(i).ReceiptItemID.Equals(mIssue.IssueItems(Index).ReceiptItemID) Then
                        Session("AvailableQuantity") = mPendingItemList(i).AvailableQuantity
                        Exit For
                    End If
                Next
                Session("mIssue") = mIssue
                Response.Redirect("wfIssueItem_Ajax.aspx?BackPage=wfIssue_Ajax.aspx")
            Case "DeleteRec"
                'Dim Index As Int32 = CInt(e.CommandArgument) '' Ajay 28-02-2023
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim Index As Int32 = gvr.RowIndex
                DeleteRecord(Index)
        End Select
    End Sub
    'DONE
    Private Sub dgIssueTerms_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgIssueTerms.RowCommand
        Select Case e.CommandName

            Case "DeleteRec"

                'Dim index As Int32 = CInt(e.CommandArgument) ''Ajay 28-02-2023
                'mIssue.IssueTerms.CurrentIndex = index
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim Index As Int32 = gvr.RowIndex
                mIssue.IssueTerms.CurrentIndex = Index
                '------------
                mIssue.IssueTerms.Remove(mIssue.IssueTerms.CurrentItem)
                Session("mIssue") = mIssue
                dgIssueTerms.DataSource = mIssue.IssueTerms
                dgIssueTerms.DataBind()
        End Select
    End Sub
    'DONE
    Private Sub cmbToType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbToType.SelectedIndexChanged
        Dim ToType As Int16 = Val(cmbToType.SelectedValue)
        Select Case ToType
            Case 1   'Vendor
                lblSelectDetails.Visible = True
                cmbVendorList.Visible = True
                cmbAircraftList.Visible = False
                cmbLocationStore.Visible = False
                btnAddSupplierSpecificTerms.Visible = True
            Case 2  'Aircraft
                lblSelectDetails.Visible = True
                cmbAircraftList.Visible = True
                cmbVendorList.Visible = False
                cmbLocationStore.Visible = False
                btnAddSupplierSpecificTerms.Visible = False
            Case 7
                'lblSelectDetails.Visible = False
                lblSelectDetails.Visible = True 'Added By Prashant 5-July-2011
                cmbLocationStore.Visible = False
                cmbAircraftList.Visible = False
                'cmbVendorList.Visible = False
                cmbVendorList.Visible = True 'Added By Prashant 5-July-2011
                btnAddSupplierSpecificTerms.Visible = False
            Case 8   'Store
                lblSelectDetails.Visible = True
                cmbLocationStore.Visible = True
                cmbAircraftList.Visible = False
                cmbVendorList.Visible = False
                btnAddSupplierSpecificTerms.Visible = False
            Case 18
                lblSelectDetails.Visible = True
                If mIssue.TransTypeID = 14 Then
                    cmbAircraftList.Visible = True
                ElseIf mIssue.TransTypeID = 44 Then
                    cmbWorkShop.Visible = True
                ElseIf mIssue.TransTypeID = 59 Then 'Issue to work order as spare assembly/Component requisition Added By Prashant on 25-Jun-2021 STR25062021
                    cmbWorkOrder.Visible = True
                End If
                cmbVendorList.Visible = False
                cmbLocationStore.Visible = False
                btnAddSupplierSpecificTerms.Visible = False
        End Select
        If cmbToType.Enabled = True Then
            cmbToType.Focus()
        End If
    End Sub
	'DONE
	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		If IsValid Then
			Save()
		Else
			upnlValidationSummary.Update()
		End If
	End Sub

	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		'MarkLog(Util.Action.Close, ModuleName, "", Util.ErrorType.NoError, Guid.Empty)
		SetObject()
		IssueTo()
		mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + IIf(mIssueTo <> "", " to " + mIssueTo, "")
		MarkLog(Util.Action.Close, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)

		If Not mOpenFrom Is Nothing AndAlso mOpenFrom = "FromwfStockCard" Then  'Added By Prashant 3-Apr-2014 ALL03042014
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		ElseIf Not mOpenFrom Is Nothing AndAlso mOpenFrom = "FromReqItemStatusReport" Then 'Added By Vikrant on 13-Oct-2014 For Req Item Status Report
			RemoveSession()
			Session.Remove("ModuleName")
			Response.Redirect("Index.aspx")
		End If

		Session("IsValid") = IsValid

		If mIssue.StatusID <> 2 Then

			If mIssue.IsDirty Then
				MSGBoxCtrl.Show(MSGBox.Message_Title.CloseConfirm, MSGBox.Message_Text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
				If IsValid Then SetObject()
			Else
				GoTo RedirectToIndex
			End If

		ElseIf Session("IsAttachmentNotSave") = True Then

			If mIssue.IsDirty Then
				Dim ExtraMessage As String = "As there is a change in Attachment. Do you want to save Attachment?"
				MSGBoxCtrl.Show(MSGBox.Message_Title.Confirmation, MSGBox.Message_Text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "SaveAttachment")
				If IsValid Then SetObject()
			Else
				GoTo RedirectToIndex
			End If

		Else
RedirectToIndex:
			RemoveSession()
			Session.Remove("IsAllPartsSelected") 'Added By Vikrant On 03-Feb-2016 For ALL03022016
			Session.Remove("ModuleName")
			Response.Redirect("Index.aspx")
		End If


		'If mIssue.StatusID <> 2 Then
		'    If mIssue.IsDirty Then
		'        MSGBoxCtrl.Show(MSGBox.Message_Title.CloseConfirm, MSGBox.Message_Text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
		'        If IsValid Then
		'            SetObject()
		'        End If
		'    Else
		'        RemoveSession()
		'        Session.Remove("IsAllPartsSelected") 'Added By Vikrant On 03-Feb-2016 For ALL03022016
		'        Session.Remove("ModuleName")
		'        Response.Redirect("Index.aspx")
		'    End If
		'ElseIf Session("IsAttachmentNotSave") = True Then
		'    If mIssue.IsDirty And mIssue.StatusID = 2 Then
		'        Dim ExtraMessage As String = "As their is change in Attachment.Do you want to save Attchament?"
		'        MSGBoxCtrl.Show(MSGBox.Message_Title.Confirmation, MSGBox.Message_Text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "SaveAttachment")
		'        If IsValid Then
		'            SetObject()
		'        End If
		'    Else
		'        RemoveSession()
		'        Session.Remove("IsAllPartsSelected") 'Added By Vikrant On 03-Feb-2016 For ALL03022016
		'        Session.Remove("ModuleName")
		'        Response.Redirect("Index.aspx")
		'    End If
		'Else
		'    RemoveSession()
		'    Session.Remove("IsAllPartsSelected") 'Added By Vikrant On 03-Feb-2016 For ALL03022016
		'    Session.Remove("ModuleName")
		'    Response.Redirect("Index.aspx")
		'End If



		'    If mIssue.IsDirty Then
		'    If Session("IsAttachmentNotSave") = True AndAlso mIssue.StatusID = 2 Then
		'        Dim ExtraMessage As String = "As their is change in Attachment.Do you want to save Attchament?"
		'        MSGBoxCtrl.Show(MSGBox.Message_Title.Confirmation, MSGBox.Message_Text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "SaveAttachment")
		'    Else
		'        MSGBoxCtrl.Show(MSGBox.Message_Title.CloseConfirm, MSGBox.Message_Text.Save, "", MsgBoxStyle.YesNo, "Close")
		'    End If
		'    If IsValid Then
		'        SetObject()
		'    End If
		'    'Else
		'    '    RemoveSession()
		'    '    Session.Remove("ModuleName")
		'    '    Response.Redirect("Index.aspx")
		'    'End If
		'Else
		'        RemoveSession()
		'    Session.Remove("IsAllPartsSelected") 'Added By Vikrant On 03-Feb-2016 For ALL03022016
		'    Session.Remove("ModuleName")
		'    Response.Redirect("Index.aspx")
		'End If
	End Sub
	'Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
	'    'MarkLog(Util.Action.Close, ModuleName, "", Util.ErrorType.NoError, Guid.Empty)
	'    SetObject()
	'    IssueTo()
	'    mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + IIf(mIssueTo <> "", " to " + mIssueTo, "")
	'    MarkLog(Util.Action.Close, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)

	'    If Not mOpenFrom Is Nothing AndAlso mOpenFrom = "FromwfStockCard" Then  'Added By Prashant 3-Apr-2014 ALL03042014
	'        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
	'        Exit Sub
	'    ElseIf Not mOpenFrom Is Nothing AndAlso mOpenFrom = "FromReqItemStatusReport" Then 'Added By Vikrant on 13-Oct-2014 For Req Item Status Report
	'        RemoveSession()
	'        Session.Remove("ModuleName")
	'        Response.Redirect("Index.aspx")
	'    End If

	'    Session("IsValid") = IsValid
	'    If mIssue.IsDirty Then
	'        If Session("IsAttachmentNotSave") = True AndAlso mIssue.StatusID = 2 Then
	'            Dim ExtraMessage As String = "As their is change in Attachment.Do you want to save Attchament?"
	'            MSGBoxCtrl.Show(MSGBox.Message_Title.Confirmation, MSGBox.Message_Text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "SaveAttachment")
	'        Else
	'            MSGBoxCtrl.Show(MSGBox.Message_Title.CloseConfirm, MSGBox.Message_Text.Save, "", MsgBoxStyle.YesNo, "Close")
	'        End If
	'        If IsValid Then
	'            SetObject()
	'        End If
	'        'Else
	'        '    RemoveSession()
	'        '    Session.Remove("ModuleName")
	'        '    Response.Redirect("Index.aspx")
	'        'End If
	'    Else
	'            RemoveSession()
	'        Session.Remove("IsAllPartsSelected") 'Added By Vikrant On 03-Feb-2016 For ALL03022016
	'        Session.Remove("ModuleName")
	'        Response.Redirect("Index.aspx")
	'    End If
	'End Sub
	'DONE
	Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not IsInRole(Rights.Print) Then
            MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        SetReport() 'Added By Prashant 16-Sep-2013 ALL16092013
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    'DONE
    Private Sub btnReleaseNoteNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReleaseNoteNo.Click
        If Not IsInRole(Rights.Print) Then
            MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter

        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
            myReport = New crptReleaseNoteHeligo
        ElseIf AppSettings("ClientCode") = "IND" Then
            myReport = New crptIssueDetailPotraitIND
        Else
            myReport = New crptReleaseNote
        End If

        Dim objIssue As rptIssues
        Dim objChilds As rptIssueChields
        Dim letter As rptLetterHead
        Dim ds As New dsIssue
        objIssue = rptIssues.GetIssues(mIssue.ID)
        objChilds = rptIssueChields.GetIssuechilds(mIssue.ID)
        letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", AppSettings("Logo"))
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, objIssue)
        da.Fill(ds, objChilds)
        da.Fill(ds, letter)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    'DONE
    Private Sub btnWOReturnParts_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Response.Redirect("wfWOReturnedList.aspx")
    End Sub
    'DONE
    Private Sub cmbAircraftList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraftList.SelectedIndexChanged
        'mIssue.MachineID = mMachineList.Item(cmbAircraftList.SelectedIndex).ID
        mIssue.MachineID = mMachineNameValueList(cmbAircraftList.SelectedIndex).ID
        txtRegNo.DataBind()
        If cmbAircraftList.Enabled = True Then
            cmbAircraftList.Focus()
        End If
    End Sub
    'DONE
    Private Sub cmbStoreList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbStoreList.SelectedIndexChanged

        mUserHasNoStoreRights = UserHasNoStoreRights.GetUserHasNoStoreRights(User.Identity.Name, cmbStoreList.SelectedValue.ToString) 'Added By Prashant 31-Oct-2018 ALL30102018
        If mUserHasNoStoreRights.Count > 0 Then
            MSGBoxCtrl.Show("Alert!", "Sorry you do not have rights to select this store. Please contact with admin.", "", MsgBoxStyle.OkOnly, "ResetFromStore")
        End If

        If (cmbStoreList.SelectedValue = cmbLocationStore.SelectedValue) Then
            If Not ((cmbStoreList.SelectedIndex = 0) Or (cmbLocationStore.SelectedIndex = 0)) Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.SelectConformation, MSGBox.Message_Text.SelectConformation, "Do not select same store names", MsgBoxStyle.OkOnly, "ResetFromStore")
            End If
        End If

        'Added By Saylee on 21-July-2016 for store validation regarding NotInUse
        ''Check whether Issue Date is greater than NotInUse of Store 
        If mStoreList(New Guid(cmbStoreList.SelectedValue)).NotInUse = True Then
            If CDate(mStoreList(New Guid(cmbStoreList.SelectedValue)).NotInUseDate) <= CDate(mIssue.IDate) Then
                MSGBoxCtrl.Show("Alert!", "Store is not applicable since " + mStoreList(New Guid(cmbStoreList.SelectedValue)).NotInUseDateFormatted, "Select another Store from list or select date before " + mStoreList(New Guid(cmbStoreList.SelectedValue)).NotInUseDateFormatted + " & try again", MsgBoxStyle.OkOnly, "")
                'Exit Sub
            End If
        End If

        cmbStoreList.Focus()
    End Sub
    'DONE
    Private Sub cmbLocationStore_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLocationStore.SelectedIndexChanged
        If cmbStoreList.SelectedValue = cmbLocationStore.SelectedValue Then
            MSGBoxCtrl.Show(MSGBox.Message_Title.SelectConformation, MSGBox.Message_Text.SelectConformation, "Do not select same store names", MsgBoxStyle.OkOnly, "ResetToStore")
        End If

        'Added By Saylee on 21-July-2016 for store validation regarding NotInUse
        ''Check whether Issue Date is greater than NotInUse of ToStore 
        If mStoreList(New Guid(cmbLocationStore.SelectedValue)).NotInUse = True Then
            If CDate(mStoreList(New Guid(cmbLocationStore.SelectedValue)).NotInUseDate) <= CDate(mIssue.IDate) Then
                MSGBoxCtrl.Show("Alert!", "Destination Store is not applicable since " + mStoreList(New Guid(cmbLocationStore.SelectedValue)).NotInUseDateFormatted, "Select another Store from list or select date before " + mStoreList(New Guid(cmbLocationStore.SelectedValue)).NotInUseDateFormatted + " & try again", MsgBoxStyle.OkOnly, "")
                ' Exit Sub
            End If
        End If

        cmbLocationStore.Focus()

    End Sub
    'Added by Saylee on 2-June-2010
    'DONE
    Private Sub btnSentToBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSentToBill.Click
        'If (Not User.IsInRole("SentToBillView")) Then
        '    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
        '    Exit Sub
        'End If
        If ISInDate() = True Then
            If IsValid Then
                Session("IsValid") = IsValid
                mIssue.IsSync = 1
                Session("mIssue") = mIssue
                If Session("IsValid") Then
                    Session.Remove("IsValid")
                    '' DataFieldBind()
                    Save()
                Else
                    Session.Remove("IsValid")
                    'Response.Redirect("wfIssue_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                End If
            Else
                upnlValidationSummary.Update()
            End If
        Else
            Dim ToDate As String
            ToDate = (New SmartDate(DateAdd(DateInterval.Month, 1, DateAdd(DateInterval.Day, -(Day(mIssue.IDate)), mIssue.IDate)))).FormattedText
            MSGBoxCtrl.Show("Alert!", "This Transaction cannot be sent for billing. Accounts are closed upto " + ToDate, "", MsgBoxStyle.OkOnly, "Close")
        End If
    End Sub
    'Added By Utkarsh ON 18-Oct-2012 FOR ALL18102012
    'DONE
    'Commened by Sankalp 29-09-25
    'Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
    '    Dim fileSize1 As Integer = 0
    '    Dim file1(fileSize1) As Byte
    '    mIssue.ImageFile = file1
    '    mIssue.Size = 0
    '    mIssue.Extension = ""
    '    ImageButton1.Visible = False
    '    btnDelAttach.Enabled = False
    'End Sub
    'DONE
    'Commened by Sankalp 29-09-25
    'Private Sub ImageButton2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
    '    '----------------------------------------------------------------------
    '    Dim No As New Random
    '    Dim StrName As String = "abc" & No.Next.ToString
    '    '----------------------------------------------------------------------
    '    If mIssue.Size > 0 Then
    '        Dim path As String = AppSettings("DOCPath") & "\" & StrName & mIssue.Extension
    '        Dim fs As FileStream
    '        If File.Exists(AppSettings("DOCPath")) = False Then
    '            'Delete File if exist
    '            System.IO.File.Delete(AppSettings("DOCPath") & StrName & mIssue.Extension)
    '            ' Create the file.
    '            fs = File.Create(path)
    '            '' Add some information to the file.
    '            fs.Write(mIssue.ImageFile, 0, mIssue.ImageFile.Length)
    '            fs.Close()
    '            Session("DOCPath") = path
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    '        End If
    '    End If
    'End Sub
    'End
    'Added By Vikrant On 10-Nov-2014 For All10112014
    Protected Sub btnRequistionPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRequistionPrint.Click
        If Not IsInRole(Rights.Print) Then
            MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        SetRequistionReport(False)
    End Sub
    'End
    Private Sub btnIssueTag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIssueTag.Click
        If Not IsInRole(Rights.Print) Then
            MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        SetReport(IsForIssueTag:=True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        ' Session("UserEmailID") = SI.UTILITY.User.GetUser(Thread.CurrentPrincipal.Identity.Name).UserEmail
        SetUserMailIDs()
        '-------------
        If AppSettings("ClientCode") = "IND" And mIssue.ToTypeID = 18 And mIssue.TransTypeID = 14 Then 'Issue To Aircraft As Requisition
            Dim EmployeeMailID As String = Employee.GetEmployee(mIssue.ReqEmployeeID).Email
            Session("UserEmailID") = EmployeeMailID
        End If

        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            If mIssue.ToTypeID = 18 Then
                email = New Thread(Sub() SetRequistionReport(True))
            Else
                email = New Thread(Sub() SetReport(False, True))
            End If
            email.IsBackground = True
            email.Start()
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        Finally
            mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + " to " + Issue.GetIssue(mIssue.ID).IssueToName
            MarkLog(Util.Action.SendMail, ModuleName, mIssueDetail, Util.ErrorType.HandledError, mIssue.ID, EventLogID)
            MSGBoxCtrl.Show("Mail!", "Mail Sent Successfully", "", MsgBoxStyle.OkOnly, "")
        End Try
    End Sub
    Protected Sub txtReqEmployeeName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) ''Added By Prashant on 13-Jun-2022 IRM13062022
        'SetEmpID()
        Dim message As String = ""
        If IsNumeric(txtReqEmployeeName.Text) Then
            Dim mEmployeeListForCombo As EmployeeListForCombo
            mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo(BarcodeNo:=txtReqEmployeeName.Text)
            If mEmployeeListForCombo.Count > 0 Then
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeListForCombo(0).ID.ToString, mIssue.IDateFormatted.ToString)
                If mEmployeeStatus.Count > 0 Then
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert, MSGBox.Message_Text.Custom, message, MsgBoxStyle.OkOnly, "ResetIssuedToEmployee")
                        Exit Sub
                    End If
                    txtReqEmployeeName.Text = mEmployeeListForCombo(0).LicenceNoName
                    txtReqEmployeeName.DataBind()
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
                    MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert, MSGBox.Message_Text.Custom, message, MsgBoxStyle.OkOnly, "ResetIssuedToEmployee")
                    Exit Sub
                End If
                mIssue.ToolsIssuedToEmployeeID = New Guid(hdnIssuedToEmployeeId.Value)
                mIssue.ToolsIssuedToEmployeeName = txtReqEmployeeName.Text
            Else
                txtReqEmployeeName.Text = ""
                mIssue.ToolsIssuedToEmployeeID = Guid.Empty
                mIssue.ToolsIssuedToEmployeeName = ""
            End If
        Else
            If txtReqEmployeeName.Text <> "" Then
                mEmployeeList = EmployeeList.GetEmployeeList()
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeList(txtReqEmployeeName.Text, "").ID.ToString, mIssue.IDateFormatted.ToString)
                If mEmployeeStatus.Count > 0 Then
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert, MSGBox.Message_Text.Custom, message, MsgBoxStyle.OkOnly, "ResetIssuedToEmployee")
                        Exit Sub
                    End If
                    mIssue.ToolsIssuedToEmployeeID = mEmployeeList(txtReqEmployeeName.Text, "").ID
                    mIssue.ToolsIssuedToEmployeeName = txtReqEmployeeName.Text
                Else
                    txtReqEmployeeName.Text = ""
                    mIssue.ToolsIssuedToEmployeeID = Guid.Empty
                    mIssue.ToolsIssuedToEmployeeName = ""
                End If
            End If
        End If
        Session("mIssue") = mIssue
    End Sub

    'Sankalp 29-09-25
    Private Sub btnSaveAttachment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAttachment.Click
        mIsAttachmentNotSave = False
        Session("IsAttachmentNotSave") = mIsAttachmentNotSave
		mFileAttachments = mIssue.FileAttachments
		SetObject()
		mIssue.SaveUpdatedAttachment(IssueID:=mIssue.ID)
		Session("mIssue") = mIssue
		MarkLog(Action.Save, ModuleName, "Attachment", ErrorType.NoError, mIssue.ID, EventLogID)
        MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully, MSGBox.Message_Text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
    End Sub

#End Region

#Region " Show BrokenRules "
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
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
        If strMsg <> "" Then
            cvWorkShop.ErrorMessage = strMsg
            cvWorkShop.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region " Status "
    'Authorize
    Private Sub btnAuthorized_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
        SetObject()
        If Not mIssue.IsValid Then
            ValidationCode()
            upnlValidationSummary.Update()
            Exit Sub
        End If
        Dim txtValue As TextBox
        Dim mIssueItem As IssueItem
        Dim i As Integer = 0
        For Each mIssueItem In mIssue.IssueItems
            With mIssueItem
                Try
                    txtValue = CType(Me.dgIssueItems.Rows(i).FindControl("txtQty"), TextBox)
                    If txtValue.Text = "" Then
                        If Not CustomValidate2() Then Exit Sub
                    End If
                Catch ex As Exception

                End Try
            End With
            i = i + 1
        Next

        If IsValid Then
            If mIssue.TransTypeID = 19 And mIssue.IssueItems.IsSerializedExists Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.DiscardAuthorization, MSGBox.Message_Text.DiscardAuthorization, "Part No(s). : " & mIssue.IssueItems.SerializedPartNoList, MsgBoxStyle.YesNo, "IsSerializedExistsStatus")
                Session("IsValid") = IsValid
                mIssue.StatusID = 2
                Session("mIssue") = mIssue
                Session("IsForWOReturn") = False
            Else
                MSGBoxCtrl.Show(MSGBox.Message_Title.StatusAuthorized, MSGBox.Message_Text.StatusAuthorized, "<strong>Issue</strong>", MsgBoxStyle.YesNo, "Status")
                Session("IsValid") = IsValid
                mIssue.StatusID = 2
                Session("mIssue") = mIssue
                Session("IsForWOReturn") = False
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    'Cancel
    'Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    '    If IsValid Then
    '        Dim IsInUse As IsInUse = (IsInUse.GetIsInUseIssueINReceipt(mIssue.ID))
    '        If IsInUse.IsInUse Then
    '            MSGBoxCtrl.show(MSGBox.Message_title.Cancel, MSGBox.Message_text.Cancel, "<Strong>Issue,It is used in ReceiptCumInvoice</Strong>", MsgBoxStyle.OkOnly, "Status")
    '            mIssue.StatusID = 4
    '            Session("mIssue") = mIssue
    '            Exit Sub
    '        End If

    '        If mIssue.TransTypeID = 19 And mIssue.IssueItems.IsSerializedExists = True Then
    '            MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<strong>Issue</strong>", MsgBoxStyle.YesNo, "IsSerializedExistsStatus")
    '            Session("IsValid") = IsValid
    '            mIssue.StatusID = 4
    '            Session("mIssue") = mIssue
    '        Else
    '            MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<strong>Issue</strong>", MsgBoxStyle.YesNo, "Status")
    '            Session("IsValid") = IsValid
    '            mIssue.StatusID = 4
    '            Session("mIssue") = mIssue
    '        End If
    '    Else
    '        upnlValidationSummary.Update()
    '    End If
    'End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If IsValid Then

            For i As Integer = 0 To mIssue.IssueItems.Count - 1
                If mIssue.IssueItems(i).CountOf > 0 Then
                    MSGBoxCtrl.Show("Alert!", "Can not be Canceled<BR>As " + mIssue.IssueItems(i).ItemName + " Serial No. " + mIssue.IssueItems(i).SerialNo + " is already received after this transaction.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            Next

            Dim IsInUse As IsInUse = (IsInUse.GetIsInUseIssueINReceipt(mIssue.ID))
            If IsInUse.IsInUse Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.Cancel, MSGBox.Message_Text.Cancel, "<Strong>Issue,It is used in ReceiptCumInvoice</Strong>", MsgBoxStyle.OkOnly, "StatusCancel")
                Session("mIssue") = mIssue
                Exit Sub
            End If

            If mIssue.TransTypeID = 19 And mIssue.IssueItems.IsSerializedExists = True Then
                MSGBoxCtrl.Show(MSGBox.Message_Title.StatusCanceled, MSGBox.Message_Text.StatusCanceled, "<strong>Issue</strong>", MsgBoxStyle.YesNo, "IsSerializedExistsStatus")
                Session("IsValid") = IsValid
                mIssue.StatusID = 2
                Session("mIssue") = mIssue
            Else
                MSGBoxCtrl.Show(MSGBox.Message_Title.StatusCanceled, MSGBox.Message_Text.StatusCanceled, "<strong>Issue</strong>", MsgBoxStyle.YesNo, "StatusCancel")
                Session("IsValid") = IsValid
                Session("mIssue") = mIssue
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnLineMaintenanceReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLineMaintenanceReturn.Click
        If IsValid Then
            'Added By Vikrant On 01-Mar-2018 For BA15022018
            If mIssue.TransTypeID = Util.Trans.IssueToAircraft And mIssue.ToTypeID = 18 Then
                Dim mConsumableAndExpendableList As ConsumableAndExpendableList
                mConsumableAndExpendableList = ConsumableAndExpendableList.GetList(ReqID:=mIssue.RequisitionID.ToString)
                If mConsumableAndExpendableList.Count > 0 Then
                    MSGBoxCtrl.Show("Alert!", "This Transaction cannot be opened for Line Maintenance Return.Consumable & Expendable(C&E) entry already exists against this transaction", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
            'End
            MSGBoxCtrl.Show(MSGBox.Message_Title.LineMaintenanceReturn, MSGBox.Message_Text.LineMaintenanceReturn, "<strong>Issue for Line Maintenance Return</strong>", MsgBoxStyle.YesNo, "Status")
            mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + " to " + Issue.GetIssue(mIssue.ID).IssueToName
            MarkLog(Util.Action.Edit, ModuleName, mIssueDetail, Util.ErrorType.HandledError, mIssue.ID, EventLogID)
            Session("IsValid") = IsValid
            Session("ToMakeAuthorizeButtonInvisibel") = "ToMakeAuthorizeButtonInvisibel"
            mIssue.StatusID = 1
            Session("mIssue") = mIssue
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    ''Return Authorized For WO QTY
    Private Sub btnReturnAuthorized_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturnAuthorized.Click
        If IsValid Then
            MSGBoxCtrl.Show(MSGBox.Message_Title.WOIssueReturn, MSGBox.Message_Text.WOIssueReturn, "<strong>Issue for Work Order Quantity Return</strong>", MsgBoxStyle.YesNo, "Status")
            Session("ReturnWO") = "ReturnWO"
            Session("IsValid") = IsValid
            ''mIssue.StatusID = 1 'Commented by Saylee on 1-Oct-2019
            mIssue.StatusID = 2 ''Added by Saylee on 1-Oct-2019 :--  as Issue will be authorized if partially parts are returned,for remaining parts,issue will be authorized
            Session("mIssue") = mIssue
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
#End Region

#Region " Barcode "
    Private Sub btnAddBarcodeItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddBarcodeItem.Click
        If IsValid Then
            If txtBarcodeItem.Text <> "" Then '--
                If ((mIssue.TransTypeID = 14) Or (mIssue.TransTypeID = 20) Or (mIssue.TransTypeID = 15) Or (mIssue.TransTypeID = 17) Or (mIssue.TransTypeID = 24) Or (mIssue.TransTypeID = 63) Or (mIssue.TransTypeID = 19) Or (mIssue.TransTypeID = 25) Or (mIssue.TransTypeID = 26) Or (mIssue.TransTypeID = 44) Or (mIssue.TransTypeID = 45) Or (mIssue.TransTypeID = 52)) Then '2
                    Dim mPendingItemList As PendingToIssueList
                    Dim mItemListByBarcode As ItemListByBarcode
                    mItemListByBarcode = ItemListByBarcode.GetItemListByBarcode(txtBarcodeItem.Text)

                    If mItemListByBarcode.Count > 0 Then
                        mPendingItemList = PendingToIssueList.GetPendingToIssueListForBarcode(New Guid(cmbStoreList.SelectedValue),
                                                                                              mItemListByBarcode(0).ItemName, "", "", "",
                                                                                              cmbStoreList.SelectedItem.Text, txtIssueDate.Text,
                                                                                              mIssue.TransTypeID, mItemListByBarcode(0).ItemID.ToString,
                                                                                              , , txtBarcodeItem.Text,
                                                                                              mItemListByBarcode(0).ReceiptItemID.ToString,
                                                                                              ToTypeIDOfIssue:=mIssue.ToTypeID)
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Invalid Barcode Number.", False), True)
                        txtBarcodeItem.Text = ""
                        Exit Sub
                    End If


                    'mPendingItemList = PendingToIssueList.GetPendingToIssueListForBarcode(New Guid(cmbStoreList.SelectedValue), mItemListByBarcode(0).ItemName, "", "", "", cmbStoreList.SelectedItem.Text, txtIssueDate.Text, mIssue.TransTypeID, mItemListByBarcode(0).ItemID.ToString, , , txtBarcodeItem.Text, mItemListByBarcode(0).ReceiptItemID.ToString)
                    If mPendingItemList.Count > 0 Then '3
                        If mIssue.IssueItems.Contains(mPendingItemList(0).ReceiptItemID) Then '4
                            MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate, MSGBox.Message_Text.Duplicate, "Issue Item", MsgBoxStyle.OkOnly, "")
                            mIssue.StoreID = mPendingItemList.Item(0).StoreID
                            Exit Sub
                        Else '4
                            If mIssue.TransTypeID = 14 Then '5
                                If IsDBNull(mPendingItemList(0).Expirydate) = False Then '6
                                    If mPendingItemList(0).Expirydate <= Today.Date Then '7
                                        MSGBoxCtrl.Show(MSGBox.Message_Title.PartExpired, MSGBox.Message_Text.PartExpired, "<BR> <BR> Do you want to continue", MsgBoxStyle.YesNo, "Expired")
                                        Session("Index2") = 0
                                        mIssue.StoreID = mPendingItemList.Item(0).StoreID
                                        Session("mPendingItemList") = mPendingItemList
                                        Session("ItemName") = mPendingItemList(0).ItemName
                                        Exit Sub
                                    ElseIf mPendingItemList(0).ExpiryQtrs <> "" Then  '7
                                        If mPendingItemList(0).ExpiryQtrDate <= Today.Date Then 'I
                                            MSGBoxCtrl.Show(MSGBox.Message_Title.PartExpired, MSGBox.Message_Text.PartExpired, "<BR> <BR> Do you want to continue", MsgBoxStyle.YesNo, "Expired")
                                            Session("Index2") = 0
                                            mIssue.StoreID = mPendingItemList.Item(0).StoreID
                                            Session("mPendingItemList") = mPendingItemList
                                            Session("ItemName") = mPendingItemList(0).ItemName
                                            Exit Sub
                                        Else 'I
                                            AddItemByBarcode(mPendingItemList)
                                        End If 'I
                                    Else '7
                                        AddItemByBarcode(mPendingItemList)
                                    End If '7
                                Else '6
                                    AddItemByBarcode(mPendingItemList)
                                End If '6
                            Else '5
                                AddItemByBarcode(mPendingItemList)
                            End If '5
                        End If '4
                    Else '3
                        MSGBoxCtrl.Show("Add alert !", "Item can not be added <br> Item not present in Stock or Wrong Store selected", "", MsgBoxStyle.OkOnly, "")
                    End If
                End If
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Invalid Barcode Number.", False), True)
                Exit Sub
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Public Sub AddItemByBarcode(ByVal mPendingItemList As PendingToIssueList)
        mIssue.IssueItems.Add(mIssue.ID, mIssue.TransTypeID)
        mIssue.IssueItems.CurrentItem.ReceiptItemID = mPendingItemList(0).ReceiptItemID
        If mPendingItemList(0).IsSerialized Then '9
            mIssue.IssueItems.CurrentItem.DisplayQty = 1
        Else '9
            'Commented and added by Prashant 20-Apr-2016
            'mIssue.IssueItems.CurrentItem.DisplayQty = 0
            mIssue.IssueItems.CurrentItem.DisplayQty = mPendingItemList(0).AvailableQuantity
        End If '9
        mIssue.IssueItems.CurrentItem.DisplayUnitID = mPendingItemList(0).UnitID
        mIssue.IssueItems.CurrentItem.DisplayUnitName = mPendingItemList(0).UnitName
        mIssue.IssueItems.CurrentItem.CodeNo = mPendingItemList(0).CodeNo
        If mPendingItemList(0).CalibrationDueDateFormatted.ToString <> "" Then
            mIssue.IssueItems.CurrentItem.CalibrationDueDate = mPendingItemList(0).CalibrationDueDateFormatted.ToString
        Else
            mIssue.IssueItems.CurrentItem.CalibrationDueDate = System.DBNull.Value
        End If
        Session("mIssue") = mIssue
        SetcmbRequisitionItemTypeList()
        mRequisitionItemTypeList = RequisitionItemTypeList.GetRequisitionItemTypeList()  'Added By Prashant 13-Apr-2015 ALL13042015
        dgIssueItems.DataSource = mIssue.IssueItems
        dgIssueItems.DataBind()

        txtBarcodeItem.Text = ""
        txtBarcodeItem.Focus()
    End Sub
    Public Sub AddExpiredItemByBarcode()
        Page.Validate()
        If IsValid Then '1
            Dim mPendingItemList As PendingToIssueList
            mPendingItemList = Session("mPendingItemList")
            If mPendingItemList.Count > 0 Then '2
                If mIssue.IssueItems.Contains(mPendingItemList(0).ReceiptItemID) Then '3
                    MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate, MSGBox.Message_Text.Duplicate, "Issue Item", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else '3
                    AddItemByBarcode(mPendingItemList)
                End If '3
            Else '2
                MSGBoxCtrl.Show("Add alert !", "Item can not be added <br> Item not present in Stock or Wrong Store selected", "", MsgBoxStyle.OkOnly, "")
            End If '2
            Session.Remove("mPendingItemList")
        End If '1
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'Comment Sankalp 29-09-25
    'Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
    '    AttachMyFile()
    'End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetEmployeeList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim itemlist As EmpNoNameAutoComplete
        itemlist = EmpNoNameAutoComplete.GeEmpNoNameList(prefixText)
        If count = 0 Then
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemlist
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).ToArray
        Else
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemlist
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region

#Region "MultipleAttachment"
    'Sankalp 26-09-25
    Private Sub btnSelectFiles_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnSelectFiles.Click
        SetObject()
        Session("mIssue") = mIssue
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
    End Sub
    Private Sub dgItemAttachment_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgItemAttachment.RowCommand
        'Dim mFileAttachments As FileAttachments
        Select Case e.CommandName
            Case "View"
                Dim Index As Integer = CInt(e.CommandArgument)
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttachments = mIssue.FileAttachments
                If mFileAttachments.Count = 1 Then
                    mFileAttachments.CurrentIndex = 0
                Else
                    mFileAttachments.CurrentIndex = Index - 1
                End If

                If mFileAttachments.CurrentItem.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttachments.CurrentItem.ImageFile, 0, mFileAttachments.CurrentItem.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
                    End If
                End If
                dgItemAttachment.DataSource = mIssue.FileAttachments
                dgItemAttachment.DataBind()
                upnlItemAttachment.Update()
                upnldgItemAttachment.Update()
            Case "Remove"
                Dim Index As Integer = CInt(e.CommandArgument) + dgItemAttachment.PageSize * dgItemAttachment.PageIndex
                mFileAttachments = mIssue.FileAttachments
                If mFileAttachments.Count = 1 Then
                    DeleteAttachment(0)
                    'mIssue.IsAttachmentAdded = False
                    Session("IsAttachmentDeleted") = IsAttachmentDeleted
                Else
                    DeleteAttachment(Index - 1)
                End If
        End Select
    End Sub
    Private Sub DeleteAttachment(ByVal Index As Int32)
        Session("IsAttachmentNotSave") = mIsAttachmentNotSave
        MSGBoxCtrl.Show(MSGBox.Message_Title.RemoveItem, MSGBox.Message_Text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
        mIssue.FileAttachments.CurrentIndex = Index
        Session("mIssue") = mIssue
    End Sub

    Private Sub hdnBtnFileUpload_Click(sender As Object, e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
        Session("IsAttachmentNotSave") = mIsAttachmentNotSave
        upnlItemAttachment.Update()
    End Sub
    Private Sub AttachMyFile()
        Try
            If Not mIssue.FileAttachments.Contains(mIssue.ID, CType(Session("FileUpload.FileName"), String)) Then

                mIssue.FileAttachments.Add(mIssue.ID, CType(Session("FileUpload.FileName"), String))
                mIssue.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
                mIssue.FileAttachments.CurrentItem.Size = Session("Size")
                mIssue.FileAttachments.CurrentItem.Extension = Session("Extension")
                Session("mIssue") = mIssue
                dgItemAttachment.DataSource = mIssue.FileAttachments
                dgItemAttachment.DataBind()

                For i As Integer = 0 To mIssue.FileAttachments.Count - 1
                    Dim txtValue As TextBox
                    txtValue = CType(Me.dgItemAttachment.Rows(i).FindControl("txtFileName"), TextBox)
                    txtValue.Text = mIssue.FileAttachments(i).FileName
                Next

                Session.Remove("Size")
                Session.Remove("ImageFile")
                Session.Remove("Extension")
                Session.Remove("FileUpload.FileName")
                upnlItemAttachment.Update()
                upnldgItemAttachment.Update()
            Else
                Session("mIssue") = mIssue
                'Session("mReceiptCumInvoice") = mReceiptCumInvoice
                MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate, MSGBox.Message_Text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Catch ex As Exception
        End Try
    End Sub

#End Region
End Class