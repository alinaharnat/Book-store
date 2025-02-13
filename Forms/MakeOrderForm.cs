using Bookstore.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Bookstore.Forms
{
    public partial class MakeOrderForm : Form
    {
        public Cart UserCart {  get; set; } 
        DataBase dataBase = new DataBase();

        private int userId;
        public MakeOrderForm()
        {
            InitializeComponent();
        }
        public MakeOrderForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;

        }

        private void makeOrderForm_Load(object sender, EventArgs e)
        {
            DeliveryAddress.LoadDeliveryAddressesIntoComboBox(addressesComboBox, dataBase.getSqlConnection());
        }

        private void makeOrderButton_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();
            if (addressesComboBox.SelectedValue != null && paymentComboBox.Text != null)
            {
                bool result = false;
               
                    int address = (int)addressesComboBox.SelectedValue;
                    string payment = paymentComboBox.Text;
                    string status = "Нове";
                    result = Cart.SaveOrder(userId, UserCart.GetItems(), UserCart.GetTotalPrice(), payment, DateTime.Now, status, address, connection);
                if (result == true ) 
                {
                    UserCart.ClearCart();
                    MessageBox.Show("Замовлення успішно сформовано!");
                    this.Close();
                    var form = new UserMainForm(userId);
                    form.ShowDialog();  
                }
                else
                {
                    MessageBox.Show("Недостатньо товарів для оформлення замовлення.");
                }
                
               
                   

              
               
            }
            else
            {
                MessageBox.Show("Оберіть необхідну інформацію.");
            }
           
        }
    }
}
