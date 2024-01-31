Imports System.Data.OleDb
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Public Class frmPOS
    Dim Lview As ListViewItem
    Dim Amount As Double
    Private Sub frmPOS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Connection()
        getTransactionNum()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblDate.Text = Now.ToShortDateString
        lblTime.Text = Now.ToShortTimeString
    End Sub
    Private Sub getTransactionNum()
        sql = "Select TransNo From tblTransactions Order by TransNo desc"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        If dr.Read = True Then
            lblTransNum.Text = Val(dr(0)) + 1
        Else
            lblTransNum.Text = 1000001
        End If
    End Sub
    Private Sub txtCode_TextChanged(sender As Object, e As EventArgs) Handles txtcode.TextChanged
        sql = "Select ProdName,Amount,Quantity,CriticalLevel,Status from qryProducts where ProductCode='" & txtcode.Text & "' and Quantity>'0'"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader
        If dr.Read = True Then
            txtProdName.Text = dr("ProdName")
            txtAmt.Text = dr("Amount")
            txtQty.Text = dr("Quantity")
            txtCrit.Text = dr("CriticalLevel")
            txtStatus.Text = dr("Status")
        Else
            MsgBox("Item not found or Item is out of stock", MsgBoxStyle.Critical)
            clear()
        End If
    End Sub
    Private Sub clear()
        txtProdName.Clear()
        txtAmt.Clear()
        txtCrit.Clear()
        txtQty.Clear()
        txtStatus.Text = "******"
    End Sub
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim a As String = InputBox("Enter number of products?", "Quantity")
        If a = "" Or a = 0 Then
            MsgBox("Please enter number of products")
        Else
            If Val(a) > Val(txtQty.Text) Then
                MsgBox("Number of products is greater than the available products", MsgBoxStyle.Exclamation, "Re-enter number of products")
            Else
                txtQty.Text = Val(txtQty.Text) - Val(a)
                Amount = Val(txtAmt.Text) * Val(a)
                Lview = Me.ListView1.Items.Add(txtcode.Text)
                Lview.SubItems.Add(txtProdName.Text)
                Lview.SubItems.Add(txtAmt.Text)
                Lview.SubItems.Add(a)
                Lview.SubItems.Add(Amount)
                If Val(txtQty.Text) = 0 Then
                    txtStatus.Text = "OUT OF STOCK"
                ElseIf Val(txtQty.Text) <= Val(txtCrit.Text) Then
                    txtStatus.Text = "CRITICAL LEVEL"
                End If
            End If
        End If
        Total()
        TotalItem()
        getVAT()
        clear()
    End Sub
    Private Sub Total()
        Const col As Integer = 4
        Dim total As Integer
        Dim lvsi As ListViewItem.ListViewSubItem
        For i As Integer = 0 To ListView1.Items.Count - 1
            lvsi = ListView1.Items(i).SubItems(col)
            total += Double.Parse(lvsi.Text)
        Next
        lblTotal.Text = Format(Val(total), "0.00")
    End Sub
    Private Sub TotalItem()
        Const col As Integer = 3
        Dim total As Integer
        Dim lvsi As ListViewItem.ListViewSubItem
        For i As Integer = 0 To ListView1.Items.Count - 1
            lvsi = ListView1.Items(i).SubItems(col)
            total += Double.Parse(lvsi.Text)
        Next
        lblTotalItems.Text = Val(total)
    End Sub
    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If MsgBox("Are your sure you want to Remove Product?", vbQuestion + vbYesNo) = vbYes Then
            If ListView1.Items.Count = 0 Then
                MsgBox("No Products on the list", MsgBoxStyle.Critical)
            Else
                If ListView1.SelectedItems.Count > 0 Then
                    Dim lvalue As Integer = Integer.Parse(ListView1.SelectedItems(0).SubItems(3).Text)
                    Dim newQty As Integer = lvalue + Val(txtQty.Text)
                    txtQty.Text = newQty
                    ListView1.Items.Remove(ListView1.FocusedItem)
                    If Val(txtQty.Text) > Val(txtCrit.Text) Then
                        txtStatus.Text = "AVAILABLE"
                    End If
                    TotalItem()
                    Total()
                    getVAT()
                End If
            End If
        End If
    End Sub
    Private Sub btnPayment_Click(sender As Object, e As EventArgs) Handles btnPayment.Click
        frmPayments.Show()
        frmPayments.lblGrandTotal.Text = Me.lblTotal.Text
    End Sub
    Private Sub getVAT()
        Dim VatableSales As Double
        VatableSales = lblTotal.Text / 1.12
        lblVatAmount.Text = Format(Val(VatableSales), "0.00")
        lblVatSales.Text = Val(lblTotal.Text) - Val(lblVatAmount.Text)
    End Sub
    Private Sub cbDiscType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbDiscType.SelectedIndexChanged
        Dim Discount, NewTotal As Double
        Discount = Val(lblTotal.Text) * 0.2
        lblDiscount.Text = Format(Val(Discount), "0.00")
        NewTotal = Val(lblTotal.Text) - Val(lblDiscount.Text)
        lblTotal.Text = Format(Val(NewTotal), "0.00")
        lblVatAmount.Text = "0.00"
        lblVatSales.Text = "0.00"
    End Sub
    Private Sub SaveTransact()
        sql = "Insert into tblTransactions(TransNo,TransDate,TransTime,TotalAmount,VAT,VatableSales,Discount,DiscountType,CashierName)values(@TransNo,@TransDate,@TransTime,@TotalAmount,@VAT,@VatableSales,@Discount,@DiscountType,@CashierName)"
        cmd = New OleDbCommand(sql, cn)
        With cmd
            .Parameters.AddWithValue("@TransNo", lblTransNum.Text)
            .Parameters.AddWithValue("@TransDate", lblDate.Text)
            .Parameters.AddWithValue("@TransTime", lblTime.Text)
            .Parameters.AddWithValue("@TotalAmount", lblTotal.Text)
            .Parameters.AddWithValue("@VAT", lblVatAmount.Text)
            .Parameters.AddWithValue("@VatableSales", lblVatSales.Text)
            .Parameters.AddWithValue("@Discount", lblDiscount.Text)
            .Parameters.AddWithValue("@DiscountType", cbDiscType.Text)
            .Parameters.AddWithValue("@CashierName", lblCashier.Text)
            .ExecuteNonQuery()
        End With
        For Each i As ListViewItem In ListView1.Items
            Dim updatedQty As Integer = Val(i.SubItems(3).Text)
            Dim productCode As String = i.Text
            UpdateProduct(productCode, updatedQty)
        Next
        For Each i As ListViewItem In ListView1.Items
            sql = "Insert into tblTransactionDetails(TransNo,ProductCode,Amount,Qty,Total)Values(@TransNo,@ProductCode,@Amount,@Qty,@Total)"
            cmd = New OleDbCommand(sql, cn)
            With cmd
                .Parameters.AddWithValue("@TransNo", lblTransNum.Text)
                .Parameters.AddWithValue("@ProductCode", i.Text)
                .Parameters.AddWithValue("@Amount", i.SubItems(2).Text)
                .Parameters.AddWithValue("@Qty", i.SubItems(3).Text)
                .Parameters.AddWithValue("@Total", i.SubItems(4).Text)
                .ExecuteNonQuery()
            End With
        Next
        sql = "Insert into tblPayments(TransNo,TotalAmount,AmountPaid,AmountChange,MOP,RefNo)Values(@TransNo,@TotalAmount,@AmountPaid,@AmountChange,@MOP,@RefNo)"
        cmd = New OleDbCommand(sql, cn)
        With cmd
            .Parameters.AddWithValue("@TransNo", lblTransNum.Text)
            .Parameters.AddWithValue("@TotalAmount", lblTotal.Text)
            .Parameters.AddWithValue("@AmountPaid", lblAmountPaid.Text)
            .Parameters.AddWithValue("@AmountChange", lblChange.Text)
            .Parameters.AddWithValue("@MOP", lblMOP.Text)
            .Parameters.AddWithValue("@RefNo", lblRefNo.Text)
            .ExecuteNonQuery()
        End With
        MsgBox("Transaction Successfully Saved", MsgBoxStyle.Information)
        ActLogs("Saved New Transaction")
        ListView1.Items.Clear()
        ResetCurrency()
        getTransactionNum()
    End Sub
    Private Sub UpdateProduct(productCode As String, updatedQty As Integer)

        sql = "UPDATE tblProducts SET Quantity=Quantity - @UpdatedQty WHERE ProductCode = @ProductCode"
        cmd = New OleDbCommand(sql, cn)
        With cmd
            .Parameters.AddWithValue("@UpdatedQty", updatedQty)
            .Parameters.AddWithValue("@ProductCode", productCode)
            .ExecuteNonQuery()
        End With
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If MsgBox("Save Transaction?", vbQuestion + vbYesNo) = vbYes Then
            If Val(lblAmountPaid.Text) < Val(lblTotal.Text) Then
                MsgBox("Insufficient Amount", MsgBoxStyle.Critical, "Please Re-Enter Payment")
            Else
                SaveTransact()
            End If
        End If
    End Sub
    Private Sub ResetCurrency()
        lblAmountPaid.Text = "0.00"
        lblVatAmount.Text = "0.00"
        lblVatSales.Text = "0.00"
        lblTotalItems.Text = "0.00"
        lblMOP.Text = "******"
        lblDiscount.Text = "0.00"
        lblRefNo.Text = "*****"
        lblTotal.Text = "0.00"
        lblChange.Text = "0.00"
    End Sub
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Call ResetText()
        Call clear()
        ListView1.Items.Clear()
        Call ResetCurrency()
    End Sub
    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        If MsgBox("Are you sure you want to Logout?", vbQuestion + vbYesNo) = vbYes Then
            MsgBox("Thanks and Goodbye" & lblCashier.Text & " ! " & " Have a nice day!!!", MsgBoxStyle.Information, "USER LOGOUT")
            ActLogs("Logged Out")
            Me.Hide()
            Application.Restart()
        End If
    End Sub
End Class