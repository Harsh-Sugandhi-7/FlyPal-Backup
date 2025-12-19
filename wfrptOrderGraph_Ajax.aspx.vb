Public Class wfrptOrderGraph_Ajax
    Inherits System.Web.UI.Page

#Region " Enum "
    Enum GraphType
        Order = 1
        Invoice = 2
        Status = 3
    End Enum
#End Region

#Region " Variable Declaration "
    Public mItem As Item
    Public mItemList As ItemList
    Dim ToDate As String
    Dim PartNo As String
    Dim Description As String
    Dim mGraphType As Int16
    Dim mItemID As Guid

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mItemList = CType(Session("mItemList"), ItemList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        mItemID = Session("mItemID")
        mGraphType = CType(Session("mGraphType"), Int16)
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("mItemList") = mItemList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemList")
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
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
        ToDate = cmbYear.SelectedItem.Text

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
    Private Sub setHeading()
        If mGraphType = 2 Then
            lbltitle.Text = "Invoice Register"
        ElseIf mGraphType = 3 Then
            lbltitle.Text = "Status Graph"
        ElseIf mGraphType = 4 Then
            lbltitle.Text = "Status Quantity Graph"
        End If
        'lblResult.Text = "List of Part Nos: " & mItemList.Count & " Record(s) found."
    End Sub
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
    Private Sub SetCombo()
        Dim i As Integer
        If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
            For i = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today).Year)
            Next
            cmbYear.SelectedIndex = 10
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mItemID = Guid.Empty
        mGraphType = CType(Request.QueryString("mGraphType"), Int16)
        Session("mGraphType") = mGraphType
        mItemList = ItemList.GetItemList(0, "", "", "", "", "", "", False)
        'dgPartSearch.DataSource = mItemList
        Session("mItemList") = mItemList
        DataBind()
    End Sub
    'Public Sub NewPage(ByVal s As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
    '    dgPartSearch.CurrentPageIndex = e.NewPageIndex
    '    mItemList = ItemList.GetItemList(0, "", "", "", "", "", "", False)
    '    dgPartSearch.DataSource = mItemList
    '    Session("mItemList") = mItemList
    '    dgPartSearch.DataBind()
    'End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then
            'RemoveSession()
            SetCombo()
            If cmbYear.Enabled = True Then
                SetFocus(cmbYear)
            End If
            DataFieldBind()
            'ControlVisibility(6)
        End If

        setHeading()
    End Sub
    'Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
    '    Dim Index As Int16 = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
    '    ClearControls()
    '    ControlVisibility1(Index)
    '    If cmbSearch.Enabled = True Then
    '        SetFocus(cmbSearch)
    '    End If
    'End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        upnlDisplaySearchCriteria.Update()
    End Sub
    'Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
    '    dgPartSearch.CurrentPageIndex = 0
    '    PartNo = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
    '    Description = IIf(cmbSearch.SelectedIndex = 2, Trim(txtSearchFor.Text), "")
    '    Session("PartNo") = PartNo
    '    Session("Description") = Description
    '    SetValues()
    '    FindNow(cmbSearch.SelectedIndex, PartNo, Description)
    '    upnldgPartSearch.Update()
    'End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Private Sub dgPartSearch_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPartSearch.ItemCommand
    '    'Dim Index As Int16 = e.Item.ItemIndex + dgPartSearch.CurrentPageIndex * dgPartSearch.PageSize
    '    Select Case e.CommandName
    '        Case "Select"

    '            Description = mItemList(mItemID).Description
    '            Session("PartNo") = PartNo
    '            Session("Description") = Description
    '            Session("mItemID") = mItemID()
    '            SetValues()
    '    End Select
    '    upnlDisplaySearchCriteria.Update()
    'End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As DataSet
        Dim rpt As Object
        Dim rptSearch As rptLetterHead
        SetValues()
        ' rptSearch = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "For Year : " + cmbYear.SelectedValue)
        'Added by Archana on 12-Aug-09
            rptSearch = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), cmbYear.SelectedValue, PartNo, Description, AppSettings("Logo"))   'Changed By Utkarsh For Report Logo.
        If mGraphType = 2 Then
            ds = New dsInvoice
            rpt = New rptInvoiceRegisterGraph
            myReport = New crptInvoiceGraph
            rpt = rptInvoiceRegisterGraph.GetSales(ToDate, mItemID)
        ElseIf mGraphType = 3 Then
            ds = New dsStatusGraph
            rpt = New rptStatusGraph
            myReport = New crptStatusGraph
            rpt = rptStatusGraph.GetSales(ToDate, mItemID)
        ElseIf mGraphType = 4 Then
            ds = New dsStatusGraph
            rpt = New rptStatusGraphQty
            myReport = New crptStatusGraphQty
            rpt = rptStatusGraphQty.GetSalesQty(ToDate, mItemID)
        End If
        If rpt.Count <= 0 Then
             MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else

            If mGraphType = 2 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 905)
            ElseIf mGraphType = 3 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 906)
            ElseIf mGraphType = 4 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 907)
            End If
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


        If mGraphType = 2 Then
            MarkLog(Util.Action.Print, "InvoiceRegGraph", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) '905  Invoice
        ElseIf mGraphType = 3 Then
            MarkLog(Util.Action.Print, "TranStatusCount", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) '906
        ElseIf mGraphType = 4 Then
            MarkLog(Util.Action.Print, "TranStatusQty", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) '907
        End If

    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class