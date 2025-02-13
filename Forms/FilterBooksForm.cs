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
    public partial class FilterBooksForm : Form
    {
        DataBase dataBase = new DataBase();
        public FilterBooksForm()
        {
            InitializeComponent();
        }
        public decimal? PriceMin { get; private set; }
        public decimal? PriceMax { get; private set; }
        public int? SelectedGenreId { get; private set; }
        public List<int> SelectedAuthorIds { get; private set; }

        public FilterBooksForm(decimal? priceMin, decimal? priceMax, int? selectedGenreId, List<int> selectedAuthorIds)
        {
            InitializeComponent();

            PriceMin = priceMin;
            PriceMax = priceMax;
            SelectedGenreId = selectedGenreId;
            SelectedAuthorIds = new List<int>(selectedAuthorIds);

            Author.LoadAuthorsForFiltration(authorsCheckedListBox, dataBase.getSqlConnection());
            Subcategory.LoadSubcategoriesIntoComboBox(subcategoryComboBox, dataBase.getSqlConnection());

            ApplyFilter();
           
        }

        private void FilterBooksForm_Load(object sender, EventArgs e)
        {
            

        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            PriceMin = minPrice.Value > 0 ? minPrice.Value : (decimal?)null;
            PriceMax = maxPrice.Value > 0 ? maxPrice.Value : (decimal?)null;

            SelectedGenreId = subcategoryComboBox.SelectedIndex != -1 ? (int?)subcategoryComboBox.SelectedValue : null;

            SelectedAuthorIds.Clear();
            foreach (var item in authorsCheckedListBox.CheckedItems)
            {
                var author = (KeyValuePair<int, string>)item;
                SelectedAuthorIds.Add(author.Key);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

  
        private void ApplyFilter()
        {
          minPrice.Value = PriceMin ?? 0;
          maxPrice.Value = PriceMax ?? 0;

            if (SelectedGenreId.HasValue)
            {
                subcategoryComboBox.SelectedValue = SelectedGenreId;
            }
               

            for (int i = 0; i < authorsCheckedListBox.Items.Count; i++)
            {
                var author = (KeyValuePair<int, string>)authorsCheckedListBox.Items[i];
                authorsCheckedListBox.SetItemChecked(i, SelectedAuthorIds.Contains(author.Key));
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            subcategoryComboBox.SelectedIndex = -1; 
        }
    }
}
