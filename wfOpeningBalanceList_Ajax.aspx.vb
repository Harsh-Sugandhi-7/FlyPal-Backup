Imports System.Collections.Generic
Imports System.Linq

'Created By Utkarsh On 11-Nov-2013

Public Class wfOpeningBalanceList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
	Dim EventLogID As Guid 'Added By Utkarsh
	Dim mModuleList As ModuleList
#End Region

#Region " Business Methods "
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub GetSession()
		mItem = Session("mItem")
		mModuleList = Session("mModuleList")
	End Sub
    Private Sub SetSession()
        Session("mItem") = mItem
    End Sub
    Private Sub NewRecord()
        mItem.ItemApplicables.Add(mItem.ID)
        'mItem.ItemApplicables.CurrentIndex = mItem.ItemApplicables.Count - 1
        mItem.ItemApplicables.CurrentItem.SrNo = mItem.ItemApplicables.Count
        mItem.ItemApplicables.CurrentItem.ModelName = ""
        For i As Integer = 0 To mItem.ItemApplicables.Count - 1
            mItem.ItemApplicables(i).SrNo = i + 1
        Next
        Session("mItem") = mItem
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mItem.OpeningBalances.CurrentIndex = Index
        Session("mItem") = mItem
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mItem = Session("mItem")
                            'Changed By Utkarsh FOR opening Stock On 26-Mar-2013 FOR All26032013
                            If Not mItem.IsNew Then
                                Dim mOpeningBalanceCollection As Collections.Hashtable
                                Dim openingBalanceDetails As String = String.Empty
                                mOpeningBalanceCollection = IIf(Session("mOpeningBalanceCollection") Is Nothing, New Collections.Hashtable, Session("mOpeningBalanceCollection"))
                                openingBalanceDetails = "Part No : " & mItem.Name & ", Description : " & mItem.Description & ", Receipt Date : " & mItem.OpeningBalances.CurrentItem.InvoiceDateFormatted & ", Receipt No. : " & mItem.OpeningBalances.CurrentItem.FullInvoiceNo & _
                                ", Quantity : " & mItem.OpeningBalances.CurrentItem.Qty & ", Release Note No. : " & mItem.OpeningBalances.CurrentItem.ReleaseNoteNo & ", Store : " & mItem.OpeningBalances.CurrentItem.StoreName
                                If mOpeningBalanceCollection.ContainsKey("delete") Then
                                    mOpeningBalanceCollection.Item("delete") = mOpeningBalanceCollection.Item("delete") & Environment.NewLine & openingBalanceDetails
                                Else
                                    mOpeningBalanceCollection.Add("delete", openingBalanceDetails)
                                End If
                                Session("mOpeningBalanceCollection") = mOpeningBalanceCollection
                            End If
                            'End
                            mItem.OpeningBalances.Remove(mItem.OpeningBalances.CurrentItem)
                            BindGrid()
                            ControlVisibility()
                            Session("mItem") = mItem
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.Information, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.Information, "")
                            End If
                            BindGrid()
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        BindGrid()
                    End If
                Case MsgBoxResult.Ok ' And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            '    DataFieldBind()
        End If
    End Sub
    Private Sub BindGrid()
        lblResult.Text = "List of Opening Balances :" + CType(mItem.OpeningBalances.Count, String) + " Record(s)."
        'Added by Archana on 4-Nov-2009

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            gdvOpeningBalanceList.Columns(14).HeaderText = "RNN No."
        Else
            gdvOpeningBalanceList.Columns(14).HeaderText = "Batch No."
        End If
        gdvOpeningBalanceList.DataSource = mItem.OpeningBalances
        gdvOpeningBalanceList.DataBind()
        upnlGrid.Update()
    End Sub
    Private Sub ControlVisibility()
        txtAsOnDate.Enabled = IIf(mItem.OpeningBalances.Count > 0, False, True)
        upnlAsOnDate.Update()
        'If mItem.OpeningBalances.Count > 0 Then
        '    'gdvOpeningBalanceList.Columns(21).Visible = IIf(mItem.OpeningBalances.CurrentItem.IsNew = True, False, True)
        '    Dim lb As LinkButton 'ButtonColumn 
        '    For j As Integer = 0 To gdvOpeningBalanceList.Rows.Count - 1
        '        If mItem.OpeningBalances(j).IsNew = True Then
        '            lb = CType(gdvOpeningBalanceList.Rows(j).Cells(21).FindControl("lnkPrintAcceptanceTag"), LinkButton)
        '            lb.Visible = False
        '        End If
        '    Next
        'End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        BindGrid()
        txtAsOnDate.DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtAsOnDate" Then
            If txtAsOnDate.Text.Trim.Length = 0 Then
                custValidator.ErrorMessage = "Select As On Date."
                e.IsValid = False
                Exit Sub
            End If
            If (IsDate(txtAsOnDate.Text.Trim)) Then
                If CDate(txtAsOnDate.Text.Trim) > Today.Date Then
                    custValidator.ErrorMessage = "As on Date should not be greater than todays date."
                    e.IsValid = False
                End If
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh
        If Not IsPostBack Then
            'setFocus(txtAsOnDate)
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If (Not User.IsInRole("PartNew") And mItem.IsNew) Or (Not User.IsInRole("PartEdit") And Not mItem.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
            Exit Sub
        End If

        If IsValid Then
            mItem.OpeningBalances.Add(mItem.ID, mItem.SerialisedStatus, mItem.UnitID)
            If txtAsOnDate.Text.Trim = String.Empty Then
                mItem.OpeningBalances.CurrentItem.InvoiceDate = System.DBNull.Value
                mItem.AsOnDate = System.DBNull.Value
            Else
                mItem.OpeningBalances.CurrentItem.InvoiceDate = txtAsOnDate.Text.Trim
                mItem.AsOnDate = txtAsOnDate.Text.Trim
            End If

            mItem.OpeningBalances.CurrentItem.Location = mItem.Location

            'New Addition By Yogita on 12-Dec-2007 to solve Bud No:-OBD13
            'If mItem.OpeningBalances.CurrentItem.IsNew = True Then
            '    If mItem.ExpiryMonths > 0 Then
            '        mItem.OpeningBalances.CurrentItem.StartDate = Date.Today
            '        If Not (mItem.OpeningBalances.CurrentItem.StartDate) Is System.DBNull.Value Then
            '            mItem.OpeningBalances.CurrentItem.ExpiryDate = CDate(mItem.OpeningBalances.CurrentItem.StartDate).AddMonths(mItem.ExpiryMonths)
            '        End If
            '    End If
            'End If

            mItem.OpeningBalances.CurrentItem.ExpiryMonth = mItem.ExpiryMonths
            mItem.OpeningBalances.CurrentItem.ExpiryQuarter = mItem.ExpiryQuaters 'Added By Prashant 15/2/208
            ''If mItem.ExpiryMonths > 0 Or mItem.ExpiryQuaters > 0 Then
            ''    mItem.OpeningBalances.CurrentItem.StartDate = Date.Today
            ''End If

            If mItem.SerialisedStatus Then
                'mItem.OpeningBalances.CurrentItem.Qty = 1
                mItem.OpeningBalances.CurrentItem.DisplayQty = 1
            End If
            Session("mItem") = mItem
            Response.Redirect("wfOpeningBalance_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfOpeningBalanceList_Ajax.aspx")
        Else
            upnlValidations.Update()
        End If
    End Sub
    'Added By Prashant 21-June-2009 for grid sorting
    Private Sub gdvOpeningBalanceList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvOpeningBalanceList.Sorting
        mItem.OpeningBalances.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mItem") = mItem
        BindGrid()
    End Sub
    Private Sub gdvOpeningBalanceList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvOpeningBalanceList.RowCommand
        Select Case e.CommandName
            Case "EditRecord"
                Dim Index As Int32 = CInt(e.CommandArgument) + gdvOpeningBalanceList.PageSize * gdvOpeningBalanceList.PageIndex
                If (Not User.IsInRole("PartView") And Not User.IsInRole("PartEdit")) Then
                    SetSession()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
                    BindGrid()
                    Exit Sub
                End If
                Session("EditForExpiryInfo") = "True" 'Added by Vikrant FOR ALL11052012-13
                Session("EditItem") = True
                mItem.OpeningBalances.CurrentIndex = Index
                Session("mItem") = mItem
                '*********************************
                'Added by Saylee on 7-Jun-2011
                Dim tmpItem As Item = mItem.Clone
                Session("tmpItem") = tmpItem
                Session("ItemIndex") = mItem.OpeningBalances.CurrentIndex
                '*********************************
                'Added By Utkarsh FOR opening Stock On 20-Feb-2013 FOR All20022013-3
                If Not mItem.IsNew Then
                    Dim openingBalanceDetails = "Part No : " & mItem.Name & ", Description : " & mItem.Description & ", Receipt Date : " & mItem.OpeningBalances.CurrentItem.InvoiceDateFormatted & ", Receipt No. : " & mItem.OpeningBalances.CurrentItem.FullInvoiceNo & _
                    ", Quantity : " & mItem.OpeningBalances.CurrentItem.Qty & ", Release Note No. : " & mItem.OpeningBalances.CurrentItem.ReleaseNoteNo & ", Store : " & mItem.OpeningBalances.CurrentItem.StoreName
                    MarkLog(Util.Action.Edit, "Opening Stock", openingBalanceDetails, Util.ErrorType.NoError, mItem.ID, EventLogID)
                End If
                'End
                Response.Redirect("wfOpeningBalance_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfOpeningBalanceList_Ajax.aspx")

            Case "DeleteRecord"
                Dim Index As Int32 = CInt(e.CommandArgument) + gdvOpeningBalanceList.PageSize * gdvOpeningBalanceList.PageIndex
                If (Not User.IsInRole("PartNew") And mItem.IsNew) Or (Not User.IsInRole("PartEdit") And Not mItem.IsNew) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
                    BindGrid()
                    Exit Sub
                End If
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                mItem.OpeningBalances.CurrentIndex = Index
                Session("mItem") = mItem
                BindGrid()
            Case "PrintAcceptanceTag"  'Added By Prashant 26-Feb-2021 IND26022021
                Dim Index As Int32
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim rowIndex As Integer = gvr.RowIndex
                Index = rowIndex
                'Dim Index As Int32 = CInt(e.CommandArgument) + gdvOpeningBalanceList.PageSize * gdvOpeningBalanceList.PageIndex
                mItem.OpeningBalances.CurrentIndex = Index
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim obj As rptStoresAcceptanceTag
                Dim letter As rptLetterHead
                Dim ds As New dsStoresAcceptanceTag
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                obj = rptStoresAcceptanceTag.GetStoresAcceptanceTag(mItem.OpeningBalances.CurrentItem.ReceiptID)
				letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"),
														 "", AppSettings("WODocumentNo"),
														 AppSettings("WORevisionNo"),
														 AppSettings("Barcode"),
														 AppSettings("ClientCode"),
														 SearchString4:=mModuleList.Item("Acceptance Tag").FormRevisionNo)

				If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Taj" Or AppSettings("ClientCode") = "HSC" Then
					If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then
						myReport = New crptStoreAcceptanceTag6
					Else
						myReport = New crptStoreAcceptanceTag6WithoutBarcode
					End If
				ElseIf AppSettings("ClientCode") = "CE" Or AppSettings("ClientCode") = "Heligo" Then
					myReport = New crptServiceableUnserviceableTagForCE
				ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
					myReport = New crptStoreAcceptanceTagYATA
				ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Novo" Then
					myReport = New crptStoreAcceptanceTagNOVO
				ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IRM") Then
					If AppSettings("ClientCode") = "IRM" Then
						myReport = New crptStoreAcceptanceTagIRM
					Else
						Print(obj)
						Exit Sub
					End If
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

                da.Fill(ds, obj)
                da.Fill(ds, letter)
                da.Fill(ds, mrptImage)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport
                Dim Str1 As String
                Str1 = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
                BindGrid()
        End Select
    End Sub
    Private Sub Print(Optional ByVal obj As rptStoresAcceptanceTag = Nothing) 'Added By Prashant 26-Feb-2021 IND26022021
        Dim pdfList As New System.Collections.ArrayList
        Dim pageCount As Integer = 0
        Dim PDFNo As Integer = 1
        Dim tmp As Integer
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim letter As rptLetterHead
        Dim ds As New dsStoresAcceptanceTag
        Dim mrptImage As rptImage

        Dim mmrptStoresAcceptanceTag = (From c In obj
                                        Where c.PartStatusID = 2
                                        Select c).ToList
        If mmrptStoresAcceptanceTag.Count > 0 Then
            'myReport = New crptUnserviceableTagForStarAir 'crptQUARANTINETagForStarAir '
            If AppSettings("ClientCode") = "IRMI" Then
                myReport = New crptUnserviceableTagForIRM
            ElseIf AppSettings("ClientCode") = "STR" Then
                myReport = New crptUnserviceableTagForStarAir 'crptQUARANTINETagForStarAir '
            ElseIf AppSettings("ClientCode") = "BAP" Then
                myReport = New crptStoreAcceptanceTagBharatAviation
            End If
            letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), AppSettings("Barcode"), AppSettings("ClientCode"))
            ds.Clear()
            da.Fill(ds, "rptStoresAcceptanceTag", mmrptStoresAcceptanceTag)
            da.Fill(ds, letter)
            mrptImage = rptImage.GetImage(ds)
            da.Fill(ds, "rptImage", mrptImage)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            Dim a As New Random
            tmp = a.Next

            Dim MyFile1 = "C:\Temp\" & "Unserviceable" & tmp & PDFNo.ToString & ".pdf"

            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()

            pdfList.Add(MyFile1)
            PDFNo = PDFNo + 1
        End If

        Dim mmrptSERVICEABLETag = Nothing

        If AppSettings("ClientCode") = "IRMI" Then 'For IRM if item is Serviceable and Not primary category is tool i.e. 2 then
            mmrptSERVICEABLETag = (From c In obj
                                   Where c.PartStatusID = 1 And (c.PrimaryCategoryID <> 2 Or c.StatusEquipment = False)
                                   Select c).ToList
        Else
            mmrptSERVICEABLETag = (From c In obj
                                   Where c.PartStatusID = 1
                                   Select c).ToList
        End If

        If mmrptSERVICEABLETag.Count > 0 Then
            '' myReport = New crptStoreAcceptanceTag1 'crptQUARANTINETagForStarAir
            If AppSettings("ClientCode") = "IRM" Then
                myReport = New crptStoreAcceptanceTagIRM
            ElseIf AppSettings("ClientCode") = "STR" Then
                myReport = New crptStoreAcceptanceTag1  'crptQUARANTINETagForStarAir
            ElseIf AppSettings("ClientCode") = "BAP" Then
                myReport = New crptStoreAcceptanceServiceableTagBharatAviation

            End If
            letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), AppSettings("Barcode"), AppSettings("ClientCode"))
            ds.Clear()
            da.Fill(ds, "rptStoresAcceptanceTag", mmrptSERVICEABLETag)
            da.Fill(ds, letter)
            mrptImage = rptImage.GetImage(ds)
            da.Fill(ds, "rptImage", mrptImage)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            Dim a As New Random

            tmp = a.Next
            Dim MyFile1 = "C:\Temp\" & "Serviceable" & tmp & PDFNo.ToString & ".pdf"

            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()

            pdfList.Add(MyFile1)
            PDFNo = PDFNo + 1
        End If
        Dim mmrptRotableTag = (From c In obj
                               Where c.PartStatusID = 3
                               Select c).ToList
        If mmrptRotableTag.Count > 0 Then
            myReport = New crptRotableTagForStarAir
            letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), AppSettings("Barcode"), AppSettings("ClientCode"))
            ds.Clear()
            da.Fill(ds, "rptStoresAcceptanceTag", mmrptRotableTag)
            da.Fill(ds, letter)
            mrptImage = rptImage.GetImage(ds)
            da.Fill(ds, "rptImage", mrptImage)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            Dim a As New Random

            tmp = a.Next
            Dim MyFile1 = "C:\Temp\" & "RotableTServiceable" & tmp & PDFNo.ToString & ".pdf"

            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()

            pdfList.Add(MyFile1)
            PDFNo = PDFNo + 1
        End If

        Dim mmrptQUARANTINETag = (From c In obj
                                  Where c.PartStatusID = 4
                                  Select c).ToList
        If mmrptQUARANTINETag.Count > 0 Then
            'myReport = New crptQUARANTINETagForStarAir
            If AppSettings("ClientCode") = "IRM" Then
                myReport = New crptQuarantineTagIRM
            ElseIf AppSettings("ClientCode") = "STR" Then
                myReport = New crptQUARANTINETagForStarAir  'crptQUARANTINETagForStarAir
            End If
            letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), AppSettings("Barcode"), AppSettings("ClientCode"))
            ds.Clear()
            da.Fill(ds, "rptStoresAcceptanceTag", mmrptQUARANTINETag)
            da.Fill(ds, letter)
            mrptImage = rptImage.GetImage(ds)
            da.Fill(ds, "rptImage", mrptImage)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            Dim a As New Random

            tmp = a.Next
            Dim MyFile1 = "C:\Temp\" & "QUARANTINETServiceable" & tmp & PDFNo.ToString & ".pdf"

            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()

            pdfList.Add(MyFile1)
            PDFNo = PDFNo + 1
        End If

        Dim mmrptSCRAPTag = (From c In obj
                             Where c.PartStatusID = 5
                             Select c).ToList
        If mmrptSCRAPTag.Count > 0 Then
            myReport = New crptSCRAPTagForStarAir
            letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), AppSettings("Barcode"), AppSettings("ClientCode"))
            ds.Clear()
            da.Fill(ds, "rptStoresAcceptanceTag", mmrptSCRAPTag)
            da.Fill(ds, letter)
            mrptImage = rptImage.GetImage(ds)
            da.Fill(ds, "rptImage", mrptImage)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            Dim a As New Random

            tmp = a.Next
            Dim MyFile1 = "C:\Temp\" & "SCRAPTServiceable" & tmp & PDFNo.ToString & ".pdf"

            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()

            pdfList.Add(MyFile1)
            PDFNo = PDFNo + 1
        End If

        If AppSettings("ClientCode") = "IRMI" Then
            Dim mmServiceableTagToolsEquipment = Nothing  'For IRM if item is Serviceable and primary category is tool i.e. 2 and marked as calibrated i.e. Status Equipment=1 then
            mmServiceableTagToolsEquipment = (From c In obj
                                              Where c.PartStatusID = 1 And c.PrimaryCategoryID = 2 And c.StatusEquipment = True
                                              Select c).ToList
            If mmServiceableTagToolsEquipment.Count > 0 Then
                If AppSettings("ClientCode") = "IRMI" Then
                    myReport = New crptTagServiceableTagToolsEquipment
                End If
                letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), AppSettings("Barcode"), AppSettings("ClientCode"))
                ds.Clear()
                da.Fill(ds, "rptStoresAcceptanceTag", mmServiceableTagToolsEquipment)
                da.Fill(ds, letter)
                mrptImage = rptImage.GetImage(ds)
                da.Fill(ds, "rptImage", mrptImage)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport

                Dim a As New Random

                tmp = a.Next
                Dim MyFile1 = "C:\Temp\" & "Serviceable" & tmp & PDFNo.ToString & ".pdf"

                myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

                Dim myExportOption As CrystalDecisions.Shared.ExportOptions
                Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

                myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
                myDiskOption.DiskFileName = MyFile1
                myExportOption = myReport.ExportOptions
                With myExportOption
                    .DestinationOptions = myDiskOption
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                End With
                myReport.Export()
                myReport.Close()
                myReport.Dispose()
                GC.Collect()

                pdfList.Add(MyFile1)
                PDFNo = PDFNo + 1
            End If
        End If

        Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"

        Dim filesByte As New List(Of Byte())()
        For Each file__1 As String In pdfList 'files
            filesByte.Add(File.ReadAllBytes(file__1))
        Next

        File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))

        Session("CrystalReport") = MergedPath
        Session("PrintReportWithAttachment") = "True"

        Dim Files As String() = Directory.GetFiles("C:\Temp\")
        For Each file__1 As String In Files
            If file__1.ToUpper().Contains("serviceable".ToUpper()) Then
                File.Delete(file__1)
            End If
        Next
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub txtAsOnDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtAsOnDate.TextChanged
        If Not IsDate(txtAsOnDate.Text.Trim) Then
            txtAsOnDate.Text = ""
        End If
    End Sub
    '-----------------------------------------------
#End Region

#Region "Navigation"
    'Private Sub btnPartInformation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPartInformation.Click
    '    Session("mItem") = mItem
    '    'Response.Redirect(Request.QueryString("BackPage") & "?")
    '    Response.Redirect(Request.QueryString("BackPage")) ' & "?")
    'End Sub

    'Private Sub btnAlternatePart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlternatePart.Click
    '    Session("mItem") = mItem
    '    Response.Redirect("wfAlternatePartChild_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
    'End Sub

    'Private Sub btnApplicability_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApplicability.Click
    '    NewRecord()
    '    Session("mItem") = mItem
    '    Response.Redirect("wfApplicableFor_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
    '    'Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
    'End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Commented by Kalpesh 
        'As NewRecord Procedure is adding new blank Apllicable model to Collection
        'But Back button is not calling now 'wfApplicableFor' page.
        'NewRecord()

        Session("mItem") = mItem
        'Response.Redirect(Request.QueryString("BackPage") & "?")
        Response.Redirect(Request.QueryString("BackPage")) ' & "?")
    End Sub
#End Region


End Class