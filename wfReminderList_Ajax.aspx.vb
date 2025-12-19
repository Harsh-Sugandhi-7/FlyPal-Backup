Public Class wfReminderList_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declartation "
    Public mReminder As Reminder
    Public mDayOfWeek As String
    Public Type As Int32 = 0
    Private mAlertCount As AlertCount
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mReminder = CType(Session("mReminder"), Reminder)
        mDayOfWeek = CType(Session("mDayOfWeek"), String)
    End Sub
    Private Sub SetSession()
        Session("mReminder") = mReminder
        Session("mDayOfWeek") = mDayOfWeek
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mReminder")
        Session.Remove("mDayOfWeek")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
    'Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
    '    Dim CustValidate As CustomValidator
    '    CustValidate = CType(s, CustomValidator)

    '    If CustValidate.ControlToValidate = "lstReminders" Then
    '        If lstReminders.SelectedIndex = -1 Then
    '            CustValidate.ErrorMessage = "Select Reminder from the list."
    '            e.IsValid = False
    '        Else
    '            e.IsValid = True
    '        End If
    '    End If
    'End Sub
    Private Sub GetReminders()
        mAlertCount = AlertCount.GetAlertCountList(IsBAReorderQtyFormulaRequired:=IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS", True, False)) 'Added by Vikrant On 28-May-2020 Optional Parameter IsBAReorderQtyFormulaRequired
         If User.IsInRole("PendingOrderView") Then                            'Pending Order'
            lstReminders.Items.Add("Pending Order" + " (" + mAlertCount.PendingOrderCount.ToString + ")")
        End If
        If User.IsInRole("PendingPaymentView") Then                          'Pending Payment
            Dim mrptPendingPayment As rptPendingPayment
            mrptPendingPayment = rptPendingPayment.GetPendingPayment("1/1/1900", "1/1/2200", "", "")
            lstReminders.Items.Add("Pending Payment" + " (" + mrptPendingPayment.Count.ToString + ")")
        End If
        If User.IsInRole("PendingRCIFromStoreView") Then                     'Pending to receipts from other Store'
            lstReminders.Items.Add("Pending to receipts from other Store" + " (" + mAlertCount.PendingToReceiptFromStoreCount.ToString + ")")
        End If
        If User.IsInRole("PendingRCIFromStoreForLoanView") Then              'Pending to receipts as loan taken from other Store'
            lstReminders.Items.Add("Pending to receipts as loan taken from other Store" + " (" + mAlertCount.PendingtoreceiptsasloantakenfromotherStoreCount.ToString + ")")
        End If
        If User.IsInRole("PendingRCIFromStoreForLoanReturnView") Then        'Pending returnable against loan issued to Store'
            lstReminders.Items.Add("Pending returnable against loan issued to Store" + " (" + mAlertCount.PendingToReceiptAgainstLoanIssueToStoreCount.ToString + ")")
        End If
        If User.IsInRole("PendingRCIFromAircraftForLoanReturnView") Then     'Pending returnable against loan issued to Aircraft'
            lstReminders.Items.Add("Pending returnable against loan issued to Aircraft" + " (" + mAlertCount.PendingreturnableagainstloanissuedtoAircraftCount.ToString + ")")
        End If
        If User.IsInRole("PendingRCIFromVendorView") Then                    'Pending returnable Exchange/Repair issued to Vendor'
            lstReminders.Items.Add("Pending returnable Exchange/Repair issued to Vendor" + " (" + mAlertCount.PendingToReceiptAgainstExchangeRepairFromVendorCount.ToString + ")")
        End If
        If User.IsInRole("PendingIssueLoanReturnToStoreView") Then           'Pending to issue as loan return to Store'
            lstReminders.Items.Add("Pending to issue as loan return to Store" + " (" + mAlertCount.PendingToIssueAsLoanTakenFromStoreCount.ToString + ")")
        End If
        If User.IsInRole("ExpiryDateView") Then                              'Expiry Date'
            lstReminders.Items.Add("Expiry Date" + " (" + mAlertCount.ExpiryDateCount.ToString + ")")
        End If
        If User.IsInRole("NoValueItemNoInvoiceView") Then                    'No Value/Invoice'
            lstReminders.Items.Add("No Value/Invoice" + " (" + mAlertCount.NoValueItemNoInvoiceCount.ToString + ")")
        End If
        If User.IsInRole("QUARANTINEStoreStockView") Then                    'Quarantine StoreStock
            Dim mrptQUARANTINEReport As rptQUARANTINEReport
            mrptQUARANTINEReport = rptQUARANTINEReport.GetQUARANTINEReport("1/1/1900", "1/1/2200", "", "", "")
            lstReminders.Items.Add("Quarantine StoreStock" + " (" + mrptQUARANTINEReport.Count.ToString + ")")
        End If
        If User.IsInRole("MinLevelItemsView") Then                           'Min Level Items'
            lstReminders.Items.Add("Min Level Items" + " (" + mAlertCount.MinLevelItemsCount.ToString + ")")
        End If
        If User.IsInRole("OrderRegView") Then                                'Ordered Items  Only Order Items count shown
            lstReminders.Items.Add("Ordered Items" + " (" + mAlertCount.OrderedItemsCount.ToString + ")")
        End If
        'Receipt
        If User.IsInRole("ReceiptPORegView") Then                               'Receipt against Purchase Order 'Only Receipt Items count shown
            lstReminders.Items.Add("Receipt against Purchase Order Register" + " (" + mAlertCount.ReceiptagainstPurchaseOrderCount.ToString + ")")
        End If

        ''RCIs
        'If User.IsInRole("RCIFromPORegView") Then                               'Receipt cum Invoice against Purchase Order Register
        '    lstReminders.Items.Add("Receipt cum Invoice against Purchase Order Register")
        'End If

        'If User.IsInRole("RCIFromAircraftRegView") Then                         'Received from Aircraft Register
        '    lstReminders.Items.Add("Received from Aircraft Register")
        'End If

        'If User.IsInRole("RCIFromStoreRegView") Then                            'Received from Store Register
        '    lstReminders.Items.Add("Received from Store Register")
        'End If

        'If User.IsInRole("RCIFromVendorRegView") Then                            'Received as Exchange / Repair from Vendor Register
        '    lstReminders.Items.Add("Received as Exchange / Repair from Vendor Register")
        'End If

        'If User.IsInRole("RCIFromStoreForLoanRegView") Then                      'Received as loan taken from another Store Register
        '    lstReminders.Items.Add("Received as loan taken from another Store Register")
        'End If

        'If User.IsInRole("RCIFromAircraftForLoanReturnRegView") Then             'Receipt against loan issued to Aircraft Register
        '    lstReminders.Items.Add("Receipt against loan issued to Aircraft Register")
        'End If

        'If User.IsInRole("RCIFromStoreForLoanReturnRegView") Then                'Receipt against loan issued to Store Register
        '    lstReminders.Items.Add("Receipt against loan issued to Store Register")
        'End If

        ''Issues
        'If User.IsInRole("IssueToAircraftRegView") Then                             'Issue to Aircraft Register
        '    lstReminders.Items.Add("Issue to Aircraft Register")
        'End If

        'If User.IsInRole("IssueToStoreRegView") Then                                'Issue to Store Register
        '    lstReminders.Items.Add("Issue to Store Register")
        'End If

        'If User.IsInRole("IssueToVendorRegView") Then                               'Issue to Vendor Exchange / Repair Register
        '    lstReminders.Items.Add("Issue to Vendor Exchange / Repair Register")
        'End If

        'If User.IsInRole("IssueLoanToStoreRegView") Then                            'Issue loan to another Store Register
        '    lstReminders.Items.Add("Issue loan to another Store Register")
        'End If

        'If User.IsInRole("IssueLoanToAircraftRegView") Then                         'Issue loan to another Aircraft Register
        '    lstReminders.Items.Add("Issue loan to another Aircraft Register")
        'End If

        'If User.IsInRole("IssueLoanReturnToStoreRegView") Then                      'Issue for loan return to Store Register
        '    lstReminders.Items.Add("Issue for loan return to Store Register")
        'End If

        'If User.IsInRole("IssueToDiscardRegView") Then                              'Issue for Part Discard Register
        '    lstReminders.Items.Add("Issue for Part Discard Register")
        'End If

        'If User.IsInRole("PendingReceiptsOfRentalLeaseView") Then            'Pending Issue To Supplier As Rental Lease 
        '    lstReminders.Items.Add("Pending Receipts Of Rental/Lease")
        'End If

        'If User.IsInRole("ReOrderLevelItemsView") Then                  'Added By Prashant 10-Oct-2014 ALL10102014
        '    lstReminders.Items.Add("Re-Order-Level-Items")
        'End If

        'RCIs
        If User.IsInRole("RCIFromPORegView") Then                               'Receipt cum Invoice against Purchase Order Register
            lstReminders.Items.Add("Receipt cum Invoice against Purchase Order Register" + " (" + mAlertCount.RCIItemsAgainstPOCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        If User.IsInRole("RCIFromAircraftRegView") Then                         'Received from Aircraft Register
            lstReminders.Items.Add("Received from Aircraft Register" + " (" + mAlertCount.ReceivedFromAircraftItemsCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        If User.IsInRole("RCIFromStoreRegView") Then                            'Received from Store Register
            lstReminders.Items.Add("Received from Store Register" + " (" + mAlertCount.ReceivedFromStoreItemsCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        If User.IsInRole("RCIFromVendorRegView") Then                            'Received as Exchange / Repair from Vendor Register
            lstReminders.Items.Add("Received as Exchange / Repair from Vendor Register" + " (" + mAlertCount.PendingToReceiptAgainstExchangeRepairFromVendorCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        If User.IsInRole("RCIFromStoreForLoanRegView") Then                      'Received as loan taken from another Store Register
            lstReminders.Items.Add("Received as loan taken from another Store Register" + " (" + mAlertCount.ReceivedAsLoanTakenFromStoreItemsCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        If User.IsInRole("RCIFromAircraftForLoanReturnRegView") Then             'Receipt against loan issued to Aircraft Register
            lstReminders.Items.Add("Receipt against loan issued to Aircraft Register" + " (" + mAlertCount.ReceiptAgainstLoanIssuedToAircraftItemsCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        If User.IsInRole("RCIFromStoreForLoanReturnRegView") Then                'Receipt against loan issued to Store Register
            lstReminders.Items.Add("Receipt against loan issued to Store Register" + " (" + mAlertCount.ReceiptAgainstLoanIssuedToStoreItemsCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        'Issues
        If User.IsInRole("IssueToAircraftRegView") Then                             'Issue to Aircraft Register
            lstReminders.Items.Add("Issue to Aircraft Register" + " (" + mAlertCount.IssueToAircraftItemsCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        If User.IsInRole("IssueToStoreRegView") Then                                'Issue to Store Register
            lstReminders.Items.Add("Issue to Store Register" + " (" + mAlertCount.IssueToStoreItemsCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        If User.IsInRole("IssueToVendorRegView") Then                               'Issue to Vendor Exchange / Repair Register
            lstReminders.Items.Add("Issue to Vendor Exchange / Repair Register" + " (" + mAlertCount.IssueToVendorAsExchangeRepairItemsCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        If User.IsInRole("IssueLoanToStoreRegView") Then                            'Issue loan to another Store Register
            lstReminders.Items.Add("Issue loan to another Store Register" + " (" + mAlertCount.IssueLoanToStoreItemsCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        If User.IsInRole("IssueLoanToAircraftRegView") Then                         'Issue loan to another Aircraft Register
            lstReminders.Items.Add("Issue loan to another Aircraft Register" + " (" + mAlertCount.IssueLoanToAircraftItemsCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        If User.IsInRole("IssueLoanReturnToStoreRegView") Then                      'Issue for loan return to Store Register
            lstReminders.Items.Add("Issue for loan return to Store Register" + " (" + mAlertCount.IssueLoanReturnToStoreItemsCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        If User.IsInRole("IssueToDiscardRegView") Then                              'Issue for Part Discard Register
            lstReminders.Items.Add("Issue for Part Discard Register" + " (" + mAlertCount.IssuePartDiscardItemsCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        If User.IsInRole("PendingReceiptsOfRentalLeaseView") Then            'Pending Issue To Supplier As Rental Lease 
            lstReminders.Items.Add("Pending Receipts Of Rental/Lease" + " (" + mAlertCount.PendingRentalLeaseReceiptItemCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If
        If User.IsInRole("ReOrderLevelItemsView") Then                  'Added By Prashant 10-Oct-2014 ALL10102014
            lstReminders.Items.Add("Re-Order-Level-Items" + " (" + mAlertCount.ReOrderLevelItemsCount.ToString + ")") 'Added by Vikrant On 28-May-2020 - Count
        End If

        If User.IsInRole("CalibrationDueReportView") Then                                 'Calibration Due Report
            'Dim mAlertList As AlertList
            'mAlertList = AlertList.GetAlertList()
            lstReminders.Items.Add("Due For Calibration Item" + " (" + mAlertCount.DueFCICount.ToString + ")")
        End If

        'Maintenance
        If User.IsInRole("Due-PeriodWiseView") Then                                 'Due-PeriodWise
            lstReminders.Items.Add("Due-Periodwise")
        End If

        If User.IsInRole("MELSnagRegisterView") Then                  'Added By Saylee 18-Apr-2018
            lstReminders.Items.Add(IIf(AppSettings("MELSnagNomenclature") = "True", "Open ADD/Defect(s)", "Open MEL/Snag(s)"))
        End If

        'NOTE: Any new role : if checked any Role here, add same into Reminder.vb file (AutoReminder Folder)
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not IsPostBack Then
            'Session("MiddleFrame") = ""   'Added Code
            GetSession()
            If lstReminders.Enabled = True Then
                setFocus(lstReminders)
            End If
            GetReminders()
        End If

    End Sub
    Private Sub btnShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShow.Click
        If lstReminders.SelectedIndex = -1 Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Select Report Type from the List.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString= "Pending Order" Then
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Pending Order") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim rpt As rptPendingOrder
                Dim ds As New dsOrder
                myReport = New crptPendingOrder
                Dim objsearch As rptSearchingCriteria
                Dim dsPenOrd As New dsOrder
                objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "")
                rpt = rptPendingOrder.GetPendingOrder("1/1/1900", "1/1/2200", "", "", "")
                If rpt.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(dsPenOrd, rpt)
                da.Fill(dsPenOrd, objsearch)
                myReport.SetDataSource(dsPenOrd)
                Session("CrystalReport") = myReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
            Finally
            End Try
        End If

        'If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString = "Pending Payment" Then
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Pending Payment") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim rpt As rptPendingPayment
                myReport = New crptPendingPayment
                Dim objsearch As rptSearchingCriteriaForReceipt

                Dim dsPenPay As New dsPendingPayment
                objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                rpt = rptPendingPayment.GetPendingPayment("1/1/1900", "1/1/2200", "", "")

                If rpt.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                dsPenPay.Clear()
                da.Fill(dsPenPay, rpt)
                da.Fill(dsPenPay, objsearch)
                myReport.SetDataSource(dsPenPay)
                Session("CrystalReport") = myReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
            Finally
            End Try
        End If

        'If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString = "Pending to receipts from other Store" Then
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Pending to receipts from other Store") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim objsearch As rptSearchingCriteriaForReceipt
                Dim mrptIssueRegForReminder As rptIssueRegForReminder

                Dim ds As New dsIssue
                myReport = New crptPendingToReceiptIssues

                mrptIssueRegForReminder = rptIssueRegForReminder.GetPendingToReceiptFromStore("1/1/1900", "1/1/2200", "", "", "", "")

                objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                If mrptIssueRegForReminder.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, mrptIssueRegForReminder)
                da.Fill(ds, objsearch)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
            Finally
            End Try
        End If
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Pending to receipts as loan taken from other Store") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim objsearch As rptSearchingCriteriaForReceipt
                Dim mrptIssueRegForReminder As rptIssueRegForReminder

                Dim ds As New dsIssue
                myReport = New crptPendingToReceiptIssues

                mrptIssueRegForReminder = rptIssueRegForReminder.GetPendingToReceiptAsLoanFromStore("1/1/1900", "1/1/2200", "", "", "", "")

                objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                If mrptIssueRegForReminder.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, mrptIssueRegForReminder)
                da.Fill(ds, objsearch)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
            Finally
            End Try
        End If
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Pending returnable against loan issued to Store") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim objsearch As rptSearchingCriteriaForReceipt
                Dim mrptIssueRegForReminder As rptIssueRegForReminder

                Dim ds As New dsIssue
                myReport = New crptPendingToReceiptIssues

                mrptIssueRegForReminder = rptIssueRegForReminder.GetPendingToReceiptAgainstLoanIssueToStore("1/1/1900", "1/1/2200", "", "", "", "")

                objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                If mrptIssueRegForReminder.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, mrptIssueRegForReminder)
                da.Fill(ds, objsearch)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
            Finally
            End Try
        End If
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Pending returnable against loan issued to Aircraft") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim objsearch As rptSearchingCriteriaForReceipt
                Dim mrptIssueRegForReminder As rptIssueRegForReminder

                Dim ds As New dsIssue
                myReport = New crptPendingToReceiptIssues
                mrptIssueRegForReminder = rptIssueRegForReminder.GetPendingToReceiptAgainstLoanIssueAircraft("1/1/1900", "1/1/2200", "", "", "", "")
                objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                If mrptIssueRegForReminder.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, mrptIssueRegForReminder)
                da.Fill(ds, objsearch)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
            Finally
            End Try
        End If

        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Pending returnable Exchange/Repair issued to Vendor") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim objsearch As rptSearchingCriteriaForReceipt
                Dim mrptIssueRegForReminder As rptIssueRegForReminder

                Dim ds As New dsIssue
                myReport = New crptPendingToReceiptIssues
                mrptIssueRegForReminder = rptIssueRegForReminder.GetPendingToReceiptAgainstExchangeRepairFromVendor("1/1/1900", "1/1/2200", "", "", "", "")
                objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                If mrptIssueRegForReminder.Count <= 0 Then
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, mrptIssueRegForReminder)
                da.Fill(ds, objsearch)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
            Finally
            End Try
        End If

        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Pending to issue as loan return to Store") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim objsearch As rptSearchingCriteriaForReceipt
                Dim mrptPendingToIssueAsLoanTakenFromStore As rptPendingToIssueAsLoanTakenFromStore
                Dim ds As New dsIssue
                myReport = New crptPendingToIssueToStoreAsLoanReturn
                mrptPendingToIssueAsLoanTakenFromStore = rptPendingToIssueAsLoanTakenFromStore.GetPendingToIssueAsLoanTakenFromStore("1/1/1900", "1/1/2200", "", "", "", "")

                objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                If mrptPendingToIssueAsLoanTakenFromStore.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, mrptPendingToIssueAsLoanTakenFromStore)
                da.Fill(ds, objsearch)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
            Finally
            End Try
        End If

        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Expiry Date") Then

            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim rpt As rptExpiryDate
            Dim ds As New dsExpiryDate
            Dim objSearch As rptSearchingCriteria
            Dim cate As String
            Dim store As String
            Dim Nome As String
            myReport = New crptExpiryDate
            cate = ""
            store = ""
            Nome = ""

            objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), Today.Date.ToString, "", "", "", "", "", "", "", "", "", "", "")
            rpt = rptExpiryDate.GetExpiryDate(Today.Date.ToString, "", "", "", "", "", 1, Today.Date)

            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            ds.Clear()
            da.Fill(ds, rpt)
            da.Fill(ds, objSearch)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        End If
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("No Value/Invoice") Then
            Try

                Dim da As New CSLA.Data.ObjectAdapter
                Dim ds As New dsNoValueItemNoInvoice
                Dim obj As rptNoValueItemNoInvoice
                Dim myReport As New crptNoValueItemNoInvoice

                obj = New rptNoValueItemNoInvoice
                Dim objSearch As rptSearchingCriteria
                objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "", "", "", "", "", "", "", "", "", "", "", "")
                obj = rptNoValueItemNoInvoice.GetNoValueItemNoInvoiceList()
                If obj.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, obj)
                da.Fill(ds, objSearch)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
            Finally
            End Try
        End If
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Quarantine StoreStock") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim objsearch As rptSearchingCriteria
                Dim rpt As rptQUARANTINEReport
                Dim ds As New dsQUARANTINEReport
                myReport = New crptQUARANTINEReport
                rpt = rptQUARANTINEReport.GetQUARANTINEReport("1/1/1900", "1/1/2200", "", "", "")
                objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "")

                If rpt.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, rpt)
                da.Fill(ds, objsearch)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
            Finally
            End Try
        End If
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Min Level Items") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim objsearch As rptSearchingCriteria
                Dim rpt As rptMinLevelItem
                Dim ds As New dsMinStockLevel
                myReport = New crptMinLevelItem
                rpt = rptMinLevelItem.GetMinLevelItem("", "", "", "")
                objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", "", "", "", "", "", "", "", "", "", "")

                If rpt.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, rpt)
                da.Fill(ds, objsearch)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
            Finally
            End Try
        End If

        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Ordered Items") Then
            Try
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim objReg As rptOrderRegister
                Dim da As New CSLA.Data.ObjectAdapter
                Dim dsOrder As New dsOrder

                myReport = New crptOrderRegister
                objReg = rptOrderRegister.GetOrderList("", "", "", "", "", "", "1/1/1900", "1/1/2200", 0, "", "")
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                If objReg.Count <= 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx?Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                dsOrder.Clear()
                da.Fill(dsOrder, objReg)
                da.Fill(dsOrder, objSearch)
                myReport.SetDataSource(dsOrder)
                Session("CrystalReport") = myReport
                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

            Catch ex As Exception
                '        '        MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                '        '        Cursor.Current = Cursors.Default
            End Try
        End If

        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Receipt against Purchase Order Register") Then
            Try
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim objReg As rptReceiptReg
                Dim da As New CSLA.Data.ObjectAdapter
                Dim dsReceipt As New dsReceipt

                myReport = New crptReceiptRegister

                objReg = rptReceiptReg.GetRecepitList("1/1/1900", "1/1/2200", "", "", "", "", "", 0, "", "", "", "", "", "", "", 0, Util.Trans.ReceiptAgainstPuchaseOrder)
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                If objReg.Count <= 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx?Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                dsReceipt.Clear()
                da.Fill(dsReceipt, objReg)
                da.Fill(dsReceipt, objSearch)
                myReport.SetDataSource(dsReceipt)

                Session("CrystalReport") = myReport
                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

            Catch ex As Exception
                '    '        MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                '    '        Cursor.Current = Cursors.Default
            End Try
        End If
        ''--------------------------------
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Receipt cum Invoice against Purchase Order Register") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                'Commented and added by utkarsh on 15-jan-2014
                'Dim ds As New dsReceipt
                Dim ds As New dsRecCumInvReg
                'End
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim rpt As rptReceiptCumInvReg
                myReport = New crptReceiptCumInvoiceRegDetail

                rpt = rptReceiptCumInvReg.GetRecCumInvList("1/1/1900", "1/1/2200", "", "", "", "", "", 0, "", "", "", "", "", "", "", 0, "", "", Util.Trans.ReceiptcumInvoiceAgainstPuchaseOrder)
                Dim Title As String = GetTitlePurchase()

                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Title, "", "", "", "", "", "", "")

                If rpt.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx?Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, rpt)
                da.Fill(ds, objSearch)
                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport
                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

            Catch ex As Exception
                '        MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                '        Cursor.Current = Cursors.Default
            End Try
        End If
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Received from Aircraft Register") Then
            Try

                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                'Commented and added by utkarsh on 15-jan-2014
                'Dim ds As New dsReceipt
                Dim ds As New dsRecCumInvReg
                'End
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim rpt As rptReceiptCumInvReg

                myReport = New crptReceiptCumInvoiceRegDetail

                rpt = rptReceiptCumInvReg.GetRecCumInvList("1/1/1900", "1/1/2200", "", "", "", "", "", 0, "", "", "", "", "", "", "", 0, "", "", Util.Trans.ReceivedFromAircraft)
                Dim Title As String = GetTitleAircraft()
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Title, "", "", "", "", "", "", "")
                If rpt.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx?Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, rpt)
                da.Fill(ds, objSearch)
                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport
                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

            Catch ex As Exception
                '        MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                '        Cursor.Current = Cursors.Default
            End Try
        End If
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Received from Store Register") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                'Commented and added by utkarsh on 15-jan-2014
                'Dim ds As New dsReceipt
                Dim ds As New dsRecCumInvReg
                'End
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim rpt As rptReceiptCumInvReg
                myReport = New crptReceiptCumInvoiceRegDetail

                rpt = rptReceiptCumInvReg.GetRecCumInvList("1/1/1900", "1/1/2200", "", "", "", "", "", 0, "", "", "", "", "", "", "", 0, "", "", Util.Trans.ReceivedFromOtherStore)
                Dim Title As String = GetTitleStore()
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Title, "", "", "", "", "", "", "")
                If rpt.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx?Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, rpt)
                da.Fill(ds, objSearch)
                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport
                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

            Catch ex As Exception
                '        MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    Finally
                '        Cursor.Current = Cursors.Default
            End Try
        End If

        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Received as Exchange / Repair from Vendor Register") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                'Commented and added by utkarsh on 15-jan-2014
                'Dim ds As New dsReceipt
                Dim ds As New dsRecCumInvReg
                'End
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim rpt As rptReceiptCumInvReg
                myReport = New crptReceiptCumInvoiceRegDetail

                rpt = rptReceiptCumInvReg.GetRecCumInvList("1/1/1900", "1/1/2200", "", "", "", "", "", 0, "", "", "", "", "", "", "", 0, "", "", Util.Trans.ExchangeRepairReceivedFromVendor)
                Dim Title As String = GetTitleVendor()
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Title, "", "", "", "", "", "", "")
                If rpt.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx?Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, rpt)
                da.Fill(ds, objSearch)
                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport
                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

            Catch ex As Exception
                '        MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                '        Cursor.Current = Cursors.Default
            End Try
        End If


        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Received as loan taken from another Store Register") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                'Commented and added by utkarsh on 15-jan-2014
                'Dim ds As New dsReceipt
                Dim ds As New dsRecCumInvReg
                'End
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim rpt As rptReceiptCumInvReg
                myReport = New crptReceiptCumInvoiceRegDetail

                rpt = rptReceiptCumInvReg.GetRecCumInvList("1/1/1900", "1/1/2200", "", "", "", "", "", 0, "", "", "", "", "", "", "", 0, "", "", Util.Trans.LoanTakenFromStore)

                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                If rpt.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx?Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, rpt)
                da.Fill(ds, objSearch)
                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport
                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

            Catch ex As Exception
                '        MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                '        Cursor.Current = Cursors.Default
            End Try
        End If
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Receipt against loan issued to Aircraft Register") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                'Commented and added by utkarsh on 15-jan-2014
                'Dim ds As New dsReceipt
                Dim ds As New dsRecCumInvReg
                'End
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim rpt As rptReceiptCumInvReg
                myReport = New crptReceiptCumInvoiceRegDetail

                rpt = rptReceiptCumInvReg.GetRecCumInvList("1/1/1900", "1/1/2200", "", "", "", "", "", 0, "", "", "", "", "", "", "", 0, "", "", Util.Trans.ReceiptAgainstLoanIssuedToAircraft)

                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                If rpt.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx?Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, rpt)
                da.Fill(ds, objSearch)
                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport
                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
                '        MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                '        Cursor.Current = Cursors.Default
            End Try
        End If
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Receipt against loan issued to Store Register") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                'Commented and added by utkarsh on 15-jan-2014
                'Dim ds As New dsReceipt
                Dim ds As New dsRecCumInvReg
                'End
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim rpt As rptReceiptCumInvReg
                myReport = New crptReceiptCumInvoiceRegDetail

                rpt = rptReceiptCumInvReg.GetRecCumInvList("1/1/1900", "1/1/2200", "", "", "", "", "", 0, "", "", "", "", "", "", "", 0, "", "", Util.Trans.ReceiptAgainstLoanIssuedToStore)

                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                If rpt.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx?Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ds.Clear()
                da.Fill(ds, rpt)
                da.Fill(ds, objSearch)
                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport
                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
                '        MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                '        Cursor.Current = Cursors.Default
            End Try
        End If
        ''--------------------------------
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Issue to Aircraft Register") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                'Commneted and added by utkarsh on 15-Jan-2014
                'Dim ds As New dsReceipt
                Dim ds As New dsIssue
                'End
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

                Dim rpt As rptIssueReg


                myReport = New crptIssueRegister

                rpt = rptIssueReg.GetrptIssueList("", "", "1/1/1900", "1/1/2200", "", "", "", 0, 0, "", "", "", "", "", "", , , Util.Trans.IssueToAircraft)

                Dim Title As String = GetTitle6()
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Title, "", "", "", "", "", "", "")
                If rpt.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx??Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                da.Fill(ds, rpt)
                da.Fill(ds, objSearch)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
                '    '        MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                '    '        Cursor.Current = Cursors.Default
            End Try
        End If

        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Issue to Store Register") Then
            Try

                Dim da As New CSLA.Data.ObjectAdapter
                'Commneted and added by utkarsh on 15-Jan-2014
                'Dim ds As New dsReceipt
                Dim ds As New dsIssue
                'End
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

                Dim rpt As rptIssueReg

                myReport = New crptIssueRegister


                rpt = rptIssueReg.GetrptIssueList("", "", "1/1/1900", "1/1/2200", "", "", "", 0, 0, "", "", "", "", "", "", , , Util.Trans.IssueToStore)
                Dim Title As String = GetTitle5()
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Title, "", "", "", "", "", "", "")

                If rpt.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx??Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                da.Fill(ds, rpt)
                da.Fill(ds, objSearch)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
                'MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                '        Cursor.Current = Cursors.Default
            End Try
        End If

        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Issue to Vendor Exchange / Repair Register") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                'Commneted and added by utkarsh on 15-Jan-2014
                'Dim ds As New dsReceipt
                Dim ds As New dsIssue
                'End
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

                Dim rpt As rptIssueReg

                myReport = New crptIssueRegister


                rpt = rptIssueReg.GetrptIssueList("", "", "1/1/1900", "1/1/2200", "", "", "", 0, 0, "", "", "", "", "", "", , , Util.Trans.ExchangeRepairIssueToVendor)
                Dim Title As String = GetTitle4()
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Title, "", "", "", "", "", "", "")

                If rpt.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx??Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                da.Fill(ds, rpt)
                da.Fill(ds, objSearch)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
                '        '        MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                '        '        Cursor.Current = Cursors.Default
            End Try
        End If

        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Issue loan to another Store Register") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                'Commneted and added by utkarsh on 15-Jan-2014
                'Dim ds As New dsReceipt
                Dim ds As New dsIssue
                'End
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

                Dim rpt As rptIssueReg

                myReport = New crptIssueRegister

                rpt = rptIssueReg.GetrptIssueList("", "", "1/1/1900", "1/1/2200", "", "", "", 0, 0, "", "", "", "", "", "", , , Util.Trans.LoanIssueToStore)
                Dim Title As String = GetTitle3()
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Title, "", "", "", "", "", "", "")

                If rpt.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx??Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                da.Fill(ds, rpt)
                da.Fill(ds, objSearch)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
                '        '        MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                '        '        Cursor.Current = Cursors.Default
            End Try
        End If
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Issue loan to another Aircraft Register") Then
            Try
                '        '        Dim frm As New frmrptIssueRegister(Util.Trans.LoanIssuedToAircraft)

                Dim da As New CSLA.Data.ObjectAdapter
                'Commneted and added by utkarsh on 15-Jan-2014
                'Dim ds As New dsReceipt
                Dim ds As New dsIssue
                'End
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

                Dim rpt As rptIssueReg

                myReport = New crptIssueRegister

                rpt = rptIssueReg.GetrptIssueList("", "", "1/1/1900", "1/1/2200", "", "", "", 0, 0, "", "", "", "", "", "", , , Util.Trans.LoanIssuedToAircraft)
                Dim Title As String = GetTitle2()
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Title, "", "", "", "", "", "", "")

                If rpt.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx??Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                da.Fill(ds, rpt)
                da.Fill(ds, objSearch)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
                '        '        MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                '        '        Cursor.Current = Cursors.Default
            End Try
        End If

        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Issue for loan return to Store Register") Then
            Dim da As New CSLA.Data.ObjectAdapter
            'Commneted and added by utkarsh on 15-Jan-2014
            'Dim ds As New dsReceipt
            Dim ds As New dsIssue
            'End
            Dim objSearch As rptSearchingCriteriaForReceipt
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim rpt As rptIssueReg
            myReport = New crptIssueRegister

            rpt = rptIssueReg.GetrptIssueList("", "", "1/1/1900", "1/1/2200", "", "", "", 0, 0, "", "", "", "", "", "", , , Util.Trans.LoanReturnToStore)
            Dim Title As String = GetTitle1()
            objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Title, "", "", "", "", "", "", "")
            If rpt.Count = 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfReminderList.aspx??Backpage="
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")

                Exit Sub
            End If
            da.Fill(ds, rpt)
            da.Fill(ds, objSearch)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            Dim Str As String
            'Str = "<script language=Javascript>openTranDetail();</script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        End If

        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Issue for Part Discard Register") Then
            Try
                Dim da As New CSLA.Data.ObjectAdapter
                'Commneted and added by utkarsh on 15-Jan-2014
                'Dim ds As New dsReceipt
                Dim ds As New dsIssue
                'End
                Dim objSearch As rptSearchingCriteriaForReceipt
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

                Dim rpt As rptIssueReg

                myReport = New crptIssueRegister

                rpt = rptIssueReg.GetrptIssueList("", "", "1/1/1900", "1/1/2200", "", "", "", 0, 0, "", "", "", "", "", "", , , Util.Trans.DisacrdPart)

                Dim Title As String = GetTitle()
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Title, "", "", "", "", "", "", "")

                If rpt.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfReminderList.aspx??Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                da.Fill(ds, rpt)
                da.Fill(ds, objSearch)

                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport

                Dim Str As String
                'Str = "<script language=Javascript>openTranDetail();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Catch ex As Exception
                '        '        MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                '        '        Cursor.Current = Cursors.Default
            End Try
        End If

        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString = "Due-Periodwise" Then
            Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
            Dim ReportStatusList As New rptStatusList

            ReportMaintenanceDetails = New ReportMaintenanceDetailList
            ReportStatusList = New rptStatusList
            Dim da As New CSLA.Data.ObjectAdapter
            Dim ds As New dsReportMaintenanceDetail

            Dim rptDueDetail As New crDueReportDetailLandscape

            Dim mCompanyDetail As New CompanyDetail
            Dim searchstr As String = ""

            '--SetValues
            Dim MachineName As String = "{00000000-0000-0000-0000-000000000000}"
            Dim Average As String = ""
            Dim AsonDate As String = Today.Date.ToShortDateString
            Dim Aircraft As String = ""
            Dim AvgMnths As Integer = 0
            '------

            '----ReportDetail()
            Dim ReportType As Integer = 1
            Dim mDueLimits As DueLimits = DueLimits.GetDueLimits(New Guid("{00000000-0000-0000-0000-000000000000}"))
            Dim mMachineList As MachineList = MachineList.GetMachineListDueMonitoringStatus(AsonDate, mDueLimits, MachineName, , AvgMnths)

            ReportMaintenanceDetails.Add(mMachineList, ReportType)
            '------------------
            Dim mDueLimit As DueLimit
            For Each mDueLimit In mDueLimits
                If CDec(Val(mDueLimit.PeriodLimit)) > 0 Then
                    If searchstr = "" Then
                        searchstr = "For Next" & " " & searchstr & " " & mDueLimit.PeriodLimit & " " & mDueLimit.PeriodName
                    Else
                        searchstr = searchstr & ", " & mDueLimit.PeriodLimit & " " & mDueLimit.PeriodName
                    End If
                End If
            Next
            searchstr = searchstr & ", " & "As On Date:" & New SmartDate(Today.Date.ToString).FormattedText

            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, "Maintenance Status Report", searchstr, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), SearchStr13:=AppSettings("MELSnagNomenclature").ToString)
            If ReportMaintenanceDetails.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            ds.Clear()
            da.Fill(ds, ReportMaintenanceDetails)
            da.Fill(ds, Report)
            da.Fill(ds, ReportStatusList)
            rptDueDetail.SetDataSource(ds)
            Session("CrystalReport") = rptDueDetail
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        End If

        If lstReminders.Items(lstReminders.SelectedIndex).ToString.Contains("Pending Receipts Of Rental/Lease") Then
            Dim rpt As rptPendingReceiptsOfRentalLease
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim objsearch As rptSearchingCriteriaForReceipt
            Dim ds As New dsPendingReceiptsOfRentalLease

            rpt = rptPendingReceiptsOfRentalLease.GetPendingReceiptsOfRentalLease("1/1/1800", "1/1/3300", "", "", "", "")

            myReport = New crptPendingReceiptsOfRentalLease

            objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")

            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            ds.Clear()
            da.Fill(ds, rpt)
            da.Fill(ds, objsearch)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        End If
        If lstReminders.Items(lstReminders.SelectedIndex).ToString.Contains("Re-Order-Level-Items") Then 'Added By Prashant 10-Oct-2014 ALL10102014
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myreport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim ObjSearch As rptSearchingCriteria
            Dim rpt As rptReOrderLevelItem
            Dim ds As New dsReOrderLevel
            myreport = New crptReOrderLevelItem

            rpt = rptReOrderLevelItem.GetMinReOrderItem("", "", "", "", Guid.Empty, False,
                                                        ClientCode:=AppSettings("ClientCode"))

            ObjSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", "", "", "", "", "", "", "", "", "", "", 0, "", "", "", AppSettings("Logo"), Search10:=AppSettings("ClientCode"))

            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, rpt)
            da.Fill(ds, ObjSearch)
            da.Fill(ds, mrptImage)
            myreport.SetDataSource(ds)
            Session("CrystalReport") = myreport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        End If

        If lstReminders.Items(lstReminders.SelectedIndex).ToString = IIf(AppSettings("MELSnagNomenclature") = "True", "Open ADD/Defect(s)", "Open MEL/Snag(s)") Then
            Dim mMELSnagCorrectiveActionRegisterReport As MELSnagCorrectiveActionRegisterReport
            Dim MachineID As String = "{00000000-0000-0000-0000-000000000000}"
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim mCompanyDetail As New CompanyDetail

            Dim ReportName As String

            mMELSnagCorrectiveActionRegisterReport = MELSnagCorrectiveActionRegisterReport.GetMELSnagCorrectiveActionRegisterReport(, , MachineID, 1)
            If mMELSnagCorrectiveActionRegisterReport.Count = 0 Then 'Added By Vikrant On 28-Mar-2014 For ALL01042014
                myReport = New crptBlankDefectReport
            Else
                If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                    myReport = New crptDefectReportBA 'Added by Saylee on 18-Feb-2014 for BA18022014
                Else
                    If AppSettings("ClientCode") = "GEP" Then
                        myReport = New crptMELSnagCorrectiveActionRegisterReportForGEP
                    Else
                        myReport = New crptMELSnagCorrectiveActionRegisterReport
                    End If

                    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1180)
                End If
            End If
            ReportName = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Register", "MEL Register")

            Dim ReportData As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
              mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
              mCompanyDetail.WebSite, ReportName, "--", New SmartDate(Today.Date.ToString).FormattedText, "", "Open", "", ProductVersion:=("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:="All", SearchStr10:=AppSettings("Logo"), SearchStr13:=AppSettings("MELSnagNomenclature").ToString)

            Dim ds As New dsMELSnagCorrectiveActionRegisterReport
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mMELSnagCorrectiveActionRegisterReport)
            da.Fill(ds, mrptImage)
            da.Fill(ds, ReportData)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        End If
        If lstReminders.Items(lstReminders.SelectedIndex).Value.ToString.Contains("Due For Calibration Item") Then
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim da As New CSLA.Data.ObjectAdapter
            Dim ds As New dsCalibration
            Dim Obj As rptDueCalibrationList
            'Dim objsearch As rptSearchingCriteria


            Dim mCompanyDetail As New CompanyDetail
            Dim Str1 As String = "As On Date : " & New SmartDate(Today.Date.ToString).FormattedText
            'SetValues()

            Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
            mCompanyDetail.WebSite, "Calibration Due Report", "", Str1, "", "", "", AppSettings("Product Version"), AppSettings("SINote"))

            Obj = rptDueCalibrationList.GetrptDueCalibrationList(, Today.Date, , , , , New SmartDate(Today.Date.ToString).Date.AddMonths(1).ToShortDateString)
            'objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", ToDate, PartNo, "", "", "", "", "", "", "", Description, "", , , )

            If Obj.Count <= 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
                msg1.ReplacePage = "Dashboard.aspx?Backpage="
                msg1.Show()
                Exit Sub
            End If
            ds.Clear()
            da.Fill(ds, Obj)
            'da.Fill(ds, objsearch)
            da.Fill(ds, Report)
            '************************Report Show ***************************

            ''myReport = New crDueCalibration

            If AppSettings("ClientCode") = "BA" Then 'Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then 'Added by Vikrant on 09-Feb-2015 For ALL09022015
                myReport = New crDueCalibrationBA
            Else
                myReport = New crDueCalibration
            End If

            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            'Dim Str As String
            'Str = "<script language=Javascript>openTranDetail();</script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        If Session("MiddleFrame") = "" Then
            RemoveSession()
            Response.Redirect("index.aspx")
        End If

    End Sub
#End Region

#Region "Variable Declaration for Method"
    Public mTransTypeList As TransactionList
#End Region

#Region "Reports Business Methods"
    Private Function GetTitle() As String           'New Addition
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String = mTransTypeList.GetTransactionTypeName(Util.Trans.DisacrdPart).ToString + " Register"
        If mTitle = "" Then
            Return "Goods Outward Note Register (Summary Report)"
        Else
            Return mTitle + " (Summary Report)" '"Goods Outward Note Register (Summary Report)"
        End If
    End Function
    Private Function GetTitle1() As String           'New Addition
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String = mTransTypeList.GetTransactionTypeName(Util.Trans.LoanReturnToStore).ToString + " Register"
        If mTitle = "" Then
            Return "Goods Outward Note Register (Summary Report)"
        Else
            Return mTitle + " (Summary Report)" '"Goods Outward Note Register (Summary Report)"
        End If
    End Function
    Private Function GetTitle2() As String           'New Addition
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String = mTransTypeList.GetTransactionTypeName(Util.Trans.LoanIssuedToAircraft).ToString + " Register"
        If mTitle = "" Then
            Return "Goods Outward Note Register (Summary Report)"
        Else
            Return mTitle + " (Summary Report)" '"Goods Outward Note Register (Summary Report)"
        End If
    End Function
    Private Function GetTitle3() As String           'New Addition
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String = mTransTypeList.GetTransactionTypeName(Util.Trans.LoanIssueToStore).ToString + " Register"
        If mTitle = "" Then
            Return "Goods Outward Note Register (Summary Report)"
        Else
            Return mTitle + " (Summary Report)" '"Goods Outward Note Register (Summary Report)"
        End If
    End Function
    Private Function GetTitle4() As String           'New Addition
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String = mTransTypeList.GetTransactionTypeName(Util.Trans.ExchangeRepairIssueToVendor).ToString + " Register"
        If mTitle = "" Then
            Return "Goods Outward Note Register (Summary Report)"
        Else
            Return mTitle + " (Summary Report)" '"Goods Outward Note Register (Summary Report)"
        End If
    End Function
    Private Function GetTitle5() As String           'New Addition
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String = mTransTypeList.GetTransactionTypeName(Util.Trans.IssueToStore).ToString + " Register"
        If mTitle = "" Then
            Return "Goods Outward Note Register (Summary Report)"
        Else
            Return mTitle + " (Summary Report)" '"Goods Outward Note Register (Summary Report)"
        End If
    End Function
    Private Function GetTitle6() As String           'New Addition
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String = mTransTypeList.GetTransactionTypeName(Util.Trans.IssueToAircraft).ToString + " Register"
        If mTitle = "" Then
            Return "Goods Outward Note Register (Summary Report)"
        Else
            Return mTitle + " (Summary Report)" '"Goods Outward Note Register (Summary Report)"
        End If
    End Function
    Private Function GetTitleVendor() As String
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String = mTransTypeList.GetTransactionTypeName(Util.Trans.ExchangeRepairReceivedFromVendor).ToString + " Register"

        If mTitle = "" Then
            Return "Goods Receipt Register (Summary Report)"
        Else
            Return mTitle + " (Summary Report)" '"Receipt-Cum-Invoice Register (Summary Report)"
        End If
    End Function
    Private Function GetTitleStore() As String
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String = mTransTypeList.GetTransactionTypeName(Util.Trans.ReceivedFromOtherStore).ToString + " Register"

        If mTitle = "" Then
            Return "Goods Receipt Register (Summary Report)"
        Else
            Return mTitle + " (Summary Report)" '"Receipt-Cum-Invoice Register (Summary Report)"
        End If
    End Function
    Private Function GetTitleAircraft() As String
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String = mTransTypeList.GetTransactionTypeName(Util.Trans.ReceivedFromAircraft).ToString + " Register"

        If mTitle = "" Then
            Return "Goods Receipt Register (Summary Report)"
        Else
            Return mTitle + " (Summary Report)" '"Receipt-Cum-Invoice Register (Summary Report)"
        End If
    End Function
    Private Function GetTitlePurchase() As String
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String = mTransTypeList.GetTransactionTypeName(Util.Trans.ReceiptcumInvoiceAgainstPuchaseOrder).ToString + " Register"

        If mTitle = "" Then
            Return "Goods Receipt Register (Summary Report)"
        Else
            Return mTitle + " (Summary Report)" '"Receipt-Cum-Invoice Register (Summary Report)"
        End If
    End Function
#End Region

End Class