using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;  
namespace Menu
{
    public partial class FrmPrincipalDiagnostico : Form
    {
        public FrmPrincipalDiagnostico()
        {
            InitializeComponent();
        }
        public void CargarDatos()
        {
            MySqlConnection cnn = new MySqlConnection(Conexion.cad);
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT TblConsultas.Folio,TblConsultas.Fecha,TblConsultas.Clave_Pacientes,TblPacientes.Nombre,TblPacientes.ApellidoPat,TblPacientes.ApellidoMat, TblPacientes.Domicilio,TblDoctores.Nombre as Doctor,TblConsultas.Hora, TblConsultas.Estatura, TblConsultas.Peso, TblConsultas.Temperatura, TblConsultas.Precion,TblConsultas.Sintomas,TblConsultas.Enfermedad,TblConsultas.FechaCita FROM TblConsultas INNER JOIN TblDoctores ON TblDoctores.Clave_Doctores=TblConsultas.Clave_Doctores INNER JOIN TblPacientes ON TblPacientes.Clave_Pacientes=TblConsultas.Clave_Pacientes",cnn);
               DataSet ds = new DataSet();
            da.Fill(ds);
            DgvDatos.DataSource = ds;
            DgvDatos.DataMember = ds.Tables[0].TableName;
        }

        private void TsBtnNuevo_Click(object sender, EventArgs e)
        {
            FrmHistorial frmhistorial = new FrmHistorial();
            frmhistorial.ShowDialog();
            CargarDatos();
            frmhistorial.AutoClave();
        }

        private void TsBtnEditar_Click(object sender, EventArgs e)
        {
            FrmHistorial n = new FrmHistorial();
            n.Text = "Editar registro de Diagnóstico de un Paciente";
            n.TipoEstado = false;
            n.Clave = Convert.ToInt32(DgvDatos.SelectedRows[0].Cells[0].Value.ToString());
            n.ShowDialog();
            CargarDatos();
            
            DgvDatos.Sort(DgvDatos.Columns[0], ListSortDirection.Ascending);
           
            
        }

        private void TsBtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            String cad1;
            errorProvider1.Clear();
            try
            {
                foreach (DataGridViewRow fila in DgvDatos.Rows)
                {
                    TxtBuscar.Text = TxtBuscar.Text.ToUpper();
                    cad1 = fila.Cells[3].Value.ToString();
                    if (cad1.StartsWith(TxtBuscar.Text) == true)
                    {
                        fila.Selected = true;
                        break;
                    }
                }
                TxtBuscar.SelectionStart = TxtBuscar.Text.Length;
            }
            catch
            {
                errorProvider1.SetError(TxtBuscar, "No existe el Nombre...!!");
                TxtBuscar.Focus();
            }
        }

        private void FrmPrincipalDiagnostico_Load(object sender, EventArgs e)
        {
            CargarDatos();
            DgvDatos.Sort(DgvDatos.Columns[0], ListSortDirection.Ascending);

        }

        private void EliminarPaciente()
        {
            MySqlConnection conexion = new MySqlConnection(Conexion.cad);
            conexion.Open();
            MySqlCommand comando = new MySqlCommand("delete from TblConsultas where Folio = " + this.DgvDatos.SelectedRows[0].Cells[0].Value, conexion);
            comando.ExecuteNonQuery();

        }
        private void TsBtnEliminar_Click(object sender, EventArgs e)
        {
            DialogResult respuesta;
            respuesta = MessageBox.Show("¿Seguro que desea eliminar el Paciente?\n", "Eliminar ...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                EliminarPaciente();
                CargarDatos();
                DgvDatos.Sort(DgvDatos.Columns[0], ListSortDirection.Ascending);
            }
        }

        private void TxtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
            base.OnKeyPress(e);

        }
    }
}
