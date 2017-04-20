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
            this.pbFrontal = new System.Windows.Forms.PictureBox();
            this.pbTrasera = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbImpresora)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFrontal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTrasera)).BeginInit();
            this.SuspendLayout();
            // 
            // lblImpresora
            // 
            this.lblImpresora.AutoSize = true;
            this.lblImpresora.Location = new System.Drawing.Point(82, 122);
            this.lblImpresora.Name = "lblImpresora";
            this.lblImpresora.Size = new System.Drawing.Size(53, 13);
            this.lblImpresora.TabIndex = 1;
            this.lblImpresora.Text = "Impresora";
            // 
            // pbImpresora
            // 
            this.pbImpresora.Image = global::ImpresionLicencias.Properties.Resources.printerON;
            this.pbImpresora.Location = new System.Drawing.Point(82, 24);
            this.pbImpresora.Name = "pbImpresora";
            this.pbImpresora.Size = new System.Drawing.Size(116, 91);
            this.pbImpresora.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImpresora.TabIndex = 0;
            this.pbImpresora.TabStop = false;
            // 
            // pbFrontal
            // 
            this.pbFrontal.Location = new System.Drawing.Point(82, 167);
            this.pbFrontal.Name = "pbFrontal";
            this.pbFrontal.Size = new System.Drawing.Size(128, 90);
            this.pbFrontal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbFrontal.TabIndex = 2;
            this.pbFrontal.TabStop = false;
            // 
            // pbTrasera
            // 
            this.pbTrasera.Location = new System.Drawing.Point(82, 263);
            this.pbTrasera.Name = "pbTrasera";
            this.pbTrasera.Size = new System.Drawing.Size(128, 90);
            this.pbTrasera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbTrasera.TabIndex = 3;
            this.pbTrasera.TabStop = false;
            // 
            // frmInterfazWEB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 379);
            this.Controls.Add(this.pbTrasera);
            this.Controls.Add(this.pbFrontal);
            this.Controls.Add(this.lblImpresora);
            this.Controls.Add(this.pbImpresora);
            this.Name = "frmInterfazWEB";
            this.Text = "Impresión Licencias";
            this.Load += new System.EventHandler(this.frmInterfazWEB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbImpresora)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFrontal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTrasera)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImpresora;
        private System.Windows.Forms.Label lblImpresora;
        private System.Windows.Forms.PictureBox pbFrontal;
        private System.Windows.Forms.PictureBox pbTrasera;
    }
}