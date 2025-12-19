'AJAX Conversion By Vikrant On 11-Feb-2014

Public Class wfrptChangeReleaseNoteNo_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mChangeReleaseNoteNoList As ChangeReleaseNoteNoList
    Public mOpenState As Boolean
    Public mCurrentLocation As String
    Public mReceiptItemID As Guid
    Public mItemTypeList As ItemTypeList
    'Added by Vikrant on 4-AUG-2011
    Dim EventLogID As Guid

    Dim mReleaseNoteDate As String
    Dim mReleaseNoteNo As String
    Dim mReceiptDate As String
    Dim mChangedReleaseNoteNo As String
    Dim mChangedReleaseNoteDate As String
    Public mItemID As Guid
    Dim TempPartNo As String
    Dim SerialNo As String
    Dim ReceiptNo As String
    Public mReceiptID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mChangeReleaseNoteNoList = CType(Session("mChangeReleaseNoteNoList"), ChangeReleaseNoteNoList)
        mCurrentLocation = CType(Session("mCurrentLocation"), String)
        mReceiptItemID = CType(Session("mReceiptItemID"), Guid)
        mItemTypeList = Session("mItemTypeList")

        mItemID = CType(Session("mItemID"), Guid)
        mReleaseNoteNo = CType(Session("mReleaseNoteNo"), String)
        mReleaseNoteDate = CType(Session("mReleaseNoteDate"), String)
        mReceiptDate = CType(Session("mReceiptDate"), String)
        TempPartNo = Session("TempPartNo")
        SerialNo = Session("SerialNo")
        ReceiptNo = Session("ReceiptNo")
        mReceiptID = CType(Session("mReceiptID"), Guid)
    End Sub
      Private Sub RemoveSession()
        Session.Remove("Location")
        Session.Remove("PartType")
        Session.Remove("SearchIndex")
        Session.Remove("PartNoLocation")
        Session.Remove("mChangeReleaseNoteNoList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptChangeReleaseNoteNo_Ajax.aspx" Then
            RemoveSession()
        End If
    End Sub
    Private Sub ChangeLocation(ByVal mReceiptItemID As Guid, ByVal mItemID As Guid, ByVal mReleaseNoteNo As String, ByVal mReleaseNoteDate As String, ByVal mReceiptDate As String, ByVal TempPartNo As String, ByVal SerialNo As String, ByVal ReceiptNo As String, ByVal mReceiptID As Guid)
        Session("mReceiptItemID") = mReceiptItemID
        Session("mItemID") = mItemID
        Session("mReleaseNoteNo") = mReleaseNoteNo
        Session("mReleaseNoteDate") = mReleaseNoteDate
        Session("mReceiptDate") = mReceiptDate
        Session("TempPartNo") = TempPartNo
        Session("SerialNo") = SerialNo
        Session("ReceiptNo") = ReceiptNo
        Session("mReceiptID") = mReceiptID
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub BindGrid()
        dgPartSearch.DataSource = mChangeReleaseNoteNoList
        dgPartSearch.DataBind()
    End Sub
    Private Sub FindNow(ByVal PartNo As String, Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal ReleaseNoteNo As String = "")
        'This step is Imp when details form  is opened dirctly.
        dgPartSearch.DataSource = Nothing
        mChangeReleaseNoteNoList = Nothing

        'Get List From the Database as per Criteria
        mChangeReleaseNoteNoList = ChangeReleaseNoteNoList.GetChangeReleaseNoteNoList(PartNo, Text, No, ReleaseNoteNo)

        'Set DataSource of the Grid
        dgPartSearch.DataSource = mChangeReleaseNoteNoList
        Session("mChangeReleaseNoteNoList") = mChangeReleaseNoteNoList
    End Sub
    Public Sub SetControl()
        FindNow(Trim(txtPartNo.Text), "", CInt(IIf(Val(txtNo.Text) = 0, 0, Val(txtNo.Text))), Trim(txtReleaseNoteNo.Text))
        dgPartSearch.DataBind()

        lblResult.Text = "List of Parts :" & mChangeReleaseNoteNoList.Count & " Record(s) found. "
        upnlGrid.Update()
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mChangeReleaseNoteNoList = ChangeReleaseNoteNoList.GetChangeReleaseNoteNoList("", "", 0)
        dgPartSearch.DataSource = mChangeReleaseNoteNoList
        Session("mChangeReleaseNoteNoList") = mChangeReleaseNoteNoList
        dgPartSearch.DataBind()
        lblResult.Text = "List of Parts :" & mChangeReleaseNoteNoList.Count & " Record(s) found "
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'RemoveSession()
        ClearAll()
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Vikrant on 4-AUG-2011
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfrptChangeReleaseNoteNo_Ajax.aspx"
            If txtPartNo.Enabled = True Then
                SetFocus(txtPartNo)
            End If
            DataFieldBind()
            SetControl()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartSearch.PageIndex = 0

        FindNow(Trim(txtPartNo.Text), "", CInt(IIf(Val(txtNo.Text) = 0, 0, Val(txtNo.Text))), Trim(txtReleaseNoteNo.Text))
        dgPartSearch.DataBind()

        lblResult.Text = "List of Parts :" & mChangeReleaseNoteNoList.Count & " Record(s) found "
        upnlGrid.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Added by Vikrant on 4-AUG-2011
        MarkLog(Util.Action.Close, "Change Release Note No", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub dgPartSearch_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartSearch.RowCommand
        Select Case e.CommandName
            Case "Change"
                Dim index As Integer = CInt(e.CommandArgument) + dgPartSearch.PageSize * dgPartSearch.PageIndex

                Dim mReceiptItemID As Guid = mChangeReleaseNoteNoList(index).ID
                Dim mItemID As Guid = mChangeReleaseNoteNoList(index).ItemID
                Dim mReleaseNoteNo As String = mChangeReleaseNoteNoList(index).ReleaseNoteNo
                Dim mReleaseNoteDate As String = mChangeReleaseNoteNoList(index).ReleaseNoteDateFormatted.ToString
                Dim mReceiptDate As String = mChangeReleaseNoteNoList(index).DateFormatted.ToString
                Dim TempPartNo As String = mChangeReleaseNoteNoList(index).ItemName
                Dim SerialNo As String = mChangeReleaseNoteNoList(index).SerialNo
                Dim ReceiptNo As String = mChangeReleaseNoteNoList(index).ReceiptNo
                Dim mReceiptID As Guid = mChangeReleaseNoteNoList(index).ReceiptID
                ChangeLocation(mReceiptItemID, mItemID, mReleaseNoteNo, mReleaseNoteDate, mReceiptDate, TempPartNo, SerialNo, ReceiptNo, mReceiptID)
                'Added by Vikrant on 4-AUG-2011
                MarkLog(Util.Action.Edit, "Change Release Note No", "Part : " + TempPartNo + " Release Note No : " + mChangeReleaseNoteNoList.Item(mChangeReleaseNoteNoList.CurrentIndex).ReleaseNoteNo + " Release Note Date : " + mChangeReleaseNoteNoList.Item(mChangeReleaseNoteNoList.CurrentIndex).ReleaseNoteDateFormatted, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                BindValueForChangeReleaseNoteNo(mReleaseNoteNo, New SmartDate(mReleaseNoteDate).FormattedText)
                pnlReleaseNoteNo.Visible = True
                upnlChangeReleaseNoteNo.Update()
                mdlPopUpChangeReleaseNoteNo.Show()
                BindGrid()
        End Select
    End Sub
    Private Sub dgPartSearch_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartSearch.Sorting
        mChangeReleaseNoteNoList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mChangeReleaseNoteNoList") = mChangeReleaseNoteNoList
        dgPartSearch.DataSource = mChangeReleaseNoteNoList
        dgPartSearch.DataBind()
    End Sub
    Private Sub dgPartSearch_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartSearch.PageIndexChanging
        dgPartSearch.PageIndex = e.NewPageIndex
        Session("mChangeReleaseNoteNoList") = mChangeReleaseNoteNoList
        dgPartSearch.DataSource = mChangeReleaseNoteNoList
        dgPartSearch.DataBind()
    End Sub
#End Region

#Region "Change Release Note No"

#Region "Methods"
    Public Sub Save()
        'If Len(txtChangedReleaseNoteNo.Text) <> 0 Then
        ChangeReleaseNoteNoList.ChangeReleaseNoteNo(mReceiptItemID, mItemID, txtCurrentReleaseNoteNo.Text, txtChangedReleaseNoteNo.Text, txtCurrentReleaseNoteDate.Text, txtChangeReleaseNoteDate.Text)
        Dim ReceiptInfo As String
        Dim SerialNoInfo As String
        If SerialNo.ToString = "&nbsp;" Then
            SerialNoInfo = ""
        Else
            SerialNoInfo = SerialNo
        End If
        ReceiptInfo = "Part No. : " + TempPartNo + "  Serial No. : " + SerialNoInfo + " Receipt No. : " + ReceiptNo + " Old Release Note No. : " + txtCurrentReleaseNoteNo.Text + " New Release Note No. : " + txtChangedReleaseNoteNo.Text + " Old Release Note Date : " + txtCurrentReleaseNoteDate.Text + " New Release Note Date : " + txtChangeReleaseNoteDate.Text
        MarkLog(Util.Action.Save, "Receipt : Change Release Note No.", ReceiptInfo, Util.ErrorType.NoError, mReceiptItemID, EventLogID)
        RemoveSessionForReleaseNote()
        mdlPopUpChangeReleaseNoteNo.Hide()
        pnlReleaseNoteNo.Visible = False
        upnlChangeReleaseNoteNo.Update()
        SetControl()
        'End If
    End Sub
    Private Sub ResetValues()
        txtChangedReleaseNoteNo.Text = ""
        txtChangeReleaseNoteDate.Text = ""
    End Sub
    Public Sub Customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)

        If CustValid.ControlToValidate = "txtChangeReleaseNoteDate" Then
            If CDate(txtChangeReleaseNoteDate.Text) > CDate(mReceiptDate.ToString) Then
                CustValid.ErrorMessage = "Release note date should be less or equal to receipt date"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "SaveConfirmation" Then
                        mChangedReleaseNoteNo = Session("mChangedReleaseNoteNo")
                        txtChangedReleaseNoteNo.Text = mChangedReleaseNoteNo.ToString

                        mChangedReleaseNoteDate = Session("mChangedReleaseNoteDate")
                        txtChangeReleaseNoteDate.Text = mChangedReleaseNoteDate

                        Session.Remove("mChangedReleaseNoteNo")
                        Session.Remove("mChangedReleaseNoteDate")
                        Save()
                    End If
                Case MsgBoxResult.No
                    RemoveSessionForReleaseNote()
                    mdlPopUpChangeReleaseNoteNo.Hide()
                    pnlReleaseNoteNo.Visible = False
                    upnlChangeReleaseNoteNo.Update()
                    'Response.Redirect(Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            mChangedReleaseNoteNo = Session("mChangedReleaseNoteNo")
            txtChangedReleaseNoteNo.Text = mChangedReleaseNoteNo.ToString

            mChangedReleaseNoteDate = Session("mChangedReleaseNoteDate")
            txtChangeReleaseNoteDate.Text = mChangedReleaseNoteDate

            Session.Remove("mChangedReleaseNoteNo")
            Session.Remove("mChangedReleaseNoteDate")
            'Response.Redirect("wfrptReleaseNoteNo.aspx?BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
    Private Sub RemoveSessionForReleaseNote()
        Session.Remove("mReceiptItemID")
        Session.Remove("mItemID")
        Session.Remove("mReleaseNoteNo")
        Session.Remove("mReleaseNoteDate")
        Session.Remove("mReceiptDate")
        Session.Remove("TempPartNo")
        Session.Remove("SerialNo")
        Session.Remove("ReceiptNo")
    End Sub
    Private Sub BindValueForChangeReleaseNoteNo(ByVal RelNoteNo As String, ByVal RelNoteDate As String)
        ResetValues()
        txtCurrentReleaseNoteNo.Text = RelNoteNo
        If RelNoteDate = "&nbsp;" Then
            txtCurrentReleaseNoteDate.Text = ""
        Else
            txtCurrentReleaseNoteDate.Text = RelNoteDate
        End If
        txtCurrentReleaseNoteDate.ReadOnly = True
        calCurrentReleaseNoteDate_CalendarExtender.Enabled = False

        If txtChangedReleaseNoteNo.Enabled = True Then
            setFocus(txtChangedReleaseNoteNo)
        End If
        upnlChangeReleaseNoteNo.DataBind()
    End Sub
#End Region

#Region "Events"
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If mChangeReleaseNoteNoList.Contains(mReceiptID, txtChangedReleaseNoteNo.Text.Trim) = True Then
            MSGBoxCtrl.show("Alert!", "Release Note No. Already Exist For This Receipt", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If IsValid Then
            mChangedReleaseNoteNo = txtChangedReleaseNoteNo.Text
            Session("mChangedReleaseNoteNo") = mChangedReleaseNoteNo

            mChangedReleaseNoteDate = txtChangeReleaseNoteDate.Text
            Session("mChangedReleaseNoteDate") = mChangedReleaseNoteDate

            If (txtChangeReleaseNoteDate.Text = "" And txtChangedReleaseNoteNo.Text = "") Then
                MSGBoxCtrl.show("Alert!", "Either enter Release Note No. or select Release Note Date", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                If txtCurrentReleaseNoteDate.Text <> "" And txtChangeReleaseNoteDate.Text = "" Then
                    MSGBoxCtrl.show("Alert!", "Release Note Date Not Selected. Do you want to continue?", "", MsgBoxStyle.YesNo, "SaveConfirmation")
                ElseIf txtCurrentReleaseNoteNo.Text <> "" And txtChangedReleaseNoteNo.Text = "" Then
                    MSGBoxCtrl.show("Alert!", "Release Note No. Not Entered. Do you want to continue?", "", MsgBoxStyle.YesNo, "SaveConfirmation")
                Else
                    Save()
                End If
            End If
        End If
    End Sub
    Private Sub btnCloseChangeReleaseNoteNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseChangeReleaseNoteNo.Click
        RemoveSessionForReleaseNote()
        mdlPopUpChangeReleaseNoteNo.Hide()
        pnlReleaseNoteNo.Visible = False
        upnlChangeReleaseNoteNo.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region
    
#End Region

End Class