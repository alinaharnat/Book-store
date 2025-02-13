namespace Bookstore
{
    partial class AdminMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminMainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.deliveryLabel = new System.Windows.Forms.Label();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.ordersPanel = new System.Windows.Forms.Panel();
            this.ordersLabel = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.clientsPanel = new System.Windows.Forms.Panel();
            this.clientsLable = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.employeesPanel = new System.Windows.Forms.Panel();
            this.employeesLabel = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.booksPanel = new System.Windows.Forms.Panel();
            this.booksLabel = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.mainLabel = new System.Windows.Forms.Label();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.exitPictureBox = new System.Windows.Forms.PictureBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.GetCitiesStatisticsButton = new System.Windows.Forms.Button();
            this.GetPublishersStatisticsButton = new System.Windows.Forms.Button();
            this.GetRepeatedCustomersButton = new System.Windows.Forms.Button();
            this.GetSalesStatisticsButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.ordersPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.clientsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.employeesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.booksPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(186)))), ((int)(((byte)(244)))));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.ordersPanel);
            this.panel1.Controls.Add(this.clientsPanel);
            this.panel1.Controls.Add(this.employeesPanel);
            this.panel1.Controls.Add(this.booksPanel);
            this.panel1.Controls.Add(this.mainPanel);
            this.panel1.Location = new System.Drawing.Point(-1, -31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(264, 732);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(186)))), ((int)(((byte)(244)))));
            this.panel2.Controls.Add(this.deliveryLabel);
            this.panel2.Controls.Add(this.pictureBox7);
            this.panel2.Location = new System.Drawing.Point(0, 516);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(264, 58);
            this.panel2.TabIndex = 6;
            // 
            // deliveryLabel
            // 
            this.deliveryLabel.AutoSize = true;
            this.deliveryLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deliveryLabel.Location = new System.Drawing.Point(76, 14);
            this.deliveryLabel.Name = "deliveryLabel";
            this.deliveryLabel.Size = new System.Drawing.Size(115, 32);
            this.deliveryLabel.TabIndex = 1;
            this.deliveryLabel.Text = "Доставка";
            this.deliveryLabel.Click += new System.EventHandler(this.itemsLabel_Click);
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox7.Image")));
            this.pictureBox7.Location = new System.Drawing.Point(11, 0);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(50, 55);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox7.TabIndex = 0;
            this.pictureBox7.TabStop = false;
            // 
            // ordersPanel
            // 
            this.ordersPanel.Controls.Add(this.ordersLabel);
            this.ordersPanel.Controls.Add(this.pictureBox5);
            this.ordersPanel.Location = new System.Drawing.Point(0, 438);
            this.ordersPanel.Name = "ordersPanel";
            this.ordersPanel.Size = new System.Drawing.Size(264, 58);
            this.ordersPanel.TabIndex = 5;
            // 
            // ordersLabel
            // 
            this.ordersLabel.AutoSize = true;
            this.ordersLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ordersLabel.Location = new System.Drawing.Point(79, 13);
            this.ordersLabel.Name = "ordersLabel";
            this.ordersLabel.Size = new System.Drawing.Size(149, 32);
            this.ordersLabel.TabIndex = 1;
            this.ordersLabel.Text = "Замовлення";
            this.ordersLabel.Click += new System.EventHandler(this.ordersLabel_Click);
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(11, 4);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(50, 50);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 0;
            this.pictureBox5.TabStop = false;
            // 
            // clientsPanel
            // 
            this.clientsPanel.Controls.Add(this.clientsLable);
            this.clientsPanel.Controls.Add(this.pictureBox4);
            this.clientsPanel.Location = new System.Drawing.Point(0, 356);
            this.clientsPanel.Name = "clientsPanel";
            this.clientsPanel.Size = new System.Drawing.Size(264, 58);
            this.clientsPanel.TabIndex = 4;
            // 
            // clientsLable
            // 
            this.clientsLable.AutoSize = true;
            this.clientsLable.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clientsLable.Location = new System.Drawing.Point(79, 14);
            this.clientsLable.Name = "clientsLable";
            this.clientsLable.Size = new System.Drawing.Size(96, 32);
            this.clientsLable.TabIndex = 1;
            this.clientsLable.Text = "Клієнти";
            this.clientsLable.Click += new System.EventHandler(this.clientsLable_Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(11, 5);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(50, 50);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            // 
            // employeesPanel
            // 
            this.employeesPanel.Controls.Add(this.employeesLabel);
            this.employeesPanel.Controls.Add(this.pictureBox3);
            this.employeesPanel.Location = new System.Drawing.Point(0, 276);
            this.employeesPanel.Name = "employeesPanel";
            this.employeesPanel.Size = new System.Drawing.Size(264, 58);
            this.employeesPanel.TabIndex = 3;
            // 
            // employeesLabel
            // 
            this.employeesLabel.AutoSize = true;
            this.employeesLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.employeesLabel.Location = new System.Drawing.Point(79, 13);
            this.employeesLabel.Name = "employeesLabel";
            this.employeesLabel.Size = new System.Drawing.Size(144, 32);
            this.employeesLabel.TabIndex = 1;
            this.employeesLabel.Text = "Працівники";
            this.employeesLabel.Click += new System.EventHandler(this.employeesLabel_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(11, 4);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(50, 50);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // booksPanel
            // 
            this.booksPanel.Controls.Add(this.booksLabel);
            this.booksPanel.Controls.Add(this.pictureBox2);
            this.booksPanel.Location = new System.Drawing.Point(0, 200);
            this.booksPanel.Name = "booksPanel";
            this.booksPanel.Size = new System.Drawing.Size(264, 58);
            this.booksPanel.TabIndex = 2;
            // 
            // booksLabel
            // 
            this.booksLabel.AutoSize = true;
            this.booksLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.booksLabel.Location = new System.Drawing.Point(79, 13);
            this.booksLabel.Name = "booksLabel";
            this.booksLabel.Size = new System.Drawing.Size(79, 32);
            this.booksLabel.TabIndex = 1;
            this.booksLabel.Text = "Книги";
            this.booksLabel.Click += new System.EventHandler(this.booksLabel_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(11, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(50, 50);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(141)))), ((int)(((byte)(225)))));
            this.mainPanel.Controls.Add(this.pictureBox1);
            this.mainPanel.Controls.Add(this.mainLabel);
            this.mainPanel.Location = new System.Drawing.Point(0, 126);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(264, 58);
            this.mainPanel.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(11, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 55);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // mainLabel
            // 
            this.mainLabel.AutoSize = true;
            this.mainLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mainLabel.Location = new System.Drawing.Point(79, 12);
            this.mainLabel.Name = "mainLabel";
            this.mainLabel.Size = new System.Drawing.Size(105, 32);
            this.mainLabel.TabIndex = 0;
            this.mainLabel.Text = "Головна";
            // 
            // pictureBox6
            // 
            this.pictureBox6.Location = new System.Drawing.Point(197, 3);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(59, 50);
            this.pictureBox6.TabIndex = 6;
            this.pictureBox6.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(10, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bookstore";
            // 
            // exitPictureBox
            // 
            this.exitPictureBox.Image = global::Bookstore.Properties.Resources.cross;
            this.exitPictureBox.Location = new System.Drawing.Point(1164, 14);
            this.exitPictureBox.Name = "exitPictureBox";
            this.exitPictureBox.Size = new System.Drawing.Size(25, 25);
            this.exitPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.exitPictureBox.TabIndex = 1;
            this.exitPictureBox.TabStop = false;
            this.exitPictureBox.Click += new System.EventHandler(this.exitPictureBox_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(286, 83);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(863, 538);
            this.dataGridView1.TabIndex = 26;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(120)))), ((int)(((byte)(178)))));
            this.panel3.Controls.Add(this.GetCitiesStatisticsButton);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.pictureBox6);
            this.panel3.Controls.Add(this.GetPublishersStatisticsButton);
            this.panel3.Controls.Add(this.GetRepeatedCustomersButton);
            this.panel3.Controls.Add(this.exitPictureBox);
            this.panel3.Controls.Add(this.GetSalesStatisticsButton);
            this.panel3.Location = new System.Drawing.Point(-1, -2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1202, 59);
            this.panel3.TabIndex = 27;
            // 
            // GetCitiesStatisticsButton
            // 
            this.GetCitiesStatisticsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetCitiesStatisticsButton.ForeColor = System.Drawing.Color.White;
            this.GetCitiesStatisticsButton.Location = new System.Drawing.Point(889, 12);
            this.GetCitiesStatisticsButton.Name = "GetCitiesStatisticsButton";
            this.GetCitiesStatisticsButton.Size = new System.Drawing.Size(261, 36);
            this.GetCitiesStatisticsButton.TabIndex = 10;
            this.GetCitiesStatisticsButton.Text = "Замовлення по містах";
            this.GetCitiesStatisticsButton.UseVisualStyleBackColor = true;
            this.GetCitiesStatisticsButton.Click += new System.EventHandler(this.GetCitiesStatisticsButton_Click);
            // 
            // GetPublishersStatisticsButton
            // 
            this.GetPublishersStatisticsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetPublishersStatisticsButton.ForeColor = System.Drawing.Color.White;
            this.GetPublishersStatisticsButton.Location = new System.Drawing.Point(262, 12);
            this.GetPublishersStatisticsButton.Name = "GetPublishersStatisticsButton";
            this.GetPublishersStatisticsButton.Size = new System.Drawing.Size(243, 36);
            this.GetPublishersStatisticsButton.TabIndex = 7;
            this.GetPublishersStatisticsButton.Text = "Популярність видавництв";
            this.GetPublishersStatisticsButton.UseVisualStyleBackColor = true;
            this.GetPublishersStatisticsButton.Click += new System.EventHandler(this.GetPublishersStatisticsButton_Click);
            // 
            // GetRepeatedCustomersButton
            // 
            this.GetRepeatedCustomersButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetRepeatedCustomersButton.ForeColor = System.Drawing.Color.White;
            this.GetRepeatedCustomersButton.Location = new System.Drawing.Point(700, 12);
            this.GetRepeatedCustomersButton.Name = "GetRepeatedCustomersButton";
            this.GetRepeatedCustomersButton.Size = new System.Drawing.Size(183, 36);
            this.GetRepeatedCustomersButton.TabIndex = 9;
            this.GetRepeatedCustomersButton.Text = "Повторні покупці";
            this.GetRepeatedCustomersButton.UseVisualStyleBackColor = true;
            this.GetRepeatedCustomersButton.Click += new System.EventHandler(this.GetRepeatedCustomersButton_Click);
            // 
            // GetSalesStatisticsButton
            // 
            this.GetSalesStatisticsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetSalesStatisticsButton.ForeColor = System.Drawing.Color.White;
            this.GetSalesStatisticsButton.Location = new System.Drawing.Point(511, 12);
            this.GetSalesStatisticsButton.Name = "GetSalesStatisticsButton";
            this.GetSalesStatisticsButton.Size = new System.Drawing.Size(183, 36);
            this.GetSalesStatisticsButton.TabIndex = 8;
            this.GetSalesStatisticsButton.Text = "Аналітика продажів";
            this.GetSalesStatisticsButton.UseVisualStyleBackColor = true;
            this.GetSalesStatisticsButton.Click += new System.EventHandler(this.GetSalesStatisticsButton_Click);
            // 
            // AdminMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AdminMainForm";
            this.Load += new System.EventHandler(this.AdminMainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.ordersPanel.ResumeLayout(false);
            this.ordersPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.clientsPanel.ResumeLayout(false);
            this.clientsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.employeesPanel.ResumeLayout(false);
            this.employeesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.booksPanel.ResumeLayout(false);
            this.booksPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label mainLabel;
        private System.Windows.Forms.Panel ordersPanel;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Panel clientsPanel;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Panel employeesPanel;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Panel booksPanel;
        private System.Windows.Forms.Label booksLabel;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label ordersLabel;
        private System.Windows.Forms.Label clientsLable;
        private System.Windows.Forms.Label employeesLabel;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox exitPictureBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label deliveryLabel;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button GetCitiesStatisticsButton;
        private System.Windows.Forms.Button GetRepeatedCustomersButton;
        private System.Windows.Forms.Button GetSalesStatisticsButton;
        private System.Windows.Forms.Button GetPublishersStatisticsButton;
    }
}