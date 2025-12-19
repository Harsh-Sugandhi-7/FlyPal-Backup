Imports System.Linq

Public Class wfTransTextSeries_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "

    Dim str As String
    Dim mTransName As String
    Dim mTransTypeID As Integer
    Dim mTransDate As String

    Dim mTransactionList As TransactionList

    Dim mTransTextSeries As TransTextSeries

    Dim mID As Guid

    Dim mBaseTransTypeID As Integer
    Dim mBaseTransTypeList As BaseTransTypeList

    Dim mTransTextSeriesCollection As TransTextSeriesCollection

    Dim EventLogDetail As String

#End Region

#Region " Helper Methods "

    Private Sub GetSession()
        str = Session("BackPagestr_ForTransSeries")
        mTransName = Session("TransName_ForTransSeries")
        mTransTypeID = Session("TransTypeID_ForTransSeries")
        mTransDate = Session("TransDate_ForTransSeries")

        mTransTextSeries = Session("TransTextSeries")
        mID = Session("TransTextSeries_ID")
        mBaseTransTypeID = Session("BaseTransTypeID_ForTransSeries")

        mTransTextSeriesCollection = Session("TransTextSeriesCollection")
    End Sub

    Private Sub RemoveSession()
        Session.Remove("TransName_ForTransSeries")
        Session.Remove("TransTypeID_ForTransSeries")
        Session.Remove("TransDate_ForTransSeries")
        Session.Remove("TransTextSeries")
        Session.Remove("TransTextSeries_ID")
        Session.Remove("TransTextSeriesCollection")
        Session.Remove("BaseTransTypeID_ForTransSeries")
    End Sub

    Private Sub SetObject()

        mTransTextSeries = Session("TransTextSeries")

        If Not mTransTextSeries Is Nothing Then

            mTransTextSeries.IsAutoRenew = chkAutoRenew.Checked

            Dim txtTransTextPrefixValue, txtStartingTransNoValue As TextBox
            Dim cmbSuffixValue As DropDownList

            For i As Integer = 0 To dgTransSeriesDetails.Rows.Count - 1

                txtTransTextPrefixValue = CType(Me.dgTransSeriesDetails.Rows(i).FindControl("txtPrefix"), TextBox)
                txtTransTextPrefixValue.Width = IIf(mTransTextSeries.DatePeriodFormatID = 3, "300", "200").ToString

                mTransTextSeries.TransTextSeriesDetails(i).Prefix = Trim(txtTransTextPrefixValue.Text)

                cmbSuffixValue = CType(Me.dgTransSeriesDetails.Rows(i).FindControl("cmbSuffixList"), DropDownList)
                If cmbSuffixValue.Items.Count > 0 Then
                    If mTransTextSeries.DatePeriodFormatID <> 3 Then
                        mTransTextSeries.TransTextSeriesDetails(i).Suffix = cmbSuffixValue.SelectedItem.Text
                    End If
                End If

                txtStartingTransNoValue = CType(Me.dgTransSeriesDetails.Rows(i).FindControl("txtStartingTransNo"), TextBox)
                mTransTextSeries.TransTextSeriesDetails(i).StartingTransNo = Val(txtStartingTransNoValue.Text)

            Next

            Session("TransTextSeries") = mTransTextSeries

        End If

    End Sub

    Private Function Save() As Boolean

        Dim isSaveDone As Boolean = False

        Try

            If CheckIsDateRangeValid() = False Then
                MSGBoxCtrl.Show("Date Range Not Valid", "Date difference can not be greater than 1 Year", "", MsgBoxStyle.OkOnly, "")
                Return False  'Added by utkarsh on 13-Dec-2013 : validation of date range
            End If

            'Added by utkarsh on 16-Dec-2013 : validation of date range
            If CheckFromDateToDate() = False Then
                cvBaseType.IsValid = False
                cvBaseType.ErrorMessage = "From Date should be greater than To Date."
                Return False
            End If
            'End

            If mTransTextSeries.IsValid Then
                Try
                    'Check if for same transTypeID and FromDate - ToDate entry present
                    If mTransTextSeriesCollection.Contains(mBaseTransTypeID, mTransTextSeries.FromDate.ToString, mTransTextSeries.ToDate.ToString) AndAlso (mTransTextSeries.IsNew) Then
                        MSGBoxCtrl.Show("Overlapping Entry", "Transaction Series already present for specified Date range.", "", MsgBoxStyle.OkOnly, "")
                    Else
                        If Not mTransDate Is Nothing Then    'If this page is called from Transaction page
                            If ((mTransTextSeries.FromDate <= CDate(mTransDate)) And (mTransTextSeries.ToDate >= CDate(mTransDate))) Then     'Passed TrasnDate shld fall btw FromDate - Todate
                                mTransTextSeries.ApplyEdit()
                                mTransTextSeries = CType(mTransTextSeries.Save(), TransTextSeries)
                                EventLogDetail = "Transaction : " & mTransTextSeries.BaseTransTypeName & ", Date Period : " & mTransTextSeries.DatePeriodFormat & ", Date Range : " & mTransTextSeries.FromDateFormatted & " To " & mTransTextSeries.ToDateFormatted & ", Auto Renew : " & mTransTextSeries.IsAutoRenew.ToString
                                MarkLog(Util.Action.Save, "TransactionTextSeries", EventLogDetail, Util.ErrorType.NoError, mTransTextSeries.ID, EventLogID)
                                isSaveDone = True
                                DataBindTransSeriesGrid()
                                upnlTransSeriesGrid.Update()
                                Session("TransTextSeries") = mTransTextSeries
                            Else
                                isSaveDone = False
                                MSGBoxCtrl.Show("Transaction Date Restriction", "Transaction Date should fall between From Date and To Date ", "", MsgBoxStyle.OkOnly, "")
                            End If
                        Else
                            mTransTextSeries.ApplyEdit()
                            mTransTextSeries = CType(mTransTextSeries.Save(), TransTextSeries)
                            EventLogDetail = "Transaction : " & mTransTextSeries.BaseTransTypeName & ", Date Period : " & mTransTextSeries.DatePeriodFormat & ", Date Range : " & mTransTextSeries.FromDateFormatted & " To " & mTransTextSeries.ToDateFormatted & ", Auto Renew : " & mTransTextSeries.IsAutoRenew.ToString
                            MarkLog(Util.Action.Save, "TransactionTextSeries", EventLogDetail, Util.ErrorType.NoError, mTransTextSeries.ID, EventLogID)
                            isSaveDone = True
                            DataBindTransSeriesGrid()
                            upnlTransSeriesGrid.Update()
                        End If

                    End If

                Catch ex As SqlClient.SqlException
                    isSaveDone = False
                End Try

            End If
        Catch ex As Exception
            isSaveDone = False
        End Try

        Return isSaveDone

    End Function

    Private Sub EditRecord()

        Session.Remove("TransName_ForTransSeries")
        Session.Remove("TransTypeID_ForTransSeries")
        Session.Remove("TransDate_ForTransSeries")
        Session("BaseTransTypeID_ForTransSeries") = mTransTextSeries.BaseTransTypeID
        BindParentCombo()
        EnableDisbaleControl_New()
        cmbBaseTypeList.SelectedValue = mTransTextSeries.BaseTransTypeID
        SetDateControlFormat(mTransTextSeries.DatePeriodFormatID)
        txtFromDate.Text = Format(CDate(mTransTextSeries.FromDateFormatted), AppSettings("DateFormat"))     'mTransTextSeries.FromDateFormatted
        txtToDate.Text = Format(CDate(mTransTextSeries.ToDateFormatted), AppSettings("DateFormat"))                          'mTransTextSeries.ToDateFormatted
        chkAutoRenew.Checked = mTransTextSeries.IsAutoRenew
        EventLogDetail = "Transaction : " & mTransTextSeries.BaseTransTypeName & ", Date Period : " & mTransTextSeries.DatePeriodFormat & ", Date Range : " & mTransTextSeries.FromDateFormatted & " To " & mTransTextSeries.ToDateFormatted & ", Auto Renew : " & mTransTextSeries.IsAutoRenew.ToString
        MarkLog(Flypal.Util.Action.Edit, "TransactionTextSeries", EventLogDetail, Flypal.Util.ErrorType.NoError, mTransTextSeries.ID, EventLogID)
        DataBindTransSeriesDetailGrid()
        upnlTransTextSeries.Update()
    End Sub

    Private Sub DeleteRecord(mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        Session("TransTextSeries_ID") = mID
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mID = New Guid(Session("TransTextSeries_ID").ToString)
                            TransTextSeries.DeleteTransTextSeries(mID)
                            Session.Remove("TransTextSeries")
                            EventLogDetail = "Transaction : " & mTransTextSeries.BaseTransTypeName & ", Date Period : " & mTransTextSeries.DatePeriodFormat & ", Date Range : " & mTransTextSeries.FromDateFormatted & " To " & mTransTextSeries.ToDateFormatted & ", Auto Renew : " & mTransTextSeries.IsAutoRenew.ToString
                            MarkLog(Action.Delete, "TransactionTextSeries", EventLogDetail, ErrorType.NoError, mTransTextSeries.ID, EventLogID)
                            NewRecord()
                        Catch ex As SqlException
                            DataBindTransSeriesGrid()
                            upnlTransSeriesGrid.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Session.Remove("TransTextSeries")
                    NewRecord()
                Case MsgBoxResult.Ok
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then
            Session("sender") = ""
        End If
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbBaseTypeList" And cmbBaseTypeList.SelectedIndex = 0 Then
            custValidator.ErrorMessage = "Select Base Type List."
            e.IsValid = False
        Else
            e.IsValid = True
        End If

        upnlErrorList.Update()
    End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)

        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        SetObject()

        Dim str1 As String = ""
        'Log
        If Not mTransTextSeries Is Nothing Then
            If Not mTransTextSeries.IsValid Then
                For i As Integer = 0 To mTransTextSeries.GetBrokenRulesCollection.Count - 1
                    str1 = str1 + mTransTextSeries.GetBrokenRulesCollection(i).Description + "<BR>"
                Next
            End If

            For i As Integer = 0 To mTransTextSeries.TransTextSeriesDetails.Count - 1
                If Not mTransTextSeries.TransTextSeriesDetails(i).IsValid Then
                    For j As Integer = 0 To mTransTextSeries.TransTextSeriesDetails(i).GetBrokenRulesCollection.Count - 1
                        str1 = str1 + mTransTextSeries.TransTextSeriesDetails.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
                    Next
                End If
            Next

            If str1 <> "" Then
                cvBaseType.ErrorMessage = str1
                cvBaseType.IsValid = False
            End If
        End If
        upnlErrorList.Update()
    End Sub
    Private Sub BindParentCombo()
        mBaseTransTypeList = BaseTransTypeList.GetBaseTransTypeList()

        cmbBaseTypeList.DataSource = mBaseTransTypeList
        cmbBaseTypeList.DataBind()
    End Sub
    Private Sub EnableDisbaleControl_New()

        'Enable only if opened from Menu & in case of NEW
        lblTransactionDate.Visible = CInt(Session("OpenFrmLnk")) = 0
        lblTransactionDateValue.Visible = CInt(Session("OpenFrmLnk")) = 0
        cmbBaseTypeList.Enabled = ((CInt(Session("OpenFrmLnk")) = 1) And mTransTextSeries.IsNew = True)
        btnNew.Enabled = CInt(Session("OpenFrmLnk")) = 1
        rdbCalendarYear.Enabled = mTransTextSeries.isEnabled_DatePeriod_Options
        rdbFinancialYear.Enabled = mTransTextSeries.isEnabled_DatePeriod_Options
        rdbCustom.Enabled = mTransTextSeries.isEnabled_DatePeriod_Controls
        txtFromDate.Enabled = mTransTextSeries.isEnabled_DatePeriod_Controls
        txtToDate.Enabled = mTransTextSeries.isEnabled_DatePeriod_Controls
        chkAutoRenew.Enabled = mTransTextSeries.isEnabled_AutoRenew_Options

        If CInt(Session("OpenFrmLnk")) = 1 Then    'Open from Link/Menu
            lblInfoText1.Text = ""
            lblInfoText1.Visible = False
            lblInfoText2.Text = "Enter the following details, save it, and press Close button to close this page."
        Else                                        'Open from Transaction
            lblInfoText1.Visible = True
            lblInfoText1.Text = lblInfoText1.Text.Replace("[TransName]", mTransName).Replace("[TransDate]", mTransDate)
            lblInfoText2.Text = "Enter the following details, save it, and press Continue button to go back to transaction page."
        End If

        upnlTransTextSeries.Update()

    End Sub
    Private Sub SetDateControlFormat(ByVal DatePeriodFormatID As Integer)
        Select Case DatePeriodFormatID
            Case 1      'Financial Year     -> Fix both Date cntrl values
                rdbFinancialYear.Checked = True
            Case 2      'Calendar Year      -> Fix both Date cntrl values
                rdbCalendarYear.Checked = True
            Case 3      'Custom             -> Open both Date cntrl
                rdbCustom.Checked = True
        End Select
    End Sub
    Private Sub SetDateControlDefaultValues(ByVal DatePeriodFormatID As Integer)

        Dim Year As Integer
        If CInt(Session("OpenFrmLnk")) = 0 Then
            Year = CDate(mTransDate).Year
        Else
            Year = Date.Today.Year
        End If

        Select Case DatePeriodFormatID
            Case 1      'Financial Year     -> Fix both Date cntrl values
                txtFromDate.Text = DateSerial(Year, 4, 1)
                txtToDate.Text = DateSerial((Year + 1), 3, 31)
            Case 2      'Calendar Year      -> Fix both Date cntrl values
                txtFromDate.Text = DateSerial(Year, 1, 1)
                txtToDate.Text = DateSerial(Year, 12, 31)
            Case 3      'Custom             -> Open both Date cntrl
                txtFromDate.Text = DateSerial(Year, 1, 1)
                txtToDate.Text = DateSerial(Year, 12, 31)
        End Select

        txtFromDate.Text = Format(CDate(txtFromDate.Text), AppSettings("DateFormat"))
        txtToDate.Text = Format(CDate(txtToDate.Text), AppSettings("DateFormat"))


        chkAutoRenew.Enabled = DatePeriodFormatID <> 3    'Not enabled for Custom format

        mTransTextSeries.FromDate = txtFromDate.Text
        mTransTextSeries.ToDate = txtToDate.Text

    End Sub
    Private Sub DataBindTransSeriesDetailGrid()
        dgTransSeriesDetails.DataSource = mTransTextSeries.TransTextSeriesDetails
        dgTransSeriesDetails.DataBind()
        FillSuffix()
    End Sub
    Private Sub FillSuffix()

        Dim txtValue As TextBox
        Dim txtStartingTransNoValue As TextBox
        Dim cmbValue As DropDownList

        Dim dt As DataTable = GetSuffixList()

        For i As Integer = 0 To dgTransSeriesDetails.Rows.Count - 1

            txtValue = CType(Me.dgTransSeriesDetails.Rows(i).FindControl("txtPrefix"), TextBox)
            txtValue.Width = IIf(mTransTextSeries.DatePeriodFormatID = 3 Or mTransTextSeries.IsNew = False, "300", "200").ToString
            txtValue.Enabled = IIf((mTransTextSeries.IsNew = True), True, False)

            cmbValue = CType(Me.dgTransSeriesDetails.Rows(i).FindControl("cmbSuffixList"), DropDownList)
            cmbValue.Visible = (mTransTextSeries.DatePeriodFormatID <> 3) And (mTransTextSeries.IsNew = True)
            If cmbValue.Visible Then cmbValue.DataSource = dt : cmbValue.DataBind()

            txtStartingTransNoValue = CType(Me.dgTransSeriesDetails.Rows(i).FindControl("txtStartingTransNo"), TextBox)
            txtStartingTransNoValue.Enabled = IIf((mTransTextSeries.IsNew = True), True, False)

        Next
    End Sub
    Private Function GetSuffixList() As DataTable

        Dim dt As New DataTable
        dt.Columns.Add("Suffix", GetType(String))

        Dim fromyear As Integer = CInt(New SmartDate(mTransTextSeries.FromDate.ToString).Date.ToString("yyyy"))
        Dim toyear As Integer = CInt(New SmartDate(mTransTextSeries.ToDate.ToString).Date.ToString("yyyy"))

        If fromyear = toyear Then
            dt.Rows.Add(fromyear.ToString)                                                                  '2012
            dt.Rows.Add(fromyear.ToString.Substring(2, 2))                                                  '12
        Else
            dt.Rows.Add(fromyear.ToString + "-" + toyear.ToString)                                          '2012-2013
            dt.Rows.Add(fromyear.ToString + "-" + toyear.ToString.Substring(2, 2))                          '2012-13
            dt.Rows.Add(fromyear.ToString.Substring(2, 2) + "-" + toyear.ToString)                          '12-2013
            dt.Rows.Add(fromyear.ToString.Substring(2, 2) + "-" + toyear.ToString.Substring(2, 2))          '12-13
        End If

        Return dt
    End Function
    Private Sub DataBindTransSeriesGrid()
        mTransTextSeriesCollection = TransTextSeriesCollection.GetTransTextSeriesCollection(mTransTextSeries.BaseTransTypeID)
        Session("TransTextSeriesCollection") = mTransTextSeriesCollection
        dgTransactionSeriesList.DataSource = mTransTextSeriesCollection
        dgTransactionSeriesList.DataBind()
        dgTransactionSeriesList.Columns(7).Visible = CInt(Session("OpenFrmLnk")) = 1 'Available only if opened Menu Link
        dgTransactionSeriesList.Columns(8).Visible = CInt(Session("OpenFrmLnk")) = 1 'Available only if opened Menu Link
        upnlTransSeriesGrid.Update()
    End Sub
    Private Sub OnDateFormatOptionChanged(ByVal DatePeriodFormatID As Integer)
        mTransTextSeries.DatePeriodFormatID = DatePeriodFormatID
        SetDateControlDefaultValues(mTransTextSeries.DatePeriodFormatID)

        Session("TransTextSeries") = mTransTextSeries
    End Sub
    Private Sub [Continue]()

        Session.Remove("TransTextSeries_ID")
        Session.Remove("TransTextSeriesCollection")
        Session.Remove("TransTextSeries")
        Session.Remove("TransTypeID_ForTransSeries")
        Session.Remove("TransName_ForTransSeries")
        Session.Remove("TransDate_ForTransSeries")
        Session.Remove("BaseTransTypeID_ForTransSeries")


        'Get Text for Order from Transaction Series
        Session("TransText_ForTransSeries") = mTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mTransTypeID).TransText
        Session("TransNo_ForTransSeries") = mTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mTransTypeID).StartingTransNo

        RemoveSession()

        ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
    End Sub
    Private Sub [Close]()
        RemoveSession()

        If CInt(Session("OpenFrmLnk")) = 1 Then
            Session.Remove("OpenFrmLnk")
            Session.Remove("BackPagestr_ForTransSeries")
            Session.Remove("MiddleFrame")
            Response.Redirect("Dashboard.aspx")
        Else
            Session.Remove("OpenFrmLnk")
            Session.Remove("BackPagestr_ForTransSeries")
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        End If


    End Sub
    Private Function CheckFromDateToDate() As Boolean
        If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
            Dim mFromDate, mToDate As Date

            mFromDate = txtFromDate.Text.Trim
            mToDate = txtToDate.Text.Trim

            Dim Diff = DateDiff(DateInterval.Day, mFromDate, mToDate)

            If Diff < 0 Then
                Return False
            Else
                Return True
            End If

        Else
            Return False
        End If
    End Function
#End Region

#Region " Events "

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            EventLogID = CType(Session("EventLogID"), Guid)
            GetSession()

            If Not IsPostBack Then

                If Not Request.QueryString("OpenFrmLnk") Is Nothing Then
                    Session("OpenFrmLnk") = Request.QueryString("OpenFrmLnk")
                End If

                If mTransTextSeries Is Nothing Then

                    If CInt(Session("OpenFrmLnk")) = 0 Then     'Open from Transaction
                        mBaseTransTypeID = BaseTransTypeList.GetBaseTransTypeForTransTypeID(mTransTypeID)(0).ID
                        Session("BaseTransTypeID_ForTransSeries") = mBaseTransTypeID
                        mTransTextSeries = TransTextSeries.NewTransTextSeries(mBaseTransTypeID)
                        lblTransactionDateValue.Text = mTransDate
                    ElseIf CInt(Session("OpenFrmLnk")) = 1 Then 'Open from Menu
                        mTransTextSeries = TransTextSeries.NewTransTextSeries(0)
                        Session.Remove("BackPagestr_ForTransSeries")    'Explicitly set nothing
                        str = Nothing
                        Session("MiddleFrame") = "wfTransTextSeries_Ajax.aspx?"
                    End If
                    Session("TransTextSeries") = mTransTextSeries

                End If

                cmbBaseTypeList.SelectedValue = mTransTextSeries.BaseTransTypeID
                rdbFinancialYear.Checked = True
                rdbCalendarYear.Checked = False
                rdbCustom.Checked = False
                BindParentCombo()
                EnableDisbaleControl_New()
                SetDateControlFormat(mTransTextSeries.DatePeriodFormatID)
                SetDateControlDefaultValues(mTransTextSeries.DatePeriodFormatID)
                DataBindTransSeriesDetailGrid()
                DataBindTransSeriesGrid()
                upnlTransSeriesGrid.Update()

            End If
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    Protected Sub cmbBaseTypeList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBaseTypeList.SelectedIndexChanged

        mBaseTransTypeID = cmbBaseTypeList.SelectedValue
        mTransTextSeries = TransTextSeries.NewTransTextSeries(mBaseTransTypeID)
        mTransTextSeries.BaseTransTypeID = mBaseTransTypeID
        mTransTextSeries.BaseTransTypeName = cmbBaseTypeList.SelectedItem.Text
        rdbFinancialYear.Checked = True
        rdbCalendarYear.Checked = False
        rdbCustom.Checked = False
        Session("BaseTransTypeID_ForTransSeries") = mBaseTransTypeID
        Session("TransTextSeries") = mTransTextSeries
        SetDateControlFormat(mTransTextSeries.DatePeriodFormatID)
        SetDateControlDefaultValues(mTransTextSeries.DatePeriodFormatID)
        DataBindTransSeriesDetailGrid()
        DataBindTransSeriesGrid()
        upnlTransSeriesGrid.Update()
        EnableDisbaleControl_New()

    End Sub
    Protected Sub rdbFinancialYear_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbFinancialYear.CheckedChanged
        OnDateFormatOptionChanged(1)
        FillSuffix()
    End Sub
    Protected Sub rdbCalendarYear_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbCalendarYear.CheckedChanged
        OnDateFormatOptionChanged(2)
        FillSuffix()
    End Sub
    Protected Sub rdbCustom_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbCustom.CheckedChanged
        chkAutoRenew.Checked = False
        OnDateFormatOptionChanged(3)
        FillSuffix()
    End Sub
    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNew.Click

        mBaseTransTypeID = 0
        cmbBaseTypeList.SelectedValue = mBaseTransTypeID
        NewRecord()
        cvBaseType.ErrorMessage = ""
        cvBaseType.IsValid = True
        upnlErrorList.Update()

    End Sub
    Private Sub NewRecord()
        mTransTextSeries = TransTextSeries.NewTransTextSeries(mBaseTransTypeID)
        mTransTextSeries.BaseTransTypeID = mBaseTransTypeID
        mTransTextSeries.BaseTransTypeName = cmbBaseTypeList.SelectedItem.Text
        rdbFinancialYear.Checked = True
        rdbCalendarYear.Checked = False
        rdbCustom.Checked = False
        EnableDisbaleControl_New()
        SetDateControlFormat(mTransTextSeries.DatePeriodFormatID)
        SetDateControlDefaultValues(mTransTextSeries.DatePeriodFormatID)
        DataBindTransSeriesDetailGrid()
        DataBindTransSeriesGrid()
        BindParentCombo()
        upnlTransTextSeries.Update()
        upnlTransSeriesGrid.Update()
        upnldgTransSeriesDetails.Update()
        Session("TransTextSeries") = mTransTextSeries
        Session("BaseTransTypeID_ForTransSeries") = mBaseTransTypeID
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        If IsValid Then

            SetObject()

            If Save() Then

                If CInt(Session("OpenFrmLnk")) = 1 Then 'If opened Menu Link

                    cmbBaseTypeList.SelectedValue = Session("BaseTransTypeID_ForTransSeries") ' mTransTextSeries.BaseTransTypeID

                    mBaseTransTypeID = cmbBaseTypeList.SelectedValue

                    NewRecord()

                Else
                    Me.Continue()
                End If

            End If
        End If

    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Protected Sub txtFromDate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFromDate.TextChanged
        If txtFromDate.Text.Trim = String.Empty Then
            mTransTextSeries.FromDate = System.DBNull.Value
        Else
            mTransTextSeries.FromDate = txtFromDate.Text.Trim
            FillSuffix()
        End If
        Session("TransTextSeries") = mTransTextSeries
    End Sub

    Protected Sub txtTodate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtToDate.TextChanged
        If txtToDate.Text.Trim = String.Empty Then
            mTransTextSeries.ToDate = System.DBNull.Value
        Else
            mTransTextSeries.ToDate = txtToDate.Text.Trim
            FillSuffix()
        End If
        Session("TransTextSeries") = mTransTextSeries


    End Sub

#End Region

    Private Function CheckIsDateRangeValid() As Boolean
        If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
            Dim mFromDate, mToDate As Date

            mFromDate = txtFromDate.Text
            mToDate = txtToDate.Text

            Dim Diff = DateDiff(DateInterval.Day, mFromDate, mToDate)

            If Diff > 365 Then
                Return False
            Else
                Return True
            End If

        Else
            Return False
        End If

    End Function

    'Added by bhushan

    Private Sub dgTransactionSeriesList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTransactionSeriesList.RowCommand

        Dim Index As Integer = (CType(e.CommandArgument, Integer))

        mTransTextSeries = TransTextSeries.GetTransTextSeries(mTransTextSeriesCollection.Item(Index).ID)
        Session("TransTextSeries_ID") = mTransTextSeriesCollection.Item(Index).ID 'New Guid(e.Item.Cells(0).Text)
        Session("TransTextSeries") = mTransTextSeries

        Select Case e.CommandName

            Case ("EditRecord")

                EditRecord()

            Case ("DeleteRecord")

                DeleteRecord(mTransTextSeries.ID)

        End Select

    End Sub

    Private Sub dgTransactionSeriesList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgTransactionSeriesList.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then

            Dim TransTextSeriesID As Guid = (DataBinder.Eval(e.Row.DataItem, "ID"))
            Dim grdTransTextseries As GridView = DirectCast(e.Row.FindControl("grdTransTextseries"), GridView)


            Dim mTransTextSeriesDetails As TransTextSeriesDetails = TransTextSeriesDetails.GetTransTextSeriesDetails(TransTextSeriesID)
            grdTransTextseries.DataSource = mTransTextSeriesDetails
            grdTransTextseries.DataBind()

        End If
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
End Class