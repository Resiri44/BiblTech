using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class Issues : Form
    {
        private const string сonnectionString = @"Data Source=adclg1; Initial catalog=Nefedkin419/4_UP; Integrated Security=True";
        private Form mainForm;
        private string userRole;
        public Issues(Form main, string role)
        {
            InitializeComponent();
            LoadIssues();
            mainForm = main;
            userRole = role;
            ConfigureAccess();
            txtIdUser.Tag = "Id пользователя";
            txtIdBook.Tag = "Id книги";
            txtDataIs.Tag = "Дата выдачи";
            txtDataRe.Tag = "Срок возврата";
            txtStatus.Tag = "Статус";
            InitializePlaceholders();
        }
        private void InitializePlaceholders()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textBox && textBox.Tag != null)
                {
                    textBox.Text = textBox.Tag.ToString();
                    textBox.ForeColor = Color.Gray;
                }
            }
        }
        private void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Tag != null && textBox.Text == textBox.Tag.ToString())
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Tag != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Tag.ToString();
                textBox.ForeColor = Color.Gray;
            }
        }
        private void ConfigureAccess()
        {
            if (userRole == "Читатель")
            {
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
        }
        private void LoadIssues()
        {
            DatabaseHelper db = new DatabaseHelper();
            string query = "SELECT * FROM Issues";
            DataTable Issues = db.ExecuteQuery(query);
            dataGridView1.DataSource = Issues;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mainForm.Show(); // Показать главную форму
            this.Hide();    // Закрыть текущую форму
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string iduser = txtIdUser.Text.Trim();
            string idbook = txtIdBook.Text.Trim();
            DateTime datais = DateTime.Parse(txtDataIs.Text.Trim());
            DateTime datare = DateTime.Parse(txtDataRe.Text.Trim());
            string status = txtStatus.Text.Trim();

            string query = "INSERT INTO Issues (IDПользователя, IDКниги, ДатаВыдачи, СрокВозврата, Статус) " +
                           "VALUES (@IdUser, @IdBook, @DataIs, @DataRe, @Status)";
            using (SqlConnection conn = new SqlConnection(сonnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdUser", iduser);
                    cmd.Parameters.AddWithValue("@IdBook", idbook);
                    cmd.Parameters.AddWithValue("@DataIs", datais);
                    cmd.Parameters.AddWithValue("@DataRe", datare);
                    cmd.Parameters.AddWithValue("@Status", status);

                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Выдача успешно добавлена.");
            LoadIssues(); // Обновление данных в DataGridView
            ClearFields(); // Очистка текстовых полей
            InitializePlaceholders();
        }
        private void ClearFields()
        {
            txtIdUser.Clear();
            txtIdBook.Clear();
            txtDataIs.Clear();
            txtDataRe.Clear();
            txtStatus.Clear();
        }
        private void dataGridViewIssues_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                txtIdUser.Text = dataGridView1.SelectedRows[0].Cells["IDПользователя"].Value.ToString();
                txtIdBook.Text = dataGridView1.SelectedRows[0].Cells["IDКниги"].Value.ToString();
                txtDataIs.Text = dataGridView1.SelectedRows[0].Cells["ДатаВыдачи"].Value.ToString();
                txtDataRe.Text = dataGridView1.SelectedRows[0].Cells["СрокВозврата"].Value.ToString();
                txtStatus.Text = dataGridView1.SelectedRows[0].Cells["Статус"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для редактирования.");
                return;
            }
            // Получение ID выбранной записи
            int id = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;
            string iduser = txtIdUser.Text.Trim();
            string idbook = txtIdBook.Text.Trim();
            DateTime datais = DateTime.Parse(txtDataIs.Text.Trim());
            DateTime datare = DateTime.Parse(txtDataRe.Text.Trim());
            string status = txtStatus.Text.Trim();

            string query = "UPDATE Issues SET IDПользователя = @IdUser, IDКниги = @IdBook, ДатаВыдачи = @DataIs, СрокВозврата = @DataRe, " +
                           "Статус = @Status WHERE ID = @ID";
            using (SqlConnection conn = new SqlConnection(сonnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@IdUser", iduser);
                    cmd.Parameters.AddWithValue("@IdBook", idbook);
                    cmd.Parameters.AddWithValue("@DataIs", datais);
                    cmd.Parameters.AddWithValue("@DataRe", datare);
                    cmd.Parameters.AddWithValue("@Status", status);

                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Данные о выдаче успешно обновлены.");
            LoadIssues(); // Обновление данных в DataGridView
            ClearFields(); // Очистка текстовых полей
            InitializePlaceholders();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите выдачу для удаления.");
                return;
            }
            // Получение ID выбранной записи
            int id = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

            // SQL-запрос для удаления записи
            string query = "DELETE FROM Issues WHERE ID = @ID";

            using (SqlConnection conn = new SqlConnection(сonnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Выдача успешно удалена.");
            LoadIssues(); // Обновление данных в DataGridView
            ClearFields(); // Очистка текстовых полей
            InitializePlaceholders();
        }

        private void Issues_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Close();
        }
    }
}
