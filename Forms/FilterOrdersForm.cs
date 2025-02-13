using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Bookstore.Forms
{
    public partial class FilterOrdersForm : Form
    {
        private DataBase dataBase = new DataBase();

        public string SelectedStatus { get; private set; }
        public decimal? OrderMinPrice { get; private set; }
        public decimal? OrderMaxPrice { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }

        public FilterOrdersForm()
        {
            InitializeComponent();
        }
        public FilterOrdersForm(string selectedStatus, decimal? orderMinPrice, decimal? orderMaxPrice, DateTime? startDate, DateTime? endDate)
        {
            InitializeComponent();


            SelectedStatus = selectedStatus;
            OrderMinPrice = orderMinPrice;
            OrderMaxPrice = orderMaxPrice;
            StartDate = startDate;
            EndDate = endDate;
            ApplyFilter();
        }

        private void FilterOrdersForm_Load(object sender, EventArgs e)
        {
           

        }



        private void ApplyFilter()
        {
            if (!string.IsNullOrEmpty(SelectedStatus))
            {
                comboBoxStatus.SelectedItem = SelectedStatus;
            }

            if (OrderMinPrice.HasValue)
            {
                numericUpDownMinPrice.Value = OrderMinPrice.Value;
            }

            if (OrderMaxPrice.HasValue)
            {
                numericUpDownMaxPrice.Value = OrderMaxPrice.Value;
            }
            if (StartDate.HasValue)
            {
                textBoxStartDate.Text = StartDate.Value.ToString("dd.MM.yyyy");
            }

            if (EndDate.HasValue)
            {
                textBoxEndDate.Text = EndDate.Value.ToString("dd.MM.yyyy");
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            SelectedStatus = comboBoxStatus.SelectedIndex != -1 ? comboBoxStatus.SelectedItem.ToString() : null;
            OrderMinPrice = numericUpDownMinPrice.Value > 0 ? numericUpDownMinPrice.Value : (decimal?)null;
            OrderMaxPrice = numericUpDownMaxPrice.Value > 0 ? numericUpDownMaxPrice.Value : (decimal?)null;
            if (DateTime.TryParse(textBoxStartDate.Text, out DateTime start))
            {
                StartDate = start;
            }
            else
            {
                StartDate = null;
            }

            if (DateTime.TryParse(textBoxEndDate.Text, out DateTime end))
            {
                EndDate = end;
            }
            else
            {
                EndDate = null; 
            }

            this.DialogResult = DialogResult.OK;
            this.Close();

        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            comboBoxStatus.SelectedIndex = -1;

        }

       
    }
}
