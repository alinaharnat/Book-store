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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Bookstore.Forms
{
    public partial class CartForm : Form
    {
        public Cart UserCart { get; set; }
        private int selectedBookId;
        private int userId;

        public CartForm()
        {
            InitializeComponent();
        }
        public CartForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;   
            
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void CartForm_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false; 
            dataGridView1.DataSource = UserCart.GetItems();
            totalTextBox.Text = UserCart.GetTotalPrice().ToString();   

            
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BookId",
                HeaderText = "book_id",
                Name = "BookIdColumn"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BookTitle", 
                HeaderText = "Назва книги",
                Name = "TitleColumn"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Quantity", 
                HeaderText = "Кількість",
                Name = "QuantityColumn"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "BookPrice", 
                HeaderText = "Ціна за одиницю",
                Name = "PriceColumn"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ItemPrice", 
                HeaderText = "Вартість позиції",
                Name = "TotalPriceColumn"
            });
            dataGridView1.Columns[0].Visible = false;
            changePanel.Visible = false;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            int newQuantity = (int)numericUpDownQuantity.Value;

            UserCart.UpdateItemQuantity(selectedBookId, newQuantity);
            dataGridView1.DataSource = null;

            dataGridView1.DataSource = UserCart.GetItems();

            changePanel.Visible = false;
            totalTextBox.Text = UserCart.GetTotalPrice().ToString();
        }

        private void changeItemButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                selectedBookId = (int)selectedRow.Cells[0].Value;
                int quantity = (int)selectedRow.Cells[2].Value;

                numericUpDownQuantity.Value = quantity;
                changePanel.Visible = true;
                

            }
            else
            {
                MessageBox.Show("Оберіть позицію для редагування.");
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                selectedBookId = (int)selectedRow.Cells[0].Value;
                UserCart.RemoveItem(selectedBookId);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource= UserCart.GetItems();
                totalTextBox.Text = UserCart.GetTotalPrice().ToString();

            }
            else
            {
                MessageBox.Show("Оберіть позицію для видалення.");
            }

        }

        private void makeOrderButton_Click(object sender, EventArgs e)
        {
            var form = new MakeOrderForm(userId);
           
            form.UserCart = this.UserCart;
            form.ShowDialog();
            this.Close();
        }
    }
}
