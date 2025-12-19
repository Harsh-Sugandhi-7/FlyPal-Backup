'Added by Utkarsh On 27-Jan-2014
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Public Class wfrptMachineCurrentStatusList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachineList As ListOfAircraftCurrentStatus  'Public mMachineList As tmpMachineList
    Public mModelList As ModelList
    Public Sortfield As String
    Public SortFlag As Boolean
    Public Idx As Integer
    Public SearchFor As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachineList = CType(Session("mMachinelist"), ListOfAircraftCurrentStatus) 'CType(Session("mMachinelist"), tmpMachineList)
        mModelList = Session("mModelList")
        SortFlag = CType(Session("SortFlag"), Boolean)
        Idx = CType(Session("Idx"), Integer)
        SearchFor = CType(Session("SearchFor"), String)
    End Sub
    Private Sub SetSession()
        Session("mMachineList") = mMachineList
        Session("SortFlag") = SortFlag
        Session("Idx") = Idx
        Session("SearchFor") = SearchFor
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineList")
        Session.Remove("SortFlag")
        Session.Remove("Idx")
        Session.Remove("SearchFor")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptMachineCurrentStatusList_Ajax.aspx?" Then
            Session.Remove("mMachineList")
            Session.Remove("mMachine")
            Session.Remove("SortFlag")
            Session.Remove("Idx")
            Session.Remove("SearchFor")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub FindNow(Optional ByVal mMachineCategoryName As String = "", _
                        Optional ByVal mRegNo As String = "", _
                        Optional ByVal mModelName As String = "{00000000-0000-0000-0000-000000000000}", _
                        Optional ByVal mManufacturerName As String = "", _
                        Optional ByVal mOwnerName As String = "", _
                        Optional ByVal mAddTopItem As String = "")

        'clear the obj and grid for new search
        gdvMachineList.DataSource = Nothing
        'get the new list

        'mMachineList = tmpMachineList.GetMachineList(mMachineCategoryName, mRegNo, mModelName, mManufacturerName, mOwnerName, mAddTopItem, True, txtDate.Value.ToString)
        mMachineList = ListOfAircraftCurrentStatus.GetListOfAircraftCurrentStatus(mMachineCategoryName, mRegNo, mModelName, mManufacturerName, mOwnerName, txtDate.Text.ToString)

        'bind the list to the grid
        gdvMachineList.DataSource = mMachineList
        gdvMachineList.DataBind()
        Session("mMachineList") = mMachineList
        lblResult.Text = "List of Aircraft as per criteria: " & mMachineList.Count & " Record(s) found."
        upnlGrid.Update()
    End Sub
    Private Sub CallFindNow(ByVal Index As Int32)
        Dim SearchText As String = Trim(txtFor.Text)
        Select Case Index
            Case 0  'All
                FindNow()
            Case 1  'RegNo.
                FindNow(, SearchText)

            Case 2  'Model Name
                Dim ModelID As String = Request.Form("cmbModel").ToString
                FindNow(, , ModelID)
            Case 3  'Manufacturer
                FindNow(, , , SearchText)
        End Select
        gdvMachineList.PageIndex = 0
    End Sub
    Private Sub DisplayControls(ByVal Index As Integer)
        txtFor.Visible = IIf(Index = 1 Or Index = 3, True, False)
        lblFor.Visible = IIf(Index <> 0, True, False)
        cmbModel.Visible = IIf(Index = 2 And (Index <> 0 Or Index <> 1 Or Index <> 3), True, False)
        upnlSearch.Update()
    End Sub
#End Region

#Region " DataBinding "
    Private Sub DatafieldBind()
        'mMachineList = tmpMachineList.GetMachineList(, , , , , , True, txtDate.Text)
        mModelList = ModelList.GetModelList(1, "", , , "(All)")
        Session("mModelList") = mModelList
        cmbModel.DataSource = mModelList
        FindNow()

        'DeVeN 17-06-2009
        'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
        If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL" Or AppSettings("ClientCode") = "BAL" Or AppSettings("ClientCode") = "SAP") Then
            'gdvMachineList.Columns.Item(9).HeaderText = "Flights"
            gdvMachineList.Columns(9).HeaderText = "Flights"
        Else
            gdvMachineList.Columns.Item(9).HeaderText = "Cycles"
        End If

        DataBind()
        If IsNothing(Session("Idx")) Then cmbLookIn.SelectedIndex = 0 Else cmbLookIn.SelectedIndex = Idx
        If IsNothing(Session("SearchFor")) Then txtFor.Text = "" Else txtFor.Text = CType(Session("SearchFor"), String)
        Session("Idx") = cmbLookIn.SelectedIndex
        DisplayControls(cmbLookIn.SelectedIndex)
        Session("SearchFor") = txtFor.Text

        'DeVeN 17-06-2009
        'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
        'If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL" Or AppSettings("ClientCode") = "BAL" Or AppSettings("ClientCode") = "SAP") Then
        '    gdvMachineList.Columns.Item(9).HeaderText = "Flights"
        'Else
        '    gdvMachineList.Columns.Item(9).HeaderText = "Cycles"
        'End If
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Utkarsh
        If Not IsPostBack And Session("sender") = "" Then
            If Sortfield = "" Then  REM:this is to set the sort Command
                Sortfield = "RegNo"
            End If
            If cmbLookIn.Enabled = True Then
                setFocus(cmbLookIn)
            End If
            txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Session("MiddleFrame") = "wfrptMachineCurrentStatusList_Ajax.aspx?"
            DatafieldBind()
            'Added By Vikrant On 26-Nov-2018 For APFT26112018
            If (Not AppSettings("ClientCode") Is Nothing) Then
                If (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                    txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout. License No.: __________ Date: _____________"
                ElseIf (AppSettings("ClientCode") = "Indamer") Then
                    txtBottomLine.Text = "Date:" + vbCrLf + vbCrLf + "Place:" + vbCrLf + vbCrLf + "Prepared By:                                                                                                      Checked By:                                                                                                                 Approved By:"
                ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Then 'Added By Vikrant On 14-March-2014 For All14032014
                    txtBottomLine.Text = "I hereby certify that the data specified above has been certified throughout : 									Technical Support Division: __________________ Date: _____________"
                ElseIf AppSettings("ClientCode") = "APFT" Or
                       AppSettings("ClientCode") = "AAP" Then 'Added By Saylee On 1-Oct-2018 
                    txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout. Continuing Airworthiness Manager: __________________ Date: _____________"
                End If
            Else
                txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout. Planning Manager: __________________ License No.: __________ Date: _____________"
            End If
            'End
            'Prashant 12-Dec-2022
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "AircraftCurrentStatus") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
            End If
            '--------------------------
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindNow.Click
        If Not IsValid Then Exit Sub
        Dim Index As Int32 = IIf(cmbLookIn.SelectedIndex > 0, cmbLookIn.SelectedIndex, 0)
        Session("Idx") = Index
        Session("SearchFor") = txtFor.Text
        CallFindNow(Index)

        If mMachineList.Count = 0 Then
            btnPrint.Enabled = False
        ElseIf mMachineList.Count > 0 Then
            btnPrint.Enabled = True
        End If
        DisplayControls(Index)
        upnlActionBtns.Update()

        'DeVeN 17-06-2009
        'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
        If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL" Or AppSettings("ClientCode") = "BAL" Or AppSettings("ClientCode") = "SAP") Then
            'gdvMachineList.Columns.Item(9).HeaderText = "Flights"
            gdvMachineList.Columns(9).HeaderText = "Flights"
        Else
            gdvMachineList.Columns.Item(9).HeaderText = "Cycles"
        End If
    End Sub
    Private Sub cmbLookIn_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookIn.SelectedIndexChanged
        Dim Index As Int32 = IIf(cmbLookIn.SelectedIndex > 0, cmbLookIn.SelectedIndex, 0)
        Session.Remove("SearchFor")
        txtFor.Text = ""
        If cmbLookIn.SelectedIndex <> 2 Then
            cmbModel.ClearSelection()
        End If
        DisplayControls(Index)
        If cmbLookIn.Enabled = True Then
            setFocus(cmbLookIn)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub gdvMachineList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvMachineList.PageIndexChanging
        gdvMachineList.PageIndex = e.NewPageIndex
        gdvMachineList.DataSource = mMachineList
        Session("mMachineList") = mMachineList
        gdvMachineList.DataBind()

        'DeVeN 17-06-2009
        'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
        If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then
            gdvMachineList.Columns(9).HeaderText = "Flights"
        Else
            gdvMachineList.Columns.Item(9).HeaderText = "Cycles"
        End If
    End Sub
    Private Sub gdvMachineList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvMachineList.Sorting
        Sortfield = e.SortExpression
        Dim Index As Int32 = IIf(cmbLookIn.SelectedIndex > 0, cmbLookIn.SelectedIndex, 0)
        'CallFindNow(Index)
        If SortFlag = 0 Then
            mMachineList.Sort("RegNo", System.ComponentModel.ListSortDirection.Descending)
            SortFlag = 1
            Session("SortFlag") = SortFlag
        Else
            mMachineList.Sort("RegNo", System.ComponentModel.ListSortDirection.Ascending)
            SortFlag = 0
            Session("SortFlag") = SortFlag
        End If
        'gdvMachineList.DataSource = mMachineList
        ' Session("mMachineList") = mMachineList
        'gdvMachineList.DataBind()

        'New addition by Rupali on 18-Jun-09 for Sorting Order
        mMachineList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMachineList") = mMachineList
        gdvMachineList.DataSource = mMachineList
        gdvMachineList.DataBind()
        lblResult.Text = "List of Aircraft as per criteria: " & mMachineList.Count & " Record(s) found."

        'DeVeN 17-06-2009
        'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
        If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then
            gdvMachineList.Columns(9).HeaderText = "Flights"
        Else
            gdvMachineList.Columns.Item(9).HeaderText = "Cycles"
        End If
    End Sub
    Protected Sub OnDataBound(sender As Object, e As EventArgs)
        If gdvMachineList.Rows.Count = 0 Then Exit Sub
        Dim j As Integer = gdvMachineList.Rows.Count - 1
        For i As Integer = gdvMachineList.Rows.Count - 1 To 1 Step -1
            Dim row As GridViewRow = gdvMachineList.Rows(i)
            Dim previousRow As GridViewRow = gdvMachineList.Rows(i - 1)
            If row.Cells(1).Text = previousRow.Cells(1).Text Then
                If previousRow.Cells(1).RowSpan = 0 Then
                    If row.Cells(1).RowSpan = 0 Then
                        previousRow.Cells(1).RowSpan += 2
                        previousRow.Cells(12).RowSpan += 2
                        If i = j Then 'i.e Last row bottom border
                            'Do nothing 
                        Else
                            'gdvMachineList.Rows(i).Attributes("style") = "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-color:rgb(128,0,64);"
                            'previousRow.Cells(1).Attributes("style") = "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-bottom-color:rgb(128,0,64); border-bottom-width: 3px;"

                            'gdvMachineList.Rows(i).Attributes.Add("style", "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-color:rgb(128,0,64);")
                            'previousRow.Cells(1).Attributes.Add("style", "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-bottom-color:rgb(128,0,64); border-bottom-width: 3px;")
                         End If
                    Else
                        previousRow.Cells(1).RowSpan = row.Cells(1).RowSpan + 1
                        previousRow.Cells(12).RowSpan = row.Cells(12).RowSpan + 1
                    End If
                    row.Cells(1).Visible = False
                    row.Cells(12).Visible = False
                End If
            Else
                Dim RegNoRecordsCount = Aggregate Machi In mMachineList
                          Where Machi.RegNo = row.Cells(1).Text
                          Into Count()
                If RegNoRecordsCount = 1 Then
                    'gdvMachineList.Rows(i).Attributes.Add("style", "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-color:rgb(128,0,64);")
                End If
            End If
        Next
    End Sub
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Prashant 12-Dec-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "AircraftCurrentStatus")
    End Sub
    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Prashant 12-Dec-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "AircraftCurrentStatus")
    End Sub
#End Region

#Region " Report "
    'Created By:- Jyoti
#Region " Report Variable "
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass

    Private SearchStr1 As String
    Private SearchStr2 As String
    Private SearchStr3 As String
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCurrentStatus
        Dim ReportDetails As New rptStatusList
        Rpt = New crptCurrentStatus
        mMachineList = Session("mMachineList")
        If cmbLookIn.SelectedIndex = 0 Then
            SearchStr1 = "The report shows all records till date."
            SearchStr2 = ""
        ElseIf cmbLookIn.SelectedIndex = 1 Or cmbLookIn.SelectedIndex = 2 Or cmbLookIn.SelectedIndex = 3 Then
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbLookIn.SelectedItem.Text + " " + ":" + " " + IIf(cmbLookIn.SelectedIndex = 2, cmbModel.SelectedItem.Text, txtFor.Text.Trim)
        End If
        SearchStr3 = "As On Date : " + " " + txtDate.Text.ToString

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                       mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                       mCompanyDetail.WebSite, "Aircraft Current Status Report", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", txtBottomLine.Text, AppSettings("Logo"))

        If mMachineList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 720)
        End If

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mMachineList)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        Dim EventLogDetail As String
        EventLogDetail = "Search : " & cmbLookIn.SelectedItem.Text & ", For : " & "" & ", Date : " & txtDate.Text.Trim
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "AircraftCurrentStatus", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    'Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Dim da As New CSLA.Data.ObjectAdapter
    '    Dim ds As New dsCommon
    '    Dim ReportDetails As New rptStatusList
    '    Rpt = New crListCurrentStatus

    '    If cmbLookIn.SelectedIndex = 0 Then
    '        SearchStr1 = "The report shows all records till date."
    '        SearchStr2 = ""
    '    ElseIf cmbLookIn.SelectedIndex = 1 Or cmbLookIn.SelectedIndex = 2 Or cmbLookIn.SelectedIndex = 3 Then
    '        SearchStr1 = "The report shows records filtered by the following criteria"
    '        SearchStr2 = "By" + " " + cmbLookIn.SelectedItem.Text + " " + ":" + " " + IIf(cmbLookIn.SelectedIndex = 2, cmbModel.SelectedItem.Text, txtFor.Text.Trim)
    '    End If
    '    SearchStr3 = "As On Date : " + " " + txtDate.Text.ToString

    '    ReportDetails.Add(New rptStatus(, 0, , _
    '          , , , gdvMachineList.Columns.Item(1).HeaderText, , gdvMachineList.Columns.Item(2).HeaderText, gdvMachineList.Columns.Item(3).HeaderText, _
    '          gdvMachineList.Columns.Item(4).HeaderText, gdvMachineList.Columns.Item(5).HeaderText, , gdvMachineList.Columns.Item(6).HeaderText, gdvMachineList.Columns.Item(7).HeaderText, _
    '            gdvMachineList.Columns.Item(8).HeaderText, gdvMachineList.Columns.Item(9).HeaderText, , gdvMachineList.Columns.Item(12).HeaderText, LHData13:=gdvMachineList.Columns.Item(11).HeaderText))  'Addition of 'gdvMachineList.Columns.Item(12).HeaderText' by Utkarsh on 25-Aug-2011 for 'ALL25082011

    '    'Dim TotalCount As Integer
    '    'TotalCount = Me.mMachineList.Count
    '    'Dim I As Integer

    '    'Dim str(8) As String

    '    'For I = 0 To TotalCount - 1

    '    Dim TotalCount As Integer
    '    Dim mCurrentPageindex As Integer = Me.gdvMachineList.PageIndex 'Code Added							
    '    TotalCount = Me.gdvMachineList.PageCount
    '    Dim j As Integer
    '    Dim I As Integer
    '    Dim str(10) As String

    '    For j = 0 To TotalCount - 1

    '        Me.gdvMachineList.PageIndex = j
    '        Me.gdvMachineList.DataSource = mMachineList
    '        Session("mMachineList") = mMachineList
    '        gdvMachineList.DataBind()

    '        'DeVeN 17-06-2009
    '        'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
    '        If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then
    '            gdvMachineList.Columns.Item(9).HeaderText = "Flights"
    '        Else
    '            gdvMachineList.Columns.Item(9).HeaderText = "Cycles"
    '        End If

    '        For I = 0 To Me.gdvMachineList.PageSize - 1
    '            If I <= Me.gdvMachineList.Rows.Count - 1 Then

    '                str(0) = ""
    '                str(1) = ""
    '                str(2) = ""
    '                str(3) = ""
    '                str(4) = ""
    '                str(5) = ""
    '                str(6) = ""
    '                str(7) = ""
    '                str(8) = ""
    '                str(9) = ""
    '                str(10) = ""
    '                If Me.gdvMachineList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.gdvMachineList.Rows(I).Cells.Item(1).Text
    '                If Me.gdvMachineList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.gdvMachineList.Rows(I).Cells.Item(2).Text
    '                If Me.gdvMachineList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.gdvMachineList.Rows(I).Cells.Item(3).Text
    '                If Me.gdvMachineList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.gdvMachineList.Rows(I).Cells.Item(4).Text
    '                If Me.gdvMachineList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.gdvMachineList.Rows(I).Cells.Item(5).Text
    '                If Me.gdvMachineList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.gdvMachineList.Rows(I).Cells.Item(6).Text
    '                If Me.gdvMachineList.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.gdvMachineList.Rows(I).Cells.Item(7).Text
    '                If Me.gdvMachineList.Rows(I).Cells.Item(8).Text <> "&nbsp;" Then str(7) = Me.gdvMachineList.Rows(I).Cells.Item(8).Text
    '                If Me.gdvMachineList.Rows(I).Cells.Item(9).Text <> "&nbsp;" Then str(8) = Me.gdvMachineList.Rows(I).Cells.Item(9).Text
    '                If Me.gdvMachineList.Rows(I).Cells.Item(11).Text <> "&nbsp;" Then str(9) = Me.gdvMachineList.Rows(I).Cells.Item(11).Text
    '                If Me.gdvMachineList.Rows(I).Cells.Item(12).Text <> "&nbsp;" Then str(10) = Me.gdvMachineList.Rows(I).Cells.Item(12).Text

    '                ReportDetails.Add(New rptStatus(, 1, , _
    '                 , , , , , , , , , , , _
    '           , , , str(8), , , str(9), str(10), str(0), str(3), str(1), str(4), str(2), , str(5), str(6), str(7)))
    '            End If
    '        Next
    '    Next

    '    mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

    '    Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
    '                   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
    '                   mCompanyDetail.WebSite, "Aircraft Current Status Report", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

    '    If mMachineList.Count = 0 Then
    '        MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
    '        Exit Sub
    '    Else
    '        RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 720)
    '    End If

    '    'ds.Clear()
    '    Dim mrptImage As rptImage = rptImage.GetImage(ds)
    '    da.Fill(ds, ReportDetails)
    '    da.Fill(ds, Report)
    '    da.Fill(ds, mrptImage)
    '    Rpt.SetDataSource(ds)
    '    Session("CrystalReport") = Rpt

    '    'MarkLog(Util.Action.View, "Report", "Aircraft Current Status Report", Util.ErrorType.NoError, Guid.Empty)

    '    Dim EventLogDetail As String
    '    Dim fortex As String = IIf(cmbLookIn.SelectedIndex = 2, cmbModel.SelectedItem.Text, txtFor.Text.Trim)
    '    EventLogDetail = "Search : " & cmbLookIn.SelectedItem.Text & ", For : " & fortex & ", Date : " & txtDate.Text.Trim
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    '    MarkLog(Util.Action.Print, "AircraftCurrentStatus", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    '    Me.gdvMachineList.PageIndex = mCurrentPageindex
    '    Me.gdvMachineList.DataSource = mMachineList
    '    Session("mMachineList") = mMachineList
    '    gdvMachineList.DataBind()



    '    'DeVeN 17-06-2009
    '    'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
    '    If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then
    '        gdvMachineList.Columns.Item(9).HeaderText = "Flights"
    '    Else
    '        gdvMachineList.Columns.Item(9).HeaderText = "Cycles"
    '    End If
    'End Sub
#End Region

#End Region
End Class