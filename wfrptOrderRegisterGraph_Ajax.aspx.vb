Public Class wfrptOrderRegisterGraph_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
    Dim ToDate As String
    Dim PartNo As String
    Dim Description As String
    Dim mItemID As Guid
    Dim mItemList As ItemList
    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mItemList = Session("mItemList")
        PartNo = Session("PartNo")
        Description = Session("Description")
        mItemID = Session("mItemID")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mItemList")
    End Sub
    Private Sub ControlVisibility()
        If (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
            lblStep2.Text = "Step III. Selection of Part Number/Description"
            lblStep3.Text = "Step IV. Display Report"
        Else
            lblStep2.Text = "Step II. Selection of Part Number/Description"
            lblStep3.Text = "Step III. Display Report"
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok

            End Select
        End If
    End Sub
    'Private Sub ControlVisibility(ByVal Index As Int16)
    '    lblFor.Visible = (cmbSearch.SelectedIndex <> 0)
    '    txtSearchFor.Visible = (cmbSearch.SelectedIndex <> 0)
    'End Sub
    'Private Sub ControlVisibility1(ByVal Index As Int16)
    '    lblFor.Visible = (Index <> 0)
    '    txtSearchFor.Visible = (Index <> 0)
    'End Sub
    'Private Sub ClearControls()
    '    txtSearchFor.Text = ""
    'End Sub
    Private Sub SetValues()
        'ToDate = cmbYear.SelectedItem.Text

        If (txtPartDescription.Text.Trim.IndexOf("[") > 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtPartDescription.Text)
            Description = Trim(txtPartDescription.Text)
        End If
        If PartNo <> "" Then
            mItemID = mItemList(PartNo).ID
        Else
            mItemID = Guid.Empty
        End If

        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")
        lblDispYear.Text = "Year : " & ToDate

        mCompleteSearchingCriteria = lblDispYear.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text

    End Sub

    'Private Sub FindNow(ByVal LookInType As Integer, ByVal ItemName As String, ByVal Description As String)
    '    'dereference the objects
    '    mItemList = ItemList.GetItemList(LookInType, ItemName, Description, "", "", "", "", False)
    '    dgPartSearch.DataSource = mItemList
    '    dgPartSearch.DataBind()
    '    Session("mItemList") = mItemList
    '    lblResult.Text = "List of Part No.s: " & mItemList.Count & " Record(s) found."
    'End Sub

    Private Sub ControlInVisible()
        lblDispYear.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
    End Sub
    Private Sub ControlVisible()
        lblDispYear.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtFromDate" Then

            If DateDiff(DateInterval.Month, CDate(txtFromDate.Text), CDate(txtToDate.Text)) >= 12 Then
                custValidator.ErrorMessage = "Date Difference should not be greater than 12 Months"
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mItemID = Guid.Empty
        mItemList = ItemList.GetItemList(0, "", "", "", "", "", "", False)
        'dgPartSearch.DataSource = mItemList
        Session("mItemList") = mItemList
        DataBind()
    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtFromDate.Text = DateSerial(Year(Today.Date), Month(Today.Date), 1).ToString(AppSettings("DateFormat"))
            txtToDate.Text = CDate(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString(AppSettings("DateFormat"))
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        upnlDisplaySearchCriteria.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim ds As New dsOrder
            Dim rpt As rptOrderRegisterGraph
            Dim rptSearch As rptLetterHead
            SetValues()
            ' rptSearch = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "For Year : " + cmbYear.SelectedValue)
            'Added by Archana on 12-Aug-09
            rptSearch = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), txtFromDate.Text + "," + txtToDate.Text + "," + txtIntOrderNo.Text.Trim, PartNo, Description, AppSettings("Logo"))   'Changed By Utkarsh For Report Logo.
            myReport = New crptOrderGraph
            rpt = rptOrderRegisterGraph.GetSales(txtFromDate.Text, txtToDate.Text, mItemID.ToString, txtIntOrderNo.Text.Trim)

            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 904)
            End If
            ds.Clear()
            '-----------Added by Utkarsh for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            '----------------------------------------------------------
            da.Fill(ds, rpt)
            da.Fill(ds, rptSearch)
            da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            Dim Str As String

            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

            MarkLog(Util.Action.Print, "OrderRegGraph", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) '904  Order
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

   

End Class