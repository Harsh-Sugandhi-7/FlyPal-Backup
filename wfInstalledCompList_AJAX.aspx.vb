Imports System.Linq
Imports System.Collections
Imports System.Collections.Generic
Imports System.Web.Script.Serialization


'AJAX Conversion By: Saylee on 25-Mar-2015 : ModuleID:302

Public Class wfInstalledCompList_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    'New Added
    Public mCompStatusList As CompStatusList
    Public mInstList As List(Of CompStatusInfo) = New List(Of CompStatusInfo)
    Public mRemList As List(Of CompStatusInfo) = New List(Of CompStatusInfo)
    'End

    'Public mInstalledCompList As tmpInstalledCompList
    'Public mRemovedCompList As tmpRemovedCompList
    Public mMachineNameValueList As MachineNameValueList
    Public mAssemblylist As AssemblyList
    Public RemoveDate As String
    Public AircraftId As String
    Public AssemblyId As String
    Public PartNo As String 'added by Rahul on 29-apr-09

    Public mMachineMaintenance As MachineMaintenance      'Added by Saylee on 8th-Oct-2009

    Dim EventLogID As Guid 'Added By Utkarsh On 26-Jul-2011 For All19072011
    Dim MaintDetail As String 'Added By Utkarsh On 26-Jul-2011 For All19072011

    'Added by Vikrant on 26-July-2011
    Public mRegNo As String
    Public mAssemblyInfo As String
    Public mAssemblyType As String
    Dim mCompStatusID As Guid
    Dim mFileAttach As FileAttach 'Added By Vikrant On 01-Dec-2014
    Dim RecordsToShowForRemCompList As Integer
    Dim RecordsToShowForInstCompList As Integer

    Dim IsReadOnly As Boolean 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
    Public mSpareAssemblyComponent As Integer 'Added By Vikrant On 24-Dec-2020 For ALL27072020
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        'New Added
        mInstList = CType(Session("mInstList"), List(Of CompStatusInfo))
        mRemList = CType(Session("mRemList"), List(Of CompStatusInfo))
        mCompStatusList = CType(Session("mCompStatusList"), CompStatusList)
        'End
        'mInstalledCompList = CType(Session("mInstalledCompList"), tmpInstalledCompList)
        'mRemovedCompList = CType(Session("mRemovedCompList"), tmpRemovedCompList)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        AircraftId = CType(Session("AircraftId"), String)
        RemoveDate = CType(Session("RemoveDate"), String)
        'Added by Rahul on 29-Apr-2009
        Part = CType(Session("Part"), String)
        SerialNo = CType(Session("SerialNo"), String)
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 8th-Oct-2009
        mFileAttach = Session("mFileAttach") 'Added By Vikrant On 01-Dec-2014
        RecordsToShowForRemCompList = CType(Session("RecordsToShowForRemCompList"), Integer)
        RecordsToShowForInstCompList = CType(Session("RecordsToShowForInstCompList"), Integer)
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        mSpareAssemblyComponent = CType(Session("mSpareAssemblyComponent"), Integer) 'Added By Vikrant On 24-Dec-2020 For ALL27072020
    End Sub
    Private Sub RemoveSession()
        'Session.Remove("mInstalledCompList")
        'Session.Remove("mRemovedCompList")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mMachineMaintenance") 'Added by Saylee on 8th-Oct-2009
        Session.Remove("mFileAttach") 'Added By Vikrant On 01-Dec-2014
        Session.Remove("RecordsToShowForRemCompList")
        Session.Remove("RecordsToShowForInstCompList")
        'New Added
        Session.Remove("mCompStatusList")
        Session.Remove("mInstList")
        Session.Remove("mRemList")
        'End
        Session.Remove("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft

    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfInstalledCompList_Ajax.aspx?SpareAssemblyComponent=" & Session("mSpareAssemblyComponent") Then
            'Session.Remove("mInstalledCompList")
            'Session.Remove("mRemovedCompList")
            Session.Remove("mMachineNameValueList")
            Session.Remove("AircraftId")
            Session.Remove("RemoveDate")

            'Added by Rahul on 29-Apr-2009
            Session.Remove("Model")
            Session.Remove("SerialNo")
            ''====================

            Session.Remove("mMachineMaintenance") 'Added by Saylee on 8th-Oct-2009
            Session.Remove("mFileAttach") 'Added By Vikrant On 01-Dec-2014
            Session.Remove("RecordsToShowForRemCompList")
            Session.Remove("RecordsToShowForInstCompList")
            'New Added
            Session.Remove("mCompStatusList")
            Session.Remove("mInstList")
            Session.Remove("mRemList")
            'End
            Session.Remove("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
            Session.Remove("mSpareAssemblyComponent") 'Added By Vikrant On 24-Dec-2020 For ALL27072020
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        'Response.Redirect("wfInstalledCompList_Ajax.aspx?MsgResult=0")
                    Else
                        Session("sender") = ""
                        'Response.Redirect("wfInstalledCompList_Ajax.aspx?MsgResult=0")
                    End If
                Case MsgBoxResult.Yes
                    Try
                        If MSGBoxCtrl.Sender = "RevertRemoval" Then
                            Session("sender") = ""
                            'Added by Saylee on 8th-Oct-2009
                            Dim RevertRemovalID As Guid = Session("RevertRemovalID")
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(RevertRemovalID, 4)
                            '=============================
                            'Changed by vikrant on 26-July-2011
                            mRegNo = mCompStatusList(RevertRemovalID).MachineInfo 'cmbMachine.SelectedItem.Text
                            mAssemblyType = mCompStatusList(RevertRemovalID).AssemblyType
                            mAssemblyInfo = mCompStatusList(RevertRemovalID).AssemblyInfo
                            mCompStatusID = mCompStatusList(RevertRemovalID).ID
                            MaintDetail = "Reg No. : " & mRegNo & " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo & " Part Info : " & mCompStatusList(RevertRemovalID).CompInfo.Replace(Environment.NewLine, " ") + " Removed On : " + mCompStatusList(RevertRemovalID).RemovedOnFormatted.ToString

                            'Added by Saylee on 28-Nov-2014
                            If mCompStatusList(RevertRemovalID).IsRemAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(RevertRemovalID, 2) 'Sort = 2: for Removal 
                            End If

                            CompStatus.RevertRemovalCompStatus(mCompStatusList(RevertRemovalID).ID, mCompStatusList(RevertRemovalID).RemovedOnFormatted.ToString, mCompStatusList(RevertRemovalID).IsExpired, mCompStatusList(RevertRemovalID).AssemblyStatusID)

                            Try
                                MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                                If Not mFileAttach Is Nothing Then
                                    If mFileAttach.Size > 0 Then
                                        FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID, 2) 'Sort = 2: for Removal 
                                    End If
                                End If
                                'Added by Utkarsh on 11-Feb-2014 For Tech Direction Report
                                Dim mtechDirection As rptTechDirection = rptTechDirection.GetTechDirection(mCompStatusList(RevertRemovalID).ID, 2) '2 for compoenent
                                If Not mtechDirection.IsNew Then 'there is no entry for current component.
                                    rptTechDirection.DeleteTechDirection(mtechDirection.ID)
                                End If
                                'end
                                ' MaintDetail = "Reg No. : " + mRemovedCompList(mRemovedCompList.CurrentIndex).MachineInfo & " Assembly Info : " & mRemovedCompList(mRemovedCompList.CurrentIndex).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mRemovedCompList(mRemovedCompList.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") + " Removed On : " + mRemovedCompList(mRemovedCompList.CurrentIndex).RemovedOnFormatted
                                MarkLog(Util.Action.Save, "CompRemoval", "Revert: " & MaintDetail, Util.ErrorType.NoError, RevertRemovalID, EventLogID)
                                FindNow()
                                SetCaption()
                                'SetGrid()
                                ControlVisibility()
                                UpnlInstalledCompList.Update()
                                upnlRemovedCompList.Update()
                                upnlInstalledCompHeader.Update()
                                upnlRemovedCompHeader.Update()
                                upnlPrintInstalledCompList.Update()
                                upnlButtons.Update()
                            Catch ex As Exception
                                '
                            End Try
                            'Response.Redirect("wfInstalledCompList_Ajax.aspx?MsgResult=0")
                        End If
                    Catch ex As SqlException
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DatabaseException, SIMsgBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly)
                        'msg1.ReplacePage = "wfInstalledCompList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
                        DataFieldBind()
                        FindNow()
                        upnlRemovedCompHeader.Update()
                        upnlRemovedCompList.Update()
                        msgCount = ex.Errors.Count
                    Finally
                        If msgCount = 0 Then
                            'MaintDetail = "Reg No. : " + mRemovedCompList(mRemovedCompList.CurrentIndex).MachineInfo & " Assembly Info : " & mRemovedCompList(mRemovedCompList.CurrentIndex).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mRemovedCompList(mRemovedCompList.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") + " Removed On : " + mRemovedCompList(mRemovedCompList.CurrentIndex).RemovedOnFormatted
                            MarkLog(Util.Action.RevertRemoval, "ComponentRemoval", MaintDetail, Util.ErrorType.NoError, mCompStatusID, EventLogID)
                        End If
                    End Try
                Case MsgBoxResult.No
                    Session("sender") = ""
                    'Response.Redirect("wfInstalledCompList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    FindNow()
                    'Response.Redirect("wfInstalledCompList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    FindNow()
                    'Response.Redirect("wfInstalledCompList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfInstalledCompList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            '   DataFieldBind()
        End If
    End Sub
    Private Sub SetCaption()
        If RecordsToShowForRemCompList < mRemList.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
            lblRemovedComponents.Text = "List of Removed components as per selected criteria : " & RecordsToShowForRemCompList.ToString & " of " & mRemList.Count & " Record(s) shown."
        Else
            lblRemovedComponents.Text = "List of Removed components as per selected criteria : " & mRemList.Count & " Record(s) found."
        End If
        If RecordsToShowForInstCompList < mInstList.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
            lblInstalledComponents.Text = "List of Installed components as per selected criteria : " & RecordsToShowForInstCompList.ToString & " of " & mInstList.Count & " Record(s) shown."
        Else
            lblInstalledComponents.Text = "List of Installed components as per selected criteria : " & mInstList.Count & " Record(s) found."
        End If
        'lblRemovedComponents.Text = "List of Removed components as per selected criteria : " & mRemovedCompList.Count & " Record(s) found."
        'lblInstalledComponents.Text = "List of Installed components as per selected criteria : " & mInstalledCompList.Count & " Record(s) found."
    End Sub
    Private Sub ControlVisibility()
        btnPrintInstalled.Enabled = mInstList.Count > 0
        btnPrintRemoved.Enabled = mRemList.Count > 0
        EnableLinks()
        SetEnable()
        'Added By Vikrant On 24-Dec-2020 For ALL27072020
        'cmbAircraft.Visible = IIf(mSpareAssemblyComponent = 0, True, False)
        'lblAircraft.Visible = IIf(mSpareAssemblyComponent = 0, True, False)
        placeHolder1.Visible = IIf(mSpareAssemblyComponent = 0, True, False)
        'End
    End Sub
    'Added By Vikrant On 01-Dec-2014
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        If mIsAttachemntAdded = True Then
            mFileAttach = FileAttach.GetAttachment(ID, 2) 'Sort = 2 - Removal
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub ViewImage(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        GetAttachment(ID, mIsAttachemntAdded) 'Sort = 2 - Removal
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
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    Private Sub SetEnable()
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        For j As Integer = 0 To dgRemovedList.Rows.Count - 1
            'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
            'Disable Comply,Edit and Delete links if Aircraft is ReadOnly
            If IsReadOnly = True And mSpareAssemblyComponent = 0 Then
                dgRemovedList.Rows(j).Cells(9).Enabled = False
                dgRemovedList.Rows(j).Cells(10).Enabled = False
            Else
                dgRemovedList.Rows(j).Cells(9).Enabled = True
                dgRemovedList.Rows(j).Cells(10).Enabled = True
            End If
            '*************************
        Next

        For j As Integer = 0 To dgInstalledList.Rows.Count - 1
            'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
            'Disable Comply,Edit and Delete links if Aircraft is ReadOnly
            If IsReadOnly = True And mSpareAssemblyComponent = 0 Then
                dgInstalledList.Rows(j).Cells(10).Enabled = False
            Else
                dgInstalledList.Rows(j).Cells(10).Enabled = True
            End If
            '*************************
        Next

        If IsReadOnly = True And mSpareAssemblyComponent = 0 Then 'mSpareAssemblyComponent Added by Vikrant on 06-Jan-2021 for ALL27072020
            lblReadOnly.Visible = True
        Else
            lblReadOnly.Visible = False
        End If
        upnlRemovedCompList.Update()
        UpnlInstalledCompList.Update()

    End Sub
    Private Sub SetGrid()
        Dim B As Boolean
        For j As Integer = 0 To dgRemovedList.Rows.Count - 1
            B = CType(Me.dgRemovedList.Rows.Item(j).Cells(14).Text, Boolean)
            If B = False Then
                dgRemovedList.Rows.Item(j).Cells(13).Enabled = False
            End If
        Next
        SetEnable()
    End Sub
    'End
    Private Sub FindNow()
        RecordsToShowForInstCompList = dgInstalledList.PageSize
        RecordsToShowForRemCompList = dgRemovedList.PageSize
        Session("RecordsToShowForInstCompList") = RecordsToShowForInstCompList
        Session("RecordsToShowForRemCompList") = RecordsToShowForRemCompList

        Session("RemoveDate") = calRemovalDate.Text
        Session("AircraftId") = cmbAircraft.SelectedValue
        Session("AssemblyId") = cmbAssembly.SelectedValue
        'Added By Rahul on 29-Apr-2009
        Session("PartNO") = Trim(txtPart.Text)
        Session("SerialNo") = Trim(txtSerialNo.Text)
        '============================================
        'New Added
        If mSpareAssemblyComponent = 0 Then '
            mCompStatusList = CompStatusList.GetCompStatusList(CurrentDate:=calRemovalDate.Text, AssemblyID:=New Guid(cmbAssembly.SelectedValue), PartName:=Trim(txtPart.Text), CompSerialNo:=Trim(txtSerialNo.Text), MachineID:=cmbAircraft.SelectedValue.ToString, IsCompInstalled:=True, IsCompRemoved:=True, IsCompPeriodsRequired:=False)
        Else 'condition added by Vikrant For ALL27072020
            mCompStatusList = CompStatusList.GetCompStatusList(CurrentDate:=calRemovalDate.Text, AssemblyID:=New Guid(cmbAssembly.SelectedValue), PartName:=Trim(txtPart.Text), CompSerialNo:=Trim(txtSerialNo.Text), MachineID:=Guid.Empty.ToString, IsCompInstalled:=True, IsCompRemoved:=True, IsCompPeriodsRequired:=False, IsSpareAssemblyInstalledRemovedCompRequired:=True, IsSpareComponentAlsoRequired:=True)
        End If

        Session("mCompStatusList") = mCompStatusList
        mRemList = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Removed"
                                        Select StatusInfo).ToList()
        'Commented and Added By Vikrant On 06-Jan-2021 For ALL27072020
        'mInstList = (From StatusInfo As CompStatusInfo In mCompStatusList
        '                                Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
        '                                Select StatusInfo).ToList()
        If mSpareAssemblyComponent = 0 Then
            mInstList = (From StatusInfo As CompStatusInfo In mCompStatusList
                                       Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                       Select StatusInfo).ToList()
        Else
            mInstList = (From StatusInfo As CompStatusInfo In mCompStatusList
                                                   Where StatusInfo.IsInstalledRemoved = "Installed"
                                                   Select StatusInfo).ToList()
        End If
        'End
        Session("mRemList") = mRemList
        Session("mInstList") = mInstList
        'End
        'mRemovedCompList = tmpRemovedCompList.GetRemovedCompList(calRemovalDate.Text, cmbAircraft.SelectedValue, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue))

        'mInstalledCompList = tmpInstalledCompList.GetInstalledCompList(calRemovalDate.Text, cmbAircraft.SelectedValue, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue))

        'Added By Vikrant For Showing First 5 records
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            dgRemovedList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Removed"
                                        Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)

            'Commented and Added By Vikrant On 06-Jan-2021 For ALL27072020
            'dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
            '                            Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
            '                            Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
            If mSpareAssemblyComponent = 0 Then
                dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                        Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
            Else
                dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed"
                                        Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
            End If
            'End
        Else
            dgRemovedList.DataSource = mRemList
            dgInstalledList.DataSource = mInstList
        End If
        'End
        'Session("mInstalledCompList") = mInstalledCompList
        'Session("mRemovedCompList") = mRemovedCompList
        dgInstalledList.DataBind()
        dgRemovedList.DataBind()
    End Sub
    Private Sub RemoveRecord(ByVal mCompStatusInfo As CompStatusInfo)
        Dim checkRemovedAssemblyList As tmpRemovedAssemblyList = tmpRemovedAssemblyList.GetRemovedAssemblyList(Today.ToString, cmbAircraft.SelectedValue.ToString, Trim(txtPart.Text), Trim(txtSerialNo.Text))
        Session("checkRemovedAssemblyList") = checkRemovedAssemblyList
        If checkRemovedAssemblyList.Contains(mCompStatusInfo.ID) Then
            MSGBoxCtrl.show(MSGBox.Message_title.ComponentIsRemoved, MSGBox.Message_text.ComponentIsRemoved, "Selected " & mCompStatusInfo.CompInfo & ", Already removed, cannot remove again", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.NewRemovalCompStatus(mCompStatusInfo.ID, calRemovalDate.Text, mCompStatusInfo.AssemblyStatusID, Guid.Empty.ToString)
        Session("From") = 1 'NewRemove
        Session("mCompStatus") = mCompStatus
        Dim mPrevCompStatus As CompStatus = CompStatus.GetCompStatus(mCompStatusInfo.ID, mCompStatusInfo.AssemblyStatusID, mCompStatusInfo.InstalledOnFormatted.ToString)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompStatusInfo.AssemblyStatusID)
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mPrevCompStatus") = mPrevCompStatus

        Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompStatus.ID, Sort:=2) 'Sort = 2 : Removal
        Session("mFileAttach") = mFileAttach
        '' NewMachineMaintenance() 'Added by Saylee on 8th-Oct-2009

        'Dim str As String
        'str = "<script language='javascript'>openledgersame('wfRemoveComp.aspx?BackPage=Index.aspx');</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfRemoveComp_Ajax.aspx?BackPage=Index.aspx');", True)
        'Changed By Utkarsh On 26-Jul-2011 For All19072011
        MaintDetail = "Reg No. : " + mCompStatusInfo.MachineInfo & " Assembly Info : " & mCompStatusInfo.AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mCompStatusInfo.CompInfo.Replace(Environment.NewLine, " ")
        MarkLog(Util.Action.Remove, "ComponentRemoval", MaintDetail, Util.ErrorType.NoError, mCompStatus.ID, EventLogID)
        'End
    End Sub
    Private Sub EditRecord(ByVal mCompStatusInfo As CompStatusInfo)

        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.GetRemovalCompStatusFromEntry(mCompStatusInfo.ID, mCompStatusInfo.AssemblyStatusID, mCompStatusInfo.RemovedOnFormatted.ToString)
        Session("From") = 2 'EditRemove
        Session("mCompStatus") = mCompStatus
        Dim mPrevCompStatus As CompStatus = CompStatus.GetCompStatus(mCompStatusInfo.ID, mCompStatusInfo.AssemblyStatusID, mCompStatusInfo.RemovedOnFormatted.ToString)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompStatusInfo.AssemblyStatusID)
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mPrevCompStatus") = mPrevCompStatus

        'If mCompStatus.IsAttachmentAdded Then
        '    Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mRemovedCompList(Index).CompStatusID, 2) 'Sort = 2 : Removal
        '    Session("mFileAttach") = mFileAttach
        'Else
        '    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mRemovedCompList(Index).CompStatusID, Sort:=2)
        '    Session("mFileAttach") = mFileAttach
        'End If

        'Commented By Utkarsh On 26-Jul-2011 For All19072011

        'Dim RemovalComp As String 'Added Code  Jan,24,2007
        'RemovalComp = "ATAChapter -> " + mCompStatus.ATAChapter + " Part -> " + mCompStatus.PartNameSerialNo 'Added Code  Jan,24,2007

        'End

        '' GetMachineMaintenance()  'Added by Saylee on 8th-Oct-2009

        'Changed By Utkarsh On 26-Jul-2011 For All19072011
        MaintDetail = "Reg No. : " + mCompStatusInfo.MachineInfo & " Assembly Info : " & mCompStatusInfo.AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mCompStatusInfo.CompInfo.Replace(Environment.NewLine, " ") + " Removed On : " + mCompStatusInfo.RemovedOnFormatted.ToString
        MarkLog(Util.Action.Edit, "ComponentRemoval", MaintDetail, Util.ErrorType.NoError, mCompStatus.ID, EventLogID)
        'End

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfRemoveComp_Ajax.aspx?BackPage=Index.aspx');", True)
    End Sub
    Private Sub EnableLinks()

        If Not mInstList Is Nothing Then
            If RecordsToShowForInstCompList < mInstList.Count Then
                lnkInstCompShowAllRecords.Enabled = True
                lnkInstCompShowAllRecordsTop.Enabled = True
            Else
                lnkInstCompShowAllRecords.Enabled = False
                lnkInstCompShowAllRecordsTop.Enabled = False
            End If
        End If
        If Not mRemList Is Nothing Then
            If RecordsToShowForRemCompList < mRemList.Count Then
                lnkRemCompShowAllRecords.Enabled = True
                lnkRemCompShowAllRecordsTop.Enabled = True
            Else
                lnkRemCompShowAllRecords.Enabled = False
                lnkRemCompShowAllRecordsTop.Enabled = False
            End If
        End If
    End Sub
    Private Sub RevertRecord(ByVal RevertRemovalID As Guid)
        'mRemovedCompList.CurrentIndex = index
        'Session("mRemovedCompList") = mRemovedCompList
        Session("RevertRemovalID") = RevertRemovalID

        MSGBoxCtrl.show("Revert Confirmation!", "Confirm Revert Component Removal <BR> <BR> Do you want to Revert the current Removed Component?", "", MsgBoxStyle.YesNo, "RevertRemoval")
    End Sub
    Private Sub HistoryRecord(ByVal mCompStatusInfo As CompStatusInfo) 'Added by Saylee on 20-Oct-2009
        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.GetRemovalCompStatusFromEntry(mCompStatusInfo.ID, mCompStatusInfo.AssemblyStatusID, mCompStatusInfo.RemovedOnFormatted.ToString)
        Session("From") = 2 'EditRemove
        Session("mCompStatus") = mCompStatus

        Dim mPrevCompStatus As CompStatus
        If mSpareAssemblyComponent = 0 Then
            mPrevCompStatus = CompStatus.GetCompStatus(mCompStatusInfo.ID, mCompStatusInfo.AssemblyStatusID, mCompStatusInfo.RemovedOnFormatted.ToString)
        Else
            mPrevCompStatus = CompStatus.GetSpareCompStatus(mCompStatusInfo.ID, True)
        End If

        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompStatusInfo.AssemblyStatusID)
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mPrevCompStatus") = mPrevCompStatus

        'Commented By Utkarsh On 26-Jul-2011 For All19072011

        'Dim RemovalComp As String
        'RemovalComp = "ATAChapter -> " + mCompStatus.ATAChapter + " Part -> " + mCompStatus.PartNameSerialNo 'Added Code  Jan,24,2007

        'End

        'Added by Saylee on 20-Oct-2009
        Dim mUpdateHistoryCompStausList As UpdateHistoryCompStatusList
        Session("PartName") = mCompStatus.PartName
        Session("CompSerialNo") = mCompStatus.SerialNo
        mUpdateHistoryCompStausList = UpdateHistoryCompStatusList.GetRemovedCompList(calRemovalDate.Text, mAssemblyStatus.ID, mCompStatus.CompID)
        Session("mUpdateHistoryCompStausList") = mUpdateHistoryCompStausList
        '========================================

        'Changed By Utkarsh On 26-Jul-2011 For All19072011
        MaintDetail = "Reg No. : " + mCompStatusList(mCompStatus.ID).MachineInfo & " Assembly Info : " & mCompStatusList(mCompStatus.ID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mCompStatusList(mCompStatus.ID).CompInfo.Replace(Environment.NewLine, " ")
        MarkLog(Util.Action.View, "ComponentRemoval", MaintDetail, Util.ErrorType.NoError, mCompStatus.ID, EventLogID)
        'End

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenRemHistoryWindow", "OpenRemHistoryWindow()", True)
    End Sub

#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        Dim TodayDate As String = Today.Date.ToString(AppSettings("DateFormat").ToString)
        If IsNothing(Session("RemoveDate")) Then
            calRemovalDate.Text = TodayDate
            RemoveDate = TodayDate 'Added By Rahul on 29-Apr-2009
        Else
            calRemovalDate.Text = RemoveDate
        End If
        Session("RemoveDate") = calRemovalDate.Text

        'mMachineNameValueList = tmpMachineList.GetMachineList(, , , , , "<SELECT>")

        ''mMachineNameValueList = MachineList.GetMachineListMonitoringStatus(Today.Date.ToShortDateString, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "<SELECT>")
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToShortDateString, SkipIsForInventoryAircarft:=True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()

        If IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString Then cmbAircraft.SelectedIndex = 0 Else cmbAircraft.SelectedValue = AircraftId
        AircraftId = cmbAircraft.SelectedValue

        ''mAssemblylist = AssemblyList.GetAssemblyList(0, AircraftId, calRemovalDate.Value.ToString, "<All>")
        If mSpareAssemblyComponent = 0 Then
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, AircraftId, calRemovalDate.Text, "(All)", True)
        Else
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, AircraftId, calRemovalDate.Text, "(All)", True, IsForSpareAssembly:=IIf(mSpareAssemblyComponent = 0, False, True))
        End If

        cmbAssembly.DataSource = mAssemblylist
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mAssemblylist") = mAssemblylist
        If IsNothing(AssemblyId) Then AssemblyId = Guid.Empty.ToString Else AssemblyId = AssemblyId
        '''mInstalledCompList = tmpInstalledCompList.GetInstalledCompList(RemoveDate, AircraftId, PartNo, SerialNo, New Guid(AssemblyId))
        '''mRemovedCompList = tmpRemovedCompList.GetRemovedCompList(RemoveDate, AircraftId, PartNo, SerialNo, New Guid(AssemblyId))
        'Added By Vikrant For Showing First 5 records
        '''If AppSettings("IsShowAllRecordsVisible") = "True" Then
        '''    Dim InstList = (From StatusInfo As tmpInstalledCompList.tmpInstalledCompInfo In mInstalledCompList
        '''                                               Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
        '''    Dim RemList = (From StatusInfo As tmpRemovedCompList.tmpRemovedCompInfo In mRemovedCompList
        '''                                              Select StatusInfo).ToList.Take(RecordsToShowForRemCompList)
        '''    dgRemovedList.DataSource = RemList
        '''    dgInstalledList.DataSource = InstList
        '''Else
        '''    dgRemovedList.DataSource = mRemovedCompList
        '''    dgInstalledList.DataSource = mInstalledCompList
        '''End If
        'End
        '''Session("mInstalledCompList") = mInstalledCompList
        '''Session("mRemovedCompList") = mRemovedCompList
        If mSpareAssemblyComponent = 0 Then  'If Condition Added by Vikrant on 06-Jan-2021 for ALL27072020
            'DO Nothing
        Else
            dgRemovedList.Columns(1).Visible = False
            dgInstalledList.Columns(1).Visible = False
        End If
        DataBind()

        'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraft.SelectedValue)).IsReadOnly 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft

        Session("IsReadOnly") = IsReadOnly
        '***********************************

        If IsNothing(AssemblyId) Or AssemblyId = Guid.Empty.ToString Then cmbAssembly.SelectedIndex = 0 Else cmbAssembly.SelectedValue = AssemblyId

        RemoveDate = calRemovalDate.Text

        AssemblyId = cmbAssembly.SelectedValue

        'Added By Rahul on 29-Apr-2009
        txtPart.Text = PartNo
        txtSerialNo.Text = SerialNo
        '===========================
        Session("RemoveDate") = RemoveDate
        Session("AircraftId") = AircraftId
        Session("AssemblyId") = AssemblyId
        'Added by Vikrant on 06-Jan-2021 for ALL27072020
        If (mSpareAssemblyComponent = 1) Then
            Dim da As New CSLA.Data.ObjectAdapter
            Dim ds As New DataSet()
            da.Fill(ds, mAssemblylist)
            Dim dv As DataView = ds.Tables(0).DefaultView
            dv.RowFilter = "IsSpareAssembly='True'"
            For Each dr As DataRowView In dv
                For Each item As ListItem In cmbAssembly.Items
                    If dr("ID").ToString() = item.Value.ToString() Then
                        item.Attributes.Add("style", "background-color:#ffbf00;color:black;font-weight:bold;")
                    End If
                Next
            Next
        End If
        '----------
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        REM:put here the code to initialize the page
        ClearAll()
        GetSession()
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And MSGBoxCtrl.Sender = "" Then
            SetFocus(cmbAircraft)
            'Added by Vikrant on 06-Jan-2021 for ALL27072020
            mSpareAssemblyComponent = Request.QueryString("SpareAssemblyComponent")
            Session("mSpareAssemblyComponent") = mSpareAssemblyComponent
            'End
            Session("MiddleFrame") = "wfInstalledCompList_Ajax.aspx?SpareAssemblyComponent=" & mSpareAssemblyComponent
            RecordsToShowForInstCompList = dgInstalledList.PageSize
            RecordsToShowForRemCompList = dgRemovedList.PageSize
            Session("RecordsToShowForInstCompList") = RecordsToShowForInstCompList
            Session("RecordsToShowForRemCompList") = RecordsToShowForRemCompList
            DataFieldBind()
            FindNow()
            ControlVisibility()
            SetCaption()
            'SetGrid()
        End If

    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If IsValid Then
            Session("RemoveDate") = calRemovalDate.Text
            Session("AircraftId") = cmbAircraft.SelectedValue
            'Added By Rahul on 29-Apr-2009
            Session("Part") = Trim(txtPart.Text)
            Session("SerialNo") = Trim(txtSerialNo.Text)

            FindNow()
            SetCaption()
            ControlVisibility()
            'SetGrid()

            UpnlInstalledCompList.Update()
            upnlRemovedCompList.Update()
            upnlInstalledCompHeader.Update()
            upnlRemovedCompHeader.Update()
            upnlPrintInstalledCompList.Update()
            upnlButtons.Update()
        End If
    End Sub
    Private Sub dgInstalledList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInstalledList.RowCommand
        Dim mCompStatusInfo As CompStatusInfo
        Select Case e.CommandName
            Case "RemoveRec"
                'Dim Index As Integer = CInt(e.CommandArgument) + dgInstalledList.PageSize * dgInstalledList.PageIndex
                mCompStatusInfo = mCompStatusList(New Guid(e.CommandArgument.ToString))
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("ComponentRemovalNew")) Then
                    'Changed by Vikrant on 26-July-2011
                    MaintDetail = "Reg No. : " + mCompStatusInfo.MachineInfo & " Assembly Info : " & mCompStatusInfo.AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mCompStatusInfo.CompInfo.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.Remove, "ComponentRemoval", User.Identity.Name & " is not Authorized User to remove " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                RemoveRecord(mCompStatusInfo)
            Case "ShowVal"
                Dim mtmpInstalledCompList As tmpInstalledCompList
                mCompStatusInfo = mCompStatusList(New Guid(e.CommandArgument.ToString))
                If mSpareAssemblyComponent = 1 Then 'mSpareAssemblyComponent = 1 Added by Vikrant on 06-Jan-2021 for ALL27072020
                    mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(calRemovalDate.Text, Guid.Empty.ToString, _
                                                mCompStatusInfo.PartName, mCompStatusInfo.CompSerialNo, New Guid(cmbAssembly.SelectedValue), IsSpareAssembly:=True)
                    'End
                Else
                    mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(calRemovalDate.Text, cmbAircraft.SelectedValue, _
                                                    mCompStatusInfo.PartName, mCompStatusInfo.CompSerialNo, New Guid(cmbAssembly.SelectedValue))
                End If

                'Dim JsonString As String = New JavaScriptSerializer().Serialize(mInstalledCompList)
                'Dim CompInfo As String = mCompStatusInfo.PartName + " - " + mCompStatusInfo.CompSerialNo
                'JsonString = JsonString.Replace("\r", "").Replace("\n", "")

                'mCompStatusList(New Guid(e.CommandArgument.ToString)).TSNValues = mInstalledCompList(0).TSNFormatted
                'mCompStatusList(New Guid(e.CommandArgument.ToString)).TSOValues = mInstalledCompList(0).TSOFormatted
                'mCompStatusList(New Guid(e.CommandArgument.ToString)).InstalledOnValues = mInstalledCompList(0).TextFormatted.ToString
                'Session("mCompStatusList") = mCompStatusList
                If AppSettings("IsShowAllRecordsVisible") = "True" Then
                    'Commented and Added By Vikrant On 06-Jan-2021 For ALL27072020
                    'dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                    '                            Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                    '                            Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
                    If mSpareAssemblyComponent = 0 Then
                        dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                                Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                                Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
                    Else
                        dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                                    Where StatusInfo.IsInstalledRemoved = "Installed"
                                                    Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
                    End If
                    
                    'End
                Else

                    'Commented and Added By Vikrant On 06-Jan-2021 For ALL27072020
                    'dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                    '                            Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                    '                            Select StatusInfo).ToList()
                    If mSpareAssemblyComponent = 0 Then
                        dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                                Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                                Select StatusInfo).ToList()
                    Else
                        dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                                Where StatusInfo.IsInstalledRemoved = "Installed"
                                                Select StatusInfo).ToList()
                    End If
                    'End
                End If

                Dim InstLabel, TSOLabel, TSNLabel As Label
                Dim Instlnkbtn, TSOlnkbtn, TSNlnkbtn As LinkButton
                Dim currentRow As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)

                InstLabel = CType(currentRow.FindControl("lblInstValues"), Label)
                TSOLabel = CType(currentRow.FindControl("lblTSOValues"), Label)
                TSNLabel = CType(currentRow.FindControl("lblTSNValues"), Label)

                Instlnkbtn = CType(currentRow.FindControl("lnkInstValue"), LinkButton)
                TSOlnkbtn = CType(currentRow.FindControl("lnkTSOValue"), LinkButton)
                TSNlnkbtn = CType(currentRow.FindControl("lnkTSNValue"), LinkButton)

                Instlnkbtn.Visible = False
                TSNlnkbtn.Visible = False
                TSOlnkbtn.Visible = False

                If mtmpInstalledCompList.Count > 0 Then
                    If mtmpInstalledCompList.Contains(mCompStatusInfo.ID) Then
                        TSNLabel.Text = mtmpInstalledCompList(mCompStatusInfo.ID).TSNFormatted
                        TSOLabel.Text = mtmpInstalledCompList(mCompStatusInfo.ID).TSOFormatted
                        InstLabel.Text = mtmpInstalledCompList(mCompStatusInfo.ID).TextFormatted.ToString
                    End If
                Else
                    TSNLabel.Text = ""
                    TSOLabel.Text = ""
                    InstLabel.Text = ""
                End If

                'dgInstalledList.DataBind()
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowValues", "ShowValues('" & JsonString.ToString & "','" & CompInfo & "');", True)
        End Select
    End Sub
    Private Sub dgRemovedList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRemovedList.RowCommand
        Dim mCompStatusInfo As CompStatusInfo
        Select Case e.CommandName
            Case "EditRec"
                'GridBind()
                mCompStatusInfo = mCompStatusList(New Guid(e.CommandArgument.ToString))
                'Dim Index As Integer = CInt(e.CommandArgument) + dgRemovedList.PageSize * dgRemovedList.PageIndex
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("ComponentRemovalView") And Not User.IsInRole("ComponentRemovalEdit")) Then
                    'Changed By Utkarsh On 26-Jul-2011 For All19072011
                    MaintDetail = "Reg No. : " + mCompStatusInfo.MachineInfo & " Assembly Info : " & mCompStatusInfo.AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mCompStatusInfo.CompInfo.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.Edit, "ComponentRemoval", User.Identity.Name & " is not Authorized User to edit " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                EditRecord(mCompStatusInfo)
            Case "RevertRemoval"
                mCompStatusInfo = mCompStatusList(New Guid(e.CommandArgument.ToString))
                'Dim Index As Integer = CInt(e.CommandArgument) + dgRemovedList.PageSize * dgRemovedList.PageIndex
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If Not User.IsInRole("ComponentRemovalDelete") Then
                    MaintDetail = "Reg No. : " + mCompStatusInfo.MachineInfo & " Assembly Info : " & mCompStatusInfo.AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mCompStatusInfo.CompInfo.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.RevertRemoval, "ComponentRemoval", User.Identity.Name & " is not Authorized User to revert removal " & MaintDetail, Util.ErrorType.NoError, mCompStatusInfo.ID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                RevertRecord(mID)
            Case "History"
                mCompStatusInfo = mCompStatusList(New Guid(e.CommandArgument.ToString))
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("ComponentRemovalView") And Not User.IsInRole("ComponentRemovalEdit")) Then
                    'MarkLog(Util.Action.Edit, "CompRemoval", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfInstalledCompList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                    'msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                HistoryRecord(mCompStatusInfo)
            Case "ViewRec"
                mCompStatusInfo = mCompStatusList(New Guid(e.CommandArgument.ToString))
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("ComponentRemovalView") And Not User.IsInRole("ComponentRemovalEdit")) Then
                    MaintDetail = "Reg No. : " + mCompStatusInfo.MachineInfo & " Assembly Info : " & mCompStatusInfo.AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mCompStatusInfo.CompInfo.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.View, "ComponentRemoval", User.Identity.Name & " is not Authorized User to view attachment " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                End If

                Dim mIsAttachemntAdded As Boolean = mCompStatusInfo.IsRemAttachmentAdded  'e.Item.Cells(14).Text
                ViewImage(mID, mIsAttachemntAdded)
            Case "ShowVal"
                Dim mtmpRemovedCompList As tmpRemovedCompList
                mCompStatusInfo = mCompStatusList(New Guid(e.CommandArgument.ToString))
                mtmpRemovedCompList = tmpRemovedCompList.GetRemovedCompList(calRemovalDate.Text, cmbAircraft.SelectedValue, mCompStatusInfo.PartName, mCompStatusInfo.CompSerialNo, New Guid(cmbAssembly.SelectedValue))

                'Dim JsonString As String = New JavaScriptSerializer().Serialize(mInstalledCompList)
                'Dim CompInfo As String = mCompStatusInfo.PartName + " - " + mCompStatusInfo.CompSerialNo
                'JsonString = JsonString.Replace("\r", "").Replace("\n", "")

                'mCompStatusList(New Guid(e.CommandArgument.ToString)).TSNValues = mInstalledCompList(0).TSNFormatted
                'mCompStatusList(New Guid(e.CommandArgument.ToString)).TSOValues = mInstalledCompList(0).TSOFormatted
                'mCompStatusList(New Guid(e.CommandArgument.ToString)).InstalledOnValues = mInstalledCompList(0).TextFormatted.ToString
                'Session("mCompStatusList") = mCompStatusList
                If AppSettings("IsShowAllRecordsVisible") = "True" Then
                    dgRemovedList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                                Where StatusInfo.IsInstalledRemoved = "Removed"
                                                Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
                Else
                    dgRemovedList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                                Where StatusInfo.IsInstalledRemoved = "Removed"
                                                Select StatusInfo).ToList()
                End If

                Dim RemLabel, TSOLabel As Label
                Dim Remlnkbtn, TSOlnkbtn As LinkButton
                Dim currentRow As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)

                RemLabel = CType(currentRow.FindControl("lblRemValues"), Label)
                TSOLabel = CType(currentRow.FindControl("lblRemTSOValues"), Label)


                Remlnkbtn = CType(currentRow.FindControl("lnkRemValue"), LinkButton)
                TSOlnkbtn = CType(currentRow.FindControl("lnkRemTSOValue"), LinkButton)

                Remlnkbtn.Visible = False
                TSOlnkbtn.Visible = False

                If mtmpRemovedCompList.Count > 0 Then
                    If mtmpRemovedCompList.Contains(mCompStatusInfo.ID) Then
                        TSOLabel.Text = mtmpRemovedCompList(mCompStatusInfo.ID).TSOFormatted
                        RemLabel.Text = mtmpRemovedCompList(mCompStatusInfo.ID).TextFormatted.ToString
                    End If

                Else
                    TSOLabel.Text = ""
                    RemLabel.Text = ""
                End If

        End Select
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        txtPart.Text = ""
        txtSerialNo.Text = ""
        'mAssemblylist = AssemblyList.GetAssemblyList(0, cmbAircraft.SelectedValue, calRemovalDate.Value.ToString, "<All>")
        If mSpareAssemblyComponent = 0 Then
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, calRemovalDate.Text, "(All)", True)
        Else
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, calRemovalDate.Text, "(All)", True, IsForSpareAssembly:=IIf(mSpareAssemblyComponent = 0, False, True))
        End If

        Session("mAssemblyList") = mAssemblylist
        cmbAssembly.DataSource = mAssemblylist
        cmbAssembly.DataBind()
        SetFocus(cmbAircraft)
        'FindNow called
        Session("RemoveDate") = calRemovalDate.Text
        Session("AircraftId") = cmbAircraft.SelectedValue
        'Added By Rahul on 29-Apr-2009
        Session("Part") = Trim(txtPart.Text)
        Session("SerialNo") = Trim(txtSerialNo.Text)

        'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraft.SelectedValue)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnly") = IsReadOnly

        If IsReadOnly = True And mSpareAssemblyComponent = 0 Then 'mSpareAssemblyComponent Added by Vikrant on 06-Jan-2021 for ALL27072020
            lblReadOnly.Visible = True
        Else
            lblReadOnly.Visible = False
        End If
        '*************************************************

        'FindNow()
        'SetCaption()
        'ControlVisibility()
        ''SetGrid()
        'UpnlInstalledCompList.Update()
        'upnlRemovedCompList.Update()
        'upnlInstalledCompHeader.Update()
        'upnlRemovedCompHeader.Update()
        'upnlPrintInstalledCompList.Update()
        'upnlButtons.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Session.Remove("AircraftId")
        Session.Remove("RemoveDate")
        Response.Redirect("Dashboard.aspx?BackPage=")
    End Sub
    Private Sub dgInstalledList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgInstalledList.Sorting
        'mInstalledCompList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        'Session("mInstalledCompList") = mInstalledCompList
        'Commented and Added By Vikrant On 06-Jan-2021 For ALL27072020
        'mInstList = (From StatusInfo As CompStatusInfo In mCompStatusList
        '                                Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
        '                                Order By e.SortExpression
        '                                Select StatusInfo).ToList()
        If mSpareAssemblyComponent = 0 Then
            mInstList = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                        Order By e.SortExpression
                                        Select StatusInfo).ToList()
        Else
            mInstList = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed"
                                        Order By e.SortExpression
                                        Select StatusInfo).ToList()
        End If
        'End
        Session("mInstList") = mInstList
        'Added By Vikrant For Showing First 5 records
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            'Commented and Added By Vikrant On 06-Jan-2021 For ALL27072020
            'dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
            '                            Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
            '                            Order By e.SortExpression
            '                            Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
            If mSpareAssemblyComponent = 0 Then 'If condition added by Vikrant For ALL27072020
                dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                        Order By e.SortExpression
                                        Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
            Else
                dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed"
                                        Order By e.SortExpression
                                        Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
            End If
            
            'End
        Else
            dgInstalledList.DataSource = mInstList
        End If
        dgInstalledList.DataBind()
    End Sub
    Private Sub dgRemovedList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRemovedList.Sorting
        'mRemovedCompList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        'Session("mRemovedCompList") = mRemovedCompList
        mRemList = (From StatusInfo As CompStatusInfo In mCompStatusList
                           Where StatusInfo.IsInstalledRemoved = "Removed"
                           Order By e.SortExpression
                           Select StatusInfo).ToList()
        Session("mRemList") = mRemList
        'Added By Vikrant For Showing First 5 records
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            dgRemovedList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                           Where StatusInfo.IsInstalledRemoved = "Removed"
                           Order By e.SortExpression
                           Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
        Else
            dgRemovedList.DataSource = mRemList
        End If
        dgRemovedList.DataBind()
        'SetGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub calRemovalDate_TextChanged(sender As Object, e As System.EventArgs) Handles calRemovalDate.TextChanged
        txtPart.Text = ""
        txtSerialNo.Text = ""
        If mSpareAssemblyComponent = 0 Then 'If condition added by Vikrant For ALL27072020
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, calRemovalDate.Text, "(All)", True)
        Else
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, calRemovalDate.Text, "(All)", True, IsForSpareAssembly:=IIf(mSpareAssemblyComponent = 0, False, True))
        End If

        Session("mAssemblyList") = mAssemblylist
        cmbAssembly.DataSource = mAssemblylist
        cmbAssembly.DataBind()
        SetFocus(cmbAircraft)
        'FindNow called
        Session("RemoveDate") = calRemovalDate.Text
        Session("AircraftId") = cmbAircraft.SelectedValue
        'Added By Rahul on 29-Apr-2009
        Session("Part") = Trim(txtPart.Text)
        Session("SerialNo") = Trim(txtSerialNo.Text)

        'FindNow()
        'SetCaption()
        'ControlVisibility()
        ''SetGrid()
        'UpnlInstalledCompList.Update()
        'upnlRemovedCompList.Update()
        'upnlInstalledCompHeader.Update()
        'upnlRemovedCompHeader.Update()
        'upnlPrintInstalledCompList.Update()
        'upnlButtons.Update()
        'upnlSearchCriteria.Update()
    End Sub
    Private Sub cmbAssembly_SelectedIndexChanged(sender As Object, e As System.EventArgs)
        txtPart.Text = ""
        txtSerialNo.Text = ""

        Session("RemoveDate") = calRemovalDate.Text
        Session("AircraftId") = cmbAircraft.SelectedValue
        'Added By Rahul on 29-Apr-2009
        Session("Part") = Trim(txtPart.Text)
        Session("SerialNo") = Trim(txtSerialNo.Text)

        FindNow()
        SetCaption()
        ControlVisibility()
        'SetGrid()
        UpnlInstalledCompList.Update()
        upnlRemovedCompList.Update()
        upnlInstalledCompHeader.Update()
        upnlRemovedCompHeader.Update()
        upnlPrintInstalledCompList.Update()
        upnlButtons.Update()
        upnlSearchCriteria.Update()
    End Sub
    Private Sub lnkInstCompShowAllRecords_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkInstCompShowAllRecords.Click, lnkInstCompShowAllRecordsTop.Click
        RecordsToShowForInstCompList = mInstList.Count
        Session("RecordsToShowForInstCompList") = RecordsToShowForInstCompList
        'Dim list = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        dgInstalledList.DataSource = mInstList
        dgInstalledList.DataBind()
        'SetGrid()
        ControlVisibility()
        SetCaption()
        UpnlInstalledCompList.Update()
        upnlInstalledCompHeader.Update()
        upnlPrintInstalledCompList.Update()
        upnlButtons.Update()
    End Sub
    Private Sub lnkRemCompShowAllRecords_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRemCompShowAllRecords.Click, lnkRemCompShowAllRecordsTop.Click
        RecordsToShowForRemCompList = mRemList.Count
        Session("RecordsToShowForRemCompList") = RecordsToShowForRemCompList
        'Dim list = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        dgRemovedList.DataSource = mRemList
        dgRemovedList.DataBind()
        'SetGrid()
        ControlVisibility()
        SetCaption()
        upnlRemovedCompList.Update()
        upnlRemovedCompHeader.Update()
        upnlButtons.Update()
    End Sub
    Protected Sub dgInstalledList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgInstalledList.Columns(i).HeaderText 'dgInstalledList.HeaderRow.Cells(i).Text 'DirectCast(DirectCast(dgInstalledList.HeaderRow.Cells(i), System.Web.UI.WebControls.DataControlFieldHeaderCell).ContainingField, System.Web.UI.WebControls.BoundField).HeaderText
                'dgInstalledList.HeaderRow.Cells(i).Text
            Next
        End If
    End Sub
    Protected Sub dgRemovedList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgRemovedList.Columns(i).HeaderText 'dgInstalledList.HeaderRow.Cells(i).Text 'DirectCast(DirectCast(dgInstalledList.HeaderRow.Cells(i), System.Web.UI.WebControls.DataControlFieldHeaderCell).ContainingField, System.Web.UI.WebControls.BoundField).HeaderText
                'dgInstalledList.HeaderRow.Cells(i).Text
            Next
        End If
    End Sub
#End Region

#Region " Report "
    'Created By :- Jyoti
#Region " Report Variable "
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass

    Private SearchStr1 As String = ""
    Private SearchStr2 As String = ""
    Private SearchStr3 As String = ""
    Private SearchStr4 As String = ""

    Dim Part As String
    Dim SerialNo As String
#End Region

#Region " Event "
    Private Sub btnPrintInstalled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintInstalled.Click

        If (Not User.IsInRole("ComponentRemovalPrint")) Then
            'Commented By Utkarsh On 26-Jul-2011 For All19072011

            'MarkLog(Util.Action.Print, "ComponentRemoval", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)

            'End

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'dgInstalledList.DataSource = mInstList
        'dgInstalledList.DataBind()

        Rpt = New crListInstalledRemovedComp
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        SearchStr1 = "Aircraft :" + "  " + cmbAircraft.SelectedItem.Text
        SearchStr2 = "Assembly :" + "  " + cmbAssembly.SelectedItem.Text
        If Part = "" Then
            SearchStr3 = ""
        Else
            SearchStr3 = "Part :" + " " + Part
        End If
        If SerialNo = "" Then
            SearchStr4 = ""
        Else
            SearchStr4 = "Serial No. :" + " " + SerialNo
        End If

        ReportDetails.Add(New rptStatus(, 0, "Installation Information", lblRemovalDate.Text, New SmartDate(calRemovalDate.Text).FormattedText))

        ReportDetails.Add(New rptStatus(, 1, , , , lblInstalledComponents.Text))

        ReportDetails.Add(New rptStatus(, 2, , _
              , , , dgInstalledList.Columns.Item(1).HeaderText, , dgInstalledList.Columns.Item(2).HeaderText, _
              dgInstalledList.Columns.Item(3).HeaderText, dgInstalledList.Columns.Item(4).HeaderText, _
              dgInstalledList.Columns.Item(5).HeaderText, dgInstalledList.Columns.Item(6).HeaderText, _
              dgInstalledList.Columns.Item(7).HeaderText, , , , , , , , , , , , dgInstalledList.Columns.Item(8).HeaderText, , , dgInstalledList.Columns.Item(9).HeaderText))

        Dim TotalCount As Integer
        TotalCount = Me.mInstList.Count
        Dim I As Integer

        Dim str(8) As String

        'For I = 0 To TotalCount - 1
        For I = 0 To dgInstalledList.Rows.Count - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""
            str(8) = ""

            Dim InstLabel, TSOLabel, TSNLabel As Label

            InstLabel = CType(Me.dgInstalledList.Rows(I).FindControl("lblInstValues"), Label)
            TSOLabel = CType(Me.dgInstalledList.Rows(I).FindControl("lblTSOValues"), Label)
            TSNLabel = CType(Me.dgInstalledList.Rows(I).FindControl("lblTSNValues"), Label)

            If Me.dgInstalledList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgInstalledList.Rows(I).Cells.Item(1).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgInstalledList.Rows(I).Cells.Item(2).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgInstalledList.Rows(I).Cells.Item(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgInstalledList.Rows(I).Cells.Item(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgInstalledList.Rows(I).Cells.Item(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgInstalledList.Rows(I).Cells.Item(6).Text.Replace("<BR>", vbCrLf)
            If InstLabel.Text <> "&nbsp;" Then str(6) = InstLabel.Text.Replace("<BR>", vbCrLf)
            If TSOLabel.Text <> "&nbsp;" Then str(7) = TSOLabel.Text.Replace("<BR>", vbCrLf)
            If TSNLabel.Text <> "&nbsp;" Then str(8) = TSNLabel.Text.Replace("<BR>", vbCrLf)
            ReportDetails.Add(New rptStatus(, 3, , _
             , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6), , , , , , , str(7), , str(8)))

        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "List of Installed Component Report", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mInstList.Count = 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            msg1.ReplacePage = "wfInstalledCompList.aspx?Backpage="
            msg1.Show()
            Exit Sub
        End If

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'Commented By Utkarsh On 26-Jul-2011 For All19072011

        'MarkLog(Util.Action.Print, "ComponentRemoval", "List of Installed Component Report", Util.ErrorType.NoError, Guid.Empty)

        'End
        'Dim Str1 As String
        'Str1 = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str1)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnPrintRemoved_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintRemoved.Click
        If (Not User.IsInRole("ComponentRemovalPrint")) Then
            'Commented By Utkarsh On 26-Jul-2011 For All19072011
            'MarkLog(Util.Action.Print, "ComponentRemoval", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
            'End
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'dgRemovedList.DataSource = mRemList
        'dgRemovedList.DataBind()
        'SetGrid()
        Rpt = New crListInstalledRemovedComp
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        SearchStr1 = "Aircraft :" + "  " + cmbAircraft.SelectedItem.Text
        SearchStr2 = "Assembly :" + "  " + cmbAssembly.SelectedItem.Text
        If Part = "" Then
            SearchStr3 = ""
        Else
            SearchStr3 = "Part :" + " " + Part
        End If
        If SerialNo = "" Then
            SearchStr4 = ""
        Else
            SearchStr4 = "Serial No. :" + " " + SerialNo
        End If

        ReportDetails.Add(New rptStatus(, 0, "Removal Information", lblRemovalDate.Text, New SmartDate(calRemovalDate.Text).FormattedText))

        ReportDetails.Add(New rptStatus(, 1, , , , lblRemovedComponents.Text))

        ReportDetails.Add(New rptStatus(, 2, , _
              , , , dgRemovedList.Columns.Item(1).HeaderText, , dgRemovedList.Columns.Item(2).HeaderText, _
              dgRemovedList.Columns.Item(3).HeaderText, dgRemovedList.Columns.Item(4).HeaderText, _
              dgRemovedList.Columns.Item(5).HeaderText, dgRemovedList.Columns.Item(6).HeaderText, _
              dgRemovedList.Columns.Item(7).HeaderText, , , , , , , , , , , , dgRemovedList.Columns.Item(8).HeaderText))

        Dim TotalCount As Integer
        TotalCount = Me.mRemList.Count
        Dim I As Integer

        Dim str(7) As String

        'For I = 0 To TotalCount - 1
        For I = 0 To dgRemovedList.Rows.Count - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""

            Dim RemLabel, TSOLabel As Label

            RemLabel = CType(Me.dgRemovedList.Rows(I).FindControl("lblRemValues"), Label)
            TSOLabel = CType(Me.dgRemovedList.Rows(I).FindControl("lblRemTSOValues"), Label)

            If Me.dgRemovedList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgRemovedList.Rows(I).Cells.Item(1).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgRemovedList.Rows(I).Cells.Item(2).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgRemovedList.Rows(I).Cells.Item(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgRemovedList.Rows(I).Cells.Item(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgRemovedList.Rows(I).Cells.Item(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgRemovedList.Rows(I).Cells.Item(6).Text.Replace("<BR>", vbCrLf)
            If RemLabel.Text <> "&nbsp;" Then str(6) = RemLabel.Text.Replace("<BR>", vbCrLf)
            If TSOLabel.Text <> "&nbsp;" Then str(7) = TSOLabel.Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 3, , _
           , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6), , , , , , , str(7)))
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "List of Installed Removed Component Report", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mRemList.Count = 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            msg1.ReplacePage = "wfInstalledCompList.aspx?Backpage="
            msg1.Show()
            Exit Sub
        End If

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'Commented By Utkarsh On 26-Jul-2011 For All19072011
        'MarkLog(Util.Action.Print, "ComponentRemoval", "List of Removed Component Report", Util.ErrorType.HandledError, Guid.Empty)
        'End
        'Dim Str1 As String
        'Str1 = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str1)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub txtPart_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Part = txtPart.Text
    End Sub
    Private Sub txtSerialNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        SerialNo = txtSerialNo.Text
    End Sub
#End Region

#End Region



End Class