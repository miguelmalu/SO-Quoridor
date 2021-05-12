using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Quoridor
{
    public partial class Form2 : Form
    {
        PictureBox seleccionado = null;

        public Form2()
        {
            InitializeComponent();
        }

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
