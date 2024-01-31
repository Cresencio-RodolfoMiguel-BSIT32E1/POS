Imports System.Data.OleDb
Public Class frmActLogs
    Private Sub frmActLogs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Connection()
        LoadActivityLogs()
    End Sub
    Private Sub LoadActivityLogs()
        sql = "SELECT * FROM qryActLogs order by ActID desc"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        ListView1.Items.Clear()
        While dr.Read()
            Dim x As New ListViewItem(dr("Username").ToString())
            x.SubItems.Add(dr("Firstname").ToString() & " " & dr("Lastname").ToString)
            x.SubItems.Add(dr("Role").ToString())
            x.SubItems.Add(dr("Action").ToString())
            x.SubItems.Add(dr("ActDate").ToString())
            x.SubItems.Add(dr("ActTime").ToString())
            ListView1.Items.Add(x)
        End While
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmDashboard.Show()
        Me.Hide()
    End Sub
End Class