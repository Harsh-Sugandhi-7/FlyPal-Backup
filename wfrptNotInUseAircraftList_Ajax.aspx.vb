Public Class wfrptNotInUseAircraftList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim StartDate As String
    Public mNotInUseAircraftList As NotInUseAircraftList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mNotInUseAircraftList = CType(Session("mNotInUseAircraftList"), NotInUseAircraftList)
    End Sub
    Private Sub SetSession()
        Session("mNotInUseAircraftList") = mNotInUseAircraftList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mNotInUseAircraftList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
#End Region

#Region " Helper Methods "
    Private Sub SetValues()
        'If Not (txtAsOnDate.IsDateValue) Then
        '    StartDate = ""
        'Else
        '    StartDate = txtAsOnDate.Text.ToString
        'End If
        StartDate = txtAsOnDate.Text.ToString
    End Sub
    Private Sub ResetValues()
        StartDate = txtAsOnDate.Text.ToString
    End Sub
    Private Sub SetReport()
        SetValues()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsNotInUseAircraftList
        Dim mCompanyDetail As New CompanyDetail

        myReport = New crptNotInUseAircraftList
        mNotInUseAircraftList = NotInUseAircraftList.GetNotInUseAircraftList(txtAsOnDate.Text.ToString)

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, "Not In Use Aircrafts Report", New SmartDate(StartDate).FormattedText, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mNotInUseAircraftList.Count = 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfrptNotInUseAircraftList.aspx?Backpage="
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mNotInUseAircraftList)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        ResetValues()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            txtAsOnDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        'Response.End()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
#End Region

End Class