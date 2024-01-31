Imports System.Data.OleDb
Public Class frmUsers
    Private Sub frmUsers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Connection()
        LoadUsers()
    End Sub
    Private Sub LoadUsers()
        sql = "Select * from qryUsers"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        Dim x As ListViewItem
        ListView1.Items.Clear()
        Do While dr.Read = True
            x = New ListViewItem(dr("Username").ToString)
            x.SubItems.Add(dr("Firstname").ToString & " " & dr("Lastname").ToString)
            x.SubItems.Add(dr("AccessLevel").ToString)
            x.SubItems.Add(dr("Role").ToString)
            x.SubItems.Add(dr("Password").ToString)
            ListView1.Items.Add(x)
        Loop
        totalUsers()
    End Sub
    Private Sub SaveUser()
        Dim roleID As String = "R0"
        Select Case cbRole.Text
            Case "IT"
                roleID = "R1"
            Case "Manager"
                roleID = "R2"
            Case "Cashier"
                roleID = "R3"
        End Select
        sql = "Insert Into tblUsers([Username], [Lastname], [Firstname], [AccessLevel], [RoleID], [Password]) values ([@Username], [@Lastname], [@Firstname], [@AccessLevel], [@RoleID], [@Password])"
        cmd = New OleDbCommand(sql, cn)
        With cmd
            .Parameters.AddWithValue("[@Username]", txtUsername.Text)
            .Parameters.AddWithValue("[@Lastname]", txtLastname.Text)
            .Parameters.AddWithValue("[@Firstname]", txtFirstname.Text)
            .Parameters.AddWithValue("[@AccessLevel]", cbAccess.Text)
            .Parameters.AddWithValue("[@RoleID]", roleID)
            .Parameters.AddWithValue("[@Password]", txtPassword.Text)
            .ExecuteNonQuery()
        End With
        ActLogs("Added a new user")
        LoadUsers()
        clear()
    End Sub
    Private Sub UpdateUser()
        Dim roleID As String = "R0"
        Select Case cbRole.Text
            Case "IT"
                roleID = "R1"
            Case "Manager"
                roleID = "R2"
            Case "Cashier"
                roleID = "R3"
        End Select
        sql = "Update tblUsers set  Lastname=@Lastname, Firstname=@Firstname, AccessLevel=@AccessLevel, RoleID=@RoleID, Password=@Password where Username=@Username"
        cmd = New OleDbCommand(sql, cn)
        With cmd
            .Parameters.AddWithValue("[@Lastname]", txtLastname.Text)
            .Parameters.AddWithValue("[@Firstname]", txtFirstname.Text)
            .Parameters.AddWithValue("[@AccessLevel]", cbAccess.Text)
            .Parameters.AddWithValue("[@RoleID]", roleID)
            .Parameters.AddWithValue("[@Password]", txtPassword.Text)
            .Parameters.AddWithValue("[@Username]", txtUsername.Text)
            .ExecuteNonQuery()
        End With
        ActLogs("Updated user info")
        LoadUsers()
        clear()
    End Sub
    Private Sub CheckUser()
        sql = "Select Username from tblUsers where Username='" & txtUsername.Text & "'"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        If dr.HasRows = True Then
            MsgBox("User already Exist", MsgBoxStyle.Exclamation)
        ElseIf txtUsername.Text = "" Or txtPassword.Text = "" Or txtConfirmPass.Text = "" Or txtLastname.Text = "" Or txtFirstname.Text = "" Then
            MsgBox("All fields are required!", vbCritical, "Error")
            Return
        Else
            SaveUser()
        End If
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If MsgBox("Are you sure you want to add new user?", vbQuestion + vbYesNo) = vbYes Then
            If txtUsername.Text = "" Or txtPassword.Text = "" Or txtConfirmPass.Text = "" Or txtLastname.Text = "" Or txtFirstname.Text = "" Then
                MsgBox("Please fill all the information", MsgBoxStyle.Exclamation)
            ElseIf txtPassword.Text <> txtConfirmPass.Text Then
                MsgBox("Password and Confirm Password is not match!!")
            Else
                CheckUser()
            End If
        End If
    End Sub
    Private Sub clear()
        txtFirstname.Clear()
        txtLastname.Clear()
        txtPassword.Clear()
        txtSearch.Clear()
        txtUsername.Clear()
        txtConfirmPass.Clear()
        cbAccess.Text = " "
        cbRole.Text = " "
    End Sub
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        clear()
    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If MsgBox("Are you sure you want to update user info?", vbQuestion + vbYesNo) = vbYes Then
            If txtUsername.Text = "" Or txtPassword.Text = "" Or txtConfirmPass.Text = "" Or txtLastname.Text = "" Or txtFirstname.Text = "" Then
                MsgBox("Please fill all the information", MsgBoxStyle.Exclamation)
            ElseIf txtPassword.Text <> txtConfirmPass.Text Then
                MsgBox("Password and Confirm Password mismatched!!")
            Else
                UpdateUser()
            End If
        End If
    End Sub
    Private Sub txtUsername_TextChanged(sender As Object, e As EventArgs) Handles txtUsername.TextChanged
        sql = "Select Lastname, Firstname, AccessLevel, Role, Password from qryUsers where Username='" & txtUsername.Text & "'"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        If dr.Read = True Then
            txtLastname.Text = dr("Lastname").ToString
            txtFirstname.Text = dr("Firstname").ToString
            cbAccess.Text = dr("AccessLevel").ToString
            cbRole.Text = dr("Role").ToString
            txtPassword.Text = dr("Password").ToString
        End If
    End Sub
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        sql = "Select * from qryUsers where Username like '&" & txtSearch.Text & "%' or Lastname like '%" & txtSearch.Text & "%' or Firstname like '%" & txtSearch.Text & "%'"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        Dim x As ListViewItem
        ListView1.Items.Clear()
        Do While dr.Read = True
            x = New ListViewItem(dr("Username").ToString)
            x.SubItems.Add(dr("Firstname").ToString & " " & dr("Lastname").ToString)
            x.SubItems.Add(dr("AccessLevel").ToString)
            x.SubItems.Add(dr("Role").ToString)
            x.SubItems.Add(dr("Password").ToString)
            ListView1.Items.Add(x)
        Loop
        totalUsers()
    End Sub
    Private Sub totalUsers()
        lblTotal.Text = ListView1.Items.Count.ToString()
    End Sub
    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            txtUsername.Text = ListView1.SelectedItems(0).SubItems(0).Text
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub
End Class