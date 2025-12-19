Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Public Class wfrptCodeNoRecordsList_Ajax
    Inherits System.Web.UI.Page

#Region " Variables "
    Dim mCompanyDetail As New CompanyDetail
    Public mCategoryList As CategoryList
    Public PartNo As String = ""
    Public Description As String = ""
    Public SerialNo As String = ""
    Public EventLogDetails As String = String.Empty
#End Region

#Region " Helper Methods "
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    '
            End Select
        End If
    End Sub
    Private Sub Display()
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblSerialNo1.Visible = True
        lblCodeNo1.Visible = True
        lblCategory1.Visible = True
        upnlSerachCriteria.Update()
    End Sub
    Private Sub SetValues()
        PartNo = IIf(PartNo <> "", PartNo, "")
        Description = IIf(Description <> "", Description, "")
        If txtSerialNo.Text.Trim <> "" Then
            SerialNo = txtSerialNo.Text.Trim
        Else
            SerialNo = ""
        End If
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblSerialNo1.Text = "Serial No. : " + IIf(SerialNo <> "", SerialNo, "All")
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "")
        lblCodeNo1.Text = "Code No. : " & txtCodeNo.Text.Trim
        lblCategory1.Text = "Category : " & IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, "All")
        EventLogDetails = lblPartNo.Text + ", " + lblDesc.Text + ", " + ", " + lblSerialNo1.Text + ", " + lblCodeNo1.Text + ", " + lblCategory1.Text + ", " + cmbSortBy.SelectedItem.Text
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rpt As rptCodeNoRecordsList
        If txtSerialNo.Text.Trim <> "" Then
            SerialNo = txtSerialNo.Text.Trim
        Else
            SerialNo = ""
        End If
        If cmbSortBy.SelectedValue = 2 Then
            myReport = New crptToolTypeRecordsList
        Else
            myReport = New crptCodeNoRecordsList
        End If
        rpt = rptCodeNoRecordsList.GetCodeNoRecordsList(ItemName:=PartNo, ItemDescription:=Description, SerialNo:=SerialNo, CodeNo:=txtCodeNo.Text.Trim, CategoryID:=cmbCategory.SelectedValue.ToString, Sortedby:=CInt(cmbSortBy.SelectedValue))
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1316)
            ' MarkLog(Util.Action.Print, "ToolingList", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            If IsExcel = False Then 'Added by Shital on 18-Jan-2021
                MarkLog(Util.Action.Print, "ToolingList", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            End If
        End If
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
       mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
       mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
        "", PartNo, Description, SerialNo, SearchStr4:=txtCodeNo.Text.Trim, SearchStr5:=IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, ""), ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:=cmbSortBy.SelectedItem.Text, SearchStr7:="", SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"), SearchStr11:="")
        If IsExcel = False Then     'PDF format
            Dim ds As New dsCodeNoRecordsList
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
        ElseIf IsExcel = True Then  'Excel format
            Dim ds As New dsExcekCodeNoRecordsList
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "rptCodeNoRecordsList", rpt)

            Dim columnToRemove2 As String() = {"ReportName", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "ShortName", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next
            Dim columnToRemove As String()
            If cmbSortBy.SelectedValue = 2 Then
                columnToRemove = {"ItemID", "CodeNo"}
            Else
                columnToRemove = {"ItemID", "ToolType"}
            End If


            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("rptCodeNoRecordsList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("rptCodeNoRecordsList").Columns.Remove(columnToRemove(i))
                End If
            Next

            If ds.Tables("rptCodeNoRecordsList").Columns.Contains("ItemName") Then
                ds.Tables("rptCodeNoRecordsList").Columns("ItemName").ColumnName = "Part No."
            End If
            If ds.Tables("rptCodeNoRecordsList").Columns.Contains("ItemDescription") Then
                ds.Tables("rptCodeNoRecordsList").Columns("ItemDescription").ColumnName = "Description"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Part No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Description"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Serial No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Code No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Category"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()
            ds.Tables("ReportData").TableName = "Searching Criteria"
			ds.Tables("rptCodeNoRecordsList").TableName = "Tooling List"
			Session("ExcelFileName") = "Tooling List"
			dsNew = ds
			Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            MarkLog(Util.Action.Print, "ToolingList", "Export To excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryList = CategoryList.GetCategoryList("(All)", True)
        cmbCategory.DataSource = mCategoryList
        cmbCategory.DataBind()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetValues()
        SetReport(False)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetValues()
        SetReport(True)
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

#Region " Service Methods "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetPartNoDescriptionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim itemlist As ItemListAutoComplete
        itemlist = ItemListAutoComplete.GetItemList(prefixText, False)
        If count = 0 Then
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).ToArray
        Else
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetSerialNo(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mSerialNoListAutoComplete As SerialNoListAutoComplete = SerialNoListAutoComplete.GetSerialNoList(prefixText)
        If count = 0 Then
            Return (From c As SerialNoListAutoComplete.SerialNoListAutoCompleteInfo In mSerialNoListAutoComplete Select c.SerialNo).ToArray
        Else
            Return (From c As SerialNoListAutoComplete.SerialNoListAutoCompleteInfo In mSerialNoListAutoComplete
               Select c.SerialNo).Take(count).ToArray
        End If
    End Function
#End Region

End Class