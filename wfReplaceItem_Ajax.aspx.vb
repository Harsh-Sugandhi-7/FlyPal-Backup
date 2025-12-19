Public Class wfReplaceItem_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItemList As ItemList
    Public mItem As Item
    Public CatIndex As Integer = 0
    Public RepCatIndex As Integer = 0
    Public Flag As Boolean = False
    Dim MsgText As String
    Dim Detail As String
    Dim EventLogID As Guid
    Public mItemHavingSameSerialNo As ItemHavingSameSerialNo
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mItemList = Session("mItemList")
        MsgText = Session("MsgText")
    End Sub
    Private Sub SetSession()
        Session("mItemList") = mItemList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemList")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfReplaceItem_Ajax.aspx?") <= 0 Then
            RemoveSession()
        End If
    End Sub
    Private Sub UpdateItemIDOfItems()
        Try
            Dim mItem As Item = Item.GetItem(New Guid(cmbItem.SelectedValue))

            Flag = Session("Flag")

            mItem.UpdateItemID(ItemID:=mItem.ID, ReplaceWithItemID:=New Guid(cmbReplaceWithItem.SelectedValue), _
                               ItemName:=mItem.Name, Description:=mItem.Description, _
                               ReplaceWithItemName:=cmbReplaceWithItem.SelectedItem.Text, _
                               ReplaceWithDescription:=Item.GetItem(New Guid(cmbReplaceWithItem.SelectedValue)).Description, _
                               IsReplaceAndDelete:=Flag)

            Session.Remove("Flag")

            Detail = "Original Item : " & cmbItem.SelectedItem.Text & " Replaced Item : " & cmbReplaceWithItem.SelectedItem.Text & IIf(Flag = True, (" Deleted Item : " & cmbItem.SelectedItem.Text + " ID: " + cmbItem.SelectedValue.ToString), "") & " By User : " & User.Identity.Name

            MarkLog(Util.Action.Save, "ReplaceItem", Detail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Continue1" Then
                        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, MsgText, MsgBoxStyle.YesNo, "Continue2")
                        Exit Sub
                    End If
                    If MSGBoxCtrl.Sender = "Continue2" Then
                        UpdateItemIDOfItems()
                        DataFieldBind()
                        VisibleFalse()
                        upnlItemDetails.Update()
                        upnlReplaceWithItemDetails.Update()
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Continue1" Then
                        DataFieldBind()
                    End If
                    If MSGBoxCtrl.Sender = "Continue2" Then
                        DataFieldBind()
                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Cannot" Then
                        DataFieldBind()
                        VisibleFalse()
                        upnlItemDetails.Update()
                        upnlReplaceWithItemDetails.Update()
                    End If
            End Select
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub VisibleFalse()
        lblDesc.Visible = False
        lblDescription.Visible = False
        lblUn.Visible = False
        lblUnit.Visible = False
        lblCat.Visible = False
        lblCategory.Visible = False
        lblSerialize.Visible = False
        lblSerializeStatus.Visible = False
        lblCalibration.Visible = False
        lblCalibrationInterval.Visible = False
        lblSerialNo.Visible = False
        lblSerialNumbser.Visible = False

        lblReplaceWithItemDesc.Visible = False
        lblReplaceWithItemDescription.Visible = False
        lblReplaceWithItemUn.Visible = False
        lblReplaceWithItemUnit.Visible = False
        lblReplaceWithItemCat.Visible = False
        lblReplaceWithItemCategory.Visible = False
        lblReplaceWithItemSerialize.Visible = False
        lblReplaceWithItemSerializeStatus.Visible = False
        lblReplaceWithItemCalibration.Visible = False
        lblReplaceWithItemCalibrationInterval.Visible = False
        lblReplaceWithItemSerialNo.Visible = False
        lblReplaceWithItemSerialNumbser.Visible = False
    End Sub
#End Region

#Region " DataFieldBind "
    Public Sub DataFieldBind()
        mItemList = ItemList.GetItemList(0, IsSelectTagRequired:=True)
        cmbItem.DataSource = mItemList
        Session("mItemList") = mItemList

        cmbReplaceWithItem.DataSource = mItemList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If cmbItem.Enabled = True Then
                setFocus(cmbItem)
            End If
            Session("MiddleFrame") = "wfReplaceItem_Ajax.aspx?"
            DataFieldBind()
        End If
    End Sub
    Private Sub btnReplaceNDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReplaceNDelete.Click
        If IsValid Then
            If lblUnit.Text = lblReplaceWithItemUnit.Text And lblCategory.Text = lblReplaceWithItemCategory.Text And
                lblSerializeStatus.Text = lblReplaceWithItemSerializeStatus.Text And lblCalibrationInterval.Text = lblReplaceWithItemCalibrationInterval.Text And
                lblSerialNumbser.Text <> lblReplaceWithItemSerialNumbser.Text Then
            ElseIf lblCategory.Text <> lblReplaceWithItemCategory.Text And lblSerialNumbser.Text = lblReplaceWithItemSerialNumbser.Text Then
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not replace item as both items information not matching/ Both items serial numbers are same.", MsgBoxStyle.OkOnly, "Cannot")
                Exit Sub
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not replace item as both items information not matching/ Both items serial numbers are not same.", MsgBoxStyle.OkOnly, "Cannot")
                Exit Sub
            End If
            'If CType(sender, Button).ID = "btnReplace" Then
            '    MsgText = "You Are Going To Replace Item " & cmbItem.SelectedItem.Text & " with " & cmbReplaceWithItem.SelectedItem.Text & "." & "<BR> <BR> Do you want to continue? "
            '    Flag = False
            'Else
            'If CType(sender, Button).ID = "btnReplaceNDelete" Then
            MsgText = "You Are Going To Replace & Delete Item " & cmbItem.SelectedItem.Text & " with " & cmbReplaceWithItem.SelectedItem.Text & "." & "<BR> <BR> Do you want to continue? "
            Flag = True
            'End If
            Session("MsgText") = MsgText
            Session("Flag") = Flag
            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, MsgText, MsgBoxStyle.YesNo, "Continue1")
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub cmbItem_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbItem.SelectedIndexChanged, cmbReplaceWithItem.SelectedIndexChanged
        If CType(sender, DropDownList).ID = "cmbItem" Then
            mItem = Item.GetItem(New Guid(cmbItem.SelectedValue))
            lblDesc.Visible = True
            lblDescription.Visible = True
            lblDescription.Text = mItem.Description
            lblUn.Visible = True
            lblUnit.Visible = True
            lblUnit.Text = mItem.UnitName
            lblCat.Visible = True
            lblCategory.Visible = True
            lblCategory.Text = mItem.CategoryName
            lblSerialize.Visible = True
            lblSerializeStatus.Visible = True
            If mItem.SerialisedStatus = True Then
                lblSerializeStatus.Text = "Yes"
            Else
                lblSerializeStatus.Text = "No"
            End If
            lblCalibration.Visible = True
            lblCalibrationInterval.Visible = True
            If mItem.CalibrationPeriodInID = 1 Then
                lblCalibrationInterval.Text = mItem.BenchmarkMonths.ToString + " In Days"
            ElseIf mItem.CalibrationPeriodInID = 2 Then
                lblCalibrationInterval.Text = mItem.BenchmarkMonths.ToString + " In Month"
            ElseIf mItem.CalibrationPeriodInID = 3 Then
                lblCalibrationInterval.Text = mItem.BenchmarkMonths.ToString + " In Year"
            Else
                lblCalibrationInterval.Text = ""
            End If
        End If
        If CType(sender, DropDownList).ID = "cmbReplaceWithItem" Then
            mItem = Item.GetItem(New Guid(cmbReplaceWithItem.SelectedValue))
            lblReplaceWithItemDesc.Visible = True
            lblReplaceWithItemDescription.Visible = True
            lblReplaceWithItemDescription.Text = mItem.Description
            lblReplaceWithItemUn.Visible = True
            lblReplaceWithItemUnit.Visible = True
            lblReplaceWithItemUnit.Text = mItem.UnitName
            lblReplaceWithItemCat.Visible = True
            lblReplaceWithItemCategory.Visible = True
            lblReplaceWithItemCategory.Text = mItem.CategoryName
            lblReplaceWithItemSerialize.Visible = True
            lblReplaceWithItemSerializeStatus.Visible = True
            If mItem.SerialisedStatus = True Then
                lblReplaceWithItemSerializeStatus.Text = "Yes"
            Else
                lblReplaceWithItemSerializeStatus.Text = "No"
            End If
            lblReplaceWithItemCalibration.Visible = True
            lblReplaceWithItemCalibrationInterval.Visible = True
            If mItem.CalibrationPeriodInID = 1 Then
                lblReplaceWithItemCalibrationInterval.Text = mItem.BenchmarkMonths.ToString + " In Days"
            ElseIf mItem.CalibrationPeriodInID = 2 Then
                lblReplaceWithItemCalibrationInterval.Text = mItem.BenchmarkMonths.ToString + " In Month"
            ElseIf mItem.CalibrationPeriodInID = 3 Then
                lblReplaceWithItemCalibrationInterval.Text = mItem.BenchmarkMonths.ToString + " In Year"
            Else
                lblReplaceWithItemCalibrationInterval.Text = ""
            End If

            If lblUnit.Text <> lblReplaceWithItemUnit.Text Then
                lblReplaceWithItemUnit.BackColor = Color.Yellow
            Else
                lblReplaceWithItemUnit.BackColor = Color.White
            End If
            If lblCategory.Text <> lblReplaceWithItemCategory.Text Then
                lblReplaceWithItemCategory.BackColor = Color.Yellow
            Else
                lblReplaceWithItemCategory.BackColor = Color.White
            End If
            If lblSerializeStatus.Text <> lblReplaceWithItemSerializeStatus.Text Then
                lblReplaceWithItemSerializeStatus.BackColor = Color.Yellow
            Else
                lblReplaceWithItemSerializeStatus.BackColor = Color.White
            End If
            If lblCalibrationInterval.Text <> lblReplaceWithItemCalibrationInterval.Text Then
                lblReplaceWithItemCalibrationInterval.BackColor = Color.Yellow
            Else
                lblReplaceWithItemCalibrationInterval.BackColor = Color.White
            End If

        End If
        If cmbItem.SelectedItem.Text = cmbReplaceWithItem.SelectedItem.Text Then
            If cmbItem.SelectedIndex = 0 Or cmbReplaceWithItem.SelectedIndex = 0 Then
                cmbItem.ClearSelection()
                cmbReplaceWithItem.ClearSelection()
                VisibleFalse()
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not replace item as both items are same.", MsgBoxStyle.OkOnly, "Cannot")
                cmbItem.ClearSelection()
                cmbReplaceWithItem.ClearSelection()
                VisibleFalse()
            End If

        ElseIf cmbItem.SelectedIndex > 0 And cmbReplaceWithItem.SelectedIndex > 0 Then
            mItemHavingSameSerialNo = ItemHavingSameSerialNo.GetItemList(cmbItem.SelectedValue.ToString, cmbReplaceWithItem.SelectedValue.ToString)
            If mItemHavingSameSerialNo.Count > 0 Then
                If mItemHavingSameSerialNo.Contains(New Guid(cmbItem.SelectedValue), New Guid(cmbReplaceWithItem.SelectedValue)) <> "" Then
                    lblSerialNo.Visible = True
                    lblSerialNumbser.Visible = True
                    lblReplaceWithItemSerialNo.Visible = True
                    lblReplaceWithItemSerialNumbser.Visible = True
                    lblSerialNumbser.Text = mItemHavingSameSerialNo.Contains(New Guid(cmbItem.SelectedValue), New Guid(cmbReplaceWithItem.SelectedValue))
                    lblReplaceWithItemSerialNumbser.Text = mItemHavingSameSerialNo.Contains(New Guid(cmbItem.SelectedValue), New Guid(cmbReplaceWithItem.SelectedValue))
                    If lblSerialNumbser.Text = lblReplaceWithItemSerialNumbser.Text Then
                        lblReplaceWithItemSerialNumbser.BackColor = Color.Green
                    End If
                End If
            Else
                lblSerialNo.Visible = False
                lblSerialNumbser.Visible = False
                lblReplaceWithItemSerialNo.Visible = False
                lblReplaceWithItemSerialNumbser.Visible = False
                lblSerialNumbser.Text = "a"
                lblReplaceWithItemSerialNumbser.Text = "b"
            End If
            End If
            upnlItemDetails.Update()
            upnlReplaceWithItemDetails.Update()
    End Sub
#End Region


End Class