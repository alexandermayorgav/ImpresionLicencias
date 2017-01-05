using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpresionLicencias
{
    public partial class frmConsulta : Form
    {
        private ConsumeWS objWS;
        private List<verLicencias> objVerLicencias;

        public frmConsulta()
        {
            InitializeComponent();
            this.objWS = new ConsumeWS();
            obtenerDatos();
        }

        private void frmConsulta_Load(object sender, EventArgs e)
        {
        
        }
        private void obtenerDatos()
        {
            objVerLicencias = this.objWS.obtenerLicencias();
            gridDatos.Rows.Clear();
            int row = 0;
            foreach (var item in objVerLicencias)
            {
                this.gridDatos.Rows.Add();
                this.gridDatos.Rows[row].Cells["numero"].Value = item.numero;
                this.gridDatos.Rows[row].Cells["nombre"].Value = item.nombres.ToUpper();
                this.gridDatos.Rows[row].Cells["apellidos"].Value = item.primerAp.ToUpper() + " " + item.segundoAp.ToUpper();
                this.gridDatos.Rows[row].Cells["tipoLicencia"].Value = item.TipoLicencia.ToUpper();
                this.gridDatos.Rows[row].Cells["idLicencia"].Value = item.idLicencias;
                row++;
            } 
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            obtenerDatos();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int idLicencia = Convert.ToInt32( gridDatos.CurrentRow.Cells["idLicencia"].Value);
            //verLicencias o = this.objVerLicencias.Where(i => i.idLicencias == idLicencia).ToList().First();
            Form1 frm = new Form1(this.objVerLicencias.Where(i=> i.idLicencias == idLicencia).ToList().First());
            frm.ShowDialog();
        }
    }
}
