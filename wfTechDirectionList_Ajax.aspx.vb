'Added By Vikrant On 19-Jan-2014 For All19062014

Imports System.Linq
Imports System.Collections.Generic
Imports System.Text
Public Class wfTechDirectionList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mTechnicalDirectionList As TechnicalDirectionList
    Dim mDistinctTDText As DistinctTDText
    Dim mCompStatus As CompStatus
    Dim mAssemblyStatus As AssemblyStatus
    Dim SearchIndex, DateIndex, FromDate, ToDate, TDText, PartName, SerialNo, No As String
    Dim IntExtIndex As String = "0"
    Dim EventLogID As Guid
    Dim mWODetail As String
    Dim totcnt As Integer
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mDistinctTDText = Session("mDistinctTDText")
        TDText = Session("TDText")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        mTechnicalDirectionList = CType(Session("mTechnicalDirectionList"), TechnicalDirectionList)
        totcnt = Session("totcnt")
        PartName = Session("PartName")
        SerialNo = Session("SerialNo")
        IntExtIndex = Session("IntExtIndex")
    End Sub
    Private Sub SetSession()
        Session("mTechnicalDirectionList") = mTechnicalDirectionList
        Session("mDistinctTDText") = mDistinctTDText
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("No") = No
        Session("TDText") = TDText
        Session("PartName") = PartName
        Session("SerialNo") = SerialNo
        Session("IntExtIndex") = IntExtIndex
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mTechnicalDirectionList")
        Session.Remove("mDistinctTDText")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("SearchIndex")
        Session.Remove("DateIndex")
        Session.Remove("No")
        Session.Remove("TDText")
        Session.Remove("totcnt")
        Session.Remove("PartName")
        Session.Remove("SerialNo")
        Session.Remove("IntExtIndex")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfTechDirectionList_Ajax.aspx") <= 0 Then
            RemoveSession()
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                txtFromDate.Text = CDate("1-Jan-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("1-Jan-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = Today.AddDays(-6).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
            Case 2 'Last 1 Month
                txtFromDate.Text = Today.AddDays(1).AddMonths(-1).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
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
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End If
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date)
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date)
                txtFromDate.Text = CDate(FromDate).ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate(ToDate).ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub setVariables()
        SearchIndex = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex <= 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        TDText = IIf(cmbTDText.SelectedIndex <= 0, "", cmbTDText.SelectedValue)
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        PartName = IIf(SearchIndex = 3, txtPartNo.Text.Trim, "")
        SerialNo = IIf(SearchIndex = 4, txtPartNo.Text.Trim, "")
        IntExtIndex = IIf(cmbIntExt.SelectedIndex <= 0, 0, cmbIntExt.SelectedValue.ToString)

        No = txtNo.Text.Trim
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("No") = No
        Session("TDText") = TDText
        Session("PartName") = PartName
        Session("SerialNo") = SerialNo
        Session("IntExtIndex") = IntExtIndex
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex, IntExtIndex)
        dgTechnicalDirectionList.DataBind()
        cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        ''cmbWO.SelectedValue = TDText
        cmbIntExt.SelectedIndex = IntExtIndex
        cmbTDText.SelectedValue = IIf(TDText = "", "(All)", TDText)
        txtNo.Text = No
        txtPartNo.Text = IIf(SearchIndex = 3, PartName, SerialNo)
        ControlVisibility(SearchIndex, DateIndex)

        lblResult.Text = "List of Technical Direction(s) Jobs as per criteria :" & mTechnicalDirectionList.Count & " Record(s) found."
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Int32 = 0, Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal PartName As String = "", Optional ByVal SerialNo As String = "", Optional ByVal IntExtIndex As String = "")
        mTechnicalDirectionList = Nothing
        dgTechnicalDirectionList.DataSource = Nothing

        mTechnicalDirectionList = TechnicalDirectionList.GetTechnicalDirection(Text, No, FromDate, ToDate, PartName, SerialNo, CType(IntExtIndex, Integer))

        Dim tmpTechnicalDirectionList = (From c As TechnicalDirectionList.TechnicalDirectionListInfo In mTechnicalDirectionList
                                 Order By CDate(c.TDDate.ToString) Descending, c.TDNo Descending
                                 Select c).ToList

        dgTechnicalDirectionList.DataSource = tmpTechnicalDirectionList
        'dgWOJobList.DataBind()
        Session("mTechnicalDirectionList") = mTechnicalDirectionList
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer, ByVal IntExtIndex As String)
        Select Case Index
            Case -1 'all
                FindNow(, , , , , , IntExtIndex)
            Case 0 'all
                FindNow(, , , , , , IntExtIndex)
            Case 1 'TD Date 
                FindNow(, , txtFromDate.Text, txtToDate.Text, , , IntExtIndex)
            Case 2  'TD No
                FindNow(TDText, CInt(Val(No)), , , , , IntExtIndex)
            Case 3  'Part Name
                FindNow(, , , , PartName, , IntExtIndex)
            Case 4  'Serial No.
                FindNow(, , , , , SerialNo, IntExtIndex)
        End Select
        dgTechnicalDirectionList.PageIndex = 0
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        'lblFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        'lblToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        'txtFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        'txtToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        cmbTDText.Visible = IIf(SearchIndex = 2, True, False)
        txtPartNo.Visible = IIf((SearchIndex = 3 Or SearchIndex = 4), True, False)
        txtNo.Visible = IIf(cmbTDText.SelectedIndex > 0, True, False)
        If SearchIndex = 1 And DateIndex = 6 Then
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
        txtPartNo.Text = ""
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then

                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok 'And Session("sender") = ""       
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
#End Region

#Region " DataFieldBind "
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        TDText = Session("TDText")
        PartName = IIf(IsNothing(PartName), "", PartName)
        SerialNo = IIf(IsNothing(SerialNo), "", SerialNo)


        mDistinctTDText = DistinctTDText.GetDistinctText("(All)")
        cmbTDText.DataSource = mDistinctTDText
        Session("mDistinctTDText") = mDistinctTDText

        mTechnicalDirectionList = TechnicalDirectionList.GetTechnicalDirection(, , , , , )

        totcnt = mTechnicalDirectionList.Count
        Session("mTechnicalDirectionList") = mTechnicalDirectionList

        dgTechnicalDirectionList.DataSource = mTechnicalDirectionList
        Session("mTechnicalDirectionList") = mTechnicalDirectionList

        lblResult.Text = "List of Technical Direction(s) as per criteria :" & mTechnicalDirectionList.Count & " Record(s) found."
        DataBind()

    End Sub
    'added by Saylee on 16-Feb-2017 to show proper PeriodUnit for Technical Direction
    Public Function GetPeriodUnitID(PeriodID As Integer) As Integer
        Select Case PeriodID
            Case 1
                Return 1
            Case 2
                Return 0
            Case 3
                Return 6
            Case 4
                Return 7
            Case 5
                Return 8
            Case 6
                Return 9
            Case 7
                Return 10
            Case 8
                Return 11
            Case 9
                Return 12
            Case 10
                Return 13
            Case 11
                Return 14
            Case 12
                Return 15
            Case 13
                Return 16
            Case 14
                Return 17
            Case 15
                Return 18
        End Select
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfTechDirectionList_Ajax.aspx"
            If cmbSearch.Enabled = True Then
                setFocus(cmbSearch)
            End If
            DataFieldBind()
            SetControl()
            MessageBoxResult()
        End If
    End Sub
    Private Sub dgTechnicalDirectionList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTechnicalDirectionList.RowCommand
        Dim mStatusID As Guid
        Dim mTypeID As Int16
        Dim mtechDirection As rptTechDirection
        Dim mAssemblyList As AssemblyList
        Dim mMachineID As Guid
        Dim Index As Int32

        Select Case e.CommandName
            Case "EditRec"
                'Index = CInt(e.CommandArgument)
                Index = CInt(e.CommandArgument) + dgTechnicalDirectionList.PageIndex * dgTechnicalDirectionList.PageSize
                'mId = New Guid(dgTechnicalDirectionList.DataKeys(Index).Item("ID").ToString)
                mStatusID = New Guid(dgTechnicalDirectionList.DataKeys(Index).Item("StatusID").ToString)
                mTypeID = CInt(dgTechnicalDirectionList.DataKeys(Index).Item("TypeID").ToString)
                mMachineID = New Guid(dgTechnicalDirectionList.DataKeys(Index).Item("MachineID").ToString)

                'mWODetail = mWorkOrderNo + " Dated : " + mDate + " Description : " + mDescription + " Action : " + mAction + " Job Type : " + mJobType + " Job status : " + mJobSatatus
                'MarkLog(Util.Action.Edit, "Work Order Job", mWODetail, Util.ErrorType.NoError, mId, EventLogID)

                If mTypeID = 1 Then
                    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mStatusID)
                    mtechDirection = rptTechDirection.GetTechDirection(mAssemblyStatus.ID, 1, mAssemblyStatus.RemovedOn.ToString) '1 for Assembly
                    If mAssemblyStatus.RemovalReasonName = "(SELECT)" Then
                        mtechDirection.RemovalReason = ""
                    Else
                        mtechDirection.RemovalReason = mAssemblyStatus.RemovalReasonName
                    End If
                    'mtechDirection.Date = mAssemblyStatus.RemovedOn 'Commented by Saylee on 27-Mar-2017 as date should be TDdate and not Removal date
                    mtechDirection.RemovalDate = mAssemblyStatus.RemovedOn
                    mtechDirection.Position = mAssemblyStatus.Position 'Added By Prashant 3-Jun-2022
                    mAssemblyList = AssemblyList.GetAssemblyList(1, mAssemblyStatus.MachineID.ToString, mAssemblyStatus.RemovedOn.ToString)
                    mtechDirection.ATA = mAssemblyStatus.ATAChapter
                    mtechDirection.PartNo = mAssemblyStatus.ModelName
                    mtechDirection.Description = ""
                    mtechDirection.SerialNo = mAssemblyStatus.Assembly.SerialNo
                    mtechDirection.ModelName = mAssemblyList(0).ModelName 'mMachineinfo(0).ModelName
                    mtechDirection.AircaftName = mAssemblyList(0).RegNo
                    mtechDirection.AircaftSrNo = mAssemblyList(0).SerialNo
                    mtechDirection.IsRemUnschedule = mAssemblyStatus.IsRemUnschedule
                    '  mtechDirection.TimeSinceNew = String.Join(", ", From c As AssemblyStatusPeriod In mAssemblyStatus.AssemblyStatusPeriods Select New Period(c.PeriodID, c.AssemblyRemovalValue, 0, CBool(IIf(c.PeriodID = 2, True, False)), False, c.HourType).TextFormatted)
                    mtechDirection.TimeSinceNew = String.Join(", ", From c As AssemblyStatusPeriod In mAssemblyStatus.AssemblyStatusPeriods Select New Period(c.PeriodID, c.AssemblyRemovalValue, GetPeriodUnitID(c.PeriodID), CBool(IIf(c.PeriodID = 2, True, False)), False, c.HourType).TextFormatted)
                Else
                    mCompStatus = CompStatus.GetCompStatus(mStatusID, Guid.Empty, Today.ToString)
                    mtechDirection = rptTechDirection.GetTechDirection(mCompStatus.ID, 2, mCompStatus.RemovedOn.ToString) '2 for compoenent
                    If mCompStatus.RemovalReasonName = "(SELECT)" Then
                        mtechDirection.RemovalReason = ""
                    Else
                        mtechDirection.RemovalReason = mCompStatus.RemovalReasonName
                    End If
                    'mtechDirection.Date = mCompStatus.RemovedOn 'Commented by Saylee on 27-Mar-2017 as date should be TDdate and not Removal date
                    mtechDirection.RemovalDate = mCompStatus.RemovedOn
                    mtechDirection.Position = mCompStatus.Position 'Added By Prashant 3-Jun-2022
                    mAssemblyList = AssemblyList.GetAssemblyList(1, mMachineID.ToString, mCompStatus.RemovedOn.ToString)
                    mtechDirection.ATA = mCompStatus.ATAChapter
                    mtechDirection.PartNo = mCompStatus.PartName
                    mtechDirection.Description = mCompStatus.Description
                    mtechDirection.SerialNo = mCompStatus.SerialNo
                    mtechDirection.ModelName = mAssemblyList(0).ModelName
                    mtechDirection.AircaftName = mAssemblyList(0).RegNo
                    mtechDirection.AircaftSrNo = mAssemblyList(0).SerialNo
                    mtechDirection.IsRemUnschedule = mCompStatus.IsRemUnschedule
                    'mtechDirection.TimeSinceNew = String.Join(", ", From c As CompStatusPeriod In mCompStatus.CompStatusPeriods Select New Period(c.PeriodID, c.CompRemovalValue, 0, CBool(IIf(c.PeriodID = 2, True, False)), False, c.HourType).TextFormatted)
                    mtechDirection.TimeSinceNew = String.Join(", ", From c As CompStatusPeriod In mCompStatus.CompStatusPeriods Select New Period(c.PeriodID, c.CompRemovalValue, GetPeriodUnitID(c.PeriodID), CBool(IIf(c.PeriodID = 2, True, False)), False, c.HourType).TextFormatted)
                End If
                Session("mrptTechDirection") = mtechDirection
                Dim str As String
                str = "openledgersame('wfTechDirection.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        End Select
    End Sub
    Private Sub dgTechnicalDirectionList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTechnicalDirectionList.PageIndexChanging
        dgTechnicalDirectionList.PageIndex = e.NewPageIndex
        dgTechnicalDirectionList.DataSource = mTechnicalDirectionList
        Session("mTechnicalDirectionList") = mTechnicalDirectionList
        dgTechnicalDirectionList.DataBind()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click, cmbIntExt.SelectedIndexChanged, txtNo.TextChanged, txtPartNo.TextChanged, txtFromDate.TextChanged, txtToDate.TextChanged
        setVariables()
        CallFindNow(SearchIndex, IntExtIndex)
        dgTechnicalDirectionList.DataBind()
        lblResult.Text = "List of Technical Direction(s) as per criteria :" & mTechnicalDirectionList.Count & " Record(s) found."
        upnlGridView.Update()
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        ClearControls()
        cmbDate.SelectedIndex = 0
        cmbTDText.SelectedIndex = 0
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        If cmbSearch.Enabled = True Then
            setFocus(cmbSearch)
        End If
        Call btnFindNow_Click(sender, e)
    End Sub
    Private Sub cmbTDText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTDText.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbTDText.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        If cmbTDText.Enabled = True Then
            setFocus(cmbTDText)
        End If
        Call btnFindNow_Click(sender, e)
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        If cmbDate.Enabled = True Then
            setFocus(cmbDate)
        End If
        Call btnFindNow_Click(sender, e)
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click, btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgTechnicalDirectionList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgTechnicalDirectionList.Sorting
        mTechnicalDirectionList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgTechnicalDirectionList.DataSource = mTechnicalDirectionList
        Session("mTechnicalDirectionList") = mTechnicalDirectionList
        dgTechnicalDirectionList.DataBind()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        Dim Rpt As New crTechnicalDirectionRegister
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsTechnicalDirection
        Dim ReportDetails As New rptStatusList
        Dim mCompanyDetail As New CompanyDetail

        Dim TotalCount As Integer
        Dim mCurrentPageindex As Integer = Me.dgTechnicalDirectionList.PageIndex
        TotalCount = Me.dgTechnicalDirectionList.PageCount
        Dim j As Integer
        Dim I As Integer
        Dim str(14) As String

        For j = 0 To TotalCount - 1
            Me.dgTechnicalDirectionList.PageIndex = j
            Me.dgTechnicalDirectionList.DataSource = mTechnicalDirectionList
            Session("mTechnicalDirectionList") = mTechnicalDirectionList
            dgTechnicalDirectionList.DataBind()
            For I = 0 To Me.dgTechnicalDirectionList.PageSize - 1
                If I <= Me.dgTechnicalDirectionList.Rows.Count - 1 Then
                    str(0) = ""
                    str(1) = ""
                    str(2) = ""
                    str(3) = ""
                    str(4) = ""
                    str(5) = ""
                    str(6) = ""
                    str(7) = ""
                    str(8) = ""
                    str(9) = ""
                    str(10) = ""
                    str(11) = ""
                    str(12) = ""
                    str(13) = ""
                    str(14) = ""

                    If Me.dgTechnicalDirectionList.Rows(I).Cells(3).Text <> "&nbsp;" Then str(0) = Me.dgTechnicalDirectionList.Rows(I).Cells(3).Text
                    If Me.dgTechnicalDirectionList.Rows(I).Cells(4).Text <> "&nbsp;" Then str(1) = Me.dgTechnicalDirectionList.Rows(I).Cells(4).Text
                    If Me.dgTechnicalDirectionList.Rows(I).Cells(6).Text <> "&nbsp;" Then str(2) = Me.dgTechnicalDirectionList.Rows(I).Cells(6).Text
                    If Me.dgTechnicalDirectionList.Rows(I).Cells(7).Text <> "&nbsp;" Then str(3) = Me.dgTechnicalDirectionList.Rows(I).Cells(7).Text
                    If Me.dgTechnicalDirectionList.Rows(I).Cells(8).Text <> "&nbsp;" Then str(4) = Me.dgTechnicalDirectionList.Rows(I).Cells(8).Text
                    If Me.dgTechnicalDirectionList.Rows(I).Cells(9).Text <> "&nbsp;" Then str(5) = Me.dgTechnicalDirectionList.Rows(I).Cells(9).Text
                    If Me.dgTechnicalDirectionList.Rows(I).Cells(10).Text <> "&nbsp;" Then str(6) = Me.dgTechnicalDirectionList.Rows(I).Cells(10).Text
                    If Me.dgTechnicalDirectionList.Rows(I).Cells(11).Text <> "&nbsp;" Then str(7) = Me.dgTechnicalDirectionList.Rows(I).Cells(11).Text
                    If Me.dgTechnicalDirectionList.Rows(I).Cells(12).Text <> "&nbsp;" Then str(8) = Me.dgTechnicalDirectionList.Rows(I).Cells(12).Text

                    ReportDetails.Add(New rptStatus(, 1, , str(0), str(1), str(2), str(3), str(4), str(5), str(6), _
                                   str(7), str(8), str(9), str(10), str(11), str(12), str(13), str(14)))
                End If
            Next
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, Title, "", "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mTechnicalDirectionList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1293)
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'Changed By Utkarsh On 20-Jul-2011 For All19072011
        ' MarkLog(Util.Action.Print, "ReceiptCumInvoice", "Receipt-Cum-Invoice List Report", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
        MarkLog(Util.Action.Print, "TechnicalDirection", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'Added by Saylee on 16-June 2007
        Me.dgTechnicalDirectionList.PageIndex = mCurrentPageindex
        Me.dgTechnicalDirectionList.DataSource = mTechnicalDirectionList
        Session("mTechnicalDirectionList") = mTechnicalDirectionList
        dgTechnicalDirectionList.DataBind()
        upnlGridView.Update()
    End Sub
    Protected Sub dgTechnicalDirectionList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgTechnicalDirectionList.Columns(i).HeaderText
            Next
        End If
    End Sub
#End Region

End Class