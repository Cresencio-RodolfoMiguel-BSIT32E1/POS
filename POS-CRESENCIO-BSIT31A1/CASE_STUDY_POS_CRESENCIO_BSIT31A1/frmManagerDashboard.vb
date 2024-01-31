Public Class frmManagerDashboard
    Private Sub POSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles POSToolStripMenuItem.Click
        frmPOS.Show()
        Me.Hide()
    End Sub
    Private Sub SalesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalesToolStripMenuItem.Click
        frmSales.Show()
    End Sub
    Private Sub ProductsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProductsToolStripMenuItem.Click
        frmProducts.ShowDialog()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If MsgBox("Are you sure you want to Logout?", vbQuestion + vbYesNo) = vbYes Then
            MsgBox("Thanks and Goodbye" & tssEmpName.Text & " ! " & " Have a nice day!!!", MsgBoxStyle.Information, "USER LOGOUT")
            ActLogs("Logged Out")
            Me.Hide()
            Application.Restart()
        End If
    End Sub
End Class