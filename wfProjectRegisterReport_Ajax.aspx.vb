Imports System.Collections.Generic
Imports System.Web.Services
Imports System.Linq
Imports System.Linq.Enumerable
Public Class wfProjectRegisterReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mProject As Project
    Public mCustomerList As VendorList
    Public mProjectList As ProjectList
    Public mProjectDistinctTextList As ProjectDistinctTextList
    Dim FromDate, ToDate, ProjectNo, ProjectText, Customer, RegNo, Model, SerialNo As String
    Dim No As Integer
    Dim EventLogID As Guid
    Dim rptCustomer As String
    Dim rptProjectText As String
    Dim mProjectRegisterReportSearchCriteria As String = String.Empty
#End Region

#Region "Helper Methods"
    Private Sub SetSearchCriteriaLabels()
        Try
            If Not IsDate(txtFromDate.Text) Then
                FromDate = New SmartDate(Today.ToString()).FormattedText
                lblSearchCriteriaFromDate.Text = "From Date : " + ""
            Else
                FromDate = txtFromDate.Text.ToString()
                lblSearchCriteriaFromDate.Text = "From Date : " + New SmartDate(txtFromDate.Text.ToString()).FormattedText
            End If

            If Not IsDate(txtToDate.Text) Then
                ToDate = New SmartDate(Today.ToString()).FormattedText
                lblSearchCriteriaToDate.Text = "To Date : " + ""
            Else
                ToDate = txtToDate.Text.ToString()
                lblSearchCriteriaToDate.Text = "To Date : " + New SmartDate(txtToDate.Text.ToString()).FormattedText
            End If

            If cmbCustomer.SelectedIndex = 0 Then
                Customer = "00000000-0000-0000-0000-000000000000"
                rptCustomer = "ALL"
                lblSearchCriteriaCustomer.Text = "Customer : " + "ALL"
            Else
                Customer = cmbCustomer.SelectedValue.ToString()
                rptCustomer = cmbCustomer.SelectedItem.Text.ToString()
                lblSearchCriteriaCustomer.Text = "Customer : " + cmbCustomer.SelectedItem.Text.ToString()
            End If

            If cmbProjectText.SelectedIndex = 0 Then
                ProjectText = ""
                rptProjectText = "ALL"
                lblSearchCriteriaProjectText.Text = "Project Text : " + "ALL"
            Else
                ProjectText = cmbProjectText.SelectedItem.Text.ToString()
                rptProjectText = cmbProjectText.SelectedItem.Text.ToString()
                lblSearchCriteriaProjectText.Text = "Project Text : " + cmbProjectText.SelectedItem.Text.ToString()
            End If
            If txtNo.Text = "" Then
                No = 0
                lblSearchCriteriaProjectNo.Text = "Project No. : " + ""
            Else
                No = txtNo.Text
                lblSearchCriteriaProjectNo.Text = "Project No. : " + txtNo.Text
            End If

            If txtRegNo.Text = "" Then
                RegNo = ""
                lblSearchCriteriaRegNo.Text = "Reg No. : " + ""
            Else
                RegNo = txtRegNo.Text
                lblSearchCriteriaRegNo.Text = "Reg No. : " + txtRegNo.Text
            End If


            If txtModelNo.Text = "" Then
                Model = ""
                lblSearchCriteriaModel.Text = "Model : " + ""
            Else
                Model = txtModelNo.Text
                lblSearchCriteriaModel.Text = "Model : " + txtModelNo.Text
            End If


            If txtSerialNo.Text = "" Then
                SerialNo = ""
                lblSearchCriteriaSerialNo.Text = "Serial No. : " + ""
            Else
                SerialNo = txtSerialNo.Text
                lblSearchCriteriaSerialNo.Text = "Serial No. :" + txtSerialNo.Text
            End If


            mProjectRegisterReportSearchCriteria = lblSearchCriteriaFromDate.Text.Trim() +
                                                        ", " + lblSearchCriteriaToDate.Text.Trim() +
                                                        ", " + lblSearchCriteriaCustomer.Text.Trim() +
                                                        ", " + lblSearchCriteriaProjectText.Text.Trim() +
                                                        ", " + lblSearchCriteriaProjectNo.Text.Trim() +
                                                        ", " + lblSearchCriteriaRegNo.Text.Trim() +
                                                        ", " + lblSearchCriteriaModel.Text.Trim() +
                                                        ", " + lblSearchCriteriaSerialNo.Text.Trim()
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub
    Private Sub DisplaySearchCriteriaLabels()
        Try
            lblSummary.Visible = True
            lblSearchCriteriaFromDate.Visible = True
            lblSearchCriteriaToDate.Visible = True
            lblSearchCriteriaCustomer.Visible = True
            lblSearchCriteriaProjectText.Visible = True
            lblSearchCriteriaProjectNo.Visible = True
            lblSearchCriteriaRegNo.Visible = True
            lblSearchCriteriaModel.Visible = True
            lblSearchCriteriaSerialNo.Visible = True

        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean)
        Session("IsExcel") = IsExcel
        Dim objDA As New ObjectAdapter
        Dim crystalReport As Engine.ReportClass = New crptProjectRegister
        Dim objSearch As rptSearchingCriteria
        Dim mCompanyDetail As New CompanyDetail
        Dim ds As New dsDiscrepancyRegister
        Dim mProjectRegisterReport As ProjectRegisterReport
        Try
            SetSearchCriteriaLabels()
            mProjectRegisterReport = ProjectRegisterReport.GetProjectRegisterReport(FromDate:=FromDate,
                                                                                                ToDate:=ToDate,
                                                                                                Text:=ProjectText,
                                                                                                No:=No,
                                                                                                CustomerID:=Customer,
                                                                                                RegNo:=RegNo,
                                                                                                Model:=Model,
                                                                                                SerialNo:=SerialNo)

            objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate:=FromDate, ToDate:=ToDate, PartNo:=ProjectText, SupplierName:=No, BranchName:=cmbCustomer.SelectedItem.Text.ToString(), Category:=RegNo, Nomenclature:=Model, store:=SerialNo, Aircraft:="", KitName:="", Description:="", RelNoteNo:="")

            If mProjectRegisterReport.Count > 0 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1595)
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound,
                                "No records found for this search criteria.", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            Dim Report As New ReportData(CompanyName:=mCompanyDetail.CompanyName,
                                         Address:=mCompanyDetail.Address,
                                         Tel1:=mCompanyDetail.Tel1,
                                         Tel2:=mCompanyDetail.Tel2,
                                         Fax:=mCompanyDetail.Fax,
                                         Email:=mCompanyDetail.Email,
                                         WebSite:=mCompanyDetail.WebSite,
                                         ReportName:="Project Register",
                                         ProductVersion:=AppSettings("Product Version"),
                                         SINote:=AppSettings("SINote"),
                                         SearchStr1:=FromDate,
                                         SearchStr2:=ToDate,
                                         SearchStr3:=rptCustomer,
                                         SearchStr4:=rptProjectText,
                                         SearchStr5:="",'rptParaMELSnagCategory,
                                         SearchStr6:="",
                                         SearchStr7:=No,
                                         SearchStr8:="",
                                         SearchStr9:="",
                                         SearchStr10:="",
                                         SearchStr11:="",
                                         SearchStr12:="",
                                         SearchStr13:=AppSettings("ClientCode"))
            ds.Clear()
            If IsExcel Then
                Dim ProjectregisterColumnsForExportToExcel As New List(Of String)
                objDA.Fill(ds, TableName:="ProjectRegister", mProjectRegisterReport)
                objDA.Fill(ds, Report)
                objDA.Fill(ds, "rptSearchingCriteria", objSearch)



                Dim columnToRemove As String() = {"ID", "ProjectDate", "No", "TransTypeID", "CustomerID", "StatusID", "Remark", "IsAttachmentAdded",
                    "ReceivingDate", "InspectionDate", "InspectionDateFormatted", "ReceivingPersonID", "PartNo", "CustomerContractID", "CustomerContractNo",
                    "ProjectID", "nWOID", "WONo", "WODate", "CallOutID", "nwoStatusID", "csstatusName", "nwoMachineID", "nwoRegNo", "ModelName",
                    "woSerialNo", "woCustomerID", "nwoCustomerName", "CustomerAddress", "WOStartDate", "WOStartDateFormatted", "WOCloseDate",
                    "WOActualTime", "WORemark", "IsClosed", "HourType", "WorkShopID", "WorkShopName", "LogID", "WOJobTypeID", "WOTypeName",
                    "FormNo", "IssueNo", "RevisionNo", "CustomerWONo", "IssueTo", "LogNo", "BillingDate", "BillingDateFormatted", "InvoiceNumber",
                    "BillingRemark", "BillingBy", "woTransTypeID", "BillingRequired", "SpareCount", "BarcodeNo", "AssemblyStatusID", "JobCount",
                    "nIsAuthorized", "nWorkOrderStatusID", "CreatedBy", "AuthorizedBy", "StatusName", "Text"}
                For i As Integer = 0 To columnToRemove.Length - 1
                    If ds.Tables("ProjectRegister").Columns.Contains(columnToRemove(i)) Then
                        ds.Tables("ProjectRegister").Columns.Remove(columnToRemove(i))
                    End If
                Next

                Dim columnToRemove2 As String() = {"CompanyName", "Aircraft", "KitName", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10", "RelNoteNo", "ReportDate", "Description", "FromStore"}
                For i As Integer = 0 To columnToRemove2.Length - 1
                    If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                        ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                    End If
                Next

                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(ds.Tables("rptSearchingCriteria"))
                dsNew.Merge(ds.Tables("ProjectRegister"))
                'dsNew.Merge(ds.Tables("ExcelrptExpiryStockBalance"))


                dsNew.Tables("rptSearchingCriteria").Columns("FromDate").ColumnName = "From Date"
                dsNew.Tables("rptSearchingCriteria").Columns("ToDate").ColumnName = "To Date"
                dsNew.Tables("rptSearchingCriteria").Columns("PartNo").ColumnName = "Project Text"
                dsNew.Tables("rptSearchingCriteria").Columns("SupplierName").ColumnName = "Project No."
                dsNew.Tables("rptSearchingCriteria").Columns("BranchName").ColumnName = "Customer"
                dsNew.Tables("rptSearchingCriteria").Columns("Category").ColumnName = "Reg No."
                dsNew.Tables("rptSearchingCriteria").Columns("Nomenclature").ColumnName = "Model"
                dsNew.Tables("rptSearchingCriteria").Columns("store").ColumnName = "Serial No."


                dsNew.Tables("ProjectRegister").Columns("ProjectDateFormatted").ColumnName = "Project Date"
                dsNew.Tables("ProjectRegister").Columns("ProjectNumber").ColumnName = "Project No."
                dsNew.Tables("ProjectRegister").Columns("CustomerName").ColumnName = "Customer"
                dsNew.Tables("ProjectRegister").Columns("Description").ColumnName = "Description"
                dsNew.Tables("ProjectRegister").Columns("ReceivingDateFormatted").ColumnName = "Receiving Date"
                dsNew.Tables("ProjectRegister").Columns("RegNo").ColumnName = "Reg. No."
                dsNew.Tables("ProjectRegister").Columns("ProjectModelName").ColumnName = "Model"
                dsNew.Tables("ProjectRegister").Columns("SerialNo").ColumnName = "Serial No."
                dsNew.Tables("ProjectRegister").Columns("WODateFormatted").ColumnName = "W.O. Date"
                dsNew.Tables("ProjectRegister").Columns("nWOText").ColumnName = "W.O. No."
                dsNew.Tables("ProjectRegister").Columns("WOBy").ColumnName = "Created By"
                dsNew.Tables("ProjectRegister").Columns("nAuthorizedBy").ColumnName = "Submitted By"
                dsNew.Tables("ProjectRegister").Columns("csnWOStatus").ColumnName = "W.O. Status"
                dsNew.Tables("ProjectRegister").Columns("WOCloseDateFormatted").ColumnName = "Closing Date"
                dsNew.Tables("ProjectRegister").Columns("ClosedBy").ColumnName = "Closed By"


                dsNew.Tables("ProjectRegister").Columns("Project Date").SetOrdinal(0)
                dsNew.Tables("ProjectRegister").Columns("Project No.").SetOrdinal(1)
                dsNew.Tables("ProjectRegister").Columns("Customer").SetOrdinal(2)
                dsNew.Tables("ProjectRegister").Columns("Description").SetOrdinal(3)
                dsNew.Tables("ProjectRegister").Columns("Receiving Date").SetOrdinal(4)
                dsNew.Tables("ProjectRegister").Columns("Reg. No.").SetOrdinal(5)
                dsNew.Tables("ProjectRegister").Columns("Model").SetOrdinal(6)
                dsNew.Tables("ProjectRegister").Columns("Serial No.").SetOrdinal(7)


                dsNew.Tables("ProjectRegister").Columns("W.O. Date").SetOrdinal(8)
                dsNew.Tables("ProjectRegister").Columns("W.O. No.").SetOrdinal(9)
                dsNew.Tables("ProjectRegister").Columns("Created By").SetOrdinal(10)
                dsNew.Tables("ProjectRegister").Columns("Submitted By").SetOrdinal(11)
                dsNew.Tables("ProjectRegister").Columns("W.O. Status").SetOrdinal(12)
                dsNew.Tables("ProjectRegister").Columns("Closing Date").SetOrdinal(13)
                dsNew.Tables("ProjectRegister").Columns("Closed By").SetOrdinal(14)

                'ds.Tables("ProjectRegister").Columns("Model").SetOrdinal(6)
                'ds.Tables("ProjectRegister").Columns("W.O. Date").SetOrdinal(8)



                dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
                dsNew.Tables("ProjectRegister").TableName = "Project Register Report"

                'ProjectregisterColumnsForExportToExcel.AddRange(New String() {"Period(Qtrs)", "Period(Month)", "BalQty"})
                Session("ProjectregisterColumnsForExportToExcel") = ProjectregisterColumnsForExportToExcel
                Session("DataTableToBeFormattedForExportToExcel") = "Project Register Report"

                Session("dsNew") = dsNew
				Session("ExcelFileName") = "Project Register Report"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)

			Else

                Dim mRptImage As rptImage = rptImage.GetImage(ds)
                objDA.Fill(ds, TableName:="ProjectRegister", mProjectRegisterReport)
                objDA.Fill(ds, mRptImage)
                objDA.Fill(ds, Report)
                crystalReport.SetDataSource(ds)
                Session("CrystalReport") = crystalReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(page:=Me, type:=[GetType], key:="openTranDetail", script:=Str, addScriptTags:=True)
                MarkLog(Action:=Action.Print, ModuleName:="ProjectRegister", Detail:=mProjectRegisterReportSearchCriteria,
                        ErrorType:=ErrorType.NoError, TransID:=Guid.Empty, EventLogID)
            End If
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtNo').value,event)")
    End Sub
#End Region

#Region " DatafieldBinding "
    Private Sub DataFieldBind()
        txtFromDate.Text = Today.AddMonths(-1).ToString(AppSettings("DateFormat"))
        txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))

        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        ProjectText = Session("ProjectText")
        mProjectDistinctTextList = ProjectDistinctTextList.GetDistinctTextList("37", AddTopItem:="(All)", TransTypeID:=101) '37 is for tabProject Text
        cmbProjectText.DataSource = mProjectDistinctTextList

        mCustomerList = VendorList.GetVendorstList(0, , , , , , "(All)", True, False)
        cmbCustomer.DataSource = mCustomerList
        Session("mCustomerList") = mCustomerList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        addAttributes()
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
        End If
    End Sub
    Private Sub btnSearchCriteria_Click(sender As Object, e As EventArgs) Handles btnSearchCriteria.Click
        Try
            SetSearchCriteriaLabels()
            DisplaySearchCriteriaLabels()
            upnlSearchCriteria.Update()
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub
    Private Sub btnDisplay_Click(sender As Object, e As EventArgs) Handles btnDisplay.Click
        Try
            If txtFromDate.Text = "" Then
                MSGBoxCtrl.Show("Alert!", "From date required. ", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            If txtToDate.Text = "" Then
                MSGBoxCtrl.Show("Alert!", "To date required. ", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            SetReport(False)
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub

    Private Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Try
            If txtFromDate.Text = "" Then
                MSGBoxCtrl.Show("Alert!", "From date required. ", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            If txtToDate.Text = "" Then
                MSGBoxCtrl.Show("Alert!", "To date required. ", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            SetReport(True)
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub
#End Region
#Region " Service Methods "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetRegTextList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim DistinctTextList As DistinctTextListAutoComplete
        DistinctTextList = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, 32)
        If count = 0 Then
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
        Else
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
        End If
    End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetModelNameList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim DistinctTextList As DistinctTextListAutoComplete
        DistinctTextList = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, 27)
        If count = 0 Then
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
        Else
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
        End If
    End Function
#End Region
End Class