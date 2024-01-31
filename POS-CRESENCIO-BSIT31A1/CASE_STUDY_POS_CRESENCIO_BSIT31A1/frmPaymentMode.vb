Imports System.Data.OleDb
Public Class frmPaymentMode
    Private Sub frmPaymentMode_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Connection()
        LoadPaymentMode()
    End Sub
    Private Sub LoadPaymentMode()
        sql = "Select * from tblPaymentMode"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        Dim x As ListViewItem
        ListView1.Items.Clear()
        Do While dr.Read = True
            x = New ListViewItem(dr("PaymentModeID").ToString)
            x.SubItems.Add(dr("PaymentMode").ToString)
            x.SubItems.Add(dr("Status").ToString)
            ListView1.Items.Add(x)
        Loop
    End Sub
    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            cbPMode.Text = ListView1.SelectedItems(0).SubItems(1).Text
        End If
    End Sub
    Private Sub cbPMode_TextChanged(sender As Object, e As EventArgs) Handles cbPMode.TextChanged
        sql = "Select PaymentMode, Status from tblPaymentMode where PaymentMode='" & cbPMode.Text & "'"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        If dr.Read = True Then
            cbPMode.Text = dr("PaymentMode").ToString
            cbStatus.Text = dr("Status").ToString
        End If
    End Sub
    Private Sub AddPaymentMode()
        sql = "Insert Into tblPaymentMode ([PaymentMode], [Status]) values ([@PaymentMode], [@Status])"
        cmd = New OleDbCommand(sql, cn)
        With cmd
            .Parameters.AddWithValue("[@PaymentMode]", cbPMode.Text)
            .Parameters.AddWithValue("[@Status]", cbStatus.Text)
            .ExecuteNonQuery()
        End With
        MsgBox("Payment Mode Added!", MsgBoxStyle.Information, "Payment Mode")
        ActLogs("Added Payment Mode")
        clear()
        LoadPaymentMode()
    End Sub
    Private Sub CheckPaymentMode()
        sql = "Select PaymentMode from tblPaymentMode where PaymentMode='" & cbPMode.Text & "'"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        If dr.HasRows = True Then
            MsgBox("PaymentMode already Exist", MsgBoxStyle.Exclamation)
        ElseIf cbPMode.Text = "" Or cbStatus.Text = "" Then
            MsgBox("All fields are required!", vbCritical, "Error")
            Return
        Else
            AddPaymentMode()
        End If
    End Sub
    Private Sub UpdatePaymentMode()
        sql = "Update tblPaymentMode set [Status]=@Status where [PaymentMode]=@PaymentMode"
        cmd = New OleDbCommand(sql, cn)
        With cmd
            .Parameters.AddWithValue("@Status", cbStatus.Text)
            .Parameters.AddWithValue("@PaymentMode", cbPMode.Text)
            .ExecuteNonQuery()
        End With
        MsgBox("Payment Mode Updated!", MsgBoxStyle.Information, "Payment Mode")
        ActLogs("Updated Payment Mode")
        clear()
        LoadPaymentMode()
    End Sub


    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If MsgBox("Do you want to add new payment mode?", vbQuestion + vbYesNo) = vbYes Then
            If cbPMode.Text = "" Or cbStatus.Text = "" Then
                MsgBox("Please fill all the information", MsgBoxStyle.Exclamation)
            Else
                CheckPaymentMode()
            End If
        End If
    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If MsgBox("Do you want to update payment mode?", vbQuestion + vbYesNo) = vbYes Then
            If cbPMode.Text = "" Or cbStatus.Text = "" Then
                MsgBox("Please fill all the information", MsgBoxStyle.Exclamation)
            Else
                UpdatePaymentMode()
            End If
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        clear()
    End Sub
    Private Sub clear()
        cbPMode.Text = " "
        cbStatus.Text = " "
    End Sub
End Class