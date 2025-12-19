'Created by Utkarsh ON 29-Nov-2013
Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic

Public Class wfrptPartHistory_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
    Public mATA As ATA
    Public mStoreList As StoreList
    Public PartNo As String = ""
    Public Description As String = ""
    Public ReleaseNoteNo As String = ""
    Public mType As Int16
    Public strCustomer, strStore, StoreName As String
    Dim mStoreID As Guid
    Dim mCustomerID As Guid
    Public LookInType As Integer = 2
    Public PartID As String = "{00000000-0000-0000-0000-000000000000}"
    Public EventLogDetails As String = String.Empty
    Public SerialNo As String = ""
    Public ATACode As String = ""
    Dim mLocation As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mType = Session("mType")
        PartID = Session("PartID")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mStoreList = CType(Session("mStoreList"), StoreList)
    End Sub
    Private Sub SetSession()
        Session("PartNo") = PartNo
        Session("Description") = Description
        Session("mStoreList") = mStoreList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mType")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mStoreList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblRelNoteNo.Visible = (mType = 4)
        lblSerialNo1.Visible = True
        upnlSerachCriteria.Update()
    End Sub
    Private Sub ATAChapter()
        SetPartID()
        mItem = Item.GetItem(New Guid(PartID))
        mLocation = mItem.Location
        If Not mItem.ATAID.Equals(Guid.Empty) Then
            mATA = ATA.GetATA(mItem.ATAID)
            ATACode = mATA.ATACode & " - " & mATA.ATANomenclature
        End If
    End Sub
    Private Sub SetValues()
        PartNo = IIf(PartNo <> "", PartNo, "")
        Description = IIf(Description <> "", Description, "")
        If txtRelNoteNo.Text.Trim <> "" Then
            ReleaseNoteNo = txtRelNoteNo.Text.Trim
        Else
            ReleaseNoteNo = ""
        End If
        If txtSerialNo.Text.Trim <> "" Then
            SerialNo = txtSerialNo.Text.Trim
        Else
            SerialNo = ""
        End If
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        lblCustomerName.Text = "Customer : " & txtCustomerList.Text.Trim
        mStoreID = New Guid(Request.Form("cmbStore")) 'Added By Utkarsh ON 26-Aug-2013 FOR ALL26082013
        lblStoreName.Text = "Store : " & mStoreList(mStoreID).Name
        SetCustomerID()
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblRelNoteNo.Text = "Release Note No. : " + IIf(ReleaseNoteNo <> "", ReleaseNoteNo, "All")
        lblSerialNo1.Text = "Serial No. : " + IIf(SerialNo <> "", SerialNo, "All")
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "")
        EventLogDetails = lblCustomerName.Text + ", " + lblStoreName.Text + ", " + "Include Valued Stores Only : " + IIf(chkCustomerStock.Checked, "True", "False") + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + lblRelNoteNo.Text + ", " + lblSerialNo1.Text
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean)
        Session("IsExcel") = IsExcel
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim mCompanyDetail As New CompanyDetail
        Dim rpt As rptPartHistory
        GetSession()
        If txtRelNoteNo.Text.Trim <> "" Then
            ReleaseNoteNo = txtRelNoteNo.Text.Trim
        Else
            ReleaseNoteNo = ""
        End If
        If txtSerialNo.Text.Trim <> "" Then
            SerialNo = txtSerialNo.Text.Trim
        Else
            SerialNo = ""
        End If
        Dim ds As New dsPartHistory
        'Bin Card History
        If cmbFormat.SelectedIndex = 0 Then   'Added By Utkarsh ON 21-Feb-2013 FOR All20022013
            If AppSettings("ClientCode") = "Heligo" Then
                myReport = New crptBinCardReportHeligo '5
            ElseIf AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "Taj" Then 'Added By Ajay ON  ClientCode Taj 18-07-2022 
                myReport = New crptBinCardReportIND
            Else
                myReport = New crptBinCardReport '6
            End If
        ElseIf cmbFormat.SelectedIndex = 1 Then   'Added By Prashant ON 12-Jun-2013 FOR YA12062013   
            If AppSettings("ClientCode") = "Taj" Then
                myReport = New crptBinCardReportINDWithRate
            Else
                myReport = New crptBinCardReportFormat2   'Added By Utkarsh ON 21-Feb-2013 FOR All20022013
            End If

        ElseIf cmbFormat.SelectedIndex = 2 Then   'Added By Prashant ON 12-Jun-2013 FOR YA12062013   
            If AppSettings("ClientCode") = "CE" Then
                myReport = New crptBinCardReportWithPartStatusForCHM
            Else
                myReport = New crptBinCardReportWithPartStatus
            End If
        End If

        ''Added By Prashant ON 12-Mar-2013 FOR All12032013
        Dim RateValue As String = ""
        Dim ReportName As String = ""
        If rdoBase.Checked = True Then
            RateValue = "Base Value"
            ReportName = " Bin Card Report (Base Value)"
        ElseIf rdoLanding.Checked = True Then
            RateValue = "Landing Value"
            ReportName = " Bin Card Report (Landing Value)"
        Else
            RateValue = "Commercial Value"
            ReportName = " Bin Card Report (Commercial Value)"
        End If
        'End
        If PartNo = "" And Description = "" Then
            MSGBoxCtrl.show(MSGBox.Message_title.CurrentlySelected, MSGBox.Message_text.CurrentlySelected, "Please Select the item from the List", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        rpt = rptPartHistory.GetPartHistory(PartNo, Description, mType, ReleaseNoteNo, mCustomerID, mStoreID, chkCustomerStock.Checked, chkIsValued.Checked, RateValue, SerialNo:=SerialNo)
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 706)
            EventLogDetails = EventLogDetails + ", Format : " + cmbFormat.SelectedItem.Text + IIf(cmbFormat.SelectedIndex = 1, "-" + RateValue, "")
            MarkLog(Util.Action.Print, "PartBinCardHistory", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
        'Added By Prashant ON 25-Feb-2013 FOR All20022013
        Dim AlternateParts As String = ""
        If rpt.Count > 0 Then
            For i As Integer = 0 To rpt.Count - 1
                If rpt(i).AlternateParts <> "" Then
                    AlternateParts = rpt(i).AlternateParts
                End If
            Next
        End If
        '-------------------------------- FOR All20022013

        'Added By Utkarsh ON 22-Apr-2013 FOR BA-22042013-1
        Dim Applicability As String = ""
        If rpt.Count > 0 Then
            For i As Integer = 0 To rpt.Count - 1
                If rpt(i).Applicability <> "" Then
                    Applicability = rpt(i).Applicability
                End If
            Next
        End If
        'End
        ATAChapter()
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", PartNo, AlternateParts, _
                                                              AppSettings("Logo"), Applicability, ATACode, IIf(mStoreList(mStoreID).Name = "(All)", "", mStoreList(mStoreID).Name), _
                                                              RateValue, ReportName, Description, ReleaseNoteNo, 0, SerialNo, Item.GetItem(New Guid(PartID)).UnitName)

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "", mLocation, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        ds.Clear()
        If IsExcel = False Then
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
        End If
        da.Fill(ds, rpt)
        da.Fill(ds, objsearch)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
    End Sub
    Private Sub SetPage()
        lblIsValued.Text = "Step III. Selection of IsValued Store"
        Label2.Text = "Step IV. Selection of Part Number"
        lbltitle.Text = "Part History-Bin Card"
        lblRelNoteNo.Visible = True
        lblSerialNo1.Visible = True
        lblStep2.Visible = True
        btnClose.ToolTip = "Click to Close Part Bin Card History screen"
        lblReleaseNoteNo.Visible = True
        'lblNote.Visible = True
        txtRelNoteNo.Visible = True
        txtRelNoteNo.Enabled = True
        upnlTitle.Update()
        upnlDetails.Update()
    End Sub
    Private Sub ClearAll()
        mType = Session("mType")
        If Session("MiddleFrame") <> "wfrptPartHistory_Ajax.aspx?Type=" & mType Then
            RemoveSession()
        End If
    End Sub
    Private Sub LoadComobBox()
        cmbStore.DataSource = mStoreList
        cmbStore.DataBind()
    End Sub
    Private Sub SetPartID()
        If hdnpartId.Value <> String.Empty Then
            PartID = hdnpartId.Value.ToString
        End If
    End Sub
    Private Sub SetCustomerID()
        If (hdnCustomerID.Value <> String.Empty And chkCustomerStock.Checked = True) Then
            mCustomerID = New Guid(hdnCustomerID.Value.ToString)
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mStoreList = StoreList.GetStoreList(3, "", "(All)", True) 'Added By Prashant 30-Apr-2013 'ALL29042013
        Session("mStoreList") = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"
        LoadComobBox()
        LookInType = 2
        PartID = "{00000000-0000-0000-0000-000000000000}"
        Session("LookInType") = LookInType
        Session("PartID") = PartID
        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        custValidator.ControlToValidate = "txtsearch"
        If txtSearch.Text = "" Then
            e.IsValid = False
        ElseIf (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
            e.IsValid = False
        ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
            If PartNo = "" Or Description = "" Then
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            mType = Request.QueryString("Type")
            Session("mType") = mType
            Session("MiddleFrame") = "wfrptPartHistory_Ajax.aspx?Type=" & mType
            DataFieldBind()
            SetPage()
            'Ajay 08-Nov-2022
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "PartBinCardHistory") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
            End If
            '--------------------------
        End If
    End Sub
    Private Sub chkCustomerStock_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCustomerStock.CheckedChanged
        If chkCustomerStock.Checked = True Then
            lblCustomer.Enabled = True
            txtCustomerList.Enabled = True
            If txtCustomerList.Text.Trim <> "" Then
                SetCustomerID()
                mStoreList = StoreList.GetStoreList(mCustomerID, "(All)", True) 'Added By Prashant 30-Apr-2013 'ALL29042013
            Else
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)       'All  'Added By Prashant 30-Apr-2013 'ALL29042013
            End If
        Else
            LookInType = 2
            lblCustomer.Enabled = False
            txtCustomerList.Text = ""
            txtCustomerList.Enabled = False 'VVVVVVVVVV
            mStoreList = StoreList.GetSelfStoreList("", "(All)", True)         'Self 'All  'Added By Prashant 30-Apr-2013 'ALL29042013
            mCustomerID = Guid.Empty
            hdnCustomerID.Value = String.Empty
        End If
        Session("mStoreList") = mStoreList
        LoadComobBox()
    End Sub
    Private Sub txtCustomerList_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCustomerList.TextChanged
        If chkCustomerStock.Checked Then
            If txtCustomerList.Text.Trim <> "" Then
                SetCustomerID()                'If Customer Selected
                mStoreList = StoreList.GetStoreList(mCustomerID, "(All)", True) 'Passing selected customer  'Added By Prashant 30-Apr-2013 'ALL29042013
            Else
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)       'All 'Added By Prashant 30-Apr-2013 'ALL29042013
            End If
            Session("mStoreList") = mStoreList
        End If
        LoadComobBox()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            SetValues()
            SetReport(False)
        Else
            upnlValidations.Update()
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Page.IsValid Then
            Dim da As New CSLA.Data.ObjectAdapter
            Dim rpt As rptPartHistory
            Dim objsearch As rptSearchingCriteria
            SetValues()
            SetPartID()
            Dim Value As String = ""
            Dim ReportName As String = ""
            Dim RateValue As String = ""
            If rdoBase.Checked = True Then
                RateValue = "Base Value"
                ReportName = " Bin Card Report (Base Value)"
            ElseIf rdoLanding.Checked = True Then
                RateValue = "Landing Value"
                ReportName = " Bin Card Report (Landing Value)"
            Else
                RateValue = "Commercial Value"
                ReportName = " Bin Card Report (Commercial Value)"
            End If
            rpt = rptPartHistory.GetPartHistory(PartNo, Description, mType, ReleaseNoteNo, mCustomerID, mStoreID, chkCustomerStock.Checked, chkIsValued.Checked, RateValue, SerialNo:=SerialNo)
            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            Dim AlternateParts As String = ""
            If rpt.Count > 0 Then
                For i As Integer = 0 To rpt.Count - 1
                    If rpt(i).AlternateParts <> "" Then
                        AlternateParts = rpt(i).AlternateParts
                    End If
                Next
            End If

            Dim Applicability As String = ""
            If rpt.Count > 0 Then
                For i As Integer = 0 To rpt.Count - 1
                    If rpt(i).Applicability <> "" Then
                        Applicability = rpt(i).Applicability
                    End If
                Next
            End If
            'End
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", PartNo, AlternateParts, _
                                                                  AppSettings("Logo"), Applicability, "", IIf(mStoreList(mStoreID).Name = "(All)", "", mStoreList(mStoreID).Name), _
                                                                  RateValue, ReportName, Description, ReleaseNoteNo, 0, SerialNo, WorkShop:=Item.GetItem(New Guid(PartID)).UnitName)
            Dim dtrptSearchingCriteria As New DataTable
            dtrptSearchingCriteria.TableName = "rptSearchingCriteria"
            dtrptSearchingCriteria.Columns.Add("Part Number", System.Type.GetType("System.String"))
            dtrptSearchingCriteria.Columns.Add("Description ", System.Type.GetType("System.String"))
            dtrptSearchingCriteria.Columns.Add("Store", System.Type.GetType("System.String"))
            dtrptSearchingCriteria.Columns.Add("Serial No.", System.Type.GetType("System.String"))
            dtrptSearchingCriteria.Columns.Add("Rel. Note No.", System.Type.GetType("System.String"))
            dtrptSearchingCriteria.Columns.Add("Alternate Part(s)", System.Type.GetType("System.String"))
            dtrptSearchingCriteria.Columns.Add("Applicability", System.Type.GetType("System.String"))
            dtrptSearchingCriteria.Columns.Add("WorkShop", System.Type.GetType("System.String")).ColumnName = "Unit"
            dtrptSearchingCriteria.Rows.Add(objsearch(0).PartNo, objsearch(0).Description, objsearch(0).Store, objsearch(0).FromStore, _
                                            objsearch(0).RelNoteNo, objsearch(0).SupplierName, objsearch(0).Category, objsearch(0).WorkShop)
            Dim dt As New DataTable
            dt.TableName = "rptPartHistory"
            dt.Columns.Add("Date", System.Type.GetType("System.String"))
            dt.Columns.Add("Transaction", System.Type.GetType("System.String"))
            dt.Columns.Add("Type", System.Type.GetType("System.String"))
            dt.Columns.Add("Transaction No.", System.Type.GetType("System.String"))
            dt.Columns.Add("From", System.Type.GetType("System.String"))
            dt.Columns.Add("To", System.Type.GetType("System.String"))
            dt.Columns.Add("Supp. Inv. No", System.Type.GetType("System.String"))
            dt.Columns.Add("Supp. Inv. Date", System.Type.GetType("System.String"))
            dt.Columns.Add("Rel. Note No", System.Type.GetType("System.String"))
            dt.Columns.Add("Rel. Note Date", System.Type.GetType("System.String"))
            dt.Columns.Add("Serial No.", System.Type.GetType("System.String"))
            dt.Columns.Add("Location", System.Type.GetType("System.String"))
            dt.Columns.Add("Batch No.", System.Type.GetType("System.String"))
            dt.Columns.Add("Rate", System.Type.GetType("System.Decimal"))
            dt.Columns.Add("In Qty.", System.Type.GetType("System.Decimal"))
            dt.Columns.Add("Out Qty.", System.Type.GetType("System.Decimal"))
            dt.Columns.Add("BalQty", System.Type.GetType("System.Decimal"))
            dt.Columns.Add("Store", System.Type.GetType("System.String"))
            For i As Integer = 0 To rpt.Count - 1 'ds.Tables("rptPartHistory").Rows.Count - 1
                Dim TotBalQty As Decimal = (TotBalQty) + rpt(i).RecQty - rpt(i).IssQty
                If AppSettings("ClientCode") = "Taj" Then
                    dt.Rows.Add(rpt(i).PHDate, rpt(i).TransType, rpt(i).TransTypeName, rpt(i).IdentityNo, rpt(i).ToFrom, rpt(i).IssueTo, rpt(i).VendorInvoiceNo, rpt(i).VendorInvoiceDate, rpt(i).ReleaseNoteNo, rpt(i).ReleaseNoteDate, rpt(i).SerialNo, rpt(i).Location, rpt(i).BatchNo, rpt(i).EffRate, rpt(i).RecQty, rpt(i).IssQty, TotBalQty, rpt(i).Store)
                Else
                    dt.Rows.Add(rpt(i).PHDate, rpt(i).TransType, rpt(i).TransTypeName, rpt(i).IdentityNo, rpt(i).ToFrom, rpt(i).IssueTo, rpt(i).VendorInvoiceNo, rpt(i).VendorInvoiceDate, rpt(i).ReleaseNoteNo, rpt(i).ReleaseNoteDate, rpt(i).SerialNo, rpt(i).Location, rpt(i).BatchNo, rpt(i).EffRate, rpt(i).RecQty, rpt(i).IssQty, TotBalQty)
                End If

            Next

            Dim dsNew As New DataSet
            dsNew.Clear()
            dsNew.Tables.Add(dtrptSearchingCriteria)
            dsNew.Tables.Add(dt)
            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
			dsNew.Tables("rptPartHistory").TableName = ReportName
			Session("ExcelFileName") = ReportName
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "PartBinCardHistory", "Export To excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        Else
            upnlValidations.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session.Remove("PartID")
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    'Added By Utkarsh ON 04-Nov-2012 FOR ALL05112012
    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        If Page.IsValid Then
            SetValues()
            SetPartID()
            Dim ParameterValues As Hashtable = New Hashtable
            ParameterValues.Add("PartNo", PartNo)
            ParameterValues.Add("Description", Description)
            ParameterValues.Add("ReleaseNoteNo", ReleaseNoteNo)
            ParameterValues.Add("CustomerID", mCustomerID)
            ParameterValues.Add("StoreID", mStoreID)
            ParameterValues.Add("IsCustomerStore", chkCustomerStock.Checked)
            ParameterValues.Add("IsValuedStore", chkIsValued.Checked)
            ParameterValues.Add("PartID", PartID)
            ParameterValues.Add("SerialNo", SerialNo)
            ParameterValues.Add("WithAlternateParts", chkCheckForAlternatePart.Checked)
            Session("ParameterValues") = ParameterValues
            Dim str As String
            mItem = Item.GetItem(New Guid(PartID))
            If (mItem.PrimaryCategoryID = 1 Or mItem.PrimaryCategoryID = 2) Then
                str = "openledgersame('wfStockCard_Ajax.aspx?');"
            Else
                str = "openledgersame('wfStockCardExpendable_Ajax.aspx?');"
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        Else
            upnlValidations.Update()
        End If
    End Sub
    'Private Sub btnPreview1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview1.Click
    '    SetValues()
    '    SetPartID()
    '    Dim ParameterValues As Hashtable = New Hashtable
    '    ParameterValues.Add("PartNo", PartNo)
    '    ParameterValues.Add("Description", Description)
    '    ParameterValues.Add("ReleaseNoteNo", ReleaseNoteNo)
    '    ParameterValues.Add("CustomerID", mCustomerID)
    '    ParameterValues.Add("StoreID", mStoreID)
    '    ParameterValues.Add("IsCustomerStore", chkCustomerStock.Checked)
    '    ParameterValues.Add("IsValuedStore", chkIsValued.Checked)
    '    ParameterValues.Add("PartID", PartID)

    '    Session("ParameterValues") = ParameterValues
    '    Dim str As String
    '    str = "openledgersame('wfStockCardExpendable_Ajax.aspx?');"
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    'End Sub
    'End
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    'Ajay 08-Nov-2022
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 08-Nov-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "PartBinCardHistory")
    End Sub

    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 08-Nov-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "PartBinCardHistory")
    End Sub
    '-----
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetPartNoDescriptionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim itemlist As ItemListAutoComplete
        itemlist = ItemListAutoComplete.GetItemList(prefixText, False)
        If count = 0 Then
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).ToArray
        Else
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCustomerList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim type As String = contextKey.Split("=")(1)
        Dim mVendorListAutoComplete As VendorListAutoComplete = VendorListAutoComplete.GetVendorListAutoComplete(prefixText, type)
        If count = 0 Then
            Return (From c As VendorListAutoComplete.VendorListAutoCompleteInfo In mVendorListAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.VendorID.ToString())).ToArray
        Else
            Return (From c As VendorListAutoComplete.VendorListAutoCompleteInfo In mVendorListAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.VendorID.ToString())).Take(count).ToArray
        End If
    End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetReleNoteNoList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim partID As String = contextKey.Split("=")(1)
        Dim mRelNoteNolist As ReleaseNoteNoList = ReleaseNoteNoList.GetReleaseNoteNoList(New Guid(partID))

        If count = 0 Then
            Return (From c As ReleaseNoteNoList.ReleaseNoteNoInfo In mRelNoteNolist
               Select c.ReleaseNoteNo).ToArray
        Else
            Return (From c As ReleaseNoteNoList.ReleaseNoteNoInfo In mRelNoteNolist
               Select c.ReleaseNoteNo).Take(count).ToArray
        End If
    End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetSerialNo(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim partID As String = contextKey.Split("=")(1)
        Dim mItem As Item = Item.GetItem(New Guid(partID))
        Dim mSerialNoListAutoComplete As SerialNoListAutoComplete = SerialNoListAutoComplete.GetSerialNoList(prefixText, mItem.Name, 1)
        If count = 0 Then
            Return (From c As SerialNoListAutoComplete.SerialNoListAutoCompleteInfo In mSerialNoListAutoComplete Select c.SerialNo).ToArray
        Else
            Return (From c As SerialNoListAutoComplete.SerialNoListAutoCompleteInfo In mSerialNoListAutoComplete
               Select c.SerialNo).Take(count).ToArray
        End If
    End Function
#End Region
End Class