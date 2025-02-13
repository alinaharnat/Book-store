using Bookstore.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookstore.Forms
{
    public partial class UserMainForm : Form
    {
        //filtration
        private decimal? priceMin;
        private decimal? priceMax;
        private int? selectedGenreId;
        private List<int> selectedAuthorIds;

        // for items
        public Cart UserCart { get;  set; }
        private int userId;

        public UserMainForm(int userId)
        {
            InitializeComponent();
            priceMin = null;
            priceMax = null;
            selectedGenreId = null;
            selectedAuthorIds = new List<int>();
            dataGridView1.RowTemplate.Height = 100;
            this.userId = userId;
            UserCart = new Cart();
        }
        DataBase dataBase = new DataBase();
        private void UserMainForm_Load(object sender, EventArgs e)
        {
           dataGridView1.DataSource = Book.GetBooksAsDataTable(priceMin, priceMax, selectedGenreId, selectedAuthorIds, dataBase.getSqlConnection());
            if (dataGridView1.Columns[1] is DataGridViewImageColumn imageColumn)
            {
                imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom; 
            }
            else
            {
                
                DataGridViewImageColumn newImageColumn = new DataGridViewImageColumn
                {
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };
                dataGridView1.Columns.RemoveAt(1); 
                dataGridView1.Columns.Insert(1, newImageColumn); 
            }

            dataGridView1.Columns[1].Width = 100;
            dataGridView1.RowTemplate.Height = 150; 

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string keyword = searchTextBox.Text.Trim(); 
            Book.HighlightRows(keyword, dataGridView1, dataBase.getSqlConnection());
        }

        private void FilterBooks()
        {
            var dt = Book.GetBooksAsDataTable( priceMin, priceMax, selectedGenreId, selectedAuthorIds, dataBase.getSqlConnection());
            dataGridView1.DataSource = dt;
        }

        private void applyFilterButton_Click(object sender, EventArgs e)
        {
            using (FilterBooksForm filterForm = new FilterBooksForm(priceMin, priceMax, selectedGenreId, selectedAuthorIds))
            {
                if (filterForm.ShowDialog() == DialogResult.OK)
                {
                    priceMin = filterForm.PriceMin;
                    priceMax = filterForm.PriceMax;
                    selectedGenreId = filterForm.SelectedGenreId;
                    selectedAuthorIds = filterForm.SelectedAuthorIds;

                    FilterBooks();
                }
            }
        }

        private void removeFilterButton_Click(object sender, EventArgs e)
        {
            priceMin = null;
            priceMax = null;
            selectedGenreId = null;
            selectedAuthorIds.Clear();

            FilterBooks();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = dataGridView1.SelectedRows[0].Index;
            int bookId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
            if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
            {
                string bookInfo = Book.GetBookInfo(dataGridView1, rowIndex);
                var form = new AddNewItemForm(userId, bookInfo, bookId);
                form.UserCart = this.UserCart;
                form.ShowDialog();

            }
            }

        private void userCartPictureBox_Click(object sender, EventArgs e)
        {
            var form = new CartForm(userId);
            form.UserCart = this.UserCart;
            form.ShowDialog();
        }

        private void ordersLabel_Click(object sender, EventArgs e)
        {
            var form = new UserOrdersForm(userId);
            form.UserCart = this.UserCart;
            form.ShowDialog();  
        }

        private void exitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void BestPriceAndRatingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Book.SortBooksPriceRating(priceMin,priceMax,selectedGenreId,selectedAuthorIds,dataBase.getSqlConnection());  
        }

        private void SortByPagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Book.SortBooksByPages(priceMin, priceMax, selectedGenreId, selectedAuthorIds, dataBase.getSqlConnection());
        }

        private void SortByDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Book.GetBooksAsDataTable(priceMin, priceMax, selectedGenreId, selectedAuthorIds, dataBase.getSqlConnection());
        }
    }
}
