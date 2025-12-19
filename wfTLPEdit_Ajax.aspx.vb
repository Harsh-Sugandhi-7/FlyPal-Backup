'Created By Utkarsh On 30-Mar-2012

Imports System.Configuration.ConfigurationManager
Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services
Imports InfoSoftGlobal
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Services
Imports System.Text
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Imports System
Imports System.IO
Partial Class wfTLPEdit_Ajax
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Variable Declaration"
    Public mLog As Log
    Public mMachine As Machine
    Public mFlightLogClassificationList As FlightLogClassificationList
    Private Flag As Int16
    Dim Type As Integer
    Private LogListCount As Integer = 0
    Dim EventLogID As Guid
    Dim mLogDetail As String

    Public mSearchListPilot As SearchList
    Dim TakeOffTouchDown As Boolean
    Dim Pilot1ID As Guid
    Dim Pilot2ID As Guid
    Dim SourceID As Guid
    Dim DestinationID As Guid
    Dim SetValue As Boolean = False
    Dim IsValueZero As Boolean = False
    Public Event TextChanged As EventHandler

    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False

    Public mLogListOnDate As LogList 'Added by Saylee on 11-Apr-2016
    Public mIsLastLog As Boolean 'Added By Saylee on 31-Aug-2016 
    Dim mCompanyDetail As New CompanyDetail 'PBH Collective Hrs by Saylee on 30-Nov-2022
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mLog = CType(Session("mLog"), Log)
        mMachine = CType(Session("mMachine"), Machine)
        mFlightLogClassificationList = CType(Session("mFlightLogClassificationList"), FlightLogClassificationList)
        LogListCount = CType(Session("LogListCount"), Integer)
        mSearchListPilot = Session("mSearchListPilot")
        Pilot1ID = CType(Session("Pilot1ID"), Guid)
        Pilot2ID = CType(Session("Pilot2ID"), Guid)
        SourceID = CType(Session("SourceID"), Guid)
        DestinationID = CType(Session("DestinationID"), Guid)
        SetValue = CType(Session("SetValue"), Boolean)

        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mIsLastLog = CType(Session("mIsLastLog"), Boolean)
        mLogListOnDate = Session("mLogListOnDate")
        mCompanyDetail = Session("mCompanyDetail") 'PBH Collective Hrs by Saylee on 30-Nov-2022
    End Sub
    Private Sub SetSession()
        Session("mLog") = mLog
        Session("mMachine") = mMachine
        Session("mFlightLogClassificationList") = mFlightLogClassificationList
        Session("LogListCount") = LogListCount

        Session("mSearchListPilot") = mSearchListPilot

        Session("Pilot1ID") = Pilot1ID
        Session("Pilot2ID") = Pilot2ID
        Session("SourceID") = SourceID
        Session("DestinationID") = DestinationID
        Session("SetValue") = SetValue
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        Session("mIsLastLog") = mIsLastLog
        Session("mCompanyDetail") = mCompanyDetail 'PBH Collective Hrs by Saylee on 30-Nov-2022
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachine")

        Session.Remove("mLog")
        Session.Remove("LogListCount")
        Session.Remove("mSearchListPlace")
        Session.Remove("mSearchListPilot")

        Session.Remove("Pilot1ID")
        Session.Remove("Pilot2ID")
        Session.Remove("SourceID")
        Session.Remove("DestinationID")
        Session.Remove("SetValue")
        Session.Remove("ID")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("mIsLastLog")
        Session.Remove("mCompanyDetail") 'PBH Collective Hrs by Saylee on 30-Nov-2022
    End Sub
    Private Sub SetFromSearch()
        Dim Type As Short = Val(Request.QueryString("Type"))
        Dim Id As String = Request.QueryString("Id")
        Dim Name As String = Request.QueryString("Name")
        Dim AddType As Short = Val(Request.QueryString("AddType"))

        'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
        If AppSettings("ClientCode") = "Heligo" Or
           AppSettings("ClientCode") = "UHPL" Or
           AppSettings("ClientCode") = "APFT" Or
           AppSettings("ClientCode") = "AAP" Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
            mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
        End If
        If Type = -1 Then
            Select Case AddType
                Case 0
                    'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
                    If AppSettings("ClientCode") = "Heligo" Or
                       AppSettings("ClientCode") = "UHPL" Or
                       AppSettings("ClientCode") = "APFT" Or
                       AppSettings("ClientCode") = "AAP" Then  'ClientCode APFT added on 25-Jan-2018 For APFT25012018
                        mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
                    Else
                        mLog.PilotID1 = New Guid(Id)
                    End If
                Case 1
                    mLog.PilotID2 = New Guid(Id)
                Case 2
                    mLog.DestinationID = New Guid(Id)
                Case 3
                    mLog.SourceID = New Guid(Id)
            End Select
        End If
        Session("mLog") = mLog
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Try
            Dim str As String
            'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)

            ' '' ''AJAX-Date Control is present in Update Panel so ClientScript is not working/fired. Instead of it use ScriptManager
            str = "try{document.getElementById('" + cntrl.ClientID + "').focus();}catch (Error) {}"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
        Catch ex As Exception
            '
        End Try

    End Sub
    Private Sub EnableDisableButton()

        '  btnDefectActionList.Enabled = Not mLog.IsNew
        calDateTime.Enabled = IIf(mLog.IsNew And mLog.LogDetails.Count = 0, True, False)

        ' btnFuelOil.Enabled = Not mLog.IsNew         'Added by Saylee on 6-Sep-2012
        ' btnFlightCrew.Enabled = Not mLog.IsNew      'Added by Saylee on 6-Sep-2012
        'btnMaintenanceAcitvity.Enabled = Not mLog.IsNew

        If Not mLog.IsNew Then
            txtAirBorneTime.BackColor = Color.Gainsboro
            txtGroundRunTime.BackColor = Color.Gainsboro
            txtPercentTimeOnGround.BackColor = Color.Gainsboro
            txtPrevHobbsValue.BackColor = Color.Gainsboro
            txtPrevHobbsOffset.BackColor = Color.Gainsboro
            txtCurrentHobbsOffset.BackColor = Color.Gainsboro
            txtCurrentHobbsValue.BackColor = Color.Gainsboro
            txtTotalTime.BackColor = Color.Gainsboro
            txtTotalLandings.BackColor = Color.Gainsboro
        End If


        If Not mLog.IsNew Then

            'Pilot1.Enabled = False
            'Pilot2.Enabled = False
            'Pilot1.ReadOnly = True
            'Pilot2.ReadOnly = True

            Place1.Enabled = False
            Place2.Enabled = False
            Place1.ReadOnly = True
            Place2.ReadOnly = True

            'Pilot1.BackColor = Color.Gainsboro
            'Pilot2.BackColor = Color.Gainsboro

            Place1.BackColor = Color.Gainsboro
            Place2.BackColor = Color.Gainsboro

        Else
            Pilot1.Enabled = True
            Pilot2.Enabled = True
            Pilot1.ReadOnly = False
            Pilot2.ReadOnly = False

            'Place1.Enabled = True
            'Place2.Enabled = True
            'Place1.ReadOnly = False
            'Place2.ReadOnly = False

            Pilot1.BackColor = Color.White
            Pilot2.BackColor = Color.White
            'Place1.BackColor = Color.White
            'Place2.BackColor = Color.White
        End If

        'End


        pnlHours.Visible = True '= Not (mMachine.HourType = 2) 'Added Code
        pnlDecimal.Visible = (mMachine.HourType = 2)

        '================Visibility for Hours and Decimal===================
        '*pnlHours   

        lblAirBorneTime.Visible = True
        txtAirBorneTime.Visible = True
        txtBlockTime.Visible = True
        lblGroundRunTime.Visible = True
        txtGroundRunTime.Visible = True
        lblPercentTimeOnGround.Visible = False
        txtPercentTimeOnGround.Visible = False
        lblTotalLandings.Visible = True
        txtTotalLandings.Visible = True

        'pnlDecimal
        lblHobbsPrevVal.Visible = (mMachine.HourType = 2)
        txtPrevHobbsValue.Visible = (mMachine.HourType = 2)
        lblOffsetPreVal.Visible = (mMachine.HourType = 2)
        txtPrevHobbsOffset.Visible = (mMachine.HourType = 2)
        lblOffsetCurrentVal.Visible = (mMachine.HourType = 2)
        txtCurrentHobbsOffset.Visible = (mMachine.HourType = 2)
        lblHobbsCurrentReading.Visible = (mMachine.HourType = 2)
        txtCurrentHobbsValue.Visible = (mMachine.HourType = 2)

        '===========ReadOnly for Hours and Decimal=============
        lblTotalBlockTime.Visible = (mMachine.HourType = 1)
        txtBlockTime.Visible = (mMachine.HourType = 1)
        lblAirBorneTime.Visible = (mMachine.HourType = 1)
        txtAirBorneTime.Visible = (mMachine.HourType = 1)
        lblGroundRunTime.Visible = (mMachine.HourType = 1)
        txtGroundRunTime.Visible = (mMachine.HourType = 1)
        lblPercentTimeOnGround.Visible = False
        txtPercentTimeOnGround.Visible = False

        If TakeOffTouchDown Then
            txtAirBorneTime.BackColor = Color.Gainsboro
            txtGroundRunTime.BackColor = Color.Gainsboro
            txtAirBorneTime.ReadOnly = True
            txtGroundRunTime.ReadOnly = True
            txtTotalLandings.ReadOnly = True
            txtTotalLandings.BackColor = Color.Gainsboro
        End If

        'pnlHours
        If Not TakeOffTouchDown Then
            txtAirBorneTime.ReadOnly = Not mLog.IsNew
        End If
        txtCurrentHobbsValue.ReadOnly = Not mLog.IsNew

        mIsLastLog = IIf((MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID, True).LogID).Equals(mLog.ID), True, False)
        btnAddRoute.Enabled = IIf(mIsLastLog = True Or mLog.IsNew, True, False) 'IIf(mLog.IsNew, True, False)
        'Added by Saylee on 28-Mar-2014 For BA28032014
        lblClassificationStar.Visible = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "YA", True, False)
        'End 
        ' '' ''AJAX- To reflect changes of controls we have call ".Update()" method of respective Panel

        If mCompanyDetail.IsSyncApplication Then
            btnAddPilot.Visible = False

        End If
        upnlTabs.Update()
        upnlFlightSummary.Update()
    End Sub
    Private Sub ControlVisibility()
        'Airframe ----> 
        'Hours
        dgAFPeriods.Columns(3).Visible = mLog.LogAFAssemblies.ShowHours
        dgAFPeriods.Columns(4).Visible = mLog.LogAFAssemblies.ShowHours

        'Landings
        dgAFPeriods.Columns(5).Visible = mLog.LogAFAssemblies.ShowLandings
        dgAFPeriods.Columns(6).Visible = mLog.LogAFAssemblies.ShowLandings
        'Cycles
        dgAFPeriods.Columns(7).Visible = mLog.LogAFAssemblies.ShowCycles
        dgAFPeriods.Columns(8).Visible = mLog.LogAFAssemblies.ShowCycles
        'Starts
        dgAFPeriods.Columns(9).Visible = mLog.LogAFAssemblies.ShowStarts
        dgAFPeriods.Columns(10).Visible = mLog.LogAFAssemblies.ShowStarts
        'NG
        dgAFPeriods.Columns(11).Visible = mLog.LogAFAssemblies.ShowNGCycles
        dgAFPeriods.Columns(12).Visible = mLog.LogAFAssemblies.ShowNGCycles
        'NF
        dgAFPeriods.Columns(13).Visible = mLog.LogAFAssemblies.ShowNFCycles
        dgAFPeriods.Columns(14).Visible = mLog.LogAFAssemblies.ShowNFCycles
        'RINS
        dgAFPeriods.Columns(15).Visible = mLog.LogAFAssemblies.ShowRINS
        dgAFPeriods.Columns(16).Visible = mLog.LogAFAssemblies.ShowRINS
        'Bleeds  'Added By Prashant 8-July-2009
        dgAFPeriods.Columns(17).Visible = mLog.LogAFAssemblies.ShowBleeds
        dgAFPeriods.Columns(18).Visible = mLog.LogAFAssemblies.ShowBleeds
        'ImpellerCycles  'Added By Prashant 10-Aug-2009
        dgAFPeriods.Columns(19).Visible = mLog.LogAFAssemblies.ShowImpellerCycles
        dgAFPeriods.Columns(20).Visible = mLog.LogAFAssemblies.ShowImpellerCycles
        'CTCycles
        dgAFPeriods.Columns(21).Visible = mLog.LogAFAssemblies.ShowCTCycles
        dgAFPeriods.Columns(22).Visible = mLog.LogAFAssemblies.ShowCTCycles
        'PTCycles
        dgAFPeriods.Columns(23).Visible = mLog.LogAFAssemblies.ShowPTCycles
        dgAFPeriods.Columns(24).Visible = mLog.LogAFAssemblies.ShowPTCycles
        '--------------------------------------
        'Added by Shweta on 8-May-2012 for ALL02052012
        'Generator Mods
        dgAFPeriods.Columns(25).Visible = mLog.LogAFAssemblies.ShowGeneratorMods
        dgAFPeriods.Columns(26).Visible = mLog.LogAFAssemblies.ShowGeneratorMods
        '--------------------------------------
        'Engine ----> 
        'Hours
        dgEnginePeriods.Columns(3).Visible = mLog.LogEngAssemblies.ShowHours
        dgEnginePeriods.Columns(4).Visible = mLog.LogEngAssemblies.ShowHours
        'Landings
        dgEnginePeriods.Columns(5).Visible = mLog.LogEngAssemblies.ShowLandings
        dgEnginePeriods.Columns(6).Visible = mLog.LogEngAssemblies.ShowLandings
        'Cycles
        dgEnginePeriods.Columns(7).Visible = mLog.LogEngAssemblies.ShowCycles
        dgEnginePeriods.Columns(8).Visible = mLog.LogEngAssemblies.ShowCycles
        'Starts
        dgEnginePeriods.Columns(9).Visible = mLog.LogEngAssemblies.ShowStarts
        dgEnginePeriods.Columns(10).Visible = mLog.LogEngAssemblies.ShowStarts
        'NG
        dgEnginePeriods.Columns(11).Visible = mLog.LogEngAssemblies.ShowNGCycles
        dgEnginePeriods.Columns(12).Visible = mLog.LogEngAssemblies.ShowNGCycles
        'NF
        dgEnginePeriods.Columns(13).Visible = mLog.LogEngAssemblies.ShowNFCycles
        dgEnginePeriods.Columns(14).Visible = mLog.LogEngAssemblies.ShowNFCycles
        'RINS
        dgEnginePeriods.Columns(15).Visible = mLog.LogEngAssemblies.ShowRINS
        dgEnginePeriods.Columns(16).Visible = mLog.LogEngAssemblies.ShowRINS
        'RINS
        dgEnginePeriods.Columns(17).Visible = mLog.LogEngAssemblies.ShowCFactor
        dgEnginePeriods.Columns(18).Visible = mLog.LogEngAssemblies.ShowCFactor
        'Bleeds  'Added By Prashant 8-July-2009
        dgEnginePeriods.Columns(19).Visible = mLog.LogEngAssemblies.ShowBleeds
        dgEnginePeriods.Columns(20).Visible = mLog.LogEngAssemblies.ShowBleeds
        'ImpellerCycles  'Added By Prashant 10-Aug-2009
        dgEnginePeriods.Columns(21).Visible = mLog.LogEngAssemblies.ShowImpellerCycles
        dgEnginePeriods.Columns(22).Visible = mLog.LogEngAssemblies.ShowImpellerCycles
        'CTCycles  
        dgEnginePeriods.Columns(23).Visible = mLog.LogEngAssemblies.ShowCTCycles
        dgEnginePeriods.Columns(24).Visible = mLog.LogEngAssemblies.ShowCTCycles
        'PTCycles  
        dgEnginePeriods.Columns(25).Visible = mLog.LogEngAssemblies.ShowPTCycles
        dgEnginePeriods.Columns(26).Visible = mLog.LogEngAssemblies.ShowPTCycles
        '--------------------------------------
        'Added by Shweta on 8-May-2012 for ALL02052012
        'Generator Mods
        dgEnginePeriods.Columns(27).Visible = mLog.LogEngAssemblies.ShowGeneratorMods
        dgEnginePeriods.Columns(28).Visible = mLog.LogEngAssemblies.ShowGeneratorMods

        'Rapid TakeOff Factor  'Code added for Rapid TakeOff on 31-Oct-2022 by Saylee
        dgEnginePeriods.Columns(29).Visible = mLog.LogEngAssemblies.ShowRapidTakeOffFactors
        dgEnginePeriods.Columns(30).Visible = mLog.LogEngAssemblies.ShowRapidTakeOffFactors


        'APU ----> 
        If mLog.LogAPUAssemblies.Count = 0 Then
            dgAPUPeriods.Visible = False
            lblAPUPeriod.Visible = False
        Else 'Added By utkarsh on 30-sep-2013 for log_ajax changes
            dgAPUPeriods.Visible = True
            lblAPUPeriod.Visible = True
        End If
        'Hours
        dgAPUPeriods.Columns(3).Visible = mLog.LogAPUAssemblies.ShowHours
        dgAPUPeriods.Columns(4).Visible = mLog.LogAPUAssemblies.ShowHours
        'Landings
        dgAPUPeriods.Columns(5).Visible = mLog.LogAPUAssemblies.ShowLandings
        dgAPUPeriods.Columns(6).Visible = mLog.LogAPUAssemblies.ShowLandings
        'Cycles
        dgAPUPeriods.Columns(7).Visible = mLog.LogAPUAssemblies.ShowCycles
        dgAPUPeriods.Columns(8).Visible = mLog.LogAPUAssemblies.ShowCycles
        'Starts
        dgAPUPeriods.Columns(9).Visible = mLog.LogAPUAssemblies.ShowStarts
        dgAPUPeriods.Columns(10).Visible = mLog.LogAPUAssemblies.ShowStarts
        'NG
        dgAPUPeriods.Columns(11).Visible = mLog.LogAPUAssemblies.ShowNGCycles
        dgAPUPeriods.Columns(12).Visible = mLog.LogAPUAssemblies.ShowNGCycles
        'NF
        dgAPUPeriods.Columns(13).Visible = mLog.LogAPUAssemblies.ShowNFCycles
        dgAPUPeriods.Columns(14).Visible = mLog.LogAPUAssemblies.ShowNFCycles
        'RINS
        dgAPUPeriods.Columns(15).Visible = mLog.LogAPUAssemblies.ShowRINS
        dgAPUPeriods.Columns(16).Visible = mLog.LogAPUAssemblies.ShowRINS
        'Bleeds  'Added By Prashant 8-July-2009
        dgAPUPeriods.Columns(17).Visible = mLog.LogAPUAssemblies.ShowBleeds
        dgAPUPeriods.Columns(18).Visible = mLog.LogAPUAssemblies.ShowBleeds
        'ImpellerCycles
        dgAPUPeriods.Columns(19).Visible = mLog.LogAPUAssemblies.ShowImpellerCycles
        dgAPUPeriods.Columns(20).Visible = mLog.LogAPUAssemblies.ShowImpellerCycles
        'CTCycles 
        dgAPUPeriods.Columns(21).Visible = mLog.LogAPUAssemblies.ShowCTCycles
        dgAPUPeriods.Columns(22).Visible = mLog.LogAPUAssemblies.ShowCTCycles
        'PTCycles  
        dgAPUPeriods.Columns(23).Visible = mLog.LogAPUAssemblies.ShowPTCycles
        dgAPUPeriods.Columns(24).Visible = mLog.LogAPUAssemblies.ShowPTCycles

        'Added by Shweta on 8-May-2012 for ALL02052012
        'Generator Mods
        dgAPUPeriods.Columns(25).Visible = mLog.LogAPUAssemblies.ShowGeneratorMods
        dgAPUPeriods.Columns(26).Visible = mLog.LogAPUAssemblies.ShowGeneratorMods
        '-----------------
        'CGB ----> 
        If mLog.LogCGBAssemblies.Count = 0 Then
            dgCGBPeriods.Visible = False
            lblCGBPeriod.Visible = False
        Else 'Added By utkarsh on 30-sep-2013 for log_ajax changes
            dgCGBPeriods.Visible = True
            lblCGBPeriod.Visible = True
        End If
        'Hours
        dgCGBPeriods.Columns(3).Visible = mLog.LogCGBAssemblies.ShowHours
        dgCGBPeriods.Columns(4).Visible = mLog.LogCGBAssemblies.ShowHours
        'Landings
        dgCGBPeriods.Columns(5).Visible = mLog.LogCGBAssemblies.ShowLandings
        dgCGBPeriods.Columns(6).Visible = mLog.LogCGBAssemblies.ShowLandings
        'Cycles
        dgCGBPeriods.Columns(7).Visible = mLog.LogCGBAssemblies.ShowCycles
        dgCGBPeriods.Columns(8).Visible = mLog.LogCGBAssemblies.ShowCycles
        'Starts
        dgCGBPeriods.Columns(9).Visible = mLog.LogCGBAssemblies.ShowStarts
        dgCGBPeriods.Columns(10).Visible = mLog.LogCGBAssemblies.ShowStarts
        'NG
        dgCGBPeriods.Columns(11).Visible = mLog.LogCGBAssemblies.ShowNGCycles
        dgCGBPeriods.Columns(12).Visible = mLog.LogCGBAssemblies.ShowNGCycles
        'NF
        dgCGBPeriods.Columns(13).Visible = mLog.LogCGBAssemblies.ShowNFCycles
        dgCGBPeriods.Columns(14).Visible = mLog.LogCGBAssemblies.ShowNFCycles
        'RINS
        dgCGBPeriods.Columns(15).Visible = mLog.LogCGBAssemblies.ShowRINS
        dgCGBPeriods.Columns(16).Visible = mLog.LogCGBAssemblies.ShowRINS
        'Bleeds  'Added By Prashant 8-July-2009
        dgCGBPeriods.Columns(17).Visible = mLog.LogCGBAssemblies.ShowBleeds
        dgCGBPeriods.Columns(18).Visible = mLog.LogCGBAssemblies.ShowBleeds
        'ImpellerCycles  'Added By Prashant 10-Aug-2009
        dgCGBPeriods.Columns(19).Visible = mLog.LogCGBAssemblies.ShowImpellerCycles
        dgCGBPeriods.Columns(20).Visible = mLog.LogCGBAssemblies.ShowImpellerCycles
        'CTCycles  
        dgCGBPeriods.Columns(21).Visible = mLog.LogCGBAssemblies.ShowCTCycles
        dgCGBPeriods.Columns(22).Visible = mLog.LogCGBAssemblies.ShowCTCycles
        'PTCycles  
        dgCGBPeriods.Columns(23).Visible = mLog.LogCGBAssemblies.ShowPTCycles
        dgCGBPeriods.Columns(24).Visible = mLog.LogCGBAssemblies.ShowPTCycles

        'Added by Shweta on 8-May-2012 for ALL02052012
        'Generator Mods
        dgCGBPeriods.Columns(25).Visible = mLog.LogCGBAssemblies.ShowGeneratorMods
        dgCGBPeriods.Columns(26).Visible = mLog.LogCGBAssemblies.ShowGeneratorMods
        '---------------------------
        If mLog.LogDetails.Count = 0 Then
            lblTLPGridTitle.Visible = False
            dgLogDetails.Visible = False
        Else
            lblTLPGridTitle.Visible = True
            dgLogDetails.Visible = True
        End If
        'dgLogDetails.Columns(23).Visible = (mLog.LogDetails.Count > 1) 'Added on 22-Mar-2022
        ControlVisibilityForAttachment()

        '=====================================
        ''Added by Saylee on 1-Mar-2022
        'ALL Assembly ----> 

        'Hours
        grdAllAssemblies.Columns(4).Visible = mLog.ALL_LogAssemblies.ShowHours
        grdAllAssemblies.Columns(5).Visible = mLog.ALL_LogAssemblies.ShowHours

        'Landings
        grdAllAssemblies.Columns(6).Visible = mLog.ALL_LogAssemblies.ShowLandings
        grdAllAssemblies.Columns(7).Visible = mLog.ALL_LogAssemblies.ShowLandings
        'Cycles
        grdAllAssemblies.Columns(8).Visible = mLog.ALL_LogAssemblies.ShowCycles
        grdAllAssemblies.Columns(9).Visible = mLog.ALL_LogAssemblies.ShowCycles
        'Starts
        grdAllAssemblies.Columns(10).Visible = mLog.ALL_LogAssemblies.ShowStarts
        grdAllAssemblies.Columns(11).Visible = mLog.ALL_LogAssemblies.ShowStarts
        'NG
        grdAllAssemblies.Columns(12).Visible = mLog.ALL_LogAssemblies.ShowNGCycles
        grdAllAssemblies.Columns(13).Visible = mLog.ALL_LogAssemblies.ShowNGCycles
        'NF
        grdAllAssemblies.Columns(14).Visible = mLog.ALL_LogAssemblies.ShowNFCycles
        grdAllAssemblies.Columns(15).Visible = mLog.ALL_LogAssemblies.ShowNFCycles
        'RINS
        grdAllAssemblies.Columns(16).Visible = mLog.ALL_LogAssemblies.ShowRINS
        grdAllAssemblies.Columns(17).Visible = mLog.ALL_LogAssemblies.ShowRINS
        'Bleeds  'Added By Prashant 8-July-2009
        grdAllAssemblies.Columns(18).Visible = mLog.ALL_LogAssemblies.ShowBleeds
        grdAllAssemblies.Columns(19).Visible = mLog.ALL_LogAssemblies.ShowBleeds
        'ImpellerCycles  'Added By Prashant 10-Aug-2009
        grdAllAssemblies.Columns(20).Visible = mLog.ALL_LogAssemblies.ShowImpellerCycles
        grdAllAssemblies.Columns(21).Visible = mLog.ALL_LogAssemblies.ShowImpellerCycles
        'CTCycles
        grdAllAssemblies.Columns(22).Visible = mLog.ALL_LogAssemblies.ShowCTCycles
        grdAllAssemblies.Columns(23).Visible = mLog.ALL_LogAssemblies.ShowCTCycles
        'PTCycles
        grdAllAssemblies.Columns(24).Visible = mLog.ALL_LogAssemblies.ShowPTCycles
        grdAllAssemblies.Columns(25).Visible = mLog.ALL_LogAssemblies.ShowPTCycles
        '--------------------------------------
        'Added by Shweta on 7-May-2012 for ALL02052012
        'Generator Mods
        grdAllAssemblies.Columns(26).Visible = mLog.ALL_LogAssemblies.ShowGeneratorMods
        grdAllAssemblies.Columns(27).Visible = mLog.ALL_LogAssemblies.ShowGeneratorMods




        ' '' ''AJAX- To reflect changes of controls we have call ".Update()" method of respective Panel
        upnlLogDetailsGrid.Update()

        upnlAirframeDetail.Update()
        upnlEngineDetail.Update()
        upnlAPUDetail.Update()
        upnlCGBDetail.Update()
        upnlAssemblyInfo.Update()
    End Sub
    Public Function IsZeroValueLog(Optional ByVal isFromDataBindGrid As Boolean = False) As Boolean

        Dim isZero As Boolean = False
        Dim flag As Boolean = False
        If Val(mLog.TotalTime) = 0 Then

        End If
        For i As Integer = 0 To mLog.LogAFAssemblies.Count - 1
            If mLog.LogAFAssemblies.ShowHours Then
                If mLog.IsHobbs Then
                    If Val(mLog.LogAFAssemblies(i).Hours) = 0 Then
                        flag = True
                        Exit For
                    End If
                Else
                    If mLog.LogAFAssemblies(i).Hours = "0:00" Then
                        flag = True
                        Exit For
                    End If
                End If
            End If
            If mLog.LogAFAssemblies.ShowLandings Then
                If Val(mLog.LogAFAssemblies(i).Landings) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogAFAssemblies.ShowCycles Then
                If Val(mLog.LogAFAssemblies(i).Cycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogAFAssemblies.ShowStarts Then
                If Val(mLog.LogAFAssemblies(i).Starts) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogAFAssemblies.ShowNGCycles Then
                If Val(mLog.LogAFAssemblies(i).NGCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogAFAssemblies.ShowNFCycles Then
                If Val(mLog.LogAFAssemblies(i).NFCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogAFAssemblies.ShowRINS Then
                If Val(mLog.LogAFAssemblies(i).RINS) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogAFAssemblies.ShowBleeds Then
                If Val(mLog.LogAFAssemblies(i).Bleeds) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogAFAssemblies.ShowImpellerCycles Then
                If Val(mLog.LogAFAssemblies(i).ImpellerCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogAFAssemblies.ShowCTCycles Then
                If Val(mLog.LogAFAssemblies(i).CTCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogAFAssemblies.ShowPTCycles Then
                If Val(mLog.LogAFAssemblies(i).PTCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            'Added by Shweta on 7-May-2012 for ALL02052012
            If mLog.LogAFAssemblies.ShowGeneratorMods Then
                If Val(mLog.LogAFAssemblies(i).GeneratorMods) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            '----------
        Next
        If flag = True Then
            flag = False
            isZero = True
        End If

        For i As Integer = 0 To mLog.LogAPUAssemblies.Count - 1
            If mLog.LogAPUAssemblies(i).ShowHours Then
                If mLog.IsHobbs Then
                    If Val(mLog.LogAPUAssemblies(i).Hours) = 0 Then
                        flag = True
                        Exit For
                    End If
                Else
                    If mLog.LogAPUAssemblies(i).Hours = "0:00" Then
                        flag = True
                        Exit For
                    End If
                End If
            End If
            If mLog.LogAPUAssemblies(i).ShowImpellerCycles Then
                If (Val(mLog.LogAPUAssemblies(i).ImpellerCycles) = 0) Then
                    flag = True
                    Exit For
                End If
            End If

            If mLog.LogAPUAssemblies(i).ShowLandings Then
                If (Val(mLog.LogAPUAssemblies(i).Landings) = 0) Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogAPUAssemblies(i).ShowNFCycles Then
                If (Val(mLog.LogAPUAssemblies(i).NFCycles) = 0) Then
                    flag = True
                    Exit For
                End If
            End If

            If mLog.LogAPUAssemblies(i).ShowNGCycles Then
                If (Val(mLog.LogAPUAssemblies(i).NGCycles) = 0) Then
                    flag = True
                    Exit For
                End If
            End If

            If mLog.LogAPUAssemblies(i).ShowCTCycles Then
                If (Val(mLog.LogAPUAssemblies(i).CTCycles) = 0) Then
                    flag = True
                    Exit For

                End If
            End If
            If mLog.LogAPUAssemblies(i).ShowPTCycles Then
                If (Val(mLog.LogAPUAssemblies(i).PTCycles) = 0) Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogAPUAssemblies(i).ShowBleeds Then
                If (Val(mLog.LogAPUAssemblies(i).Bleeds) = 0) Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogAPUAssemblies(i).ShowStarts Then
                If (Val(mLog.LogAPUAssemblies(i).Starts) = 0) Then
                    flag = True
                    Exit For
                End If
            End If
            'Added by Shweta on 7-May-2012 for ALL02052012
            If mLog.LogAPUAssemblies(i).ShowGeneratorMods Then
                If (Val(mLog.LogAPUAssemblies(i).GeneratorMods) = 0) Then
                    flag = True
                    Exit For
                End If
            End If
            '----------
            If mLog.LogAPUAssemblies(i).ShowCycles Then
                If (Val(mLog.LogAPUAssemblies(i).Cycles) = 0) Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogAPUAssemblies(i).ShowRINS Then
                If (Val(mLog.LogAPUAssemblies(i).RINS) = 0) Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogAPUAssemblies(i).ShowStarts Then
                If (Val(mLog.LogAPUAssemblies(i).Starts) = 0) Then
                    flag = True
                    Exit For
                End If
            End If
        Next
        If flag = True Then
            flag = False
            isZero = True
        End If

        For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1
            'Engine
            If mLog.LogEngAssemblies(i).ShowHours Then
                If mLog.IsHobbs Then
                    If Val(mLog.LogEngAssemblies(i).Hours) = 0 Then
                        flag = True
                        Exit For
                    End If
                Else
                    If mLog.LogEngAssemblies(i).Hours = "0:00" Then
                        flag = True
                        Exit For
                    End If
                End If
            End If
            If mLog.LogEngAssemblies(i).ShowLandings Then
                If Val(mLog.LogEngAssemblies(i).Landings) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogEngAssemblies(i).ShowCycles Then
                If Val(mLog.LogEngAssemblies(i).Cycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogEngAssemblies(i).ShowStarts Then
                If Val(mLog.LogEngAssemblies(i).Starts) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogEngAssemblies(i).ShowNGCycles Then
                If Val(mLog.LogEngAssemblies(i).NGCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogEngAssemblies(i).ShowNFCycles Then
                If Val(mLog.LogEngAssemblies(i).NFCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogEngAssemblies(i).ShowRINS Then
                If Val(mLog.LogEngAssemblies(i).RINS) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogEngAssemblies(i).ShowCFactors Then
                If Val(mLog.LogEngAssemblies(i).CFactor) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogEngAssemblies(i).ShowBleeds Then
                If Val(mLog.LogEngAssemblies(i).Bleeds) = 0 Then
                    flag = True
                    Exit For
                End If
                '----------------------------- 
            End If
            'Added by Shweta on 7-May-2012 for ALL02052012
            If mLog.LogEngAssemblies(i).ShowGeneratorMods Then
                If Val(mLog.LogEngAssemblies(i).GeneratorMods) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogEngAssemblies(i).ShowImpellerCycles Then
                If Val(mLog.LogEngAssemblies(i).ImpellerCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogEngAssemblies(i).ShowPTCycles Then
                If Val(mLog.LogEngAssemblies(i).PTCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogEngAssemblies(i).ShowCTCycles Then
                If Val(mLog.LogEngAssemblies(i).CTCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            '-----------------
            If mLog.LogEngAssemblies(i).ShowRapidTakeOffFactors Then   ' 'Code added for Rapid TakeOff on 31-Oct-2022 by Saylee
                If Val(mLog.LogEngAssemblies(i).RapidTakeOffFactor) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
        Next
        If flag = True Then
            flag = False
            isZero = True
        End If

        'Added by Shweta on 8May-2012
        For i As Integer = 0 To mLog.LogCGBAssemblies.Count - 1
            If mLog.LogCGBAssemblies.ShowHours Then
                If mLog.IsHobbs Then
                    If Val(mLog.LogCGBAssemblies(i).Hours) = 0 Then
                        flag = True
                        Exit For
                    End If
                Else
                    If mLog.LogCGBAssemblies(i).Hours = "0:00" Then
                        flag = True
                        Exit For
                    End If
                End If
            End If
            If mLog.LogCGBAssemblies.ShowLandings Then
                If Val(mLog.LogCGBAssemblies(i).Landings) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogCGBAssemblies.ShowCycles Then
                If Val(mLog.LogCGBAssemblies(i).Cycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogCGBAssemblies.ShowStarts Then
                If Val(mLog.LogCGBAssemblies(i).Starts) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogCGBAssemblies.ShowNGCycles Then
                If Val(mLog.LogCGBAssemblies(i).NGCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogCGBAssemblies.ShowNFCycles Then
                If Val(mLog.LogCGBAssemblies(i).NFCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogCGBAssemblies.ShowRINS Then
                If Val(mLog.LogCGBAssemblies(i).RINS) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogCGBAssemblies.ShowBleeds Then
                If Val(mLog.LogCGBAssemblies(i).Bleeds) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogCGBAssemblies.ShowImpellerCycles Then
                If Val(mLog.LogCGBAssemblies(i).ImpellerCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogCGBAssemblies.ShowCTCycles Then
                If Val(mLog.LogCGBAssemblies(i).CTCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            If mLog.LogCGBAssemblies.ShowPTCycles Then
                If Val(mLog.LogCGBAssemblies(i).PTCycles) = 0 Then
                    flag = True
                    Exit For
                End If
            End If
            'Added by shweta on 7-May-2012 for ALL02052012
            If mLog.LogCGBAssemblies.ShowGeneratorMods Then
                If Val(mLog.LogCGBAssemblies(i).GeneratorMods) = 0 Then
                    flag = True
                    Exit For
                End If
            End If


        Next
        If flag = True Then
            flag = False
            isZero = True
        End If
        '----------------------------
        Return isZero
    End Function
    Private Sub SetObject()
        With mLog
            If Not IsDate(calDateTime.Text) Then
                .Date = System.DBNull.Value
            Else
                .Date = calDateTime.Text.ToString.Trim
            End If

            .LogText = Trim(txtLogText.Text)
            .LogNo = CInt(Val(Trim(txtLogNo.Text)))
            If .IsUTC = True Then
                'If Not IsDate(CalUTCDateTime.Text) Then
                '    .SouUniverseDateTime = System.DBNull.Value
                'Else
                '    .SouUniverseDateTime = CalUTCDateTime.Text.ToString.Trim
                'End If

                'If takeofftouchdown Then
                '    If Not IsDate(calUTCTakeOffDateTime.Text) Then
                '        .TakeOffUniverseDateTime = System.DBNull.Value
                '    Else
                '        .TakeOffUniverseDateTime = calUTCTakeOffDateTime.Text.ToString.Trim
                '    End If

                '    If Not IsDate(calUTCTouchDownDateTime.Text) Then
                '        .TouchDownUniverseDateTime = System.DBNull.Value
                '    Else
                '        .TouchDownUniverseDateTime = calUTCTouchDownDateTime.Text.ToString.Trim
                '    End If
                'End If

            Else
                'If takeofftouchdown Then
                '    If Not IsDate(calTakeOffLocalDateTime.Text) Then
                '        .TakeOffLocalDateTime = System.DBNull.Value
                '    Else
                '        .TakeOffLocalDateTime = calTakeOffLocalDateTime.Text.ToString.Trim
                '    End If

                '    If Not IsDate(calTouchDownLocalDateTime.Text) Then
                '        .TouchDownLocalDateTime = System.DBNull.Value
                '    Else
                '        .TouchDownLocalDateTime = calTouchDownLocalDateTime.Text.ToString.Trim
                '    End If
                'End If

                'If Not IsDate(calDeparture.Text) Then
                '    .SouLocalDateTime = System.DBNull.Value
                'Else
                '    .SouLocalDateTime = calDeparture.Text.ToString.Trim
                'End If

            End If
            '.SouDayLightTime = cmbDepartureDayLightTime.SelectedValue
            If .IsUTC = True Then

                '    If Not IsDate(CalUTCArrival.Text) Then
                '        .DesUniverseDateTime = System.DBNull.Value
                '    Else
                '        .DesUniverseDateTime = CalUTCArrival.Text.ToString.Trim
                '    End If
            End If
            '.DesDayLightTime = cmbArrivalDayLightTime.SelectedValue

            If Not TakeOffTouchDown Then
                .TimeInAir = Trim(txtAirBorneTime.Text)
            End If
            If Not AppSettings("Log") = "True" Then .TimeOnGround = Trim(txtGroundRunTime.Text)
            .PercentTimeOnGround = Val(Trim(txtPercentTimeOnGround.Text))
            If mMachine.HourType = 2 Then
                .PrevHobbsValue = Trim(txtPrevHobbsValue.Text)
                .PrevHobbsOffsetValue = Trim(txtPrevHobbsOffset.Text)
                .CurrentHobbsOffsetValue = Trim(txtCurrentHobbsOffset.Text)
                .CurrentHobbsValue = Trim(txtCurrentHobbsValue.Text)
                .OffSet = Trim(txtCurrentHobbsOffset.Text)
            End If
            .LogPageNo = txtLogPageNo.Text
            '.FlightNo = txtFlightNo.Text.Trim
            .Remark = Trim(txtRemark.Text)
            .FlightLogClassificationID = New Guid(cmbFlightLogClassification.SelectedValue.ToString)
            .FlightLogClassificationName = cmbFlightLogClassification.SelectedItem.Text
            If Session("IsValueZero") = "True" Then
                .IsValZero = True
            Else
                .IsValZero = False
            End If
            .TotalLandings = Val(txtTotalLandings.Text.Trim)

            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    .IsAttachmentAdded = True
                Else
                    .IsAttachmentAdded = False
                End If
            End If

            '''Added by Saylee on 4-Apr-2022, only for edited logs
            .RemarkEdit = "<NewRemark><Update> Update Due to Log Edit</Update><UserName>" + User.Identity.Name + "</UserName><LogDet>" + .LogTextNo + "</LogDet><UpdateDateTime>" + New SmartDate(Now.ToString("yyyy/MM/dd HH:mm")).FormattedTextTime + "</UpdateDateTime></NewRemark>"
            .IsLogEdited = True
            '*************************
        End With
        ' '' '''AttachMyFile()
        dgAFPeriods.DataBind()
        dgEnginePeriods.DataBind()
        dgAPUPeriods.DataBind()
        dgCGBPeriods.DataBind()
        Session("mLog") = mLog
    End Sub
    Private Sub GetAttachment()
        If mLog.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mLog.ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mLog.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub SaveAttachment() '
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mLog.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mLog.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        GetAttachment()
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
    Private Sub NewRecord()
        mLog = Log.NewLog(mMachine, Today.Date)
        mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
        mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
        ''''' CHECK_isRequiredAssembliesInstalled()
        Session("mLog") = mLog
        MarkLog(Util.Action.[New], "Flight Log Edit", "", Util.ErrorType.HandledError, mLog.ID, EventLogID)

        ' '' ''AJAX- Title line comment as it present in SetTitle function and also Update panel need to called after that.
        SetTitle()
        Dim str1 As String
        str1 = "delete_cookie();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str1, True)
        ' '' ''lblTitle.Text = "Status of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"
    End Sub
    'Added by Vikrant On 03-Aug-2015 For  ALL030812015
    Private Function SaveLogFlexiLog() As Boolean
        'Authentication
        If Not mLog.Date Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                'Changes by Kalpesh in 13-3-2013
                'These lines commented
                '
                'Dim strOutString As String = ReadXMLFile()
                'strOutString = strOutString.Split(CChar("$"))(1)
                'Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, CInt(strOutString), mCheck.SubscriptionDate)

                'Changes by Kalpesh in 13-3-2013
                'These lines commented
                '
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------

                'CNDC
                If DateDiff(DateInterval.Day, mLog.Date, maxAllowableDate) < 0 _
                    Or (IsDate(mLog.SouLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.SouLocalDateTime, maxAllowableDate) < 0) _
                    Or (IsDate(mLog.DesLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.DesLocalDateTime, maxAllowableDate) < 0) Then
                    'If DateDiff(DateInterval.Day, CDate(mLog.Date), maxAllowableDate) < 0 Or DateDiff(DateInterval.Day, CDate(mLog.SouLocalDateTime), maxAllowableDate) < 0 Or DateDiff(DateInterval.Day, CDate(mLog.DesLocalDateTime), maxAllowableDate) < 0 Then

                    ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly)
                    ' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
                    ' '' ''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")

                    DataFieldBind()
                    Exit Function
                End If
            End If
        End If
        'Authentication
        Dim LogClone As Log
        LogClone = CType(mLog.Clone, Log)
        'Code Added By Deven 21-03-2008 
        'BindClassification()
        '-------------------------------

        SetObject()
        SetAirFrameGridObject()
        SetEngineGridObject(True)
        SetAPUGridObject(True)
        SetCGBGridObject(True)

        If mLog.IsValid = True Then
            Try
                If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
                    'If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Then
                    'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
                    If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
                    Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
                        ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Restriction, SIMsgBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly)
                        ' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
                        ' '' ''msg1.Show()

                        ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                        DataBindGrid()
                        MSGBoxCtrl.Show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly, "")

                        Return False
                        Exit Function
                    End If
                End If

                'Added By Prashant 12-Apr-2010
                Dim IsMELCount As Boolean = False
                Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
                mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)
                For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1
                    'If (mTempMELSnagCorrectiveActionList(i).DueDate > calDateTime.Value) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = True) Then
                    If mTempMELSnagCorrectiveActionList(i).IsMEL = True And mTempMELSnagCorrectiveActionList(i).DueDate.ToString <> "" Then   'Added By Prashant 23-Sep-2010
                        If (CDate(calDateTime.Text) > CDate(mTempMELSnagCorrectiveActionList(i).DueDate)) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then
                            IsMELCount = True
                            Exit For
                        Else
                            IsMELCount = False
                        End If
                    End If
                Next
                mTempMELSnagCorrectiveActionList = Nothing
                '-----------------------------------
                If IsMELCount = True Then
                    ' '' ''Dim msg1 As New SIMsgBox(Page, "Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
                    ' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
                    ' '' ''Session("sender") = "MEL"
                    ' '' ''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    DataBindGrid()
                    MSGBoxCtrl.Show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo, IIf(Session("New") = "True", "MELNew", "MEL"))

                    Exit Function
                ElseIf IsEngineHoursSame() = False Or IsCGBHoursSame() = False Or IsZeroValueLog() = True Then  'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.HoursZero, SIMsgBox.Message_text.HoursZero, "Airframe,Engine,APU... Hours/Landins/Cycles... are Zero. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the readings.", MsgBoxStyle.YesNo)

                    ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching/are zero entered. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo)
                    ' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
                    ' '' ''Session("sender") = "SaveLogAfterHrsSame"
                    ' '' ''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    ' MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching/are zero entered. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")
                    DataBindGrid()
                    MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "There is some information missing / not entered correctly.</br> </br> Do you still want to Save Log? ", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")
                    Exit Function
                End If


                mLog.ApplyEdit()
                'Add Pilot and Co-pilot in Log Crew as Child...

                'IF Pilot 1 deleted...
                If mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then
                    If mLog.LogCrews.Contains(mLog.PrevPilotID1) Then
                        mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID1, ""))
                    End If
                ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And mLog.PrevPilotID1.Equals(Guid.Empty) Then
                    If Not mLog.LogCrews.Contains(mLog.PilotID1) Then
                        Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                        LogCrew.CrewID = mLog.PilotID1
                        LogCrew.DutyAsID = 1
                        mLog.LogCrews.Add(LogCrew)
                    End If
                ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then
                    If mLog.PilotID1.Equals(mLog.PrevPilotID1) Then
                        If Not mLog.LogCrews.Contains(mLog.PilotID1) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID1
                            LogCrew.DutyAsID = 1
                            mLog.LogCrews.Add(LogCrew)
                        End If
                    Else
                        If Not mLog.LogCrews.Contains(mLog.PrevPilotID1) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID1
                            LogCrew.DutyAsID = 1
                            mLog.LogCrews.Add(LogCrew)
                        Else
                            mLog.LogCrews(mLog.PrevPilotID1, "").CrewID = mLog.PilotID1
                        End If
                    End If
                End If

                'Pilot 2
                'IF Pilot 2 deleted...
                If mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then
                    If mLog.LogCrews.Contains(mLog.PrevPilotID2) Then
                        mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID2, ""))
                    End If
                ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And mLog.PrevPilotID2.Equals(Guid.Empty) Then
                    If Not mLog.LogCrews.Contains(mLog.PilotID2) Then
                        Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                        LogCrew.CrewID = mLog.PilotID2
                        LogCrew.DutyAsID = 2
                        mLog.LogCrews.Add(LogCrew)
                    End If
                ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then
                    If mLog.PilotID2.Equals(mLog.PrevPilotID2) Then
                        If Not mLog.LogCrews.Contains(mLog.PilotID2) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID2
                            LogCrew.DutyAsID = 2
                            mLog.LogCrews.Add(LogCrew)
                        End If
                    Else
                        If Not mLog.LogCrews.Contains(mLog.PrevPilotID2) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID2
                            LogCrew.DutyAsID = 2
                            mLog.LogCrews.Add(LogCrew)
                        Else
                            mLog.LogCrews(mLog.PrevPilotID2, "").CrewID = mLog.PilotID2
                        End If
                    End If
                End If

                'End If
                'End 

                'Added By Vikrant on 01-Dec-2021 for PBH
                Dim IsNewLog As Boolean
                IsNewLog = mLog.IsNew
                'End
                mLog = CType(mLog.Save(), Log)
                SaveAttachment()
                'Added By Vikrant on 01-Dec-2021 for PBH
                Dim mMaxLogOfAircraft As MaxLogOfAircraft
                mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

                If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
                    If Not (AppSettings("ClientCode") = "Heligo" Or
                       AppSettings("ClientCode") = "UHPL" Or
                       AppSettings("ClientCode") = "APFT" Or
                       AppSettings("ClientCode") = "AAP") Then
                        If Not (CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime)) Then 'Last Log
                            SetPBHValues(mLog, IsNewLog)
                        End If
                    Else
                        If Not (CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate)) Then 'Last Log
                            SetPBHValues(mLog, IsNewLog)
                        End If
                    End If
                End If
                'End
                Dim IsShowDateCntrl As String
                If (Session("IsSaveAndNew") Is Nothing OrElse Session("IsSaveAndNew") <> 1) Then
                    IsShowDateCntrl = "False"
                    Session("IsSaveAndNew") = 0
                Else
                    IsShowDateCntrl = "True"
                End If
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AfterSave", "AfterSave('" + IsShowDateCntrl + "');", True)
                '-----------------------------------------------------------------------
                mLog = Log.GetLog(mLog.ID)
                mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
                MarkLog(Util.Action.Save, "Flight Log Edit", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
                '-----------------------------------------------------------------------
                Session("mLog") = mLog
                Return True
            Catch ex As SqlException
                Session("LogClone") = LogClone
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 50000 Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.LogExist, MSGBox.Message_text.LogExist, " between Current Date and Time Span for this Aircraft. ", MsgBoxStyle.OkOnly, "")
                End If
                'Added by utkash on 1-oct-2013 for log_ajax changes
                mLog = LogClone
                Session("mLog") = mLog
                'end
                Return False
            Finally
                LogClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    'End
    Public Function IsEngineHoursSame() As Boolean 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
        Dim IsSame As Boolean = True
        For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1
            'If mLog.LogAFAssemblies(0).Hours = mLog.LogEngAssemblies(i).Hours Then
            If mLog.TotalTime = mLog.LogEngAssemblies(i).Hours Then
                IsSame = True
            Else
                IsSame = False
                Exit For
            End If
        Next
        If IsSame = True Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function IsCGBHoursSame() As Boolean 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
        Dim IsSame As Boolean = True
        ''If mLog.LogCGBAssemblies Is Nothing Then
        ''    Return True
        ''End If
        For i As Integer = 0 To mLog.LogCGBAssemblies.Count - 1
            'If mLog.LogAFAssemblies(0).Hours = mLog.LogCGBAssemblies(i).Hours Then
            If mLog.TotalTime = mLog.LogCGBAssemblies(i).Hours Then
                IsSame = True
            Else
                IsSame = False
                Exit For
            End If
        Next
        If IsSame = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub SetAirFrameGridObject(Optional ByVal isFromDataBindGrid As Boolean = False)  ' For First Grid i.e AirFrame
        Dim txtAirFrameHours, txtAirFrameLandings, txtAirFrameCycles, txtAirFrameStarts, txtAirFrameNGCycles, txtAirFrameNFCycles, txtAirFrameRins,
            txtAirFrameBleeds, txtAirFrameImpellerCycles, txtAirFrameCTCycles, txtAirFramePTCycles, txtAirframeGeneratorMods As TextBox

        ' '' ''AJAX- DataGrid is replaced by GridView for removing "Refresh" buttons. So here "dgAFPeriods.Items" is replaced by "dgAFPeriods.Rows"
        For i As Integer = 0 To Me.dgAFPeriods.Rows.Count - 1
            txtAirFrameHours = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameHours"), TextBox)
            txtAirFrameLandings = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameLandings"), TextBox)
            txtAirFrameCycles = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameCycles"), TextBox)
            txtAirFrameStarts = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameStarts"), TextBox)
            txtAirFrameNGCycles = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameNGCycles"), TextBox)
            txtAirFrameNFCycles = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameNFCycles"), TextBox)
            txtAirFrameRins = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameRins"), TextBox)
            'Added By Prashant 8-July-2009
            txtAirFrameBleeds = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameBleeds"), TextBox)
            '-----------------------------
            'Added By Prashant 10-Aug-2009
            txtAirFrameImpellerCycles = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameImpellerCycles"), TextBox)
            txtAirFrameCTCycles = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameCTCycles"), TextBox)
            txtAirFramePTCycles = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFramePTCycles"), TextBox)
            '-----------------------------
            'Added by Shweta on 8-May-2012  for ALL02052012
            txtAirframeGeneratorMods = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirframeGeneratorMods"), TextBox)
            If isFromDataBindGrid Then If mLog.LogAFAssemblies.ShowHours Then mLog.LogAFAssemblies(i).Hours = Trim(txtAirFrameHours.Text)
            If mLog.LogAFAssemblies.ShowLandings Then mLog.LogAFAssemblies(i).Landings = Trim(txtAirFrameLandings.Text)
            If mLog.LogAFAssemblies.ShowCycles Then mLog.LogAFAssemblies(i).Cycles = Trim(txtAirFrameCycles.Text)
            If mLog.LogAFAssemblies.ShowStarts Then mLog.LogAFAssemblies(i).Starts = Trim(txtAirFrameStarts.Text)
            If mLog.LogAFAssemblies.ShowNGCycles Then mLog.LogAFAssemblies(i).NGCycles = Trim(txtAirFrameNGCycles.Text)
            If mLog.LogAFAssemblies.ShowNFCycles Then mLog.LogAFAssemblies(i).NFCycles = Trim(txtAirFrameNFCycles.Text)
            If mLog.LogAFAssemblies.ShowRINS Then mLog.LogAFAssemblies(i).RINS = Trim(txtAirFrameRins.Text)
            'Added By Prashant 8-July-2009
            If mLog.LogAFAssemblies.ShowBleeds Then mLog.LogAFAssemblies(i).Bleeds = Trim(txtAirFrameBleeds.Text)
            '-----------------------------

            'Added By Prashant 10-Aug-2009
            If mLog.LogAFAssemblies.ShowImpellerCycles Then mLog.LogAFAssemblies(i).ImpellerCycles = Trim(txtAirFrameImpellerCycles.Text)
            If mLog.LogAFAssemblies.ShowCTCycles Then mLog.LogAFAssemblies(i).CTCycles = Trim(txtAirFrameCTCycles.Text)
            If mLog.LogAFAssemblies.ShowPTCycles Then mLog.LogAFAssemblies(i).PTCycles = Trim(txtAirFramePTCycles.Text)
            '-----------------------------
            If mLog.LogAFAssemblies.ShowGeneratorMods Then mLog.LogAFAssemblies(i).GeneratorMods = Trim(txtAirframeGeneratorMods.Text) 'Added by Shweta on 8-May-2012  for ALL02052012

            'Added By Saylee 21-Mar-2013 for ALL11032013 - 1
            If mLog.LogAFAssemblies.ShowCycles Then mLog.UpdateChildPeriods(3, "Cycles", mLog.LogAFAssemblies(i).Cycles)
            If mLog.LogAFAssemblies.ShowNGCycles Then mLog.UpdateChildPeriods(4, "NgCycles", mLog.LogAFAssemblies(i).NGCycles)
            If mLog.LogAFAssemblies.ShowNFCycles Then mLog.UpdateChildPeriods(5, "NfCycles", mLog.LogAFAssemblies(i).NFCycles)
            If mLog.LogAFAssemblies.ShowRINS Then mLog.UpdateChildPeriods(6, "RINS", mLog.LogAFAssemblies(i).RINS)
            If mLog.LogAFAssemblies.ShowLandings Then mLog.UpdateChildPeriods(7, "Landings", mLog.LogAFAssemblies(i).Landings)
            If mLog.LogAFAssemblies.ShowStarts Then mLog.UpdateChildPeriods(8, "Starts", mLog.LogAFAssemblies(i).Starts)
            'If mLog.LogAFAssemblies.ShowLandings  Then mLog.UpdateChildPeriods(9, "Accumulated Cycles", mLog.LogAFAssemblies(i).AccumulatedCycles)

            If mLog.LogAFAssemblies.ShowBleeds Then mLog.UpdateChildPeriods(11, "Bleeds", mLog.LogAFAssemblies(i).Bleeds)
            If mLog.LogAFAssemblies.ShowImpellerCycles Then mLog.UpdateChildPeriods(12, "ImpellerCycles", mLog.LogAFAssemblies(i).ImpellerCycles)
            If mLog.LogAFAssemblies.ShowCTCycles Then mLog.UpdateChildPeriods(13, "CTCycles", mLog.LogAFAssemblies(i).CTCycles)
            If mLog.LogAFAssemblies.ShowPTCycles Then mLog.UpdateChildPeriods(14, "PTCycles", mLog.LogAFAssemblies(i).PTCycles)
            If mLog.LogAFAssemblies.ShowGeneratorMods Then mLog.UpdateChildPeriods(15, "GeneratorMods", mLog.LogAFAssemblies(i).GeneratorMods)
            '-----------------------------

        Next i
        Session("mLog") = mLog
    End Sub
    Public Sub SetEngineGridObject(Optional ByVal isFromDataBindGrid As Boolean = False)        ' For Second Grid i.e ENGINE
        Dim txtEngineHours, txtEngineLandings, txtEngineCycles, txtEngineStarts, txtEngineNGCycles, txtEngineNFCycles, txtEngineRins,
            txtEngineCFactors, txtEngineBleeds, txtEngineImpellerCycles, txtEngineCTCycles, txtEnginePTCycles, txtEngineGeneratorMods, txtEngineRapidTakeOffFactor As TextBox

        ' '' ''AJAX- DataGrid is replaced by GridView for removing "Refresh" buttons. So here "dgEnginePeriods.Items" is replaced by "dgEnginePeriods.Rows"
        For i As Integer = 0 To Me.dgEnginePeriods.Rows.Count - 1
            txtEngineHours = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineHours"), TextBox)
            txtEngineLandings = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineLandings"), TextBox)
            txtEngineCycles = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineCycles"), TextBox)
            txtEngineStarts = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineStarts"), TextBox)
            txtEngineNGCycles = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineNGCycles"), TextBox)
            txtEngineNFCycles = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineNFCycles"), TextBox)
            txtEngineRins = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineRins"), TextBox)
            txtEngineCFactors = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineCFactors"), TextBox)
            'Added By Prashant 8-July-2009
            txtEngineBleeds = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineBleeds"), TextBox)
            '-----------------------------------
            'Added By Prashant 10-Aug-2009
            txtEngineImpellerCycles = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineImpellerCycles"), TextBox)
            txtEngineCTCycles = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineCTCycles"), TextBox)
            txtEnginePTCycles = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEnginePTCycles"), TextBox)
            '-----------------------------------
            'If mLog.LogEngAssemblies(i).ShowHours Then mLog.LogEngAssemblies(i).Hours = Trim(txtEngineHours.Text)
            txtEngineGeneratorMods = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineGeneratorMods"), TextBox) 'Added by Shweta on 8-May-2012  for ALL02052012

            txtEngineRapidTakeOffFactor = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineRapidTakeOffFactor"), TextBox) ' 'Code added for Rapid TakeOff on 31-Oct-2022 by Saylee


            If isFromDataBindGrid Then If mLog.LogEngAssemblies(i).ShowHours Then mLog.LogEngAssemblies(i).Hours = Trim(txtEngineHours.Text)
            If mLog.LogEngAssemblies(i).ShowLandings Then mLog.LogEngAssemblies(i).Landings = Trim(txtEngineLandings.Text)
            If mLog.LogEngAssemblies(i).ShowCycles Then mLog.LogEngAssemblies(i).Cycles = Trim(txtEngineCycles.Text)
            If mLog.LogEngAssemblies(i).ShowStarts Then mLog.LogEngAssemblies(i).Starts = Trim(txtEngineStarts.Text)
            If mLog.LogEngAssemblies(i).ShowNGCycles Then mLog.LogEngAssemblies(i).NGCycles = Trim(txtEngineNGCycles.Text)
            If mLog.LogEngAssemblies(i).ShowNFCycles Then mLog.LogEngAssemblies(i).NFCycles = Trim(txtEngineNFCycles.Text)
            If mLog.LogEngAssemblies(i).ShowRINS Then mLog.LogEngAssemblies(i).RINS = Trim(txtEngineRins.Text)
            If mLog.LogEngAssemblies(i).ShowCFactors Then mLog.LogEngAssemblies(i).CFactor = Trim(txtEngineCFactors.Text)
            'Added By Prashant 8-July-2009
            If mLog.LogEngAssemblies(i).ShowBleeds Then mLog.LogEngAssemblies(i).Bleeds = Trim(txtEngineBleeds.Text)
            '----------------------------- 
            'Added By Prashant 10-Aug-2009
            If mLog.LogEngAssemblies(i).ShowImpellerCycles Then mLog.LogEngAssemblies(i).ImpellerCycles = Trim(txtEngineImpellerCycles.Text)
            If mLog.LogEngAssemblies(i).ShowCTCycles Then mLog.LogEngAssemblies(i).CTCycles = Trim(txtEngineCTCycles.Text)
            If mLog.LogEngAssemblies(i).ShowPTCycles Then mLog.LogEngAssemblies(i).PTCycles = Trim(txtEnginePTCycles.Text)
            '-----------------------------
            If mLog.LogEngAssemblies(i).ShowGeneratorMods Then mLog.LogEngAssemblies(i).GeneratorMods = Trim(txtEngineGeneratorMods.Text) 'Added by Shweta on 8-May-2012  for ALL02052012
            If mLog.LogEngAssemblies(i).ShowRapidTakeOffFactors Then mLog.LogEngAssemblies(i).RapidTakeOffFactor = Trim(txtEngineRapidTakeOffFactor.Text) ' 'Code added for Rapid TakeOff on 31-Oct-2022 by Saylee

        Next i
        Session("mLog") = mLog
    End Sub
    Public Sub SetAPUGridObject(Optional ByVal isFromDataBindGrid As Boolean = False)        ' For Third Grid i.e APU
        Dim txtAPUHours, txtAPULandings, txtAPUCycles As TextBox, txtAPUStarts, txtAPUNGCycles, txtAPUNFCycles, txtAPURins, txtAPUBleeds,
            txtAPUImpellerCycles, txtAPUCTCycles, txtAPUPTCycles, txtAPUGeneratorMods As TextBox

        ' '' ''AJAX- DataGrid is replaced by GridView for removing "Refresh" buttons. So here "dgAPUPeriods.Items" is replaced by "dgAPUPeriods.Rows"
        For i As Integer = 0 To Me.dgAPUPeriods.Rows.Count - 1
            txtAPUHours = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUHours"), TextBox)
            txtAPULandings = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPULandings"), TextBox)
            txtAPUCycles = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUCycles"), TextBox)
            txtAPUStarts = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUStarts"), TextBox)
            txtAPUNGCycles = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUNGCycles"), TextBox)
            txtAPUNFCycles = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUNFCycles"), TextBox)
            txtAPURins = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPURins"), TextBox)
            'Added By Prashant 8-July-2009
            txtAPUBleeds = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUBleeds"), TextBox)
            '-----------------------------
            'Added By Prashant 10-Aug-2009
            txtAPUImpellerCycles = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUImpellerCycles"), TextBox)
            txtAPUCTCycles = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUCTCycles"), TextBox)
            txtAPUPTCycles = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUPTCycles"), TextBox)
            '-----------------------------
            txtAPUGeneratorMods = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUGeneratorMods"), TextBox) 'Added by Shweta on 8-May-2012  for ALL02052012
            'If mLog.LogAPUAssemblies(i).ShowHours Then mLog.LogAPUAssemblies.Item(i).Hours = Trim(txtAPUHours.Text)
            If isFromDataBindGrid Then If mLog.LogAPUAssemblies(i).ShowHours Then mLog.LogAPUAssemblies.Item(i).Hours = Trim(txtAPUHours.Text)
            If mLog.LogAPUAssemblies(i).ShowLandings Then mLog.LogAPUAssemblies.Item(i).Landings = Trim(txtAPULandings.Text)
            If mLog.LogAPUAssemblies(i).ShowCycles Then mLog.LogAPUAssemblies.Item(i).Cycles = Trim(txtAPUCycles.Text)
            If mLog.LogAPUAssemblies(i).ShowStarts Then mLog.LogAPUAssemblies.Item(i).Starts = Trim(txtAPUStarts.Text)
            If mLog.LogAPUAssemblies(i).ShowNGCycles Then mLog.LogAPUAssemblies.Item(i).NGCycles = Trim(txtAPUNGCycles.Text)
            If mLog.LogAPUAssemblies(i).ShowNFCycles Then mLog.LogAPUAssemblies.Item(i).NFCycles = Trim(txtAPUNFCycles.Text)
            If mLog.LogAPUAssemblies(i).ShowRINS Then mLog.LogAPUAssemblies.Item(i).RINS = Trim(txtAPURins.Text)
            'Added By Prashant 8-July-2009
            If mLog.LogAPUAssemblies(i).ShowBleeds Then mLog.LogAPUAssemblies.Item(i).Bleeds = Trim(txtAPUBleeds.Text)
            '-----------------------------
            'Added By Prashant 10-Aug-2009
            If mLog.LogAPUAssemblies(i).ShowImpellerCycles Then mLog.LogAPUAssemblies.Item(i).ImpellerCycles = Trim(txtAPUImpellerCycles.Text)
            If mLog.LogAPUAssemblies(i).ShowCTCycles Then mLog.LogAPUAssemblies.Item(i).CTCycles = Trim(txtAPUCTCycles.Text)
            If mLog.LogAPUAssemblies(i).ShowPTCycles Then mLog.LogAPUAssemblies.Item(i).PTCycles = Trim(txtAPUPTCycles.Text)
            '-----------------------------
            'Added by Shweta on 8-May-2012  for ALL02052012
            If mLog.LogAPUAssemblies(i).ShowGeneratorMods Then mLog.LogAPUAssemblies(i).GeneratorMods = Trim(txtAPUGeneratorMods.Text)
        Next i
        Session("mLog") = mLog
    End Sub
    Public Sub SetCGBGridObject(Optional ByVal isFromDataBindGrid As Boolean = False)         'For 4th Grid i.e CGB
        Dim txtCGBHours, txtCGBLandings, txtCGBCycles As TextBox, txtCGBStarts, txtCGBNGCycles, txtCGBNFCycles, txtCGBRins, txtCGBBleeds,
            txtCGBImpellerCycles, txtCGBCTCycles, txtCGBPTCycles, txtCGBGeneratorMods As TextBox

        ' '' ''AJAX- DataGrid is replaced by GridView for removing "Refresh" buttons. So here "dgCGBPeriods.Items" is replaced by "dgCGBPeriods.Rows"
        For i As Integer = 0 To Me.dgCGBPeriods.Rows.Count - 1
            txtCGBHours = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBHours"), TextBox)
            txtCGBLandings = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBLandings"), TextBox)
            txtCGBCycles = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBCycles"), TextBox)
            txtCGBStarts = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBStarts"), TextBox)
            txtCGBNGCycles = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBNGCycles"), TextBox)
            txtCGBNFCycles = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBNFCycles"), TextBox)
            txtCGBRins = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBRins"), TextBox)
            txtCGBBleeds = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBBleeds"), TextBox)
            txtCGBImpellerCycles = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBImpellerCycles"), TextBox)
            txtCGBCTCycles = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBCTCycles"), TextBox)
            txtCGBPTCycles = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBPTCycles"), TextBox)
            txtCGBGeneratorMods = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBGeneratorMods"), TextBox)  'Added by Shweta on 8-May-2012 for ALL02052012

            If isFromDataBindGrid Then If mLog.LogCGBAssemblies(i).ShowHours Then mLog.LogCGBAssemblies.Item(i).Hours = Trim(txtCGBHours.Text)
            If mLog.LogCGBAssemblies(i).ShowLandings Then mLog.LogCGBAssemblies.Item(i).Landings = Trim(txtCGBLandings.Text)
            If mLog.LogCGBAssemblies(i).ShowCycles Then mLog.LogCGBAssemblies.Item(i).Cycles = Trim(txtCGBCycles.Text)
            If mLog.LogCGBAssemblies(i).ShowStarts Then mLog.LogCGBAssemblies.Item(i).Starts = Trim(txtCGBStarts.Text)
            If mLog.LogCGBAssemblies(i).ShowNGCycles Then mLog.LogCGBAssemblies.Item(i).NGCycles = Trim(txtCGBNGCycles.Text)
            If mLog.LogCGBAssemblies(i).ShowNFCycles Then mLog.LogCGBAssemblies.Item(i).NFCycles = Trim(txtCGBNFCycles.Text)
            If mLog.LogCGBAssemblies(i).ShowRINS Then mLog.LogCGBAssemblies.Item(i).RINS = Trim(txtCGBRins.Text)
            If mLog.LogCGBAssemblies(i).ShowBleeds Then mLog.LogCGBAssemblies.Item(i).Bleeds = Trim(txtCGBBleeds.Text)
            If mLog.LogCGBAssemblies(i).ShowImpellerCycles Then mLog.LogCGBAssemblies.Item(i).ImpellerCycles = Trim(txtCGBImpellerCycles.Text)
            If mLog.LogCGBAssemblies(i).ShowCTCycles Then mLog.LogCGBAssemblies.Item(i).CTCycles = Trim(txtCGBCTCycles.Text)
            If mLog.LogCGBAssemblies(i).ShowPTCycles Then mLog.LogCGBAssemblies.Item(i).PTCycles = Trim(txtCGBPTCycles.Text)
            If mLog.LogCGBAssemblies(i).ShowGeneratorMods Then mLog.LogCGBAssemblies.Item(i).GeneratorMods = Trim(txtCGBGeneratorMods.Text) 'Added by Shweta on 8-May-2012  for ALL02052012

        Next i
        Session("mLog") = mLog
    End Sub
    'Added By Vikrant on 01-Dec-2021 for PBH
    ''Private Sub SetPBHValues(ByVal TmpLog As Log, ByVal IsLogNew As Boolean)
    ''    Try

    ''        If mCompanyDetail.IsCombinedHours = False Then 'PBH Collective Hrs by Saylee on 30-Nov-2022

    ''            Dim mPBH As PBH = PBH.GetPBHByMachine(TmpLog.MachineID, "")
    ''            If Not mPBH.MachineID.Equals(Guid.Empty) Then
    ''                If CDate(mLog.Date) >= CDate(mPBH.StartDate) Then

    ''                    mPBH.CurrentHours = TmpLog.LogAFAssemblies(0).FinalHours_Dec
    ''                    mPBH.ElapsedHours = New Period(1, (New Period(1, TmpLog.LogAFAssemblies(0).FinalHours_Dec, 1, False, False).DbValueDec - mPBH.StartHoursDec), 1, False, False).Value
    ''                    mPBH.RemainingHours = New Period(1, mPBH.HoursFrequencyDec - mPBH.ElapsedHoursDec, 1, False, False).Value
    ''                    mPBH.LastLogDetails = TmpLog.DateFormatted
    ''                    'For Not Active Case: If RemainingHours<=0 then mark Not Active flag
    ''                    'Also mark Not InUse in tabMachine at same time 
    ''                    If mPBH.RemainingHoursDec <= 0 Then
    ''                        mPBH.IsNotActive = True
    ''                        mPBH.NotActiveDate = TmpLog.DateFormatted.ToString
    ''                        mPBH.MachineNotInUse = True
    ''                        Session("IsAircraftMadeNotInUse") = "True"
    ''                    End If
    ''                    mPBH.Save()
    ''                End If
    ''            End If
    ''        ElseIf mCompanyDetail.IsCombinedHours = True Then 'PBH Collective Hrs by Saylee on 30-Nov-2022
    ''            Dim mPBH As PBH
    ''            If IsLogNew Then
    ''                Dim mPBHList As PBHList = PBHList.GetList(IsAllRecordsRequired:=1)
    ''                mPBH = PBH.GetPBH(mPBHList(0).ID)
    ''                If CDate(mLog.Date) >= CDate(mPBH.StartDate) Then
    ''                    mPBH.RemainingHours = New Period(1, mPBH.RemainingHoursDec - New Period(1, TmpLog.LogAFAssemblies(0).Hours_Dec, 1, False, False).DbValueDec, 1, False, False).Value
    ''                    mPBH.ElapsedHours = New Period(1, mPBH.HoursFrequencyDec - mPBH.RemainingHoursDec, 1, False, False).Value
    ''                    mPBH.LastLogDetails = TmpLog.DateFormatted
    ''                    If mPBH.RemainingHoursDec <= 0 Then
    ''                        mPBH.IsNotActive = True
    ''                        mPBH.NotActiveDate = TmpLog.DateFormatted.ToString
    ''                        mPBH.MachineNotInUse = True
    ''                        Session("IsAircraftMadeNotInUse") = "True"
    ''                    End If
    ''                    mPBH.Save()
    ''                End If

    ''            End If
    ''        End If
    ''    Catch ex As Exception
    ''        Throw ex
    ''    End Try
    ''End Sub
    Private Sub SetPBHValues(ByVal TmpLog As Log, ByVal IsLogNew As Boolean)
        Try
            If mCompanyDetail.IsCombinedHours = False Then 'PBH Collective Hrs by Saylee on 30-Nov-2022
                If IsLogNew Then

                    Dim mPBH As PBH = PBH.GetPBHByMachine(TmpLog.MachineID, "")
                    If Not mPBH.MachineID.Equals(Guid.Empty) Then
                        If CDate(Today.Date) >= CDate(mPBH.StartDate) Then
                            mPBH.CurrentHours = TmpLog.LogAFAssemblies(0).FinalHours_Dec
                            If IsLogNew Then
                                mPBH.ElapsedHours = New Period(1, (New Period(1, TmpLog.LogAFAssemblies(0).FinalHours_Dec, 1, False, False).DbValueDec - mPBH.StartHoursDec), 1, False, False).Value
                            Else
                                mPBH.ElapsedHours = New Period(1, (New Period(1, TmpLog.LogAFAssemblies(0).FinalHours_Dec, 1, False, False).DbValueDec - mPBH.StartHoursDec + New Period(1, TmpLog.LogAFAssemblies(0).Hours_Dec, 1, False, False).DbValueDec), 1, False, False).Value
                            End If

                            mPBH.RemainingHours = New Period(1, (mPBH.HoursFrequencyDec + mPBH.CarryForwardHoursDec) - mPBH.ElapsedHoursDec, 1, False, False).Value
                            mPBH.LastLogDetails = TmpLog.DateFormatted
                            'For Not Active Case: If RemainingHours<=0 then mark Not Active flag
                            'Also mark Not InUse in tabMachine at same time 
                            If mPBH.RemainingHoursDec <= 0 Then
                                mPBH.IsNotActive = True
                                mPBH.NotActiveDate = TmpLog.DateFormatted.ToString
                                mPBH.MachineNotInUse = True
                                Session("IsAircraftMadeNotInUse") = "True"
                            End If
                            mPBH.Save()
                        End If
                    End If
                End If
            ElseIf mCompanyDetail.IsCombinedHours = True Then 'PBH Collective Hrs by Saylee on 30-Nov-2022
                Dim mPBH As PBH
                If IsLogNew Then
                    Dim mPBHList As PBHList = PBHList.GetList(IsAllRecordsRequired:=1)
                    mPBH = PBH.GetPBH(mPBHList(0).ID)
                    If CDate(Today.Date) >= CDate(mPBH.StartDate) Then
                        mPBH.RemainingHours = New Period(1, mPBH.RemainingHoursDec - New Period(1, TmpLog.LogAFAssemblies(0).Hours_Dec, 1, False, False).DbValueDec, 1, False, False).Value
                        If mPBH.CarryForwardHoursDec < 0 Then
                            mPBH.ElapsedHours = New Period(1, mPBH.HoursFrequencyDec - mPBH.RemainingHoursDec, 1, False, False).Value
                        Else
                            mPBH.ElapsedHours = New Period(1, (mPBH.HoursFrequencyDec + mPBH.CarryForwardHoursDec) - mPBH.RemainingHoursDec, 1, False, False).Value
                        End If


                        mPBH.LastLogDetails = TmpLog.DateFormatted
                        If mPBH.RemainingHoursDec <= 0 Then
                            mPBH.IsNotActive = True
                            mPBH.NotActiveDate = TmpLog.DateFormatted.ToString
                            mPBH.MachineNotInUse = True
                            Session("IsAircraftMadeNotInUse") = "True"
                        End If
                        mPBH.Save()
                    End If

                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'End
    Private Function Save() As Boolean
        'Authentication
        If Not mLog.Date Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                'Changes by Kalpesh in 13-3-2013
                'These lines commented
                '
                'Dim strOutString As String = ReadXMLFile()
                'strOutString = strOutString.Split(CChar("$"))(1)
                'Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, CInt(strOutString), mCheck.SubscriptionDate)

                'Changes by Kalpesh in 13-3-2013
                'These lines commented
                '
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------
                'CNDC
                If DateDiff(DateInterval.Day, mLog.Date, maxAllowableDate) < -10 _
                    Or (IsDate(mLog.SouLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.SouLocalDateTime, maxAllowableDate) < -10) _
                    Or (IsDate(mLog.DesLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.DesLocalDateTime, maxAllowableDate) < -10) Then
                    'If DateDiff(DateInterval.Day, CDate(mLog.Date), maxAllowableDate) < 0 Or DateDiff(DateInterval.Day, CDate(mLog.SouLocalDateTime), maxAllowableDate) < 0 Or DateDiff(DateInterval.Day, CDate(mLog.DesLocalDateTime), maxAllowableDate) < 0 Then

                    ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater by 10 Days or more than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly)
                    ' '' ''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    ' '' ''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater by 10 Days or more than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")

                    DataFieldBind()
                    Exit Function
                End If
            End If
        End If
        'Authentication
        Dim LogClone As Log
        LogClone = CType(mLog.Clone, Log)
        'Code Added By Deven 21-03-2008 
        'BindClassification()
        '-------------------------------

        SetObject()
        SetAirFrameGridObject()
        SetEngineGridObject(True)
        SetAPUGridObject(True)
        SetCGBGridObject(True)

        If mLog.IsValid = True Then

            If mLog.LogPageNo <> "0" Or mLog.LogPageNo <> "" Then
                Dim mPrevLogDetail As PrevLogDetail = PrevLogDetail.GetPrevLogDetail(mLog.MachineID, mLog.Date, mLog.LogPageNo)
                If mPrevLogDetail.IsTLPNODuplicate And mLog.LogNo <> mPrevLogDetail.LogNo Then
                    ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, "TLP No. already exists.", MsgBoxStyle.OKOnly)
                    ' '' ''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    ' '' ''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    DataBindGrid()
                    MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "TLP No. already exists.", MsgBoxStyle.OkOnly, "")

                    Return False
                End If
            End If

            Try
                If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
                    ' If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Then
                    'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
                    If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
                    Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
                        ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Restriction, SIMsgBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OKOnly)
                        ' '' ''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                        ' '' ''msg1.Show()

                        ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                        DataBindGrid()
                        MSGBoxCtrl.Show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly, "")

                        Return False
                        Exit Function
                    End If
                End If

                If IsEngineHoursSame() = False Or IsCGBHoursSame() = False Or IsZeroValueLog() = True Then 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.HoursZero, SIMsgBox.Message_text.HoursZero, "Airframe,Engine,APU... Hours/Landins/Cycles... are Zero. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the readings.", MsgBoxStyle.YesNo)

                    ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching/are zero entered. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo)
                    ' '' ''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    ' '' ''Session("sender") = "SaveLogAfterHrsSame"
                    ' '' ''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    ''MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching/are zero entered. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")
                    DataBindGrid()
                    MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "There is some information missing / not entered correctly.</br> </br> Do you still want to Save Log? ", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")
                    Exit Function
                End If

                mLog.ApplyEdit()
                'Add Pilot and Co-pilot in Log Crew as Child...
                'Pilot In Command
                If mLog.IsNew Then
                    If Not mLog.PilotID1.Equals(Guid.Empty) Then
                        Dim mLogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                        mLogCrew.CrewID = mLog.PilotID1
                        mLogCrew.DutyAsID = 1
                        mLog.LogCrews.Add(mLogCrew)
                    End If
                    'Co-Pilot
                    If Not mLog.PilotID2.Equals(Guid.Empty) Then
                        Dim mLogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                        mLogCrew.CrewID = mLog.PilotID2
                        mLogCrew.DutyAsID = 2
                        mLog.LogCrews.Add(mLogCrew)
                    End If
                End If
                'End 
                'Added By Vikrant on 01-Dec-2021 for PBH
                Dim IsNewLog As Boolean
                IsNewLog = mLog.IsNew
                'End
                mLog = CType(mLog.Save(), Log)
                SaveAttachment()
                'Added By Vikrant on 01-Dec-2021 for PBH
                Dim mMaxLogOfAircraft As MaxLogOfAircraft
                mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

                If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
                    If Not (AppSettings("ClientCode") = "Heligo" Or
                       AppSettings("ClientCode") = "UHPL" Or
                       AppSettings("ClientCode") = "APFT" Or
                       AppSettings("ClientCode") = "AAP") Then
                        If Not (CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime)) Then 'Last Log
                            SetPBHValues(mLog, IsNewLog)
                        End If
                    Else
                        If Not (CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate)) Then 'Last Log
                            SetPBHValues(mLog, IsNewLog)
                        End If
                    End If
                End If
                'End
                ControlVisibilityForAttachment()


                ' '' ''AJAX- New JavaScript function added to Show/Hide JQuery Date Control
                Dim IsShowDateCntrl As String
                If (Session("IsSaveAndNew") Is Nothing OrElse Session("IsSaveAndNew") <> 1) Then
                    IsShowDateCntrl = "False"
                    Session("IsSaveAndNew") = 0
                Else
                    IsShowDateCntrl = "True"
                End If

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AfterSave", "AfterSave('" + IsShowDateCntrl + "');", True)
                '-----------------------------------------------------------------------
                mLog = Log.GetLog(mLog.ID)
                mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
                MarkLog(Util.Action.Save, "Flight Log Edit", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
                '-----------------------------------------------------------------------
                Session("mLog") = mLog
                Return True
            Catch ex As SqlException
                Session("LogClone") = LogClone
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                    ' '' ''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    ' '' ''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")

                ElseIf ex.Number = 8145 Then
                    ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    ' '' ''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    ' '' ''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")

                ElseIf ex.Number = 2627 Then
                    ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    ' '' ''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    ' '' ''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")

                ElseIf ex.Number = 547 Then
                    ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    ' '' ''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    ' '' ''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")

                ElseIf ex.Number = 50000 Then
                    ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.LogExist, SIMsgBox.Message_text.LogExist, " between Current Date and Time Span for this Aircraft. ", MsgBoxStyle.OKOnly)
                    ' '' ''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    ' '' ''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.Show(MSGBox.Message_title.LogExist, MSGBox.Message_text.Alert, "Log already entered between current Date and Time span for this Aircraft.", MsgBoxStyle.OkOnly, "")

                End If
                Return False
            Finally
                LogClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Function SaveLogAfterHrsSame() As Boolean
        'Authentication
        If Not mLog.Date Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                'Changes by Kalpesh in 13-3-2013
                'These lines commented
                '
                'Dim strOutString As String = ReadXMLFile()
                'strOutString = strOutString.Split(CChar("$"))(1)
                'Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, CInt(strOutString), mCheck.SubscriptionDate)

                'Changes by Kalpesh in 13-3-2013
                'These lines commented
                '
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------

                'CNDC
                If DateDiff(DateInterval.Day, mLog.Date, maxAllowableDate) < 0 _
                    Or (IsDate(mLog.SouLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.SouLocalDateTime, maxAllowableDate) < 0) _
                    Or (IsDate(mLog.DesLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.DesLocalDateTime, maxAllowableDate) < 0) Then
                    'If DateDiff(DateInterval.Day, CDate(mLog.Date), maxAllowableDate) < 0 Or DateDiff(DateInterval.Day, CDate(mLog.SouLocalDateTime), maxAllowableDate) < 0 Or DateDiff(DateInterval.Day, CDate(mLog.DesLocalDateTime), maxAllowableDate) < 0 Then

                    '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly)
                    '''''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    '''''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")

                    DataFieldBind()
                    Exit Function
                End If
            End If
        End If
        'Authentication
        Dim LogClone As Log
        LogClone = CType(mLog.Clone, Log)

        SetObject()
        SetAirFrameGridObject()
        SetEngineGridObject(True)
        SetAPUGridObject(True)
        SetCGBGridObject(True)

        If mLog.IsValid = True Then
            If mLog.LogPageNo <> "0" Or mLog.LogPageNo <> "" Then
                Dim mPrevLogDetail As PrevLogDetail = PrevLogDetail.GetPrevLogDetail(mLog.MachineID, mLog.Date, mLog.LogPageNo)
                If mPrevLogDetail.IsTLPNODuplicate And mLog.LogNo <> mPrevLogDetail.LogNo Then
                    '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, "TLP No. already exists.", MsgBoxStyle.OkOnly)
                    '''''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    '''''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "TLP No. already exists.", MsgBoxStyle.OkOnly, "")

                    Exit Function
                End If
            End If
            Try
                If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
                    'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
                    If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
                    Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
                        '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Restriction, SIMsgBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly)
                        '''''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                        '''''msg1.Show()

                        ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                        DataBindGrid()
                        MSGBoxCtrl.Show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly, "")

                        Return False
                    End If
                End If
                mLog.ApplyEdit()
                'Add Pilot and Co-pilot in Log Crew as Child...
                'Pilot In Command
                If mLog.IsNew Then
                    If Not mLog.PilotID1.Equals(Guid.Empty) Then
                        Dim mLogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                        mLogCrew.CrewID = mLog.PilotID1
                        mLogCrew.DutyAsID = 1
                        mLog.LogCrews.Add(mLogCrew)
                    End If
                    'Co-Pilot
                    If Not mLog.PilotID2.Equals(Guid.Empty) Then
                        Dim mLogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                        mLogCrew.CrewID = mLog.PilotID2
                        mLogCrew.DutyAsID = 2
                        mLog.LogCrews.Add(mLogCrew)
                    End If
                End If
                'End 
                'Added By Vikrant on 01-Dec-2021 for PBH
                Dim IsNewLog As Boolean
                IsNewLog = mLog.IsNew
                'End
                mLog = CType(mLog.Save(), Log)
                SaveAttachment()
                'Added By Vikrant on 01-Dec-2021 for PBH
                Dim mMaxLogOfAircraft As MaxLogOfAircraft
                mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

                If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
                    If Not (AppSettings("ClientCode") = "Heligo" Or
                       AppSettings("ClientCode") = "UHPL" Or
                       AppSettings("ClientCode") = "APFT" Or
                       AppSettings("ClientCode") = "AAP") Then
                        If Not (CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime)) Then 'Last Log
                            SetPBHValues(mLog, IsNewLog)
                        End If
                    Else
                        If Not (CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate)) Then 'Last Log
                            SetPBHValues(mLog, IsNewLog)
                        End If
                    End If
                End If
                'End
                ' '' ''AJAX- New JavaScript function added to Show/Hide JQuery Date Control
                Dim IsShowDateCntrl As String
                If (Session("IsSaveAndNew") Is Nothing OrElse Session("IsSaveAndNew") <> 1) Then
                    IsShowDateCntrl = "False"
                    Session("IsSaveAndNew") = 0
                Else
                    IsShowDateCntrl = "True"
                End If

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AfterSave", "AfterSave('" + IsShowDateCntrl + "');", True)
                '-----------------------------------------------------------------------
                mLog = Log.GetLog(mLog.ID)
                mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
                MarkLog(Util.Action.Save, "Flight Log Edit", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
                '-----------------------------------------------------------------------
                Session("mLog") = mLog
                Return True
            Catch ex As SqlException
                Session("LogClone") = LogClone
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly)
                    '''''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    '''''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                    '''''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    '''''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                    '''''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    '''''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                    '''''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    '''''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 50000 Then
                    '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.LogExist, SIMsgBox.Message_text.LogExist, " between Current Date and Time Span for this Aircraft. ", MsgBoxStyle.OkOnly)
                    '''''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                    '''''msg1.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.Show(MSGBox.Message_title.LogExist, MSGBox.Message_text.Alert, "Log already entered between current Date and Time span for this Aircraft.", MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                LogClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult

        ' '' ''If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
        ' '' ''    Result1 = -1
        ' '' ''Else
        ' '' ''    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        ' '' ''End If

        ' '' ''AJAX- Here  "CType(Request.QueryString("MsgResult"), MsgBoxResult)" is replaced by "MSGBoxCtrl.Result"
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    ' '' ''AJAX- "Session("sender")" is of no use now. Replaced [CType(Session("sender"), String) -> MSGBoxCtrl.Sender] wherever used in MessageBoxResult function
                    ' '' ''If CType(Session("sender"), String) = "SaveNew" Then
                    If MSGBoxCtrl.Sender = "SaveNew" Then
                        mLog = Session("mLog")
                        DataFieldBind()
                        DataBind()

                        If mLog.IsValid Then
                            If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                            If Save() = True Then
                                NewRecord()
                                Session("mLog") = mLog
                                'Added By Vikrant on 01-Dec-2021 for PBH
                                If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                                    If Session("IsAircraftMadeNotInUse") = "True" Then
                                        Session.Remove("AircraftId")
                                        Session.Remove("IsAircraftMadeNotInUse")
                                        MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                        Exit Sub
                                    End If
                                End If
                                'End
                                DataFieldBind()

                                ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                                ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                                EnableDisableButton()
                                ControlVisibility()

                                DataBindGrid()

                                SetTitle()

                                upnlLogDetails.Update()
                                upnlFlightSummary.Update()
                                upnlTabs.Update()

                            End If
                        Else
                            ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                            upnlErrorList.Update()
                        End If
                    End If
                    '**************************************************************---------------------------------------------

                    If MSGBoxCtrl.Sender = "Close" Then

                        'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
                        If Not mLog.PilotID1.Equals(Guid.Empty) Or Not mLog.PilotID2.Equals(Guid.Empty) Then
                            Dim Title As String = "Save Alert !"
                            Dim Message As String = ""
                            If Not mLog.PilotID1.Equals(Guid.Empty) Then
                                Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID1.ToString, mLog.Date.ToString)
                                If mEmployeeStatus(0).Information <> "" Then
                                    Message = "<b>Pilot in Command : </b> <br />" & mEmployeeStatus(0).Information
                                End If
                            End If
                            If Not mLog.PilotID2.Equals(Guid.Empty) Then
                                Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID2.ToString, mLog.Date.ToString)
                                If mEmployeeStatus(0).Information <> "" Then
                                    Message = IIf(Message.Length > 0, Message & "<br/ >", "") & "<b>Co-Pilot : </b> <br />" & mEmployeeStatus(0).Information
                                End If
                            End If
                            If Message.Length > 0 Then
                                DataFieldBind()
                                Session("sender") = ""
                                '''''ClientScript.RegisterStartupScript(Me.GetType(), "OpenAlertMessage", MessageBox.Show(Title, Message))

                                ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                                MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, Message, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        End If
                        'End

                        'Added by Vikrant On 03-Aug-2015 For  ALL030812015
                        Dim mMaxLogOfAircraft As MaxLogOfAircraft
                        mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID, True)

                        If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
                            ' Commented and Added by Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
                            'If (AppSettings("ClientCode") <> "Heligo") Then
                            If Not (AppSettings("ClientCode") = "Heligo" Or
                               AppSettings("ClientCode") = "UHPL" Or
                               AppSettings("ClientCode") = "APFT" Or
                               AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
                                'End
                                Dim MaxLogDateTime As String = ""
                                'If (AppSettings("LogBookTimeEntry") = "UTC") Then
                                If mMachine.IsUTC Then '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                                    MaxLogDateTime = mMaxLogOfAircraft.SouUniverseDateTimeFormatted.ToString
                                Else
                                    MaxLogDateTime = mMaxLogOfAircraft.SouLocalDateTimeFormatted.ToString
                                End If

                                If mMaxLogOfAircraft.SouUniverseDateTime.ToString <> "" Then
                                    If CDate(mLog.LogDetails(mLog.LogDetails.Count - 1).SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime) Then 'Added by Saylee on 18-May-2012 ALL17052012
                                        Session("SaveNClose") = "SaveNClose"
                                        MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
                                        Exit Sub
                                    End If
                                End If
                            Else
                                If CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate) Then 'Added by Saylee on 18-May-2012 ALL17052012
                                    Session("SaveNClose") = "SaveNClose"
                                    MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
                                    Exit Sub
                                End If
                            End If
                        End If
                        'End

                        'Added By Prashant 12-Apr-2010
                        Dim IsMELCount As Boolean = False
                        Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
                        mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)
                        For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1
                            If mTempMELSnagCorrectiveActionList(i).IsMEL = True Then   'Added By Prashant 23-Sep-2010
                                If (CDate(mLog.Date) > CDate(mTempMELSnagCorrectiveActionList(i).DueDate)) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then
                                    IsMELCount = True
                                    Exit For
                                Else
                                    IsMELCount = False
                                End If
                            End If
                        Next
                        mTempMELSnagCorrectiveActionList = Nothing
                        '-----------------------------------
                        If IsMELCount = True Then
                            ' '' ''Dim msg1 As New SIMsgBox(Page, "Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
                            ' '' ''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                            ' '' ''Session("sender") = "MELClose"
                            ' '' ''msg1.Show()

                            ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                            DataBindGrid()
                            MSGBoxCtrl.Show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue?", "", MsgBoxStyle.YesNo, "MELClose")

                            DataBind() 'Added By Utkarsh On 12-Sep-2011
                            Exit Sub
                        Else
                            mLog = Session("mLog")
                            DataFieldBind()
                            DataBindGrid()
                            If mLog.IsValid Then
                                If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                                Session("SaveNClose") = "SaveNClose"
                                If Save() = True Then
                                    mLog = Log.GetLog(mLog.ID)
                                    mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                                    mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                                    Session("mLog") = mLog
                                    RemoveSession()
                                    'Added By Vikrant on 01-Dec-2021 for PBH
                                    If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                                        If Session("IsAircraftMadeNotInUse") = "True" Then
                                            Session.Remove("AircraftId")
                                            Session.Remove("IsAircraftMadeNotInUse")
                                            MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                            Exit Sub
                                        End If
                                    End If
                                    'End
                                    Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                                Else
                                    Exit Sub
                                End If
                            Else
                                ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                                upnlErrorList.Update()
                            End If
                        End If
                    End If

                    If MSGBoxCtrl.Sender = "SaveLogAfterHrsSame" Then 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours

                        mLog = Session("mLog")
                        Session("IsValueZero") = "True"
                        DataFieldBind()

                        If mLog.IsValid Then
                            If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                            If SaveLogAfterHrsSame() = True Then
                                If Session("New") = "True" Then
                                    Session("New") = ""
                                    NewRecord()
                                    Session("mLog") = mLog
                                    'Added By Vikrant on 01-Dec-2021 for PBH
                                    If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                                        If Session("IsAircraftMadeNotInUse") = "True" Then
                                            Session.Remove("AircraftId")
                                            Session.Remove("IsAircraftMadeNotInUse")
                                            MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                        End If
                                    End If
                                    'End
                                    DataFieldBind()

                                    ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                                    ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                                    EnableDisableButton()
                                    ControlVisibility()

                                    DataBindGrid()

                                    SetTitle()
                                    mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
                                    Session("mLogListOnDate") = mLogListOnDate
                                    If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then 'Added by Saylee on 2-Sep-2016 for ALL02092016 as per config key ShowLogDetailsOnAddingSameLogDate
                                        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
                                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
                                        upnlLogInfo.Update()
                                    End If
                                    upnlLogDetails.Update()
                                    upnlFlightSummary.Update()
                                    upnlTabs.Update()

                                Else
                                    mLog = Log.GetLog(mLog.ID)
                                    mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                                    mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                                    Session("mLog") = mLog
                                    'Added By Vikrant on 01-Dec-2021 for PBH
                                    If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                                        If Session("IsAircraftMadeNotInUse") = "True" Then
                                            Session.Remove("AircraftId")
                                            Session.Remove("IsAircraftMadeNotInUse")
                                            MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                        End If
                                    End If
                                    'End
                                    SetTitle()
                                    DataFieldBind()
                                    DataBindGrid()
                                    EnableDisableButton()
                                    ControlVisibility()
                                    upnlLogDetails.Update()
                                    If Session("SaveNClose") = "SaveNClose" Then
                                        Session("SaveNClose") = ""
                                        Session.Remove("SaveNClose")
                                        RemoveSession()
                                        Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                                    End If
                                End If
                            End If
                        Else
                            ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                            upnlErrorList.Update()
                        End If

                    End If


                    If MSGBoxCtrl.Sender = "MELNew" Then

                        mLog = Session("mLog")
                        DataFieldBind()
                        DataBind()

                        If mLog.IsValid Then
                            If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                            Session("New") = "True"
                            If Save() = True Then
                                NewRecord()
                                Session("mLog") = mLog
                                'Added By Vikrant on 01-Dec-2021 for PBH
                                If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                                    If Session("IsAircraftMadeNotInUse") = "True" Then
                                        Session.Remove("AircraftId")
                                        Session.Remove("IsAircraftMadeNotInUse")
                                        MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                        Exit Sub
                                    End If
                                End If
                                'End
                                DataFieldBind()

                                ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                                ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                                EnableDisableButton()
                                ControlVisibility()

                                DataBindGrid()

                                SetTitle()
                                mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
                                Session("mLogListOnDate") = mLogListOnDate
                                If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then 'Added by Saylee on 2-Sep-2016 for ALL02092016 as per config key ShowLogDetailsOnAddingSameLogDate
                                    '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
                                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
                                    upnlLogInfo.Update()
                                End If
                                upnlLogDetails.Update()

                                upnlFlightSummary.Update()
                                upnlTabs.Update()

                            End If
                        Else
                            ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                            upnlErrorList.Update()
                        End If
                    End If

                    If MSGBoxCtrl.Sender = "MELClose" Then

                        mLog = Session("mLog")
                        DataFieldBind()
                        DataBindGrid()
                        DataBind()

                        If mLog.IsValid Then
                            If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                            If Save() = True Then
                                mLog = Log.GetLog(mLog.ID)
                                mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                                mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                                Session("mLog") = mLog
                                RemoveSession()
                                'Added By Vikrant on 01-Dec-2021 for PBH
                                If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                                    If Session("IsAircraftMadeNotInUse") = "True" Then
                                        Session.Remove("AircraftId")
                                        Session.Remove("IsAircraftMadeNotInUse")
                                        MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                        Exit Sub
                                    End If
                                End If
                                'End
                                Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                            End If
                        Else
                            ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                            upnlErrorList.Update()
                        End If
                    End If

                    If MSGBoxCtrl.Sender = "SaveLogFlexiLog" Then 'Added by Vikrant On 03-Aug-2015 For  ALL030812015

                        mLog = Session("mLog")
                        DataFieldBind()
                        DataBindGrid()
                        ''DataBind()

                        If mLog.IsValid Then
                            If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                            If SaveLogFlexiLog() = True Then
                                If Session("New") = "True" Then
                                    Session("New") = ""
                                    NewRecord()
                                    Session("mLog") = mLog
                                    'Added By Vikrant on 01-Dec-2021 for PBH
                                    If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                                        If Session("IsAircraftMadeNotInUse") = "True" Then
                                            Session.Remove("AircraftId")
                                            Session.Remove("IsAircraftMadeNotInUse")
                                            MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                        End If
                                    End If
                                    'End

                                    DataFieldBind()
                                    DataBindGrid()
                                    EnableDisableButton()
                                    ControlVisibility()



                                    SetTitle()
                                    mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
                                    Session("mLogListOnDate") = mLogListOnDate
                                    If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then 'Added by Saylee on 2-Sep-2016 for ALL02092016 as per config key ShowLogDetailsOnAddingSameLogDate
                                        '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
                                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
                                        upnlLogInfo.Update()
                                    End If
                                    upnlLogDetails.Update()
                                    upnlFlightSummary.Update()
                                    upnlTabs.Update()
                                Else
                                    mLog = Log.GetLog(mLog.ID)
                                    mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                                    mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                                    Session("mLog") = mLog
                                    'Added By Vikrant on 01-Dec-2021 for PBH
                                    If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                                        If Session("IsAircraftMadeNotInUse") = "True" Then
                                            Session.Remove("AircraftId")
                                            Session.Remove("IsAircraftMadeNotInUse")
                                            MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                        End If
                                    End If
                                    'End
                                    SetTitle()
                                    DataFieldBind()
                                    DataBindGrid()
                                    EnableDisableButton()
                                    ControlVisibility()
                                    upnlLogDetails.Update()
                                    If Session("SaveNClose") = "SaveNClose" Then
                                        Session("SaveNClose") = ""
                                        Session.Remove("SaveNClose")
                                        RemoveSession()
                                        Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                                    End If
                                End If
                            End If
                        Else
                            upnlErrorList.Update()
                        End If
                    End If
                    'End

                    If MSGBoxCtrl.Sender = "MEL" Then

                        mLog = Session("mLog")
                        DataFieldBind()
                        DataBind()

                        If mLog.IsValid Then
                            If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                            If Save() = True Then
                                mLog = Log.GetLog(mLog.ID)
                                mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                                mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                                Session("mLog") = mLog
                                'Added By Vikrant on 01-Dec-2021 for PBH
                                If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                                    If Session("IsAircraftMadeNotInUse") = "True" Then
                                        Session.Remove("AircraftId")
                                        Session.Remove("IsAircraftMadeNotInUse")
                                        MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                        Exit Sub
                                    End If
                                End If
                                'End
                                ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                                ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                                DataFieldBind()

                                EnableDisableButton()
                                ControlVisibility()

                                DataBindGrid()

                                SetTitle()

                                upnlLogDetails.Update()
                                upnlFlightSummary.Update()
                                upnlTabs.Update()

                            End If
                        Else
                            ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                            upnlErrorList.Update()
                        End If
                    End If

                    If MSGBoxCtrl.Sender = "Remove" Then
                        mLog.LogDetails.Remove(CType(Session("ID"), Guid))
                        SetLogObject()
                        ' '' ''AJAX- New JavaScript function added to Show/Hide JQuery Date Control
                        Dim IsShowDateCntrl As String
                        If mLog.IsNew AndAlso mLog.LogDetails.Count = 0 Then
                            IsShowDateCntrl = "True"
                        Else
                            IsShowDateCntrl = "False"
                        End If

                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AfterSave", "AfterSave('" + IsShowDateCntrl + "');", True)
                        'End
                        If Not mLog.IsNew And mLog.IsValid Then
                            mLog = CType(mLog.Save(), Log)
                            'Added By Vikrant on 01-Dec-2021 for PBH
                            Dim mMaxLogOfAircraft As MaxLogOfAircraft
                            mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)


                            If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
                                If Not (AppSettings("ClientCode") = "Heligo" Or
                                   AppSettings("ClientCode") = "UHPL" Or
                                   AppSettings("ClientCode") = "APFT" Or
                                   AppSettings("ClientCode") = "AAP") Then
                                    If Not (CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime)) Then 'Last Log
                                        SetPBHValues(mLog, False)
                                    End If
                                Else
                                    If Not (CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate)) Then 'Last Log
                                        SetPBHValues(mLog, False)
                                    End If
                                End If
                            End If
                            'End
                        End If
                        Session("mLog") = mLog
                        ControlVisibility()
                        EnableDisableButton()
                        DataFieldBind()
                        DataBindGrid()
                    End If

                Case MsgBoxResult.No
                    If Session("New") = "True" Then Session("New") = ""
                    If MSGBoxCtrl.Sender = "SaveNew" Then
                        NewRecord()
                        DataFieldBind()
                        DataBindGrid()
                        ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                        ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                    If MSGBoxCtrl.Sender = "SaveLogAfterHrsSame" Then
                        Session.Remove("IsValueZero")
                        ControlVisibility()
                        EnableDisableButton()
                        DataFieldBind()
                        DataBindGrid()
                        ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                        ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                    If MSGBoxCtrl.Sender = "SaveLogAfterHrsSame" Then
                        Session.Remove("IsValueZero")
                        ControlVisibility()
                        EnableDisableButton()
                        DataFieldBind()
                        DataBindGrid()
                        ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                        ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If

                    If MSGBoxCtrl.Sender = "Remove" Then
                        ControlVisibility()
                        EnableDisableButton()
                        DataFieldBind()
                        DataBindGrid()
                        ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                        ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                    If MSGBoxCtrl.Sender = "MELClose" Then
                        ControlVisibility()
                        EnableDisableButton()
                        DataFieldBind()
                        DataBindGrid()
                        RemoveSession()
                        Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                    If MSGBoxCtrl.Sender = "MEL" Or MSGBoxCtrl.Sender = "MELNew" Then
                        ControlVisibility()
                        EnableDisableButton()
                        DataFieldBind()
                        DataBindGrid()
                        ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                        ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session("SaveNClose") = ""
                        Session.Remove("SaveNClose")
                        ControlVisibility()
                        EnableDisableButton()
                        DataFieldBind()
                        DataBindGrid()
                        RemoveSession()
                        Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If

                Case MsgBoxResult.Cancel

                    If MSGBoxCtrl.Sender = "Save" Or MSGBoxCtrl.Sender = "SaveNew" Then
                        ControlVisibility()
                        EnableDisableButton()
                        ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                        ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                Case MsgBoxResult.Ok
                    'Added By Vikrant on 01-Dec-2021 for PBH
                    If MSGBoxCtrl.Sender = "AircraftMadeNotInUse" Then
                        Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Exit Sub
                    End If
                    'End
                    ControlVisibility()
                    EnableDisableButton()

                    DataFieldBind()
                    DataBindGrid()
                    ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                    ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
                    DataBindGrid()
                    ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                    ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            If Session("New") = "True" Then Session("New") = ""
            ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
            ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 Then
            If Session("New") = "True" Then Session("New") = ""
        End If
    End Sub

    '''''Private Sub CHECK_isRequiredAssembliesInstalled()
    '''''    If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
    '''''        'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
    '''''        If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
    '''''        Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then

    '''''            '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.EntryRestriction, SIMsgBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly)
    '''''            '''''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
    '''''            '''''msg1.Show()

    '''''            MSGBoxCtrl.show(MSGBox.Message_title.EntryRestriction, MSGBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly, "")

    '''''            Exit Sub
    '''''        End If
    '''''    End If

    '''''    Dim tmpAssemblyStatusList As tmpAssemblyStatusList = tmpAssemblyStatusList.GetAssemblyStatusList(Now.ToShortDateString, mMachine.ID, True)
    '''''    Dim IsAirFrameAvailable As Boolean = False
    '''''    Dim IsEngineAvailable As Boolean = False
    '''''    Dim AssembliesNotFound As String = ""
    '''''    Dim Obj As tmpAssemblyStatusList.tmpAssemblyStatusInfo
    '''''    For Each Obj In tmpAssemblyStatusList
    '''''        If Obj.AssemblyTypeID = 1 Then IsAirFrameAvailable = True
    '''''        If Obj.AssemblyTypeID = 2 Then IsEngineAvailable = True
    '''''    Next
    '''''    If (Not (IsAirFrameAvailable And IsEngineAvailable)) Then
    '''''        If IsEngineAvailable = False Then AssembliesNotFound = "Engine"
    '''''        If IsAirFrameAvailable = False Then AssembliesNotFound = AssembliesNotFound + IIf(AssembliesNotFound = "", "Machine", ",Machine").ToString

    '''''        '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.EntryRestriction, SIMsgBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly)
    '''''        '''''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
    '''''        '''''msg1.Show()

    '''''        MSGBoxCtrl.show(MSGBox.Message_title.EntryRestriction, MSGBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly, "")

    '''''        Exit Sub
    '''''    End If
    '''''End Sub
    Private Sub SetTitle()
        Dim Index As Integer
        Index = Session("Index")

        If mLog.IsNew Then
            If mLog.Date Is DBNull.Value Then
                lblTitle.Text = "Log Details of " & mMachine.RegNo & " as of - [New]"
            Else
                lblTitle.Text = "Log Details of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"
            End If
        Else
            lblTitle.Text = "Log Details of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText
        End If

        upnlTitle.Update()  ' '' ''AJAX- call "upnlTitle.Update" to show changes in title 
    End Sub
    Private Sub addAttributes()
        ' txtLogPageNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtLogPageNo').value)")
    End Sub
    Private Sub NewRecord(ByVal LogDate As String, Optional ByVal mSouLocalDateTime As String = "", Optional ByVal mSouUTCDateTime As String = "")
        mLog = Log.NewLog(mMachine, LogDate, mSouLocalDateTime, mSouUTCDateTime)
        'mLog.BeginEdit()
        mMachine = Machine.GetMachine(mMachine.ID)
        DataBind()
        '''''CHECK_isRequiredAssembliesInstalled()
    End Sub
    Private Sub EditRecord(ByVal LogDate As DateTime)
        mLog = Log.GetLog(mLog.ID)
        'mLog.BeginEdit()
        mLog.Date = LogDate
        DataBind()
        '''''CHECK_isRequiredAssembliesInstalled()
    End Sub
    Private Sub CopyFromClone(ByVal ClonedLog As Log, Optional ByVal isFromLogDate As Boolean = False)
        mLog.PilotID1 = ClonedLog.PilotID1
        mLog.PilotID2 = ClonedLog.PilotID2
        mLog.IsUTC = ClonedLog.IsUTC
        mLog.SourceID = ClonedLog.SourceID


        If Not mLog.IsNew Then
            mLog.SouLocalDateTime = ClonedLog.SouLocalDateTime
            mLog.SouDayLightTime = ClonedLog.SouDayLightTime
            mLog.DesLocalDateTime = ClonedLog.DesLocalDateTime
            mLog.DesDayLightTime = ClonedLog.DesDayLightTime
        End If


        mLog.DestinationID = ClonedLog.DestinationID
        If mLog.IsUTC Then
            If Not isFromLogDate Then
                mLog.SouUniverseDateTime = ClonedLog.SouUniverseDateTime
            End If
        End If
        If isFromLogDate Then
            mLog.DesLocalDateTime = ClonedLog.DesLocalDateTime
            mLog.DesDayLightTime = ClonedLog.DesDayLightTime
            If TakeOffTouchDown Then
                mLog.TouchDownLocalDateTime = mLog.DesLocalDateTime
            End If
        End If

        If Not TakeOffTouchDown Then
            mLog.TimeOnGround = ClonedLog.TimeOnGround
            mLog.PercentTimeOnGround = ClonedLog.PercentTimeOnGround
            mLog.TimeInAir = ClonedLog.TimeInAir
        End If

        mLog.Remark = ClonedLog.Remark

        mLog.LogPageNo = ClonedLog.LogPageNo
        mLog.FlightNo = ClonedLog.FlightNo
        mLog.FlightLogClassificationID = ClonedLog.FlightLogClassificationID
        mLog.FlightLogClassificationName = ClonedLog.FlightLogClassificationName

        'Hobbs - taken
        mLog.CurrentHobbsValue = ClonedLog.CurrentHobbsValue
        mLog.OffSet = ClonedLog.OffSet

        'Added by Yogita Ajax
        mLog.ImageFile = ClonedLog.ImageFile
        mLog.ImageSize = ClonedLog.ImageSize
        mLog.FileExtension = ClonedLog.FileExtension
        '--------------------------------------
        Session("mLog") = mLog
    End Sub

    Private Sub SetFromAutoComplete()
        Dim tempString As String
        Dim tempString1 As String

        tempString = Place1.Text.Trim
        If Not tempString = String.Empty Then
            If tempString.IndexOf("[") >= 0 Then
                tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
            End If
        End If

        tempString1 = Place2.Text.Trim
        If Not tempString1 = String.Empty Then
            If tempString1.IndexOf("[") >= 0 Then
                tempString1 = tempString1.Substring(0, tempString1.IndexOf("[")).Trim
            End If
        End If

        'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
        If AppSettings("ClientCode") = "Heligo" Or
           AppSettings("ClientCode") = "UHPL" Or
           AppSettings("ClientCode") = "APFT" Or
           AppSettings("ClientCode") = "AAP" Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
            mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
            mLog.Pilot1Name = "None"
        Else
            mLog.PilotID1 = mSearchListPilot.Item(Pilot1.Text.Trim).GId
            mLog.Pilot1Name = mSearchListPilot.Item(Pilot1.Text.Trim).Name
        End If

        mLog.PilotID2 = mSearchListPilot.Item(Pilot2.Text.Trim).GId
        mLog.Pilot2Name = mSearchListPilot.Item(Pilot2.Text.Trim).Name

    End Sub

    Private Sub EditLogDetailRecord(ByVal SrNo As Integer)
        Dim mlogdetail As LogDetail
        mlogdetail = mLog.LogDetails.Item(SrNo)
        Session("mLogDetail") = mlogdetail
        Session("mRegNo") = mMachine.RegNo

        'Added by Saylee on 29-Apr-2022
        Dim clnLogDetail As LogDetail
        Dim clnLogTLP As Log

        clnLogDetail = CType(mlogdetail.Clone, LogDetail)
        Session("clnLogDetail") = clnLogDetail

        clnLogTLP = CType(mLog.Clone, Log)
        Session("clnLogTLP") = clnLogTLP
        '**************************************************
        Response.Redirect("wfTLPDetailEdit_Ajax.aspx?BackPage=wfTLPEdit_Ajax.aspx")
    End Sub

    Private Sub SetLogObject()
        If Not mLog.LogDetails.Count = 0 Then
            'Time
            mLog.BlockTime = mLog.LogDetails.TotalBlockTime
            mLog.TimeInAir = mLog.LogDetails.TotalTimeInAir

            mLog.TimeOnGround = mLog.LogDetails.TotalTimeOnGround
            mLog.TotalTime = mLog.LogDetails.TotalTime


            'Fuel
            If mLog.LogFuels.Count > 0 Then
                mLog.LogFuels.CurrentItem.FuelUplifted = mLog.LogDetails.TotalFuelUplifted
                mLog.LogFuels.CurrentItem.FuelOnArrival = mLog.LogDetails(mLog.LogDetails.Count - 1).FuelOnArrival 'set Last child Fule at arrival to Log Fuel's Last Child
            End If

            'Total Landings
            mLog.TotalLandings = mLog.LogDetails.TotalLandings

            'Place
            mLog.SourceID = mLog.LogDetails(0).SourceID  'IF Source place changed

            mLog.DestinationID = mLog.LogDetails(mLog.LogDetails.Count - 1).DestinationID
        Else
            'Set Default

            'Time
            mLog.BlockTime = "0:00"
            mLog.TimeInAir = "0:00"
            mLog.TimeOnGround = "0:00"
            mLog.TotalTime = "0:00"

            'Fuel
            If mLog.LogFuels.Count > 0 Then
                mLog.LogFuels.CurrentItem.FuelUplifted = 0D
                mLog.LogFuels.CurrentItem.FuelOnArrival = 0D 'set Last child Fule at arrival to Log Fuel's Last Child
            End If

            'Total Landings
            mLog.TotalLandings = 0D

            'Place
            'mLog.SourceID = mLog.LogDetails(0).SourceID  'IF Source place changed
            mLog.DestinationID = Guid.Empty
        End If
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mLog.IsAttachmentAdded = True
            Else
                mLog.IsAttachmentAdded = False
            End If
        End If
        Session("mLog") = mLog
    End Sub
    'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013
    Private Function CheckZeroDifferenceValue() As Boolean
        If mLog.IsHobbs Then
            If Val(mLog.TotalTime) <> 0 Then
                Return False
            End If
            If Val(mLog.TimeInAir) <> 0 Then
                Return False
            End If
        Else
            If mLog.TimeInAir = "0:00" OrElse mLog.TimeInAir = "" Then
            Else
                Return False
            End If
            If mLog.TotalTime = "0:00" OrElse mLog.TotalTime = "" Then
            Else
                Return False
            End If
            If mLog.BlockTime = "0:00" OrElse mLog.BlockTime = "" Then
            Else
                Return False
            End If
            If mLog.TimeOnGround = "0:00" OrElse mLog.TimeOnGround = "" Then
            Else
                Return False
            End If
        End If
        If Val(mLog.TotalLandings) <> 0 Then
            Return False
        End If

        Dim checkcol = mLog.LogAFAssemblies
        If Not callZeroDifferenceValue(checkcol) Then
            Return False
        End If
        checkcol = mLog.LogAPUAssemblies
        If Not callZeroDifferenceValue(checkcol) Then
            Return False
        End If
        checkcol = mLog.LogEngAssemblies
        If Not callZeroDifferenceValue(checkcol) Then
            Return False
        End If
        checkcol = mLog.LogCGBAssemblies
        If Not callZeroDifferenceValue(checkcol) Then
            Return False
        End If
        Return True
    End Function
    Private Function callZeroDifferenceValue(ByVal obj) As Boolean
        For i As Integer = 0 To obj.Count - 1
            If mLog.IsHobbs Then
                If Val(obj(i).Hours) <> 0 Then
                    Return False
                End If
            Else
                If obj(i).Hours <> "0:00" Then
                    Return False
                End If
            End If
            If obj(i).ShowLandings Then
                If Val(obj(i).Landings) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowCycles Then
                If Val(obj(i).Cycles) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowStarts Then
                If Val(obj(i).Starts) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowNGCycles Then
                If Val(obj(i).NGCycles) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowNFCycles Then
                If Val(obj(i).NFCycles) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowRINS Then
                If Val(obj(i).RINS) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowBleeds Then
                If Val(obj(i).Bleeds) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowImpellerCycles Then
                If Val(obj(i).ImpellerCycles) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowCTCycles Then
                If Val(obj(i).CTCycles) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowPTCycles Then
                If Val(obj(i).PTCycles) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowGeneratorMods Then
                If Val(obj(i).GeneratorMods) <> 0 Then
                    Return False
                End If
            End If
        Next
        Return True
    End Function
    'End
#End Region

#Region " Data Binding "
    Private Sub GridColumnHeadingSet()
        'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
        If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then
            dgAFPeriods.Columns(7).HeaderText = "Flights"
            dgAFPeriods.Columns(8).HeaderText = "Final Flights"
            dgEnginePeriods.Columns(7).HeaderText = "Flights"
            dgEnginePeriods.Columns(8).HeaderText = "Final Flights"
            dgAPUPeriods.Columns(7).HeaderText = "Flights"
            dgAPUPeriods.Columns(8).HeaderText = "Final Flights"
            dgCGBPeriods.Columns(7).HeaderText = "Flights"
            dgCGBPeriods.Columns(8).HeaderText = "Final Flights"
            lblCGBPeriod.Text = "CGB Period"
            'Dim txtCGBHours, txtCGBLandings, txtCGBCycles As TextBox, txtCGBStarts, txtCGBNGCycles, txtCGBNFCycles, txtCGBRins, txtCGBBleeds, txtCGBImpellerCycles, txtCGBCTCycles, txtCGBPTCycles As TextBox

            ' '' ''AJAX- DataGrid is replaced by GridView for removing "Refresh" buttons. So here "dgCGBPeriods.Items" is replaced by "dgCGBPeriods.Rows"
            For l As Integer = 0 To dgCGBPeriods.Rows.Count - 1
                CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBHours"), TextBox).ReadOnly = True
                CType(dgCGBPeriods.Rows(l).FindControl("txtCGBLandings"), TextBox).ReadOnly = True
                CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBCycles"), TextBox).ReadOnly = True
                CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBStarts"), TextBox).ReadOnly = True
                CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBNGCycles"), TextBox).ReadOnly = True
                CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBNFCycles"), TextBox).ReadOnly = True
                CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBRins"), TextBox).ReadOnly = True
                CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBBleeds"), TextBox).ReadOnly = True
                CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBImpellerCycles"), TextBox).ReadOnly = True
                CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBCTCycles"), TextBox).ReadOnly = True
                CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBPTCycles"), TextBox).ReadOnly = True
            Next l
        End If

        dgLogDetails.Columns(5).Visible = Not mLog.IsUTC
        dgLogDetails.Columns(6).Visible = mLog.IsUTC
        dgLogDetails.Columns(7).Visible = Not mLog.IsUTC
        dgLogDetails.Columns(8).Visible = mLog.IsUTC

        dgLogDetails.Columns(10).Visible = Not mLog.IsUTC
        dgLogDetails.Columns(11).Visible = mLog.IsUTC
        dgLogDetails.Columns(12).Visible = Not mLog.IsUTC
        dgLogDetails.Columns(13).Visible = mLog.IsUTC



    End Sub
    Private Sub DataFieldBind()
        dgAFPeriods.DataSource = mLog.LogAFAssemblies
        dgEnginePeriods.DataSource = mLog.LogEngAssemblies
        dgAPUPeriods.DataSource = mLog.LogAPUAssemblies
        dgCGBPeriods.DataSource = mLog.LogCGBAssemblies

        grdAllAssemblies.DataSource = mLog.ALL_LogAssemblies ''Added by Saylee on 1-Mar-2022

        txtLogNo.Text = mLog.LogNo
        txtLogText.Text = mLog.LogText

        If Not mLog.Date Is System.DBNull.Value Then
            calDateTime.Text = Format(CDate(mLog.Date), AppSettings("DateFormat"))
        Else
            calDateTime.Text = ""
        End If


        If TakeOffTouchDown Then
            txtBlockTime.Text = mLog.BlockTime
            txtGroundRunTime.Text = mLog.TimeOnGround
        Else
            txtBlockTime.Text = mLog.DiffTime
        End If


        mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("", "<SELECT>")
        cmbFlightLogClassification.DataSource = mFlightLogClassificationList
        Session("mFlightLogClassificationList") = mFlightLogClassificationList

        mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
        Session("mLogListOnDate") = mLogListOnDate

        DataBind()
        GridColumnHeadingSet()

        If cmbFlightLogClassification.Items.Contains(New System.Web.UI.WebControls.ListItem(mLog.FlightLogClassificationName, mLog.FlightLogClassificationID.ToString)) Then
            cmbFlightLogClassification.SelectedValue = mLog.FlightLogClassificationID.ToString
        Else
            cmbFlightLogClassification.SelectedValue = Guid.Empty.ToString
        End If

        mSearchListPilot = SearchList.GetSearchList("Pilot", "", "")
        Session("mSearchListPilot") = mSearchListPilot

        Pilot1.Text = mLog.Pilot1Name
        Pilot2.Text = mLog.Pilot2Name


        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "") 'PBH Collective Hrs by Saylee on 30-Nov-2022
        Session("mCompanyDetail") = mCompanyDetail

        ' '' ''AJAX- In DataFieldBind we binds object values to various controls. To reflect values we have call ".Update()" method of respective Panel
        upnlLogDetails.Update()

        upnlFlightSummary.Update()

        upnlAirframeDetail.Update()
        upnlEngineDetail.Update()
        upnlAPUDetail.Update()
        upnlCGBDetail.Update()

        upnlRemark.Update()
    End Sub
    Private Sub BindClassification()
        mLog = CType(Session("mLog"), Log)
        mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("", "<SELECT>")
        cmbFlightLogClassification.DataSource = mFlightLogClassificationList
        Session("mFlightLogClassificationList") = mFlightLogClassificationList
        cmbFlightLogClassification.DataBind()
        If cmbFlightLogClassification.Items.Contains(New System.Web.UI.WebControls.ListItem(mLog.FlightLogClassificationName, mLog.FlightLogClassificationID.ToString)) Then
            cmbFlightLogClassification.SelectedValue = mLog.FlightLogClassificationID.ToString
        Else
            cmbFlightLogClassification.SelectedValue = Guid.Empty.ToString
        End If

        upnlLogDetails.Update()
    End Sub
    Private Sub DataBindValuesGrid()
        If Not mLog Is Nothing Then

            SetAirFrameGridObject()
            SetEngineGridObject(True)
            SetAPUGridObject(True)
            SetCGBGridObject(True)

            dgAFPeriods.DataSource = mLog.LogAFAssemblies
            dgAFPeriods.DataBind()

            dgEnginePeriods.DataSource = mLog.LogEngAssemblies
            dgEnginePeriods.DataBind()

            dgAPUPeriods.DataSource = mLog.LogAPUAssemblies
            dgAPUPeriods.DataBind()

            dgCGBPeriods.DataSource = mLog.LogCGBAssemblies
            dgCGBPeriods.DataBind()

            GridColumnHeadingSet()

            ' '' ''AJAX- In DataFieldBind we binds object values to various controls. To reflect values we have call ".Update()" method of respective Panel

            upnlAirframeDetail.Update()
            upnlEngineDetail.Update()
            upnlAPUDetail.Update()
            upnlCGBDetail.Update()

            Session("mLog") = mLog
        End If
    End Sub
    Private Sub DataBindGrid()
        If Not mLog Is Nothing Then

            SetAirFrameGridObject(True)
            SetEngineGridObject(True)
            SetAPUGridObject(True)
            SetCGBGridObject(True)

            dgLogDetails.DataSource = mLog.LogDetails
            dgLogDetails.DataBind()

            dgAFPeriods.DataSource = mLog.LogAFAssemblies
            dgAFPeriods.DataBind()

            dgEnginePeriods.DataSource = mLog.LogEngAssemblies
            dgEnginePeriods.DataBind()

            dgAPUPeriods.DataSource = mLog.LogAPUAssemblies
            dgAPUPeriods.DataBind()

            dgCGBPeriods.DataSource = mLog.LogCGBAssemblies
            dgCGBPeriods.DataBind()

            GridColumnHeadingSet()

            ' '' ''AJAX- In DataFieldBind we binds object values to various controls. To reflect values we have call ".Update()" method of respective Panel
            upnlLogDetailsGrid.Update()

            upnlAirframeDetail.Update()
            upnlEngineDetail.Update()
            upnlAPUDetail.Update()
            upnlCGBDetail.Update()

            Session("mLog") = mLog
        End If
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        GridColumnHeadingSet()
        Dim tempString As String 'Added By Utkarsh On 24-Nov-2011 For ALL23112011

        If custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 500 Then
                custValidator.ErrorMessage = "Max. length of Remark should be 500 char"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "Pilot1" Then

            If Not mSearchListPilot.Contains(Pilot1.Text.Trim) Then
                custValidator.ErrorMessage = "Enter correct Pilot1 name."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "Place1" Then
            'Added by Utkarsh On 24-Nov-2011 For ALL23112011
            tempString = Place1.Text.Trim
            If Not tempString = String.Empty Then
                'tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
                If tempString.IndexOf("[") < 0 Then
                    custValidator.ErrorMessage = "Enter correct Source name."
                    e.IsValid = False
                Else
                    tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
                    'If Not mSearchListPlace.Contains(tempString) Then
                    '    custValidator.ErrorMessage = "Enter correct Source name."
                    '    e.IsValid = False
                    'Else
                    '    e.IsValid = True
                    'End If
                End If
            End If
            'End
        ElseIf custValidator.ControlToValidate = "Pilot2" Then
            If Not mSearchListPilot.Contains(Pilot2.Text.Trim) Then
                custValidator.ErrorMessage = "Enter correct Pilot2 name."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        ElseIf custValidator.ControlToValidate = "Place2" Then
            'Added by Utkarsh On 24-Nov-2011 For ALL23112011
            tempString = Place2.Text.Trim
            If Not tempString = String.Empty Then
                '  tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
                If tempString.IndexOf("[") < 0 Then
                    custValidator.ErrorMessage = "Enter correct Destination name."
                    e.IsValid = False
                Else
                    tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
                    'If Not mSearchListPlace.Contains(tempString) Then
                    '    custValidator.ErrorMessage = "Enter correct Destination name."
                    '    e.IsValid = False
                    'Else
                    '    e.IsValid = True
                    'End If
                End If
            End If
            'End
            '''''ElseIf custValidator.ControlToValidate = "txtLogPageNo" Then
            '''''    If txtLogPageNo.Text.Trim = "0" Or txtLogPageNo.Text.Trim = "" Then
            '''''        custValidator.ErrorMessage = "Enter TLP No."
            '''''        e.IsValid = False
            '''''    Else
            '''''        e.IsValid = True
            '''''    End If
            ''Added by Saylee on 28-Mar-2014 For BA28032014
        ElseIf custValidator.ControlToValidate = "cmbFlightLogClassification" Then
            If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "YA") Then
                If cmbFlightLogClassification.SelectedIndex = 0 Then
                    custValidator.ErrorMessage = "Please select Classification."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            End If

        End If
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs) ' Validation From AIRFRAMEGRID (Grid-1)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        SetObject()
        SetAirFrameGridObject()
        SetEngineGridObject()
        SetAPUGridObject()
        SetCGBGridObject()
        GridColumnHeadingSet()
        Dim str As String = ""
        'Log
        If Not mLog.IsValid Then
            For i As Integer = 0 To mLog.GetBrokenRulesCollection.Count - 1
                str = str + mLog.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        'AirFrame
        For i As Integer = 0 To mLog.LogAFAssemblies.Count - 1
            If Not mLog.LogAFAssemblies(i).IsValid Then
                Dim x As Integer
                For x = 0 To mLog.LogAFAssemblies(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.LogAFAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        'Engine
        For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1
            If Not mLog.LogEngAssemblies(i).IsValid Then
                Dim x As Integer
                For x = 0 To mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        'APU
        For i As Integer = 0 To mLog.LogAPUAssemblies.Count - 1
            If Not mLog.LogAPUAssemblies(i).IsValid Then
                Dim x As Integer
                For x = 0 To mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False

        End If
        Flag = 1
    End Sub
    Public Function CustomValidate2() As Boolean    'For DgLog Fuel Oils
        Dim str As String = ""
        'AirFrame
        For i As Integer = 0 To mLog.LogAFAssemblies.Count - 1
            If Not mLog.LogAFAssemblies(i).IsValid Then
                Dim x As Integer
                For x = 0 To mLog.LogAFAssemblies(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.LogAFAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        'Engine
        For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1
            If Not mLog.LogEngAssemblies(i).IsValid Then
                Dim x As Integer
                For x = 0 To mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        'APU
        For i As Integer = 0 To mLog.LogAPUAssemblies.Count - 1
            If Not mLog.LogAPUAssemblies(i).IsValid Then
                Dim x As Integer
                For x = 0 To mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next

        'Log Oils
        For i As Integer = 0 To mLog.LogOils.Count - 1
            If Not mLog.LogOils(i).IsValid Then
                For j As Integer = 0 To mLog.LogOils(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.LogOils.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
                Next
            End If
        Next
        For i As Integer = 0 To mLog.FuelUpLifts.Count - 1
            If Not mLog.FuelUpLifts(i).IsValid Then
                For j As Integer = 0 To mLog.FuelUpLifts(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.FuelUpLifts.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
                Next
            End If
        Next
        For i As Integer = 0 To mLog.LogFuels.Count - 1
            If Not mLog.LogFuels(i).IsValid Then
                For j As Integer = 0 To mLog.LogFuels(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.LogFuels.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
                Next
            End If
        Next

        If str <> "" Then
            cvRemark.ErrorMessage = str
            cvRemark.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        TakeOffTouchDown = CType(AppSettings("TakeOffTouchDown"), Boolean)
        mLog.IsTakeoffTouchDown = TakeOffTouchDown
        EventLogID = CType(Session("EventLogID"), Guid)
        addAttributes()
        If Not IsPostBack Then 'And CType(Session("sender"), String) = ""
            If calDateTime.Enabled = True Then
                setFocus(calDateTime)
            End If
            DataFieldBind()
            ControlVisibilityForAttachment()

            If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then 'Added by Saylee on 2-Sep-2016 for ALL02092016 as per config key ShowLogDetailsOnAddingSameLogDate
                'Added by Saylee on 11-Apr-2016 Then
                '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
                upnlLogInfo.Update()
            End If
            upnlLogDetails.Update()
            DataBindGrid()
        End If
        EnableDisableButton()
        ControlVisibility()

        ' '' ''AJAX- "MessageBoxResult()" is commented here and called from new User Control Delegate event present at the bottom "MsgBoxCtrl_UserControlButtonClicked"
        ' '' ''MessageBoxResult()

        DataBindValuesGrid()
        SetTitle()
        SetFromAutoComplete()
        mLog.LogPageNo = txtLogPageNo.Text.Trim
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.

        If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then
            BindClassification()
            SetObject()
            SetSession()
            mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
            MarkLog(Util.Action.Save, "Flight Log Edit", User.Identity.Name & " is not Authorized User to save " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            ' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            ' '' ''msg.ReplacePage = "wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            ' '' ''Session("sender") = "Authorization"
            ' '' ''msg.Show()

            ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

            Exit Sub
        End If

        If Not IsValid Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.

        'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
        If Not mLog.PilotID1.Equals(Guid.Empty) Or Not mLog.PilotID2.Equals(Guid.Empty) Then
            Dim Title As String = "Save Alert !"
            Dim Message As String = ""
            If Not mLog.PilotID1.Equals(Guid.Empty) Then
                Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID1.ToString, mLog.Date.ToString)
                If mEmployeeStatus(0).Information <> "" Then
                    Message = "<b>Pilot in Command : </b> <br />" & mEmployeeStatus(0).Information
                End If
            End If
            If Not mLog.PilotID2.Equals(Guid.Empty) Then
                Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID2.ToString, mLog.Date.ToString)
                If mEmployeeStatus(0).Information <> "" Then
                    Message = IIf(Message.Length > 0, Message & "<br/ >", "") & "<b>Co-Pilot : </b> <br />" & mEmployeeStatus(0).Information
                End If
            End If
            If Message.Length > 0 Then
                ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, Message, MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If
        'End

        'Added by Vikrant On 03-Aug-2015 For  ALL030812015
        Dim mMaxLogOfAircraft As MaxLogOfAircraft
        mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID, True)

        If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
            ' Commented and Added by Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
            'If (AppSettings("ClientCode") <> "Heligo") Then
            If Not (AppSettings("ClientCode") = "Heligo" Or
               AppSettings("ClientCode") = "UHPL" Or
               AppSettings("ClientCode") = "APFT" Or
               AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
                'End
                Dim MaxLogDateTime As String = ""
                'If (AppSettings("LogBookTimeEntry") = "UTC") Then
                If mMachine.IsUTC Then '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                    MaxLogDateTime = mMaxLogOfAircraft.SouUniverseDateTimeFormatted.ToString
                Else
                    MaxLogDateTime = mMaxLogOfAircraft.SouLocalDateTimeFormatted.ToString
                End If
                If mMaxLogOfAircraft.SouUniverseDateTime.ToString <> "" Then
                    If CDate(mLog.LogDetails(mLog.LogDetails.Count - 1).SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime) Then 'Added by Saylee on 18-May-2012 ALL17052012
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
                        Exit Sub
                    End If
                End If

            Else
                If CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate) Then 'Added by Saylee on 18-May-2012 ALL17052012
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
                    Exit Sub
                End If
            End If
        End If
        'End

        Dim IsMELCount As Boolean = False
        Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
        mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)
        For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1
            If mTempMELSnagCorrectiveActionList(i).IsMEL = True And mTempMELSnagCorrectiveActionList(i).DueDate.ToString <> "" Then
                If (CDate(calDateTime.Text) > CDate(mTempMELSnagCorrectiveActionList(i).DueDate)) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then
                    IsMELCount = True
                    Exit For
                Else
                    IsMELCount = False
                End If
            End If
        Next
        mTempMELSnagCorrectiveActionList = Nothing
        '-----------------------------------
        If IsMELCount = True Then
            ' '' ''Dim msg1 As New SIMsgBox(Page, "Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
            ' '' ''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
            ' '' ''Session("sender") = "MEL"
            ' '' ''msg1.Show()

            ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
            'DataBindGrid()
            MSGBoxCtrl.show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo, "MEL")

            If IsValid Then
                SetObject()
                SetAirFrameGridObject()
                SetEngineGridObject(True)
                SetAPUGridObject(True)
                SetCGBGridObject(True)
            End If
            Exit Sub
        End If
        '-------------------------------

        If IsValid Then
            If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
            If Save() = True Then
                mLog = Log.GetLog(mLog.ID)
                mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                Session("mLog") = mLog
                Session("mAircraftInformationBoardList") = Nothing
                'Added By Vikrant on 01-Dec-2021 for PBH
                If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                    If Session("IsAircraftMadeNotInUse") = "True" Then
                        Session.Remove("AircraftId")
                        Session.Remove("IsAircraftMadeNotInUse")
                        MSGBoxCtrl.show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                        Exit Sub
                    End If
                End If
                'End

                ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                DataFieldBind()

                EnableDisableButton()
                ControlVisibility()

                DataBindGrid()

                SetTitle()

                upnlLogDetails.Update()
                upnlFlightSummary.Update()
                upnlTabs.Update()

            End If
        Else
            ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
            upnlErrorList.Update()
        End If
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        BindClassification()
        SetObject()
        If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then
            MarkLog(Util.Action.Save, "Flight Log", User.Identity.Name & " is not Authorized User to add ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            ' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            ' '' ''msg.ReplacePage = "wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            ' '' ''Session("sender") = "Authorization"
            ' '' ''msg.Show()

            ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

            Exit Sub
        End If

        If Not IsValid Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.

        Session("IsSaveAndNew") = 1

        'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
        If IsValid Then
            If Not mLog.PilotID1.Equals(Guid.Empty) Or Not mLog.PilotID2.Equals(Guid.Empty) Then
                Dim Title As String = "Save Alert !"
                Dim Message As String = ""
                If Not mLog.PilotID1.Equals(Guid.Empty) Then
                    Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID1.ToString, mLog.Date.ToString)
                    If mEmployeeStatus(0).Information <> "" Then
                        Message = "<b>Pilot in Command : </b> <br />" & mEmployeeStatus(0).Information
                    End If
                End If
                If Not mLog.PilotID2.Equals(Guid.Empty) Then
                    Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID2.ToString, mLog.Date.ToString)
                    If mEmployeeStatus(0).Information <> "" Then
                        Message = IIf(Message.Length > 0, Message & "<br/ >", "") & "<b>Co-Pilot : </b> <br />" & mEmployeeStatus(0).Information
                    End If
                End If
                If Message.Length > 0 Then
                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, Message, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        End If
        'End

        'Added by Vikrant On 03-Aug-2015 For  ALL030812015
        Dim mMaxLogOfAircraft As MaxLogOfAircraft
        mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID, True)

        If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
            ' Commented and Added by Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
            'If (AppSettings("ClientCode") <> "Heligo") Then
            If Not (AppSettings("ClientCode") = "Heligo" Or
               AppSettings("ClientCode") = "UHPL" Or
               AppSettings("ClientCode") = "APFT" Or
               AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
                'End
                Dim MaxLogDateTime As String = ""
                'If (AppSettings("LogBookTimeEntry") = "UTC") Then
                If mMachine.IsUTC Then '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                    MaxLogDateTime = mMaxLogOfAircraft.SouUniverseDateTimeFormatted.ToString
                Else
                    MaxLogDateTime = mMaxLogOfAircraft.SouLocalDateTimeFormatted.ToString
                End If
                If mMaxLogOfAircraft.SouUniverseDateTime.ToString <> "" Then
                    If CDate(mLog.LogDetails(mLog.LogDetails.Count - 1).SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime) Then 'Added by Saylee on 18-May-2012 ALL17052012
                        Session("New") = "True"
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
                        Exit Sub
                    End If
                End If
            Else
                If CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate) Then 'Added by Saylee on 18-May-2012 ALL17052012
                    Session("New") = "True"
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
                    Exit Sub
                End If
            End If
        End If
        'End



        Dim IsMELCount As Boolean = False
        Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
        mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)
        For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1
            If mTempMELSnagCorrectiveActionList(i).IsMEL = True And mTempMELSnagCorrectiveActionList(i).DueDate.ToString <> "" Then
                If (CDate(calDateTime.Text) > CDate(mTempMELSnagCorrectiveActionList(i).DueDate)) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then
                    IsMELCount = True
                    Exit For
                Else
                    IsMELCount = False
                End If
            End If
        Next
        mTempMELSnagCorrectiveActionList = Nothing

        If IsMELCount = True Then
            ' '' ''Dim msg1 As New SIMsgBox(Page, "Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
            ' '' ''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
            ' '' ''Session("sender") = "MELNew"
            ' '' ''msg1.Show()

            ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
            MSGBoxCtrl.show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo, "MELNew")

            If IsValid Then
                SetObject()
                SetAirFrameGridObject()
                SetEngineGridObject(True)
                SetAPUGridObject(True)
                SetCGBGridObject(True)
            End If
            Exit Sub
        End If

        If IsValid Then
            If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
            Session("New") = "True"
            If Save() = True Then
                NewRecord()
                DataFieldBind()
                Session("mLog") = mLog
                Session("mAircraftInformationBoardList") = Nothing
                ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                ' '' ''Response.Redirect("wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                'Added By Vikrant on 01-Dec-2021 for PBH
                If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                    If Session("IsAircraftMadeNotInUse") = "True" Then
                        Session.Remove("AircraftId")
                        Session.Remove("IsAircraftMadeNotInUse")
                        MSGBoxCtrl.show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                        Exit Sub
                    End If
                End If
                'End
                EnableDisableButton()
                ControlVisibility()

                DataBindGrid()

                SetTitle()
                SetFromAutoComplete() 'Added By Utkarsh On 24-Aug-2011

                mLog.LogPageNo = txtLogPageNo.Text.Trim  'Added By Utkarsh On 28-Nov-2011

                mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
                Session("mLogListOnDate") = mLogListOnDate
                If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then
                    '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
                    upnlLogInfo.Update()
                End If

                upnlLogDetails.Update()
                upnlFlightSummary.Update()

                upnlAirframeDetail.Update()
                upnlEngineDetail.Update()
                upnlAPUDetail.Update()
                upnlCGBDetail.Update()

                upnlTabs.Update()
            End If
        Else
            ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
            upnlErrorList.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session("IsValid") = IsValid
        If mLog.IsDirty And mLog.IsLogEdited = False Then

            ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.CloseConfirm, SIMsgBox.Message_text.Save, "", MsgBoxStyle.YesNo)
            ' '' ''msg1.ReplacePage = "wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") '"wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
            ' '' ''Session("sender") = "Close"
            ' '' ''msg1.Show()

            ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
            DataBindGrid()
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")

            If IsValid Then
                SetObject()
                SetAirFrameGridObject()
                SetEngineGridObject(True)
                SetAPUGridObject(True)
                SetCGBGridObject(True)
                '-------------------------------
            Else
                ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                upnlErrorList.Update()
            End If
        Else
            MarkLog(Util.Action.Close, "Flight Log Edit", "", Util.ErrorType.HandledError, mLog.ID, EventLogID)

            RemoveSession()
            Response.Redirect(Request.QueryString("BackPage") & "?")
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If (Not User.IsInRole("LogPrint")) Then
            ' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            ' '' ''msg.ReplacePage = "wfTLP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            ' '' ''msg.Show()

            ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

            Exit Sub
        End If
    End Sub

    'Private Sub btnFuelOil_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFuelOil.Click
    '    SetObject()
    '    SetAirFrameGridObject()
    '    SetEngineGridObject(True)
    '    SetAPUGridObject(True)
    '    SetCGBGridObject(True)
    '    Session("OpenFromWO") = False
    '    Session("mOpenFromLogFuelNew") = False

    '    Response.Redirect("wfLogFuelOil_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfTLPEdit_Ajax.aspx")
    'End Sub
    'Private Sub btnDefectActionList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefectActionList.Click
    '    SetObject()
    '    SetAirFrameGridObject()
    '    SetEngineGridObject(True)
    '    SetAPUGridObject(True)
    '    SetCGBGridObject(True)
    '    Session("Edit") = False

    '    Response.Redirect("wfLogDefectActionList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfTLPEdit_Ajax.aspx")
    'End Sub
    'Private Sub btnFlightCrew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFlightCrew.Click
    '    SetObject()
    '    SetAirFrameGridObject()
    '    SetEngineGridObject(True)
    '    SetAPUGridObject(True)
    '    SetCGBGridObject(True)
    '    Session("Edit") = False
    '    Response.Redirect("wfLogFlightCrew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfTLPEdit_Ajax.aspx")
    'End Sub
    'Private Sub btnMaintenanceAcitvity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMaintenanceAcitvity.Click
    '    SetObject()
    '    SetAirFrameGridObject()
    '    SetEngineGridObject(True)
    '    SetAPUGridObject(True)
    '    SetCGBGridObject(True)
    '    Session("Edit") = False
    '    'Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfTLPEdit_Ajax.aspx")
    '    Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfTLPEdit_Ajax.aspx") 'Added By Prashant 23-Aug-2018
    'End Sub

    Private Sub calDateTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calDateTime.TextChanged
        If IsPostBack Then

            '# Date Control Validation #
            Try

                Dim tempdate As DateTime
                Dim Datestring As String = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))

                tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()
                If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)
                    If Not ViewState("calDateTime") Is Nothing Then
                        calDateTime.Text = Format(CDate(ViewState("calDateTime")), AppSettings("DateFormat"))
                    Else
                        calDateTime.Text = Format(Today.Date, AppSettings("DateFormat"))
                    End If
                Else
                    calDateTime.Text = Format(tempdate, AppSettings("DateFormat"))
                End If
                ViewState("calDateTime") = calDateTime.Text.Trim  'Storing Current DateValue to ViewState for Date correction
            Catch ex As Exception
                If Not ViewState("calDateTime") Is Nothing Then
                    calDateTime.Text = Format(CDate(ViewState("calDateTime")), AppSettings("DateFormat"))  'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                Else
                    calDateTime.Text = Format(Today.Date, AppSettings("DateFormat"))  'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                End If
                calDateTime_TextChanged(calDateTime.Text, e)  'Raising textchange event for further calculation
                Exit Sub
            End Try

            '# End
            If DateDiff(DateInterval.Day, SmartDate.StringToDate(mLog.Date.ToString), New SmartDate(calDateTime.Text.ToString).Date) <> 0 Then
                REM: Clone the object
                Dim clnLog As Log
                clnLog = CType(mLog.Clone, Log)
                If mLog.IsNew Then
                    NewRecord(calDateTime.Text.ToString)
                Else
                    EditRecord(calDateTime.Text.ToString)
                End If
                REM: Copy from Clone
                CopyFromClone(clnLog, True)
                DataFieldBind()
                'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
                '''''If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
                '''''Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
                '''''    ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Restriction, SIMsgBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OKOnly)
                '''''    ' '' ''msg1.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage")
                '''''    ' '' ''msg1.Show()
                '''''    MSGBoxCtrl.show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly, "")
                '''''    Exit Sub
                '''''End If
            End If

            '
            If (AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True") Then 'Added by Saylee on 2-Sep-2016 for ALL02092016 as per config key ShowLogDetailsOnAddingSameLogDate
                'Added by Saylee on 11-Apr-2016
                mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
                Session("mLogListOnDate") = mLogListOnDate
                If mLogListOnDate.Count > 0 And mLog.IsNew Then
                    '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
                    Dim str1 As String
                    str1 = "delete_cookie();"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str1, True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
                    upnlLogInfo.Update()
                End If
                upnlLogDetails.Update()
                '****************************************
            End If


            SetTitle()
            'Added By utkarsh ON 30-sep-2013 for Log_ajax changes
            EnableDisableButton()
            ControlVisibility()
            'End
            'upnlFlightSummary.Update()
            'upnlAirframeDetail.Update()
            'upnlEngineDetail.Update()
            'upnlAPUDetail.Update()
            'upnlCGBDetail.Update()


        End If
    End Sub
    '   Private Sub btnAddPilot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPilot.Click
    Protected Sub btnAddPilot_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddPilot.Click

        SetObject()
        SetAirFrameGridObject()
        SetEngineGridObject(True)
        SetAPUGridObject(True)
        SetCGBGridObject(True)

        Dim mEmployee As Employee
        mEmployee = Employee.NewPilot()
        Session("mEmployee") = mEmployee

        Response.Redirect("wfPilot.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfTLPEdit_Ajax.aspx")
    End Sub
    'Private Overloads Sub btnFlightLogClassification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFlightLogClassification.Click
    Protected Sub btnFlightLogClassification_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFlightLogClassification.Click
        Response.Redirect("wfFlightLogClassification.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfTLPEdit_Ajax.aspx")
    End Sub
    Private Sub cmbFlightLogClassification_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFlightLogClassification.SelectedIndexChanged


        mLog.FlightLogClassificationID = New Guid(cmbFlightLogClassification.SelectedValue.ToString)
        mLog.FlightLogClassificationName = cmbFlightLogClassification.SelectedItem.Text
        Session("mLog") = mLog
    End Sub
    Private Sub btnAddRoute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddRoute.Click
        SetObject()
        SetAirFrameGridObject()
        SetEngineGridObject(True)
        SetAPUGridObject(True)
        SetCGBGridObject(True)

        Session("Edit") = False

        Dim mLogDetail As LogDetail
        mLogDetail = LogDetail.NewChildLogDetail(mLog.ID, mLog.Date.ToString)

        If mLog.LogDetails.Count = 0 Then
            'Setting Source Place for the first child of the Log i.e. LogDetail
            mLogDetail.SourceID = mLog.SourceID
            mLogDetail.FuelOnDeparture = mLog.TotalFuelOnDeparture
        Else
            'Setting Source Place for the new child of the Log i.e. LogDetail from last child
            mLogDetail.SourceID = mLog.LogDetails.CurrentItem.DestinationID
            mLogDetail.FuelOnDeparture = mLog.LogDetails.CurrentItem.FuelOnArrival
        End If
        Session("mLogDetail") = mLogDetail
        Session("mRegNo") = mMachine.RegNo
        Session("mLog") = mLog
        Session("mMachine") = mMachine 'Added By Saylee On 12-Feb-2014 For ALL12022014-1
        Response.Redirect("wfTLPDetail_Ajax.aspx?BackPage=wfTLPEdit_Ajax.aspx&ChildPage=Index.aspx")
    End Sub
    Private Sub dgLogDetails_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgLogDetails.ItemCommand
        'Dim Index As Int32 = e.Item.ItemIndex + dgLogDetails.CurrentPageIndex * dgLogDetails.PageSize
        Select Case e.CommandName
            Case "Edit"
                '  Dim ID As Guid = New Guid(e.Item.Cells(0).Text)
                Dim Index As Integer = CInt(e.CommandArgument)
                Session("LogDetailEdit") = True
                Session("mIsLastLog") = mIsLastLog
                If Index <> mLog.LogDetails.Count Then
                    Session("mIsLastLogTLP") = False
                Else
                    Session("mIsLastLogTLP") = True
                End If
                EditLogDetailRecord(Index - 1)
            Case "Remove"
                ' Dim ID As Guid = New Guid(e.Item.Cells(0).Text)
                Dim Index As Integer = CInt(e.CommandArgument)
                If Index <> mLog.LogDetails.Count Then 'This is Not Last Record...
                    ' '' ''Dim msg As New SIMsgBox(Page, "Remove Alert !", "<b>You can not remove this record.</b><BR><BR>Selected record is not last record.", "", MsgBoxStyle.OKOnly)
                    ' '' ''msg.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                    ' '' ''msg.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.show("Remove Alert !", "<b>You can not remove this record.</b><BR><BR>Selected record is not last record.", "", MsgBoxStyle.OkOnly, "")

                    Exit Sub
                Else

                    ' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
                    ' '' ''msg.ReplacePage = "wfTLP.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                    ' '' ''Session("sender") = "Remove"
                    ' '' ''msg.Show()

                    ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                    MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Remove")

                    Session("ID") = mLog.LogDetails(Index - 1).ID 'ID
                End If
        End Select
    End Sub
    ' '' ''AJAX- New Event for MessageBox Control 
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    ' '' ''AJAX- New Event to attached Browse File.
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click 'Added by Vikrant On 25-Nov-2014
        mLog.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        GetAttachment()
        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mLog.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mLog.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mLog.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mLog.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    ' '' ''Private Sub dgAFPeriods_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAFPeriods.ItemCommand
    ' '' ''    Dim Index As Integer = e.Item.ItemIndex + dgAFPeriods.CurrentPageIndex * dgAFPeriods.PageSize
    ' '' ''    Select Case e.CommandName
    ' '' ''        Case "AirFrameHours"
    ' '' ''            Dim txtAirFrameHours As TextBox
    ' '' ''            txtAirFrameHours = CType(Me.dgAFPeriods.Items(Index).FindControl("txtAirFrameHours"), TextBox)
    ' '' ''            mLog.LogAFAssemblies.Item(Index).Hours = Trim(txtAirFrameHours.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "AirFrameLandings"
    ' '' ''            Dim txtAirFrameLandings As TextBox
    ' '' ''            txtAirFrameLandings = CType(Me.dgAFPeriods.Items(Index).FindControl("txtAirFrameLandings"), TextBox)
    ' '' ''            mLog.LogAFAssemblies(Index).Landings = Trim(txtAirFrameLandings.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "AirFrameCycles"
    ' '' ''            Dim txtAirFrameCycles As TextBox
    ' '' ''            txtAirFrameCycles = CType(Me.dgAFPeriods.Items(Index).FindControl("txtAirFrameCycles"), TextBox)
    ' '' ''            mLog.LogAFAssemblies(Index).Cycles = Trim(txtAirFrameCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "AirFrameStarts"
    ' '' ''            Dim txtAirFrameStarts As TextBox
    ' '' ''            txtAirFrameStarts = CType(Me.dgAFPeriods.Items(Index).FindControl("txtAirFrameStarts"), TextBox)
    ' '' ''            mLog.LogAFAssemblies(Index).Starts = Trim(txtAirFrameStarts.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "AirFrameNGCycles"
    ' '' ''            Dim txtAirFrameNGCycles As TextBox
    ' '' ''            txtAirFrameNGCycles = CType(Me.dgAFPeriods.Items(Index).FindControl("txtAirFrameNGCycles"), TextBox)
    ' '' ''            mLog.LogAFAssemblies(Index).NGCycles = Trim(txtAirFrameNGCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "AirFrameNFCycles"
    ' '' ''            Dim txtAirFrameNFCycles As TextBox
    ' '' ''            txtAirFrameNFCycles = CType(Me.dgAFPeriods.Items(Index).FindControl("txtAirFrameNFCycles"), TextBox)
    ' '' ''            mLog.LogAFAssemblies(Index).NFCycles = Trim(txtAirFrameNFCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "AirFrameRins"
    ' '' ''            Dim txtAirFrameRins As TextBox
    ' '' ''            txtAirFrameRins = CType(Me.dgAFPeriods.Items(Index).FindControl("txtAirFrameRins"), TextBox)
    ' '' ''            mLog.LogAFAssemblies(Index).RINS = Trim(txtAirFrameRins.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "AirFrameBleeds"
    ' '' ''            Dim txtAirFrameBleeds As TextBox
    ' '' ''            txtAirFrameBleeds = CType(Me.dgAFPeriods.Items(Index).FindControl("txtAirFrameBleeds"), TextBox)
    ' '' ''            mLog.LogAFAssemblies(Index).Bleeds = Trim(txtAirFrameBleeds.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "AirFrameImpellerCycles"
    ' '' ''            Dim txtAirFrameImpellerCycles As TextBox
    ' '' ''            txtAirFrameImpellerCycles = CType(Me.dgAFPeriods.Items(Index).FindControl("txtAirFrameImpellerCycles"), TextBox)
    ' '' ''            mLog.LogAFAssemblies(Index).ImpellerCycles = Trim(txtAirFrameImpellerCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "AirFrameCTCycles"
    ' '' ''            Dim txtAirFrameCTCycles As TextBox
    ' '' ''            txtAirFrameCTCycles = CType(Me.dgAFPeriods.Items(Index).FindControl("txtAirFrameCTCycles"), TextBox)
    ' '' ''            mLog.LogAFAssemblies(Index).CTCycles = Trim(txtAirFrameCTCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "AirFramePTCycles"
    ' '' ''            Dim txtAirFramePTCycles As TextBox
    ' '' ''            txtAirFramePTCycles = CType(Me.dgAFPeriods.Items(Index).FindControl("txtAirFramePTCycles"), TextBox)
    ' '' ''            mLog.LogAFAssemblies(Index).PTCycles = Trim(txtAirFramePTCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''            'Added by Shweta on 8-May-2012 for ALL02052012
    ' '' ''        Case "AirframeGeneratorMods"
    ' '' ''            Dim txtAirframeGeneratorMods As TextBox
    ' '' ''            txtAirframeGeneratorMods = CType(Me.dgAFPeriods.Items(Index).FindControl("txtAirframeGeneratorMods"), TextBox)
    ' '' ''            mLog.LogAFAssemblies(Index).GeneratorMods = Trim(txtAirframeGeneratorMods.Text)
    ' '' ''            DataBindGrid()
    ' '' ''    End Select
    ' '' ''End Sub
    ' '' ''Private Sub dgEnginePeriods_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgEnginePeriods.ItemCommand
    ' '' ''    Dim Index As Integer = e.Item.ItemIndex + dgEnginePeriods.PageSize * dgEnginePeriods.CurrentPageIndex
    ' '' ''    Select Case e.CommandName
    ' '' ''        Case "EngineHours"
    ' '' ''            Dim txtEngineHours As TextBox
    ' '' ''            txtEngineHours = CType(Me.dgEnginePeriods.Items(Index).FindControl("txtEngineHours"), TextBox)
    ' '' ''            mLog.LogEngAssemblies(Index).Hours = Trim(txtEngineHours.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "EngineLandings"
    ' '' ''            Dim txtEngineLandings As TextBox
    ' '' ''            txtEngineLandings = CType(Me.dgEnginePeriods.Items(Index).FindControl("txtEngineLandings"), TextBox)
    ' '' ''            mLog.LogEngAssemblies(Index).Landings = Trim(txtEngineLandings.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "EngineCycles"
    ' '' ''            Dim txtEngineCycles As TextBox
    ' '' ''            txtEngineCycles = CType(Me.dgEnginePeriods.Items(Index).FindControl("txtEngineCycles"), TextBox)
    ' '' ''            mLog.LogEngAssemblies(Index).Cycles = Trim(txtEngineCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "EngineStarts"
    ' '' ''            Dim txtEngineStarts As TextBox
    ' '' ''            txtEngineStarts = CType(Me.dgEnginePeriods.Items(Index).FindControl("txtEngineStarts"), TextBox)
    ' '' ''            mLog.LogEngAssemblies(Index).Starts = Trim(txtEngineStarts.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "EngineNGCycles"
    ' '' ''            Dim txtEngineNGCycles As TextBox
    ' '' ''            txtEngineNGCycles = CType(Me.dgEnginePeriods.Items(Index).FindControl("txtEngineNGCycles"), TextBox)
    ' '' ''            mLog.LogEngAssemblies(Index).NGCycles = Trim(txtEngineNGCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "EngineNFCycles"
    ' '' ''            Dim txtEngineNFCycles As TextBox
    ' '' ''            txtEngineNFCycles = CType(Me.dgEnginePeriods.Items(Index).FindControl("txtEngineNFCycles"), TextBox)
    ' '' ''            mLog.LogEngAssemblies(Index).NFCycles = Trim(txtEngineNFCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "EngineRins"
    ' '' ''            Dim txtEngineRins As TextBox
    ' '' ''            txtEngineRins = CType(Me.dgEnginePeriods.Items(Index).FindControl("txtEngineRins"), TextBox)
    ' '' ''            mLog.LogEngAssemblies(Index).RINS = Trim(txtEngineRins.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "EngineCFactors"
    ' '' ''            Dim txtEngineCFactors As TextBox
    ' '' ''            txtEngineCFactors = CType(Me.dgEnginePeriods.Items(Index).FindControl("txtEngineCFactors"), TextBox)
    ' '' ''            mLog.LogEngAssemblies(Index).CFactor = Trim(txtEngineCFactors.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "EngineBleeds"
    ' '' ''            Dim txtEngineBleeds As TextBox
    ' '' ''            txtEngineBleeds = CType(Me.dgEnginePeriods.Items(Index).FindControl("txtEngineBleeds"), TextBox)
    ' '' ''            mLog.LogEngAssemblies(Index).Bleeds = Trim(txtEngineBleeds.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "EngineImpellerCycles"
    ' '' ''            Dim txtEngineImpellerCycles As TextBox
    ' '' ''            txtEngineImpellerCycles = CType(Me.dgEnginePeriods.Items(Index).FindControl("txtEngineImpellerCycles"), TextBox)
    ' '' ''            mLog.LogEngAssemblies(Index).ImpellerCycles = Trim(txtEngineImpellerCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "EngineCTCycles"
    ' '' ''            Dim txtEngineCTCycles As TextBox
    ' '' ''            txtEngineCTCycles = CType(Me.dgEnginePeriods.Items(Index).FindControl("txtEngineCTCycles"), TextBox)
    ' '' ''            mLog.LogEngAssemblies(Index).CTCycles = Trim(txtEngineCTCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "EnginePTCycles"
    ' '' ''            Dim txtEnginePTCycles As TextBox
    ' '' ''            txtEnginePTCycles = CType(Me.dgEnginePeriods.Items(Index).FindControl("txtEnginePTCycles"), TextBox)
    ' '' ''            mLog.LogEngAssemblies(Index).PTCycles = Trim(txtEnginePTCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''            'Added by Shweta on 8-May-2012 for ALL02052012
    ' '' ''        Case "EngineGeneratorMods"
    ' '' ''            Dim txtEngineGeneratorMods As TextBox
    ' '' ''            txtEngineGeneratorMods = CType(Me.dgEnginePeriods.Items(Index).FindControl("txtEngineGeneratorMods"), TextBox)
    ' '' ''            mLog.LogEngAssemblies(Index).PTCycles = Trim(txtEngineGeneratorMods.Text)
    ' '' ''            DataBindGrid()
    ' '' ''            '------------------------------
    ' '' ''    End Select
    ' '' ''End Sub
    ' '' ''Private Sub dgAPUPeriods_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAPUPeriods.ItemCommand
    ' '' ''    Dim Index As Integer = e.Item.ItemIndex + dgAPUPeriods.CurrentPageIndex * dgAPUPeriods.PageSize
    ' '' ''    Select Case e.CommandName
    ' '' ''        Case "APUHours"
    ' '' ''            Dim txtAPUHours As TextBox
    ' '' ''            txtAPUHours = CType(Me.dgAPUPeriods.Items(Index).FindControl("txtAPUHours"), TextBox)
    ' '' ''            mLog.LogAPUAssemblies(Index).Hours = Trim(txtAPUHours.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "EngineLandings"
    ' '' ''            Dim txtAPULandings As TextBox
    ' '' ''            txtAPULandings = CType(Me.dgAPUPeriods.Items(Index).FindControl("txtAPULandings"), TextBox)
    ' '' ''            mLog.LogAPUAssemblies(Index).Landings = Trim(txtAPULandings.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "APUCycles"
    ' '' ''            Dim txtAPUCycles As TextBox
    ' '' ''            txtAPUCycles = CType(Me.dgAPUPeriods.Items(Index).FindControl("txtAPUCycles"), TextBox)
    ' '' ''            mLog.LogAPUAssemblies(Index).Cycles = Trim(txtAPUCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "APUStarts"
    ' '' ''            Dim txtAPUStarts As TextBox
    ' '' ''            txtAPUStarts = CType(Me.dgAPUPeriods.Items(Index).FindControl("txtAPUStarts"), TextBox)
    ' '' ''            mLog.LogAPUAssemblies(Index).Starts = Trim(txtAPUStarts.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "APUNGCycles"
    ' '' ''            Dim txtAPUNGCycles As TextBox
    ' '' ''            txtAPUNGCycles = CType(Me.dgAPUPeriods.Items(Index).FindControl("txtAPUNGCycles"), TextBox)
    ' '' ''            mLog.LogAPUAssemblies(Index).NGCycles = Trim(txtAPUNGCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "APUNFCycles"
    ' '' ''            Dim txtAPUNFCycles As TextBox
    ' '' ''            txtAPUNFCycles = CType(Me.dgAPUPeriods.Items(Index).FindControl("txtAPUNFCycles"), TextBox)
    ' '' ''            mLog.LogAPUAssemblies(Index).NFCycles = Trim(txtAPUNFCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "APURins"
    ' '' ''            Dim txtAPURins As TextBox
    ' '' ''            txtAPURins = CType(Me.dgAPUPeriods.Items(Index).FindControl("txtAPURins"), TextBox)
    ' '' ''            mLog.LogAPUAssemblies(Index).RINS = Trim(txtAPURins.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "APUBleeds"
    ' '' ''            Dim txtAPUBleeds As TextBox
    ' '' ''            txtAPUBleeds = CType(Me.dgAPUPeriods.Items(Index).FindControl("txtAPUBleeds"), TextBox)
    ' '' ''            mLog.LogAPUAssemblies(Index).Bleeds = Trim(txtAPUBleeds.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "APUImpellerCycles"
    ' '' ''            Dim txtAPUImpellerCycles As TextBox
    ' '' ''            txtAPUImpellerCycles = CType(Me.dgAPUPeriods.Items(Index).FindControl("txtAPUImpellerCycles"), TextBox)
    ' '' ''            mLog.LogAPUAssemblies(Index).ImpellerCycles = Trim(txtAPUImpellerCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "APUCTCycles"
    ' '' ''            Dim txtAPUCTCycles As TextBox
    ' '' ''            txtAPUCTCycles = CType(Me.dgAPUPeriods.Items(Index).FindControl("txtAPUCTCycles"), TextBox)
    ' '' ''            mLog.LogAPUAssemblies(Index).CTCycles = Trim(txtAPUCTCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "APUPTCycles"
    ' '' ''            Dim txtAPUPTCycles As TextBox
    ' '' ''            txtAPUPTCycles = CType(Me.dgAPUPeriods.Items(Index).FindControl("txtAPUPTCycles"), TextBox)
    ' '' ''            mLog.LogAPUAssemblies(Index).PTCycles = Trim(txtAPUPTCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''            'Added by Shweta on 8-May-2012 for ALL02052012
    ' '' ''        Case "APUGeneratorMods"
    ' '' ''            Dim txtAPUGeneratorMods As TextBox
    ' '' ''            txtAPUGeneratorMods = CType(Me.dgAPUPeriods.Items(Index).FindControl("txtAPUGeneratorMods"), TextBox)
    ' '' ''            mLog.LogAPUAssemblies(Index).GeneratorMods = Trim(txtAPUGeneratorMods.Text)
    ' '' ''            DataBindGrid()
    ' '' ''            '------------------------------
    ' '' ''    End Select
    ' '' ''End Sub
    ' '' ''Private Sub dgCGBPeriods_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgCGBPeriods.ItemCommand
    ' '' ''    Dim Index As Integer = e.Item.ItemIndex + dgCGBPeriods.CurrentPageIndex * dgCGBPeriods.PageSize
    ' '' ''    Select Case e.CommandName
    ' '' ''        Case "CGBHours"
    ' '' ''            Dim txtCGBHours As TextBox
    ' '' ''            txtCGBHours = CType(Me.dgCGBPeriods.Rows(Index).FindControl("txtCGBHours"), TextBox)
    ' '' ''            mLog.LogCGBAssemblies(Index).Hours = Trim(txtCGBHours.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "CGBLandings"
    ' '' ''            Dim txtCGBLandings As TextBox
    ' '' ''            txtCGBLandings = CType(Me.dgCGBPeriods.Items(Index).FindControl("txtCGBLandings"), TextBox)
    ' '' ''            mLog.LogCGBAssemblies(Index).Landings = Trim(txtCGBLandings.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "CGBCycles"
    ' '' ''            Dim txtCGBCycles As TextBox
    ' '' ''            txtCGBCycles = CType(Me.dgCGBPeriods.Items(Index).FindControl("txtCGBCycles"), TextBox)
    ' '' ''            mLog.LogCGBAssemblies(Index).Cycles = Trim(txtCGBCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "CGBStarts"
    ' '' ''            Dim txtCGBStarts As TextBox
    ' '' ''            txtCGBStarts = CType(Me.dgCGBPeriods.Items(Index).FindControl("txtCGBStarts"), TextBox)
    ' '' ''            mLog.LogCGBAssemblies(Index).Starts = Trim(txtCGBStarts.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "CGBNGCycles"
    ' '' ''            Dim txtCGBNGCycles As TextBox
    ' '' ''            txtCGBNGCycles = CType(Me.dgCGBPeriods.Items(Index).FindControl("txtCGBNGCycles"), TextBox)
    ' '' ''            mLog.LogCGBAssemblies(Index).NGCycles = Trim(txtCGBNGCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "CGBNFCycles"
    ' '' ''            Dim txtCGBNFCycles As TextBox
    ' '' ''            txtCGBNFCycles = CType(Me.dgCGBPeriods.Items(Index).FindControl("txtCGBNFCycles"), TextBox)
    ' '' ''            mLog.LogCGBAssemblies(Index).NFCycles = Trim(txtCGBNFCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "CGBRins"
    ' '' ''            Dim txtCGBRins As TextBox
    ' '' ''            txtCGBRins = CType(Me.dgCGBPeriods.Items(Index).FindControl("txtCGBRins"), TextBox)
    ' '' ''            mLog.LogCGBAssemblies(Index).RINS = Trim(txtCGBRins.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "CGBBleeds"
    ' '' ''            Dim txtCGBBleeds As TextBox
    ' '' ''            txtCGBBleeds = CType(Me.dgCGBPeriods.Items(Index).FindControl("txtCGBBleeds"), TextBox)
    ' '' ''            mLog.LogCGBAssemblies(Index).Bleeds = Trim(txtCGBBleeds.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "CGBImpellerCycles"
    ' '' ''            Dim txtCGBImpellerCycles As TextBox
    ' '' ''            txtCGBImpellerCycles = CType(Me.dgCGBPeriods.Items(Index).FindControl("txtCGBImpellerCycles"), TextBox)
    ' '' ''            mLog.LogCGBAssemblies(Index).ImpellerCycles = Trim(txtCGBImpellerCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "CGBCTCycles"
    ' '' ''            Dim txtCGBCTCycles As TextBox
    ' '' ''            txtCGBCTCycles = CType(Me.dgCGBPeriods.Items(Index).FindControl("txtCGBCTCycles"), TextBox)
    ' '' ''            mLog.LogCGBAssemblies(Index).CTCycles = Trim(txtCGBCTCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''        Case "CGBPTCycles"
    ' '' ''            Dim txtCGBPTCycles As TextBox
    ' '' ''            txtCGBPTCycles = CType(Me.dgCGBPeriods.Items(Index).FindControl("txtCGBPTCycles"), TextBox)
    ' '' ''            mLog.LogCGBAssemblies(Index).PTCycles = Trim(txtCGBPTCycles.Text)
    ' '' ''            DataBindGrid()
    ' '' ''            'Added by Shweta on 7-May-2012 for ALL02052012
    ' '' ''        Case "CGBGeneratorMods"
    ' '' ''            Dim txtCGBGeneratorMods As TextBox
    ' '' ''            txtCGBGeneratorMods = CType(Me.dgCGBPeriods.Items(Index).FindControl("txtCGBGeneratorMods"), TextBox)
    ' '' ''            mLog.LogCGBAssemblies(Index).GeneratorMods = Trim(txtCGBGeneratorMods.Text)
    ' '' ''            DataBindGrid()
    ' '' ''            '------------------------------
    ' '' ''    End Select
    ' '' ''End Sub
    Private Sub lnkAllAssembly_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAllAssembly.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowAssembly", "ShowAssembly();", True)
    End Sub
#End Region

#Region " Air Frame Grid "
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAirFrameHours_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent

        Dim txtAirFrameHours As TextBox = TryCast(currentRow.FindControl("txtAirFrameHours"), TextBox)
        mLog.LogAFAssemblies.Item(currentRow.RowIndex).Hours = Trim(txtAirFrameHours.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAirFrameLandings_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAirFrameLandings As TextBox = TryCast(currentRow.FindControl("txtAirFrameLandings"), TextBox)
        mLog.LogAFAssemblies(currentRow.RowIndex).Landings = Trim(txtAirFrameLandings.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAirFrameCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAirFrameCycles As TextBox = TryCast(currentRow.FindControl("txtAirFrameCycles"), TextBox)
        mLog.LogAFAssemblies(currentRow.RowIndex).Cycles = Trim(txtAirFrameCycles.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAirFrameStarts_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAirFrameStarts As TextBox = TryCast(currentRow.FindControl("txtAirFrameStarts"), TextBox)
        mLog.LogAFAssemblies(currentRow.RowIndex).Starts = Trim(txtAirFrameStarts.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAirFrameNGCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAirFrameNGCycles As TextBox = TryCast(currentRow.FindControl("txtAirFrameNGCycles"), TextBox)
        mLog.LogAFAssemblies(currentRow.RowIndex).NGCycles = Trim(txtAirFrameNGCycles.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAirFrameNFCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAirFrameNFCycles As TextBox = TryCast(currentRow.FindControl("txtAirFrameNFCycles"), TextBox)
        mLog.LogAFAssemblies(currentRow.RowIndex).NFCycles = Trim(txtAirFrameNFCycles.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAirFrameRins_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAirFrameRins As TextBox = TryCast(currentRow.FindControl("txtAirFrameRins"), TextBox)
        mLog.LogAFAssemblies(currentRow.RowIndex).RINS = Trim(txtAirFrameRins.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAirFrameBleeds_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAirFrameBleeds As TextBox = TryCast(currentRow.FindControl("txtAirFrameBleeds"), TextBox)
        mLog.LogAFAssemblies(currentRow.RowIndex).Bleeds = Trim(txtAirFrameBleeds.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAirFrameImpellerCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAirFrameImpellerCycles As TextBox = TryCast(currentRow.FindControl("txtAirFrameImpellerCycles"), TextBox)
        mLog.LogAFAssemblies(currentRow.RowIndex).ImpellerCycles = Trim(txtAirFrameImpellerCycles.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAirFrameCTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAirFrameCTCycles As TextBox = TryCast(currentRow.FindControl("txtAirFrameCTCycles"), TextBox)
        mLog.LogAFAssemblies(currentRow.RowIndex).CTCycles = Trim(txtAirFrameCTCycles.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAirFramePTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAirFramePTCycles As TextBox = TryCast(currentRow.FindControl("txtAirFramePTCycles"), TextBox)
        mLog.LogAFAssemblies(currentRow.RowIndex).PTCycles = Trim(txtAirFramePTCycles.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAirframeGeneratorMods_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAirFrameGeneratorMods As TextBox = TryCast(currentRow.FindControl("txtAirFrameGeneratorMods"), TextBox)
        mLog.LogAFAssemblies(currentRow.RowIndex).GeneratorMods = Trim(txtAirFrameGeneratorMods.Text)
        DataBindGrid()
    End Sub
#End Region

#Region " Engine Grid "
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtEngineHours_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEngineHours As TextBox = TryCast(currentRow.FindControl("txtEngineHours"), TextBox)
        mLog.LogEngAssemblies(currentRow.RowIndex).Hours = Trim(txtEngineHours.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtEngineLandings_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEngineLandings As TextBox = TryCast(currentRow.FindControl("txtEngineLandings"), TextBox)
        mLog.LogEngAssemblies(currentRow.RowIndex).Landings = Trim(txtEngineLandings.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtEngineCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEngineCycles As TextBox = TryCast(currentRow.FindControl("txtEngineCycles"), TextBox)
        mLog.LogEngAssemblies(currentRow.RowIndex).Cycles = Trim(txtEngineCycles.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtEngineStarts_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEngineStarts As TextBox = TryCast(currentRow.FindControl("txtEngineStarts"), TextBox)
        mLog.LogEngAssemblies(currentRow.RowIndex).Starts = Trim(txtEngineStarts.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub btnEngineNGCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEngineNGCycles As TextBox = TryCast(currentRow.FindControl("txtEngineNGCycles"), TextBox)
        mLog.LogEngAssemblies(currentRow.RowIndex).NGCycles = Trim(txtEngineNGCycles.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtEngineNFCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEngineNFCycles As TextBox = TryCast(currentRow.FindControl("txtEngineNFCycles"), TextBox)
        mLog.LogEngAssemblies(currentRow.RowIndex).NFCycles = Trim(txtEngineNFCycles.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtEngineRins_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEngineRins As TextBox = TryCast(currentRow.FindControl("txtEngineRins"), TextBox)
        mLog.LogEngAssemblies(currentRow.RowIndex).RINS = Trim(txtEngineRins.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtEngineCFactors_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEngineCFactors As TextBox = TryCast(currentRow.FindControl("txtEngineCFactors"), TextBox)
        mLog.LogEngAssemblies(currentRow.RowIndex).CFactor = Trim(txtEngineCFactors.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtEngineBleeds_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEngineBleeds As TextBox = TryCast(currentRow.FindControl("txtEngineBleeds"), TextBox)
        mLog.LogEngAssemblies(currentRow.RowIndex).Bleeds = Trim(txtEngineBleeds.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtEngineImpellerCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEngineImpellerCycles As TextBox = TryCast(currentRow.FindControl("txtEngineImpellerCycles"), TextBox)
        mLog.LogEngAssemblies(currentRow.RowIndex).ImpellerCycles = Trim(txtEngineImpellerCycles.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtEngineCTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEngineCTCycles As TextBox = TryCast(currentRow.FindControl("txtEngineCTCycles"), TextBox)
        mLog.LogEngAssemblies(currentRow.RowIndex).CTCycles = Trim(txtEngineCTCycles.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtEnginePTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEnginePTCycles As TextBox = TryCast(currentRow.FindControl("txtEnginePTCycles"), TextBox)
        mLog.LogEngAssemblies(currentRow.RowIndex).PTCycles = Trim(txtEnginePTCycles.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtEngineGeneratorMods_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEngineGeneratorMods As TextBox = TryCast(currentRow.FindControl("txtEngineGeneratorMods"), TextBox)
        mLog.LogAFAssemblies(currentRow.RowIndex).GeneratorMods = Trim(txtEngineGeneratorMods.Text)
        DataBindGrid()

    End Sub
    'Code added for Rapid TakeOff on 31-Oct-2022 by Saylee
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtEngineRapidTakeOffFactor_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEngineRapidTakeOffFactor As TextBox = TryCast(currentRow.FindControl("txtEngineRapidTakeOffFactor"), TextBox)
        mLog.LogEngAssemblies(currentRow.RowIndex).RapidTakeOffFactor = Trim(txtEngineRapidTakeOffFactor.Text)
        DataBindGrid()
    End Sub
#End Region

#Region " APU Grid "
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAPUHours_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAPUHours As TextBox = TryCast(currentRow.FindControl("txtAPUHours"), TextBox)
        mLog.LogAPUAssemblies(currentRow.RowIndex).Hours = Trim(txtAPUHours.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAPULandings_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAPULandings As TextBox = TryCast(currentRow.FindControl("txtAPULandings"), TextBox)
        mLog.LogAPUAssemblies(currentRow.RowIndex).Landings = Trim(txtAPULandings.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAPUCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAPUCycles As TextBox = TryCast(currentRow.FindControl("txtAPUCycles"), TextBox)
        mLog.LogAPUAssemblies(currentRow.RowIndex).Cycles = Trim(txtAPUCycles.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAPUStarts_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAPUStarts As TextBox = TryCast(currentRow.FindControl("txtAPUStarts"), TextBox)
        mLog.LogAPUAssemblies(currentRow.RowIndex).Starts = Trim(txtAPUStarts.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAPUNGCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAPUNGCycles As TextBox = TryCast(currentRow.FindControl("txtAPUNGCycles"), TextBox)
        mLog.LogAPUAssemblies(currentRow.RowIndex).NGCycles = Trim(txtAPUNGCycles.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAPUNFCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAPUNFCycles As TextBox = TryCast(currentRow.FindControl("txtAPUNFCycles"), TextBox)
        mLog.LogAPUAssemblies(currentRow.RowIndex).NFCycles = Trim(txtAPUNFCycles.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAPURins_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAPURins As TextBox = TryCast(currentRow.FindControl("txtAPURins"), TextBox)
        mLog.LogAPUAssemblies(currentRow.RowIndex).RINS = Trim(txtAPURins.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAPUBleeds_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAPUBleeds As TextBox = TryCast(currentRow.FindControl("txtAPUBleeds"), TextBox)
        mLog.LogAPUAssemblies(currentRow.RowIndex).Bleeds = Trim(txtAPUBleeds.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAPUImpellerCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAPUImpellerCycles As TextBox = TryCast(currentRow.FindControl("txtAPUImpellerCycles"), TextBox)
        mLog.LogAPUAssemblies(currentRow.RowIndex).ImpellerCycles = Trim(txtAPUImpellerCycles.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAPUCTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAPUCTCycles As TextBox = TryCast(currentRow.FindControl("txtAPUCTCycles"), TextBox)
        mLog.LogAPUAssemblies(currentRow.RowIndex).CTCycles = Trim(txtAPUCTCycles.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAPUPTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAPUPTCycles As TextBox = TryCast(currentRow.FindControl("txtAPUPTCycles"), TextBox)
        mLog.LogAPUAssemblies(currentRow.RowIndex).PTCycles = Trim(txtAPUPTCycles.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtAPUGeneratorMods_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtAPUPGeneratorMods As TextBox = TryCast(currentRow.FindControl("txtAPUPGeneratorMods"), TextBox)
        mLog.LogAPUAssemblies(currentRow.RowIndex).GeneratorMods = Trim(txtAPUPGeneratorMods.Text)
        DataBindGrid()
    End Sub
#End Region

#Region " CGB Grid "
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtCGBHours_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtCGBHours As TextBox = TryCast(currentRow.FindControl("txtCGBHours"), TextBox)
        mLog.LogCGBAssemblies(currentRow.RowIndex).Hours = Trim(txtCGBHours.Text)
        DataBindGrid()
    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtCGBLandings_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtCGBLandings As TextBox = TryCast(currentRow.FindControl("txtCGBLandings"), TextBox)
        mLog.LogCGBAssemblies(currentRow.RowIndex).Landings = Trim(txtCGBLandings.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtCGBCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtCGBCycles As TextBox = TryCast(currentRow.FindControl("txtCGBCycles"), TextBox)
        mLog.LogCGBAssemblies(currentRow.RowIndex).Cycles = Trim(txtCGBCycles.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtCGBStarts_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtCGBStarts As TextBox = TryCast(currentRow.FindControl("txtCGBStarts"), TextBox)
        mLog.LogCGBAssemblies(currentRow.RowIndex).Starts = Trim(txtCGBStarts.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtCGBNGCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtCGBNGCycles As TextBox = TryCast(currentRow.FindControl("txtCGBNGCycles"), TextBox)
        mLog.LogCGBAssemblies(currentRow.RowIndex).NGCycles = Trim(txtCGBNGCycles.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtCGBNFCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtCGBNFCycles As TextBox = TryCast(currentRow.FindControl("txtCGBNFCycles"), TextBox)
        mLog.LogCGBAssemblies(currentRow.RowIndex).NFCycles = Trim(txtCGBNFCycles.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtCGBRINS_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtCGBRins As TextBox = TryCast(currentRow.FindControl("txtCGBRins"), TextBox)
        mLog.LogCGBAssemblies(currentRow.RowIndex).RINS = Trim(txtCGBRins.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtCGBBleeds_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtCGBBleeds As TextBox = TryCast(currentRow.FindControl("txtCGBBleeds"), TextBox)
        mLog.LogCGBAssemblies(currentRow.RowIndex).Bleeds = Trim(txtCGBBleeds.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtCGBImpellerCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtCGBImpellerCycles As TextBox = TryCast(currentRow.FindControl("txtCGBImpellerCycles"), TextBox)
        mLog.LogCGBAssemblies(currentRow.RowIndex).ImpellerCycles = Trim(txtCGBImpellerCycles.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtCGBCTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtCGBCTCycles As TextBox = TryCast(currentRow.FindControl("txtCGBCTCycles"), TextBox)
        mLog.LogCGBAssemblies(currentRow.RowIndex).CTCycles = Trim(txtCGBCTCycles.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtCGBPTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtCGBPTCycles As TextBox = TryCast(currentRow.FindControl("txtCGBPTCycles"), TextBox)
        mLog.LogCGBAssemblies(currentRow.RowIndex).PTCycles = Trim(txtCGBPTCycles.Text)
        DataBindGrid()

    End Sub
    ' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
    Protected Sub txtCGBGeneratorMods_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtCGBGeneratorMods As TextBox = TryCast(currentRow.FindControl("txtCGBGeneratorMods"), TextBox)
        mLog.LogCGBAssemblies(currentRow.RowIndex).GeneratorMods = Trim(txtCGBGeneratorMods.Text)
        DataBindGrid()
    End Sub

#End Region



#Region "Web Methods"
    <WebMethod(EnableSession:=True)> _
    Public Shared Function LogDetails(MachineID, LogDate) As Object
        Dim mLogListOnDate As LogList = LogList.GetLogList(MachineID, LogDate.Text.ToString, LogDate.Text.ToString)
        Return mLogListOnDate
    End Function
#End Region
End Class
