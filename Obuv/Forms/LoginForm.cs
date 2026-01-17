using Npgsql;
using Obuv.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Obuv
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public void UserAuth()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Resources.connectionDB))
            {
                connection.Open();
                string query = @"SELECT user_role.""role"", name, login, password
                                        FROM public.users
                                        JOIN user_role ON user_role.id = users.role
                                        WHERE login = @login and password = @password";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection)) 
                {
                    command.Parameters.AddWithValue("login", textlogin.Text);
                    command.Parameters.AddWithValue("password", textpassword.Text);
                    using (NpgsqlDataReader reader = command.ExecuteReader()) 
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            { //Успешная авторизация 
                                MessageBox.Show("Авторизация прошла успешно!", "Информация", MessageBoxButtons.OK);
                                this.Hide();
                                MainForm mainForm = new MainForm();
                                mainForm.Show();
                            }
                            else
                            {
                                //Ошибка авторизации
                                MessageBox.Show("Ошибка авторизации!");
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserAuth();
            if(textlogin.Text == "")
            {
                MessageBox.Show("Заполните поле логин!");
                return;
            }
            if (textpassword.Text == "")
            {
                MessageBox.Show("Заполните поле пароль!");
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        private void textlogin_Click(object sender, EventArgs e)
        {
            textlogin.Clear();
            textlogin.Font = new Font("Times New Roman",20, FontStyle.Bold);
            textlogin.ForeColor = Color.Black;
            
        }

        private void textpassword_Click(object sender, EventArgs e)
        {
            textpassword.Clear();
            textpassword.ForeColor = Color.Black;
            textpassword.PasswordChar = '*';
            textpassword.Font = new Font("Times New Roman", 20, FontStyle.Bold);
            
        }
    }
}
