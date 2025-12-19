Partial Class wfrptABCAnalysis
    Inherits System.Web.UI.Page

#Region "Web Form Designer Generated Code"
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtFromDate As SIControls.SICalendar
    Protected WithEvents txtToDate As SIControls.SICalendar


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Dim mABCAnalysisList As ABCAnalysisList
    Dim Fromdate As String
    Dim ToDate As String
    Dim PartNo As String
    Dim Description As String
    Dim mID As Guid
    Dim qtya As Int16 = 0
    Dim qtyb As Int16 = 0
    Dim qtyc As Int16 = 0
    Dim vala As Int16 = 0
    Dim valb As Int16 = 0
    Dim valc As Int16 = 0
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mABCAnalysisList = Session("mABCAnalysisList")
    End Sub
    Private Sub SetSession()
        Session("mABCAnalysisList") = mABCAnalysisList
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ClearControls()
        txtQtyA.Text = ""
        txtQtyB.Text = ""
        txtQtyC.Text = ""
        txtValA.Text = ""
        txtValB.Text = ""
        txtValC.Text = ""
    End Sub
    Private Sub SetValues()
        If txtToDate.Value.ToString = "" Or txtFromDate.Value.ToString = "" Then
            Fromdate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRange.Text = "Date Range : All"
        Else
            ToDate = txtToDate.Value.ToString
            Fromdate = txtFromDate.Value.ToString
            lblDateRange.Text = "From Date : " & New SmartDate(txtFromDate.Value.ToString).FormattedText & " To Date : " & New SmartDate(txtToDate.Value.ToString).FormattedText
        End If
        qtya = Val(txtQtyA.Text)
        qtyb = Val(txtQtyB.Text)
        qtyc = Val(txtQtyC.Text)
        lblQty.Text = "Qty A % : " & qtya & " , " & " Qty B % : " & qtyb & " , " & " Qty C % : " & qtyc

        vala = Val(txtValA.Text)
        valb = Val(txtValB.Text)
        valc = Val(txtValC.Text)
        lblVal.Text = "Val A % : " & vala & " , " & " Val B % : " & valb & " , " & " Val C % : " & valc
    End Sub
    Private Sub ResetValues()
        ToDate = Format(CDate(Today.Date).Year, "")
    End Sub
    Private Sub PageInitialization()
        txtFromDate.Value = Today.Date
        txtToDate.Value = Today.Date
        txtQtyA.Text = 10
        txtQtyB.Text = 20
        txtQtyC.Text = 70
        txtValA.Text = 75
        txtValB.Text = 15
        txtValC.Text = 10
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If txtQtyA.Enabled = True Then
                SetFocus(txtQtyA)
            End If
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
            mID = Guid.Empty
            DataBind()
            PageInitialization()
        End If
        SetValues()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Dim QtySum As Int32
        Dim ValSum As Int32
        QtySum = Val(qtya + qtyb + qtyc)
        ValSum = Val(vala + valb + valc)
        If QtySum <> 100 Or ValSum <> 100 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "Sum of value and quantity must be Hundred. ", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfrptABCAnalysis.aspx?Backpage="
            msg1.Show()
            Exit Sub
        End If
        Dim str As String
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As DataSet
        Dim rptSearch As rptGraphSearchingCriteria
        SetValues()
        Dim rpt As ABCAnalysisList
        ds = New dsABCAnalysis
        myReport = New crptABCAnalysis
        rpt = ABCAnalysisList.GetABCAnalysisList(Fromdate, ToDate, qtya, qtyb, qtyc, vala, valb, valc)
        'CNDC Change of rpt.FromDate to rpt.FromDateDBValue and rpt.ToDate to rpt.ToDateDBValue 
        'rptSearch = rptGraphSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate)
        rptSearch = rptGraphSearchingCriteria.GetSearchingCriteria( _
              New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), rpt.FromDateDBValue, _
              rpt.ToDateDBValue, rpt.QtyAPercentage, rpt.QtyBPercentage, _
              rpt.QtyCPercentage, rpt.ValAPercentage, rpt.ValBPercentage, _
              rpt.ValCPercentage, rpt.TotalQuantity, rpt.TotalAmount, _
              rpt.QtyA, rpt.QtyB, rpt.QtyC, rpt.ValA, _
              rpt.ValB, rpt.ValC, rpt.ConsA, rpt.ConsB, _
              rpt.ConsC, rpt.CalcA, rpt.CalcB, rpt.CalcC, AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.
        If rpt.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfrptABCAnalysis.aspx?Backpage="
            msg1.Show()
            Exit Sub
        Else
            
           RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 903)
        End If
        ds.Clear()
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, rpt)
        da.Fill(ds, rptSearch)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", str)
        ResetValues()
    End Sub
#End Region

End Class
