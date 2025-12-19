'***********************************
' Created by:	Harsh Sugandhi
' Created On:	4th April 2025
' Created For:	FLYPAL-2297 Deccan => GST Value Report.
'***********************************

<CLSCompliant(False)>
Public Class GSTValueListReport
    Inherits Page


#Region " Variable Declaration "

    Public _GSTChargeList As GSTChargeList
    Public GSTChargeName As String = ""

    Dim ModuleName As String = "GSTValueListReport"

#End Region

#Region " Helper Method(s) "

    Private Overloads Sub SetFocus(control As WebControl)

        Try

            If control.Enabled = False Or control.Visible = False Then Exit Sub

            Dim script As String
            script = "<script type='text/javascript'> 
                    document.getElementById('" + control.ClientID + "').focus();</script>"

            ClientScript.RegisterStartupScript([GetType],
                                               "FocusScript",
                                               script)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub PreserveStateOfFavIcon()

        Try

            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, ModuleName) Then

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "MarkAsFavorite",
                                                    "MarkAsFavorite();",
                                                    True)

            Else

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "RemoveFromFavorite",
                                                    "RemoveFromFavorite();",
                                                    True)

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SetValues()

        GSTChargeName = IIf(hdnBtnGSTChargeList.Value = String.Empty,
                            String.Empty,
                            hdnBtnGSTChargeList.Value)

    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()

        Try

            _GSTChargeList = GSTChargeList.GetGSTChargeList()
            chkGSTChargeList.DataSource = _GSTChargeList

            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))

            DataBind()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Events "

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        EventLogID = CType(Session("EventLogID"), Guid)
        Try

            If Not IsPostBack Then

                SetFocus(txtFromDate)

                DataFieldBind()
                PreserveStateOfFavIcon()
                Session("MiddleFrame") = $"wfGSTValueList.aspx?"

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnDisplayReport.Click, btnExport.Click
        Try

            If Not IsValid Then

                upnlValidationErrors.Update()
                Exit Sub

            End If

            If sender.ID = "btnExport" Then
                SetReport(IsExcel:=True)
            Else
                SetReport()
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SetReport(Optional IsExcel As Boolean = False)

        Dim da As New ObjectAdapter
        Dim myReport As Engine.ReportClass = New crptGSTValueList
        Dim _CompanyDetail As New CompanyDetail
        Dim dataSet As New dsGSTValueList
        Dim _GSTValueList As GSTValueList

        Try

            SetValues()

            _GSTValueList = GSTValueList.GetGSTValueList(FromDate:=txtFromDate.Text,
                                                         ToDate:=txtToDate.Text,
                                                         ChargeID:=GSTChargeName)

            If _GSTValueList.Count <= 0 Then

                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound,
                                MSGBox.Message_text.NoRecordFound,
                                "There is no record for this search criteria",
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Sub

            Else

                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1366)

                MarkLog(Action.Print,
                        "GST Value List Report.",
                        "",
                        ErrorType.NoError,
                        Guid.Empty,
                        EventLogID)

            End If

            _CompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

            Dim Report As New ReportData(_CompanyDetail.CompanyName,
                                         _CompanyDetail.Address,
                                         _CompanyDetail.Tel1,
                                         _CompanyDetail.Tel2,
                                         _CompanyDetail.Fax,
                                         _CompanyDetail.Email,
                                         WebSite:="",
                                         ReportName:="",
                                         SearchStr1:=New SmartDate(txtFromDate.Text).FormattedText,
                                         SearchStr2:=New SmartDate(txtToDate.Text).FormattedText,
                                         SearchStr3:=GSTChargeName,
                                         SearchStr4:="",
                                         SearchStr5:="",
                                         ProductVersion:="",
                                         SINote:="",
                                         SearchStr6:="",
                                         SearchStr7:="",
                                         SearchStr8:="",
                                         SearchStr9:="",
                                         SearchStr10:=AppSettings("Logo"),
                                         SearchStr11:=AppSettings("MROISONo"),
                                         SearchStr12:="",
                                         SearchStr13:="",
                                         SearchStr14:="")

            If IsExcel = False Then
                dataSet.Clear()
                Dim companyLogo As rptImage = rptImage.GetImage(dataSet)
                da.Fill(dataSet, companyLogo)
                da.Fill(dataSet, _GSTValueList)
                da.Fill(dataSet, Report)
                myReport.SetDataSource(dataSet)
                Session("CrystalReport") = myReport

                Dim Str As String
                Str = "openCrystalReport();"

                ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "Display Report",
                                                Str,
                                                True)
            ElseIf IsExcel = True Then
                dataSet.Clear()
                da.Fill(dataSet, _GSTValueList)
                da.Fill(dataSet, Report)

                Dim columnToRemove As String() = {"InvoiceID", "InvoiceDate"}

                For i As Integer = 0 To columnToRemove.Length - 1
                    If dataSet.Tables("GSTValueList").Columns.Contains(columnToRemove(i)) Then
                        dataSet.Tables("GSTValueList").Columns.Remove(columnToRemove(i))
                    End If
                Next

                Dim columnToRemove2 As String() = {"SearchStr4", "ReportName", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email",
                                                   "WebSite", "ProductVersion", "ShortName", "SINote", "CurrencyName", "CurrencySymbol",
                                                   "ApprovalNo", "SearchStr5",
                                                   "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12",
                                                   "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18",
                                                   "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24",
                                                   "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30",
                                                   "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36",
                                                   "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42",
                                                   "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48",
                                                   "SearchStr49", "SearchStr50", "SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54",
                                                   "SearchStr55", "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",
                                                   "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65", "SearchStr66",
                                                   "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70", "SearchStr71", "SearchStr72",
                                                   "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78",
                                                   "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84",
                                                   "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90",
                                                   "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95", "SearchStr96",
                                                   "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
                For i As Integer = 0 To columnToRemove2.Length - 1
                    If dataSet.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                        dataSet.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                    End If
                Next

                If dataSet.Tables("GSTValueList").Columns.Contains("InvoiceNumber") Then
                    dataSet.Tables("GSTValueList").Columns("InvoiceNumber").ColumnName = "Invoice No."
                End If
                If dataSet.Tables("GSTValueList").Columns.Contains("InvoiceDateFormatted") Then
                    dataSet.Tables("GSTValueList").Columns("InvoiceDateFormatted").ColumnName = "Invoice Date"
                End If
                If dataSet.Tables("GSTValueList").Columns.Contains("CTotalAmount") Then
                    dataSet.Tables("GSTValueList").Columns("CTotalAmount").ColumnName = "Invoice Amount"
                End If
                If dataSet.Tables("GSTValueList").Columns.Contains("TotalAmount") Then
                    dataSet.Tables("GSTValueList").Columns("TotalAmount").ColumnName = "Base Amount"
                End If
                If dataSet.Tables("GSTValueList").Columns.Contains("ConversionFactor") Then
                    dataSet.Tables("GSTValueList").Columns("ConversionFactor").ColumnName = "Factor"
                End If
                If dataSet.Tables("GSTValueList").Columns.Contains("ChargePercentage") Then
                    dataSet.Tables("GSTValueList").Columns("ChargePercentage").ColumnName = "Percentage"
                End If
                If dataSet.Tables("GSTValueList").Columns.Contains("CChargeAmount") Then
                    dataSet.Tables("GSTValueList").Columns("CChargeAmount").ColumnName = "Invoice Charge Amount"
                End If
                If dataSet.Tables("GSTValueList").Columns.Contains("ChargeAmount") Then
                    dataSet.Tables("GSTValueList").Columns("ChargeAmount").ColumnName = "Base Charge Amount"
                End If

                dataSet.Tables("GSTValueList").Columns("Invoice No.").SetOrdinal(0)
                dataSet.Tables("GSTValueList").Columns("Invoice Date").SetOrdinal(1)
                dataSet.Tables("GSTValueList").Columns("Supplier").SetOrdinal(2)
                dataSet.Tables("GSTValueList").Columns("Currency").SetOrdinal(3)
                dataSet.Tables("GSTValueList").Columns("Factor").SetOrdinal(4)
                dataSet.Tables("GSTValueList").Columns("Charge").SetOrdinal(5)
                dataSet.Tables("GSTValueList").Columns("Percentage").SetOrdinal(6)
                dataSet.Tables("GSTValueList").Columns("Invoice Charge Amount").SetOrdinal(7)
                dataSet.Tables("GSTValueList").Columns("Base Charge Amount").SetOrdinal(8)
                dataSet.Tables("GSTValueList").Columns("Invoice Amount").SetOrdinal(9)
                dataSet.Tables("GSTValueList").Columns("Base Amount").SetOrdinal(10)

                If dataSet.Tables("ReportData").Columns.Contains("SearchStr1") Then
                    dataSet.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
                End If
                If dataSet.Tables("ReportData").Columns.Contains("SearchStr2") Then
                    dataSet.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
                End If
                If dataSet.Tables("ReportData").Columns.Contains("SearchStr3") Then
                    dataSet.Tables("ReportData").Columns("SearchStr3").ColumnName = "Selected Charges"
                End If

                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(dataSet.Tables("ReportData"))
                dsNew.Tables("ReportData").TableName = "Searching Criteria"
                dsNew.Merge(dataSet.Tables("GSTValueList"))
                dsNew.Tables("GSTValueList").TableName = "GST Value"
				Session("ExcelFileName") = "GST Value"
				Session("dsNew") = dsNew
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
                MarkLog(Util.Action.Print, "GSTValueList", "Export To Excel ", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")

    End Sub

    Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click
        MarkFavourite(HttpContext.Current.User.Identity.Name, ModuleName)

    End Sub

    Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click
        RemoveFavourite(HttpContext.Current.User.Identity.Name, ModuleName)

    End Sub

#End Region

End Class