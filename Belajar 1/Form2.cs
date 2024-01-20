using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;

namespace Belajar_1
{
    public partial class Form2 : Form
    {
        private SqlCommand cmd;
        private DataSet ds;
        private SqlDataAdapter da;

        Koneksi Konn = new Koneksi();
        public Form2()
        {
            InitializeComponent();
        }

        void loadTable()
        {
            SqlConnection conn = Konn.GetConn();
            try
            {
                conn.Open();
                cmd = new SqlCommand("SELECT * FROM barang", conn);
                ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "barang");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "barang";
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //dataGridView1.RowHeadersVisible = false;
                //dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    dataGridView1.Rows[r.Index].HeaderCell.Value = (r.Index + 1).ToString();
                }
                dataGridView1.Columns[3].DefaultCellStyle.Format = "C2";
                dataGridView1.Columns[3].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("id-ID");

            }
            catch (Exception G)
            {
                MessageBox.Show(G.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        void autoID()
        {
            try
            {
                SqlConnection conn = Konn.GetConn();
                cmd = new SqlCommand("SELECT MAX(kodebarang) FROM barang", conn);
                conn.Open();
                var currentID = cmd.ExecuteScalar() as string;

                if (currentID == null)
                {
                    textBox1.Text = "KD001";
                }
                else
                {
                    int intval = int.Parse(currentID.Substring(2, 3));
                    intval++;
                    textBox1.Text = String.Format("KD{0:000}", intval);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
        }

        void clearText()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            loadTable();
            clearText();
            autoID();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["kodebarang"].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["namabarang"].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["satuan"].Value.ToString();
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["harga"].Value.ToString();
            textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["stok"].Value.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "" || textBox5.Text.Trim() == "")
            {
                MessageBox.Show("Data Belum Lengkap!");
            }
            else
            {
                SqlConnection conn = Konn.GetConn();
                try
                {
                    cmd = new SqlCommand("INSERT INTO barang VALUES ('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', " + textBox4.Text + ", " + textBox5.Text + ")", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data Berhasil Ditambah!");
                    loadTable();
                    clearText();
                    autoID();
                }
                catch (Exception X)
                {
                    MessageBox.Show(X.ToString());
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "" || textBox5.Text.Trim() == "")
            {
                MessageBox.Show("Data Belum Lengkap!");
            }
            else
            {
                SqlConnection conn = Konn.GetConn();
                cmd = new SqlCommand("SELECT * FROM barang WHERE kodebarang = '" + textBox1.Text + "'", conn);
                conn.Open();
                SqlDataReader read = cmd.ExecuteReader();
                try
                {
                    if (read.Read())
                    {
                        DialogResult dr = MessageBox.Show("Data " + textBox1.Text + " Akan Diubah.\n\nData Sebelumnya\nNama Barang = " + read["namabarang"].ToString() + "\nSatuan = " + read["satuan"].ToString() + "\nHarga = " + read["harga"].ToString() + "\nStok = " + read["stok"].ToString() + "\n\nData Saat ini\nNama Barang = " + textBox2.Text + "\nSatuan = " + textBox3.Text + "\nHarga = " + textBox4.Text + "\nStok = " + textBox5.Text + "\n\nIngin Melanjutkan?", "Konfirmasi", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            cmd = new SqlCommand("UPDATE barang SET namabarang = '" + textBox2.Text + "', satuan = '" + textBox3.Text + "', harga = " + textBox4.Text + ", stok = " + textBox5.Text + " WHERE kodebarang = '" + textBox1.Text + "'", conn);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Data Berhasil Diubah!");
                            conn.Close();
                            loadTable();
                            clearText();
                            autoID();
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception X)
                {
                    MessageBox.Show(X.ToString());
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "" || textBox5.Text.Trim() == "")
            {
                MessageBox.Show("Data Belum Lengkap!");
            }
            else
            {
                SqlConnection conn = Konn.GetConn();
                try
                {
                    DialogResult dr = MessageBox.Show("Data " + textBox1.Text + " Akan Dihapus.\n\nIngin Melanjutkan?", "Konfirmasi",MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        cmd = new SqlCommand("DELETE FROM barang WHERE kodebarang = '" + textBox1.Text + "'", conn);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data Berhasil Dihapus!");
                        loadTable();
                        clearText();
                        autoID();
                    }
                    else
                    {

                    }                    
                }
                catch (Exception X)
                {
                    MessageBox.Show(X.ToString());
                }
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;
        }
    }
}
