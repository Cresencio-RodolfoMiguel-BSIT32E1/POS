Imports System.Data.OleDb
Module DatabaseConnection
    Public cn As New OleDbConnection
    Public cmd As OleDbCommand
    Public dr As OleDbDataReader
    Public sql As String

    Public Sub Connection()
        cn.Close()
        cn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Acer\OneDrive\Desktop\codes\POS-CRESENCIO-BSIT31A1\CASE_STUDY_POS_CRESENCIO_BSIT31A1\bin\x64\Debug\POSdb.accdb"
        cn.Open()
        'MsgBox("Connection Success")
    End Sub
End Module
