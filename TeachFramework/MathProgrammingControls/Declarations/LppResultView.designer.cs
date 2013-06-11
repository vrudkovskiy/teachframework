namespace TeachFramework.Controls
{
    partial class LppResultView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.coordinatesDataGrid = new System.Windows.Forms.DataGridView();
            this.successRadio = new System.Windows.Forms.RadioButton();
            this.tfUnlimitedRadio = new System.Windows.Forms.RadioButton();
            this.setIncompatibleRadio = new System.Windows.Forms.RadioButton();
            this.tfValueTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.coordinatesDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // coordinatesDataGrid
            // 
            this.coordinatesDataGrid.AllowUserToAddRows = false;
            this.coordinatesDataGrid.AllowUserToDeleteRows = false;
            this.coordinatesDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.coordinatesDataGrid.Location = new System.Drawing.Point(3, 26);
            this.coordinatesDataGrid.Name = "coordinatesDataGrid";
            this.coordinatesDataGrid.Size = new System.Drawing.Size(476, 67);
            this.coordinatesDataGrid.TabIndex = 0;
            // 
            // successRadio
            // 
            this.successRadio.AutoSize = true;
            this.successRadio.Checked = true;
            this.successRadio.Location = new System.Drawing.Point(14, 3);
            this.successRadio.Name = "successRadio";
            this.successRadio.Size = new System.Drawing.Size(115, 17);
            this.successRadio.TabIndex = 1;
            this.successRadio.TabStop = true;
            this.successRadio.Text = "Ввести розв’язок";
            this.successRadio.UseVisualStyleBackColor = true;
            this.successRadio.CheckedChanged += new System.EventHandler(this.SuccessRadioCheckedChanged);
            // 
            // tfUnlimitedRadio
            // 
            this.tfUnlimitedRadio.AutoSize = true;
            this.tfUnlimitedRadio.Location = new System.Drawing.Point(186, 3);
            this.tfUnlimitedRadio.Name = "tfUnlimitedRadio";
            this.tfUnlimitedRadio.Size = new System.Drawing.Size(181, 17);
            this.tfUnlimitedRadio.TabIndex = 2;
            this.tfUnlimitedRadio.Text = "Ф-ція цілі необмежена на МПР";
            this.tfUnlimitedRadio.UseVisualStyleBackColor = true;
            // 
            // setIncompatibleRadio
            // 
            this.setIncompatibleRadio.AutoSize = true;
            this.setIncompatibleRadio.Location = new System.Drawing.Point(450, 3);
            this.setIncompatibleRadio.Name = "setIncompatibleRadio";
            this.setIncompatibleRadio.Size = new System.Drawing.Size(103, 17);
            this.setIncompatibleRadio.TabIndex = 3;
            this.setIncompatibleRadio.Text = "МПР несумісна";
            this.setIncompatibleRadio.UseVisualStyleBackColor = true;
            // 
            // tfValueTextBox
            // 
            this.tfValueTextBox.Location = new System.Drawing.Point(522, 49);
            this.tfValueTextBox.Name = "tfValueTextBox";
            this.tfValueTextBox.Size = new System.Drawing.Size(63, 20);
            this.tfValueTextBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(486, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "f(x) =";
            // 
            // LppResultView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tfValueTextBox);
            this.Controls.Add(this.setIncompatibleRadio);
            this.Controls.Add(this.tfUnlimitedRadio);
            this.Controls.Add(this.successRadio);
            this.Controls.Add(this.coordinatesDataGrid);
            this.Name = "LppResultView";
            this.Size = new System.Drawing.Size(588, 96);
            ((System.ComponentModel.ISupportInitialize)(this.coordinatesDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView coordinatesDataGrid;
        private System.Windows.Forms.RadioButton successRadio;
        private System.Windows.Forms.RadioButton tfUnlimitedRadio;
        private System.Windows.Forms.RadioButton setIncompatibleRadio;
        private System.Windows.Forms.TextBox tfValueTextBox;
        private System.Windows.Forms.Label label1;
    }
}
