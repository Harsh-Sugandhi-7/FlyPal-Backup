
'AJAX Conversion by Saylee on 27-Mar-2015

Public Class wfUpdateComplyHistoryCompMonitorInspStatusList_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAssemblyStatus As AssemblyStatus
    Public mCompStatus As CompStatus
    Public mMachine As Machine
    Dim Flag As Int16
    Private mUpdateComplyHistoryCompMonitorInspStatusList As UpdateComplyHistoryCompMonitorInspStatusList
    'Added by Vikrant on 2-Aug-2011
    Dim EventLogID As Guid
    Public mDetail As String
    'Added By Saylee On 1-Dec-2014
    Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mUpdateComplyHistoryCompMonitorInspStatusList = CType(Session("mUpdateComplyHistoryCompMonitorInspStatusList"), UpdateComplyHistoryCompMonitorInspStatusList)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mMachine = CType(Session("mMachine"), Machine)
    End Sub
    Private Sub SetSession()
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mMachine") = mMachine
        Session("mUpdateComplyHistoryCompMonitorInspStatusList") = mUpdateComplyHistoryCompMonitorInspStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mUpdateComplyHistoryCompMonitorInspStatusList")
    End Sub
    Public Sub Save()
        Dim txtDoneRemark, txtWONo, txtRequiredManHours As TextBox
        Dim j As Int32
        'Added by Saylee on 5-Nov-2020 for ALL27072020
        Dim mHourType As Integer = 1
        Dim mAssemblyStatusID As Guid = Guid.Empty
        If mCompStatus.IsSpareComp = False Then

            mHourType = mMachine.HourType
            mAssemblyStatusID = mAssemblyStatus.ID
        End If
        '***********

        For j = 0 To Me.dgMonitorInspStatusList.Rows.Count - 1
            txtDoneRemark = CType(Me.dgMonitorInspStatusList.Rows(j).FindControl("txtDoneRemark"), TextBox)
            txtWONo = CType(Me.dgMonitorInspStatusList.Rows(j).FindControl("txtWONo"), TextBox)
            txtRequiredManHours = CType(Me.dgMonitorInspStatusList.Rows(j).FindControl("txtRequiredManHours"), TextBox)
            Try
                ''Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mUpdateComplyHistoryCompMonitorInspStatusList(j).ID, mAssemblyStatus.ID, mCompStatus.ID, mMachine.HourType)
                '' Dim mCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mUpdateComplyHistoryCompMonitorInspStatusList(j).ID, mAssemblyStatus.ID, mCompStatus.ID, mMachine.HourType)
                ''Dim mCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetComplyCompMonitorInspStatusFromEntry(mPrevCompMonitorInspStatus.ID, mPrevCompMonitorInspStatus.AssemblyStatusID, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mMachine.HourType)

                Dim mCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mUpdateComplyHistoryCompMonitorInspStatusList(j).ID, mAssemblyStatusID, mCompStatus.ID, mHourType, CompStatus:=mCompStatus)
                mCompMonitorInspStatus.DoneRemark = Trim(txtDoneRemark.Text)
                mCompMonitorInspStatus.DoneWONo = Trim(txtWONo.Text)
                mCompMonitorInspStatus.RequiredManHours = Trim(txtRequiredManHours.Text).Split(" ")(0)

                If mCompMonitorInspStatus.IsValid Then
                    If mCompMonitorInspStatus.IsDirty Then
                        mCompMonitorInspStatus.ApplyEdit()
                        mCompMonitorInspStatus = CType(mCompMonitorInspStatus.Save(), CompMonitorInspStatus)
                        'Added by Vikrant on 2-Aug-2011
                        mDetail = "Reg No. : " & dgMonitorInspStatusList.Rows(j).Cells(4).Text & " Part : " & txtPart.Text & " Serial No. : " & txtSerialNo.Text & " Description : " & dgMonitorInspStatusList.Rows(j).Cells(3).Text & " Done On Date : " & dgMonitorInspStatusList.Rows(j).Cells(4).Text
                        MarkLog(Util.Action.Save, "Component Inspection Status", mDetail, Util.ErrorType.NoError, mCompMonitorInspStatus.ID, EventLogID)
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try

        Next j
        'Added By Vikrant On 14-Jan-2015 For ALL14012015
        mUpdateComplyHistoryCompMonitorInspStatusList = UpdateComplyHistoryCompMonitorInspStatusList.GetComplyHistoryCompMonitorInspStatusList(mCompStatus.CompID, mUpdateComplyHistoryCompMonitorInspStatusList(0).PartMonitorInspID, mHourType)
        Session("mUpdateComplyHistoryCompMonitorInspStatusList") = mUpdateComplyHistoryCompMonitorInspStatusList
        'End
    End Sub
    Private Sub ControlVisibility()
        btnSaveNew.Enabled = (mUpdateComplyHistoryCompMonitorInspStatusList.Count > 0)
        btnSaveNewTop.Enabled = (mUpdateComplyHistoryCompMonitorInspStatusList.Count > 0)

        btnSaveNewTop.Visible = (mUpdateComplyHistoryCompMonitorInspStatusList.Count > 10)
        btnBackTop.Visible = (mUpdateComplyHistoryCompMonitorInspStatusList.Count > 10)
        'Added By Vikrant On 14-Jan-2015 For ALL14012015
        btnPrintTop.Visible = (mUpdateComplyHistoryCompMonitorInspStatusList.Count > 10)
        btnPrintTop.Enabled = (mUpdateComplyHistoryCompMonitorInspStatusList.Count > 0)
        btnPrint.Enabled = (mUpdateComplyHistoryCompMonitorInspStatusList.Count > 0)
        'End
    End Sub
    Private Sub SetGrid()
        Dim B As Boolean
        For j As Integer = 0 To dgMonitorInspStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorInspStatusList.Rows(j).Cells(13).Text, Boolean)
            If B = False Then
                dgMonitorInspStatusList.Rows(j).Cells(12).Enabled = False
            End If
        Next
    End Sub
#End Region

#Region " DataBind "
    Private Sub DataFieldBind()
        dgMonitorInspStatusList.DataSource = mUpdateComplyHistoryCompMonitorInspStatusList

        txtATA.Text = mUpdateComplyHistoryCompMonitorInspStatusList(0).ATA
        txtDescription.Text = mUpdateComplyHistoryCompMonitorInspStatusList(0).Description
        txtPart.Text = mUpdateComplyHistoryCompMonitorInspStatusList(0).PartName
        txtSerialNo.Text = mUpdateComplyHistoryCompMonitorInspStatusList(0).SerialNo

        'Added By Vikrant On 14-Jan-2015 For All14012015
        txtFrequency.Text = mUpdateComplyHistoryCompMonitorInspStatusList(0).FrequencyValueFormatted.Replace("<BR>", Chr(13))
        txtCodeFormNo.Text = mUpdateComplyHistoryCompMonitorInspStatusList(0).CodeFormNo
        txtMonitorInfo.Text = mUpdateComplyHistoryCompMonitorInspStatusList(0).MonitorInfo
        txtReference.Text = mUpdateComplyHistoryCompMonitorInspStatusList(0).Reference
        'End
        DataBind()
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        Dim str As String = ""

        'Added by Saylee on 5-Nov-2020 for ALL27072020
        Dim mHourType As Integer = 1
        Dim mAssemblyStatusID As Guid = Guid.Empty
        If mCompStatus.IsSpareComp = False Then

            mHourType = mMachine.HourType
            mAssemblyStatusID = mAssemblyStatus.ID
        End If
        '***********

        Dim txtDoneRemark, txtWONo, txtRequiredManHours As TextBox
        Dim j As Int32
        For j = 0 To Me.dgMonitorInspStatusList.Rows.Count - 1
            txtDoneRemark = CType(Me.dgMonitorInspStatusList.Rows(j).FindControl("txtDoneRemark"), TextBox)
            txtWONo = CType(Me.dgMonitorInspStatusList.Rows(j).FindControl("txtWONo"), TextBox)
            txtRequiredManHours = CType(Me.dgMonitorInspStatusList.Rows(j).FindControl("txtRequiredManHours"), TextBox)
            Try
                Dim mCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mUpdateComplyHistoryCompMonitorInspStatusList(j).ID, mAssemblyStatusID, mCompStatus.ID, mHourType, CompStatus:=mCompStatus)
                mCompMonitorInspStatus.DoneRemark = Trim(txtDoneRemark.Text)
                mCompMonitorInspStatus.DoneWONo = Trim(txtWONo.Text)
                mCompMonitorInspStatus.RequiredManHours = Trim(txtRequiredManHours.Text).Split(" ")(0)

                If Not mCompMonitorInspStatus.IsValid Then
                    For i As Integer = 0 To mCompMonitorInspStatus.GetBrokenRulesCollection.Count - 1
                        str = str + mCompMonitorInspStatus.GetBrokenRulesCollection(i).Description + "<BR>"
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
    Private Sub btnSaveNewTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveNewTop.Click, btnSaveNew.Click
        If IsValid Then
            Save()
            dgMonitorInspStatusList.DataSource = mUpdateComplyHistoryCompMonitorInspStatusList
            dgMonitorInspStatusList.DataBind()
            SetGrid()
            upnlGrid.Update()
            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    Private Sub dgMonitorInspStatusList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgMonitorInspStatusList.PageIndexChanging
        dgMonitorInspStatusList.PageIndex = e.NewPageIndex
        dgMonitorInspStatusList.DataSource = mUpdateComplyHistoryCompMonitorInspStatusList
        dgMonitorInspStatusList.DataBind()
        Session("mUpdateComplyHistoryCompMonitorInspStatusList") = mUpdateComplyHistoryCompMonitorInspStatusList
        SetGrid()
    End Sub
    Private Sub dgMonitorInspStatusList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorInspStatusList.RowCommand
        Dim Index As Int16

        Select Case e.CommandName
            Case "ViewRec"
                Index = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageSize * dgMonitorInspStatusList.PageIndex
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mUpdateComplyHistoryCompMonitorInspStatusList(Index).ID)
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
        MarkLog(Util.Action.Close, "Component Insp Status", "", Util.ErrorType.NoError, mUpdateComplyHistoryCompMonitorInspStatusList.Item(mUpdateComplyHistoryCompMonitorInspStatusList.CurrentIndex).ID, EventLogID)
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
        'Added by Saylee on 5-Nov-2020 for ALL27072020
        Dim mHourType As Integer = 1
        If mCompStatus.IsSpareComp = False Then

            mHourType = mMachine.HourType
        End If
        '***********

        mUpdateComplyHistoryCompMonitorInspStatusList = UpdateComplyHistoryCompMonitorInspStatusList.GetComplyHistoryCompMonitorInspStatusList(mCompStatus.CompID, mUpdateComplyHistoryCompMonitorInspStatusList(0).PartMonitorInspID, mHourType, True)

        'mUpdateComplyHistoryCompMonitorServiceStatusList.Sort("DoneOn", System.ComponentModel.ListSortDirection.Ascending)'Sort Not Working in Some Scenarios
        myReport = New crptCompInspectionComplyHistory

        If mUpdateComplyHistoryCompMonitorInspStatusList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
              mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
              mCompanyDetail.WebSite, "Compliance History" + Chr(13) + "(Component Inspection)", mUpdateComplyHistoryCompMonitorInspStatusList(0).PartName, mUpdateComplyHistoryCompMonitorInspStatusList(0).SerialNo, mUpdateComplyHistoryCompMonitorInspStatusList(0).Description, mUpdateComplyHistoryCompMonitorInspStatusList(0).FrequencyValueFormatted, mUpdateComplyHistoryCompMonitorInspStatusList(0).CodeFormNo, AppSettings("Product Version"), AppSettings("SINote"), mUpdateComplyHistoryCompMonitorInspStatusList(0).ATA, mUpdateComplyHistoryCompMonitorInspStatusList(0).MonitorInfo, mUpdateComplyHistoryCompMonitorInspStatusList(0).Reference, "", AppSettings("Logo"))

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mUpdateComplyHistoryCompMonitorInspStatusList)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)

        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    'End
#End Region

End Class