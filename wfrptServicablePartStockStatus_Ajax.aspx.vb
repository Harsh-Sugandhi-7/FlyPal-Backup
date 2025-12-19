Imports System.Collections.Generic
Imports Flypal.ModelListAutoComplete
Imports System.Linq
Imports System.Web.Mail
Imports Flypal.SendMailFile
Public Class wfrptServicablePartStockStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItemList As ItemList
    Public mStore As Store
    Public mStoreList As StoreList
    Public PartNo As String = ""
    Public Description As String = ""
    Public mStoreID As Guid
    Public AsOnDate As String
    Public mCategoryLists As CategoryList
    Public mCategory As Category
    Public mCategoryID As Guid
    Public StrCategory As String
    Dim mServicablePartSearchingCriteria As String = String.Empty
#End Region

#Region " Helper Methods "

    Private Sub GetSession()
        mStoreList = CType(Session("mStoreList"), StoreList)
        mItemList = CType(Session("mItemList"), ItemList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mCategoryLists = CType(Session("mCategoryLists"), CategoryList)
    End Sub
   Private Sub RemoveSession()
        Session.Remove("mItemList")
        Session.Remove("mStoreList")
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility2()
        lblDateRange.Visible = True
        lblStoreName.Visible = True
        lblCategoryName.Visible = True
        lblModel1.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblCritPartStatus.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRange.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblStoreName.Visible = False
        lblCustomerName.Visible = False
        lblCategoryName.Visible = False
        lblModel1.Visible = False
        lblCritPartStatus.Visible = False
    End Sub
    Private Sub SetValues()
        If txtDate.Text = String.Empty Then
            AsOnDate = "1/1/3050"
            lblDateRange.Text = "Date Range  : All"
        Else
            AsOnDate = txtDate.Text
            lblDateRange.Text = "As On Date : " & New SmartDate(txtDate.Text).FormattedText
        End If

        lblStoreName.Text = "Store : " & IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "All")
       
        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text.Trim)
            Description = Trim(txtSearch.Text.Trim)
        End If

        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        If cmbCategory.SelectedIndex = 0 Then
            StrCategory = ""
            mCategoryID = Guid.Empty
            lblCategoryName.Text = "Category Name : All"
        Else
            mCategory = Category.GetCategory(New Guid(cmbCategory.SelectedValue))
            StrCategory = mCategory.Name
            mCategoryID = mCategory.ID
            lblCategoryName.Text = "Category Name : " & StrCategory
        End If
       Session("mCategory") = mCategory
        Session("mCategoryID") = mCategoryID
        mServicablePartSearchingCriteria = lblDateRange.Text + ", " + lblCustomerName.Text + ", " + lblStoreName.Text + ", " + lblCategoryName.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + lblAssembly1.Text + ", " + lblModel1.Text + ", " + lblCritPartStatus.Text + ", " + IIf(chkIsOTP.Checked, ", One Time Purchase Item Only", "")
    End Sub
    Private Sub SetReport()
        Try

            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

            Dim objsearch As rptSearchingCriteria
            Dim rpt As rptServicablePartStockStatusList

            SetValues()
            mCategoryID = Session("mCategoryID")

            Dim ds As New dsServicablePartStockStatusList
            myReport = New crptServicablePartStockStatusList
            rpt = rptServicablePartStockStatusList.GetServicableItemStockStatusList(PartNo, Description, AsOnDate, mCategoryID.ToString, _
                                                                                    cmbStore.SelectedValue.ToString, _
                                                                                    IsOneTimePurchaseItemOnly:=IIf(chkIsOTP.Checked, 1, 0), _
                                                                                    IswithUnserviceableAlso:=IIf(chkIswithunServiceablealso.Checked, 1, 0))

            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1500)
            End If

            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AsOnDate, PartNo, "", "", _
                                                                  StrCategory, "", IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text.ToString, ""), "", "", _
                                                                  Description, "", 0, "", "", "", "", "", "", Search3:=AppSettings("ClientCode"), _
                                                                  Search4:=IIf(chkIsOTP.Checked, "Yes", ""))

            ds.Clear()


            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)

            da.Fill(ds, rpt)
            da.Fill(ds, objsearch)

            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            MarkLog(Util.Action.Print, "ServicablePartStatus", mServicablePartSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)

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
        'Store
        mStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        mCategoryLists = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryLists
        Session("mCategoryLists") = mCategoryLists

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            txtDate.Text = New SmartDate(Today.Date).FormattedText
            DataFieldBind()
        End If
       
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
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