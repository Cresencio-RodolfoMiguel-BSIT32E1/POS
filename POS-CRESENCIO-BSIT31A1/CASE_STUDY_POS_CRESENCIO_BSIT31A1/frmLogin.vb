Imports System.Data.OleDb
Public Class frmLogin
    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Connection()
    End Sub
    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        sql = "Select * from qryUsers where Username='" & txtUsername.Text & "' and Password='" & txtPassword.Text & "'"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        If dr.Read() Then
            Dim role As String = dr("Role").ToString()
            Dim firstname As String = dr("Firstname").ToString()
            Dim username As String = dr("Username").ToString()
            Select Case role
                Case "IT"
                    frmDashboard.tssEmpName.Text = dr("Firstname").ToString & " " & dr("Lastname").ToString
                    frmDashboard.tssPosition.Text = dr("Role").ToString
                    frmDashboard.tssUsername.Text = username
                    MsgBox("Hello, Admin " & firstname & "!", vbInformation, "Successful Login!")
                    frmPOS.lblCashier.Text = dr("Firstname").ToString & " " & dr("Lastname").ToString
                    ActLogs("Logged In")
                    frmDashboard.Show()
                    Me.Hide()
                Case "Manager"
                    frmManagerDashboard.tssEmpName.Text = dr("Firstname").ToString & " " & dr("Lastname").ToString
                    frmManagerDashboard.tssPosition.Text = dr("Role").ToString
                    frmManagerDashboard.tssUsername.Text = dr("Username").ToString
                    MsgBox("Hello, Manager " & firstname & "!", vbInformation, "Successful Login!")
                    frmPOS.lblCashier.Text = dr("Firstname").ToString & " " & dr("Lastname").ToString
                    ActLogs("Logged In")
                    frmManagerDashboard.Show()
                    Me.Hide()
                Case "Cashier"
                    MsgBox("Hello, Cashier " & firstname & "!", vbInformation, "Successful Login!")
                    frmPOS.lblCashier.Text = dr("Firstname").ToString & " " & dr("Lastname").ToString
                    ActLogs("Logged In")
                    frmPOS.Show()
                    Me.Hide()
            End Select
        End If
    End Sub
End Class
