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
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Socket server;
        Thread atender;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; //Necesario para que los elementos de los formularios puedan ser
            //accedidos desde threads diferentes a los que los crearon
        }

        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string[] trozos = Encoding.ASCII.GetString(msg2).Split('/');
                int codigo = Convert.ToInt32(trozos[0]);
                string mensaje = trozos[1].Split('\0')[0];

                switch (codigo)
                {
                    case 1: //Registro
                        if (mensaje == "0")
                            MessageBox.Show(UsuarioBox.Text + " se ha registrado correctamente");
                        else if (mensaje == "-1")
                            MessageBox.Show(UsuarioBox.Text + " ya está en uso");
                        else if (mensaje == "-2")
                            MessageBox.Show("El nombre de usuario es muy largo");
                        break;
                    case 2: //Login
                        if (mensaje == "0")
                            MessageBox.Show("Has iniciado sesión como " + UsuarioBox.Text);
                        else if (mensaje == "1")
                            MessageBox.Show("Ya habías iniciado sesión como " + UsuarioBox.Text + " en este cliente");
                        else if (mensaje == "2")
                            MessageBox.Show("Ya habías iniciado sesión como " + UsuarioBox.Text + " en otro cliente");
                        else if (mensaje == "-1")
                            MessageBox.Show("No se ha podido iniciar sesión, la lista de conectados está llena");
                        else if (mensaje == "-2")
                            MessageBox.Show("En este cliente ya se había iniciado sesión con otro usuario");
                        else if (mensaje == "-3")
                            MessageBox.Show("Datos de acceso inválidos");
                        break;
                    case 3: //Contraseña del usuario
                        if (mensaje != "fail")
                            MessageBox.Show("Tu contraseña es: " + mensaje);
                        else
                            MessageBox.Show("No se ha encontrado el usuario");
                        break;
                    case 4: //Jugadores de la partida
                        if (mensaje != "fail")
                        {
                            string jugadores = "Los jugadores en esta partida son: ";
                            for (int i = 1; i < (trozos.Length - 1); i++)
                                jugadores = jugadores + trozos[i] + ", ";
                            jugadores = jugadores.Remove(jugadores.Length - 2);
                            MessageBox.Show(jugadores);
                        }
                        else
                            MessageBox.Show("No se ha encontrado la partida");
                        break;
                    case 5: //Ganador de la partida
                        if (mensaje != "fail")
                            MessageBox.Show("El ganador de la partida es: " + mensaje);
                        else
                            MessageBox.Show("No se ha encontrado la partida");
                        break;
                    case 6: //Duración de la partida
                        int tiempo = Convert.ToInt32(mensaje);
                        if (mensaje != "fail")
                            MessageBox.Show("La duración de la partida es de: " + tiempo + " minutos");
                        else
                            MessageBox.Show("No se ha encontrado la partida");
                        break;
                    case 7: //Ganador más rápido
                        if (mensaje != "fail")
                            MessageBox.Show("El ganador más rápdio es: " + mensaje);
                        else
                            MessageBox.Show("No se han obtenido datos en la consulta");
                        break;
                    case 8: //Notificación de Lista de Conectados
                        int numero = Convert.ToInt32(mensaje);
                        if (numero != 0)
                        {
                            ConectadosGrid.ColumnCount = 1;
                            ConectadosGrid.RowCount = numero;
                            for (int i = 2; i < trozos.Length; i++)
                                ConectadosGrid.Rows[i - 2].Cells[0].Value = trozos[i];
                        }
                        break;
                }
            }
        }

        private void Conectar_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            //IPAddress direc = IPAddress.Parse("192.168.56.102"); //Entorno Desarrollo
            //IPEndPoint ipep = new IPEndPoint(direc, 9080); //Entorno Desarrollo
            IPAddress direc = IPAddress.Parse("147.83.117.22"); //Entorno Producción
            IPEndPoint ipep = new IPEndPoint(direc, 50058); //Entorno Producción
            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                if (this.BackColor == Color.Green)
                    MessageBox.Show("Ya estás conectado");
                else {
                    this.BackColor = Color.Green;
                    MessageBox.Show("Conectado");
                    ConectadosGrid.Columns.Clear();
                    ConectadosGrid.Rows.Clear();
                }
                //Pongo en marcha el thread que atenderá los mensajes del servidor
                ThreadStart ts = delegate { AtenderServidor(); };
                atender = new Thread(ts);
                atender.Start();
            }
            catch (SocketException)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                this.BackColor = Color.Gray;
                MessageBox.Show("Error en la conexión con el servidor");
                return;
            }
        }

        private void Desconectar_Click(object sender, EventArgs e)
        {
            try
            {
                //Desconexión
                string mensaje = "0/";
                //Enviamos al servidor la consulta
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                //Nos desconectamos
                atender.Abort();
                this.BackColor = Color.Gray;
                MessageBox.Show("Desconectado");
                server.Shutdown(SocketShutdown.Both);
                server.Close();  
            }
            catch (Exception)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("Error al desconectarse del servidor");
                return;
            } 
        }

        private void PassCheckBox_Check(object sender, EventArgs e)
        {
            if (PassCheckBox.Checked)
                ContraseñaBox.UseSystemPasswordChar = false;
            else
                ContraseñaBox.UseSystemPasswordChar = true;
        }

        private void Registrarse_Click(object sender, EventArgs e)
        {
            try
            {
                if ((UsuarioBox.Text != "") && (ContraseñaBox.Text != ""))
                {
                    //Registro
                    string mensaje = "1/" + UsuarioBox.Text + "/" + ContraseñaBox.Text;
                    // Enviamos al servidor la consulta
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                    MessageBox.Show("Error en los campos de los datos");
            }
            catch (Exception)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("Error en el registro");
                return;
            }
        }

        private void Entrar_Click(object sender, EventArgs e)
        {
            try
            {
                if ((UsuarioBox.Text != "") && (ContraseñaBox.Text != ""))
                {
                    //Login
                    string mensaje = "2/" + UsuarioBox.Text + "/" + ContraseñaBox.Text;
                    // Enviamos al servidor la consulta
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                    MessageBox.Show("Error en los campos de los datos de Acceso");
            }
            catch (Exception)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("Error en el login");
                return;
            }
        }

        private void Consultar_Click(object sender, EventArgs e)
        {
            try
            {
                //Contraseña del usuario
                if (Contraseña.Checked)
                {
                    if (UsuarioBox.Text != "")
                    {
                        string mensaje = "3/" + UsuarioBox.Text;
                        // Enviamos al servidor la consulta
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }
                    else
                        MessageBox.Show("Error en el campo de datos: Usuario");
                }
                //Jugadores de la partida
                else if (Jugadores.Checked)
                {                 
                    if (PartidaBox.Text != "")
                    {
                        string mensaje = "4/" + PartidaBox.Text;
                        // Enviamos al servidor la consulta
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }
                    else
                        MessageBox.Show("Error en el campo de datos: Partida");
                }
                //Ganador de la partida
                else if (Ganador.Checked)
                {
                    if (PartidaBox.Text != "")
                    {
                        string mensaje = "5/" + PartidaBox.Text;
                        // Enviamos al servidor la consulta
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }
                    else
                        MessageBox.Show("Error en el campo de datos: Partida");
                }
                //Duración de la partida
                else if (Tiempo.Checked)
                { 
                    if (PartidaBox.Text != "")
                    {
                        string mensaje = "6/" + PartidaBox.Text;
                        // Enviamos al servidor la consulta
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }
                    else
                        MessageBox.Show("Error en el campo de datos: Partida");
                }
                //Ganador más rápido
                else if (Rápido.Checked)
                {
                    string mensaje = "7/";
                    // Enviamos al servidor la consulta
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
            }
            catch (Exception)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("Error en la petición");
                return;
            }
        }
    }
}
