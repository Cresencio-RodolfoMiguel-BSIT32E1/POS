Imports System.Data.OleDb
Public Class frmSalesDetails
    Private Sub frmSalesDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Connection()
        LoadSales()
    End Sub

    Private Sub LoadSales()
        sql = "Select ProductCode, ProdName, Amount, Qty, Total from qrySalesDetails where TransNo='" & lblTransNo.Text & "'"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        Dim x As ListViewItem
        ListView1.Items.Clear()
        Do While dr.Read = True
            x = New ListViewItem(dr("ProductCode").ToString)
            x.SubItems.Add(dr("ProdName").ToString)
            x.SubItems.Add(dr("Amount").ToString)
            x.SubItems.Add(dr("Qty").ToString)
            x.SubItems.Add(dr("Total").ToString)
            ListView1.Items.Add(x)
        Loop
        Call Total()
        Call TotalItems()
    End Sub
    Private Sub Total()
        Const col As Integer = 4
        Dim total As Integer
        Dim lvsi As ListViewItem.ListViewSubItem
        For i As Integer = 0 To ListView1.Items.Count - 1
            lvsi = ListView1.Items(i).SubItems(col)
            total += Double.Parse(lvsi.Text)
        Next
        lblTotalAmnt.Text = Format(Val(total), "0.00")
    End Sub
    Private Sub TotalItems()
        Const col As Integer = 3
        Dim total As Integer
        Dim lvsi As ListViewItem.ListViewSubItem
        For i As Integer = 0 To ListView1.Items.Count - 1
            lvsi = ListView1.Items(i).SubItems(col)
            total += Double.Parse(lvsi.Text)
        Next
        lblTotalItems.Text = Val(total)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub
End Class