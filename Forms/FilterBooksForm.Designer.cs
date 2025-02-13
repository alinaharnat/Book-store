namespace Bookstore.Forms
{
    partial class FilterBooksForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.authorsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.minPrice = new System.Windows.Forms.NumericUpDown();
            this.maxPrice = new System.Windows.Forms.NumericUpDown();
            this.subcategoryComboBox = new System.Windows.Forms.ComboBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.minPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxPrice)).BeginInit();
            this.SuspendLayout();
            // 
            // authorsCheckedListBox
            // 
            this.authorsCheckedListBox.FormattingEnabled = true;
            this.authorsCheckedListBox.Location = new System.Drawing.Point(663, 51);
            this.authorsCheckedListBox.Name = "authorsCheckedListBox";
            this.authorsCheckedListBox.Size = new System.Drawing.Size(282, 418);
            this.authorsCheckedListBox.TabIndex = 0;
            // 
            // minPrice
            // 
            this.minPrice.Location = new System.Drawing.Point(79, 52);
            this.minPrice.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.minPrice.Name = "minPrice";
            this.minPrice.Size = new System.Drawing.Size(120, 26);
            this.minPrice.TabIndex = 2;
            // 
            // maxPrice
            // 
            this.maxPrice.Location = new System.Drawing.Point(252, 52);
            this.maxPrice.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.maxPrice.Name = "maxPrice";
            this.maxPrice.Size = new System.Drawing.Size(120, 26);
            this.maxPrice.TabIndex = 3;
            // 
            // subcategoryComboBox
            // 
            this.subcategoryComboBox.FormattingEnabled = true;
            this.subcategoryComboBox.Location = new System.Drawing.Point(440, 51);
            this.subcategoryComboBox.Name = "subcategoryComboBox";
            this.subcategoryComboBox.Size = new System.Drawing.Size(187, 28);
            this.subcategoryComboBox.TabIndex = 4;
            // 
            // OkButton
            // 
            this.OkButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(141)))), ((int)(((byte)(225)))));
            this.OkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OkButton.Location = new System.Drawing.Point(301, 488);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(272, 46);
            this.OkButton.TabIndex = 5;
            this.OkButton.Text = "Застосувати";
            this.OkButton.UseVisualStyleBackColor = false;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(141)))), ((int)(((byte)(225)))));
            this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearButton.Location = new System.Drawing.Point(440, 98);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(187, 37);
            this.clearButton.TabIndex = 7;
            this.clearButton.Text = "Прибрати вибір";
            this.clearButton.UseVisualStyleBackColor = false;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Ціна:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "від";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(217, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "до";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(436, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Жанр:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(659, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 20);
            this.label5.TabIndex = 12;
            this.label5.Text = "Автори:";
            // 
            // FilterBooksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(966, 558);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.subcategoryComboBox);
            this.Controls.Add(this.maxPrice);
            this.Controls.Add(this.minPrice);
            this.Controls.Add(this.authorsCheckedListBox);
            this.Name = "FilterBooksForm";
            this.Text = "Фільтрувати";
            this.Load += new System.EventHandler(this.FilterBooksForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.minPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxPrice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox authorsCheckedListBox;
        private System.Windows.Forms.NumericUpDown minPrice;
        private System.Windows.Forms.NumericUpDown maxPrice;
        private System.Windows.Forms.ComboBox subcategoryComboBox;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}