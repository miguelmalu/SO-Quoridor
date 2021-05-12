using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        PictureBox seleccionado = null;

        int nForm;
        int ID;
        string usuario;

        Socket server;

        public Form2(int nForm, int ID, string usuario, Socket server)
        {
            InitializeComponent();
            this.nForm = nForm;
            this.server = server;
            this.ID = ID;
            this.usuario = usuario;
        }

        private void Saludar_Click(object sender, EventArgs e)
        {
            try
            {
                //Saludar
                string mensaje = "12/" + nForm + "/" + ID + "/" + usuario + ": Hola";
                // Enviamos al servidor la consulta
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            catch (Exception)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("Error en el Saludo");
                return;
            }
        }

        public void TomaRespuesta2(string mensaje)
        {
            MessageBox.Show(mensaje);
        }


        //QUORIDOR

        public void seleccion(object objeto)
        {
            PictureBox ficha = (PictureBox)objeto;
            seleccionado = ficha;
            seleccionado.BackColor = Color.Lime;
        }

        private void cuadroClick(object sender, MouseEventArgs e)
        {
            movimiento((PictureBox)sender);
        }

        private void movimiento(PictureBox cuadro)
        {
            if (seleccionado != null)
            {
                Point anterior = seleccionado.Location;
                seleccionado.Location = cuadro.Location;
                seleccionado.BackColor = Color.White;
                seleccionado = null;
            }
        }

        private void seleccionJugador1(object sender, MouseEventArgs e)
        {
            seleccion(sender);
        }

        private void seleccionJugador2(object sender, MouseEventArgs e)
        {
            seleccion(sender);
        }
    }
}
