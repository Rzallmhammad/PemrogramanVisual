Imports System.Data.OleDb

Public Class Form1
    Dim conn As New OleDbConnection
    Dim cmd As OleDbCommand
    Dim dt As New DataTable
    Dim da As New OleDbDataAdapter()

    ' Procedure untuk menampilkan data pada DataGridView
    Private Sub viewer()
        conn.Open()
        cmd = conn.CreateCommand()
        cmd.CommandType = CommandType.Text
        da = New OleDbDataAdapter("select * from vbsavedata", conn)
        da.Fill(dt)
        DataGridView1.DataSource = dt
        conn.Close()
    End Sub

    ' Procedure yang dijalankan saat form dimuat
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Mengatur koneksi ke basis data MS Access
        conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\belajar\AbsensiMahasiswa\AbsensiMahasiswa\bin\Debug\vbsavedata.accdb"
        ' Menampilkan data pada DataGridView
        viewer()
    End Sub

    ' Procedure yang dijalankan saat tombol "Save" ditekan
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            conn.Open()
            cmd = conn.CreateCommand()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "INSERT INTO vbsavedata (NIM, Nama, Prodi) VALUES ('" + txtNIM.Text + "','" + txtNama.Text + "', '" + txtProdi.Text + "')"
            cmd.ExecuteNonQuery()
            conn.Close()
            MessageBox.Show("Record Saved MS Access", "Vb Save Database", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Membersihkan dan mengisi kembali DataTable
            dt.Clear()
            da.Fill(dt)
            ' Merefresh DataGridView
            DataGridView1.Refresh()

            ' Mengosongkan TextBox
            txtNIM.Text = ""
            txtNama.Text = ""
            txtProdi.Text = ""

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Vb Save Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
            conn.Close()
        End Try
    End Sub

    ' Procedure yang dijalankan saat sel pada DataGridView diklik
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            txtNIM.Text = DataGridView1.SelectedRows(0).Cells(0).Value.ToString()
            txtNama.Text = DataGridView1.SelectedRows(0).Cells(1).Value.ToString()
            txtProdi.Text = DataGridView1.SelectedRows(0).Cells(2).Value.ToString()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Vb Save Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
            conn.Close()
        End Try
    End Sub

    ' Procedure yang dijalankan saat tombol "Delete" ditekan
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            ' Validasi NIM sebagai angka sebelum menghapus
            If Not Integer.TryParse(txtNIM.Text, Nothing) Then
                MessageBox.Show("NIM harus berupa nilai numerik.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            conn.Open()
            cmd = conn.CreateCommand()
            cmd.CommandType = CommandType.Text

            ' Menggunakan parameterized query untuk mencegah SQL Injection
            cmd.CommandText = "DELETE FROM vbsavedata WHERE NIM = @NIM"
            cmd.Parameters.AddWithValue("@NIM", CInt(txtNIM.Text)) ' Konversi ke tipe data yang sesuai

            cmd.ExecuteNonQuery()
            conn.Close()

            MessageBox.Show("Record Successfully Deleted", "Vb Save Database", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Membersihkan dan mengisi kembali DataTable
            dt.Clear()
            da.Fill(dt)
            ' Merefresh DataGridView
            DataGridView1.Refresh()

            ' Mengosongkan TextBox
            txtNIM.Text = ""
            txtNama.Text = ""
            txtProdi.Text = ""

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Vb Save Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
            conn.Close()
        End Try
    End Sub

    ' Procedure yang dijalankan saat tombol "Exit" ditekan
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Dim iExit As DialogResult

        iExit = MessageBox.Show("confirm if you want to exit", "Vb Save Database", MessageBoxButtons.YesNo, MessageBoxIcon.Information)

        If iExit = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub
End Class
