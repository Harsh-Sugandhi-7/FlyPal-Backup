Imports System.Collections.Generic
Imports Flypal.ModelListAutoComplete
Imports System.Linq
Imports System.Web.Mail
Imports Flypal.SendMailFile
Public Class wfrptStoreBalanceForNilRotable_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItemList As ItemList
    Public mStore As Store
    Public mStoreList As StoreList
    Public mCustomerList As VendorList
    Public PartNo As String = ""
    Public Description As String = ""
    Public ModelName, AssemblyType, Location As String
    Public AssemblyTypeID As Integer = 0
    Public strCustomer As String
    Public flag As Int16
    Public mAssemblyTypeList As AssemblyTypeList
    Public mModelList As ModelList
    Public mStoreID As Guid
    Public mCustomerID As Guid
    Public ToDate As String
    Public mCategoryLists As CategoryList
    Public mCategory As Category
    Public mCategoryID As Guid
    Public StrCategory As String
    Public LookInType As Integer = 2
    Public CustomerID As String = "{00000000-0000-0000-0000-000000000000}"
    Public AssemblyTypID As Integer = 0
    Dim NameOfStore As String = ""
    Public strStore As String = ""
    Public mPartStatusList As PartStatusList
    Dim value As String
    Dim ReportName As String
    Dim mStoreBlanceSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
    Dim mText As String = ""
    Dim email As Thread
    Dim mModuleList As ModuleList

#End Region

#Region " Helper Methods "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim mModelList As ModelListAutoComplete
        Dim str As String = contextKey 'Holds the parameters to filter criteria..
        Dim AssemblyTypID As Integer = CInt(str)
        mModelList = ModelListAutoComplete.GetModelList(prefixText, 1)

        If count = 0 Then
            Return (From c As ModelListAutoCompleteInfo In mModelList
               Select c.Name).ToList
        Else
            Return (From c As ModelListAutoCompleteInfo In mModelList
                   Select c.Name).Take(count).ToList
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
    Private Sub GetSession()
        mCustomerList = CType(Session("mCustomerList"), VendorList)
        mStoreList = CType(Session("mStoreList"), StoreList)
        mItemList = CType(Session("mItemList"), ItemList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mAssemblyTypeList = CType(Session("mAssemblyTypeList"), AssemblyTypeList)
        mModelList = CType(Session("mModelList"), ModelList)
        Location = Session("Location")
        mCategoryLists = CType(Session("mCategoryLists"), CategoryList)
        LookInType = Session("LookInType")
        CustomerID = Session("CutomerID")
        AssemblyTypID = Session("AssemblyTypID")
        mPartStatusList = Session("mPartStatusList")
        mModuleList = Session("mModuleList")
    End Sub
    Private Sub SetSession()
        Session("mCustomerList") = mCustomerList
        Session("mStoreList") = mStoreList
        Session("mItemList") = mItemList
        Session("mAssemblyTypeList") = mAssemblyTypeList
        Session("mModelList") = mModelList
        Session("mCategoryLists") = mCategoryLists
        Session("LookInType") = LookInType
        Session("CutomerID") = CustomerID
        Session("AssemblyTypID") = AssemblyTypID
        Session("mPartStatusList") = mPartStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemList")
        Session.Remove("mCustomerList")
        Session.Remove("mStoreList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mAssemblyTypeList")
        Session.Remove("mModelList")
        Session.Remove("Location")
        Session.Remove("LookInType")
        Session.Remove("CutomerID")
        Session.Remove("AssemblyTypID")
        Session.Remove("mPartStatusList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetValues()
        If txtDate.Text = String.Empty Then
            ToDate = "1/1/3050"
        Else
            ToDate = txtDate.Text
        End If
        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text.Trim)
            Description = Trim(txtSearch.Text.Trim)
        End If
        mStoreBlanceSearchingCriteria = "As On Date " + ToDate + ", " + "Part No " + PartNo + ", " + "Description " + Description
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean, Optional ByVal ByMail As Boolean = False)
        Try
            'Session("IsExcel") = IsExcel
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim objsearch As rptSearchingCriteria
            Dim rpt As StoreBalanceForNilRotable
            SetValues()
            Dim ds As New dsStoreBalanceForNilRotable

            myReport = New crptStoreBalanceForNilRotable
            rpt = StoreBalanceForNilRotable.GetStoreBalanceForNilRotable(PartNo, Description, "", Guid.Empty, Guid.Empty, False, ToDate, False, _
                                                                          Guid.Empty.ToString, Guid.Empty.ToString, IsForAPI:=True)
            If ByMail = False Then
                If rpt.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1454)
                End If
            End If
            If (ByMail = True And rpt.Count <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "", _
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                    ReportGeneratedBy:=Session("ReportGenratedBy"), _
                    SmtpHost:=mModuleList.Item("StoreBalance").SmtpHost, SmtpPort:=mModuleList.Item("StoreBalance").SmtpPort, _
                    SmtpUser:=mModuleList.Item("StoreBalance").SmtpUser, SmtpPassword:=mModuleList.Item("StoreBalance").SmtpPassword)
                Exit Sub
            End If
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", ToDate, PartNo, "", mText, "", _
                                                                  "", "", "", "", Description, "", , , "", "", AppSettings("Logo"), Search1:="", Search2:="", _
                                                                  Search3:=AppSettings("ClientCode"), Search4:="")
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
                If IsExcel = False Then         'PDF Format
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
                    MarkLog(Util.Action.Print, "MinStockRotable", mStoreBlanceSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                ElseIf IsExcel = True Then      'Excel Format
                    Dim columnToRemove2 As String() = {"FromDate", "CompanyName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", _
                                                       "TransTypeID", "FromStore", "WorkOrderNo", "Search2", "Search3", "Search5", "Search6", "Search7", _
                                                       "Search8", "Search9", "Search10", "Aircraft", "SupplierName", "BranchName", "Category", "Nomenclature", _
                                                       "Store", "KitName", "WorkShop", "WorkOrderText", "Search1", "Search4"}
                    For i As Integer = 0 To columnToRemove2.Length - 1
                        If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                            ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                        End If
                    Next

                    Dim columnToRemove As String() = {"ItemID"}
                    For i As Integer = 0 To columnToRemove.Length - 1
                        If ds.Tables("StoreBalanceForNilRotable").Columns.Contains(columnToRemove(i)) Then
                            ds.Tables("StoreBalanceForNilRotable").Columns.Remove(columnToRemove(i))
                        End If
                    Next

                    Dim dsNew As New DataSet
                    dsNew.Clear()
                    dsNew.Merge(ds.Tables("rptSearchingCriteria"))
                    'dsNew.Tables("rptSearchingCriteria").Columns("KitName").ColumnName = "Model"
                    'dsNew.Tables("rptSearchingCriteria").Columns("ToDate").ColumnName = "As On Date"
                    'dsNew.Tables("rptSearchingCriteria").Columns("WorkShop").ColumnName = "Bin Location"
                    'dsNew.Tables("rptSearchingCriteria").Columns("Nomenclature").ColumnName = "Part Status"
                    'dsNew.Tables("rptSearchingCriteria").Columns("WorkOrderText").ColumnName = "Value"
                    'dsNew.Tables("rptSearchingCriteria").Columns("Search1").ColumnName = "Applicability"
                    'dsNew.Tables("rptSearchingCriteria").Columns("BranchName").ColumnName = "valued parts value greater than entered value "
                    'dsNew.Tables("rptSearchingCriteria").Columns("Search4").ColumnName = "One Time Purchase Part(s) Only"
                    dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
                    dsNew.Merge(ds.Tables("StoreBalanceForNilRotable"))
                    dsNew.Tables("StoreBalanceForNilRotable").Columns("PartName").ColumnName = "Part No."
                    dsNew.Tables("StoreBalanceForNilRotable").Columns("PartDescription").ColumnName = "Description"
                    dsNew.Tables("StoreBalanceForNilRotable").Columns("BalQty").ColumnName = "Toal Stock  Qty."
                    dsNew.Tables("StoreBalanceForNilRotable").Columns("ServiceablePartBalQty").ColumnName = "Serviceable Stock Qty."
                     dsNew.Tables("StoreBalanceForNilRotable").TableName = "Min Stock Rotable"
					Session("ExcelFileName") = "Min Stock Rotable"
					Session("dsNew") = dsNew
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    'Added by Prashant on 19-Jan-2021
                    MarkLog(Util.Action.Print, "MinStockRotable", "Export To Excel " + mStoreBlanceSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                End If
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Min Stock Rotable", "Min Stock Rotable", "", "", _
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                          ReportGeneratedBy:=Session("ReportGenratedBy"), _
                    SmtpHost:=mModuleList.Item("StoreBalance").SmtpHost, SmtpPort:=mModuleList.Item("StoreBalance").SmtpPort, _
                    SmtpUser:=mModuleList.Item("StoreBalance").SmtpUser, SmtpPassword:=mModuleList.Item("StoreBalance").SmtpPassword)
            End If

        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (SetReport Sub Method): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
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
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant on 04-Dec-2013
        If Not IsPostBack And Session("sender") = "" Then
            txtDate.Text = New SmartDate(Today.Date).FormattedText
            DataFieldBind()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport(False, False)
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
        Session("UserEmailID") = mModuleList.Item("StoreBalance").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("StoreBalance").SendCCMailID
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetReport(True, False)
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

End Class