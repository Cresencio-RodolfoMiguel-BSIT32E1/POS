Imports System.Data.OleDb
Public Class frmPayments
    Private Sub frmPayments_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Connection()
        loadMOP()

    End Sub

    Private Sub loadMOP()
        'can be applied to our system
        sql = "Select distinct PaymentMode from tblPaymentMode where status='1'"
        cmd = New OleDbCommand(sql, cn)
        Dim da As New OleDbDataAdapter(cmd)
        Dim dt As New DataTable
        da.Fill(dt)
        cboMOP.DataSource = dt
        cboMOP.DisplayMember = "PaymentMode"

    End Sub

    Private Sub cboMOP_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMOP.SelectedIndexChanged
        If cboMOP.Text = "Cash" Then
            txtRefNo.ReadOnly = True
            txtAmountPaid.Clear()
            txtAmountPaid.Enabled = True
        Else
            txtRefNo.ReadOnly = False
            txtAmountPaid.Text = lblGrandTotal.Text
            txtChange.Text = "0.00"
            txtAmountPaid.Enabled = False

        End If
    End Sub

    Private Sub txtAmountPaid_TextChanged(sender As Object, e As EventArgs) Handles txtAmountPaid.TextChanged
        Dim change As Double
        change = Val(txtAmountPaid.Text) - Val(lblGrandTotal.Text)
        txtChange.Text = Format(Val(change), "0.00")
    End Sub

    Private Sub btnEnter_Click(sender As Object, e As EventArgs) Handles btnEnter.Click
        If Val(txtAmountPaid.Text) < Val(lblGrandTotal.Text) Then
            MsgBox("Insufficient Amount", MsgBoxStyle.Critical)
        Else
            frmPOS.lblAmountPaid.Text = Me.txtAmountPaid.Text
            frmPOS.lblChange.Text = Me.txtChange.Text
            frmPOS.lblMOP.Text = Me.cboMOP.Text
            frmPOS.lblRefNo.Text = Me.txtRefNo.Text
            Me.Hide()

        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim a As String = InputBox("Enter Mode of Payment")
        sql = "Insert Into tblPaymentMode(PaymentMode, Status) values (@PaymentMode, @Values)"
        cmd = New OleDbCommand(sql, cn)
        With cmd
            .Parameters.AddWithValue("@PaymentMode", a)
            .Parameters.AddWithValue("@Status", "1")
            .ExecuteNonQuery()

        End With
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub
End Class
