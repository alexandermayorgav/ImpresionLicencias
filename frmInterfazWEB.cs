using Print;
using Print.Printing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpresionLicencias
{
    public partial class frmInterfazWEB : Form
    {

        private Socket sc_listener;
        private byte[] buffer;
        private readonly int puerto = 1500;
        private Artifacts artifacts = new Artifacts();
        private EstatusImpresora estatusImpresora;
        private EstatusImpresion estatusImpresion;
        private readonly string serverName = "http://localhost/ImpresionWeb/";
        //private readonly string[] separador = new string[] { ".jpg" };
        public enum EstatusImpresora {
            Conectada,
            Desconectada
        }
        public enum EstatusImpresion
        {
            OK, Error       
        }
        public frmInterfazWEB()
        {
            InitializeComponent();
            getConfiguracion();
            if (IsPrinterOnline(Sesion.impresora))
            {
                pbImpresora.Image = ImpresionLicencias.Properties.Resources.printerON;
                lblImpresora.Text = "Impresora "+ Sesion.impresora  + " conectada";
                this.estatusImpresora = EstatusImpresora.Conectada;
            }
            else
            {
                pbImpresora.Image = ImpresionLicencias.Properties.Resources.printerOFF;
                lblImpresora.Text = "Impresora " +Sesion.impresora + " desconectada";
                this.estatusImpresora = EstatusImpresora.Desconectada;
            }
        }

        private void frmInterfazWEB_Load(object sender, EventArgs e)
        {
            
            sc_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ip_local = new IPEndPoint(IPAddress.Loopback, puerto);
            sc_listener.Bind(ip_local);
            sc_listener.Listen(10);

            AsyncCallback callback = new AsyncCallback(procces_incoming_socket);
            sc_listener.BeginAccept(callback, sc_listener);
            
        }

        private void getConfiguracion()
        { 
            Sesion.impresora = System.Configuration.ConfigurationManager.AppSettings["impresora"];

        }


        private void imprimirLicenciaByIdLicencia(int idLicencia)
        {
            this.estatusImpresion = EstatusImpresion.Error;
            ConsumeWS objWS = new ConsumeWS();
            verLicencias objLicencia = objWS.obtenerLicenciasByIdLicencia(idLicencia);
            objWS.getDocumentosLicencia(objLicencia.idPersona);
            Form1 frm = new Form1(objLicencia, objWS.getDocumentosLicencia(objLicencia.idPersona));
            if( frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.estatusImpresion = EstatusImpresion.OK;
        }

        void procces_incoming_socket(IAsyncResult socket_object)
        {
            Socket sc_listener = ((Socket)socket_object.AsyncState).EndAccept(socket_object);

            AsyncCallback receive = new AsyncCallback(receive_data);
            buffer = new byte[100];
            sc_listener.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, receive, sc_listener);
        }

        public void receive_data(IAsyncResult socket)
        {
            // the system need to wait so i make a loop when it gets data 
            //i end the loop by flag=false
            bool flag = true;
            Socket re_socket = ((Socket)socket.AsyncState);

            string respuesta = string.Empty;
            if (this.estatusImpresora == EstatusImpresora.Desconectada)
            {
                flag = false;
                respuesta = lblImpresora.Text;
            }

            while (flag)
            {
                try
                {
                    int bytes_recieved = re_socket.EndReceive(socket);
                    string data = UTF8Encoding.UTF8.GetString(buffer);

                    imprimirLicenciaByIdLicencia(Convert.ToInt32(data));
                    flag = false;
                    respuesta = this.estatusImpresion.ToString();
                }
                catch (Exception ex)
                {

                    respuesta ="Error. "+  ex.Message+ " " + ex.StackTrace;
                    flag = false;
                }
                
            }
           
            byte[] buffers = UTF8Encoding.UTF8.GetBytes(respuesta);
            re_socket.Send(buffers);
            // if the socket is not closed php will load for maximum required time and then error
            re_socket.Close();
            //start for next listening (O-0)
            AsyncCallback callback = new AsyncCallback(procces_incoming_socket);
            sc_listener.BeginAccept(callback, sc_listener);
        }

        /// <summary>
        /// Genera un objeto Image a partir de una URL
        /// </summary>
        /// <param name="archivo">nombre del archivo ubicado en el servidor</param>
        /// <returns>objecto Image</returns>
        private Image imagenFromURL(string archivo)
        {
            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(this.serverName + archivo + ".jpg");
            MemoryStream ms = new MemoryStream(bytes);
            return System.Drawing.Image.FromStream(ms);
        }

        /// <summary>
        ///Metodo para comprobar si una impresora esta online o offline
        /// </summary>
        /// <param name="printerName">nombre de la impresora</param>
        /// <returns></returns>
        public bool IsPrinterOnline(string printerName)
        {
            string str = "";
            bool online = false;

            //set the scope of this search to the local machine
            ManagementScope scope = new ManagementScope(ManagementPath.DefaultPath);
            //connect to the machine
            scope.Connect();

            //query for the ManagementObjectSearcher
            SelectQuery query = new SelectQuery("select * from Win32_Printer");

            ManagementClass m = new ManagementClass("Win32_Printer");

            ManagementObjectSearcher obj = new ManagementObjectSearcher(scope, query);

            //get each instance from the ManagementObjectSearcher object
            using (ManagementObjectCollection printers = m.GetInstances())
                //now loop through each printer instance returned
                foreach (ManagementObject printer in printers)
                {
                    //first make sure we got something back
                    if (printer != null)
                    {
                        //get the current printer name in the loop
                        str = printer["Name"].ToString().ToLower();

                        //check if it matches the name provided
                        if (str.Equals(printerName.ToLower()))
                        {
                            //since we found a match check it's status
                            if (printer["WorkOffline"].ToString().ToLower().Equals("true") || printer["PrinterStatus"].Equals(7))
                                //it's offline
                                online = false;
                            else
                                //it's online
                                online = true;
                        }
                    }
                    else
                        throw new Exception("No printers were found");
                }
            return online;
        }

    }
}
