'AJAX Conversion By Vikrant On 02-Jun-2014 
'Created By Saylee
Public Class wfIssueBERPart_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mIssueBERPartList As IssueBERPartList
    Dim IssuePartNo, IssueSerialNo, IssueDescription As String
    Dim mDate As String
    Dim EventLogID As Guid
    Public LookInType As Integer = 0
    Public PartID As String = ""

    Public mReceiptCumInvoice As ReceiptCumInvoice
    Public mIssue As Issue
    Public RCIBERDetail, IssueBERDetail As String
    Public Remark As String
    Public mItemList As ItemList
    Public mType As Int16
    Dim Index As Integer
    Dim mIssueChildID As Guid
    Dim PartInfo As String
    Dim SerialNo As String
    Dim mRate As String
    Dim mTransactionListCount As TransactionListCount  'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
    Dim mLastWarrantyInformation As LastWarrantyInformation
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mIssueBERPartList = Session("mIssueBERPartList")
        IssuePartNo = IIf(IsNothing(Session("IssuePartNo")), "", Session("IssuePartNo"))
        IssueSerialNo = IIf(IsNothing(Session("IssueSerialNo")), "", Session("IssueSerialNo"))
        IssueDescription = IIf(IsNothing(Session("IssueDescription")), "", Session("IssueDescription"))
        mDate = IIf(IsNothing(Session("mDate")), "", Session("mDate"))
        mType = Session("mType")
        mItemList = CType(Session("mItemList"), ItemList)
        PartID = Session("PartID")
        LookInType = Session("LookInType")
        Index = Session("Index")
        mIssueChildID = Session("mIssueChildID")
        mTransactionListCount = Session("mTransactionListCount") 'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    'Added By Vikrant On 24-July-2014 For BA24072014
    Private Function CheckDateForTransactionLock(TransDate As Date) As Boolean
        Dim FirstDayofLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
        Dim FirstDayofMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)
        If (TransDate >= FirstDayofLastMonth) Then
            If (TransDate < FirstDayofMonth) And (Day(Today.Date) > 10) Then
                Return True
            Else
                Return False
            End If
        Else
            Return True
        End If
    End Function
    'End
    Private Sub SetSession()
        Session("mIssueBERPartList") = mIssueBERPartList
        Session("IssuePartNo") = IssuePartNo
        Session("IssueSerialNo") = IssueSerialNo
        Session("IssueDescription") = IssueDescription
        Session("mDate") = mDate
        Session("mItemList") = mItemList
        Session("PartID") = PartID
        Session("LookInType") = LookInType
        Session("Index") = Index
        Session("mIssueChildID") = mIssueChildID
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mIssueBERPartList")
        'Session.Remove("IssuePartNo")
        'Session.Remove("IssueSerialNo")
        'Session.Remove("IssueDescription")
        'Session.Remove("mDate")
        Session.Remove("mType")
        Session.Remove("PartID")
        Session.Remove("LookInType")
        Session.Remove("Index")
        Session.Remove("mIssueChildID")
        Session.Remove("mTransactionListCount") 'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfIssueBERPart_Ajax.aspx" Then
            RemoveSession()
        End If
    End Sub
    Private Sub AddAttributes()
        txtName.Attributes.Add("onblur", "calltxtSearchEvent()")
    End Sub
    Private Overloads Sub setFocus(cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub DiscardRecord()
        MSGBoxCtrl.show(MSGBox.Message_title.DiscardBERConfirmation, MSGBox.Message_text.DiscardBERConfirmation, "", MsgBoxStyle.YesNo, "Discard")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Discard" Then
                        Try
                            Session("sender") = ""
                            'Added By Vikrant On 01-Oct-2012
                            PartInfo = mIssueBERPartList(CType(Session("mIssueChildID"), Guid)).PartName + "-" + mIssueBERPartList(mIssueChildID).Description
                            SerialNo = mIssueBERPartList(mIssueChildID).SerialNo
                            'mRate = mIssueBERPartList(CType(Session("mIssueChildID"), Guid)).CCommercialRate.ToString
                            'mRate = mIssueBERPartList(CType(Session("mIssueChildID"), Guid)).EffRate.ToString
                            'End
                            Session("PartInfo") = PartInfo
                            Session("SerialNo") = SerialNo
                            mIssueChildID = CType(Session("mIssueChildID"), Guid)
                            If (CreateAutoRCITransTextSeries(mIssueBERPartList(mIssueChildID)) = True) Then
                                If (CreateAutoIssueTransTextSeries() = True) Then 'Added By Prashant 12-Mar-2014 'ALL12032014
                                    Session("ShowNotification") = True
                                    Session("EffRateOfPart") = (mIssueBERPartList(CType(Session("mIssueChildID"), Guid)).EffRate * mIssueBERPartList(CType(Session("mIssueChildID"), Guid)).Qty) 'Amount
                                    Session("MaxEffectiveRateValue") = mIssueBERPartList(CType(Session("mIssueChildID"), Guid)).MaxEffectiveRateValue.ToString
                                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openledgersame", "FileUpload();", True)
                                    Exit Sub
                                End If
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    'Response.Redirect("wfIssueBERPart_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "RCITransTextSeriesAlert" Then
                        Session("sender") = ""
                        Session("AddTransTextSeries") = "True"
                        txtDate.Text = mDate.ToString
                        DataFieldBind()
                        upnlGridView.Update()
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    ElseIf MSGBoxCtrl.Sender = "IssueTransTextSeriesAlert" Then
                        Session("sender") = ""
                        Session("AddTransTextSeries") = "True"
                        txtDate.Text = mDate.ToString
                        DataFieldBind()
                        upnlGridView.Update()
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    Else
                        Session("sender") = ""
                        'Response.Redirect("wfIssueBERPart_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                    End If
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    'Response.Redirect("wfIssueBERPart_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfIssueBERPart_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub FindNow([Date] As String, Optional IssuePartNo As String = "", Optional IssueSerialNo As String = "", Optional IssueDescription As String = "")
        'This step is Imp when details form  is opened dirctly.
        dgBERPartList.DataSource = Nothing
        mIssueBERPartList = Nothing

        'Get List From the Database as per Criteria
        mIssueBERPartList = IssueBERPartList.GetIssueBERPartList([Date], IssuePartNo, IssueSerialNo, IssueDescription)

        'Set DataSource of the Grid
        dgBERPartList.DataSource = mIssueBERPartList
        dgBERPartList.DataBind()
        lblResult.Text = "List of Parts : " & mIssueBERPartList.Count & " Record(s) found. "
        Session("mIssueBERPartList") = mIssueBERPartList
    End Sub
    Public Sub SetControl()
        IssuePartNo = Session("IssuePartNo")
        IssueSerialNo = Session("IssueSerialNo")
        mDate = Session("mDate")
        FindNow(mDate, IssuePartNo, IssueSerialNo, IssueDescription)
    End Sub
    '--------------------------------------------------------------------------------------------------------
    Public Function CreateAutoRCITransTextSeries(IssueBERPart As IssueBERPartList.IssueBERPartListInfo) As Boolean  'Added By Prashant 12-Mar-2014 'ALL12032014
        mDate = Session("mDate")
        mReceiptCumInvoice = ReceiptCumInvoice.NewReceiptCumInvoice(Util.Trans.ExchangeRepairReceivedFromVendor)
        mReceiptCumInvoice.ReceiptCumInvoiceItems.Add(mReceiptCumInvoice.ID)
        mReceiptCumInvoice.RecCumInvDate = mDate.ToString
        '------------------------------------------------===========================
        If (mReceiptCumInvoice.IsNew) And (mReceiptCumInvoice.InvText = "" Or mReceiptCumInvoice.RecText = "") Then

            Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mReceiptCumInvoice.TransTypeID, mReceiptCumInvoice.RecCumInvDateFormatted)

            If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mReceiptCumInvoice.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mReceiptCumInvoice.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mReceiptCumInvoice.TransTypeID).TransText = "")) Then

                Dim str = "<script language='javascript'>openledgersame('index.aspx');</script>"

                Session("BackPagestr_ForTransSeries") = str

                Session("TransName_ForTransSeries") = "Receipt Cum Invoice"
                Session("TransTypeID_ForTransSeries") = mReceiptCumInvoice.TransTypeID
                Session("TransDate_ForTransSeries") = mReceiptCumInvoice.RecCumInvDateFormatted

                MSGBoxCtrl.Show("RCI Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "")
                Session("sender") = "RCITransTextSeriesAlert"
                txtDate.Text = mDate.ToString
                DataFieldBind()
                Exit Function
            Else
                Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                If mAutoRenewTransTextSeries.IsRenewed Then
                    With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mReceiptCumInvoice.TransTypeID)
                        mReceiptCumInvoice.InvText = .TransText
                        mReceiptCumInvoice.InvNo = .StartingTransNo
                    End With
                Else
                    Dim str = "<script language='javascript'>openledgersame('index.aspx');</script>"

                    Session("BackPagestr_ForTransSeries") = str

                    Session("TransName_ForTransSeries") = "Receipt Cum Invoice"
                    Session("TransTypeID_ForTransSeries") = mReceiptCumInvoice.TransTypeID
                    Session("TransDate_ForTransSeries") = mReceiptCumInvoice.RecCumInvDateFormatted

                    MSGBoxCtrl.Show("RCI Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "")
                    Session("sender") = "RCITransTextSeriesAlert"
                    txtDate.Text = mDate.ToString
                    DataFieldBind()
                    Exit Function
                End If
            End If
        Else
            Return True
        End If
        '------------------------------------------------===========================
    End Function
    '---------------------------------------------------------------------------------------------------------
    Public Function CreateAutoRCI(IssueBERPart As IssueBERPartList.IssueBERPartListInfo, Optional EffRateOfPart As Decimal = 0) As Boolean
        mDate = Session("mDate")
        Dim mPendingToReceiveTransItemList As PendingToReceiveTransItemList
        mReceiptCumInvoice = ReceiptCumInvoice.NewReceiptCumInvoice(Util.Trans.ExchangeRepairReceivedFromVendor)
        mReceiptCumInvoice.ReceiptCumInvoiceItems.Add(mReceiptCumInvoice.ID)
        mReceiptCumInvoice.CurrencyID = IssueBERPart.CurrencyID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ConversionFactor = IssueBERPart.ConversionFactor 'mReceiptCumInvoice.ConversionFactor

        mReceiptCumInvoice.RecCumInvDate = mDate.ToString
        mReceiptCumInvoice.VendorID = IssueBERPart.VendorID
        'mReceiptCumInvoice.StoreID = IssueBERPart.FromStoreID
        mReceiptCumInvoice.FromTypeID = 14

        mReceiptCumInvoice.VendorName = IssueBERPart.VendorName

        mPendingToReceiveTransItemList = PendingToReceiveTransItemList.GetPendingToReceiveTransItemList(mReceiptCumInvoice.TransTypeID, mReceiptCumInvoice.VendorID, 0, mReceiptCumInvoice.RecCumInvDate.ToString, IssueBERPart.OrderID,
                                                                                                        OrderItemID:=IssueBERPart.OrderItemId.ToString, IsFromIssueBERParts:=True)

        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = mPendingToReceiveTransItemList(IssueBERPart.ItemID).Type

        If mPendingToReceiveTransItemList(IssueBERPart.ItemID).Type = 3 Then mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemID = mPendingToReceiveTransItemList(IssueBERPart.ItemID).OrderItemID
        If mPendingToReceiveTransItemList(IssueBERPart.ItemID).Type = 3 Then mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = ((EffRateOfPart / IssueBERPart.ConversionFactor) / IssueBERPart.OrderItemReceiptBalanceQty) 'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate

        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo = ""
        If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized = True Then
            mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo = IssueBERPart.SerialNo

            mLastWarrantyInformation = LastWarrantyInformation.GetLastWarrantyInformation(IssueBERPart.ItemID.ToString, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo)
            If mLastWarrantyInformation.Count > 0 Then
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsWarranty = mLastWarrantyInformation(0).IsWarranty
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays = mLastWarrantyInformation(0).WarrantyInDays
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate = mLastWarrantyInformation(0).WarrantyStartDateFormatted.ToString
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate = mLastWarrantyInformation(0).WarrantyExpiryDateFormatted.ToString
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CodeNo = mLastWarrantyInformation(0).CodeNo
                mLastWarrantyInformation = Nothing
            End If
            'PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP

        End If

        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemList(IssueBERPart.ItemID).UnitID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = IssueBERPart.OrderItemReceiptBalanceQty

        Dim mItemTypeList As PartTypeList = PartTypeList.GetPartTypeList()
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTypeID = mItemTypeList.GetFirstUnServiceablePartType().ID

        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Remark = "System generated Receipt is created to Issue the part to Discard as it is BER and is discarded as per supplier's recommendation."
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptDate = mPendingToReceiveTransItemList(IssueBERPart.ItemID).OriginalReceiptDate
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptTextNo = mPendingToReceiveTransItemList(IssueBERPart.ItemID).OriginalReceiptTextNo

        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteNo = IssueBERPart.ReleaseNoteNo
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteDate = IssueBERPart.ReleaseNoteDate
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = IssueBERPart.FromStoreID
        'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = 0
        'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CCommercialRate = IssueBERPart.CCommercialRate
        'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CCommercialRate = IssueBERPart.CEffRate
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CEffRate = ((EffRateOfPart / IssueBERPart.ConversionFactor) / IssueBERPart.OrderItemReceiptBalanceQty) 'IssueBERPart.CEffRate

        mReceiptCumInvoice.Invoice.CalculateTotal()

        Dim Remark As String = "System generated Receipt is created to Issue the part to Discard as it is BER and is discarded as per supplier's recommendation."
        mReceiptCumInvoice.StatusID = 2
        mReceiptCumInvoice.UserName = User.Identity.Name


        Try
            If mReceiptCumInvoice.IsValid Then

                mReceiptCumInvoice.ApplyEdit()
                mReceiptCumInvoice.Save()
                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                RCIBERDetail = "Receipt-Cum-Invoice : " + mReceiptCumInvoice.ReceiptNo + " Dated : " + mReceiptCumInvoice.RecCumInvDateFormatted '+ " from " + IssueBERPart.VendorName
                MarkLog(Util.Action.Authorize, "Received as Exchange / Repair from Supplier", RCIBERDetail + vbCrLf + Remark, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
                Return True
            Else
                Dim strMSG As String = ""
                If Not mReceiptCumInvoice.IsValid Then
                    For i As Integer = 0 To mReceiptCumInvoice.GetBrokenRulesCollection.Count - 1
                        strMSG = strMSG + mReceiptCumInvoice.GetBrokenRulesCollection(i).Description + "<Br>"
                    Next
                End If
                Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
                If Not mReceiptCumInvoice.ReceiptCumInvoiceItems.IsValid Then
                    For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
                        For i As Integer = 0 To mReceiptCumInvoiceItem.GetBrokenRulesCollection.Count - 1
                            strMSG = strMSG + mReceiptCumInvoiceItem.ItemName + " : " + mReceiptCumInvoiceItem.GetBrokenRulesCollection(i).Description + "<Br>"
                        Next
                    Next
                End If
                If strMSG.Trim <> "" Then
                    Session("strMSG") = strMSG
                    Return False
                    ''cvControlValidator.ErrorMessage = strMSG
                    ''cvControlValidator.IsValid = False
                End If
            End If
        Catch ex As SqlException
            If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                If InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Or InStr(ex.Message, "*15-TB02-CX07*", CompareMethod.Text) Or InStr(ex.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex.Message.Substring(ex.Message.IndexOf("PartNo.:")) + "Goods Receipt Qty can not be greater than Order / Issue Qty.", MsgBoxStyle.OkOnly, "")
                ElseIf InStr(ex.Message, "FKtabInvoiceChargetabCharge", CompareMethod.Text) Then
                    MSGBoxCtrl.Show("Goods Receipt Charge Deleted ! ", "Goods Receipt charge Not Available<Br><BR>Selected Charge is no longer exist in the Database <BR><BR> Remove Charge and try Again", " ", MsgBoxStyle.OkOnly, "")
                Else
                    MSGBoxCtrl.Show("Goods Receipt is not Saved !", "", ex.Message, MsgBoxStyle.OkOnly, "")
                End If
            End If
            txtDate.Text = mDate.ToString
            Return False
        Catch ex1 As Exception
            If InStr(ex1.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Or InStr(ex1.Message, "*15-TB02-CX07*", CompareMethod.Text) Or InStr(ex1.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "<br>Goods Receipt Qty can not be greater than Order / Issue Qty.", MsgBoxStyle.OkOnly, "")
                '-------------------------------------------------------
                Session("sender") = "Status"
                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                txtDate.Text = mDate.ToString
                DataFieldBind()
                '-------------------------------------------------------
            Else
                MSGBoxCtrl.Show("Save Alert !", "There is some problem in Saving Goods Receipt. <BR> <BR>  Please Check the Entry and Try Again  !", "", MsgBoxStyle.OkOnly, "")
                '-------------------------------------------------------
                Session("sender") = "Status"
                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                txtDate.Text = mDate.ToString
                DataFieldBind()
                '-------------------------------------------------------
            End If
            Return False
        Finally
            ''
        End Try
    End Function
    '--------------------------------------------------------------------------
    Private Function CreateAutoIssueTransTextSeries() As Boolean 'Added By Prashant 12-Mar-2014 'ALL12032014
        mDate = Session("mDate")
        Dim mIssue As Issue = Issue.NewIssue(Util.Trans.DisacrdPart)
        mIssue.IDate = mDate.ToString

        '-------------------------------------------------------------------------====================
        If (mIssue.IsNew) And (mIssue.Text = "") Then

            Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mIssue.TransTypeID, mIssue.IDateFormatted)

            If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mIssue.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mIssue.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mIssue.TransTypeID).TransText = "")) Then

                Dim str = "<script language='javascript'>openledgersame('index.aspx');</script>"

                Session("BackPagestr_ForTransSeries") = str

                Session("TransName_ForTransSeries") = "Issue"
                Session("TransTypeID_ForTransSeries") = mIssue.TransTypeID
                Session("TransDate_ForTransSeries") = mIssue.IDateFormatted
                Session("AddTransTextSeries") = "True"

                MSGBoxCtrl.Show("Issue Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "")
                Session("sender") = "IssueTransTextSeriesAlert"
                txtDate.Text = mDate.ToString
                DataFieldBind()
                Exit Function
            Else
                Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                If mAutoRenewTransTextSeries.IsRenewed Then
                    With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mIssue.TransTypeID)
                        mIssue.Text = .TransText
                        mIssue.No = .StartingTransNo
                    End With
                Else
                    Dim str = "<script language='javascript'>openledgersame('index.aspx');</script>"

                    Session("BackPagestr_ForTransSeries") = str

                    Session("TransName_ForTransSeries") = "Issue"
                    Session("TransTypeID_ForTransSeries") = mIssue.TransTypeID
                    Session("TransDate_ForTransSeries") = mIssue.IDateFormatted
                    Session("AddTransTextSeries") = "True"

                    MSGBoxCtrl.Show("Issue Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "")
                    Session("sender") = "IssueTransTextSeriesAlert"
                    txtDate.Text = mDate.ToString
                    DataFieldBind()
                    Exit Function

                End If
            End If
        Else
            Return True
        End If
        '-------------------------------------------------------------------------====================
    End Function
    'Added By Vikrant On 02-Jun-2014
    Private Sub AttachMyFile()
        Try
            mIssue.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
            mIssue.Size = Session("FileUpload.FileSize")
            mIssue.Extension = Session("FileUpload.FileExtension")
            Session("mIssue") = mIssue
            Session.Remove("FileUpload.FileSize")
            Session.Remove("FileUpload.FileContent")
            Session.Remove("FileUpload.FileExtension")
        Catch ex As Exception
            MSGBoxCtrl.Show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
        End Try
    End Sub
    'End
    Private Function CreateAutoIssue(Optional EffRateOfPart As Decimal = 0) As Boolean 'Added By Saylee on 17-Oct-2012
        mDate = Session("mDate")
        mReceiptCumInvoice = Session("mReceiptCumInvoice")
        Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
        'Dim mItemDetailForIssue As ItemDetailForIssue
        mIssue = Issue.NewIssue(Util.Trans.DisacrdPart)
        mIssue.IDate = mDate.ToString
        mIssue.VendorID = mReceiptCumInvoice.VendorID
        For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
            mIssue.IssueItems.Add(mIssue.ID, mIssue.TransTypeID)
            mIssue.IssueItems.CurrentItem.ReceiptItemID = mReceiptCumInvoiceItem.ID
            mIssue.IssueItems.CurrentItem.DisplayQty = mReceiptCumInvoiceItem.Qty
            'Commented and Added By Prashant 3-Apr-2014 'ALL03042014
            'mIssue.IssueItems.CurrentItem.DiscardAmt = mReceiptCumInvoiceItem.CCommercialRate
            'Commented and added By Prashant 'Added By Prashant 4-Nov-2014  ALL04112014
            ' mIssue.IssueItems.CurrentItem.DiscardAmt = (mReceiptCumInvoiceItem.CEffRate * mIssue.IssueItems.CurrentItem.DisplayQty)
            'Commented and added by Prashant 4-Feb-2016 
            'mIssue.IssueItems.CurrentItem.DiscardAmt = (mReceiptCumInvoiceItem.EffRate * mIssue.IssueItems.CurrentItem.DisplayQty) 
            mIssue.IssueItems.CurrentItem.DiscardAmt = EffRateOfPart
            '-----------------------------------
            'Commented By Prashant 30-Mar-2016 No need to save Order Item ID form here for Issue to discard.
            'mIssue.IssueItems.CurrentItem.OrderItemID = mReceiptCumInvoiceItem.OrderItemID  
            mIssue.IssueItems.CurrentItem.Remark = "System generated Issue to Discard as Part is BER and is discarded at supplier's place as per supplier recommendation"
        Next
        AttachMyFile() 'Added By Vikrant On 02-Jun-2014
        mIssue.StoreID = mReceiptCumInvoiceItem.StoreID
        mIssue.UserName = User.Identity.Name
        mIssue.CalculateTotal()
        mIssue.MachineID = Guid.Empty
        mIssue.ToStoreID = Guid.Empty
        mIssue.WorkShopID = Guid.Empty
        mIssue.nWOID = Guid.Empty
        mIssue.UserName = User.Identity.Name
        'mIssue.Remark = "System generated Issue to Discard as Part is BER and is discarded at supplier's place as per supplier recommendation"
        Remark = "System generated Issue to Discard as Part is BER and is discarded at supplier's place as per supplier recommendation."
        mIssue.StatusID = 2


        Try
            If mIssue.IsValid Then
                mIssue.Save()
                Session("mIssue") = mIssue
                IssueBERDetail = "Issue : " + mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted '+ " to " + mReceiptCumInvoice.VendorName
                MarkLog(Util.Action.Authorize, "Issue to Supplier for Exchange / Repair", IssueBERDetail + vbCrLf + mIssue.Remark, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                Return True
            Else
                Dim strMSG As String = ""
                If Not mIssue.IsValid Then
                    For i As Integer = 0 To mIssue.GetBrokenRulesCollection.Count - 1
                        strMSG = strMSG + mIssue.GetBrokenRulesCollection(i).Description + "<Br>"
                    Next
                End If
                Dim mIssueItem As IssueItem
                If Not mIssue.IssueItems.IsValid Then
                    For Each mIssueItem In mIssue.IssueItems
                        For i As Integer = 0 To mIssueItem.GetBrokenRulesCollection.Count - 1
                            strMSG = strMSG + mIssueItem.ItemName + " : " + mIssueItem.GetBrokenRulesCollection(i).Description + "<Br>"
                        Next
                    Next
                End If
                If strMSG.Trim <> "" Then
                    Session("strMSG") = strMSG
                    ''cvControlValidator.ErrorMessage = strMSG
                    ''cvControlValidator.IsValid = False
                    Return False
                End If
            End If
        Catch ex1 As Exception

            If InStr(ex1.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Stock Qty.", MsgBoxStyle.OkOnly, "")
                Session("sender") = "Status"
                Session("mIssue") = mIssue
                DataFieldBind()
            ElseIf InStr(ex1.Message, "CCtabnWOSpareFromWOJobSparePendingIssuedQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabnWOToolsPendingIssuedQty", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, "Issue Qty can not be greater than Required Qty.", MsgBoxStyle.OkOnly, "")
                Session("sender") = "Status"
                Session("mIssue") = mIssue
                DataFieldBind()
            ElseIf InStr(ex1.Message, "CCtabReceiptItemLoanQty", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Receipt Qty.", MsgBoxStyle.OkOnly, "")
            ElseIf InStr(ex1.Message, "CCtabSalesOrderItemIssueBalQty", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Sales Order Qty.", MsgBoxStyle.OkOnly, "")
            ElseIf InStr(ex1.Message, "CCtabOrderItemEROQty", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Issue Qty can not be greater than Exchange/Repair/Overhaul Qty.", MsgBoxStyle.OkOnly, "")
            End If
            Return False
        End Try
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mIssueBERPartList = IssueBERPartList.GetIssueBERPartList(txtDate.Text)
        dgBERPartList.DataSource = mIssueBERPartList
        Session("mIssueBERPartList") = mIssueBERPartList
        DataBind()
        Session("LookInType") = LookInType
        IssuePartNo = Trim(txtName.Text)
        IssueSerialNo = Trim(txtSerialNo.Text)
        mDate = txtDate.Text

        Session("IssuePartNo") = IssuePartNo
        Session("IssueSerialNo") = IssueSerialNo
        Session("IssueDescription") = IssueDescription
        Session("mDate") = mDate
        lblResult.Text = "List of BER Parts :" & mIssueBERPartList.Count & " Record(s) found "
        'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
        mTransactionListCount = TransactionListCount.GetTransactionListCountt(21)
        Session("mTransactionListCount") = mTransactionListCount
        'End
    End Sub
    Public Sub SendMail(Index As Integer, PartInfo As String, SerialNo As String, Rate As String, Remark As String)
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
        ' If AppSettings("MailsRequire") = "True" Then
        If mModuleList.Item("IssueListForBER").MailsRequire = True Then
            If User.Identity.Name.ToUpper = "BTPLADMIN" Or User.Identity.Name.ToUpper = "BYTZADMIN" Then ' BYTZADMIN For Deccan 'Added by Prashant 15-Oct-2019 
                'Do nothing
                Exit Sub
            End If
            Try
                Dim str As String


                str = str + ("<p><font face=""Calibri"">Following Part Is Discarded: </font></p> ")

                str = str + ("</body></html>")

                str = str + ("<p><font face=""Calibri"">")
                str = str + ("<b>Discarded By: </b> " & User.Identity.Name + " <b> on: </b> " + New SmartDate(Today.Date).FormattedText)
                str = str + ("</p></font>")

                str = str + ("<p><font face=""Calibri"">")
                str = str + ("<b>Part Info: </b> " & PartInfo)
                str = str + ("<b> Serial No.: </b> " & SerialNo)
                str = str + ("</p></font>")

                str = str + ("<p><font face=""Calibri"">")
                str = str + ("<b>Discard Amount: </b> " & Rate)
                str = str + ("</p></font>")

                str = str + ("<p><font face=""Calibri"">")
                str = str + ("<b>Remark: </b> " & Remark)
                str = str + ("</p></font>")

                SendMailFile.SendMailFile(Nothing, User.Identity.Name, Subject:="Discarded Part Information", Text:="", Info:=str, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"))
            Catch ex As Exception
            End Try
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        AddAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If Session("mDate") <> "" Then
                txtDate.Text = CDate(mDate.ToString).ToString(AppSettings("DateFormat"))
            Else
                txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            End If
            Session("MiddleFrame") = "wfIssueBERPart_Ajax.aspx"
            mType = Request.QueryString("Type")
            Session("mType") = mType
            DataFieldBind()
            SetControl()
        End If
    End Sub
    Private Sub dgBERPartList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgBERPartList.PageIndexChanging
        dgBERPartList.PageIndex = e.NewPageIndex
        dgBERPartList.DataSource = mIssueBERPartList
        Session("mIssueBERPartList") = mIssueBERPartList
        dgBERPartList.DataBind()
    End Sub
    Private Sub dgBERPartList_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgBERPartList.RowCommand
        Select Case e.CommandName
            Case "Discard"
                mDate = txtDate.Text
                Session("mDate") = mDate
                'Added By Vikrant On 24-July-2014 For BA24072014
                If AppSettings("LockBackDatedTransaction") = "True" Then
                    If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then
                        'Do nothing
                    Else
                        If CheckDateForTransactionLock(CDate(mDate)) Then
                            MSGBoxCtrl.Show("Save Alert!", "Previous Months transactions can only be saved until " & DateSerial(Year(CDate(mDate).AddMonths(1)), Month(CDate(mDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "Kindly book this transaction in current month to reflect in Valuation.", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                    End If
                End If
                'End
                Index = CInt(e.CommandArgument) + dgBERPartList.PageSize * dgBERPartList.PageIndex
                mIssueChildID = mIssueBERPartList(Index).IssueChildID
                Session("Index") = Index
                Session("mIssueChildID") = mIssueChildID
                DiscardRecord()
        End Select
    End Sub
    Private Sub FindNow(sender As Object, e As ImageClickEventArgs) Handles btnFindNow.Click
        dgBERPartList.PageIndex = 0

        If (txtName.Text.Trim.IndexOf("[") >= 0 And txtName.Text.Trim.IndexOf("]") > 0) Then
            IssuePartNo = txtName.Text.Substring(0, txtName.Text.Trim.IndexOf("[")).Trim
            IssueDescription = Mid(txtName.Text.Trim, txtName.Text.Trim.IndexOf("[") + 2, txtName.Text.Trim.IndexOf("]") - txtName.Text.Trim.IndexOf("[") - 1).Trim
        Else
            IssuePartNo = Trim(txtName.Text.Trim)
            IssueDescription = Trim(txtName.Text.Trim)
        End If

        IssueSerialNo = Trim(txtSerialNo.Text)
        mDate = txtDate.Text

        Session("IssuePartNo") = IssuePartNo
        Session("IssueDescription") = IssueDescription
        Session("IssueSerialNo") = IssueSerialNo
        Session("mDate") = mDate

        FindNow(mDate, IssuePartNo, IssueSerialNo, IssueDescription)
        upnlGridView.Update()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "Issue BER Part", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        mIssueBERPartList = Nothing
        RemoveSession()
        Session.Remove("mDate")
        Session.Remove("IssuePartNo")
        Session.Remove("IssueSerialNo")
        Session.Remove("IssueDescription")
        Session("MiddleFrame") = ""
        Session("sender") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub txtName_TextChanged(sender As Object, e As EventArgs) Handles txtName.TextChanged
        If (txtName.Text.Trim.IndexOf("[") > 0 And txtName.Text.Trim.IndexOf("]") > 0) Then
            IssuePartNo = txtName.Text.Substring(0, txtName.Text.Trim.IndexOf("[")).Trim
            IssueDescription = Mid(txtName.Text.Trim, txtName.Text.Trim.IndexOf("[") + 2, txtName.Text.Trim.IndexOf("]") - txtName.Text.Trim.IndexOf("[") - 1).Trim
            LookInType = 1
            Session("LookInType") = LookInType
        Else
            IssuePartNo = Trim(txtName.Text)
            IssueDescription = Trim(txtName.Text)
            LookInType = 1
            Session("LookInType") = LookInType
        End If

        mItemList = ItemList.GetItemList(0, IssuePartNo, IssueDescription, "", "", "", "", False)
        Session("IssuePartNo") = IssuePartNo
        Session("IssueDescription") = IssueDescription

        'PartID = mItemList(IssuePartNo).ID.ToString
        PartID = IssuePartNo
        Session("PartID") = PartID
        txtSerialNo.Text = ""
    End Sub
    Private Sub txtDate_TextChanged(sender As Object, e As EventArgs) Handles txtDate.TextChanged
        dgBERPartList.PageIndex = 0

        If (txtName.Text.Trim.IndexOf("[") >= 0 And txtName.Text.Trim.IndexOf("]") > 0) Then
            IssuePartNo = txtName.Text.Substring(0, txtName.Text.Trim.IndexOf("[")).Trim
            IssueDescription = Mid(txtName.Text.Trim, txtName.Text.Trim.IndexOf("[") + 2, txtName.Text.Trim.IndexOf("]") - txtName.Text.Trim.IndexOf("[") - 1).Trim
        Else
            IssuePartNo = Trim(txtName.Text.Trim)
            IssueDescription = Trim(txtName.Text.Trim)
        End If

        IssueSerialNo = Trim(txtSerialNo.Text)
        mDate = txtDate.Text

        Session("IssuePartNo") = IssuePartNo
        Session("IssueDescription") = IssueDescription
        Session("IssueSerialNo") = IssueSerialNo
        Session("mDate") = mDate

        FindNow(mDate, IssuePartNo, IssueSerialNo, IssueDescription)
        upnlGridView.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click
        If (CreateAutoRCI(mIssueBERPartList(mIssueChildID), CType(Session("FileUpload.EffRateOfPart"), Decimal)) = True) And (CreateAutoIssue(CType(Session("FileUpload.EffRateOfPart"), Decimal)) = True) Then 'Added By Prashant 12-Mar-2014 'ALL12032014
            DataFieldBind()
            upnlGridView.Update()
            Session.Remove("ShowNotification")
            Session.Remove("EffRateOfPart")
            SendMail(Index, Session("PartInfo"), Session("SerialNo"), Session("FileUpload.EffRateOfPart"), Remark)
            MSGBoxCtrl.Show("BER Part Discarded Successfully!", "<BR>" + RCIBERDetail + "<BR> " + IssueBERDetail, "", MsgBoxStyle.OkOnly, "")
            Session.Remove("PartInfo")
            Session.Remove("SerialNo")
            Session.Remove("mDate")
            Session.Remove("IssuePartNo")
            Session.Remove("IssueSerialNo")
            Session.Remove("IssueDescription")
            Session.Remove("FileUpload.EffRateOfPart")
            Exit Sub
        End If
    End Sub
#End Region

End Class