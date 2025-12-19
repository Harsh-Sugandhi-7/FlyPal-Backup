'AJAX Conversion By Vikrant On 08-Apr-2015

Public Class wfUpdateInstalledCompHistory_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mMachineNameValueList As MachineNameValueList
    Private mUpdateHistoryCompStausList As UpdateHistoryCompStatusList
    Private AircraftId As String
    Private RemoveDate As String
    Private mMachine As Machine
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mUpdateHistoryCompStausList = CType(Session("mUpdateHistoryCompStausList"), UpdateHistoryCompStatusList)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        AircraftId = CType(Session("AircraftId"), String)
        RemoveDate = CType(Session("RemoveDate"), String)
        mMachine = CType(Session("mMachine"), Machine)
    End Sub
    Private Sub SetSession()
        Session("mUpdateHistoryCompStausList") = mUpdateHistoryCompStausList
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("AircraftId") = AircraftId
        Session("RemoveDate") = RemoveDate
        Session("mMachine") = mMachine
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mUpdateHistoryCompStausList")
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfUpdateRemovedCompHistory.aspx?" Then
            Session.Remove("mUpdateHistoryCompStausList")
            Session.Remove("mMachineNameValueList")
            Session.Remove("AircraftId")
            Session.Remove("RemoveDate")
        End If
    End Sub
    Private Sub SetCaption()
        lblInstalledCompList.Text = "History for Installed Components as of " & txtDate.Text & "  : " & mUpdateHistoryCompStausList.Count & " Record(s) found."
    End Sub
    Private Sub FindNow()

    End Sub
    Private Sub SetGrid()
        Dim B As Boolean
        For j As Integer = 0 To dgRemovedCompList.Rows.Count - 1
            B = CType(Me.dgRemovedCompList.Rows(j).Cells(10).Text, Boolean)
            If B = False Then
                dgRemovedCompList.Rows(j).Cells(9).Enabled = False
            End If
        Next
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        If IsNothing(Session("RemoveDate")) Then
            txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            RemoveDate = Today.Date.ToString(AppSettings("DateFormat"))
        Else
            txtDate.Text = CDate(RemoveDate).ToString(AppSettings("DateFormat"))
        End If
        Session("RemoveDate") = txtDate.Text

        dgRemovedCompList.DataSource = mUpdateHistoryCompStausList
        Session("mUpdateHistoryCompStausList") = mUpdateHistoryCompStausList
        dgRemovedCompList.DataBind()

        txtPartNo.Text = Session("PartName")
        txtSerialNo.Text = Session("CompSerialNo") ' mUpdateHistoryCompStausList(0).CompSerialNo

        DataBind()
        '----------------------------------------------
        ''If mMachineNameValueList.Count > 1 And (Session("AircraftId") = Guid.Empty.ToString Or IsNothing(Session("AircraftId"))) Then
        ''    cmbMachine.SelectedIndex = 1
        ''Else
        ''    cmbMachine.SelectedValue = CType(Session("AircraftId"), String)
        ''End If

    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        ''Dim custvalid As CustomValidator = CType(s, CustomValidator)
        ''If custvalid.ControlToValidate = "cmbMachine" Then
        ''    If cmbMachine.SelectedIndex = 0 Then
        ''        custvalid.ErrorMessage = "Please select an Aircraft from the list."
        ''        e.IsValid = False
        ''    Else
        ''        e.IsValid = True
        ''    End If
        ''End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        REM:put here the code to initialize the page
        GetSession()
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            DataFieldBind()
            SetCaption()
            SetGrid()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session.Remove("mUpdateHistoryCompStausList")
        'Response.Redirect(Request.QueryString("BackPage")) '' & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub dgRemovedCompList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRemovedCompList.RowCommand
        Dim Index As Int16
        Select Case e.CommandName
            Case "ViewRec"
                Index = CInt(e.CommandArgument) + dgRemovedCompList.PageSize * dgRemovedCompList.PageIndex
                Dim No As New Random
                'Added By Saylee On 1-Dec-2014
                Dim mFileAttach As FileAttach
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mUpdateHistoryCompStausList(Index).ID)
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub dgRemovedCompList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRemovedCompList.Sorting
        mUpdateHistoryCompStausList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mUpdateHistoryCompStausList") = mUpdateHistoryCompStausList
        dgRemovedCompList.DataSource = mUpdateHistoryCompStausList
        dgRemovedCompList.DataBind()
        SetGrid()
    End Sub
    Private Sub lnkHistoryCard_Click(sender As Object, e As System.EventArgs) Handles lnkHistoryCard.Click 'Added by Saylee on 12-Jan-2018 for ALL12012018
        Dim Rpt As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCompHistory 'dsCompHistoryList
        Dim ObjHistoryCard As ComponentHistory ''CompHistoryCardList
        Dim mCompanyDetail As New CompanyDetail


        If AppSettings("ClientCode") = "Indamer" Then
            Rpt = New crptComponentHistoryInd 'crptCompHistoryCardListForIndamer
        ElseIf AppSettings("ClientCode") = "STR" Then 'Added By Vikrant On 14-Aug-2018 For StarAir14082018
            Rpt = New crptComponentHistoryStarAir
        Else
            Rpt = New crptComponentHistory 'crptCompHistoryCardList
        End If

        '********************************

        ObjHistoryCard = ComponentHistory.GetComponentHistory(New SmartDate(Today.Date.ToString, False), mUpdateHistoryCompStausList(0).CompID)
        Session("ObjHistoryCard") = ObjHistoryCard
        If ObjHistoryCard.Count = 0 Then
            ''Dim msg1 As New SIMsgBox(Page, " Record Not Present!  ", "There is no record for the selected criteria.", "", MsgBoxStyle.OkOnly)
            ''msg1.ReplacePage = "wfrptComponentHistoryCard.aspx?BackPage=" & Request.QueryString("BackPage")
            ''msg1.Show()
            MSGBoxCtrl.show(" Record Not Present!  ", "There is no record for the selected criteria.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim EventLogDetail As String = "Printed From Component Update Installed History with As On Date: " + New SmartDate(Today.Date.ToString, False).FormattedText + " , Part: " + mUpdateHistoryCompStausList(0).PartName + " , Serial No.: " + mUpdateHistoryCompStausList(0).CompSerialNo
        Dim ReportData As Flypal.ReportData
        If ObjHistoryCard.Count > 0 Then
            ReportData = New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             "", "Component History Card Report", New SmartDate(Today.Date.ToString, False).FormattedText, "", mUpdateHistoryCompStausList(0).PartName, mUpdateHistoryCompStausList(0).CompSerialNo, ObjHistoryCard(0).ATA, AppSettings("Product Version"), AppSettings("SINote"), mUpdateHistoryCompStausList(0).Description, "", "", "Assembly", AppSettings("Logo"))

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
#End Region

End Class