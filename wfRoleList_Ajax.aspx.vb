Public Class wfRoleList_Ajax
    Inherits System.Web.UI.Page



#Region " Variable Declaration "
    Public mRoleList As RoleList
    Public mRole As Role
    Dim RoleName As String
    'Added by Vikrant on 4-AUG-2011
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mRoleList = CType(Session("mRoleList"), RoleList)
        mRole = CType(Session("mRole"), Role)
        RoleName = Session("RoleName")
    End Sub
    Private Sub SetSession()
        Session("mRoleList") = mRoleList
        Session("mRole") = mRole
        Session("RoleName") = RoleName
    End Sub
    Private Sub RemoveSession()
        Session.Remove("RoleName")
    End Sub
    Private Sub NewRecord()
        mRole = Role.NewRole
        Session("mRole") = mRole
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mRole = Role.GetRole(mId, HttpContext.Current.User.Identity.Name)
        Session("mRole") = mRole
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        '''''msg1.ReplacePage = "wfRoleList.aspx?MsgResult=0&BackPage="
        '''''Session("sender") = "Delete"
        '''''msg1.Show()
        dgRoleList.DataSource = mRoleList
        dgRoleList.DataBind()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")

        mRole = Role.GetRole(mId)
        Session("mRole") = mRole
    End Sub
    'Added By Vikrant On 19-Feb-2021 For ALL19022021
    Private Sub CreateCopy(ByVal mId As Guid)
        Dim tempRole As SI.UTILITY.Role = SI.UTILITY.Role.GetRole(mId)
        mRole = SI.UTILITY.Role.NewRole
        'mRole = tempRole., mRole.RoleID
        'mRole.Name = ""
        For i As Integer = 0 To tempRole.Calibration_Modules.Count - 1
            mRole.Calibration_Modules.Item(i).IsSelectedView = tempRole.Calibration_Modules.Item(i).IsSelectedView
            mRole.Calibration_Modules.Item(i).IsSelectedPrint = tempRole.Calibration_Modules.Item(i).IsSelectedPrint
            mRole.Calibration_Modules.Item(i).IsSelectedNew = tempRole.Calibration_Modules.Item(i).IsSelectedNew
            mRole.Calibration_Modules.Item(i).IsSelectedEdit = tempRole.Calibration_Modules.Item(i).IsSelectedEdit
            mRole.Calibration_Modules.Item(i).IsSelectedDelete = tempRole.Calibration_Modules.Item(i).IsSelectedDelete
        Next
        For i As Integer = 0 To tempRole.DentBuckleChart_Modules.Count - 1
            mRole.DentBuckleChart_Modules.Item(i).IsSelectedView = tempRole.DentBuckleChart_Modules.Item(i).IsSelectedView
            mRole.DentBuckleChart_Modules.Item(i).IsSelectedPrint = tempRole.DentBuckleChart_Modules.Item(i).IsSelectedPrint
            mRole.DentBuckleChart_Modules.Item(i).IsSelectedNew = tempRole.DentBuckleChart_Modules.Item(i).IsSelectedNew
            mRole.DentBuckleChart_Modules.Item(i).IsSelectedEdit = tempRole.DentBuckleChart_Modules.Item(i).IsSelectedEdit
            mRole.DentBuckleChart_Modules.Item(i).IsSelectedDelete = tempRole.DentBuckleChart_Modules.Item(i).IsSelectedDelete
            mRole.DentBuckleChart_Modules.Item(i).IsSelectedAuthorized = tempRole.DentBuckleChart_Modules.Item(i).IsSelectedAuthorized
        Next
        'For i As Integer = 0 To tempRole.EntryModules.Count - 1
        '    mRole.Inv_Master_Modules.Item(i).IsSelectedView = tempRole.Inv_Master_Modules.Item(i).IsSelectedView
        '    mRole.Inv_Master_Modules.Item(i).IsSelectedPrint = tempRole.Inv_Master_Modules.Item(i).IsSelectedPrint
        '    mRole.Inv_Master_Modules.Item(i).IsSelectedNew = tempRole.Inv_Master_Modules.Item(i).IsSelectedNew
        '    mRole.Inv_Master_Modules.Item(i).IsSelectedEdit = tempRole.Inv_Master_Modules.Item(i).IsSelectedEdit
        '    mRole.Inv_Master_Modules.Item(i).IsSelectedDelete = tempRole.Inv_Master_Modules.Item(i).IsSelectedDelete
        '    mRole.Inv_Master_Modules.Item(i).IsSelectedAuthorized = tempRole.Inv_Master_Modules.Item(i).IsSelectedAuthorized
        'Next
        'For i As Integer = 0 To tempRole.FDTL_Modules.Count - 1
        '    mRole.FDTL_Modules.Add(tempRole.FDTL_Modules(i), mRole.RoleID)
        'Next
        For i As Integer = 0 To tempRole.Hangar_Modules.Count - 1
            mRole.Hangar_Modules.Item(i).IsSelectedView = tempRole.Hangar_Modules.Item(i).IsSelectedView
            mRole.Hangar_Modules.Item(i).IsSelectedPrint = tempRole.Hangar_Modules.Item(i).IsSelectedPrint
            mRole.Hangar_Modules.Item(i).IsSelectedNew = tempRole.Hangar_Modules.Item(i).IsSelectedNew
            mRole.Hangar_Modules.Item(i).IsSelectedEdit = tempRole.Hangar_Modules.Item(i).IsSelectedEdit
            mRole.Hangar_Modules.Item(i).IsSelectedDelete = tempRole.Hangar_Modules.Item(i).IsSelectedDelete
            mRole.Hangar_Modules.Item(i).IsSelectedAuthorized = tempRole.Hangar_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.InfoDisplay_Modules.Count - 1
            mRole.InfoDisplay_Modules.Item(i).IsSelectedView = tempRole.InfoDisplay_Modules.Item(i).IsSelectedView
            mRole.InfoDisplay_Modules.Item(i).IsSelectedPrint = tempRole.InfoDisplay_Modules.Item(i).IsSelectedPrint
            mRole.InfoDisplay_Modules.Item(i).IsSelectedNew = tempRole.InfoDisplay_Modules.Item(i).IsSelectedNew
            mRole.InfoDisplay_Modules.Item(i).IsSelectedEdit = tempRole.InfoDisplay_Modules.Item(i).IsSelectedEdit
            mRole.InfoDisplay_Modules.Item(i).IsSelectedDelete = tempRole.InfoDisplay_Modules.Item(i).IsSelectedDelete
            mRole.InfoDisplay_Modules.Item(i).IsSelectedAuthorized = tempRole.InfoDisplay_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Inv_ExportInvoice_Modules.Count - 1
            mRole.Inv_ExportInvoice_Modules.Item(i).IsSelectedView = tempRole.Inv_ExportInvoice_Modules.Item(i).IsSelectedView
            mRole.Inv_ExportInvoice_Modules.Item(i).IsSelectedPrint = tempRole.Inv_ExportInvoice_Modules.Item(i).IsSelectedPrint
            mRole.Inv_ExportInvoice_Modules.Item(i).IsSelectedNew = tempRole.Inv_ExportInvoice_Modules.Item(i).IsSelectedNew
            mRole.Inv_ExportInvoice_Modules.Item(i).IsSelectedEdit = tempRole.Inv_ExportInvoice_Modules.Item(i).IsSelectedEdit
            mRole.Inv_ExportInvoice_Modules.Item(i).IsSelectedDelete = tempRole.Inv_ExportInvoice_Modules.Item(i).IsSelectedDelete
            mRole.Inv_ExportInvoice_Modules.Item(i).IsSelectedAuthorized = tempRole.Inv_ExportInvoice_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Inv_GoodsIssues_Modules.Count - 1
            mRole.Inv_GoodsIssues_Modules.Item(i).IsSelectedView = tempRole.Inv_GoodsIssues_Modules.Item(i).IsSelectedView
            mRole.Inv_GoodsIssues_Modules.Item(i).IsSelectedPrint = tempRole.Inv_GoodsIssues_Modules.Item(i).IsSelectedPrint
            mRole.Inv_GoodsIssues_Modules.Item(i).IsSelectedNew = tempRole.Inv_GoodsIssues_Modules.Item(i).IsSelectedNew
            mRole.Inv_GoodsIssues_Modules.Item(i).IsSelectedEdit = tempRole.Inv_GoodsIssues_Modules.Item(i).IsSelectedEdit
            mRole.Inv_GoodsIssues_Modules.Item(i).IsSelectedDelete = tempRole.Inv_GoodsIssues_Modules.Item(i).IsSelectedDelete
            mRole.Inv_GoodsIssues_Modules.Item(i).IsSelectedAuthorized = tempRole.Inv_GoodsIssues_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Inv_GoodsReceipts_Modules.Count - 1
            mRole.Inv_GoodsReceipts_Modules.Item(i).IsSelectedView = tempRole.Inv_GoodsReceipts_Modules.Item(i).IsSelectedView
            mRole.Inv_GoodsReceipts_Modules.Item(i).IsSelectedPrint = tempRole.Inv_GoodsReceipts_Modules.Item(i).IsSelectedPrint
            mRole.Inv_GoodsReceipts_Modules.Item(i).IsSelectedNew = tempRole.Inv_GoodsReceipts_Modules.Item(i).IsSelectedNew
            mRole.Inv_GoodsReceipts_Modules.Item(i).IsSelectedEdit = tempRole.Inv_GoodsReceipts_Modules.Item(i).IsSelectedEdit
            mRole.Inv_GoodsReceipts_Modules.Item(i).IsSelectedDelete = tempRole.Inv_GoodsReceipts_Modules.Item(i).IsSelectedDelete
            mRole.Inv_GoodsReceipts_Modules.Item(i).IsSelectedAuthorized = tempRole.Inv_GoodsReceipts_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Inv_LineMaintenance_Modules.Count - 1
            mRole.Inv_LineMaintenance_Modules.Item(i).IsSelectedView = tempRole.Inv_LineMaintenance_Modules.Item(i).IsSelectedView
            mRole.Inv_LineMaintenance_Modules.Item(i).IsSelectedPrint = tempRole.Inv_LineMaintenance_Modules.Item(i).IsSelectedPrint
            mRole.Inv_LineMaintenance_Modules.Item(i).IsSelectedNew = tempRole.Inv_LineMaintenance_Modules.Item(i).IsSelectedNew
            mRole.Inv_LineMaintenance_Modules.Item(i).IsSelectedEdit = tempRole.Inv_LineMaintenance_Modules.Item(i).IsSelectedEdit
            mRole.Inv_LineMaintenance_Modules.Item(i).IsSelectedDelete = tempRole.Inv_LineMaintenance_Modules.Item(i).IsSelectedDelete
            mRole.Inv_LineMaintenance_Modules.Item(i).IsSelectedAuthorized = tempRole.Inv_LineMaintenance_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Inv_Master_Modules.Count - 1
            mRole.Inv_Master_Modules.Item(i).IsSelectedView = tempRole.Inv_Master_Modules.Item(i).IsSelectedView
            mRole.Inv_Master_Modules.Item(i).IsSelectedPrint = tempRole.Inv_Master_Modules.Item(i).IsSelectedPrint
            mRole.Inv_Master_Modules.Item(i).IsSelectedNew = tempRole.Inv_Master_Modules.Item(i).IsSelectedNew
            mRole.Inv_Master_Modules.Item(i).IsSelectedEdit = tempRole.Inv_Master_Modules.Item(i).IsSelectedEdit
            mRole.Inv_Master_Modules.Item(i).IsSelectedDelete = tempRole.Inv_Master_Modules.Item(i).IsSelectedDelete
        Next
        For i As Integer = 0 To tempRole.Inv_New_Requisition_Modules.Count - 1
            mRole.Inv_New_Requisition_Modules.Item(i).IsSelectedView = tempRole.Inv_New_Requisition_Modules.Item(i).IsSelectedView
            mRole.Inv_New_Requisition_Modules.Item(i).IsSelectedPrint = tempRole.Inv_New_Requisition_Modules.Item(i).IsSelectedPrint
            mRole.Inv_New_Requisition_Modules.Item(i).IsSelectedNew = tempRole.Inv_New_Requisition_Modules.Item(i).IsSelectedNew
            mRole.Inv_New_Requisition_Modules.Item(i).IsSelectedEdit = tempRole.Inv_New_Requisition_Modules.Item(i).IsSelectedEdit
            mRole.Inv_New_Requisition_Modules.Item(i).IsSelectedDelete = tempRole.Inv_New_Requisition_Modules.Item(i).IsSelectedDelete
            mRole.Inv_New_Requisition_Modules.Item(i).IsSelectedAuthorized = tempRole.Inv_New_Requisition_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Inv_PurchaseEnquiries_Modules.Count - 1
            mRole.Inv_PurchaseEnquiries_Modules.Item(i).IsSelectedView = tempRole.Inv_PurchaseEnquiries_Modules.Item(i).IsSelectedView
            mRole.Inv_PurchaseEnquiries_Modules.Item(i).IsSelectedPrint = tempRole.Inv_PurchaseEnquiries_Modules.Item(i).IsSelectedPrint
            mRole.Inv_PurchaseEnquiries_Modules.Item(i).IsSelectedNew = tempRole.Inv_PurchaseEnquiries_Modules.Item(i).IsSelectedNew
            mRole.Inv_PurchaseEnquiries_Modules.Item(i).IsSelectedEdit = tempRole.Inv_PurchaseEnquiries_Modules.Item(i).IsSelectedEdit
            mRole.Inv_PurchaseEnquiries_Modules.Item(i).IsSelectedDelete = tempRole.Inv_PurchaseEnquiries_Modules.Item(i).IsSelectedDelete
            mRole.Inv_PurchaseEnquiries_Modules.Item(i).IsSelectedAuthorized = tempRole.Inv_PurchaseEnquiries_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Inv_PurchaseInvoices_Modules.Count - 1
            mRole.Inv_PurchaseInvoices_Modules.Item(i).IsSelectedView = tempRole.Inv_PurchaseInvoices_Modules.Item(i).IsSelectedView
            mRole.Inv_PurchaseInvoices_Modules.Item(i).IsSelectedPrint = tempRole.Inv_PurchaseInvoices_Modules.Item(i).IsSelectedPrint
            mRole.Inv_PurchaseInvoices_Modules.Item(i).IsSelectedNew = tempRole.Inv_PurchaseInvoices_Modules.Item(i).IsSelectedNew
            mRole.Inv_PurchaseInvoices_Modules.Item(i).IsSelectedEdit = tempRole.Inv_PurchaseInvoices_Modules.Item(i).IsSelectedEdit
            mRole.Inv_PurchaseInvoices_Modules.Item(i).IsSelectedDelete = tempRole.Inv_PurchaseInvoices_Modules.Item(i).IsSelectedDelete
            mRole.Inv_PurchaseInvoices_Modules.Item(i).IsSelectedAuthorized = tempRole.Inv_PurchaseInvoices_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Inv_PurchaseOrders_Modules.Count - 1
            mRole.Inv_PurchaseOrders_Modules.Item(i).IsSelectedView = tempRole.Inv_PurchaseOrders_Modules.Item(i).IsSelectedView
            mRole.Inv_PurchaseOrders_Modules.Item(i).IsSelectedPrint = tempRole.Inv_PurchaseOrders_Modules.Item(i).IsSelectedPrint
            mRole.Inv_PurchaseOrders_Modules.Item(i).IsSelectedNew = tempRole.Inv_PurchaseOrders_Modules.Item(i).IsSelectedNew
            mRole.Inv_PurchaseOrders_Modules.Item(i).IsSelectedEdit = tempRole.Inv_PurchaseOrders_Modules.Item(i).IsSelectedEdit
            mRole.Inv_PurchaseOrders_Modules.Item(i).IsSelectedDelete = tempRole.Inv_PurchaseOrders_Modules.Item(i).IsSelectedDelete
            mRole.Inv_PurchaseOrders_Modules.Item(i).IsSelectedAuthorized = tempRole.Inv_PurchaseOrders_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Inv_PurchaseQuotations_Modules.Count - 1
            mRole.Inv_PurchaseQuotations_Modules.Item(i).IsSelectedView = tempRole.Inv_PurchaseQuotations_Modules.Item(i).IsSelectedView
            mRole.Inv_PurchaseQuotations_Modules.Item(i).IsSelectedPrint = tempRole.Inv_PurchaseQuotations_Modules.Item(i).IsSelectedPrint
            mRole.Inv_PurchaseQuotations_Modules.Item(i).IsSelectedNew = tempRole.Inv_PurchaseQuotations_Modules.Item(i).IsSelectedNew
            mRole.Inv_PurchaseQuotations_Modules.Item(i).IsSelectedEdit = tempRole.Inv_PurchaseQuotations_Modules.Item(i).IsSelectedEdit
            mRole.Inv_PurchaseQuotations_Modules.Item(i).IsSelectedDelete = tempRole.Inv_PurchaseQuotations_Modules.Item(i).IsSelectedDelete
            mRole.Inv_PurchaseQuotations_Modules.Item(i).IsSelectedAuthorized = tempRole.Inv_PurchaseQuotations_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Inv_Reliability_Modules.Count - 1
            mRole.Inv_Reliability_Modules.Item(i).IsSelectedView = tempRole.Inv_Reliability_Modules.Item(i).IsSelectedView
            mRole.Inv_Reliability_Modules.Item(i).IsSelectedPrint = tempRole.Inv_Reliability_Modules.Item(i).IsSelectedPrint
            mRole.Inv_Reliability_Modules.Item(i).IsSelectedNew = tempRole.Inv_Reliability_Modules.Item(i).IsSelectedNew
            mRole.Inv_Reliability_Modules.Item(i).IsSelectedEdit = tempRole.Inv_Reliability_Modules.Item(i).IsSelectedEdit
            mRole.Inv_Reliability_Modules.Item(i).IsSelectedDelete = tempRole.Inv_Reliability_Modules.Item(i).IsSelectedDelete
        Next
        For i As Integer = 0 To tempRole.Inv_Reports_Modules.Count - 1
            mRole.Inv_Reports_Modules.Item(i).IsSelectedView = tempRole.Inv_Reports_Modules.Item(i).IsSelectedView
        Next
        For i As Integer = 0 To tempRole.Inv_Requisition_Modules.Count - 1
            mRole.Inv_Requisition_Modules.Item(i).IsSelectedView = tempRole.Inv_Requisition_Modules.Item(i).IsSelectedView
            mRole.Inv_Requisition_Modules.Item(i).IsSelectedPrint = tempRole.Inv_Requisition_Modules.Item(i).IsSelectedPrint
            mRole.Inv_Requisition_Modules.Item(i).IsSelectedNew = tempRole.Inv_Requisition_Modules.Item(i).IsSelectedNew
            mRole.Inv_Requisition_Modules.Item(i).IsSelectedEdit = tempRole.Inv_Requisition_Modules.Item(i).IsSelectedEdit
            mRole.Inv_Requisition_Modules.Item(i).IsSelectedDelete = tempRole.Inv_Requisition_Modules.Item(i).IsSelectedDelete
            mRole.Inv_Requisition_Modules.Item(i).IsSelectedAuthorized = tempRole.Inv_Requisition_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Inv_SalesModules_Modules.Count - 1
            mRole.Inv_SalesModules_Modules.Item(i).IsSelectedView = tempRole.Inv_SalesModules_Modules.Item(i).IsSelectedView
            mRole.Inv_SalesModules_Modules.Item(i).IsSelectedPrint = tempRole.Inv_SalesModules_Modules.Item(i).IsSelectedPrint
            mRole.Inv_SalesModules_Modules.Item(i).IsSelectedNew = tempRole.Inv_SalesModules_Modules.Item(i).IsSelectedNew
            mRole.Inv_SalesModules_Modules.Item(i).IsSelectedEdit = tempRole.Inv_SalesModules_Modules.Item(i).IsSelectedEdit
            mRole.Inv_SalesModules_Modules.Item(i).IsSelectedDelete = tempRole.Inv_SalesModules_Modules.Item(i).IsSelectedDelete
            mRole.Inv_SalesModules_Modules.Item(i).IsSelectedAuthorized = tempRole.Inv_SalesModules_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Inv_WorkInvoice_Modules.Count - 1
            mRole.Inv_WorkInvoice_Modules.Item(i).IsSelectedView = tempRole.Inv_WorkInvoice_Modules.Item(i).IsSelectedView
            mRole.Inv_WorkInvoice_Modules.Item(i).IsSelectedPrint = tempRole.Inv_WorkInvoice_Modules.Item(i).IsSelectedPrint
            mRole.Inv_WorkInvoice_Modules.Item(i).IsSelectedNew = tempRole.Inv_WorkInvoice_Modules.Item(i).IsSelectedNew
            mRole.Inv_WorkInvoice_Modules.Item(i).IsSelectedEdit = tempRole.Inv_WorkInvoice_Modules.Item(i).IsSelectedEdit
            mRole.Inv_WorkInvoice_Modules.Item(i).IsSelectedDelete = tempRole.Inv_WorkInvoice_Modules.Item(i).IsSelectedDelete
            mRole.Inv_WorkInvoice_Modules.Item(i).IsSelectedAuthorized = tempRole.Inv_WorkInvoice_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Inv_WorkOrder_Modules.Count - 1
            mRole.Inv_WorkOrder_Modules.Item(i).IsSelectedView = tempRole.Inv_WorkOrder_Modules.Item(i).IsSelectedView
            mRole.Inv_WorkOrder_Modules.Item(i).IsSelectedPrint = tempRole.Inv_WorkOrder_Modules.Item(i).IsSelectedPrint
            mRole.Inv_WorkOrder_Modules.Item(i).IsSelectedNew = tempRole.Inv_WorkOrder_Modules.Item(i).IsSelectedNew
            mRole.Inv_WorkOrder_Modules.Item(i).IsSelectedEdit = tempRole.Inv_WorkOrder_Modules.Item(i).IsSelectedEdit
            mRole.Inv_WorkOrder_Modules.Item(i).IsSelectedDelete = tempRole.Inv_WorkOrder_Modules.Item(i).IsSelectedDelete
            mRole.Inv_WorkOrder_Modules.Item(i).IsSelectedAuthorized = tempRole.Inv_WorkOrder_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.InventoryDashboard_Modules.Count - 1
            mRole.InventoryDashboard_Modules.Item(i).IsSelectedView = tempRole.InventoryDashboard_Modules.Item(i).IsSelectedView
        Next
        For i As Integer = 0 To tempRole.Maint_CWP_Modules.Count - 1
            mRole.Maint_CWP_Modules.Item(i).IsSelectedView = tempRole.Maint_CWP_Modules.Item(i).IsSelectedView
            mRole.Maint_CWP_Modules.Item(i).IsSelectedPrint = tempRole.Maint_CWP_Modules.Item(i).IsSelectedPrint
            mRole.Maint_CWP_Modules.Item(i).IsSelectedNew = tempRole.Maint_CWP_Modules.Item(i).IsSelectedNew
            mRole.Maint_CWP_Modules.Item(i).IsSelectedEdit = tempRole.Maint_CWP_Modules.Item(i).IsSelectedEdit
            mRole.Maint_CWP_Modules.Item(i).IsSelectedDelete = tempRole.Maint_CWP_Modules.Item(i).IsSelectedDelete
        Next
        For i As Integer = 0 To tempRole.Maint_Maintenance_Modules.Count - 1
            mRole.Maint_Maintenance_Modules.Item(i).IsSelectedView = tempRole.Maint_Maintenance_Modules.Item(i).IsSelectedView
            mRole.Maint_Maintenance_Modules.Item(i).IsSelectedPrint = tempRole.Maint_Maintenance_Modules.Item(i).IsSelectedPrint
            mRole.Maint_Maintenance_Modules.Item(i).IsSelectedNew = tempRole.Maint_Maintenance_Modules.Item(i).IsSelectedNew
            mRole.Maint_Maintenance_Modules.Item(i).IsSelectedEdit = tempRole.Maint_Maintenance_Modules.Item(i).IsSelectedEdit
            mRole.Maint_Maintenance_Modules.Item(i).IsSelectedDelete = tempRole.Maint_Maintenance_Modules.Item(i).IsSelectedDelete
            mRole.Maint_Maintenance_Modules.Item(i).IsSelectedAuthorized = tempRole.Maint_Maintenance_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Maint_Master_Modules.Count - 1
            mRole.Maint_Master_Modules.Item(i).IsSelectedView = tempRole.Maint_Master_Modules.Item(i).IsSelectedView
            mRole.Maint_Master_Modules.Item(i).IsSelectedPrint = tempRole.Maint_Master_Modules.Item(i).IsSelectedPrint
            mRole.Maint_Master_Modules.Item(i).IsSelectedNew = tempRole.Maint_Master_Modules.Item(i).IsSelectedNew
            mRole.Maint_Master_Modules.Item(i).IsSelectedEdit = tempRole.Maint_Master_Modules.Item(i).IsSelectedEdit
            mRole.Maint_Master_Modules.Item(i).IsSelectedDelete = tempRole.Maint_Master_Modules.Item(i).IsSelectedDelete
        Next
        For i As Integer = 0 To tempRole.Maint_MPD_Modules.Count - 1
            mRole.Maint_MPD_Modules.Item(i).IsSelectedView = tempRole.Maint_MPD_Modules.Item(i).IsSelectedView
            mRole.Maint_MPD_Modules.Item(i).IsSelectedPrint = tempRole.Maint_MPD_Modules.Item(i).IsSelectedPrint
            mRole.Maint_MPD_Modules.Item(i).IsSelectedNew = tempRole.Maint_MPD_Modules.Item(i).IsSelectedNew
            mRole.Maint_MPD_Modules.Item(i).IsSelectedEdit = tempRole.Maint_MPD_Modules.Item(i).IsSelectedEdit
            mRole.Maint_MPD_Modules.Item(i).IsSelectedDelete = tempRole.Maint_MPD_Modules.Item(i).IsSelectedDelete
        Next
        For i As Integer = 0 To tempRole.Maint_Reports_Modules.Count - 1
            mRole.Maint_Reports_Modules.Item(i).IsSelectedView = tempRole.Maint_Reports_Modules.Item(i).IsSelectedView
        Next
        For i As Integer = 0 To tempRole.MaintenanceDashboard_Modules.Count - 1
            mRole.MaintenanceDashboard_Modules.Item(i).IsSelectedView = tempRole.MaintenanceDashboard_Modules.Item(i).IsSelectedView
        Next
        For i As Integer = 0 To tempRole.Manual_Modules.Count - 1
            mRole.Manual_Modules.Item(i).IsSelectedView = tempRole.Manual_Modules.Item(i).IsSelectedView
            mRole.Manual_Modules.Item(i).IsSelectedPrint = tempRole.Manual_Modules.Item(i).IsSelectedPrint
            mRole.Manual_Modules.Item(i).IsSelectedNew = tempRole.Manual_Modules.Item(i).IsSelectedNew
            mRole.Manual_Modules.Item(i).IsSelectedEdit = tempRole.Manual_Modules.Item(i).IsSelectedEdit
            mRole.Manual_Modules.Item(i).IsSelectedDelete = tempRole.Manual_Modules.Item(i).IsSelectedDelete
        Next
        For i As Integer = 0 To tempRole.MEL_Modules.Count - 1
            mRole.MEL_Modules.Item(i).IsSelectedView = tempRole.MEL_Modules.Item(i).IsSelectedView
            mRole.MEL_Modules.Item(i).IsSelectedPrint = tempRole.MEL_Modules.Item(i).IsSelectedPrint
            mRole.MEL_Modules.Item(i).IsSelectedNew = tempRole.MEL_Modules.Item(i).IsSelectedNew
            mRole.MEL_Modules.Item(i).IsSelectedEdit = tempRole.MEL_Modules.Item(i).IsSelectedEdit
            mRole.MEL_Modules.Item(i).IsSelectedDelete = tempRole.MEL_Modules.Item(i).IsSelectedDelete
        Next
        For i As Integer = 0 To tempRole.PaymentAdvice_Modules.Count - 1
            mRole.PaymentAdvice_Modules.Item(i).IsSelectedView = tempRole.PaymentAdvice_Modules.Item(i).IsSelectedView
            mRole.PaymentAdvice_Modules.Item(i).IsSelectedPrint = tempRole.PaymentAdvice_Modules.Item(i).IsSelectedPrint
            mRole.PaymentAdvice_Modules.Item(i).IsSelectedNew = tempRole.PaymentAdvice_Modules.Item(i).IsSelectedNew
            mRole.PaymentAdvice_Modules.Item(i).IsSelectedEdit = tempRole.PaymentAdvice_Modules.Item(i).IsSelectedEdit
            mRole.PaymentAdvice_Modules.Item(i).IsSelectedDelete = tempRole.PaymentAdvice_Modules.Item(i).IsSelectedDelete
            mRole.PaymentAdvice_Modules.Item(i).IsSelectedAuthorized = tempRole.PaymentAdvice_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.QA_Modules.Count - 1
            mRole.QA_Modules.Item(i).IsSelectedView = tempRole.QA_Modules.Item(i).IsSelectedView
            mRole.QA_Modules.Item(i).IsSelectedPrint = tempRole.QA_Modules.Item(i).IsSelectedPrint
            mRole.QA_Modules.Item(i).IsSelectedNew = tempRole.QA_Modules.Item(i).IsSelectedNew
            mRole.QA_Modules.Item(i).IsSelectedEdit = tempRole.QA_Modules.Item(i).IsSelectedEdit
            mRole.QA_Modules.Item(i).IsSelectedDelete = tempRole.QA_Modules.Item(i).IsSelectedDelete
        Next
        'For i As Integer = 0 To tempRole.ReportModules.Count - 1
        '    mRole.ReportModules.Add(tempRole.ReportModules(i), mRole.RoleID)
        'Next
        For i As Integer = 0 To tempRole.SpareMaint_Maintenance_Modules.Count - 1
            mRole.SpareMaint_Maintenance_Modules.Item(i).IsSelectedView = tempRole.SpareMaint_Maintenance_Modules.Item(i).IsSelectedView
            mRole.SpareMaint_Maintenance_Modules.Item(i).IsSelectedPrint = tempRole.SpareMaint_Maintenance_Modules.Item(i).IsSelectedPrint
            mRole.SpareMaint_Maintenance_Modules.Item(i).IsSelectedNew = tempRole.SpareMaint_Maintenance_Modules.Item(i).IsSelectedNew
            mRole.SpareMaint_Maintenance_Modules.Item(i).IsSelectedEdit = tempRole.SpareMaint_Maintenance_Modules.Item(i).IsSelectedEdit
            mRole.SpareMaint_Maintenance_Modules.Item(i).IsSelectedDelete = tempRole.SpareMaint_Maintenance_Modules.Item(i).IsSelectedDelete
            mRole.SpareMaint_Maintenance_Modules.Item(i).IsSelectedAuthorized = tempRole.SpareMaint_Maintenance_Modules.Item(i).IsSelectedAuthorized
        Next
        For i As Integer = 0 To tempRole.Tool_Modules.Count - 1
            mRole.Tool_Modules.Item(i).IsSelectedView = tempRole.Tool_Modules.Item(i).IsSelectedView
        Next
        Session("CreateCopy") = True
        Session("CopiedRoleName") = tempRole.Name
        tempRole = Nothing
        Session("mRole") = mRole
        MarkLog(Util.Action.[New], "Role Manager", "By Create Copy", Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    'End
    'End of Code
    Private Sub setObject()
        mRole.Name = Trim(txtFind.Text)
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Try
            Dim str As String
            'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
            str = "document.getElementById('" + cntrl.ClientID + "').focus();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
        Catch ex As Exception
            '
        End Try
    End Sub
    Private Sub CallFindNow()
        RoleName = txtFind.Text
        Session("RoleName") = RoleName
        Call FindNow(txtFind.Text)
    End Sub
    Private Sub FindNow(Optional ByVal mName As String = "")
        dgRoleList.DataSource = Nothing

        mRoleList = RoleList.GetRoleList(Trim(txtFind.Text), , HttpContext.Current.User.Identity.Name)
        Session("mRoleList") = mRoleList

        dgRoleList.DataSource = mRoleList
        dgRoleList.DataBind()
        upnlRoleList.Update()
    End Sub
    Public Sub SetControl()
        RoleName = Session("RoleName")
        txtFind.Text = RoleName
        FindNow(RoleName)
        lbllistroles.Text = "List of Roles as per criteria: " & mRoleList.Count & " Record(s) found."
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        ' '' ''If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
        ' '' ''    Result1 = -1
        ' '' ''Else
        ' '' ''    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        ' '' ''End If
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    ' '' ''If CType(Session("sender"), String) = "Delete" Then
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mRole = CType(Session("mRole"), Role)
                            Role.DeleteRole(mRole.RoleID)
                            FindNow()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                                ' '' ''msg1.ReplacePage = "wfRoleList.aspx?BackPage="
                                ' '' ''msg1.Show()

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                                ' '' ''msg1.ReplacePage = "wfRoleList.aspx?BackPage="
                                ' '' ''msg1.Show()

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")

                            ElseIf ex.Number = 547 Then
                                ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                                ' '' ''msg1.ReplacePage = "wfRoleList.aspx?BackPage="
                                ' '' ''msg1.Show()

                                'Added by Vikrant on 4-AUG-2011
                                MarkLog(Util.Action.Delete, "Role Manager", "Can't Delete : " + mRole.Name + " is currently in use", Util.ErrorType.NoError, mRoleList.Item(mRoleList.CurrentIndex).RoleID, EventLogID)

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If

                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Role Manager", mRole.Name, Util.ErrorType.NoError, mRole.RoleID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    DataFieldBind()
                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    ' '' ''Response.Redirect("wfRoleList.aspx?MsgResult=0&BackPage=")
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    ' '' ''Response.Redirect("wfRoleList.aspx?MsgResult=0&BackPage=")
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            ' '' ''Response.Redirect("wfRoleList.aspx?MsgResult=0&BackPage=")
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mRoleList = RoleList.GetRoleList("", "", HttpContext.Current.User.Identity.Name)
        Session("mRoleList") = mRoleList

        dgRoleList.DataSource = mRoleList
        dgRoleList.DataBind()

        RoleName = Session("RoleName")
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()

        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Vikrant on 4-AUG-2011

        If Not IsPostBack And CType(Session("Sender"), String) = "" Then

            If txtFind.Enabled = True Then
                setFocus(txtFind)
            End If

            Session("MiddleFrame") = "wfRoleList_Ajax.aspx"

            DataFieldBind()
            SetControl()
        End If
        'set the label
        lbllistroles.Text = "List of Roles as per criteria: " & mRoleList.Count & " Record(s) found."
        ' '' ''MessageBoxResult()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCl.Click, btnClose.Click
        'Added by Vikrant on 4-AUG-2011
        MarkLog(Util.Action.Close, "Role Manager", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Session("MiddleFrame") = ""
        mRoleList = Nothing
        Response.Redirect("DashBoard.aspx")
    End Sub

    Private Sub dgRoleList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRoleList.RowCommand

        ''If e.Item.Cells(0).Text = "ID" Or e.Item.Cells(0).Text = "" Then Exit Sub ??
        ''Dim mId As Guid = New Guid(e.Item.Cells(0).Text)

        Dim Idx As Integer = CInt(e.CommandArgument) + dgRoleList.PageIndex * dgRoleList.PageSize
        Dim mId As Guid = mRoleList.Item(Idx).RoleID
        Dim mRName As String = mRoleList.Item(Idx).Name

        If Not User.IsInRole("RoleManagerView") Then 'Added by Saylee on 1-Feb-2013 for ALL01022013
            ' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            ' '' ''msg.ReplacePage = "wfRoleList.aspx?BackPage="
            ' '' ''msg.Show()

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")

            Exit Sub
        End If

        Select Case e.CommandName
            Case "EditLnk"
                Dim mRoleName As String = mRName ''CStr(e.Item.Cells(1).Text)

                EditRecord(mId)

                'Added by Vikrant on 4-AUG-2011
                MarkLog(Util.Action.Edit, "Role Manager", "Role Name : " + mRoleName, Util.ErrorType.NoError, mRoleList.Item(mRoleList.CurrentIndex).RoleID, EventLogID)

                '''''Dim str As String
                '''''str = "<script language='javascript'>openledgersame('wfRole.aspx?BackPage=index.aspx'); </script>"
                '''''ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)

                Dim Str As String
                Str = "openledgersame('wfRole_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", Str, True)

            Case "DeleteLnk"
                Dim mRoleName As String = mRName ''CStr(e.Item.Cells(1).Text)
                If mRoleName = "BTPLAdministrator" Then
                    ' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.DeleteAlert, SIMsgBox.Message_text.DeleteAlert, "You can not delete this entry", MsgBoxStyle.OkOnly)
                    ' '' ''msg.ReplacePage = "wfRoleList.aspx?BackPage="
                    ' '' ''msg.Show()

                    MSGBoxCtrl.show(MSGBox.Message_title.DeleteAlert, MSGBox.Message_text.DeleteAlert, "You can not delete this entry", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    DeleteRecord(mId)
                End If

                'Added By Vikrant On 19-Feb-2021 For ALL19022021
            Case "CreateCopyLnk"
                Dim mRoleName As String = mRName ''CStr(e.Item.Cells(1).Text)

                If mRoleName = "BTPLAdministrator" Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "You can not create copy of this role", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    CreateCopy(mId)
                End If

                Dim Str As String
                Str = "openledgersame('wfRole_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", Str, True)
                'End
        End Select
    End Sub

    Private Sub dgRoleList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgRoleList.PageIndexChanging
        dgRoleList.PageIndex = e.NewPageIndex
        'DataFieldBind()
        dgRoleList.DataSource = mRoleList
        Session("mRoleList") = mRoleList
        dgRoleList.DataBind()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAd.Click
        'Added by Vikrant on 4-AUG-2011

        If Not User.IsInRole("RoleManagerView") Then 'Added by Saylee on 1-Feb-2013 for ALL01022013
            ' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            ' '' ''msg.ReplacePage = "wfRoleList.aspx?BackPage="
            ' '' ''msg.Show()
            Exit Sub
        End If


        MarkLog(Util.Action.[New], "Role Manager", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        NewRecord()

        '''''Dim str As String
        '''''str = "<script language='javascript'>openledgersame('wfRole.aspx?BackPage=index.aspx'); </script>"
        '''''ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)

        Dim Str As String
        Str = "openledgersame('wfRole_Ajax.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", Str, True)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindNow.Click
        dgRoleList.PageIndex = 0
        CallFindNow()
        lbllistroles.Text = "List of Users as per criteria: " & mRoleList.Count & " Record(s) found."
        upnlRoleList.Update()
    End Sub

    'Private Sub dgRoleList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRoleList.PageIndexChanged
    '    dgRoleList.CurrentPageIndex = e.NewPageIndex
    '    DataFieldBind()
    'End Sub

    'Private Sub dgRoleList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRoleList.PageIndexChanged
    '    dgRoleList.CurrentPageIndex = e.NewPageIndex
    '    'DataFieldBind()
    '    dgRoleList.DataSource = mRoleList
    '    Session("mRoleList") = mRoleList
    '    dgRoleList.DataBind()
    'End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

#End Region


End Class