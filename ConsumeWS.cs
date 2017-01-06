using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Xml;
using System.Net.Http;
using System.Web;
using System.Collections;
using System.ComponentModel.DataAnnotations;
namespace ImpresionLicencias
{
    public class ConsumeWS
    {
        private Request objRequest;
        private String url = "http://wirecanary.com/licencia/app.php";
        private Usuario objUsuario;
        private Response objResponse;
        private List<DatosWS> lstDatos;

        public ConsumeWS()
        {
            this.objRequest = new Request();
            this.objResponse = new Response();
            login();
        }

        private void login()
        {
            this.objUsuario= new Usuario();
            objUsuario.user = "bond";
            objUsuario.pass = "6620b6b585264768253cf6f9e9e7a037d4a97b6bedde3499c1d071975155ec11ed8c205bcf1d3f07cc83f30036f76a14192104593262f1e37756d52eebfbecee";// SHA512("temporal007");
            string jsonUser = JsonConvert.SerializeObject(objUsuario);

            objRequest.cmd = "login";
            objRequest.log = "login";
            objRequest.origin = "Alexander";
            objRequest.token = "";
            objRequest.param = jsonUser;

            string jsonRequest = JsonConvert.SerializeObject(objRequest);

            String response = HttpPostRequest(url, JsonToDictionary(jsonRequest,"request"));

            this.objResponse = JsonConvert.DeserializeObject<Response>(response);
            String token = objResponse.data.Split(':')[1].Substring(1);
            token = token.Substring(0, token.Length - 2);
            this.objUsuario.token = token;
        }

        public void actualizarLicencia(int idLicencia)
        {
            ejecutarConsulta("UPDATE licencia set estatus ='impresa' WHERE idLicencias = "+idLicencia);
            ejecutarConsulta("UPDATE turno set estatus='entrega' WHERE idTramite = " + idLicencia);

        }
        public List<verLicencias> obtenerLicencias()
        {
            ejecutarConsulta("SELECT L.idLicencias, L.numero, P.idPersona, P.nombres, P.primerAp, P.segundoAp, TP.descripcion as TipoLicencia ,L.fechaExpedicion,L.fechaExpiracion,P.RFC, ID.nombreCalle, ID.numeroExterior, ID.Colonia,getMunicipioByCveMunCveEnt(ID.cveMun,ID.cveEnt) as Municipio ,ID.codigoPostal,getEstadoByCveEnt(ID.cveEnt) as Estado, DE.tipoSangre, DE.estatura, DE.colorOjos, DE.donaOrganos, DE.colorCabello, DE.senasParticulares,CE.nombre as contacto, CE.telefeno as telContacto FROM licencia L INNER JOIN tipoLicencia TP ON L.idTipoLicencia = TP.idTipoLicencia INNER JOIN persona P ON L.idPersona = P.idPersona INNER JOIN turno T on T.idTramite = L.idLicencias AND T.estatus = 'pago' INNER JOIN contacto_emergencia CE on P.idPersona = CE.idPersona INNER JOIN persona_domicilio PD ON PD.idPersona = P.idPersona INNER JOIN inegi_domicilio ID on ID.idDomicilio = PD.idDomicilio INNER JOIN persona_datos_extras DE ON DE.idPersona = P.idPersona WHERE L.estatus='enTramite'");
            String[] arr;
            List<verLicencias> lstLicencias =  new System.Collections.Generic.List<verLicencias>();
            foreach (var item in this.lstDatos[0].recordset)
            {
                arr = item.ToString().Replace('\n', ' ').Trim().Replace('\r', ' ').Trim().Replace('\"', ' ').Trim().Replace('[', ' ').Trim().Replace(']', ' ').Trim().Replace('\t', ' ').Trim().Split(',');
                lstLicencias.Add(new verLicencias
                {
                    idLicencias =Convert.ToInt32( arr[0]),
                    numero = arr[1],
                    nombres = arr[3],
                    primerAp = arr[4],
                    segundoAp = arr[5],
                    idPersona =Convert.ToInt32( arr[2]),
                    TipoLicencia = arr[6],
                    fechaExpedicion = DateTime.Parse(arr[7]),
                    fechaExpira = DateTime.Parse(arr[8]),
                    RFC = arr[9],
                    calle = arr[10],
                    numeroCalle = arr[11],
                    colonia = arr[12],
                    municipio =  arr[13],
                    CP =  arr[14],
                    estado = arr[15],
                    sangre = arr[16],
                    estatura = Convert.ToInt32( arr[17]),
                    ojos = arr[18],
                    donador = arr[19].Equals("1")?true:false,
                    cabello = arr[20],
                    señas = arr[21],
                    contacto = arr[22],
                    telContacto = arr[23]
                    
                });
            }
            return lstLicencias;
        }

        private void ejecutarConsulta(String query)
        {
            try
            {
                this.objRequest.cmd = "query";
                this.objRequest.token = this.objUsuario.token;
                Query objQuery = new Query();
                objQuery.add(query);
                this.objRequest.param = JsonConvert.SerializeObject(objQuery);
                string json = JsonConvert.SerializeObject(objRequest);
                string mensaje = HttpPostRequest(url, JsonToDictionary(json, "request"));
                this.objResponse = JsonConvert.DeserializeObject<Response>(mensaje);
                this.lstDatos = (List<DatosWS>)JsonConvert.DeserializeObject(objResponse.data, typeof(List<DatosWS>));
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }

            
        }

        private string HttpPostRequest(string url, Dictionary<string, string> postParameters)
        {
            string postData = "";

            foreach (string key in postParameters.Keys)
            {
                postData += HttpUtility.UrlEncode(key) + "="
                      + HttpUtility.UrlEncode(postParameters[key]) + "&";
            }

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            myHttpWebRequest.Method = "POST";

            byte[] data = Encoding.ASCII.GetBytes(postData);

            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.ContentLength = data.Length;

            Stream requestStream = myHttpWebRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            Stream responseStream = myHttpWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

            string pageContent = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            responseStream.Close();

            myHttpWebResponse.Close();

            return pageContent;
        }

        /// <summary>
        /// Genera un diccionario con los parametros
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Dictionary<string, string> JsonToDictionary(string request, string paramName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add(paramName, request);
            return result;
        }

        /// <summary>
        /// No se usa porque no obtiene el valor adecuado
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string SHA512(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }
    }
    public class Persona
    {
        public int idPersona { get; set; }
        public string telCasa { get; set; }
    }
   
    public class verLicencias {
        [Display(AutoGenerateField = false)]
       // [HiddenInput(DisplayValue = false)]
        public int idLicencias { get; set; }
        [Display(Name = "Número de Licencia")]
        public string numero { get; set; }
        [Display(Name ="Nombre")]
        public string nombres { get; set; }
        [Display(Name = "Apellido Paterno")]
        public string primerAp { get; set; }
        [Display(Name = "Apellido Materno")]
        public string segundoAp { get; set; }
        [Display(AutoGenerateField = false)]
     //   [HiddenInput(DisplayValue = false)]
            
        public int idPersona { get; set; }
        [Display(Name = "Tipo de Licencia")]
        public string TipoLicencia { get; set; }
        public DateTime fechaExpedicion { get; set; }
        public DateTime fechaExpira { get; set; }
        public string  RFC { get; set; }

        public string calle { get; set; }
        public string numeroCalle { get; set; }
        public string colonia { get; set; }
        public string municipio { get; set; }
        public string CP { get; set; }
        public string estado { get; set; }


        public string sangre { get; set; }
        public int estatura { get; set; }
        public string ojos { get; set; }
        public string requerimiento { get; set; }
        public bool donador { get; set; }
        public string cabello { get; set; }
        public string señas { get; set; }
        public string contacto { get; set; }
        public string telContacto { get; set; }




    }

    public class DatosWS
    {
        public double time { get; set; }
        public Newtonsoft.Json.Linq.JArray recordset { get; set; }
        public int numRows { get; set; }
        public int numFields { get; set; }
        public int lastInsertId { get; set; }
        public string error { get; set; }
        public List<String> nameFields { get; set; }
    }
    public class Query
    {
        public List<String> querys { get; set; }
        public Query()
        {
            this.querys = new List<string>();
        }
        public void add(String query)
        {
            String strQ = query.Trim();
            strQ = strQ.Replace("SELECT", "XELECT");
            this.querys.Add(strQ);
        }
    }
    public class Usuario
    {
        public String user { get; set; }
        public String pass { get; set; }
        public String token { get; set; }
    }
    public class Response
    {

        public String result { get; set; }
        public String msg { get; set; }
        public String data { get; set; }
    }
    public class Request
    {
        public String cmd { get; set; }
        public String log { get; set; }
        public String token { get; set; }
        public String param { get; set; }
        public String origin { get; set; }



    }
}
