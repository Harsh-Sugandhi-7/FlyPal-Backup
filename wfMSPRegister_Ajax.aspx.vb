Public Class wfMSPRegister_Ajax
    Inherits Page

#Region " Variable Declaration "

    Dim mExpiryDateSearchingCriteria As String = String.Empty

#End Region

#Region " Helper Methods "

    Private Sub RemoveSession()

    End Sub

    Private Overloads Sub setFocus(cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub

    Private Sub ControlVisibility()
    End Sub

    Private Sub Display()
    End Sub

    Private Sub SetValues()

        'mExpiryDateSearchingCriteria = lblDateRange.Text.Trim + ", " + lblRangeDisp.Text + ", " + lblStoreName.Text.Trim + ", " + lblCategoryName.Text + ", " + lblNomenclatureName.Text + ", " + lblPartNo.Text.Trim + ", " + lblDesc.Text.Trim
    End Sub

    Private Sub SetReport(Optional IsExcel As Boolean = False)

        Dim da As New ObjectAdapter
        Dim myReport As Engine.ReportClass
        Dim ds As New dsMSPInOrderWorkOrder
        Dim mCompanyDetail As New CompanyDetail
        Dim rpt As New Object

        Try

            SetValues()

            If cmbMSPIn.SelectedValue = "0" Then 'Order

                myReport = New crptMSPInOrder

                rpt = MSPInOrder.GetMSPInOrder(FromDate:=txtFromDate.Text,
                                               ToDate:=txtToDate.Text,
                                               MSPText:=IIf(cmbMSPText.SelectedIndex = 0, "", cmbMSPText.SelectedItem.Text),
                                               MSPNo:=Val(txtNo.Text),
                                               AssemblyID:=IIf(cmbAssemblyList.SelectedIndex = 0,
                                                               "{00000000-0000-0000-0000-000000000000}",
                                                               cmbAssemblyList.SelectedValue))

            ElseIf cmbMSPIn.SelectedValue = "1" Then 'Work Order

                myReport = New crptMSPInWorkOrder

                rpt = MSPInWorkOrder.GetMSPInWorkOrder(FromDate:=txtFromDate.Text,
                                                       ToDate:=txtToDate.Text,
                                                       MSPText:=IIf(cmbMSPText.SelectedIndex = 0, "", cmbMSPText.SelectedItem.Text),
                                                       MSPNo:=Val(txtNo.Text),
                                                       AssemblyID:=IIf(cmbAssemblyList.SelectedIndex = 0,
                                                                       "{00000000-0000-0000-0000-000000000000}",
                                                                       cmbAssemblyList.SelectedValue))

            ElseIf cmbMSPIn.SelectedValue = "2" Then 'Line maintenance Order

                myReport = New crptMSPInLineMaintenanceOrder

                rpt = MSPInLineMaintenanceOrder.GetMSPInLineMaintenance(FromDate:=txtFromDate.Text,
                                                                        ToDate:=txtToDate.Text,
                                                                        MSPText:=IIf(cmbMSPText.SelectedIndex = 0, "", cmbMSPText.SelectedItem.Text),
                                                                        MSPNo:=Val(txtNo.Text),
                                                                        AssemblyID:=IIf(cmbAssemblyList.SelectedIndex = 0,
                                                                                        "{00000000-0000-0000-0000-000000000000}",
                                                                                        cmbAssemblyList.SelectedValue))

            End If

            Dim mReport As New ReportData(mCompanyDetail.CompanyName,
                                          mCompanyDetail.Address,
                                          mCompanyDetail.Tel1,
                                          mCompanyDetail.Tel2,
                                          mCompanyDetail.Fax,
                                          mCompanyDetail.Email,
                                          WebSite:="",
                                          ReportName:="Purchase Consumption",
                                          SearchStr1:=New SmartDate(txtFromDate.Text).FormattedText,
                                          SearchStr2:=New SmartDate(txtToDate.Text).FormattedText,
                                          SearchStr3:=IIf(cmbAssemblyList.SelectedIndex = 0,
                                                          "",
                                                          cmbAssemblyList.SelectedItem.Text),
                                          SearchStr4:=IIf(cmbMSPText.SelectedIndex = 0,
                                                          "",
                                                          cmbMSPText.SelectedItem.Text + IIf(txtNo.Text = "",
                                                                                             "",
                                                                                             "-" + txtNo.Text)),
                                          SearchStr5:="",
                                          ProductVersion:=AppSettings("Product Version"),
                                          SINote:=AppSettings("SINote"),
                                          SearchStr6:="",
                                          SearchStr7:="",
                                          SearchStr8:="",
                                          SearchStr9:=AppSettings("Logo"),
                                          SearchStr10:=AppSettings("ClientCode"),
                                          SearchStr11:="",
                                          SearchStr12:="",
                                          SearchStr13:="",
                                          SearchStr14:="",
                                          SearchStr15:="",
                                          SearchStr16:="")

            If rpt.Count <= 0 Then

                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound,
                                MSGBox.Message_text.NoRecordFound,
                                "There is no record for this search criteria",
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Sub

            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1550)
            End If

            If IsExcel = False Then

                ds.Clear()
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                da.Fill(ds, rpt)
                da.Fill(ds, mrptImage)
                da.Fill(ds, mReport)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport

                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "openTranDetail",
                                                    Str,
                                                    True)
            Else

                ds.Clear()
                da.Fill(ds, mReport)
                da.Fill(ds, "MSPDue", rpt)

                Dim columnToRemove2 As String() = {"SearchStr4", "ReportName", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "ShortName", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
                For i As Integer = 0 To columnToRemove2.Length - 1
                    If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                        ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                    End If
                Next

                Dim columnToRemove As String() = {"ID", "MSPID", "AssemblyID", "VendorID", "Text", "No", "Date", "FromDate", "ToDate"}

                For i As Integer = 0 To columnToRemove.Length - 1
                    If ds.Tables("MSPDue").Columns.Contains(columnToRemove(i)) Then
                        ds.Tables("MSPDue").Columns.Remove(columnToRemove(i))
                    End If
                Next

                If ds.Tables("MSPDue").Columns.Contains("DateFormatted") Then
                    ds.Tables("MSPDue").Columns("DateFormatted").ColumnName = "Date"
                End If
                If ds.Tables("MSPDue").Columns.Contains("VendorName") Then
                    ds.Tables("MSPDue").Columns("VendorName").ColumnName = "Vendor"
                End If
                If ds.Tables("MSPDue").Columns.Contains("ContractNo") Then
                    ds.Tables("MSPDue").Columns("ContractNo").ColumnName = "Contract No."
                End If
                If ds.Tables("MSPDue").Columns.Contains("PlanName") Then
                    ds.Tables("MSPDue").Columns("PlanName").ColumnName = "Plan Name"
                End If
                If ds.Tables("MSPDue").Columns.Contains("FromDateFormatted") Then
                    ds.Tables("MSPDue").Columns("FromDateFormatted").ColumnName = "From Date"
                End If
                If ds.Tables("MSPDue").Columns.Contains("ToDateFormatted") Then
                    ds.Tables("MSPDue").Columns("ToDateFormatted").ColumnName = "To Date"
                End If
                If ds.Tables("MSPDue").Columns.Contains("RemainingDays") Then
                    ds.Tables("MSPDue").Columns("RemainingDays").ColumnName = "Remaining Days"
                End If
                If ds.Tables("MSPDue").Columns.Contains("AssemblyName") Then
                    ds.Tables("MSPDue").Columns("AssemblyName").ColumnName = "Applicable To"
                End If

                ds.Tables("MSPDue").Columns("Date").SetOrdinal(0)
                ds.Tables("MSPDue").Columns("MSPNo").SetOrdinal(1)
                ds.Tables("MSPDue").Columns("Contract No.").SetOrdinal(2)
                ds.Tables("MSPDue").Columns("Plan Name").SetOrdinal(3)
                ds.Tables("MSPDue").Columns("Vendor").SetOrdinal(4)
                ds.Tables("MSPDue").Columns("From Date").SetOrdinal(5)
                ds.Tables("MSPDue").Columns("To Date").SetOrdinal(6)
                ds.Tables("MSPDue").Columns("Remaining Days").SetOrdinal(7)

                If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                    ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "As On Date"
                End If
                If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                    ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Applicable To"
                End If
                If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                    ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "MSP No."
                End If

                Dim dsNew As New DataSet

                dsNew.Clear()
                dsNew.Merge(ds.Tables("ReportData"))
                dsNew.Tables("ReportData").TableName = "Searching Criteria"
                dsNew.Merge(ds.Tables("MSPDue"))
                dsNew.Tables("MSPDue").TableName = "MSP Due"
				Session("ExcelFileName") = "MSP Register"
				Session("dsNew") = dsNew

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "openFile",
                                                    "openFile();",
                                                    True)
                MarkLog(Action.Print,
                        "MSPDue",
                        "Export To Excel ",
                        ErrorType.NoError,
                        Guid.Empty,
                        EventLogID)

            End If

            MarkLog(Action.Print,
                    "MSPDue",
                    "",
                    ErrorType.NoError,
                    Guid.Empty,
                    EventLogID)

        Catch ex As Exception
            Throw ex
        End Try

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

        cmbMSPText.DataSource = DistinctTextListForMSP.GetDistinctTextList("32", ,
                                                                           True,
                                                                           "(ALL)")
        cmbAssemblyList.DataSource = AssemblyList.GetAssemblyListForComboBox(0,
                                                                             Guid.Empty.ToString,
                                                                             Today.Date.ToString,
                                                                             "(ALL)",
                                                                             IsInstalled:=True,
                                                                             IsForSpareAssembly:=False)

        DataBind()

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then

            RemoveSession()
            txtFromDate.Text = Today.Date.AddMonths(-1).ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()

        End If

        MessageBoxResult()

    End Sub

    Private Sub btnDisplay_Click(sender As System.Object, e As System.EventArgs) Handles btnDisplay.Click
        SetReport(IsExcel:=False)
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

#End Region

End Class