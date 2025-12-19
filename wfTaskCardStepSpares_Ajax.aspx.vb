
'AJAX Conversion By : Saylee on 16-Sep-2014

Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Public Class wfTaskCardStepSpares_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mTaskCard As TaskCard
    Public mItemListForCombo As ItemList
    Public PartNo As String
    Public Description As String
    Public ItemID As Guid

    Dim clnTaskCard As TaskCard
    Public PartID As String = "{00000000-0000-0000-0000-000000000000}"
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mTaskCard = Session("mTaskCard")
        PartNo = Session("PartNo")
        Description = Session("Description")
        ItemID = Session("ItemID")
        mItemListForCombo = Session("mItemListForCombo")
        PartID = Session("PartID")
    End Sub
    Private Sub SetSession()
        Session("mTaskCard") = mTaskCard
        Session("PartNo") = PartNo
        Session("Description") = Description
        Session("ItemID") = ItemID
        Session("mItemListForCombo") = mItemListForCombo
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        'Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        cntrl.Focus()
    End Sub
    Private Sub SetPage()
        If Session("WorkSpareEdit") Then
            lblTitle.Text = "Additional Work Spare [" & mTaskCard.TaskCardStepsSpares.CurrentItem.PartNo & " ]"
        Else
            lblTitle.Text = "Additional Work Spare [New]"
        End If
        txtTaskCardNo.Text = mTaskCard.TaskCardNo
    End Sub
    Private Sub SetPartID()
        If hdnpartId.Value <> String.Empty Then
            PartID = hdnpartId.Value.ToString

        End If
    End Sub
    Private Function setObject() As Boolean
        'mTaskCard.TaskCardStepsSpares.CurrentItem.ItemID = New Guid(PartID) 'New Guid(cmbItemList.SelectedValue.ToString)
        ItemID = IIf(txtItemList.Text = "", Guid.Empty, mItemListForCombo(PartNo).ID)
        mTaskCard.TaskCardStepsSpares.CurrentItem.ItemID = ItemID
        mTaskCard.TaskCardStepsSpares.CurrentItem.PartNo = Trim(txtPartNo.Text)
        mTaskCard.TaskCardStepsSpares.CurrentItem.Description = Trim(txtDescription.Text)
        mTaskCard.TaskCardStepsSpares.CurrentItem.RequiredQty = Val(txtReqQty.Text)
        mTaskCard.TaskCardStepsSpares.CurrentItem.Remark = Trim(txtRemark.Text)
        mTaskCard.TaskCardStepsSpares.CurrentItem.OffSerialNo = Trim(txtOffSerialNo.Text)
        mTaskCard.TaskCardStepsSpares.CurrentItem.OnSerialNo = Trim(txtOnSerialNo.Text)
        Return True
    End Function
    Private Sub ControlVisibility()
        If txtItemList.Text <> String.Empty Then '  If cmbItemList.SelectedIndex > 0 Then
            txtPartNo.Enabled = False
            txtDescription.Enabled = False
        Else
            txtPartNo.Enabled = True
            txtDescription.Enabled = True
        End If
    End Sub
    'Added By Vikrant On 02-Jan-2014 For All02012014
    Private Sub SaveTaskCard()
        Try
            mTaskCard.Save()
            MarkLog(Util.Action.Save, "TaskCard", "Additional Work Spare : " + Chr(13) + "Part No. : " + txtPartNo.Text + "," + "Description : " + txtDescription.Text + "," + "Qty. : " + txtReqQty.Text, Util.ErrorType.NoError, mTaskCard.ID, EventLogID)
            MarkLog(Util.Action.Save, "TaskCard", "TaskCard No : " & mTaskCard.TaskCardNo, Util.ErrorType.NoError, mTaskCard.ID, EventLogID)
        Catch ex As Exception
            MSGBoxCtrl.show(MSGBox.Message_title.Exception, MSGBox.Message_text.ErrorMessage, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End Try
    End Sub
    'End
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mItemListForCombo = ItemList.GetItemList(0, , , , , , , False)
        Session("mItemListForCombo") = mItemListForCombo
        'cmbItemList.DataSource = mItemListForCombo
        'Session("mItemListForCombo") = mItemListForCombo
        PartID = "{00000000-0000-0000-0000-000000000000}"
        Session("PartID") = PartID
        DataBind()
        txtItemList.Text = IIf(Not mTaskCard.TaskCardStepsSpares.CurrentItem.ItemID.Equals(Guid.Empty), mTaskCard.TaskCardStepsSpares.CurrentItem.PartNo + " [" + mTaskCard.TaskCardStepsSpares.CurrentItem.Description + "]", "")
        hdnpartId.Value = IIf(Not mTaskCard.TaskCardStepsSpares.CurrentItem.ItemID.Equals(Guid.Empty), mTaskCard.TaskCardStepsSpares.CurrentItem.ItemID.ToString, "{00000000-0000-0000-0000-000000000000}")
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtItemList" Then
            If (txtItemList.Text.Trim.IndexOf("[") < 0 Or txtItemList.Text.Trim.IndexOf("]") < 0) Then
                custValidator.ErrorMessage = "Enter whole Part No. and Description"
                e.IsValid = False
            ElseIf Trim(txtPartNo.Text) = "" Then
                custValidator.ErrorMessage = "Part No. Required."
                e.IsValid = False
            ElseIf Len(txtPartNo.Text) > 50 Then
                custValidator.ErrorMessage = "Part No Should be less than or equal to  50 characters."
                e.IsValid = False
            ElseIf Trim(txtDescription.Text) = "" Then
                custValidator.ErrorMessage = "Description Required."
                e.IsValid = False
            ElseIf Len(txtDescription.Text) > 200 Then
                custValidator.ErrorMessage = "Description should be less than or equal to 200 characters."
                e.IsValid = False
            End If

            'ElseIf custValidator.ControlToValidate = "txtDescription" Then
            '    If Trim(txtDescription.Text) = "" Then
            '        custValidator.ErrorMessage = "Description Required."
            '        e.IsValid = False
            '    ElseIf Len(txtDescription.Text) > 200 Then
            '        custValidator.ErrorMessage = "Description should be less than or equal to 200 characters."
            '        e.IsValid = False
            '    End If
        ElseIf custValidator.ControlToValidate = "txtReqQty" Then
            'Commented and added by Shweta on 3rd Sep 2013 for BA02092013
            ' If Val(txtReqQty.Text) <= 0 Then 
            If Val(txtReqQty.Text) < 0 Then
                'end
                custValidator.ErrorMessage = "Qty cannot be negative."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 500 Then
                custValidator.ErrorMessage = "Remark should be less than or equal to 500 characters."
                e.IsValid = False
            End If
        End If
    End Sub
    Private Function CustomValidate1() As Boolean
        '10
        'Dim strMSG As String = ""
        'Dim mTaskCardStepsSpare As TaskCardSpare
        'If Not mTaskCard.TaskCardStepsSpares.IsValid Then
        '    For Each mTaskCardStepsSpare In mTaskCard.TaskCardStepsSpares
        '        For i As Integer = 0 To mTaskCardStepsSpare.GetBrokenRulesCollection.Count - 1
        '            strMSG = strMSG + mTaskCardStepsSpare.GetBrokenRulesCollection(i).Description + "<Br>"
        '        Next
        '    Next
        'End If
        'If strMSG.Trim <> "" Then
        '    cvDescription.ErrorMessage = strMSG
        '    cvDescription.IsValid = False
        '    Return False
        'End If
        'Return True
        '10

        Dim strMSG As String = ""
        Dim mTaskCardStepsSpare As TaskCardSpare
        If Not mTaskCard.IsValid Then
            For Each mTaskCardStepsSpare In mTaskCard.TaskCardStepsSpares
                For i As Integer = 0 To mTaskCardStepsSpare.GetBrokenRulesCollection.Count - 1
                    strMSG = strMSG + mTaskCardStepsSpare.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If
        If strMSG.Trim <> "" Then
            cvDescription.ErrorMessage = strMSG
            cvDescription.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub addAttributes()
        txtReqQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtReqQty').value,event)")
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        If Not IsPostBack Then
            setFocus(txtItemList)
            DataFieldBind()
            ControlVisibility()
            SetPage()
        End If
        
    End Sub
    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If (Not User.IsInRole("TaskCardNew") And mTaskCard.IsNew) Or (Not User.IsInRole("TaskCardEdit") And Not mTaskCard.IsNew) Then
            setObject()
             MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If Not Page.IsValid Then upnlValidationsummary.Update() : Exit Sub

        SetPartID()
        If Session("WorkSpareEdit") = False Then
            If Trim(txtPartNo.Text) <> "" And Not mTaskCard.TaskCardStepsSpares.Contains(mTaskCard.TaskCardStepsSpares.CurrentItem.ID, mTaskCard.ID, Trim(txtPartNo.Text)) Then
                setObject()
                If Not CustomValidate1() Then
                    upnlValidationsummary.Update()
                    Exit Sub
                End If
                Session("mTaskCard") = mTaskCard
                Session("wfTaskCard.TaskCard") = mTaskCard
                SaveTaskCard() 'Added By Vikrant On 03-Jan-2014 For All02012014
                Session("WorkSpareEdit") = False
            ElseIf Trim(txtPartNo.Text) <> "" And mTaskCard.TaskCardStepsSpares.Contains(mTaskCard.TaskCardStepsSpares.CurrentItem.ID, mTaskCard.ID, Trim(txtPartNo.Text)) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                mTaskCard.TaskCardStepsSpares.CurrentItem.Description = ""
                Exit Sub
            Else
                setObject()
                If Not CustomValidate1() Then
                    upnlValidationsummary.Update()
                    Exit Sub
                End If
            End If
        Else

            Dim clnTaskCard As TaskCard
            clnTaskCard = mTaskCard.Clone
            setObject()
            If Not CustomValidate1() Then
                mTaskCard = clnTaskCard
                Session("mTaskCard") = mTaskCard
                Session("wfTaskCard.TaskCard") = clnTaskCard
                upnlValidationsummary.Update()
                Exit Sub
            End If
            If mTaskCard.TaskCardStepsSpares.CurrentItem.IsDirty Then
                'If Not mTaskCard.TaskCardStepsSpares.Contains(mTaskCard.TaskCardStepsSpares.CurrentItem.ID, mTaskCard.TaskCardStepsSpares.CurrentItem.TaskCardID, New Guid(PartID)) Then

                'Added by Sachin 12-09-23 
                Dim EitherPartNoOrItemID As Boolean = False
                'If Not mTaskCard.TaskCardTools.Contains(mTaskCard.TaskCardTools.CurrentItem.ID, mTaskCard.TaskCardTools.CurrentItem.TaskCardID, New Guid(PartID)) Then
                ItemID = IIf(txtItemList.Text = "", Guid.Empty, mItemListForCombo(PartNo).ID)
                If ItemID.Equals(Guid.Empty) Then
                    If Not mTaskCard.TaskCardTools.Contains(mTaskCard.TaskCardTools.CurrentItem.ID, mTaskCard.TaskCardTools.CurrentItem.TaskCardID, PartNo:=PartNo) Then
                        EitherPartNoOrItemID = True
                    Else
                        EitherPartNoOrItemID = False
                    End If
                Else
                    If Not mTaskCard.TaskCardTools.Contains(mTaskCard.TaskCardTools.CurrentItem.ID, mTaskCard.TaskCardTools.CurrentItem.TaskCardID, ItemID:=ItemID) Then
                        EitherPartNoOrItemID = True
                    Else
                        EitherPartNoOrItemID = False
                    End If
                End If
                'If Not mTaskCard.TaskCardTools.Contains(mTaskCard.TaskCardTools.CurrentItem.ID, mTaskCard.TaskCardTools.CurrentItem.TaskCardID, ItemID) Then
                If EitherPartNoOrItemID = True Then
                    ''''''''''''''''''''''''''upto here 
                    'ItemID = IIf(txtItemList.Text = "", Guid.Empty, mItemListForCombo(PartNo).ID)
                    'If Not mTaskCard.TaskCardStepsSpares.Contains(mTaskCard.TaskCardStepsSpares.CurrentItem.ID, mTaskCard.TaskCardStepsSpares.CurrentItem.TaskCardID, ItemID) Then
                    ''These 2 lines commented by Sachin 12-009-23

                    If Not CustomValidate1() Then
                        mTaskCard = clnTaskCard
                        Session("mTaskCard") = mTaskCard
                        Session("wfTaskCard.TaskCard") = clnTaskCard
                        upnlValidationsummary.Update()
                        Exit Sub
                    End If
                    Session("mTaskCard") = mTaskCard
                    Session("wfTaskCard.TaskCard") = mTaskCard
                    SaveTaskCard() 'Added By Vikrant On 03-Jan-2014 For All02012014
                    setFocus(txtItemList)

                ElseIf Not mTaskCard.TaskCardStepsSpares.Contains(ItemID) Then
                    If Not CustomValidate1() Then
                        mTaskCard = clnTaskCard
                        Session("mTaskCard") = mTaskCard
                        Session("wfTaskCard.TaskCard") = clnTaskCard
                        upnlValidationsummary.Update()
                        Exit Sub
                    End If
                    Session("mTaskCard") = mTaskCard
                    Session("wfTaskCard.TaskCard") = clnTaskCard
                    SaveTaskCard() 'Added By Vikrant On 03-Jan-2014 For All02012014
                    setFocus(txtItemList)
                Else
                    mTaskCard = clnTaskCard
                    Session("mTaskCard") = mTaskCard
                    Session("wfTaskCard.TaskCard") = clnTaskCard
                    MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
            Session("WorkSpareEdit") = False
        End If
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("ItemID")
        Session.Remove("ComeForEdit")
        Session.Remove("mItemListForCombo")
        ControlVisibility()
        SetPage()

        upnlValidationsummary.Update()
        upnlSpareDetail.Update()

        'response.Redirect(Request.QueryString("BackPage1") & "?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
    End Sub
    Private Sub txtItemList_TextChanged(sender As Object, e As System.EventArgs) Handles txtItemList.TextChanged
        'txtPartNo.Text = IIf(cmbItemList.SelectedIndex > 0, mItemListForCombo(cmbItemList.SelectedIndex).Name, "")
        'txtDescription.Text = IIf(cmbItemList.SelectedIndex > 0, mItemListForCombo(cmbItemList.SelectedIndex).Description, "")
        'ControlVisibility()

        If (txtItemList.Text.Trim.IndexOf("[") > 0 And txtItemList.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtItemList.Text.Substring(0, txtItemList.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtItemList.Text.Trim, txtItemList.Text.Trim.IndexOf("[") + 2, txtItemList.Text.Trim.IndexOf("]") - txtItemList.Text.Trim.IndexOf("[") - 1).Trim
            txtPartNo.Text = PartNo
            txtDescription.Text = Description
        Else
            PartNo = Trim(txtItemList.Text)
            Description = Trim(txtItemList.Text)
            txtPartNo.Text = PartNo
            txtDescription.Text = ""
        End If
        SetSession()
        ControlVisibility()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        If mTaskCard.TaskCardStepsSpares.CurrentItem.IsNew And Not Session("WorkSpareEdit") = True Then mTaskCard.TaskCardStepsSpares.Remove(mTaskCard.TaskCardStepsSpares.CurrentItem)
        Session("WorkSpareEdit") = False
        upnlValidationsummary.Update()
        Session.Remove("mItemListForCombo")
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        'Response.Redirect(Request.QueryString("BackPage1") & "?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
    End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetItemList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()


        Dim ItemList As New ItemListAutoComplete
        ItemList = ItemListAutoComplete.GetItemList(prefixText, False)
        If count = 0 Then
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In ItemList
              Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).ToArray
        Else
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In ItemList
                   Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region
End Class