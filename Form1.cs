using Print;
using Print.Printing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpresionLicencias
{
    public partial class Form1 : Form
    {

        private Bitmap imgFrontal;
        private Bitmap imgTrasera;
        private const String nombreImgFrontal= "licenciaFrontal.jpg";
        private const String nombreImgTrasera = "licenciaTrasera.jpg";
        private Artifacts artifacts = new Artifacts();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea imprimir la licencia?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                return;
            if (ValidateChildren())
            {
                CardModel card = new CardModel(artifacts);
                card.PrinterName = "TOPPAN CP500";

                if (File.Exists(nombreImgFrontal))
                    card.Color1 = Image.FromFile(nombreImgFrontal);
                if (File.Exists(nombreImgTrasera))
                    card.Color2 = Image.FromFile(nombreImgTrasera);
                PrintService.Print(card);
            }
        }

        private void guardarImagen()
        {
            this.imgFrontal = new Bitmap(2048, 1300);
            panelFrontal.DrawToBitmap(this.imgFrontal, new Rectangle(0, 0, this.imgFrontal.Width, this.imgFrontal.Height));
            this.imgFrontal.Save(nombreImgFrontal, ImageFormat.Jpeg);
            this.pbImgFrontal.Image = this.imgFrontal;

            this.imgTrasera = new Bitmap(2048, 1300);
            panelTrasera.DrawToBitmap(this.imgTrasera, new Rectangle(0, 0, this.imgTrasera.Width, this.imgTrasera.Height));
            this.imgTrasera.Save(nombreImgTrasera, ImageFormat.Jpeg);
            this.pbImgTrasera.Image = this.imgTrasera;


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Parent = pbLogo;
            label2.BackColor = Color.Transparent;
            
            lblDonador.Parent = lblTextoDonador;
            lblDonador.BackColor = Color.Transparent;
           
            lblSeñas.BackColor = Color.Transparent;
            Tab1.SelectedTab = tabTrasera;
            Tab1.SelectedTab = tabFrontal;
            Tab1.SelectedTab = tabVista;
            guardarImagen();
            Tab1.TabPages.Remove(tabTrasera);
            Tab1.TabPages.Remove(tabFrontal);
            
        }

        private void pbImgFrontal_MouseMove(object sender, MouseEventArgs e)
        {
            this.pbImgZoom.Image = this.imgFrontal;
        }

        private void pbImgFrontal_MouseLeave(object sender, EventArgs e)
        {
            this.pbImgZoom.Image = null;
                
        }

        private void pbImgTrasera_MouseMove(object sender, MouseEventArgs e)
        {
            this.pbImgZoom.Image = this.imgTrasera;
        }

        private void pbImgTrasera_MouseLeave(object sender, EventArgs e)
        {
            this.pbImgZoom.Image = null;

        }
    }
}
