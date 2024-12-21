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

namespace WindowsFormsApp1
{
    public partial class TopBook : Form
    {
        private const string сonnectionString = @"Data Source=adclg1; Initial catalog=Nefedkin419/4_UP; Integrated Security=True";
        private Form mainForm;
        public TopBook(Form main)
        {
            InitializeComponent();
            mainForm = main;
        }
        private void LoadPopularBooks()
        {
            DateTime startDate = dateTimePickerStart.Value.Date;
            DateTime endDate = dateTimePickerEnd.Value.Date;

            string query = @"
        SELECT 
            Books.ID, 
            Books.Название AS Книга, 
            COUNT(Issues.ID) AS КоличествоВыдач
        FROM 
            Books
        LEFT JOIN 
            Issues ON Books.ID = Issues.IDКниги
        WHERE 
            Issues.ДатаВыдачи BETWEEN @StartDate AND @EndDate
        GROUP BY 
            Books.ID, Books.Название
        ORDER BY 
            КоличествоВыдач DESC;";

            using (SqlConnection conn = new SqlConnection(сonnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable popularBooks = new DataTable();
                adapter.Fill(popularBooks);
                dataGridView1.DataSource = popularBooks;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadPopularBooks();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mainForm.Show(); // Показать главную форму
            this.Hide();    // Закрыть текущую форму
        }

        private void TopBook_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Close();
        }
    }
}
