using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Autorization());
        }
    }
    public class DatabaseHelper
    {
        private string connectionString = @"Data Source=adclg1; Initial catalog=Nefedkin419/4_UP; Integrated Security=True";

        public DataTable ExecuteQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public void ExecuteNonQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.ExecuteNonQuery();
            }
        }
    }
    public static class InputValidator
    {
        // Проверка на числовое значение (int)
        public static bool ValidateInt(string input, string fieldName)
        {
            if (!int.TryParse(input.Trim(), out _))
            {
                MessageBox.Show($"Введите корректное значение для поля \"{fieldName}\" (целое число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        // Проверка на числовое значение (decimal)
        public static bool ValidateDecimal(string input, string fieldName)
        {
            if (!decimal.TryParse(input.Trim(), out _))
            {
                MessageBox.Show($"Введите корректное значение для поля \"{fieldName}\" (дробное число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        // Проверка на корректную дату
        public static bool ValidateDate(string input, string fieldName)
        {
            if (!DateTime.TryParse(input.Trim(), out _))
            {
                MessageBox.Show($"Введите корректную дату для поля \"{fieldName}\".", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        // Проверка на пустое поле
        public static bool ValidateNotEmpty(string input, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show($"Поле \"{fieldName}\" не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        public static bool ValidateNotEmpty(TextBox textBox, string fieldName)
        {
            // Проверка на пустое поле или текст подсказки
            if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text == textBox.Tag?.ToString())
            {
                MessageBox.Show($"Поле \"{fieldName}\" не может быть пустым или содержать подсказку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

    }
}
