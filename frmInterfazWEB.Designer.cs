namespace ImpresionLicencias
{
    partial class frmInterfazWEB
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
            this.lblImpresora = new System.Windows.Forms.Label();
            this.pbImpresora = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbImpresora)).BeginInit();
            this.SuspendLayout();
            // 
            // lblImpresora
            // 
            this.lblImpresora.AutoSize = true;
            this.lblImpresora.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImpresora.Location = new System.Drawing.Point(32, 148);
            this.lblImpresora.Name = "lblImpresora";
            this.lblImpresora.Size = new System.Drawing.Size(81, 20);
            this.lblImpresora.TabIndex = 1;
            this.lblImpresora.Text = "Impresora";
            // 
            // pbImpresora
            // 
            this.pbImpresora.Image = global::ImpresionLicencias.Properties.Resources.printerON;
            this.pbImpresora.Location = new System.Drawing.Point(59, 24);
            this.pbImpresora.Name = "pbImpresora";
            this.pbImpresora.Size = new System.Drawing.Size(157, 121);
            this.pbImpresora.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImpresora.TabIndex = 0;
            this.pbImpresora.TabStop = false;
            // 
            // frmInterfazWEB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 229);
            this.Controls.Add(this.lblImpresora);
            this.Controls.Add(this.pbImpresora);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInterfazWEB";
            this.Text = "Impresión Licencias";
            this.Load += new System.EventHandler(this.frmInterfazWEB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbImpresora)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImpresora;
        private System.Windows.Forms.Label lblImpresora;
    }
}