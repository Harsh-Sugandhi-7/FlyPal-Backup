'Added by Utkarsh on 22-Jan-2014
Imports System.Linq
Imports System.Collections.Generic
Imports System.Text

Public Class wfrptLogParameterReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachineNameValueList As MachineNameValueList
    Public mParameterList As ParameterList
    Dim FromDate As String = "1-1-1900"
    Dim ToDate As String = "1-1-2200"
    Dim Aircraft As String = ""
    Dim Count As Integer = 0
    Dim AircraftIndex As Integer
    Dim Assembly1 As String
    Dim mAssemblyList As AssemblyList
    Dim EventLogDetail As String
    Public mAssemblyParameterList As AssemblyParameterList
#End Region
#Region " Business Method "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mParameterList = CType(Session("mParameterList"), ParameterList)
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mAssemblyParameterList = CType(Session("mAssemblyParameterList"), AssemblyParameterList)
    End Sub
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mParameterList") = mParameterList
        Session("mAssemblyList") = mAssemblyList
        Session("mAssemblyParameterList") = mAssemblyParameterList
    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mParameterList")
        Session.Remove("mParameter")
        Session.Remove("mAssemblyList")
        Session.Remove("mAssemblyParameterList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub PageInitialization()
        txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
    End Sub
    Private Sub ResetValues()
        ToDate = Format(CDate(Today.Date).Year, "")
    End Sub
    Private Function CreateDataTable(ByVal FromDate As String, ByVal ToDate As String, ByVal ParameterIDs As String) As DataTable
        Dim dataTable As New DataTable("TMainReport")
        Dim conString As String = AppSettings("DB:FlyPal")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "LogParameterValueListUsingPivotQuery"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@FromDate", FromDate)
        cmd.Parameters.AddWithValue("@ToDate", ToDate)
        cmd.Parameters.AddWithValue("@AssemblyID", "'" & cmbAssembly.SelectedValue.ToString & "'")
        cmd.Parameters.AddWithValue("@ParameterIDs", "'" & ParameterIDs & "'")

        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        'Dim data As DataRow = dataTable.NewRow
        'data(0) = "Customer : " + cmbVendorText.SelectedItem.ToString
        'data(1) = "5"
        'data(2) = 10
        'dataTable.Rows.InsertAt(data, 0)
        con.Close()
        Return dataTable
    End Function
    Private Sub GenerateXLSXFile(tbl As DataTable, ByVal FromDate As String, ByVal ToDate As String)
        Dim mCompanyDetail As New CompanyDetail
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim dsNew As New DataSet
        Dim da As New CSLA.Data.ObjectAdapter
        dsNew.Clear()

        Dim params = String.Join(",", (From c As AssemblyParameter In mAssemblyParameterList
                                      Where c.IsSelect = True
                                      Select c.ParameterName).ToArray)

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Log Parameter Report", FromDate, ToDate, cmbAircraft.SelectedItem.Text, cmbAssembly.SelectedItem.Text, params, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        da.Fill(dsNew, Report)
        dsNew.Tables(0).TableName = "Searching Criteria"
        dsNew.Merge(tbl)

        Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
        For i As Integer = 0 To columnToRemove2.Length - 1
            If dsNew.Tables("Searching Criteria").Columns.Contains(columnToRemove2(i)) Then
                dsNew.Tables("Searching Criteria").Columns.Remove(columnToRemove2(i))
            End If
        Next

        dsNew.Tables("Searching Criteria").Columns("SearchStr1").ColumnName = "From Date"
        dsNew.Tables("Searching Criteria").Columns("SearchStr2").ColumnName = "To Date"
        dsNew.Tables("Searching Criteria").Columns("SearchStr3").ColumnName = "Aircraft"
        dsNew.Tables("Searching Criteria").Columns("SearchStr4").ColumnName = "Assembly"
        dsNew.Tables("Searching Criteria").Columns("SearchStr5").ColumnName = "Parameters"

        'dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("ReleaseNoteNo").ColumnName = "Customer"

        'dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
        dsNew.Tables("TMainReport").TableName = "Log Parameter Report"
		Session("ExcelFileName") = "Log Parameter Report"
		Session("dsNew") = dsNew
		Session("ExcelFileName") = "Log Parameter Report"
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate


        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
        MarkLog(Util.Action.Print, "LogParameter", "Export To excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
    Private Sub SetValues()
        If txtToDate.Text.Trim = "" Or txtFromDate.Text.Trim = "" Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            ToDate = txtToDate.Text.Trim
            FromDate = txtFromDate.Text.Trim
            lblDateRangeFrom.Text = "From Date : " & FromDate & " To Date : " & ToDate
        End If

        If cmbAircraft.SelectedIndex = 0 Then       'Aircraft
            Aircraft = ""
            lblAircraft.Text = "Aircraft : All"
        Else
            Aircraft = mMachineNameValueList(New Guid(cmbAircraft.SelectedValue)).RegNo
            lblAircraft.Text = "Aircraft : " & Aircraft
        End If

        If cmbAircraft.SelectedItem.Text = "(SELECT)" Then
            Aircraft = ""
        Else
            If cmbAssembly.SelectedItem.Text = "(All)" Or cmbAssembly.SelectedItem.Text = "<All>" Then
                Assembly1 = ""
            Else
                Assembly1 = cmbAssembly.SelectedItem.Text
                lblAssembly1.Text = "Assembly Name : " & Assembly1  'Added Code
            End If
            Aircraft = cmbAircraft.SelectedItem.Text
            lblAircraft.Text = "Aircraft Name : " & Aircraft
        End If

        'For i As Integer = 0 To cmbParameter.Items.Count - 1
        '    mMachine.MachineParameters.Item(i).IsSelect = cmbParameter.Items.Item(i).Selected
        '    If cmbParameter.Items.Item(i).Selected = True Then
        '        Count = Count + 1
        '    End If
        'Next
        'If Count > 5 And rdoPortrait.Checked = True Then
        '    Dim msg1 As New SIMsgBox(Page, "<BR>Too many selection", "<BR><BR>Portrait option does not allow more than 5 parameters, use landscape option", "Portrait option does not allow more than 5 parameters, use landscape option.", MsgBoxStyle.OKOnly)
        '    msg1.ReplacePage = "wfrptLogParameterReport.aspx?"
        '    msg1.Show()
        '    Exit Sub
        'ElseIf Count > 9 And rdoLandScape.Checked = True Then
        '    Dim msg1 As New SIMsgBox(Page, "<BR>Too many selection", "<BR><BR>Landscape option does not allow more than 9 parameters, please break paramters into multiple report prints.", "Landscape option does not allow more than 9 parameters, please break paramters into multiple report prints", MsgBoxStyle.OKOnly)
        '    msg1.ReplacePage = "wfrptLogParameterReport.aspx?"
        '    msg1.Show()
        '    Exit Sub
        'End If
        EventLogDetail = lblDateRangeFrom.Text + ", " + lblAircraft.Text + ", " + lblAssembly1.Text + ", Report Type : " + (IIf(rdoLandScape.Checked, rdoLandScape.Text, rdoPortrait.Text))
    End Sub
    Private Function SetParameterValues(ByVal IsExcel As Boolean) As Boolean
        Dim ParamIDs As New StringBuilder
        For i As Integer = 0 To cmbParameter.Items.Count - 1
            'mMachine.MachineParameters.Item(i).IsSelect = cmbParameter.Items.Item(i).Selected
            mAssemblyParameterList.Item(i).IsSelect = cmbParameter.Items.Item(i).Selected
            If cmbParameter.Items.Item(i).Selected = True Then
                If ParamIDs.ToString = "" Then
                    ParamIDs.Append("<ParamID>")
                End If
                ParamIDs.Append("<id>")
                ParamIDs.Append(cmbParameter.Items.Item(i).Value)
                ParamIDs.Append("</id>")
                Count = Count + 1
            End If
        Next
        If ParamIDs.ToString <> "" Then
            ParamIDs.Append("</ParamID>")
        End If
        Session("ParamIDs") = ParamIDs.ToString
        Session("mAssemblyParameterList") = mAssemblyParameterList
        If Not IsExcel Then
            If Count > 4 And rdoPortrait.Checked = True Then
                MSGBoxCtrl.show("Too many selection", "Portrait option does not allow more than 4 parameters, use landscape option", "Portrait option does not allow more than 4 parameters, use landscape option.", MsgBoxStyle.OkOnly, "")
                Return False
                Exit Function
            ElseIf Count > 9 And rdoLandScape.Checked = True Then
                MSGBoxCtrl.show("Too many selection", "Landscape option does not allow more than 9 parameters, please break parameters into multiple report prints.", "Landscape option does not allow more than 9 parameters, please break parameters into multiple report prints", MsgBoxStyle.OkOnly, "")
                Return False
                Exit Function
            End If
        End If
        Return True
    End Function
    Private Sub ControlVisibility()
        lblSummary.Visible = False
        lblDateRangeFrom.Visible = False
        lblAircraft.Visible = False
        upnlCriteria.Update()
    End Sub
    Private Sub ControlVisibility1()
        lblSummary.Visible = True
        lblDateRangeFrom.Visible = True
        lblAircraft.Visible = True
        lblAssembly1.Visible = True
        upnlCriteria.Update()
    End Sub
    Public Sub SetReport(ByVal IsExcel As Boolean)
        GetSession()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objReg As LogParameterList
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsLogParameterList As New dsLogParameterList

        If rdoPortrait.Checked = True Then
            myReport = New crLogParameterList
        Else
            myReport = New crLogParameterListLandScape
        End If

        Dim params = String.Join(",", (From c As AssemblyParameter In mAssemblyParameterList
                                       Where c.IsSelect = True
                                       Select c.ParameterName).ToArray)

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Log Parameter Report", FromDate, ToDate, cmbAircraft.SelectedItem.Text, cmbAssembly.SelectedItem.Text, params, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        mAssemblyParameterList = Session("mAssemblyParameterList")
        ''Session("mAssemblyParameterList") = mAssemblyParameterList
        objReg = LogParameterList.GetLogParameterList(New Guid(cmbAssembly.SelectedValue.ToString), FromDate, ToDate, mAssemblyParameterList)

        If objReg.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf Not IsExcel Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1111)
        End If

        dsLogParameterList.Clear()
        If IsExcel = False Then 'If PDF format
            Dim mrptImage As rptImage = rptImage.GetImage(dsLogParameterList) 'Added by Shweta on 22-Feb-2012
            da.Fill(dsLogParameterList, objReg)
            da.Fill(dsLogParameterList, mrptImage) 'Added by Shweta on 22-Feb-2012
            da.Fill(dsLogParameterList, Report)

            myReport.SetDataSource(dsLogParameterList)
            Session("CrystalReport") = myReport
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            'Dim params = String.Join(",", (From c As AssemblyParameter In mAssemblyParameterList
            '                           Where c.IsSelect = True
            '                           Select c.ParameterName).ToArray)
            EventLogDetail = EventLogDetail + ", Parameters : " + params
            MarkLog(Util.Action.Print, "LogParameter", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Else
            'Dim params = String.Join(",", (From c As AssemblyParameter In mAssemblyParameterList
            '                           Where c.IsSelect = True
            '                           Select c.ParameterName).ToArray)

            da.Fill(dsLogParameterList, "ExcelLogParameterList", objReg)
            da.Fill(dsLogParameterList, "ReportData", Report)
            Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If dsLogParameterList.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    dsLogParameterList.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"LogText", "DateFormatString", "LogNo", "SrNo", "ParameterID", "ExpYear", "ExpiryDateDBValue", "StoreName", "LocationName", "Text", "No", "ReceiptDate"}
            For i As Integer = 0 To columnToRemove.Length - 1
                If dsLogParameterList.Tables("ExcelLogParameterList").Columns.Contains(columnToRemove(i)) Then
                    dsLogParameterList.Tables("ExcelLogParameterList").Columns.Remove(columnToRemove(i))
                End If
            Next

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(dsLogParameterList.Tables("ReportData"))
            dsNew.Merge(dsLogParameterList.Tables("ExcelLogParameterList"))

            dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
            dsNew.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            dsNew.Tables("ReportData").Columns("SearchStr3").ColumnName = "Aircraft"
            dsNew.Tables("ReportData").Columns("SearchStr4").ColumnName = "Assembly"
            dsNew.Tables("ReportData").Columns("SearchStr5").ColumnName = "Parameters"

            dsNew.Tables("ExcelLogParameterList").Columns("LogDate").ColumnName = "Log Date"
            dsNew.Tables("ExcelLogParameterList").Columns("LogTextNo").ColumnName = "Log No."
            dsNew.Tables("ExcelLogParameterList").Columns("ParameterName").ColumnName = "Parameter Name"
            'dsNew.Tables("ExcelLogParameterList").Columns("LogParameterValue").DataType = GetType(Decimal)
            'dsNew.Tables("ExcelLogParameterList").Columns("MinValue").DataType = GetType(Decimal)
            'dsNew.Tables("ExcelLogParameterList").Columns("MaxValue").DataType = GetType(Decimal)
            dsNew.Tables("ExcelLogParameterList").Columns("MinValue").ColumnName = "Min.Value"
            dsNew.Tables("ExcelLogParameterList").Columns("MaxValue").ColumnName = "Max.Value"
            dsNew.Tables("ExcelLogParameterList").Columns("LogParameterValue").ColumnName = "Actual Value"


            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("ExcelLogParameterList").TableName = "Log Parameter Report"
			Session("ExcelFileName") = "Log Parameter Report"
			Session("dsNew") = dsNew

			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        End If

       
        'ResetValues()
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptLogParameterReport_Ajax.aspx" Then
            RemoveSessions()
        End If
    End Sub
#End Region

#Region " DataBinding "
    Private Sub DatafieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.Date.ToString, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
        UpnlDetails.Update()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Utkarsh
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptLogParameterReport_Ajax.aspx"
            If cmbAircraft.Enabled = True Then
                setFocus(cmbAircraft)
            End If
            DatafieldBind()
            PageInitialization()
        End If
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            cmbAssembly.Enabled = False
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex

        Else
            cmbAssembly.Enabled = True
            Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text.Trim, "(SELECT)", True)
            Session("mAssemblyList") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
            cmbAssembly.DataBind()

            ''Commented by Saylee on 23-Apr-2010
            ''mMachine = Machine.GetMachine(mMachineNameValueList.Item(cmbAircraft.SelectedIndex).ID)
            ''cmbParameter.DataSource = mMachine.MachineParameters

            ''If mMachine.MachineParameters.Count > 0 Then
            ''    cmbParameter.Enabled = True
            ''    cmbParameter.DataBind()
            ''Else
            ''    cmbParameter.Enabled = False
            ''    cmbParameter.Items.Clear()
            ''End If

        End If
        If cmbAircraft.SelectedIndex = 0 Then
            cmbParameter.Enabled = False
            cmbParameter.Items.Clear()
        End If
        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
    End Sub
    Private Sub cmbAssembly_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssembly.SelectedIndexChanged
        Dim mAssemblyParameterList As AssemblyParameterList = AssemblyParameterList.GetChildAssemblyParameterList(New Guid(cmbAssembly.SelectedValue.ToString))
        Session("mAssemblyParameterList") = mAssemblyParameterList

        cmbParameter.DataSource = mAssemblyParameterList

        If mAssemblyParameterList.Count > 0 Then
            cmbParameter.Enabled = True
            cmbParameter.DataBind()
        Else
            cmbParameter.Enabled = False
            cmbParameter.Items.Clear()
        End If

        If cmbAssembly.SelectedIndex = 0 Then
            cmbParameter.Enabled = False
            cmbParameter.Items.Clear()
        End If
        If cmbAssembly.Enabled = True Then
            setFocus(cmbAssembly)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid() Then
            ControlVisibility1()
            SetValues()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            SetValues()
            If SetParameterValues(False) Then
                SetReport(False)
            End If
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSessions()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        If IsValid() Then
            SetValues()
            If SetParameterValues(True) Then
                GenerateXLSXFile(CreateDataTable(txtFromDate.Text, txtToDate.Text, Session("ParamIDs").ToString), txtFromDate.Text, txtToDate.Text)
            End If
        End If
    End Sub
#End Region


    
End Class