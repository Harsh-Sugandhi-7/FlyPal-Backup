Imports System.Linq
Imports System.Collections.Generic

Public Class wfrptFanBladeMonitoring_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mMachineNameValueList As MachineNameValueList
    Public mAssemblylist As AssemblyList
    Dim EventLogID As Guid
    Public mFanBladeMonitoring As FanBladeMonitoring
    Public mCompanyDetail As New CompanyDetail
    Dim EventLogDetail As String
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        mFanBladeMonitoring = CType(Session("mFanBladeMonitoring"), FanBladeMonitoring)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mAssemblylist")
        Session.Remove("mFanBladeMonitoring")
    End Sub
    Private Sub SetAssemblyCombo()
        If cmbAircraft.SelectedIndex > 0 Then
            'mAssemblylist = AssemblyList.GetAssemblyListForComboBox(2, cmbAircraft.SelectedValue.ToString, txtAsOnDate.Text, "(ALL)")
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(2, cmbAircraft.SelectedValue.ToString, txtAsOnDate.Text, AddTopItem:="", IsInstalled:=True)
            Session("mAssemblylist") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
        Else
            cmbAssembly.DataSource = Nothing
        End If
        cmbAssembly.DataBind()
    End Sub
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(txtAsOnDate.Text, , , , , , , True, "(SELECT)", , True)
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataSource = mMachineNameValueList
        cmbAircraft.DataBind()

        SetAssemblyCombo()
    End Sub
    Private Sub ControlVisibility()
        cmbAssembly.Enabled = IIf(cmbAircraft.SelectedIndex > 0, True, False)
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsFanBladeMonitoring
      
        EventLogDetail = "AsOn Date : " + txtAsOnDate.Text.ToString + "," + " Aircraft :" + cmbAircraft.SelectedItem.ToString + "," + " Assembly : " + cmbAssembly.SelectedItem.Text
        mFanBladeMonitoring = FanBladeMonitoring.GetFanBladeMonitoring(AsOnDate:=txtAsOnDate.Text, MachineID:=cmbAircraft.SelectedValue.ToString, AssemblyID:=cmbAssembly.SelectedValue.ToString)

        If mFanBladeMonitoring.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf (mFanBladeMonitoring.Count > 0 And IsExcel = False) Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1545)
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
              mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
              mCompanyDetail.WebSite, ReportName:="Fan Blades Distribution Plot", SearchStr1:=New SmartDate(txtAsOnDate.Text).FormattedText, SearchStr2:=cmbAircraft.SelectedItem.ToString, _
              SearchStr3:=cmbAssembly.SelectedItem.ToString, SearchStr4:=mAssemblylist(New Guid(cmbAssembly.SelectedValue)).SerialNo, SearchStr5:="", ProductVersion:=AppSettings("Product Version"), SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"), _
              SearchStr11:="", SearchStr12:="" & mCompanyDetail.Fax & " " & mCompanyDetail.Email, SearchStr13:="", _
              SearchStr14:="", SearchStr16:="", SearchStr15:="", _
              SearchStr17:="", SearchStr18:="", SearchStr19:="", SearchStr20:="", _
              SearchStr21:="", SearchStr22:="", SearchStr23:="", SearchStr24:="", _
              SearchStr25:="", SINote:=AppSettings("SINote"))
        If IsExcel = False Then
            myReport = New crptFanBladeMonitoring
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mFanBladeMonitoring)
            da.Fill(ds, Report)
            da.Fill(ds, mrptImage)
            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport
            Dim Str1 As String
            Str1 = "openTranDetail();"
            MarkLog(Util.Action.Print, "FanBladeMonitoring", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
        Else
            ds.Clear()
            da.Fill(ds, mFanBladeMonitoring)
            da.Fill(ds, "ReportData", Report)

            Dim columnToRemove As String() = {"CompStatusID", "ItemDescription", "Position", "InstalledOn", "RemovedOn", "Sort", "IsFanBladeDistribution"}
            Dim columnToRemove1 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ReportName", "ProductVersion", "SINote", "ReportDate", _
                                               "SearchStr4", "SearchStr5", "SearchStr6", "SearchStr7", "CurrencyName", "CurrencySymbol", "SearchStr8", "SearchStr9", _
                                               "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", "SearchStr15", "SearchStr16", _
                                               "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", _
                                               "SearchStr25"}
            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("FanBladeMonitoring").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("FanBladeMonitoring").Columns.Remove(columnToRemove(i))
                End If
            Next

            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove1(i))
                End If
            Next

            'set Column Sequence
            ds.Tables("FanBladeMonitoring").Columns("FanBladePosition").SetOrdinal(0)
            ds.Tables("FanBladeMonitoring").Columns("ItemName").SetOrdinal(1)
            ds.Tables("FanBladeMonitoring").Columns("CompSerialNo").SetOrdinal(2)
            ds.Tables("FanBladeMonitoring").Columns("MomentWeight").SetOrdinal(3)
            ds.Tables("FanBladeMonitoring").Columns("BalanceScrew").SetOrdinal(4)

             Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("FanBladeMonitoring"))


            dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "As On Date"
            dsNew.Tables("ReportData").Columns("SearchStr2").ColumnName = "Aircraft"
            dsNew.Tables("ReportData").Columns("SearchStr3").ColumnName = "Assembly"
            
            dsNew.Tables("FanBladeMonitoring").Columns("FanBladePosition").ColumnName = "Position"
            dsNew.Tables("FanBladeMonitoring").Columns("ItemName").ColumnName = "Part No."
            dsNew.Tables("FanBladeMonitoring").Columns("CompSerialNo").ColumnName = "Serial No."
            dsNew.Tables("FanBladeMonitoring").Columns("MomentWeight").ColumnName = "Moment Weight"
            dsNew.Tables("FanBladeMonitoring").Columns("BalanceScrew").ColumnName = "Balance Screw"

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("FanBladeMonitoring").TableName = "Fan Blades Distribution Plot"
			Session("ExcelFileName") = "Fan Blades Distribution Plot"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "FanBladeMonitoring", "Export To excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal ByVale As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            cmbAircraft.Focus()
            txtAsOnDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport(False)
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            SetReport(True)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtAsOnDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAsOnDate.TextChanged
        DataFieldBind()
        ControlVisibility()
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        SetAssemblyCombo()
        ControlVisibility()
    End Sub
#End Region

End Class