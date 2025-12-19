Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq
Imports System.Linq.Enumerable
Imports System
Imports System.IO
Imports System.Text

Public Class wfCustomerContractList_Ajax
    Inherits System.Web.UI.Page

    ''mTransTypeID = 94

#Region " Variable Declaration "
    Private mCustomerContract As CustomerContract
    Private mCustomerContractList As CustomerContractList
    Private mDistinctCustomerContractText As DistinctTextListAutoComplete
    Dim DateIndex, FromDate, ToDate, Text, StatusID, No, IsDateChecked As String
    Dim EventLogID As Guid
    Dim totcnt As Integer
    Dim mFileAttach As FileAttach
    ''  Dim mTransactionListCount As TransactionListCount
    Dim mStatusList As StatusList
    Protected mtmpVendorList As VendorList
    Dim CustomerID As String = Guid.Empty.ToString

    Public mEventLog As EventLog
    Public mUser As User

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mCustomerContract = Session("mCustomerContract")
        mCustomerContractList = Session("mCustomerContractList")
        mDistinctCustomerContractText = Session("mDistinctCustomerContractText")

        Text = Session("Text")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")

        DateIndex = Session("DateIndex")
        StatusID = Session("StatusID")

        '' mTransactionListCount = Session("mTransactionListCount")
        IsDateChecked = Session("IsDateChecked")
        CustomerID = Session("CustomerID")
        mtmpVendorList = Session("mtmpVendorList")
        mUser = Session("mUser")
    End Sub
    Private Sub SetSession()
        Session("mCustomerContract") = mCustomerContract
        Session("mCustomerContractList") = mCustomerContractList
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate

        Session("DateIndex") = DateIndex
        Session("StatusID") = StatusID

        Session("No") = No
      
        Session("Text") = Text
        Session("IsDateChecked") = IsDateChecked
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCustomerContract")
        Session.Remove("mCustomerContractList")

        Session.Remove("FromDate")
        Session.Remove("ToDate")

        Session.Remove("DateIndex")
        Session.Remove("StatusID")

        Session.Remove("No")
        Session.Remove("RegNo")
      
        Session.Remove("Text")

        ''    Session.Remove("mTransactionListCount")
        Session.Remove("IsDateChecked")
        Session.Remove("CustomerID")
        Session.Remove("mtmpVendorList")
       
        Session.Remove("mUser")

    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfCustomerContractList_Ajax.aspx") <= 0 Then
            RemoveSession()
            Session.Remove("mCustomerContractList")
            Session.Remove("IsPageLoadedForFirstTime")
            Session.Remove("mFileAttach")
        End If
    End Sub
    Private Sub addAttributes()
        'txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value)")
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub SetGrid()

        'btnAddNew.Visible = IIf(AppSettings("ClientCode") <> "A3S", True, False)
        btnAddNew.Visible = False

    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 'All'
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate
        End Select
    End Sub
    Private Sub setVariables()

        DateIndex = IIf(cmbDate.SelectedIndex <= 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

        Text = IIf(cmbContract.SelectedIndex <= 0, "", cmbContract.SelectedValue)

        StatusID = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)

      
        No = txtNo.Text.Trim
     
        IsDateChecked = chkDate.Checked
     
        CustomerID = IIf(cmbCustomer.SelectedIndex <= 0, Guid.Empty.ToString, cmbCustomer.SelectedValue)
     
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("DateIndex") = DateIndex
        Session("StatusID") = StatusID
        Session("No") = No
        Session("CustomerID") = CustomerID
        Session("Text") = Text

        Session("IsDateChecked") = IsDateChecked

    End Sub

    Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Int32 = 0, Optional ByVal FromDate As String = "", Optional ByVal ToDate As String = "", Optional ByVal StatusID As Integer = 0, Optional ByVal AddTopItem As String = "", Optional ByVal IsDateChecked As String = "", Optional ByVal CustomerID As String = "{00000000-0000-0000-0000-000000000000}")
        mCustomerContractList = Nothing
        dgContractList.DataSource = Nothing


        mCustomerContractList = CustomerContractList.GetCustomerContractList(FromDate, ToDate, Text, No, StatusID, CustomerID, , , Trans.CustomerContract)
        dgContractList.DataSource = mCustomerContractList
        Session("mCustomerContractList") = mCustomerContractList
    End Sub

    Private Sub SetControl()
        setPeriod(DateIndex)
          FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
        No = IIf(No Is Nothing, txtNo.Text.Trim, No)

        Text = IIf(Text Is Nothing, IIf(cmbContract.SelectedIndex <= 0, "", cmbContract.SelectedValue), Text)
      
        CustomerID = IIf(CustomerID Is Nothing, IIf(cmbCustomer.SelectedIndex <= 0, Guid.Empty.ToString, cmbCustomer.SelectedValue), CustomerID)

     
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("DateIndex") = DateIndex
        Session("StatusID") = StatusID


        Session("StatusId") = StatusID

        Session("No") = No
        Session("Text") = Text
        Session("IsDateChecked") = IsDateChecked
       
        Session("CustomerID") = CustomerID

        txtFromDate.Text = FromDate
        txtToDate.Text = ToDate
       

        txtFromDate.Text = FromDate
        txtToDate.Text = ToDate
       
        txtNo.Text = No

        cmbCustomer.SelectedValue = CustomerID
        cmbStatus.SelectedValue = StatusID

        If mDistinctCustomerContractText.Contains(Text) Then
            cmbContract.SelectedValue = IIf(Text = "", "(ALL)", Text)
        Else
            cmbContract.SelectedValue = "(ALL)"
        End If

        chkDate.Checked = IIf(IsDateChecked Is Nothing, True, IsDateChecked)
        txtNo.Text = No


        mUser = CType(Session("mUser"), User)
        mEventLog = Session("mEventLog")
        If mUser Is Nothing Then mUser = SI.UTILITY.User.GetUser(mEventLog.UserID)

      
        Session("mUser") = mUser

      
       

        FindNow(Text, Val(No), FromDate, ToDate, Val(StatusID), "", IIf(IsDateChecked Is Nothing, True, IsDateChecked), CustomerID)
        dgContractList.DataBind()

        cmbDate.SelectedIndex = DateIndex
        cmbContract.SelectedValue = IIf(Text = "", "(ALL)", Text)
        txtNo.Text = No


        ControlVisibility(DateIndex)
        dgContractList.DataBind()
        If mCustomerContractList.Count > 0 And mCustomerContractList.Count <> mCustomerContractList.TotalRecords Then
            lblResult.Text = "List of Contract(s) as per criteria : Recent " & mCustomerContractList.Count & " of " & mCustomerContractList.TotalRecords.ToString & " Record(s)."
        Else
            lblResult.Text = "List of Contract(s) as per criteria : " & mCustomerContractList.Count & " Record(s)."
        End If
    End Sub
    Private Sub ControlVisibility(Optional ByVal DateIndex As Int32 = 0)
        If DateIndex = 6 Then
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            lblFromDate.Visible = True
            lblToDate.Visible = True
        ElseIf (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
            txtFromDate.Visible = True
            txtToDate.Visible = True
            lblFromDate.Visible = True
            lblToDate.Visible = True
        ElseIf DateIndex = 0 Then
            txtFromDate.Visible = False
            txtToDate.Visible = False
            lblFromDate.Visible = False
            lblToDate.Visible = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If

    End Sub
    Private Sub SetTitle()
        lbltitle.InnerText = "List of Contract(s)  [Total No of Record(s):-" + mCustomerContractList.TotalRecords.ToString() + "]"
        upnltitle.Update()
    End Sub
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean) 'Added By Vikrant On 01-Dec-2014
        If mIsAttachemntAdded = True Then
            mFileAttach = FileAttach.GetAttachment(ID, 1) 'Sort = 1 - Removal
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim TextNo As String
                        Try
                            Dim mContract As CustomerContract
                            Session("sender") = ""
                            mContract = CType(Session("mCustomerContract"), CustomerContract)


                            TextNo = mContract.ContractNumber + " Dated : " + mContract.ContractDateFormatted.ToString
                            CustomerContract.DeleteCustomerContract(mContract.ID)
                          
                            DataFieldBind()
                            SetControl()
                            SetGrid()

                            upnlGridView.Update()
                            upnlGrid.Update()
                            upnlResult.Update()

                        Catch ex As SqlException
                            Dim UseInstr As String = String.Empty
                            If ex.Message.Contains("FKtabReqtabCWP") Then
                                UseInstr = "Requisition"
                            ElseIf ex.Message.Contains("FKtabMROInvoicetabCWP") Then
                                UseInstr = "Invoice"
                            End If
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, UseInstr, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "CustomerContract", "Can't delete : " + TextNo + " is Currently in use", Util.ErrorType.NoError, mCustomerContract.ID, EventLogID)
                            End If
                            DataFieldBind()
                            SetControl()
                            SetGrid()
                            upnlGrid.Update()
                            upnlResult.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DeletedSuccessFully, MSGBox.Message_text.DeletedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "CustomerContract", TextNo, Util.ErrorType.NoError, mCustomerContract.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
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









#End Region

#Region "DataFieldBind"
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)

        mDistinctCustomerContractText = mDistinctCustomerContractText.GetDistinctTextList(IsForText:=True, TransTypeID:=94, TagText:="(ALL)")
        cmbContract.DataSource = mDistinctCustomerContractText
        Session("mDistinctCustomerContractText") = mDistinctCustomerContractText

        mStatusList = StatusList.GetStatusList(0, IsSelectTagRequired:=True)
        cmbStatus.DataSource = mStatusList
        Session("mStatusList") = mStatusList


        'mTransactionListCount = TransactionListCount.GetTransactionListCountt(Util.Trans.CustomerContract)
        'Session("mTransactionListCount") = mTransactionListCount

        mtmpVendorList = VendorList.GetVendortList(0, , , , , , True, True, False)
        cmbCustomer.DataSource = mtmpVendorList
        Session("mtmpVendorList") = mtmpVendorList

        DataBind()

        If cmbStatus.Items.Count > 0 Then
            ' Replace the text and value of the 0th item
            cmbStatus.Items(0).Text = "(All)"

        End If

        If cmbCustomer.Items.Count > 0 Then
            ' Replace the text and value of the 0th item
            cmbCustomer.Items(0).Text = "(All)"

        End If

    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfCustomerContractList_Ajax.aspx"
            chkDate.Checked = True

            mEventLog = EventLog.GetEventLog(CType(Session("EventLogID"), Guid))
            Session("mEventLog") = mEventLog
            DataFieldBind()
            SetControl()
        End If

        SetGrid()
        SetTitle()
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(DateIndex)
        setPeriod(DateIndex)
        If cmbDate.Enabled = True Then
            SetFocus(cmbDate)
        End If

        setVariables()
      
        SetGrid()


        upnlGrid.Update()
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
        upnlResult.Update()
        upnlContract.Update()
        upnlContractNo.Update()
    End Sub
    Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFindNow.Click
        
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(DateIndex)
        setVariables()

        FindNow(Text, Val(No), FromDate, ToDate, Val(StatusID), "", IsDateChecked, CustomerID)

        dgContractList.DataBind()
        SetGrid()
        ControlVisibility()
        lblResult.Text = "List of Contract(s) as per criteria : " & mCustomerContractList.Count & " Record(s)."
        upnlGrid.Update()
        upnlResult.Update()
        upnlContract.Update()
        upnlContractNo.Update()

        If mCustomerContractList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Private Sub dgContractList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgContractList.PageIndexChanging
        dgContractList.PageIndex = e.NewPageIndex
        dgContractList.DataSource = mCustomerContractList
        Session("mCWPList") = mCustomerContractList
        dgContractList.DataBind()
        SetGrid()
    End Sub

    Private Sub dgContractList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgContractList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mCustomerContract = CustomerContract.GetCustomerContract(mID)
                If (Not User.IsInRole("CustomerContractView") And Not User.IsInRole("CustomerContractEdit")) Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "CustomerContract", User.Identity.Name & " is not Authorized User to edit " + mCustomerContract.ContractNumber, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

              

                Session("mCustomerContract") = mCustomerContract
                Dim mCustomerContractDetail As String = "Contract : " + mCustomerContract.ContractNumber + " dated : " + mCustomerContract.ContractDateFormatted
                MarkLog(Util.Action.Edit, "CustomerContract", mCustomerContractDetail, Util.ErrorType.NoError, mCustomerContract.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfCustomerContract_Ajax.aspx?BackPage=Index.aspx');", True)
            Case "DeleteRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mCustomerContract = CustomerContract.GetCustomerContract(mID)
                If (Not User.IsInRole("CustomerContractDelete")) Then
                    SetSession()
                    MarkLog(Util.Action.Delete, "CustomerContract", User.Identity.Name & " is not Authorized User to delete " + mCustomerContract.ContractNumber, Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                    '************************************
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")

                    Session("mCustomerContract") = mCustomerContract
                End If
            
        End Select
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        mCustomerContract = CustomerContract.NewCustomerContract()
        MarkLog(Util.Action.[New], "CustomerContract", "", Util.ErrorType.NoError, mCustomerContract.ID, EventLogID)

        Session("mCustomerContract") = mCustomerContract
        SetGrid()
        upnlGridView.Update()
      
        'Dim str As String
        'str = "openledgersame('wfCustomerContract_Ajax.aspx?BackPage=index.aspx');"
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfCustomerContract_Ajax.aspx?BackPage=Index.aspx');", True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("MiddleFrame") = ""
        RemoveSession()
       
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub dgCWPList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgContractList.Sorting
        mCustomerContractList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgContractList.DataSource = mCustomerContractList
        Session("mCustomerContractList") = mCustomerContractList
        dgContractList.DataBind()
        SetGrid()
    End Sub







#End Region

   

End Class