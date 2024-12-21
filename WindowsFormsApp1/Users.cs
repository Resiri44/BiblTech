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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WindowsFormsApp1
{
    public partial class Users : Form
    {
        private const string сonnectionString = @"Data Source=adclg1; Initial catalog=Nefedkin419/4_UP; Integrated Security=True";
        private Form mainForm;
        public Users(Form main)
        {
            InitializeComponent();
            LoadUsers();
            mainForm = main;
            txtRoleFilter.Tag = "Роль";

            txtName.Tag = "ФИО";
            txtLogin.Tag = "Логин";
            txtPassword.Tag = "Пароль";
            txtNumber.Tag = "Номер телефона";
            txtRole.Tag = "Роль";
            txtData.Tag = "Дата регистрации";
            InitializePlaceholders();
            txtName.Text = txtName.Tag.ToString();
            txtName.ForeColor = Color.Gray;

            txtLogin.Text = txtLogin.Tag.ToString();
            txtLogin.ForeColor = Color.Gray;

            txtPassword.Text = txtPassword.Tag.ToString();
            txtPassword.ForeColor = Color.Gray;

            txtNumber.Text = txtNumber.Tag.ToString();
            txtNumber.ForeColor = Color.Gray;

            txtRole.Text = txtRole.Tag.ToString();
            txtRole.ForeColor = Color.Gray;

            txtData.Text = txtData.Tag.ToString();
            txtData.ForeColor = Color.Gray;
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
        private void UpdateField(TextBox textBox, string value)
        {
            textBox.Text = value;
            textBox.ForeColor = Color.Black; // Цвет текста чёрный для данных из таблицы
        }
        private void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.ForeColor == Color.Gray && textBox.Text == textBox.Tag?.ToString())
            {
                textBox.Text = ""; // Очистка текста подсказки
                textBox.ForeColor = Color.Black; // Обычный цвет текста
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Tag?.ToString(); // Установка текста подсказки
                textBox.ForeColor = Color.Gray; // Цвет текста подсказки
            }
        }

        private void LoadUsers()
        {
            DatabaseHelper db = new DatabaseHelper();
            string query = "SELECT * FROM Users";
            DataTable Books = db.ExecuteQuery(query);
            dataGridView1.DataSource = Books;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mainForm.Show(); // Показать главную форму
            this.Hide();    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    string name = txtName.Text.Trim();
            //    string login = txtLogin.Text.Trim();
            //    string number = txtNumber.Text.Trim();
            //    string role = txtRole.Text.Trim();
            //    string password = txtPassword.Text.Trim();
            //    // Получение данных из текстовых полей
            //    if (string.IsNullOrWhiteSpace(txtName.Text.Trim()))
            //    {
            //        MessageBox.Show("Введите корректное ФИО.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //    if (string.IsNullOrWhiteSpace(txtLogin.Text.Trim()))
            //    {
            //        MessageBox.Show("Введите корректный логин", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //    if (string.IsNullOrWhiteSpace(txtNumber.Text.Trim()))
            //    {
            //        MessageBox.Show("Введите корректный номер", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //    if (string.IsNullOrWhiteSpace(txtRole.Text.Trim()))
            //    {
            //        MessageBox.Show("Введите корректную роль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //    if (string.IsNullOrWhiteSpace(txtPassword.Text.Trim()))
            //    {
            //        MessageBox.Show("Введите корректный пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    if (!DateTime.TryParse(txtData.Text.Trim(), out DateTime data))
            //    {
            //        MessageBox.Show("Введите корректную дату.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    // SQL-запрос для добавления записи
            //    string query = "INSERT INTO Users (ФИО, Телефон, ЭлектроннаяПочта, Role, ДатаРегистрации, Password) " +
            //               "VALUES (@Name, @Number, @Login, @Role, @Data, @Password)";

            //    using (SqlConnection conn = new SqlConnection(сonnectionString))
            //    {
            //        conn.Open();
            //        using (SqlCommand cmd = new SqlCommand(query, conn))
            //        {
            //            cmd.Parameters.AddWithValue("@Name", name);
            //            cmd.Parameters.AddWithValue("@Number", number);
            //            cmd.Parameters.AddWithValue("@Login", login);
            //            cmd.Parameters.AddWithValue("@Role", role);
            //            cmd.Parameters.AddWithValue("@Data", data);
            //            cmd.Parameters.AddWithValue("@Password", password);

            //            cmd.ExecuteNonQuery();
            //        }
            //    }

            //    MessageBox.Show("Пользователь успешно добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    // Очистка полей после добавления
            //    ClearFields();
            //    LoadUsers();
            //    InitializePlaceholders();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}


            if (!InputValidator.ValidateNotEmpty(txtName.Text, "ФИО")) return;
            if (!InputValidator.ValidateNotEmpty(txtLogin.Text, "ЭлектроннаяПочта")) return;
            if (!InputValidator.ValidateNotEmpty(txtNumber.Text, "Телефон")) return;
            if (!InputValidator.ValidateNotEmpty(txtRole.Text, "Role")) return;
            if (!InputValidator.ValidateNotEmpty(txtData.Text, "ДатаРегистрации")) return;
            if (!InputValidator.ValidateNotEmpty(txtPassword.Text, "Password")) return;
            if (string.IsNullOrWhiteSpace(txtData.Text) || !DateTime.TryParse(txtData.Text.Trim(), out DateTime parsedDate))
            {
                MessageBox.Show("Введите корректную дату.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Получение данных из текстовых полей
            string name = txtName.Text.Trim();
            string login = txtLogin.Text.Trim();
            string number = txtNumber.Text.Trim();
            string role = txtRole.Text.Trim();
            //DateTime data = DateTime.Parse(txtData.Text.Trim());
            string password = txtPassword.Text.Trim();

            string inputDate = txtData.Text.Trim();

            // SQL-запрос для добавления записи
            string query = "INSERT INTO Users (ФИО, Телефон, ЭлектроннаяПочта, Role, ДатаРегистрации, Password) " +
                           "VALUES (@Name, @Number, @Login, @Role, @Data, @Password)";

            using (SqlConnection conn = new SqlConnection(сonnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Number", number);
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@Data", parsedDate);
                    cmd.Parameters.AddWithValue("@Password", password);

                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Пользователь успешно добавлен.");
            LoadUsers(); // Обновление данных в DataGridView
            ClearFields(); // Очистка текстовых полей
            InitializePlaceholders();
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtLogin.Clear();
            txtPassword.Clear();
            txtRole.Clear();
            txtNumber.Clear();
            txtData.Clear();
        }
        private void dataGridViewUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                UpdateField(txtName, dataGridView1.SelectedRows[0].Cells["ФИО"].Value?.ToString() ?? "");
                UpdateField(txtLogin, dataGridView1.SelectedRows[0].Cells["ЭлектроннаяПочта"].Value?.ToString() ?? "");
                UpdateField(txtNumber, dataGridView1.SelectedRows[0].Cells["Телефон"].Value?.ToString() ?? "");
                UpdateField(txtRole, dataGridView1.SelectedRows[0].Cells["Role"].Value?.ToString() ?? "");
                UpdateField(txtData, dataGridView1.SelectedRows[0].Cells["ДатаРегистрации"].Value?.ToString() ?? "");
                UpdateField(txtPassword, dataGridView1.SelectedRows[0].Cells["Password"].Value?.ToString() ?? "");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для редактирования.");
                return;
            }
            // Получение ID выбранной записи
            int id = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;
            // Получение данных из текстовых полей
            string name = txtName.Text.Trim();
            string login = txtLogin.Text.Trim();
            string number = txtNumber.Text.Trim();
            string role = txtRole.Text.Trim();
            DateTime data = DateTime.Parse(txtData.Text.Trim());
            string password = txtPassword.Text.Trim();

            // SQL-запрос для обновления записи
            string query = "UPDATE Users SET ФИО = @Name, Телефон = @Number, ЭлектроннаяПочта = @Login, Role = @Role, " +
                           "ДатаРегистрации = @Data, Password = @Password WHERE ID = @ID";
            using (SqlConnection conn = new SqlConnection(сonnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Number", number);
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@Data", data);
                    cmd.Parameters.AddWithValue("@Password", password);

                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Данные о пользователе успешно обновлены.");
            LoadUsers(); // Обновление данных в DataGridView
            ClearFields(); // Очистка текстовых полей
            InitializePlaceholders();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для удаления.");
                return;
            }
            // Получение ID выбранной записи
            int id = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

            // SQL-запрос для удаления записи
            string query = "DELETE FROM Users WHERE ID = @ID";

            using (SqlConnection conn = new SqlConnection(сonnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Пользователь успешно удален.");
            LoadUsers(); // Обновление данных в DataGridView
            ClearFields(); // Очистка текстовых полей
            InitializePlaceholders();
        }

        private void ApplyFilter()
        {
            // Получение данных из текстовых полей
            string role = txtRoleFilter.Text.Trim();

            // Формирование SQL-запроса с учетом фильтров
            string query = "SELECT ID, ФИО, Телефон, ЭлектроннаяПочта, Role, ДатаРегистрации, Password FROM Users WHERE 1=1";

            if (!string.IsNullOrEmpty(role))
            {
                query += " AND Role LIKE @Role";
            }

            using (SqlConnection conn = new SqlConnection(сonnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                // Добавление параметров в запрос
                if (!string.IsNullOrEmpty(role))
                {
                    cmd.Parameters.AddWithValue("@Role", $"%{role}%");
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable users = new DataTable();

                // Заполнение DataTable
                adapter.Fill(users);

                // Привязка данных к DataGridView
                dataGridView1.DataSource = users;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtRoleFilter.Clear();
            LoadUsers();
            InitializePlaceholders();
        }

        private void Users_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Close();
        }
    }
}
