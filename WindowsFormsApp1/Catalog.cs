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
    public partial class Catalog : Form
    {
        private const string сonnectionString = @"Data Source=adclg1; Initial catalog=Nefedkin419/4_UP; Integrated Security=True";
        private Form mainForm;
        private string userRole;

        
        public Catalog(Form main, string role)
        {
            InitializeComponent();
            LoadBooks();
            mainForm = main;
            userRole = role;
            ConfigureAccess();
            //txtAuthorFilter.Tag = "Автор";
            //txtGenreFilter.Tag = "Жанр";
            //txtYearFilter.Tag = "Год";

            txtTitle.Tag = "Название";
            txtAuthor.Tag = "Автор";
            txtGenre.Tag = "Жанр";
            txtYear.Tag = "Год издания";
            txtPublisher.Tag = "Издательство";
            txtISBN.Tag = "ISBN";
            txtCopies.Tag = "Количество";
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
                btnAddBook.Enabled = false;
                btnEditBook.Enabled = false;
                btnDeleteBook.Enabled = false;
            }
        }
        private void LoadBooks()
        {
            DatabaseHelper db = new DatabaseHelper();
            string query = "SELECT * FROM Books";
            DataTable Books = db.ExecuteQuery(query);
            dataGridView1.DataSource = Books;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mainForm.Show(); // Показать главную форму
            this.Hide();    // Закрыть текущую форму
        }

        private void button1_Click(object sender, EventArgs e)
        {
                // Получение данных из текстовых полей
                string isbn = txtISBN.Text.Trim();
                string title = txtTitle.Text.Trim();
                string author = txtAuthor.Text.Trim();
                string genre = txtGenre.Text.Trim();
                int year = int.Parse(txtYear.Text.Trim());
                string publisher = txtPublisher.Text.Trim();
                int copies = int.Parse(txtCopies.Text.Trim());

                // SQL-запрос для добавления записи
                string query = "INSERT INTO Books (ISBN, Название, Автор, Жанр, Год, Издательство, КоличествоЭкземпляров) " +
                               "VALUES (@ISBN, @Title, @Author, @Genre, @Year, @Publisher, @Copies)";

                using (SqlConnection conn = new SqlConnection(сonnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ISBN", isbn);
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Author", author);
                        cmd.Parameters.AddWithValue("@Genre", genre);
                        cmd.Parameters.AddWithValue("@Year", year);
                        cmd.Parameters.AddWithValue("@Publisher", publisher);
                        cmd.Parameters.AddWithValue("@Copies", copies);

                        cmd.ExecuteNonQuery();
                    }
                }
            MessageBox.Show("Книга успешно добавлена.");
                LoadBooks(); // Обновление данных в DataGridView
                ClearFields(); // Очистка текстовых полей
            }

        private void ClearFields()
        {
            txtISBN.Clear();
            txtTitle.Clear();
            txtAuthor.Clear();
            txtGenre.Clear();
            txtYear.Clear();
            txtPublisher.Clear();
            txtCopies.Clear();
        }

        private void UpdateField(TextBox textBox, string value)
        {
            textBox.Text = value;
            textBox.ForeColor = Color.Black; // Устанавливаем обычный цвет текста
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                txtISBN.Text = dataGridView1.SelectedRows[0].Cells["ISBN"].Value.ToString();
                txtTitle.Text = dataGridView1.SelectedRows[0].Cells["Название"].Value.ToString();
                txtAuthor.Text = dataGridView1.SelectedRows[0].Cells["Автор"].Value.ToString();
                txtGenre.Text = dataGridView1.SelectedRows[0].Cells["Жанр"].Value.ToString();
                txtYear.Text = dataGridView1.SelectedRows[0].Cells["Год"].Value.ToString();
                txtPublisher.Text = dataGridView1.SelectedRows[0].Cells["Издательство"].Value.ToString();
                txtCopies.Text = dataGridView1.SelectedRows[0].Cells["КоличествоЭкземпляров"].Value.ToString();
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

            // Получение данных из текстовых полей
            string isbn = txtISBN.Text.Trim();
            string title = txtTitle.Text.Trim();
            string author = txtAuthor.Text.Trim();
            string genre = txtGenre.Text.Trim();
            int year = int.Parse(txtYear.Text.Trim());
            string publisher = txtPublisher.Text.Trim();
            int copies = int.Parse(txtCopies.Text.Trim());

            // SQL-запрос для обновления записи
            string query = "UPDATE Books SET ISBN = @ISBN, Название = @Title, Автор = @Author, Жанр = @Genre, " +
                           "Год = @Year, Издательство = @Publisher, КоличествоЭкземпляров = @Copies WHERE ID = @ID";

            using (SqlConnection conn = new SqlConnection(сonnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@ISBN", isbn);
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Author", author);
                    cmd.Parameters.AddWithValue("@Genre", genre);
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@Publisher", publisher);
                    cmd.Parameters.AddWithValue("@Copies", copies);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Данные о книге успешно обновлены.");
            LoadBooks(); // Обновление данных в DataGridView
            ClearFields(); // Очистка текстовых полей
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
            string query = "DELETE FROM Books WHERE ID = @ID";

            using (SqlConnection conn = new SqlConnection(сonnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Книга успешно удалена.");
            LoadBooks(); // Обновление данных в DataGridView
            ClearFields(); // Очистка текстовых полей
        }

        private void ApplyFilter()
        {
            // Получение данных из текстовых полей
            string author = txtAuthorFilter.Text.Trim();
            string genre = txtGenreFilter.Text.Trim();
            string year = txtYearFilter.Text.Trim();

            // Формирование SQL-запроса с учетом фильтров
            string query = "SELECT ID, ISBN, Название, Автор, Жанр, Год, Издательство, КоличествоЭкземпляров FROM Books WHERE 1=1";

            if (!string.IsNullOrEmpty(author))
            {
                query += " AND Автор LIKE @Author";
            }
            if (!string.IsNullOrEmpty(genre))
            {
                query += " AND Жанр LIKE @Genre";
            }
            if (!string.IsNullOrEmpty(year))
            {
                if (int.TryParse(year, out int year1))
                {
                    query += " AND Год = @Year";
                }
                else
                {
                    MessageBox.Show("Введите корректное числовое значение для года.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            using (SqlConnection conn = new SqlConnection(сonnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                // Добавление параметров в запрос
                if (!string.IsNullOrEmpty(author))
                {
                    cmd.Parameters.AddWithValue("@Author", $"%{author}%");
                }
                if (!string.IsNullOrEmpty(genre))
                {
                    cmd.Parameters.AddWithValue("@Genre", $"%{genre}%");
                }
                if (!string.IsNullOrEmpty(year))
                {
                    cmd.Parameters.AddWithValue("@Year", year);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable books = new DataTable();

                // Заполнение DataTable
                adapter.Fill(books);

                // Привязка данных к DataGridView
                dataGridView1.DataSource = books;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtAuthorFilter.Clear();
            txtGenreFilter.Clear();
            txtYearFilter.Clear();
            LoadBooks(); // Загрузка всех записей
            InitializePlaceholders();
        }
        private void Catalog_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Close();
        }
    }
}
