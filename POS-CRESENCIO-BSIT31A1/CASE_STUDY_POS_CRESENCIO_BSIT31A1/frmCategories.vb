Imports System.Data.OleDb
Public Class frmCategories
    Private Sub frmCategories_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Connection()
        Call LoadCategories()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub LoadCategories()
        sql = "Select * from tblCategories"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        Dim x As ListViewItem
        ListView1.Items.Clear()
        Do While dr.Read = True
            x = New ListViewItem(dr("CategoryID").ToString)
            x.SubItems.Add(dr("Category").ToString)
            ListView1.Items.Add(x)
        Loop
    End Sub
    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            txtCategID.Text = ListView1.SelectedItems(0).SubItems(0).Text
        End If
    End Sub
    Private Sub txtCategID_TextChanged(sender As Object, e As EventArgs) Handles txtCategID.TextChanged
        sql = "Select Category from tblCategories where CategoryID='" & txtCategID.Text & "'"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        If dr.Read = True Then
            txtCategory.Text = dr("Category").ToString
        End If
    End Sub
    Private Sub AddCategories()
        sql = "Insert Into tblCategories ([CategoryID], [Category]) values ([@CategoryID], [@Category])"
        cmd = New OleDbCommand(sql, cn)
        With cmd
            .Parameters.AddWithValue("[@CategoryID]", txtCategID.Text)
            .Parameters.AddWithValue("[@Category]", txtCategory.Text)
            .ExecuteNonQuery()
        End With
        MsgBox("Category Added!", MsgBoxStyle.Information, "Category")
        ActLogs("Added Category")
        clear()
        LoadCategories()
    End Sub
    Private Sub CheckCategories()
        sql = "Select CategoryID from tblCategories where CategoryID='" & txtCategID.Text & "'"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        If dr.HasRows = True Then
            MsgBox("Category already Exist", MsgBoxStyle.Exclamation)
        ElseIf txtCategID.Text = "" Or txtCategory.Text = "" Then
            MsgBox("All fields are required!", vbCritical, "Error")
            Return
        Else
            AddCategories()
        End If
    End Sub
    Private Sub UpdateCategories()
        sql = "Update tblCategories set [Category]=@Category where [CategoryID]=@CategoryID"
        cmd = New OleDbCommand(sql, cn)
        With cmd
            .Parameters.AddWithValue("@Category", txtCategory.Text)
            .Parameters.AddWithValue("@CategoryID", txtCategID.Text)
            .ExecuteNonQuery()
        End With
        MsgBox("Category Updated!", MsgBoxStyle.Information, "Category")
        ActLogs("Updated Category")
        clear()
        LoadCategories()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If MsgBox("Are you sure you want to add new category?", vbQuestion + vbYesNo) = vbYes Then
            If txtCategID.Text = "" Or txtCategory.Text = "" Then
                MsgBox("Please fill all the information", MsgBoxStyle.Exclamation)
            Else
                CheckCategories()
            End If
        End If
    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If MsgBox("Are you sure you want to update category?", vbQuestion + vbYesNo) = vbYes Then
            If txtCategID.Text = "" Or txtCategory.Text = "" Then
                MsgBox("Please fill all the information", MsgBoxStyle.Exclamation)
            Else
                UpdateCategories()
            End If
        End If
    End Sub
    Private Sub clear()
        txtCategID.Clear()
        txtCategory.Clear()
    End Sub
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        clear()
    End Sub
End Class