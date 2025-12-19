Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Public Class wfrptPartHistoryOfAllParts_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
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
    Dim email As Thread
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mType = Session("mType")
        PartID = Session("PartID")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mStoreList = CType(Session("mStoreList"), StoreList)
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mType")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mStoreList")
    End Sub
    Private Sub Display()
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblRelNoteNo.Visible = (mType = 4)
        lblSerialNo1.Visible = True
        upnlSerachCriteria.Update()
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
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False, Optional ByVal ByMail As Boolean = False)
        Session("IsExcel") = IsExcel
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As rptfetchPartHistoryBinCardOfAllParts
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
        Dim ds As New dsPartHistoryBinCardOfAllParts
        'Bin Card History

        If cmbFormat.SelectedIndex = 0 Then
            myReport = New crptBinCardReportForAllParts
        ElseIf cmbFormat.SelectedIndex = 1 Then
            myReport = New crptBinCardReportWithRateForAllParts
        End If

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
        rpt = rptfetchPartHistoryBinCardOfAllParts.GetPartHistory(PartNo, Description, mType, ReleaseNoteNo, mCustomerID, mStoreID, chkCustomerStock.Checked, chkIsValued.Checked, RateValue, SerialNo:=SerialNo, FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text)
            If ByMail = False Then
            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 706)
                EventLogDetails = EventLogDetails + ", Format : " + cmbFormat.SelectedItem.Text + IIf(cmbFormat.SelectedIndex = 1, "-" + RateValue, "")
                MarkLog(Util.Action.Print, "PartBinCardHistory", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            End If
        End If
        If (ByMail = True And rpt.Count <= 0) Then
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "", _
                Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                ReportGeneratedBy:=Session("ReportGenratedBy"), _
                SmtpHost:=mModuleList.Item("PartBinCardHistoryOfAllParts").SmtpHost, SmtpPort:=mModuleList.Item("PartBinCardHistoryOfAllParts").SmtpPort, _
                SmtpUser:=mModuleList.Item("PartBinCardHistoryOfAllParts").SmtpUser, SmtpPassword:=mModuleList.Item("PartBinCardHistoryOfAllParts").SmtpPassword)
            Exit Sub
        End If
        'Added By Prashant ON 25-Feb-2013 FOR All20022013
        Dim AlternateParts As String = ""
        'If rpt.Count > 0 Then
        '    For i As Integer = 0 To rpt.Count - 1
        '        If rpt(i).AlternateParts <> "" Then
        '            AlternateParts = rpt(i).AlternateParts
        '        End If
        '    Next
        'End If
        '-------------------------------- FOR All20022013

        'Added By Utkarsh ON 22-Apr-2013 FOR BA-22042013-1
        Dim Applicability As String = ""
        'If rpt.Count > 0 Then
        '    For i As Integer = 0 To rpt.Count - 1
        '        If rpt(i).Applicability <> "" Then
        '            Applicability = rpt(i).Applicability
        '        End If
        '    Next
        'End If
        'End

        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), txtFromDate.Text, txtToDate.Text, PartNo, AlternateParts, AppSettings("Logo"), Applicability, "", IIf(mStoreList(mStoreID).Name = "(All)", "", mStoreList(mStoreID).Name), RateValue, ReportName, Description, ReleaseNoteNo, 0, SerialNo)

        ds.Clear()
        If IsExcel = False Then
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
        End If
        da.Fill(ds, rpt)
        da.Fill(ds, objsearch)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
       If ByMail = False Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        Else
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "", "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, _
                                      Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                SmtpHost:=mModuleList.Item("PartBinCardHistoryOfAllParts").SmtpHost, SmtpPort:=mModuleList.Item("PartBinCardHistoryOfAllParts").SmtpPort, _
                SmtpUser:=mModuleList.Item("PartBinCardHistoryOfAllParts").SmtpUser, SmtpPassword:=mModuleList.Item("PartBinCardHistoryOfAllParts").SmtpPassword)
        End If
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
        lbltitle.Text = "Part History-Bin Card"
        lblRelNoteNo.Visible = True
        lblSerialNo1.Visible = True
        lblStep2.Visible = True
        btnClose.ToolTip = "Click to Close Part Bin Card History screen"
        txtRelNoteNo.Visible = True
        txtRelNoteNo.Enabled = True
        upnlTitle.Update()
        upnlDetails.Update()
    End Sub
    Private Sub ClearAll()
        mType = Session("mType")
        If Session("MiddleFrame") <> "wfrptPartHistoryOfAllParts_Ajax.aspx?Type=" & mType Then
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
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            mType = Request.QueryString("Type")
            Session("mType") = mType
            Session("MiddleFrame") = "wfrptPartHistoryOfAllParts_Ajax.aspx?Type=" & mType
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
            SetPage()
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
            SetReport(False, False)
        Else
            upnlTitle.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session.Remove("PartID")
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            email = New Thread(Sub() SetReport(False, True))
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
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        If Page.IsValid Then

            'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
            'Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

            Session("UserEmailID") = mModuleList.Item("PartBinCardHistoryOfAllParts").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("PartBinCardHistoryOfAllParts").SendCCMailID
            '--------------------------
            Dim Str As String
            Str = "OpenByMaiWindow();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
        Else
            upnlTitle.Update()
        End If
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
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