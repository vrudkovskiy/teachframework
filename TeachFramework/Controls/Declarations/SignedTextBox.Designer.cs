namespace TeachFramework.Controls
{
    partial class SignedTextBox
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
            this._stbText = new System.Windows.Forms.Label();
            this._stbTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _stbText
            // 
            this._stbText.AutoSize = true;
            this._stbText.Location = new System.Drawing.Point(3, 3);
            this._stbText.Name = "_stbText";
            this._stbText.Size = new System.Drawing.Size(0, 13);
            this._stbText.TabIndex = 0;
            // 
            // _stbTextBox
            // 
            this._stbTextBox.Location = new System.Drawing.Point(94, 0);
            this._stbTextBox.Name = "_stbTextBox";
            this._stbTextBox.Size = new System.Drawing.Size(126, 20);
            this._stbTextBox.TabIndex = 1;
            // 
            // SignedTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._stbTextBox);
            this.Controls.Add(this._stbText);
            this.Name = "SignedTextBox";
            this.Size = new System.Drawing.Size(223, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _stbText;
        private System.Windows.Forms.TextBox _stbTextBox;
    }
}
