Imports System.Text
Public Class wfrptKit_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
    Public mKitList As KitList
    Public PartNo As String
    Public Description As String
    Public strKit As String
    Public mType As Int16
    Public ToStore As String = ""
    Public mStoreList As StoreList
    Dim mStoreID As Guid
    Dim mKitSearchingCriteria As String = String.Empty

    Dim da As New CSLA.Data.ObjectAdapter
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim objsearch As rptSearchingCriteria
    Dim rpt As rptKitList
    Dim ds As New dsKit
    Dim StoreIDList, StoreNameList As String
    Dim StrStore As String
    Dim StoreIDXML As New StringBuilder
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mKitList = CType(Session("mKitList"), KitList)
        mStoreList = CType(Session("mStoreList"), StoreList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        mType = Session("mType")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        ToStore = Session("ToStore")
    End Sub
    Private Sub SetSession()
        Session("mKitList") = mKitList
        Session("mStoreList") = mStoreList
        Session("mtype") = mType
        Session("PartNo") = PartNo
        Session("Description") = Description
        Session("ToStore") = ToStore
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mKitList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mType")
        Session.Remove("mStoreList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblKitName.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblToStore.Visible = False
    End Sub
    Private Sub SetValues()
        lblDateRangeFrom.Text = "As On Date : " & New SmartDate(txtAsOnDate.Text.ToString).FormattedText

        strKit = IIf(cmbKitList.SelectedIndex > 0, cmbKitList.SelectedItem.Text, "") 'cmbKitList.SelectedItem.Text '  Trim(txtKitName.Text)
        StoreIDList = hdnStoreIDList.Value
        StoreNameList = hdnStoreNameList.Value
        StrStore = IIf(StoreNameList = String.Empty, "All", StoreNameList)

        If mType = 2 Then
            lblKitName.Text = "Kit Name   :" & IIf(strKit <> "", strKit, "All")
        Else
            lblKitName.Text = "Inspection   :" & IIf(strKit <> "", strKit, "All")
        End If
        lblToStore.Text = "Store : " & StrStore
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        mKitSearchingCriteria = lblDateRangeFrom.Text + ", " + lblKitName.Text + ", " + lblToStore.Text + ", " + PartNo + ", " + Description
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rptkitPartlist As rptKitParts
        Dim rpt As rptKitList
        SetValues()

        If StoreIDList.ToString <> "" Then
            StoreIDXML.Append("<StoreIDs>")
            For Each value As String In StoreIDList.Split(",")
                StoreIDXML.Append("<ID>")
                StoreIDXML.Append(value)
                StoreIDXML.Append("</ID>")
            Next
            StoreIDXML.Append("</StoreIDs>")
        End If

        Dim ds As New dsKit
        If cmbFormat.SelectedIndex = 0 Then        'Format1
            If mType = 2 Then
                myReport = New crptKitList
            Else
                If chkForLocation.Checked = True Then
                    ' myReport = New crptInspectionList
                    myReport = New crptInspectionListLocation 'Wise
                Else
                    If AppSettings("ClientCode") = "BA" Then
                        myReport = New crptInspectionKitReportForBA
                    Else
                        myReport = New crptInspectionKitReport
                    End If

                End If
            End If
            rpt = rptKitList.GetKitList(strKit, CType(mType, Integer), StoreIDXML.ToString, chkForLocation.Checked, PartNo, Description, txtAsOnDate.Text.ToString, _
                                       chkConsiderAlternatePart.Checked, ClientCode:=AppSettings("ClientCode"))
            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 708)
            End If
            ds.Clear()
            da.Fill(ds, rpt)
        Else                            'Format2
            myReport = New crptInspectionKitReportForTaj
            rptkitPartlist = rptKitParts.GetKitParts(New Guid(cmbKitList.SelectedValue.ToString), , StoreIDXML.ToString, , PartNo, Description, txtAsOnDate.Text.ToString, chkConsiderAlternatePart.Checked, cmbFormat.SelectedIndex)
            If rptkitPartlist.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 708)
            End If
            ds.Clear()
            da.Fill(ds, "rptKitParts", rptkitPartlist)
        End If

        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), New SmartDate(txtAsOnDate.Text.ToString).FormattedText, "", PartNo, "", "", "", "", StoreNameList, "", strKit, Description, "", 0, "", "", chkConsiderAlternatePart.Checked, AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, objsearch)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(ds)

        Session("CrystalReport") = myReport
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
        MarkLog(Util.Action.Print, "KitReport", mKitSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()

        mKitList = KitList.GetKitList(0, "", "", "(All)")
        cmbKitList.DataSource = mKitList
        Session("mKitList") = mKitList

        mStoreList = StoreList.GetStoreList(0, "", , True)  'Added By Prashant 30-Apr-2012 'ALL29042013
        ChkStoreList.DataSource = mStoreList
        Session("mStoreList") = mStoreList

        lblStoreCount.Text = "You have " + (mStoreList.Count).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            RemoveSession()
            mType = Request.QueryString("Type")
            Session("mType") = mType
            If cmbKitList.Enabled = True Then
                setFocus(cmbKitList)
            End If
            txtAsOnDate.Text = New SmartDate(Now.Date.ToString).FormattedText
            DataFieldBind()
            ControlVisibility(2)
        End If
        'If mType = 2 Then
        '    lbltitle.Text = "Kit Report"
        '    lblStep1.Text = "Step I.Enter Kit Name"
        '    lblKit.Text = "Kit Name"
        'Else
        '    lbltitle.Text = "Inspection Kit Report"
        '    lblStep1.Text = "Step II.Enter Inspection Kit Name"
        '    lblKit.Text = "Inspection"
        'End If
        'MessageBoxResult()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblKitName.Visible = True
        lblToStore.Visible = True
        lblDateRangeFrom.Visible = True
        SetValues()
        upnlselection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()
            GenerateXLSXFile(CreateDataTable())
        End If
    End Sub
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("Inspection Kit")
        Dim conString As String = AppSettings("DB:FlyPal")
        Dim con = New SqlConnection(conString)

        If StoreIDList.ToString <> "" Then
            StoreIDXML.Append("<StoreIDs>")
            For Each value As String In StoreIDList.Split(",")
                StoreIDXML.Append("<ID>")
                StoreIDXML.Append(value)
                StoreIDXML.Append("</ID>")
            Next
            StoreIDXML.Append("</StoreIDs>")
        End If

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "ExcelrptfetchKitList"
        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.AddWithValue("@Kit", strKit)
        cmd.Parameters.AddWithValue("Type", CType(mType, Integer))
        cmd.Parameters.AddWithValue("@ItemName", PartNo)
        cmd.Parameters.AddWithValue("@Description", Description)
        cmd.Parameters.AddWithValue("@StoreIDs", StoreIDXML.ToString)
        cmd.Parameters.AddWithValue("@KitID", IIf(cmbKitList.SelectedIndex <= 0, Guid.Empty, New Guid(cmbKitList.SelectedValue)))
        cmd.Parameters.AddWithValue("@AsOnDate", txtAsOnDate.Text)
        cmd.Parameters.AddWithValue("@IsLocation", chkForLocation.Checked)
        cmd.Parameters.AddWithValue("@ConsiderAlternateParts", chkConsiderAlternatePart.Checked)
        cmd.Parameters.AddWithValue("@ClientCode", AppSettings("ClientCode"))

        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()


        'dataTable.Columns.Remove("Rem1")
        'dataTable.Columns.Remove("Rem2")
        'dataTable.Columns.Remove("Rem3")
        Return dataTable
    End Function
    Private Sub GenerateXLSXFile(ByVal tbl As DataTable)
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), New SmartDate(txtAsOnDate.Text.ToString).FormattedText, "", PartNo, "", "", "", "", StrStore, "", strKit, Description, "", 0, "", "", chkConsiderAlternatePart.Checked, AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.
        da.Fill(ds, "rptSearchingCriteria", objsearch)
        Dim columnToRemove As String() = {"FromDate", "ToDate", "CompanyName", "BranchName", "SupplierName", "CurrencySymbol", "CurrencyName", "Category", "Nomenclature", "Aircraft", "RelNoteNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10", "WorkOrderText", "WorkShop", "FromStore", "ProductVersion", "SINote", "TransTypeID", "WorkOrderNo"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove(i))
            End If
        Next
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim dsNew As New DataSet
        dsNew.Clear()
        dsNew.Merge(ds.Tables("rptSearchingCriteria"))
        dsNew.Merge(tbl)
		Session("ExcelFileName") = "Kit Report"
		dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
		Session("dsNew") = dsNew
        'Session("DataTable") = tbl
        'Session("ReportName") = "RCI Register"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        MarkLog(Util.Action.Print, "KitReport", "Export To excel " + mKitSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
End Class