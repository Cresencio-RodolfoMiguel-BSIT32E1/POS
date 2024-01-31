Imports System.Data.OleDb
Public Module ActivityLogs
    Public Sub ActLogs(Action As String)
        Call Connection()
        Dim Username As String = frmLogin.txtUsername.Text
        Dim TsDate As String = Date.Now.ToString("yyyy-MM-dd")
        Dim TsTime As String = Date.Now.ToString("HH:mm:ss")
        sql = "INSERT INTO tblActLogs (Username, [Action],[ActDate], [ActTime]) VALUES (@Username, @Action, @ActDate, @ActTime)"
        cmd = New OleDbCommand(sql, cn)
        With cmd
            .Parameters.AddWithValue("@Username", Username)
            .Parameters.AddWithValue("@Action", Action)
            .Parameters.AddWithValue("@ActDate", TsDate)
            .Parameters.AddWithValue("@ActTime", TsTime)
            .ExecuteNonQuery()
        End With
    End Sub
End Module
