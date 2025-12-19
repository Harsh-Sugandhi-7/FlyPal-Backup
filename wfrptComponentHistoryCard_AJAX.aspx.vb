
'CREATED By : Saylee
'Dated      : 30-Jan-2014

Imports System.Collections.Generic
Imports Flypal.PartListAutoComplete
Imports System.Linq


Public Class wfrptComponentHistoryCard_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mCompList As CompList
    Public mPartList As PartList

    Public EventLogID As Guid
    Public PartNo As String = ""
    Public Description As String = ""
    Dim mModuleList As ModuleList '  Ajay on 22-July-2022 (FormRevisionNo)
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mCompList = Session("mCompList")
        mPartList = Session("mPartList")
        mModuleList = Session("mModuleList") 'Added by Ajay on 22-July-2022 for Add FormRevisionNo 
    End Sub
    Private Sub SetSession()
        Session("mCompList") = mCompList
        Session("mPartList") = mPartList
    End Sub
    Private Sub Controlvisibility()
        If rdoAirframe.Checked Then
            dgCompHistory.Columns(5).Visible = True
            dgCompHistory.Columns(6).Visible = False
            dgCompHistory.Columns(14).Visible = True
            dgCompHistory.Columns(15).Visible = False
            dgCompHistory.Columns(16).Visible = True
            dgCompHistory.Columns(17).Visible = False
            dgCompHistory.Columns(20).Visible = True
            dgCompHistory.Columns(21).Visible = False

            dgComplianceHistory.Columns(7).Visible = True
            dgComplianceHistory.Columns(8).Visible = False

        ElseIf rdoAssembly.Checked Then
            dgCompHistory.Columns(5).Visible = False
            dgCompHistory.Columns(6).Visible = True
            dgCompHistory.Columns(14).Visible = False
            dgCompHistory.Columns(15).Visible = True
            dgCompHistory.Columns(16).Visible = False
            dgCompHistory.Columns(17).Visible = True
            dgCompHistory.Columns(20).Visible = False
            dgCompHistory.Columns(21).Visible = True

            dgComplianceHistory.Columns(7).Visible = False
            dgComplianceHistory.Columns(8).Visible = True
        End If

        dgCompHistory.Columns(7).HeaderText = Session("HeaderInspStatus")
        dgCompHistory.Columns(22).HeaderText = Session("HeaderInspStatus")

        lblTBOFreq.InnerText = Session("TBOFreq")
        lblSLLFreq.InnerText = Session("SLLFreq")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        Dim mPartID As String
        mPartID = IIf(PartID.Value.Length > 0, PartID.Value, Guid.Empty.ToString)

        If custValidator.ControlToValidate = "cmbComponent" Then
            If cmbComponent.Visible = False And mPartID = Guid.Empty.ToString Then
                custValidator.ErrorMessage = "Please select the Component"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        '''mPartList = PartList.GetPartList("", "", "<SELECT>")
        '''cmbPart.DataSource = mPartList
        '''cmbPart.DataBind()

        txtAsOnDate.Text = Now.Date.ToString(AppSettings("DateFormat"))

        ''Session("mPartList") = mPartList
        DataBind()
    End Sub


#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal ByVale As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("Sender"), String) = "" Then
            txtAsOnDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            cmbComponent.Enabled = False
            pnlTBOSLL.Visible = False
            DataFieldBind()
            setFocus(txtPartDescription)
        End If

    End Sub
    Private Sub txtPartDescription_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPartDescription.TextChanged
        Dim mPartID As String
        mPartID = IIf(PartID.Value.Length > 0, PartID.Value, Guid.Empty.ToString)

        If mPartID = Guid.Empty.ToString Then
            cmbComponent.Visible = False
            cmbtempComponent.Visible = True
            cmbtempComponent.Enabled = False

            upnlComponent.Update()
            Exit Sub
        Else
            mCompList = CompList.GetCompList("", "", txtAsOnDate.Text, 0, mPartID)
            cmbtempComponent.Visible = False
            cmbComponent.Visible = True

            cmbComponent.DataSource = mCompList
            cmbComponent.DataBind()
            If mCompList.Count = 0 Then
                cmbComponent.Enabled = False
                Exit Sub
            Else
                cmbComponent.Enabled = True

                Session("mCompList") = mCompList

                setFocus(txtPartDescription)
            End If
            upnlComponent.Update()
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

        If (cmbComponent.SelectedValue.ToString) = "" Then
            MSGBoxCtrl.show(" Component Not Present!  ", "Please select a component.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        'Dim Rpt As New crptCompHistoryCardList   'Commented By Utkarsh On 23-May-2011
        Dim Rpt As New CrystalDecisions.CrystalReports.Engine.ReportClass 'Added By Utkarsh On 23-May-2011
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCompHistory 'dsCompHistoryList
        Dim ObjHistoryCard As ComponentHistory ''CompHistoryCardList
        Dim mCompanyDetail As New CompanyDetail

        'Added By Utkarsh On 23-May-2011

        If AppSettings("ClientCode") = "Indamer" Then
            Rpt = New crptComponentHistoryInd 'crptCompHistoryCardListForIndamer
        ElseIf AppSettings("ClientCode") = "STR" Then 'Added By Vikrant On 14-Aug-2018 For StarAir14082018
            Rpt = New crptComponentHistoryStarAir
        Else
            Rpt = New crptComponentHistory 'crptCompHistoryCardList
        End If

        '********************************

        ObjHistoryCard = ComponentHistory.GetComponentHistory(New SmartDate(txtAsOnDate.Text, False), New Guid(cmbComponent.SelectedValue.ToString))
        Session("ObjHistoryCard") = ObjHistoryCard
        If ObjHistoryCard.Count = 0 Then
            ''Dim msg1 As New SIMsgBox(Page, " Record Not Present!  ", "There is no record for the selected criteria.", "", MsgBoxStyle.OkOnly)
            ''msg1.ReplacePage = "wfrptComponentHistoryCard.aspx?BackPage=" & Request.QueryString("BackPage")
            ''msg1.Show()
            MSGBoxCtrl.show(" Record Not Present!  ", "There is no record for the selected criteria.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If (txtPartDescription.Text.Trim.IndexOf("[") >= 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtPartDescription.Text.Trim)
            Description = Trim(txtPartDescription.Text.Trim)
        End If

        Dim EventLogDetail As String = "As On Date: " + New SmartDate(txtAsOnDate.Text, False).FormattedText + " , Part: " + txtPartDescription.Text + " , Serial No.: " + cmbComponent.SelectedItem.Text
        Dim ReportData As Flypal.ReportData
        If ObjHistoryCard.Count > 0 Then
            ReportData = New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
             "", "Component History Card Report", New SmartDate(txtAsOnDate.Text, False).FormattedText, txtPartDescription.Text, PartNo, cmbComponent.SelectedItem.Text,
             ObjHistoryCard(0).ATA, AppSettings("Product Version"), AppSettings("SINote"), Description, SearchStr7:=mModuleList.Item("Component History Card").FormRevisionNo, SearchStr8:=AppSettings("ClientCode"),
             SearchStr9:=IIf(rdoAirframe.Checked, "Airframe", IIf(rdoAssembly.Checked, "Assembly", "Component")), SearchStr10:=AppSettings("Logo"), SearchStr11:=AppSettings("ShowMaintenanceForNewClients"))
            'Added By Utkarsh On 7-Jun-2011 For All07062011
            ''ObjHistoryCard(0).CompOHServiceFrequencyValue()

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1135)

            '*******************************
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ObjHistoryCard)
        da.Fill(ds, mrptImage)
        da.Fill(ds, ReportData)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "Component History Card", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mCompList")
        Session.Remove("mPartList")

        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub


    Private Sub lnkViewID_Click(sender As Object, e As System.EventArgs) Handles lnkViewID.Click

        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

        If (cmbComponent.SelectedValue.ToString) = "" Then
            MSGBoxCtrl.show(" Component Not Present!  ", "Please select a component.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If


        Dim ObjHistoryCard As ComponentHistory
        ObjHistoryCard = ComponentHistory.GetComponentHistory(New SmartDate(txtAsOnDate.Text, False), New Guid(cmbComponent.SelectedValue.ToString), , True)
        Session("ObjHistoryCard") = ObjHistoryCard
        dgCompHistory.DataSource = ObjHistoryCard
        dgCompHistory.DataBind()


        Dim ObjHistoryComplianceCard As ComponentHistory
        ObjHistoryComplianceCard = ComponentHistory.GetComponentHistory(New SmartDate(txtAsOnDate.Text, False), New Guid(cmbComponent.SelectedValue.ToString), True)

        dgComplianceHistory.DataSource = ObjHistoryComplianceCard
        dgComplianceHistory.DataBind()

        If ObjHistoryCard.Count > 0 Then
            Session("HeaderInspStatus") = ObjHistoryCard(0).InstallationStatusName
            Session("TBOFreq") = ObjHistoryCard(0).OHFrequency
            Session("SLLFreq") = ObjHistoryCard(0).SLLFrequency
        End If
        pnlTBOSLL.Visible = True
        Controlvisibility()
        dgCompHistory.DataBind()
        dgComplianceHistory.DataBind()
        upnlCompHistory.Update()
        upnlComplianceHistory.Update()
    End Sub
#End Region


#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetPartNoDescriptionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()


        Dim partlist As PartListAutoComplete
        partlist = PartListAutoComplete.GetPartList(prefixText)
        If count = 0 Then
            Return (From c As PartListAutoCompleteInfo In partlist
              Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Part, c.ID.ToString())).ToArray
        Else
            Return (From c As PartListAutoCompleteInfo In partlist
                   Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Part, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region


End Class