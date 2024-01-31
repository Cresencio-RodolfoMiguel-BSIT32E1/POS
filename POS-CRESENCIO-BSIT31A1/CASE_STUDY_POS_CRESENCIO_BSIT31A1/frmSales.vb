Imports System.Data.OleDb
Public Class frmSales
    Private Sub frmSales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Connection()
        LoadSales()
    End Sub
    Private Sub LoadSales()
        sql = "Select TransNo, TransDate, TransTime, TotalAmount, VAT, VatableSales, Discount, DiscountType, AmountPaid, AmountChange, MOP, RefNo, CashierName from qrySales order by TransNo asc"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        Dim x As ListViewItem
        ListView1.Items.Clear()
        Do While dr.Read = True
            x = New ListViewItem(dr("TransNo").ToString)
            x.SubItems.Add(dr("TransDate").ToString)
            x.SubItems.Add(dr("TransTime").ToString)
            x.SubItems.Add(dr("TotalAmount").ToString)
            x.SubItems.Add(dr("VAT").ToString)
            x.SubItems.Add(dr("VatableSales").ToString)
            x.SubItems.Add(dr("Discount").ToString)
            x.SubItems.Add(dr("DiscountType").ToString)
            x.SubItems.Add(dr("AmountPaid").ToString)
            x.SubItems.Add(dr("AmountChange").ToString)
            x.SubItems.Add(dr("MOP").ToString)
            x.SubItems.Add(dr("RefNo").ToString)
            x.SubItems.Add(dr("CashierName").ToString)
            ListView1.Items.Add(x)
        Loop
        Total()
    End Sub
    Private Sub Total()
        Const col As Integer = 3
        Dim total As Integer
        Dim lvsi As ListViewItem.ListViewSubItem
        For i As Integer = 0 To ListView1.Items.Count - 1
            lvsi = ListView1.Items(i).SubItems(col)
            total += Double.Parse(lvsi.Text)
        Next
        lblTotal.Text = Format(Val(total), "0.00")
    End Sub
    Private Sub ListView1_MouseClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseClick
        If ListView1.Items.Count > 0 Then
            Dim i As ListViewItem = ListView1.SelectedItems(0)
            frmSalesDetails.lblTransNo.Text = i.SubItems(0).Text
            frmSalesDetails.Show()
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class