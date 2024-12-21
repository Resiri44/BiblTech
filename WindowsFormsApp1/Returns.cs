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
    public partial class Returns : Form
    {
        private const string сonnectionString = @"Data Source=adclg1; Initial catalog=Nefedkin419/4_UP; Integrated Security=True";
        private Form mainForm;
        private string userRole;
        public Returns(Form main, string role)
        {
            InitializeComponent();
            LoadReturns();
            mainForm = main;
            userRole = role;
            ConfigureAccess();
            txtIdIssue.Tag = "Id выдачи";
            txtDataRe.Tag = "Дата возврата";
            txtFine.Tag = "Штраф";
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
        private void LoadReturns()
        {
            DatabaseHelper db = new DatabaseHelper();
            string query = "SELECT * FROM Returns";
            DataTable Returns = db.ExecuteQuery(query);
            dataGridView1.DataSource = Returns;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mainForm.Show(); // Показать главную форму
            this.Hide();    // Закрыть текущую форму
        }
        private void ClearFields()
        {
            txtIdIssue.Clear();
            txtDataRe.Clear();
            txtFine.Clear();
        }
        private void dataGridViewIssues_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                txtIdIssue.Text = dataGridView1.SelectedRows[0].Cells["IDВыдачи"].Value.ToString();
                txtDataRe.Text = dataGridView1.SelectedRows[0].Cells["ДатаВозврата"].Value.ToString();
                txtFine.Text = dataGridView1.SelectedRows[0].Cells["Штраф"].Value.ToString();
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

                    // Получение ID возврата из выбранной строки
                    int returnID = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

                    // Получение данных из текстовых полей
                    if (!int.TryParse(txtIdIssue.Text.Trim(), out int issueID))
                    {
                        MessageBox.Show("Введите корректный ID выдачи (число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!decimal.TryParse(txtFine.Text.Trim(), out decimal fine))
                    {
                        MessageBox.Show("Введите корректный штраф (число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!DateTime.TryParse(txtDataRe.Text.Trim(), out DateTime returnDate))
                    {
                        MessageBox.Show("Введите корректную дату возврата.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // SQL-запрос для обновления записи
                    string query = "UPDATE Returns SET IDВыдачи = @IssueID, ДатаВозврата = @ReturnDate, Штраф = @Fine WHERE ID = @ReturnID";

                    using (SqlConnection conn = new SqlConnection(сonnectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ReturnID", returnID);
                            cmd.Parameters.AddWithValue("@IssueID", issueID);
                            cmd.Parameters.AddWithValue("@ReturnDate", returnDate);
                            cmd.Parameters.AddWithValue("@Fine", fine);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Запись о возврате успешно обновлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Обновление данных в DataGridView
                    LoadReturns();
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
            string query = "DELETE FROM Returns WHERE ID = @ID";

            using (SqlConnection conn = new SqlConnection(сonnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Запись успешно удалена.");
            LoadReturns(); // Обновление данных в DataGridView
            ClearFields(); // Очистка текстовых полей
            InitializePlaceholders();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Получение данных из текстовых полей
                if (!int.TryParse(txtIdIssue.Text.Trim(), out int issueID))
                {
                    MessageBox.Show("Введите корректный ID выдачи (число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!decimal.TryParse(txtFine.Text.Trim(), out decimal fine))
                {
                    MessageBox.Show("Введите корректный штраф (число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(txtDataRe.Text.Trim(), out DateTime returnDate))
                {
                    MessageBox.Show("Введите корректную дату возврата.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // SQL-запрос для добавления записи
                string query = "INSERT INTO Returns (IDВыдачи, ДатаВозврата, Штраф) VALUES (@IssueID, @ReturnDate, @Fine)";

                using (SqlConnection conn = new SqlConnection(сonnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IssueID", issueID);
                        cmd.Parameters.AddWithValue("@ReturnDate", returnDate);
                        cmd.Parameters.AddWithValue("@Fine", fine);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запись о возврате успешно добавлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Очистка полей после добавления
                ClearFields();
                LoadReturns();
                InitializePlaceholders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Returns_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Close();
        }
    }
}