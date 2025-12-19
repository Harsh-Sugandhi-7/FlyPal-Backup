'Created by Vikrant on 30-Apr-2014 For ALL30042014

Imports System.Linq
Imports System.Collections.Generic

Public Class wfAircraftCRS_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim mnWO As nWO
    Dim mnWOJobTask As nWOJobTask
    Dim mnWOJobTasks As nWOJobTasks
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mnWO = CType(Session("mnWO"), nWO)
    End Sub
    Private Sub DataFieldBind()
        mnWOJobTasks = nWOJobTasks.GetWOTasks(mnWO.ID, "")

        For i As Integer = 0 To mnWOJobTasks.Count - 1
            txtRemovalReason.Text = txtRemovalReason.Text + CStr(i + 1) + ") " + mnWOJobTasks(i).TaskDescription + Chr(13)
        Next

        txtWONo.Text = mnWO.WONumber
        txtAircraft.Text = mnWO.RegNo
        If AppSettings("ClientCode") = "Novo" Then
            txtCAANApprovalNo.Text = "CAAB 145-001"
        Else
            txtCAANApprovalNo.Text = "CAAN 145 002"
        End If
        DataBind()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        If Not Page.IsPostBack Then
            DataFieldBind()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If IsValid Then
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim da As New CSLA.Data.ObjectAdapter
            Dim ds As New dsnWORegister
            Dim mCompanyDetail As New CompanyDetail

            If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "STR") Then
                myReport = New crptAircraftCertificateofReleaseToServiceSTR
            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Novo") Then
                myReport = New crptAircraftCertificateofReleaseToServiceNOVO
            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "KamAir") Then
                myReport = New crptAircraftCertificateofReleaseToServiceKamAir
            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "HSC") Then
                myReport = New crptAircraftCertificateofReleaseToServiceHeliStar
            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "SAA") Then
                myReport = New crptAircraftCertificateofReleaseToServiceForSaurya
            Else
                myReport = New crptAircraftCertificateofReleaseToService
            End If

            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                  mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                  mCompanyDetail.WebSite, "", txtRemovalReason.Text.Trim, txtIssue.Text.Trim, txtAmendment.Text.Trim, txtDate.Text, txtFromDate.Text, _
                  AppSettings("Product Version"), AppSettings("SINote"), txtToDate.Text, txtCAANApprovalNo.Text.Trim, txtFormTrackingNo.Text.Trim, txtWorkPackageRef.Text.Trim, _
                  AppSettings("Logo"), SearchStr11:=AppSettings("ClientCode"), SearchStr12:=AppSettings("WO-CRSIssueRev"), SearchStr13:=AppSettings("WODocumentNo"), _
                  SearchStr14:="")
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, Report)
            da.Fill(ds, mnWO)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If
    End Sub
#End Region

End Class