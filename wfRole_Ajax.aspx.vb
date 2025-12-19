'*************************************
'Modified by Harsh Sugandhi on 6th January 2025 for FLYAPL-2107 => Add a new grid for Project module links in Role Manager.
'*************************************

Public Class wfRole_Ajax
    Inherits Page

#Region " Variable Declaration "

    Public mRoleList As RoleList
    Public mRole As Role
    Public mRoleID As Guid
    Dim EventLogID As Guid

#End Region

#Region " Business Methods "

    Private Sub GetSession()

        mRole = CType(Session("mRole"), Role)
        mRoleList = CType(Session("mRoleList"), RoleList)

    End Sub

    Private Sub SetSession()

        Session("mRole") = mRole
        Session("mRoleList") = mRoleList

    End Sub

    Private Sub RemoveSession()

        Session.Remove("mRole")
        Session.Remove("mRoleList")
        'Added By Vikrant On 19-Feb-2021 For ALL19022021
        Session.Remove("CreateCopy")
        Session.Remove("CopiedRoleName")
        'End

    End Sub

#Region " SetObject "

    Private Function SetObject() As Boolean

        mRole.Name = Trim(txtRoleName.Text)
        Dim j As Integer = 0

        Try

            While j < mRole.Inv_Master_Modules.Count

                Dim item As GridViewRow
                item = dgMasters.Rows(j)

                mRole.Inv_Master_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_Master_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_Master_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_Master_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_Master_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                j = j + 1

            End While

            j = 0
            While j < mRole.Inv_Requisition_Modules.Count

                Dim item As GridViewRow
                item = dgRequisition.Rows(j)

                mRole.Inv_Requisition_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_Requisition_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_Requisition_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_Requisition_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_Requisition_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Inv_Requisition_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Inv_PurchaseEnquiries_Modules.Count

                Dim item As GridViewRow
                item = dgPurchaseEnquiry.Rows(j)

                mRole.Inv_PurchaseEnquiries_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_PurchaseEnquiries_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_PurchaseEnquiries_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_PurchaseEnquiries_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_PurchaseEnquiries_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Inv_PurchaseEnquiries_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Inv_PurchaseQuotations_Modules.Count

                Dim item As GridViewRow
                item = dgPurchaseQuotation.Rows(j)

                mRole.Inv_PurchaseQuotations_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_PurchaseQuotations_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_PurchaseQuotations_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_PurchaseQuotations_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_PurchaseQuotations_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Inv_PurchaseQuotations_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Inv_PurchaseOrders_Modules.Count

                Dim item As GridViewRow
                item = dgPurchaseOrder.Rows(j)

                mRole.Inv_PurchaseOrders_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_PurchaseOrders_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_PurchaseOrders_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_PurchaseOrders_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_PurchaseOrders_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Inv_PurchaseOrders_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Inv_GoodsReceipts_Modules.Count

                Dim item As GridViewRow
                item = dgGoodsReceipt.Rows(j)

                mRole.Inv_GoodsReceipts_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_GoodsReceipts_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_GoodsReceipts_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_GoodsReceipts_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_GoodsReceipts_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Inv_GoodsReceipts_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Inv_GoodsIssues_Modules.Count

                Dim item As GridViewRow
                item = dgGoodsIssue.Rows(j)

                mRole.Inv_GoodsIssues_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_GoodsIssues_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_GoodsIssues_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_GoodsIssues_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_GoodsIssues_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Inv_GoodsIssues_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Inv_PurchaseInvoices_Modules.Count

                Dim item As GridViewRow
                item = dgPurchaseInvoice.Rows(j)

                mRole.Inv_PurchaseInvoices_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_PurchaseInvoices_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_PurchaseInvoices_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_PurchaseInvoices_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_PurchaseInvoices_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Inv_PurchaseInvoices_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Inv_SalesModules_Modules.Count

                Dim item As GridViewRow
                item = dgSalesModules.Rows(j)

                mRole.Inv_SalesModules_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_SalesModules_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_SalesModules_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_SalesModules_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_SalesModules_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Inv_SalesModules_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Inv_WorkOrder_Modules.Count

                Dim item As GridViewRow
                item = dgWorkOrder.Rows(j)

                mRole.Inv_WorkOrder_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_WorkOrder_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_WorkOrder_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_WorkOrder_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_WorkOrder_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Inv_WorkOrder_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked
                mRole.Inv_WorkOrder_Modules.Item(j).IsSelectedCompleted = CType(item.FindControl("chkCompleted"), CheckBox).Checked 'Added By Vikrant on 30-Jun-2021 For ALL30062021 

                j = j + 1

            End While

            j = 0
            While j < mRole.Maint_Master_Modules.Count

                Dim item As GridViewRow
                item = dgMaintMasters.Rows(j)

                mRole.Maint_Master_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Maint_Master_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Maint_Master_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Maint_Master_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Maint_Master_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Maint_Maintenance_Modules.Count

                Dim item As GridViewRow
                item = dgMaintenance.Rows(j)

                mRole.Maint_Maintenance_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Maint_Maintenance_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Maint_Maintenance_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Maint_Maintenance_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Maint_Maintenance_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Maint_Maintenance_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Calibration_Modules.Count

                Dim item As GridViewRow
                item = dgCalibration.Rows(j)

                mRole.Calibration_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Calibration_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Calibration_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Calibration_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Calibration_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Manual_Modules.Count

                Dim item As GridViewRow
                item = dgManual.Rows(j)

                mRole.Manual_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Manual_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Manual_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Manual_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Manual_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.QA_Modules.Count

                Dim item As GridViewRow
                item = dgAudit.Rows(j)

                mRole.QA_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.QA_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.QA_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.QA_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.QA_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.MEL_Modules.Count

                Dim item As GridViewRow
                item = dgMEL.Rows(j)

                mRole.MEL_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.MEL_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.MEL_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.MEL_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.MEL_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Discrepancy_Modules.Count

                Dim item As GridViewRow
                item = dgDiscrepancy.Rows(j)

                mRole.Discrepancy_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Discrepancy_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Discrepancy_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Discrepancy_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Discrepancy_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Inv_WorkInvoice_Modules.Count

                Dim item As GridViewRow
                item = dgWorkInvoice.Rows(j)

                mRole.Inv_WorkInvoice_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_WorkInvoice_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_WorkInvoice_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_WorkInvoice_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_WorkInvoice_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Inv_WorkInvoice_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1


            End While

            j = 0
            While j < mRole.Inv_Reliability_Modules.Count

                Dim item As GridViewRow
                item = dgReliability.Rows(j)

                mRole.Inv_Reliability_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_Reliability_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_Reliability_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_Reliability_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_Reliability_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While

            'Added BY VIkrant on 27-Aug-2012 For ALL27082012
            j = 0
            While j < mRole.Inv_New_Requisition_Modules.Count

                Dim item As GridViewRow
                item = dgNewRequisition.Rows(j)

                mRole.Inv_New_Requisition_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_New_Requisition_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_New_Requisition_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_New_Requisition_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_New_Requisition_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Inv_New_Requisition_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While
            'End

            j = 0
            While j < mRole.Tool_Modules.Count

                Dim item As GridViewRow
                item = dgTools.Rows(j)
                mRole.Tool_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Inv_Reports_Modules.Count

                Dim item As GridViewRow
                item = dgInventoryReports.Rows(j)

                mRole.Inv_Reports_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Maint_Reports_Modules.Count

                Dim item As GridViewRow
                item = dgMaintenanceReports.Rows(j)

                mRole.Maint_Reports_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Inv_LineMaintenance_Modules.Count 'Added by Prashant 15-Nov-2012 ALL12112012

                Dim item As GridViewRow
                item = dgLineMaintenance.Rows(j)

                mRole.Inv_LineMaintenance_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_LineMaintenance_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_LineMaintenance_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_LineMaintenance_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_LineMaintenance_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Inv_LineMaintenance_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.Inv_ExportInvoice_Modules.Count 'Added By Shweta 03-April-2013 For All03042013-1

                Dim item As GridViewRow
                item = dgExportInvoice.Rows(j)

                mRole.Inv_ExportInvoice_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_ExportInvoice_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_ExportInvoice_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_ExportInvoice_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_ExportInvoice_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Inv_ExportInvoice_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            'Added By Vikrant For MPD
            j = 0
            While j < mRole.Maint_MPD_Modules.Count

                Dim item As GridViewRow
                item = dgMPD.Rows(j)

                mRole.Maint_MPD_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Maint_MPD_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Maint_MPD_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Maint_MPD_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Maint_MPD_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While
            'End

            'Added By Saylee For CWP
            j = 0
            While j < mRole.Maint_CWP_Modules.Count

                Dim item As GridViewRow
                item = dgCWP.Rows(j)

                mRole.Maint_CWP_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Maint_CWP_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Maint_CWP_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Maint_CWP_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Maint_CWP_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While
            'End

            'D&BChart
            j = 0
            While j < mRole.DentBuckleChart_Modules.Count

                Dim item As GridViewRow
                item = dgDentBuckleChart.Rows(j)

                mRole.DentBuckleChart_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.DentBuckleChart_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.DentBuckleChart_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.DentBuckleChart_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.DentBuckleChart_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.DentBuckleChart_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While
            'End

            'Hangar Planning
            j = 0
            While j < mRole.Hangar_Modules.Count

                Dim item As GridViewRow
                item = dgHangar.Rows(j)

                mRole.Hangar_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Hangar_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Hangar_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Hangar_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Hangar_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Hangar_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While
            'End

            'Company Document Added by Vikrant On 12-Oct-2021 For ALL12102021
            j = 0
            While j < mRole.CompanyDocument_Modules.Count

                Dim item As GridViewRow
                item = dgCompanyDocument.Rows(j)

                mRole.CompanyDocument_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.CompanyDocument_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.CompanyDocument_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.CompanyDocument_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.CompanyDocument_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.CompanyDocument_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While
            'End

            'Payment Advice Added by Shital on 28-Jan-2018
            j = 0
            While j < mRole.PaymentAdvice_Modules.Count

                Dim item As GridViewRow
                item = dgPaymentAdvice.Rows(j)

                mRole.PaymentAdvice_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.PaymentAdvice_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.PaymentAdvice_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.PaymentAdvice_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.PaymentAdvice_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.PaymentAdvice_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While
            'End

            j = 0
            While j < mRole.InfoDisplay_Modules.Count

                Dim item As GridViewRow
                item = dgInfoDisplay.Rows(j)

                mRole.InfoDisplay_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.InfoDisplay_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.InfoDisplay_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.InfoDisplay_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.InfoDisplay_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.InfoDisplay_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            'Maintenanace DashBoard
            j = 0
            While j < mRole.MaintenanceDashboard_Modules.Count

                Dim item As GridViewRow
                item = dgMaintenanceDashboard.Rows(j)

                mRole.MaintenanceDashboard_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                j = j + 1

            End While

            'Inventory DashBoard
            j = 0
            While j < mRole.InventoryDashboard_Modules.Count

                Dim item As GridViewRow
                item = dgInventoryDashboard.Rows(j)

                mRole.InventoryDashboard_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked

                j = j + 1

            End While

            'Spare Maint Added by Saylee on  18-Aug-2020, LockDown 4.0

            j = 0
            While j < mRole.SpareMaint_Maintenance_Modules.Count

                Dim item As GridViewRow
                item = dgSpareMaint.Rows(j)

                mRole.SpareMaint_Maintenance_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.SpareMaint_Maintenance_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.SpareMaint_Maintenance_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.SpareMaint_Maintenance_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.SpareMaint_Maintenance_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.SpareMaint_Maintenance_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked
                mRole.SpareMaint_Maintenance_Modules.Item(j).IsSelectedCompleted = CType(item.FindControl("chkCompleted"), CheckBox).Checked 'Added By Vikrant on 30-Jun-2021 For ALL30062021 

                j = j + 1

            End While

            'Component ReservationAdded by Shital On 29-Nov-2021
            j = 0
            While j < mRole.ComponentReservation_Modules.Count

                Dim item As GridViewRow
                item = dgComponentReservation.Rows(j)

                mRole.ComponentReservation_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.ComponentReservation_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.ComponentReservation_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.ComponentReservation_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.ComponentReservation_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While
            'End

            j = 0
            While j < mRole.DocumentLocker_Modules.Count

                Dim item As GridViewRow
                item = dgDocumentLocker.Rows(j)

                mRole.DocumentLocker_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.DocumentLocker_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.DocumentLocker_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.DocumentLocker_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.DocumentLocker_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.AdminUtilities_Modules.Count

                Dim item As GridViewRow
                item = dgAdminUtilitiess.Rows(j)

                mRole.AdminUtilities_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked

                j = j + 1

            End While

            j = 0
            While j < mRole.MROContract_Modules.Count

                Dim item As GridViewRow
                item = dgMROContract.Rows(j)

                mRole.MROContract_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.MROContract_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.MROContract_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.MROContract_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.MROContract_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.MROContract_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            'WO Invoice
            j = 0
            While j < mRole.Inv_nWOInvoice_Modules.Count

                Dim item As GridViewRow
                item = dgWorkOrderInvoice.Rows(j)

                mRole.Inv_nWOInvoice_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_nWOInvoice_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_nWOInvoice_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_nWOInvoice_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_nWOInvoice_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Inv_nWOInvoice_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            'MSP [Ajay 26-04-2023]
            j = 0
            While j < mRole.Inv_nMSP_Modules.Count

                Dim item As GridViewRow
                item = dgMSP.Rows(j)

                mRole.Inv_nMSP_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Inv_nMSP_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Inv_nMSP_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Inv_nMSP_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Inv_nMSP_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While

            'WO Invoice
            j = 0
            While j < mRole.ADSBReviewMeeting_Modules.Count

                Dim item As GridViewRow
                item = dgADSBReviewMeeting.Rows(j)

                mRole.ADSBReviewMeeting_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.ADSBReviewMeeting_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.ADSBReviewMeeting_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.ADSBReviewMeeting_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.ADSBReviewMeeting_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.ADSBReviewMeeting_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            'EmpCAAuthorization_Modules
            j = 0
            While j < mRole.EmpCAAuthorization_Modules.Count

                Dim item As GridViewRow
                item = dgEmpCAAuthorization.Rows(j)

                mRole.EmpCAAuthorization_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.EmpCAAuthorization_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.EmpCAAuthorization_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.EmpCAAuthorization_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.EmpCAAuthorization_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While

            'DueJobPlanning_Modules
            j = 0
            While j < mRole.DueJobPlanning_Modules.Count

                Dim item As GridViewRow
                item = dgDueJobPlanning.Rows(j)

                mRole.DueJobPlanning_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.DueJobPlanning_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.DueJobPlanning_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.DueJobPlanning_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.DueJobPlanning_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While

            'Project
            j = 0
            While j < mRole.Project_Modules.Count

                Dim item As GridViewRow
                item = GV_Project.Rows(j)

                mRole.Project_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.Project_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.Project_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.Project_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.Project_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked
                mRole.Project_Modules.Item(j).IsSelectedAuthorized = CType(item.FindControl("chkAuthorized"), CheckBox).Checked

                j = j + 1

            End While

            'Sankalp 29/7/25
            'Cabin Defect
            j = 0
            While j < mRole.CabinDefect_Modules.Count

                Dim item As GridViewRow
                item = dgCabinDefect.Rows(j)

                mRole.CabinDefect_Modules.Item(j).IsSelectedView = CType(item.FindControl("chkView"), CheckBox).Checked
                mRole.CabinDefect_Modules.Item(j).IsSelectedPrint = CType(item.FindControl("chkPrint"), CheckBox).Checked
                mRole.CabinDefect_Modules.Item(j).IsSelectedNew = CType(item.FindControl("chkAdd"), CheckBox).Checked
                mRole.CabinDefect_Modules.Item(j).IsSelectedEdit = CType(item.FindControl("chkEdit"), CheckBox).Checked
                mRole.CabinDefect_Modules.Item(j).IsSelectedDelete = CType(item.FindControl("chkDelete"), CheckBox).Checked

                j = j + 1

            End While

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Function

#End Region

#Region " CheckChecked "
    Private Function CheckCheckedMasters() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_Master_Modules.Count
            Dim item As GridViewRow
            item = dgMasters.Rows(j)
            If mRole.Inv_Master_Modules.Item(j).IsSelectedView = True Or mRole.Inv_Master_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_Master_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_Master_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_Master_Modules.Item(j).IsSelectedDelete = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedRequisitions() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_Requisition_Modules.Count
            Dim item As GridViewRow
            item = dgRequisition.Rows(j)
            If mRole.Inv_Requisition_Modules.Item(j).IsSelectedView = True Or mRole.Inv_Requisition_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_Requisition_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_Requisition_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_Requisition_Modules.Item(j).IsSelectedDelete = True Or mRole.Inv_Requisition_Modules.Item(j).IsSelectedAuthorized = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedPurchaseEnquiries() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_PurchaseEnquiries_Modules.Count
            Dim item As GridViewRow
            item = dgPurchaseEnquiry.Rows(j)
            If mRole.Inv_PurchaseEnquiries_Modules.Item(j).IsSelectedView = True Or mRole.Inv_PurchaseEnquiries_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_PurchaseEnquiries_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_PurchaseEnquiries_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_PurchaseEnquiries_Modules.Item(j).IsSelectedDelete = True Or mRole.Inv_PurchaseEnquiries_Modules.Item(j).IsSelectedAuthorized = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedPurchaseQuotations() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_PurchaseQuotations_Modules.Count
            Dim item As GridViewRow
            item = dgPurchaseQuotation.Rows(j)
            If mRole.Inv_PurchaseQuotations_Modules.Item(j).IsSelectedView = True Or mRole.Inv_PurchaseQuotations_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_PurchaseQuotations_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_PurchaseQuotations_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_PurchaseQuotations_Modules.Item(j).IsSelectedDelete = True Or mRole.Inv_PurchaseQuotations_Modules.Item(j).IsSelectedAuthorized = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedPurchaseOrders() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_PurchaseOrders_Modules.Count
            Dim item As GridViewRow
            item = dgPurchaseOrder.Rows(j)
            If mRole.Inv_PurchaseOrders_Modules.Item(j).IsSelectedView = True Or mRole.Inv_PurchaseOrders_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_PurchaseOrders_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_PurchaseOrders_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_PurchaseOrders_Modules.Item(j).IsSelectedDelete = True Or mRole.Inv_PurchaseOrders_Modules.Item(j).IsSelectedAuthorized = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedGoodsReceipts() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_GoodsReceipts_Modules.Count
            Dim item As GridViewRow
            item = dgGoodsReceipt.Rows(j)
            If mRole.Inv_GoodsReceipts_Modules.Item(j).IsSelectedView = True Or mRole.Inv_GoodsReceipts_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_GoodsReceipts_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_GoodsReceipts_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_GoodsReceipts_Modules.Item(j).IsSelectedDelete = True Or mRole.Inv_GoodsReceipts_Modules.Item(j).IsSelectedAuthorized = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedGoodsIssues() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_GoodsIssues_Modules.Count
            Dim item As GridViewRow
            item = dgGoodsIssue.Rows(j)
            If mRole.Inv_GoodsIssues_Modules.Item(j).IsSelectedView = True Or mRole.Inv_GoodsIssues_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_GoodsIssues_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_GoodsIssues_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_GoodsIssues_Modules.Item(j).IsSelectedDelete = True Or mRole.Inv_GoodsIssues_Modules.Item(j).IsSelectedAuthorized = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedPurchaseInvoices() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_PurchaseInvoices_Modules.Count
            Dim item As GridViewRow
            item = dgPurchaseInvoice.Rows(j)
            If mRole.Inv_PurchaseInvoices_Modules.Item(j).IsSelectedView = True Or mRole.Inv_PurchaseInvoices_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_PurchaseInvoices_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_PurchaseInvoices_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_PurchaseInvoices_Modules.Item(j).IsSelectedDelete = True Or mRole.Inv_PurchaseInvoices_Modules.Item(j).IsSelectedAuthorized = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedSalesModules() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_SalesModules_Modules.Count
            Dim item As GridViewRow
            item = dgSalesModules.Rows(j)
            If mRole.Inv_SalesModules_Modules.Item(j).IsSelectedView = True Or mRole.Inv_SalesModules_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_SalesModules_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_SalesModules_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_SalesModules_Modules.Item(j).IsSelectedDelete = True Or mRole.Inv_SalesModules_Modules.Item(j).IsSelectedAuthorized = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedMaintMasters() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Maint_Master_Modules.Count
            Dim item As GridViewRow
            item = dgMaintMasters.Rows(j)
            If mRole.Maint_Master_Modules.Item(j).IsSelectedView = True Or mRole.Maint_Master_Modules.Item(j).IsSelectedPrint = True Or mRole.Maint_Master_Modules.Item(j).IsSelectedNew = True Or mRole.Maint_Master_Modules.Item(j).IsSelectedEdit = True Or mRole.Maint_Master_Modules.Item(j).IsSelectedDelete = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedMaintenance() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Maint_Maintenance_Modules.Count
            Dim item As GridViewRow
            item = dgMaintenance.Rows(j)
            If mRole.Maint_Maintenance_Modules.Item(j).IsSelectedView = True Or mRole.Maint_Maintenance_Modules.Item(j).IsSelectedPrint = True Or mRole.Maint_Maintenance_Modules.Item(j).IsSelectedNew = True Or mRole.Maint_Maintenance_Modules.Item(j).IsSelectedEdit = True Or mRole.Maint_Maintenance_Modules.Item(j).IsSelectedDelete = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedTool() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Tool_Modules.Count
            Dim item As GridViewRow
            item = dgTools.Rows(j)
            If mRole.Tool_Modules.Item(j).IsSelectedView = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedAdminUtilities() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.AdminUtilities_Modules.Count
            Dim item As GridViewRow
            item = dgAdminUtilitiess.Rows(j)
            If mRole.AdminUtilities_Modules.Item(j).IsSelectedView = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckForAdministrator() As Boolean ''''Added by Prashant 6-Sep-2013 ALL06092013
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Tool_Modules.Count
            If ((mRole.Tool_Modules.Item(j).ModuleID = 1003 Or mRole.Tool_Modules.Item(j).ModuleID = 1004) And (mRole.Tool_Modules.Item(j).IsSelectedView = False)) Then
                Return False
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return True
    End Function '''End
    Private Function CheckCheckedManual() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Manual_Modules.Count
            Dim item As GridViewRow
            item = dgManual.Rows(j)
            If mRole.Manual_Modules.Item(j).IsSelectedView = True Or mRole.Manual_Modules.Item(j).IsSelectedPrint = True Or mRole.Manual_Modules.Item(j).IsSelectedNew = True Or mRole.Manual_Modules.Item(j).IsSelectedEdit = True Or mRole.Manual_Modules.Item(j).IsSelectedDelete = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedCalibration() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Calibration_Modules.Count
            Dim item As GridViewRow
            item = dgCalibration.Rows(j)
            If mRole.Calibration_Modules.Item(j).IsSelectedView = True Or mRole.Calibration_Modules.Item(j).IsSelectedPrint = True Or mRole.Calibration_Modules.Item(j).IsSelectedNew = True Or mRole.Calibration_Modules.Item(j).IsSelectedEdit = True Or mRole.Calibration_Modules.Item(j).IsSelectedDelete = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedQA() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.QA_Modules.Count
            Dim item As GridViewRow
            item = dgAudit.Rows(j)
            If mRole.QA_Modules.Item(j).IsSelectedView = True Or mRole.QA_Modules.Item(j).IsSelectedPrint = True Or mRole.QA_Modules.Item(j).IsSelectedNew = True Or mRole.QA_Modules.Item(j).IsSelectedEdit = True Or mRole.QA_Modules.Item(j).IsSelectedDelete = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedMEL() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.MEL_Modules.Count
            Dim item As GridViewRow
            item = dgMEL.Rows(j)
            If mRole.MEL_Modules.Item(j).IsSelectedView = True Or mRole.MEL_Modules.Item(j).IsSelectedPrint = True Or mRole.MEL_Modules.Item(j).IsSelectedNew = True Or mRole.MEL_Modules.Item(j).IsSelectedEdit = True Or mRole.MEL_Modules.Item(j).IsSelectedDelete = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function

    Private Function CheckCheckedDiscrepancy() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Discrepancy_Modules.Count
            Dim item As GridViewRow
            item = dgDiscrepancy.Rows(j)
            If mRole.Discrepancy_Modules.Item(j).IsSelectedView = True Or mRole.Discrepancy_Modules.Item(j).IsSelectedPrint = True Or mRole.Discrepancy_Modules.Item(j).IsSelectedNew = True Or mRole.Discrepancy_Modules.Item(j).IsSelectedEdit = True Or mRole.Discrepancy_Modules.Item(j).IsSelectedDelete = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedMaintenanceReports() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Maint_Reports_Modules.Count
            Dim item As GridViewRow
            item = dgMaintenanceReports.Rows(j)
            If mRole.Maint_Reports_Modules.Item(j).IsSelectedView = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedWO() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_WorkOrder_Modules.Count
            Dim item As GridViewRow
            item = dgWorkOrder.Rows(j)
            If mRole.Inv_WorkOrder_Modules.Item(j).IsSelectedView = True Or mRole.Inv_WorkOrder_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_WorkOrder_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_WorkOrder_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_WorkOrder_Modules.Item(j).IsSelectedDelete = True Or mRole.Inv_WorkOrder_Modules.Item(j).IsSelectedAuthorized = True Or mRole.Inv_WorkOrder_Modules.Item(j).IsSelectedCompleted = True Then 'IsSelectedCompleted Added By Vikrant on 30-Jun-2021 For ALL30062021 
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedWOInvoice() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_nWOInvoice_Modules.Count
            Dim item As GridViewRow
            item = dgWorkOrderInvoice.Rows(j)
            If mRole.Inv_nWOInvoice_Modules.Item(j).IsSelectedView = True Or mRole.Inv_nWOInvoice_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_nWOInvoice_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_nWOInvoice_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_nWOInvoice_Modules.Item(j).IsSelectedDelete = True Or mRole.Inv_nWOInvoice_Modules.Item(j).IsSelectedAuthorized = True Or mRole.Inv_nWOInvoice_Modules.Item(j).IsSelectedCompleted = True Then 'IsSelectedCompleted Added By Vikrant on 30-Jun-2021 For ALL30062021 
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    '======= Ajay 26-04-2023 =========
    Private Function CheckCheckedMSP() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_nMSP_Modules.Count
            Dim item As GridViewRow
            item = dgMSP.Rows(j)
            If mRole.Inv_nMSP_Modules.Item(j).IsSelectedView = True Or mRole.Inv_nMSP_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_nMSP_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_nMSP_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_nMSP_Modules.Item(j).IsSelectedDelete = True Or mRole.Inv_nMSP_Modules.Item(j).IsSelectedAuthorized = True Or mRole.Inv_nMSP_Modules.Item(j).IsSelectedCompleted = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedWorkInvoice() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_WorkInvoice_Modules.Count
            Dim item As GridViewRow
            item = dgWorkInvoice.Rows(j)
            If mRole.Inv_WorkInvoice_Modules.Item(j).IsSelectedView = True Or mRole.Inv_WorkInvoice_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_WorkInvoice_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_WorkInvoice_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_WorkInvoice_Modules.Item(j).IsSelectedDelete = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedReliability() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_Reliability_Modules.Count
            Dim item As GridViewRow
            item = dgReliability.Rows(j)
            If mRole.Inv_Reliability_Modules.Item(j).IsSelectedView = True Or mRole.Inv_Reliability_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_Reliability_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_Reliability_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_Reliability_Modules.Item(j).IsSelectedDelete = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedNewRequisitions() As Boolean 'Added BY VIkrant on 27-Aug-2012 For ALL27082012
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_New_Requisition_Modules.Count
            Dim item As GridViewRow
            item = dgNewRequisition.Rows(j)
            If mRole.Inv_New_Requisition_Modules.Item(j).IsSelectedView = True Or mRole.Inv_New_Requisition_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_New_Requisition_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_New_Requisition_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_New_Requisition_Modules.Item(j).IsSelectedDelete = True Or mRole.Inv_New_Requisition_Modules.Item(j).IsSelectedAuthorized = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function 'End
    Private Function CheckCheckedLineMaintenance() As Boolean 'Added by Prashant 15-Nov-2012 ALL12112012
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_LineMaintenance_Modules.Count
            Dim item As GridViewRow
            item = dgLineMaintenance.Rows(j)
            If mRole.Inv_LineMaintenance_Modules.Item(j).IsSelectedView = True Or mRole.Inv_LineMaintenance_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_LineMaintenance_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_LineMaintenance_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_LineMaintenance_Modules.Item(j).IsSelectedDelete = True Or mRole.Inv_LineMaintenance_Modules.Item(j).IsSelectedAuthorized = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedExportInvoice() As Boolean 'Added By Shweta 03-April-2013 For All03042013-1
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_ExportInvoice_Modules.Count
            Dim item As GridViewRow
            item = dgExportInvoice.Rows(j)
            If mRole.Inv_ExportInvoice_Modules.Item(j).IsSelectedView = True Or mRole.Inv_ExportInvoice_Modules.Item(j).IsSelectedPrint = True Or mRole.Inv_ExportInvoice_Modules.Item(j).IsSelectedNew = True Or mRole.Inv_ExportInvoice_Modules.Item(j).IsSelectedEdit = True Or mRole.Inv_ExportInvoice_Modules.Item(j).IsSelectedDelete = True Or mRole.Inv_ExportInvoice_Modules.Item(j).IsSelectedAuthorized = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedInventoryReports() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Inv_Reports_Modules.Count
            Dim item As GridViewRow
            item = dgInventoryReports.Rows(j)
            If mRole.Inv_Reports_Modules.Item(j).IsSelectedView = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedMPD() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Maint_MPD_Modules.Count
            Dim item As GridViewRow
            item = dgMPD.Rows(j)
            If mRole.Maint_MPD_Modules.Item(j).IsSelectedView = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedCWP() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Maint_CWP_Modules.Count
            Dim item As GridViewRow
            item = dgCWP.Rows(j)
            If mRole.Maint_CWP_Modules.Item(j).IsSelectedView = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    'D&BChart
    Private Function CheckCheckedDentBuckleChart() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.DentBuckleChart_Modules.Count
            Dim item As GridViewRow
            item = dgDentBuckleChart.Rows(j)
            If mRole.DentBuckleChart_Modules.Item(j).IsSelectedView = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    'End
    'Hangar Planning
    Private Function CheckCheckedHangar() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.Hangar_Modules.Count
            Dim item As GridViewRow
            item = dgHangar.Rows(j)
            If mRole.Hangar_Modules.Item(j).IsSelectedView = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    'Company Document Added by Vikrant On 12-Oct-2021 For ALL12102021
    Private Function CheckCheckedCompanyDocument() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.CompanyDocument_Modules.Count
            Dim item As GridViewRow
            item = dgCompanyDocument.Rows(j)
            If mRole.CompanyDocument_Modules.Item(j).IsSelectedView = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    'End
    'Company Document Added by Shital On 29-Nov-2021
    Private Function CheckCheckedComponentReservation() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.ComponentReservation_Modules.Count
            Dim item As GridViewRow
            item = dgComponentReservation.Rows(j)
            If mRole.ComponentReservation_Modules.Item(j).IsSelectedView = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    'End
    'Payment Advice 
    Private Function CheckCheckedPaymentAdvice() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.PaymentAdvice_Modules.Count
            Dim item As GridViewRow
            item = dgPaymentAdvice.Rows(j)
            If mRole.PaymentAdvice_Modules.Item(j).IsSelectedView = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    'End

    Private Function CheckCheckedInfoDisplay() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.InfoDisplay_Modules.Count
            Dim item As GridViewRow
            item = dgInfoDisplay.Rows(j)
            If mRole.InfoDisplay_Modules.Item(j).IsSelectedView = True Or mRole.InfoDisplay_Modules.Item(j).IsSelectedPrint = True Or mRole.InfoDisplay_Modules.Item(j).IsSelectedNew = True Or mRole.InfoDisplay_Modules.Item(j).IsSelectedEdit = True Or mRole.InfoDisplay_Modules.Item(j).IsSelectedDelete = True Or mRole.InfoDisplay_Modules.Item(j).IsSelectedAuthorized = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedMaintenanceDashboard() As Boolean 'Added by Saylee on 22-May-2020, Lockdown 3.0
        SetObject()
        Dim j As Integer = 0
        While j < mRole.MaintenanceDashboard_Modules.Count
            Dim item As GridViewRow
            item = dgMaintenanceDashboard.Rows(j)
            If mRole.MaintenanceDashboard_Modules.Item(j).IsSelectedView = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedInventoryDashboard() As Boolean 'Added by Saylee on 22-May-2020, Lockdown 3.0
        SetObject()
        Dim j As Integer = 0
        While j < mRole.InventoryDashboard_Modules.Count
            Dim item As GridViewRow
            item = dgInventoryDashboard.Rows(j)
            If mRole.InventoryDashboard_Modules.Item(j).IsSelectedView = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    'Spare Maint Added by Saylee on  18-Aug-2020, LockDown 4.0
    Private Function CheckCheckedSpareMaint() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.SpareMaint_Maintenance_Modules.Count
            Dim item As GridViewRow
            item = dgSpareMaint.Rows(j)
            If mRole.SpareMaint_Maintenance_Modules.Item(j).IsSelectedView = True Or mRole.SpareMaint_Maintenance_Modules.Item(j).IsSelectedPrint = True Or mRole.SpareMaint_Maintenance_Modules.Item(j).IsSelectedNew = True Or mRole.SpareMaint_Maintenance_Modules.Item(j).IsSelectedEdit = True Or mRole.SpareMaint_Maintenance_Modules.Item(j).IsSelectedDelete = True Or mRole.SpareMaint_Maintenance_Modules.Item(j).IsSelectedAuthorized = True Or mRole.SpareMaint_Maintenance_Modules.Item(j).IsSelectedCompleted = True Then 'IsSelectedCompleted Added By Vikrant on 30-Jun-2021 For ALL30062021 
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedDocumentLocker() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.DocumentLocker_Modules.Count
            Dim item As GridViewRow
            item = dgDocumentLocker.Rows(j)
            If mRole.DocumentLocker_Modules.Item(j).IsSelectedView = True Or mRole.DocumentLocker_Modules.Item(j).IsSelectedPrint = True Or mRole.DocumentLocker_Modules.Item(j).IsSelectedNew = True Or mRole.DocumentLocker_Modules.Item(j).IsSelectedEdit = True Or mRole.DocumentLocker_Modules.Item(j).IsSelectedDelete = True Or mRole.DocumentLocker_Modules.Item(j).IsSelectedAuthorized = True Or mRole.DocumentLocker_Modules.Item(j).IsSelectedCompleted = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedMROContract() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.MROContract_Modules.Count
            Dim item As GridViewRow
            item = dgMROContract.Rows(j)
            If mRole.MROContract_Modules.Item(j).IsSelectedView = True Or mRole.MROContract_Modules.Item(j).IsSelectedPrint = True _
                Or mRole.MROContract_Modules.Item(j).IsSelectedNew = True Or mRole.MROContract_Modules.Item(j).IsSelectedEdit = True _
                Or mRole.MROContract_Modules.Item(j).IsSelectedDelete = True Or mRole.MROContract_Modules.Item(j).IsSelectedAuthorized = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedADSBReviewMeeting() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.ADSBReviewMeeting_Modules.Count
            Dim item As GridViewRow
            item = dgADSBReviewMeeting.Rows(j)
            If mRole.ADSBReviewMeeting_Modules.Item(j).IsSelectedView = True Or mRole.ADSBReviewMeeting_Modules.Item(j).IsSelectedPrint = True _
                Or mRole.ADSBReviewMeeting_Modules.Item(j).IsSelectedNew = True Or mRole.ADSBReviewMeeting_Modules.Item(j).IsSelectedEdit = True _
                Or mRole.ADSBReviewMeeting_Modules.Item(j).IsSelectedDelete = True Or mRole.ADSBReviewMeeting_Modules.Item(j).IsSelectedAuthorized = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedEmpCAAuthorization() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.EmpCAAuthorization_Modules.Count
            Dim item As GridViewRow
            item = dgEmpCAAuthorization.Rows(j)
            If mRole.EmpCAAuthorization_Modules.Item(j).IsSelectedView = True Or mRole.EmpCAAuthorization_Modules.Item(j).IsSelectedPrint = True Or mRole.EmpCAAuthorization_Modules.Item(j).IsSelectedNew = True Or mRole.EmpCAAuthorization_Modules.Item(j).IsSelectedEdit = True Or mRole.EmpCAAuthorization_Modules.Item(j).IsSelectedDelete = True Or mRole.EmpCAAuthorization_Modules.Item(j).IsSelectedAuthorized = True Or mRole.EmpCAAuthorization_Modules.Item(j).IsSelectedCompleted = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function
    Private Function CheckCheckedDueJobPlanning() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.DueJobPlanning_Modules.Count
            Dim item As GridViewRow
            item = dgDueJobPlanning.Rows(j)
            If mRole.DueJobPlanning_Modules.Item(j).IsSelectedView = True Or mRole.DueJobPlanning_Modules.Item(j).IsSelectedPrint = True Or mRole.DueJobPlanning_Modules.Item(j).IsSelectedNew = True Or mRole.DueJobPlanning_Modules.Item(j).IsSelectedEdit = True Or mRole.DueJobPlanning_Modules.Item(j).IsSelectedDelete = True Or mRole.DueJobPlanning_Modules.Item(j).IsSelectedAuthorized = True Or mRole.DueJobPlanning_Modules.Item(j).IsSelectedCompleted = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function

    'Sankalp Cabin Defect
    Private Function CheckCheckedCabinDefect() As Boolean
        SetObject()
        Dim j As Integer = 0
        While j < mRole.CabinDefect_Modules.Count
            Dim item As GridViewRow
            item = dgCabinDefect.Rows(j)
            If mRole.CabinDefect_Modules.Item(j).IsSelectedView = True Or
                mRole.CabinDefect_Modules.Item(j).IsSelectedPrint = True Or
                mRole.CabinDefect_Modules.Item(j).IsSelectedNew = True Or
                mRole.CabinDefect_Modules.Item(j).IsSelectedEdit = True Or
                mRole.CabinDefect_Modules.Item(j).IsSelectedDelete = True Or
                mRole.CabinDefect_Modules.Item(j).IsSelectedAuthorized = True Or
                mRole.CabinDefect_Modules.Item(j).IsSelectedCompleted = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If
        End While
        Return False
    End Function

    Private Function AtLeastOneProjectActionChecked() As Boolean

        Dim j As Integer = 0
        Try

            SetObject()

            While j < mRole.DueJobPlanning_Modules.Count

                Dim item As GridViewRow
                item = GV_Project.Rows(j)

                If mRole.Project_Modules.Item(j).IsSelectedView = True Or
                   mRole.Project_Modules.Item(j).IsSelectedPrint = True Or
                   mRole.Project_Modules.Item(j).IsSelectedNew = True Or
                   mRole.Project_Modules.Item(j).IsSelectedEdit = True Or
                   mRole.Project_Modules.Item(j).IsSelectedDelete = True Or
                   mRole.Project_Modules.Item(j).IsSelectedAuthorized = True Then

                    Return True
                    Exit Function

                Else
                    j = j + 1
                End If

            End While

            Return False

        Catch ex As Exception
            Throw ex
        End Try

    End Function


#End Region

    Private Overloads Sub SetFocus(cntrl As WebControl)

        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub

        Try

            Dim str As String
            str = "document.getElementById('" + cntrl.ClientID + "').focus();"

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "focusscript",
                                                str,
                                                True)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub MessageBoxResult()

        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "Delete" Then

                        Try

                            Session("sender") = ""
                            mRole = CType(Session("mRole"), Role)

                        Catch ex As SqlException

                            If ex.Number = 8145 Then

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

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                                MSGBox.Message_text.ReferenceDelete,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            End If

                            DataFieldBind()

                        End Try

                    End If

                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
                    DataFieldBind()
            End Select

        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then
            Session("sender") = ""
        End If

    End Sub

    Private Sub ConrolVisibility()

        Try

            If (Not AppSettings("ClientCode") Is Nothing) AndAlso
               (AppSettings("ClientCode") = "TAAL" Or
                AppSettings("ClientCode") = "GlobalJet") Then

                lblWorkOrder.Text = "Engineering Order"
            Else
                lblWorkOrder.Text = "Work Order"
            End If

            'Added By Vikrant on 29-Aug-2012 For 
            If AppSettings("NewRequisition") = "True" Then

                chkRequisition.Enabled = False
                dgRequisition.Enabled = False

            Else

                chkNewRequisition.Enabled = False
                dgNewRequisition.Enabled = False

            End If
            'End

            dgMEL.Columns(1).HeaderText = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD / Defect", "MEL / Snag") 'Added By Vikrant On 07-Sep-2020 For ALL07092020

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SetGrid()

        Dim chkView As CheckBox
        Dim chkPrint As CheckBox
        Dim chkAdd As CheckBox
        Dim chkEdit As CheckBox
        Dim chkDelete As CheckBox
        Dim chkAuthorized As CheckBox

        Try

            For j As Integer = 0 To dgRequisition.Rows.Count - 1

                If AppSettings("NewRequisition") = "True" Then

                    If (Me.dgRequisition.Rows.Item(j).Cells(0).Text = "New Requisition") Then

                        chkView = CType(dgNewRequisition.Rows.Item(j).Cells(1).FindControl("chkView"), CheckBox)
                        chkView.Enabled = True
                        chkPrint = CType(dgNewRequisition.Rows.Item(j).Cells(2).FindControl("chkPrint"), CheckBox)
                        chkPrint.Enabled = True
                        chkAdd = CType(dgNewRequisition.Rows.Item(j).Cells(3).FindControl("chkAdd"), CheckBox)
                        chkAdd.Enabled = True
                        chkEdit = CType(dgNewRequisition.Rows.Item(j).Cells(4).FindControl("chkEdit"), CheckBox)
                        chkEdit.Enabled = True
                        chkDelete = CType(dgNewRequisition.Rows.Item(j).Cells(5).FindControl("chkDelete"), CheckBox)
                        chkDelete.Enabled = True
                        chkAuthorized = CType(dgNewRequisition.Rows.Item(j).Cells(6).FindControl("chkAuthorized"), CheckBox)
                        chkAuthorized.Enabled = True

                    Else

                        chkView = CType(dgRequisition.Rows.Item(j).Cells(1).FindControl("chkView"), CheckBox)
                        chkView.Enabled = False
                        If chkView.Checked = True Then chkView.Checked = False
                        chkPrint = CType(dgRequisition.Rows.Item(j).Cells(2).FindControl("chkPrint"), CheckBox)
                        chkPrint.Enabled = False
                        If chkPrint.Checked = True Then chkPrint.Checked = False
                        chkAdd = CType(dgRequisition.Rows.Item(j).Cells(3).FindControl("chkAdd"), CheckBox)
                        chkAdd.Enabled = False
                        If chkAdd.Checked = True Then chkAdd.Checked = False
                        chkEdit = CType(dgRequisition.Rows.Item(j).Cells(4).FindControl("chkEdit"), CheckBox)
                        chkEdit.Enabled = False
                        If chkEdit.Checked = True Then chkEdit.Checked = False
                        chkDelete = CType(dgRequisition.Rows.Item(j).Cells(5).FindControl("chkDelete"), CheckBox)
                        chkDelete.Enabled = False
                        If chkDelete.Checked = True Then chkDelete.Checked = False
                        chkAuthorized = CType(dgRequisition.Rows.Item(j).Cells(6).FindControl("chkAuthorized"), CheckBox)
                        chkAuthorized.Enabled = False
                        If chkAuthorized.Checked = True Then chkAuthorized.Checked = False

                    End If

                Else

                    If (Me.dgRequisition.Rows.Item(j).Cells(0).Text = "New Requisition") Then

                        chkView = CType(dgRequisition.Rows.Item(j).Cells(1).FindControl("chkView"), CheckBox)
                        chkView.Enabled = False
                        If chkView.Checked = True Then chkView.Checked = False
                        chkPrint = CType(dgRequisition.Rows.Item(j).Cells(2).FindControl("chkPrint"), CheckBox)
                        chkPrint.Enabled = False
                        If chkPrint.Checked = True Then chkPrint.Checked = False
                        chkAdd = CType(dgRequisition.Rows.Item(j).Cells(3).FindControl("chkAdd"), CheckBox)
                        chkAdd.Enabled = False
                        If chkAdd.Checked = True Then chkAdd.Checked = False
                        chkEdit = CType(dgRequisition.Rows.Item(j).Cells(4).FindControl("chkEdit"), CheckBox)
                        chkEdit.Enabled = False
                        If chkEdit.Checked = True Then chkEdit.Checked = False
                        chkDelete = CType(dgRequisition.Rows.Item(j).Cells(5).FindControl("chkDelete"), CheckBox)
                        chkDelete.Enabled = False
                        If chkDelete.Checked = True Then chkDelete.Checked = False
                        chkAuthorized = CType(dgRequisition.Rows.Item(j).Cells(6).FindControl("chkAuthorized"), CheckBox)
                        chkAuthorized.Enabled = False
                        If chkAuthorized.Checked = True Then chkAuthorized.Checked = False

                    Else

                        chkView = CType(dgRequisition.Rows.Item(j).Cells(1).FindControl("chkView"), CheckBox)
                        chkView.Enabled = True
                        chkPrint = CType(dgRequisition.Rows.Item(j).Cells(2).FindControl("chkPrint"), CheckBox)
                        chkPrint.Enabled = True
                        chkAdd = CType(dgRequisition.Rows.Item(j).Cells(3).FindControl("chkAdd"), CheckBox)
                        chkAdd.Enabled = True
                        chkEdit = CType(dgRequisition.Rows.Item(j).Cells(4).FindControl("chkEdit"), CheckBox)
                        chkEdit.Enabled = True
                        chkDelete = CType(dgRequisition.Rows.Item(j).Cells(5).FindControl("chkDelete"), CheckBox)
                        chkDelete.Enabled = True
                        chkAuthorized = CType(dgRequisition.Rows.Item(j).Cells(6).FindControl("chkAuthorized"), CheckBox)
                        chkAuthorized.Enabled = True

                    End If

                End If

            Next

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    'Added By VIkrant On 29-Aug-2012 For ALL29082012
    Private Function IsNewRequisitionChecked() As Boolean
        If mRole.Inv_New_Requisition_Modules(0).IsSelectedNew Or mRole.Inv_New_Requisition_Modules(0).IsSelectedEdit Or mRole.Inv_New_Requisition_Modules(0).IsSelectedDelete Or
           mRole.Inv_New_Requisition_Modules(0).IsSelectedView Or mRole.Inv_New_Requisition_Modules(0).IsSelectedPrint Or mRole.Inv_New_Requisition_Modules(0).IsSelectedAuthorized Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function IsOldRequisitionChecked() As Boolean
        If mRole.Inv_Requisition_Modules(0).IsSelectedNew Or mRole.Inv_Requisition_Modules(0).IsSelectedEdit Or mRole.Inv_Requisition_Modules(0).IsSelectedDelete Or
           mRole.Inv_Requisition_Modules(0).IsSelectedView Or mRole.Inv_Requisition_Modules(0).IsSelectedPrint Or mRole.Inv_Requisition_Modules(0).IsSelectedAuthorized Then
            Return True
        Else
            Return False
        End If
    End Function
    'End

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()

        Try

            dgMasters.DataSource = mRole.Inv_Master_Modules
            dgRequisition.DataSource = mRole.Inv_Requisition_Modules
            dgPurchaseEnquiry.DataSource = mRole.Inv_PurchaseEnquiries_Modules
            dgPurchaseQuotation.DataSource = mRole.Inv_PurchaseQuotations_Modules
            dgPurchaseOrder.DataSource = mRole.Inv_PurchaseOrders_Modules
            dgGoodsReceipt.DataSource = mRole.Inv_GoodsReceipts_Modules
            dgGoodsIssue.DataSource = mRole.Inv_GoodsIssues_Modules
            dgPurchaseInvoice.DataSource = mRole.Inv_PurchaseInvoices_Modules
            dgSalesModules.DataSource = mRole.Inv_SalesModules_Modules
            dgWorkOrder.DataSource = mRole.Inv_WorkOrder_Modules

            If (Not AppSettings("ClientCode") Is Nothing) AndAlso
               (AppSettings("ClientCode") = "TAAL" Or
                AppSettings("ClientCode") = "GlobalJet") Then

                dgWorkOrder.Columns(1).HeaderText = "Engineering Order"
            Else
                dgWorkOrder.Columns(1).HeaderText = "Work Order"
            End If

            dgInventoryReports.DataSource = mRole.Inv_Reports_Modules
            'Maintenance
            dgMaintMasters.DataSource = mRole.Maint_Master_Modules
            dgMaintenance.DataSource = mRole.Maint_Maintenance_Modules
            dgMaintenanceReports.DataSource = mRole.Maint_Reports_Modules
            'Calibration
            dgCalibration.DataSource = mRole.Calibration_Modules
            'Manual
            dgManual.DataSource = mRole.Manual_Modules
            'Tools
            dgTools.DataSource = mRole.Tool_Modules
            'QA
            dgAudit.DataSource = mRole.QA_Modules
            'MEL
            dgMEL.DataSource = mRole.MEL_Modules
            'Discrepancy
            dgDiscrepancy.DataSource = mRole.Discrepancy_Modules
            'Work Invoice
            dgWorkInvoice.DataSource = mRole.Inv_WorkInvoice_Modules
            'Reliability
            dgReliability.DataSource = mRole.Inv_Reliability_Modules
            dgNewRequisition.DataSource = mRole.Inv_New_Requisition_Modules 'Added BY VIkrant on 27-Aug-2012 For ALL27082012
            'Line Maintenance
            dgLineMaintenance.DataSource = mRole.Inv_LineMaintenance_Modules
            'ExportInvoice
            dgExportInvoice.DataSource = mRole.Inv_ExportInvoice_Modules
            dgMPD.DataSource = mRole.Maint_MPD_Modules 'Added By Vikrant For MPD

            dgCWP.DataSource = mRole.Maint_CWP_Modules 'Added By Saylee For CWP
            dgDentBuckleChart.DataSource = mRole.DentBuckleChart_Modules 'D&BChart
            dgInfoDisplay.DataSource = mRole.InfoDisplay_Modules
            dgHangar.DataSource = mRole.Hangar_Modules 'Added by Abhishek For Hangar Planning 

            dgPaymentAdvice.DataSource = mRole.PaymentAdvice_Modules 'Added by Shital on 28-JAn-2018

            dgMaintenanceDashboard.DataSource = mRole.MaintenanceDashboard_Modules
            dgInventoryDashboard.DataSource = mRole.InventoryDashboard_Modules
            dgMEL.Columns(1).HeaderText = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD / Defect", "MEL / Snag") 'Added By Vikrant On 07-Sep-2020 For ALL07092020
            dgSpareMaint.DataSource = mRole.SpareMaint_Maintenance_Modules ''Spare Maint Added by Saylee on  18-Aug-2020, LockDown 4.0
            dgCompanyDocument.DataSource = mRole.CompanyDocument_Modules 'Company Document Added by Vikrant On 12-Oct-2021 For ALL12102021
            dgComponentReservation.DataSource = mRole.ComponentReservation_Modules 'Component Reservation Added by Shital On 29-Nov-2021
            dgDocumentLocker.DataSource = mRole.DocumentLocker_Modules
            dgAdminUtilitiess.DataSource = mRole.AdminUtilities_Modules

            dgWorkOrderInvoice.DataSource = mRole.Inv_nWOInvoice_Modules
            dgMSP.DataSource = mRole.Inv_nMSP_Modules 'Ajay 26-04-2023
            dgMROContract.DataSource = mRole.MROContract_Modules
            dgADSBReviewMeeting.DataSource = mRole.ADSBReviewMeeting_Modules
            dgEmpCAAuthorization.DataSource = mRole.EmpCAAuthorization_Modules
            dgDueJobPlanning.DataSource = mRole.DueJobPlanning_Modules
            dgCabinDefect.DataSource = mRole.CabinDefect_Modules 'Sankalp Cabin Defect 29/7/25
            GV_Project.DataSource = mRole.Project_Modules

            DataBind()
            VisibilityCode()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Public Sub VisibilityCode()

        Try

            For i As Integer = 0 To dgMaintenance.Rows.Count - 1

                Dim chkAuthorized As CheckBox
                Dim chkView As CheckBox
                Dim chkPrint As CheckBox
                Dim chkAdd As CheckBox
                Dim chkEdit As CheckBox
                Dim chkDelete As CheckBox

                'Dim ModuleDescription As String
                Dim child As RoleModule
                'ModuleDescription = (dgMaintenance.DataKeys(i).Values("ModuleDescription").ToString)
                child = mRole.Maint_Maintenance_Modules.Item(i)

                chkAuthorized = dgMaintenance.Rows(i).FindControl("chkAuthorized")

                chkView = dgMaintenance.Rows(i).FindControl("chkView")
                chkPrint = dgMaintenance.Rows(i).FindControl("chkPrint")
                chkAdd = dgMaintenance.Rows(i).FindControl("chkAdd")
                chkEdit = dgMaintenance.Rows(i).FindControl("chkEdit")
                chkDelete = dgMaintenance.Rows(i).FindControl("chkDelete")

                chkView.Visible = child.ModuleFunctions.Contains("View")
                chkPrint.Visible = child.ModuleFunctions.Contains("Print")
                chkAdd.Visible = child.ModuleFunctions.Contains("New")
                chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
                chkDelete.Visible = child.ModuleFunctions.Contains("Delete")
                chkAuthorized.Visible = child.ModuleFunctions.Contains("Authorized")

            Next

            'Added By Vikrant on 30-Jun-2021 For ALL30062021 
            For i As Integer = 0 To dgWorkOrder.Rows.Count - 1

                Dim chkCompleted As CheckBox
                Dim child As RoleModule
                Dim chkAuthorized As CheckBox
                Dim chkView As CheckBox
                Dim chkPrint As CheckBox
                Dim chkAdd As CheckBox
                Dim chkEdit As CheckBox
                Dim chkDelete As CheckBox

                child = mRole.Inv_WorkOrder_Modules.Item(i)

                chkCompleted = dgWorkOrder.Rows(i).FindControl("chkCompleted")
                chkCompleted.Visible = child.ModuleFunctions.Contains("Completed")
                chkAuthorized = dgWorkOrder.Rows(i).FindControl("chkAuthorized")
                chkView = dgWorkOrder.Rows(i).FindControl("chkView")
                chkPrint = dgWorkOrder.Rows(i).FindControl("chkPrint")
                chkAdd = dgWorkOrder.Rows(i).FindControl("chkAdd")
                chkEdit = dgWorkOrder.Rows(i).FindControl("chkEdit")
                chkDelete = dgWorkOrder.Rows(i).FindControl("chkDelete")
                chkView.Visible = child.ModuleFunctions.Contains("View")
                chkPrint.Visible = child.ModuleFunctions.Contains("Print")
                chkAdd.Visible = child.ModuleFunctions.Contains("New")
                chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
                chkDelete.Visible = child.ModuleFunctions.Contains("Delete")
                chkAuthorized.Visible = child.ModuleFunctions.Contains("Authorized")

            Next

            For i As Integer = 0 To dgSpareMaint.Rows.Count - 1

                Dim chkCompleted As CheckBox
                Dim child As RoleModule
                Dim chkAuthorized As CheckBox

                child = mRole.SpareMaint_Maintenance_Modules.Item(i)

                chkCompleted = dgSpareMaint.Rows(i).FindControl("chkCompleted")
                chkCompleted.Visible = child.ModuleFunctions.Contains("Completed")
                chkAuthorized = dgSpareMaint.Rows(i).FindControl("chkAuthorized")
                chkAuthorized.Visible = child.ModuleFunctions.Contains("Authorized")

            Next
            'End

            For i As Integer = 0 To dgMROContract.Rows.Count - 1

                Dim chkAuthorized As CheckBox
                Dim child As RoleModule

                child = mRole.MROContract_Modules.Item(i)

                chkAuthorized = dgMROContract.Rows(i).FindControl("chkAuthorized")
                chkAuthorized.Visible = child.ModuleFunctions.Contains("Authorized")

            Next

            For i As Integer = 0 To dgWorkOrderInvoice.Rows.Count - 1

                Dim chkAuthorized As CheckBox
                Dim chkView As CheckBox
                Dim chkPrint As CheckBox
                Dim chkAdd As CheckBox
                Dim chkEdit As CheckBox
                Dim chkDelete As CheckBox
                Dim child As RoleModule

                child = mRole.Inv_nWOInvoice_Modules.Item(i)

                chkAuthorized = dgWorkOrderInvoice.Rows(i).FindControl("chkAuthorized")
                chkView = dgWorkOrderInvoice.Rows(i).FindControl("chkView")
                chkPrint = dgWorkOrderInvoice.Rows(i).FindControl("chkPrint")
                chkAdd = dgWorkOrderInvoice.Rows(i).FindControl("chkAdd")
                chkEdit = dgWorkOrderInvoice.Rows(i).FindControl("chkEdit")
                chkDelete = dgWorkOrderInvoice.Rows(i).FindControl("chkDelete")
                chkView.Visible = child.ModuleFunctions.Contains("View")
                chkPrint.Visible = child.ModuleFunctions.Contains("Print")
                chkAdd.Visible = child.ModuleFunctions.Contains("New")
                chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
                chkDelete.Visible = child.ModuleFunctions.Contains("Delete")
                chkAuthorized.Visible = child.ModuleFunctions.Contains("Authorized")

            Next

            '========== Ajay 26-04-2023=================
            For i As Integer = 0 To dgMSP.Rows.Count - 1

                Dim chkView As CheckBox
                Dim chkPrint As CheckBox
                Dim chkAdd As CheckBox
                Dim chkEdit As CheckBox
                Dim chkDelete As CheckBox
                Dim child As RoleModule

                child = mRole.Inv_nMSP_Modules.Item(i)

                chkView = dgMSP.Rows(i).FindControl("chkView")
                chkPrint = dgMSP.Rows(i).FindControl("chkPrint")
                chkAdd = dgMSP.Rows(i).FindControl("chkAdd")
                chkEdit = dgMSP.Rows(i).FindControl("chkEdit")
                chkDelete = dgMSP.Rows(i).FindControl("chkDelete")
                chkView.Visible = child.ModuleFunctions.Contains("View")
                chkPrint.Visible = child.ModuleFunctions.Contains("Print")
                chkAdd.Visible = child.ModuleFunctions.Contains("New")
                chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
                chkDelete.Visible = child.ModuleFunctions.Contains("Delete")

            Next

            For i As Integer = 0 To dgADSBReviewMeeting.Rows.Count - 1

                Dim chkAuthorized As CheckBox
                Dim chkView As CheckBox
                Dim chkPrint As CheckBox
                Dim chkAdd As CheckBox
                Dim chkEdit As CheckBox
                Dim chkDelete As CheckBox
                Dim child As RoleModule

                child = mRole.ADSBReviewMeeting_Modules.Item(i)

                chkAuthorized = dgADSBReviewMeeting.Rows(i).FindControl("chkAuthorized")
                chkView = dgADSBReviewMeeting.Rows(i).FindControl("chkView")
                chkPrint = dgADSBReviewMeeting.Rows(i).FindControl("chkPrint")
                chkAdd = dgADSBReviewMeeting.Rows(i).FindControl("chkAdd")
                chkEdit = dgADSBReviewMeeting.Rows(i).FindControl("chkEdit")
                chkDelete = dgADSBReviewMeeting.Rows(i).FindControl("chkDelete")
                chkView.Visible = child.ModuleFunctions.Contains("View")
                chkPrint.Visible = child.ModuleFunctions.Contains("Print")
                chkAdd.Visible = child.ModuleFunctions.Contains("New")
                chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
                chkDelete.Visible = child.ModuleFunctions.Contains("Delete")
                chkAuthorized.Visible = child.ModuleFunctions.Contains("Authorized")

            Next

            For i As Integer = 0 To dgEmpCAAuthorization.Rows.Count - 1

                Dim chkView As CheckBox
                Dim chkPrint As CheckBox
                Dim chkAdd As CheckBox
                Dim chkEdit As CheckBox
                Dim chkDelete As CheckBox
                Dim child As RoleModule

                child = mRole.EmpCAAuthorization_Modules.Item(i)
                chkView = dgEmpCAAuthorization.Rows(i).FindControl("chkView")
                chkPrint = dgEmpCAAuthorization.Rows(i).FindControl("chkPrint")
                chkAdd = dgEmpCAAuthorization.Rows(i).FindControl("chkAdd")
                chkEdit = dgEmpCAAuthorization.Rows(i).FindControl("chkEdit")
                chkDelete = dgEmpCAAuthorization.Rows(i).FindControl("chkDelete")
                chkView.Visible = child.ModuleFunctions.Contains("View")
                chkPrint.Visible = child.ModuleFunctions.Contains("Print")
                chkAdd.Visible = child.ModuleFunctions.Contains("New")
                chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
                chkDelete.Visible = child.ModuleFunctions.Contains("Delete")

            Next

            For i As Integer = 0 To dgDueJobPlanning.Rows.Count - 1

                Dim chkView As CheckBox
                Dim chkPrint As CheckBox
                Dim chkAdd As CheckBox
                Dim chkEdit As CheckBox
                Dim chkDelete As CheckBox
                Dim child As RoleModule

                child = mRole.DueJobPlanning_Modules.Item(i)

                chkView = dgDueJobPlanning.Rows(i).FindControl("chkView")
                chkPrint = dgDueJobPlanning.Rows(i).FindControl("chkPrint")
                chkAdd = dgDueJobPlanning.Rows(i).FindControl("chkAdd")
                chkEdit = dgDueJobPlanning.Rows(i).FindControl("chkEdit")
                chkDelete = dgDueJobPlanning.Rows(i).FindControl("chkDelete")
                chkView.Visible = child.ModuleFunctions.Contains("View")
                chkPrint.Visible = child.ModuleFunctions.Contains("Print")
                chkAdd.Visible = child.ModuleFunctions.Contains("New")
                chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
                chkDelete.Visible = child.ModuleFunctions.Contains("Delete")

            Next

            'Sankalp Cabin Defect 29/7/25
            For i As Integer = 0 To dgCabinDefect.Rows.Count - 1

                Dim chkView As CheckBox
                Dim chkPrint As CheckBox
                Dim chkAdd As CheckBox
                Dim chkEdit As CheckBox
                Dim chkDelete As CheckBox
                Dim child As RoleModule

                child = mRole.CabinDefect_Modules.Item(i)

                chkView = dgCabinDefect.Rows(i).FindControl("chkView")
                chkPrint = dgCabinDefect.Rows(i).FindControl("chkPrint")
                chkAdd = dgCabinDefect.Rows(i).FindControl("chkAdd")
                chkEdit = dgCabinDefect.Rows(i).FindControl("chkEdit")
                chkDelete = dgCabinDefect.Rows(i).FindControl("chkDelete")
                chkView.Visible = child.ModuleFunctions.Contains("View")
                chkPrint.Visible = child.ModuleFunctions.Contains("Print")
                chkAdd.Visible = child.ModuleFunctions.Contains("New")
                chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
                chkDelete.Visible = child.ModuleFunctions.Contains("Delete")

            Next


            'Added by Prashant on 15-Dec-2023
            For i As Integer = 0 To dgPurchaseOrder.Rows.Count - 1

                Dim chkView As CheckBox
                Dim chkPrint As CheckBox
                Dim chkAdd As CheckBox
                Dim chkEdit As CheckBox
                Dim chkDelete As CheckBox
                Dim chkAuthorized As CheckBox
                Dim child As RoleModule

                child = mRole.Inv_PurchaseOrders_Modules.Item(i)

                chkView = dgPurchaseOrder.Rows(i).FindControl("chkView")
                chkPrint = dgPurchaseOrder.Rows(i).FindControl("chkPrint")
                chkAdd = dgPurchaseOrder.Rows(i).FindControl("chkAdd")
                chkEdit = dgPurchaseOrder.Rows(i).FindControl("chkEdit")
                chkDelete = dgPurchaseOrder.Rows(i).FindControl("chkDelete")
                chkAuthorized = dgPurchaseOrder.Rows(i).FindControl("chkAuthorized")
                chkView.Visible = child.ModuleFunctions.Contains("View")
                chkPrint.Visible = child.ModuleFunctions.Contains("Print")
                chkAdd.Visible = child.ModuleFunctions.Contains("New")
                chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
                chkDelete.Visible = child.ModuleFunctions.Contains("Delete")
                chkAuthorized.Visible = child.ModuleFunctions.Contains("Authorized")

            Next

            For i As Integer = 0 To dgHangar.Rows.Count - 1

                Dim chkAuthorized As CheckBox
                Dim chkView As CheckBox
                Dim chkPrint As CheckBox
                Dim chkAdd As CheckBox
                Dim chkEdit As CheckBox
                Dim chkDelete As CheckBox
                Dim child As RoleModule

                child = mRole.Hangar_Modules.Item(i)

                chkAuthorized = dgHangar.Rows(i).FindControl("chkAuthorized")
                chkView = dgHangar.Rows(i).FindControl("chkView")
                chkPrint = dgHangar.Rows(i).FindControl("chkPrint")
                chkAdd = dgHangar.Rows(i).FindControl("chkAdd")
                chkEdit = dgHangar.Rows(i).FindControl("chkEdit")
                chkDelete = dgADSBReviewMeeting.Rows(i).FindControl("chkDelete")
                chkView.Visible = child.ModuleFunctions.Contains("View")
                chkPrint.Visible = child.ModuleFunctions.Contains("Print")
                chkAdd.Visible = child.ModuleFunctions.Contains("New")
                chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
                chkDelete.Visible = child.ModuleFunctions.Contains("Delete")
                chkAuthorized.Visible = child.ModuleFunctions.Contains("Authorized")

            Next

            For i As Integer = 0 To dgCompanyDocument.Rows.Count - 1

                Dim chkAuthorized As CheckBox
                Dim chkView As CheckBox
                Dim chkPrint As CheckBox
                Dim chkAdd As CheckBox
                Dim chkEdit As CheckBox
                Dim chkDelete As CheckBox
                Dim child As RoleModule

                child = mRole.CompanyDocument_Modules.Item(i)

                chkAuthorized = dgCompanyDocument.Rows(i).FindControl("chkAuthorized")
                chkView = dgCompanyDocument.Rows(i).FindControl("chkView")
                chkPrint = dgCompanyDocument.Rows(i).FindControl("chkPrint")
                chkAdd = dgCompanyDocument.Rows(i).FindControl("chkAdd")
                chkEdit = dgCompanyDocument.Rows(i).FindControl("chkEdit")
                chkDelete = dgCompanyDocument.Rows(i).FindControl("chkDelete")
                chkView.Visible = child.ModuleFunctions.Contains("View")
                chkPrint.Visible = child.ModuleFunctions.Contains("Print")
                chkAdd.Visible = child.ModuleFunctions.Contains("New")
                chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
                chkDelete.Visible = child.ModuleFunctions.Contains("Delete")
                chkAuthorized.Visible = child.ModuleFunctions.Contains("Authorized")

            Next

            For i As Integer = 0 To dgMEL.Rows.Count - 1
                Dim chkAuthorized As CheckBox

                Dim chkView As CheckBox
                Dim chkPrint As CheckBox
                Dim chkAdd As CheckBox
                Dim chkEdit As CheckBox
                Dim chkDelete As CheckBox
                Dim child As RoleModule

                child = mRole.MEL_Modules.Item(i)

                chkView = dgMEL.Rows(i).FindControl("chkView")
                chkPrint = dgMEL.Rows(i).FindControl("chkPrint")
                chkAdd = dgMEL.Rows(i).FindControl("chkAdd")
                chkEdit = dgMEL.Rows(i).FindControl("chkEdit")
                chkDelete = dgMEL.Rows(i).FindControl("chkDelete")
                chkView.Visible = child.ModuleFunctions.Contains("View")
                chkPrint.Visible = child.ModuleFunctions.Contains("Print")
                chkAdd.Visible = child.ModuleFunctions.Contains("New")
                chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
                chkDelete.Visible = child.ModuleFunctions.Contains("Delete")

            Next

            For i As Integer = 0 To dgDiscrepancy.Rows.Count - 1

                Dim chkAuthorized As CheckBox
                Dim chkView As CheckBox
                Dim chkPrint As CheckBox
                Dim chkAdd As CheckBox
                Dim chkEdit As CheckBox
                Dim chkDelete As CheckBox


                Dim child As RoleModule
                child = mRole.Discrepancy_Modules.Item(i)

                chkView = dgDiscrepancy.Rows(i).FindControl("chkView")
                chkPrint = dgDiscrepancy.Rows(i).FindControl("chkPrint")
                chkAdd = dgDiscrepancy.Rows(i).FindControl("chkAdd")
                chkEdit = dgDiscrepancy.Rows(i).FindControl("chkEdit")
                chkDelete = dgDiscrepancy.Rows(i).FindControl("chkDelete")
                chkView.Visible = child.ModuleFunctions.Contains("View")
                chkPrint.Visible = child.ModuleFunctions.Contains("Print")
                chkAdd.Visible = child.ModuleFunctions.Contains("New")
                chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
                chkDelete.Visible = child.ModuleFunctions.Contains("Delete")

            Next

            For i As Integer = 0 To GV_Project.Rows.Count - 1

                Dim chkAuthorized As CheckBox
                Dim chkView As CheckBox
                Dim chkPrint As CheckBox
                Dim chkAdd As CheckBox
                Dim chkEdit As CheckBox
                Dim chkDelete As CheckBox
                Dim child As RoleModule

                Try

                    child = mRole.Project_Modules.Item(i)

                    chkView = GV_Project.Rows(i).FindControl("chkView")
                    chkPrint = GV_Project.Rows(i).FindControl("chkPrint")
                    chkAdd = GV_Project.Rows(i).FindControl("chkAdd")
                    chkEdit = GV_Project.Rows(i).FindControl("chkEdit")
                    chkDelete = GV_Project.Rows(i).FindControl("chkDelete")
                    chkAuthorized = GV_Project.Rows(i).FindControl("chkAuthorized")

                    chkView.Visible = child.ModuleFunctions.Contains("View")
                    chkPrint.Visible = child.ModuleFunctions.Contains("Print")
                    chkAdd.Visible = child.ModuleFunctions.Contains("New")
                    chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
                    chkDelete.Visible = child.ModuleFunctions.Contains("Delete")
                    chkAuthorized.Visible = child.ModuleFunctions.Contains("Authorized")

                Catch ex As Exception
                    Throw ex.GetBaseException()
                End Try

            Next
			For i As Integer = 0 To dgDocumentLocker.Rows.Count - 1


				Dim chkView As CheckBox
				Dim chkPrint As CheckBox
				Dim chkAdd As CheckBox
				Dim chkEdit As CheckBox
				Dim chkDelete As CheckBox
				Dim child As RoleModule

				child = mRole.DocumentLocker_Modules.Item(i)

				chkView = dgDocumentLocker.Rows(i).FindControl("chkView")
				chkPrint = dgDocumentLocker.Rows(i).FindControl("chkPrint")
				chkAdd = dgDocumentLocker.Rows(i).FindControl("chkAdd")
				chkEdit = dgDocumentLocker.Rows(i).FindControl("chkEdit")
				chkDelete = dgDocumentLocker.Rows(i).FindControl("chkDelete")
				chkView.Visible = child.ModuleFunctions.Contains("View")
				chkPrint.Visible = child.ModuleFunctions.Contains("Print")
				chkAdd.Visible = child.ModuleFunctions.Contains("New")
				chkEdit.Visible = child.ModuleFunctions.Contains("Edit")
				chkDelete.Visible = child.ModuleFunctions.Contains("Delete")


			Next
		Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub Customvalidate(s As Object, e As ServerValidateEventArgs)

        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)

        If CustValid.ControlToValidate = "txtRoleName" Then

            'Added by Prashant 6-Sep-2013 ALL06092013
            If mRole.Name = "Administrator" Then

                If Not CheckForAdministrator() Then

                    CustValid.ErrorMessage = "User manager and Role manager modules are mandatory for the Administrator role."
                    e.IsValid = False

                Else
                    e.IsValid = True
                End If
                'End
            Else

                If Not (CheckCheckedMasters() Or
                        CheckCheckedRequisitions() Or
                        CheckCheckedPurchaseEnquiries() Or
                        CheckCheckedPurchaseQuotations() Or
                        CheckCheckedPurchaseOrders() Or
                        CheckCheckedGoodsReceipts() Or
                        CheckCheckedGoodsIssues() Or
                        CheckCheckedPurchaseInvoices() Or
                        CheckCheckedSalesModules() Or
                        CheckCheckedMaintMasters() Or
                        CheckCheckedMaintenance() Or
                        CheckCheckedTool() Or
                        CheckCheckedCalibration() Or
                        CheckCheckedManual() Or
                        CheckCheckedMaintenanceReports() Or
                        CheckCheckedQA() Or
                        CheckCheckedMEL() Or
                        CheckCheckedWO() Or
                        CheckCheckedWorkInvoice() Or
                        CheckCheckedReliability() Or
                        CheckCheckedNewRequisitions() Or
                        CheckCheckedLineMaintenance() Or
                        CheckCheckedExportInvoice() Or
                        CheckCheckedInventoryReports() Or
                        CheckCheckedMPD() Or CheckCheckedCWP() Or
                        CheckCheckedDentBuckleChart() Or
                        CheckCheckedInfoDisplay() Or
                        CheckCheckedHangar() Or
                        CheckCheckedPaymentAdvice() Or
                        CheckCheckedMaintenanceDashboard() Or
                        CheckCheckedInventoryDashboard() Or
                        CheckCheckedCompanyDocument() Or
                        CheckCheckedComponentReservation() Or
                        CheckCheckedWOInvoice() Or
                        CheckCheckedMSP() Or
                        CheckCheckedADSBReviewMeeting() Or
                        CheckCheckedMROContract() Or
                        CheckCheckedDocumentLocker() Or
                        CheckCheckedSpareMaint() Or
                        CheckCheckedDueJobPlanning() Or
                        CheckCheckedCabinDefect() Or 'Sankalp Cabin Defects
                        CheckCheckedDiscrepancy() Or
                        AtLeastOneProjectActionChecked()) Then

                    CustValid.ErrorMessage = "Select at least one Module for the Role."
                    e.IsValid = False

                Else
                    e.IsValid = True
                End If

            End If

        End If

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011

        Try

            If Not IsPostBack And CType(Session("sender"), String) = "" Then

                If txtRoleName.Enabled = True Then
                    SetFocus(txtRoleName)
                End If

                DataFieldBind()

            End If

            If mRole.IsNew Then

                'Added By Vikrant On 19-Feb-2021 For ALL19022021
                If Session("CreateCopy") = "True" Then
                    lbltitle.Text = "Role Information [ New as " + Session("CopiedRoleName") + " ] "
                    'End
                Else
                    lbltitle.Text = "Role Information [New]"
                End If

            Else

                lbltitle.Text = "Role Information [ " & mRole.Name & " ]"
                txtRoleName.BackColor = Color.Silver
                txtRoleName.ReadOnly = True

            End If

            ConrolVisibility()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        RemoveSession()
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub

    Private Sub SaveDetails(sender As Object, e As EventArgs) Handles btnSave.Click

        Try

            If Not User.IsInRole("RoleManagerView") Then 'Added by Saylee on 1-Feb-2013 for ALL01022013

                MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                MSGBox.Message_text.Authorization,
                                "",
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Sub

            End If

            If IsValid Then

                SetObject()
                mRole.Save()

                'Added By Vikrant On 19-Feb-2021 For ALL19022021
                If Session("CreateCopy") = "True" Then

                    MarkLog(Action.Save,
                            "Role Manager",
                            "Copy of User Name : " + Session("CopiedRoleName") + ", New Role Name : " + mRole.Name + ", Created By : " + User.Identity.Name,
                            ErrorType.NoError,
                            mRole.RoleID,
                            EventLogID)
                    'End
                Else

                    MarkLog(Action.Save,
                            "Role Manager", "Role Name : " + mRole.Name,
                            ErrorType.NoError,
                            mRole.RoleID,
                            EventLogID)

                End If
                'End

                Session("mRole") = mRole
                DataFieldBind()
                SetSession()

                MSGBoxCtrl.Show("Alert..!",
                                "Rights assigned Successfully to " + mRole.Name + "..!!", "",
                                MsgBoxStyle.OkOnly,
                                "")

            End If

        Catch ex As SqlException

            If ex.Number = 8114 Or ex.Number = 8115 Then

                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow,
                                MSGBox.Message_text.NumericOverFlow,
                                " Rate or Qty or Conversion Factor. ",
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Sub

            ElseIf ex.Number = 8145 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.ProcedureError,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Sub

            ElseIf ex.Number = 2627 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.Duplicate,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Sub

            ElseIf ex.Number = 547 Then

                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                MSGBox.Message_text.ReferenceDelete,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

            End If

        Catch ex As Exception

            MSGBoxCtrl.show(MSGBox.Message_title.Exception,
                            MSGBox.Message_text.Exception,
                            ex.Message,
                            MsgBoxStyle.OkOnly,
                            "")

        End Try

    End Sub

    Private Sub dgMasters_RowCommand(sender As Object, e As Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMasters.RowCommand
        ''Dim index As Int32 = e.Item.ItemIndex + dgMasters.CurrentPageIndex * dgMasters.PageSize

        Dim index As Int32 = CInt(e.CommandArgument) + dgMasters.PageIndex * dgMasters.PageSize
        dgMasters.DataSource = mRole.Inv_Master_Modules
        dgMasters.DataBind()
    End Sub

    Private Sub dgRequisition_RowCommand(sender As Object, e As Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRequisition.RowCommand
        ''Dim index As Int32 = e.Item.ItemIndex + dgRequisition.CurrentPageIndex * dgRequisition.PageSize

        Dim index As Int32 = CInt(e.CommandArgument) + dgRequisition.PageIndex * dgRequisition.PageSize
        dgRequisition.DataSource = mRole.Inv_Requisition_Modules
        dgRequisition.DataBind()
    End Sub

    'Added BY VIkrant on 27-Aug-2012 For ALL27082012
    Private Sub dgNewRequisition_RowCommand(sender As Object, e As Web.UI.WebControls.GridViewCommandEventArgs) Handles dgNewRequisition.RowCommand
        ''Dim index As Int32 = e.Item.ItemIndex + dgNewRequisition.CurrentPageIndex * dgNewRequisition.PageSize

        Dim index As Int32 = CInt(e.CommandArgument) + dgNewRequisition.PageIndex * dgNewRequisition.PageSize
        dgNewRequisition.DataSource = mRole.Inv_New_Requisition_Modules
        dgNewRequisition.DataBind()
    End Sub
    'End

    Private Sub chkMPD_CheckedChanged(sender As Object, e As EventArgs) Handles chkMPD.CheckedChanged
        mRole.Maint_MPD_Modules.SelectAll(chkMPD.Checked)
        dgMPD.DataSource = mRole.Maint_MPD_Modules
        dgMPD.DataBind()
    End Sub

    Private Sub chkCWP_CheckedChanged(sender As Object, e As EventArgs) Handles chkCWP.CheckedChanged
        mRole.Maint_CWP_Modules.SelectAll(chkCWP.Checked)
        dgCWP.DataSource = mRole.Maint_CWP_Modules
        dgCWP.DataBind()
    End Sub

    'D&BChart
    Private Sub chkDentBuckleChart_CheckedChanged(sender As Object, e As EventArgs) Handles chkDentBuckleChart.CheckedChanged
        mRole.DentBuckleChart_Modules.SelectAll(chkDentBuckleChart.Checked)
        dgDentBuckleChart.DataSource = mRole.DentBuckleChart_Modules
        dgDentBuckleChart.DataBind()
    End Sub
    'End

    'Hangar Planning
    Private Sub chkHangar_CheckedChanged(sender As Object, e As EventArgs) Handles chkHangar.CheckedChanged
        mRole.Hangar_Modules.SelectAll(chkHangar.Checked)
        dgHangar.DataSource = mRole.Hangar_Modules
        dgHangar.DataBind()
        VisibilityCode()
    End Sub

    'Company Document Added by Vikrant On 12-Oct-2021 For ALL12102021
    Private Sub chkCompanyDocument_CheckedChanged(sender As Object, e As EventArgs) Handles chkCompanyDocument.CheckedChanged
        mRole.CompanyDocument_Modules.SelectAll(chkCompanyDocument.Checked)
        dgCompanyDocument.DataSource = mRole.CompanyDocument_Modules
        dgCompanyDocument.DataBind()
        VisibilityCode()
    End Sub
    'End

    'Company Document Added by Shital On 29-Nov-2021
    Private Sub chkComponentReservation_CheckedChanged(sender As Object, e As EventArgs) Handles chkComponentReservation.CheckedChanged
        mRole.ComponentReservation_Modules.SelectAll(chkComponentReservation.Checked)
        dgComponentReservation.DataSource = mRole.ComponentReservation_Modules
        dgComponentReservation.DataBind()
    End Sub
    'End

    'Payment Advice Added by Shital on 28-Jan-2018
    Private Sub chkPaymentAdvice_CheckedChanged(sender As Object, e As EventArgs) Handles chkPaymentAdvice.CheckedChanged
        mRole.PaymentAdvice_Modules.SelectAll(chkPaymentAdvice.Checked)
        dgPaymentAdvice.DataSource = mRole.PaymentAdvice_Modules
        dgPaymentAdvice.DataBind()
    End Sub
    'End

    Private Sub chkInfoDisplay_CheckedChanged(sender As Object, e As EventArgs) Handles chkInfoDisplay.CheckedChanged
        mRole.InfoDisplay_Modules.SelectAll(chkInfoDisplay.Checked)
        dgInfoDisplay.DataSource = mRole.InfoDisplay_Modules
        dgInfoDisplay.DataBind()
    End Sub

    'Maintenance Dashboard Added by Saylee on 26-May-2020 , LOCKDOWN 4.0
    Private Sub chkMaintenanceDashboards_CheckedChanged(sender As Object, e As EventArgs) Handles chkMaintenanceDashboards.CheckedChanged
        mRole.MaintenanceDashboard_Modules.SelectAll(chkMaintenanceDashboards.Checked)
        dgMaintenanceDashboard.DataSource = mRole.MaintenanceDashboard_Modules
        dgMaintenanceDashboard.DataBind()
    End Sub
    'End

    'Inventory Dashboards Added by Saylee on 26-May-2020 , LOCKDOWN 4.0
    Private Sub chkInventoryDashboards_CheckedChanged(sender As Object, e As EventArgs) Handles chkInventoryDashboards.CheckedChanged
        mRole.InventoryDashboard_Modules.SelectAll(chkInventoryDashboards.Checked)
        dgInventoryDashboard.DataSource = mRole.InventoryDashboard_Modules
        dgInventoryDashboard.DataBind()
    End Sub
    'End

    'Spare Maint Added by Saylee on  18-Aug-2020, LockDown 4.0
    Private Sub chkSpareMaint_CheckedChanged(sender As Object, e As EventArgs) Handles chkSpareMaint.CheckedChanged
        mRole.SpareMaint_Maintenance_Modules.SelectAll(chkSpareMaint.Checked)
        dgSpareMaint.DataSource = mRole.SpareMaint_Maintenance_Modules
        dgSpareMaint.DataBind()
        VisibilityCode()
    End Sub

#Region "CheckedChanged"

    Private Sub chkMasters_CheckedChanged(sender As Object, e As EventArgs) Handles chkMasters.CheckedChanged
        mRole.Inv_Master_Modules.SelectAll(chkMasters.Checked)
        dgMasters.DataSource = mRole.Inv_Master_Modules
        dgMasters.DataBind()
    End Sub

    Private Sub chkRequisition_CheckedChanged(sender As Object, e As EventArgs) Handles chkRequisition.CheckedChanged
        mRole.Inv_Requisition_Modules.SelectAll(chkRequisition.Checked)
        dgRequisition.DataSource = mRole.Inv_Requisition_Modules
        dgRequisition.DataBind()
    End Sub

    Private Sub chkPurchaseEnquiry_CheckedChanged(sender As Object, e As EventArgs) Handles chkPurchaseEnquiry.CheckedChanged
        mRole.Inv_PurchaseEnquiries_Modules.SelectAll(chkPurchaseEnquiry.Checked)
        dgPurchaseEnquiry.DataSource = mRole.Inv_PurchaseEnquiries_Modules
        dgPurchaseEnquiry.DataBind()
    End Sub

    Private Sub chkPurchaseQuotation_CheckedChanged(sender As Object, e As EventArgs) Handles chkPurchaseQuotation.CheckedChanged
        mRole.Inv_PurchaseQuotations_Modules.SelectAll(chkPurchaseQuotation.Checked)
        dgPurchaseQuotation.DataSource = mRole.Inv_PurchaseQuotations_Modules
        dgPurchaseQuotation.DataBind()
    End Sub

    Private Sub chkPurchaseOrder_CheckedChanged(sender As Object, e As EventArgs) Handles chkPurchaseOrder.CheckedChanged
        mRole.Inv_PurchaseOrders_Modules.SelectAll(chkPurchaseOrder.Checked)
        dgPurchaseOrder.DataSource = mRole.Inv_PurchaseOrders_Modules
        dgPurchaseOrder.DataBind()
    End Sub

    Private Sub chkGoodsReceipt_CheckedChanged(sender As Object, e As EventArgs) Handles chkGoodsReceipt.CheckedChanged
        mRole.Inv_GoodsReceipts_Modules.SelectAll(chkGoodsReceipt.Checked)
        dgGoodsReceipt.DataSource = mRole.Inv_GoodsReceipts_Modules
        dgGoodsReceipt.DataBind()
    End Sub

    Private Sub chkGoodsIssue_CheckedChanged(sender As Object, e As EventArgs) Handles chkGoodsIssue.CheckedChanged
        mRole.Inv_GoodsIssues_Modules.SelectAll(chkGoodsIssue.Checked)
        dgGoodsIssue.DataSource = mRole.Inv_GoodsIssues_Modules
        dgGoodsIssue.DataBind()
    End Sub

    Private Sub chkPurchaseInvoice_CheckedChanged(sender As Object, e As EventArgs) Handles chkPurchaseInvoice.CheckedChanged
        mRole.Inv_PurchaseInvoices_Modules.SelectAll(chkPurchaseInvoice.Checked)
        dgPurchaseInvoice.DataSource = mRole.Inv_PurchaseInvoices_Modules
        dgPurchaseInvoice.DataBind()
    End Sub

    Private Sub chkSalesModules_CheckedChanged(sender As Object, e As EventArgs) Handles chkSalesModules.CheckedChanged
        mRole.Inv_SalesModules_Modules.SelectAll(chkSalesModules.Checked)
        dgSalesModules.DataSource = mRole.Inv_SalesModules_Modules
        dgSalesModules.DataBind()
    End Sub

    Private Sub chkWorkOrder_CheckedChanged(sender As Object, e As EventArgs) Handles chkWorkOrder.CheckedChanged
        mRole.Inv_WorkOrder_Modules.SelectAll(chkWorkOrder.Checked)
        dgWorkOrder.DataSource = mRole.Inv_WorkOrder_Modules
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            dgWorkOrder.Columns(1).HeaderText = "Engineering Order"
        Else
            dgWorkOrder.Columns(1).HeaderText = "Work Order"
        End If

        dgWorkOrder.DataBind()
    End Sub

    Private Sub chkInventoryReports_CheckedChanged(sender As Object, e As EventArgs) Handles chkInventoryReports.CheckedChanged
        mRole.Inv_Reports_Modules.SelectAll(chkInventoryReports.Checked)
        dgInventoryReports.DataSource = mRole.Inv_Reports_Modules
        dgInventoryReports.DataBind()
    End Sub

    Private Sub chkMaintMasters_CheckedChanged(sender As Object, e As EventArgs) Handles chkMaintMasters.CheckedChanged
        mRole.Maint_Master_Modules.SelectAll(chkMaintMasters.Checked)
        dgMaintMasters.DataSource = mRole.Maint_Master_Modules
        dgMaintMasters.DataBind()
    End Sub

    Private Sub chkTool_CheckedChanged(sender As Object, e As EventArgs) Handles chkTool.CheckedChanged
        Dim IsTools As Boolean
        If chkTools.Checked = True Or chkTool.Checked = True Then
            IsTools = True
        Else
            IsTools = False
        End If
        mRole.Tool_Modules.SelectAll(IsTools)
        dgTools.DataSource = mRole.Tool_Modules
        dgTools.DataBind()
        Dim IsAdminUtilitiess As Boolean

        If chkTool.Checked = True Then
            IsAdminUtilitiess = True
        Else
            IsAdminUtilitiess = False
        End If
        mRole.AdminUtilities_Modules.SelectAll(IsAdminUtilitiess)
        dgAdminUtilitiess.DataSource = mRole.AdminUtilities_Modules
        dgAdminUtilitiess.DataBind()
    End Sub

    Private Sub chkTools_CheckedChanged(sender As Object, e As EventArgs) Handles chkTools.CheckedChanged
        Dim IsTools As Boolean
        If chkTools.Checked = True Or chkTool.Checked = True Then
            IsTools = True
        Else
            IsTools = False
        End If
        mRole.Tool_Modules.SelectAll(IsTools)
        dgTools.DataSource = mRole.Tool_Modules
        dgTools.DataBind()
    End Sub

    Private Sub chkAdminUtilitiess_CheckedChanged(sender As Object, e As EventArgs) Handles chkAdminUtilitiess.CheckedChanged
        Dim IsAdminUtilitiess As Boolean
        If chkAdminUtilitiess.Checked = True Or chkAdminUtilitiess.Checked = True Then
            IsAdminUtilitiess = True
        Else
            IsAdminUtilitiess = False
        End If
        mRole.AdminUtilities_Modules.SelectAll(IsAdminUtilitiess)
        dgAdminUtilitiess.DataSource = mRole.AdminUtilities_Modules
        dgAdminUtilitiess.DataBind()
    End Sub

    Private Sub chkDocumentLocker_CheckedChanged(sender As Object, e As EventArgs) Handles chkDocumentLocker.CheckedChanged
        Dim IsDocumentLocker As Boolean
        If chkDocumentLocker.Checked = True Or chkDocumentLocker.Checked = True Then
            IsDocumentLocker = True
        Else
            IsDocumentLocker = False
        End If
        mRole.DocumentLocker_Modules.SelectAll(IsDocumentLocker)
        dgDocumentLocker.DataSource = mRole.DocumentLocker_Modules
        dgDocumentLocker.DataBind()
    End Sub

    Private Sub chkMROContract_CheckedChanged(sender As Object, e As EventArgs) Handles chkMROContract.CheckedChanged
        Dim IsMROContract As Boolean
        If chkMROContract.Checked = True Or chkMROContract.Checked = True Then
            IsMROContract = True
        Else
            IsMROContract = False
        End If
        mRole.MROContract_Modules.SelectAll(IsMROContract)
        dgMROContract.DataSource = mRole.MROContract_Modules
        dgMROContract.DataBind()
    End Sub

    Private Sub chkMainteance_CheckedChanged(sender As Object, e As EventArgs) Handles chkMainteance.CheckedChanged
        mRole.Maint_Maintenance_Modules.SelectAll(chkMainteance.Checked)
        dgMaintenance.DataSource = mRole.Maint_Maintenance_Modules
        dgMaintenance.DataBind()
        VisibilityCode()
    End Sub

    Private Sub chkADSBReviewMeeting_CheckedChanged(sender As Object, e As EventArgs) Handles chkADSBReviewMeeting.CheckedChanged
        Dim IsADSBReviewMeeting As Boolean
        If chkADSBReviewMeeting.Checked = True Then
            IsADSBReviewMeeting = True
        Else
            IsADSBReviewMeeting = False
        End If
        mRole.ADSBReviewMeeting_Modules.SelectAll(IsADSBReviewMeeting)
        dgADSBReviewMeeting.DataSource = mRole.ADSBReviewMeeting_Modules
        dgADSBReviewMeeting.DataBind()

        VisibilityCode()
    End Sub

    Private Sub chkMaintenanceReports_CheckedChanged(sender As Object, e As EventArgs) Handles chkMaintenanceReports.CheckedChanged
        mRole.Maint_Reports_Modules.SelectAll(chkMaintenanceReports.Checked)
        dgMaintenanceReports.DataSource = mRole.Maint_Reports_Modules
        dgMaintenanceReports.DataBind()
    End Sub

    Private Sub chkCalibration_CheckedChanged(sender As Object, e As EventArgs) Handles chkCalibration.CheckedChanged
        mRole.Calibration_Modules.SelectAll(chkCalibration.Checked)
        dgCalibration.DataSource = mRole.Calibration_Modules
        dgCalibration.DataBind()
    End Sub

    Private Sub chkManual_CheckedChanged(sender As Object, e As EventArgs) Handles chkManual.CheckedChanged
        mRole.Manual_Modules.SelectAll(chkManual.Checked)
        dgManual.DataSource = mRole.Manual_Modules
        dgManual.DataBind()
    End Sub

    Private Sub chkAudit1_CheckedChanged(sender As Object, e As EventArgs) Handles chkAudit1.CheckedChanged
        mRole.QA_Modules.SelectAll(chkAudit1.Checked)
        dgAudit.DataSource = mRole.QA_Modules
        dgAudit.DataBind()
    End Sub

    Private Sub chkMEL_CheckedChanged(sender As Object, e As EventArgs) Handles chkMEL.CheckedChanged
        mRole.MEL_Modules.SelectAll(chkMEL.Checked)
        dgMEL.DataSource = mRole.MEL_Modules
        dgMEL.DataBind()
    End Sub

    Private Sub ckWorkInvoice_CheckedChanged(sender As Object, e As EventArgs) Handles ckWorkInvoice.CheckedChanged
        mRole.Inv_WorkInvoice_Modules.SelectAll(ckWorkInvoice.Checked)
        dgWorkInvoice.DataSource = mRole.Inv_WorkInvoice_Modules
        dgWorkInvoice.DataBind()
        VisibilityCode()
    End Sub

    Private Sub chkReliability_CheckedChanged(sender As Object, e As EventArgs) Handles chkReliability.CheckedChanged
        mRole.Inv_Reliability_Modules.SelectAll(chkReliability.Checked)
        dgReliability.DataSource = mRole.Inv_Reliability_Modules
        dgReliability.DataBind()
    End Sub

    'Added BY VIkrant on 27-Aug-2012 For ALL27082012
    Private Sub chkNewRequisition_CheckedChanged(sender As Object, e As EventArgs) Handles chkNewRequisition.CheckedChanged
        mRole.Inv_New_Requisition_Modules.SelectAll(chkNewRequisition.Checked)
        dgNewRequisition.DataSource = mRole.Inv_New_Requisition_Modules
        dgNewRequisition.DataBind()
    End Sub
    'End

    'Added by Prashant 15-Nov-2012 ALL12112012
    Private Sub chkLineMaintenance_CheckedChanged(sender As Object, e As EventArgs) Handles chkLineMaintenance.CheckedChanged
        mRole.Inv_LineMaintenance_Modules.SelectAll(chkLineMaintenance.Checked)
        dgLineMaintenance.DataSource = mRole.Inv_LineMaintenance_Modules
        dgLineMaintenance.DataBind()
    End Sub

    'Added By Shweta 03-April-2013 For All03042013-1
    Private Sub chkExportInvoice_CheckedChanged(sender As Object, e As EventArgs) Handles chkExportInvoice.CheckedChanged
        mRole.Inv_ExportInvoice_Modules.SelectAll(chkExportInvoice.Checked)
        dgExportInvoice.DataSource = mRole.Inv_ExportInvoice_Modules
        dgExportInvoice.DataBind()
    End Sub

    Private Sub chkWorkOrderInvoice_CheckedChanged(sender As Object, e As EventArgs) Handles chkWorkOrderInvoice.CheckedChanged
        mRole.Inv_nWOInvoice_Modules.SelectAll(chkWorkOrderInvoice.Checked)
        dgWorkOrderInvoice.DataSource = mRole.Inv_nWOInvoice_Modules

        dgWorkOrderInvoice.DataBind()
        VisibilityCode()
    End Sub

    '========== Ajay 26-04-2023=======================
    Private Sub chkMSP_CheckedChanged(sender As Object, e As EventArgs) Handles chkMSP.CheckedChanged
        mRole.Inv_nMSP_Modules.SelectAll(chkMSP.Checked)
        dgMSP.DataSource = mRole.Inv_nMSP_Modules

        dgMSP.DataBind()
        VisibilityCode()
    End Sub

    Private Sub chkEmpCAAuthorization_CheckedChanged(sender As Object, e As EventArgs) Handles chkEmpCAAuthorization.CheckedChanged
        mRole.EmpCAAuthorization_Modules.SelectAll(chkEmpCAAuthorization.Checked)
        dgEmpCAAuthorization.DataSource = mRole.EmpCAAuthorization_Modules

        dgEmpCAAuthorization.DataBind()
        VisibilityCode()
    End Sub

    Private Sub chkDueJobPlanning_CheckedChanged(sender As Object, e As EventArgs) Handles chkDueJobPlanning.CheckedChanged
        mRole.DueJobPlanning_Modules.SelectAll(chkDueJobPlanning.Checked)
        dgDueJobPlanning.DataSource = mRole.DueJobPlanning_Modules

        dgDueJobPlanning.DataBind()
        VisibilityCode()
    End Sub

    'Sankalp 29/7/25
    Private Sub chkCabinDefect_CheckedChanged(sender As Object, e As EventArgs) Handles chkCabinDefect.CheckedChanged
        mRole.CabinDefect_Modules.SelectAll(chkCabinDefect.Checked)
        dgCabinDefect.DataSource = mRole.CabinDefect_Modules

        dgCabinDefect.DataBind()
        VisibilityCode()
    End Sub

    Private Sub chkInventory_CheckedChanged(sender As Object, e As EventArgs) Handles chkInventory.CheckedChanged
        mRole.Inv_Master_Modules.SelectAll(chkInventory.Checked)
        dgMasters.DataSource = mRole.Inv_Master_Modules
        dgMasters.DataBind()

        'mRole.Inv_Requisition_Modules.SelectAll(chkInventory.Checked)
        'dgRequisition.DataSource = mRole.Inv_Requisition_Modules
        'dgRequisition.DataBind()

        mRole.Inv_PurchaseEnquiries_Modules.SelectAll(chkInventory.Checked)
        dgPurchaseEnquiry.DataSource = mRole.Inv_PurchaseEnquiries_Modules
        dgPurchaseEnquiry.DataBind()

        mRole.Inv_PurchaseQuotations_Modules.SelectAll(chkInventory.Checked)
        dgPurchaseQuotation.DataSource = mRole.Inv_PurchaseQuotations_Modules
        dgPurchaseQuotation.DataBind()

        mRole.Inv_PurchaseOrders_Modules.SelectAll(chkInventory.Checked)
        dgPurchaseOrder.DataSource = mRole.Inv_PurchaseOrders_Modules
        dgPurchaseOrder.DataBind()

        mRole.Inv_GoodsReceipts_Modules.SelectAll(chkInventory.Checked)
        dgGoodsReceipt.DataSource = mRole.Inv_GoodsReceipts_Modules
        dgGoodsReceipt.DataBind()

        mRole.Inv_GoodsIssues_Modules.SelectAll(chkInventory.Checked)
        dgGoodsIssue.DataSource = mRole.Inv_GoodsIssues_Modules
        dgGoodsIssue.DataBind()

        mRole.Inv_PurchaseInvoices_Modules.SelectAll(chkInventory.Checked)
        dgPurchaseInvoice.DataSource = mRole.Inv_PurchaseInvoices_Modules
        dgPurchaseInvoice.DataBind()

        mRole.Inv_SalesModules_Modules.SelectAll(chkInventory.Checked)
        dgSalesModules.DataSource = mRole.Inv_SalesModules_Modules
        dgSalesModules.DataBind()

        mRole.Inv_WorkOrder_Modules.SelectAll(chkInventory.Checked)
        dgWorkOrder.DataSource = mRole.Inv_WorkOrder_Modules
        dgWorkOrder.DataBind()

        mRole.Inv_Reports_Modules.SelectAll(chkInventory.Checked)
        dgInventoryReports.DataSource = mRole.Inv_Reports_Modules
        dgInventoryReports.DataBind()

        mRole.Calibration_Modules.SelectAll(chkInventory.Checked)
        dgCalibration.DataSource = mRole.Calibration_Modules
        dgCalibration.DataBind()

        'mRole.Inv_WorkInvoice_Modules.SelectAll(chkInventory.Checked)
        'dgWorkInvoice.DataSource = mRole.Inv_WorkInvoice_Modules
        'dgWorkInvoice.DataBind()

        mRole.Inv_Reliability_Modules.SelectAll(chkInventory.Checked)
        dgReliability.DataSource = mRole.Inv_Reliability_Modules
        dgReliability.DataBind()

        'Added BY VIkrant on 27-Aug-2012 For ALL27082012
        If AppSettings("NewRequisition") = "True" Then 'Added on 29-Aug-2012
            mRole.Inv_New_Requisition_Modules.SelectAll(chkInventory.Checked)
            dgNewRequisition.DataSource = mRole.Inv_New_Requisition_Modules
            dgNewRequisition.DataBind()
        Else
            mRole.Inv_Requisition_Modules.SelectAll(chkInventory.Checked)
            dgRequisition.DataSource = mRole.Inv_Requisition_Modules
            dgRequisition.DataBind()
        End If
        'End

        'Payment Advice
        mRole.PaymentAdvice_Modules.SelectAll(chkPaymentAdvice.Checked)
        dgPaymentAdvice.DataSource = mRole.PaymentAdvice_Modules
        dgPaymentAdvice.DataBind()
        'End

        'Added by Saylee on 26-May-2020 , LOCKDOWN 4.0
        mRole.MaintenanceDashboard_Modules.SelectAll(chkMaintenanceDashboards.Checked)
        dgMaintenanceDashboard.DataSource = mRole.MaintenanceDashboard_Modules
        dgMaintenanceDashboard.DataBind()

        mRole.InventoryDashboard_Modules.SelectAll(chkInventoryDashboards.Checked)
        dgInventoryDashboard.DataSource = mRole.InventoryDashboard_Modules
        dgInventoryDashboard.DataBind()
        'End


        mRole.Inv_nWOInvoice_Modules.SelectAll(chkInventory.Checked)
        dgWorkOrderInvoice.DataSource = mRole.Inv_WorkOrder_Modules
        dgWorkOrderInvoice.DataBind()

        '======== Ajay 26-04-2023 ==============
        mRole.Inv_nMSP_Modules.SelectAll(chkInventory.Checked)
        dgMSP.DataSource = mRole.Inv_nMSP_Modules
        dgMSP.DataBind()

        'Added by Prashant 15-Nov-2012 ALL12112012
        'mRole.Inv_LineMaintenance_Modules.SelectAll(chkInventory.Checked)
        'dgLineMaintenance.DataSource = mRole.Inv_LineMaintenance_Modules
        'dgLineMaintenance.DataBind()

        'Added By Shweta 03-April-2013 For All03042013-1
        mRole.Inv_ExportInvoice_Modules.SelectAll(chkInventory.Checked)
        dgExportInvoice.DataSource = mRole.Inv_ExportInvoice_Modules
        dgExportInvoice.DataBind()
        If chkInventory.Checked = True Then
            chkInventory.Checked = True
            chkMasters.Checked = True
            chkPurchaseEnquiry.Checked = True
            chkPurchaseQuotation.Checked = True
            chkPurchaseOrder.Checked = True
            chkGoodsReceipt.Checked = True
            chkGoodsIssue.Checked = True
            chkPurchaseInvoice.Checked = True
            chkSalesModules.Checked = True
            chkCalibration.Checked = True
            ckWorkInvoice.Checked = True
            chkLineMaintenance.Checked = True
            chkExportInvoice.Checked = True
            chkInventoryReports.Checked = True
            chkReliability.Checked = True
            chkPaymentAdvice.Checked = True
            chkInventoryDashboards.Checked = True
        Else
            chkInventory.Checked = False
            chkMasters.Checked = False
            chkPurchaseEnquiry.Checked = False
            chkPurchaseQuotation.Checked = False
            chkPurchaseOrder.Checked = False
            chkGoodsReceipt.Checked = False
            chkGoodsIssue.Checked = False
            chkPurchaseInvoice.Checked = False
            chkSalesModules.Checked = False
            chkCalibration.Checked = False
            ckWorkInvoice.Checked = False
            chkLineMaintenance.Checked = False
            chkExportInvoice.Checked = False
            chkInventoryReports.Checked = False
            chkReliability.Checked = False
            chkPaymentAdvice.Checked = False
            chkInventoryDashboards.Checked = False
        End If
    End Sub

    Private Sub chkMaintenance_CheckedChanged(sender As Object, e As EventArgs) Handles chkMaintenance.CheckedChanged

        mRole.Maint_Master_Modules.SelectAll(chkMaintenance.Checked)
        dgMaintMasters.DataSource = mRole.Maint_Master_Modules
        dgMaintMasters.DataBind()

        mRole.Maint_Maintenance_Modules.SelectAll(chkMaintenance.Checked)
        dgMaintenance.DataSource = mRole.Maint_Maintenance_Modules
        dgMaintenance.DataBind()

        mRole.Maint_Reports_Modules.SelectAll(chkMaintenance.Checked)
        dgMaintenanceReports.DataSource = mRole.Maint_Reports_Modules
        dgMaintenanceReports.DataBind()


        mRole.Inv_WorkOrder_Modules.SelectAll(chkMaintenance.Checked)
        dgWorkOrder.DataSource = mRole.Inv_WorkOrder_Modules

        '=========== Ajay 26-04-2023 =============
        mRole.Inv_nMSP_Modules.SelectAll(chkMaintenance.Checked)
        dgMSP.DataSource = mRole.Inv_nMSP_Modules
        dgMSP.DataBind()

        mRole.Inv_nWOInvoice_Modules.SelectAll(chkMaintenance.Checked)
        dgWorkOrderInvoice.DataSource = mRole.Inv_nWOInvoice_Modules
        dgWorkOrderInvoice.DataBind()

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            dgWorkOrder.Columns(1).HeaderText = "Engineering Order"
        Else
            dgWorkOrder.Columns(1).HeaderText = "Work Order"
        End If

        dgWorkOrder.DataBind()

        mRole.QA_Modules.SelectAll(chkMaintenance.Checked)
        dgAudit.DataSource = mRole.QA_Modules
        dgAudit.DataBind()

        mRole.MEL_Modules.SelectAll(chkMaintenance.Checked)
        dgMEL.DataSource = mRole.MEL_Modules
        dgMEL.DataBind()

        mRole.Discrepancy_Modules.SelectAll(chkMaintenance.Checked)
        dgDiscrepancy.DataSource = mRole.Discrepancy_Modules
        dgDiscrepancy.DataBind()

        mRole.Manual_Modules.SelectAll(chkMaintenance.Checked)
        dgManual.DataSource = mRole.Manual_Modules
        dgManual.DataBind()

        mRole.Maint_MPD_Modules.SelectAll(chkMaintenance.Checked)
        dgMPD.DataSource = mRole.Maint_MPD_Modules
        dgMPD.DataBind()

        mRole.Maint_CWP_Modules.SelectAll(chkMaintenance.Checked)
        dgCWP.DataSource = mRole.Maint_CWP_Modules
        dgCWP.DataBind()

        'D&BChart
        mRole.DentBuckleChart_Modules.SelectAll(chkMaintenance.Checked)
        dgDentBuckleChart.DataSource = mRole.DentBuckleChart_Modules
        dgDentBuckleChart.DataBind()
        'End
        'Hangar Planning
        mRole.Hangar_Modules.SelectAll(chkMaintenance.Checked)
        dgHangar.DataSource = mRole.Hangar_Modules
        dgHangar.DataBind()
        VisibilityCode()
        'End

        'Company Document Added by Vikrant On 12-Oct-2021 For ALL12102021
        mRole.CompanyDocument_Modules.SelectAll(chkMaintenance.Checked)
        dgCompanyDocument.DataSource = mRole.CompanyDocument_Modules
        dgCompanyDocument.DataBind()
        VisibilityCode()
        'End

        'Company Document Added by Shital On 29-Nov-2021
        mRole.ComponentReservation_Modules.SelectAll(chkMaintenance.Checked)
        dgComponentReservation.DataSource = mRole.ComponentReservation_Modules
        dgComponentReservation.DataBind()
        'End

        mRole.InfoDisplay_Modules.SelectAll(chkInfoDisplay.Checked)
        dgInfoDisplay.DataSource = mRole.InfoDisplay_Modules
        dgInfoDisplay.DataBind()

        'Spare Maint Added by Saylee on  18-Aug-2020, LockDown 4.0
        mRole.SpareMaint_Maintenance_Modules.SelectAll(chkMaintenance.Checked)
        dgSpareMaint.DataSource = mRole.SpareMaint_Maintenance_Modules
        dgSpareMaint.DataBind()


        'For Maintenance Dashboard Added By Baba 
        mRole.MaintenanceDashboard_Modules.SelectAll(chkMaintenance.Checked)
        dgMaintenanceDashboard.DataSource = mRole.MaintenanceDashboard_Modules
        dgMaintenanceDashboard.DataBind()

        'For AD/SB Added By Sachin
        Dim IsADSBReviewMeeting As Boolean
        If chkADSBReviewMeeting.Checked = True Then
            IsADSBReviewMeeting = True
        Else
            IsADSBReviewMeeting = False
        End If
        mRole.ADSBReviewMeeting_Modules.SelectAll(chkMaintenance.Checked)
        dgADSBReviewMeeting.DataSource = mRole.ADSBReviewMeeting_Modules
        dgADSBReviewMeeting.DataBind()
        VisibilityCode()
        '****************************************

        If chkMaintenance.Checked = True Then
            chkMaintMasters.Checked = True
            chkMainteance.Checked = True
            chkManual.Checked = True
            chkWorkOrder.Checked = True
            chkAudit1.Checked = True
            chkMEL.Checked = True
            chkMaintenanceReports.Checked = True
            chkMPD.Checked = True
            chkCWP.Checked = True
            chkDentBuckleChart.Checked = True 'D&BChart
            chkHangar.Checked = True 'Hangar Planning
            chkCompanyDocument.Checked = True 'Company Document Added by Vikrant On 12-Oct-2021 For ALL12102021
            chkComponentReservation.Checked = True 'Component Reservation Added by Shital On 29-Nov-2021
            chkWorkOrderInvoice.Checked = True
            chkMaintenanceDashboards.Checked = True
            chkADSBReviewMeeting.Checked = True
            chkSpareMaint.Checked = True
        Else
            chkMaintMasters.Checked = False
            chkMainteance.Checked = False
            chkManual.Checked = False
            chkWorkOrder.Checked = False
            chkAudit1.Checked = False
            chkMEL.Checked = False
            chkMaintenanceReports.Checked = False
            chkMPD.Checked = False
            chkCWP.Checked = False
            chkDentBuckleChart.Checked = False 'D&BChart
            chkHangar.Checked = False  'Hangar Planning
            chkCompanyDocument.Checked = False 'Company Document Added by Vikrant On 12-Oct-2021 For ALL12102021
            chkComponentReservation.Checked = False 'Component Reservation Added by Shital On 29-Nov-2021
            chkWorkOrderInvoice.Checked = False
            chkMaintenanceDashboards.Checked = False
            chkADSBReviewMeeting.Checked = False
            chkSpareMaint.Checked = False

        End If
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

    Private Sub chkDiscrepancy_CheckedChanged(sender As Object, e As EventArgs) Handles chkDiscrepancy.CheckedChanged
        mRole.Discrepancy_Modules.SelectAll(chkDiscrepancy.Checked)
        dgDiscrepancy.DataSource = mRole.Discrepancy_Modules
        dgDiscrepancy.DataBind()
        VisibilityCode()
    End Sub

    Private Sub Project_SelectAll(sender As Object, e As EventArgs) Handles chkProject.CheckedChanged

        Try

            mRole.Project_Modules.SelectAll(chkProject.Checked)

            GV_Project.DataSource = mRole.Project_Modules
            GV_Project.DataBind()

            VisibilityCode()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#End Region

End Class