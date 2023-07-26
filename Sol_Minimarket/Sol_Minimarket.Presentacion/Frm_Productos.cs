using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sol_Minimarket.Entidades;
using Sol_Minimarket.Negocio;

namespace Sol_Minimarket.Presentacion
{
    public partial class Frm_Productos : Form
    {
        public Frm_Productos()
        {
            InitializeComponent();
        }

        #region "Mis Variables"
        int Estadoguardar = 0; //Sin ninguna Acción
        int Codigo_al = 0;
        #endregion

        #region "Mis Métodos"

        private void Formato_al()
        {
            Dgv_principal.Columns[0].Width = 100;
            Dgv_principal.Columns[0].HeaderText = "CÓDIGO_AL";
            Dgv_principal.Columns[1].Width = 300;
            Dgv_principal.Columns[1].HeaderText = "ALMACÉN";
        }    

        private void Listado_al(string cTexto)
        {
            try
            {
                Dgv_principal.DataSource = N_Almacenes.Listado_al(cTexto);
                this.Formato_al();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void Estado_Botonesprincipales(bool lEstado)
        {
            this.Btn_nuevo.Enabled = lEstado;
            this.Btn_actualizar.Enabled = lEstado;
            this.Btn_eliminar.Enabled = lEstado;
            this.Btn_reporte.Enabled = lEstado;
            this.Btn_salir.Enabled = lEstado;
        }

        private void Estado_Botonesprocesos(bool lEstado)
        {
            this.Btn_cancelar.Visible= lEstado;
            this.Btn_guardar.Visible= lEstado;
            this.Btn_retornar.Visible=!lEstado;
        }

        private void Selecciona_Item()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Dgv_principal.CurrentRow.Cells["codigo_al"].Value)))
            {
                MessageBox.Show("No se tiene información para visualizar", "Aviso del Sistema",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                this.Codigo_al = Convert.ToInt32(Dgv_principal.CurrentRow.Cells["codigo_al"].Value);
                Txt_descripcion_pr.Text = Convert.ToString(Dgv_principal.CurrentRow.Cells["descripcion_al"].Value);
            }
        }
        #endregion
        private void Frm_Productos_Load(object sender, EventArgs e)
        {
            this.Listado_al("%");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Btn_guardar_Click(object sender, EventArgs e)
        {
            if (Txt_descripcion_pr.Text == string.Empty)
            {
                MessageBox.Show("Falta ingresar datos requeridos (*)", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else //Se procedería a registrar la información
            {
                E_Almacenes oAl = new E_Almacenes();
                string Rpta = "";
                oAl.Codigo_al = this.Codigo_al;
                oAl.Descripcion_al = Txt_descripcion_pr.Text.Trim();
                Rpta = N_Almacenes.Guardar_al(Estadoguardar, oAl);
                if (Rpta == "OK")
                {
                    this.Listado_al("%");
                    MessageBox.Show("Los datos se guardaron correctamente", "Aviso del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Estadoguardar = 0; //Sin ninguna acción
                    this.Estado_Botonesprincipales(true);
                    this.Estado_Botonesprocesos(false);
                    Txt_descripcion_pr.Text = "";
                    Txt_descripcion_pr.ReadOnly = true;
                    Tbp_principal.SelectedIndex= 0;
                    this.Codigo_al = 0;
                }
                else
                {
                    MessageBox.Show(Rpta, "Aviso del sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Btn_nuevo_Click(object sender, EventArgs e)
        {
            Estadoguardar = 1; //Nuevo Registro
            this.Estado_Botonesprincipales(false);
            this.Estado_Botonesprocesos(true);
            Txt_descripcion_pr.Text = "";
            Txt_descripcion_pr.ReadOnly = false;
            Tbp_principal.SelectedIndex = 1;
            Txt_descripcion_pr.Focus();
            

        }

        private void Btn_actualizar_Click(object sender, EventArgs e)
        {
            Estadoguardar = 2; //Actualizar Registro
            this.Estado_Botonesprincipales(false);
            this.Estado_Botonesprocesos(true);
            this.Selecciona_Item();
            Tbp_principal.SelectedIndex = 1;
            Txt_descripcion_pr.ReadOnly = false;
            Txt_descripcion_pr.Focus();
        }

        private void Btn_cancelar_Click(object sender, EventArgs e)
        {
            Estadoguardar = 0; //Sin ninguna acción
            this.Codigo_al = 0;
            Txt_descripcion_pr.Text = "";
            Txt_descripcion_pr.ReadOnly = true;
            this.Estado_Botonesprincipales(true);
            this.Estado_Botonesprocesos(false);
            Tbp_principal.SelectedIndex = 0;
        }

        private void Dgv_principal_DoubleClick(object sender, EventArgs e)
        {
            this.Selecciona_Item();
            this.Estado_Botonesprocesos(false);
            Tbp_principal.SelectedIndex = 1;

        }

        private void Btn_retornar_Click(object sender, EventArgs e)
        {
            this.Estado_Botonesprocesos(false);
            Tbp_principal.SelectedIndex = 0;
            this.Codigo_al = 0;
        }

        private void Btn_eliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Dgv_principal.CurrentRow.Cells["codigo_al"].Value)))
            {
                MessageBox.Show("No se tiene información para visualizar", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult Opcion;
                Opcion = MessageBox.Show("¿Estás seguro de eliminar el registro seleccionado?","Aviso del Sistema",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if(Opcion == DialogResult.Yes)
                {
                    string Rpta = "";
                    this.Codigo_al = Convert.ToInt32(Dgv_principal.CurrentRow.Cells["codigo_al"].Value);
                    Rpta = N_Almacenes.Eliminar_al(this.Codigo_al);
                    if (Rpta.Equals("OK"))
                    {
                        this.Listado_al("%");
                        MessageBox.Show("Registro Eliminado", "Aviso del sistema", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.Codigo_al = 0;
                    }
                }

            }

        }

        private void Btn_buscar_Click(object sender, EventArgs e)
        {
            this.Listado_al(Txt_buscar.Text.Trim());
        }

        private void Btn_reporte_Click(object sender, EventArgs e)
        {
            Reportes.Frm_Rpt_Almacenes oRpt3 = new Reportes.Frm_Rpt_Almacenes();
            oRpt3.txt_p1.Text = Txt_buscar.Text;
            oRpt3.ShowDialog();
        }

        private void Btn_salir_Click(object sender, EventArgs e)
        {
            DialogResult Opcion;
            Opcion = MessageBox.Show("¿Desea Salir del Sistema?", "Aviso del Sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (Opcion == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
