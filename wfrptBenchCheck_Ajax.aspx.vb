Public Class wfrptBenchCheck_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mStoreList As StoreList
    Public PartNo As String
    Public Description As String
    Public StoreID As Guid 'Added By Prashant On 30-Apr-2013 For ALL29042013-4
    Dim EventLogID As Guid 'Added by Prashant
    Dim mBenchCheckSearchingCriteria As String = String.Empty
    'Added By Abhishek on 11-OCT-2017
    Dim da As New CSLA.Data.ObjectAdapter
    Dim objSearch As rptSearchingCriteria
    Dim ds As New dsBenchCheck
    Dim rpt As rptBenchCheck
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mStoreList = CType(Session("mStoreList"), StoreList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)

       
    End Sub
    Private Sub SetSession()
        Session("mStoreList") = mStoreList
        Session("PartNo") = PartNo
        Session("Description") = Description
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mStoreList")
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub Controlvisibility()
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblStoreName.Visible = False
    End Sub
    Private Sub Display()
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblStoreName.Visible = True
    End Sub
    Private Sub SetValues()
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        StoreID = New Guid(cmbStore.SelectedValue) 'Added By Prashant On 30-Apr-2013 For ALL29042013-4
        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        PartNo = IIf(PartNo <> "" And Not IsNothing(PartNo), PartNo, "")  'Shweta
        Description = IIf(Description <> "" And Not IsNothing(Description), Description, "") 'Shweta
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        lblStoreName.Text = "Store Name : " & IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "All")
        mBenchCheckSearchingCriteria = lblStoreName.Text.Trim + ", " + lblPartNo.Text.Trim + ", " + lblDesc.Text.Trim
    End Sub
    Private Sub SetReport()
        SetValues()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteria
        Dim ds As New dsBenchCheck
        Dim rpt As rptBenchCheck
        myReport = New crptBenchCheck
        rpt = rptBenchCheck.GetBenchCheck(PartNo, Description, "", cmbStore.SelectedValue.ToString)
        objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", PartNo, "", "", "", "", IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""), "", "", Description, AppSettings("Logo"))
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 713)
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, rpt)
        da.Fill(ds, objSearch)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "BenchCheck", mBenchCheckSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
     Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack Then
            DataFieldBind()
        End If
    End Sub
     Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
         mStoreList = Nothing
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region
    'Added By Abhishek on 11-OCT-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then


            SetValues()
            rpt = rptBenchCheck.GetBenchCheck(PartNo, Description, "", cmbStore.SelectedValue.ToString)
            objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", PartNo, "", "", "", "", IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""), "", "", Description, AppSettings("Logo"))
            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 713)
            End If
            da.Fill(ds, "rptBenchCheck", rpt)
            da.Fill(ds, "rptSearchingCriteria", objSearch)
            Dim columnToRemove1 As String() = {"CompanyName", "FromDate", "ToDate", "SupplierName", "BranchName", "Category", "Nomenclature", "Aircraft", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "WorkShop", "WorkOrderText", "WorkOrderNo", "FromStore", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove1(i))
                End If
            Next

            If ds.Tables("rptBenchCheck").Columns.Contains("PartName") Then
                ds.Tables("rptBenchCheck").Columns("PartName").ColumnName = "Part"
            End If
            If ds.Tables("rptBenchCheck").Columns.Contains("PartDescription") Then
                ds.Tables("rptBenchCheck").Columns("PartDescription").ColumnName = "Description"
            End If
            If ds.Tables("rptBenchCheck").Columns.Contains("ReleaseNoteNo") Then
                ds.Tables("rptBenchCheck").Columns("ReleaseNoteNo").ColumnName = "Release Note No."
            End If
            If ds.Tables("rptBenchCheck").Columns.Contains("SerialNo") Then
                ds.Tables("rptBenchCheck").Columns("SerialNo").ColumnName = "Serial No."
            End If

            If ds.Tables("rptBenchCheck").Columns.Contains("BenchMarkMonths") Then
                ds.Tables("rptBenchCheck").Columns("BenchMarkMonths").ColumnName = "Bench Check Months"
            End If

            If ds.Tables("rptBenchCheck").Columns.Contains("StartDate") Then
                ds.Tables("rptBenchCheck").Columns("StartDate").ColumnName = "Start Date"
            End If
            If ds.Tables("rptBenchCheck").Columns.Contains("CheckDate") Then
                ds.Tables("rptBenchCheck").Columns("CheckDate").ColumnName = "Check Date"
            End If
            If ds.Tables("rptBenchCheck").Columns.Contains("StockBalanceQty") Then
                ds.Tables("rptBenchCheck").Columns("StockBalanceQty").ColumnName = "Stock Qty."
            End If
            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("rptSearchingCriteria"))
            dsNew.Merge(ds.Tables("rptBenchCheck"))

            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            dsNew.Tables("rptBenchCheck").TableName = "Bench Check"
			Session("ExcelFileName") = "Bench Check"
			Session("dsNew") = dsNew
			'Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
			'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
			'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
			'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "BenchCheck", "Export To excel  " + mBenchCheckSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If
    End Sub
End Class