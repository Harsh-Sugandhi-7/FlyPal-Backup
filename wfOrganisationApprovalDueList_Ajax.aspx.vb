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


Public Class wfOrganisationApprovalDueList_Ajax
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
    Protected mOrganisationApprovalDueList As OrganisationApprovalDueList
    Protected mEmployeeList As EmployeeList
    Protected mDocument As DocumentList
    Public DateRange As String = ""
    Private mEmployeeDepartmentList As EmployeeDepartmentList
    Dim Report As ReportData
    Public EventLogDetails As String = String.Empty
#End Region

#Region " Business Method "
    Private Sub GetSession()
        mOrganisationApprovalDueList = Session("mOrganisationApprovalDueList")
    End Sub
    Private Sub SetSession()
        Session("mOrganisationApprovalDueList") = mOrganisationApprovalDueList
    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mOrganisationApprovalDueList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Public Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        GetSession()
        SetValues()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim obj As OrganisationApprovalDueList
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim dsOrganisationApprovalDueList As New dsOrganisationApprovalDueList
        myReport = New crptOrganisationApprovalDueList

        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim SearchStr3 As String
        Dim SearchStr4 As String

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

        Dim SearchStr5 As String

        mOrganisationApprovalDueList = OrganisationApprovalDueList.GetOrganisationApprovalDueList(AsOnDate:=txtAsOnDate.Text, Range:=cmbRange.SelectedIndex, _
                                                                                                  DocumentID:=cmbDocumentList.SelectedValue.ToString, _
                                                                                                  DocumentNumber:=txtDocumentNo.Text)

        If mOrganisationApprovalDueList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Report = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, "Organisation Approval Due List", SearchStr1, SearchStr2, SearchStr3, SearchStr4, SearchStr5, _
             AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        dsOrganisationApprovalDueList.Clear()
        If IsExcel = False Then
            Dim mrptImage As rptImage = rptImage.GetImage(dsOrganisationApprovalDueList)
            da.Fill(dsOrganisationApprovalDueList, mOrganisationApprovalDueList)
            da.Fill(dsOrganisationApprovalDueList, mrptImage)
            da.Fill(dsOrganisationApprovalDueList, Report)
            myReport.SetDataSource(dsOrganisationApprovalDueList)
            Session("CrystalReport") = myReport

            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "OrganisationApprovalDueList", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Else
            da.Fill(dsOrganisationApprovalDueList, "OrganisationApprovalDueList", mOrganisationApprovalDueList)
            da.Fill(dsOrganisationApprovalDueList, "ReportData", Report)
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
                If dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns.Contains(columnToRemove(i)) Then
                    dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns.Remove(columnToRemove(i))
                End If
            Next
            Dim columnToRemoveCriteria As String() = {"ReportDate", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", _
                                                   "ReportName", "SearchStr1", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", _
                                                   "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr10", "SearchStr11", _
                                                   "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", _
                                                   "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100", _
                                                   "ShortName"}

            For i As Integer = 0 To columnToRemoveCriteria.Length - 1
                If dsOrganisationApprovalDueList.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
                    dsOrganisationApprovalDueList.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
                End If
            Next

            If dsOrganisationApprovalDueList.Tables("ReportData").Columns.Contains("SearchStr4") Then
                dsOrganisationApprovalDueList.Tables("ReportData").Columns("SearchStr4").ColumnName = "Date"
            End If
            If dsOrganisationApprovalDueList.Tables("ReportData").Columns.Contains("SearchStr2") Then
                dsOrganisationApprovalDueList.Tables("ReportData").Columns("SearchStr2").ColumnName = "Document"
            End If
            If dsOrganisationApprovalDueList.Tables("ReportData").Columns.Contains("SearchStr3") Then
                dsOrganisationApprovalDueList.Tables("ReportData").Columns("SearchStr3").ColumnName = "Document No."
            End If

            If dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns.Contains("DocumentName") Then
                dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns("DocumentName").ColumnName = "Type of licence/approval/permit."
            End If
            If dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns.Contains("DocumentNumber") Then
                dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns("DocumentNumber").ColumnName = "Approval/Permit/NOC no."
            End If
            If dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns.Contains("IssuingAuthority") Then
                dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns("IssuingAuthority").ColumnName = "Issuing authority"
            End If
            If dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns.Contains("WarningDays") Then
                dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns("WarningDays").ColumnName = "Warning Days"
            End If

            If dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns.Contains("DateOfIssue") Then
                dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns("DateOfIssue").ColumnName = "Date of Issue"
            End If
            If dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns.Contains("DateOfExpiry") Then
                dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns("DateOfExpiry").ColumnName = "Date of Expiry"
            End If
            If dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns.Contains("WarningDateOfExpiry") Then
                dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns("WarningDateOfExpiry").ColumnName = "Warning Date"
            End If
            If dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns.Contains("RemainingDays") Then
                dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns("RemainingDays").ColumnName = "Remaining Days As Per Warning Days"
            End If
            If dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns.Contains("RemainingDaysAsPerExpiryDate") Then
                dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").Columns("RemainingDaysAsPerExpiryDate").ColumnName = "Remaining Days as per Expiry Date"
            End If
            
            Dim ReportLabel As String = "Organisation Approval Due List"

            Dim dataview As DataView = dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").DefaultView
            dsOrganisationApprovalDueList.Tables("OrganisationApprovalDueList").TableName = ReportLabel.Replace("/", " ")


            dsOrganisationApprovalDueList.Tables("ReportData").TableName = "Searching Criteria"
            Session("DataTableToBeFormattedForExportToExcel") = ReportLabel.Replace("/", " ")
            Dim dsNew As New DataSet
            dsNew.Clear()
			Session("ExcelFileName") = ReportLabel.Replace("/", " ")

			dsNew.Merge(dsOrganisationApprovalDueList.Tables("Searching Criteria"))
            dsNew.Merge(dataview.ToTable())
            Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "OrganisationApprovalDueList", "Export To Excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Sub SetValues()
        Dim mEmployee As String = ""
        Dim mDocument As String = ""
        If cmbDocumentList.SelectedIndex > 0 Then
            mDocument = cmbDocumentList.SelectedItem.Text
            lblDocumentCriteria.Text = "Document : " & mDocument
        Else
            mDocument = ""
            lblDocumentCriteria.Text = "Document : ALL"
        End If
        EventLogDetails = lblEmployeeCriteria.Text + " " + lblDocumentCriteria.Text + ", Document No.: " + txtDocumentNo.Text
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        cmbDocumentList.DataSource = DocumentList.GetDocumentList(, "(ALL)")
        cmbDocumentList.DataBind()
        txtAsOnDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        txtAsOnDate.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If cmbDocumentList.Enabled = True Then
                setFocus(cmbDocumentList)
            End If
            DataFieldBind()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            SetReport(False)
        End If
    End Sub

    Private Sub btnExpotToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpotToExcel.Click
        If IsValid() Then
            SetReport(True)
        End If

        'If IsValid = True Then


        '    Dim da As New CSLA10.Data.ObjectAdapter
        '    Dim ds As New dsOrganisationApprovalDueList

        '    SetReport()

        '    If mOrganisationApprovalDueList.Count = 0 Then
        '        MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
        '        Exit Sub
        '    End If

        '    ds.Clear()
        '    da.Fill(ds, "OrganisationApprovalDueList", mOrganisationApprovalDueList)
        '    da.Fill(ds, "ReportData", Report)
        '    Dim columnToRemove As String() = { _
        '                                       "ID", _
        '                                       "EmployeeID", _
        '                                       "DocumentID", _
        '                                       "DocumentValidityInID", _
        '                                       "DoneStatus", _
        '                                       "ImageFile", _
        '                                       "ImageSize", _
        '                                       "FileExtension", "ReferenceID", "HistoryCount", "IsApplicable" _
        '                                     }
        '    For i As Integer = 0 To columnToRemove.Length - 1
        '        If ds.Tables("OrganisationApprovalDueList").Columns.Contains(columnToRemove(i)) Then
        '            ds.Tables("OrganisationApprovalDueList").Columns.Remove(columnToRemove(i))
        '        End If
        '    Next
        '    Dim columnToRemoveCriteria As String() = {"ReportDate", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", _
        '                                           "ReportName", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", _
        '                                           "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr10", "SearchStr11", _
        '                                               "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", _
        '                                              "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100" _
        '                                          }

        '    For i As Integer = 0 To columnToRemoveCriteria.Length - 1
        '        If ds.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
        '            ds.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
        '        End If
        '    Next
        '    For i As Integer = 0 To ds.Tables("ReportData").Columns.Count - 1
        '        If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr1" Then
        '            ds.Tables("ReportData").Columns(i).ColumnName = "Employee"
        '        End If
        '        If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr5" Then
        '            ds.Tables("ReportData").Columns(i).ColumnName = "Department"
        '        End If
        '        If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr2" Then
        '            ds.Tables("ReportData").Columns(i).ColumnName = "Training"
        '        End If
        '        If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr3" Then
        '            ds.Tables("ReportData").Columns(i).ColumnName = "Training Org"
        '        End If

        '        If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr4" Then
        '            ds.Tables("ReportData").Columns(i).ColumnName = "As On Date"
        '        End If
        '    Next
        '    Dim ReportLabel As String = "Organisation Approval Due List"

        '    Dim dataview As DataView = ds.Tables("OrganisationApprovalDueList").DefaultView
        '    ds.Tables("OrganisationApprovalDueList").TableName = ReportLabel.Replace("/", " ")


        '    ds.Tables("ReportData").TableName = "Searching Criteria"
        '    Session("DataTableToBeFormattedForExportToExcel") = ReportLabel.Replace("/", " ")
        '    Dim dsNew As New DataSet
        '    dsNew.Clear()


        '    dsNew.Merge(ds.Tables("Searching Criteria"))
        '    dsNew.Merge(dataview.ToTable())
        '    Session("dsNew") = dsNew
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        '    'Added by Prashant on 19-Jan-2021
        '    MarkLog(Util.Action.Print, "OrganisationApprovalDueList", "Export To Excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End If
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
    Private Sub txtDocumentNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDocumentNo.TextChanged
        setFocus(txtDocumentNo)
    End Sub
#End Region
End Class