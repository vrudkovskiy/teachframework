namespace TeachFramework.Controls
{
    partial class StepsContainer
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
            this._steps = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // _steps
            // 
            this._steps.FormattingEnabled = true;
            this._steps.Location = new System.Drawing.Point(4, 5);
            this._steps.Name = "_steps";
            this._steps.Size = new System.Drawing.Size(442, 21);
            this._steps.TabIndex = 0;
            // 
            // StepsContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._steps);
            this.Name = "StepsContainer";
            this.Size = new System.Drawing.Size(451, 30);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox _steps;
    }
}
