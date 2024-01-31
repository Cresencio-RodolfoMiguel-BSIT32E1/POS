Public Class frmDashboard
    Private Sub POSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles POSToolStripMenuItem.Click
        frmPOS.Show()
        Me.Hide()
    End Sub

    Private Sub ProductsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProductsToolStripMenuItem.Click
        frmProducts.Show()
    End Sub
    Private Sub UsersToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UsersToolStripMenuItem.Click
        frmUsers.Show()
    End Sub
    Private Sub ActivityLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ActivityLogsToolStripMenuItem.Click
        frmActLogs.Show()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If MsgBox("Are you sure you want to Logout?", vbQuestion + vbYesNo) = vbYes Then
            MsgBox("Thanks and Goodbye " & tssEmpName.Text & " ! " & " Have a nice day!!!", MsgBoxStyle.Information, "USER LOGOUT")
            ActLogs("Logged Out")
            Me.Hide()
            Application.Restart()
        End If
    End Sub
    Private Sub CategoriesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CategoriesToolStripMenuItem.Click
        frmCategories.Show()
    End Sub
    Private Sub PaymentModeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PaymentModeToolStripMenuItem.Click
        frmPaymentMode.Show()
    End Sub
    Private Sub SalesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalesToolStripMenuItem.Click
        frmSales.Show()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        DateTime.Text = Now.ToLongDateString & Now.ToLongTimeString
    End Sub

    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
