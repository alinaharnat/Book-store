using Bookstore.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
namespace Bookstore
{
    public partial class LoginForm : System.Windows.Forms.Form
    {
        DataBase dataBase = new DataBase();
        private int userId;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void exitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void registrationLabel_Click(object sender, EventArgs e)
        {
            var form = new RegistrationForm();
            form.ShowDialog();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            var loginEmail = emailTextBox.Text;
            var loginPassword = passwordTextBox.Text;

            //User

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataTable dt = new DataTable();

            string sqlQuery = $"select user_id, user_email, user_password from Users where user_email ='{loginEmail}' and user_password ='{loginPassword}'";
            SqlCommand command = new SqlCommand(sqlQuery, dataBase.getSqlConnection());

            sqlDataAdapter.SelectCommand = command;
            sqlDataAdapter.Fill(dt);

            //Employee

            SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter();
            DataTable dt2 = new DataTable();

            string sqlQuery2 = $"select employee_id, employee_email, employee_password from Employees where employee_email ='{loginEmail}' and employee_password ='{loginPassword}'";
            SqlCommand command2 = new SqlCommand(sqlQuery2, dataBase.getSqlConnection());

            sqlDataAdapter2.SelectCommand = command2;
            sqlDataAdapter2.Fill(dt2);

            if (dt.Rows.Count == 1)
            {
                userId = Convert.ToInt32(dt.Rows[0]["user_id"]);

                var form = new UserMainForm(userId);
                form.ShowDialog();

            }
            else if(dt2.Rows.Count== 1)
            {
            var form = new AdminMainForm();
            form.ShowDialog();
                //
            }
            else
            {
                MessageBox.Show("Акаунт не існує");
            }
        }
    }
}
