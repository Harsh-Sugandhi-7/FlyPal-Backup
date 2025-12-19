Imports System.Collections.Generic
Imports Flypal.ModelListAutoComplete
Imports System.Linq
Imports System.Text

Public Class wfCustomerContractTasks_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCustomerContract As CustomerContract
    Public mLocationList As LocationList
    Public mCapabilityTaskList As CapabilityTaskList

    Dim Flag As Int16
#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mCustomerContract = Session("mCustomerContract")
        mLocationList = Session("mLocationList")
        mCapabilityTaskList = Session("mCapabilityTaskList")
    End Sub
    Public Sub SetSession()
        Session("mCustomerContract") = mCustomerContract
        Session("mLocationList") = mLocationList
        Session("mCapabilityTaskList") = mCapabilityTaskList
    End Sub
    Private Sub SetTitle()
        If mCustomerContract.IsNew Then
            lbltitle.Text = "Customer Contract Tasks [New]"
        Else

            lbltitle.Text = "Customer Contract Tasks"

        End If
        upnlTitle.Update()
    End Sub
    Private Sub DataFieldBind()
        mLocationList = LocationList.GetLocationList(0, IsSelectTagRequired:=True)
        cmbLocation.DataSource = mLocationList
        Session("mLocationList") = mLocationList

        mCapabilityTaskList = CapabilityTaskList.GetCapabilityTaskList(AddTopItem:="(SELECT)")
        cmbCapabilityTask.DataSource = mCapabilityTaskList
        Session("mCapabilityTaskList") = mCapabilityTaskList

        dgSkillList.DataSource = mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills

        DataBind()

        cmbCapabilityTask.SelectedValue = mCustomerContract.CustomerContractTasks.CurrentItem.CapabilityTaskID.ToString
        cmbLocation.SelectedValue = mCustomerContract.CustomerContractTasks.CurrentItem.LocationID.ToString
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                   
                    If CustomValidateSkill() = False Then
                        upnlValidationsummary.Update()
                        Exit Sub
                    End If
                    If Not customvalidate1() Then upnlValidationsummary.Update() : Exit Sub
                    If Save() Then
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                    End If
                Case MsgBoxResult.No


                    If MSGBoxCtrl.Sender = "Close" Then


                        If mCustomerContract.CustomerContractTasks.CurrentItem.IsNew Then
                            If mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.Count > 0 Then
                                If mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.CurrentItem.IsNew Then
                                    mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.Remove(mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.CurrentItem)
                                End If
                            End If

                            mCustomerContract.CustomerContractTasks.Remove(mCustomerContract.CustomerContractTasks.CurrentItem)
                        Else
                            If mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.Count > 0 Then
                                If mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.CurrentItem.IsNew Then
                                    mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.Remove(mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.CurrentItem)
                                End If
                            End If

                        End If
                        If Session("Edit") = True Then
                            mCustomerContract = IIf(Session("mCustomerContractClone") Is Nothing, mCustomerContract, Session("mCustomerContractClone"))
                        End If
                        Session.Remove("Edit")
                    End If

                    Session("mCustomerContract") = mCustomerContract
                    Session("sender") = ""
                    Dim mopenas As String = Request.QueryString("Type")
                    If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                        Exit Sub
                    End If
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""

        End If
    End Sub
    Private Sub SetObject()
        mCustomerContract.CustomerContractTasks.CurrentItem.LocationID = New Guid(cmbLocation.SelectedValue.ToString)
        mCustomerContract.CustomerContractTasks.CurrentItem.LocationName = IIf(cmbLocation.SelectedIndex = 0, "", cmbLocation.SelectedItem.Text) 'mLocationList(New Guid(cmbLocation.SelectedValue.ToString)).Name
        mCustomerContract.CustomerContractTasks.CurrentItem.CapabilityTaskID = New Guid(cmbCapabilityTask.SelectedValue.ToString)
        mCustomerContract.CustomerContractTasks.CurrentItem.CapabilityTaskDescription = Trim(txtTaskDescription.Text)
        mCustomerContract.CustomerContractTasks.CurrentItem.TATHours = Val(Trim(txtTATHours.Text.ToString))
        mCustomerContract.CustomerContractTasks.CurrentItem.TATDays = Trim(txtTATDays.Text.ToString)
        mCustomerContract.CustomerContractTasks.CurrentItem.IsDays = chkIsInDays.Checked
        mCustomerContract.CustomerContractTasks.CurrentItem.IsFixedRate = chkIsFixedRate.Checked
        mCustomerContract.CustomerContractTasks.CurrentItem.CFixedRate = Val(txtCRate.Text.ToString)

        mCustomerContract.CustomerContractTasks.CurrentItem.SparesMarkupPercent = Val(txtSpareMarkUpPercent.Text.ToString)
        mCustomerContract.CustomerContractTasks.CurrentItem.ConversionFactor = mCustomerContract.ConversionFactor


        mCustomerContract.CustomerContractTasks.CurrentItem.IsHangarInDays = Not rdbHangarHour.Checked
        mCustomerContract.CustomerContractTasks.CurrentItem.IsHangarInDays = rdbHangarDay.Checked
        mCustomerContract.CustomerContractTasks.CurrentItem.CHangarUsageperHourRate = Val(txtHangarHour.Text.ToString)
        mCustomerContract.CustomerContractTasks.CurrentItem.CHangarUsageperDaytRate = Val(txtHangarDay.Text.ToString)

        mCustomerContract.CustomerContractTasks.CurrentItem.IsParkingInDays = Not rdbParkingSpaceHour.Checked
        mCustomerContract.CustomerContractTasks.CurrentItem.IsParkingInDays = rdbParkingSpaceDay.Checked
        mCustomerContract.CustomerContractTasks.CurrentItem.CParkingSpaceperHourRate = Val(txtParkingSpaceHour.Text.ToString)
        mCustomerContract.CustomerContractTasks.CurrentItem.CParkingSpaceperDayRate = Val(txtParkingSpaceDay.Text.ToString)

        Session("mCustomerContract") = mCustomerContract
    End Sub
    Public Function Save() As Boolean

        SetObject()
        setObjectSkill()
        If Not mCustomerContract.CustomerContractTasks.CurrentItem.IsValid Then Return False
        Session("mCustomerContract") = mCustomerContract
        Return True
    End Function
    Public Sub ControlVisibility()
        'TAT
        If chkIsInDays.Checked Then
            txtTATDays.Visible = True
            txtTATHours.Visible = False
            txtTATHours.Text = "0"
        Else
            txtTATHours.Visible = True
            txtTATDays.Visible = False
            txtTATDays.Text = "0"
        End If

        'Fixed Rate
        If chkIsFixedRate.Checked Then
            pnlfixedRate.Enabled = True
            pnlNotFixedRate.Enabled = False

            txtHangarHour.Text = "0.00"
            txtHangarDay.Text = "0.00"
            txtParkingSpaceHour.Text = "0.00"
            txtParkingSpaceDay.Text = "0.00"
        Else
            pnlfixedRate.Enabled = False
            pnlNotFixedRate.Enabled = True
        End If

        'Hangar
        If rdbHangarDay.Checked And mCustomerContract.StatusID = 1 Then
            txtHangarDay.Enabled = True
            txtHangarHour.Enabled = False


            txtHangarDay.ReadOnly = False
            txtHangarDay.BackColor = Color.White
            txtHangarHour.Text = "0.00"


            txtHangarHour.ReadOnly = True
            txtHangarHour.BackColor = Color.FromName("#E0E0E0")
        ElseIf rdbHangarHour.Checked And mCustomerContract.StatusID = 1 Then
            txtHangarDay.Enabled = False
            txtHangarHour.Enabled = True

            txtHangarDay.ReadOnly = True
            txtHangarDay.BackColor = Color.FromName("#E0E0E0")

            txtHangarHour.ReadOnly = False
            txtHangarHour.BackColor = Color.White
            txtHangarDay.Text = "0.00"

        End If

        'Parking
        If rdbParkingSpaceDay.Checked Then
            txtParkingSpaceDay.Enabled = True
            txtParkingSpaceHour.Enabled = False


            txtParkingSpaceDay.ReadOnly = False
            txtParkingSpaceDay.BackColor = Color.White
            txtParkingSpaceHour.Text = "0.00"


            txtParkingSpaceHour.ReadOnly = True
            txtParkingSpaceHour.BackColor = Color.FromName("#E0E0E0")
        ElseIf rdbParkingSpaceHour.Checked Then
            txtParkingSpaceDay.Enabled = False
            txtParkingSpaceHour.Enabled = True

            txtParkingSpaceDay.ReadOnly = True
            txtParkingSpaceDay.BackColor = Color.FromName("#E0E0E0")

            txtParkingSpaceHour.ReadOnly = False
            txtParkingSpaceHour.BackColor = Color.White
            txtParkingSpaceDay.Text = "0.00"
        End If

        If Not mCustomerContract.StatusID = 1 Then
            txtHangarDay.Enabled = False
            txtHangarHour.Enabled = False
            txtParkingSpaceDay.Enabled = False
            txtParkingSpaceHour.Enabled = False
            txtCRate.Enabled = False
        End If


    End Sub
    Public Function customvalidate1() As Boolean

        If Flag = 1 Then Exit Function
        SetObject()
        Dim str As String = ""
         If Not mCustomerContract.CustomerContractTasks.CurrentItem.IsValid Then
            For i As Integer = 0 To mCustomerContract.CustomerContractTasks.CurrentItem.GetBrokenRulesCollection.Count - 1
                str = str + mCustomerContract.CustomerContractTasks.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If str <> "" Then
            cvValid.ErrorMessage = str
            cvValid.IsValid = False
            Return False
        End If
        Flag = 1
        Return True
    End Function
    Private Function CustomValidateSkill() As Boolean
        Dim strError As String = String.Empty
        Dim builder = New StringBuilder()


        Dim txtContractSkill As TextBox
        Dim txtSkillRate As TextBox
        Dim lblDuplicateSkill As Label
        Dim cvValidatorHeader As RequiredFieldValidator
        Dim upnlHeaderValidate As UpdatePanel

        For j As Integer = 0 To dgSkillList.Rows.Count - 1
        
            cvValidatorHeader = CType(Me.dgSkillList.Rows(j).FindControl("rfvHeader"), RequiredFieldValidator)
            upnlHeaderValidate = CType(Me.dgSkillList.Rows(j).FindControl("upnlSkillValidate"), UpdatePanel)
            txtContractSkill = CType(Me.dgSkillList.Rows(j).FindControl("txtContractSkill"), TextBox)
            txtSkillRate = CType(Me.dgSkillList.Rows(j).FindControl("txtSkillRate"), TextBox)
            lblDuplicateSkill = CType(Me.dgSkillList.Rows(j).FindControl("lblDuplicateSkill"), Label)
            Dim mSkillHeader As Skill = Skill.GetSkillByName(Trim(txtContractSkill.Text))


            If txtContractSkill.Text = "" Then
                cvValidatorHeader.IsValid = False
                cvValidatorHeader.Text = "* Skill Required"
                strError = "* Skill Required"
                upnlHeaderValidate.Update()
            ElseIf mSkillHeader.Name = "" Then
                cvValidatorHeader.IsValid = False
                cvValidatorHeader.Text = "* Select proper Skill"
                strError = "* Select proper Skill"
                upnlHeaderValidate.Update()
            ElseIf txtSkillRate.Text = "" Or txtSkillRate.Text = "0" Then
                cvValidatorHeader.IsValid = False
                cvValidatorHeader.Text = "* Rate Required"
                strError = "* Rate Required"
                upnlHeaderValidate.Update()
            End If
        Next

        If strError <> "" Then
            Return False
        End If
        Return True
    End Function
    Private Sub addAttributes()
        txtHangarHour.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
        txtHangarDay.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
        txtParkingSpaceHour.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
        txtParkingSpaceDay.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
        txtSpareMarkUpPercent.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
        txtTATDays.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
        txtCRate.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
        txtTATHours.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
    End Sub
    Private Sub SetAttributes()
        Dim txtValue As TextBox

        For i As Integer = 0 To dgSkillList.Rows.Count - 1

            Try
                txtValue = CType(Me.dgSkillList.Rows(i).FindControl("txtSkillRate"), TextBox)
                txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

            Catch ex As Exception
                Dim a As Integer = 0
            End Try

        Next

    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        SetTitle()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            ControlVisibility()
        End If
        SetAttributes()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked

        MessageBoxResult()

    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not customvalidate1() Then upnlValidationsummary.Update() : Exit Sub

        If hiddenSkillError.Value = "error" Then
            MSGBoxCtrl.Show("Alert..!!", "Duplicate Skill cannot be added. Please select another Skill.", "", MsgBoxStyle.OkOnly, "")
            hiddenSkillError.Value = ""
            Exit Sub
        End If
        If CustomValidateSkill() = False Then
            upnlValidationsummary.Update()
            Exit Sub
        End If
        If Page.IsValid Then
            If Save() Then
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
            End If
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetObject()
        SetSession()
        If mCustomerContract.CustomerContractTasks.CurrentItem.IsDirty Then
            Session("IsValid") = "True"
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
          
            If mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.Count > 0 Then
                If mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.CurrentItem.IsNew Then
                    mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.Remove(mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.CurrentItem)
                End If
            End If
            If mCustomerContract.CustomerContractTasks.CurrentItem.IsNew Then
                mCustomerContract.CustomerContractTasks.Remove(mCustomerContract.CustomerContractTasks.CurrentItem)
            End If
            Session("mCustomerContract") = mCustomerContract
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        End If

    End Sub

    Private Sub cmbCapabilityTask_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCapabilityTask.SelectedIndexChanged
        If cmbCapabilityTask.SelectedIndex > 0 Then
            txtTaskDescription.Text = mCapabilityTaskList(cmbCapabilityTask.SelectedIndex).TaskDescription

        End If
    End Sub
    Private Sub chkIsFixedRate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsFixedRate.CheckedChanged
        If chkIsFixedRate.Checked Then
            pnlfixedRate.Enabled = True
            pnlNotFixedRate.Enabled = False
            txtHangarHour.Text = "0.00"
            txtHangarDay.Text = "0.00"
            txtParkingSpaceHour.Text = "0.00"
            txtParkingSpaceDay.Text = "0.00"
            hiddenSkillError.Value = ""
            txtParkingSpaceDay.Text = "0.00"
            txtSpareMarkUpPercent.Text = "0.00"
            If mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.Count > 0 Then
                For i As Integer = mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.Count - 1 To 0 Step -1
                    mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.RemoveAt(i)
                Next

                dgSkillList.DataSource = mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills
                dgSkillList.DataBind()
                ''upnlSkill
            End If


        Else
            pnlfixedRate.Enabled = False
            pnlNotFixedRate.Enabled = True
            txtCRate.Text = "0.00"
        End If
    End Sub
    Private Sub rdbHangarDay_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbHangarDay.CheckedChanged, rdbHangarHour.CheckedChanged
        If rdbHangarDay.Checked Then
            txtHangarDay.Enabled = True
            txtHangarHour.Enabled = False


            txtHangarDay.ReadOnly = False
            txtHangarDay.BackColor = Color.White
            txtHangarHour.Text = "0.00"


            txtHangarHour.ReadOnly = True
            txtHangarHour.BackColor = Color.FromName("#E0E0E0")
        ElseIf rdbHangarHour.Checked Then
            txtHangarDay.Enabled = False
            txtHangarHour.Enabled = True

            txtHangarDay.ReadOnly = True
            txtHangarDay.BackColor = Color.FromName("#E0E0E0")

            txtHangarHour.ReadOnly = False
            txtHangarHour.BackColor = Color.White
            txtHangarDay.Text = "0.00"
        End If
    End Sub
    Private Sub rdbParkingSpaceDay_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbParkingSpaceDay.CheckedChanged, rdbParkingSpaceHour.CheckedChanged
        If rdbParkingSpaceDay.Checked Then
            txtParkingSpaceDay.Enabled = True
            txtParkingSpaceHour.Enabled = False


            txtParkingSpaceDay.ReadOnly = False
            txtParkingSpaceDay.BackColor = Color.White
            txtParkingSpaceHour.Text = "0.00"


            txtParkingSpaceHour.ReadOnly = True
            txtParkingSpaceHour.BackColor = Color.FromName("#E0E0E0")
        ElseIf rdbParkingSpaceHour.Checked Then
            txtParkingSpaceDay.Enabled = False
            txtParkingSpaceHour.Enabled = True

            txtParkingSpaceDay.ReadOnly = True
            txtParkingSpaceDay.BackColor = Color.FromName("#E0E0E0")

            txtParkingSpaceHour.ReadOnly = False
            txtParkingSpaceHour.BackColor = Color.White
            txtParkingSpaceDay.Text = "0.00"
        End If
    End Sub
    Private Sub chkIsInDays_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsInDays.CheckedChanged
        'TAT
        If chkIsInDays.Checked Then
            txtTATDays.Visible = True
            txtTATHours.Visible = False
            txtTATHours.Text = "0"
        Else
            txtTATHours.Visible = True
            txtTATDays.Visible = False
            txtTATDays.Text = "0"
        End If
    End Sub
    Protected Sub txtContractSkill_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtContractSkill As TextBox = TryCast(currentRow.FindControl("txtContractSkill"), TextBox)
        Dim txtSkillRate As TextBox = TryCast(currentRow.FindControl("txtSkillRate"), TextBox)

        '   Dim mSkillList As SkillList = SkillList.GetSkillList(Trim(txtContractSkill.Text), "")
        Dim mSkillHeader As Skill = Skill.GetSkillByName(Trim(txtContractSkill.Text))

        mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.CurrentItem.SkillID = mSkillHeader.ID
        mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.CurrentItem.CRate = txtSkillRate.Text
        mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.CurrentItem.ConversionFactor = mCustomerContract.ConversionFactor
        Session("mCustomerContract") = mCustomerContract


        ScriptManager.RegisterStartupScript(Me, Me.GetType, "CheckDuplicateSkill();", "CheckDuplicateSkill();", True)

    End Sub
    Protected Sub btnAddSkill_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddSkill.Click
        If CustomValidateSkill() = False Then upnlValidationsummary.Update() : Exit Sub

        setObjectSkill()

        mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.Add(mCustomerContract.CustomerContractTasks.CurrentItem.ID)
        dgSkillList.DataSource = mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills
        dgSkillList.DataBind()

        upnlSkill.Update()

    End Sub
    Private Sub dgSkillList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSkillList.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                setObjectSkill()
                mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.Remove(CInt(e.CommandArgument) - 1)
                dgSkillList.DataSource = mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills
                dgSkillList.DataBind()
                Session("mCustomerContract") = mCustomerContract
        End Select
    End Sub
    Private Sub setObjectSkill()
        Dim mCustomerContractClone As CustomerContract
        mCustomerContractClone = mCustomerContract.Clone
        Try
            Dim child As CustomerContractTaskSkill
            Dim txt As TextBox
            Dim ID As Guid
            For i As Integer = 0 To dgSkillList.Rows.Count - 1
                ID = New Guid(dgSkillList.DataKeys(i).Values("ID").ToString)
                child = mCustomerContract.CustomerContractTasks.CurrentItem.CustomerContractTaskSkills.Item(ID)
                txt = dgSkillList.Rows(i).FindControl("txtContractSkill")
                Dim mSkillList As SkillList = SkillList.GetSkillList(txt.Text)

                child.SkillID = mSkillList(0).ID
                child.SkillName = Trim(txt.Text)

                txt = dgSkillList.Rows(i).FindControl("txtSkillRate")
                child.CRate = Trim(txt.Text)

            Next
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Service Methods"
    'Skill
    <System.Web.Services.WebMethod(), System.Web.Script.Services.ScriptMethod()>
    Public Shared Function GetSkillList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim mSkillList As SkillList
        mSkillList = SkillList.GetSkillList(prefixText)

        Return (From c As SkillList.SkillInfo In mSkillList
          Select c.CodeWithName).Take(count).ToList

    End Function
#End Region








End Class