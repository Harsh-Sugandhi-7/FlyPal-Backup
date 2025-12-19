'Created BY : Saylee
'Dated      : 30-Jan-2024

Public Class APPFlightLogFuelOil
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mLog As Log
    Public mMachine As Machine
    Public mTankList As TankList
    Public mFuelUpliftUnit As UnitListMain
    Private Flag As Int16

    Private mOpenFromWO As Boolean = False
    Private mWOStatusID As Integer = 0
    Private mStatusIDForWO As Integer = 0

    Private mOpenFromLogFuelNew As Boolean = False
    Dim EventLogID As Guid
    Dim mLogDetail As String

    Dim mUpdateFuelsOfAllAboveLogs As UpdateFuelsOfAllAboveLogs
    Public mFuelType As FuelType
    Public mFuelTypeList As FuelTypeList
    Public mnWO As nWO
#End Region

#Region "Data Binding"
    Private Sub DataFieldBind()
        mFuelTypeList = FuelTypeList.GetFuelTypeList("", "<SELECT>")
        cmbFuelType.DataSource = mFuelTypeList
        Session("mFuelTypeList") = mFuelTypeList

        'dgLogFuel.DataSource = mLog.LogFuels

        'dgLogOil.DataSource = mLog.LogOils
        mUpdateFuelsOfAllAboveLogs = UpdateFuelsOfAllAboveLogs.GetLogFuelAndOilList(mLog.ID, mLog.MachineID)
        Session("mUpdateFuelsOfAllAboveLogs") = mUpdateFuelsOfAllAboveLogs

        DataBind()
    End Sub
    Private Sub DataBindGrid()
        'dgLogFuel.DataSource = mLog.LogFuels
        'dgLogFuel.DataBind()

        'dgLogOil.DataSource = mLog.LogOils
        'dgLogOil.DataBind()

        ''txtTotalFuelUplift.DataBind()
        'txtTotalFuelOnDeparture.DataBind()
        'txtTotalFuelOnArrival.DataBind()
        'txtTotalFuelConsumption.DataBind()
        Session("mLog") = mLog
    End Sub

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mLog = CType(Session("mLog"), Log)
        mMachine = CType(Session("mMachine"), Machine)
        mTankList = CType(Session("mTankList"), TankList)
        mFuelUpliftUnit = CType(Session("mFuelUpliftUnit"), UnitListMain)
        mOpenFromWO = CType(Session("OpenFromWO"), Boolean)
        mWOStatusID = CType(Session("WOStatusID"), Integer)
        mStatusIDForWO = CType(Session("StatusIDForWO"), Integer)
        mOpenFromLogFuelNew = CType(Session("mOpenFromLogFuelNew"), Boolean)

        mUpdateFuelsOfAllAboveLogs = Session("mUpdateFuelsOfAllAboveLogs") 'Saylee on 16-Nov-2011 for ALL16112012
        mFuelType = CType(Session("mFuelType"), FuelType)  'Added By Shweta On 14-June-2013 For  ALL05062013
        mFuelTypeList = CType(Session("mFuelTypeList"), FuelTypeList) 'Added By Shweta On 14-June-2013 For  ALL05062013
        mnWO = Session("mnWO")
    End Sub
    Private Sub SetSession()
        Session("mLog") = mLog
        Session("mMachine") = mMachine
        Session("mTankList") = mTankList
        Session("mFuelUpliftUnit") = mFuelUpliftUnit
        Session("OpenFromWO") = mOpenFromWO
        Session("mWOStatusID") = mWOStatusID
        Session("mStatusIDForWO") = mStatusIDForWO
        Session("mOpenFromLogFuelNew") = mOpenFromLogFuelNew

        Session("mUpdateFuelsOfAllAboveLogs") = mUpdateFuelsOfAllAboveLogs 'Saylee on 16-Nov-2011 for ALL16112012
        Session("mFuelType") = mFuelType
        Session("FuelTypeList") = mFuelTypeList
    End Sub
    Private Sub ShowAlertMsg(ByVal Msg As String, ByVal MsgTitle As String, Optional ShowAgreebutton As Boolean = False, Optional AgreeString As String = "")

        Dim str As String
        If ShowAgreebutton = False Then
            str = "opennotificationpopup('" & Msg & "','" & MsgTitle & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str, True)
        Else
            str = "openAgreenotificationpopup('" & Msg & "','" & MsgTitle & "','" & AgreeString & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str, True)
        End If

    End Sub
    Private Sub RemoveSession()
        Session.Remove("mFuelUpliftUnit")
        Session.Remove("mTankList")
        Session.Remove("mMachine")
        Session.Remove("mWOStatusID")
        Session.Remove("mStatusIDForWO")
        Session.Remove("mOpenFromLogFuelNew")
        Session.Remove("mFuelType")
        Session.Remove("mFuelTypeList")
    End Sub
    Private Sub SetTitle()
        If mLog.IsNew Then 'New SmartDate(mLog.Date.ToString).FormattedText
            lblTitle.InnerText = "Log Details of " & mLog.LogNoLogPageNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"
        Else
            lblTitle.InnerText = "Log Details of " & mLog.LogNoLogPageNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText
        End If


        ' lblFuelOilUnit1.Text = UnitListMain.GetUnitList()(mMachine.UnitID).Name
        ' lblFuelOilUnit2.Text = (mLog.FuelUpLifts.CurrentItem.CUpLift).ToString + " " + lblFuelOilUnit1.Text
    End Sub
    Private Sub addAttributes()
        'txtTotalFuelUplift.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtTotalFuelUplift').value,event)")
        'lblFuelOilUnit2.Text = Val(txtTotalFuelUplift.Text.Trim)
    End Sub
    Private Sub SetObject()
        'mLog.FuelUpLifts.CurrentItem.UpLift = CDec(Val(txtTotalFuelUplift.Text)) 'txtFuelUplift
        'mLog.FuelUpLifts.CurrentItem.UnitID = CInt(cmbFuelUpliftUnit.SelectedValue)

        ''Added By Utkarsh ON 31-Aug-2012 FOR ALL-30082012
        'mLog.FuelUpLifts.CurrentItem.TOWeight = txtTOWeight.Text.Trim
        'mLog.FuelUpLifts.CurrentItem.Altitude = txtAltitude.Text.Trim
        'mLog.FuelUpLifts.CurrentItem.Remark = txtRemark.Text.Trim
        'End
        mLog.FuelUpLifts.CurrentItem.FuelTypeID = New Guid(cmbFuelType.SelectedValue.ToString)  'Added By Shweta On 14-June-2013 For  ALL05062013
        Session("mLog") = mLog
    End Sub
    Public Sub SetGridObject()        ' For First Grid i.e AirFrame
        Dim txtFuelUpLifted, txtFuelAtArrival As TextBox
        Dim txtWOFuelUpLifted, txtWOFuelDrainedOut As TextBox

        Dim txtBurnOnGround As TextBox  'Added By Utkarsh ON 31-Aug-2012 FOR ALL-30082012



        'For i As Integer = 0 To Me.dgLogFuel.Rows.Count - 1
        '    txtFuelUpLifted = CType(Me.dgLogFuel.Rows(i).FindControl("txtFuelUpLifted"), TextBox)
        '    txtFuelAtArrival = CType(Me.dgLogFuel.Rows(i).FindControl("txtFuelAtArrival"), TextBox)

        '    txtWOFuelUpLifted = CType(Me.dgLogFuel.Rows(i).FindControl("txtWOFuelUpLifted"), TextBox)
        '    txtWOFuelDrainedOut = CType(Me.dgLogFuel.Rows(i).FindControl("txtWOFuelDrainedOut"), TextBox)

        '    mLog.LogFuels.Item(i).FuelUplifted = Val(txtFuelUpLifted.Text.Trim)
        '    mLog.LogFuels.Item(i).FuelOnArrival = Val(txtFuelAtArrival.Text.Trim)

        '    mLog.LogFuels.Item(i).WOFuelUplifted = Val(txtWOFuelUpLifted.Text.Trim)
        '    mLog.LogFuels.Item(i).WOFuelDrainedOut = Val(txtWOFuelDrainedOut.Text.Trim)

        '    'Added By Utkarsh ON 31-Aug-2012 FOR ALL-30082012
        '    txtBurnOnGround = CType(Me.dgLogFuel.Rows(i).FindControl("txtBurnOnGround"), TextBox)
        '    mLog.LogFuels.Item(i).BurnOnGround = Val(txtBurnOnGround.Text.Trim)
        '    'End

        'Next i

        'Dim txtValue As TextBox
        'Dim txtUpdatedDate, txtUpdatedTime As TextBox  'Added By Vikrant On 21-Dec-2018 For ALL21122018
        '' '' ''For i As Integer = 0 To Me.dgLogOil.Items.Count - 1
        '' '' ''    txtValue = CType(Me.dgLogOil.Items(i).FindControl("txtValue"), TextBox)
        '' '' ''    mLog.LogOils.Item(i).Value = Val(txtValue.Text.Trim)
        '' '' ''Next i   
        'For i As Integer = 0 To Me.dgLogOil.Rows.Count - 1
        '    txtValue = CType(Me.dgLogOil.Rows(i).FindControl("txtValue"), TextBox)

        '    mLog.LogOils.Item(i).Value = Val(txtValue.Text.Trim)
        '    'Added By Vikrant On 21-Dec-2018 For ALL21122018
        '    txtUpdatedDate = CType(Me.dgLogOil.Rows(i).FindControl("txtUpdatedDate"), TextBox)
        '    txtUpdatedTime = CType(Me.dgLogOil.Rows(i).FindControl("txtTime"), TextBox)
        '    If txtUpdatedTime.Text <> "" Then
        '        mLog.LogOils.Item(i).OilUpdatedDateTime = CType(txtUpdatedDate.Text.ToString.Trim + " " + txtUpdatedTime.Text.ToString.Trim, DateTime)
        '    Else
        '        mLog.LogOils.Item(i).OilUpdatedDateTime = System.DBNull.Value
        '    End If
        '    'End
        'Next i

        Session("mLog") = mLog
    End Sub

    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs) ' Validation From AIRFRAMEGRID (Grid-1)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        SetObject()
        SetGridObject()
        Dim str As String = ""
        'Log
        If Not mLog.IsValid Then
            For i As Integer = 0 To mLog.GetBrokenRulesCollection.Count - 1
                str = str + mLog.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
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
            'cvFuelUpLiftList.ErrorMessage = str
            ShowAlertMsg(str, "Alert..!!")
            custValidator.IsValid = False
        End If
        Flag = 1
    End Sub
    Private Sub ControlVisibilityOnWO()

        ''Added by Saylee on 14-Dec-2010
        'Dim txtWOFuelUpLifted, txtWOFuelDrainedOut As TextBox
        '' '' ''Dim btnWOFuelUpLifted, btnWOFuelDrainedOut As Button
        'Dim txtFuelUpLifted, txtFuelAtArrival As TextBox
        '' '' ''Dim btnFuelUpLifted, btnFuelAtArrival As Button

        ''Added By Utkarsh ON 03-Sep-2012 FOR ALL-30082012
        'Dim txtBurnOnGround As TextBox
        '' '' ''Dim btnBurnOnGround As Button
        ''End   

        'For i As Integer = 0 To Me.dgLogFuel.Rows.Count - 1


        '    txtFuelUpLifted = CType(Me.dgLogFuel.Rows(i).FindControl("txtFuelUpLifted"), TextBox)
        '    txtFuelAtArrival = CType(Me.dgLogFuel.Rows(i).FindControl("txtFuelAtArrival"), TextBox)

        '    txtWOFuelUpLifted = CType(Me.dgLogFuel.Rows(i).FindControl("txtWOFuelUpLifted"), TextBox)
        '    txtWOFuelDrainedOut = CType(Me.dgLogFuel.Rows(i).FindControl("txtWOFuelDrainedOut"), TextBox)



        '    'Added By Utkarsh ON 03-Sep-2012 FOR ALL-30082012
        '    txtBurnOnGround = CType(Me.dgLogFuel.Rows(i).FindControl("txtBurnOnGround"), TextBox)

        '    'End

        '    If mOpenFromWO = False Then
        '        txtWOFuelUpLifted.Enabled = False
        '        txtWOFuelDrainedOut.Enabled = False


        '        txtFuelUpLifted.Enabled = True
        '        txtFuelAtArrival.Enabled = True


        '        'Added By Utkarsh ON 03-Sep-2012 FOR ALL-30082012
        '        txtBurnOnGround.Enabled = True
        '        ' '' ''btnBurnOnGround.Enabled = True

        '        txtTOWeight.Visible = True

        '        txtAltitude.Visible = True

        '        txtRemark.Visible = True



        '    Else
        '        txtWOFuelUpLifted.Enabled = True
        '        txtWOFuelDrainedOut.Enabled = True
        '        ' '' ''btnWOFuelUpLifted.Enabled = True
        '        ' '' ''btnWOFuelDrainedOut.Enabled = True

        '        txtFuelUpLifted.Enabled = False
        '        txtFuelAtArrival.Enabled = False
        '        ' '' ''btnFuelUpLifted.Enabled = False
        '        ' '' ''btnFuelAtArrival.Enabled = False


        '        'Added By Utkarsh ON 03-Sep-2012 FOR ALL-30082012
        '        txtBurnOnGround.Enabled = False
        '        ' '' ''btnBurnOnGround.Enabled = False

        '        txtTOWeight.Visible = False

        '        txtAltitude.Visible = False

        '        txtRemark.Visible = False
        '        'End
        '        ''txtWOFuelUpLifted.ReadOnly = IIf(mWOStatusID <> 3, True, False) And IIf(mStatusIDForWO <> 4, True, False)
        '        ''txtWOFuelDrainedOut.ReadOnly = IIf(mWOStatusID <> 3, True, False) And IIf(mStatusIDForWO <> 4, True, False)
        '        ''btnWOFuelUpLifted.Enabled = IIf(mWOStatusID = 3, True, False) And IIf(mStatusIDForWO = 4, True, False)
        '        ''btnWOFuelDrainedOut.Enabled = IIf(mWOStatusID = 3, True, False) And IIf(mStatusIDForWO = 4, True, False)
        '    End If
        'Next i

        'If mOpenFromWO = True Or mOpenFromLogFuelNew = True Then
        '    'btnLogDetails.Enabled = False
        '    'btnDefectActionList.Enabled = False
        '    'btnParameterList.Enabled = False
        '    'btnLogPax.Enabled = False
        '    'btnHobbsOffset.Enabled = False
        '    dgLogOil.Visible = IIf(mOpenFromWO = True, False, True)

        '    txtTotalFuelUplift.Enabled = False
        '    cmbFuelUpliftUnit.Enabled = False
        '    lblLogOil.Visible = IIf(mOpenFromWO = True, False, True)

        '    dgLogFuel.Enabled = IIf(mWOStatusID = 3, False, True) And IIf(mStatusIDForWO = 4, False, True)



        '    btnSave.Enabled = IIf(mWOStatusID = 3, False, True) And IIf(mStatusIDForWO = 4, False, True)
        '    'btnMaintenanceAcitvity.Enabled = False

        'Else
        '    'btnLogDetails.Enabled = True
        '    'btnDefectActionList.Enabled = True
        '    'btnParameterList.Enabled = True
        '    'btnLogPax.Enabled = True
        '    'btnHobbsOffset.Enabled = True
        '    dgLogOil.Visible = True

        '    txtTotalFuelUplift.Enabled = True
        '    cmbFuelUpliftUnit.Enabled = True
        '    lblLogOil.Visible = True
        '    'btnMaintenanceAcitvity.Enabled = True
        'End If
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        addAttributes()
        If Not IsPostBack And CType(Session("sender"), String) = "" Then

            'If txtTotalFuelUplift.Enabled = True Then
            '    SetFocus(txtTotalFuelUplift)
            'End If
            DataFieldBind()
        End If
        ' '' ''MessageBoxResult()
        SetTitle()
        ' ControlVisibility()

    End Sub
    'Private Sub txtTotalFuelUplift_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTotalFuelUplift.TextChanged
    '    SetObject()
    '    '  lblFuelOilUnit2.Text = (mLog.FuelUpLifts.CurrentItem.CUpLift).ToString + " " + lblFuelOilUnit1.Text
    'End Sub
    'Private Sub cmbFuelUpliftUnit_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFuelUpliftUnit.SelectedIndexChanged
    '    SetObject()
    '    'SetFocus(cmbFuelUpliftUnit)
    '    'lblFuelOilUnit2.Text = (mLog.FuelUpLifts.CurrentItem.CUpLift).ToString + " " + lblFuelOilUnit1.Text
    '    'upnlTotalFuelUpLift.Update()
    'End Sub
    Protected Sub txtFuelUpLifted_TextChanged(ByVal sender As Object, ByVal e As EventArgs)


        'Dim txtFuelUpLifted As TextBox = DirectCast(sender, TextBox)
        'Dim gv1 As GridViewRow = DirectCast(txtFuelUpLifted.NamingContainer, GridViewRow)


        'txtFuelUpLifted = CType(Me.dgLogFuel.Rows(gv1.RowIndex).FindControl("txtFuelUpLifted"), TextBox)
        'mLog.LogFuels(gv1.RowIndex).FuelUplifted = Val(txtFuelUpLifted.Text)
        'DataBindGrid()
        'ControlVisibilityOnWO()
        'customvalidate1(Nothing, Nothing)
        '''' upnlError.Update()
        'upnldgLogFuel.Update()

    End Sub

    Protected Sub txtFuelAtArrival_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Dim txtFuelAtArrival As TextBox = DirectCast(sender, TextBox)
        'Dim gv1 As GridViewRow = DirectCast(txtFuelAtArrival.NamingContainer, GridViewRow)

        'txtFuelAtArrival = CType(Me.dgLogFuel.Rows(gv1.RowIndex).FindControl("txtFuelAtArrival"), TextBox)
        'mLog.LogFuels(gv1.RowIndex).FuelOnArrival = Val(txtFuelAtArrival.Text)
        'DataBindGrid()
        'ControlVisibilityOnWO()
        'customvalidate1(Nothing, Nothing)
        ''''    upnlError.Update()
        'upnldgLogFuel.Update()

    End Sub

    Protected Sub txtBurnOnGround_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

        'Dim txtBurnOnGround As TextBox = DirectCast(sender, TextBox)
        'Dim gv1 As GridViewRow = DirectCast(txtBurnOnGround.NamingContainer, GridViewRow)
        'txtBurnOnGround = CType(Me.dgLogFuel.Rows(gv1.RowIndex).FindControl("txtBurnOnGround"), TextBox)
        'mLog.LogFuels.Item(gv1.RowIndex).BurnOnGround = Val(txtBurnOnGround.Text.Trim)
        'DataBindGrid()
        'ControlVisibilityOnWO()
        'customvalidate1(Nothing, Nothing)
        ''''   upnlError.Update()
        'upnldgLogFuel.Update()


    End Sub

    Protected Sub txtValue_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent

        'Dim txtValue As TextBox = TryCast(currentRow.FindControl("txtValue"), TextBox)

        'mLog.LogOils.Item(currentRow.RowIndex).Value = Val(txtValue.Text)    ' Trim(txtValue.Text)   'Changed by Yogita it characters entered in value textbox
        'DataBindGrid()
        'ControlVisibilityOnWO()
        'UpnldgLogOil.Update()
    End Sub
#End Region


End Class