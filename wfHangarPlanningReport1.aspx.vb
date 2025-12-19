Public Class wfHangarPlanningReport1
    Inherits System.Web.UI.Page
#Region "variable declaration"
    Public mhangarlist As HangarList
    Public mhanger As Hanger
    Dim Index As Int32
    Public shango As HangarList
    Dim mSearchIndex, mFromDate, mToDate, mAircraft, mHang, mText, mRemark As String
    ' Dim mNo As Integer
    Dim mNo As String
    Public mHangerMasterList As HangerMasterList
    Public mDistinctTextListForHangar As DistinctTextListForHangar
    Public mDistinctHangarListForHangar As DistinctHangarListForHangar
    Public mdistinctGood As DistinctGood
    Public mDistinctAircraftListForHangar As DistinctAircraftListForHangar
    Dim mFileAttach As FileAttach
#End Region
#Region "business properties"
    Private Sub GetSession()
        mhangarlist = CType(Session("mHangarList"), HangarList)
        mhanger = CType(Session("mHanger"), Hanger)
        Session("NewPage") = "False"
        mSearchIndex = Session("SearchIndex")
        mFromDate = Session("FromDate")
        mToDate = Session("ToDate")
        mAircraft = Session("mAircraft")
        mHang = Session("mHang")
        mText = Session("mText")
        mNo = Session("No")
        mRemark = Session("mRemark")
        mFileAttach = Session("mFileAttach")
    End Sub
    Private Sub SetSession()
        Session("mhangarlist") = mhangarlist
        Session(" mhanger") = mhanger
        Session("No") = mNo
        Session("SearchIndex") = mSearchIndex
        Session("FromDate") = mFromDate
        Session("ToDate") = mToDate
        Session("mAircraft") = mAircraft
        Session("mHang") = mHang
        Session("mText") = mText
        Session("No") = mNo
        Session("mRemark") = mRemark
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub RemoveSession()
        Session.Remove("SearchIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("mAircraft")
        Session.Remove("mHang")
        Session.Remove("mText")
        Session.Remove("No")
        Session.Remove("mFileAttach")
    End Sub

    Private Sub setVariables()
        mFromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ' mFromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, Today.Date)
        mToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        mHang = IIf(cmbHanger.SelectedIndex <= 0, Guid.Empty.ToString, cmbHanger.SelectedValue)
        mAircraft = IIf(cmbAircraft.SelectedIndex <= 0, Guid.Empty.ToString, cmbAircraft.SelectedValue)
        mText = IIf(cmbText.SelectedIndex <= 0, "", cmbText.SelectedValue)

        mNo = txtNo.Text.Trim
        Session("FromDate") = mFromDate
        Session("ToDate") = mToDate
        Session("SearchIndex") = mSearchIndex
        Session("mAircraft") = mAircraft
        Session("mHangar") = mHang
        Session("No") = mNo
        Session("mText") = mText
    End Sub

    Private Sub SetControl()
        cmbAircraft.SelectedValue = mAircraft
        cmbHanger.SelectedValue = mHang
        cmbText.SelectedValue = mText
        ' txtNo.Text = mNo.ToString
        txtNo.Text = mNo
        If txtFromDate.Text = "" And txtToDate.Text = "" Then
            txtFromDate.Text = DateTime.Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = DateTime.Today.Date.ToString(AppSettings("DateFormat"))
        End If
        txtFromDate.Text = DateTime.Today.Date.ToString(AppSettings("DateFormat"))
        txtToDate.Text = DateTime.Today.Date.ToString(AppSettings("DateFormat"))
    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfhangarPlanningReport1.aspx?") <= 0 Then
            RemoveSession()
            Session.Remove("mHangarList")
        End If
    End Sub
#End Region
#Region " DataBinding "
    Private Sub Datafield()
        mdistinctGood = DistinctGood.GetDistinctText("3", 0, True, AddTopItem:="(ALL)")
        cmbHanger.DataSource = mdistinctGood
        cmbHanger.DataBind()
        mDistinctTextListForHangar = DistinctTextListForHangar.GetDistinctText("28", , True, "(ALL)")
        cmbText.DataSource = mDistinctTextListForHangar
        cmbText.DataBind()
        mDistinctAircraftListForHangar = DistinctAircraftListForHangar.GetDistinctText("2", 0, True, AddTopItem:="(ALL)")
        cmbAircraft.DataSource = mDistinctAircraftListForHangar
        cmbAircraft.DataBind()
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtNo').value,event)")
    End Sub
#End Region
#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        If Not IsPostBack Then

            Session("MiddleFrame") = "wfhangarPlanningReport1.aspx?"
            mhangarlist = HangarList.GetHangarList()
            Datafield()
            SetControl()
            addAttributes()
        End If
    End Sub
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click

        Dim da As New CSLA.Data.ObjectAdapter
        Dim rpt As New crptHangarPlanning
        Dim mCompanyDetail As New CompanyDetail
        setVariables()
        Dim obj As HangarList
        Dim ds As New dsHangarPlanning
        ' obj = HangarList.GetHangarList(mAircraft, mHang, mFromDate, mToDate, "", mText, IIf(mNo = "", 0, CInt(mNo)))
        obj = HangarList.GetHangarList(mAircraft, mHang, mFromDate, mToDate, "", mText, CInt(Val(mNo)))
        If obj.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, "Hangar Planning Report", IIf(txtFromDate.Text <> "", txtFromDate.Text, DateTime.Today.Date.ToString(AppSettings("DateFormat"))), IIf(txtToDate.Text <> "", txtToDate.Text, DateTime.Today.Date.ToString(AppSettings("DateFormat"))), IIf(cmbHanger.SelectedIndex > 0, cmbHanger.SelectedItem.Text, ""), mText, mNo, AppSettings("Product Version"), AppSettings("SINote"), IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, ""), "", "", "", AppSettings("Logo"))
        da.Fill(ds, obj)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        rpt.SetDataSource(ds)
        Session("CrystalReport") = rpt
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
        ' upnlPrint.Update()
        UpdatePanel1.Update()
    End Sub
    Protected Sub cmbText_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbText.SelectedIndexChanged
        txtNo.Visible = IIf(cmbText.SelectedIndex < 0, False, True)
    End Sub
    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        ClearAll()
        ' RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region
    
End Class