Public Class wfPendingMELSnagListForNRCJobs_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMELSnagCorrectiveActionListNew As MELSnagCorrectiveActionListNew
    Public AssemblyId As String
    Public ATAChapterId As String
    Dim Name, No, ATANomenclature As String
    Dim StatusCode, ATACode, MELSnagCode As Integer
    Public mATAList As ATAList 'Added By Saylee on 12-Aug-2010
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Dim mMELSnagDetail As String
    Dim mAssemblylist As AssemblyList 'Added By Vikrant On 02-Sept-2014 For All04092014
    Dim mNRC As NRC
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        StatusCode = Session("StatusCode")
        MELSnagCode = Session("MELSnagCode")
        mMELSnagCorrectiveActionListNew = Session("mMELSnagCorrectiveActionListNew")
        mATAList = CType(Session("mATAList"), ATAList)
        ATAChapterId = CType(Session("ATAChapterId"), String)
        mAssemblylist = Session("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
        AssemblyId = CType(Session("AssemblyId"), String)
        mNRC = Session("mNRC")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMELSnagCorrectiveActionListNew")
        Session.Remove("mATAList")
        Session.Remove("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
        Session.Remove("AssemblyId")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "" Then
            Session.Remove("mMELSnagCorrectiveActionListNew")
            Session.Remove("Name")
            Session.Remove("StatusCode")
            Session.Remove("MELSnagCode")
            Session.Remove("ATACode")
            Session.Remove("ATANomenclature")
            Session.Remove("ATAChapterId")
            Session.Remove("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
            Session.Remove("AssemblyId")
        End If
    End Sub
    Private Sub FindNow(Optional ByVal ATACode As Integer = 0, Optional ByVal ATANomenclature As String = "", Optional ByVal MELSnag As Integer = 0, Optional ByVal AssemblyStatusID As String = "{00000000-0000-0000-0000-000000000000}")
        'Get List From the Database as per Criteria  
        If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then
            mMELSnagCorrectiveActionListNew = MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(ToDate:=mNRC.NRCDateFormatted.ToString, MachineID:=mNRC.MachineID.ToString, InvestigationStatus:=2, TimeFormat:="HH:mm", ATACode:=ATACode, ATANomenclature:=ATANomenclature, AssemblyStatusID:=AssemblyStatusID, MELSnag:=MELSnag)
        Else
            mMELSnagCorrectiveActionListNew = MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(ToDate:=mNRC.NRCDateFormatted.ToString, MachineID:=mNRC.MachineID.ToString, InvestigationStatus:=2, TimeFormat:="HH:mm", ATACode:=ATACode, ATANomenclature:=ATANomenclature, AssemblyStatusID:=AssemblyStatusID, MELSnag:=MELSnag)
        End If
        'Set DataSource of the Grid
        dgSnagCorrectiveActionList.DataSource = mMELSnagCorrectiveActionListNew
        Session("mMELSnagCorrectiveActionListNew") = mMELSnagCorrectiveActionListNew
        dgSnagCorrectiveActionList.DataBind()
        lblResult.Text = "List of Open " + IIf(AppSettings("MELSnagNomenclature") = "True", "ADD/Defect", "MEL/Snag") + " for Aircraft " & mNRC.RegNo & " till " & mNRC.NRCDateFormatted.ToString & " : " & mMELSnagCorrectiveActionListNew.Count & " Record(s) found."
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added

            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""

        End If
    End Sub
    Private Sub SetControl()
        Name = Session("Name")
        StatusCode = CType(Session("StatusCode"), Integer)
        MELSnagCode = CType(Session("MELSnagCode"), Integer)
        ATACode = CType(Session("ATACode"), Integer)
        ATANomenclature = Session("ATANomenclature")

        ATAChapterId = Session("ATAChapterId")

        FindNow(mATAList(New Guid(ATAChapterId)).ATACode, mATAList(New Guid(ATAChapterId)).ATANomenclature, MELSnagCode, AssemblyId)

        dgSnagCorrectiveActionList.DataBind()
        cmbMELSnag.SelectedValue = MELSnagCode
        cmbATAChapter.SelectedValue = ATAChapterId
        cmbAssembly.SelectedValue = AssemblyId

        upnlGridView.Update()
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
    End Sub
#End Region

#Region " DataBinding "
    Public Sub DataFieldBind()
        MELSnagCode = Session("MELSnagCode")
        Session("MELSnagCode") = MELSnagCode

        ATACode = Session("ATACode")
        Session("ATACode") = ATACode

        ATANomenclature = Session("ATANomenclature")
        Session("ATANomenclature") = ATANomenclature

        If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then
            mMELSnagCorrectiveActionListNew = MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(ToDate:=mNRC.NRCDateFormatted.ToString, MachineID:=mNRC.MachineID.ToString, InvestigationStatus:=2, TimeFormat:="HH:mm")
        Else
            mMELSnagCorrectiveActionListNew = MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(ToDate:=mNRC.NRCDateFormatted.ToString, MachineID:=mNRC.MachineID.ToString, InvestigationStatus:=2)
        End If
        dgSnagCorrectiveActionList.DataSource = mMELSnagCorrectiveActionListNew

        mATAList = ATAList.GetATAList("", "(All)") 'Added By Saylee on 12-Aug-2010
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList

        Name = Session("Name")
        Session("Name") = Name
        Session("mMELSnagCorrectiveActionListNew") = mMELSnagCorrectiveActionListNew
        If mATAList.Count <> 0 Then
            If IsNothing(ATAChapterId) Then ATAChapterId = mATAList(0).ID.ToString Else ATAChapterId = ATAChapterId
        Else
            ATAChapterId = "00000000-0000-0000-0000-000000000000"
        End If

        'Added By Vikrant On 02-Sept-2014 For All04092014
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, mNRC.MachineID.ToString, Today.Date.ToString, "(All)", True)
        cmbAssembly.DataSource = mAssemblylist
        Session("mAssemblylist") = mAssemblylist
        'End
        dgSnagCorrectiveActionList.Columns(13).HeaderText = IIf(AppSettings("MELSnagNomenclature") = "True", "Is ADD", "Is MEL")
        Try
            DataBind()
        Catch ex As Exception

        End Try

        cmbMELSnag.SelectedValue = MELSnagCode


        If mATAList.Count > 1 Then cmbATAChapter.SelectedIndex = 0 Else cmbATAChapter.SelectedValue = ATAChapterId
        ATAChapterId = cmbATAChapter.SelectedValue
        Session("ATAChapterId") = ATAChapterId
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
        Else
            dgSnagCorrectiveActionList.DataSource = mMELSnagCorrectiveActionListNew
            dgSnagCorrectiveActionList.DataBind()
        End If
        lblResult.Text = "List of Open " + IIf(AppSettings("MELSnagNomenclature") = "True", "ADD/Defect", "MEL/Snag") + " for Aircraft " & mNRC.RegNo & " till " & mNRC.NRCDateFormatted.ToString & " : " & mMELSnagCorrectiveActionListNew.Count & " Record(s) found."
        dgSnagCorrectiveActionList.Columns(14).HeaderText = IIf(AppSettings("ClientCode") = "IND", "OJS Nos.", "NRC Nos.")
        'Added By Vikrant On 07-Sep-2020 For ALL07092020
        cmbMELSnag.Items(1).Text = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD", "MEL")
        cmbMELSnag.Items(2).Text = IIf(AppSettings("MELSnagNomenclature") = "True", "Defect", "Snag")
        dgSnagCorrectiveActionList.Columns(13).HeaderText = IIf(AppSettings("MELSnagNomenclature") = "True", "Is ADD", "Is MEL")
        'End
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        MELSnagCode = cmbMELSnag.SelectedValue
        AssemblyId = cmbAssembly.SelectedValue

        Session("Name") = Name
        Session("ATACode") = mATAList(New Guid(cmbATAChapter.SelectedValue)).ATACode
        Session("ATANomenclature") = mATAList(New Guid(cmbATAChapter.SelectedValue)).ATANomenclature
        Session("ATAChapterId") = mATAList(New Guid(cmbATAChapter.SelectedValue)).ID.ToString
        Session("MELSnagCode") = MELSnagCode
        Session("AssemblyId") = AssemblyId


        dgSnagCorrectiveActionList.PageIndex = 0
        FindNow(mATAList(New Guid(cmbATAChapter.SelectedValue)).ATACode, mATAList(New Guid(cmbATAChapter.SelectedValue)).ATANomenclature, MELSnagCode, cmbAssembly.SelectedValue.ToString)

        upnlGridView.Update()
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
    End Sub
    Private Sub dgSnagCorrectiveActionList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgSnagCorrectiveActionList.PageIndexChanging
        dgSnagCorrectiveActionList.PageIndex = e.NewPageIndex
        dgSnagCorrectiveActionList.DataSource = mMELSnagCorrectiveActionListNew
        Session("mMELSnagCorrectiveActionListNew") = mMELSnagCorrectiveActionListNew
        dgSnagCorrectiveActionList.DataBind()
    End Sub
    Private Sub dgSnagCorrectiveActionList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSnagCorrectiveActionList.RowCommand
        Select Case e.CommandName
            Case "Select"
                Dim Idx As Integer = CInt(e.CommandArgument) + dgSnagCorrectiveActionList.PageSize * dgSnagCorrectiveActionList.PageIndex
                If mNRC.NRCJobs.Contains(mMELSnagCorrectiveActionListNew(Idx).ID) Then
                    MSGBoxCtrl.show("Alert !", "Selected " + IIf(AppSettings("MELSnagNomenclature") = "True", "ADD/Defect", "MEL/Snag") + " is already added as Job.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim MELSnagDetail, MEL, CompInfo As String
                MEL = IIf(mMELSnagCorrectiveActionListNew(Idx).IsMEL, IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Category : ", "MEL Category : ") + mMELSnagCorrectiveActionListNew(Idx).MELCategoryName + IIf(Trim(mMELSnagCorrectiveActionListNew(Idx).ATAChapter) <> "", ", ATA : " + Trim(mMELSnagCorrectiveActionListNew(Idx).ATAChapter) + IIf(mMELSnagCorrectiveActionListNew(Idx).DueDateFormatted.ToString <> "", ", Due Date : " + mMELSnagCorrectiveActionListNew(Idx).DueDateFormatted.ToString, ""), ""), "")
                CompInfo = IIf(mMELSnagCorrectiveActionListNew(Idx).PartNo <> "" Or mMELSnagCorrectiveActionListNew(Idx).Description <> "" Or mMELSnagCorrectiveActionListNew(Idx).PartSerialNo <> "", "Comp. Info. : " + IIf(mMELSnagCorrectiveActionListNew(Idx).PartNo <> "", "Part No. : " + mMELSnagCorrectiveActionListNew(Idx).PartNo, "") + IIf(mMELSnagCorrectiveActionListNew(Idx).Description <> "", " Description : " + mMELSnagCorrectiveActionListNew(Idx).Description, "") + IIf(mMELSnagCorrectiveActionListNew(Idx).PartNo <> "", " Serial No. : " + mMELSnagCorrectiveActionListNew(Idx).PartSerialNo, ""), "")
                MELSnagDetail = "Defect No. : " + mMELSnagCorrectiveActionListNew(Idx).DefectNo + Environment.NewLine + "Defect Desc. : " +
                                mMELSnagCorrectiveActionListNew(Idx).Defect + Environment.NewLine + "Date of Occurrence : " +
                                mMELSnagCorrectiveActionListNew(Idx).DateOfOccurrenceFormatted.ToString + Environment.NewLine + "Log No. : " +
                                mMELSnagCorrectiveActionListNew(Idx).LogNo + IIf(CompInfo <> "", Environment.NewLine + CompInfo, "") +
                                IIf(MEL <> "", Environment.NewLine + MEL, "")
                mNRC.NRCJobs.CurrentItem.MELSnagCorrectiveActionID = mMELSnagCorrectiveActionListNew(Idx).ID
                mNRC.NRCJobs.CurrentItem.Observation = MELSnagDetail
                Session("mNRC") = mNRC
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenNRCWindow", "OpenNRCWindow();", True)

        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        If mNRC.NRCJobs.CurrentItem.IsNew Then mNRC.NRCJobs.Remove(mNRC.NRCJobs.CurrentItem)
        Session("mNRC") = mNRC
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub dgSnagCorrectiveActionList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSnagCorrectiveActionList.Sorting
        mMELSnagCorrectiveActionListNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMELSnagCorrectiveActionListNew") = mMELSnagCorrectiveActionListNew
        dgSnagCorrectiveActionList.DataSource = mMELSnagCorrectiveActionListNew
        dgSnagCorrectiveActionList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    
End Class