Public Class wfCWPComp_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Description "
    Public mCWP As CWP
    Protected mItemList As ItemList
    Protected mEmployeeListForComboOnCWPComp As EmployeeListForCombo 'Added by Saylee on 18-Jan-2018 for BA15012018
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mCWP = Session("mCWP")
        mItemList = Session("mItemList")
        mEmployeeListForComboOnCWPComp = Session("mEmployeeListForComboOnCWPComp") 'Added by Saylee on 18-Jan-2018 for BA15012018
    End Sub
    Private Sub setSession()
        Session("mCWP") = mCWP
    End Sub
    Private Sub RemoveSession()
        Session.Remove("Edit")
        Session.Remove("mItemList")
        Session.Remove("mEmployeeListForComboOnCWPComp") 'Added by Saylee on 18-Jan-2018 for BA15012018
    End Sub
    Private Sub DataFieldBind()
        mItemList = ItemList.GetItemsList(0, IsSelectTagRequired:=True) 'PartListForCombo.GetPartListForCombo(Guid.Empty, "", , , "(SELECT)")
        cmbPartList.DataSource = mItemList
        Session("mItemList") = mItemList

        If mCWP.CWPComps.CurrentItem.ReleaseNoteDate Is DBNull.Value Then
            txtReleaseNoteDate.Text = ""
        Else
            txtReleaseNoteDate.Text = mCWP.CWPComps.CurrentItem.ReleaseNoteDateFormatted
        End If

        'Added by Saylee on 18-Jan-2018 for BA15012018
        mEmployeeListForComboOnCWPComp = EmployeeListForCombo.GetEmployeeListForCombo("(SELECT)")
        cmbTechEmployeeList.DataSource = mEmployeeListForComboOnCWPComp
        cmbEngEmployeeList.DataSource = mEmployeeListForComboOnCWPComp
        Session("mEmployeeListForComboOnCWPComp") = mEmployeeListForComboOnCWPComp

        If Not mCWP.CWPComps.CurrentItem.TechEmployeeID.Equals(Guid.Empty) Then
            Dim mTechLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mCWP.CWPComps.CurrentItem.TechEmpName.ToString, User.Identity.Name, True, "(SELECT)", False)
            cmbTechLicenseNoList.DataSource = mTechLicenseNoList
        End If

        If Not mCWP.CWPComps.CurrentItem.EngEmployeeID.Equals(Guid.Empty) Then
            Dim mEngLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mCWP.CWPComps.CurrentItem.EngEmpName.ToString, User.Identity.Name, True, "(SELECT)", False)
            cmbEngLicenseNoList.DataSource = mEngLicenseNoList
        End If
        '*********************************************
        DataBind()

        '  'Added by Saylee on 18-Jan-2018 for BA15012018
        If Not mCWP.CWPComps.CurrentItem.TechEmployeeID.Equals(Guid.Empty) And mCWP.CWPComps.CurrentItem.TechLicenseNo <> "" Then cmbTechLicenseNoList.SelectedValue = mCWP.CWPComps.CurrentItem.TechLicenseNo
        If Not mCWP.CWPComps.CurrentItem.EngEmployeeID.Equals(Guid.Empty) And mCWP.CWPComps.CurrentItem.EngLicenseNo <> "" Then cmbEngLicenseNoList.SelectedValue = mCWP.CWPComps.CurrentItem.EngLicenseNo
        '*****************************************************
    End Sub
    Private Function setObject() As Boolean
        mCWP.CWPComps.CurrentItem.SrNo = mCWP.CWPComps.CurrentIndex + 1
        mCWP.CWPComps.CurrentItem.PartID = New Guid(cmbPartList.SelectedValue)
        mCWP.CWPComps.CurrentItem.PartNo = Trim(txtPartNo.Text)
        mCWP.CWPComps.CurrentItem.Description = Trim(txtPartDescription.Text)
        mCWP.CWPComps.CurrentItem.OffSerialNo = Trim(txtOffSerialNo.Text)
        mCWP.CWPComps.CurrentItem.OnSerialNo = Trim(txtOnSerialNo.Text)
        mCWP.CWPComps.CurrentItem.Qty = Val(txtQty.Text)
        mCWP.CWPComps.CurrentItem.ReleaseNoteNo = Trim(txtReleaseNoteNo.Text)
        If txtReleaseNoteDate.Text <> "" Then
            mCWP.CWPComps.CurrentItem.ReleaseNoteDate = txtReleaseNoteDate.Text
        Else
            mCWP.CWPComps.CurrentItem.ReleaseNoteDate = DBNull.Value
        End If

        'Added by Saylee on 18-Jan-2018 for BA15012018
        mCWP.CWPComps.CurrentItem.TechEmployeeID = New Guid(cmbTechEmployeeList.SelectedValue)
        mCWP.CWPComps.CurrentItem.EngEmployeeID = New Guid(cmbEngEmployeeList.SelectedValue)


        mCWP.CWPComps.CurrentItem.TechEmpName = IIf(cmbTechEmployeeList.SelectedIndex > 0, mEmployeeListForComboOnCWPComp(New Guid(cmbTechEmployeeList.SelectedValue.ToString)).Name, "") 'cmbTechEmployeeList.SelectedItem.ToString
        mCWP.CWPComps.CurrentItem.EngEmpName = IIf(cmbEngEmployeeList.SelectedIndex > 0, mEmployeeListForComboOnCWPComp(New Guid(cmbEngEmployeeList.SelectedValue.ToString)).Name, "")
        '*******************************************
        mCWP.ApplyEdit()
        Return True
    End Function
    Private Sub SetLicenseNo()  'Added by Saylee on 18-Jan-2018 for BA15012018
        If cmbTechEmployeeList.SelectedIndex > 0 Then
            cmbTechLicenseNoList.SelectedValue = mCWP.CWPComps.CurrentItem.TechLicenseNo
        End If

        If cmbEngEmployeeList.SelectedIndex > 0 Then
            cmbEngLicenseNoList.SelectedValue = mCWP.CWPComps.CurrentItem.EngLicenseNo
        End If
    End Sub
    Private Sub addAttributes()
        txtQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQty').value,event)")
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        Dim mQtyBalReceived As Decimal = 0
        If custValidator.ControlToValidate = "cmbPartList" Then
            If cmbPartList.SelectedIndex <= 0 And (txtPartNo.Text = "" Or txtPartDescription.Text = "") Then
                custValidator.ErrorMessage = "Either select Part No. from list or enter Part and Description manually"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtQty" Then
            If Val(txtQty.Text) <= 0 Then
                custValidator.ErrorMessage = "Quantity required"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub ControlVisibility()
        If cmbPartList.SelectedIndex = 0 Then
            txtPartNo.Enabled = True
            txtPartNo.BackColor = Color.White
            txtPartDescription.Enabled = True
            txtPartDescription.BackColor = Color.White
           
        Else
            txtPartNo.Enabled = False
            txtPartNo.BackColor = Color.Gainsboro
            txtPartDescription.Enabled = False
            txtPartDescription.BackColor = Color.Gainsboro
        End If
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        getSession()
        addAttributes()
        If Not IsPostBack Then
            cmbPartList.Focus()
            DataFieldBind()
            ControlVisibility()
            SetLicenseNo()  'Added by Saylee on 18-Jan-2018 for BA15012018
        End If
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If IsValid Then
            If mCWP.CWPComps.CurrentItem.IsNew And Not Session("Edit") = True And mCWP.CWPComps.Contains(Trim(txtPartNo.Text)) Then
                Session("Duplicate") = "Duplicate"
                Session("ToCleareList") = "ToCleareList"
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            If setObject() Then
                Session("mCWP") = mCWP
                RemoveSession()
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mCWP.CWPComps.CurrentItem.IsNew And Not Session("Edit") = True Then mCWP.CWPComps.Remove(mCWP.CWPComps.CurrentItem)
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub cmbPartList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbPartList.SelectedIndexChanged
        If cmbPartList.SelectedIndex = 0 Then
            txtPartNo.Text = ""
            txtPartDescription.Text = ""
        Else
            txtPartNo.Text = mItemList(cmbPartList.SelectedIndex).Name
            txtPartDescription.Text = mItemList(cmbPartList.SelectedIndex).Description
        End If
        ControlVisibility()
    End Sub
    Private Sub cmbTechEmployeeList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbTechEmployeeList.SelectedIndexChanged
        ControlVisibility()
        If cmbTechEmployeeList.SelectedIndex > 0 Then
            Dim mTechLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mEmployeeListForComboOnCWPComp(New Guid(cmbTechEmployeeList.SelectedValue.ToString)).Name, User.Identity.Name, True, "(SELECT)", False)

            cmbTechLicenseNoList.DataSource = mTechLicenseNoList
            cmbTechLicenseNoList.DataBind()
        Else
            cmbTechLicenseNoList.ClearSelection()
        End If
        'mCWP.CWPComps.CurrentItem.TechLicenseNo = ""

        Dim mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(cmbTechEmployeeList.SelectedValue.ToString, mCWP.CWPDateFormatted.ToString)
        If (mEmployeeStatus(0).Information <> "") Then
            cmbTechEmployeeList.SelectedIndex = 0
            cmbTechLicenseNoList.ClearSelection()
            MSGBoxCtrl.show("Save Alert!", mEmployeeStatus(0).Information, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Private Sub cmbEngEmployeeList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbEngEmployeeList.SelectedIndexChanged
        ControlVisibility()
        'If cmbEngEmployeeList.SelectedIndex > 0 Then
        Dim mEngLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mEmployeeListForComboOnCWPComp(New Guid(cmbEngEmployeeList.SelectedValue.ToString)).Name, User.Identity.Name, True, "(SELECT)", False)

        cmbEngLicenseNoList.DataSource = mEngLicenseNoList
        cmbEngLicenseNoList.DataBind()
        'Else
        cmbEngLicenseNoList.ClearSelection()
        '  End If
        'mCWP.CWPComps.CurrentItem.EngLicenseNo = ""

        Dim mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(cmbEngEmployeeList.SelectedValue.ToString, mCWP.CWPDateFormatted.ToString)
        If (mEmployeeStatus(0).Information <> "") Then
            cmbEngEmployeeList.SelectedIndex = 0
            cmbEngEmployeeList.ClearSelection()
            MSGBoxCtrl.show("Save Alert!", mEmployeeStatus(0).Information, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Private Sub cmbEngLicenseNoList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbEngLicenseNoList.SelectedIndexChanged
        mCWP.CWPComps.CurrentItem.EngLicenseNo = IIf(cmbEngLicenseNoList.SelectedIndex > 0, cmbEngLicenseNoList.SelectedItem.ToString, "")
        Session("mCWP") = mCWP
    End Sub
    Private Sub cmbTechLicenseNoList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbTechLicenseNoList.SelectedIndexChanged
        mCWP.CWPComps.CurrentItem.TechLicenseNo = IIf(cmbTechLicenseNoList.SelectedIndex > 0, cmbTechLicenseNoList.SelectedItem.ToString, "")
        Session("mCWP") = mCWP
    End Sub
#End Region

 

    
End Class