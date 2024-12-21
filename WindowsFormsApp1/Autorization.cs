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
    public partial class Autorization : Form
    {
        private const string сonnectionString = @"Data Source=adclg1; Initial catalog=Nefedkin419/4_UP; Integrated Security=True";
        public Autorization()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
                string query = "SELECT Role FROM Users WHERE ЭлектроннаяПочта = @Login AND Password = @Password";

                using (SqlConnection conn = new SqlConnection(сonnectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Login", txtLogin.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                    conn.Open();

                    object role = cmd.ExecuteScalar();
                    if (role != null)
                    {
                        string userRole = role.ToString(); // Получение роли пользователя
                        Main mainForm = new Main(userRole, this); // Передача роли в главную форму
                        mainForm.Show();
                        this.Hide(); // Скрыть окно авторизации
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
        }
    }
}
