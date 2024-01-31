Imports System.Data.OleDb

Public Class frmProducts
    Private Sub frmProducts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Connection()
        Call LoadProducts()
        Call GetProductCode()
        Call Category()
        Call GetTotalProducts()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub GetProductCode()
        sql = "Select ProductCode from tblProducts order by ProductCode desc"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        If dr.Read = True Then
            txtPCode.Text = Val(dr(0)) + 1
        Else
            txtPCode.Text = 100000
        End If

    End Sub

    Private Sub LoadProducts()
        sql = "Select * from qryProducts"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        Dim x As ListViewItem
        ListView1.Items.Clear()
        Do While dr.Read = True
            x = New ListViewItem(dr("ProductCode").ToString)
            x.SubItems.Add(dr("ProdName").ToString)
            x.SubItems.Add(dr("ProdDescription").ToString)
            x.SubItems.Add(dr("Category").ToString)
            x.SubItems.Add(dr("Amount").ToString)
            x.SubItems.Add(dr("Quantity").ToString)
            x.SubItems.Add(dr("CriticalLevel").ToString)
            x.SubItems.Add(dr("Status").ToString)
            ListView1.Items.Add(x)
        Loop
    End Sub


    Private Sub Category()
        sql = "Select distinct Category from tblCategories"
        cmd = New OleDbCommand(sql, cn)
        Dim da As New OleDbDataAdapter(cmd)
        Dim dt As New DataTable
        da.Fill(dt)
        With cbCategory
            .DataSource = dt
            .DisplayMember = "Category"
        End With
    End Sub

    Private Sub GetTotalProducts()
        sql = "Select sum(quantity) as Total from qryProducts"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        If dr.Read = True Then
            lblTotal.Text = dr("Total")
        End If
    End Sub



    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        sql = "Select * from qryProducts where ProdName Like '%" & txtSearch.Text & "%'"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        Dim x As ListViewItem
        ListView1.Items.Clear()
        Do While dr.Read = True
            x = New ListViewItem(dr("ProductCode").ToString)
            x.SubItems.Add(dr("ProdName").ToString)
            x.SubItems.Add(dr("ProdDescription").ToString)
            x.SubItems.Add(dr("Category").ToString)
            x.SubItems.Add((dr("Amount").ToString))
            x.SubItems.Add(dr("Quantity").ToString)
            x.SubItems.Add(dr("CriticalLevel").ToString)
            x.SubItems.Add(dr("Status").ToString)
            ListView1.Items.Add(x)

        Loop
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            txtPCode.Text = ListView1.SelectedItems(0).SubItems(0).Text
            txtPName.Text = ListView1.SelectedItems(0).SubItems(1).Text
            txtPDesc.Text = ListView1.SelectedItems(0).SubItems(2).Text
            cbCategory.Text = ListView1.SelectedItems(0).SubItems(3).Text
            txtAmount.Text = ListView1.SelectedItems(0).SubItems(4).Text
            txtQty.Text = ListView1.SelectedItems(0).SubItems(5).Text
            txtCritical.Text = ListView1.SelectedItems(0).SubItems(6).Text
            cbStatus.Text = ListView1.SelectedItems(0).SubItems(7).Text
            cbStatus.Text = ListView1.SelectedItems(0).SubItems(7).Text

        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs)
        clear()
        GetProductCode()
    End Sub
    Private Sub AddProducts()
        Dim CategoryID As String = "C0"

        Select Case cbCategory.Text
                Case "Coffee"
                    CategoryID = "C1"
                Case "Frappe"
                    CategoryID = "C2"
                Case "Milktea"
                    CategoryID = "C3"
                Case "Pastries"
                    CategoryID = "C4"
            End Select
        sql = "Insert into tblProducts ([ProductCode],[ProdName],[ProdDescription],[CategoryID],[Amount],[Quantity],[CriticalLevel],[Status])values([@ProductCode],[@ProdName],[@ProdDescription],[@CategoryID],[@Amount],[@Quantity],[@CriticalLevel],[@Status])"
        cmd = New OleDbCommand(sql, cn)
        With cmd
            .Parameters.AddWithValue("[@ProductCode]", txtPCode.Text)
            .Parameters.AddWithValue("[@ProdName]", txtPName.Text)
            .Parameters.AddWithValue("[@ProdDescription]", txtPDesc.Text)
            .Parameters.AddWithValue("[@CategoryID]", CategoryID)
            .Parameters.AddWithValue("[@Amount]", txtAmount.Text)
            .Parameters.AddWithValue("[@Quantity]", txtQty.Text)
            .Parameters.AddWithValue("[@CriticalLevel]", txtCritical.Text)
            .Parameters.AddWithValue("[@Status", cbStatus.Text)
            .ExecuteNonQuery()

        End With
        MsgBox("New Product Successfully Save!!", MsgBoxStyle.Information)
        ActLogs("Added a new product")
        clear()
        LoadProducts()
        GetProductCode()
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If MsgBox("Do you want to add new product?", vbQuestion + vbYesNo) = vbYes Then
            If txtPCode.Text = "" Or txtPName.Text = "" Then
                MsgBox("Please complete the product information", MsgBoxStyle.Exclamation)
            Else
                AddProducts()
            End If
        End If

    End Sub
    Private Sub UpdateProducts()
        Dim CategoryID As String = "C0"
        Select Case cbCategory.Text
            Case "Coffee"
                CategoryID = "C1"
            Case "Frappe"
                CategoryID = "C2"
            Case "Milktea"
                CategoryID = "C3"
            Case "Pastries"
                CategoryID = "C4"
        End Select
        sql = "Update tblProducts Set [ProdName]=[@ProdName],[ProdDescription]=[@ProdDescription],[CategoryID]=[@CategoryID],[Amount]=[@Amount],[Quantity]=[@Quantity],[CriticalLevel]=[@CriticalLevel],[Status]=[@Status] where [ProductCode]=[@ProductCode]"
        cmd = New OleDbCommand(sql, cn)
        With cmd
            .Parameters.AddWithValue("[@ProdName]", txtPName.Text)
            .Parameters.AddWithValue("[@ProdDescription]", txtPDesc.Text)
            .Parameters.AddWithValue("[@CategoryID]", CategoryID)
            .Parameters.AddWithValue("[@Amount]", txtAmount.Text)
            .Parameters.AddWithValue("[@Quantity]", txtQty.Text)
            .Parameters.AddWithValue("[@CriticalLevel]", txtCritical.Text)
            .Parameters.AddWithValue("[@Status", cbStatus.Text)
            .Parameters.AddWithValue("[@ProductCode]", txtPCode.Text)
            .ExecuteNonQuery()

        End With
        MsgBox("Product Successfully Updated!!", MsgBoxStyle.Information)
        ActLogs("Updated a product")
        clear()
        LoadProducts()
        GetProductCode()
    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If MsgBox("Do you want to update the product?", vbQuestion + vbYesNo) = vbYes Then
            If txtPCode.Text = "" Or txtPName.Text = "" Then
                MsgBox("Please complete the product information", MsgBoxStyle.Exclamation)
            Else
                UpdateProducts()
            End If
        End If

    End Sub
    Private Sub clear()
        txtPName.Clear()
        txtPDesc.Clear()
        txtAmount.Clear()
        txtCritical.Clear()
        txtQty.Clear()
        GetProductCode()
    End Sub
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        clear()
    End Sub
End Class