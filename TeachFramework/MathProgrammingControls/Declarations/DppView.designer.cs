namespace TeachFramework.Controls
{
    partial class DppView
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
            this.variablesPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lpp = new LppView();
            this.SuspendLayout();
            // 
            // variablesPanel
            // 
            this.variablesPanel.Location = new System.Drawing.Point(100, 150);
            this.variablesPanel.Name = "variablesPanel";
            this.variablesPanel.Size = new System.Drawing.Size(163, 74);
            this.variablesPanel.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(36, 174);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Цілі:";
            // 
            // lpp
            // 
            this.lpp.ControlName = "LPPView";
            this.lpp.Location = new System.Drawing.Point(3, 3);
            this.lpp.Name = "lpp";
            this.lpp.Size = new System.Drawing.Size(475, 153);
            this.lpp.TabIndex = 3;
            this.lpp.Value = null;
            // 
            // DppView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lpp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.variablesPanel);
            this.Name = "DppView";
            this.Size = new System.Drawing.Size(487, 227);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel variablesPanel;
        private System.Windows.Forms.Label label1;
        private LppView lpp;
    }
}
