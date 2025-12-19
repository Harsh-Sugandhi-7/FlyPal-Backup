
'Created By Utkarsh On 07-May-2012 FOR ALLIssue04052012

Partial Class wfIssueForUnusedReturn
    Inherits Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.

    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

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
    Dim mVendorList As VendorList
    'Dim mMachineList As FlyPal22.Maintain.SelectList 'Dim mMachineList As tmpMachineList
    Dim mMachineNameValueList As MachineNameValueList
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
    Dim MarkLogDetailas As ArrayList = New ArrayList 'Added By Utkarsh On 07-May-2012 FOR ALLIssue04052012
    Public Flag As Integer
    'Added By Vikrant on 30-Oct-2013 For All29102013
    Dim ReturnDate As DateTime
    Dim Remark As String

    Dim DateFormat As String = AppSettings("DateFormat").ToString()
    'End 
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
        Session.Remove("tmpIssue") 'Added By Vikrant on 06-Nov-2013 For All29102013
    End Sub
    Private Sub SetPage()
        If mIssue.No > 0 Then
            lblTitle.Text = Session("ModuleName") + " [" + mIssue.Text + "-" + CType(mIssue.No, String) + "]"
        Else
            lblTitle.Text = Session("ModuleName") + " [ New ]"
        End If
    End Sub
    Private Sub Enable()
        IssueDate.Enabled = True
        cmbStoreList.Enabled = True
        cmbLocationStore.Enabled = True
        cmbVendorList.Enabled = True
        cmbAircraftList.Enabled = True
        cmbWO.Enabled = True
        IssueDate.Enabled = True
        cmbWorkShop.Enabled = True
        cmbWorkOrder.Enabled = True
    End Sub
    Private Sub Disable()
        IssueDate.Enabled = False
        cmbStoreList.Enabled = False
        cmbLocationStore.Enabled = False
        cmbVendorList.Enabled = False
        cmbAircraftList.Enabled = False
        cmbWO.Enabled = False
        IssueDate.Enabled = False
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
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " Your subscription has been expired. can not save Issue. <br> Issue Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Issue. <br> Issue Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    DataFieldBind()
                    Exit Sub
                End If

                'Added By Vikrant On 28-July-2014 For BA24072014
                If AppSettings("LockBackDatedTransaction") = "True" Then
                    If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then
                        'Do nothing
                    Else
                        If CheckDateForTransactionLock(mIssue.IDate) Then
                            MSGBoxCtrl.Show("Save Alert!", "Previous Months transactions can only be saved until " & DateSerial(Year(CDate(mIssue.IDate).AddMonths(1)), Month(CDate(mIssue.IDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "Kindly book this transaction in current month to reflect in Valuation.", MsgBoxStyle.OkOnly, "")
                            Exit Sub
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
                        'cvControlValidator.ErrorMessage = strMSG
                        'cvControlValidator.IsValid = False
                    End If
                End If
                'MarkLog(Util.Action.Save, ModuleName, mIssue.IssueNo, Util.ErrorType.NoError, mIssue.ID)
                IssueTo()
                mIssueDetail = "Unused Return " + " Issue No. : " + mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + " to " + mIssueTo

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
                Response.Redirect("wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage"), False)
            Else
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, "Issue can not be saved without Item.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Issue can not be saved without Item.", MsgBoxStyle.OkOnly, "")
                mIssue = IssueClone
                SetObject()
                Session("mIssue") = mIssue
                DataFieldBind()
            End If
        Catch ex As SqlException
            Session("IssueClone") = IssueClone
            If ex.Number = 8114 Or ex.Number = 8115 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 8145 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                If InStr(ex.Message, "CCtabRequisitionItemIssueBalQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabReceiptItemLoanQty", CompareMethod.Text) Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PendingQty, SIMsgBox.Message_text.PendingQty, "Issue Qty can not be greater than Receipt Qty.", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                    'msg1.Show()
                    'Session("sender") = "Status"
                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, "Issue Qty can not be greater than Receipt Qty.", MsgBoxStyle.OkOnly, "Status")
                    mIssue = IssueClone
                    SetObject()
                    Session("mIssue") = mIssue
                    DataFieldBind()
                ElseIf InStr(ex.Message, "CCtabnWOSpareFromWOJobSparePendingIssuedQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabnWOToolsPendingIssuedQty", CompareMethod.Text) Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PendingQty, SIMsgBox.Message_text.PendingQty, "Issue Qty can not be greater than Required Qty.", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                    'msg1.Show()
                    'Session("sender") = "Status"
                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, "Issue Qty can not be greater than Required Qty.", MsgBoxStyle.OkOnly, "Status")
                    mIssue = IssueClone
                    SetObject()
                    Session("mIssue") = mIssue
                    DataFieldBind()
                ElseIf InStr(ex.Message, "FKtabIssueTermtabTerm", CompareMethod.Text) Then
                    'Dim msg1 As New SIMsgBox(Page, "Term Deleted! ", "Term Not Avalable<Br><BR>Selected Term is no longer exist in the Database <BR><BR> Remove Term and try Again", " ", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                    'msg1.Show()
                    MSGBoxCtrl.Show("Term Deleted! ", "Term Not Avalable<Br><BR>Selected Term is no longer exist in the Database <BR><BR> Remove Term and try Again", " ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8144 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure + "," + ex.Message, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure + "," + ex.Message, MsgBoxStyle.OkOnly, "")
                End If
            End If
        Catch ex1 As Exception

            If InStr(ex1.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PendingQty, SIMsgBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Stock Qty.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                'Session("sender") = "Status"
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Stock Qty.", MsgBoxStyle.OkOnly, "Status")
                mIssue = IssueClone
                SetObject()
                Session("mIssue") = mIssue
                DataFieldBind()
            ElseIf InStr(ex1.Message, "CCtabnWOSpareFromWOJobSparePendingIssuedQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabnWOToolsPendingIssuedQty", CompareMethod.Text) Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PendingQty, SIMsgBox.Message_text.PendingQty, "Issue Qty can not be greater than Required Qty.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                'Session("sender") = "Status"
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, "Issue Qty can not be greater than Required Qty.", MsgBoxStyle.OkOnly, "Status")
                mIssue = IssueClone
                SetObject()
                Session("mIssue") = mIssue
                DataFieldBind()
            ElseIf InStr(ex1.Message, "CCtabReceiptItemLoanQty", CompareMethod.Text) Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PendingQty, SIMsgBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Receipt Qty.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Receipt Qty.", MsgBoxStyle.OkOnly, "")
            ElseIf InStr(ex1.Message, "CCtabSalesOrderItemIssueBalQty", CompareMethod.Text) Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PendingQty, SIMsgBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Sales Order Qty.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Sales Order Qty.", MsgBoxStyle.OkOnly, "")
            ElseIf InStr(ex1.Message, "CCtabOrderItemEROQty", CompareMethod.Text) Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PendingQty, SIMsgBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Exchange/Repair/Overhaul Qty.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Exchange/Repair/Overhaul Qty.", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            IssueClone = Nothing
        End Try
    End Sub
    'Added By Vikrant on 1-Nov-2013 For All29102013
    Private Sub ClearModalPopControls()
        calReturnDate.Text = ""
        txtRemarkForUnsedParts.Text = ""
        calReturnDate.DataBind()
        txtRemarkForUnsedParts.DataBind()
        upnlUnusedReturnedParts.Update()
    End Sub
    'End
    'Added By Vikrant On 24-July-2014 For BA24072014
    Private Function CheckDateForTransactionLock(ByVal TransDate As Date) As Boolean
        Dim FirstDayofLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
        Dim FirstDayofMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)
        If (TransDate >= FirstDayofLastMonth) Then
            If (TransDate < FirstDayofMonth) And (Day(Today.Date) > 10) Then
                Return True
            Else
                Return False
            End If
        Else
            Return True
        End If
    End Function
    'End
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
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " Your subscription has been expired. can not save Issue. <br> Issue Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Issue. <br> Issue Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    DataFieldBind()
                    Exit Sub
                End If

                'Added By Vikrant On 28-July-2014 For BA24072014
                If AppSettings("LockBackDatedTransaction") = "True" Then
                    If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then
                        'Do nothing
                    Else
                        If CheckDateForTransactionLock(mIssue.IDate) Then
                            MSGBoxCtrl.Show("Save Alert!", "Previous Months transactions can only be saved until " & DateSerial(Year(CDate(mIssue.IDate).AddMonths(1)), Month(CDate(mIssue.IDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "Kindly book this transaction in current month to reflect in Valuation.", MsgBoxStyle.OkOnly, "")
                            Exit Sub
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
                        Session("IsValid") = mIssue.IsValid
                        'Dim msg1 As New SIMsgBox(Page, "Discard confirmation", "Your are about to Discard Serialized/Rotable Item : " + mIssue.IssueItems.SerializedPartNo + "  Do you want to continue?", "", MsgBoxStyle.YesNo)
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Discard, SIMsgBox.Message_text.Discard, "Part No. " & mIssue.IssueItems.SerializedPartNo, MsgBoxStyle.YesNo)
                        'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                        'Session("sender") = "IsSerializedExists"
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.Discard, MSGBox.Message_text.Discard, "Part No. " & mIssue.IssueItems.SerializedPartNo, MsgBoxStyle.YesNo, "IsSerializedExists")
                        Exit Sub
                    End If

                    '------------------------------------------------------------------------------------------------------------

                    'Added By Utkarsh On 07-May-2012 FOR ALLIssue04052012
                    If Not Session("IsFromMessageBox") = "True" Then

                        Dim txtReturnValue As TextBox

                        For i As Integer = 0 To GVIssueItems.Rows.Count - 1
                            txtReturnValue = CType(GVIssueItems.Rows(i).FindControl("txtReturnQty"), TextBox)
                            Dim ReturnQty As Decimal = Val(txtReturnValue.Text.Trim)
                            mIssue.IssueItems(i).nWOPendingQty = ReturnQty
                        Next

                        Session("mIssue") = mIssue
                        Dim tmpIssueClone As Issue
                        tmpIssueClone = mIssue.Clone
                        Session("tmpIssue") = tmpIssueClone 'Added By Vikrant on 30-Oct-2013 For All29102013

                        'Dim msg1 As New SIMsgBox(Page, "Save Alert ! ", "<b>If Return Qty is same as Issued Qty than that item will be deleted,and if all items Return Qty and Issued Qty are same than this issue will be deleted.</b><BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo)
                        'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                        'msg1.Show()
                        'Session("sender") = "Status"
                        MSGBoxCtrl.Show("Save Alert ! ", "<b>If Return Qty is same as Issued Qty then that item will be deleted,and if all line item's Return Qty and Issued Qty are same than that issue will be deleted.</b><BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo, "Status")
                        Session("UnusedReturn") = "UnusedReturn"
                        Session("IsValid") = IsValid
                        Exit Sub
                    Else
                        Session.Remove("IsFromMessageBox")
                        Session.Remove("UnusedReturn")

                        If UnusedIssuedItems() = False Then Exit Sub

                        Session.Remove("UnusedReturn")

                        If mIssue.IssueItems.Count = 0 Then
                            mIssue.Delete()
                        End If
                        MarkLogDetailas = Session("MarkLogDetailas")
                        Session.Remove("IsFromMessageBox")
                    End If
                    'End
                    Dim Script As String
                    Try
                        'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries
                        'Check if IssueText is blank then call TransTextSeries UI

                        If (mIssue.IsNew) And (mIssue.Text = "") Then

                            Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mIssue.TransTypeID, mIssue.IDateFormatted)

                            If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mIssue.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mIssue.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mIssue.TransTypeID).TransText = "")) Then

                                Dim str = "<script language='javascript'>openledgersame('wfIssueForUnusedReturn.aspx?BackPage=Index.aspx');</script>"

                                Session("BackPagestr_ForTransSeries") = str

                                Session("TransName_ForTransSeries") = "Issue"
                                Session("TransTypeID_ForTransSeries") = mIssue.TransTypeID
                                Session("TransDate_ForTransSeries") = mIssue.IDateFormatted

                                Dim msg1 As New SIMsgBox(Page, "Sales Order Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly)
                                msg1.ReplacePage = "wfSalesOrder.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                                Session("sender") = "IssueTransTextSeriesAlert"
                                Exit Sub

                            Else
                                Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                                If mAutoRenewTransTextSeries.IsRenewed Then
                                    With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mIssue.TransTypeID)
                                        mIssue.Text = .TransText
                                        mIssue.No = .StartingTransNo
                                    End With
                                Else
                                    Dim str = "<script language='javascript'>openledgersame('wfIssueForUnusedReturn.aspx?BackPage=Index.aspx');</script>"

                                    Session("BackPagestr_ForTransSeries") = str

                                    Session("TransName_ForTransSeries") = "Issue"
                                    Session("TransTypeID_ForTransSeries") = mIssue.TransTypeID
                                    Session("TransDate_ForTransSeries") = mIssue.IDateFormatted
                                    Dim msg1 As New SIMsgBox(Page, "Unused Return Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly)
                                    msg1.ReplacePage = "wfSalesOrder.aspx?BackPage=" & Request.QueryString("BackPage")
                                    msg1.Show()
                                    Session("sender") = "IssueTransTextSeriesAlert"
                                    Exit Sub

                                End If
                            End If

                        End If
                        'End
                        mIssue.Save()
                    Catch ex As SqlException
                        If ex.Number = 547 Then
                            Dim StrError As String = "Core Unit is already returned from Aircraft against this Part."
                            Script = "alert('" & StrError & "');"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Script, True)
                            Exit Sub
                        Else
                            Script = "alert('" & "Error Occured" & "');"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Script", Script, True)
                            Exit Sub
                        End If
                    Catch ex As Exception
                        Script = "alert('" & "Error Occured" & "');"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Script", Script, True)
                        Exit Sub
                    End Try 'End
                    UpdateUnusedReturnPart() 'Added By Vikrant on 30-Oct-2013 For All29102013
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
                        'cvControlValidator.ErrorMessage = strMSG
                        'cvControlValidator.IsValid = False
                    End If
                End If
                'MarkLog(Util.Action.Save, ModuleName, mIssue.IssueNo, Util.ErrorType.NoError, mIssue.ID)
                IssueTo()
                mIssueDetail = "Unused Return " + " Issue No. : " + mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + " to " + mIssueTo

                If mIssue.StatusID = 2 Then
                    MarkLog(Util.Action.Authorize, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                ElseIf mIssue.StatusID = 3 Then
                    MarkLog(Util.Action.Amend, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                ElseIf mIssue.StatusID = 4 Then
                    MarkLog(Util.Action.Cancel, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                Else
                    MarkLog(Util.Action.Save, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                End If

                ''Added By Utkarsh On 07-May-2012 FOR ALLIssue04052012

                'Log all the items which are removed from the list as issued and returned qty are same....
                'Log the issue which is deleted which has only one item with issued and returned qty are same....
                If Not MarkLogDetailas Is Nothing Then
                    For i As Integer = 0 To MarkLogDetailas.Count - 1
                        If MarkLogDetailas(i) = "0" Then
                            MarkLog(Util.Action.Delete, ModuleName, MarkLogDetailas(i + 1).ToString, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                        Else
                            MarkLog(Util.Action.Edit, ModuleName, MarkLogDetailas(i + 1).ToString, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                        End If

                        i = i + 1
                    Next i
                End If
                'End 
                mIssue.MarkClean()
                Session("mIssue") = mIssue
                Response.Redirect("wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage"), False)   'Commented For Barcode Implementation by Vikrant
            Else
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, "Issue can not be saved without Item.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Issue can not be saved without Item.", MsgBoxStyle.OkOnly, "")
                mIssue = IssueClone
                SetObject()
                Session("mIssue") = mIssue
                DataFieldBind()
            End If
        Catch ex As SqlException
            Session("IssueClone") = IssueClone
            If ex.Number = 8114 Or ex.Number = 8115 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 8145 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                If InStr(ex.Message, "CCtabRequisitionItemIssueBalQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabReceiptItemLoanQty", CompareMethod.Text) Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PendingQty, SIMsgBox.Message_text.PendingQty, "Issue Qty can not be greater than Receipt Qty.", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                    'msg1.Show()
                    'Session("sender") = "Status"
                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, "Issue Qty can not be greater than Receipt Qty.", MsgBoxStyle.OkOnly, "Status")
                    mIssue = IssueClone
                    SetObject()
                    Session("mIssue") = mIssue
                    DataFieldBind()
                ElseIf InStr(ex.Message, "CCtabnWOSpareFromWOJobSparePendingIssuedQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabnWOToolsPendingIssuedQty", CompareMethod.Text) Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PendingQty, SIMsgBox.Message_text.PendingQty, "Issue Qty can not be greater than Required Qty.", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                    'msg1.Show()
                    'Session("sender") = "Status"
                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, "Issue Qty can not be greater than Required Qty.", MsgBoxStyle.OkOnly, "Status")
                    mIssue = IssueClone
                    SetObject()
                    Session("mIssue") = mIssue
                    DataFieldBind()
                ElseIf InStr(ex.Message, "FKtabIssueTermtabTerm", CompareMethod.Text) Then
                    'Dim msg1 As New SIMsgBox(Page, "Term Deleted! ", "Term Not Avalable<Br><BR>Selected Term is no longer exist in the Database <BR><BR> Remove Term and try Again", " ", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                    'msg1.Show()
                    MSGBoxCtrl.Show("Term Deleted! ", "Term Not Avalable<Br><BR>Selected Term is no longer exist in the Database <BR><BR> Remove Term and try Again", " ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8144 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure + "," + ex.Message, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure + "," + ex.Message, MsgBoxStyle.OkOnly, "")
                End If
            End If
        Catch ex1 As Exception

            If InStr(ex1.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PendingQty, SIMsgBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Stock Qty.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                'Session("sender") = "Status"
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Stock Qty.", MsgBoxStyle.OkOnly, "Status")
                mIssue = IssueClone
                SetObject()
                Session("mIssue") = mIssue
                DataFieldBind()
            ElseIf InStr(ex1.Message, "CCtabnWOSpareFromWOJobSparePendingIssuedQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabnWOToolsPendingIssuedQty", CompareMethod.Text) Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PendingQty, SIMsgBox.Message_text.PendingQty, "Issue Qty can not be greater than Required Qty.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                'Session("sender") = "Status"
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, "Issue Qty can not be greater than Required Qty.", MsgBoxStyle.OkOnly, "Status")
                mIssue = IssueClone
                SetObject()
                Session("mIssue") = mIssue
                DataFieldBind()
            ElseIf InStr(ex1.Message, "CCtabReceiptItemLoanQty", CompareMethod.Text) Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PendingQty, SIMsgBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Receipt Qty.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Receipt Qty.", MsgBoxStyle.OkOnly, "")
            ElseIf InStr(ex1.Message, "CCtabSalesOrderItemIssueBalQty", CompareMethod.Text) Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PendingQty, SIMsgBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Sales Order Qty.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Sales Order Qty.", MsgBoxStyle.OkOnly, "")
            ElseIf InStr(ex1.Message, "CCtabOrderItemEROQty", CompareMethod.Text) Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PendingQty, SIMsgBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Exchange/Repair/Overhaul Qty.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Exchange/Repair/Overhaul Qty.", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            IssueClone = Nothing
        End Try
    End Sub
    Private Function UnusedIssuedItems() As Boolean
        For Each item As IssueItem In mIssue.IssueItems
            Dim ReturnQty As Decimal = item.nWOPendingQty
            Dim DisplayQty As Decimal = item.DisplayQty
            If ReturnQty > item.DisplayQty Then
                Dim StrError As String = "Sr.No. " & item.SRNo & " Part No. " & item.ItemName & " Return Qty should not be greater than Issued Qty."
                Dim Script As String = "alert('" & StrError & "');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Script", Script, True)
                'Dim msg1 As New SIMsgBox(Page, "Return Qty Alert ! ", StrError, "", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                Return False
            ElseIf (DisplayQty - ReturnQty) < item.TotalConsumableAndExpendableUsedQty Then
                Dim StrError As String = "Sr.No. " & item.SRNo & " Part No. " & item.ItemName & " Return Qty should not be greater than Consumable And Expendable Used(Used+Scrap) Qty."
                Dim Script As String = "alert('" & StrError & "');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Script", Script, True)
                'Dim msg1 As New SIMsgBox(Page, "Return Qty Alert ! ", StrError, "", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                Return False
            ElseIf ReturnQty > 0 Then
                'Added By Vikrant on 06-Nov-2013 For All29102013
                If item.IsReturnableFromAircraft = True And item.LoanQty < item.Qty Then
                    Dim StrError As String = "Core Unit is already returned from Aircraft against this Part."
                    Dim Script As String = "alert('" & StrError & "');"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Script, True)
                    Return False
                End If
                'End
                Dim OriginalQty As String
                OriginalQty = item.DisplayQty
                item.DisplayQty = item.DisplayQty - ReturnQty
                item.nWOPendingQty = 0
                If item.DisplayQty = 0 Then
                    MarkLogDetailas.Add("0") '0 Status for Delete
                Else
                    MarkLogDetailas.Add("1") '0 Status for Edit
                End If
                mIssueDetail = "Unused Return " + " Issue No. : " + mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + " Part No. : " & item.ItemName & " Desc. : " & item.ItemDesc & " Original Qty : " & OriginalQty & " Return Qty : " & ReturnQty & " User Name : " & User.Identity.Name
                MarkLogDetailas.Add(mIssueDetail)
            End If
        Next
        IssueTo()

        mIssue.RemoveQtyZeroItems()

        If mIssue.IssueItems.Count = 0 Then
            MarkLogDetailas.Add("0") '0 Status for Delete
            'mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted & " TO : " & mIssueTo
            MarkLogDetailas.Add(mIssueDetail)
        End If
        GVIssueItems.DataSource = mIssue.IssueItems
        GVIssueItems.DataBind()
        Session("mIssue") = mIssue
        Session("MarkLogDetailas") = MarkLogDetailas
        Return True
    End Function
    Private Sub SetObject()
        mIssue.IDate = IssueDate.Text
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

        Select Case mIssue.ToTypeID
            Case 1, 7  '7 Added By Prashant 5-July-2011 for Discard
                mIssue.VendorID = New Guid(cmbVendorList.SelectedValue)
                mIssue.MachineID = Guid.Empty
                mIssue.ToStoreID = Guid.Empty
                mIssue.WorkShopID = Guid.Empty
                mIssue.nWOID = Guid.Empty
            Case 2
                mIssue.VendorID = Guid.Empty
                mIssue.MachineID = New Guid(cmbAircraftList.SelectedValue)
                mIssue.ToStoreID = Guid.Empty
                mIssue.WorkShopID = Guid.Empty
            Case 8
                mIssue.VendorID = Guid.Empty
                mIssue.MachineID = Guid.Empty
                mIssue.ToStoreID = New Guid(cmbLocationStore.SelectedValue)
                mIssue.WorkShopID = Guid.Empty
                mIssue.nWOID = Guid.Empty
            Case 16
                mIssue.VendorID = Guid.Empty
                mIssue.MachineID = Guid.Empty
                mIssue.ToStoreID = Guid.Empty
                mIssue.WorkShopID = New Guid(cmbWorkShop.SelectedValue)
                mIssue.nWOID = Guid.Empty
            Case 17
                mIssue.VendorID = Guid.Empty
                mIssue.MachineID = Guid.Empty
                mIssue.ToStoreID = Guid.Empty
                mIssue.WorkShopID = Guid.Empty
                mIssue.nWOID = New Guid(cmbWorkOrder.SelectedValue)
        End Select
        'Commented By Vikrant On 05-Mar-2019 For ALL05032019
        'mIssue.UserName = User.Identity.Name
        mIssue.UserName = mIssue.AuthorizedBy
        'End
        mIssue.CalculateTotal()            'Added By Saylee on 7-July-2011

        'Added by vikrant on 25-AUG-2011
        Dim txtValue As TextBox
        Dim mIssueItem As IssueItem
        Dim i As Integer = 0
        For Each mIssueItem In mIssue.IssueItems
            With mIssueItem
                Try
                    txtValue = CType(Me.GVIssueItems.Rows(i).FindControl("txtQty"), TextBox)
                    .DisplayQty = CDec(Val(txtValue.Text))
                Catch ex As Exception

                End Try
            End With
            i = i + 1
        Next
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.RemoveItem, SIMsgBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo)
        msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
        Session("sender") = "Remove"
        msg1.Show()
        mIssue.IssueItems.CurrentIndex = Index
        Session("mIssue") = mIssue
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
    Private Sub ReturnWOQty()
        Dim mtmpIssueItem As IssueItem
        For Each mtmpIssueItem In mIssue.IssueItems
            mtmpIssueItem.DisplayQty = mtmpIssueItem.DisplayQty - mtmpIssueItem.WOReturnQty
            mtmpIssueItem.WOReturnQty = 0

        Next
        mIssue.RemoveQtyZeroItems()
        GVIssueItems.DataSource = mIssue.IssueItems
        GVIssueItems.DataBind()
        Session("mIssue") = mIssue
    End Sub
    'Added By Vikrant on 30-Oct-2013 For All29102013
    Private Sub UpdateUnusedReturnPart()
        Dim tmpIssue As Issue = Session("tmpIssue")
        Dim mtmpIssueItemForUpdate As IssueItem
        For Each mtmpIssueItemForUpdate In tmpIssue.IssueItems
            If mtmpIssueItemForUpdate.nWOPendingQty > 0 Then
                mIssue.UpdateUnusedReturnParts(calReturnDate.Text, tmpIssue.IDateFormatted, tmpIssue.IssueNo, tmpIssue.IssueToName,
                                               mtmpIssueItemForUpdate.StoreName, "", mtmpIssueItemForUpdate.ItemName, mtmpIssueItemForUpdate.ItemDesc,
                                               mtmpIssueItemForUpdate.SerialNo, mtmpIssueItemForUpdate.DisplayQty, mtmpIssueItemForUpdate.nWOPendingQty,
                                               0, 0, txtRemarkForUnsedParts.Text, mtmpIssueItemForUpdate.ReceiptItemID, tmpIssue.ToTypeID,
                                               mtmpIssueItemForUpdate.RequisitionItemID, tmpIssue.ReqTextNo, tmpIssue.ReqDateFormatted.ToString,
                                               mtmpIssueItemForUpdate.DisplayUnitName, User.Identity.Name, mtmpIssueItemForUpdate.BatchNo)
            End If
        Next
    End Sub
    'End
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
                            mIssue.CalculateTotal()            'Added By Saylee on 7-July-2011
                            Session("mIssue") = mIssue
                            Response.Redirect("wfIssueForUnusedReturn.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
                        'Added Code
                        Session("sender") = ""
                        If Session("IsValid") Then

                            DataFieldBind()

                            '------------------------------------------------------------------------------------------------------------
                            If mIssue.TransTypeID = 19 And mIssue.IssueItems.IsSerializedExists = True Then
                                'Dim msg1 As New SIMsgBox(Page, "Discard confirmation", "Your are about to Discard Serialized/Rotable Item : " + mIssue.IssueItems.SerializedPartNo + "  Do you want to continue?", "", MsgBoxStyle.YesNo)
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Discard, SIMsgBox.Message_text.Discard, "Part No. " & mIssue.IssueItems.SerializedPartNo, MsgBoxStyle.YesNo)
                                'msg1.ReplacePage = "wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage")
                                'Session("sender") = "IsSerializedExists"
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.Discard, MSGBox.Message_text.Discard, "Part No. " & mIssue.IssueItems.SerializedPartNo, MsgBoxStyle.YesNo, "IsSerializedExists")
                                Exit Sub
                            End If
                            '------------------------------------------------------------------------------------------------------------

                            Session.Remove("IsValid")
                            If Not CustomValidate2() Then Exit Sub
                            Save()
                        Else
                            Session.Remove("IsValid")
                            Response.Redirect("wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If

                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If Session("IsValid") Then
                            If CType(Session("UnusedReturn"), String) = "UnusedReturn" Then
                                Session.Remove("IsValid")
                                DataFieldBind()
                                Session("IsFromMessageBox") = True
                                Save()
                                Dim tmpIssue As Issue = Session("tmpIssue")
                                If mIssue.IssueItems.Count = 0 Then
                                    If Not (tmpIssue.IssueItems.CurrentItem.IsReturnableFromAircraft And tmpIssue.IssueItems.CurrentItem.LoanQty < tmpIssue.IssueItems.CurrentItem.Qty) Then
                                        Response.Redirect("Index.aspx")
                                    End If

                                End If
                            End If
                        Else
                            Session.Remove("IsValid")
                            Response.Redirect("wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                        '------------------------------------------------------------------------------------------------------
                    ElseIf MSGBoxCtrl.Sender = "IsSerializedExists" Then  '' Close confirmation
                        'Added Code
                        Session("sender") = ""
                        If Session("IsValid") Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If Not CustomValidate2() Then Exit Sub
                            SaveIsSerializedExists()
                        Else
                            Session.Remove("IsValid")
                            Response.Redirect("wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                    ElseIf MSGBoxCtrl.Sender = "IsSerializedExistsStatus" Then
                        Session("sender") = ""
                        If Session("IsValid") Then
                            If CType(Session("ReturnWO"), String) = "ReturnWO" And CType(Session("IsForWOReturn"), Boolean) = True Then
                                ReturnWOQty()
                                Session.Remove("ReturnWO")
                                If mIssue.IssueItems.Count = 0 Then
                                    mIssue.Delete()
                                    mIssue.Save()
                                    Response.Redirect("Index.aspx")
                                End If
                            End If
                            Session.Remove("IsValid")
                            DataFieldBind()
                            SaveIsSerializedExists()
                        Else
                            Session.Remove("IsValid")
                            Response.Redirect("wfIssueForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                        '------------------------------------------------------------------------------------------------------
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session.Remove("mTypeList1")
                        If mIssue.IsNew Then
                            Session.Remove("mIssue")
                        End If
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")

                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        Session.Remove("mTypeList1")

                        If CType(Session("UnusedReturn"), String) = "UnusedReturn" Then
                            Session.Remove("UnusedReturn")
                        End If
                        Response.Redirect("wfIssueForUnusedReturn.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
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
                        'Response.Redirect("Index.aspx")
                        Response.Redirect("wfIssueForUnusedReturn.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

                        '-----------------Added by Vikrant on 26-aug-2011--------------------------
                    ElseIf MSGBoxCtrl.Sender = "Expired" Then  '' Expired confirmation
                        Session("sender") = ""
                        Session.Remove("mPendingItemList")
                        DataFieldBind()
                        '-----------------------------------------------------

                    Else
                        Session("Sender") = ""
                        Response.Redirect("wfIssueForUnusedReturn.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
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
                        Response.Redirect("wfIssueForUnusedReturn.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        'Added by Utkarsh On 20-Nov-2013 For TransTextSeries
                    ElseIf CType(Session("sender"), String) = "IssueTransTextSeriesAlert" Then
                        Session("sender") = ""
                        Session("AddTransTextSeries") = "True"
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                        'ENd
                    Else
                        Session("sender") = ""
                        DataFieldBind()
                        Response.Redirect("wfIssueForUnusedReturn.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
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
            Response.Redirect("wfIssueForUnusedReturn.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub SetControlStatus(ByVal StatusId As Int16, Optional ByVal IsForWOReturn As Boolean = False)
        btnAddItem.Enabled = False
        txtRemark.Enabled = False
        txtText.Enabled = False
        txtNo.Enabled = False
        txtPerson.Enabled = False

        GVIssueItems.Columns(15).Visible = IIf(CType(mIssue.TransTypeID, Flypal.Trans) = Flypal.Trans.DisacrdPart,
                                                     True,
                                                     False)
    End Sub
    Private Sub ControlVisibility()

        If mIssue.StatusID = 1 Then
            IssueDate.Enabled = True
        Else
            IssueDate.Enabled = False
        End If

        btnCancel.Visible = (Not mIssue.IsNew) And (mIssue.StatusID = 2) And (mIssue.IsSync = 0)  'One Condition Added by Saylee on 2-June-2010

        GVIssueItems.Columns(19).Visible = IIf(CType(Session("IsForWOReturn"), Boolean) = True, True, False)

        '--------------Added by vikrant on 26-aug-2011----------------
        If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then

            If ((mIssue.TransTypeID = 14) Or (mIssue.TransTypeID = 20) Or (mIssue.TransTypeID = 15) Or
                (mIssue.TransTypeID = 17) Or (mIssue.TransTypeID = 24) Or (mIssue.TransTypeID = 63) Or
                (mIssue.TransTypeID = 19) Or (mIssue.TransTypeID = 25) Or (mIssue.TransTypeID = 26) Or
                (mIssue.TransTypeID = 44) Or (mIssue.TransTypeID = 45) Or (mIssue.TransTypeID = 52)) Then

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
                Session.Remove("strMSG")

            End If

        Else
            ControlVisibilityForQty()
        End If

        If (mIssue.IssueItems.Count = 0) Then

            btnPrint.Enabled = False
            btnReleaseNoteNo.Enabled = False

        ElseIf mIssue.IsNew Then

            btnPrint.Enabled = False
            btnReleaseNoteNo.Enabled = False

        End If

        'Added By Prashant 17-Aug-2011
        If Not IsInRole(Rights.Authorized) Then

            btnCancel.Enabled = False
            btnCancel.ToolTip = "You are not authorized user "

        End If

    End Sub
    'Added by vikrant on 31-aug-2011
    Public Sub ControlVisibilityForQty()
        Dim txtValue As TextBox
        Dim mIssueItem As IssueItem
        Dim i As Integer = 0
        For Each mIssueItem In mIssue.IssueItems
            With mIssueItem
                Try
                    txtValue = CType(Me.GVIssueItems.Rows(i).FindControl("txtQty"), TextBox)
                    'txtValue.ReadOnly = False
                    txtValue.Enabled = False
                Catch ex As Exception

                End Try
            End With
            i = i + 1
        Next
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
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        IsInRoleString = "UnusedIssuedItems"
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
        End Select
    End Function
    Private Sub addattributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
        Dim txtReturnvalue As TextBox
        For i As Integer = 0 To GVIssueItems.Rows.Count - 1
            Try
                txtReturnvalue = CType(Me.GVIssueItems.Rows(i).FindControl("txtReturnQty"), TextBox)
                txtReturnvalue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtReturnvalue.ClientID + "').value,event)")
            Catch ex As Exception
            End Try

        Next
    End Sub
    Public Sub TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtValue As TextBox
        Dim mIssueItem As IssueItem
        Dim i As Integer = 0
        For Each mIssueItem In mIssue.IssueItems
            With mIssueItem
                Try
                    txtValue = CType(Me.GVIssueItems.Rows(i).FindControl("txtQty"), TextBox)
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
                Case Util.Trans.IssuetoSupplierNone
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
                Case Else
                    Return False
            End Select
        End If
    End Function
    Private Sub IssueTo()
        If mIssue.TransTypeID = Trans.ExchangeRepairIssueToVendor Or mIssue.TransTypeID = Trans.LoanIssueToVendor Or mIssue.TransTypeID = Trans.IssueToCustomer Or mIssue.TransTypeID = Trans.LoanIssueToCustomer Or mIssue.TransTypeID = Trans.IssuetoSupplierNone Or mIssue.TransTypeID = Trans.DisacrdPart Or mIssue.TransTypeID = Trans.IssuetoSupplierasRentalLease Or mIssue.TransTypeID = Trans.IssueToCustomerAsRepairedReturn Or mIssue.TransTypeID = Trans.IssueforLoanReturntoCustomer Or mIssue.TransTypeID = Trans.IssueforLoanReturntoSupplier Then
            mIssueTo = cmbVendorList.SelectedItem.Text
        ElseIf mIssue.TransTypeID = Trans.IssueToAircraft Or mIssue.TransTypeID = Trans.LoanIssuedToAircraft Then
            mIssueTo = cmbAircraftList.SelectedItem.Text
        ElseIf mIssue.TransTypeID = Trans.IssueToStore Or mIssue.TransTypeID = Trans.LoanIssueToStore Or mIssue.TransTypeID = Trans.LoanReturnToStore Then
            mIssueTo = cmbLocationStore.SelectedItem.Text
        ElseIf mIssue.TransTypeID = Trans.IssueToWorkShop Or mIssue.TransTypeID = Trans.LoanIssueToWorkShop Then            'Added By Prashant 7/4/2008
            mIssueTo = cmbWorkShop.SelectedItem.Text
        ElseIf mIssue.TransTypeID = Trans.IssueToWorkOrder Or mIssue.TransTypeID = Trans.IssueToWorkOrderAsSpares Or mIssue.TransTypeID = Trans.IssueToWorkOrderAsTools Then           'Added By Prashant 7/4/2008
            mIssueTo = cmbWorkOrder.SelectedItem.Text
        End If
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
            mVendorList = VendorList.GetVendortList(0, , , , , , True, getVendorStatus(mIssue.TransTypeID, RequstFor.Customer), getVendorStatus(mIssue.TransTypeID, RequstFor.Supplier))
        End If
        '--------------------------------------
        'mMachineList = FlyPal22.Maintain.SelectList.GetMachineList(New SmartDate(mIssue.IDate.ToString).FormattedText, mIssue.IsNew, "<SELECT>")
        mMachineNameValueList = MachineNameValueList.GetMachineList(mIssue.IDateFormatted.ToString, IsTagRequired:=True, TagText:="(SELECT)", ForInventory:=True)
        mStatusList = StatusList.GetStatusList(mIssue.StatusID, True)
        ''OLD WO Object ===Commented by Saylee on 8-Dec-2010
        '' mWOList = FlyPal22.Maintain.WOList.GetWOList(, , 0, New SmartDate("01-01-1800").FormattedText, New SmartDate(mIssue.IDate.ToString).FormattedText, , , , , , , 2, , "<SELECT>")
        ''mnWOListForCombo = nWOListForCombo.GetnWOListForCombo("(SELECT)", , , New SmartDate("01-01-1800").FormattedText, New SmartDate(mIssue.IDate.ToString).FormattedText, , ,  2)
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

        cmbWO.DataSource = mnWOListForCombo
        cmbWorkOrder.DataSource = mnWOListForCombo
        GVIssueItems.DataSource = mIssue.IssueItems
        'dgIssueTerms.DataSource = mIssue.IssueTerms
        IssueDate.Text = CDate(mIssue.IDate).ToString(DateFormat)

        SetSession()


        ''cmbWO.DataSource = mWOList
        ''cmbWorkOrder.DataSource = mWOList

        cmbWO.DataSource = mnWOListForCombo
        cmbWorkOrder.DataSource = mnWOListForCombo

        DataBind()
        Select Case mIssue.ToTypeID
            Case 1  'Vendor
                cmbVendorList.Visible = True
            Case 2  'Aircraft
                cmbAircraftList.Visible = True
            Case 7  'Discard
                cmbVendorList.Visible = True
                'lblSelectDetails.Visible = False
                lblSelectDetails.Visible = True 'Added By Prashant 5-July-2011
                lblSelectDetailsStar1.Visible = False
            Case 8  'Store
                cmbLocationStore.Visible = True
            Case 16 'Work Shop
                cmbWorkShop.Visible = True
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
    End Sub

    Public Sub customValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        'Added By Vikrant on 30-Oct-2013 For All29102013
        If custValidator.ControlToValidate = "calReturnDate" Then
            If Trim(calReturnDate.Text) = "" Then
                custValidator.ErrorMessage = "Select Return Date."
                e.IsValid = False
            ElseIf DateDiff(DateInterval.Day, mIssue.IDate, CDate(calReturnDate.Text)) < 0 Then
                custValidator.ErrorMessage = "Return Date must be greater than or equal to Issue Date."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtRemarkForUnsedParts" Then
            If Trim(txtRemarkForUnsedParts.Text).Length > 150 Then
                custValidator.ErrorMessage = "Remark must be less than 150 characters."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbToType" Then
            Dim cnt As Integer = 0
            For i As Integer = 0 To GVIssueItems.Rows.Count - 1
                Dim txtBox As TextBox = GVIssueItems.Rows(i).FindControl("txtReturnQty")
                If Val(Trim(txtBox.Text)) <> 0 Then
                    Exit For
                Else
                    cnt = cnt + 1
                End If
            Next
            If cnt = GVIssueItems.Rows.Count Then
                custValidator.ErrorMessage = "Please enter Return Qty for at least one Item."
                e.IsValid = False
            End If
        End If
        'End
    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011

        SetControlStatus(mIssue.StatusID, mIsForWOReturn)
        If Not IsPostBack And Session("sender") = "" Then
            If txtText.Enabled = True Then
                setFocus(txtText)
            End If
            'Added by Utkarsh ON 20-Nov-2013 FOr TransTextSeries
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then

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
            'End
            DataFieldBind()
        End If
        SetPage()
        'MessageBoxResult()
        ControlVisibility()
        If mIssue.IsNew Then
            lblStatus.Text = "OPEN"
        End If
        If mIssue.IssueItems.Count > 0 Then
            Disable()
        Else
            Enable()
        End If
        addattributes() 'Changed By Utkarsh On 10-May-2012 For ALLIssue04052012
        TextChanged(sender, e)
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            Exit Sub
        End If
        'Page.Validate("1")
        If IsValid Then
            'Commented & Added By Vikrant On 29-10-2013 For All29102013
            'Save()
            ClearModalPopControls()
            mdlPopUpUnusedReturnedParts.Show()
            'End
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("Index.aspx")
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not IsInRole(Rights.Print) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter

        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

        If mIssue.TransTypeID = Flypal.Util.Trans.DisacrdPart Then
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                myReport = New crptIssueLandScapeDiscard
            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
                myReport = New crptIssueDetailPotraitTAALDiscard
            Else
                myReport = New crptIssueDetailPotraitDiscard
            End If
        Else
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                myReport = New crptIssueLandScape
            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
                myReport = New crptIssueDetailPotraitTAAL
            Else
                myReport = New crptIssueDetailPotrait
            End If
        End If
        '---------- 'Addded by vikrant on 7-sept-2011------------
        Dim mSearchstring As String
        If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then
            mSearchstring = "True"
        Else
            mSearchstring = "False"
        End If
        '--------------------------------------------------------
        Dim objIssue As rptIssues
        Dim objChilds As rptIssueChields
        Dim letter As rptLetterHead
        Dim ds As New dsIssue

        objIssue = rptIssues.GetIssues(mIssue.ID)
        objChilds = rptIssueChields.GetIssuechilds(mIssue.ID)
        letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", mSearchstring, AppSettings("Logo"))
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, objIssue)
        da.Fill(ds, objChilds)
        da.Fill(ds, letter)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        Dim str As String
        str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", str, True)
    End Sub
    Private Sub btnReleaseNoteNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReleaseNoteNo.Click
        If Not IsInRole(Rights.Print) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then
            myReport = New crptReleaseNoteHeligo
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
        Dim str As String
        str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", str, True)
    End Sub
    'Added By Vikrant On 29-10-2013 For All29102013
    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If IsValid Then
            mdlPopUpUnusedReturnedParts.Hide()
            Save()
        End If
    End Sub
    Private Sub btnCloseUnusedReturnPart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseUnusedReturnPart.Click
        mdlPopUpUnusedReturnedParts.Hide()
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'End
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
            'cvWorkShop.ErrorMessage = strMsg
            'cvWorkShop.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

    'Added By Vikrant On 06-Nov-2013 For All29102013
    Private Sub calReturnDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calReturnDate.TextChanged
        If IsDate(calReturnDate.Text) Or (calReturnDate.Text = "") Then
            If Trim(calReturnDate.Text) <> "" Then
                Dim tmpReturnDate As New SmartDate()
                tmpReturnDate.Text = CDate(calReturnDate.Text).ToString(DateFormat)
                calReturnDate.Text = tmpReturnDate.FormattedText
            Else
                calReturnDate.Text = ""
            End If
        Else
            calReturnDate.Text = ""
        End If
    End Sub
    'Added by Utkarsh on 14-Nov-2013 for Trans Text Series

    Private Sub IssueDateChanged(sender As Object, e As EventArgs) Handles IssueDate.TextChanged

        mIssue = Session("mIssue")
        If Not IsDate(IssueDate) Then
            mIssue.IDate = DBNull.Value
        Else
            mIssue.IDate = IssueDate.Text
        End If
        txtText.Text = mIssue.Text
        Session("mIssue") = mIssue

    End Sub
    'End
End Class
