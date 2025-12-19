Imports System.Collections.Generic
Imports System
Imports System.Web.UI
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html.simpleparser
Imports System.Text


Public Class wfSearchCriteriaForEmployeeDocumentDueList_Ajax
    Inherits System.Web.UI.Page


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents txtAsOnDate As SIControls.SICalendar

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Dim FromDate As String = "1-1-1900"
    Dim ToDate As String = "1-1-2200"
    Protected mEmployeeDocumentDueList As EmployeeDocumentDueList
    Protected mEmployeeList As EmployeeList
    Protected mDocument As DocumentList
    Public DateRange As String = ""
    Private mEmployeeDepartmentList As EmployeeDepartmentList  'Added by Shital on 07-Dec-2020
    Dim Report As ReportData
    Public EventLogDetails As String = String.Empty
#End Region

#Region " Business Method "
    Private Sub GetSession()
        mEmployeeDocumentDueList = Session("mEmployeeDocumentDueList")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeDocumentDueList") = mEmployeeDocumentDueList
    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mEmployeeDocumentDueList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Public Sub SetReport()
        GetSession()
        SetValues()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim obj As EmployeeDocumentDueList
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim dsEmployeeDocumentDueList As New dsEmployeeDocumentDueList

        'Here crTestCaseStatusReport is used to show the Test Case Summary Report
        myReport = New crEmployeeDocumentDueList

        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim SearchStr3 As String
        Dim SearchStr4 As String

        If cmbEmployeeList.SelectedIndex > 0 Then
            SearchStr1 = cmbEmployeeList.SelectedItem.Text
        Else
            SearchStr1 = ""
        End If

        If cmbDocumentList.SelectedIndex > 0 Then
            SearchStr2 = cmbDocumentList.SelectedItem.Text
        Else
            SearchStr2 = ""
        End If

        If txtDocumentNo.Text <> "" Then
            SearchStr3 = txtDocumentNo.Text
        Else
            SearchStr3 = ""
        End If

        DateRange = cmbRange.SelectedItem.Text
        If txtAsOnDate.Text <> "" Then
            SearchStr4 = New SmartDate(txtAsOnDate.Text.ToString).FormattedText & " " & "Date Range : " & DateRange
        Else
            SearchStr4 = ""
        End If

        If Not IsDate(txtAsOnDate.Text.Trim) Then
            FromDate = "1/1/1900"
        Else
            FromDate = txtAsOnDate.Text.ToString
        End If

        'lblDateRange.Text = "Date : " & IIf(FromDate <> "1/1/1900", New SmartDate(txtAsOnDate.Value.ToString).FormattedText, "All")
        'lblRangeDisp.Text = "Date Range : " & DateRange
        'Added by Shital 07-Dec-2020 
        Dim SearchStr5 As String
        If cmbDepartmentList.SelectedIndex > 0 Then
            SearchStr5 = cmbDepartmentList.SelectedItem.Text
        Else
            SearchStr5 = ""
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Report = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, "Employee Document Due List", SearchStr1, SearchStr2, SearchStr3, SearchStr4, SearchStr5, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        mEmployeeDocumentDueList = EmployeeDocumentDueList.GetEmployeeDocumentDueList(New Guid(cmbEmployeeList.SelectedValue.ToString), New Guid(cmbDocumentList.SelectedValue.ToString), txtDocumentNo.Text, txtAsOnDate.Text, cmbRange.SelectedIndex, , , , cmbDepartmentList.SelectedValue.ToString)

        If mEmployeeDocumentDueList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim mrptImage As rptImage = rptImage.GetImage(dsEmployeeDocumentDueList) 'Added by Shweta on 27-Feb-2012
        da.Fill(dsEmployeeDocumentDueList, mEmployeeDocumentDueList)
        da.Fill(dsEmployeeDocumentDueList, mrptImage)
        da.Fill(dsEmployeeDocumentDueList, Report)
        myReport.SetDataSource(dsEmployeeDocumentDueList)
        Session("CrystalReport") = myReport

        Dim Str As String
        If IsExcel = False Then

            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "EmployeeDocumentDueList", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Sub SetValues()
        Dim mEmployee As String = ""
        Dim mDocument As String = ""

        If cmbEmployeeList.SelectedIndex > 0 Then
            mEmployee = cmbEmployeeList.SelectedItem.Text
            lblEmployeeCriteria.Text = "Employee : " & mEmployee
        Else
            mEmployee = ""
            lblEmployeeCriteria.Text = "Employee : All"
        End If

        If cmbDocumentList.SelectedIndex > 0 Then
            mDocument = cmbDocumentList.SelectedItem.Text
            lblDocumentCriteria.Text = "Document : " & mDocument
        Else
            mDocument = ""
            lblDocumentCriteria.Text = "Document : All"
        End If
        EventLogDetails = lblEmployeeCriteria.Text + ", Department: " + IIf(cmbDepartmentList.SelectedIndex > 0, cmbDepartmentList.SelectedItem.Text, "") + " " + lblDocumentCriteria.Text + ", Document No.: " + txtDocumentNo.Text
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()

        cmbEmployeeList.DataSource = EmployeeList.GetEmployeeList(, , "<ALL>")
        cmbEmployeeList.DataBind()

        cmbDocumentList.DataSource = DocumentList.GetDocumentList(, "<ALL>")
        cmbDocumentList.DataBind()

        txtAsOnDate.Text = Today.Date.ToString(AppSettings("DateFormat"))

        'Added by Shital on 07-Dec-2020
        mEmployeeDepartmentList = EmployeeDepartmentList.GetEmployeeDepartmentList("(ALL)")
        cmbDepartmentList.DataSource = mEmployeeDepartmentList
        Session("mEmployeeDepartmentList") = mEmployeeDepartmentList
        '--------------------

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        'txtAsOnDate.ShowClearButton = False
        Dim mScriptManager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        mScriptManager.RegisterPostBackControl(Me.hyConverttoPdf)
        If Not IsPostBack Then
            If cmbEmployeeList.Enabled = True Then
                setFocus(cmbEmployeeList)
            End If
            DataFieldBind()
            'PageInitialization()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            SetReport()
        End If
    End Sub
    'Added by shital on 08-Dec-2002
    Private IsExcel As Boolean = False
    Private Sub btnExpotToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpotToExcel.Click
        Dim PeriodColumnsForExportToExcel As New List(Of String)
        If IsValid = True Then
            IsExcel = True

            Dim da As New CSLA10.Data.ObjectAdapter
            Dim ds As New dsEmployeeDocumentDueList

            SetReport()

            If mEmployeeDocumentDueList.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            ds.Clear()
            da.Fill(ds, "EmployeeDocumentDueList", mEmployeeDocumentDueList)
            da.Fill(ds, "ReportData", Report)
            Dim columnToRemove As String() = { _
                                               "ID", _
                                               "EmployeeID", _
                                               "DocumentID", _
                                               "DocumentValidityInID", _
                                               "DoneStatus", _
                                               "ImageFile", _
                                               "ImageSize", _
                                               "FileExtension", "ReferenceID", "HistoryCount", "IsApplicable" _
                                             }
            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("EmployeeDocumentDueList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("EmployeeDocumentDueList").Columns.Remove(columnToRemove(i))
                End If
            Next
            Dim columnToRemoveCriteria As String() = {"ReportDate", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", _
                                                   "ReportName", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", _
                                                   "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr10", "SearchStr11", _
                                                       "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", _
                                                      "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100" _
                                                  }

            For i As Integer = 0 To columnToRemoveCriteria.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
                End If
            Next
            For i As Integer = 0 To ds.Tables("ReportData").Columns.Count - 1
                If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr1" Then
                    ds.Tables("ReportData").Columns(i).ColumnName = "Employee"
                End If
                If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr5" Then
                    ds.Tables("ReportData").Columns(i).ColumnName = "Department"
                End If
                If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr2" Then
                    ds.Tables("ReportData").Columns(i).ColumnName = "Training"
                End If
                If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr3" Then
                    ds.Tables("ReportData").Columns(i).ColumnName = "Training Org"
                End If

                If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr4" Then
                    ds.Tables("ReportData").Columns(i).ColumnName = "As On Date"
                End If
            Next
            Dim ReportLabel As String = "Employee Document Due List"

            Dim dataview As DataView = ds.Tables("EmployeeDocumentDueList").DefaultView
            ds.Tables("EmployeeDocumentDueList").TableName = ReportLabel.Replace("/", " ")


			ds.Tables("ReportData").TableName = "Searching Criteria"
            Session("DataTableToBeFormattedForExportToExcel") = ReportLabel.Replace("/", " ")
            Dim dsNew As New DataSet
            dsNew.Clear()
			Session("ExcelFileName") = ReportLabel.Replace("/", " ")

			dsNew.Merge(ds.Tables("Searching Criteria"))
            dsNew.Merge(dataview.ToTable())
            Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "EmployeeDocumentDueList", "Export To Excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSessions()
        Session("mDefectList") = ""
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblEmployeeCriteria.Visible = True
        lblDocumentCriteria.Visible = True
        lblDocumentNoCriteria.Visible = True
        lblAsOnDate1.Visible = True
        lblRangeDisp.Visible = True

        lblAsOnDate1.Text = "As On Date : " + New SmartDate(txtAsOnDate.Text.ToString).FormattedText
        lblDocumentNoCriteria.Text = "Document No : " + txtDocumentNo.Text
        DateRange = cmbRange.SelectedItem.Text
        lblRangeDisp.Text = "Date Range : " & DateRange
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub cmbDocumentList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDocumentList.SelectedIndexChanged
        setFocus(cmbDocumentList)
    End Sub
    Private Sub cmbEmployeeList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbEmployeeList.SelectedIndexChanged
        setFocus(cmbEmployeeList)
    End Sub
    Private Sub txtDocumentNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDocumentNo.TextChanged
        setFocus(txtDocumentNo)
    End Sub
    'Private Sub txtAsOnDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAsOnDate.CalendarVisibleChanged
    '    cmbEmployeeList.Visible = Not CType(sender, Boolean)
    '    cmbDocumentList.Visible = Not CType(sender, Boolean)
    'End Sub
#End Region


    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub

#Region " Show Status With Color "

#Region "Variable Declaration"
    ' Table with TimeEntry data after PIVOT
    Private _dtEntry As New DataTable("Entry")
    ' Columns 
    Private mDocumentList As DocumentList
    ' Rows
    Private mLicenseNoListWithEmployee As LicenseNoListWithEmployee
    ' TimeEntry data before PIVOT
    ' EmployeeDocumentDueList


#End Region


#Region " Methods "
    Protected Sub LoadGrid()
        grdMain.Controls.Clear()
        grdMain.Columns.Clear()
        _dtEntry.Columns.Clear()
        _dtEntry.Rows.Clear()


        mDocumentList = DocumentList.GetDocumentList(OnlyDocumentsInEmployee:=True)

        If cmbEmployeeList.SelectedIndex > 0 Then
            mLicenseNoListWithEmployee = LicenseNoListWithEmployee.GetLicenseNoList(SearchText:=cmbEmployeeList.SelectedItem.ToString, WithoutLicenseNoAlso:=1, OnlyEmployeesHavingDocuments:=True)
        Else '
            mLicenseNoListWithEmployee = LicenseNoListWithEmployee.GetLicenseNoList(WithoutLicenseNoAlso:=1, OnlyEmployeesHavingDocuments:=True)

        End If
         mEmployeeDocumentDueList = EmployeeDocumentDueList.GetEmployeeDocumentDueList(New Guid(cmbEmployeeList.SelectedValue.ToString), New Guid(cmbDocumentList.SelectedValue.ToString), txtDocumentNo.Text, txtAsOnDate.Text, cmbRange.SelectedIndex, , , , cmbDepartmentList.SelectedValue.ToString, IsForShowingColorcode:=True)


        If mEmployeeDocumentDueList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If


        ' Add first column which is used to store labels 
        _dtEntry.Columns.Add("EmpName")
        _dtEntry.Columns.Add("LicenseNo")

        ' Create the column in the grid
        Dim tfProject As New TemplateField()
        grdMain.Columns.Add(tfProject)


        tfProject.ItemTemplate = New GridViewTemplate(ListItemType.Item, "EmpName", "0", "String", True)
        tfProject.HeaderTemplate = New GridViewTemplate(ListItemType.Header, "Employee", "0", "String", True)

        Dim tfProject1 As New TemplateField()
        grdMain.Columns.Add(tfProject1)

        tfProject1.ItemTemplate = New GridViewTemplate(ListItemType.Item, "LicenseNo", "1", "String", True)
        tfProject1.HeaderTemplate = New GridViewTemplate(ListItemType.Header, "License Number", "1", "String", True)


        ' Create dynamic columns 
        Dim ic As Integer = 1

        For i As Integer = 0 To mDocumentList.Count - 1
            If mEmployeeDocumentDueList.Contains(mDocumentList(i).ID, "", "") Then
                ic += 1
                Dim tf As New TemplateField()
                tf.ItemTemplate = New GridViewTemplate(ListItemType.Item, mDocumentList(i).ID.ToString(), ic.ToString(), "Guid", True)
                tf.HeaderTemplate = New GridViewTemplate(ListItemType.Header, mDocumentList(i).Name.ToString(), ic.ToString(), "String", False)
                tf.HeaderStyle.Wrap = True
                tf.ItemStyle.Font.Bold = True
                tf.ItemStyle.HorizontalAlign = HorizontalAlign.Center

                grdMain.Columns.Add(tf)
                _dtEntry.Columns.Add(mDocumentList(i).ID.ToString())

            End If
        Next i




        ' Create rows in table
        Dim mLicenseNoListWithEmployeeInfo As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo
        For Each mLicenseNoListWithEmployeeInfo In mLicenseNoListWithEmployee

            If mEmployeeDocumentDueList.Contains(mLicenseNoListWithEmployeeInfo.EmpID, "") Then
                Dim r As DataRow = _dtEntry.NewRow()
                _dtEntry.Rows.Add(r)
                r(0) = mLicenseNoListWithEmployeeInfo.EmpName.ToString()

                r(1) = mLicenseNoListWithEmployeeInfo.LicenseNo.ToString()
            End If
        Next mLicenseNoListWithEmployeeInfo




        ' Do PIVOT processing for EmployeeDocumentDueListInfo rows 
        Dim mEmployeeDocumentDueListInfo As EmployeeDocumentDueList.EmployeeDocumentDueListInfo
        For Each mEmployeeDocumentDueListInfo In mEmployeeDocumentDueList
            Dim ie As Integer = 0
            Dim ip As Integer = 0
            For i As Integer = 0 To mDocumentList.Count - 1
                If mEmployeeDocumentDueList.Contains(mDocumentList(i).ID, "", "") Then
                    If mDocumentList(i).ID = mEmployeeDocumentDueListInfo.DocumentID Then
                        Exit For
                    Else
                        ie += 1
                    End If
                End If
            Next i
            If ie = mDocumentList.Count Then
                ' Throw New Exception("Unknown Certification")
                Exit For
            End If
            For Each mLicenseNoListWithEmployeeInfo In mLicenseNoListWithEmployee
                If mEmployeeDocumentDueList.Contains(mLicenseNoListWithEmployeeInfo.EmpID, "") Then
                    If mLicenseNoListWithEmployeeInfo.EmpID = mEmployeeDocumentDueListInfo.EmployeeID Then
                        Exit For
                    Else
                        ip += 1
                    End If
                End If
            Next mLicenseNoListWithEmployeeInfo
            If ip = mLicenseNoListWithEmployee.Count Then
                ' Throw New Exception("Unknown Part")
                Exit For
            End If
            _dtEntry.Rows(ip)((ie + 2)) = mEmployeeDocumentDueListInfo.DateOfExpiry.ToString()

        Next mEmployeeDocumentDueListInfo



        Session("_dtEntry") = _dtEntry
        ' Set datasource to our newly created table and bind it to the grid
        grdMain.DataSource = _dtEntry
        grdMain.DataBind()
        If mLicenseNoListWithEmployee.Count > 15 Then
            btnShowStatusCloseTop.Visible = True
        Else
            btnShowStatusCloseTop.Visible = False
        End If

        upnlGridShowStatus.Update()


        ' Save row and colum definitions to ViewState
        ViewState.Add("mDocumentList", mDocumentList)
        ViewState.Add("mLicenseNoListWithEmployee", mLicenseNoListWithEmployee)
        mdlPopUpShowStatus.Show()
    End Sub
    Protected Sub grdMain_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        ' If (e.Row.RowIndex >= 0) Then
        'For Each Cell As TableCell In e.Row.Cells
        '    If Cell.Text <> "" Then
        '        Cell.BackColor = Color.Red
        '    End If

        'Next
        '   End If
        If (e.Row.RowIndex <> -1) Then
            _dtEntry = Session("_dtEntry")
            For j As Integer = 2 To _dtEntry.Rows(e.Row.RowIndex).ItemArray.Length - 1
                If _dtEntry.Rows(e.Row.RowIndex).ItemArray(j).ToString <> "" Then


                    Dim testDate As Date = CDate(_dtEntry.Rows(e.Row.RowIndex).ItemArray(j).ToString)
                    Dim Date3Month As Date = CStr(DateAdd(DateInterval.Month, 3, CDate(txtAsOnDate.Text.ToString)))

                    If CDate(testDate.ToString) <= CDate(txtAsOnDate.Text.ToString) Then  'Expired 
                        e.Row.Cells(j).BackColor = Color.Red
                    End If

                    If CDate(testDate.ToString) <= CDate(Date3Month.ToString) And CDate(testDate.ToString) > CDate(txtAsOnDate.Text.ToString) Then 'Expiration within 3 Months
                        e.Row.Cells(j).BackColor = Color.Orange
                    End If

                    If CDate(testDate.ToString) > CDate(Date3Month.ToString) Then 'Expiration Over 3 Mobths
                        e.Row.Cells(j).BackColor = Color.Green

                    End If
                    e.Row.Cells(j).ForeColor = Color.White
                    e.Row.Cells(j).Wrap = False
                Else
                    e.Row.Cells(j).BackColor = Color.LightGray
                End If
            Next
        End If

    End Sub

#End Region


#Region "GridViewTemplate"
    ' Implements ITemplate interface for new columns
    Public Class GridViewTemplate
        Implements ITemplate

        Private _templateType As ListItemType
        Private _columnName As String
        Private _col As String
        Private _dataType As String
        Private _isLabel As Boolean

        Public Sub New(ByVal type As ListItemType, ByVal colname As String, ByVal col As String, ByVal DataType As String, ByVal isLabel As Boolean)
            _templateType = type
            _columnName = colname
            _dataType = DataType
            _col = col
            _isLabel = isLabel

        End Sub 'New



        Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
            Dim lbl As New Label()

            Select Case _templateType
                Case ListItemType.Header

                    container.Controls.Add(lbl)
                    lbl.Text = _columnName
                Case ListItemType.Item


                    AddHandler lbl.DataBinding, AddressOf lbl_DataBinding

                    container.Controls.Add(lbl)
                    If _columnName = "EmpName" Then
                        lbl.Width = 200
                    ElseIf _columnName = "LicenseNo" Then
                        lbl.Width = 100
                    End If

                    ''If _isLabel Then
                    ''    AddHandler lbl.DataBinding, AddressOf lbl_DataBinding

                    ''    container.Controls.Add(lbl)
                    ''Else
                    ''    Dim chk As New CheckBox
                    ''    chk.ID = "chk" + _col
                    ''    container.Controls.Add(chk)
                    ''    AddHandler chk.DataBinding, AddressOf chk_DataBinding
                    ''End If
            End Select
        End Sub 'ITemplate.InstantiateIn
        ' Databind an edit box in the grid
        Sub chk_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim txtdata As CheckBox = CType(sender, CheckBox)
            Dim container As GridViewRow = CType(txtdata.NamingContainer, GridViewRow)
            Dim dataValue As Object = DataBinder.Eval(container.DataItem, _columnName)
            ' Add JavaScript function sav(row,col,hours) which will save changes

            ' txtdata.Attributes.Add("onchange", "sav(" + container.RowIndex.ToString() + "," + _columnName + ",this.firstChild.checked)")
            If Not dataValue Is DBNull.Value Then
                txtdata.Checked = dataValue.ToString()
                'If CType(container.DataItem, System.Data.DataRowView).Row.ItemArray(1) = "Yes" Then 'Checked If Project Exists For selected Employee
                '    txtdata.Enabled = False
                'End If
            End If
        End Sub 'edt_DataBinding



        ' Databind a label 
        Sub lbl_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim lbl As Label = CType(sender, Label)
            Dim container As GridViewRow = CType(lbl.NamingContainer, GridViewRow)
            Dim dataValue As Object = DataBinder.Eval(container.DataItem, _columnName)

            If Not dataValue Is DBNull.Value Then
                lbl.Text = dataValue.ToString()
            End If
        End Sub 'lbl_DataBinding
    End Class 'GridViewTemplate
#End Region

    Private Sub btnShowStatusOnGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowStatusOnGrid.Click
        LoadGrid()

    End Sub

    Private Sub btnShowStatusClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowStatusClose.Click
        mdlPopUpShowStatus.Hide()
    End Sub

    Private Sub hyConverttoPdf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hyConverttoPdf.Click
        ''Try


        ''    Response.ContentType = "application/pdf"
        ''    Response.AddHeader("content-disposition", "attachment;filename=EmployeeDoc.pdf")
        ''    LoadGrid()
        ''    Response.Cache.SetCacheability(HttpCacheability.NoCache)
        ''    Dim sw As StringWriter = New StringWriter()
        ''    Dim hw As HtmlTextWriter = New HtmlTextWriter(sw)
        ''    grdMain.RenderControl(hw)
        ''    Dim style As String = "<style> .textmode { mso-number-format:\@; } </style>"
        ''    Response.Write(style)
        ''    Dim sr As StringReader = New StringReader(sw.ToString())
        ''    ''''Dim pdfDoc As Document = New Document(PageSize.A4, 10.0F, 10.0F, 10.0F, 0.0F)
        ''    Dim pdfDoc As iTextSharp.text.Document = New iTextSharp.text.Document(PageSize.A4, 10.0!, 10.0!, 10.0!, 0.0!)
        ''    Dim htmlparser As HTMLWorker = New HTMLWorker(pdfDoc)
        ''    PdfWriter.GetInstance(pdfDoc, Response.OutputStream)

        ''    pdfDoc.Open()
        ''    htmlparser.Parse(sr)
        ''    pdfDoc.Close()
        ''    Response.Write(pdfDoc)
        ''    Response.End()
        ''    grdMain.AllowPaging = True
        ''    grdMain.DataBind()
        ''Catch ex As Exception

        ''End Try

        If grdMain.Rows.Count > 0 Then
            Try
                grdMain.Columns(0).Visible = False
                Response.ClearContent()
                Response.Buffer = True
                Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "DocumentDue.xls"))
                Response.ContentEncoding = Encoding.UTF8
                Response.ContentType = "application/ms-excel"
                LoadGrid()
                Dim sw As New StringWriter()
                Dim htw As New HtmlTextWriter(sw)
                grdMain.RenderControl(htw)

                Dim style As String = "<style> .textmode { mso-number-format:\@; } </style>"
                Response.Write(style)
                Response.Write(sw.ToString())
                Response.Flush()
                Response.End()
                ' HttpContext.Current.ApplicationInstance.CompleteRequest()
            Catch ex As Exception
                Throw ex
            Finally
                grdMain.Columns(0).Visible = True
            End Try
        End If
    End Sub
#End Region

   
End Class