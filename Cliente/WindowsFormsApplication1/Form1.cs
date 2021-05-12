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

        string usuario;

        delegate void DelegadoParaPonerTexto(string texto);

        List<Form2> formularios = new List<Form2>();

        List<string> conectados = new List<string>();
        List<string> invitados = new List<string>();
        int numSeleccionados = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void ListaConectados(string mensaje)
        {
            string[] trozos = mensaje.Split('/');
            int numero = Convert.ToInt32(trozos[1]);
            if (numero != 0)
            {
                ConectadosGrid.ColumnCount = 1;
                ConectadosGrid.RowCount = numero;
                for (int i = 2; i < trozos.Length; i++)
                {
                    mensaje = trozos[i].Split('\0')[0];
                    ConectadosGrid.Rows[i - 2].Cells[0].Value = mensaje;
                }
            }
        }

        private void PonerEnMarchaFormulario(int ID)
        {
            int cont = formularios.Count;
            Form2 f = new Form2(cont, ID, usuario, server);
            formularios.Add(f);
            f.ShowDialog();
        }

        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                //Limpio el mensaje
                string respuesta = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                string[] trozos = respuesta.Split('/');
                int codigo = Convert.ToInt32(trozos[0]);
                string mensaje;
                int nform;

                switch (codigo)
                {
                    case 1: //Registro
                        int codigo2 = Convert.ToInt32(trozos[1]);
                        if (codigo2 == 0)
                            MessageBox.Show(UsuarioBox.Text + " se ha registrado correctamente");
                        else if (codigo2 == -1)
                            MessageBox.Show(UsuarioBox.Text + " ya está en uso");
                        else if (codigo2 == -2)
                            MessageBox.Show("El nombre de usuario es muy largo");
                        break;
                    case 2: //Login
                        codigo2 = Convert.ToInt32(trozos[1]);
                        if (codigo2 == -3)
                            MessageBox.Show("Datos de acceso inválidos");
                        else {
                            MessageBox.Show("Datos de acceso correctos");
                            if (codigo2 == 1) {
                                MessageBox.Show("Has iniciado sesión como " + UsuarioBox.Text);
                                conectados.Add(UsuarioBox.Text);
                                this.usuario = UsuarioBox.Text;
                            } else if (codigo2 == 2)
                                MessageBox.Show("Ya habías iniciado sesión como " + UsuarioBox.Text + " en este cliente");
                            else if (codigo2 == 3)
                                MessageBox.Show("Ya habías iniciado sesión como " + UsuarioBox.Text + " en otro cliente");
                            else if (codigo2 == -1)
                                MessageBox.Show("No se ha podido iniciar sesión, la lista de conectados está llena");
                            else if (codigo2 == -2)
                                MessageBox.Show("En este cliente ya se había iniciado sesión con otro usuario");
                        }
                        break;
                    case 3: //Contraseña del usuario
                        mensaje = trozos[1].Split('\0')[0];
                        if (mensaje != "fail")
                            MessageBox.Show("Tu contraseña es: " + mensaje);
                        else
                            MessageBox.Show("No se ha encontrado el usuario");
                        break;
                    case 4: //Jugadores de la partida
                        mensaje = trozos[1].Split('\0')[0];
                        if (mensaje != "fail")
                        {
                            string jugadores = "Los jugadores en esta partida son: ";
                            for (int i = 1; i < (trozos.Length - 1); i++)
                            {
                                mensaje = trozos[i].Split('\0')[0];
                                jugadores = jugadores + mensaje + ", ";
                            }
                            jugadores = jugadores.Remove(jugadores.Length - 2);
                            MessageBox.Show(jugadores);
                        }
                        else
                            MessageBox.Show("No se ha encontrado la partida");
                        break;
                    case 5: //Ganador de la partida
                        mensaje = trozos[1].Split('\0')[0];
                        if (mensaje != "fail")
                            MessageBox.Show("El ganador de la partida es: " + mensaje);
                        else
                            MessageBox.Show("No se ha encontrado la partida");
                        break;
                    case 6: //Duración de la partida
                        int tiempo = Convert.ToInt32(trozos[1]);
                        if (trozos[1] != "fail")
                            MessageBox.Show("La duración de la partida es de: " + tiempo + " minutos");
                        else
                            MessageBox.Show("No se ha encontrado la partida");
                        break;
                    case 7: //Ganador más rápido
                        mensaje = trozos[1].Split('\0')[0];
                        if (trozos[1] != "fail")
                            MessageBox.Show("El ganador más rápdio es: " + mensaje);
                        else
                            MessageBox.Show("No se han obtenido datos en la consulta");
                        break;
                    case 8: //Notificación de Lista de Conectados
                        //Haz tu lo que no me dejas hacer a mi
                        DelegadoParaPonerTexto delegado = new DelegadoParaPonerTexto(ListaConectados);
                        ConectadosGrid.Invoke(delegado, new object[] { respuesta });
                        break;
                    case 9: //Notificación de Invitar
                        mensaje = trozos[1].Split('\0')[0];
                        if (mensaje == "-4")
                            MessageBox.Show("No se ha seleccionado ningún jugador");
                        else if (mensaje == "-1")
                            MessageBox.Show("No se ha encontrado a un invitado");
                        else if (mensaje == "-2")
                            MessageBox.Show("No hay jugadores suficientes");
                        else if (mensaje == "-3")
                            MessageBox.Show("No se permiten más partidas");
                        else
                        {
                            int ID = Convert.ToInt32(trozos[2]);
                            string pregunta = mensaje + " te ha invitado a la partida " + ID;
                            DialogResult dialogo = MessageBox.Show(pregunta, "Invitación", MessageBoxButtons.YesNo);
                            if (dialogo == DialogResult.Yes)
                                mensaje = "10/" + ID + "/YES";
                            else
                                mensaje = "10/" + ID + "/NO";
                            // Enviamos al servidor la consulta
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                            server.Send(msg);
                        }
                        break;
                    case 10: //Notificación de Respuesta Invitación
                        mensaje = trozos[2].Split('\0')[0];
                        if (mensaje == "TRUE")
                        {
                            MessageBox.Show("La partida iniciará en breves");
                            int ID = Convert.ToInt32(trozos[1]);
                            //Ahora se tendría que iniciar el Form2
                            ThreadStart ts = delegate { PonerEnMarchaFormulario(ID); };
                            Thread T = new Thread(ts);
                            T.Start();
                        }
                        else if (mensaje == "FALSE")
                            MessageBox.Show("No se cumplen los requisitos para iniciar un partida");
                        break;
                    //case 11: //Notificación de Jugada
                    //    break;
                    case 12: //Notificación de Mensaje
                        nform = Convert.ToInt32(trozos[1]);
                        mensaje = trozos[3].Split('\0')[0];
                        formularios[nform].TomaRespuesta2(mensaje);
                        //MessageBox.Show(mensaje);
                        break;
                }
            }
        }

        private void Conectar_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            //IPAddress direc = IPAddress.Parse("192.168.56.102"); //Entorno Desarrollo
            //IPEndPoint ipep = new IPEndPoint(direc, 9046); //Entorno Desarrollo
            IPAddress direc = IPAddress.Parse("147.83.117.22"); //Entorno Producción
            IPEndPoint ipep = new IPEndPoint(direc, 50057); //Entorno Producción
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
                conectados.Remove(UsuarioBox.Text); //Posible error
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


        private void ConectadosGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int fila = e.RowIndex;
            ////for (int i=0; i < invitados.Count; i++) {
            ////    if (invitados[i] != ConectadosGrid.CurrentCell.Value.ToString())
            ////    {
            ////        if (invitados[i] != usuario)
            ////        {
            //            ConectadosGrid.Rows[fila].DefaultCellStyle.BackColor = Color.Green;
            //            invitados.Add(ConectadosGrid.CurrentCell.Value.ToString());
            //            numSeleccionados++;
            //        } else
            //            MessageBox.Show("Eres el anfitrión, no te puedes invitar");
            //    } else {
            //        ConectadosGrid.Rows[fila].DefaultCellStyle.BackColor = Color.White;
            //        invitados.Remove(ConectadosGrid.CurrentCell.Value.ToString());
            //        numSeleccionados--;
            //        MessageBox.Show("Se ha eliminado a " + ConectadosGrid.CurrentCell.Value.ToString() + " de la lista de invitados");
            //    }

            //}
            ConectadosGrid.Rows[fila].DefaultCellStyle.BackColor = Color.Green;
            invitados.Add(ConectadosGrid.CurrentCell.Value.ToString());
            numSeleccionados++;
        }

        private void Invitar_Click(object sender, EventArgs e)
        {
            try
            {
                //Invitar
                string mensaje = "9/";
                if (numSeleccionados >= 1)
                {
                    for (int i = 0; i < numSeleccionados; i++)
                        mensaje = mensaje + invitados[i] + ",";
                    mensaje = mensaje.Remove(mensaje.Length - 1);
                    // Enviamos al servidor la consulta
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    //Limpiamos selección
                    ConectadosGrid.ClearSelection();
                    ConectadosGrid.RowsDefaultCellStyle.BackColor = Color.White; //No va
                    invitados.Clear();
                    numSeleccionados = 0;
                } else
                    MessageBox.Show("No se ha seleccionado ningún jugador");
            }
            catch (Exception)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("Error en las invitaciones");
                return;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Mensaje de desconexión
            string mensaje = "0/";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos
            atender.Abort();
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }
    }
}
