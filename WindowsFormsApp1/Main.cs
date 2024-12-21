using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Main : Form
    {
        private string userRole;
        private Autorization autorization1;
        public Main(string role, Autorization autorization)
        {
            InitializeComponent();
            userRole = role; 
            ConfigureMenuAccess();
            autorization1 = autorization;
        }

        private void ConfigureMenuAccess()
        {
            if (userRole == "Читатель")
            {
                btnUsers.Enabled = false; // Запрет доступа к "Пользователи"
            }
            else if (userRole == "Библиотекарь")
            {
                btnUsers.Enabled = false; // Запрет доступа к "Пользователи"
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Открытие формы и передача роли
            Catalog mainForm = new Catalog(this, userRole);
            mainForm.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Открыть форму авторизации
            autorization1.Show();

            // Закрыть текущую форму
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Users users = new Users(this);
            users.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Issues issues = new Issues(this, userRole);
            issues.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Returns returns = new Returns(this, userRole);
            returns.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Request request = new Request(this);
            request.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TopBook top = new TopBook(this);
            top.Show();
            this.Hide();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            autorization1.Close();
        }
    }
}
