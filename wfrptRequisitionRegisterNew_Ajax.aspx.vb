Public Class wfrptRequisitionRegisterNew_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public FromDate As String = ""
    Public ToDate As String = ""
    Public Requisition, Type As Integer
    Public EmployeeName As String
    Public LocationName As String
    Public RequisitionText As String = ""
    Public RequisitionNo As Integer = 0
    Public PartNo As String = ""
    Public Description As String = ""
    Public StatusID As Integer = 0
    Public StatusName As String = ""
    Public TypeoFRequisition As String = ""
    Dim mSearchCriteriaForEventLog As String = String.Empty
    'Added by Abhishek on 27-SEP-2017s
    Dim objSearch As rptSearchingCriteriaForRequisitionNew
    Dim objReg As rptRequisitionRegisterNew
    Dim da As New CSLA.Data.ObjectAdapter
    Dim dsRequisitionRegisterNew As New dsRequisitionRegisterNew
    Dim ReportDetails As New rptStatusList
    Dim QuotationText1 As String = ""
    Dim BranchID As Integer = -1

#End Region

#Region " Business Properties and Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblRequisitionNo.Visible = True
        lblStatus1.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblReqType.Visible = True
        lblLocation1.Visible = True
        lblEmployee1.Visible = True
    End Sub
    Private Sub ControlVisibility(ByVal index As Integer)
        If index = 6 Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf index = 1 Or index = 2 Or index = 3 Or index = 4 Or index = 5 Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All'
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If

        If txtLocation.Text = "" Then
            LocationName = ""
            lblLocation1.Text = "Location : All"
        Else
            LocationName = txtLocation.Text.Trim
            lblLocation1.Text = "Location : " + LocationName
        End If

        If txtEmployee.Text = "" Then
            EmployeeName = ""
            lblEmployee1.Text = "Employee : All"
        Else
            EmployeeName = txtEmployee.Text.Trim
            lblEmployee1.Text = "Employee : " + EmployeeName
        End If


        If cmbRequisition.SelectedIndex > 0 Then
            Type = IIf(cmbRequisition.SelectedIndex = 1 Or cmbRequisition.SelectedIndex = 3 Or cmbRequisition.SelectedIndex = 4, cmbType.SelectedValue, 0)
            Requisition = cmbRequisition.SelectedValue
            TypeoFRequisition = cmbRequisition.SelectedItem.Text
            lblReqType.Text = "Requisition Type : " + cmbRequisition.SelectedItem.Text
        Else
            Requisition = 0
            Type = 0
            TypeoFRequisition = ""
            lblReqType.Text = "Requisition Type : All"
        End If

        If cmbRequisitionEngineeringBranches.SelectedIndex > 0 Then
            lblReqBranch.Text = "Branch : " + cmbRequisitionEngineeringBranches.SelectedItem.Text
        Else
            lblReqBranch.Text = ""
        End If

        If txtRequisitionNo.Text <> "" Then
            RequisitionNo = txtRequisitionNo.Text.Trim
            lblRequisitionNo.Text = txtRequisitionText.Text.Trim + txtRequisitionNo.Text.Trim
        Else
            lblRequisitionNo.Text = "Requisition No. : "
            RequisitionNo = 0
        End If

        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        RequisitionText = IIf(txtRequisitionText.Text.Trim <> "", txtRequisitionText.Text.Trim, "")
        StatusID = IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedIndex, 0)
        If StatusID > 0 Then
            StatusName = cmbStatus.SelectedItem.Text
        Else
            StatusName = ""
        End If
        lblStatus1.Text = "Status : " + StatusName
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")
        mSearchCriteriaForEventLog = lblDateRangeFrom.Text + ", " + lblReqType.Text + "," + lblRequisitionNo.Text + ", " + ", " + lblLocation1.Text + ", " + lblEmployee1.Text + ", " + lblStatus1.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text
    End Sub
    Public Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteriaForRequisitionNew
        Dim objReg As rptRequisitionRegisterNew
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsRequisitionRegisterNew As New dsRequisitionRegisterNew
        Dim ReportDetails As New rptStatusList
        Dim QuotationText1 As String = ""
        Dim BranchID As Integer = -1

        If cmbRequisitionEngineeringBranches.SelectedItem.Text = "(All)" Then
            BranchID = -1
        Else
            BranchID = cmbRequisitionEngineeringBranches.SelectedValue
        End If

        SetValues()

        If AppSettings("ClientCode") = "IND" Then
            myReport = New crptRequisitionRegisterLandscapeNewIND
        Else
            myReport = New crptRequisitionRegisterLandscapeNew
        End If


        objReg = rptRequisitionRegisterNew.GetRequisitionList(RequisitionText, RequisitionNo, FromDate, ToDate, LocationName, EmployeeName, IIf(cmbRequisition.SelectedIndex = 1 Or cmbRequisition.SelectedIndex = 3 Or cmbRequisition.SelectedIndex = 4, cmbType.SelectedValue, 0), StatusID, PartNo, Description, , BranchID, cmbRequisition.SelectedValue)
        objSearch = rptSearchingCriteriaForRequisitionNew.GetSearchingCriteriaForRequisition(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), RequisitionText, RequisitionNo, FromDate, ToDate, LocationName, EmployeeName, PartNo, Description, StatusName, TypeoFRequisition, AppSettings("Logo"), cmbRequisitionEngineeringBranches.SelectedItem.Text, , , Today.Date.ToString(AppSettings("DateFormat")))


        If objReg.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf objReg.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1229)
        End If

        dsRequisitionRegisterNew.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(dsRequisitionRegisterNew)
        da.Fill(dsRequisitionRegisterNew, mrptImage)
        da.Fill(dsRequisitionRegisterNew, objReg)
        da.Fill(dsRequisitionRegisterNew, objSearch)
        myReport.SetDataSource(dsRequisitionRegisterNew)

        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "RequisitionRegister", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
    Private Sub addAttributes()
        txtRequisitionNo.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtRequisitionNo').value,event)")
    End Sub
    Private Sub controlvisibility1()
        cmbType.Visible = IIf(cmbRequisition.SelectedIndex = 1 Or cmbRequisition.SelectedIndex = 3 Or cmbRequisition.SelectedIndex = 4, True, False)
        lblType.Visible = IIf(cmbRequisition.SelectedIndex = 1 Or cmbRequisition.SelectedIndex = 3 Or cmbRequisition.SelectedIndex = 4, True, False)
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If cmbRequisition.Enabled = True Then
                setFocus(cmbRequisition)
            End If
            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
            controlvisibility1()
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            SetReport()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Protected Sub cmbRequisitionType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbRequisition.SelectedIndexChanged
        cmbRequisitionEngineeringBranches.Items.Clear()
        If cmbRequisition.SelectedIndex = 1 Then
            cmbRequisitionEngineeringBranches.Items.Add(New ListItem("Line Maintenance", "1"))
            cmbRequisitionEngineeringBranches.Items.Add(New ListItem("Base Maintenance", "2"))
            cmbRequisitionEngineeringBranches.Items.Add(New ListItem("Technical Planning", "4"))
        ElseIf cmbRequisition.SelectedIndex = 2 Then
            cmbRequisitionEngineeringBranches.Items.Add(New ListItem("None", "0"))
        ElseIf cmbRequisition.SelectedIndex = 3 Then
            cmbRequisitionEngineeringBranches.Items.Add(New ListItem("Workshop", "3"))
        ElseIf cmbRequisition.SelectedIndex = 4 Then
            cmbRequisitionEngineeringBranches.Items.Add(New ListItem("Technical Planning", "4"))
        Else
            cmbRequisitionEngineeringBranches.Items.Add(New ListItem("(All)", "-1"))
            cmbRequisitionEngineeringBranches.Items.Add(New ListItem("None", "0"))
            cmbRequisitionEngineeringBranches.Items.Add(New ListItem("Line Maintenance", "1"))
            cmbRequisitionEngineeringBranches.Items.Add(New ListItem("Base Maintenance", "2"))
            cmbRequisitionEngineeringBranches.Items.Add(New ListItem("Workshop", "3"))
            cmbRequisitionEngineeringBranches.Items.Add(New ListItem("Technical Planning", "4"))
        End If
        controlvisibility1()
        upnlSelectionOfRequisitionType.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()
            GenerateXLSXFile(CreateDataTable())
        End If
    End Sub
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("Requistion Register")
        Dim conString As String = AppSettings("DB:FlyPal")
        Dim con = New SqlConnection(conString)
        con.Open()
        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "ExcelrptfetchRequisitionListNew"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@Text", RequisitionText)
        cmd.Parameters.AddWithValue("@No", RequisitionNo)
        cmd.Parameters.AddWithValue("@FromDate", FromDate)
        cmd.Parameters.AddWithValue("@ToDate", ToDate)
        cmd.Parameters.AddWithValue("@LocationName", LocationName)
        cmd.Parameters.AddWithValue("@EmployeeName", EmployeeName)
        cmd.Parameters.AddWithValue("@RequisitionTypeID", Type)
        cmd.Parameters.AddWithValue("@StatusID", StatusID)
        cmd.Parameters.AddWithValue("@ItemName", PartNo)
        cmd.Parameters.AddWithValue("@ItemDesc", Description)
        cmd.Parameters.AddWithValue("@RegNo", "") 'Added By Vikrant on 11-Sept-2012 For ALL11092012-3
        cmd.Parameters.AddWithValue("@RequisitionEngineeringBrancheID", BranchID) 'Added By Vikrant on 11-Sept-2012 For ALL11092012-3
        cmd.Parameters.AddWithValue("@TransTypeID", Requisition)
        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()


        'dataTable.Columns.Remove("Rem1")
        'dataTable.Columns.Remove("Rem2")
        'dataTable.Columns.Remove("Rem3")
        Return dataTable
    End Function
    Private Sub GenerateXLSXFile(ByVal tbl As DataTable)
        If IsValid Then
            If AppSettings("ClientCode") = "IND" Then
                tbl.Columns("NRC No.").ColumnName = "OJS No."
            Else
                tbl.Columns("NRC No.").ColumnName = "NRC No."
            End If
            tbl.Columns.Remove("Date1")
            tbl.Columns.Remove("No")

            If cmbRequisitionEngineeringBranches.SelectedItem.Text = "(All)" Then
                BranchID = -1
            Else
                BranchID = cmbRequisitionEngineeringBranches.SelectedValue
            End If

            SetValues()
            objReg = rptRequisitionRegisterNew.GetRequisitionList(RequisitionText, RequisitionNo, FromDate, ToDate, LocationName, EmployeeName, _
                                                                  IIf(cmbRequisition.SelectedIndex = 1 Or cmbRequisition.SelectedIndex = 3 Or cmbRequisition.SelectedIndex = 4, cmbType.SelectedValue, 0), StatusID, PartNo, Description, , BranchID, cmbRequisition.SelectedValue)
            objSearch = rptSearchingCriteriaForRequisitionNew.GetSearchingCriteriaForRequisition(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), RequisitionText, RequisitionNo, FromDate, ToDate, LocationName, EmployeeName, PartNo, Description, StatusName, TypeoFRequisition, AppSettings("Logo"), cmbRequisitionEngineeringBranches.SelectedItem.Text, , , Today.Date.ToString(AppSettings("DateFormat")))


            If objReg.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf objReg.Count > 0 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1229)
            End If

            dsRequisitionRegisterNew.Clear()
            ' da.Fill(dsRequisitionRegisterNew, objReg)
            da.Fill(dsRequisitionRegisterNew, "rptSearchingCriteriaForRequisitionNew", objSearch)

            Dim columnToRemove1 As String() = {"ID", "RegNo", "RequisitionType", "SearchString1", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ReportName", "SearchStr5", "ProductVersion", "SINote", "SearchString4", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr14", "SearchStr13", "SearchStr12", "SearchStr11", "CurrencyName", "CurrencySymbol", "ShortName", "RequisitionDate", "SearchString3"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If dsRequisitionRegisterNew.Tables("rptSearchingCriteriaForRequisitionNew").Columns.Contains(columnToRemove1(i)) Then
                    dsRequisitionRegisterNew.Tables("rptSearchingCriteriaForRequisitionNew").Columns.Remove(columnToRemove1(i))
                End If
            Next
            If dsRequisitionRegisterNew.Tables("rptSearchingCriteriaForRequisitionNew").Columns.Contains("SearchString2") Then
                dsRequisitionRegisterNew.Tables("rptSearchingCriteriaForRequisitionNew").Columns("SearchString2").ColumnName = "Branch"
            End If
            If dsRequisitionRegisterNew.Tables("rptSearchingCriteriaForRequisitionNew").Columns.Contains("ItemName") Then
                dsRequisitionRegisterNew.Tables("rptSearchingCriteriaForRequisitionNew").Columns("ItemName").ColumnName = "Part No."
            End If
            If dsRequisitionRegisterNew.Tables("rptSearchingCriteriaForRequisitionNew").Columns.Contains("ItemDescription") Then
                dsRequisitionRegisterNew.Tables("rptSearchingCriteriaForRequisitionNew").Columns("ItemDescription").ColumnName = "Description"
            End If

            If (tbl.Rows.Count = 0) Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()
            dsNew.Merge(dsRequisitionRegisterNew.Tables("rptSearchingCriteriaForRequisitionNew"))
            dsNew.Merge(tbl)
			dsNew.Tables("rptSearchingCriteriaForRequisitionNew").TableName = "Searching Criteria"
			Session("ExcelFileName") = "Requisition Register"
			Session("dsNew") = dsNew
			'Session("DataTable") = tbl
			'Session("ReportName") = "RCI Register"
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "RequisitionRegister", "Export To Excel " + mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
End Class