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
        private verLicencias objlicencia;

        public Form1(verLicencias licencia)
        {
            InitializeComponent();
            this.objlicencia = licencia;
            setValoresFrontal();
            setValoresTrasera();
        }

        private void setValoresFrontal()
        {
            lblNombre.Text = this.objlicencia.nombres.ToUpper().Trim();
            lblApellidos.Text = this.objlicencia.primerAp.ToUpper().Trim() + " " + this.objlicencia.segundoAp.ToUpper().Trim();
            lblRFC.Text = this.objlicencia.RFC.Trim();
            lblDireccion.Text = this.objlicencia.calle.ToUpper().Trim() + " " + this.objlicencia.numeroCalle.Trim();
            lblColonia.Text = this.objlicencia.colonia.ToUpper().Trim();
            lblMunicipio.Text = this.objlicencia.municipio.Trim() + " " + this.objlicencia.CP.Trim();
            lblEstado.Text = this.objlicencia.estado.Trim();
            lblFechaExpedicion.Text = String.Format("{0:dd-MMM-yyyy}", this.objlicencia.fechaExpedicion).ToUpper().Trim();
            lblFechaVencimiento.Text = String.Format("{0:dd-MMM-yyyy}", this.objlicencia.fechaExpira).ToUpper().Trim();
            lblTipoLicencia.Text = this.objlicencia.TipoLicencia.Trim();
            lblNumeroLicencia.Text = this.objlicencia.numero.Trim();
        }

        private void setValoresTrasera()
        {
            String valor = "-";
            if (this.objlicencia.sangre.Contains("pos"))
		        valor="+";
            lblSangre.Text = this.objlicencia.sangre.Substring(0, 1).Trim() + valor;
            lblEstatura.Text = this.objlicencia.estatura.ToString().Trim();
            lblOjos.Text = this.objlicencia.ojos.Trim();
            lblSeñas.Text = this.objlicencia.señas.Trim();
            lblCabello.Text = this.objlicencia.cabello.Trim();
            lblContacto.Text = this.objlicencia.contacto.Trim();
            lblTelefonoContacto.Text = this.objlicencia.telContacto.Trim();
            lblTextoDonador.Text = "Manifiesto que " + (this.objlicencia.donador ? "SI " : "NO ") + lblTextoDonador.Text;
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
            if (MessageBox.Show("Desea activar la licencia?", "CONFIRMACIÓN", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)==DialogResult.OK)
            {
                new ConsumeWS().actualizarLicencia(this.objlicencia.idLicencias);
                this.Close();
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
            
           // lblDonador.Parent = lblTextoDonador;
           // lblDonador.BackColor = Color.Transparent;
           
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
