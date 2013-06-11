namespace TeachFramework
{
    /// <summary>
    /// Form for teaching WinForm application
    /// </summary>
    partial class WinAppForm
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
            this.components = new System.ComponentModel.Container();
            this.submitButton = new System.Windows.Forms.Button();
            this._controlsPanel = new System.Windows.Forms.Panel();
            this.autoButton = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.resentBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(551, 531);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(117, 37);
            this.submitButton.TabIndex = 1;
            this.submitButton.Text = "Далі";
            this.submitButton.UseVisualStyleBackColor = true;
            // 
            // _controlsPanel
            // 
            this._controlsPanel.AutoScroll = true;
            this._controlsPanel.Location = new System.Drawing.Point(13, 13);
            this._controlsPanel.Name = "_controlsPanel";
            this._controlsPanel.Size = new System.Drawing.Size(655, 512);
            this._controlsPanel.TabIndex = 2;
            // 
            // autoButton
            // 
            this.autoButton.Location = new System.Drawing.Point(427, 531);
            this.autoButton.Name = "autoButton";
            this.autoButton.Size = new System.Drawing.Size(118, 37);
            this.autoButton.TabIndex = 3;
            this.autoButton.Text = "Підставити правильні дані";
            this.autoButton.UseVisualStyleBackColor = true;
            this.autoButton.Click += new System.EventHandler(this.AutoButtonClick);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // resentBtn
            // 
            this.resentBtn.Location = new System.Drawing.Point(12, 531);
            this.resentBtn.Name = "resentBtn";
            this.resentBtn.Size = new System.Drawing.Size(123, 37);
            this.resentBtn.TabIndex = 4;
            this.resentBtn.Text = "До списку програм";
            this.resentBtn.UseVisualStyleBackColor = true;
            this.resentBtn.Click += new System.EventHandler(this.ResentBtnClick);
            // 
            // WinAppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 573);
            this.Controls.Add(this.resentBtn);
            this.Controls.Add(this.autoButton);
            this.Controls.Add(this._controlsPanel);
            this.Controls.Add(this.submitButton);
            this.Name = "WinAppForm";
            this.Text = "TeachAppForm";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Panel _controlsPanel;
        private System.Windows.Forms.Button autoButton;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button resentBtn;
    }
}

