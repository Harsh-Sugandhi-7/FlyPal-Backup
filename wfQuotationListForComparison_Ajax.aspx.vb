Imports System.Web.Services
Imports System.Text
Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq
Imports System
Imports System.IO
Public Class wfQuotationListForComparison_Ajax
    Inherits System.Web.UI.Page

#Region " Variables and Declarations "
    Dim mItems As Items
    Dim mName As String
    Dim OpenFrom As String
    Public mEnqID As Guid
    Public mQuotationForComparison As QuotationForComparison
    Public mQuotationListForComparison As QuotationListForComparison
    Dim i As Integer = 0
    Dim mBaseCurrency As Currency
    Public mOrder As Order
    Dim NumberOfOrderDetails As StringBuilder = New StringBuilder
    Public mReqID As Guid
    Dim IsAgainstEnquiry As String
    Dim IsAgainstRequisition As String
    Dim DoneOrder As String
    Dim mModuleList As ModuleList    'Added by Prashant on 3-Sep-2020 STR03092020 
#End Region

#Region " Methods "
    Private Sub GetSession()
        mItems = Session("mItems")
        mName = Session("mName")
        OpenFrom = Session("OpenFrom")
        mEnqID = Session("mEnqID")
        mReqID = Session("mReqID")
        mQuotationListForComparison = Session("mQuotationListForComparison")
        IsAgainstEnquiry = Session("IsAgainstEnquiry")
        IsAgainstRequisition = Session("IsAgainstRequisition")
        DoneOrder = Session("DoneOrder")
        mModuleList = Session("mModuleList")
    End Sub
    Private Sub SetSession()
        Session("mItems") = mItems
        Session("mName") = mName
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEnqID")
        Session.Remove("mReqID")
        Session.Remove("IsAgainstEnquiry")
        Session.Remove("IsAgainstRequisition")
        Session.Remove("DoneOrder")
    End Sub
    Private Sub FindNow(Optional ByVal mEnqID As String = "{00000000-0000-0000-0000-000000000000}", _
                        Optional ByVal mReqID As String = "{00000000-0000-0000-0000-000000000000}", _
                        Optional ByVal IsAgainstEnquiry As String = "False", _
                        Optional ByVal IsAgainstRequisition As String = "False",
                        Optional ByVal DoneOrder As String = "False")
        mQuotationListForComparison = QuotationListForComparison.GetQuotationListForComparison(New Guid(mEnqID), txtSearch.Text.Trim, _
                                                                                               txtTransactionDate.Text.Trim, ReqID:=mReqID.ToString, _
                                                                                               IsAgainstEnquiry:=IsAgainstEnquiry, _
                                                                                               IsAgainstRequisition:=IsAgainstRequisition, _
                                                                                               DoneOrder:=CBool(DoneOrder))
        dgQuotationList.DataSource = mQuotationListForComparison

        mBaseCurrency = Currency.GetBaseCurrency()
        dgQuotationList.Columns(14).HeaderText = "Base Rate " + "(" + mBaseCurrency.Symbol + ")"  'Base Rate
        dgQuotationList.DataBind()
        If CBool(DoneOrder) = True Then
            btnCreateOrder.Visible = False
            btnCreateOrderTop.Visible = False
            For i As Integer = 0 To dgQuotationList.Rows.Count - 1
                Dim txtValue As TextBox
                If CType(Me.dgQuotationList.Rows(i).FindControl("txtRemark"), TextBox) Is Nothing Then
                    'Do nothing
                Else
                    txtValue = CType(Me.dgQuotationList.Rows(i).FindControl("txtRemark"), TextBox)
                    txtValue.Enabled = False
                End If
            Next
        End If
        Session("mQuotationListForComparison") = mQuotationListForComparison
        upnlGrid.Update()
    End Sub
    Private Sub ShowMessage(Optional ByVal OrderDetail As String = "")
        Dim str1 As String = ""
        str1 = str1 + ("<span class=""clsLabelAuto"">Order(s) Created Successfully! <BR>" + OrderDetail + "</BR></span>")
        MSGBoxCtrl.show("Alert!", str1, "", MsgBoxStyle.OkOnly, "OrderCreated")
        Exit Sub
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                Case MsgBoxResult.No

                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Selectedmultipletimes" Then
                        For k As Integer = 0 To mQuotationListForComparison.Count - 1
                            mQuotationListForComparison(k).IsSelected = False
                        Next
                        Session("mQuotationListForComparison") = mQuotationListForComparison
                    End If
                    If MSGBoxCtrl.Sender = "OrderCreated" Then
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                        'FindNow(mEnqID.ToString)
                    End If
            End Select
        End If
    End Sub
    Private Sub SetGrid()
        Dim P As Integer
        For j As Integer = 0 To dgQuotationList.Rows.Count - 1
            P = mQuotationListForComparison(j).ImageSize

            Dim img As ImageButton = dgQuotationList.Rows(j).Cells(0).FindControl("ViewAttachment")
            If P > 0 Then
                img.Visible = True
            Else
                img.Visible = False
            End If
        Next
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If txtTransactionDate.Text.ToString = "" Then
            txtTransactionDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End If
        If Not IsPostBack Then
            mEnqID = New Guid(Request.QueryString("EnqID"))
            mReqID = New Guid(Request.QueryString("ReqID"))

            IsAgainstEnquiry = Request.QueryString("AgainstEnquiry")
            Session("IsAgainstEnquiry") = IsAgainstEnquiry
            IsAgainstRequisition = Request.QueryString("AgainstRequisition")
            Session("IsAgainstRequisition") = IsAgainstRequisition
            DoneOrder = Request.QueryString("DoneOrders")
            Session("DoneOrder") = DoneOrder
            Session("mEnqID") = mEnqID
            Session("mReqID") = mReqID
            FindNow(mEnqID.ToString, mReqID:=mReqID.ToString, IsAgainstEnquiry:=IsAgainstEnquiry, IsAgainstRequisition:=IsAgainstRequisition, DoneOrder:=DoneOrder)
        End If
        SetGrid()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click, txtTransactionDate.TextChanged
        'dgPartList.PageIndex = 0
        'Dim Index As Int32 = Val(cmblookin.SelectedIndex)
        FindNow(mEnqID.ToString, mReqID.ToString, IsAgainstEnquiry, IsAgainstRequisition, DoneOrder)
    End Sub
    Private Sub btnCreateOrder_Click(sender As Object, e As System.EventArgs) Handles btnCreateOrder.Click, btnCreateOrderTop.Click

        mBaseCurrency = Currency.GetBaseCurrency()
        Dim checkString = Request.Form("chkSelectList")
        Dim chkItemIDList = Request.Form("chkItemIDList")

        If checkString Is Nothing Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, " Record", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else

            Dim values = checkString.Split(","c)

            Dim ItemIDvalues As String() = chkItemIDList.Split(","c)

            '---'Added By Prashant 25-Nov-2019 BA25112019 To assign typed remark to order item Note. 
            'It will get saved in order item only if user click on create order, not in quotation item----------------------------------------------------------------
            For i As Integer = 0 To dgQuotationList.Rows.Count - 1
                Dim txtValue As TextBox
                If CType(Me.dgQuotationList.Rows(i).FindControl("txtRemark"), TextBox) Is Nothing Then
                    'Do nothing
                Else
                    txtValue = CType(Me.dgQuotationList.Rows(i).FindControl("txtRemark"), TextBox)
                    mQuotationListForComparison(i).Remark = txtValue.Text
                End If
            Next

            For Each value As String In values
                mQuotationListForComparison(New Guid(value), "").IsSelected = True
            Next

            'Dim MaxDate = (From d In mQuotationListForComparison Select d.QDate).Max()

            'Dim MaxDate = (From c In mQuotationListForComparison Where c.IsSelected = True Select c.QDate).Max()
            'If CType(txtTransactionDate.Text, Date) < CType(MaxDate, Date) Then
            '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, " Record", MsgBoxStyle.OkOnly, "")
            '    Exit Sub
            'End If
            'For Each ItemIDvalue As String In ItemIDvalues
            '    mQuotationListForComparison(New Guid(ItemIDvalue.ToString)).IsItemIDSelected = True
            'Next
            'Dim SelectedItemCount = (From c In mQuotationListForComparison
            '                  Where c.IsItemIDSelected = True _
            '                Group By ItemID = c.ItemID Into Group
            '                Select New With {.ItemID = ItemID, .InstanceCount = Group.Count()})

            Dim SelectedItemCount = (From c In ItemIDvalues
                           Group By c Into Group
                            Select New With {.ItemName = c, .InstanceCount = Group.Count()})


            Dim ItemCount
            For Each ItemCount In SelectedItemCount
                If ItemCount.InstanceCount > 1 Then
                    MSGBoxCtrl.show("Alert!", "Multiple Quotations Selected for Part No. " + ItemCount.ItemName, "", MsgBoxStyle.OkOnly, "Selectedmultipletimes")
                    SelectedItemCount = ""
                    Exit Sub
                End If
            Next

            Dim GroupByVendorID = (From c In mQuotationListForComparison
                               Where c.IsSelected = True _
                             Group By VendorID = c.VendorID, CurrencyID = c.QuotationCurrencyID, ConversionFactor = c.QuotationConversionFactor, TransTypeID = c.TransTypeID Into Group
                             Select New With {.VendorID = VendorID, .CurrencyID = CurrencyID, .ConversionFactor = ConversionFactor, .TransTypeID = TransTypeID, .ReceiptItemCollection = Group})

            Dim variable
            For Each variable In GroupByVendorID
                If variable.TransTypeID = 36 Then   'Repair/Overhul Quoation 36 TransTypeID 
                    mOrder = Order.NewOrder(38)
                    mOrder.AgainstTypeID = 2        'Repair/Overhul Order Against Quoation
                    mOrder.ExchangeOrderTypeID = 1
                Else
                    mOrder = Order.NewOrder(5)
                    mOrder.AgainstTypeID = 7
                End If

                mOrder.OrderDate = txtTransactionDate.Text 'Today.Date  '

                mOrder.VendorID = variable.VendorID
                mOrder.CurrencyID = variable.CurrencyID 'mBaseCurrency.ID
                mOrder.ConversionFactor = variable.ConversionFactor 'mBaseCurrency.ConversionFactor
                mOrder.UserName = User.Identity.Name
                Dim receiptitemchildcol
                For Each receiptitemchildcol In variable.ReceiptItemCollection
                    mOrder.OrderItems.Add(mOrder.ID)

                    With mOrder.OrderItems.CurrentItem
                        mOrder.QuotationNo = receiptitemchildcol.QuotationTextNo
                        mOrder.QuotationDate = receiptitemchildcol.QuotationDateFormatted.ToString
                        mOrder.OrderItems.CurrentItem.ItemID = receiptitemchildcol.ItemID
                        mOrder.OrderItems.CurrentItem.ConversionFactor = mOrder.ConversionFactor
                        mOrder.OrderItems.CurrentItem.CRate = receiptitemchildcol.QuotationItemCRate
                        mOrder.OrderItems.CurrentItem.UnitID = receiptitemchildcol.UnitID
                        mOrder.OrderItems.CurrentItem.Note = receiptitemchildcol.Remark 'Added By Prashant 25-Nov-2019 BA25112019 To assign typed remark to order item Note
                        mOrder.OrderItems.CurrentItem.FromNo = receiptitemchildcol.RequisitionTextNo
                        .OrderItemQuotationItems.Add(.ID, receiptitemchildcol.QuotationItemID, receiptitemchildcol.QuotationItemQty, receiptitemchildcol.QuotationTextNo, receiptitemchildcol.QuotationDate.ToString, receiptitemchildcol.QuotationID)

                        Dim mVendor As Vendor
                        Dim mGSTPercentage As GSTPercentage
                        If AppSettings("IsGSTApplicable") = "True" And Not mOrder.VendorID.Equals(Guid.Empty) Then
                            mVendor = Vendor.GetVendor(mOrder.VendorID)
                            If mVendor.CountryName.ToUpper = "INDIA" And CDate(mOrder.OrderDateFormatted.ToString) >= CDate("01-Jul-2017") And mVendor.ClientCountryName.ToUpper.Equals("INDIA") Then
                                mGSTPercentage = GSTPercentage.GetPercentage(mOrder.OrderDateFormatted.ToString, 1, .ItemID.ToString)
                                If Not mGSTPercentage Is Nothing Then
                                    Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
                                    If Len(mVendor.StateCode) > 0 Then
                                        If mVendor.StateCode = mVendor.ClientStateCode Then
                                            .CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
                                            .SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
                                            .CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
                                            .SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)
                                            .IGSTPercentage = 0
                                            .IGSTCAmount = 0
                                            .TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount
                                            mOrder.StateCode = mVendor.StateCode
                                            mOrder.ClientStateCode = mVendor.ClientStateCode
                                            mOrder.VendorCountry = mVendor.CountryName
                                            mOrder.Visibility = 1
                                        Else
                                            .IGSTPercentage = (mGSTPercentage.GSTPercentage)
                                            .IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)
                                            .CGSTPercentage = 0
                                            .SGSTPercentage = 0
                                            .CGSTCAmount = 0
                                            .SGSTCAmount = 0
                                            .TotalCAmount = .CAmount + .IGSTCAmount
                                            mOrder.StateCode = mVendor.StateCode
                                            mOrder.ClientStateCode = mVendor.ClientStateCode
                                            mOrder.VendorCountry = mVendor.CountryName
                                            mOrder.Visibility = 2
                                        End If
                                        .HSNACSCode = mtmpItem.HSNACSCode
                                    Else
                                        .CGSTPercentage = 0
                                        .SGSTPercentage = 0
                                        .CGSTCAmount = 0
                                        .SGSTCAmount = 0
                                        .IGSTPercentage = 0
                                        .IGSTCAmount = 0
                                        .HSNACSCode = ""
                                        mOrder.StateCode = mVendor.StateCode
                                        mOrder.ClientStateCode = mVendor.ClientStateCode
                                        mOrder.VendorCountry = mVendor.CountryName
                                        mOrder.Visibility = 3
                                    End If
                                End If
                            Else
                                .CGSTPercentage = 0
                                .SGSTPercentage = 0
                                .CGSTCAmount = 0
                                .SGSTCAmount = 0
                                .IGSTPercentage = 0
                                .IGSTCAmount = 0
                                .HSNACSCode = ""
                                mOrder.StateCode = mVendor.StateCode
                                mOrder.ClientStateCode = mVendor.ClientStateCode
                                mOrder.VendorCountry = mVendor.CountryName
                                mOrder.Visibility = 3
                            End If
                        Else
                            .CGSTPercentage = 0
                            .SGSTPercentage = 0
                            .CGSTCAmount = 0
                            .SGSTCAmount = 0
                            .IGSTPercentage = 0
                            .IGSTCAmount = 0
                            .HSNACSCode = ""
                            mOrder.Visibility = 3
                        End If
                    End With
                Next
                mOrder.CalculateTotal()
                mOrder.Save()
                NumberOfOrderDetails.Append(mOrder.Text.ToString & "-" & mOrder.No.ToString + " Dated : " + mOrder.OrderDateFormatted + "<BR>")
            Next
            RemoveSession()
            ShowMessage(NumberOfOrderDetails.ToString)
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Protected Sub OnDataBound(sender As Object, e As EventArgs)
        If dgQuotationList.Rows.Count = 0 Then Exit Sub
        Dim j As Integer = dgQuotationList.Rows.Count - 1
        For i As Integer = dgQuotationList.Rows.Count - 1 To 1 Step -1


            Dim row As GridViewRow = dgQuotationList.Rows(i)
            Dim previousRow As GridViewRow = dgQuotationList.Rows(i - 1)
            'For j As Integer = 2 To row.Cells.Count - 1
            If mQuotationListForComparison(i).QuotationID.Equals(Guid.Empty) Then
                'row.Cells(1).Attributes("readonly") = "true"
                'row.Cells(1).Enabled = False
                'row.Cells(1).Attributes.Add("disabled", "disabled")
                dgQuotationList.Rows(i).Cells(6).Text = ""
                dgQuotationList.Rows(i).Cells(8).Text = ""
                dgQuotationList.Rows(i).Cells(9).Text = ""
                dgQuotationList.Rows(i).Cells(10).Text = ""
                dgQuotationList.Rows(i).Cells(11).Text = ""
                dgQuotationList.Rows(i).Cells(12).Text = ""
                dgQuotationList.Rows(i).Cells(13).Text = ""
                dgQuotationList.Rows(i).Cells(14).Text = ""
                dgQuotationList.Rows(i).Cells(15).Text = ""
                dgQuotationList.Rows(i).Cells(16).Text = ""
                dgQuotationList.Rows(i).Cells(17).Text = ""
                dgQuotationList.Rows(i).Cells(18).Text = ""
            End If



            If row.Cells(2).Text = previousRow.Cells(2).Text Then
                If previousRow.Cells(2).RowSpan = 0 Then
                    If row.Cells(2).RowSpan = 0 Then
                        previousRow.Cells(2).RowSpan += 2
                        If i = j Then 'i.e Last row bottom border
                            'Do nothing 
                        Else
                            dgQuotationList.Rows(i).Attributes("style") = "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-color:rgb(128,0,64);"
                            previousRow.Cells(2).Attributes("style") = "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-bottom-color:rgb(128,0,64); border-bottom-width: 3px;"
                        End If
                    Else
                        previousRow.Cells(2).RowSpan = row.Cells(2).RowSpan + 1
                        'previousRow.Cells(2).Attributes("style") = "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-bottom-color:rgb(128,0,64); border-bottom-width: 3px;"
                    End If
                    row.Cells(2).Visible = False
                End If
            End If
            'If row.Cells(3).Text = previousRow.Cells(3).Text Then
            '    If previousRow.Cells(3).RowSpan = 0 Then
            '        If row.Cells(3).RowSpan = 0 Then
            '            previousRow.Cells(3).RowSpan += 2
            '            If i = j Then 'i.e Last row bottom border
            '                'Do nothing 
            '            Else
            '                dgQuotationList.Rows(i).Attributes("style") = "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-color:rgb(128,0,64);"
            '                previousRow.Cells(3).Attributes("style") = "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-bottom-color:rgb(128,0,64); border-bottom-width: 3px;"
            '            End If
            '        Else
            '            previousRow.Cells(3).RowSpan = row.Cells(3).RowSpan + 1
            '            'previousRow.Cells(2).Attributes("style") = "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-bottom-color:rgb(128,0,64); border-bottom-width: 3px;"
            '        End If
            '        row.Cells(3).Visible = False
            '    End If
            'End If
        Next
        If mQuotationListForComparison(0).QuotationID.Equals(Guid.Empty) Then
            'dgQuotationList.Rows(0).Cells(1).Attributes("disabled") = "disabled"
            dgQuotationList.Rows(0).Cells(6).Text = ""
            dgQuotationList.Rows(0).Cells(8).Text = ""
            dgQuotationList.Rows(0).Cells(9).Text = ""
            dgQuotationList.Rows(0).Cells(10).Text = ""
            dgQuotationList.Rows(0).Cells(11).Text = ""
            dgQuotationList.Rows(0).Cells(12).Text = ""
            dgQuotationList.Rows(0).Cells(13).Text = ""
            dgQuotationList.Rows(0).Cells(14).Text = ""
            dgQuotationList.Rows(0).Cells(15).Text = ""
            dgQuotationList.Rows(0).Cells(16).Text = ""
            dgQuotationList.Rows(0).Cells(17).Text = ""
            dgQuotationList.Rows(0).Cells(18).Text = ""
        End If
    End Sub
    Private Sub btnPrintTop_Click(sender As Object, e As System.EventArgs) Handles btnPrintTop.Click, btnPrintBottom.Click, btnByMailTop.Click, btnByMailBottom.Click, hdnBtnSendMail.Click
        'ALL21012019
        Dim ItemCount
        Dim checkString = Request.Form("chkSelectList")

        Dim chkItemIDList = Request.Form("chkItemIDList")

        If Not checkString Is Nothing Then
            Dim ItemIDvalues As String() = chkItemIDList.Split(","c)
            Dim SelectedItemCount = (From c In ItemIDvalues
                              Group By c Into Group
                               Select New With {.ItemName = c, .InstanceCount = Group.Count()})

            For Each ItemCount In SelectedItemCount
                If ItemCount.InstanceCount > 1 Then
                    MSGBoxCtrl.show("Alert!", "Multiple Quotations Selected for Part No. " + ItemCount.ItemName, "", MsgBoxStyle.OkOnly, "Selectedmultipletimes")
                    SelectedItemCount = ""
                    Exit Sub
                End If
            Next

            Dim values = checkString.Split(","c)
            'For Each value As String In values
            '    mQuotationListForComparison(New Guid(value), "").IsSelected = True
            'Next
            'End
            '---Added By Prashant 21-Oct-2019 to Make Remark Field Editable----------------------------------------------------------------
            For i As Integer = 0 To dgQuotationList.Rows.Count - 1
                Dim txtValue As TextBox
                If CType(Me.dgQuotationList.Rows(i).FindControl("txtRemark"), TextBox) Is Nothing Then
                    'Do nothing
                Else
                    txtValue = CType(Me.dgQuotationList.Rows(i).FindControl("txtRemark"), TextBox)
                    mQuotationListForComparison(i).Remark = txtValue.Text
                    If mQuotationListForComparison.Item(i).IsDirty Then
                        Try
                            QuotationListForComparison.QuotationItemRemark(mQuotationListForComparison(i).QuotationItemID, _
                                                                           txtValue.Text.Trim)
                        Catch ex As Exception
                        End Try
                    End If
                End If
            Next
            For Each value As String In values
                mQuotationListForComparison(New Guid(value), "").IsSelected = True
            Next
            dgQuotationList.DataSource = mQuotationListForComparison
            dgQuotationList.DataBind()
            '---End of Added By Prashant 21-Oct-2019 to Make Remark Field Editable----------------------------------------------------------------
        End If

        'WORKING CODE IN VB.NET For LINQ GROUP & SUM
        'Dim VendorwiseAmountSum = From c In mQuotationListForComparison
        '                         Group c By VendorID = c.VendorID, VendorName = c.VendorName Into Group
        '                         Select New With {Key .VendorID = VendorID, Key .VendorName = VendorName, Key .QuotationItemAmount = Group.Sum(Function(x) x.QuotationItemAmount)}
        If (sender.ID = "btnPrintTop" Or sender.ID = "btnPrintBottom") Then
            SetReport(False, mQuotationListForComparison)
        ElseIf (sender.ID = "btnByMailTop" Or sender.ID = "btnByMailBottom") Then
            Session("UserEmailID") = mModuleList.Item("QuotationComparison").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("QuotationComparison").SendCCMailID
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", "OpenByMaiWindow();", True)
        ElseIf (sender.ID = "hdnBtnSendMail") Then
            Dim email As Thread
            Try
                email = New Thread(Sub() SetReport(True, mQuotationListForComparison))
                email.IsBackground = True
                email.Start()
            Catch ex As Exception
                Dim Day, Month, Year As String
                Day = Format(Today.Date.Day, "0#")
                Month = Format(Today.Date.Month, "0#")
                Year = Format(Today.Date.Year, "0#")
                Dim todaydate As String = Day & Month & Year
                Dim Path As String = AppSettings("DOCPath") & todaydate
                FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
                FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (Quotation List For Comparison): " + ex.GetBaseException.Message + vbLf)
                FileClose(1)
            End Try
        End If
    End Sub
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False, Optional ByVal mQuotationListForComparison As QuotationListForComparison = Nothing)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Then
            myReport = New crQuotationForComparisonBA
        ElseIf AppSettings("ClientCode") = "Heligo" Then
            myReport = New crQuotationForComparisonHeligo
        Else
            myReport = New crQuotationForComparison
        End If
        Dim ds As New dsQuotationForComparison
        Dim mCompanyDetail As New CompanyDetail
        Dim Header As String = ""
        Header = "Enq. No./Req. No."
        Dim mReport As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
                                     mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, "", _
                                     Header, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), , , , , , _
                                     SearchStr11:=AppSettings("ClientCode"))

        mQuotationListForComparison = Session("mQuotationListForComparison")

        da.Fill(ds, mQuotationListForComparison)
        da.Fill(ds, mReport)

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        'ALL21012019
        For k As Integer = 0 To mQuotationListForComparison.Count - 1
            mQuotationListForComparison(k).IsSelected = False
        Next
        Session("mQuotationListForComparison") = mQuotationListForComparison
        'End

        If ByMail Then
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "COMPARATIVE STATEMENT & TECHNICAL VETTING NOTE", "COMPARATIVE STATEMENT & TECHNICAL VETTING NOTE", _
                                      "", "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                       SmtpHost:=mModuleList.Item("QuotationComparison").SmtpHost, SmtpPort:=mModuleList.Item("QuotationComparison").SmtpPort, _
                                      SmtpUser:=mModuleList.Item("QuotationComparison").SmtpUser, SmtpPassword:=mModuleList.Item("QuotationComparison").SmtpPassword)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If
    End Sub

    Public mQuotation As Quotation
    Private Sub dgQuotationList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgQuotationList.RowCommand
        Select Case e.CommandName
            Case "ViewRec"

                Dim index As Int32 = CInt(e.CommandArgument) + dgQuotationList.PageIndex * dgQuotationList.PageSize
                mQuotationForComparison = mQuotationListForComparison.Item(index)

                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mQuotation = Quotation.GetQuotation(mQuotationForComparison.QuotationID)

                If mQuotation.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mQuotation.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mQuotation.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mQuotation.ImageFile, 0, mQuotation.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        Dim Str As String
                        Str = "openFile();"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
                    End If
                Else
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
        End Select
    End Sub
#End Region

End Class