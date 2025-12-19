'Added by Utkarsh on 12-Feb-2014

Imports System.Linq
Imports System.Collections.Generic
Public Class wfrptBarcodeNo_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mStockItemListForAcceptanceTag As StockItemListForAcceptanceTag
    Public mOpenState As Boolean
    Dim PartNo As String
    Dim Location, SearchIndex, PartType, PartNoLocation As String
    Public mCurrentLocation As String
    Public mReceiptItemID As Guid
    Dim EventLogID As Guid
    Public mPartName As String
    Public mLocation As String
    Public mPartType As String
    Public StoreID As Guid 'Added by Vikrant on 24-Jan-2012 For ALL24012012
    Public mStoreList As StoreList 'Added by Vikrant on 24-Jan-2012 For ALL24012012
    Public Store As String
    Public mPartTypeList As PartTypeList
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mStockItemListForAcceptanceTag = CType(Session("mStockItemListForAcceptanceTag"), StockItemListForAcceptanceTag)
        PartNo = IIf(IsNothing(Session("PartNo")), "", Session("PartNo"))
        Location = IIf(IsNothing(Session("Location")), "", Session("Location"))
        mCurrentLocation = CType(Session("mCurrentLocation"), String)
        mReceiptItemID = CType(Session("mReceiptItemID"), Guid)
        PartType = IIf(IsNothing(Session("PartType")), "", Session("PartType"))
        SearchIndex = IIf(IsNothing(Session("SearchIndex")), "", Session("SearchIndex"))
        PartNoLocation = Session("PartNoLocation")
        mStoreList = CType(Session("mStoreList"), StoreList) 'Added by Vikrant on 24-Jan-2012 For ALL24012012
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mStockItemListForAcceptanceTag") = mStockItemListForAcceptanceTag
        Session("PartNo") = PartNo
        Session("Location") = Location
        Session("mCurrentLocation") = mCurrentLocation
        Session("mReceiptItemID") = mReceiptItemID
        Session("PartType") = PartType
        Session("SearchIndex") = SearchIndex
        Session("PartNoLocation") = PartNoLocation
        Session("mStoreList") = mStoreList 'Added by Vikrant on 24-Jan-2012 For ALL24012012
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Location")
        Session.Remove("PartType")
        Session.Remove("SearchIndex")
        Session.Remove("PartNoLocation")
        Session.Remove("mStoreList") 'Added by Vikrant on 24-Jan-2012 For ALL24012012
        Session.Remove("mStockItemListForAcceptanceTag")
        Session.Remove("mCurrentLocation")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptBarcodeNo_Ajax.aspx" Then
            RemoveSession()
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility1(ByVal SearchIndex As Int32)
        If SearchIndex = 0 Then
            lblFor.Visible = False
            txtSearchFor.Visible = False
            cmbPartType.Visible = False
            cmbPartType.SelectedIndex = 0
            cmbStoreList.Visible = False
            cmbStoreList.SelectedIndex = 0
            lblStoreCount.Visible = False
        ElseIf SearchIndex = 1 Then
            lblFor.Visible = True
            txtSearchFor.Visible = True
            txtSearchFor.Text = PartNo
            cmbPartType.Visible = False
            cmbPartType.SelectedIndex = 0
            cmbStoreList.Visible = False
            cmbStoreList.SelectedIndex = 0
            lblStoreCount.Visible = False
        ElseIf SearchIndex = 2 Then
            lblFor.Visible = True
            txtSearchFor.Visible = True
            txtSearchFor.Text = Location
            cmbPartType.Visible = False
            cmbPartType.SelectedIndex = 0
            cmbStoreList.Visible = False
            cmbStoreList.SelectedIndex = 0
            lblStoreCount.Visible = False
        ElseIf SearchIndex = 3 Then
            lblFor.Visible = False
            txtSearchFor.Visible = False
            cmbPartType.Visible = True
            cmbStoreList.Visible = False
            cmbStoreList.SelectedIndex = 0
            lblStoreCount.Visible = False
            'Added by Vikrant on 24-Jan-2012 For ALL24012012
        ElseIf SearchIndex = 4 Then
            lblFor.Visible = True
            txtSearchFor.Visible = False
            cmbPartType.Visible = False
            cmbStoreList.Visible = True
            cmbStoreList.SelectedIndex = 0
            lblStoreCount.Visible = True
        End If
        upnlSearch.Update()
    End Sub
    Private Sub ClearControls()
        txtSearchFor.Text = ""
    End Sub
    Private Sub ResetValues()
        PartNo = ""
        Location = ""
    End Sub
    ' by Vikrant on 24-Jan-2012 For ALL24012012
    Private Sub FindNow(ByVal LookinType As Integer, Optional ByVal ItemName As String = "", Optional ByVal Location As String = "", Optional ByVal ItemTypeID As Integer = 0, Optional ByVal StoreID As String = "{00000000-0000-0000-0000-000000000000}")
        If LookinType = -1 Then
            LookinType = 0
        End If

        gdPartSearch.DataSource = Nothing
        mStockItemListForAcceptanceTag = Nothing
        mStockItemListForAcceptanceTag = StockItemListForAcceptanceTag.GetStockItemListForAcceptanceTag(PartNo, Location, ItemTypeID, StoreID, IIf(AppSettings("ToAllowPrintTagForOpenReceipt") = "True", True, False))       'StockItemList.GetStockItemList(PartNo, Location, ItemTypeID, Store) ' by Vikrant on 24-Jan-2012 For ALL24012012

        gdPartSearch.DataSource = mStockItemListForAcceptanceTag
        Session("mStockItemListForAcceptanceTag") = mStockItemListForAcceptanceTag

    End Sub
    Public Sub SetControl()
        SearchIndex = Session("SearchIndex")
        PartNo = Session("PartNo")
        Location = Session("Location")
        PartType = Session("PartType")
        If cmbSearch.SelectedIndex = 4 Then
            StoreID = Session("StoreID")
        Else
            StoreID = Guid.Empty
        End If
        FindNow(SearchIndex, PartNo, Location, PartType, StoreID.ToString)
        BindGrid()
        cmbSearch.SelectedIndex = SearchIndex
        cmbPartType.SelectedValue = PartType

        ControlVisibility1(SearchIndex)
        lblResult.Text = "List of Parts : " & mStockItemListForAcceptanceTag.Count & " Record(s) found. "
    End Sub
    'Added by Vikrant on 24-Jan-2012 For ALL24012012
    Private Sub PrintAcceptanceTag()
        Dim chkBox As CheckBox
        For i As Integer = 0 To gdPartSearch.Rows.Count - 1
            chkBox = CType(gdPartSearch.Rows.Item(i).Cells(1).FindControl("chkSelect"), CheckBox)
            Dim mReceiptItemID As New Guid(gdPartSearch.DataKeys(i).Values(0).ToString)
            mStockItemListForAcceptanceTag(mReceiptItemID).IsSelected = chkBox.Checked
        Next
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim obj As rptStoresAcceptanceTag
        Dim letter As rptLetterHead
        Dim ds As New dsStoresAcceptanceTag
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        obj = rptStoresAcceptanceTag.GetStoresAcceptanceTagForAll(mReceiptItemID, True, True, mStockItemListForAcceptanceTag)
		'Replace AppSettings("WORevisionNo") with  mModuleList.Item("Acceptance Tag").FormRevisionNo by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
		'  letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), AppSettings("Barcode"), AppSettings("ClientCode"))
		letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "",
												 AppSettings("WODocumentNo"),
												 mModuleList.Item("Acceptance Tag").FormRevisionNo,
												 AppSettings("Barcode"),
												 AppSettings("ClientCode"),
												 SearchString4:=mModuleList.Item("Acceptance Tag").FormRevisionNo)

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
			'ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "LAMA" Then
			'    myReport = New crptServiceableUnserviceableTagForLama
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
			'Print(obj)
			'Exit Sub
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
        For i As Integer = 0 To mStockItemListForAcceptanceTag.Count - 1
            mStockItemListForAcceptanceTag.Item(i).IsSelected = False
        Next
        Session("mStockItemListForAcceptanceTag") = mStockItemListForAcceptanceTag
        gdPartSearch.DataSource = mStockItemListForAcceptanceTag
        BindGrid()
        MarkLog(Util.Action.Print, "Acceptance Tag", "Part : " + mPartName, Util.ErrorType.NoError, mReceiptItemID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        'End If
        'Next

    End Sub

    Private Sub AddItemsToList()
        Dim chkBox As CheckBox
        For i As Integer = 0 To gdPartSearch.Rows.Count - 1
            chkBox = CType(gdPartSearch.Rows.Item(i).Cells(1).FindControl("chkSelect"), CheckBox)
            Dim mReceiptItemID As New Guid(gdPartSearch.DataKeys(i).Values(0).ToString)
            mStockItemListForAcceptanceTag(mReceiptItemID).IsSelected = chkBox.Checked
        Next
        Session("mStockItemListForAcceptanceTag") = mStockItemListForAcceptanceTag
    End Sub
    Private Sub BindGrid()
        lblResult.Text = "List of Parts : " & mStockItemListForAcceptanceTag.Count & " Record(s) found "
        gdPartSearch.DataBind()
        upnlGrid.Update()
    End Sub
    Private Sub StoreListBind()
        If Not Session("mStoreList") Is Nothing Then
            mStoreList = Session("mStoreList")
        Else
            mStoreList = StoreList.GetStoreList(0, "", True, True)
            Session("mStoreList") = mStoreList
            lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"
        End If
        cmbStoreList.DataSource = mStoreList
        cmbStoreList.DataBind()
        cmbStoreList.SelectedValue = StoreID.ToString
    End Sub
    Private Sub Print(Optional ByVal obj As rptStoresAcceptanceTag = Nothing)
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
            ''myReport = New crptUnserviceableTagForStarAir 'crptQUARANTINETagForStarAir '

            If AppSettings("ClientCode") = "IRM" Then
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
            For i As Integer = 0 To mStockItemListForAcceptanceTag.Count - 1
                mStockItemListForAcceptanceTag.Item(i).IsSelected = False
            Next
            Session("mStockItemListForAcceptanceTag") = mStockItemListForAcceptanceTag
            gdPartSearch.DataSource = mStockItemListForAcceptanceTag
            BindGrid()

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

        If AppSettings("ClientCode") = "IRM" Then 'For IRM if item is Serviceable and Not primary category is tool i.e. 2 then
            mmrptSERVICEABLETag = (From c In obj
                           Where c.PartStatusID = 1 And (c.PrimaryCategoryID <> 2 Or c.StatusEquipment = False)
                           Select c).ToList
        Else
            mmrptSERVICEABLETag = (From c In obj
                           Where c.PartStatusID = 1
                           Select c).ToList
        End If

        If mmrptSERVICEABLETag.Count > 0 Then
            ' myReport = New crptStoreAcceptanceTag1 'crptQUARANTINETagForStarAir
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
            ' myReport = New crptQUARANTINETagForStarAir
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

        If AppSettings("ClientCode") = "IRM" Then
            Dim mmServiceableTagToolsEquipment = Nothing  'For IRM if item is Serviceable and primary category is tool i.e. 2 and marked as calibrated i.e. Status Equipment=1 then
            mmServiceableTagToolsEquipment = (From c In obj
                               Where c.PartStatusID = 1 And c.PrimaryCategoryID = 2 And c.StatusEquipment = True
                               Select c).ToList
            If mmServiceableTagToolsEquipment.Count > 0 Then
                If AppSettings("ClientCode") = "IRM" Then
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
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Added by Vikrant on 24-Jan-2012 For ALL24012012
        'mStoreList = StoreList.GetStoreList(0, "", True)
        'cmbStoreList.DataSource = mStoreList
        'cmbStoreList.DataBind()
        'Session("mStoreList") = mStoreList
        ''-----------------------------------------------
        mPartTypeList = PartTypeList.GetPartTypeList(True)
        cmbPartType.DataSource = mPartTypeList
        cmbPartType.DataBind()

        mStockItemListForAcceptanceTag = StockItemListForAcceptanceTag.GetStockItemListForAcceptanceTag("", "", 0, (Guid.Empty).ToString, IIf(AppSettings("ToAllowPrintTagForOpenReceipt") = "True", True, False))
        gdPartSearch.DataSource = mStockItemListForAcceptanceTag
        Session("mStockItemListForAcceptanceTag") = mStockItemListForAcceptanceTag
        BindGrid()
        SearchIndex = Session("SearchIndex")
        PartNo = Session("PartNo")
        Location = Session("Location")
        PartType = Session("PartType")
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfrptBarcodeNo_Ajax.aspx"
            If cmbSearch.Enabled = True Then
                setFocus(cmbSearch)
            End If
            DataFieldBind()
            'SetControl()
        End If
        If IsPostBack Then
            AddItemsToList()
        End If
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
        ClearControls()
        ControlVisibility1(cmbSearch.SelectedIndex)
        If cmbSearch.Enabled = True Then
            setFocus(cmbSearch)
        End If
        If cmbSearch.SelectedIndex = 4 Then
            StoreListBind()
        End If
    End Sub
    Private Sub btnSearch_Click(sender As Object, e As ImageClickEventArgs) Handles btnSearch.Click

        gdPartSearch.PageIndex = 0
        SearchIndex = cmbSearch.SelectedIndex
        PartNo = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
        Location = IIf(cmbSearch.SelectedIndex = 2, Trim(txtSearchFor.Text), "")
        PartType = cmbPartType.SelectedValue

        If cmbSearch.SelectedIndex = 4 Then
            StoreID = New Guid(Request.Form("cmbStoreList").ToString)
        Else

            StoreID = Guid.Empty
        End If

        Session("SearchIndex") = SearchIndex
        Session("PartNo") = PartNo
        Session("Location") = Location
        Session("PartType") = PartType
        Session("StoreID") = StoreID 'Added by Vikrant on 24-Jan-2012 For ALL24012012

        FindNow(SearchIndex, PartNo, Location, PartType, StoreID.ToString)  'added by Vikrant on 24-Jan-2012 For ALL24012012
        BindGrid()
        ControlVisibility1(SearchIndex)
        If cmbSearch.SelectedIndex = 4 Then
            StoreListBind()
        End If

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "Acceptance Tag", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        mStockItemListForAcceptanceTag = Nothing
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub gdPartSearch_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdPartSearch.PageIndexChanging
        gdPartSearch.PageIndex = e.NewPageIndex
        gdPartSearch.DataSource = mStockItemListForAcceptanceTag
        BindGrid()
    End Sub
    Private Sub dgPartSearch_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdPartSearch.Sorting
        mStockItemListForAcceptanceTag.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mStockItemListForAcceptanceTag") = mStockItemListForAcceptanceTag
        gdPartSearch.DataSource = mStockItemListForAcceptanceTag
        BindGrid()
    End Sub
    Private Sub btnPrintAcceptanceTag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintAcceptanceTag.Click

        Dim count As Integer = 0

        count = (From c As StockItemForAcceptanceTag In mStockItemListForAcceptanceTag Where c.IsSelected
                 Select c).Count
        If count >= 1 And count <= 25 Then
            'AddSelectedItem()
            PrintAcceptanceTag()
        Else
            If count = 0 Then
                MSGBoxCtrl.show("Acceptance Tag", "Please Select At least One Record(Max. Allowed 25)", "", MsgBoxStyle.OkOnly, "")
            ElseIf count > 25 Then
                MSGBoxCtrl.show("Alert", count & " records selected.<br>Can not print more than 25 records.", "", MsgBoxStyle.OkOnly, "")
            End If

        End If
    End Sub
#End Region


End Class