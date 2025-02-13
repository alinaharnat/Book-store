namespace Bookstore.Forms
{
    partial class MakeOrderForm
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
            this.addressesComboBox = new System.Windows.Forms.ComboBox();
            this.paymentComboBox = new System.Windows.Forms.ComboBox();
            this.makeOrderButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // addressesComboBox
            // 
            this.addressesComboBox.FormattingEnabled = true;
            this.addressesComboBox.Location = new System.Drawing.Point(290, 161);
            this.addressesComboBox.Name = "addressesComboBox";
            this.addressesComboBox.Size = new System.Drawing.Size(237, 28);
            this.addressesComboBox.TabIndex = 0;
            // 
            // paymentComboBox
            // 
            this.paymentComboBox.FormattingEnabled = true;
            this.paymentComboBox.Items.AddRange(new object[] {
            "Карткою",
            "GooglePay",
            "При отриманні"});
            this.paymentComboBox.Location = new System.Drawing.Point(290, 227);
            this.paymentComboBox.Name = "paymentComboBox";
            this.paymentComboBox.Size = new System.Drawing.Size(237, 28);
            this.paymentComboBox.TabIndex = 1;
            // 
            // makeOrderButton
            // 
            this.makeOrderButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(141)))), ((int)(((byte)(225)))));
            this.makeOrderButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.makeOrderButton.Location = new System.Drawing.Point(290, 319);
            this.makeOrderButton.Name = "makeOrderButton";
            this.makeOrderButton.Size = new System.Drawing.Size(237, 38);
            this.makeOrderButton.TabIndex = 2;
            this.makeOrderButton.Text = "Оформити";
            this.makeOrderButton.UseVisualStyleBackColor = false;
            this.makeOrderButton.Click += new System.EventHandler(this.makeOrderButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(286, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Адреса доставки:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(286, 204);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Метод оплати:";
            // 
            // MakeOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(822, 548);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.makeOrderButton);
            this.Controls.Add(this.paymentComboBox);
            this.Controls.Add(this.addressesComboBox);
            this.Name = "MakeOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Деталі замовлення";
            this.Load += new System.EventHandler(this.makeOrderForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox addressesComboBox;
        private System.Windows.Forms.ComboBox paymentComboBox;
        private System.Windows.Forms.Button makeOrderButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}