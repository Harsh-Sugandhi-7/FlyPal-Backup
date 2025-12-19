'AJAX Conversion By Vikrant On 20-Aug-2014

Public Class wfRequisitionList_Ajax
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
    End Enum
#End Region

#Region " Variable Declaration "
    Public mRequisitionListNew As RequisitionListNew
    Public mRequisitionNew As RequisitionNew
    Public mDistinctTextListForRequisition As DistinctTextListForRequisition
    Public mLocationList As LocationList
    Dim TransTypeID, ReqTypeID As Integer
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, RequisitionText, PartNoSearchForRequisition, DescriptionSearchForRequisition, No, Location, SearchText As String  ''SearchText  added by Ajay on 13-Jan-23
    Dim EventLogID As Guid
    Dim mRequisitionDetail As String
    ''Dim mTransactionListCount As TransactionListCount 'Added By Shweta On 19-August-2013 for ALL16082013-1
    Dim mModuleName As String = String.Empty 'All13082014
    Dim LocationIDSearchForRequisition As Guid = Guid.Empty
    Public mToSetLocationList As LocationList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mRequisitionNew = Session("mRequisitionNew")
        mRequisitionListNew = Session("mRequisitionListNew")
        mDistinctTextListForRequisition = Session("mDistinctTextListForRequisition")
        mLocationList = Session("mLocationList")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        ReqTypeID = Session("ReqTypeID")
        RequisitionText = Session("RequisitionText")
        PartNoSearchForRequisition = Session("PartNoSearchForRequisition")
        Location = Session("Location")
        LocationIDSearchForRequisition = Session("LocationIDSearchForRequisition")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        TransTypeID = CInt(Session("TransTypeID"))
        mModuleName = Session("mModuleName") 'All13082014
        DescriptionSearchForRequisition = Session("DescriptionSearchForRequisition")
        SearchText = Session("SearchText") 'Ajay 10-Jan-2023
    End Sub
    Private Sub SetSession()
        Session("mRequisitionNew") = mRequisitionNew
        Session("mRequisitionListNew") = mRequisitionListNew
        Session("mDistinctTextListForRequisition") = mDistinctTextListForRequisition
        Session("mLocationList") = mLocationList
        Session("TransTypeID") = TransTypeID
        Session("mModuleName") = mModuleName
        SearchText = Session("SearchText") 'Ajay 11-Jan-2023
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mRequisitionNew")
        Session.Remove("mRequisitionListNew")
        Session.Remove("mDistinctTextListForRequisition")
        Session.Remove("mLocationList")
        Session.Remove("mModuleName") 'All13082014
        Session.Remove("DescriptionSearchForRequisition")
        Session.Remove("SearchText") 'Ajay 10-Jan-2023
    End Sub
    Private Sub ClearAll()
        TransTypeID = Session("TransTypeID")
        If Session("MiddleFrame") <> "wfRequisitionList_Ajax.aspx?TransTypeID=" & TransTypeID Then
            Session.Remove("mRequisitionNew")
            Session.Remove("mRequisitionListNew")
            Session.Remove("mDistinctTextListForRequisition")
            Session.Remove("mLocationList")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("StatusId")
            Session.Remove("ReqTypeID")
            Session.Remove("RequisitionText")
            Session.Remove("PartNoSearchForRequisition")
            Session.Remove("No")
            Session.Remove("Location")
            Session.Remove("LocationIDSearchForRequisition")
            Session.Remove("BackPage")
            Session.Remove("mModuleName") 'All13082014
            Session.Remove("DescriptionSearchForRequisition")
        End If
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgRequisitionList.DataBind()
        'cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        cmbStatus.SelectedValue = StatusId
        cmbType.SelectedValue = ReqTypeID
        If cmbRequisitionText.Items.Contains(New System.Web.UI.WebControls.ListItem(RequisitionText)) Then 'Added By Rajnish On 01-01-2008
            cmbRequisitionText.SelectedValue = RequisitionText
        Else
            cmbRequisitionText.SelectedValue = "(All)"
        End If
        cmbRequisitionLocation.SelectedValue = LocationIDSearchForRequisition.ToString
        'Try
        '    cmbRequisitionLocation.SelectedValue = IIf(Location = "", "(SELECT)", Location)
        'Catch ex As Exception
        '    '
        'End Try
        txtPartNoSearch.Text = PartNoSearchForRequisition
        txtDescriptionSearch.Text = DescriptionSearchForRequisition
        txtNo.Text = No
        ControlVisibility(SearchIndex, DateIndex)
        ''lblResult.Text = "List of " & mModuleName & " as per criteria :" & mRequisitionListNew.Count & " Record(s) found."
        lblResult.Text = "As per criteria :" & mRequisitionListNew.Count & " Record(s) found."
        'Ajay 11-Jan-2023
        If Not SearchText Is Nothing Then
            SearchText = IIf(SearchText = "", "", SearchText)
        Else
            SearchText = ""
        End If
    End Sub
    Private Sub NewRecord()
        mRequisitionNew = RequisitionNew.NewRequisition(TransTypeID)
        mRequisitionNew.ReqDate = Today.Date
        If AppSettings("ClientCode") = "CE" Then ''Added By Prashant on 18-May-2022 CE1852022
            mToSetLocationList = LocationList.GetLocationsList(0, , , , , , )
            If mToSetLocationList.Count > 0 Then
                mToSetLocationList.Sort("Name", ComponentModel.ListSortDirection.Descending) ''To show default location as “Muharaq”
                mRequisitionNew.LocationID = mToSetLocationList(0).ID
            End If
        End If
        Session("mRequisitionNew") = mRequisitionNew
        TransTypeID = Session("TransTypeID")
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mRequisitionNew = RequisitionNew.GetRequisition(mId)
        Dim child As RequisitionItemNew
        For Each child In mRequisitionNew.RequisitionItemsNew
            If child.ItemID.Equals(Guid.Empty) Then
                ' ''partno id .....
                ' ''child.ItemID = Guid.NewGuid
                ' ''child.Save()
            End If
        Next
        mRequisitionNew.MarkClean()
        Session("mRequisitionNew") = mRequisitionNew
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mRequisitionNew = RequisitionNew.GetRequisition(mId)
        Session("mRequisitionNew") = mRequisitionNew
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
                            Dim mRequisitionNew As RequisitionNew
                            Session("Sender") = ""
                            mRequisitionNew = CType(Session("mRequisitionNew"), RequisitionNew)
                            'mRequisitionNew.DeleteRequisition(mRequisition.ID)
                            mRequisitionNew.Delete()
                            mRequisitionNew.Save()
                            DataFieldBind()
                            SetControl()
                            ControlEnability()
                            upnlTitle.Update()
                            upnlGrid.Update()
                            ''upnlActionBtnBottom.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                mRequisitionDetail = mRequisitionNew.RequisitionNo + " Dated : " + mRequisitionNew.ReqDateFormatted + " Requested By : " + mRequisitionNew.EmployeeName + " Status : " + IIf(mRequisitionNew.StatusID = 1, "Open", "Authorized")
                                MarkLog(Util.Action.Delete, mModuleName, mRequisitionDetail, Util.ErrorType.NoError, mRequisitionNew.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub FindNow(Optional ByVal ItemName As String = "", Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, _
                        Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal StatusID As Integer = 0, _
                        Optional ByVal RequestingLocation As String = "", Optional ByVal Aircraft As String = "", Optional ByVal Employee As String = "", _
                        Optional ByVal LocationID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal ReqTypeID As Integer = 0, _
                        Optional ByVal Description As String = "", Optional ByVal SearchText As String = "") 'Ajay SearchText 10-Jan-2023
        mRequisitionListNew = Nothing
        dgRequisitionList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mRequisitionListNew = RequisitionListNew.GetRequisitionList(ItemName, Text, No, FromDate, ToDate, StatusID, RequestingLocation, Employee, _
                                                                    LocationID, Aircraft, ReqTypeID, TransTypeID, Description:=Description, SearchText:=SearchText)
        'Set DataSource of the Grid
        Session("mRequisitionListNew") = mRequisitionListNew
        dgRequisitionList.DataSource = mRequisitionListNew
        ''lblResult.Text = "List of " & mModuleName & " as per criteria :" & mRequisitionListNew.Count & " Record(s) found."
        lblResult.Text = "As per criteria :" & mRequisitionListNew.Count & " Record(s) found."
        dgRequisitionList.PageSize = CInt(cmbShowE.SelectedItem.ToString) 'Ajay 11-Jan-2022
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        FindNow(ItemName:=Trim(PartNoSearchForRequisition), Text:=Trim(RequisitionText), No:=CInt(Val(No)), FromDate:=txtFromDate.Text.Trim, ToDate:=txtToDate.Text.Trim, _
                StatusID:=CInt(StatusId), RequestingLocation:=Trim(Location), ReqTypeID:=CInt(ReqTypeID), Description:=Trim(DescriptionSearchForRequisition), SearchText:=txtSearchBox.Text.Trim)
        'Select Case Index
        '    Case -1
        '        Call FindNow() 'for all records
        '    Case 0  'all
        '        Call FindNow() 'for all records
        '    Case 1 'Requisition date
        '        Call FindNow("", "", 0, txtFromDate.Text, txtToDate.Text)
        '    Case 2  'Requisition Text , No 
        '        Call FindNow("", RequisitionText, CInt(Val(No)), FromDate, ToDate)
        '    Case 3 'Location
        '        Call FindNow("", "", 0, FromDate, ToDate, 0, Location)
        '    Case 4 ' Status
        '        Call FindNow("", "", 0, FromDate, ToDate, StatusId, "")
        '    Case 5 'Part No
        '        Call FindNow(PartNoSearchForRequisition, "", 0, FromDate, ToDate, 0, Location, "", "", Guid.Empty.ToString)
        '    Case 6 'Description
        '        Call FindNow("", "", 0, FromDate, ToDate, 0, "", "", "", Guid.Empty.ToString, Description:=PartNoSearchForRequisition)
        '    Case 7 'Type
        '        Call FindNow("", "", 0, FromDate, ToDate, 0, "", "", "", Guid.Empty.ToString, ReqTypeID)
        'End Select
        dgRequisitionList.PageIndex = 0
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
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
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date)
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date)
                txtFromDate.Text = CDate(FromDate).ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate(ToDate).ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        ''cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        lblFromDate.Visible = CBool(IIf(DateIndex <> 0, True, False))
        lblToDate.Visible = CBool(IIf(DateIndex <> 0, True, False))
        If DateIndex = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
        ''cmbRequisitionText.Visible = IIf(SearchIndex = 2, True, False)
        ''lblNo.Visible = IIf(SearchIndex = 2 And cmbRequisitionText.SelectedIndex <> 0, True, False)
        ''txtNo.Visible = IIf(SearchIndex = 2 And cmbRequisitionText.SelectedIndex <> 0, True, False)
        ''cmbRequisitionLocation.Visible = IIf(SearchIndex = 3, True, False)
        ''cmbStatus.Visible = IIf(SearchIndex = 4, True, False)
        ''txtPartNoSearch.Visible = IIf(SearchIndex = 5 Or SearchIndex = 6, True, False)
        ''cmbType.Visible = IIf(SearchIndex = 7, True, False)
    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
        txtPartNoSearch.Text = ""
    End Sub
    Private Sub setVariables()
        ''SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        RequisitionText = IIf(cmbRequisitionText.SelectedIndex <= 0, "", cmbRequisitionText.SelectedValue)
        Location = IIf(cmbRequisitionLocation.SelectedIndex > 0, cmbRequisitionLocation.SelectedItem.Text, "")
        LocationIDSearchForRequisition = IIf(cmbRequisitionLocation.SelectedIndex > 0, New Guid(cmbRequisitionLocation.SelectedValue), Guid.Empty)
        PartNoSearchForRequisition = txtPartNoSearch.Text.Trim
        DescriptionSearchForRequisition = txtDescriptionSearch.Text.Trim
        No = txtNo.Text.Trim
        ReqTypeID = IIf(cmbType.SelectedIndex <= 0, 0, cmbType.SelectedValue)
        SearchText = IIf(txtSearchBox.Text = "", "", txtSearchBox.Text) 'Ajay 11-01-2023

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
        Session("ReqTypeID") = ReqTypeID
        Session("RequisitionText") = RequisitionText
        Session("Location") = Location
        Session("LocationIDSearchForRequisition") = LocationIDSearchForRequisition
        Session("No") = No
        Session("PartNoSearchForRequisition") = PartNoSearchForRequisition
        Session("DescriptionSearchForRequisition") = DescriptionSearchForRequisition
        Session("SearchText") = SearchText 'Ajay 11-01-2023
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub ControlEnability()
        ''  BtnPrint.Enabled = IIf(dgRequisitionList.Rows.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(dgRequisitionList.Rows.Count = 0, False, True)
    End Sub
    Private Sub ControlVisibility()
        If TransTypeID = Util.Trans.EngineeringRequisition Or TransTypeID = Util.Trans.WorkShopRequisition Then
            dgRequisitionList.Columns(5).Visible = True
        Else
            dgRequisitionList.Columns(5).Visible = False
        End If
        If TransTypeID = Util.Trans.EngineeringRequisition Or TransTypeID = Util.Trans.WorkShopRequisition Then
            ''cmbSearch.Items.Add(New ListItem("Type", "7"))
            lblType.Visible = True
            cmbType.Visible = True
        End If
        dgRequisitionList.Columns(4).Visible = IIf(TransTypeID = Util.Trans.EngineeringRequisition Or _
                                                   TransTypeID = Util.Trans.WorkShopRequisition Or _
                                                   TransTypeID = Util.Trans.PlanningRequisition, True, False)
        dgRequisitionList.Columns(3).Visible = IIf(TransTypeID = Util.Trans.WorkShopRequisition, True, False)
        If TransTypeID = Util.Trans.StoresRequisition Then 'Added By Prashant 5-Aug-2020 All05082020
            If AppSettings("ClientCode") = "Heligo" Then 'Added By Prashant 4-Mar-2021
                dgRequisitionList.Columns(12).Visible = True
                lblGreen.Visible = True
                lblGreenInfo.Visible = True
                lblYellow.Visible = True
                lblYellowInfo.Visible = True
                lblOrange.Visible = True
                lblOrangeInfo.Visible = True
            Else
                dgRequisitionList.Columns(12).Visible = False
                lblGreen.Visible = False
                lblGreenInfo.Visible = False
            End If
            If AppSettings("ClientCode") = "STR" Then 'Added By Prashant 4-Mar-2021 StarAir04032021
                lnkCreateRequisition.Visible = True
            End If
            dgRequisitionList.Columns(10).Visible = False
        End If
        dgRequisitionList.Columns(10).Visible = IIf(TransTypeID = Util.Trans.StoresRequisition Or _
                                                   TransTypeID = Util.Trans.WorkShopRequisition, False, True)
        'Added by Shital on 01-March-2021
        If TransTypeID = Util.Trans.PlanningRequisition Or TransTypeID = Util.Trans.EngineeringRequisition Or TransTypeID = Util.Trans.WorkShopRequisition Then
            lblYellow.Visible = True
            lblYellowInfo.Visible = True
            lblOrange.Visible = True
            lblOrangeInfo.Visible = True
        End If
        '---------
        txtSearchBox.Visible = True 'Ajay 11-Jan-2023
    End Sub
    'All13082014
    Private Sub SetValuesByRequisitionType()
        If TransTypeID = Util.Trans.EngineeringRequisition Then
            mModuleName = "Engineering Requisition"

            If AppSettings("ClientCode") = "IND" Then
                mModuleName = "Spares Requisition"
            End If
        ElseIf TransTypeID = Util.Trans.StoresRequisition Then
            mModuleName = "Stores Requisition"
        ElseIf TransTypeID = Util.Trans.WorkShopRequisition Then
            mModuleName = "WorkShop Requisition"
        ElseIf TransTypeID = Util.Trans.PlanningRequisition Then
            mModuleName = "Planning Requisition"
        End If
        Session("mModuleName") = mModuleName
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""

        'Deciding IsInRole String to check Rights
        'Select Case OrderType
        Select Case TransTypeID
            Case Util.Trans.EngineeringRequisition
                IsInRoleString = "EngineeringRequisition"
            Case Util.Trans.StoresRequisition
                IsInRoleString = "StoresRequisition"
            Case Util.Trans.WorkShopRequisition
                IsInRoleString = "WorkShopRequisition"
            Case Util.Trans.PlanningRequisition
                IsInRoleString = "PlanningRequisition"
        End Select
        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.FindNow
                Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
        End Select
    End Function
    'End
    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs) 'Added By Prashant 5-Aug-2020 All05082020
        ''If e.Row.RowType = DataControlRowType.DataRow Then
        ''    Added by Prashant Heligo21092021 17 ReqTransTypeID 18 SumOfReceiptBalanceQty 19  SumRequestedQty if Receipt is created then green 
        ''    If order is crated against plaining Req/Store req and receipt is crated against this order then show green Colour
        ''        If (CDbl(e.Row.Cells(18).Text) >= CDbl(e.Row.Cells(19).Text) And (CInt(e.Row.Cells(17).Text) = 71 Or CInt(e.Row.Cells(17).Text) = 77) And AppSettings("ClientCode") = "Heligo") Then
        ''            e.Row.Cells(13).BackColor = Color.Green
        ''        ElseIf (CDbl(e.Row.Cells(14).Text) <= 0.0) Then
        ''            e.Row.Cells(13).BackColor = Color.Green
        ''        ElseIf ((e.Row.Cells(15).Text) = 1) And ((e.Row.Cells(16).Text) = 2 Or (e.Row.Cells(16).Text) = 0) Then '1 Completed Order And (TransTypeID = 77) Then '77 Commneted by Prashant on 12-Apr-2021 As Need to show these clours to Engg, Work Shop Req. 
        ''            e.Row.Cells(13).BackColor = Color.Yellow
        ''        ElseIf ((e.Row.Cells(15).Text) = 2) And ((e.Row.Cells(16).Text) = 2 Or (e.Row.Cells(16).Text) = 0) Then '2 Partial Order And (TransTypeID = 77) Then
        ''            e.Row.Cells(13).BackColor = Color.Orange
        ''        ElseIf ((e.Row.Cells(15).Text) = 3) And ((e.Row.Cells(16).Text) = 2 Or (e.Row.Cells(16).Text) = 0) Then '3 Order not Created against Req. And (TransTypeID = 77) Then
        ''            e.Row.Cells(13).BackColor = Color.White
        ''        End If
        ''    End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Added by Prashant Heligo21092021 17 ReqTransTypeID 18 SumOfReceiptBalanceQty 19  SumRequestedQty if Receipt is created then green 
            'If order is crated against plaining Req and receipt is crated against this order then show green Colour
            If (CDbl(e.Row.Cells(17).Text) >= CDbl(e.Row.Cells(18).Text) And (CInt(e.Row.Cells(16).Text) = 71 Or CInt(e.Row.Cells(16).Text) = 77) And AppSettings("ClientCode") = "Heligo") Then
                e.Row.Cells(12).BackColor = Color.Green
            ElseIf (CDbl(e.Row.Cells(13).Text) <= 0.0) Then
                e.Row.Cells(12).BackColor = Color.Green
            ElseIf ((e.Row.Cells(14).Text) = 1) And ((e.Row.Cells(15).Text) = 2 Or (e.Row.Cells(15).Text) = 0) Then '1 Completed Order And (TransTypeID = 77) Then '77 Commneted by Prashant on 12-Apr-2021 As Need to show these clours to Engg, Work Shop Req. 
                e.Row.Cells(12).BackColor = Color.Yellow
            ElseIf ((e.Row.Cells(14).Text) = 2) And ((e.Row.Cells(15).Text) = 2 Or (e.Row.Cells(15).Text) = 0) Then '2 Partial Order And (TransTypeID = 77) Then
                e.Row.Cells(12).BackColor = Color.Orange
            ElseIf ((e.Row.Cells(14).Text) = 3) And ((e.Row.Cells(15).Text) = 2 Or (e.Row.Cells(15).Text) = 0) Then '3 Order not Created against Req. And (TransTypeID = 77) Then
                e.Row.Cells(12).BackColor = Color.White

            End If
        End If
    End Sub
#End Region

#Region " DatafieldBinding "
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        'Commented and added by Shweta on 19-August-2013 for ALL16082013-1
        'DateIndex = IIf(IsNothing(DateIndex), 2, DateIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        'end
        StatusId = Session("StatusId")
        ReqTypeID = Session("ReqTypeID")
        RequisitionText = Session("RequisitionText")
        PartNoSearchForRequisition = Session("PartNoSearchForRequisition")
        ''No = Session("No")
        Location = Session("Location")
        LocationIDSearchForRequisition = Session("LocationIDSearchForRequisition")
        TransTypeID = Session("TransTypeID")
        mDistinctTextListForRequisition = DistinctTextListForRequisition.GetDistinctTextList("16", , True, "(All)", TransTypeID)
        cmbRequisitionText.DataSource = mDistinctTextListForRequisition
        mLocationList = LocationList.GetLocationsList(0, , , , , , True, "(All)")
        cmbRequisitionLocation.DataSource = mLocationList
        Session("mLocationList") = mLocationList
        DataBind()
        'Commented and added By Shweta On 19-August-2013 for ALL16082013-1
        'mRequisitionListNew = RequisitionListNew.GetRequisitionList("", "", 0, "01/01/1900", "01/01/2050", 0, "", "", "{00000000-0000-0000-0000-000000000000}", "", ReqTypeID) 22-aug
        'dgRequisitionList.DataSource = mRequisitionListNew 22-aug
        'Session("mRequisitionListNew") = mRequisitionListNew 22-aug
        'lblResult.Text = "List of Requisition as per criteria :" & mRequisitionListNew.Count & " Record(s) found." 22-aug
        'mRequisitionListNew1 = RequisitionListNew.GetRequisitionList("", "") 
        ''mTransactionListCount = TransactionListCount.GetTransactionListCountt(TransTypeID)
        LblTitle.Text = "List of " & mModuleName ''& "(s) [Total No of Record(s):-" & mTransactionListCount(0).Count & "]"
        'End
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If cmbDate.Enabled = True Then
                cmbDate.Focus()
            End If
            cmbShowE.SelectedIndex = 4 'Ajay 18-Jan-2023
            TransTypeID = CInt(Request.QueryString("TransTypeID"))
            Session("TransTypeID") = TransTypeID
            Session("MiddleFrame") = "wfRequisitionList_Ajax.aspx?TransTypeID=" & TransTypeID
            SetValuesByRequisitionType()
            DataFieldBind()
            SetControl()
            ControlEnability()
            ControlVisibility()

        End If
    End Sub
    Private Sub dgRequisitionList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRequisitionList.RowCommand
        Dim mId As New Guid
        Dim Idx As Int32
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '' Idx = CInt(e.CommandArgument) 'Commented by Ajay on 13-Jan-2023
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay on 13-Jan-2023
                Idx = gvr.RowIndex 'Ajay on 13-Jan-2023
                mId = New Guid(dgRequisitionList.DataKeys(Idx).Value.ToString)
                EditRecord(mId)

                mRequisitionDetail = mRequisitionNew.RequisitionNo + " Dated : " + mRequisitionNew.ReqDateFormatted + " Requested By : " + mRequisitionNew.EmployeeName + " Status : " + IIf(mRequisitionNew.StatusID = 1, "Open", "Authorized")
                MarkLog(Util.Action.Edit, mModuleName, mRequisitionDetail, Util.ErrorType.NoError, mId, EventLogID)

                Dim str As String
                str = "openledgersame('wfRequisition_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '' Idx = CInt(e.CommandArgument) 'Commented by Ajay on 13-Jan-2023
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay on 13-Jan-2023
                Idx = gvr.RowIndex 'Ajay on 13-Jan-2023
                mId = New Guid(dgRequisitionList.DataKeys(Idx).Value.ToString)
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub dgRequisitionList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgRequisitionList.PageIndexChanging
        dgRequisitionList.PageIndex = e.NewPageIndex
        dgRequisitionList.DataSource = mRequisitionListNew
        Session("mRequisitionListNew") = mRequisitionListNew
        dgRequisitionList.DataBind()
        dgRequisitionList.PageSize = CInt(cmbShowE.SelectedItem.ToString) 'Ajay 11-Jan-2023
    End Sub
    'Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
    '    cmbDate.SelectedIndex = 0
    '    cmbRequisitionText.SelectedIndex = 0
    '    cmbRequisitionLocation.SelectedIndex = 0
    '    cmbStatus.SelectedIndex = 0
    '    cmbType.SelectedIndex = 0
    '    ClearControls()
    '    Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
    '    ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
    '    setPeriod(DateIndex)
    '    If cmbSearch.Enabled = True Then
    '        cmbSearch.Focus()
    '    End If
    'End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged, cmbRequisitionText.SelectedIndexChanged
        If sender.id = "cmbDate" Then
            ''ClearControls()
            ''Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
            Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
            ControlVisibility(1, DateIndex)
            setPeriod(DateIndex)
            If cmbDate.Enabled = True Then
                cmbDate.Focus()
            End If
        ElseIf sender.id = "cmbRequisitionText" Then
            txtNo.Text = "0"
            ''Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
           If cmbRequisitionText.Enabled = True Then
                cmbRequisitionText.Focus()
            End If
        End If
    End Sub
    Private Sub imgFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgFindNow.Click  ''btnFindNow.Click
        setVariables()
        CallFindNow(SearchIndex)
        dgRequisitionList.DataBind()
        ControlEnability()
        ''lblResult.Text = "List of " & mModuleName & " as per criteria :" & mRequisitionListNew.Count & " Record(s) found."
        lblResult.Text = "As per criteria :" & mRequisitionListNew.Count & " Record(s) found."
        upnlGrid.Update()
        '' upnlActionBtnBottom.Update()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click  '',btnAddNew.Click
        If (Not IsInRole(Rights.New)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        NewRecord()
        MarkLog(Util.Action.[New], mModuleName, "", Util.ErrorType.NoError, mRequisitionNew.ID, EventLogID)
        Dim str As String
        str = "openledgersame('wfRequisition_Ajax.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click ''btnClose.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgRequisitionList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRequisitionList.Sorting
        mRequisitionListNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRequisitionListNew") = mRequisitionListNew
        dgRequisitionList.DataSource = mRequisitionListNew
        dgRequisitionList.DataBind()
    End Sub
    Private Sub btnPrintTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click '', BtnPrint.Click
        If (Not IsInRole(Rights.Print)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mCompanyDetail As New CompanyDetail
        Dim SearchStr1 As String = ""
        Dim SearchStr2 As String = ""
        Dim Rpt As New crRequisitionNewList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList
        SearchStr1 = ""
        SearchStr2 = ""
        'If cmbSearch.SelectedIndex = 0 Then
        '    'All
        '    SearchStr1 = "The report shows all records till date."
        '    SearchStr2 = ""
        'ElseIf cmbSearch.SelectedIndex = 1 Then
        '    'Date
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    If cmbDate.SelectedIndex = 0 Then
        '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text
        '    ElseIf cmbDate.SelectedIndex = 6 Then
        '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Text.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Text.ToString).FormattedText
        '    Else
        '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Text.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Text.ToString).FormattedText
        '    End If
        'ElseIf cmbSearch.SelectedIndex = 2 Then
        '    'Requisition No.
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbRequisitionText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearch.SelectedIndex = 3 Then
        '    'Requisition Location.
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbRequisitionLocation.SelectedItem.Text
        'ElseIf cmbSearch.SelectedIndex = 4 Then
        '    'Status
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text
        'ElseIf cmbSearch.SelectedIndex = 5 Then
        '    'Part No.
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtPartNoSearch.Text
        'ElseIf cmbSearch.SelectedIndex = 6 Then
        '    'Description.
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtPartNoSearch.Text
        'ElseIf cmbSearch.SelectedIndex = 7 Then
        '    'Type
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbType.SelectedItem.Text
        'End If

        ReportDetails.Add(New rptStatus(, 0, , _
              dgRequisitionList.Columns.Item(1).HeaderText, dgRequisitionList.Columns.Item(2).HeaderText, dgRequisitionList.Columns.Item(3).HeaderText, _
              dgRequisitionList.Columns.Item(4).HeaderText, dgRequisitionList.Columns.Item(5).HeaderText, dgRequisitionList.Columns.Item(6).HeaderText, _
              dgRequisitionList.Columns.Item(7).HeaderText))

        Dim TotalCount As Integer
        Dim mCurrentPageindex As Integer = Me.dgRequisitionList.PageIndex
        TotalCount = Me.dgRequisitionList.PageCount
        Dim j As Integer
        Dim I As Integer
        Dim str(6) As String

        For j = 0 To TotalCount - 1

            Me.dgRequisitionList.PageIndex = j
            Me.dgRequisitionList.DataSource = mRequisitionListNew
            Session("mRequisitionListNew") = mRequisitionListNew
            dgRequisitionList.DataBind()
            For I = 0 To Me.dgRequisitionList.PageSize - 1
                If I <= Me.dgRequisitionList.Rows.Count - 1 Then

                    str(0) = ""
                    str(1) = ""
                    str(2) = ""
                    str(3) = ""
                    str(4) = ""
                    str(5) = ""
                    str(6) = ""

                    If Me.dgRequisitionList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgRequisitionList.Rows(I).Cells.Item(1).Text
                    If Me.dgRequisitionList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgRequisitionList.Rows(I).Cells.Item(2).Text
                    If Me.dgRequisitionList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgRequisitionList.Rows(I).Cells.Item(3).Text
                    If Me.dgRequisitionList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgRequisitionList.Rows(I).Cells.Item(4).Text
                    If Me.dgRequisitionList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgRequisitionList.Rows(I).Cells.Item(5).Text
                    If Me.dgRequisitionList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgRequisitionList.Rows(I).Cells.Item(6).Text
                    If Me.dgRequisitionList.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgRequisitionList.Rows(I).Cells.Item(7).Text

                    ReportDetails.Add(New rptStatus(, 1, , str(0), str(1), str(2), str(3), str(4), str(5), str(6)))
                End If
            Next
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, mModuleName & " List Report", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)

        Me.dgRequisitionList.PageIndex = mCurrentPageindex
        Me.dgRequisitionList.DataSource = mRequisitionListNew
        Session("mRequisitionListNew") = mRequisitionListNew
        dgRequisitionList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub lnkCreateRequisition_Click(sender As Object, e As System.EventArgs) Handles lnkCreateRequisition.Click 'Added by Prashant 5-Mar-2021 StarAir04032021
        If (Not IsInRole(Rights.New)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mRequisitionItemListNew As RequisitionItemListNew
        mRequisitionNew = RequisitionNew.NewRequisition(Trans.StoresRequisition)
        mRequisitionNew.ReqDate = Today.Date
        mRequisitionItemListNew = RequisitionItemListNew.GetRequisitionItemList(PartNo:="", IsReOrderLevelItemsRequired:=2, _
                                                                                IsBAReorderQtyFormulaRequired:=False)

        '-----------------------Req Item--------------
        For t As Integer = 0 To mRequisitionItemListNew.Count - 1
            'If mRequisitionItemNew.IsSelect Then
            If Not mRequisitionNew.RequisitionItemsNew.Contains(mRequisitionItemListNew(t).ItemID) Then
                mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, Guid.Empty)
                With mRequisitionNew.RequisitionItemsNew.CurrentItem
                    .ItemID = mRequisitionItemListNew(t).ItemID
                    .PartNo = mRequisitionItemListNew(t).PartNo
                    .Description = mRequisitionItemListNew(t).Description
                    .Unit = mRequisitionItemListNew(t).Unit
                    .UnitID = mRequisitionItemListNew(t).UnitID
                    .RequestedQty = mRequisitionItemListNew(t).ReOrderQty
                End With
            End If
        Next
        '---------------------------------------------
        Session("mRequisitionNew") = mRequisitionNew
        TransTypeID = Session("TransTypeID")
        'Session("AddReOrderParts") = "True"
        MarkLog(Util.Action.[New], mModuleName, "", Util.ErrorType.NoError, mRequisitionNew.ID, EventLogID)


        Dim str As String
        str = "openledgersame('wfRequisition_Ajax.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    'Ajay 11-Jan-2023
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Dim ExpiryDateList = ((From res In mWOList).ToList.Take(CInt(DropDownList1.SelectedItem.ToString))).ToList
        dgRequisitionList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        dgRequisitionList.DataSource = mRequisitionListNew
        dgRequisitionList.DataBind()
        'dgEmployeeList.PageIndex = e.OnSelectedIndexChanged
        ' DataBind()

        'SetGrid()
        'GridColumnsVisibility()
        'upnlGridView.Update()
        'upnlResult.Update()
        ControlVisibility(0)
        setVariables()
        SetControl()
        ControlEnability()
        ControlVisibility()
        upnlGrid.Update()
    End Sub
    'Ajay 11-Jan-2023
    Private Sub txtSearchBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchBox.TextChanged
        ControlVisibility(0)
        setVariables()
        CallFindNow(SearchIndex)
        dgRequisitionList.DataBind()

        SetControl()
        ControlEnability()
        ControlVisibility()
        upnlGrid.Update()
    End Sub
    '-----
#End Region

    
End Class