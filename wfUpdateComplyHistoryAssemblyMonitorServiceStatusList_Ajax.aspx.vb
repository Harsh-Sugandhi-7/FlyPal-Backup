'AJAX Conversion By Vikrant on 26-Mar-2015

Public Class wfUpdateComplyHistoryAssemblyMonitorServiceStatusList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAssemblyStatus As AssemblyStatus
    Public mMachine As Machine
    Dim Flag As Int16
    Private mUpdateComplyHistoryAssemblyMonitorServiceStatusList As UpdateComplyHistoryAssemblyMonitorServiceStatusList
    'Added by Vikrant on 2-Aug-2011
    Dim EventLogID As Guid
    Public mDetail As String
    'Added By Saylee On 1-Dec-2014
    Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mUpdateComplyHistoryAssemblyMonitorServiceStatusList = CType(Session("mUpdateComplyHistoryAssemblyMonitorServiceStatusList"), UpdateComplyHistoryAssemblyMonitorServiceStatusList)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mMachine = CType(Session("mMachine"), Machine)
    End Sub
    Private Sub SetSession()
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMachine") = mMachine
        Session("mUpdateComplyHistoryAssemblyMonitorServiceStatusList") = mUpdateComplyHistoryAssemblyMonitorServiceStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mUpdateComplyHistoryAssemblyMonitorServiceStatusList")
    End Sub
    Public Sub Save()
        Dim txtDoneRemark, txtWONo, txtRequiredManHours As TextBox
        Dim j As Int32
        For j = 0 To Me.dgMonitorServiceStatusList.Rows.Count - 1
            txtDoneRemark = CType(Me.dgMonitorServiceStatusList.Rows(j).FindControl("txtDoneRemark"), TextBox)
            txtWONo = CType(Me.dgMonitorServiceStatusList.Rows(j).FindControl("txtWONo"), TextBox)
            txtRequiredManHours = CType(Me.dgMonitorServiceStatusList.Rows(j).FindControl("txtRequiredManHours"), TextBox)
            Dim ID As Guid = New Guid(dgMonitorServiceStatusList.DataKeys(j).Values("ID").ToString)
            Try
                Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(ID, mAssemblyStatus.ID, mMachine.HourType)
                mAssemblyMonitorServiceStatus.DoneRemark = Trim(txtDoneRemark.Text)
                mAssemblyMonitorServiceStatus.DoneWONo = Trim(txtWONo.Text)
                mAssemblyMonitorServiceStatus.RequiredManHours = Trim(txtRequiredManHours.Text).Split(" ")(0)
                If mAssemblyMonitorServiceStatus.IsValid Then
                    If mAssemblyMonitorServiceStatus.IsDirty Then
                        mAssemblyMonitorServiceStatus.ApplyEdit()
                        mAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Save(), AssemblyMonitorServiceStatus)
                        'Added by Vikrant on 2-Aug-2011
                        mDetail = "Reg No. : " & dgMonitorServiceStatusList.Rows(j).Cells(5).Text & " Model : " & txtModel.Text & " Serial No. : " & txtSerialNo.Text & " Description : " & txtDescription.Text & " Done On Date : " & mAssemblyMonitorServiceStatus.DoneOnFormatted
                        MarkLog(Util.Action.Save, "Assembly Service Status", mDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)

                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        Next j
        'Added By Vikrant On 14-Jan-2015 For ALL14012015
        mUpdateComplyHistoryAssemblyMonitorServiceStatusList = UpdateComplyHistoryAssemblyMonitorServiceStatusList.GetComplyHistoryAssemblyMonitorServiceStatusList(mAssemblyStatus.AssemblyID, mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).ModelMonitorServiceID, mMachine.HourType)
        Session("mUpdateComplyHistoryAssemblyMonitorServiceStatusList") = mUpdateComplyHistoryAssemblyMonitorServiceStatusList
        'End
    End Sub
    Private Sub ControlVisibility()
        btnSaveNew.Enabled = (mUpdateComplyHistoryAssemblyMonitorServiceStatusList.Count > 0)
        btnSaveNewTop.Enabled = (mUpdateComplyHistoryAssemblyMonitorServiceStatusList.Count > 0)

        btnSaveNewTop.Visible = (mUpdateComplyHistoryAssemblyMonitorServiceStatusList.Count > 10)
        btnBackTop.Visible = (mUpdateComplyHistoryAssemblyMonitorServiceStatusList.Count > 10)
        'Added By Vikrant On 14-Jan-2015 For ALL14012015
        btnPrintTop.Visible = (mUpdateComplyHistoryAssemblyMonitorServiceStatusList.Count > 10)
        btnPrintTop.Enabled = (mUpdateComplyHistoryAssemblyMonitorServiceStatusList.Count > 0)
        btnPrint.Enabled = (mUpdateComplyHistoryAssemblyMonitorServiceStatusList.Count > 0)
        'End
    End Sub
    Private Sub SetGrid()
        Dim B As Boolean
        For j As Integer = 0 To dgMonitorServiceStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorServiceStatusList.Rows(j).Cells(12).Text, Boolean)
            If B = False Then
                dgMonitorServiceStatusList.Rows(j).Cells(11).Enabled = False
            End If
        Next
    End Sub
#End Region

#Region " DataBind "
    Private Sub DataFieldBind()
        dgMonitorServiceStatusList.DataSource = mUpdateComplyHistoryAssemblyMonitorServiceStatusList
        txtDescription.Text = mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).Description
        txtModel.Text = mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).ModelName
        txtSerialNo.Text = mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).SerialNo
        'Added By Vikrant On 14-Jan-2015 For All14012015
        txtFrequency.Text = mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).FrequencyValueFormatted.Replace("<BR>", Chr(13))
        txtCodeFormNo.Text = mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).CodeFormNo
        txtMonitorInfo.Text = mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).MonitorInfo
        txtReference.Text = mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).Reference
        txtATA.Text = mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).ATA
        'End
        txtTaskNo.Text = mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).TaskNo
        DataBind()
        ''slblResult.Text = "List of History for Assembly Monitor Service Status as per selected criteria : " & mUpdateComplyHistoryAssemblyMonitorServiceStatusList.Count & " Record(s) found."
        'cmbAircraftList.SelectedValue = mMachine.ID.ToString
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        Dim str As String = ""

        Dim txtDoneRemark, txtWONo, txtRequiredManHours As TextBox
        Dim j As Int32
        For j = 0 To Me.dgMonitorServiceStatusList.Rows.Count - 1
            txtDoneRemark = CType(Me.dgMonitorServiceStatusList.Rows(j).FindControl("txtDoneRemark"), TextBox)
            txtWONo = CType(Me.dgMonitorServiceStatusList.Rows(j).FindControl("txtWONo"), TextBox)
            txtRequiredManHours = CType(Me.dgMonitorServiceStatusList.Rows(j).FindControl("txtRequiredManHours"), TextBox)
            Try
                Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mUpdateComplyHistoryAssemblyMonitorServiceStatusList(j).ID, mAssemblyStatus.ID, mMachine.HourType)
                mAssemblyMonitorServiceStatus.DoneRemark = Trim(txtDoneRemark.Text)
                mAssemblyMonitorServiceStatus.DoneWONo = Trim(txtWONo.Text)
                mAssemblyMonitorServiceStatus.RequiredManHours = Trim(txtRequiredManHours.Text).Split(" ")(0)

                If Not mAssemblyMonitorServiceStatus.IsValid Then
                    For i As Integer = 0 To mAssemblyMonitorServiceStatus.GetBrokenRulesCollection.Count - 1
                        str = str + mAssemblyMonitorServiceStatus.GetBrokenRulesCollection(i).Description + "<BR>"
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
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 2-Aug-2011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            DataFieldBind()
            ControlVisibility()
            SetGrid()
            Dim ServiceMPDTitle As String = ""
            If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                lbltitle.InnerText = "History for AMP(s)"
                lblServiceInformation.InnerText = "AMP Information"
                lblMonitorInfo.InnerText = "Task Type"
            Else
                lbltitle.InnerText = "History for Assembly Service Status"
                lblServiceInformation.InnerText = "Service Information"
                lblMonitorInfo.InnerText = "Monitor Info."
            End If
        End If
    End Sub
    Private Sub btnSaveNewTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveNewTop.Click, btnSaveNew.Click
        If IsValid Then
            Save()
            dgMonitorServiceStatusList.DataSource = mUpdateComplyHistoryAssemblyMonitorServiceStatusList
            dgMonitorServiceStatusList.DataBind()
            SetGrid()
            upnlGrid.Update()
            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    Private Sub dgMonitorServiceStatusList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgMonitorServiceStatusList.PageIndexChanging
        dgMonitorServiceStatusList.PageIndex = e.NewPageIndex
        dgMonitorServiceStatusList.DataSource = mUpdateComplyHistoryAssemblyMonitorServiceStatusList
        dgMonitorServiceStatusList.DataBind()
        Session("mUpdateComplyHistoryAssemblyMonitorServiceStatusList") = mUpdateComplyHistoryAssemblyMonitorServiceStatusList
        SetGrid()
    End Sub
    Private Sub dgMonitorServiceStatusList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorServiceStatusList.RowCommand
        Dim Index As Int16

        Select Case e.CommandName
            Case "ViewRec"
                Index = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageSize * dgMonitorServiceStatusList.PageIndex
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mUpdateComplyHistoryAssemblyMonitorServiceStatusList(Index).ID)
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
    Private Sub btnBackTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackTop.Click, btnBack.Click
        MarkLog(Util.Action.Close, "Assembly Service Status", "", Util.ErrorType.NoError, mUpdateComplyHistoryAssemblyMonitorServiceStatusList.Item(mUpdateComplyHistoryAssemblyMonitorServiceStatusList.CurrentIndex).ID, EventLogID)
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub

    'Added By Vikrant On 14-Jan-2015 For All14012015
    Private Sub Print(sender As Object, e As EventArgs) Handles btnPrintTop.Click, btnPrint.Click

        Dim myReport As Engine.ReportClass
        Dim da As New ObjectAdapter
        Dim ds As New dsComplyHistory
        Dim mCompanyDetail As New CompanyDetail
        Dim ReportName As String = String.Empty

        mUpdateComplyHistoryAssemblyMonitorServiceStatusList = UpdateComplyHistoryAssemblyMonitorServiceStatusList.
                                                                GetComplyHistoryAssemblyMonitorServiceStatusList(mAssemblyStatus.AssemblyID,
                                                                                                                 mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).
                                                                                                                                    ModelMonitorServiceID,
                                                                                                                 mMachine.HourType)

        myReport = New crptAssemblyServiceComplyHistory

        If mUpdateComplyHistoryAssemblyMonitorServiceStatusList.Count <= 0 Then

            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound,
                            MSGBox.Message_text.NoRecordFound,
                            "There is no record for this search criteria",
                            MsgBoxStyle.OkOnly,
                            "")
            Exit Sub

        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                     mCompanyDetail.Address,
                                     mCompanyDetail.Tel1,
                                     mCompanyDetail.Tel2,
                                     mCompanyDetail.Fax,
                                     mCompanyDetail.Email,
                                     mCompanyDetail.WebSite,
                                     "Compliance History" + Chr(13) + "(Assembly Service)",
                                     mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).ModelName,
                                     mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).SerialNo,
                                     mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).Description,
                                     "", mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).CodeFormNo,
                                     AppSettings("Product Version"),
                                     AppSettings("SINote"),
                                     mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).ATA,
                                     mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).MonitorInfo,
                                     mUpdateComplyHistoryAssemblyMonitorServiceStatusList(0).Reference,
                                     "",
                                     AppSettings("Logo"))

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mUpdateComplyHistoryAssemblyMonitorServiceStatusList)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)

        Session("CrystalReport") = myReport

        ScriptManager.RegisterStartupScript(Me,
                                            Me.GetType(),
                                            "openTranDetail",
                                            "openTranDetail();",
                                            True)

    End Sub

#End Region

End Class