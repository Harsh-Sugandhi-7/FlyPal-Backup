Public Class wfManualAvailabilityStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mManualAvailabilityStatus As ManualAvailabilityStatus
    Dim EventLogID As Guid
#End Region

#Region " Methods "
    Private Sub GetSession()
        mManualAvailabilityStatus = Session("mManualAvailabilityStatus")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mManualAvailabilityStatus")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                Case MsgBoxResult.No
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
    Private Sub DataFieldBind()
        mManualAvailabilityStatus = ManualAvailabilityStatus.GetManualAvailabilityStatus(ManualName:=txtManualName.Text.Trim, DueRange:=Val(txtDueRange.Text.Trim))
        Session("mManualAvailabilityStatus") = mManualAvailabilityStatus
        dgManualAvailabilityStatusList.DataSource = mManualAvailabilityStatus
        dgManualAvailabilityStatusList.DataBind()
        lblManual.Text = "Manual Availability Status"
        lblList.Text = "List of Manual & Subscription as per criteria : " & mManualAvailabilityStatus.Count & " Record(s) found."
    End Sub
    Private Sub ControlVisibility()
        If mManualAvailabilityStatus.Count = 0 Then
            btnPrint.Enabled = False
            btnPrintTop.Enabled = False
        Else
            btnPrint.Enabled = True
            btnPrintTop.Enabled = True
        End If

        If mManualAvailabilityStatus.Count > 20 Then
            btnPrintTop.Visible = True
            btnCloseTop.Visible = True
        Else
            btnPrintTop.Visible = True
            btnCloseTop.Visible = True

        End If
    End Sub
    Private Sub addAttributes()
        txtDueRange.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtDueRange').value,event)")
    End Sub
    'MMailChanges
    Private Sub SetGrid()
        'Dim AttchCount As Integer
        Dim IsAttachmentAdded As Boolean
        'Dim c As Boolean

        For j As Integer = 0 To dgManualAvailabilityStatusList.Rows.Count - 1
            IsAttachmentAdded = CType(Me.dgManualAvailabilityStatusList.Rows(j).Cells(10).Text, Boolean)
            If Not IsAttachmentAdded Then
                dgManualAvailabilityStatusList.Rows(j).Cells(9).Visible = False
            Else
                dgManualAvailabilityStatusList.Rows(j).Cells(9).Visible = True

            End If
        Next
    End Sub
    'End
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            ControlVisibility()
            SetGrid() 'MMailChanges
        End If
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click, btnClose.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        dgManualAvailabilityStatusList.PageIndex = 0

        mManualAvailabilityStatus = ManualAvailabilityStatus.GetManualAvailabilityStatus(ManualName:=txtManualName.Text.Trim, DueRange:=Val(txtDueRange.Text.Trim))
        dgManualAvailabilityStatusList.DataSource = mManualAvailabilityStatus
        dgManualAvailabilityStatusList.DataBind()
        Session("mManualAvailabilityStatus") = mManualAvailabilityStatus

        lblList.Text = "List of Manual & Subscription as per criteria : " & mManualAvailabilityStatus.Count & " Record(s) found."
        ControlVisibility()
        SetGrid() 'MMailChanges
        upnlGrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub dgManualAvailabilityStatusList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgManualAvailabilityStatusList.PageIndexChanging
        dgManualAvailabilityStatusList.PageIndex = e.NewPageIndex
        dgManualAvailabilityStatusList.DataSource = mManualAvailabilityStatus
        Session("mManualAvailabilityStatus") = mManualAvailabilityStatus
        dgManualAvailabilityStatusList.DataBind()
        SetGrid() 'MMailChanges
    End Sub
    'MMailChanges
    Private Sub dgManualAvailabilityStatusList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgManualAvailabilityStatusList.RowCommand
        Select Case e.CommandName
            Case "ViewAttachments"
                Dim mFileAttachments As New FileAttachments
                Dim index As Integer = CInt(e.CommandArgument) + dgManualAvailabilityStatusList.PageIndex * dgManualAvailabilityStatusList.PageSize
                Dim AttachmentCount As Integer
                'AttachmentCount = mManualAvailabilityStatus(index).LastRevAttachmentCount
                mFileAttachments = FileAttachments.GetChildFileAttachments(mManualAvailabilityStatus(index).RevisionID)
                AttachmentCount = mFileAttachments.Count
                If AttachmentCount > 1 Then
                    Dim mRevision As Revision
                    mRevision = Revision.GetRevision(mManualAvailabilityStatus(index).RevisionID, "")
                    Session("mRevision") = mRevision
                    Session("mFileAttachments") = mFileAttachments
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenManualRevisionAttachWindow", "OpenManualRevisionAttachWindow();", True)
                Else
                    Dim mFileAttach As FileAttach
                    Dim No As New Random
                    Dim StrName As String = "abc" & No.Next.ToString

                    mFileAttach = FileAttach.GetAttachment(mManualAvailabilityStatus(index).RevisionID)
                    If mFileAttach.Size > 0 Then
                        Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
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
                            Dim Detail As String = "Manual Revision Attachment( " + mFileAttach.FileName + ") viewed by  " + User.Identity.Name
                            MarkLog(Util.Action.View, "ManualRevision", Detail, Util.ErrorType.HandledError, mManualAvailabilityStatus(index).RevisionID, EventLogID)
                        End If
                    End If
                End If
        End Select
    End Sub
    'End
#End Region

#Region " Report "

#Region "Report Variable Declaration"
    Dim mCompanyDetail As New Flypal.CompanyDetail
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region "Event"
    Private Sub btnPrintTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click, btnPrint.Click
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim ds As New dsManualAvailabilityStatus
        Dim Obj As ManualAvailabilityStatus
        Rpt = New crptManualAvailabilityStatus
        mManualAvailabilityStatus = Session("mManualAvailabilityStatus")
        Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Manual Availability Status", txtManualName.Text.Trim, Val(txtDueRange.Text.Trim), "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        dgManualAvailabilityStatusList.Visible = True

        Obj = mManualAvailabilityStatus
        ds.Clear()

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, Obj)
        da.Fill(ds, mrptImage) '
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)

        Session("CrystalReport") = Rpt

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
   
#End Region

#End Region


    
End Class