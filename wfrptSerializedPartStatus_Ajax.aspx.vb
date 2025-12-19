Public Class wfrptSerializedPartStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public SerialNo As String
    Public PartNo As String = String.Empty
    Public Description As String = String.Empty
    Public mSerializedPartStatusList As SerializedPartStatusList
    Public LookInType As Integer = 0
    Public PartID As String = ""
    Public ReceiptItemID As Guid
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        LookInType = Session("LookInType")
        PartID = Session("PartID")
        mSerializedPartStatusList = Session("mSerializedPartStatusList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub RemoveSession()
        Session.Remove("LookInType")
        Session.Remove("PartID")
        Session.Remove("mSerializedPartStatusList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetValues()
        SerialNo = txtSearchSerialNo.Text.Trim

        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
    End Sub
    Private Sub AddAttributes()
        txtSearch.Attributes.Add("onblur", "callEvent()")
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
    Sub dgReceipItemList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        ' Only deal with data rows, ignore header rows, footer rows, etc.
        Dim P As Integer
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' If the user is a certain role, then do the following logic; otherwise do not
            P = CType(e.Row.Cells(18).Text, Integer)
            If P <= 0 Then
                ' Find the edit link button
                Dim lb As LinkButton = CType(e.Row.FindControl("LinkButton1"), LinkButton)

                ' Disable the edit link button
                lb.Enabled = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        AddAttributes()
        If Not IsPostBack Then
            RemoveSession()
            If txtPartNo.Enabled = True Then
                SetFocus(txtPartNo)
            End If
        End If
        'SetGrid()
        If txtSearch.Text <> "" Then
            LookInType = 1
            If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
                PartID = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Else
                PartID = Trim(txtSearch.Text)
            End If
        Else
            LookInType = 0
            PartID = ""
        End If
        Session("LookInType") = LookInType
        Session("PartID") = PartID
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetValues()
            mSerializedPartStatusList = SerializedPartStatusList.GetRecepitList(SerialNo, PartNo, Description)
            dgReceipItemList.DataSource = mSerializedPartStatusList
            DataBind()
            'SetGrid()
            Session("mSerializedPartStatusList") = mSerializedPartStatusList
            If mSerializedPartStatusList.Count > 0 Then
                lblResult.Visible = True
                dgReceipItemList.Visible = True
                lblResult.Text = "Receipt Items : " + mSerializedPartStatusList.Count.ToString + " Record(s) found."
                Dim NoOfItems As Integer = mSerializedPartStatusList(mSerializedPartStatusList.Count - 1).Counter
                If NoOfItems = 1 Then
                    lblSinglePartNo.Visible = True
                    txtPartNo.Visible = True
                    txtPartNo.Text = mSerializedPartStatusList(0).PartName
                    lblSingleDescription.Visible = True
                    txtDescription.Visible = True
                    txtDescription.Text = mSerializedPartStatusList(0).PartDescription
                    lblSingleSerialNo.Visible = True
                    txtSerialNo.Visible = True
                    txtSerialNo.Text = SerialNo
                    dgReceipItemList.Columns(3).Visible = False
                    dgReceipItemList.Columns(4).Visible = False
                Else
                    lblSinglePartNo.Visible = False
                    txtPartNo.Visible = False
                    lblSingleDescription.Visible = False
                    txtDescription.Visible = False
                    lblSingleSerialNo.Visible = False
                    txtSerialNo.Visible = False
                    dgReceipItemList.Columns(3).Visible = True
                    dgReceipItemList.Columns(4).Visible = True
                End If
            Else
                lblResult.Visible = False
                dgReceipItemList.Visible = False
                lblSinglePartNo.Visible = False
                txtPartNo.Visible = False
                lblSingleDescription.Visible = False
                txtDescription.Visible = False
                lblSingleSerialNo.Visible = False
                txtSerialNo.Visible = False
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If
        upnlGrid.Update()
        'ResetValues()
    End Sub
    Private Sub dgReceipItemList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgReceipItemList.RowCommand
        Select Case e.CommandName
            Case "ViewTag"
                Dim index As Integer = CInt(e.CommandArgument) + dgReceipItemList.PageIndex * dgReceipItemList.PageSize
                ReceiptItemID = mSerializedPartStatusList(index).ReceiptItemID
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim obj As rptStoresAcceptanceTag
                Dim letter As rptLetterHead
				If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Taj" Or AppSettings("ClientCode") = "HSC" Or AppSettings("ClientCode") = "ASH" Then
					If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then
						myReport = New crptStoreAcceptanceTag6
					Else
						myReport = New crptStoreAcceptanceTag6WithoutBarcode
					End If
				ElseIf AppSettings("ClientCode") = "CE" Or AppSettings("ClientCode") = "Heligo" Then
					'myReport = New crptServiceableUnserviceableTag
					myReport = New crptServiceableUnserviceableTagForCE
				ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
					myReport = New crptStoreAcceptanceTagYATA
				ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Novo" Then
					myReport = New crptStoreAcceptanceTagNOVO
				ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "IND" Then
					myReport = New crptStoreAcceptanceTagIND
				ElseIf AppSettings("ClientCode") = "PTW" Then
					myReport = New crptStoreAcceptanceTagForPattaya
				ElseIf AppSettings("ClientCode") = "7AR" Then
					myReport = New crptStoreAcceptanceTagWithoutBarcodeFor7Air
				Else
                    If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then
                        myReport = New crptStoreAcceptanceTag1
                    Else
                        myReport = New crptStoreAcceptanceTag1WithoutBarcode
                    End If
                End If

                Dim ds As New dsStoresAcceptanceTag
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                obj = rptStoresAcceptanceTag.GetStoresAcceptanceTag(ReceiptItemID, True)

				'Replace AppSettings("WORevisionNo") with  mModuleList.Item("PartNoSerialNoStatus").FormRevisionNo by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
				'letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), , AppSettings("ClientCode"))
				letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"),
														 "", AppSettings("WODocumentNo"),
														 mModuleList.Item("PartNoSerialNoStatus").FormRevisionNo, ,
														 AppSettings("ClientCode"),
														 SearchString4:=mModuleList.Item("Acceptance Tag").FormRevisionNo)

				da.Fill(ds, obj)
                da.Fill(ds, letter)
                da.Fill(ds, mrptImage)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Case "Attach"
                Dim index As Integer = CInt(e.CommandArgument) + dgReceipItemList.PageIndex * dgReceipItemList.PageSize
                ReceiptItemID = mSerializedPartStatusList(index).ReceiptItemID
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim FileSize As Integer = mSerializedPartStatusList(ReceiptItemID).Size
                Dim FileExtension As String = mSerializedPartStatusList(ReceiptItemID).Extension
                Dim ImageFile() As Byte = mSerializedPartStatusList(ReceiptItemID).ImageFile

                If FileSize > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & FileExtension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & FileExtension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add information to the file.
                        fs.Write(ImageFile, 0, ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        Dim Str As String
                        Str = "<script language=Javascript>openFile();</script>"
                        ClientScript.RegisterStartupScript(Me.GetType(), "openFilel", Str)
                    End If
                End If
        End Select
    End Sub
    Private Sub dgReceipItemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgReceipItemList.PageIndexChanging
        dgReceipItemList.PageIndex = e.NewPageIndex
        dgReceipItemList.DataSource = mSerializedPartStatusList
        dgReceipItemList.DataBind()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class