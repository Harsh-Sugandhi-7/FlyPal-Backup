
'CREATED By : Saylee
'Dated      : 30-Jan-2014

Imports System.Collections.Generic
Imports Flypal.ModelListAutoComplete
Imports System.Linq


Public Class wfrptAssemblyHistoryCard_AJAX
    Inherits System.Web.UI.Page


#Region " Variable Declarations "
    Public mAssemblyListForHistoryCard As AssemblyListForHistoryCard
    Public mModelList As ModelList

    Public EventLogID As Guid
    Dim mModuleList As ModuleList '  Ajay on 22-July-2022 (FormRevisionNo)
#End Region

#Region " Business Methods "
    Private Sub SetSession()
        Session("mAssemblyListForHistoryCard") = mAssemblyListForHistoryCard
        Session("mModelList") = mModelList
    End Sub
    Private Sub GetSession()
        mAssemblyListForHistoryCard = Session("mAssemblyListForHistoryCard")
        mModelList = Session("mModelList")
        mModuleList = Session("mModuleList") 'Added by Ajay on 22-July-2022 for Add FormRevisionNo 
    End Sub

    Private Shadows Sub SetFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        Dim mModelID As String
        'mModelID = IIf(ModelID.Value.Length > 0, ModelID.Value, Guid.Empty.ToString)
        If mModelList.Contains(txtModel.Text) Then
            mModelID = mModelList(txtModel.Text).ID.ToString
        Else
            mModelID = Guid.Empty.ToString
        End If


        If custValidator.ControlToValidate = "cmbAssembly" Then
            If cmbAssembly.Visible = False And mModelID = Guid.Empty.ToString Then
                custValidator.ErrorMessage = "Please Select the Model."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        txtAsOnDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
        mModelList = ModelList.GetModelList()
        Session("mModelList") = mModelList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal ByVale As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("Sender"), String) = "" Then
            txtAsOnDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            cmbAssembly.Enabled = False
            DataFieldBind()
            SetFocus(txtModel)
        End If

    End Sub
    Private Sub txtModel_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtModel.TextChanged

        Dim mModelID As String
        'mModelID = IIf(ModelID.Value.Length > 0, ModelID.Value, Guid.Empty.ToString)
        mModelID = mModelList(txtModel.Text).ID.ToString

        If mModelID = Guid.Empty.ToString Then
            cmbAssembly.ClearSelection()
            cmbAssembly.Visible = False
            cmbtempAssembly.Visible = True
            cmbtempAssembly.Enabled = False

            upnlAssembly.Update()
            Exit Sub
        Else
            mAssemblyListForHistoryCard = AssemblyListForHistoryCard.GetAssemblyList(txtModel.Text, "", txtAsOnDate.Text)
            cmbtempAssembly.Visible = False
            cmbAssembly.Visible = True

            cmbAssembly.DataSource = mAssemblyListForHistoryCard
            cmbAssembly.DataBind()
            If mAssemblyListForHistoryCard.Count = 0 Then
                cmbAssembly.Enabled = False
                Exit Sub
            Else
                cmbAssembly.Enabled = True
                Session("mAssemblyListForHistoryCard") = mAssemblyListForHistoryCard
                SetFocus(txtModel)
            End If
            upnlAssembly.Update()
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not IsValid Then Exit Sub

        If (cmbAssembly.SelectedValue.ToString) = "" Then
            MSGBoxCtrl.show(" Assembly Not Present!  ", "Please select a Assembly.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim Rpt As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsAssemblyHistory
        Dim ObjHistoryCard As AssemblyHistory
        Dim mCompanyDetail As New CompanyDetail

        Rpt = New crptAssemblyHistory

        ObjHistoryCard = AssemblyHistory.GetAssemblyHistory(New SmartDate(txtAsOnDate.Text, False), New Guid(cmbAssembly.SelectedValue.ToString))
        Session("ObjHistoryCard") = ObjHistoryCard
        If ObjHistoryCard.Count = 0 Then
            MSGBoxCtrl.show(" Record Not Present!  ", "There is no record for the selected criteria.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim ReportData As Flypal.ReportData


        If ObjHistoryCard.Count > 0 Then
            ReportData = New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
            "", "Assembly History Card Report", New SmartDate(txtAsOnDate.Text, False).FormattedText,
            txtModel.Text, mAssemblyListForHistoryCard(New Guid(cmbAssembly.SelectedValue.ToString)).AssemblyType, ObjHistoryCard(0).SerialNo, "",
            AppSettings("Product Version"), AppSettings("SINote"), "", SearchStr7:=mModuleList.Item("AssemblyHistoryCard").FormRevisionNo,
            SearchStr8:=AppSettings("ClientCode"), SearchStr9:="", SearchStr10:=AppSettings("Logo"), SearchStr11:=AppSettings("ShowMaintenanceForNewClients"))

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1275)
        End If
      
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ObjHistoryCard)
        da.Fill(ds, mrptImage)
        da.Fill(ds, ReportData)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        Dim EventLogDetail As String = "As On Date: " + New SmartDate(txtAsOnDate.Text, False).FormattedText + " , Model: " + txtModel.Text + " , Serial No.: " + ObjHistoryCard(0).SerialNo + " , Assembly Type: " + mAssemblyListForHistoryCard(New Guid(cmbAssembly.SelectedValue.ToString)).AssemblyType
        MarkLog(Util.Action.Print, "AssemblyHistoryCard", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mModelList")
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
#End Region


#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetModelList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ModelList As ModelListAutoComplete
        ModelList = ModelListAutoComplete.GetModelList(prefixText)
        If count = 0 Then
            Return (From c As ModelListAutoCompleteInfo In ModelList
              Select c.Name).ToArray
        Else
            Return (From c As ModelListAutoCompleteInfo In ModelList
                   Select c.Name).Take(count).ToArray
        End If
    End Function
#End Region
End Class