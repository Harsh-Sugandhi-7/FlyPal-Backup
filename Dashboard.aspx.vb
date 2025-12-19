Imports System.Configuration.ConfigurationManager

Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services
Imports InfoSoftGlobal
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Services
Imports System.Text
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Imports System
Imports System.IO
Partial Class Dashboard
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private i As Integer
    Private j As Integer
    'Private objBranchList As BranchList

    Private mAircraftInformationBoardList As Flypal.AircraftInformationBoard.AircraftInformationBoard
    Private mAircraftInformationBoardInnerList As Flypal.AircraftInformationBoard.AircraftInformationBoard

    Public mAircraftUtilizationByHoursCycles As AircraftUtilizationByHoursCycles
    Dim AircraftIds As String
    Dim mPeriodParameter As PeriodParamater

    Private mAlertCount As AlertCount
    Private mAlertList As AlertList  'Added by Saylee on 3-May-2010
    Private mAlert As Alert

    Dim Year As String
    Dim mMachineNameValueList As MachineNameValueList

    Private checkedIds As New List(Of String)()
    Public mUser As User
    Public mUserID As Guid

    Public mrptExpiredItemsCount As rptExpiredItemsCount
    Public mTransactionwisePendingOrders As TransactionwisePendingOrders
    Public mRootCauseCount As RootCauseCount
    Public mPendingPurchaseQuotationItems As PendingPurchaseQuotationItems
    Public mUserFavouritesList As UserFavouritesList
    Public mUserFavouritesListLinq
#End Region

#Region " Enumeration "
    Enum PeriodParamater
        TimeInAir = 1
        Landings = 4
    End Enum
#End Region

#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mAircraftInformationBoardList = CType(Session("mAircraftInformationBoardList"), Flypal.AircraftInformationBoard.AircraftInformationBoard)
    End Sub
    Private Sub SetSession()
        Session("mAircraftInformationBoardList") = mAircraftInformationBoardList
    End Sub
    Private Sub AircraftInfoBoard() 'Added by Saylee on 4-May-2010
        If User.IsInRole("AircraftInformationBoardView") Then ''Added by Saylee on 14-July-2009
            ' pnlDashBoard.Visible = True ''Added by Saylee on 14-July-2009
            ''lblHeader.Visible = True

            '#009dd9


            If Session("mAircraftInformationBoardList") Is Nothing Then
                mAircraftInformationBoardList = AircraftInformationBoard.AircraftInformationBoard.GetAircraftInformationBoardList()
                mAircraftInformationBoardInnerList = AircraftInformationBoard.AircraftInformationBoard.GetAircraftInformationBoardList()

                Session("mAircraftInformationBoardList") = mAircraftInformationBoardList
                Session("mAircraftInformationBoardInnerList") = mAircraftInformationBoardInnerList
            Else
                mAircraftInformationBoardList = Session("mAircraftInformationBoardList")
                mAircraftInformationBoardInnerList = Session("mAircraftInformationBoardInnerList")
            End If
            pnlAircraftInfoBoard.Visible = mAircraftInformationBoardList.Count > 0

            Rows = mAircraftInformationBoardList.Count + mAircraftInformationBoardList.ModelCount
            'Columns = mAircraftInformationBoardList.MaxHeadingCount(Guid.Empty) - 1 '.MaxColumns
            Columns = mAircraftInformationBoardList.GrandTotalColumnCount() - 1 '.MaxColumns

            CreateDynamicTable()
        End If ''End of User rights
    End Sub
    Private Sub CallAlert() 'Added by Saylee on 4-May-2010
        mAlertList = AlertList.GetAlertList()
        If mAlertList.Count > 0 Then 'Checeked condtion before getting count by Prashant on 24-May-2024
            mAlertCount = AlertCount.GetAlertCountList()
            For i As Integer = 0 To mAlertList.Count - 1
                mAlert = Alert.GetChildAlert(mAlertList(i).ID)
                Try
                    mAlert.DateTime = Today.Date.ToString
                    Select Case mAlertList(i).SrNo
                        Case 1
                            mAlert.Count = mAlertCount.OMRCount
                            mAlert.Save()
                        Case 2
                            mAlert.Count = mAlertCount.DueFCICount
                            mAlert.Save()
                        Case 3
                            mAlert.Count = mAlertCount.ExpiredItemsCount
                            mAlert.Save()
                        Case 4
                            mAlert.Count = mAlertCount.ExpiringItemsCount
                            mAlert.Save()
                        Case 5
                            mAlert.Count = mAlertCount.CoreUnitDueCount
                            mAlert.Save()
                    End Select
                Catch ex As Exception
                    Throw ex
                End Try
            Next
            'If mAlertList.Count > 0 Then
            lnkPendingOrder.Text = mAlertList(0).DescCount
            lnkCalibrationDueReport.Text = mAlertList(1).DescCount
            lnkExpiredItems.Text = mAlertList(2).DescCount
            lnkItemsToExpire.Text = mAlertList(3).DescCount
            lnkCoreUnitDue.Text = mAlertList(4).DescCount

            lblPendingOrder.Text = mAlertList(0).DescCount
            lblCalibrationDueReport.Text = mAlertList(1).DescCount
            lblExpiredItems.Text = mAlertList(2).DescCount
            lblItemsToExpire.Text = mAlertList(3).DescCount
            lblCoreUnitDue.Text = mAlertList(4).DescCount
            'End If
        End If
    End Sub
    Private Sub ControlVisibilty()
        ''If User.IsInRole("PendingOrderView") Then lnkPendingOrder.Visible = True

        ''If User.IsInRole("CalibrationDueReportView") Then
        ''    L1.Visible = True
        ''    lnkCalibrationDueReport.Visible = True
        ''End If
        ''If User.IsInRole("ExpiryDateView") Then
        ''    L2.Visible = True
        ''    lnkExpiredItems.Visible = True
        ''End If
        ''If User.IsInRole("ExpiryDateView") Then
        ''    L3.Visible = True
        ''    lnkItemsToExpire.Visible = True
        ''End If

        Dim mReminder As New Reminder
        mReminder = Reminder.GetAutoReminders(User.Identity.Name)
        'Activating Auto Reminder System
        Dim IsReminderStarted As Boolean
        IsReminderStarted = mReminder.StartAutoReminder(Now.DayOfWeek, User)
        If IsReminderStarted Then
            'Activating Links on dashboard
            lnkPendingOrder.Visible = True
            L1.Visible = True
            lnkCalibrationDueReport.Visible = True
            L2.Visible = True
            lnkExpiredItems.Visible = True
            L3.Visible = True
            lnkItemsToExpire.Visible = True
            L4.Visible = True
            lnkCoreUnitDue.Visible = True
        End If
    End Sub
    Private Sub CreateDynamicTable()
        PlaceHolder1.Controls.Clear()

        ' Fetch the number of Rows and Columns for the table using the properties
        Dim tblRows As Integer = Rows
        Dim tblCols As Integer = Columns

        ' Create a Table and set its properties 
        Dim tbl As Table = New Table

        ' Add the table to the placeholder control
        PlaceHolder1.Controls.Add(tbl)
        tbl.Style.Item("width") = "100%"
        ' Now iterate through the table and add your controls 
        For Row As Integer = 0 To tblRows - 1
            Dim tr As TableRow = New TableRow

            For Col As Integer = 0 To tblCols - 1

                Dim tc As TableCell = New TableCell
                tc.BorderWidth = New System.Web.UI.WebControls.Unit(1)
                tc.Width = New System.Web.UI.WebControls.Unit(150)

                Dim txtBox As Label = New Label

                If Col > 4 Then
                    txtBox.Width = New System.Web.UI.WebControls.Unit(130)
                    'txtBox.Height = New System.Web.ui.WebControls.Unit(50)
                Else
                    If Col = 0 Then
                        txtBox.Width = New System.Web.UI.WebControls.Unit(70)
                        'txtBox.Height = tc.Height
                    ElseIf Col = 1 Then
                        txtBox.Width = New System.Web.UI.WebControls.Unit(90)
                    ElseIf Col = 2 Then
                        txtBox.Width = New System.Web.UI.WebControls.Unit(75)
                        'txtBox.Height = tc.Height
                        '*****************
                        'ElseIf Col = 1 Then
                        '    txtBox.Width = New System.Web.UI.WebControls.Unit(70)
                        '    'txtBox.Height = tc.Height
                        'ElseIf Col = 2 Then
                        '    txtBox.Width = New System.Web.UI.WebControls.Unit(90)
                        '    'txtBox.Height = tc.Height
                        'ElseIf Col = 3 Then
                        '    txtBox.Width = New System.Web.UI.WebControls.Unit(70)
                        '    'txtBox.Height = tc.Height
                        'ElseIf Col = 4 Then
                        '    txtBox.Width = New System.Web.UI.WebControls.Unit(70)
                        '    'txtBox.Height = tc.Height
                        'ElseIf Col = 5 Then
                        '    txtBox.Width = New System.Web.UI.WebControls.Unit(70)
                        '    'txtBox.Height = tc.Height
                        '*******************
                    ElseIf Col = 3 Then
                        txtBox.Width = New System.Web.UI.WebControls.Unit(75)
                        'txtBox.Height = tc.Height
                    ElseIf Col = 4 Then
                        txtBox.Width = New System.Web.UI.WebControls.Unit(70)
                        'txtBox.Height = tc.Height
                    End If
                End If

                txtBox.BorderStyle = BorderStyle.None
                txtBox.BorderColor = System.Drawing.Color.White
                txtBox.BorderWidth = New System.Web.UI.WebControls.Unit(0)

                ' Add the control to the TableCell
                tc.Controls.Add(txtBox)
                ' Add the TableCell to the TableRow
                tr.Cells.Add(tc)
            Next Col
            ' Add the TableRow to the Table
            tbl.Rows.Add(tr)
        Next Row

        Dim tmpModelID, tmpMachineID As Guid
        Dim j As Integer = 1
        Dim RowNo As Integer = 0
        Dim ModelHeadingRow As Integer = 0

        Dim mAircraftInformationBoardInfo As AircraftInformationBoard.AircraftInformationBoard.AircraftInformationBoardInfo
		For Each mAircraftInformationBoardInfo In mAircraftInformationBoardList
			If RowNo = 0 AndAlso ModelHeadingRow = 0 Then
				ModelHeadingRow = RowNo
			End If
			If (Not tmpModelID.Equals(mAircraftInformationBoardInfo.ModelID)) AndAlso (Not tmpModelID.Equals(Guid.Empty)) Then
				j = 1
				ModelHeadingRow = RowNo + 1
				RowNo += 1
			End If
			'If (Not tmpModelID.Equals(mAircraftInformationBoardInfo.ModelID)) And (Not tmpModelID.Equals(Guid.Empty)) Then
			'    j = 1
			'    ModelHeadingRow = RowNo + 1
			'    RowNo = RowNo + 1
			'End If
			tmpModelID = mAircraftInformationBoardInfo.ModelID
			tmpMachineID = mAircraftInformationBoardInfo.MachineID

			If j = 1 Then 'Or (Not tmpMachineID.Equals(mAircraftInformationBoardInfo.MachineID)) Then

				Dim k As Integer = 5

				Dim isFound As Boolean = False
				Dim tmpAircraftInformationBoardInfo As AircraftInformationBoard.AircraftInformationBoard.AircraftInformationBoardInfo
				For Each tmpAircraftInformationBoardInfo In mAircraftInformationBoardInnerList
					If tmpAircraftInformationBoardInfo.ModelID.Equals(tmpModelID) AndAlso tmpAircraftInformationBoardInfo.AssemblyType.Contains("AF") Then

						isFound = True

						'0 
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(0).Controls(0), System.Web.UI.WebControls.Label).Text = "Reg. No. <BR> & <BR>Last Flown"
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(0), System.Web.UI.WebControls.TableCell).CssClass = "clsdgHeaderInfoForDashBoard"
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(0).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgHeaderInfoForDashBoard"

						'1
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(1).Controls(0), System.Web.UI.WebControls.Label).Text = "Assembly <BR> Info"
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(1), System.Web.UI.WebControls.TableCell).CssClass = "clsdgHeaderInfoForDashBoard"
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(1).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgHeaderInfoForDashBoard"

						'2
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(2).Controls(0), System.Web.UI.WebControls.Label).Text = "Current <BR> Values"
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(2), System.Web.UI.WebControls.TableCell).CssClass = "clsdgHeaderInfoForDashBoard"
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(2).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgHeaderInfoForDashBoard"
						''2
						'CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(2).Controls(0), System.Web.UI.WebControls.Label).Text = "Model"
						'CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(2), System.Web.UI.WebControls.TableCell).CssClass = "clsdgHeaderInfoForDashBoard"
						'CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(2).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgHeaderInfoForDashBoard"
						''3
						'CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(3).Controls(0), System.Web.UI.WebControls.Label).Text = "Serial No."
						'CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(3), System.Web.UI.WebControls.TableCell).CssClass = "clsdgHeaderInfoForDashBoard"
						'CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(3).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgHeaderInfoForDashBoard"
						''4
						'CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(4).Controls(0), System.Web.UI.WebControls.Label).Text = "Hours"
						'CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(4), System.Web.UI.WebControls.TableCell).CssClass = "clsdgHeaderInfoForDashBoard"
						'CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(4).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgHeaderInfoForDashBoard"
						''5
						'CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(5).Controls(0), System.Web.UI.WebControls.Label).Text = "Cycles"
						'CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(5), System.Web.UI.WebControls.TableCell).CssClass = "clsdgHeaderInfoForDashBoard"
						'CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(5).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgHeaderInfoForDashBoard"
						'6
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(3).Controls(0), System.Web.UI.WebControls.Label).Text = "Next Removal"
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(3), System.Web.UI.WebControls.TableCell).CssClass = "clsdgHeaderInfoForDashBoard"
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(3).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgHeaderInfoForDashBoard"
						'7
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(4).Controls(0), System.Web.UI.WebControls.Label).Text = "TIME SINCE OVERHAUL TSO"
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(4), System.Web.UI.WebControls.TableCell).CssClass = "clsdgHeaderInfoForDashBoard"
						CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(4).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgHeaderInfoForDashBoard"

						For c As Integer = 5 To Columns 'tmpAircraftInformationBoardInfo.TotalColumnCount
							Try
								Dim heading As String = CallByName(tmpAircraftInformationBoardInfo, "ColumnHeader" + (c - 4).ToString, CallType.Get)
								With CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(k).Controls(0), System.Web.UI.WebControls.Label)

									If Not HeadingAdded(heading, k, c, tbl.Rows.Item(ModelHeadingRow), tmpAircraftInformationBoardInfo) Then
										If Not CallByName(tmpAircraftInformationBoardInfo, "ColumnHeader" + (c - 4).ToString, CallType.Get) Is Nothing Then
											.Text = CallByName(tmpAircraftInformationBoardInfo, "ColumnHeader" + (c - 4).ToString, CallType.Get)

											'*****Added by Saylee on 21st-July-2009
											.CssClass = "clsdgHeaderInfoForDashBoard"
											.CssClass = "clsdgHeaderInfoForDashBoard"

											CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(k), System.Web.UI.WebControls.TableCell).CssClass = "clsdgHeaderInfoForDashBoard"
											CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(k).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgHeaderInfoForDashBoard"
											'*****End of Addition by Saylee

											k = k + 1
										End If
									End If

									'.BackColor = System.Drawing.Color.FromArgb(0, 157, 217)
									'.CssClass = "clsdgHeaderInfoForDashBoard"
								End With
								CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(c), System.Web.UI.WebControls.TableCell).CssClass = "clsdgHeaderInfoForDashBoard"
								CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(c).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgHeaderInfoForDashBoard"

							Catch ex As Exception
								'
							End Try
						Next
					Else
						'If isFound = True Then Exit For
					End If
				Next
			End If

			If Not mAircraftInformationBoardInfo.ToString Is Nothing Then

				Dim LastFlownDate As String = ""
				Dim mMaxLogOfAircraft As MaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mAircraftInformationBoardInfo.MachineID)

				If Not mMaxLogOfAircraft Is Nothing Then
					LastFlownDate = mMaxLogOfAircraft.LogDateFormatted.ToString 'Last Flight Log Date
				Else
					LastFlownDate = ""
				End If

				CType(tbl.Rows.Item(RowNo + 1).Controls.Item(0).Controls(0), System.Web.UI.WebControls.Label).Text = mAircraftInformationBoardInfo.RegNo + IIf(mAircraftInformationBoardInfo.RegNo = "", "", "<BR><BR>" + LastFlownDate)

				CType(tbl.Rows.Item(RowNo + 1).Controls.Item(0).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgItemInfoForDashBoard"
				CType(tbl.Rows.Item(RowNo + 1).Controls.Item(1).Controls(0), System.Web.UI.WebControls.Label).Text = mAircraftInformationBoardInfo.AssemblyType + " : " + mAircraftInformationBoardInfo.ModelName + "<BR>" + mAircraftInformationBoardInfo.SerialNo
				CType(tbl.Rows.Item(RowNo + 1).Controls.Item(1).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgItemInfoForDashBoard"
				CType(tbl.Rows.Item(RowNo + 1).Controls.Item(2).Controls(0), System.Web.UI.WebControls.Label).Text = IIf(mAircraftInformationBoardInfo.TotalHoursSinceNew <> "", mAircraftInformationBoardInfo.TotalHoursSinceNew + " H", "") + IIf(mAircraftInformationBoardInfo.TotalCyclesSinceNew <> "", "<BR>" + mAircraftInformationBoardInfo.TotalCyclesSinceNew + " C", "") + IIf(mAircraftInformationBoardInfo.TotalALLSinceNew <> "", mAircraftInformationBoardInfo.TotalALLSinceNew, "")
				CType(tbl.Rows.Item(RowNo + 1).Controls.Item(2).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgItemInfoForDashBoard"
				'CType(tbl.Rows.Item(RowNo + 1).Controls.Item(2).Controls(0), System.Web.UI.WebControls.Label).Text = mAircraftInformationBoardInfo.ModelName
				'CType(tbl.Rows.Item(RowNo + 1).Controls.Item(2).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgItemInfoForDashBoard"
				'CType(tbl.Rows.Item(RowNo + 1).Controls.Item(3).Controls(0), System.Web.UI.WebControls.Label).Text = mAircraftInformationBoardInfo.SerialNo
				'CType(tbl.Rows.Item(RowNo + 1).Controls.Item(3).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgItemInfoForDashBoard"
				'CType(tbl.Rows.Item(RowNo + 1).Controls.Item(4).Controls(0), System.Web.UI.WebControls.Label).Text = mAircraftInformationBoardInfo.TotalHoursSinceNew
				'CType(tbl.Rows.Item(RowNo + 1).Controls.Item(4).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgItemInfoForDashBoard"
				'CType(tbl.Rows.Item(RowNo + 1).Controls.Item(5).Controls(0), System.Web.UI.WebControls.Label).Text = mAircraftInformationBoardInfo.TotalCyclesSinceNew
				'CType(tbl.Rows.Item(RowNo + 1).Controls.Item(5).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgItemInfoForDashBoard"
				CType(tbl.Rows.Item(RowNo + 1).Controls.Item(3).Controls(0), System.Web.UI.WebControls.Label).Text = mAircraftInformationBoardInfo.NextRemoval
				CType(tbl.Rows.Item(RowNo + 1).Controls.Item(3).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgItemInfoForDashBoard"
				CType(tbl.Rows.Item(RowNo + 1).Controls.Item(4).Controls(0), System.Web.UI.WebControls.Label).Text = mAircraftInformationBoardInfo.TimeSinceOverHaul
				CType(tbl.Rows.Item(RowNo + 1).Controls.Item(4).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgItemInfoForDashBoard"

				'For c As Integer = 8 To 15
				'    Try
				'        With CType(tbl.Rows.Item(RowNo + 1).Controls.Item(c).Controls(0), System.Web.UI.WebControls.Label)
				'            .Text = CallByName(mAircraftInformationBoardInfo, "Column" + (c - 7).ToString, CallType.Get)
				'            .CssClass = "clsdgItemInfoForDashBoard"
				'        End With
				'    Catch ex As Exception
				'        Exit For
				'    End Try
				'Next

				For c As Integer = 5 To mAircraftInformationBoardInfo.TotalColumnCount
					Try
						With CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(c).Controls(0), System.Web.UI.WebControls.Label)
							Dim heading As String = CallByName(mAircraftInformationBoardInfo, "ColumnHeader" + (c - 4).ToString, CallType.Get)
							Dim Value As String = CallByName(mAircraftInformationBoardInfo, "Column" + (c - 4).ToString, CallType.Get)
							Dim HeadingIdex As Integer = HeadingIndex(heading, tbl.Rows.Item(ModelHeadingRow), mAircraftInformationBoardInfo.TotalColumnCount)

							If Not Value Is Nothing And HeadingIdex > 0 Then
								CType(tbl.Rows.Item(RowNo + 1).Controls.Item(HeadingIdex).Controls(0), System.Web.UI.WebControls.Label).Text = Value
								CType(tbl.Rows.Item(RowNo + 1).Controls.Item(HeadingIdex).Controls(0), System.Web.UI.WebControls.Label).CssClass = "clsdgItemInfoForDashBoard"
							End If

							'.CssClass = "clsdgItemInfoForDashBoard"
						End With

						'CType(tbl.Rows.Item(ModelHeadingRow).Controls.Item(c), System.Web.UI.WebControls.TableCell).BackColor = System.Drawing.Color.FromArgb(0, 157, 217)
					Catch ex As Exception
						'
					End Try
				Next

			End If

			RowNo = RowNo + 1
			j = j + 1
		Next

		ViewState("dynamictable") = True
    End Sub
    Private Function HeadingAdded(ByVal Heading As String, ByVal ForColumnNo As Integer, ByVal Iteration As Integer, ByVal IntheRow As TableRow, ByVal tmpAircraftInformationBoardInfo As AircraftInformationBoard.AircraftInformationBoard.AircraftInformationBoardInfo) As Boolean
        For i As Integer = 5 To ForColumnNo
            With CType(IntheRow.Controls.Item(i).Controls(0), System.Web.UI.WebControls.Label)
                If .Text = Heading Then
                    Return True
                End If
            End With
        Next

        Return False
    End Function
    Private Function HeadingIndex(ByVal Heading As String, ByVal ModelHeadingRow As TableRow, ByVal TotalColumnCount As Integer) As Integer
        For i As Integer = 5 To TotalColumnCount
            With CType(ModelHeadingRow.Controls.Item(i).Controls(0), System.Web.UI.WebControls.Label)
                If .Text = Heading Then
                    Return i
                End If
            End With
        Next

        Return 0
    End Function
    Private Sub ControlVisibilty1()
        If AppSettings("ShowDashBoard") = "True" Then
            If User.IsInRole("MaintDashBoardView") Then
                SetGraphs()
                SetPieGraph()
                SetLineGraph()
                GetLastLogDet()
                SetMELPirepsGraphs()
                SetRootCauseCount()
                GetOpenWODet() 'Added by Saylee on 28-Apr-2020, LockDown Period
                SetAircraftUtilizationGraph()
                'First set to False
                phMEL.Visible = False
                phPie.Visible = False
                phFlyingLine.Visible = False
                phCurrentStatus.Visible = False
                phLogDet.Visible = False
                phMELPirepsChart.Visible = False
                phRootCauseCount.Visible = False
                phWOLIst.Visible = False
                phAuditDetails.Visible = False
                phAircraftUtilizationGraph.Visible = False
                ' then set to true as per rights
                If User.IsInRole("LogDefectPirepsCountView") Then phMEL.Visible = True
                If User.IsInRole("PieChartTotalHrsView") Then phPie.Visible = True
                If User.IsInRole("LineGraphFlyingValuesView") Then phFlyingLine.Visible = True
                If User.IsInRole("AircraftCurrentStatusListView") Then phCurrentStatus.Visible = True
                If User.IsInRole("LastLogDetGraphValuesView") Then
                    phLogDet.Visible = True
                    phJQgrid.Visible = Session("LineLast10Logs") = False
                End If

                If User.IsInRole("PierpsMELMaintDefectCountView") Then phMELPirepsChart.Visible = True
                If User.IsInRole("RootCauseCountView") Then phRootCauseCount.Visible = True
                If User.IsInRole("OpenWOListView") Then phWOLIst.Visible = True
                If User.IsInRole("AuditStatusRegisterReportView") Then phAuditDetails.Visible = True

                'Added by Harsh on 25th Jan 2024 For TataSteel Dashboard
                If User.IsInRole("PreFlightAuthorizationView") Then
                    PreFlightAuthorizationDetails()
                    phPreFlightAuthorization.Visible = True
                End If

                If User.IsInRole("AMECertificationView") Then
                    AMECertificationDetails()
                    phAMECertification.Visible = True
                End If

                'Added by Sachin on 25th Jan 2024 For TataSteel Dashboard
                If User.IsInRole("AircraftUtilizationGraphOnDashboardView") Then phAircraftUtilizationGraph.Visible = True

                If User.IsInRole("AircraftCertificateDashBoardView") Then
                    AircraftCertificateDetails()
                    phAircraftCertificate.Visible = True
                End If

            Else
                SetRootCauseCount()
                phMEL.Visible = False
                phPie.Visible = False
                phFlyingLine.Visible = False
                phCurrentStatus.Visible = False
                phLogDet.Visible = False
                phMELPirepsChart.Visible = False
                phRootCauseCount.Visible = False
                phWOLIst.Visible = False
                phAuditDetails.Visible = False
                'Added by Harsh on 25th Jan 2024 For TataSteel Dashboard
                phPreFlightAuthorization.Visible = False
                phAMECertification.Visible = False
                phAircraftUtilizationGraph.Visible = False
            End If

            If User.IsInRole("InvDashBoardView") Then
                If User.IsInRole("OrdersPendingforReceiptsView") Then
                    SetTransactionwisePendingOrders()
                    phTransactionwisePendingOrders.Visible = True
                    phPendingPurchaseOrders.Visible = True
                    phPendingOrders.Visible = True
                End If
                If User.IsInRole("NoofshelflifeinventorydueapproachingdueView") Then
                    BarChart()
                    phExpiredItems.Visible = True
                    phExpiryDateReport.Visible = True
                End If

                If User.IsInRole("PendingPurchaseQuotationItemView") Then
                    PendingPurchaseQuotationItemsDetails()
                    phPendingPurchaseQuotationItems.Visible = True
                End If
                If User.IsInRole("RequisitionPendingForPurchaseOrderView") Then
                    RequisitionPendingForPurchaseOrderDetails()
                    phRequisitionPendingForPurchaseOrder.Visible = True
                End If
                If User.IsInRole("AircraftConsumptionGraphView") Then
                    SetAircraftConsumptionGraph()
                    phAircraftConsumption.Visible = True
                End If
                If User.IsInRole("CalibrationDueDashBoardReportView") Then
                    CalibrationDueReportDetails()
                    phCalibrationDue.Visible = True
                End If
                If User.IsInRole("MinLevelItemDashboardReportView") Then
                    MinLevelItemReportDetails()
                    phMinLevelItemReport.Visible = True
                End If
                'Added By Prashant  22-May-2020 ALL22052020
                If User.IsInRole("PendingToReceiptsFromOtherStoreView") Then
                    PendingToReceiptsFromOtherStoreDetails()
                    phPendingToReceiptsFromOtherStore.Visible = True
                End If
                If User.IsInRole("PendingToolsToReceiveFromEmployeeView") Then
                    PendingToolsToReceiveFromEmployeeDetails()
                    phPendingToolsToReceiveFromEmployee.Visible = True
                End If
                If User.IsInRole("ReceivedUnserviceablePartView") Then
                    ReceivedUnserviceablePartDetails()
                    phReceivedUnserviceablePart.Visible = True
                End If
                If User.IsInRole("ReceivedFromAircraftAsCoreUnitReturnView") Then
                    ReceivedFromAircraftAsCoreUnitReturnDetails()
                    phReceivedFromAircraftAsCoreUnitReturn.Visible = True
                End If
                If User.IsInRole("LoanTakenButNotReturnView") Then  'Loan taken but not return
                    LoanInWardRecordsDetails()
                    phLoanInWardRecord.Visible = True
                End If
                'End of Added By Prashant  22-May-2020 ALL2205202
                'Added By Vikrant On 03-Jun-2020  22-May-2020 ALL2205202
                If User.IsInRole("ReOrderLevelItemDashboardReportView") Then
                    ReOrderLevelItemReportDetails()
                    phReOrderLevelItemReport.Visible = True
                End If
                If User.IsInRole("PendingRetExchRepIssueToVendorDashboardReportView") Then
                    PendingReturnableExchangeRepairIssueToVendorItemReportDetails()
                    phPendingReturnableExchangeRepairIssueToVendorItemReport.Visible = True
                End If
                If User.IsInRole("LoanGivenButNotReceivedBackView") Then
                    LoanOutWardReportDetails()
                    phLoanOutWardReport.Visible = True
                End If

                'End
            Else
                phTransactionwisePendingOrders.Visible = False
                phExpiredItems.Visible = False
                phExpiryDateReport.Visible = False
                phCalibrationDue.Visible = False
                phMinLevelItemReport.Visible = False
                phPendingPurchaseOrders.Visible = False
                phPendingPurchaseQuotationItems.Visible = False
                phRequisitionPendingForPurchaseOrder.Visible = False
                phAircraftConsumption.Visible = False
                phPendingOrders.Visible = False
                phPendingToReceiptsFromOtherStore.Visible = False
                phPendingToolsToReceiveFromEmployee.Visible = False
                phReceivedUnserviceablePart.Visible = False
                phReceivedFromAircraftAsCoreUnitReturn.Visible = False
                phLoanInWardRecord.Visible = False
                'Added By Vikrant On 03-Jun-2020
                phReOrderLevelItemReport.Visible = False
                phPendingReturnableExchangeRepairIssueToVendorItemReport.Visible = False
                phLoanOutWardReport.Visible = False
                phAircraftCertificate.Visible = False
                'End
            End If
            cmbYear.Visible = True
            lblYear.Visible = True
            cmbMonth.Visible = True
        Else
            phMEL.Visible = False
            phPie.Visible = False
            phFlyingLine.Visible = False
            phCurrentStatus.Visible = False
            phLogDet.Visible = False
            phMELPirepsChart.Visible = False
            phAircraftConsumption.Visible = False
            phTransactionwisePendingOrders.Visible = False
            phExpiredItems.Visible = False
            phExpiryDateReport.Visible = False
            phRootCauseCount.Visible = False
            phCalibrationDue.Visible = False
            phMinLevelItemReport.Visible = False
            phPendingPurchaseOrders.Visible = False
            phPendingPurchaseQuotationItems.Visible = False
            phRequisitionPendingForPurchaseOrder.Visible = False
            phAircraftConsumption.Visible = False
            phPendingOrders.Visible = False
            phPendingToReceiptsFromOtherStore.Visible = False
            phPendingToolsToReceiveFromEmployee.Visible = False
            phReceivedUnserviceablePart.Visible = False
            phReceivedFromAircraftAsCoreUnitReturn.Visible = False
            phLoanInWardRecord.Visible = False
            'Added By Vikrant On 03-Jun-2020
            phReOrderLevelItemReport.Visible = False
            phPendingReturnableExchangeRepairIssueToVendorItemReport.Visible = False
            phLoanOutWardReport.Visible = False
            phAircraftCertificate.Visible = False
            'End
            'Added by Harsh on 25th Jan 2024 For TataSteel Dashboard
            phPreFlightAuthorization.Visible = False
            phAMECertification.Visible = False
            'End
            cmbYear.Visible = False
            lblYear.Visible = False
            cmbMonth.Visible = False
        End If

        If phMEL.Visible = True Or phPie.Visible = True Or phFlyingLine.Visible = True Or phMELPirepsChart.Visible = True Then
            cmbYear.Visible = True
            lblYear.Visible = True
            cmbMonth.Visible = True
        Else
            cmbYear.Visible = False
            lblYear.Visible = False
            cmbMonth.Visible = False
        End If

        If Session("LineLast10Logs") = False Or Session("LineLast10Logs") Is Nothing Then phLogDetLine.Visible = False

        CallUpdatePanels()

    End Sub
    Private Sub CallUpdatePanels()
        pnlReports.Update()
        upnlMyChart.Update()
        upnlMyPieChart.Update()
        upnlLineGraph.Update()

        upnlYear.Update()
        upnlJQGridLogDet.Update()
        upnlLogDetLineGraph.Update()
        upnlCurrentStatus.Update()
        upnlMELPirepsChart.Update()
        upnlTransactionwisePendingOrders.Update()
        upnlAuditDetails.Update()
        upnlWOList.Update()
    End Sub
#End Region

#Region " Business Properties "
    Protected Property Rows() As Integer
        Get
            If Not ViewState("Rows") Is Nothing Then
                Return CInt(Fix(ViewState("Rows")))
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("Rows") = value
        End Set
    End Property
    ' Columns property to hold the Columns in the ViewState
    Protected Property Columns() As Integer
        Get
            If Not ViewState("Columns") Is Nothing Then
                Return CInt(Fix(ViewState("Columns")))
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("Columns") = value
        End Set
    End Property
    Private Sub GetSession1()
        mUserID = CType(Session("UserId"), Guid)
        mUser = Session("mDashBoardUser")
        mrptExpiredItemsCount = Session("mrptExpiredItemsCount")
        mTransactionwisePendingOrders = Session("mTransactionwisePendingOrders")
        mRootCauseCount = Session("mRootCauseCount")
        mAircraftUtilizationByHoursCycles = Session("mAircraftUtilizationByHoursCycles")
    End Sub
#End Region

#Region " Data Binding "
    Private Sub SetCombo()

        Dim i As Integer
        Dim prevyear As Integer
        Dim nextyear As Integer

        Year = Now.Year
        prevyear = Year - 10
        nextyear = Year + 10

        If Not IsPostBack Then
            For i = prevyear To nextyear
                cmbYear.Items.Add(i)
            Next


            If cmbYear.Enabled = True Then
                SetFocus(cmbYear)
            End If

            'cmbYear.SelectedValue = Now.Year
            cmbYear.DataBind()

            For k As Integer = 1 To 12
                Dim mon As String = MonthName(k, False)
                cmbMonth.Items.Add(mon)
            Next
            cmbMonth.DataBind()

            If Now.Month = 1 Then
                cmbYear.SelectedValue = Now.Year - 1
                cmbMonth.SelectedValue = MonthName(12, False)
            Else
                cmbYear.SelectedValue = Now.Year
                cmbMonth.SelectedValue = MonthName(Now.Month - 1, False)
            End If
        End If
    End Sub
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString)
        Session("mMachineNameValueList") = mMachineNameValueList

        cmbAircraft.DataSource = mMachineNameValueList
        cmbAircraft.DataBind()
        cmbAircraftforAircraftUtilizationGraph.DataSource = mMachineNameValueList
        cmbAircraftforAircraftUtilizationGraph.DataBind()

        mUser = SI.UTILITY.User.GetUser(mUserID)
        Session("mDashBoardUser") = mUser

        mrptExpiredItemsCount = rptExpiredItemsCount.GetrptExpiredItemsCount()
        Session("mrptExpiredItemsCount") = mrptExpiredItemsCount
        mTransactionwisePendingOrders = TransactionwisePendingOrders.GetTransactionwisePendingOrders()
        Session("mTransactionwisePendingOrders") = mTransactionwisePendingOrders

        mRootCauseCount = RootCauseCount.GetRootCauseCount()
        Session("mRootCauseCount") = mRootCauseCount

        SetValuesForAircraftUtilizationGraph()
        If AircraftIds.ToString = "" Then 'Added By Prashant On 17-Apr-2024 For those clients who has No aircraft list like Bharat avaition
            'Do nothing
        Else
            mAircraftUtilizationByHoursCycles = AircraftUtilizationByHoursCycles.GetAircraftUtilizationGraphByPeriods(AircraftIds, New SmartDate(DateTime.Now.AddMonths(-3)), New SmartDate(Today.Date.ToString), mPeriodParameter)
            Session("mAircraftUtilizationByHoursCycles") = mAircraftUtilizationByHoursCycles
        End If
    End Sub

#End Region

#Region "Graphs"
    Private Function GetColor(ByVal i As Integer) As System.Drawing.Color
        Select Case i

            Case 0
                Return Drawing.Color.Brown
            Case 1
                Return Drawing.Color.Orange
            Case 2
                Return Drawing.Color.Yellow
            Case 3
                Return Drawing.Color.Green
            Case 4
                Return Drawing.Color.Blue
            Case 5
                Return Drawing.Color.Silver
            Case 6
                Return Drawing.Color.Purple
            Case 7
                Return Drawing.Color.Red
            Case 8
                Return Drawing.Color.Orchid
            Case 9
                Return Drawing.Color.YellowGreen
            Case 10
                Return Drawing.Color.Gold
            Case 11
                Return Drawing.Color.BlanchedAlmond
            Case 12 To 60
                Return New System.Drawing.Color()

        End Select
    End Function
    Public Sub SetGraphs()
        Dim mrptSnagMonthWiseGraph As rptSnagMonthWiseGraph
        mrptSnagMonthWiseGraph = rptSnagMonthWiseGraph.GetSnagMonthWiseGraphReport(IIf(cmbYear.SelectedIndex > -1, CInt(cmbYear.SelectedItem.Text), ""))
        Dim MELGraphValues As String = New JavaScriptSerializer().Serialize(mrptSnagMonthWiseGraph)
        MELGraphValues = MELGraphValues.Replace("MonthName", "label").Replace("SnagCount", "value")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FusionChartFunc", "FusionChartFunc('" + MELGraphValues.ToString + "');", True)
    End Sub
    Public Sub SetMELPirepsGraphs()
        'Dim mPirepsMELMonthlyCountGraphicalList As PirepsMELMonthlyCountGraphicalList
        'mPirepsMELMonthlyCountGraphicalList = PirepsMELMonthlyCountGraphicalList.GetPirepsMELCount(IIf(cmbYear.SelectedIndex > -1, CInt(cmbYear.SelectedItem.Text), ""), cmbAircraft.SelectedValue.ToString, "Pireps")
        'Dim PierpsMELGraphCount As String = New JavaScriptSerializer().Serialize(mPirepsMELMonthlyCountGraphicalList)
        'PierpsMELGraphCount = PierpsMELGraphCount.Replace("ActivityCount", "value")
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FusionChartPierpsMELFunc", "FusionChartPierpsMELFunc('" + PierpsMELGraphCount.ToString + "');", True)
        Dim mPirepsCount As PirepsMELMonthlyCountGraphicalList
        Dim mMELCount As PirepsMELMonthlyCountGraphicalList
        Dim mMaintDefectCount As PirepsMELMonthlyCountGraphicalList

        mPirepsCount = PirepsMELMonthlyCountGraphicalList.GetPirepsMELCount(IIf(cmbYear.SelectedIndex > -1, CInt(cmbYear.SelectedItem.Text), ""), cmbAircraft.SelectedValue.ToString, "Pireps")
        mMELCount = PirepsMELMonthlyCountGraphicalList.GetPirepsMELCount(IIf(cmbYear.SelectedIndex > -1, CInt(cmbYear.SelectedItem.Text), ""), cmbAircraft.SelectedValue.ToString, "MEL")
        mMaintDefectCount = PirepsMELMonthlyCountGraphicalList.GetPirepsMELCount(IIf(cmbYear.SelectedIndex > -1, CInt(cmbYear.SelectedItem.Text), ""), cmbAircraft.SelectedValue.ToString, "Maintenance Defect")

        Dim PirepsCount As String = New JavaScriptSerializer().Serialize(mPirepsCount)
        PirepsCount = PirepsCount.Replace("ActivityCount", "value")

        Dim MELCount As String = New JavaScriptSerializer().Serialize(mMELCount)
        MELCount = MELCount.Replace("ActivityCount", "value")

        Dim MaintDefectCount As String = New JavaScriptSerializer().Serialize(mMaintDefectCount)
        MaintDefectCount = MaintDefectCount.Replace("ActivityCount", "value")

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FusionChartPierpsMELFunc", "FusionChartPierpsMELFunc('" + PirepsCount.ToString + "', '" + MELCount.ToString + "', '" + MaintDefectCount.ToString + "');", True)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FusionChartPierpsMELFunc", "FusionChartPierpsMELFunc('" + PirepsCount.ToString + "');", True)
    End Sub
    Public Sub SetPieGraph()
        Dim obj As ReportFlyingHrs
        obj = ReportFlyingHrs.GetGraFlyingHrs(IIf(cmbYear.SelectedIndex > -1, cmbYear.SelectedItem.Text, ""))
        Dim PieGraphFlyingValues As String = New JavaScriptSerializer().Serialize(obj)
        PieGraphFlyingValues = PieGraphFlyingValues.Replace("RegNo", "label").Replace("FlyingHrs", "value")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FusionChartPieFunc", "FusionChartPieFunc('" + PieGraphFlyingValues.ToString + "');", True)
    End Sub
    Public Sub SetLineGraph()
        Dim MonthlyTrendList As ReportMonthlyTrendList
        MonthlyTrendList = ReportMonthlyTrendList.GetReportMonthlyTrendList(IIf(cmbYear.SelectedIndex > -1, CInt(cmbYear.SelectedItem.Text), ""), cmbAircraft.SelectedValue.ToString)
        Dim LineGraphFlyingValues As String = New JavaScriptSerializer().Serialize(MonthlyTrendList)
        LineGraphFlyingValues = LineGraphFlyingValues.Replace("Month", "label").Replace("FlyingHrsHobbs", "value")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FusionChartLineFunc", "FusionChartLineFunc('" + LineGraphFlyingValues.ToString + "');", True)
    End Sub
    'Public Sub SetActivityGraphicalGraph()
    '    Dim mMachineMaintenanceActivityGraphicalList As MachineMaintenanceActivityGraphicalList
    '    mMachineMaintenanceActivityGraphicalList = MachineMaintenanceActivityGraphicalList.GetMaintenanceActivityList(CInt(cmbYear.SelectedItem.Text), cmbAircraft.SelectedValue.ToString)
    '    Dim mActivityGraphical As String = New JavaScriptSerializer().Serialize(mMachineMaintenanceActivityGraphicalList)
    '    mActivityGraphical = mActivityGraphical.Replace("ActivityCount", "value").Replace("ActivityName", "seriesName")
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FusionChartActivityFunc", "FusionChartActivityFunc('" + mActivityGraphical.ToString + "');", True)
    'End Sub
    Public Sub GetLastLogDet()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + cmbAircraft.SelectedValue.ToString + "');", True)
    End Sub
    Public Sub SetLastLogDetLineGraphs()
        Dim mLastLogDetailsForDashBoard As LastLogDetailsForDashBoard
        mLastLogDetailsForDashBoard = LastLogDetailsForDashBoard.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString))
        Dim LastLogDetGraphValues As String = New JavaScriptSerializer().Serialize(mLastLogDetailsForDashBoard)
        LastLogDetGraphValues = LastLogDetGraphValues.Replace("DateFormatted", "label").Replace("TimeInAir", "value")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FusionChartLogDetLineFunc", "FusionChartLogDetLineFunc('" + LastLogDetGraphValues.ToString + "');", True)
    End Sub
    Public Sub SetAircraftConsumptionGraph()
        Dim mAircraftConsumptionGraph As AircraftConsumptionGraph
        mAircraftConsumptionGraph = AircraftConsumptionGraph.GetAircraftConsumption(cmbAircraft.SelectedValue.ToString, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)
        'Serialize(Object)	Converts an object to a JSON string.
        Dim AircraftConsumption As String = New JavaScriptSerializer().Serialize(mAircraftConsumptionGraph)
        AircraftConsumption = AircraftConsumption.Replace("MonthName", "label").Replace("Amount", "value")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AircraftConsumptionGraph", "AircraftConsumptionGraph('" + AircraftConsumption.ToString + "');", True)
    End Sub
    Private Sub SetTransactionwisePendingOrders()
        phPendingPurchaseOrders.Visible = False
        phTransactionwisePendingOrders.Visible = True
        mTransactionwisePendingOrders = TransactionwisePendingOrders.GetTransactionwisePendingOrders()   'Serialize(Object)	Converts an object to a JSON string.
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "TransactionwisePendingOrdersFunc", "TransactionwisePendingOrdersFunc();", True)
    End Sub
    Public Sub ExpiredItemsReport()
        phExpiredItemsCountForReport.Visible = True
        phExpiredItemsInmscolumn2d.Visible = False
        phExpiryDateReport.Visible = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ExpiredItemsReport", "ExpiredItemsReport();", True)
    End Sub
    Public Sub BarChart()
        phExpiredItemsCountForReport.Visible = False
        phExpiredItemsInmscolumn2d.Visible = True
        phExpiryDateReport.Visible = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FusionBarChart", "FusionBarChart();", True)
    End Sub
    Private Sub SetRootCauseCount()
        mRootCauseCount = RootCauseCount.GetRootCauseCount()   'Serialize(Object)	Converts an object to a JSON string.
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "RootCauseCountFunc", "RootCauseCountFunc();", True)
    End Sub

    Private Sub SetValuesForAircraftUtilizationGraph()
        If cmbAircraftforAircraftUtilizationGraph.SelectedIndex < 0 Then
            cmbAircraftforAircraftUtilizationGraph.SelectedIndex = 0   ' The first item has index 0 '
        End If

        AircraftIds = cmbAircraftforAircraftUtilizationGraph.SelectedValue

        Select Case cmbPeriod.SelectedIndex
            Case 0
                mPeriodParameter = PeriodParamater.TimeInAir
            Case 1
                mPeriodParameter = PeriodParamater.Landings

        End Select
    End Sub
    Private Sub SetAircraftUtilizationGraph()

        Try
            SetValuesForAircraftUtilizationGraph()
            Dim VendorwiseAmountSum As Object = Nothing

            mAircraftUtilizationByHoursCycles = AircraftUtilizationByHoursCycles.GetAircraftUtilizationGraphByPeriods(AircraftIds, New SmartDate(DateTime.Now.AddMonths(-3)), New SmartDate(Today.Date.ToString), mPeriodParameter)

            If mPeriodParameter = 1 Then
                VendorwiseAmountSum = From c In mAircraftUtilizationByHoursCycles
                                      Group c By RegNo = c.RegNo Into Group
                                      Select New With {Key .RegNo = RegNo, Key .FlightTimeInAir = Group.Sum(Function(x) Math.Round(Decimal.Parse(x.FlightTimeInAir), 2, MidpointRounding.AwayFromZero))}
            ElseIf mPeriodParameter = 4 Then
                VendorwiseAmountSum = From c In mAircraftUtilizationByHoursCycles
                                      Group c By RegNo = c.RegNo Into Group
                                      Select New With {Key .RegNo = RegNo, Key .FlightCycle = Group.Sum(Function(x) Math.Round(Decimal.Parse(x.FlightCycle), 2, MidpointRounding.AwayFromZero))}
            End If

            Dim AircraftUtilizationGraphValue As String = New JavaScriptSerializer().Serialize(VendorwiseAmountSum)
            If mPeriodParameter = 1 Then
                AircraftUtilizationGraphValue = AircraftUtilizationGraphValue.Replace("FlightTimeInAir", "value").Replace("RegNo", "label")
            Else
                AircraftUtilizationGraphValue = AircraftUtilizationGraphValue.Replace("FlightCycle", "value").Replace("RegNo", "label")
            End If

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AircraftUtilizationGraphFunc", "AircraftUtilizationGraphFunc('" + AircraftUtilizationGraphValue.ToString + "');", True)
            upnlAircraftUtilizationGraph.Update()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub ExpiryDateReportDetails()
        phExpiredItemsCountForReport.Visible = False
        phExpiredItemsInmscolumn2d.Visible = False
        phExpiryDateReport.Visible = True
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ExpiryDateReport", "ExpiryDateReport();", True)
    End Sub
    Public Sub CalibrationDueReportDetails()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CalibrationDueReport", "CalibrationDueReport();", True)
    End Sub
    Public Sub MinLevelItemReportDetails()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "MinLevelItemReport", "MinLevelItemReport();", True)
    End Sub
    Public Sub PendingPurchaseOrdersDetails()
        phPendingPurchaseOrders.Visible = True
        phTransactionwisePendingOrders.Visible = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "PendingPurchaseOrders", "PendingPurchaseOrders();", True)
    End Sub
    Public Sub PendingPurchaseQuotationItemsDetails()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "PendingPurchaseQuotationItems", "PendingPurchaseQuotationItems();", True)
    End Sub
    Public Sub RequisitionPendingForPurchaseOrderDetails()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "RequisitionPendingForPurchaseOrder", "RequisitionPendingForPurchaseOrder();", True)
    End Sub
    'Added by Saylee on 28-Apr-2020, LockDown  period
    Public Sub GetOpenWODet()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncOpenWOList", "FuncOpenWOList('" + cmbAircraft.SelectedItem.Text.ToString + "');", True)
    End Sub
    Public Sub PendingToReceiptsFromOtherStoreDetails()  'Added On 22-May-2020
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "PendingToReceiptsFromOtherStore", "PendingToReceiptsFromOtherStore();", True)
    End Sub
    Public Sub PendingToolsToReceiveFromEmployeeDetails() 'Added On 22-May-2020
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "PendingToolsToReceiveFromEmployee", "PendingToolsToReceiveFromEmployee();", True)
    End Sub
    Public Sub ReceivedUnserviceablePartDetails()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ReceivedUnserviceablePart", "ReceivedUnserviceablePart();", True)
    End Sub
    Public Sub ReceivedFromAircraftAsCoreUnitReturnDetails()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ReceivedFromAircraftAsCoreUnitReturn", "ReceivedFromAircraftAsCoreUnitReturn();", True)
    End Sub
    Public Sub LoanInWardRecordsDetails() 'Loan taken but not return
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "LoanInWardRecord", "LoanInWardRecord();", True)
    End Sub
    'Added By Vikrant On 03-Jun-2020
    Public Sub ReOrderLevelItemReportDetails()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ReOrderLevelItemReport", "ReOrderLevelItemReport();", True)
    End Sub
    Public Sub PendingReturnableExchangeRepairIssueToVendorItemReportDetails()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "PendingReturnableExchangeRepairIssueToVendorItemReport", "PendingReturnableExchangeRepairIssueToVendorItemReport();", True)
    End Sub
    Public Sub LoanOutWardReportDetails()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "LoanOutWardReport", "LoanOutWardReport();", True)
    End Sub

    'Added by Sachin on 25th Jan 2024 For TataSteel Dashboard
    Public Sub AircraftCertificateDetails()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "AircraftCertificate", "AircraftCertificate();", True)
    End Sub
    'End

    'Added by Harsh on 25th Jan 2024 For TataSteel Dashboard
    Public Sub PreFlightAuthorizationDetails()
        ScriptManager.RegisterStartupScript(Me, [GetType](), "PreFlightAuthorizationReport", "PreFlightAuthorizationReport();", True)
    End Sub

    Public Sub AMECertificationDetails()
        ScriptManager.RegisterStartupScript(Me, [GetType](), "AMECertificationReport", "AMECertificationReport();", True)
    End Sub
    'End

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here  
        GetSession1()
        If Not IsPostBack Then
            If Session("MiddleFrame") <> "" And Session("MiddleFrame") <> "DashboardForTodoList.aspx?" Then
                Session("IsFromLogin") = "False"
                Server.Transfer(Session("MiddleFrame"))

            Else
                Session.Remove("MiddleFrame")
            End If
            'Added for Seasonal Greetings
            If Session("IsFromLogin") = "True" Then
                Session.Remove("IsFromLogin")

                Dim mCompanyDetailForGreetings As New CompanyDetailForGreetings
                mCompanyDetailForGreetings = CompanyDetailForGreetings.GetCompanyDetail("", "", "", "", "", "", "")
                Session("mCompanyDetailForGreetings") = mCompanyDetailForGreetings
                If Not mCompanyDetailForGreetings Is Nothing Then

                    If mCompanyDetailForGreetings.ShowGreeting And IsDate(mCompanyDetailForGreetings.FromDateFormatted.ToString) And IsDate(mCompanyDetailForGreetings.FromDateFormatted.ToString) Then
                        If CDate(Today.Date) >= CDate(mCompanyDetailForGreetings.FromDateFormatted.ToString) And CDate(Today.Date) <= CDate(mCompanyDetailForGreetings.ToDateFormatted.ToString) Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenGreetingsWindow", "OpenGreetingsWindow();", True)
                            If AppSettings("ShowWODashBoard") = "True" And Session("ShowDashboardOnLogin") = "True" Then
                                Session("IsFromLogin") = "True"
                            End If
                            GoTo SkipLoop
                        End If
                    End If
                    Session.Remove("mCompanyDetailForGreetings")
                End If
            End If
            'End
SkipLoop:
            DataFieldBind()
            Dim IsFromTopHeader As Boolean = False

            If Not Request.QueryString("IsFromTopHeaderID") Is Nothing Then
                IsFromTopHeader = True
            End If

            If (AppSettings("ShowDashBoard") = "True" And Session("ShowDashboardOnLogin") = "True") Or (IsFromTopHeader = True) Then
                SetCombo()
                ControlVisibilty1()
                CallUpdatePanels()
                ' Session("ShowDashboardOnLogin") = "False" 
            ElseIf AppSettings("ShowWODashBoard") = "True" And Session("ShowDashboardOnLogin") = "True" Then
                Response.Redirect("DashBoardWO.aspx")
            End If
            '-----------------------
            If User.IsInRole("AircraftInformationBoardView") Then ''Added by Saylee on 14-July-2009
                ' pnlDashBoard.Visible = True ''Added by Saylee on 14-July-2009
                ''lblHeader.Visible = True

                '#009dd9
                lblInfoBoard.Text = "AIRCRAFT INFORMATION BOARD (As On Date : " + New SmartDate(Today.Date.ToString).FormattedText + ")"

                If Session("mAircraftInformationBoardList") Is Nothing Then
                    mAircraftInformationBoardList = AircraftInformationBoard.AircraftInformationBoard.GetAircraftInformationBoardList()
                    mAircraftInformationBoardInnerList = AircraftInformationBoard.AircraftInformationBoard.GetAircraftInformationBoardList()

                    Session("mAircraftInformationBoardList") = mAircraftInformationBoardList
                    Session("mAircraftInformationBoardInnerList") = mAircraftInformationBoardInnerList
                Else
                    mAircraftInformationBoardList = Session("mAircraftInformationBoardList")
                    mAircraftInformationBoardInnerList = Session("mAircraftInformationBoardInnerList")
                End If
                pnlAircraftInfoBoard.Visible = mAircraftInformationBoardList.Count > 0

                Rows = mAircraftInformationBoardList.Count + mAircraftInformationBoardList.ModelCount
                'Columns = mAircraftInformationBoardList.MaxHeadingCount(Guid.Empty) - 1 '.MaxColumns
                Columns = mAircraftInformationBoardList.GrandTotalColumnCount() - 1 '.MaxColumns

                CreateDynamicTable()
            End If ''End of User rights

            CallAlert() 'Added by Saylee on 4-May-2010

            'Added By Vikrant on 01-Dec-2021 for PBH
            If Session("IsOpenFromDashboard") Is Nothing Then 'Show only after login not always as dashbord.aspx loaded multiple times
                Dim mPBHList As PBHList
                Dim str1 As String
                mPBHList = PBHList.GetList(IsAllRecordsRequired:=1) '1: Records with IsRenewed = 0 for each aircraft
                For i As Integer = 0 To mPBHList.Count - 1
                    If mPBHList(i).RemainingDays < 30 OrElse mPBHList(i).RemainingHoursDec < 1800 Then '1800 : 30 Hrs
                        Session("IsOpenFromDashboard") = "True"
                        str1 = "<script language=javascript>window.open('wfAboutFlyPal.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');  </script>"
                        ClientScript.RegisterStartupScript(Me.GetType(), "OpenPageScript", str1)
                        Exit For
                    End If
                Next
            End If
            'End
            If User.IsInRole("InvStickyNoteView") And mAlertList.Count > 0 Then
                Dim str As String
                str = "displyStickyNote();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str, True)
            End If

            mUserFavouritesList = UserFavouritesList.GetUserFavourites(HttpContext.Current.User.Identity.Name)

            mUserFavouritesListLinq = From c As UserFavourites In mUserFavouritesList
                                      Where c.MainMenu <> ""
                                      Group c By key = c.MainMenu Into MenuList = Group
                                      Select MainMenu = key, SubMenuCollection = MenuList
        End If
        ControlVisibilty()
    End Sub
    Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Error
        Session("Message") = Context.Server.GetLastError.Message
        Session("Source") = Context.Server.GetLastError.Source
        Session("Trace") = Context.Server.GetLastError.StackTrace
    End Sub
    'Added by Saylee on 4-May-2010
    Private Sub lnkPendingOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkPendingOrder.Click
        'Response.Redirect("wfrptSearchPendingOrder.aspx")
        Try
            AircraftInfoBoard()
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim rpt As rptPendingOrder
            Dim ds As New dsOrder
            myReport = New crptPendingOrder
            Dim objsearch As rptSearchingCriteria
            'GetSession()
            'SetValues()
            Dim dsPenOrd As New dsOrder
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "")
            rpt = rptPendingOrder.GetPendingOrder("1/1/1900", "1/1/2200", "", "", "")
            If rpt.Count <= 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
                msg1.ReplacePage = "Dashboard.aspx?Backpage="
                msg1.Show()
                Exit Sub
            End If
            ds.Clear()
            da.Fill(dsPenOrd, rpt)
            da.Fill(dsPenOrd, objsearch)
            myReport.SetDataSource(dsPenOrd)
            Session("CrystalReport") = myReport
            Dim Str As String
            Str = "<script language=Javascript>openTranDetail();</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
            'ResetValues()
            '---
        Catch ex As Exception

            '   MessageBox.Show(ex.Message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            '  Cursor.Current = Cursors.Default
        End Try
    End Sub
    'Added by Saylee on 4-May-2010
    Private Sub lnkCalibrationDueReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCalibrationDueReport.Click
        'Response.Redirect("wfrptCallibrationDueReport.aspx")
        AircraftInfoBoard()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCalibration
        Dim Obj As rptDueCalibrationList
        'Dim objsearch As rptSearchingCriteria


        Dim mCompanyDetail As New CompanyDetail
        Dim Str1 As String = "As On Date : " & New SmartDate(Today.Date.ToString).FormattedText
        'SetValues()

        Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Calibration Due Report", "", Str1, "", "", "", AppSettings("Product Version"), AppSettings("SINote"))

        Obj = rptDueCalibrationList.GetrptDueCalibrationList(, Today.Date, , , , , New SmartDate(Today.Date.ToString).Date.AddMonths(1).ToShortDateString)
        'objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", ToDate, PartNo, "", "", "", "", "", "", "", Description, "", , , )

        If Obj.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            msg1.ReplacePage = "Dashboard.aspx?Backpage="
            msg1.Show()
            Exit Sub
        End If
        ds.Clear()
        da.Fill(ds, Obj)
        'da.Fill(ds, objsearch)
        da.Fill(ds, Report)
        '************************Report Show ***************************

        ''myReport = New crDueCalibration

        If AppSettings("ClientCode") = "BA" Then 'Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then 'Added by Vikrant on 09-Feb-2015 For ALL09022015
            myReport = New crDueCalibrationBA
        Else
            myReport = New crDueCalibration
        End If

        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
    End Sub
    'Added by Saylee on 4-May-2010
    Private Sub lnkExpiredItems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkExpiredItems.Click
        ''Response.Redirect("wfrptSearchExpiryDate.aspx")
        AircraftInfoBoard()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rpt As rptExpiryDate
        Dim ds As New dsExpiryDate
        Dim objSearch As rptSearchingCriteria
        myReport = New crptExpiryDate

        objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), Today.Date.ToString, "", "", "", "", "", "", "", "", "", "", "")
        rpt = rptExpiryDate.GetExpiryDate(Today.Date.ToString, "", "", "", "", "", -1, Today.Date)

        If rpt.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            msg1.ReplacePage = "Dashboard.aspx?Backpage="
            msg1.Show()
            Exit Sub
        End If

        ds.Clear()
        da.Fill(ds, rpt)
        da.Fill(ds, objSearch)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
    End Sub
    'Added by Saylee on 4-May-2010
    Private Sub lnkItemsToExpire_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkItemsToExpire.Click
        AircraftInfoBoard()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rpt As rptExpiryDate
        Dim ds As New dsExpiryDate
        Dim objSearch As rptSearchingCriteria
        myReport = New crptExpiryDate

        objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), Today.Date.ToString, "", "", "", "", "", "", "", "", "", "", "")
        rpt = rptExpiryDate.GetExpiryDate(Today.Date.ToString, "", "", "", "", "", 0, Today.Date)

        If rpt.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            msg1.ReplacePage = "Dashboard.aspx?Backpage="
            msg1.Show()
            Exit Sub
        End If

        ds.Clear()
        da.Fill(ds, rpt)
        da.Fill(ds, objSearch)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
    End Sub

    Private Sub lnkCoreUnitDue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCoreUnitDue.Click
        AircraftInfoBoard()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsRecCumInvReg
        Dim mCompanyDetail As New CompanyDetail
        Dim objSearch As rptSearchingCriteriaForReceipt
        myReport = New crptCoreUnitDueReport
        Dim mCoreUnitDueList As CoreUnitDueList

        mCoreUnitDueList = CoreUnitDueList.GetCoreUnitDueList(Today.Date.ToString, Guid.Empty.ToString)

        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Today.Date.ToString, "", "", "", "", "", "", "", "", "", "", "All", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")

        If mCoreUnitDueList.Count = 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            msg1.ReplacePage = "Dashboard.aspx?Backpage="
            msg1.Show()
            Exit Sub
        End If

        da.Fill(ds, mCoreUnitDueList)
        da.Fill(ds, objSearch)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
    End Sub
    '--------------------------------------------------------------
    Private Sub cmbYear_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbYear.SelectedIndexChanged
        'SetGraphs()
        'SetPieGraph()
        'If cmbAircraft.SelectedIndex > -1 Then
        '    SetLineGraph()
        'End If
        Session.Remove("LineLast10Logs")
        ControlVisibilty1()
        ' SetAircraftConsumptionGraph()

    End Sub
    Private Sub Tabular_ServerClick(sender As Object, e As System.EventArgs) Handles Tabular.ServerClick
        phJQgrid.Visible = True
        phLogDetLine.Visible = False
        Session("LineLast10Logs") = False
        ControlVisibilty1()
    End Sub
    Private Sub Line_ServerClick(sender As Object, e As System.EventArgs) Handles Line.ServerClick
        phLogDetLine.Visible = True
        phJQgrid.Visible = False
        Session("LineLast10Logs") = True
        SetLastLogDetLineGraphs()
        ControlVisibilty1()

    End Sub
    'Private Sub btnDone_ServerClick(sender As Object, e As System.EventArgs) Handles btnDone.ServerClick
    '    Session.Remove("LineLast10Logs")
    '    Dim builder = New StringBuilder()
    '    builder.Append("You have selected the following checks :<br/>")
    '    ' get the selected checkboxes from the form data
    '    Dim checkString = Request.Form("chkSelect")
    '    'If checkString Is Nothing Then
    '    '    ' MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
    '    '    mUser.UserDashBoardReports.SetALLAsfalse()
    '    '    If mUser.IsDirty Then
    '    '        mUser.IsDashBoardEnabled = False
    '    '        mUser.Save()
    '    '    End If
    '    '    '  Exit Sub
    '    'Else
    '    '    ' we'll need a split to get the individual ids
    '    '    Dim values = checkString.Split(","c)
    '    '    For Each value As String In values
    '    '        builder.Append("<br/>")
    '    '        builder.Append(value)
    '    '        checkedIds.Add(value)
    '    '    Next

    '    '    Dim i As Integer
    '    '    For i = 0 To mUser.UserDashBoardReports.Count - 1
    '    '        If checkedIds.Contains(mUser.UserDashBoardReports(i).DashBoardReportsID.ToString) Then
    '    '            Dim ID As String = mUser.UserDashBoardReports(i).DashBoardReportsID.ToString
    '    '            mUser.UserDashBoardReports(i).IsSelected = True
    '    '        Else
    '    '            mUser.UserDashBoardReports(i).IsSelected = False
    '    '        End If
    '    '    Next
    '    '    values = ""
    '    '    values = ""
    '    '    checkString = Nothing

    '    '    If mUser.IsDirty Then
    '    '        mUser.IsDashBoardEnabled = True
    '    '        mUser.Save()

    '    '    End If
    '    'End If

    '    Session("mDashBoardUser") = mUser
    '    SetCombo()



    '    ' SetLastLogDetLineGraphs()

    '    LogDetLine.Visible = False
    '    ControlVisibilty1()
    'End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        Session.Remove("LineLast10Logs")
        ControlVisibilty1()
        'SetAircraftConsumptionGraph()
    End Sub
    Private Sub cmbMonth_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbMonth.SelectedIndexChanged
        'SetAircraftConsumptionGraph()
        'upnlAircraftConsumption.Update()
        Session.Remove("LineLast10Logs")
        ControlVisibilty1()
    End Sub
    Private Sub TabularExpiredItems_ServerClick(sender As Object, e As System.EventArgs) Handles TabularExpiredItems.ServerClick
        ExpiredItemsReport()
        upnlExpiredItemsCountForReport.Update()
        upnlExpiredItemsInmscolumn2d.Update()
        upnlExpiryDateReport.Update()
    End Sub
    Private Sub BarExpiredItems_ServerClick(sender As Object, e As System.EventArgs) Handles BarExpiredItems.ServerClick
        BarChart()
        upnlExpiredItemsCountForReport.Update()
        upnlExpiredItemsInmscolumn2d.Update()
        upnlExpiryDateReport.Update()
    End Sub
    Private Sub btnExpiredItemsDetails_ServerClick(sender As Object, e As System.EventArgs) Handles btnExpiredItemsDetails.ServerClick
        ExpiryDateReportDetails()
        upnlExpiredItemsCountForReport.Update()
        upnlExpiredItemsInmscolumn2d.Update()
        upnlExpiryDateReport.Update()
    End Sub
    Private Sub btnTransactionwisePendingOrders_ServerClick(sender As Object, e As System.EventArgs) Handles btnTransactionwisePendingOrders.ServerClick
        SetTransactionwisePendingOrders()
        upnlTransactionwisePendingOrders.Update()
        upnlPendingPurchaseOrders.Update()
    End Sub
    Private Sub btnPendingPurchaseOrders_ServerClick(sender As Object, e As System.EventArgs) Handles btnPendingPurchaseOrders.ServerClick
        PendingPurchaseOrdersDetails()
        upnlTransactionwisePendingOrders.Update()
        upnlPendingPurchaseOrders.Update()
    End Sub
    'Private Sub TabularExpiryDateReport_ServerClick(sender As Object, e As System.EventArgs) Handles TabularExpiryDateReport.ServerClick
    '    ExpiryDateReportDetails()
    '    upnlExpiredItemsCountForReport.Update()
    '    upnlExpiredItemsInmscolumn2d.Update()
    '    upnlExpiryDateReport.Update()
    'End Sub
    '--------------------------------------------------------------
#End Region

    '--------------------------------------------------------------
#Region "Web Methods"

    <WebMethod(EnableSession:=True)>
    Public Shared Function AircraftCurrentStatusList() As Object
        Dim mMachineListOfAircraftCurrentStatus As ListOfAircraftCurrentStatus
        mMachineListOfAircraftCurrentStatus = ListOfAircraftCurrentStatus.GetListOfAircraftCurrentStatus(, , , , , Today.Date.ToString)
        Return mMachineListOfAircraftCurrentStatus
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function AuditDetails() As Object
        Dim mrptAuditStatusRegisterReport As rptAuditStatusRegisterReport
        mrptAuditStatusRegisterReport = rptAuditStatusRegisterReport.GetrptAuditStatusRegisterReport()
        Dim mAuditStatus = (From c As rptAuditStatusRegisterReport.rptAuditStatusRegisterReportInfo In mrptAuditStatusRegisterReport
                            Where (c.AuditExecutionStatusID = 1 Or c.AuditExecutionStatusID = 0)
                            Select c).ToList
        Return mAuditStatus
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function LogDetails(MachineID As String) As Object
        Dim mLastLogDetailsForDashBoard As LastLogDetailsForDashBoard
        mLastLogDetailsForDashBoard = LastLogDetailsForDashBoard.GetLogList(New Guid(MachineID))
        Return mLastLogDetailsForDashBoard
    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function AircraftCurrentStatusJTAbleList() As Object
        Dim mMachineListOfAircraftCurrentStatus As ListOfAircraftCurrentStatus
        mMachineListOfAircraftCurrentStatus = ListOfAircraftCurrentStatus.GetListOfAircraftCurrentStatus()



        'Dim strListOfAircraftCurrentStatus As String = New JavaScriptSerializer().Serialize(mMachineListOfAircraftCurrentStatus)
        '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JQGridCurrentStatusFunc", "JQGridCurrentStatusFunc('" + strListOfAircraftCurrentStatus.ToString + "');", True)
        Return New With {
                         Key .Result = "OK",
                         Key .Records = mMachineListOfAircraftCurrentStatus
                     }
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function ExpiredItemsCountForReport() As Object
        Dim mrptExpiredItemsCount As rptExpiredItemsCount
        mrptExpiredItemsCount = rptExpiredItemsCount.GetrptExpiredItemsCount()
        Return mrptExpiredItemsCount
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function ExpiryDateReport() As Object
        Dim mrptExpiryDate As rptExpiryDate
        mrptExpiryDate = rptExpiryDate.GetExpiryDate(Today.Date.ToString, "", "", "", "", "", 2, Today.Date.ToString) '2 index For  0 To 3 Month
        Dim TemprptExpiryDate = (From c In mrptExpiryDate
                                 Where (c.DateDifference <= 0 Or (c.DateDifference > 0 And c.DateDifference <= 7))
                                 Select c)
        Return TemprptExpiryDate
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function CalibrationDueReport() As Object
        Dim mrptDueCalibrationList As rptDueCalibrationList
        mrptDueCalibrationList = rptDueCalibrationList.GetrptDueCalibrationList(, Today.Date.ToString, "", "", "", "{00000000-0000-0000-0000-000000000000}",
            "1/1/3300", "{00000000-0000-0000-0000-000000000000}", False, 0, 0)
        Dim TempDueCalibrationList = (From c In mrptDueCalibrationList
                                      Where (c.RemainingDays <= 0 Or (c.RemainingDays > 0 And c.RemainingDays <= 7))
                                      Select c)
        Return TempDueCalibrationList
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function MinLevelItemReport() As Object
        Dim mrptMinLevelItem As rptMinLevelItem
        mrptMinLevelItem = rptMinLevelItem.GetMinLevelItem("", "", "", "", "", Guid.Empty, , 0)
        Return mrptMinLevelItem
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function PendingPurchaseOrders() As Object
        Dim mrptPendingOrder As rptPendingOrder
        mrptPendingOrder = rptPendingOrder.GetPendingOrder("1-1-1900", "1-1-2200", "", "", "")
        Return mrptPendingOrder
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function GetPendingPurchaseQuotationItem() As Object
        Dim mPendingPurchaseQuotationItems As PendingPurchaseQuotationItems
        mPendingPurchaseQuotationItems = PendingPurchaseQuotationItems.GetPendingQuotationList(Guid.Empty)
        Return mPendingPurchaseQuotationItems
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function GetRequisitionPendingForPurchaseOrder() As Object
        Dim mRequisitionItemsNew As RequisitionItemsNew
        mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForList(Today.Date.ToString, "", Guid.Empty, 2)
        Return mRequisitionItemsNew
    End Function
    'Added by Saylee on 28-Apr-2020 ''LOCKDOWN Period
    <WebMethod(EnableSession:=True)>
    Public Shared Function WODetails(MachineID As String) As Object
        Dim mWOList As nWOList
        mWOList = nWOList.GetWOList(, , "1/1/1900", "1/1/2200", MachineID, , 1, 1, , , 89)
        Return mWOList
    End Function
    'Added By Prashant  22-May-2020 ALL2205202
    <WebMethod(EnableSession:=True)>
    Public Shared Function PendingToReceiptsFromOtherStore() As Object
        Dim mrptIssueRegForReminder As rptIssueRegForReminder
        mrptIssueRegForReminder = rptIssueRegForReminder.GetPendingToReceiptFromStore("1/1/1900", "1/1/2200", "", "", "", "")
        Dim IssueRegForReminder = (From c As rptIssueRegForReminder.IssueInfo In mrptIssueRegForReminder
                                   Order By CDate(c.IssueDateFormatted.ToString) Descending
                                   Select c)
        Return IssueRegForReminder
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function PendingToolsToReceiveFromEmployeeRecords() As Object
        Dim mPendingToolsToReceiveFromEmployee As PendingToolsToReceiveFromEmployee
        mPendingToolsToReceiveFromEmployee = PendingToolsToReceiveFromEmployee.GetPendingTools("", Today.Date.ToString)
        Return mPendingToolsToReceiveFromEmployee
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function ReceivedUnserviceablePartRecords() As Object
        Dim mReceivedUnserviceablePart As StockItemListForAcceptanceTag
        mReceivedUnserviceablePart = StockItemListForAcceptanceTag.GetStockItemListForAcceptanceTag("", "", 0)
        Dim UnserviceablePart = (From c As StockItemForAcceptanceTag In mReceivedUnserviceablePart Where c.PartStatus = "Unserviceable"
                                 Order By CDate(c.DateFormatted.ToString) Descending
                                 Select c)
        Return UnserviceablePart
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function ReceivedFromAircraftAsCoreUnitReturnRecords() As Object
        Dim mRCIFromAircraftAsCoreUnitReturnList As PartListForRCIFromAircraftAsCoreUnitReturnList
        mRCIFromAircraftAsCoreUnitReturnList = PartListForRCIFromAircraftAsCoreUnitReturnList.GetPartListForRCIFromAircraftAsCoreUnitReturn([Date]:=Today.Date.ToString)
        Dim AircraftAsCoreUnitReturnList = (From c As PartListForRCIFromAircraftAsCoreUnitReturnList.PartListForRCIFromAircraftAsCoreUnitReturnListInfo In mRCIFromAircraftAsCoreUnitReturnList
                                            Order By CDate(c.IssueDateFormatted.ToString) Descending
                                            Select c)
        Return AircraftAsCoreUnitReturnList
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function LoanInWard() As Object  'Loan Taken but not return records
        Dim mLoanInWardRecords As LoanInWardRecords
        mLoanInWardRecords = LoanInWardRecords.GetLoanInWardRecords("1/1/1900", "1/1/3300", CustomerStore:=1)
        Dim InWardRecords = (From c As LoanInWardRecords.LoanInWardRecordsInfo In mLoanInWardRecords
                             Where (c.TransTypeID = 12 Or c.TransTypeID = 48 Or c.TransTypeID = 50 Or c.TransTypeID = 57) And (c.LoanQty > 0.0)
                             Order By CDate(c.ReceiptDateFormatted.ToString) Descending
                             Select c)
        Return InWardRecords
    End Function
    'End of Added By Prashant  22-May-2020 ALL2205202
    'Added By Vikrant On 02-Jun-2020 22-May-2020 ALL2205202 
    <WebMethod(EnableSession:=True)>
    Public Shared Function ReOrderLevelItemReport() As Object
        Dim mrptReOrderLevelItem As rptReOrderLevelItem
        mrptReOrderLevelItem = rptReOrderLevelItem.GetMinReOrderItem("", "", "", "", Guid.Empty, False, IsBAReorderQtyFormulaRequired:=IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS", True, False))
        Return mrptReOrderLevelItem
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function PendingReturnableExchangeRepairIssueToVendorItemReport() As Object
        Dim mrptIssueRegForReminder As rptIssueRegForReminder
        mrptIssueRegForReminder = rptIssueRegForReminder.GetPendingToReceiptAgainstExchangeRepairFromVendor("1/1/1900", "1/1/2200", "", "", "", "")
        Dim PendingToReceiptAgainstExchangeRepairFromVendor = (From c As rptIssueRegForReminder.IssueInfo In mrptIssueRegForReminder
                                                               Order By CDate(c.IssueDateFormatted.ToString) Descending
                                                               Select c)
        Return PendingToReceiptAgainstExchangeRepairFromVendor
    End Function
    <WebMethod(EnableSession:=True)>
    Public Shared Function LoanOutWardReport() As Object
        Dim mLoanOutWardRecords As LoanOutWardRecords
        mLoanOutWardRecords = LoanOutWardRecords.GetLoanOutWardRecords("01-Jan-1900", "31-Dec-2200", CustomerStore:=1)
        Dim OutWardRecords = (From c As LoanOutWardRecords.LoanOutWardRecordsInfo In mLoanOutWardRecords
                              Where (c.TransTypeID = 17 Or c.TransTypeID = 20 Or c.TransTypeID = 24 Or c.TransTypeID = 26 Or c.TransTypeID = 45) And (c.LoanQty > 0.0)
                              Order By CDate(c.IssueDateFormatted.ToString) Descending
                              Select c)
        Return OutWardRecords
    End Function

    'Added by Sachin on 25th Jan 2024 For TataSteel Dashboard
    <WebMethod(EnableSession:=True)>
    Public Shared Function AircraftCertificate() As Object

        Dim CertificateName As String = ""
        Dim mRenewMachineCertificateList As MachineCertificateList
        mRenewMachineCertificateList = MachineCertificateList.GetMachineCertificateList(Guid.Empty, Today.Date.ToString)


        Dim mSortedMachineCertificateList = (From c As MachineCertificateList.MachineCertificateListInfo In mRenewMachineCertificateList
                                             Order By c.RegNo, c.RemainingDays
                                             Select c).ToList

        Return mSortedMachineCertificateList

    End Function

    'End

    'Added by Harsh on 25th Jan 2024 For TataSteel Dashboard
    <WebMethod(EnableSession:=True)>
    Public Shared Function PreFlightAuthorizationReport() As Object

        Dim mEmployeeDocumentDueList As EmployeeDocumentDueList
        Try
            mEmployeeDocumentDueList = EmployeeDocumentDueList.GetEmployeeDocumentDueList(EmployeeID:=Guid.Empty, DocumentID:=Guid.Empty,
                                                                                          DocNo:="", AsOnDate:=Today.Date.ToString(),
                                                                                          Range:=0, IsUsedInFlightLog:=1,
                                                                                          ExpiredEntriesOnly:=0, Applicability:=0,
                                                                                          EmployeeDeptID:=Guid.Empty.ToString())
            Dim PreFlightAuthorizationRecords = From Records As EmployeeDocumentDueList.EmployeeDocumentDueListInfo In mEmployeeDocumentDueList
                                                Order By CDate(Records.DateOfExpiry) Ascending
                                                Select Records
            Return PreFlightAuthorizationRecords
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function AMECertificationReport() As Object

        Dim mEmployeeDocumentDueList As EmployeeDocumentDueList
        Try
            mEmployeeDocumentDueList = EmployeeDocumentDueList.GetEmployeeDocumentDueList(EmployeeID:=Guid.Empty, DocumentID:=Guid.Empty,
                                                                                          DocNo:="", AsOnDate:=Today.Date.ToString(),
                                                                                          Range:=0, IsUsedInFlightLog:=0,
                                                                                          ExpiredEntriesOnly:=0, Applicability:=0,
                                                                                          EmployeeDeptID:=Guid.Empty.ToString(),
                                                                                          SkipUsedInFlightLog:=1)
            Dim PreFlightAuthorizationRecords = From Records As EmployeeDocumentDueList.EmployeeDocumentDueListInfo In mEmployeeDocumentDueList
                                                Order By CDate(Records.DateOfExpiry) Ascending
                                                Select Records
            Return PreFlightAuthorizationRecords
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    'End

#End Region

#Region "Checked Selection"
    Public Function NumeroChequeInclus(ByVal numero As String) As String
        If (checkedIds.Contains(numero)) Then
            Return "checked"
        Else
            Return String.Empty
        End If
    End Function

    Private Sub cmbAircraftforAircraftUtilizationGraph_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAircraftforAircraftUtilizationGraph.SelectedIndexChanged
        SetAircraftUtilizationGraph()
    End Sub

    Private Sub cmbPeriod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPeriod.SelectedIndexChanged
        SetAircraftUtilizationGraph()
    End Sub
#End Region
    '--------------------------------------------------------------

End Class

