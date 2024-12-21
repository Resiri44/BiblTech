using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class Request : Form
    {
        private const string сonnectionString = @"Data Source=adclg1; Initial catalog=Nefedkin419/4_UP; Integrated Security=True";
        private Form mainForm;
        public Request(Form main)
        {
            InitializeComponent();
            LoadRequest();
            mainForm = main;
            txtIdUser.Tag = "Id пользователя";
            txtIdBook.Tag = "Id книги";
            txtDataReq.Tag = "Дата запроса";
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
        private void LoadRequest()
        {
            DatabaseHelper db = new DatabaseHelper();
            string query = "SELECT * FROM Requests";
            DataTable Request = db.ExecuteQuery(query);
            dataGridView1.DataSource = Request;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mainForm.Show(); // Показать главную форму
            this.Hide();    // Закрыть текущую форму
        }
        private void ClearFields()
        {
            txtIdUser.Clear();
            txtDataReq.Clear();
            txtIdBook.Clear();
            txtStatus.Clear();
        }
        private void dataGridViewIssues_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                txtIdUser.Text = dataGridView1.SelectedRows[0].Cells["IDПользователя"].Value.ToString();
                txtIdBook.Text = dataGridView1.SelectedRows[0].Cells["IDКниги"].Value.ToString();
                txtDataReq.Text = dataGridView1.SelectedRows[0].Cells["ДатаЗапроса"].Value.ToString();
                txtStatus.Text = dataGridView1.SelectedRows[0].Cells["Статус"].Value.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Получение данных из текстовых полей
                if (!int.TryParse(txtIdUser.Text.Trim(), out int IDuser))
                {
                    MessageBox.Show("Введите корректный ID пользователя (число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!decimal.TryParse(txtIdBook.Text.Trim(), out decimal IDbook))
                {
                    MessageBox.Show("Введите корректный ID книги (число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(txtDataReq.Text.Trim(), out DateTime requestDate))
                {
                    MessageBox.Show("Введите корректную дату запроса.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string status = txtStatus.Text.Trim();

                if (string.IsNullOrEmpty(status))
                {
                    MessageBox.Show("Введите статус запроса.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // SQL-запрос для добавления записи
                string query = "INSERT INTO Requests (IDПользователя, IDКниги, ДатаЗапроса, Статус) VALUES (@IDUser, @IDBook, @DataReq, @Status)";

                using (SqlConnection conn = new SqlConnection(сonnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDUser", IDuser);
                        cmd.Parameters.AddWithValue("@IDBook", IDbook);
                        cmd.Parameters.AddWithValue("@DataReq", requestDate);
                        cmd.Parameters.AddWithValue("@Status", status);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запрос успешно добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Очистка полей после добавления
                ClearFields();
                LoadRequest();
                InitializePlaceholders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запросе: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка, выбрана ли строка в DataGridView
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите запись для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Получение ID запроса из выбранной строки
                int requestID = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

                // Получение данных из текстовых полей
                if (!int.TryParse(txtIdUser.Text.Trim(), out int userID))
                {
                    MessageBox.Show("Введите корректный ID пользователя (число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txtIdBook.Text.Trim(), out int bookID))
                {
                    MessageBox.Show("Введите корректный ID книги (число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(txtDataReq.Text.Trim(), out DateTime requestDate))
                {
                    MessageBox.Show("Введите корректную дату запроса.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string status = txtStatus.Text.Trim();

                // Проверка, что статус не пустой
                if (string.IsNullOrEmpty(status))
                {
                    MessageBox.Show("Введите статус запроса.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // SQL-запрос для обновления записи
                string query = "UPDATE Requests SET IDПользователя = @UserID, IDКниги = @BookID, ДатаЗапроса = @RequestDate, Статус = @Status WHERE ID = @RequestID";

                using (SqlConnection conn = new SqlConnection(сonnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RequestID", requestID);
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@BookID", bookID);
                        cmd.Parameters.AddWithValue("@RequestDate", requestDate);
                        cmd.Parameters.AddWithValue("@Status", status);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запрос успешно обновлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Обновление таблицы
                LoadRequest();
                ClearFields();
                InitializePlaceholders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }
            // Получение ID выбранной записи
            int id = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

            // SQL-запрос для удаления записи
            string query = "DELETE FROM Requests WHERE ID = @ID";

            using (SqlConnection conn = new SqlConnection(сonnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Запрос успешно удален.");
            LoadRequest(); // Обновление данных в DataGridView
            ClearFields(); // Очистка текстовых полей
            InitializePlaceholders();
        }

        private void Request_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Close();
        }
    }
}
