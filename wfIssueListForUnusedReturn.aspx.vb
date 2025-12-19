'Created By Utkarsh On 04-May-2012 FOR ALLIssue04052012

Partial Class wfIssueListForUnusedReturn
    Inherits Page

#Region " Enumaration "

    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
    End Enum

#End Region

#Region " Variable Declaration "

    Public mIssueList As IssueList
    Public mIssue As Issue
    Public mDistinctTextListForIssue As DistinctTextListForIssue
    Dim objSearch As rptSearchingCriteriaForReceipt
    Dim objReg As rptIssueReg
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, IssueText, ReceiptText, WOText, IssueTypeId, Name, No, IssueTo, IssueAs As String
    Dim mTransTypeID As Trans
    Dim mTransTypeList As TransactionList
    Public ModuleName As String
    Public Tital As String
    Public mIssueTypeList As IssueTypeList
    'Rajnish 19-08-2008
    'Public mWOList As FlyPal22.Maintain.WOList
    Dim mDistinctWOText As nDistinctWOText
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Dim mIssueDetail As String
    Dim totcnt As Integer

    Dim DateFormat As String = AppSettings("DateFormat").ToString()

#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.

    Private designerPlaceholderDeclaration As Object

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mIssueTypeList = Session("mIssueTypeList")
        mIssue = Session("mIssue")
        mIssueList = Session("mIssueList")
        mTransTypeID = Session("mTransTypeID")
        mDistinctTextListForIssue = Session("mDistinctTextListForIssue")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        IssueTypeId = Session("IssueTypeId")
        IssueText = Session("IssueText")
        ReceiptText = Session("ReceiptText")
        Name = Session("Name")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        ModuleName = Session("ModuleName")
        IssueTo = Session("IssueTo")
        IssueAs = Session("IssueAs")
    End Sub

    Private Sub SetSession()
        Session("mIssueTypeList") = mIssueTypeList
        Session("mIssue") = mIssue
        Session("mIssueList") = mIssueList
        Session("mTransTypeID") = mTransTypeID
        Session("mDistinctTextListForIssue") = mDistinctTextListForIssue
        Session("ModuleName") = ModuleName
        Session("IssueTo") = IssueTo
        Session("IssueAs") = IssueAs
        Session("mDistinctWOText") = mDistinctWOText
    End Sub

    Private Sub RemoveSession()
        Session.Remove("mIssue")
        Session.Remove("mIssueList")
        Session.Remove("mDistinctTextListForIssue")
        Session.Remove("mDistinctTextListForReceipt")
        Session.Remove("SearchIndex")
        Session.Remove("DateIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("StatusId")
        Session.Remove("IssueTypeId")
        Session.Remove("IssueText")
        Session.Remove("ReceiptText")
        Session.Remove("WOText")

        Session.Remove("Name")
        Session.Remove("No")
        'Added on 22-01-2007 ''  value of machine is not refresing (SearcCriteriaforFlyingHours.aspx)
        Session.Remove("mMachineList")
        'Session.Remove("SelectedIndex")
        Session.Remove("mTransTypeId")
        Session.Remove("mIssueTypeList")
        Session.Remove("IssueTo")
        Session.Remove("IssueAs")
        Session.Remove("mDistinctWOText")
        Session.Remove("totcnt")
    End Sub

    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfIssueListForUnusedReturn.aspx?") <= 0 Then
            RemoveSession()
            Session.Remove("mOrder")
        End If
    End Sub

    Private Sub NewRecord()
        mIssue = Issue.NewIssue(mTransTypeID)
        mIssue.IDate = Today.Date
        If mTransTypeID = 16 Or mTransTypeID = 18 Or mTransTypeID = 49 Or mTransTypeID = 51 Or mTransTypeID = 55 Or mTransTypeID = 58 Or mTransTypeID = 59 Or mTransTypeID = 60 Then  '55, 58 Added By Prashant 6-Jan-2010 
            mIssue.IssueItems.Add(mIssue.ID, mTransTypeID)
            mIssue.IssueItems.CurrentIndex = mIssue.IssueItems.Count - 1
        End If
        Session("mIssue") = mIssue
        '---------------------------------------------------
        Session("IssueTo") = IssueTo
        Session("IssueAs") = IssueAs
        '---------------------------------------------------
    End Sub

    Private Sub EditRecord(mId As Guid)
        mIssue = Issue.GetIssue(mId)
        mIssue.MarkClean()
        Session("mIssue") = mIssue

        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
        ModuleName = mTransTypeList.GetTransactionTypeName(mIssue.TransTypeID).ToString
        Session("ModuleName") = ModuleName
        Session("mIssue") = mIssue

        '---------------------------------------------------
        Session("IssueTo") = IssueTo
        Session("IssueAs") = IssueAs
        '---------------------------------------------------
    End Sub

    Private Sub DataFieldBind()
        Session("totcnt") = totcnt
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        StatusId = Session("StatusId")
        IssueText = Session("IssueText")
        ReceiptText = Session("ReceiptText")
        'Rajnish 19-08-2008
        WOText = Session("WOText")

        IssueTypeId = Session("IssueTypeId")
        Name = Session("Name")
        mDistinctTextListForIssue = DistinctTextListForIssue.GetDistinctText("3", , True, "(All)")
        cmbIssueText.DataSource = mDistinctTextListForIssue

        mIssueList = IssueList.GetIssueList(,
                                            No:=0,
                                            FromDate:="1/1/1900",
                                            ToDate:="1/1/2200", , , ,
                                            IssueToType:=0,
                                            StatusID:=0, ,
                                            ReceiptNo:=0, , , ,
                                            TransTypeID:=mTransTypeID, , , , , ,
                                            IsUnusedReturnItem:=True)
        totcnt = mIssueList.Count  'Added by shweta on 23-12-11
        Session("totcnt") = totcnt 'Added by shweta on 23-12-11
        gvIssueList.DataSource = mIssueList
        Session("mIssueList") = mIssueList

        mIssueTypeList = IssueTypeList.GetIssueTypeList(0)

        Session("mIssueTypeList") = mIssueTypeList
        DataBind()
        lblResult.Text = "List of Issue as per criteria : " & mIssueList.Count & " Record(s) found."
    End Sub

    Private Sub DataFieldBindForSymco() 'Added by Saylee on 28-July-2010
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 2, DateIndex)
        StatusId = Session("StatusId")
        IssueText = Session("IssueText")
        ReceiptText = Session("ReceiptText")
        'Rajnish 19-08-2008
        WOText = Session("WOText")

        IssueTypeId = Session("IssueTypeId")
        Name = Session("Name")
        mDistinctTextListForIssue = DistinctTextListForIssue.GetDistinctText("3", , True, "(All)")
        cmbIssueText.DataSource = mDistinctTextListForIssue
        mIssueList = IssueList.GetIssueList(,
                                            No:=0,
                                            FromDate:="1/1/1900",
                                            ToDate:="1/1/2200", , , ,
                                            IssueToType:=0,
                                            StatusID:=0, ,
                                            ReceiptNo:=0, , , ,
                                            TransTypeID:=mTransTypeID, , , , , ,
                                            IsUnusedReturnItem:=True)
        gvIssueList.DataSource = mIssueList
        Session("mIssueList") = mIssueList

        mIssueTypeList = IssueTypeList.GetIssueTypeList(0)

        Session("mIssueTypeList") = mIssueTypeList
        DataBind()
        lblResult.Text = "List of Issue as per criteria : " & mIssueList.Count & " Record(s) found."
    End Sub

    Private Overloads Sub SetFocus(control As WebControl)
        If control.Enabled = False Or control.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Delete" Then
                        Try
                            Dim mIssue As Issue
                            Session("sender") = ""
                            mIssue = CType(Session("mIssue"), Issue)
                            SetModuleNameWhileGettingDelete(mIssue.TransTypeID)
                            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer") Then
                                If (mIssue.IsSync = 1 Or mIssue.IsSync = 2) Then
                                    Dim msg1 As New SIMsgBox(Page, "Alert!", "This Transaction cannot be deleted. Already sent for billing.", "", MsgBoxStyle.OkOnly)
                                    msg1.ReplacePage = "wfIssueListForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID
                                    DataFieldBindForSymco()
                                    SetControl()
                                    msg1.Show()
                                    Exit Sub
                                Else
                                    mIssue.Delete()
                                    mIssue.Save()
                                    Response.Redirect("wfIssueListForUnusedReturn.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID)
                                End If
                            Else
                                mIssue.Delete()
                                mIssue.Save()
                                Response.Redirect("wfIssueListForUnusedReturn.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID)
                            End If
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                                msg1.ReplacePage = "wfIssueListForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                                msg1.ReplacePage = "wfIssueListForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                                msg1.ReplacePage = "wfIssueListForUnusedReturn.aspx?BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID
                                'MarkLog(Action.Delete, ModuleName, "Can't delete : This is Currently in use", ErrorType.NoError, mIssue.ID)
                                mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + " to " + mIssueList(mIssue.ID).Destination
                                MarkLog(Action.Delete, ModuleName, "Can't delete : " & mIssueDetail & " is Currently in use", ErrorType.NoError, mIssue.ID, EventLogID)
                                msg1.Show()
                            End If
                            DataFieldBind()
                            SetControl()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Action.Delete, ModuleName, mIssue.IssueNo, ErrorType.NoError, mIssue.ID)
                                mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + " to " + mIssueList(mIssue.ID).Destination
                                MarkLog(Action.Delete, ModuleName, mIssueDetail, ErrorType.NoError, mIssue.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfIssueListForUnusedReturn.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID)
                Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfIssueListForUnusedReturn.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID)
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
                    Response.Redirect("wfIssueListForUnusedReturn.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID)
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfIssueListForUnusedReturn.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID)

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub

    Private Sub FindNow(Optional Text As String = "", Optional No As Integer = 0, Optional FromDate As String = "1-Jan-1900", Optional ToDate As String = "1-Jan-2099", Optional StoreName As String = "", Optional VendorName As String = "", Optional AircraftName As String = "", Optional ToTypeId As Int32 = 0, Optional StatusID As Int32 = 0, Optional ReceiptText As String = "", Optional ReceiptNo As Int32 = 0, Optional RealeaseNoteNo As String = "", Optional SerialNo As String = "", Optional ItemName As String = "", Optional WorkShop As String = "")
        mIssueList = Nothing
        gvIssueList.DataSource = Nothing

        Dim IsVendor As Integer
        If ToTypeId = 1 Then
            IsVendor = 1
        ElseIf ToTypeId = 15 Then
            IsVendor = 2
        Else
            IsVendor = 0
        End If
        'Get List From the Database as per Criteria             
        mIssueList = IssueList.GetIssueList(Text,
                                            No,
                                            FromDate,
                                            ToDate,
                                            StoreName,
                                            VendorName,
                                            RegNo:=AircraftName,
                                            IssueToType:=ToTypeId,
                                            StatusID,
                                            ReceiptText,
                                            ReceiptNo,
                                            ReleaseNoteNo:=RealeaseNoteNo,
                                            SerialNo,
                                            ItemName, ,
                                            mIsVendor:=IsVendor,
                                            WorkShop, , , ,
                                            IsUnusedReturnItem:=True)
        'Set DataSource of the Grid
        Session("mIssueList") = mIssueList
        gvIssueList.DataSource = mIssueList
        'Set Mapping Name 
    End Sub

    Private Sub CallFindNow(Index As Integer)
        Select Case Index
            Case -1 'all
                FindNow()
            Case 0 'all
                FindNow()
            Case 1 'issue date
                'FindNow(, , FromDate, ToDate)
                FindNow(, , FromDate_Txt.Text.ToString, ToDate_Txt.Text.ToString)
            Case 2  'issue no
                FindNow(IssueText, CInt(Val(No)))
            Case 3 'Item name
                FindNow(, , , , , , , , , , , , , Trim(Name))
            Case 4  'Aircraft name
                FindNow(, , , , , , Trim(Name), 2)
            Case 5 'serial no
                FindNow(, , , , , , , , , , , , Trim(Name))
            Case 6 'WorkShop
                FindNow(, , , , , , , 16, , , , , , , Trim(Name))
        End Select
        gvIssueList.PageIndex = 0   'Added Code on May,25,2007
    End Sub

    Private Sub SetPeriod(Index As Int32)
        Select Case Index
            Case 0 ' All                   
                FromDate_Txt.Text = CDate("1-Jan-1900").ToString(DateFormat)
                ToDate_Txt.Text = CDate("1-Jan-2200").ToString(DateFormat)
            Case 1 'Last 1 Week
                FromDate_Txt.Text = Today.AddDays(-6).ToString(DateFormat)
                ToDate_Txt.Text = Today.ToString(DateFormat)
            Case 2 'Last 1 Month
                FromDate_Txt.Text = Today.AddDays(1).AddMonths(-1).ToString(DateFormat)
                ToDate_Txt.Text = Today.ToString(DateFormat)
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        FromDate_Txt.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(DateFormat)
                        ToDate_Txt.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(DateFormat)
                    Case 4, 5, 6
                        FromDate_Txt.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(DateFormat)
                        ToDate_Txt.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(DateFormat)
                    Case 7, 8, 9
                        FromDate_Txt.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(DateFormat)
                        ToDate_Txt.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(DateFormat)
                    Case 10, 11, 12
                        FromDate_Txt.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(DateFormat)
                        ToDate_Txt.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(DateFormat)
                End Select
            Case 4 'Last 1 Year
                FromDate_Txt.Text = Today.AddDays(1).AddYears(-1).ToString(DateFormat)
                ToDate_Txt.Text = Today.ToString(DateFormat)
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    FromDate_Txt.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(DateFormat)
                Else
                    FromDate_Txt.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(DateFormat)   '31-Mar-2006
                End If
                ToDate_Txt.Text = Today.ToString(DateFormat)
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date) 'Changes by Prashant on 09-01-2008
                FromDate_Txt.Text = CDate(FromDate).ToString(DateFormat)
                ToDate_Txt.Text = CDate(ToDate).ToString(DateFormat)
        End Select
    End Sub

    Private Sub ControlVisibility(SearchIndex As Int32, Optional DateIndex As Int32 = 0)
        cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        lblFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        lblToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        FromDate_Txt.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        ToDate_Txt.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        cmbIssueText.Visible = IIf(SearchIndex = 2, True, False)
        lblNo.Visible = IIf(SearchIndex = 2 And cmbIssueText.SelectedIndex <> 0, True, False)
        txtNo.Visible = IIf(SearchIndex = 2 And cmbIssueText.SelectedIndex <> 0, True, False)
        txtName.Visible = IIf(SearchIndex >= 3 And SearchIndex <= 6, True, False)
        If SearchIndex = 1 And DateIndex = 6 Then
            FromDate_Txt.Visible = True
            ToDate_Txt.Visible = True
            FromDate_Txt.Enabled = True
            ToDate_Txt.Enabled = True
        ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            FromDate_Txt.Visible = True
            ToDate_Txt.Visible = True
            FromDate_Txt.Enabled = False
            ToDate_Txt.Enabled = False
        Else
            FromDate_Txt.Visible = False
            ToDate_Txt.Visible = False
        End If
    End Sub

    Private Sub ClearControls()
        txtNo.Text = ""
        txtName.Text = ""
    End Sub

    Private Sub CallFindNowReport(Index As Integer)
        Tital = GetTitle()
        Dim IssueText As String = ""
        IssueText = IIf(cmbIssueText.SelectedIndex <= 0, "", cmbIssueText.SelectedItem.Text)
        Select Case Index
            Case -1 'all
                objReg = rptIssueReg.GetrptIssueList(, , "1/1/1900", "1/1/2200", , , , , , , , , , , , , , mTransTypeID)
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
            Case 0 'all
                objReg = rptIssueReg.GetrptIssueList(, , "1/1/1900", "1/1/2200", , , , , , , , , , , , , , mTransTypeID)
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
            Case 1  'issue date
                objReg = rptIssueReg.GetrptIssueList(, , FromDate_Txt.Text.ToString, ToDate_Txt.Text.ToString, , , , , , , , , , , , , , mTransTypeID)
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate_Txt.Text.ToString, ToDate_Txt.Text.ToString, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
            Case 2  'issue no
                objReg = rptIssueReg.GetrptIssueList(IssueText, Trim(txtNo.Text), , , , , , , , , , , , , , , , mTransTypeID)
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", IssueText, "", "", Trim(txtNo.Text), "", "", "", "", "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
            Case 3 'Item name
                objReg = rptIssueReg.GetrptIssueList(, , , , , , , , , , , , , Trim(txtName.Text), , , , mTransTypeID)
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", Trim(txtName.Text), "", "", "", "", Tital, "", "", "", "", "", "", "")
            Case 4  'Aircraft name
                objReg = rptIssueReg.GetrptIssueList(, , , , , , Trim(txtName.Text), 2, , , , , , , , , , mTransTypeID)
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", Trim(txtName.Text), "", "", "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
            Case 5 'serial no
                objReg = rptIssueReg.GetrptIssueList(, , , , , , , , , , , , Trim(txtNo.Text), , , , , mTransTypeID)
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Tital, "", "", Trim(txtNo.Text), "", "", "", "")
        End Select
    End Sub

    Private Sub SetVariables()
        SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(FromDate_Txt.Text.ToString <> "", FromDate_Txt.Text.ToString, "1/1/1900")
        ToDate = IIf(ToDate_Txt.Text.ToString <> "", ToDate_Txt.Text.ToString, "1/1/2200")
        IssueText = IIf(cmbIssueText.SelectedIndex <= 0, "", cmbIssueText.SelectedValue)
        Name = txtName.Text.Trim
        No = txtNo.Text.Trim
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("IssueText") = IssueText
        Session("IssueTypeId") = IssueTypeId
        Session("No") = No
        Session("Name") = Name
    End Sub

    Private Sub SetControl()
        SetPeriod(DateIndex)
        CallFindNow(SearchIndex)
        gvIssueList.DataBind()
        cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        cmbIssueText.SelectedValue = IIf(IssueText = "", "(All)", IssueText)
        txtName.Text = Name
        txtNo.Text = No
        ControlVisibility(SearchIndex, DateIndex)
        lblResult.Text = "List of Issue as per criteria : " & mIssueList.Count & " Record(s) found."
    End Sub

    Private Sub SetModuleNameWhileGettingDelete(TempTransTypeID As Integer)
        Dim mTemTransTypeList As TransactionList
        mTemTransTypeList = TransactionList.GetTransactionList()
        'lblTitle.Text = "List of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
        lblTitle.Text = "List of Issue " '+ mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
        ModuleName = mTemTransTypeList.GetTransactionTypeName(TempTransTypeID).ToString
    End Sub

    Private Sub SetTitle()
        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
        'lblTitle.Text = "List of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
        ModuleName = mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
        Session("ModuleName") = ModuleName
        totcnt = Session("totcnt") 'Added by shweta on 23-12-11
        lblTitle.Text = " List of Unused Issued Items"
    End Sub

    Private Sub ControlEnability()
        BtnPrint.Enabled = IIf(gvIssueList.Rows.Count = 0, False, True)
    End Sub

    Private Sub AddAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub

    Private Function IsInRole(CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        IsInRoleString = "UnusedIssuedItems"
        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
                'Case Rights.FindNow
                '   Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
        End Select
    End Function

#End Region

#Region " Events "
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ClearAll()
        AddAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011

        If Not IsPostBack And Session("sender") = "" Then

            If cmbSearch.Enabled = True Then
                SetFocus(cmbSearch)
            End If
            Session.Remove("mPendingItemList")
            mTransTypeID = Request.QueryString("TransTypeId")
            Session("mTransTypeId") = mTransTypeID
            Session("MiddleFrame") = "wfIssueListForUnusedReturn.aspx?TransTypeId=" & mTransTypeID
            DataFieldBind()
            SetControl()

        End If

        MessageBoxResult()
        SetTitle()
        ControlEnability()

    End Sub

    Private Sub GridViewRowCommand(source As Object, e As GridViewCommandEventArgs) Handles gvIssueList.RowCommand

        Select Case e.CommandName
            Case "EditRecord"

                Dim index As Integer = CInt(e.CommandArgument) + gvIssueList.PageSize * gvIssueList.PageIndex
                Dim mId As Guid = mIssueList(index).ID
                Dim mDate As String = mIssueList(mId).ILDateFormatted.ToString
                Dim mIssueNo As String = mIssueList(mId).IssueNo
                mIssueDetail = mIssueNo + " Dated : " + mDate + " to " + mIssueList(mId).Destination

                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    ClientScript.RegisterStartupScript(type:=[GetType],
                                                       key:="OpenScript",
                                                       script:=MessageBox.Show("You are not authorized user"))
                    Exit Sub
                End If

                EditRecord(mId)
                Session("IsForWOReturn") = False
                Session("Edit") = True
                'Added By Prashant 20-Jul-2011
                mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + " to " + mIssueList(mIssue.ID).Destination
                MarkLog(Action:=Action.Edit, ModuleName,
                        Detail:=mIssueDetail,
                        ErrorType:=ErrorType.NoError,
                        TransID:=mIssue.ID, EventLogID)
                Dim str As String
                str = "<script language='javascript'>  openledgersame('wfIssueForUnusedReturn.aspx?BackPage=wfIssueListForUnusedReturn.aspx'); </script>"
                ClientScript.RegisterStartupScript(type:=[GetType],
                                                   key:="OpenScript",
                                                   script:=str)
        End Select

    End Sub

    Private Sub GridViewPagination(source As Object, e As GridViewPageEventArgs) Handles gvIssueList.PageIndexChanging
        gvIssueList.PageIndex = e.NewPageIndex
        gvIssueList.DataSource = mIssueList
        Session("mIssueList") = mIssueList
        gvIssueList.DataBind()
    End Sub

    Private Sub SearchChanged(sender As Object, e As EventArgs) Handles cmbSearch.SelectedIndexChanged
        ClearControls()
        cmbDate.SelectedIndex = 0
        cmbIssueText.SelectedIndex = 0
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        SetPeriod(DateIndex)
        If cmbSearch.Enabled = True Then
            SetFocus(cmbSearch)
        End If
    End Sub

    Private Sub IssueTextChanged(sender As Object, e As EventArgs) Handles cmbIssueText.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        SetPeriod(DateIndex)
        If cmbIssueText.Enabled = True Then
            SetFocus(cmbIssueText)
        End If
    End Sub

    Private Sub DateChanged(sender As Object, e As EventArgs) Handles cmbDate.SelectedIndexChanged
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        SetPeriod(DateIndex)
        If cmbDate.Enabled = True Then
            SetFocus(cmbDate)
        End If
    End Sub

    Private Sub FindNow(sender As Object, e As ImageClickEventArgs) Handles btnSearch.Click
        SetVariables()
        CallFindNow(SearchIndex)
        gvIssueList.DataBind()
        BtnPrint.Enabled = IIf(mIssueList.Count = 0, False, True)
        lblResult.Text = "List of Issue as per criteria : " & mIssueList.Count & " Record(s) found."
    End Sub

    Private Sub Close(sender As Object, e As EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        ModuleName = Nothing
        Response.Redirect("Dashboard.aspx")
    End Sub

    'Added By Prashant 18-June-2009
    Private Sub GridViewSortCommand(source As Object, e As GridViewSortEventArgs) Handles gvIssueList.Sorting
        mIssueList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        gvIssueList.DataSource = mIssueList
        Session("mIssueList") = mIssueList
        gvIssueList.DataBind()
    End Sub
    '------------------------------

#End Region

#Region " Report "

    'Created By :- Jyoti
    'Dated On 11/5/2007

#Region " Report Variable Declaration "

    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Private SearchStr1 As String
    Private SearchStr2 As String

#End Region

#Region " Event "

    Private Function GetTitle() As String           'New Addition
        'By - Jyoti
        'Dated by - 11/5/2007
        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String = mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString + " List Report"

        If mTitle = "" Then
            Return "Goods Outward Note List Report"
        Else
            Return mTitle
        End If
    End Function

    Private Sub Print(sender As Object, e As EventArgs) Handles BtnPrint.Click
        If Not IsInRole(Rights.Print) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            Exit Sub
        End If
        'For Issue List
        Dim Rpt As New crIssueList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList
        Dim Title As String = GetTitle()

        If cmbSearch.SelectedIndex = 0 Then
            'All
            SearchStr1 = "The report shows all records till date."
            SearchStr2 = ""
        ElseIf cmbSearch.SelectedIndex = 1 Then
            'Date
            SearchStr1 = "The report shows records filtered by the following criteria"
            If cmbDate.SelectedIndex = 0 Then
                SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text
            Else
                SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(FromDate_Txt.Text.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(ToDate_Txt.Text.ToString).FormattedText
            End If
        ElseIf cmbSearch.SelectedIndex = 2 Then
            'Issue No.
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbIssueText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        ElseIf cmbSearch.SelectedIndex = 3 Then
            'Part Number
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
        ElseIf cmbSearch.SelectedIndex = 4 Then
            'Aircraft
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
        ElseIf cmbSearch.SelectedIndex = 5 Then
            'Serial No.
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtNo.Text
        End If

        ReportDetails.Add(New rptStatus(, 0, ,
              gvIssueList.Columns.Item(1).HeaderText, gvIssueList.Columns.Item(2).HeaderText, gvIssueList.Columns.Item(3).HeaderText,
              gvIssueList.Columns.Item(4).HeaderText, gvIssueList.Columns.Item(5).HeaderText, gvIssueList.Columns.Item(6).HeaderText,
              gvIssueList.Columns.Item(7).HeaderText))

        Dim TotalCount As Integer
        Dim mCurrentPageindex As Integer = Me.gvIssueList.PageIndex 'Code Added
        TotalCount = Me.gvIssueList.PageCount
        Dim j As Integer
        Dim I As Integer
        Dim str(6) As String

        For j = 0 To TotalCount - 1

            Me.gvIssueList.PageIndex = j
            Me.gvIssueList.DataSource = mIssueList
            Session("mIssueList") = mIssueList
            gvIssueList.DataBind()
            For I = 0 To Me.gvIssueList.PageSize - 1
                If I <= gvIssueList.Rows.Count - 1 Then

                    str(0) = ""
                    str(1) = ""
                    str(2) = ""
                    str(3) = ""
                    str(4) = ""
                    str(5) = ""
                    str(6) = ""

                    If gvIssueList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = gvIssueList.Rows(I).Cells.Item(1).Text
                    If gvIssueList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = gvIssueList.Rows(I).Cells.Item(2).Text
                    If gvIssueList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = gvIssueList.Rows(I).Cells.Item(3).Text
                    If gvIssueList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = gvIssueList.Rows(I).Cells.Item(4).Text
                    If gvIssueList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = gvIssueList.Rows(I).Cells.Item(5).Text
                    If gvIssueList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = gvIssueList.Rows(I).Cells.Item(6).Text
                    If gvIssueList.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = gvIssueList.Rows(I).Cells.Item(7).Text


                    ReportDetails.Add(New rptStatus(, 1, , str(0), str(1), str(2), str(3), str(4), str(5), str(6)))
                End If
            Next
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Unused Issued Item List", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mIssueList.Count = 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            msg1.ReplacePage = "wfIssueListForUnusedReturn.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID
            ' msg1.ReplacePage = "wfIssueListForUnusedReturn.aspx?Backpage="
            msg1.Show()
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        Dim Str1 As String
        Str1 = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str1)

        gvIssueList.PageIndex = mCurrentPageindex
        gvIssueList.DataSource = mIssueList
        Session("mIssueList") = mIssueList
        gvIssueList.DataBind()
    End Sub

#End Region

#End Region

End Class

