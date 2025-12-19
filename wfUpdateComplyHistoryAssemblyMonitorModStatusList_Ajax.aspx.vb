'AJAX Conversion By Vikrant on 27-Mar-2015

Public Class wfUpdateComplyHistoryAssemblyMonitorModStatusList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAssemblyStatus As AssemblyStatus
    Public mMachine As Machine
    Dim Flag As Int16
    'Private mMachineList As MachineList
    Private mUpdateComplyHistoryAssemblyMonitorModStatusList As UpdateComplyHistoryAssemblyMonitorModStatusList
    'Added by Vikrant on 2-Aug-2011
    Dim EventLogID As Guid
    Public mDetail As String
    'Added By Prashant On 1-Dec-2014
    Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mUpdateComplyHistoryAssemblyMonitorModStatusList = CType(Session("mUpdateComplyHistoryAssemblyMonitorModStatusList"), UpdateComplyHistoryAssemblyMonitorModStatusList)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mMachine = CType(Session("mMachine"), Machine)
    End Sub
    Private Sub SetSession()
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMachine") = mMachine
        Session("mUpdateComplyHistoryAssemblyMonitorModStatusList") = mUpdateComplyHistoryAssemblyMonitorModStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mUpdateComplyHistoryAssemblyMonitorModStatusList")
    End Sub
    Public Sub Save()
        Dim txtDoneRemark, txtWONo, txtRequiredManHours As TextBox
        Dim j As Int32
        For j = 0 To Me.dgMonitorModStatusList.Rows.Count - 1
            txtDoneRemark = CType(Me.dgMonitorModStatusList.Rows(j).FindControl("txtDoneRemark"), TextBox)
            txtWONo = CType(Me.dgMonitorModStatusList.Rows(j).FindControl("txtWONo"), TextBox)
            txtRequiredManHours = CType(Me.dgMonitorModStatusList.Rows(j).FindControl("txtRequiredManHours"), TextBox)
            Try
                Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mUpdateComplyHistoryAssemblyMonitorModStatusList(j).ID, mAssemblyStatus.ID, mMachine.HourType)
                mAssemblyMonitorModStatus.DoneRemark = Trim(txtDoneRemark.Text)
                mAssemblyMonitorModStatus.DoneWONo = Trim(txtWONo.Text)
                mAssemblyMonitorModStatus.RequiredManHours = Trim(txtRequiredManHours.Text).Split(" ")(0)

                If mAssemblyMonitorModStatus.IsValid Then
                    If mAssemblyMonitorModStatus.IsDirty Then
                        mAssemblyMonitorModStatus.ApplyEdit()
                        mAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Save(), AssemblyMonitorModStatus)
                        'Added by Vikrant on 2-Aug-2011
                        mDetail = "Reg No. : " & dgMonitorModStatusList.Rows(j).Cells(6).Text & " Model : " & txtModel.Text & " Serial No. : " & txtSerialNo.Text & " Description : " & dgMonitorModStatusList.Rows(j).Cells(3).Text & " Done On Date : " & mAssemblyMonitorModStatus.DoneOnFormatted
                        MarkLog(Util.Action.Save, "Assembly Directive Status", mDetail, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        Next j
        'Added By Vikrant On 14-Jan-2015 For ALL14012015
        mUpdateComplyHistoryAssemblyMonitorModStatusList = UpdateComplyHistoryAssemblyMonitorModStatusList.GetComplyHistoryAssemblyMonitorModStatusList(mAssemblyStatus.AssemblyID, mUpdateComplyHistoryAssemblyMonitorModStatusList(0).ModelMonitorModID, mMachine.HourType)
        Session("mUpdateComplyHistoryAssemblyMonitorModStatusList") = mUpdateComplyHistoryAssemblyMonitorModStatusList
        'End
    End Sub
    Private Sub ControlVisibility()
        btnSaveNew.Enabled = (mUpdateComplyHistoryAssemblyMonitorModStatusList.Count > 0)
        btnSaveNewTop.Enabled = (mUpdateComplyHistoryAssemblyMonitorModStatusList.Count > 0)

        btnSaveNewTop.Visible = (mUpdateComplyHistoryAssemblyMonitorModStatusList.Count > 10)
        btnBackTop.Visible = (mUpdateComplyHistoryAssemblyMonitorModStatusList.Count > 10)
        'Added By Vikrant On 14-Jan-2015 For ALL14012015
        btnPrintTop.Visible = (mUpdateComplyHistoryAssemblyMonitorModStatusList.Count > 10)
        btnPrintTop.Enabled = (mUpdateComplyHistoryAssemblyMonitorModStatusList.Count > 0)
        btnPrint.Enabled = (mUpdateComplyHistoryAssemblyMonitorModStatusList.Count > 0)
        'End
    End Sub
    Private Sub SetGrid()
        Dim c As Boolean
        For j As Integer = 0 To dgMonitorModStatusList.Rows.Count - 1
            c = CType(Me.dgMonitorModStatusList.Rows(j).Cells(12).Text, Boolean)
            If c = False Then
                dgMonitorModStatusList.Rows(j).Cells(11).Enabled = False
            End If
        Next
    End Sub
#End Region

#Region " DataBind "
    Private Sub DataFieldBind()
        'mMachineList = MachineList.GetMachineListMonitoringStatus(Today.Date.ToShortDateString, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "<SELECT>")
        'cmbAircraftList.DataSource = mMachineList
        'Session("mMachineList") = mMachineList

        dgMonitorModStatusList.DataSource = mUpdateComplyHistoryAssemblyMonitorModStatusList

        'txtATA.Text = Session("ATA")
        txtDescription.Text = mUpdateComplyHistoryAssemblyMonitorModStatusList(0).Description
        txtModel.Text = mUpdateComplyHistoryAssemblyMonitorModStatusList(0).ModelName
        txtSerialNo.Text = mUpdateComplyHistoryAssemblyMonitorModStatusList(0).SerialNo
        'Added By Vikrant On 14-Jan-2015 For All14012015
        txtFrequency.Text = mUpdateComplyHistoryAssemblyMonitorModStatusList(0).FrequencyValueFormatted.Replace("<BR>", Chr(13))
        txtCodeFormNo.Text = mUpdateComplyHistoryAssemblyMonitorModStatusList(0).CodeFormNo
        txtMonitorInfo.Text = mUpdateComplyHistoryAssemblyMonitorModStatusList(0).MonitorInfo
        txtReference.Text = mUpdateComplyHistoryAssemblyMonitorModStatusList(0).Reference
        txtATA.Text = mUpdateComplyHistoryAssemblyMonitorModStatusList(0).ATA
        txtModNo.Text = mUpdateComplyHistoryAssemblyMonitorModStatusList(0).ModNo
        'End
        DataBind()
        ''lblResult.Text = "List of History for Assembly Monitor Directive Status as per selected criteria : " & mUpdateComplyHistoryAssemblyMonitorModStatusList.Count & " Record(s) found."
        'cmbAircraftList.SelectedValue = mMachine.ID.ToString
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        Dim str As String = ""

        Dim txtDoneRemark, txtWONo, txtRequiredManHours As TextBox
        Dim j As Int32
        For j = 0 To Me.dgMonitorModStatusList.Rows.Count - 1
            txtDoneRemark = CType(Me.dgMonitorModStatusList.Rows(j).FindControl("txtDoneRemark"), TextBox)
            txtWONo = CType(Me.dgMonitorModStatusList.Rows(j).FindControl("txtWONo"), TextBox)
            txtRequiredManHours = CType(Me.dgMonitorModStatusList.Rows(j).FindControl("txtRequiredManHours"), TextBox)
            Try
                Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mUpdateComplyHistoryAssemblyMonitorModStatusList(j).ID, mAssemblyStatus.ID, mMachine.HourType)
                mAssemblyMonitorModStatus.DoneRemark = Trim(txtDoneRemark.Text)
                mAssemblyMonitorModStatus.DoneWONo = Trim(txtWONo.Text)
                mAssemblyMonitorModStatus.RequiredManHours = Trim(txtRequiredManHours.Text).Split(" ")(0)

                If Not mAssemblyMonitorModStatus.IsValid Then
                    For i As Integer = 0 To mAssemblyMonitorModStatus.GetBrokenRulesCollection.Count - 1
                        str = str + mAssemblyMonitorModStatus.GetBrokenRulesCollection(i).Description + "<BR>"
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
        End If
    End Sub
    Private Sub dgMonitorModStatusList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgMonitorModStatusList.PageIndexChanging
        dgMonitorModStatusList.PageIndex = e.NewPageIndex
        dgMonitorModStatusList.DataSource = mUpdateComplyHistoryAssemblyMonitorModStatusList
        dgMonitorModStatusList.DataBind()
        Session("mUpdateComplyHistoryAssemblyMonitorModStatusList") = mUpdateComplyHistoryAssemblyMonitorModStatusList
        SetGrid()
    End Sub
    Private Sub dgMonitorModStatusList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorModStatusList.RowCommand
        Dim Index As Int16

        Select Case e.CommandName
            Case "ViewRec"
                Index = CInt(e.CommandArgument) + dgMonitorModStatusList.PageSize * dgMonitorModStatusList.PageIndex
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mUpdateComplyHistoryAssemblyMonitorModStatusList(Index).ID)
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
    Private Sub btnSaveNewTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveNewTop.Click, btnSaveNew.Click
        If IsValid Then
            Save()
            dgMonitorModStatusList.DataSource = mUpdateComplyHistoryAssemblyMonitorModStatusList
            dgMonitorModStatusList.DataBind()
            SetGrid()
            upnlGrid.Update()
            'Response.Redirect("wfUpdateComplyHistoryAssemblyMonitorModStatusList.aspx?GChildPage2=Index.aspx")
            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    Private Sub btnBackTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackTop.Click, btnBack.Click
        MarkLog(Util.Action.Close, "Assembly Directive Status", "", Util.ErrorType.NoError, mUpdateComplyHistoryAssemblyMonitorModStatusList.Item(mUpdateComplyHistoryAssemblyMonitorModStatusList.CurrentIndex).ID, EventLogID)
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    'Added By Vikrant On 14-Jan-2015 For All14012015
    Private Sub btnPrintTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click, btnPrint.Click
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsComplyHistory
        Dim mCompanyDetail As New CompanyDetail
        Dim ReportName As String = String.Empty

        mUpdateComplyHistoryAssemblyMonitorModStatusList = UpdateComplyHistoryAssemblyMonitorModStatusList.GetComplyHistoryAssemblyMonitorModStatusList(mAssemblyStatus.AssemblyID, mUpdateComplyHistoryAssemblyMonitorModStatusList(0).ModelMonitorModID, mMachine.HourType, True)

        'mUpdateComplyHistoryCompMonitorServiceStatusList.Sort("DoneOn", System.ComponentModel.ListSortDirection.Ascending)'Sort Not Working in Some Scenarios
        myReport = New crptAssemblyDirectiveComplyHistory

        If mUpdateComplyHistoryAssemblyMonitorModStatusList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Compliance History" + Chr(13) + "(Assembly Directive)", mUpdateComplyHistoryAssemblyMonitorModStatusList(0).ModelName, mUpdateComplyHistoryAssemblyMonitorModStatusList(0).SerialNo, mUpdateComplyHistoryAssemblyMonitorModStatusList(0).Description, mUpdateComplyHistoryAssemblyMonitorModStatusList(0).ModNo, mUpdateComplyHistoryAssemblyMonitorModStatusList(0).CodeFormNo, AppSettings("Product Version"), AppSettings("SINote"), mUpdateComplyHistoryAssemblyMonitorModStatusList(0).ATA, mUpdateComplyHistoryAssemblyMonitorModStatusList(0).MonitorInfo, mUpdateComplyHistoryAssemblyMonitorModStatusList(0).Reference, "", AppSettings("Logo"))

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mUpdateComplyHistoryAssemblyMonitorModStatusList)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)

        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    'End
#End Region

End Class