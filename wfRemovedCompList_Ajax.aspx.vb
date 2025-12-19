'AJAX Conversion By Vikrant On 30-Mar-2015
Imports System.Linq
Imports System.Collections
Imports System.Collections.Generic
Public Class wfRemovedCompList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    'New Added
    Public mCompStatusList As CompStatusList
    Public mInstList As List(Of CompStatusInfo) = New List(Of CompStatusInfo)
    Public mRemList As List(Of CompStatusInfo) = New List(Of CompStatusInfo)
    'End
    'Public mtmpInstalledCompList As tmpInstalledCompList
    'Public mRemovedCompList As tmpRemovedCompList
    Public mMachineNameValueList As MachineNameValueList
    Public mAssemblylist As AssemblyList
    Public mCompStatus As CompStatus
    Public RemoveDate As String
    Public InstallOnId As String
    Public AircraftId As String
    Public AssemblyId As String
    '  Public mInstallCompStatus As CompStatus   'Code Added 25,Jan,2007
    ' Public mCurrentDate As String             'Added Code  25,Jan,2007
    'Public mCompInstallInfo As String         'Added Code  25,Jan,2007
    Public PartNo As String 'added by Rahul 29-apr-09

    '28-Apr-2009
    Public mInstallInAssemblylist As AssemblyList
    Public mInstallOnAssemblyID As String

    Public mMachineMaintenance As MachineMaintenance      'Added by Saylee on 8th-Oct-2009

    Dim EventLogID As Guid 'Added By Utkarsh On 26-Jul-2011 For All19072011
    Dim MaintDetail As String 'Added By Utkarsh On 26-Jul-2011 For All19072011

    'Added By Saylee On 27-Nov-2014 
    Dim mFileAttach As FileAttach
    'End
    Dim RecordsToShowForRemCompList As Integer
    Dim RecordsToShowForInstCompList As Integer

    Dim IsReadOnly As Boolean 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
    Dim IsReadOnlyInstalledOn As Boolean 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
    Public mSpareAssemblyComponent As Integer 'Added by Shital on 23-Dec-2020 
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        'New Added
        mInstList = CType(Session("mInstList"), List(Of CompStatusInfo))
        mRemList = CType(Session("mRemList"), List(Of CompStatusInfo))
        mCompStatusList = CType(Session("mCompStatusList"), CompStatusList)
        'End
        'mtmpInstalledCompList = CType(Session("mtmpInstalledCompList"), tmpInstalledCompList)
        'mRemovedCompList = CType(Session("mRemovedCompList"), tmpRemovedCompList)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        RemoveDate = CType(Session("RemoveDate"), String)
        AircraftId = CType(Session("AircraftId"), String)
        AssemblyId = CType(Session("AssemblyId"), String)
        InstallOnId = CType(Session("InstallOnId"), String)
        'Added by Rahul on 29-Apr-2009
        PartNo = CType(IIf(Session("PartNo") Is Nothing, "", Session("PartNo")), String)
        SerialNo = CType(IIf(Session("SerialNo") Is Nothing, "", Session("SerialNo")), String)

        '28-Apr-2009
        mInstallInAssemblylist = CType(Session("mInstallInAssemblylist"), AssemblyList)
        mInstallOnAssemblyID = CType(Session("mInstallOnAssemblyID"), String)

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 8th-Oct-2009
        'Added By Saylee On 27-Nov-2014 
        mFileAttach = Session("mFileAttach")
        'End    
        RecordsToShowForRemCompList = CType(Session("RecordsToShowForRemCompList"), Integer)
        RecordsToShowForInstCompList = CType(Session("RecordsToShowForInstCompList"), Integer)
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnlyInstalledOn = Session("IsReadOnlyInstalledOn") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        mSpareAssemblyComponent = CType(Session("mSpareAssemblyComponent"), Integer) 'Added By Shital On 23-Dec-2020 For ALL27072020
    End Sub
    Private Sub RemoveSession()
        'mtmpInstalledCompList = Nothing
        'mRemovedCompList = Nothing
        mMachineNameValueList = Nothing
        mAssemblylist = Nothing
        mCompStatus = Nothing
        Session.Remove("mtmpInstalledCompList")
        Session.Remove("mRemovedCompList")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mAssemblylist")
        Session.Remove("mCompStatus")
        Session.Remove("InstallOnId")
        '28-Apr-2009
        Session.Remove("mInstallInAssemblylist")
        Session.Remove("mInstallOnAssemblyID")
        Session.Remove("mMachineMaintenance") 'Added by Saylee on 8th-Oct-2009
        'Added By Saylee On 27-Nov-2014 
        Session.Remove("mFileAttach")
        'End
        Session.Remove("RecordsToShowForRemCompList")
        Session.Remove("RecordsToShowForInstCompList")
        'New Added
        Session.Remove("mCompStatusList")
        Session.Remove("mInstList")
        Session.Remove("mRemList")
        'End
        Session.Remove("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        Session.Remove("IsReadOnlyInstalledOn")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfRemovedCompList_Ajax.aspx?SpareAssemblyComponent=" & mSpareAssemblyComponent Then
            'Session.Remove("mtmpInstalledCompList")
            'Session.Remove("mRemovedCompList")
            Session.Remove("mMachineNameValueList")
            Session.Remove("mAssemblylist")
            Session.Remove("RemoveDate")
            Session.Remove("AircraftId")
            Session.Remove("AssemblyId")
            Session.Remove("InstallOnId")

            'Added by Rahul on 29-Apr-2009
            Session.Remove("PartNo")
            Session.Remove("SerialNo")
            ''====================

            '28-Apr-2009
            Session.Remove("mInstallOnAssemblyID") 'ClearAll

            Session.Remove("mMachineMaintenance") 'Added by Saylee on 8th-Oct-2009

            'Added By Saylee On 27-Nov-2014 
            Session.Remove("mFileAttach")
            'End
            Session.Remove("RecordsToShowForRemCompList")
            Session.Remove("RecordsToShowForInstCompList")
            'New Added
            Session.Remove("mCompStatusList")
            Session.Remove("mInstList")
            Session.Remove("mRemList")
            'End
            Session.Remove("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
            Session.Remove("IsReadOnlyInstalledOn")

        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim ErrorsCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Revert" Then
                        Session("sender") = ""
                        Dim RevertInstID As Guid = Session("RevertInstID")
                        Dim mCompStatusInfo As CompStatusInfo = mCompStatusList(RevertInstID)
                        If mCompStatusInfo.IsRemoved Then
                            MSGBoxCtrl.show(MSGBox.Message_title.ComponentIsRemoved, MSGBox.Message_text.ComponentIsRemoved, "You are trying to revert the installation.Component is currently removed,first revert the removal then revert the installation.", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        'Added Try-Catch on 1stJune ,2007 
                        'CompStatus.RevertInstalledCompStatus(mtmpInstalledCompList.CurrentItem.CompStatusID, mtmpInstalledCompList.CurrentItem.AssemblyStatusID, mtmpInstalledCompList.CurrentItem.RemovedOnDBValue)
                        Try
                            'Added by Saylee on 8th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompStatusInfo.ID, 3)
                            'Added by Saylee on 28-Nov-2014

                            If mCompStatusInfo.IsAttachmentAdded Then
                                'Dim mCompStatusID As Guid = mtmpInstalledCompList.Item(mtmpInstalledCompList.CurrentIndex).CompStatusID
                                mFileAttach = FileAttach.GetAttachment(mCompStatusInfo.ID, 1) 'Sort=1, for Installation 
                            End If

                            If mSpareAssemblyComponent = 0 Then 'mSpareAssemblyComponent = 0 Added by Shital on 28-Dec-2020 for All27072020
                                CompStatus.RevertInstalledCompStatus(mCompStatusInfo.ID, mCompStatusInfo.AssemblyStatusID, mCompStatusInfo.RemovedOnFormatted.ToString)
                            Else 'Added by Shital on 28-Dec-2020 for All27072020
                                If mCompStatusInfo.IsAssemblyInstalledRemoved = "Removed Assembly" Then
                                    CompStatus.RevertInstalledCompStatus(mCompStatusInfo.ID, mCompStatusInfo.AssemblyStatusID, mCompStatusInfo.RemovedOnFormatted.ToString)
                                Else
                                    CompStatus.DeleteSpareCompStatus(mCompStatusInfo.ID, True)
                                End If
                                'End
                            End If

                            Try
                                MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                                Session("mMachineMaintenance") = mMachineMaintenance
                                'Commented By Utkarsh On 26-Jul-2011 For All19072011
                                ' MarkLog(Util.Action.Save, "ComponentInstallation", "Revert: " + "( " + mtmpInstalledCompList.CurrentItem.MachineInfo + " / " + mtmpInstalledCompList.CurrentItem.AssemblyInfo + " ) " + mtmpInstalledCompList.CurrentItem.CompInfo, Util.ErrorType.NoError, mtmpInstalledCompList.CurrentItem.CompStatusID)
                                'End

                                If Not mFileAttach Is Nothing Then
                                    If mFileAttach.Size > 0 Then
                                        FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID, 1)
                                    End If
                                End If

                            Catch ex As Exception
                                '
                            End Try
                            'Commented by Saylee on 21-Oct-2013 FOR ALL21102013
                            'Response.Redirect("wfRemovedCompList_Ajax.aspx?MsgResult=0&BackPage=")
                        Catch ex As SqlException
                            'Commented by Saylee on 21-Oct-2013 FOR ALL21102013
                            '''''Title n Text does not match the condition pls change this in shro message box object.
                            '''''This title n text is for temporary period
                            ''''Dim msg1 As New SIMsgBox(Page, "Revert Component Installation!", "Can not revert the Installation.", "", MsgBoxStyle.OKOnly)
                            ''''msg1.ReplacePage = "wfRemovedCompList_Ajax.aspx?BackPage="
                            ''''' Session("sender") = "Revert"  'Temporarily Commented to check 
                            ''''msg1.Show()
                            'Added by Saylee on 21-Oct-2013 FOR ALL21102013
                            If ex.Number = 547 Then
                                ErrorsCount = ex.Errors.Count
                                MSGBoxCtrl.show("Revert Component Installation!", "Can not revert installation as Monitoring Service / Inspection / Modification is added on this Installed Component.", "", MsgBoxStyle.OkOnly, "")
                            Else
                                ErrorsCount = ex.Errors.Count
                                MSGBoxCtrl.show("Revert Component Installation!", "Can not revert the Installation.", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            'Added By Utkarsh On 26-Jul-2011 For All19072011
                        Finally
                            If ErrorsCount = 0 Then
                                MaintDetail = "Reg No. : " + mCompStatusInfo.MachineInfo & " Assembly Info : " & mCompStatusInfo.AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mCompStatusInfo.CompInfo.Replace(Environment.NewLine, " ")
                                MarkLog(Util.Action.RevertInstallation, "Component Installation", MaintDetail, Util.ErrorType.NoError, mCompStatusInfo.ID, EventLogID)
                            End If

                            'End
                        End Try
                        'DataFieldBind()   'Added Code on June 4,2007
                        'Added by Saylee on 21-Oct-2013 FOR ALL21102013
                        'SetPage()
                        'ControlVisibility()
                        'SetGrid()
                        DataFieldBind()
                        SetPage()
                        ControlVisibility()
                        upnlInstallationGrid.Update()
                        upnlRemovalGrid.Update()
                        upnlActionBtn.Update()
                        upnlActionBtnRemoved.Update()
                        upnlActionBtnRemovedTop.Update()
                        'End
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Cancel
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            '   DataFieldBind()
        End If
    End Sub
    'Added by Saylee on 19-Mar-2013 for ALL14032013-1
    'Added (RemovedCompStatus) parameter By Utkarsh ON 04-Apr-2013 FOR ALL04042013
    Public Function CheckPeriodsForRemovedCompStatus(ByVal RemovedCompStatus As CompStatus) As Boolean
        Dim i As Integer = 0
        Dim tmpIsPeriodExists As Boolean = False
        'Commented By Utkarsh ON 04-Apr-2013 FOR ALL04042013
        'Dim RemovedCompStatus As CompStatus = CompStatus.GetCompStatus(mRemovedCompList(Index).CompStatusID, mRemovedCompList(Index).AssemblyStatusID, txtInstallationDate.Text )
        'End
        ' Dim mtmpAssemblyStatusList As AssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(New Guid(cmbInstalledOnAssembly.SelectedValue.ToString))
        Dim mAssemblyStatusList As AssemblyStatusList
        mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtInstallationDate.Text, cmbInstalledOnAssembly.SelectedValue.ToString, , , , , , , , , , True, , , cmbInstalledOnAssemblyList.SelectedValue.ToString, , , , , , , , , , , , , , , , , MonitoringInspRequired:=False, MonitoringModRequired:=False, MonitoringServiceRequired:=False).Item(0), MachineInfo).AssemblyStatusList()

        '  For j As Integer = 0 To mAssemblyStatusList.Count - 1
        If mAssemblyStatusList(0).AssemblyID.Equals(New Guid(cmbInstalledOnAssemblyList.SelectedValue.ToString)) Then
            Dim tmpAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyStatusList(0).ID)
            While i <= RemovedCompStatus.CompStatusPeriods.Count - 1
                If tmpAssemblyStatus.AssemblyStatusPeriods.Contains(RemovedCompStatus.CompStatusPeriods(i).PeriodID) Then
                    tmpIsPeriodExists = True
                Else
                    tmpIsPeriodExists = False
                    Exit While
                End If
                i = i + 1
            End While
        End If
        '   Next

        Return tmpIsPeriodExists
    End Function
    Private Sub SetPage()
        If RecordsToShowForRemCompList < mRemList.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
            lblRemovedComponents.Text = "List of Removed components as per selected criteria : " & RecordsToShowForRemCompList.ToString & " of " & mRemList.Count & " Record(s) shown."
        Else
            lblRemovedComponents.Text = "List of Removed components as per selected criteria : " & mRemList.Count & " Record(s) found."
        End If
        'lblRemovedComponents.Text = "List of Removed components as per selected criteria : " & mRemovedCompList.Count & " Record(s) found."
        '' FindNow()
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then
            'do nothing as this datagrid is not visible for heligo
            If cmbAircraft.SelectedIndex > 0 Then
                If Not mInstList Is Nothing Then
                    'Added By Vikrant For Showing First 5 records
                    If AppSettings("IsShowAllRecordsVisible") = "True" Then
                        dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAircraftNotInUse = False And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                        Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
                    Else
                        dgInstalledList.DataSource = mInstList
                    End If
                    dgInstalledList.DataBind()
                    dgInstalledList.Visible = True
                    lblInstalledComponents.Visible = True
                    'lblInstalledComponents.Text = "List of Installed components as per selected criteria : " & mtmpInstalledCompList.Count & " Record(s) found."
                    If RecordsToShowForInstCompList < mInstList.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
                        lblInstalledComponents.Text = "List of Installed components as per selected criteria : " & RecordsToShowForInstCompList.ToString & " of " & mInstList.Count & " Record(s) shown."
                    Else
                        lblInstalledComponents.Text = "List of Installed components as per selected criteria : " & mInstList.Count & " Record(s) found."
                    End If
                    lnkInstCompLoadMore.Visible = IIf(AppSettings("IsShowAllRecordsVisible") = "True", True, False)
                    lnkInstCompLoadMoreTop.Visible = IIf(AppSettings("IsShowAllRecordsVisible") = "True", True, False)
                End If
            Else
                lblInstalledComponents.Visible = False
                lnkInstCompLoadMore.Visible = False
                lnkInstCompLoadMoreTop.Visible = False
            End If

        Else
            lblInstalledComponents.Visible = True
            'lblInstalledComponents.Text = "List of Installed components as per selected criteria : " & mtmpInstalledCompList.Count & " Record(s) found."
            If RecordsToShowForInstCompList < mInstList.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
                lblInstalledComponents.Text = "List of Installed components as per selected criteria : " & RecordsToShowForInstCompList.ToString & " of " & mInstList.Count & " Record(s) shown."
            Else
                lblInstalledComponents.Text = "List of Installed components as per selected criteria : " & mInstList.Count & " Record(s) found."
            End If
            lnkInstCompLoadMore.Visible = IIf(AppSettings("IsShowAllRecordsVisible") = "True", True, False)
            lnkInstCompLoadMoreTop.Visible = IIf(AppSettings("IsShowAllRecordsVisible") = "True", True, False)
        End If
        HighlightSpareAssembly() 'Added by Shital on 28-Dec-2020 for All27072020
    End Sub
    Private Sub ControlVisibility()
        btnPrintRemoved.Enabled = (mRemList.Count > 0)
        btnPrintRemovedTop.Enabled = (mRemList.Count > 0)
        'btnPrintRemovedTop.Visible = (mRemList.Count > 15)
        btnAddNewTop.Visible = (mRemList.Count > 15)

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then
            'do nothing as this datagrid is not visible for heligo
            If cmbAircraft.SelectedIndex > 0 Then
                'btnPrintInstalled.Visible = True
                If Not mInstList Is Nothing Then btnPrintInstalled.Enabled = (mInstList.Count > 0)
            Else
                'btnPrintInstalled.Visible = False
            End If
        Else
            'btnPrintInstalled.Visible = True
            btnPrintInstalled.Enabled = (mInstList.Count > 0)
        End If
        EnableLinks()
        'Added By Shital On 23-Dec-2020 For ALL27072020
        'cmbAircraft.Visible = IIf(mSpareAssemblyComponent = 0, True, False)
        'lblAircraft.Visible = IIf(mSpareAssemblyComponent = 0, True, False)
        placeHolder1.Visible = IIf(mSpareAssemblyComponent = 0, True, False)
        PlaceHolder2.Visible = IIf(mSpareAssemblyComponent = 0, True, False)
        'End
        'Added By Prashant 2-Dec-2020
        If (User.IsInRole("BuildSpareCompNew") = True And User.IsInRole("BuildSpareCompEdit") = True) And mSpareAssemblyComponent = 0 Then
            lnkSpareComponent.Visible = True
        End If
        'End of Added By Prashant 2-Dec-2020
        SetGrid()
    End Sub
    Private Sub FindNow()
        RecordsToShowForInstCompList = dgInstalledList.PageSize
        RecordsToShowForRemCompList = dgRemovedList.PageSize
        Session("RecordsToShowForInstCompList") = RecordsToShowForInstCompList
        Session("RecordsToShowForRemCompList") = RecordsToShowForRemCompList

        dgInstalledList.PageIndex = 0
        dgRemovedList.PageIndex = 0
        Session("RemoveDate") = txtInstallationDate.Text
        Session("AircraftId") = cmbAircraft.SelectedValue
        Session("AssemblyId") = cmbAssembly.SelectedValue
        Session("InstallOnId") = cmbInstalledOnAssembly.SelectedValue

        Session("mInstallOnAssemblyID") = cmbInstalledOnAssemblyList.SelectedValue '28-Apr-2009

        'Added By Rahul on 29-Apr-2009
        Session("PartNO") = Trim(txtPart.Text)
        Session("SerialNo") = Trim(txtSerialNo.Text)
        '============================================

        'New Added
        If mSpareAssemblyComponent = 0 Then  'If Condition Added by Shitalon 23-Dec-2020
            mCompStatusList = CompStatusList.GetCompStatusList(CurrentDate:=txtInstallationDate.Text, AssemblyID:=New Guid(cmbAssembly.SelectedValue), PartName:=Trim(txtPart.Text), CompSerialNo:=Trim(txtSerialNo.Text), MachineID:=New Guid(cmbAircraft.SelectedValue).ToString, IsCompInstalled:=True, IsCompRemoved:=True, IsCompPeriodsRequired:=False, ShowForNotInUseAircrafts:=CType(AppSettings("ShowForNotInUseAircrafts"), Boolean)) 'New Added
        Else
            mCompStatusList = CompStatusList.GetCompStatusList(CurrentDate:=txtInstallationDate.Text, AssemblyID:=New Guid(cmbAssembly.SelectedValue), PartName:=Trim(txtPart.Text), CompSerialNo:=Trim(txtSerialNo.Text), MachineID:=Guid.Empty.ToString, IsCompInstalled:=True, IsCompRemoved:=True, IsCompPeriodsRequired:=False, ShowForNotInUseAircrafts:=CType(AppSettings("ShowForNotInUseAircrafts"), Boolean), IsSpareAssemblyInstalledRemovedCompRequired:=True, IsSpareComponentAlsoRequired:=True) 'New Added
        End If

        Session("mCompStatusList") = mCompStatusList
        mRemList = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Removed"
                                        Select StatusInfo).ToList()
        mInstList = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAircraftNotInUse = False And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                        Select StatusInfo).ToList()
        Session("mRemList") = mRemList
        Session("mInstList") = mInstList
        'End

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then

            If cmbAircraft.SelectedIndex > 0 Then
                'mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(txtInstallationDate.Text, IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedValue, Guid.Empty.ToString), Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue))
                dgInstalledList.Visible = True
                'Added By Vikrant For Showing First 5 records
                If AppSettings("IsShowAllRecordsVisible") = "True" Then
                    dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAircraftNotInUse = False And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                        Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
                Else
                    dgInstalledList.DataSource = mInstList
                End If
                'Session("mtmpInstalledCompList") = mtmpInstalledCompList
                dgInstalledList.DataBind()
            Else
                dgInstalledList.Visible = False
            End If

        Else
            dgInstalledList.Visible = True
            'mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(txtInstallationDate.Text, IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedValue, Guid.Empty.ToString), Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue))
            'Added By Vikrant For Showing First 5 records
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAircraftNotInUse = False And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                        Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
            Else
                dgInstalledList.DataSource = mInstList
            End If
            'Session("mtmpInstalledCompList") = mtmpInstalledCompList
            dgInstalledList.DataBind()
        End If


        'mRemovedCompList = tmpRemovedCompList.GetRemovedCompList(txtInstallationDate.Text, IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedValue, Guid.Empty.ToString), Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue), CType(AppSettings("ShowForNotInUseAircrafts"), Boolean))
        'Added By Vikrant For Showing First 5 records
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            dgRemovedList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Removed"
                                        Select StatusInfo).ToList.Take(RecordsToShowForRemCompList)
        Else
            dgRemovedList.DataSource = mRemList
        End If

        'Session("mRemovedCompList") = mRemovedCompList
        dgRemovedList.DataBind()
    End Sub
    Private Sub NewRecord()

        Dim mCompStatus As CompStatus
        Dim mAssemblyStatus As AssemblyStatus '= AssemblyStatus.GetAssemblyStatus(mCompStatus.ass)
        'Dim mMachine As Machine = Machine.GetMachine(New Guid(cmbAircraft.SelectedValue))
        'Assemblyid=Empty

        '28-Apr-2009 Commented
        'mCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, Guid.Empty, Guid.Empty, txtInstallationDate.Text , False, Guid.Empty.ToString, Guid.Empty.ToString)
        '28-Apr-2009 Replaced
        mCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, mInstallInAssemblylist.Item(cmbInstalledOnAssemblyList.SelectedIndex).ID, Guid.Empty, txtInstallationDate.Text, False, Guid.Empty.ToString, Guid.Empty.ToString)
        '-------

        mCompStatus.ModelID = mInstallInAssemblylist.Item(cmbInstalledOnAssemblyList.SelectedIndex).ModelID
        Session("ModelID") = mCompStatus.ModelID

        Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompStatus.ID, Sort:=1) 'Sort = 1 : Installation
        Session("mFileAttach") = mFileAttach

        Session("From") = 1 'NewInstall
        Session("mRemovedCompStatus") = Nothing
        Session("mCompStatus") = mCompStatus
        Session("mAssemblyStatus") = mAssemblyStatus

        '---28-Apr-2009
        Session("IsAdded") = "False"
        Session("InstallOnId") = cmbInstalledOnAssembly.SelectedValue
        Session("mInstallOnAssemblyID") = cmbInstalledOnAssemblyList.SelectedValue
        '---28-Apr-2009

        'Added by Saylee on 8th-Oct-2009

        Dim mMaxLogNo As MaxLogNo
        'mMaxLogNo = MaxLogNo.GetMaxLogNo(txtInstallationDate.Text , mAssemblyStatus.MachineID, mCompStatus.AssemblyID)
        mMaxLogNo = MaxLogNo.GetMaxLogNo(txtInstallationDate.Text, New Guid(cmbInstalledOnAssembly.SelectedValue), mInstallInAssemblylist.Item(cmbInstalledOnAssemblyList.SelectedIndex).ID)
        If mMaxLogNo.Count <> 0 Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(New Guid(cmbInstalledOnAssembly.SelectedValue), 3, txtInstallationDate.Text, mCompStatus.ID, mMaxLogNo(0).LogId, mMaxLogNo(0).LogNo, mMaxLogNo(0).LogPageNo, Guid.Empty)
        End If
        Session("mMachineMaintenance") = mMachineMaintenance


        If (Not User.IsInRole("ComponentInstallationNew") And mCompStatus.IsNew) Or (Not User.IsInRole("ComponentInstallationEdit") And Not mCompStatus.IsNew) Then
            'Changed By Utkarsh On 26-Jul-2011 For All19072011
            MarkLog(Util.Action.[New], "Component Installation", User.Identity.Name & " is not Authorized User to add new ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'End
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        'mCompInstallInfo = "ATAChapter -> " + mCompStatus.ATAChapter + "mAssemblyStatusList Part -> " + mCompStatus.PartNameSerialNo + " InstallOn -> " + CurrentDate   'Code Added Jan,25,2007
        'Changed By Utkarsh On 26-Jul-2011 For All19072011
        MarkLog(Util.Action.[New], "Component Installation", "", Util.ErrorType.NoError, mCompStatus.ID, EventLogID) 'Code Added Jan,25,2007
        'End

        '''Changed By Utkarsh ON 24-Apr-2012 For ALL23042012 (For Buddha Air)
        ''If (AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
        ''    str = "openledgersame('wfInstallCompBA.aspx?GChildPage2=Index.aspx');"
        ''Else
        ''    str = "openledgersame('wfInstallComp.aspx?GChildPage2=Index.aspx');"
        ''End If
        '''End
        ''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", str, True)
        mSpareAssemblyComponent = CType(Session("mSpareAssemblyComponent"), Integer) 'Added By Shital On 23-Dec-2020 For ALL27072020
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfInstallComp_AJAX.aspx?GChildPage2=Index.aspx');", True)
    End Sub
    Private Sub InstallRecord(ByVal mCompStatusInfo As CompStatusInfo)
        'Added By Utkarsh ON 04-Apr-2013 FOR ALL04042013
        Dim mRemovedCompStatus As CompStatus = CompStatus.GetCompStatus(mCompStatusInfo.ID, mCompStatusInfo.AssemblyStatusID, txtInstallationDate.Text)
        'End
        'Added by Saylee on 19-Mar-2013 for ALL14032013-1
        'Changed By Utkarsh ON 04-Apr-2013 FOR ALL04042013 (if condition & Message text)
        If cmbInstalledOnAssembly.SelectedIndex <> 0 AndAlso cmbInstalledOnAssemblyList.SelectedIndex <> 0 AndAlso CheckPeriodsForRemovedCompStatus(mRemovedCompStatus) = False Then
            MSGBoxCtrl.show("Component Status Installation Alert!", "Periods for " & mRemovedCompStatus.PartNameSerialNo & " are mismatching with selected " & cmbInstalledOnAssemblyList.SelectedItem.Text & " Assembly on " & cmbInstalledOnAssembly.SelectedItem.Text & " .Can not be installed.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            'Dim chkInstalledCompStatusList As tmpInstalledCompList
            'chkInstalledCompStatusList = tmpInstalledCompList.GetInstalledCompList("1/1/2099", Guid.Empty.ToString, "", "", Guid.Empty)
            'If chkInstalledCompStatusList.Contains(mRemovedCompList(Index).PartID, mRemovedCompList(Index).CompSerialNo, mRemovedCompList(Index).Code) = True Then
            '    'message for already installed
            '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ComponentIsAlreadyInstalled, SIMsgBox.Message_text.ComponentIsAlreadyInstalled, "You are trying to install component.Selected Component already installed. Can not be installed again.", MsgBoxStyle.OKOnly)
            '    msg1.ReplacePage = "wfRemovedCompList_Ajax.aspx?BackPage="
            '    Session("sender") = "InstallSelected"
            '    msg1.Show()
            '    Exit Sub
            'End If
            'AssemblyId=Empty

            '28-Apr-2009 Commented
            'Dim mCompStatus As CompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, Guid.Empty, mRemovedCompList(Index).AssemblyStatusID, txtInstallationDate.Text , True, mRemovedCompList(Index).CompStatusID.ToString, Guid.Empty.ToString)
            '28-Apr-2009 Replaced
            Dim mCompStatus As CompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, mInstallInAssemblylist.Item(cmbInstalledOnAssemblyList.SelectedIndex).ID, mCompStatusInfo.AssemblyStatusID, txtInstallationDate.Text, True, mCompStatusInfo.ID.ToString, Guid.Empty.ToString)
            '---

            'Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompStatusInfo.AssemblyStatusID)
            'Dim mMachine As Machine = Machine.GetMachine(mCompStatusInfo.MachineID)
            Dim mAssemblyStatus As AssemblyStatus
            Dim mMachine As Machine
            If mSpareAssemblyComponent = 0 Then
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompStatusInfo.AssemblyStatusID)
                mMachine = Machine.GetMachine(mCompStatusInfo.MachineID)
            End If

            Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompStatus.ID, Sort:=1) 'Sort = 1 : Installation
            Session("mFileAttach") = mFileAttach
            '---28-Apr-2009
            Session("IsAdded") = "False"
            Session("InstallOnId") = cmbInstalledOnAssembly.SelectedValue
            Session("mInstallOnAssemblyID") = cmbInstalledOnAssemblyList.SelectedValue
            '---28-Apr-2009

            Session("From") = 1 'NewInstall
            Session("InstallSelected") = 1
            Session("mCompStatus") = mCompStatus
            Session("mRemovedCompStatus") = mRemovedCompStatus
            Session("mAssemblyStatus") = mAssemblyStatus
            Session("mMachine") = mMachine

            ''NewMachineMaintenance() 'Added by Saylee on 8th-Oct-2009

            'Changed By Utkarsh On 26-Jul-2011 For All19072011
            MaintDetail = "Reg No. : " + mCompStatusList(mRemovedCompStatus.ID).MachineInfo & " Assembly Info : " & mCompStatusList(mRemovedCompStatus.ID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mCompStatusList(mRemovedCompStatus.ID).CompInfo.Replace(Environment.NewLine, " ")
            MarkLog(Util.Action.Install, "Component Installation", MaintDetail, Util.ErrorType.NoError, mRemovedCompStatus.ID, EventLogID)
            'End


            '''Changed By Utkarsh ON 24-Apr-2012 For ALL23042012 (For Buddha Air)
            ''If (AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
            ''    str = "openledgersame('wfInstallCompBA.aspx?GChildPage2=Index.aspx');"
            ''Else
            ''    str = "openledgersame('wfInstallComp.aspx?GChildPage2=Index.aspx');"
            ''End If
            '''End
            ''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", str, True)

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfInstallComp_AJAX.aspx?GChildPage2=Index.aspx');", True)
        End If
    End Sub
    Private Sub EditRecord(ByVal mCompStatusInfo As CompStatusInfo)
        If mCompStatusInfo.IsMaster Then
            'MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "You are trying to edit installed component.This is a master record and can not be edited from here", MsgBoxStyle.OkOnly, "")
            MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.GetInstallCompStatusFromEntry(mCompStatusInfo.ID, mCompStatusInfo.AssemblyStatusID, mCompStatusInfo.InstalledOnFormatted.ToString)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompStatusInfo.AssemblyStatusID)
        Dim mMachine As Machine

        If mSpareAssemblyComponent = 0 Then 'If Condition Added by Shital on 23-Dec-2020
            mMachine = Machine.GetMachine(mCompStatusInfo.MachineID) 'MachineInfo = RegNo in this case
        End If

        If mCompStatus.IsAttachmentAdded Then
            Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mCompStatusInfo.ID, 1) 'Sort = 1 - Installation
            Session("mFileAttach") = mFileAttach
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompStatusInfo.ID, Sort:=1)
            Session("mFileAttach") = mFileAttach
        End If

        Session("From") = 2 'EditInstall
        Session("mCompStatus") = mCompStatus
        Session("mRemovedCompStatus") = Nothing
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMachine") = mMachine


        '' GetMachineMaintenance()  'Added by Saylee on 8th-Oct-2009

        'Added By Utkarsh On 26-Jul-2011 For All19072011
        MaintDetail = "Reg No. : " + mCompStatusList(mCompStatus.ID).MachineInfo & " Assembly Info : " & mCompStatusList(mCompStatus.ID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mCompStatusList(mCompStatus.ID).CompInfo.Replace(Environment.NewLine, " ") + " Installed On : " + String.Format(mCompStatusList(mCompStatus.ID).InstalledOnFormatted, "dd-MM-yyyy")
        MarkLog(Util.Action.Edit, "Component Installation", MaintDetail, Util.ErrorType.NoError, mCompStatus.ID, EventLogID)
        'End

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfInstallComp_AJAX.aspx?GChildPage2=Index.aspx');", True)
    End Sub
    Private Sub RevertRecord(ByVal mCompStatusInfo As CompStatusInfo)
        If mCompStatusInfo.IsMaster Then
            MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordRevert, MSGBox.Message_text.MasterRecordRevert, "You are trying to revert the component.This is a master record and can not be reverted from here.", MsgBoxStyle.OkOnly, "")
            'Session("sender") = "Revert"
            'Session("RevertIndex") = Index
            'Me.mtmpInstalledCompList.CurrentIndex = Index
            'Session("mtmpInstalledCompList") = mtmpInstalledCompList
        Else
            'Confirm revert?
            Session("RevertInstID") = mCompStatusInfo.ID
            MSGBoxCtrl.show(MSGBox.Message_title.ConfirmRevert, MSGBox.Message_text.ConfirmRevert, "You are trying to revert the component.Confirm revert?", MsgBoxStyle.YesNo, "Revert")
        End If
    End Sub
    Private Sub HistoryRecord(ByVal mCompStatusInfo As CompStatusInfo)
        Dim mCompStatus As CompStatus = CompStatus.GetInstallCompStatusFromEntry(mCompStatusInfo.ID, mCompStatusInfo.AssemblyStatusID, mCompStatusInfo.InstalledOnFormatted.ToString)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompStatusInfo.AssemblyStatusID)
        'Dim mMachine As Machine = Machine.GetMachine(mtmpInstalledCompList(Index).MachineID)
        Session("From") = 2 'EditInstall
        Session("mCompStatus") = mCompStatus
        Session("mRemovedCompStatus") = Nothing
        Session("mAssemblyStatus") = mAssemblyStatus
        'Session("mMachine") = mMachine

        'Added by Saylee on 20-Oct-2009
        Dim mUpdateHistoryCompStausList As UpdateHistoryCompStatusList
        Session("PartName") = mCompStatus.PartName
        Session("CompSerialNo") = mCompStatus.SerialNo

        mUpdateHistoryCompStausList = UpdateHistoryCompStatusList.GetInstalledCompList(txtInstallationDate.Text, mAssemblyStatus.ID, mCompStatus.CompID)
        Session("mUpdateHistoryCompStausList") = mUpdateHistoryCompStausList
        '========================================

        'Added By Utkarsh On 26-Jul-2011 For All19072011
        MaintDetail = "Reg No. : " + mCompStatusList(mCompStatus.ID).MachineInfo & " Assembly Info : " & mCompStatusList(mCompStatus.ID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mCompStatusList(mCompStatus.ID).CompInfo.Replace(Environment.NewLine, " ")
        MarkLog(Util.Action.View, "Component Installation", MaintDetail, Util.ErrorType.NoError, mCompStatus.ID, EventLogID)
        'End
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfUpdateInstalledCompHistory.aspx?BackPage=Index.aspx');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInstallationHistoryWindow", "OpenInstallationHistoryWindow();", True)
    End Sub
    'Added by Saylee 28-Nov-2014
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        If mIsAttachemntAdded = True Then
            mFileAttach = FileAttach.GetAttachment(ID, 1) 'Sort = 1 - Installation
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub ViewImage(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        GetAttachment(ID, mIsAttachemntAdded)
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
    'End
#End Region

#Region " Data Bindng "
    Private Sub DataFieldBind()
        If Not IsDate(RemoveDate) Then
            txtInstallationDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            RemoveDate = Today.Date.ToString(AppSettings("DateFormat")) 'Added By Rahul on 29-Apr-2009
        Else
            txtInstallationDate.Text = CDate(RemoveDate).ToString(AppSettings("DateFormat"))
        End If
        'Commented and added by Saylee on 11th-Jan-2008
        ' mMachineNameValueList = tmpMachineList.GetMachineList(, , , , , "(ALL)")
        'mMachineNameValueList = tmpMachineList.GetMachineList(, , , , , "(SELECT)")

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , True, "(ALL)", , True)
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataSource = mMachineNameValueList


        If mMachineNameValueList.Count > 1 And (IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString) Then
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then
                AircraftId = Guid.Empty.ToString
                AssemblyId = Guid.Empty.ToString
            Else
                AircraftId = mMachineNameValueList(1).ID.ToString
                AssemblyId = Guid.Empty.ToString
            End If
        Else
            AircraftId = AircraftId
        End If

        'If IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString Then
        '    AssemblyId = Guid.Empty.ToString
        'Else
        '    AircraftId = AircraftId
        'End If

        'mAssemblylist = AssemblyList.GetAssemblyList(0, AircraftId, txtInstallationDate.Text , "(ALL)")
        If mSpareAssemblyComponent = 0 Then  'If Condition Added by Shital on 23-Dec-2020
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, AircraftId, txtInstallationDate.Text, "(All)", True)
        Else
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtInstallationDate.Text.ToString, "(All)", True, IsForSpareAssembly:=True) '  
        End If

        Session("mAssemblylist") = mAssemblylist
        cmbAssembly.DataSource = mAssemblylist
        If IsNothing(AssemblyId) Or AssemblyId = Guid.Empty.ToString Then AssemblyId = Guid.Empty.ToString Else AssemblyId = AssemblyId

        '28-Apr-2009
        ''mInstallInAssemblylist = AssemblyList.GetAssemblyList(0, InstallOnId, txtInstallationDate.Text , "(SELECT)")
        If mSpareAssemblyComponent = 0 Then  'If Condition Added by Shital on 23-Dec-2020
            mInstallInAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, InstallOnId, txtInstallationDate.Text, "(SELECT)", True)
        Else
            mInstallInAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, InstallOnId, txtInstallationDate.Text, "(SELECT)", True, IsForSpareAssembly:=True)
        End If


        Session("mInstallInAssemblylist") = mInstallInAssemblylist
        cmbInstalledOnAssemblyList.DataSource = mInstallInAssemblylist
        '---

        REM: Machine List without "All"
        cmbInstalledOnAssembly.DataSource = mMachineNameValueList

        'Commented and added by Rahul 29-Apr-09
        'mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(txtInstallationDate.Text , AircraftId, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(AssemblyId))

        'Commented and added by Rahul 29-Apr-09
        'VmRemovedCompList = tmpRemovedCompList.GetRemovedCompList(RemoveDate, AircraftId, PartNo, SerialNo, New Guid(AssemblyId), CType(AppSettings("ShowForNotInUseAircrafts"), Boolean))
        'New Added


        If mSpareAssemblyComponent = 0 Then  'If Condition Added by Shital on 23-Dec-2020
            mCompStatusList = CompStatusList.GetCompStatusList(CurrentDate:=RemoveDate, AssemblyID:=New Guid(AssemblyId), PartName:=PartNo, CompSerialNo:=SerialNo, MachineID:=AircraftId, IsCompInstalled:=True, IsCompRemoved:=True, IsCompPeriodsRequired:=False, ShowForNotInUseAircrafts:=CType(AppSettings("ShowForNotInUseAircrafts"), Boolean)) 'New Added
        Else
            mCompStatusList = CompStatusList.GetCompStatusList(CurrentDate:=RemoveDate, AssemblyID:=New Guid(AssemblyId), PartName:=PartNo, CompSerialNo:=SerialNo, MachineID:=AircraftId, IsCompInstalled:=True, IsCompRemoved:=True, IsCompPeriodsRequired:=False, ShowForNotInUseAircrafts:=CType(AppSettings("ShowForNotInUseAircrafts"), Boolean), IsSpareAssemblyInstalledRemovedCompRequired:=True, IsSpareComponentAlsoRequired:=True) 'New Added
        End If

        Session("mCompStatusList") = mCompStatusList
        mRemList = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Removed"
                                        Select StatusInfo).ToList()
        mInstList = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAircraftNotInUse = False And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                        Select StatusInfo).ToList()
        Session("mRemList") = mRemList
        Session("mInstList") = mInstList
        'End


        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then
            'If cmbAircraft.SelectedIndex > 0 Then
            If Not AircraftId = Guid.Empty.ToString Then
                dgInstalledList.Visible = True
                'VmInstalledCompList = tmpInstalledCompList.GetInstalledCompList(RemoveDate, AircraftId, PartNo, SerialNo, New Guid(AssemblyId))
                'Added By Vikrant For Showing First 5 records
                If AppSettings("IsShowAllRecordsVisible") = "True" Then
                    dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAircraftNotInUse = False And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                        Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
                Else
                    dgInstalledList.DataSource = mInstList
                End If
                'Session("mtmpInstalledCompList") = mtmpInstalledCompList
                dgInstalledList.DataBind()
            Else
                dgInstalledList.Visible = False
            End If
        Else
            dgInstalledList.Visible = True
            'mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(RemoveDate, AircraftId, PartNo, SerialNo, New Guid(AssemblyId))
            'Added By Vikrant For Showing First 5 records
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAircraftNotInUse = False And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                        Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
            Else
                dgInstalledList.DataSource = mInstList
            End If
            'Session("mtmpInstalledCompList") = mtmpInstalledCompList
        End If

        'Added By Vikrant For Showing First 5 records
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            dgRemovedList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Removed"
                                        Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
        Else
            dgRemovedList.DataSource = mRemList
        End If
        'Session("mRemovedCompList") = mRemovedCompList

        If mSpareAssemblyComponent = 0 Then  'If Condition Added by Shital on 23-Dec-2020
            'DO Nothing
        Else
            dgRemovedList.Columns(1).Visible = False
            dgInstalledList.Columns(1).Visible = False
        End If
        DataBind()

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then
            If IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString Then cmbAircraft.SelectedIndex = 0 Else cmbAircraft.SelectedValue = AircraftId
        Else
            If IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString Then cmbAircraft.SelectedIndex = 1 Else cmbAircraft.SelectedValue = AircraftId
        End If

        If IsNothing(AssemblyId) Or AssemblyId = Guid.Empty.ToString Then cmbAssembly.SelectedIndex = 0 Else cmbAssembly.SelectedValue = AssemblyId
        If IsNothing(InstallOnId) Or InstallOnId = Guid.Empty.ToString Then cmbInstalledOnAssembly.SelectedIndex = 0 Else cmbInstalledOnAssembly.SelectedValue = InstallOnId

        '28-Apr-2009
        If IsNothing(mInstallOnAssemblyID) Or mInstallOnAssemblyID = Guid.Empty.ToString Then cmbInstalledOnAssemblyList.SelectedIndex = 0 Else cmbInstalledOnAssemblyList.SelectedValue = mInstallOnAssemblyID

        RemoveDate = txtInstallationDate.Text
        AircraftId = cmbAircraft.SelectedValue
        AssemblyId = cmbAssembly.SelectedValue
        InstallOnId = cmbInstalledOnAssembly.SelectedValue

        'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraft.SelectedValue)).IsReadOnly 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnlyInstalledOn = mMachineNameValueList(New Guid(cmbInstalledOnAssembly.SelectedValue)).IsReadOnly

        Session("IsReadOnly") = IsReadOnly
        Session("IsReadOnlyInstalledOn") = IsReadOnlyInstalledOn
        '***********************************

        'Added By Rahul on 29-Apr-2009
        txtPart.Text = PartNo
        txtSerialNo.Text = SerialNo
        '===========================

        Session("RemoveDate") = RemoveDate
        Session("MachineId") = AircraftId
        Session("AssemblyId") = AssemblyId
        Session("InstallOnId") = InstallOnId
        '28-Apr-2009
        Session("mInstallOnAssemblyID") = mInstallOnAssemblyID
    End Sub
    'Added by Shital on 28-Dec-2020 for All27072020
    Private Sub HighlightSpareAssembly()
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
                        item.Attributes.Add("title", "Stock Assembly")
                    End If
                Next
            Next
            For Each dr As DataRowView In dv
                For Each item As ListItem In cmbInstalledOnAssemblyList.Items
                    If dr("ID").ToString() = item.Value.ToString() Then
                        item.Attributes.Add("style", "background-color:#ffbf00;color:black;font-weight:bold;")
                        item.Attributes.Add("title", "Stock Assembly")
                    End If
                Next
            Next
        End If
    End Sub
    'End
    Private Sub GridBind(Optional ByVal IsInstalledList As Boolean = False, Optional ByVal IsRemovedList As Boolean = False)
        mRemList = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Removed"
                                        Select StatusInfo).ToList()
        mInstList = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAircraftNotInUse = False And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                        Select StatusInfo).ToList()
        If IsInstalledList Then
            'Added By Vikrant For Showing First 5 records
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAircraftNotInUse = False And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                        Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
            Else
                dgInstalledList.DataSource = mInstList
            End If
            dgInstalledList.DataBind()
        End If
        If IsRemovedList Then
            'Added By Vikrant For Showing First 5 records
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                dgRemovedList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Removed"
                                        Select StatusInfo).ToList.Take(RecordsToShowForRemCompList)
            Else
                dgRemovedList.DataSource = mRemList
            End If
            dgRemovedList.DataBind()
        End If

    End Sub
    Private Sub SetGrid()
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnlyInstalledOn = Session("IsReadOnlyInstalledOn")

        For j As Integer = 0 To dgRemovedList.Rows.Count - 1
            'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
            'Disable Comply,Edit and Delete links if Aircraft is ReadOnly
            'Commented and Added By Vikrant On 05-Oct-2021 for ALL05102021-1
            'If (IsReadOnlyInstalledOn = True Or IsReadOnly = True) And mSpareAssemblyComponent = 0 Then 'mSpareAssemblyComponent Added by Shital on 23-Dec-2020
            If (IsReadOnlyInstalledOn = True) And mSpareAssemblyComponent = 0 Then 'mSpareAssemblyComponent Added by Shital on 23-Dec-2020
                'End
                dgRemovedList.Rows(j).Cells(9).Enabled = False
            Else
                dgRemovedList.Rows(j).Cells(9).Enabled = True
            End If
            '*************************
        Next

        For j As Integer = 0 To dgInstalledList.Rows.Count - 1
            'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
            'Disable Comply,Edit and Delete links if Aircraft is ReadOnly
            If IsReadOnly = True And mSpareAssemblyComponent = 0 Then 'mSpareAssemblyComponent Added by Shital on 23-Dec-2020
                dgInstalledList.Rows(j).Cells(10).Enabled = False
                dgInstalledList.Rows(j).Cells(11).Enabled = False
            Else
                dgInstalledList.Rows(j).Cells(10).Enabled = True
                dgInstalledList.Rows(j).Cells(11).Enabled = True
            End If
            '*************************
        Next

        'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        'Disable AddNew buttons if Aircraft is ReadOnly
        If IsReadOnly = True And mSpareAssemblyComponent = 0 Then
            btnAddNewTop.Enabled = False
            btnAddNew.Enabled = False
            lblReadOnly.Visible = True
        Else
            btnAddNewTop.Enabled = True
            btnAddNew.Enabled = True
            lblReadOnly.Visible = False
        End If

        If IsReadOnlyInstalledOn = True Then
            btnAddNewTop.Enabled = False
            btnAddNew.Enabled = False
            lblReadOnlyInstalledOn.Visible = True
        Else
            btnAddNewTop.Enabled = True
            btnAddNew.Enabled = True
            lblReadOnlyInstalledOn.Visible = False
        End If

        upnlRemovalGrid.Update()
        upnlInstallationGrid.Update()
        upnlActionBtnRemoved.Update()
        '*************************
    End Sub
    Private Sub EnableLinks()
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            If Not mInstList Is Nothing Then
                If RecordsToShowForInstCompList < mInstList.Count Then
                    lnkInstCompLoadMore.Enabled = True
                    lnkInstCompLoadMoreTop.Enabled = True
                Else
                    lnkInstCompLoadMore.Enabled = False
                    lnkInstCompLoadMoreTop.Enabled = False
                End If
            End If
            If Not mRemList Is Nothing Then
                If RecordsToShowForRemCompList < mRemList.Count Then
                    lnkRemCompLoadMore.Enabled = True
                    lnkRemCompLoadMoreTop.Enabled = True
                Else
                    lnkRemCompLoadMore.Enabled = False
                    lnkRemCompLoadMoreTop.Enabled = False
                End If
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'put here the code to initialize the page

        ' 'Added by Shital on 26-Aug-2020 for All27082020
        mSpareAssemblyComponent = Request.QueryString("SpareAssemblyComponent")
        Session("mSpareAssemblyComponent") = mSpareAssemblyComponent
        '************************
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 26-Jul-2011 For All19072011
        If Not IsPostBack And Session("sender") = "" Then
            '===============By Saylee on 8th Jan 2008==============
            If cmbInstalledOnAssembly.Enabled = True Then
                cmbInstalledOnAssembly.Focus()
            End If
            '======================================================

            '************************
            Session("MiddleFrame") = "wfRemovedCompList_Ajax.aspx?SpareAssemblyComponent=" & mSpareAssemblyComponent  ' 'Added by Shital on 23-Dec-2020  mSpareAssemblyComponent

            RecordsToShowForInstCompList = dgInstalledList.PageSize
            RecordsToShowForRemCompList = dgRemovedList.PageSize
            Session("RecordsToShowForInstCompList") = RecordsToShowForInstCompList
            Session("RecordsToShowForRemCompList") = RecordsToShowForRemCompList
            DataFieldBind()
            SetPage()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If IsValid Then
            FindNow()
            SetPage()
            ControlVisibility()
            upnlInstallationGrid.Update()
            upnlRemovalGrid.Update()
            upnlActionBtn.Update()
            upnlActionBtnRemoved.Update()
            upnlActionBtnRemovedTop.Update()
        End If
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        NewRecord()
    End Sub
    Private Sub dgRemovedList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRemovedList.RowCommand
        Dim mCompStatusInfo As CompStatusInfo
        Select Case e.CommandName
            Case "InstallSelected"
                mCompStatusInfo = mCompStatusList(New Guid(dgRemovedList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
                If Not User.IsInRole("ComponentInstallationNew") Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                End If
                'GridBind(False, True)
                InstallRecord(mCompStatusInfo)
            Case "ShowVal"
                Dim mtmpRemovedCompList As tmpRemovedCompList
                mCompStatusInfo = mCompStatusList(New Guid(e.CommandArgument.ToString))
                mtmpRemovedCompList = tmpRemovedCompList.GetRemovedCompList(mCompStatusInfo.RemovedOnFormatted.ToString, mCompStatusInfo.MachineID.ToString, mCompStatusInfo.PartName, mCompStatusInfo.CompSerialNo, mCompStatusInfo.AssemblyID, CType(AppSettings("ShowForNotInUseAircrafts"), Boolean))

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
    Private Sub dgInstalledList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInstalledList.RowCommand
        Dim mCompStatusInfo As CompStatusInfo
        Select Case e.CommandName
            Case "EditRec"
                mCompStatusInfo = mCompStatusList(New Guid(e.CommandArgument.ToString))
                If (Not User.IsInRole("ComponentInstallationView") And Not User.IsInRole("ComponentInstallationEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind(True, False)
                EditRecord(mCompStatusInfo)
            Case "RevertInst"
                mCompStatusInfo = mCompStatusList(New Guid(e.CommandArgument.ToString))
                If Not User.IsInRole("ComponentInstallationDelete") Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind(True, False)
                RevertRecord(mCompStatusInfo)
            Case "History"
                mCompStatusInfo = mCompStatusList(New Guid(e.CommandArgument.ToString))
                If (Not User.IsInRole("ComponentInstallationView") And Not User.IsInRole("ComponentInstallationEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind(True, False)
                HistoryRecord(mCompStatusInfo)
            Case "ViewRec"
                mCompStatusInfo = mCompStatusList(New Guid(e.CommandArgument.ToString))
                If (Not User.IsInRole("ComponentInstallationView") And Not User.IsInRole("ComponentInstallationEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind(True, False)
                ViewImage(mCompStatusInfo.ID, mCompStatusInfo.IsAttachmentAdded)
            Case "ShowVal"
                Dim mtmpInstalledCompList As tmpInstalledCompList
                mCompStatusInfo = mCompStatusList(New Guid(e.CommandArgument.ToString))
                mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(txtInstallationDate.Text, mCompStatusInfo.MachineID.ToString, mCompStatusInfo.PartName, mCompStatusInfo.CompSerialNo, mCompStatusInfo.AssemblyID) 'cmbAircraft.SelectedValue, mCompStatusInfo.PartName, mCompStatusInfo.CompSerialNo, New Guid(cmbAssembly.SelectedValue))

                If AppSettings("IsShowAllRecordsVisible") = "True" Then
                    dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                                Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAircraftNotInUse = False And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                                Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
                Else
                    dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                                Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAircraftNotInUse = False And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                                Select StatusInfo).ToList()
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

        End Select
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        REM: Assembly Combo is updated according to the machine selected. 
        txtPart.Text = ""
        txtSerialNo.Text = ""
        'mAssemblylist = AssemblyList.GetAssemblyList(0, cmbAircraft.SelectedValue, Trim(txtInstallationDate.Text ), "(ALL)")
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue.ToString, txtInstallationDate.Text, "(All)", True)
        Session("mAssemblyList") = mAssemblylist
        cmbAssembly.DataSource = mAssemblylist
        cmbAssembly.DataBind()
        If cmbAircraft.Enabled = True Then
            cmbAircraft.Focus()
        End If

        'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraft.SelectedValue)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnly") = IsReadOnly

        If IsReadOnly = True And mSpareAssemblyComponent = 0 Then
            lblReadOnly.Visible = True
        Else
            lblReadOnly.Visible = False
        End If
        '*************************************************

        'FindNow()
        'SetPage()
        'ControlVisibility()
        'upnlInstallationGrid.Update()
        'upnlRemovalGrid.Update()
        'upnlActionBtn.Update()
        'upnlActionBtnRemoved.Update()
        'upnlActionBtnRemovedTop.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        RemoveSession()
        Session.Remove("RemoveDate")
        Session.Remove("AircraftId")
        Session.Remove("AssemblyId")
        Session.Remove("InstallOnId")
        Session.Remove("MachineID")
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtPart_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Part = txtPart.Text
        FindNow()
        SetPage()
        ControlVisibility()
        upnlInstallationGrid.Update()
        upnlRemovalGrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnRemoved.Update()
        upnlActionBtnRemovedTop.Update()
        upnlSearchCriteria.Update()
    End Sub
    Private Sub txtSerialNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        SerialNo = txtSerialNo.Text
        FindNow()
        SetPage()
        ControlVisibility()
        upnlInstallationGrid.Update()
        upnlRemovalGrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnRemoved.Update()
        upnlActionBtnRemovedTop.Update()
        upnlSearchCriteria.Update()
    End Sub
    Private Sub cmbInstalledOnAssembly_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbInstalledOnAssembly.SelectedIndexChanged
        REM: Assembly Combo is updated according to the machine selected. 
        txtPart.Text = ""
        txtSerialNo.Text = ""
        ''mInstallInAssemblylist = AssemblyList.GetAssemblyList(0, cmbInstalledOnAssembly.SelectedValue, Trim(txtInstallationDate.Text ), "(SELECT)")
        mInstallInAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbInstalledOnAssembly.SelectedValue.ToString, txtInstallationDate.Text, "(SELECT)", True)
        Session("mInstallInAssemblylist") = mInstallInAssemblylist
        cmbInstalledOnAssemblyList.DataSource = mInstallInAssemblylist
        cmbInstalledOnAssemblyList.DataBind()

        'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnlyInstalledOn = mMachineNameValueList(New Guid(cmbInstalledOnAssembly.SelectedValue)).IsReadOnly 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnlyInstalledOn") = IsReadOnlyInstalledOn
        If cmbInstalledOnAssembly.Enabled = True Then
            cmbInstalledOnAssembly.Focus()
        End If

        SetGrid()
        '***********************************************
        HighlightSpareAssembly() 'Added by Shital on 28-Dec-2020 for All27072020
    End Sub
    'New addition by Rupali on 22-Jun-09 for Sorting Order
    Private Sub dgInstalledList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgInstalledList.Sorting
        mInstList = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAircraftNotInUse = False And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                        Order By e.SortExpression
                                        Select StatusInfo).ToList()
        Session("mInstList") = mInstList
        'Added By Vikrant For Showing First 5 records
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            dgInstalledList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
                                        Where StatusInfo.IsInstalledRemoved = "Installed" And StatusInfo.IsAircraftNotInUse = False And StatusInfo.IsAssemblyInstalledRemoved = "Installed Assembly"
                                        Order By e.SortExpression
                                        Select StatusInfo).ToList.Take(RecordsToShowForInstCompList)
        Else
            dgInstalledList.DataSource = mInstList
        End If

        dgInstalledList.DataBind()
        HighlightSpareAssembly() 'Added by Shital on 28-Dec-2020 for All27072020
    End Sub
    'New addition by Rupali on 22-Jun-09 for Sorting Order
    Private Sub dgRemovedList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRemovedList.Sorting
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
        HighlightSpareAssembly() 'Added by Shital on 28-Dec-2020 for All27072020
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub txtInstallationDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtInstallationDate.TextChanged
        txtPart.Text = ""
        txtSerialNo.Text = ""
        'If Condition Added by Shital on 28-Dec-2020
        If mSpareAssemblyComponent = 0 Then
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue.ToString, txtInstallationDate.Text, "(All)", True)
        Else
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtInstallationDate.Text.ToString, "(All)", True, IsForSpareAssembly:=True) '  ' mIsSpareAssembly Added by Saylee on 26-Aug-2020 for All27072020
        End If


        Session("mAssemblyList") = mAssemblylist
        cmbAssembly.DataSource = mAssemblylist
        cmbAssembly.DataBind()
        If cmbAircraft.Enabled = True Then
            cmbAircraft.Focus()
        End If

        HighlightSpareAssembly() 'Added by Shital on 28-Dec-2020 for All27072020

        'FindNow()
        'SetPage()
        'ControlVisibility()
        'upnlInstallationGrid.Update()
        'upnlRemovalGrid.Update()
        'upnlActionBtn.Update()
        'upnlActionBtnRemoved.Update()
        'upnlActionBtnRemovedTop.Update()
        'upnlSearchCriteria.Update()
    End Sub
    Private Sub cmbAssembly_SelectedIndexChanged(sender As Object, e As System.EventArgs)
        REM: Assembly Combo is updated according to the machine selected. 
        txtPart.Text = ""
        txtSerialNo.Text = ""
        FindNow()
        SetPage()
        ControlVisibility()
        upnlInstallationGrid.Update()
        upnlRemovalGrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnRemoved.Update()
        upnlActionBtnRemovedTop.Update()
        upnlSearchCriteria.Update()
    End Sub
    Private Sub lnkInstCompLoadMore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkInstCompLoadMoreTop.Click, lnkInstCompLoadMore.Click
        RecordsToShowForInstCompList = mInstList.Count
        Session("RecordsToShowForInstCompList") = RecordsToShowForInstCompList
        'Dim list = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        dgInstalledList.DataSource = mInstList
        dgInstalledList.DataBind()
        lnkInstCompLoadMoreTop.Enabled = False
        lnkInstCompLoadMore.Enabled = False
        SetPage()
        ControlVisibility()
        upnlActionBtn.Update()
    End Sub
    Private Sub lnkRemCompLoadMore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRemCompLoadMore.Click, lnkRemCompLoadMoreTop.Click
        RecordsToShowForRemCompList = mRemList.Count
        Session("RecordsToShowForRemCompList") = RecordsToShowForRemCompList
        dgRemovedList.DataSource = mRemList
        dgRemovedList.DataBind()
        'VlnkRemCompLoadMore.Enabled = False
        'VlnkRemCompLoadMoreTop.Enabled = False
        SetPage()
        ControlVisibility()
        upnlActionBtnRemoved.Update()
        upnlActionBtnRemovedTop.Update()
    End Sub
    Protected Sub dgInstalledList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgInstalledList.Columns(i).HeaderText
            Next
        End If
    End Sub
    Protected Sub dgRemovedList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgRemovedList.Columns(i).HeaderText
            Next
        End If
    End Sub
    'Added By Vikrant On 27-Jul-2020 For ALL27072020
    Private Sub lnkSpareComponent_Click(sender As Object, e As System.EventArgs) Handles lnkSpareComponent.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSpareCompInstallListWindow", "OpenSpareCompInstallListWindow();", True)
    End Sub
    'End
#End Region

#Region " Report "

#Region "Report Variable Declaration"
    Dim mCompanyDetail As New CompanyDetail
    Private SearchStr1 As String = ""
    Private SearchStr2 As String = ""
    Private SearchStr3 As String = ""
    Private SearchStr4 As String = ""
    Dim Part As String
    Dim SerialNo As String
#End Region

#Region " Events "
    Private Sub btnPrintRemoved_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintRemoved.Click, btnPrintRemovedTop.Click
        If (Not User.IsInRole("ComponentInstallationPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'dgRemovedList.DataSource = mRemList
        'dgRemovedList.DataBind()
        Dim Rpt As New crListInstalledRemovedComp
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

        ReportDetails.Add(New rptStatus(, 0, "Removal Information", "Install Date", txtInstallationDate.Text, , , , , , , , , , , , , , , , , , "Installed On", cmbInstalledOnAssembly.SelectedItem.Text))

        'ReportDetails.Add(New rptStatus(, 1, , , , dgRemovedList.CaptionText))

        ReportDetails.Add(New rptStatus(, 2, , _
              , , , dgRemovedList.Columns.Item(1).HeaderText, , dgRemovedList.Columns.Item(2).HeaderText, dgRemovedList.Columns.Item(3).HeaderText, _
              dgRemovedList.Columns.Item(4).HeaderText, dgRemovedList.Columns.Item(5).HeaderText, dgRemovedList.Columns.Item(6).HeaderText, dgRemovedList.Columns.Item(7).HeaderText, , , , , , , , , , , , dgRemovedList.Columns.Item(8).HeaderText))

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
   mCompanyDetail.WebSite, "List of Removed Component Report", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"))

        If mRemList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)

        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnPrintInstalled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintInstalled.Click

        If (Not User.IsInRole("ComponentInstallationPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'VdgInstalledList.DataSource = mInstList
        'VdgInstalledList.DataBind()
        Dim Rpt As New crListInstalledRemovedComp
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

        ReportDetails.Add(New rptStatus(, 0, "Installation Information", "Install Date", txtInstallationDate.Text, , , , , , , , , , , , , , , , , , "Installed On", cmbInstalledOnAssembly.SelectedItem.Text))           'ReportDetails.Add(New rptStatus(, 1, , , , dgInstalledList.CaptionText))

        ReportDetails.Add(New rptStatus(, 2, , _
              , , , dgInstalledList.Columns.Item(1).HeaderText, , dgInstalledList.Columns.Item(2).HeaderText, dgInstalledList.Columns.Item(3).HeaderText, _
              dgInstalledList.Columns.Item(4).HeaderText, dgInstalledList.Columns.Item(5).HeaderText, dgInstalledList.Columns.Item(6).HeaderText, dgInstalledList.Columns.Item(7).HeaderText, , , , , , , , , , , , dgInstalledList.Columns.Item(8).HeaderText, , RHData3:=dgInstalledList.Columns.Item(9).HeaderText))

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
            TSNLabel = CType(Me.dgInstalledList.Rows(I).FindControl("lblTSNValues"), Label)
            TSOLabel = CType(Me.dgInstalledList.Rows(I).FindControl("lblTSOValues"), Label)


            If Me.dgInstalledList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgInstalledList.Rows(I).Cells.Item(1).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgInstalledList.Rows(I).Cells.Item(2).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgInstalledList.Rows(I).Cells.Item(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgInstalledList.Rows(I).Cells.Item(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgInstalledList.Rows(I).Cells.Item(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgInstalledList.Rows(I).Cells.Item(6).Text.Replace("<BR>", vbCrLf)
            If InstLabel.Text <> "&nbsp;" Then str(6) = InstLabel.Text.Replace("<BR>", vbCrLf)
            If TSNLabel.Text <> "&nbsp;" Then str(7) = TSNLabel.Text.Replace("<BR>", vbCrLf)
            If TSOLabel.Text <> "&nbsp;" Then str(8) = TSOLabel.Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 3, , , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6), , , , , , , str(7), , str(8)))
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "List of Installed Component Report", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mInstList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)

        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region

    
  
   
End Class