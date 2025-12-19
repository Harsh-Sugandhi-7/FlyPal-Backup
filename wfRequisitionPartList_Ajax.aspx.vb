'AJAX Conversion By Vikrant On 09-July-2014

Public Class wfRequisitionPartList_Ajax
    Inherits System.Web.UI.Page

#Region "Variables"
    Public mRequisitionItemsNew As RequisitionItemsNew
    Public mTransDate As String
    Public mEnquiryItemID As Guid
    Public mQuotationItemID As Guid
    Public mListFor As Integer
    Public mTempCustomerID As Guid
    Public mTransTypeID As Integer
    Public mEnquiry As Enquiry  'added by Shital  om 06-Dec-2021
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mRequisitionItemsNew = CType(Session("mRequisitionItemsNew"), RequisitionItemsNew)
        mTransDate = Session("TransDate")
        mEnquiryItemID = Session("EnquiryItem")
        mListFor = Session("ListFor")
        mQuotationItemID = Session("QuotationItem")
        mTempCustomerID = Session("TempCustomerID")
        mTransTypeID = Session("TransTypeID")
        mEnquiry = Session("mEnquiry") 'added by Shital  om 06-Dec-2021
    End Sub
    Private Sub SetSession()
        Session("mRequisitionItemsNew") = mRequisitionItemsNew
    End Sub
    Private Sub RemoveSession()
        Session.Remove("TransDate")
        Session.Remove("EnquiryItem")
        Session.Remove("ListFor")
        Session.Remove("QuotationItem")
        Session.Remove("TempCustomerID")
    End Sub
    Private Sub FindNow()
        dgRequisitionItemList.PageIndex = 0
        Dim ReqNo As Integer = 0
        If txtRequisitionNo.Text.Trim <> "" Then
            ReqNo = CInt(txtRequisitionNo.Text)
        End If
        If mListFor = 0 Then
            If (CType(mEnquiry.TransTypeID, Trans) = Util.Trans.OverHaulRepairEnquiry) Then
                mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForList(mTransDate, txtPartNumber.Text, mEnquiryItemID, mListFor, _
                                                                                CInt(cmbRequisition.SelectedValue), mTempCustomerID.ToString, _
                                                                                Text:=txtRequisitionText.Text.Trim, No:=ReqNo, FromDate:=txtFromDate.Text, _
                                                                                ToDate:=txtToDate.Text, ExchangeAsRequisitionItems:=1)
            Else
                mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForList(mTransDate, txtPartNumber.Text, mEnquiryItemID, mListFor, _
                                                                                                 CInt(cmbRequisition.SelectedValue), mTempCustomerID.ToString, _
                                                                                                 Text:=txtRequisitionText.Text.Trim, No:=ReqNo, FromDate:=txtFromDate.Text, _
                                                                                                 ToDate:=txtToDate.Text)
            End If
           
            'dgRequisitionItemList.Columns(10).Visible = True 'Commented & Added For New Requisition
            'dgRequisitionItemList.Columns(11).Visible = False 'Commented & Added For New Requisition
        ElseIf mListFor = 1 Then
            mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForList(mTransDate, txtPartNumber.Text, mQuotationItemID, mListFor, CInt(cmbRequisition.SelectedValue), mTempCustomerID.ToString, Text:=txtRequisitionText.Text.Trim, No:=ReqNo)
            'dgRequisitionItemList.Columns(10).Visible = False 'Commented & Added For New Requisition
            'dgRequisitionItemList.Columns(11).Visible = True 'Commented & Added For New Requisition
        ElseIf mListFor = 2 Then
            mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForList(Today.Date.ToString, txtPartNumber.Text, mQuotationItemID, mListFor, CInt(cmbRequisition.SelectedValue), mTempCustomerID.ToString, Text:=txtRequisitionText.Text.Trim, No:=ReqNo)
        End If
        dgRequisitionItemList.DataSource = mRequisitionItemsNew
        Session("mRequisitionItemsNew") = mRequisitionItemsNew
        DataBind()
        lblResult.Text = "List of Requisition Parts as per criteria: " & mRequisitionItemsNew.Count & " Record(s) found."
    End Sub
    Private Sub ControlVisibility()
        If mListFor = 0 Then
            dgRequisitionItemList.Columns(6).Visible = True
            dgRequisitionItemList.Columns(7).Visible = False
            lblDate.Visible = False
            txtTransactionDate.Visible = False
            txtTransactionDate.Text = Request.QueryString("TransDate")
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
        ElseIf mListFor = 1 Then
            dgRequisitionItemList.Columns(6).Visible = False
            dgRequisitionItemList.Columns(7).Visible = True

            Dim chkHeader As CheckBox = DirectCast(dgRequisitionItemList.HeaderRow.FindControl(“chkSelectAll”), CheckBox)
            chkHeader.Visible = False


            lblDate.Visible = True
            txtTransactionDate.Visible = True
            txtTransactionDate.Text = Request.QueryString("TransDate")
            txtTransactionDate.Enabled = IIf(CType(IIf(Session("ItemsCount") Is Nothing, 0, Session("ItemsCount")), Integer) > 0, False, True)
        End If
    End Sub
    Private Sub AddAttributes()
        txtRequisitionNo.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtRequisitionNo').value,event)")
    End Sub
#End Region

#Region " DataBind "
    Private Sub SetObject()
        Dim chkSelect As CheckBox
        Dim Recordno As Integer
        ' Set Selected Notes value  
        For i As Integer = 0 To dgRequisitionItemList.Rows.Count - 1
            Recordno = i + dgRequisitionItemList.PageSize * dgRequisitionItemList.PageIndex
            chkSelect = CType(dgRequisitionItemList.Rows(i).FindControl("chkSelect"), CheckBox)
            mRequisitionItemsNew(Recordno).IsSelect = chkSelect.Checked
            mRequisitionItemsNew(Recordno).MarkClean()
        Next
        Session("mRequisitionItemsNew") = mRequisitionItemsNew
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        AddAttributes()
        If Not IsPostBack And Session("Sender") = "" Then
            If txtPartNumber.Enabled = True Then
                txtPartNumber.Focus()
            End If
            'If Request.QueryString("Type") = "pup" Then
            mListFor = CInt(Request.QueryString("ListFor"))
            mTransDate = Request.QueryString("TransDate")
            Session("ListFor") = mListFor
            Session("TransDate") = mTransDate
            'End If
            txtFromDate.Text = Today.Date.AddMonths(-1).ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            FindNow()
            ControlVisibility()
            lblResult.Text = "List of Requisition Parts as per criteria: " & mRequisitionItemsNew.Count & " Record(s) found."
            SetSession()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        Session("TransactionDate") = txtTransactionDate.Text
        mTransDate = txtTransactionDate.Text
        Session("mTransDate") = mTransDate
        FindNow()
        upnlDetails.Update()
    End Sub
    Protected Sub txtTransactionDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Session("TransactionDate") = txtTransactionDate.Text
        mTransDate = txtTransactionDate.Text
        Session("mTransDate") = mTransDate
        FindNow()
        upnlDetails.Update()
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        SetObject()
        Session("AddRequisitionParts") = "True"
        Session("AddPart") = "True"
        RemoveSession()
        Session("TransactionDate") = txtTransactionDate.Text
        If Session("StoreApprovalList") = "True" Then
            Session("StoreApprovalList") = "False"
            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        Else
            'Added by vikrant for popup
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
            'End
            Response.Redirect(Request.QueryString("BackPage"))
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mRequisitionItemsNew")
        RemoveSession()
        If Session("StoreApprovalList") = "True" Then
            Session("StoreApprovalList") = "False"
            Session("AddPart") = "False"
            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        Else
            Session("AddRequisitionParts") = "False"
            Session("AddPart") = "False"
            'Added by vikrant for popup
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
            'End
            Session("IsBackFromPendingList") = "True"
            Response.Redirect(Request.QueryString("BackPage1"))
        End If
    End Sub
    Private Sub dgRequisitionItemList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgRequisitionItemList.PageIndexChanging
        SetObject()
        dgRequisitionItemList.PageIndex = e.NewPageIndex
        dgRequisitionItemList.DataSource = mRequisitionItemsNew
        dgRequisitionItemList.DataBind()
        ControlVisibility()
    End Sub
    Private Sub dgRequisitionItemList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRequisitionItemList.Sorting
        mRequisitionItemsNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRequisitionItemsNew") = mRequisitionItemsNew
        dgRequisitionItemList.DataSource = mRequisitionItemsNew
        dgRequisitionItemList.DataBind()
        ControlVisibility()
    End Sub
    'Added By Prashant on 23-Dec-2020 BA23122020
    Private Sub dgRequisitionItemList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRequisitionItemList.RowCommand
        Select Case e.CommandName
            Case "AddPart"
                If (Not User.IsInRole("PartNew")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                'Dim rowIndex As Integer = gvr.RowIndex
                'Dim Index As Integer
                'Index = rowIndex
                'If IsValid Then

                Dim Index As Integer = (CInt(e.CommandArgument) + (dgRequisitionItemList.PageSize * dgRequisitionItemList.PageIndex))
                Dim ItemName As String = dgRequisitionItemList.DataKeys(CInt(e.CommandArgument)).Values(0).ToString
                Dim ItemDescription As String = dgRequisitionItemList.DataKeys(CInt(e.CommandArgument)).Values(1).ToString

                Dim mItem As Item
                mItem = Item.NewItem()
                mItem.Name = mRequisitionItemsNew(Index).PartNo
                mItem.Description = mRequisitionItemsNew(Index).Description
                Session("mItem") = mItem
                Session("Create") = "False"
                Session("PartInfo") = "True"

                Dim URL As Stack = New Stack    'STACK to store url of current page
                URL.Push(Request.Url)           'Inserting URL in STACK
                Session("URL") = URL
                Response.Redirect("wfPartInformation_Ajax.aspx?BackPage=" & "wfCommonPartList_Ajax.aspx")
                'End If
        End Select

    End Sub



#End Region





End Class