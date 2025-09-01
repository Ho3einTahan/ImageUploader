using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ImageUploader
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=localhost;Database=Uploader;Uid=root;Pwd=;SslMode=none;AllowPublicKeyRetrieval=True;";

        public Form1()
        {
            InitializeComponent();
            LoadImages(); // وقتی فرم باز میشه لیست عکس‌ها رو لود کن
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                lblPath.Text = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void ClearForm()
        {
            lblPath.Text = "No file selected";
            pictureBox1.Image = null;
        }

        private void LoadImages()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT ImageId, ImageName, UploadDate FROM Image ORDER BY UploadDate DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView1.DataSource = table;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading images: {ex.Message}");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells["ImageId"].Value != null)
            {
                int imageId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ImageId"].Value);

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        string query = "SELECT ImageData FROM Image WHERE ImageId = @ImageId";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ImageId", imageId);

                            connection.Open();
                            var reader = command.ExecuteReader();

                            if (reader.Read())
                            {
                                byte[] imageData = (byte[])reader["ImageData"];
                                using (var ms = new MemoryStream(imageData))
                                {
                                    pictureBox1.Image = Image.FromStream(ms);
                                }
                            }

                            reader.Close();
                            connection.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}");
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblPath.Text) || lblPath.Text == "No file selected")
            {
                MessageBox.Show("Please select an image first.");
                return;
            }

            try
            {
                byte[] imageData = File.ReadAllBytes(lblPath.Text);

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Image (ImageName, ImageData) VALUES (@ImageName, @ImageData)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.Add("@ImageName", MySqlDbType.VarChar).Value = Path.GetFileName(lblPath.Text);
                        command.Parameters.Add("@ImageData", MySqlDbType.LongBlob).Value = imageData;

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                MessageBox.Show("Image uploaded successfully!");
                ClearForm();
                LoadImages(); // بعد از آپلود لیست رو رفرش کن
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading image: {ex.Message}");
            }
        }
    }
}
