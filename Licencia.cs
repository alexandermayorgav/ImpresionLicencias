using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpresionLicencias
{
    public class Licencia
    {
        //FRONTAL
        public String imgFirma { get; set; }
        public String imgLogo { get; set; }
        public String Secretario { get; set; }

        public String nombre { get; set; }
        public String apellido { get; set; }
        public String RFC { get; set; }
        public String imgPersona { get; set; }
        public String imgPersonaMicro { get; set; }
        public String calle { get; set; }
        public String colonia { get; set; }
        public String municipio { get; set; }
        public String estado { get; set; }
        public String fechaEmision { get; set; }
        public String fechaExpira { get; set; }
        public String tipoLicencia { get; set; }
        public String numeroLicencia { get; set; }
        //TRASERA
        public String contactoNombre { get; set; }
        public String contactoTel { get; set; }
        public String donador { get; set; }
        public String sangre { get; set; }
        public String estatura { get; set; }
        public String ojos { get; set; }
        public String cabello { get; set; }
        public String señas { get; set; }
        public String imgfirmaPersona { get; set; }
        public String imgHuella { get; set; }


    }
}
