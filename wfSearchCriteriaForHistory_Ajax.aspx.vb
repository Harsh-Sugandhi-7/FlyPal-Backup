'Added By Vikrant On 01-Apr-2014

Public Class wfSearchCriteriaForHistory_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaratioin"
    Dim mtmpHistoryList As tmpHistoryList
    Dim mAssemblyList As AssemblyList
    Dim mCompList As CompList
    Dim mEAssemblyList As AssemblyList

    Dim ListID As Guid
    Dim MacID As String = ""
    Dim MacID1 As String = ""
    Dim I As Integer
    Dim chkFindModel As Boolean = False
    Dim chkFindNow As Boolean = False
    Dim mMacList As AssemblyList
    Dim ListMacID1 As String
    Dim ListStartDate As String
    Dim ListEndDate As String
    Dim ListPartNo As String
    Dim ListCompSerialNo As String
    Dim ListModel As String
    Dim ListSerialNo As String
    Dim ListAModelNo As String
    Dim ListASerialNo As String

    Dim StartDate As String
    Dim EndDate As String
    Dim strWorkOrderNo As String
    Dim IsRemoved As Boolean = False
    Dim IsComplied As Boolean = False
    Dim IsInstalled As Boolean = False
    Dim AssemblyType As String
    Dim ReportLabel As String
    Dim mEventLogDetails As String = String.Empty

    Public mATAList As ATAList          'Added by Saylee on 11-Dec-2014 for BA11122014
    Private mATACode As Integer
    Private mATANomenclature As String   'Added by Saylee on 11-Dec-2014 for BA11122014
    Dim IsTSICSI As Boolean = False
#End Region

#Region " Helper Methods "
    Private Sub FindNowModel(ByVal ListModel As String, ByVal ListSerialNo As String)
        mAssemblyList = Nothing
        dgModel.DataSource = Nothing
        mAssemblyList = AssemblyList.GetAssemblyList(ListModel, ListSerialNo, , , Today.Date.ToString)
        dgModel.DataSource = mAssemblyList
        Session("mAssemblyList") = mAssemblyList
        dgModel.DataBind()
        lblResult.Text = "List of Model & Serial No.s : " & mAssemblyList.Count & " Record(s) found."
    End Sub
    Private Sub FindNowEModel(ByVal ListAModelNo As String, ByVal ListASerialNo As String)
        mEAssemblyList = Nothing
        dgEModel.DataSource = Nothing
        If chkRemoval.Checked = True Then
            mEAssemblyList = AssemblyList.GetAssemblyList(ListAModelNo, ListASerialNo, , , txtFromDate.Text.ToString)
        Else
            mEAssemblyList = AssemblyList.GetAssemblyList(ListAModelNo, ListASerialNo, , , Today.Date.ToString)
        End If

        Session("mEAssemblyList") = mEAssemblyList
        dgEModel.DataSource = mEAssemblyList
        dgEModel.DataBind()
        lblResult3.Text = "List of Model & Serial No.s : " & mEAssemblyList.Count & " Record(s) found."
    End Sub
    Private Sub FindNowPart(ByVal ListPartNo As String, ByVal ListCompSerialNo As String, ByVal ListEndDate As String)
        mCompList = Nothing
        dgPart.DataSource = Nothing
        mCompList = CompList.GetCompList(ListPartNo, ListCompSerialNo, ListEndDate)
        dgPart.DataSource = mCompList
        dgPart.DataBind()
        Session("mCompList") = mCompList
        lblResult2.Text = "List of Part & Serial No.s : " & mCompList.Count & " Record(s) found."
    End Sub
    Public Sub SetMachineID()
        If ((chkFindNow = True)) Or ((chkFindNow = False)) Then
            mMacList = AssemblyList.GetAssemblyList(txtModelNo.Text, txtSerialNo.Text, 1, "{00000000-0000-0000-0000-000000000000}", Today.Date.ToString)
            If mMacList.Count = 0 Then
                MacID = "{00000000-0000-0000-0000-000000000000}"
            Else
                For I = 0 To mMacList.Count - 1
                    If MacID = "" Then
                        MacID = MacID & "{" & mMacList(0).MachineID.ToString & "}"
                    Else
                        'MacID = MacID & "','{" & mMacList(I).MachineID.ToString & "}"
                        MacID = MacID & ",{" & mMacList(I).MachineID.ToString & "}"
                    End If
                Next
            End If
        ElseIf chkFindNow = True Then
            mMacList = AssemblyList.GetAssemblyList("", "", , ListID.ToString, Today.Date.ToString)
            MacID = "{" & mMacList(0).MachineID.ToString & "}"
        End If
        ReportDetail()
    End Sub
    Private Sub ResetValues()
        StartDate = txtFromDate.Text
        EndDate = txtToDate.Text
        txtModelNo.Text = ""
        txtSerialNo.Text = ""
        txtAModelNo.Text = ""
        txtASerialNo.Text = ""
        txtCPartNo.Text = ""
        txtCSerialNo.Text = ""
        ListStartDate = ""
        ListEndDate = ""
        ListPartNo = ""
        ListCompSerialNo = ""
        ListModel = ""
        ListSerialNo = ""
        ListAModelNo = ""
        ListASerialNo = ""
        AssemblyType = ""
        strWorkOrderNo = ""
        MacID = ""
        MacID1 = ""
        ListMacID1 = ""
        IsInstalled = False
        IsRemoved = False
        IsComplied = False
        chkFindNow = False
        chkFindModel = False
    End Sub
    Public Sub SetModel()
        If ((chkFindNow = True And chkFindModel = True)) _
        Or (chkFindNow = False And chkFindModel = True) Or (chkFindNow = False And chkFindModel = False) Then
            mMacList = AssemblyList.GetAssemblyList(txtModelNo.Text, txtSerialNo.Text, , "{00000000-0000-0000-0000-000000000000}", Today.Date.ToString)

            'MacID1 = mMacList.GetDistinctMachineIDListString

            For I = 0 To mMacList.Count - 1
                If MacID1 = "" Then
                    MacID1 = MacID1 & "{" & mMacList(0).MachineID.ToString & "}"
                Else
                    MacID1 = MacID1 & "','{" & mMacList(I).MachineID.ToString & "}"
                End If
            Next I
        ElseIf ((chkFindNow = True And chkFindModel = True)) Or (chkFindNow = True) Then
            mMacList = AssemblyList.GetAssemblyList(txtModelNo.Text, txtSerialNo.Text, , ListID.ToString, Today.Date.ToString)
            MacID1 = "{" & mMacList(0).MachineID.ToString & "}"
        End If
    End Sub
    Private Sub GetSession()
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mEAssemblyList = CType(Session("mEAssemblyList"), AssemblyList)
        mCompList = CType(Session("mCompList"), CompList)

        ListModel = CType(Session("ListModel"), String)
        ListSerialNo = CType(Session("ListSerialNo"), String)
        ListModel = IIf(IsNothing(ListModel), "", ListModel)
        ListSerialNo = IIf(IsNothing(ListSerialNo), "", ListSerialNo)

        ListPartNo = CType(Session("ListPartNo"), String)
        ListCompSerialNo = CType(Session("ListCompSerialNo"), String)
        ListPartNo = IIf(IsNothing(ListPartNo), "", ListPartNo)
        ListCompSerialNo = IIf(IsNothing(ListCompSerialNo), "", ListCompSerialNo)

        ListAModelNo = CType(Session("ListAModelNo"), String)
        ListASerialNo = CType(Session("ListASerialNo"), String)
        ListAModelNo = IIf(IsNothing(ListAModelNo), "", ListAModelNo)
        ListASerialNo = IIf(IsNothing(ListASerialNo), "", ListASerialNo)
        ListID = Session("ListID")

        chkFindModel = Session("chkFindModel")
        chkFindNow = Session("chkFindNow")

        mATAList = CType(Session("mATAList"), ATAList)
    End Sub
    Public Sub SetSession()
        Session("mAssemblyList") = mAssemblyList
        Session("mEAssemblyList") = mEAssemblyList
        Session("mCompList") = mCompList

        Session("ListModel") = ListModel
        Session("ListSerialNo") = ListSerialNo
        Session("ListPartNo") = ListPartNo
        Session("ListCompSerialNo") = ListCompSerialNo
        Session("ListAModelNo") = ListAModelNo
        Session("ListASerialNo") = ListASerialNo
        Session("ListID") = ListID

        Session("chkFindModel") = chkFindModel
        Session("chkFindNow") = chkFindNow
        Session("mATAList") = mATAList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForHistory_Ajax.aspx?" Then
            Session.Remove("mAssemblyList")
            Session.Remove("mEAssemblyList")
            Session.Remove("mCompList")

            Session.Remove("ListModel")
            Session.Remove("ListSerialNo")
            Session.Remove("ListPartNo")
            Session.Remove("ListCompSerialNo")
            Session.Remove("ListAModelNo")
            Session.Remove("ListASerialNo")
            Session.Remove("ListID")

            Session.Remove("chkFindModel")
            Session.Remove("chkFindNow")
            Session.Remove("mATAList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
   
    Private Sub ControlVisibility(Optional ByVal IsAssembly As Boolean = False, Optional ByVal IsComponent As Boolean = False)
        If IsAssembly = True Then
            If chkAssembly.Checked Then
                pnlEModel.Visible = True
                dgEModel.Visible = True

                dgEModel.DataSource = mEAssemblyList
                dgEModel.DataBind()

                lblAModelNo.Visible = True
                txtAModelNo.Visible = True
                lblASerialNo.Visible = True
                txtASerialNo.Visible = True
                btnFindModel.Visible = True
                lblResult3.Visible = True
            Else
                pnlEModel.Visible = False
                dgEModel.Visible = False
                lblAModelNo.Visible = False
                txtAModelNo.Visible = False
                lblASerialNo.Visible = False
                txtASerialNo.Visible = False
                btnFindModel.Visible = False
                lblResult3.Visible = False
            End If
            upnlAssembly.Update()
        End If

        If IsComponent Then
            If chkComponent.Checked Then
                pnlPart.Visible = True
                dgPart.Visible = True

                dgPart.DataSource = mCompList
                dgPart.DataBind()

                lblCPartNo.Visible = True
                txtCPartNo.Visible = True
                lblCSerialNo.Visible = True
                txtCSerialNo.Visible = True
                btnFindPart.Visible = True
            Else
                pnlPart.Visible = False
                dgPart.Visible = False
                lblCPartNo.Visible = False
                txtCPartNo.Visible = False
                lblCSerialNo.Visible = False
                txtCSerialNo.Visible = False
                btnFindPart.Visible = False
            End If

            upnlComponent.Update()
        End If
    End Sub
    Private Sub Display()
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        lblWorkOrderNo1.Visible = True
        lblAssemblyType1.Visible = True
        lblModelNo1.Visible = True
        lblSerialNo1.Visible = True
        lblAModelNo1.Visible = True
        lblASerialNo1.Visible = True
        lblCPartNo1.Visible = True
        lblCSerialNo1.Visible = True
        lblRemovalFrom.Visible = True
        lblRemovalof.Visible = True

        upnlCurrentCriteria.Update()
    End Sub
    Private Sub SetValues()
        If cmbAssemblyType.SelectedItem.Text = "(All)" Then
            AssemblyType = ""
        Else
            AssemblyType = cmbAssemblyType.SelectedItem.Text
        End If
       
        If (chkAssembly.Checked And chkComponent.Checked) Then
            ListModel = txtModelNo.Text
            ListSerialNo = txtSerialNo.Text
            ListAModelNo = txtAModelNo.Text
            ListASerialNo = txtASerialNo.Text
            ListPartNo = txtCPartNo.Text
            ListCompSerialNo = txtCSerialNo.Text
        ElseIf (chkAssembly.Checked And chkComponent.Checked = False) Then
            ListModel = txtModelNo.Text
            ListSerialNo = txtSerialNo.Text
            ListAModelNo = txtAModelNo.Text
            ListASerialNo = txtASerialNo.Text
            ListPartNo = ""
            ListCompSerialNo = ""
        ElseIf (chkComponent.Checked And chkAssembly.Checked = False) Then
            ListModel = txtModelNo.Text
            ListSerialNo = txtSerialNo.Text
            ListAModelNo = ""
            ListASerialNo = ""
            ListPartNo = txtCPartNo.Text
            ListCompSerialNo = txtCSerialNo.Text
        End If
        MacID = ""
        strWorkOrderNo = txtWorkOrderNo.Text
        If Not IsDate(txtFromDate.Text) Then
            StartDate = ""
        Else
            StartDate = CDate(txtFromDate.Text).ToString(AppSettings("DateFormat"))
        End If
        If Not IsDate(txtToDate.Text) Then
            EndDate = ""
        Else
            EndDate = CDate(txtToDate.Text).ToString(AppSettings("DateFormat"))
        End If

        'lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", StartDate, "")
        'lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", EndDate, "")
        If (StartDate <> "") Then
            lblDateRangeFrom.Text = "From Date : " & CDate(txtFromDate.Text).ToString(AppSettings("DateFormat"))
        Else
            lblDateRangeFrom.Text = "From Date : "
        End If

        If (EndDate <> "") Then
            lblDateRangeTo.Text = "To Date : " & CDate(txtToDate.Text).ToString(AppSettings("DateFormat"))
        Else
            lblDateRangeTo.Text = "To Date : "
        End If
        lblWorkOrderNo1.Text = "Work Order No. : " & IIf(strWorkOrderNo <> "", strWorkOrderNo, "All")
        lblAssemblyType1.Text = "Assembly : " & IIf(AssemblyType <> "", AssemblyType, "All")
        lblModelNo1.Text = "Model : " & IIf(ListModel <> "", ListModel, "All")
        lblSerialNo1.Text = "Serial No. : " & IIf(ListSerialNo <> "", ListSerialNo, "All")
        lblAModelNo1.Text = "Model : " & IIf(ListAModelNo <> "", ListAModelNo, "All")
        lblASerialNo1.Text = "Assembly Serial No. : " & IIf(ListASerialNo <> "", ListASerialNo, "All")
        lblCPartNo1.Text = "Part No. : " & IIf(ListPartNo <> "", ListPartNo, "All")
        lblCSerialNo1.Text = "Component Serial No. : " & IIf(ListCompSerialNo <> "", ListCompSerialNo, "All")
        mEventLogDetails = lblDateRangeFrom.Text + "; " + lblDateRangeTo.Text + "; " + lblWorkOrderNo1.Text + "; " + "Installation To/Removal From/Compliance On Info. : " + lblModelNo1.Text + ", " + lblSerialNo1.Text + "; " + "Installation/Removal/Compliance On/of Info. : " + lblAModelNo1.Text + ", " + lblASerialNo1.Text + ", " + lblCPartNo1.Text + ", " + lblCSerialNo1.Text
    End Sub
    Public Sub ReportDetail()

        mtmpHistoryList = tmpHistoryList.GetHistoryList(StartDate, EndDate, strWorkOrderNo, AssemblyType, ListModel, _
            ListSerialNo, ListAModelNo, ListASerialNo, ListPartNo, ListCompSerialNo, MacID, chkAssembly.Checked, chkComponent.Checked, _
            IsRemoved, IsInstalled, IsComplied, , , chkIsRemUnschedule.Checked, IIf(cmbATAChapter.SelectedIndex > 0, cmbATAChapter.SelectedValue.ToString, "{00000000-0000-0000-0000-000000000000}"))
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean)
        'Session("IsExcel") = IsExcel
        Dim RptCommonHistory As CrystalDecisions.CrystalReports.Engine.ReportClass
        mtmpHistoryList = New tmpHistoryList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportHistoryList
        Dim mCompanyDetail As New CompanyDetail

        SetValues()

        RptCommonHistory = New crCommonHistory

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Or (AppSettings("ClientCode") = "YA") Or (AppSettings("ClientCode") = "TA")) Then
            If chkInstallation.Checked Or chkRemoval.Checked Or chkIsRemUnschedule.Checked Then
                RptCommonHistory = New crCommonHistoryBA
            End If
        End If

        IsRemoved = chkRemoval.Checked
        IsInstalled = chkInstallation.Checked
        IsComplied = chkCompliance.Checked
        IsTSICSI = chkTSICSI.Checked

        SetMachineID()
        'If cmbAssemblyType.SelectedIndex > 0 Then
        '    ReportLabel = "Common History For" + " " + cmbAssemblyType.SelectedItem.ToString
        'Else
        '    ReportLabel = "Common History For All"
        'End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                    mCompanyDetail.WebSite, "Common History Register", txtFromDate.Text, txtToDate.Text, AssemblyType, txtModelNo.Text, txtSerialNo.Text, AppSettings("Product Version"), AppSettings("SINote"), IIf(IsTSICSI = True, "True", "False"), txtAModelNo.Text, txtASerialNo.Text, txtCPartNo.Text, AppSettings("Logo"), SearchStr11:=txtCSerialNo.Text) 'Changed By Utkarsh For Report Logo.

        If mtmpHistoryList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
            'Added By Utkarsh On 7-Jun-2011 For All07062011
        ElseIf mtmpHistoryList.Count > 0 And Not IsExcel Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 629)
            'End

        End If
        If IsExcel = False Then 'If PDF format
            ds.Clear()
            '-----------Added by Utkarsh for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            '----------------------------------------------------------
            da.Fill(ds, mtmpHistoryList)
            da.Fill(ds, Report)
            da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
            RptCommonHistory.SetDataSource(ds)
            Session("CrystalReport") = RptCommonHistory
            ResetValues()
            SetSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            MarkLog(Util.Action.Print, "CommonHistory", mEventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ElseIf IsExcel = True Then  'Excel format
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "ExceltmpHistoryList", mtmpHistoryList)

            Dim columnToRemove As String()

            columnToRemove = {"mTSIValueFormatted", "mTSOValueFormatted", "ATANomenclature", "PeriodID", "DoneOnDateFormatted", "DoneOnDate", "Type1", "ID", _
                                    "ParentValue", "ChildValue", "Date", "ATACode", "AssignedManHours", "RequiredManHours", _
                                    "TSOValue", "TSIValue", "TSOOfHours", "TSOOfLanding", "TSOOfDate", "TSOOfCycle", "TSIOfHours", "TSIOfLanding", "TSIOfDate", "TSIOfCycle", "Description", "TSIValueFormatted", "TSOValueFormatted", "ChildValueFormatted", "ParentValueFormatted"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("ExceltmpHistoryList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("ExceltmpHistoryList").Columns.Remove(columnToRemove(i))
                End If
            Next


            If ds.Tables("ExceltmpHistoryList").Columns.Contains("DateFormatted") Then
                ds.Tables("ExceltmpHistoryList").Columns("DateFormatted").ColumnName = "Date"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("DescriptionForExcel") Then
                ds.Tables("ExceltmpHistoryList").Columns("DescriptionForExcel").ColumnName = "Description"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("LogPageNo") Then
                ds.Tables("ExceltmpHistoryList").Columns("LogPageNo").ColumnName = "Log Page No."
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("WorkOrderNo") Then
                ds.Tables("ExceltmpHistoryList").Columns("WorkOrderNo").ColumnName = "Work Order No."
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("RegNo") Then
                ds.Tables("ExceltmpHistoryList").Columns("RegNo").ColumnName = "Aircraft Reg / Tail number"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("FromOrToOrOnModel") Then
                ds.Tables("ExceltmpHistoryList").Columns("FromOrToOrOnModel").ColumnName = "Assembly"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("AssemblySerialNo") Then
                ds.Tables("ExceltmpHistoryList").Columns("AssemblySerialNo").ColumnName = "Assembly Serial No."
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("Type") Then
                ds.Tables("ExceltmpHistoryList").Columns("Type").ColumnName = "Assembly Type"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("OfModelOrPart") Then
                ds.Tables("ExceltmpHistoryList").Columns("OfModelOrPart").ColumnName = "Model/Part"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("SerialNo") Then
                ds.Tables("ExceltmpHistoryList").Columns("SerialNo").ColumnName = "Installation/Removal/Compliance On/of Serial No."
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("TSOValueFormattedForExcel") Then
                ds.Tables("ExceltmpHistoryList").Columns("TSOValueFormattedForExcel").ColumnName = "TSO"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("TSIValueFormattedForExcel") Then
                ds.Tables("ExceltmpHistoryList").Columns("TSIValueFormattedForExcel").ColumnName = "TSI"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("HistoryType") Then
                ds.Tables("ExceltmpHistoryList").Columns("HistoryType").ColumnName = "Maint.Activity"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("ChildValueFormattedForExcel") Then
                ds.Tables("ExceltmpHistoryList").Columns("ChildValueFormattedForExcel").ColumnName = "TSN"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("ParentValueFormattedForExcel") Then
                ds.Tables("ExceltmpHistoryList").Columns("ParentValueFormattedForExcel").ColumnName = "Parent Value"
            End If


            Dim columnToRemove2 As String() = {"ID", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "SearchStr3", "ProductVersion", "SINote", "ReportDate", "SearchStr6", "SearchStr10", "ShortName", "SearchStr12", "SearchStr13", "SearchStr14", "CurrencyName", "CurrencySymbol", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Installation To/Removal From/Compliance On Model No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Installation To/Removal From/Compliance On Serial No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr7") Then
                ds.Tables("ReportData").Columns("SearchStr7").ColumnName = "Installation/Removal/Compliance On/of Model No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr8") Then
                ds.Tables("ReportData").Columns("SearchStr8").ColumnName = "Installation/Removal/Compliance On/of Serial No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr9") Then
                ds.Tables("ReportData").Columns("SearchStr9").ColumnName = "Installation/Removal/Compliance On/of Part"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr11") Then
                ds.Tables("ReportData").Columns("SearchStr11").ColumnName = "Installation/Removal/Compliance On/of Comp Serial No."
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("ExceltmpHistoryList"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
			If ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Or (AppSettings("ClientCode") = "YA") Or (AppSettings("ClientCode") = "TA")) Then
				dsNew.Tables("ExceltmpHistoryList").TableName = "Technical Department"
				Session("ExcelFileName") = "Technical Department"
			Else
				dsNew.Tables("ExceltmpHistoryList").TableName = "Common History Register"
				Session("ExcelFileName") = "Common History Register"
			End If

			'Session("ExcelFileName") = dsNew.Tables("ExceltmpHistoryList").TableName
			Session("dsNew") = dsNew
			ResetValues()
            SetSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "CommonHistory", "Export To Excel " + mEventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAssemblyList = AssemblyList.GetAssemblyList(ListModel, ListSerialNo, , , Today.Date.ToString)
        dgModel.DataSource = mAssemblyList
        Session("mAssemblyList") = mAssemblyList

        mEAssemblyList = AssemblyList.GetAssemblyList(ListAModelNo, ListASerialNo, , , Today.Date.ToString)
        dgEModel.DataSource = mEAssemblyList
        Session("mEAssemblyList") = mEAssemblyList

        mCompList = CompList.GetCompList(ListPartNo, ListCompSerialNo, Today.Date.ToShortDateString)
        dgPart.DataSource = mCompList
        Session("mCompList") = mCompList

        'Added by Saylee on 11-Dec-2014 for BA11122014
        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList
        cmbATAChapter.DataBind()
        '***************************

        lblResult.Text = "List of Model & Serial No.s : " & mAssemblyList.Count & " Record(s) found."
        lblResult3.Text = "List of Model & Serial No.s : " & mEAssemblyList.Count & " Record(s) found."
        lblResult2.Text = "List of Part & Serial No.s : " & mCompList.Count & " Record(s) found."
        DataBind()
    End Sub
#End Region

#Region " EVENTS"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfSearchCriteriaForHistory_Ajax.aspx?"
            DataFieldBind()

            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            setFocus(txtWorkOrderNo)
            ResetValues()
            ControlVisibility(True, True)
            SetSession()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            If (chkAssembly.Checked = False And chkComponent.Checked = False) Then
                MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select either Assembly or Component", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            SetReport(False)
        End If
    End Sub
    Private Sub btnExportToExcel_Click(sender As Object, e As System.EventArgs) Handles btnExportToExcel.Click
        If IsValid Then
            If (chkAssembly.Checked = False And chkComponent.Checked = False) Then
                MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select either Assembly or Component", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            SetReport(True)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub chkAssembly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAssembly.CheckedChanged
        ControlVisibility(True, False)
    End Sub
    Private Sub chkComponent_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkComponent.CheckedChanged
        ControlVisibility(False, True)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindNow.Click
        chkFindNow = True
        Session("chkFindNow") = chkFindNow
        dgModel.PageIndex = 0
        ListModel = IIf(txtModelNo.Text <> "", Trim(txtModelNo.Text), "")
        ListSerialNo = IIf(txtSerialNo.Text <> "", Trim(txtSerialNo.Text), "")
        Session("ListModel") = ListModel
        Session("ListSerialNo") = ListSerialNo
        FindNowModel(ListModel, ListSerialNo)
    End Sub
    Private Sub dgModel_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgModel.PageIndexChanging
        dgModel.PageIndex = e.NewPageIndex
        Session("mAssemblyList") = mAssemblyList
        dgModel.DataSource = mAssemblyList
        dgModel.DataBind()
    End Sub
    Private Sub btnFindModel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindModel.Click
        chkFindModel = True
        Session("chkFindModel") = chkFindModel

        pnlEModel.Visible = True
        dgEModel.PageIndex = 0
        ListAModelNo = IIf(txtAModelNo.Text <> "", Trim(txtAModelNo.Text), "")
        ListASerialNo = IIf(txtASerialNo.Text <> "", Trim(txtASerialNo.Text), "")
        Session("ListAModelNo") = ListAModelNo
        Session("ListASerialNo") = ListASerialNo
        FindNowEModel(ListAModelNo, ListASerialNo)
    End Sub
    Private Sub btnFindPart_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindPart.Click
        dgPart.PageIndex = 0
        ListPartNo = IIf(txtCPartNo.Text <> "", Trim(txtCPartNo.Text), "")
        ListCompSerialNo = IIf(txtCSerialNo.Text <> "", Trim(txtCSerialNo.Text), "")
        ListEndDate = IIf(txtToDate.Text <> "", (txtToDate.Text), "")
        Session("ListPartNo") = ListPartNo
        Session("ListCompSerialNo") = ListCompSerialNo
        Session("ListEndDate") = ListEndDate
        FindNowPart(ListPartNo, ListCompSerialNo, ListEndDate)
    End Sub
    '---------------------------------------------------
    Private Sub dgModel_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgModel.Sorting
        mAssemblyList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAssemblyList") = mAssemblyList
        dgModel.DataSource = mAssemblyList
        dgModel.DataBind()
    End Sub
    Private Sub dgModel_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgModel.RowCommand
        Select Case e.CommandName
            Case "Select"
                Dim Index As Int16 = CInt(e.CommandArgument) + dgModel.PageIndex * dgModel.PageSize

                chkFindNow = True
                Session("chkFindNow") = chkFindNow
                ListModel = mAssemblyList(Index).ModelName
                ListSerialNo = mAssemblyList(Index).SerialNo
                ListID = mAssemblyList(Index).ID
                txtModelNo.Text = ListModel
                txtSerialNo.Text = ListSerialNo
                Session("ListModel") = ListModel
                Session("ListSerialNo") = ListSerialNo
                Session("ListID") = ListID

                dgModel.DataSource = mAssemblyList
                dgModel.DataBind()
        End Select
    End Sub
    Private Sub dgPart_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPart.PageIndexChanging
        dgPart.PageIndex = e.NewPageIndex
        dgPart.DataSource = mCompList
        Session("mCompList") = mCompList
        dgPart.DataBind()
        ControlVisibility(False, True)
    End Sub
    Private Sub dgPart_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPart.RowCommand
        Select Case e.CommandName
            Case "Select"
                Dim Index As Int16 = CInt(e.CommandArgument) + dgPart.PageIndex * dgPart.PageSize

                ListPartNo = mCompList(Index).PartName
                ListCompSerialNo = mCompList(Index).SerialNo
                txtCPartNo.Text = ListPartNo
                txtCSerialNo.Text = ListCompSerialNo
                Session("ListPartNo") = ListPartNo
                Session("ListCompSerialNo") = ListCompSerialNo

                dgPart.DataSource = mCompList
                dgPart.DataBind()
        End Select
    End Sub
    Private Sub dgPart_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPart.Sorting
        mCompList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCompList") = mCompList
        dgPart.DataSource = mCompList
        dgPart.DataBind()
        ControlVisibility(False, True)
    End Sub
    Private Sub dgEModel_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEModel.RowCommand
        Select Case e.CommandName
            Case "Select"
                Dim Index As Int16 = CInt(e.CommandArgument) + dgEModel.PageIndex * dgEModel.PageSize

                chkFindModel = True
                Session("chkFindModel") = chkFindModel
                ListAModelNo = mEAssemblyList(Index).ModelName
                ListASerialNo = mEAssemblyList(Index).SerialNo
                txtAModelNo.Text = ListAModelNo
                txtASerialNo.Text = ListASerialNo
                Session("ListAModelNo") = ListAModelNo
                Session("ListASerialNo") = ListASerialNo

                dgEModel.DataSource = mEAssemblyList
                dgEModel.DataBind()
        End Select
    End Sub
    Private Sub dgEModel_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgEModel.PageIndexChanging
        dgEModel.PageIndex = e.NewPageIndex
        dgEModel.DataSource = mEAssemblyList
        Session("mEAssemblyList") = mEAssemblyList
        dgEModel.DataBind()
        ControlVisibility(True, False)
    End Sub
    Private Sub dgEModel_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgEModel.Sorting
        mEAssemblyList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mEAssemblyList") = mEAssemblyList
        dgEModel.DataSource = mEAssemblyList
        dgEModel.DataBind()
        ControlVisibility(True, False)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    
    
End Class