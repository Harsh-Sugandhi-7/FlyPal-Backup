Imports System.Linq
Imports System.Collections.Generic
Imports OfficeOpenXml
Public Class wfSearchCriteriaForApprovedVendorList_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim mApprovedVendorListSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid 'Added by Prashant on 04-Dec-2013
#End Region

#Region " Business Method "
    Private Sub SetValues()
        Dim Category As String = ""
        Dim strVendor As String = ""

        'Vendor
        If txtVendorList.Text.Trim = "" Then
            lblVendor1.Text = "Vendor : All"
        Else
            strVendor = txtVendorList.Text.Trim
            lblVendor1.Text = "Vendor :" & strVendor
        End If
        'Category
        If cmbCategoryList.SelectedIndex > 0 Then
            Category = cmbCategoryList.SelectedItem.Text
            lblCategory1.Text = "Category : " & Category
        Else
            Category = ""
            lblCategory1.Text = "Category : All"
        End If
        mApprovedVendorListSearchingCriteria = lblVendor1.Text + ", " + lblCategory1.Text
    End Sub
    Private Sub ControlVisibility()
        lblVendor1.Visible = IIf(txtVendorList.Enabled = True, True, False)
        lblCategory1.Visible = True
    End Sub
    Public Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim SearchStr1 As String = ""
        Dim SearchStr2 As String = ""
        Dim IsCustomer As Boolean = False
        Dim IsServiceProvider As Boolean = False
        Dim IsSupplier As Boolean = False
        Dim rpt As ApprovedVendorList

        SetValues()

        If cmbCategoryList.SelectedIndex = 0 Then
            myReport = New crptApprovedVendorListAllCategory  'All catagory
        Else
            myReport = New crptApprovedVendorList 'Selected Category
        End If
        'Vendor
        If txtVendorList.Text.Trim <> "" Then
            SearchStr1 = txtVendorList.Text.Trim
        Else
            SearchStr1 = ""
        End If
        'Category
        If cmbCategoryList.SelectedIndex > 0 Then
            SearchStr2 = cmbCategoryList.SelectedItem.Text
            If cmbCategoryList.SelectedIndex = 1 Then IsCustomer = True
            If cmbCategoryList.SelectedIndex = 2 Then IsSupplier = True
            If cmbCategoryList.SelectedIndex = 3 Then IsServiceProvider = True
        Else
            SearchStr2 = ""
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                                     mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                                     mCompanyDetail.WebSite,
                                     ReportName:=IIf(Expression:=AppSettings("ClientCode") = "7AR", "Approved Provider List", "Approved Vendor List"),
                                     SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"),
                                     AppSettings("SINote"), "", "", "", "",
                                     AppSettings("Logo"), SearchStr11:=AppSettings("ClientCode"))       'Changed By Utkarsh On 08-Apr-2011

        rpt = ApprovedVendorList.GetVendorstList(0, SearchStr1, , , , , , IsCustomer, IsSupplier, IsServiceProvider, IsExcel)

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1243)
        End If

        If IsExcel = False Then     'PDF format
            Dim ds As New dsApprovedVendorList
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, rpt)
            da.Fill(ds, Report)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "ApprovedVendorList", mApprovedVendorListSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ElseIf IsExcel = True Then  'Excel format
            Dim PeriodColumnsForExportToExcel As New List(Of String)
            Dim ds As New dsExcelApprovedVendorList
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "ApprovedVendorList", rpt)

            Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr3", "SearchStr4", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() '= {"ID", "IsSupplier", "IsCustomer", "IsServiceProvider", "CityName", "StateName", "CountryName", "IsExcel"}
            If AppSettings("ClientCode") = "7AR" Then
                columnToRemove = {"ID", "IsSupplier", "IsCustomer", "IsServiceProvider", "CityName", "StateName", "CountryName", "IsExcel"}
                If ds.Tables("ApprovedVendorList").Columns.Contains("VendorsID") Then
                    ds.Tables("ApprovedVendorList").Columns("VendorsID").ColumnName = "ID Of Vendor"
                End If
            Else
                columnToRemove = {"ID", "IsSupplier", "IsCustomer", "IsServiceProvider", "CityName", "StateName", "CountryName", "IsExcel", "VendorsID"}
            End If
            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("ApprovedVendorList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("ApprovedVendorList").Columns.Remove(columnToRemove(i))
                End If
            Next

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Vendor"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Category"
            End If

            If ds.Tables("ApprovedVendorList").Columns.Contains("NatureOfVendor") Then
                ds.Tables("ApprovedVendorList").Columns("NatureOfVendor").ColumnName = "Nature"
            End If
            If ds.Tables("ApprovedVendorList").Columns.Contains("ContactPerson") Then
                ds.Tables("ApprovedVendorList").Columns("ContactPerson").ColumnName = "Contact Person"
            End If
            If ds.Tables("ApprovedVendorList").Columns.Contains("RepairStationCertificate") Then
                ds.Tables("ApprovedVendorList").Columns("RepairStationCertificate").ColumnName = "Repair Station Certificate"
            End If

            PeriodColumnsForExportToExcel.AddRange(New String() {"Address", "Phone"})
            Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel

            Dim dsNew As New DataSet
            dsNew.Clear()
            ds.Tables("ReportData").TableName = "Searching Criteria"
            ds.Tables("ApprovedVendorList").TableName = "Approved Vendor List"
			Session("DataTableToBeFormattedForExportToExcel") = "Approved Vendor List"
			Session("ExcelFileName") = "Approved Vendor List"
			dsNew = ds
			Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "ApprovedVendorList", "Export To Excel " + mApprovedVendorListSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant on 04-Dec-2013
        If Not IsPostBack Then
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            SetReport(False)
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            SetReport(True)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class