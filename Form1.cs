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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpresionLicencias
{
    public partial class Form1 : Form
    {
        private readonly String ruta = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        private Bitmap imgFrontal;
        private Bitmap imgTrasera;
        private const String nombreImgFrontal= "licenciaFrontal.jpg";
        private const String nombreImgTrasera = "licenciaTrasera.jpg";
        private Artifacts artifacts = new Artifacts();
        private verLicencias objlicencia;
        private List<DocumentoLicencia> lstDocumentos;

        public String getRuta()
        {
            return ruta.Substring(6);
        }
        public Form1(verLicencias licencia, List<DocumentoLicencia> lstDocumentos)
        {
            InitializeComponent();
            this.objlicencia = licencia;
            this.lstDocumentos = lstDocumentos;
            this.lstDocumentos.ForEach(i => i.archivo = getImageFromURL(i));
            setValoresFrontal();
            setValoresTrasera();
        }

        /// <summary>
        /// Genera la imagen con microtexto a partir de una imagen local y agrega la ruta de la imagen creada a la lista de documentos de la licencia
        /// </summary>
        private void getMicroTextImage()
        {
            //string imageLocalPath = getImageFromURL(lstDocumentos.Where(i=> i.imagen==DocumentoLicencia.TipoImagen.Fotografia).First().archivo);
            
            //string microtextImage = getRuta() + "\\microText" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") +".bmp";
            //if (Jura.Jura.getImage(getImageFromURL(lstDocumentos.Where(i => i.tipoImagen == DocumentoLicencia.TipoImagen.Fotografia).First()), this.objlicencia.nombres.ToUpper(), this.objlicencia.primerAp.ToUpper().TrimStart() + " " + this.objlicencia.segundoAp.ToUpper().TrimStart(), this.objlicencia.numero.TrimStart(), microtextImage) == 0)
            //{
            //    this.lstDocumentos.Add(new DocumentoLicencia()
            //    {
            //        tipoImagen = DocumentoLicencia.TipoImagen.MicroTexto,
            //        archivo = microtextImage
            //    });
            //    this.pbFotoMicro.Image = Image.FromFile(this.lstDocumentos.Where(i=> i.tipoImagen == DocumentoLicencia.TipoImagen.MicroTexto).First().archivo);
            //}
            //else

            //{
            //    MessageBox.Show("Error al generar el microtexto", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
            //}

            
        }
        /// <summary>
        /// obtiene una copia local de una imagen a partir de una URL
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns></returns>
        public string getImageFromURL(DocumentoLicencia objDocumento)
        {
            string ruta = objDocumento.archivo;
            ruta = ruta.Trim();
            if (File.Exists(ruta))
                return ruta;
            

            string url = ruta;//"http://wirecanary.com/reloj/" + ruta;
            string[] arrNombre = ruta.Split('/');
            string nombre = arrNombre[arrNombre.Length - 1];
            string rutaLocal = this.getRuta() +"\\"+ objDocumento.tipoImagen.ToString() + nombre;
            rutaLocal = rutaLocal.TrimEnd().TrimStart();
            using (WebClient client = new WebClient())
            {
                client.DownloadFileAsync(new Uri(url), rutaLocal);
            }
            return rutaLocal.Replace('\\', '/');
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
            pbFotoPersona.Image = Image.FromFile(this.lstDocumentos.Where(i=>i.tipoImagen ==  DocumentoLicencia.TipoImagen.Fotografia).First().archivo);
            pbFotoMicro.Image = Image.FromFile(this.lstDocumentos.Where(i => i.tipoImagen == DocumentoLicencia.TipoImagen.Fotografia).First().archivo);
            pbFirmaPersona.Image = Image.FromFile(this.lstDocumentos.Where(i => i.tipoImagen == DocumentoLicencia.TipoImagen.Firma).First().archivo);
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
            pbHuella.Image = Image.FromFile(this.lstDocumentos.Where(i => i.tipoImagen == DocumentoLicencia.TipoImagen.Biometrico).First().archivo);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea imprimir la licencia?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                return;
            getMicroTextImage();
            guardarImagen();


            if (ValidateChildren())
            {
                CardModel card = new CardModel(artifacts);
                card.PrinterName = Sesion.impresora;

                if (File.Exists(nombreImgFrontal))
                    card.Color1 = Image.FromFile(nombreImgFrontal);
                if (File.Exists(nombreImgTrasera))
                    card.Color2 = Image.FromFile(nombreImgTrasera);

                card.DatosRFID = this.objlicencia.getDatosLicencia();
                card.Fotografia = getBase64StringByImagen(pbFotoPersona.Image);
                PrintService.Print(card);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
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

        

        public String getBase64StringByImagen(Image imageIn)
        {
            return Convert.ToBase64String(imageToByteArray(imageIn));
        }
        public byte[] imageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
    }
}
