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
        private List<verLicencias> lstLicencias;

        public frmConsulta()
        {
            InitializeComponent();
            this.objWS = new ConsumeWS();
            obtenerDatos();
        }

        private void frmConsulta_Load(object sender, EventArgs e)
        {
        
        }
        private void mostrarDatos(List<verLicencias> datos)
        {
            
            gridDatos.Rows.Clear();
            int row = 0;
            foreach (var item in datos)
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
        private void obtenerDatos()
        {
            lstLicencias = this.objWS.obtenerLicencias();
            mostrarDatos(lstLicencias);
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int idLicencia = Convert.ToInt32( gridDatos.CurrentRow.Cells["idLicencia"].Value);
            //verLicencias o = this.objVerLicencias.Where(i => i.idLicencias == idLicencia).ToList().First();
            Form1 frm = new Form1(this.lstLicencias.Where(i=> i.idLicencias == idLicencia).ToList().First(),null);
            frm.ShowDialog();
            obtenerDatos();

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtBuscar.Text.Length != 0)
            {
                mostrarDatos(lstLicencias.Where(i => i.numero.Trim() == txtBuscar.Text || i.nombres.Trim().Contains(txtBuscar.Text) || i.primerAp.Contains(txtBuscar.Text) || i.segundoAp.Contains(txtBuscar.Text)).ToList());
            }
            else
                mostrarDatos(lstLicencias);
        }
    }
}
