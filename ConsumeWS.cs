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
namespace ImpresionLicencias
{
    public class ConsumeWS
    {

    }

    public class DatosWS
    {
        public double time { get; set; }
        public ArrayList recordset { get; set; }
        public int numRows { get; set; }
        public int numFields { get; set; }
        public int lastInsertId { get; set; }
        public string error { get; set; }
        public ArrayList nameFields { get; set; }

        private void iniciar()
        {

            String url = "http://wirecanary.com/licencia/app.php";


            Usuario user = new Usuario();
            user.user = "bond";
            user.pass = "6620b6b585264768253cf6f9e9e7a037d4a97b6bedde3499c1d071975155ec11ed8c205bcf1d3f07cc83f30036f76a14192104593262f1e37756d52eebfbecee";// SHA512("temporal007");
            string jsonUser = JsonConvert.SerializeObject(user);
            //  MessageBox.Show(jsonUser);



            Request objRequest = new Request();
            objRequest.cmd = "login";
            objRequest.log = "login";
            objRequest.origin = "Alexander";
            objRequest.token = "";
            objRequest.param = jsonUser;

            string jsonRequest = JsonConvert.SerializeObject(objRequest);

            String response = HttpPostRequest(url, JsonToDictionary(jsonRequest));

            //JsonArray ids = jsonValue.GetObject().GetNamedArray("data);
            Response objResponse = JsonConvert.DeserializeObject<Response>(response);
            String token = objResponse.data.Split(':')[1].Substring(1);
            token = token.Substring(0, token.Length - 2);
            user.token = token;

            

            objRequest.cmd = "query";
            objRequest.token = user.token;
            Query query = new Query();
            query.add("SELECT * FROM usuario");
            objRequest.param = JsonConvert.SerializeObject(query);

            response = HttpPostRequest(url, JsonToDictionary(JsonConvert.SerializeObject(objRequest)));
            objResponse = JsonConvert.DeserializeObject<Response>(response);

            List<DatosWS> datos = (List<DatosWS>)JsonConvert.DeserializeObject(objResponse.data, typeof(List<DatosWS>));


        }

        public string HttpPostRequest(string url, Dictionary<string, string> postParameters)
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
        public Dictionary<string, string> JsonToDictionary(string request)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("request", request);
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
