Public Class wfrptShowPartNoStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
    Public mItemList As ItemList
    Dim PartNo As String
    Dim Description, Unit, SearchStr3 As String
    Dim ItemID As Guid
    Dim LinkID As Guid
    Public mStockPartStatus As rptStockPartStatus
    Public mOnOrderPartStatus As rptOnOrderPartStatus
    Public mReturnablePartStatus As rptReturnablePartStatus
    Public mTransitPartList As rptTransitPartList
    Public mRequisitionItems As RequisitionItems
    Public mRequisitionItemsNew As RequisitionItemsNew 'Added By Vikrant on 04-July-2012 
    Public mrptNewRequisitionPartList As rptNewRequisitionPartList 'Added By Vikrant on 04-July-2012 
    Public mPartTypeList As PartTypeList  'Added By VIkrant on 10-Sept-2012 For ALL07092012
    Dim mSearchCriteriaForEventLog As String = String.Empty
    Dim EventLogID As Guid
    Dim mTransTypeID As Integer

    Public mRequisitionNew As RequisitionNew
    Dim TransTypeID, ReqTypeID As Integer
    Dim mOpenFrom As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        If Request.QueryString("BackPage") = "wfRequisition_Ajax.aspx" Or Request.QueryString("BackPage") = "wfRequisitionItemSearch_Ajax.aspx" Then 'Added byVikrant FOR ALL06062012
            Session("PartNo") = Session("PartNoStatus")
            Session("Description") = Session("DescriptionStatus")
        End If
        mTransTypeID = Session("TransTypeID")
        mItemList = CType(Session("mItemList"), ItemList)
        PartNo = IIf(IsNothing(Session("PartNo")), "", Session("PartNo"))
        Description = IIf(IsNothing(Session("Description")), "", Session("Description"))
        Unit = IIf(IsNothing(Session("Unit")), "", Session("Unit"))
        'ItemID = Session("ItemID")
        LinkID = Session("LinkID")
        mStockPartStatus = CType(Session("mStockPartStatus"), rptStockPartStatus)
        mOnOrderPartStatus = CType(Session("mOnOrderPartStatus"), rptOnOrderPartStatus)
        mReturnablePartStatus = CType(Session("mReturnablePartStatus"), rptReturnablePartStatus)
        mTransitPartList = CType(Session("mTransitPartList"), rptTransitPartList)
        mRequisitionItems = CType(Session("mRequisitionItems"), RequisitionItems)
        mRequisitionItemsNew = CType(Session("mRequisitionItemsNewForPartNoStatus"), RequisitionItemsNew) 'Added By Vikrant on 04-July-2012 
        mItem = CType(Session("mItemFromPartNoStatus"), Item)
    End Sub
    Private Sub SetSession()
        Session("PartNo") = PartNo
        Session("Description") = Description
        Session("Unit") = Unit
        Session("ItemID") = ItemID
        Session("LinkID") = LinkID
        Session("mStockPartStatus") = mStockPartStatus
        Session("mOnOrderPartStatus") = mOnOrderPartStatus
        Session("mReturnablePartStatus") = mReturnablePartStatus
        Session("mTransitPartList") = mTransitPartList
        Session("mRequisitionItems") = mRequisitionItems
        Session("mRequisitionItemsNewForPartNoStatus") = mRequisitionItemsNew 'Added By Vikrant on 04-July-2012 
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("Nomenclature")
        Session.Remove("Category")
        Session.Remove("Unit")
        Session.Remove("Location")
        Session.Remove("ItemID")
        Session.Remove("LinkID")
        Session.Remove("mStockPartStatus")
        Session.Remove("mOnOrderPartStatus")
        Session.Remove("mReturnablePartStatus")
        Session.Remove("mTransitPartList")
        Session.Remove("mRequisitionItems")
        Session.Remove("mRequisitionItemsNewForPartNoStatus") 'Added By Vikrant on 04-July-2012 
        Session.Remove("PartNoStatus") 'Added byVikrant FOR ALL06062012
        Session.Remove("DescriptionStatus") 'Added byVikrant FOR ALL06062012
        Session.Remove("mItemFromPartNoStatus")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        'Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsPartNoStatus
        Dim mCompanyDetail As New CompanyDetail

        Dim myReport = New crptPartNoStatus

        'GetSession()
        'mStockPartStatus = rptStockPartStatus.GetStockPartStatusList(LinkID)
        'mOnOrderPartStatus = rptOnOrderPartStatus.GetrptOnOrderPartStatusList(LinkID)
        'mReturnablePartStatus = rptReturnablePartStatus.GetrptReturnnablePartStatusList(LinkID)
        'mTransitPartList = rptTransitPartList.GetTransitPartList(LinkID, Today.Date.ToShortDateString)
        'mRequisitionItems = RequisitionItems.GetRequisitionItemsForPartnoStatus(LinkID)
        mrptNewRequisitionPartList = rptNewRequisitionPartList.GetRequisitionPartList(LinkID)  'Added By Vikrant on 04-July-2012 

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, "Part No. Status", txtPartNo.Text, txtDescription.Text, txtUnit.Text, "", "", _
                AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"), SearchStr11:=AppSettings("ClientCode"), SearchStr12:=IIf(chkShowOpenTransactionAlso.Checked, "True", "False"))




        If mStockPartStatus.Count = 0 And mOnOrderPartStatus.Count = 0 And mReturnablePartStatus.Count = 0 And mTransitPartList.Count = 0 And mRequisitionItemsNew.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 714)
        End If

        da.Fill(ds, mStockPartStatus)
        da.Fill(ds, mOnOrderPartStatus)
        da.Fill(ds, mReturnablePartStatus)
        da.Fill(ds, mTransitPartList)
        da.Fill(ds, mrptNewRequisitionPartList) 'Added By Vikrant on 04-July-2012 
        'da.Fill(ds, mRequisitionItems)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)


        With myReport
            If mStockPartStatus.Count <= 0 Then
                .Section3.SectionFormat.EnableSuppress = True
            End If
            If mOnOrderPartStatus.Count <= 0 Then
                .Section9.SectionFormat.EnableSuppress = True
            End If
            If mReturnablePartStatus.Count <= 0 Then
                .Section10.SectionFormat.EnableSuppress = True
            End If
            If mTransitPartList.Count <= 0 Then
                .Section6.SectionFormat.EnableSuppress = True
            End If
            If mrptNewRequisitionPartList.Count <= 0 Then
                .Section7.SectionFormat.EnableSuppress = True
            End If
        End With

        Session("CrystalReport") = myReport

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        mSearchCriteriaForEventLog = "Part Name : " + txtPartNo.Text + "," + " Description : " + txtDescription.Text
        MarkLog(Util.Action.Print, "PartNoStatus", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub ControlInVisible()
        'Added by Prashant 3/09/07
        lblPartNo1.Enabled = True
        lblDescription.Enabled = True
        txtPartNo.Enabled = True
        txtDescription.Enabled = True
        txtPartNo.Text = IIf(PartNo <> "", PartNo, " ")
        txtDescription.Text = IIf(Description <> "", Description, " ")
        txtUnit.Text = IIf(Unit <> "", Unit, "")
        lblInfo.Visible = True
        lblInfo1.Visible = True
        lblInfo2.Visible = True
        lblInfo3.Visible = True
        ''lblInfo.Text = "Stock status of part  " & IIf(PartNo <> "", PartNo, " ") & "  and all its alternate parts "
        ''lblInfo1.Text = "On Order Stock of  " & IIf(PartNo <> "", PartNo, " ") & "  part and all its alternate parts "
        ''lblInfo2.Text = "Returnable Stock of  " & IIf(PartNo <> "", PartNo, " ") & "  part and all its alternate parts "
        ''lblInfo3.Text = "Transit status of part  " & IIf(PartNo <> "", PartNo, " ") & "  and all its alternate parts "
        ''lblNewRequisitionPartStatus.Text = "Requisition status of part  " & IIf(PartNo <> "", PartNo, " ") & "  and all its alternate parts " 'Added By Vikrant on 04-July-2012 For ALL04072012-2
        lblInfo.Text = "Current Stock Status"
        lblInfo1.Text = "On Order Stock Status"
        lblInfo2.Text = "Returnable Stock Status"
        lblInfo3.Text = "Transit Status"
        lblNewRequisitionPartStatus.Text = "Requisition Status"


        'Added By Vikrant on 04-July-2012 
        If AppSettings("NewRequisition") = "True" Then
            lblNewRequisitionPartStatus.Visible = True
            dgNewRequisitionPartStatusList.Visible = True
        Else
            lblRequisitionPartStatus.Visible = True
            dgRequisitionPartStatus.Visible = True
        End If
        'End
        'Added byVikrant FOR ALL06062012
        'mOpenFrom Added By Prashant 5-Mar-2019 
        If Request.QueryString("BackPage") = "wfRequisition_Ajax.aspx" Or Request.QueryString("BackPage") = "wfRequisitionItemSearch_Ajax.aspx" Or mOpenFrom = "FromwfStockCard" Or mOpenFrom = "FromPurchaseOrder" Then 'FromPurchaseOrder Added By Prashant on 19-Feb-2021 Heligo19022021
            'lblNewRequisitionPartStatus.Visible = False
            'dgNewRequisitionPartStatusList.Visible = False
            lblRequisitionPartStatus.Visible = False
            dgRequisitionPartStatus.Visible = False
            If mOpenFrom = "FromwfStockCard" Then
                'btnPrint.Visible = True '' Ajay 06-02-2023
                btnPrint1.Visible = True
            Else
                'btnPrint.Visible = False '' Ajay 06-02-2023
                btnPrint1.Visible = False
            End If
            btnCreateRequisitionTop.Visible = False
            'btnCreateRequisitionBottom.Visible = False  '' Ajay 06-02-2023
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'GetSession()

        dgStockPartStatus.DataSource = mStockPartStatus
        Session("mStockPartStatus") = mStockPartStatus

        If AppSettings("ClientCode") = "BRD" Then
            dgStockPartStatus.Columns(0).HeaderText = "Part No./GSE No."
        End If

        dgOnOrderPartStatus.DataSource = mOnOrderPartStatus
        Session("mOnOrderPartStatus") = mOnOrderPartStatus

        dgReturnablePartStatus.DataSource = mReturnablePartStatus
        Session("mReturnablePartStatus") = mReturnablePartStatus

        dgPartsInTransit.DataSource = mTransitPartList
        Session("mTransitPartList") = mTransitPartList

        dgRequisitionPartStatus.DataSource = mRequisitionItems
        Session("mRequisitionItems") = mRequisitionItems

        'Added By Vikrant on 04-July-2012
        dgNewRequisitionPartStatusList.DataSource = mRequisitionItemsNew
        Session("mRequisitionItemsNewForPartNoStatus") = mRequisitionItemsNew
        'End

        DataBind()

    End Sub
    'Added By VIkrant on 10-Sept-2012 For ALL07092012
    Private Sub SetColorCodeForGridItems()
        mPartTypeList = PartTypeList.GetPartTypeList(False)
        For i As Integer = 0 To dgStockPartStatus.Rows.Count - 1
            Dim lblColor As Label
            Dim ItemTypeID As Integer = Int32.Parse(dgStockPartStatus.Rows(i).Cells(15).Text)
            lblColor = CType(Me.dgStockPartStatus.Rows(i).FindControl("lblColor"), Label)
            lblColor.BackColor = System.Drawing.ColorTranslator.FromHtml("#" & mPartTypeList(ItemTypeID, "").Color)
        Next
        For i As Integer = 0 To dgPartsInTransit.Rows.Count - 1
            Dim lblColor As Label
            Dim ItemTypeID As Integer = Int32.Parse(dgPartsInTransit.Rows(i).Cells(7).Text)
            lblColor = CType(Me.dgPartsInTransit.Rows(i).FindControl("lblColor"), Label)
            lblColor.BackColor = System.Drawing.ColorTranslator.FromHtml("#" & mPartTypeList(ItemTypeID, "").Color)
        Next

    End Sub
    'End
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        mOpenFrom = Request.QueryString("Type")  'Added By Prashant 5-Mar-2019 
        If Not IsPostBack Then
            DataFieldBind()
            SetColorCodeForGridItems()
            ControlInVisible()
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint1.Click  '' Ajay 06-02-2023  btnPrint.Click, 
        If mStockPartStatus.Count = 0 And mOnOrderPartStatus.Count = 0 And mReturnablePartStatus.Count = 0 And mTransitPartList.Count = 0 And mRequisitionItemsNew.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "There is no record Found", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose1.Click '' Ajay 06-02-2023 btnClose.Click,
        RemoveSession() 'Added byVikrant FOR ALL06062012
        If mOpenFrom = "FromPurchaseOrder" Then 'Added By Prashant on 19-Feb-2021 Heligo19022021
            'Do nothing
        Else
            Session("MiddleFrame") = "wfrptPartNoStatus_Ajax.aspx"
        End If
        If Request.QueryString("BackPage") = "wfRequisition_Ajax.aspx" Then 'Added byVikrant FOR ALL06062012
            Session("MiddleFrame") = "wfRequisitionList_Ajax.aspx?TransTypeID=" & mTransTypeID
            Response.Redirect(Request.QueryString("BackPage"))
        ElseIf Request.QueryString("BackPage") = "wfRequisitionItemSearch_Ajax.aspx" Then 'Added By Vikrant On 30-Aug-2016 For ALL30082016
            Session("MiddleFrame") = "wfRequisitionList_Ajax.aspx?TransTypeID=" & mTransTypeID
            Dim URL As Stack = CType(Session("URL"), Stack)
            Response.Redirect(URL.Peek.ToString)
        ElseIf Not mOpenFrom Is Nothing AndAlso (mOpenFrom = "FromwfStockCard" Or mOpenFrom = "FromPurchaseOrder") Then  'Added By Prashant 5-Mar-2019 
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        Else
            Response.Redirect("index.aspx")
        End If
    End Sub
    Private Sub btnCreateRequisitionTop_Click(sender As Object, e As System.EventArgs) Handles btnCreateRequisitionTop.Click '' Ajay 06-02-2023, btnCreateRequisitionBottom.Click
        mRequisitionNew = RequisitionNew.NewRequisition(65)
        mRequisitionNew.ReqDate = Today.Date
        Session("TransTypeID") = 65
        TransTypeID = 65

        ItemID = Session("ItemID")
        Dim mtmpItem As Item = Item.GetItem(ItemID)
        mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, mRequisitionNew.WorkShopID)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID = ItemID
        mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo = txtPartNo.Text.Trim
        mRequisitionNew.RequisitionItemsNew.CurrentItem.Description = txtDescription.Text.Trim
        mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID = mtmpItem.UnitID        'Added By Prashant On 07-May-2019 BA07052019
        mRequisitionNew.RequisitionItemsNew.CurrentItem.Unit = mtmpItem.UnitName        'Added By Prashant On 07-May-2019 BA07052019
        mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase = CType(Session("mItemFromPartNoStatus"), Item).IsOneTimePurchase
        If Not CType(Session("mItemFromPartNoStatus"), Item).IsOneTimePurchase Then
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = CType(Session("mItemFromPartNoStatus"), Item).MinStockLevel
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = CType(Session("mItemFromPartNoStatus"), Item).MaxStockLevel
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = CType(Session("mItemFromPartNoStatus"), Item).MinReOrderLevel
        Else
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = 0
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = 0
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = 0
        End If
        Session("OpenFromPartNoBinCard") = "OpenFromPartNoBinCard"
        Session("mRequisitionNew") = mRequisitionNew

        Session("MiddleFrame") = "wfRequisitionList_Ajax.aspx?TransTypeID=65"

        Dim str As String
        str = "openledgersame('wfRequisition_Ajax.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
#End Region
    'Added By Vikrant On 26-Aug-2019 For BA26082019
    Private Sub chkShowOpenTransactionAlso_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkShowOpenTransactionAlso.CheckedChanged, chkIsValuedStore.CheckedChanged
        'mStockPartStatus = rptStockPartStatus.GetStockPartStatusList(mItem.LinkID, , chkIsValuedStore.Checked, chkShowOpenTransactionAlso.Checked)
        'mOnOrderPartStatus = rptOnOrderPartStatus.GetrptOnOrderPartStatusList(mItem.LinkID, IsOpenTransactionsRequired:=chkShowOpenTransactionAlso.Checked)
        mStockPartStatus = rptStockPartStatus.GetStockPartStatusList(LinkID, , chkIsValuedStore.Checked, chkShowOpenTransactionAlso.Checked)
        mOnOrderPartStatus = rptOnOrderPartStatus.GetrptOnOrderPartStatusList(LinkID, IsOpenTransactionsRequired:=chkShowOpenTransactionAlso.Checked)
        Session("mStockPartStatus") = mStockPartStatus
        Session("mOnOrderPartStatus") = mOnOrderPartStatus
        dgStockPartStatus.DataSource = mStockPartStatus
        dgStockPartStatus.DataBind()

        dgOnOrderPartStatus.DataSource = mOnOrderPartStatus
        dgOnOrderPartStatus.DataBind()

        dgReturnablePartStatus.DataSource = mReturnablePartStatus
        dgReturnablePartStatus.DataBind()

        dgPartsInTransit.DataSource = mTransitPartList
        dgPartsInTransit.DataBind()

        dgRequisitionPartStatus.DataSource = mRequisitionItems
        dgRequisitionPartStatus.DataBind()

        dgNewRequisitionPartStatusList.DataSource = mRequisitionItemsNew
        dgNewRequisitionPartStatusList.DataBind()

        SetColorCodeForGridItems()
        ControlInVisible()
    End Sub

    Private Sub dgStockPartStatus_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgStockPartStatus.RowCommand
        Select Case e.CommandName
            Case "ViewRec"
                Dim mReceiptItemID As Guid
                mReceiptItemID = New Guid(e.CommandArgument.ToString)
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                Dim mFileAttach As FileAttach
                mFileAttach = FileAttach.GetAttachment(mReceiptItemID)

                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub dgStockPartStatus_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgStockPartStatus.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) And chkShowOpenTransactionAlso.Checked Then
            Dim StatusID As Integer = (DataBinder.Eval(e.Row.DataItem, "StatusID"))
            If StatusID = 1 Then
                e.Row.Cells(3).BackColor = Color.Olive
            End If
        End If
    End Sub
    Private Sub dgOnOrderPartStatus_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgOnOrderPartStatus.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) And chkShowOpenTransactionAlso.Checked Then
            Dim StatusID As Integer = (DataBinder.Eval(e.Row.DataItem, "StatusID"))
            If StatusID = 1 Then
                e.Row.Cells(3).BackColor = Color.Olive
            End If
        End If
    End Sub
    'End
End Class