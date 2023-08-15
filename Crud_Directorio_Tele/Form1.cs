using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace Crud_Directorio_Tele
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            pbImagen.Image.Save(ms, ImageFormat.Jpeg);
            byte[] aByte = ms.ToArray();
            
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                int Identificacion = int.Parse(txtIdentificacion.Text);
                String NombreCompleto = txtNombreCompleto.Text;
                long NumeroTelefonico = long.Parse(txtNumeroTelefonico.Text);
                String Cargo = txtCargo.Text;
                int NumeroOficina = int.Parse(txtNumeroOficina.Text);

                                
 
                if (Identificacion > 0 && NombreCompleto != "" && NumeroTelefonico > 0 && Cargo != "" && NumeroOficina > 0)
                {
                    string sql = "INSERT INTO directorio(Identificacion, NombreCompleto, NumeroTelefonico, Cargo,NumeroOficina,Fotografia ) VALUES ('" + Identificacion + "', '" + NombreCompleto + "', '" + NumeroTelefonico + "', '" + Cargo + "', '" + NumeroOficina + "', @imagen)";

                    try
                    {
                        MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                        comando.Parameters.AddWithValue("imagen", aByte);
                        comando.ExecuteNonQuery();
                        MessageBox.Show("Registro guardado");
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Error al guardar:" + ex.Message);
                    }
                    finally
                    {
                        conexionBD.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Debe completar todos los campos");
                }
            }
            catch (FormatException fex)
            {
                MessageBox.Show("Datos incorrectos:" + fex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {

            int Id = int.Parse(txtId.Text);
            int Identificacion = int.Parse(txtIdentificacion.Text);
            String NombreCompleto = txtNombreCompleto.Text;
            long NumeroTelefonico = long.Parse(txtNumeroTelefonico.Text);
            String Cargo = txtCargo.Text;
            int NumeroOficina = int.Parse(txtNumeroOficina.Text);

            MemoryStream ms = new MemoryStream();
            pbImagen.Image.Save(ms, ImageFormat.Jpeg);
            byte[] aByte = ms.ToArray();


            string sql = "UPDATE directorio SET Identificacion = '" + Identificacion + "', NombreCompleto = '" + NombreCompleto + "', NumeroTelefonico = '" + NumeroTelefonico + "', Cargo = '" + Cargo + "', NumeroOficina = '" + NumeroOficina + "', Fotografia= @imagen WHERE Id = '" + Id + "' ";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("@imagen", aByte);
                comando.ExecuteNonQuery();
                MessageBox.Show("Registro Actualizado");
                
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al actualizar:" + ex.Message);
            }
            finally
            {
                conexionBD.Close();
            }

        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(txtId.Text);

            string sql = "DELETE FROM directorio WHERE Id = '" + Id + "'";

            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.ExecuteNonQuery();
                MessageBox.Show("Registro eliminado");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al eliminar:" + ex.Message);
            }
            finally
            {
                conexionBD.Close();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtId.Text = "";
            txtIdentificacion.Text = "";
            txtNombreCompleto.Text = "";
            txtNumeroTelefonico.Text = "";
            txtCargo.Text = "";
            txtNumeroOficina.Text = "";
            pbImagen.Image = Crud_Directorio_Tele.Properties.Resources.agregar_imagen;
            
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            int Identificacion = int.Parse(txtIdentificacion.Text);
            MySqlDataReader reader = null;

            string sql = "SELECT Id, Identificacion, NombreCompleto, NumeroTelefonico, Cargo, NumeroOficina, Fotografia FROM directorio WHERE Identificacion LIKE '" + Identificacion + "' LIMIT 1";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {

                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        reader.Read();
                        txtId.Text = reader.GetString(0);
                        txtIdentificacion.Text = reader.GetString(1);
                        txtNombreCompleto.Text = reader.GetString(2);
                        txtNumeroTelefonico.Text = reader.GetString(3);
                        txtCargo.Text = reader.GetString(4);
                        txtNumeroOficina.Text = reader.GetString(5);
                        MemoryStream ms = new MemoryStream((byte[])reader["Fotografia"]);
                        Bitmap bm = new Bitmap(ms);
                        pbImagen.Image = bm;
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron registros");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar " + ex.Message);
            }
            finally
            {
                conexionBD.Close();
            }
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdSeleccionar = new OpenFileDialog(); 
            ofdSeleccionar.Filter = "Fotografía| *.jpg; *.png";
            ofdSeleccionar.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            ofdSeleccionar.Title = "Seleccionar imagen";

            if(ofdSeleccionar.ShowDialog() == DialogResult.OK)
            {
                pbImagen.Image = Image.FromFile(ofdSeleccionar.FileName);
            }

        }
    }
}
