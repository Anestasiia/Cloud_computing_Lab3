namespace WindowsFormsApp1
{
    partial class ContactForm
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
            this.CreateNewContactButton = new System.Windows.Forms.Button();
            this.TableData = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.TableData)).BeginInit();
            this.SuspendLayout();
            // 
            // CreateNewContactButton
            // 
            this.CreateNewContactButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CreateNewContactButton.Location = new System.Drawing.Point(12, 312);
            this.CreateNewContactButton.Name = "CreateNewContactButton";
            this.CreateNewContactButton.Size = new System.Drawing.Size(191, 49);
            this.CreateNewContactButton.TabIndex = 1;
            this.CreateNewContactButton.Text = "Create new contact";
            this.CreateNewContactButton.UseVisualStyleBackColor = true;
            this.CreateNewContactButton.Click += new System.EventHandler(this.CreateNewContactButton_Click);
            // 
            // TableData
            // 
            this.TableData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TableData.Location = new System.Drawing.Point(12, 12);
            this.TableData.Name = "TableData";
            this.TableData.RowHeadersWidth = 51;
            this.TableData.RowTemplate.Height = 24;
            this.TableData.Size = new System.Drawing.Size(1054, 282);
            this.TableData.TabIndex = 2;
            this.TableData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TableData_CellContentClick);
            // 
            // ContactForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1108, 392);
            this.Controls.Add(this.TableData);
            this.Controls.Add(this.CreateNewContactButton);
            this.Name = "ContactForm";
            this.Text = "Contact_Form";
            this.Activated += new System.EventHandler(this.ContactForm_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.TableData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button CreateNewContactButton;
        private System.Windows.Forms.DataGridView TableData;
    }
}