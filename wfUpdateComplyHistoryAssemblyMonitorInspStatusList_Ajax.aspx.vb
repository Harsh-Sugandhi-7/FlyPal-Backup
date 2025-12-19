'AJAX Conversion By Vikrant on 27-Mar-2015
Public Class wfUpdateComplyHistoryAssemblyMonitorInspStatusList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAssemblyStatus As AssemblyStatus
    Public mMachine As Machine
    Dim Flag As Int16
    'Private mMachineList As MachineList
    Private mUpdateComplyHistoryAssemblyMonitorInspStatusList As UpdateComplyHistoryAssemblyMonitorInspStatusList
    'Added by Vikrant on 2-Aug-2011
    Dim EventLogID As Guid
    Public mDetail As String
    Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mUpdateComplyHistoryAssemblyMonitorInspStatusList = CType(Session("mUpdateComplyHistoryAssemblyMonitorInspStatusList"), UpdateComplyHistoryAssemblyMonitorInspStatusList)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mMachine = CType(Session("mMachine"), Machine)
    End Sub
    Private Sub SetSession()
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMachine") = mMachine
        Session("mUpdateComplyHistoryAssemblyMonitorInspStatusList") = mUpdateComplyHistoryAssemblyMonitorInspStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mUpdateComplyHistoryAssemblyMonitorInspStatusList")
    End Sub
    Public Sub Save()
        Dim txtDoneRemark, txtWONo, txtRequiredManHours As TextBox
        Dim j As Int32
        For j = 0 To Me.dgMonitorInspStatusList.Rows.Count - 1
            txtDoneRemark = CType(Me.dgMonitorInspStatusList.Rows(j).FindControl("txtDoneRemark"), TextBox)
            txtWONo = CType(Me.dgMonitorInspStatusList.Rows(j).FindControl("txtWONo"), TextBox)
            txtRequiredManHours = CType(Me.dgMonitorInspStatusList.Rows(j).FindControl("txtRequiredManHours"), TextBox)
            Try
                Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mUpdateComplyHistoryAssemblyMonitorInspStatusList(j).ID, mAssemblyStatus.ID, mMachine.HourType)
                mAssemblyMonitorInspStatus.DoneRemark = Trim(txtDoneRemark.Text)
                mAssemblyMonitorInspStatus.DoneWONo = Trim(txtWONo.Text)
                mAssemblyMonitorInspStatus.RequiredManHours = Trim(txtRequiredManHours.Text).Split(" ")(0)

                If mAssemblyMonitorInspStatus.IsValid Then
                    If mAssemblyMonitorInspStatus.IsDirty Then
                        mAssemblyMonitorInspStatus.ApplyEdit()
                        mAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Save(), AssemblyMonitorInspStatus)
                        'Added by Vikrant on 2-Aug-2011
                        mDetail = "Reg No. : " & dgMonitorInspStatusList.Rows(j).Cells(6).Text & " Model : " & txtModel.Text & " Serial No. : " & txtSerialNo.Text & " Description : " & dgMonitorInspStatusList.Rows(j).Cells(3).Text & " Done On Date : " & mAssemblyMonitorInspStatus.DoneOnFormatted
                        MarkLog(Util.Action.Save, "Assembly Inspection Status", mDetail, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)
                    End If

                End If

            Catch ex As Exception
                Throw ex
            End Try

        Next j
        'Added By Vikrant On 14-Jan-2015 For ALL14012015
        mUpdateComplyHistoryAssemblyMonitorInspStatusList = UpdateComplyHistoryAssemblyMonitorInspStatusList.GetComplyHistoryAssemblyMonitorInspStatusList(mAssemblyStatus.AssemblyID, mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).ModelMonitorInspID, mMachine.HourType)
        Session("mUpdateComplyHistoryAssemblyMonitorInspStatusList") = mUpdateComplyHistoryAssemblyMonitorInspStatusList
        'End
    End Sub
    Private Sub ControlVisibility()
        btnSaveNew.Enabled = (mUpdateComplyHistoryAssemblyMonitorInspStatusList.Count > 0)
        btnSaveNewTop.Enabled = (mUpdateComplyHistoryAssemblyMonitorInspStatusList.Count > 0)

        btnSaveNewTop.Visible = (mUpdateComplyHistoryAssemblyMonitorInspStatusList.Count > 10)
        btnBackTop.Visible = (mUpdateComplyHistoryAssemblyMonitorInspStatusList.Count > 10)
        'Added By Vikrant On 14-Jan-2015 For ALL14012015
        btnPrintTop.Visible = (mUpdateComplyHistoryAssemblyMonitorInspStatusList.Count > 10)
        btnPrintTop.Enabled = (mUpdateComplyHistoryAssemblyMonitorInspStatusList.Count > 0)
        btnPrint.Enabled = (mUpdateComplyHistoryAssemblyMonitorInspStatusList.Count > 0)
        'End
    End Sub

#End Region

#Region " DataBind "
    Private Sub DataFieldBind()
        'mMachineList = MachineList.GetMachineListMonitoringStatus(Today.Date.ToShortDateString, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "<SELECT>")
        'cmbAircraftList.DataSource = mMachineList
        'Session("mMachineList") = mMachineList

        dgMonitorInspStatusList.DataSource = mUpdateComplyHistoryAssemblyMonitorInspStatusList

        'txtATA.Text = Session("ATA")
        'Commenetd and Added By Vikrant For MPD
        'txtDescription.Text = Session("Description")
        txtDescription.Text = mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).Description
        'End
        txtModel.Text = mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).ModelName
        txtSerialNo.Text = mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).SerialNo

        'Added By Vikrant On 14-Jan-2015 For All14012015
        txtFrequency.Text = mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).FrequencyValueFormatted.Replace("<BR>", Chr(13))
        txtCodeFormNo.Text = mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).CodeFormNo
        txtMonitorInfo.Text = mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).MonitorInfo
        txtReference.Text = mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).Reference
        txtATA.Text = mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).ATA
        'End

        DataBind()
        ''lblResult.Text = "History for Assembly Monitor Inspection Status as per selected criteria : " & mUpdateComplyHistoryAssemblyMonitorInspStatusList.Count & " Record(s) found."
        'cmbAircraftList.SelectedValue = mMachine.ID.ToString
    End Sub

    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        Dim str As String = ""

        Dim txtDoneRemark, txtWONo, txtRequiredManHours As TextBox
        Dim j As Int32
        For j = 0 To Me.dgMonitorInspStatusList.Rows.Count - 1
            txtDoneRemark = CType(Me.dgMonitorInspStatusList.Rows(j).FindControl("txtDoneRemark"), TextBox)
            txtWONo = CType(Me.dgMonitorInspStatusList.Rows(j).FindControl("txtWONo"), TextBox)
            txtRequiredManHours = CType(Me.dgMonitorInspStatusList.Rows(j).FindControl("txtRequiredManHours"), TextBox)
            Try
                Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mUpdateComplyHistoryAssemblyMonitorInspStatusList(j).ID, mAssemblyStatus.ID, mMachine.HourType)
                mAssemblyMonitorInspStatus.DoneRemark = Trim(txtDoneRemark.Text)
                mAssemblyMonitorInspStatus.DoneWONo = Trim(txtWONo.Text)
                mAssemblyMonitorInspStatus.RequiredManHours = Trim(txtRequiredManHours.Text).Split(" ")(0)

                If Not mAssemblyMonitorInspStatus.IsValid Then
                    For i As Integer = 0 To mAssemblyMonitorInspStatus.GetBrokenRulesCollection.Count - 1
                        str = str + mAssemblyMonitorInspStatus.GetBrokenRulesCollection(i).Description + "<BR>"
                    Next
                End If
            Catch ex As Exception
                Throw ex
            End Try

        Next j

        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub
    Private Sub SetGrid()
        Dim B As Boolean
        For j As Integer = 0 To dgMonitorInspStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorInspStatusList.Rows(j).Cells(12).Text, Boolean)
            If B = False Then
                dgMonitorInspStatusList.Rows(j).Cells(11).Enabled = False
            End If
        Next
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 2-Aug-2011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            DataFieldBind()
            ControlVisibility()
            SetGrid()
        End If
    End Sub
    Private Sub btnSaveNewTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveNewTop.Click, btnSaveNew.Click
        If IsValid Then
            Save()
            dgMonitorInspStatusList.DataSource = mUpdateComplyHistoryAssemblyMonitorInspStatusList
            dgMonitorInspStatusList.DataBind()
            SetGrid()
            upnlGrid.Update()
            'Response.Redirect("wfUpdateComplyHistoryAssemblyMonitorInspStatusList.aspx?GChildPage2=Index.aspx")
            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    Private Sub btnBackTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackTop.Click, btnBack.Click
        MarkLog(Util.Action.Close, "Assembly Inspection Status", "", Util.ErrorType.NoError, mUpdateComplyHistoryAssemblyMonitorInspStatusList.Item(mUpdateComplyHistoryAssemblyMonitorInspStatusList.CurrentIndex).ID, EventLogID)
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    Private Sub dgMonitorInspStatusList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgMonitorInspStatusList.PageIndexChanging
        dgMonitorInspStatusList.PageIndex = e.NewPageIndex
        dgMonitorInspStatusList.DataSource = mUpdateComplyHistoryAssemblyMonitorInspStatusList
        dgMonitorInspStatusList.DataBind()
        Session("mUpdateComplyHistoryAssemblyMonitorInspStatusList") = mUpdateComplyHistoryAssemblyMonitorInspStatusList
        SetGrid()
    End Sub
    Private Sub dgMonitorInspStatusList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorInspStatusList.RowCommand
        Dim Index As Int32

        Select Case e.CommandName
            Case "ViewRec"
                Index = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageSize * dgMonitorInspStatusList.PageIndex
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mUpdateComplyHistoryAssemblyMonitorInspStatusList(Index).ID)
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
    'Added By Vikrant On 14-Jan-2015 For All14012015
    Private Sub btnPrintTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click, btnPrint.Click
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsComplyHistory
        Dim mCompanyDetail As New CompanyDetail
        Dim ReportName As String = String.Empty

        mUpdateComplyHistoryAssemblyMonitorInspStatusList = UpdateComplyHistoryAssemblyMonitorInspStatusList.GetComplyHistoryAssemblyMonitorInspStatusList(mAssemblyStatus.AssemblyID, mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).ModelMonitorInspID, mMachine.HourType, True)

        'mUpdateComplyHistoryCompMonitorServiceStatusList.Sort("DoneOn", System.ComponentModel.ListSortDirection.Ascending)'Sort Not Working in Some Scenarios
        myReport = New crptAssemblyInspectionComplyHistory

        If mUpdateComplyHistoryAssemblyMonitorInspStatusList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Compliance History" + Chr(13) + "(Assembly Inspection)", mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).ModelName, mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).SerialNo, mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).Description, mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).FrequencyValueFormatted, mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).CodeFormNo, AppSettings("Product Version"), AppSettings("SINote"), mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).ATA, mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).MonitorInfo, mUpdateComplyHistoryAssemblyMonitorInspStatusList(0).Reference, "", AppSettings("Logo"))

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mUpdateComplyHistoryAssemblyMonitorInspStatusList)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)

        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    'End
#End Region

End Class