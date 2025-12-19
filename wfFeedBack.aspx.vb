Imports System.Net
Imports AjaxControlToolkit
Imports Flypal.CompanyDetailForAPI
'created by hitesh on 16-Jan-2025
Partial Class wfFeedBack
    Inherits System.Web.UI.Page

#Region "Variable Declarartion"

    Dim mGBUser As User
    Dim mRegInformation As RegInformation
    'Dim mEventLogSession As EventLogSetSession
    Dim mCompanyDetail As CompanyDetail
    Dim mRating As Integer
    Dim mRating_Q1 As Integer
    Dim mRating_Q2 As Integer
    Dim mRating_Q3 As Integer
    Dim mRating_Q4 As Integer
    Dim mRating_Q5 As Integer
    Dim sec As Integer = 0
#End Region

#Region "Helper Method"

    Public Sub ShowAlertBox(Optional ByVal Msg As String = "", Optional ByVal MsgType As String = "")

        Dim str As String
        str = "opennotificationpopup('" & Msg & "','" & MsgType & "');"

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str, True)

    End Sub

    Public Sub Open_FeedBack(Optional ByVal Msg As String = "")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, "open_mdlFeedBack();", True)
    End Sub


    Public Sub Close_FeedBack(Optional ByVal Msg As String = "")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, "hide_mdlFeedBack();", True)
    End Sub

	Private Sub GetSession()

		mGBUser = Session("mUser")
		mRegInformation = Session("RegInformation")
		'mEventLogSession = Session("EventLogSession")
		mRating = Session("wfFeedBack.Rating")
		sec = Session("wfFeedBack.sec")
		mCompanyDetail = Session("mCompanyDetail")
		mRating_Q1 = Session("wfFeedBack.Rating_Q1")
		mRating_Q2 = Session("wfFeedBack.Rating_Q2")
		mRating_Q3 = Session("wfFeedBack.Rating_Q3")
		mRating_Q4 = Session("wfFeedBack.Rating_Q4")
		mRating_Q5 = Session("wfFeedBack.Rating_Q5")
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mUser")
		Session.Remove("mCompanyDetail")
		Session.Remove("mRating")
	End Sub

	Public Sub Open_mdlMsgPopup(Optional ByVal Msg As String = "")
        lblMsglabel.Text = Msg
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, "open_mdlMsgPopup_popup();", True)
        'upnlMsgPopupLabel.Update()
    End Sub

    Public Sub Close_mdlMsgPopup()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, "hide_mdlMsgPopup_popup();", True)
    End Sub

    'Sankalp 08-09-25
    Public Function IsSubmitValid() As Boolean
        If Not (rdbQ1_1.Checked Or rdbQ1_2.Checked Or rdbQ1_3.Checked Or rdbQ1_4.Checked Or rdbQ1_5.Checked) Then
            ShowAlertBox("Please attempt all the questions", "error")
            lblQuestion01.Style.Add("color", "red")
            lblQuestion01.Focus()
            'upnlFeedbackForm.Update()
            Return False
        Else
            lblQuestion01.Style.Add("color", "black")
        End If

        If Not (rdbQ2_1.Checked Or rdbQ2_2.Checked Or rdbQ2_3.Checked Or rdbQ2_4.Checked Or rdbQ2_5.Checked) Then
            ShowAlertBox("Please attempt all the questions", "error")
            lblQuestion02.Style.Add("color", "red")
            lblQuestion02.Focus()
            'upnlFeedbackForm.Update()
            Return False
        Else
            lblQuestion02.Style.Add("color", "black")
        End If

        If Not (rdbQ3_1.Checked Or rdbQ3_2.Checked Or rdbQ3_3.Checked Or rdbQ3_4.Checked Or rdbQ3_5.Checked) Then
            ShowAlertBox("Please attempt all the questions", "error")
            lblQuestion03.Style.Add("color", "red")
            'upnlFeedbackForm.Update()
            lblQuestion03.Focus()
            Return False
        Else
            lblQuestion03.Style.Add("color", "black")
        End If

        If Not (rdbQ4_1.Checked Or rdbQ4_2.Checked Or rdbQ4_3.Checked Or rdbQ4_4.Checked Or rdbQ4_5.Checked) Then
            ShowAlertBox("Please attempt all the questions", "error")
            lblQuestion04.Style.Add("color", "red")
            'upnlFeedbackForm.Update()
            lblQuestion04.Focus()
            Return False
        Else
            lblQuestion04.Style.Add("color", "black")
        End If

        If Not (rdbQ5_1.Checked Or rdbQ5_2.Checked Or rdbQ5_3.Checked Or rdbQ5_4.Checked Or rdbQ5_5.Checked) Then
            ShowAlertBox("Please attempt all the questions", "error")
            lblQuestion05.Style.Add("color", "red")
            'upnlFeedbackForm.Update()
            lblQuestion05.Focus()
            Return False
        Else
            lblQuestion05.Style.Add("color", "black")
        End If

        If (ChkContactBack.Checked AndAlso txtContactNumber.Text = "" And txtContactEmail.Text = "") Then
            ShowAlertBox("Please Enter the Contact Details", "error")
            lblContactDetails.Style.Add("color", "red")
            'upnlFeedbackForm.Update()
            lblContactDetails.Focus()
            Return False
        Else
            lblContactDetails.Style.Add("color", "black")
        End If
        Return True
    End Function

#End Region

#Region "Events"
    Private Sub wfFeedBack_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            GetSession()
            If Not IsPostBack Then
                'Open_mdlMsgPopup("Please help us to improve our services by giving your valuable feedback.")
                Open_FeedBack()
            End If

        Catch ex As Exception

            ex = ex.GetBaseException

            If InStr(ex.Message, "Server was not found", CompareMethod.Text) Then
                ShowAlertBox("The server was not found or was not accessible.", "error")
            Else
                ShowAlertBox(ex.Message.ToString, "error")
            End If

        End Try
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click
        If IsSubmitValid() = False Then
            Exit Sub
        End If
        RadioButton_CheckedChanged()
        'Dim NPS As NPSService.FlyPalNPSService = New NPSService.FlyPalNPSService
        Dim NPS As NPSServiceBytzsoft.FlyPalNPSService = New NPSServiceBytzsoft.FlyPalNPSService
		'Dim result As String = NPS.SubmitFeedBack(mRegInformation.CorporateID, "", 1, Date.Today.ToString, mGBUser.Name, txtSuggestionAnswer.Text, mRating, mEventLogSession.IPAddress.ToString)
		Dim result As String = NPS.SubmitFeedBackNew(AppSettings("CorporateID"), mCompanyDetail.CompanyName, 2, Date.Today.ToString, mGBUser.Name, mRating_Q1, mRating_Q2, mRating_Q3, mRating_Q4, mRating_Q5, txtSuggestionAnswer.Text, ChkContactBack.Checked, txtContactNumber.Text, txtContactEmail.Text, IPAddress())
		'Dim result As String = NPS.SubmitFeedBack(AppSettings("CorporateID"), mCompanyDetail.CompanyName, 2, New SmartDate(Date.Today.ToString).Date.ToString("dd-MMM-yyyy"), mGBUser.Name, txtSuggestionAnswer.Text, mRating, IPAddress())

		Dim temp As String() = result.Split(",")

        If temp(0) = "1" Then

            'Update user 
            ' 
            mGBUser.FeedBackSubmittedDate = Now
            mGBUser.Save()
            Session("GlobalUser") = mGBUser
            '---

            ShowAlertBox("Feedback Submitted successfully.", "success")

            Dim msg As String = ""

            If mRating <= 6 Then
                msg = "Thank you for your feedback. We greatly value all ideas and suggestions. In the future, our team may reach out to you to learn more about it, ensuring we exceed your expectations."
            ElseIf mRating >= 7 And mRating <= 8 Then
                msg = "Thanks for your feedback. Your ideas, and suggestions play a major role to improve us."
            ElseIf mRating > 8 Then
                msg = "Thanks for your feedback. We will make sure you have the best possible experience."
            End If
            btnMsgOk.Visible = True
            'upnlFeedbackbuttons.Update()
            'Close_FeedBack()
            Open_mdlMsgPopup(msg)
        Else
            ShowAlertBox(temp(1), "error")
        End If

    End Sub

    Protected Sub btnMsgOk_Click(sender As Object, e As System.EventArgs) Handles btnMsgOk.Click
		Session("MiddleFrame") = ""
		RemoveSession()
		Response.Redirect("index.aspx")
	End Sub

    Private Sub ChkContactBack_CheckedChanged(sender As Object, e As EventArgs) Handles ChkContactBack.CheckedChanged
        If ChkContactBack.Checked Then
            divContactBack.Visible = True
        Else
            divContactBack.Visible = False

            txtContactNumber.Text = ""
            txtContactEmail.Text = ""
        End If
    End Sub

    Public Sub RadioButton_CheckedChanged()
        'Question 1
        If rdbQ1_1.Checked Then
            mRating_Q1 = 1
        ElseIf rdbQ1_2.Checked Then
            mRating_Q1 = 2
        ElseIf rdbQ1_3.Checked Then
            mRating_Q1 = 3
        ElseIf rdbQ1_4.Checked Then
            mRating_Q1 = 4
        ElseIf rdbQ1_5.Checked Then
            mRating_Q1 = 5
        End If
        'Question 2
        If rdbQ2_1.Checked Then
            mRating_Q2 = 1
        ElseIf rdbQ2_2.Checked Then
            mRating_Q2 = 2
        ElseIf rdbQ2_3.Checked Then
            mRating_Q2 = 3
        ElseIf rdbQ2_4.Checked Then
            mRating_Q2 = 4
        ElseIf rdbQ2_5.Checked Then
            mRating_Q2 = 5
        End If
        'Question 3
        If rdbQ3_1.Checked Then
            mRating_Q3 = 1
        ElseIf rdbQ3_2.Checked Then
            mRating_Q3 = 2
        ElseIf rdbQ3_3.Checked Then
            mRating_Q3 = 3
        ElseIf rdbQ3_4.Checked Then
            mRating_Q3 = 4
        ElseIf rdbQ3_5.Checked Then
            mRating_Q3 = 5
        End If
        'Question 4
        If rdbQ4_1.Checked Then
            mRating_Q4 = 1
        ElseIf rdbQ4_2.Checked Then
            mRating_Q4 = 2
        ElseIf rdbQ4_3.Checked Then
            mRating_Q4 = 3
        ElseIf rdbQ4_4.Checked Then
            mRating_Q4 = 4
        ElseIf rdbQ4_5.Checked Then
            mRating_Q4 = 5
        End If

        'Question 5
        If rdbQ5_1.Checked Then
            mRating_Q5 = 1
        ElseIf rdbQ5_2.Checked Then
            mRating_Q5 = 2
        ElseIf rdbQ5_3.Checked Then
            mRating_Q5 = 3
        ElseIf rdbQ5_4.Checked Then
            mRating_Q5 = 4
        ElseIf rdbQ5_5.Checked Then
            mRating_Q5 = 5
        End If

    End Sub
    'Protected Sub RadioButton1_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdbQ1_1.CheckedChanged, rdbQ1_2.CheckedChanged, rdbQ1_3.CheckedChanged, rdbQ1_4.CheckedChanged, rdbQ1_5.CheckedChanged

    '    'lblSuggestionQuestion.Text = "How can we improve your experience?"
    '    ' ShowAlertBox("Q1 Rating")
    '    '  mRating_Q1 = CInt(DirectCast(sender, System.Web.UI.WebControls.RadioButton).ID)
    '    Dim m_Q1Rating As String = ""
    '    m_Q1Rating = (DirectCast(sender, System.Web.UI.WebControls.RadioButton).ID)

    '    If m_Q1Rating = "rdbQ1_1" Then
    '        mRating_Q1 = 1
    '    ElseIf m_Q1Rating = "rdbQ1_2" Then
    '        mRating_Q1 = 2
    '    ElseIf m_Q1Rating = "rdbQ1_3" Then
    '        mRating_Q1 = 3
    '    ElseIf m_Q1Rating = "rdbQ1_4" Then
    '        mRating_Q1 = 4
    '    ElseIf m_Q1Rating = "rdbQ1_5" Then
    '        mRating_Q1 = 5
    '    End If

    '    Session("wfFeedBack.Rating_Q1") = mRating_Q1
    '    'upnlFeedbackForm.Update()

    'End Sub

    'Protected Sub RadioButton2_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdbQ2_1.CheckedChanged, rdbQ2_2.CheckedChanged, rdbQ2_3.CheckedChanged, rdbQ2_4.CheckedChanged, rdbQ2_5.CheckedChanged

    '    Dim m_Q2Rating As String = ""
    '    m_Q2Rating = (DirectCast(sender, System.Web.UI.WebControls.RadioButton).ID)

    '    If m_Q2Rating = "rdbQ2_1" Then
    '        mRating_Q2 = 1
    '    ElseIf m_Q2Rating = "rdbQ2_2" Then
    '        mRating_Q2 = 2
    '    ElseIf m_Q2Rating = "rdbQ2_3" Then
    '        mRating_Q2 = 3
    '    ElseIf m_Q2Rating = "rdbQ2_4" Then
    '        mRating_Q2 = 4
    '    ElseIf m_Q2Rating = "rdbQ2_5" Then
    '        mRating_Q2 = 5
    '    End If

    '    Session("wfFeedBack.Rating_Q2") = mRating_Q2
    '    'upnlFeedbackForm.Update()

    'End Sub

    'Protected Sub RadioButton3_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdbQ3_1.CheckedChanged, rdbQ3_2.CheckedChanged, rdbQ3_3.CheckedChanged, rdbQ3_4.CheckedChanged, rdbQ3_5.CheckedChanged

    '    Dim m_Q3Rating As String = ""
    '    m_Q3Rating = (DirectCast(sender, System.Web.UI.WebControls.RadioButton).ID)

    '    If m_Q3Rating = "rdbQ3_1" Then
    '        mRating_Q3 = 1
    '    ElseIf m_Q3Rating = "rdbQ3_2" Then
    '        mRating_Q3 = 2
    '    ElseIf m_Q3Rating = "rdbQ3_3" Then
    '        mRating_Q3 = 3
    '    ElseIf m_Q3Rating = "rdbQ3_4" Then
    '        mRating_Q3 = 4
    '    ElseIf m_Q3Rating = "rdbQ3_5" Then
    '        mRating_Q3 = 5
    '    End If

    '    Session("wfFeedBack.Rating_Q3") = mRating_Q3
    '    'upnlFeedbackForm.Update()

    'End Sub

    'Protected Sub RadioButton4_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdbQ4_1.CheckedChanged, rdbQ4_2.CheckedChanged, rdbQ4_3.CheckedChanged, rdbQ4_4.CheckedChanged, rdbQ4_5.CheckedChanged

    '    Dim m_Q4Rating As String = ""
    '    m_Q4Rating = (DirectCast(sender, System.Web.UI.WebControls.RadioButton).ID)

    '    If m_Q4Rating = "rdbQ4_1" Then
    '        mRating_Q4 = 1
    '    ElseIf m_Q4Rating = "rdbQ4_2" Then
    '        mRating_Q4 = 2
    '    ElseIf m_Q4Rating = "rdbQ4_3" Then
    '        mRating_Q4 = 3
    '    ElseIf m_Q4Rating = "rdbQ4_4" Then
    '        mRating_Q4 = 4
    '    ElseIf m_Q4Rating = "rdbQ4_5" Then
    '        mRating_Q4 = 5
    '    End If

    '    Session("wfFeedBack.Rating_Q4") = mRating_Q4
    '    'upnlFeedbackForm.Update()

    'End Sub

    'Protected Sub RadioButton5_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdbQ5_1.CheckedChanged, rdbQ5_2.CheckedChanged, rdbQ5_3.CheckedChanged, rdbQ5_4.CheckedChanged, rdbQ5_5.CheckedChanged

    '    Dim m_Q5Rating As String = ""
    '    m_Q5Rating = (DirectCast(sender, System.Web.UI.WebControls.RadioButton).ID)

    '    If m_Q5Rating = "rdbQ5_1" Then
    '        mRating_Q5 = 1
    '    ElseIf m_Q5Rating = "rdbQ5_2" Then
    '        mRating_Q5 = 2
    '    ElseIf m_Q5Rating = "rdbQ5_3" Then
    '        mRating_Q5 = 3
    '    ElseIf m_Q5Rating = "rdbQ5_4" Then
    '        mRating_Q5 = 4
    '    ElseIf m_Q5Rating = "rdbQ5_5" Then
    '        mRating_Q5 = 5
    '    End If

    '    Session("wfFeedBack.Rating_Q5") = mRating_Q5
    '    'upnlFeedbackForm.Update()

    'End Sub

#End Region

    'Extra Added 
    Private Function IPAddress() As String
        Dim server As String = Nothing
        server = Me.Context.Request.UserHostAddress()
        If server = "127.0.0.1" Then
            server = Dns.GetHostName()
        End If

        Dim heserver As IPHostEntry = Dns.Resolve(server)
        Dim curAdd As IPAddress
        For Each curAdd In heserver.AddressList
            curAdd.ToString()
        Next curAdd
        Return curAdd.ToString()

        ''Return Me.Context.Request.UserHostAddress()
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfFeedBack.aspx?"
            DataBind()
        End If
    End Sub
End Class