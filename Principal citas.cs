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
    public partial class FrmPrincipalCitas : Form
    {
        public FrmPrincipalCitas()
        {
            InitializeComponent();
        }

        private void CargarDatos()
        {
            MySqlConnection cnn = new MySqlConnection(Conexion.cad);
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT TblCitas.FolioCitas,TblCitas.Clave_Pacientes,TblCitas.Nombre,TblCitas.ApellidoPat,TblDoctores.Nombre as Doctor,TblCitas.Hora,TblCitas.Estatura,TblCitas.Peso,TblCitas.Temperatura,TblCitas.Precion,TblCitas.fecha FROM TblCitas INNER JOIN TblDoctores ON TblDoctores.Clave_Doctores=TblCitas.Clave_Doctores ", cnn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DgvDatos.DataSource = ds;
            DgvDatos.DataMember = ds.Tables[0].TableName;
        }


        private void TsBtnNuevo_Click(object sender, EventArgs e)
        {
            FrmCitas frmcitas = new FrmCitas();
           
            frmcitas.ShowDialog();
            CargarDatos();
        }

        private void TsBtnEditar_Click(object sender, EventArgs e)
        {

            FrmCitas n = new FrmCitas();
            n.Text = "Editar registro de la Cita";
            n.TipoEstado = false;
            n.ShowDialog();
            CargarDatos();
            DgvDatos.Sort(DgvDatos.Columns[0], ListSortDirection.Ascending);
        }

        private void FrmPrincipalCitas_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void EliminarCita()
        {
            MySqlConnection conexion = new MySqlConnection(Conexion.cad);
            conexion.Open();
            MySqlCommand comando = new MySqlCommand("delete from TblCitas where FolioCitas = " + this.DgvDatos.SelectedRows[0].Cells[0].Value, conexion);
            comando.ExecuteNonQuery();

        }

        private void TsBtnEliminar_Click(object sender, EventArgs e)
        {
            DialogResult respuesta;
            respuesta = MessageBox.Show("¿Seguro que desea eliminar la cita seleccionada?\n", "Eliminar ...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                EliminarCita();
                CargarDatos();
                DgvDatos.Sort(DgvDatos.Columns[0], ListSortDirection.Ascending);
            }

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
                //ncol = DgvDatos.SortedColumn.Index;
                foreach (DataGridViewRow fila in DgvDatos.Rows)
                {
                    TxtBuscar.Text = TxtBuscar.Text.ToUpper();
                    cad1 = fila.Cells[2].Value.ToString();
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
