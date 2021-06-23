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
        bool YAdesconectado;

        string usuario;

        delegate void DelegadoParaListaConectados(string mensaje);
        delegate void DelegadoParaActualizarSesion();

        List<Form2> formularios = new List<Form2>();
        int[] partidas = new int[100]; //Relaciona el número de form con la ID de la partida

        List<string> conectados = new List<string>();
        List<string> invitados = new List<string>();
        int numSeleccionados = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SesionLbl.Text = "No se ha iniciado sesión";
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
            ConectadosGrid.ClearSelection();
        }

        private void ActualizarSesion()
        {
            if (usuario == "")
                SesionLbl.Text = "No se ha iniciado sesión";
            else
                SesionLbl.Text = "Conectado como: " + usuario;
        }

        private void PonerEnMarchaFormulario(int ID, string jugadores)
        {
            int cont = formularios.Count();
            partidas[ID] = formularios.Count();
            Form2 f = new Form2(cont, ID, usuario, server, jugadores);
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
                int ID;

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
                                usuario = UsuarioBox.Text;
                                DelegadoParaActualizarSesion delegado2 = new DelegadoParaActualizarSesion(ActualizarSesion);
                                SesionLbl.Invoke(delegado2);
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
                    case 14: //Salir
                        codigo2 = Convert.ToInt32(trozos[1]);
                        if (codigo2 == -1)
                            MessageBox.Show("No se había iniciado sesión");
                        else if (codigo2 == 1) {
                            MessageBox.Show(usuario + " , has cerrado sesión");
                            conectados.Remove(usuario);
                            usuario = "";
                            DelegadoParaActualizarSesion delegado2 = new DelegadoParaActualizarSesion(ActualizarSesion);
                            SesionLbl.Invoke(delegado2);
                        }
                        break;
                    case 15: //EliminarCuenta
                        codigo2 = Convert.ToInt32(trozos[1]);
                        if (codigo2 == -1)
                            MessageBox.Show("No se había iniciado sesión");
                        else if (codigo2 == -2)
                            MessageBox.Show("No se ha podido eliminado el usuario");
                        else if (codigo2 == -3)
                            MessageBox.Show("Se ha eliminado el usuario, " + usuario + ", pero no se ha podido cerrar sesión");
                        else if (codigo2 == 1) {
                            MessageBox.Show(usuario + ", has cerrado sesión y se ha eliminado el usuario");
                            conectados.Remove(usuario);
                            usuario = "";
                            DelegadoParaActualizarSesion delegado2 = new DelegadoParaActualizarSesion(ActualizarSesion);
                            SesionLbl.Invoke(delegado2);
                        }
                        break;
                    case 3: //Contraseña del usuario
                        mensaje = trozos[1].Split('\0')[0];
                        if (mensaje != "fail")
                            MessageBox.Show("Tu contraseña es: " + mensaje);
                        else
                            MessageBox.Show("No se ha encontrado el usuario");
                        break;
                    case 4: //Jugadores de la partida por ID
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
                    case 16: //Jugadores de la partida contra los que he jugado
                        mensaje = trozos[1].Split('\0')[0];
                        if (mensaje != "fail")
                        {
                            string jugadores = "Has jugado contra: ";
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
                    case 18: //Partidas en un intervalo de fechas
                        mensaje = trozos[1].Split('\0')[0];
                        if (mensaje != "fail")
                        {
                            string prtds = "En ese intervalo hubo: ";
                            for (int i = 1; i < (trozos.Length - 1); i = i + 2)
                            {
                                codigo2 = Convert.ToInt32(trozos[i]);
                                mensaje = trozos[i + 1].Split('\0')[0];
                                prtds = prtds + "Partida " + codigo2 + " el " + mensaje + ", ";
                            }
                            prtds = prtds.Remove(prtds.Length - 2);
                            MessageBox.Show(prtds);
                        }
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
                    case 17: //Ganador con determinado usuario y adversario en partida
                        mensaje = trozos[1].Split('\0')[0];
                        if (mensaje != "fail")
                        {
                            string ganadores = "Los ganadores son los siguientes: ";
                            for (int i = 1; i < (trozos.Length - 1); i = i+2)
                            {
                                codigo2 = Convert.ToInt32(trozos[i]);
                                mensaje = trozos[i+1].Split('\0')[0];
                                ganadores = ganadores + mensaje + " ganó en Partida " + codigo2 + ", ";
                            }
                            ganadores = ganadores.Remove(ganadores.Length - 2);
                            MessageBox.Show(ganadores);
                        }
                        else
                            MessageBox.Show("No se ha encontrado la partida");
                        break;
                    case 8: //Notificación de Lista de Conectados
                        //Haz tu lo que no me dejas hacer a mi
                        DelegadoParaListaConectados delegado = new DelegadoParaListaConectados(ListaConectados);
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
                            ID = Convert.ToInt32(trozos[2]);
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
                            ID = Convert.ToInt32(trozos[1]);
                            string jugadores = trozos[3].Split('\0')[0];
                            //Ahora se tendría que iniciar el Form2
                            ThreadStart ts = delegate { PonerEnMarchaFormulario(ID,jugadores); };
                            Thread T = new Thread(ts);
                            T.Start();
                        }
                        else if (mensaje == "FALSE")
                            MessageBox.Show("No se cumplen los requisitos para iniciar un partida");
                        break;
                    case 11: //Notificación de Jugada
                        nform = Convert.ToInt32(trozos[1]);
                        ID = Convert.ToInt32(trozos[2]);
                        mensaje = trozos[3].Split('\0')[0];
                        //MessageBox.Show(mensaje);
                        formularios[partidas[ID]].TomaRespuesta2(mensaje);
                        break;
                    case 12: //Notificación de Mensaje
                        nform = Convert.ToInt32(trozos[1]);
                        ID = Convert.ToInt32(trozos[2]);
                        mensaje = trozos[3].Split('\0')[0];
                        formularios[partidas[ID]].TomaRespuesta3(mensaje);
                        //MessageBox.Show(mensaje);
                        break;
                    case 13: //Notificación de Abandono Partida
                        nform = Convert.ToInt32(trozos[1]);
                        ID = Convert.ToInt32(trozos[2]);
                        mensaje = trozos[3].Split('\0')[0];
                        formularios[partidas[ID]].TomaRespuesta4(mensaje);
                        //MessageBox.Show(mensaje);
                        break;
                }
            }
        }

        private void Conectar_Click(object sender, EventArgs e)
        {
            if (this.BackColor == Color.Green)
                MessageBox.Show("Ya estás conectado");
            else {
                //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
                //al que deseamos conectarnos
                //IPAddress direc = IPAddress.Parse("192.168.56.102"); //Entorno Desarrollo
                //IPEndPoint ipep = new IPEndPoint(direc, 9036); //Entorno Desarrollo
                IPAddress direc = IPAddress.Parse("147.83.117.22"); //Entorno Producción
                IPEndPoint ipep = new IPEndPoint(direc, 50057); //Entorno Producción
                //Creamos el socket 
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server.Connect(ipep);//Intentamos conectar el socket
                    this.BackColor = Color.Green;
                    MessageBox.Show("Conectado");
                    ConectadosGrid.Columns.Clear();
                    ConectadosGrid.Rows.Clear();
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
        }

        private void Desconectar_Click(object sender, EventArgs e)
        {
            try
            {
                conectados.Remove(usuario);
                usuario = "";
                DelegadoParaActualizarSesion delegado2 = new DelegadoParaActualizarSesion(ActualizarSesion);
                SesionLbl.Invoke(delegado2);
                YAdesconectado = true;
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
            //Mostrar u ocultar contraseña
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

        private void Salir_Click(object sender, EventArgs e)
        {
            try
            {
                if (UsuarioBox.Text != "")
                {
                    //Cerrar sesión
                    string mensaje = "14/";
                    // Enviamos al servidor la consulta
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                    MessageBox.Show("Error en el campo de datos: Usuario");
            }
            catch (Exception)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("Error en la petición");
                return;
            }
        }

        private void EliminarCuenta_Click(object sender, EventArgs e)
        {
            try
            {
                if (UsuarioBox.Text != "")
                {
                    //Eliminar Cuenta
                    string mensaje = "15/";
                    // Enviamos al servidor la consulta
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                    MessageBox.Show("Error en el campo de datos: Usuario");
            }
            catch (Exception)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("Error en la petición");
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
                //Jugadores de la partida contra los que he jugado
                else if (JugadoresContra.Checked)
                {
                    if (UsuarioBox.Text != "")
                    {
                        string mensaje = "16/" + UsuarioBox.Text;
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
                //Partidas en intervalo de fechas
                else if (Intervalo.Checked)
                { 
                    if ((MinBox.Text != "") && (MaxBox.Text != ""))
                    {
                        string mensaje = "18/" + MinBox.Text + "/" + MaxBox.Text;
                        // Enviamos al servidor la consulta
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }
                    else
                        MessageBox.Show("Error en el campo de datos: Intervalo Fechas");
                }
                //Ganador más rápido
                else if (Rápido.Checked)
                {
                    string mensaje = "7/";
                    // Enviamos al servidor la consulta
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                //Ganador con determinado usuario en partida
                else if (GanadorAmbos.Checked)
                {
                    if ((UsuarioBox.Text != "") && (AdversarioBox.Text != ""))
                    {
                        string mensaje = "17/" + UsuarioBox.Text + "/" + AdversarioBox.Text;
                        // Enviamos al servidor la consulta
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }
                    else
                        MessageBox.Show("Error en el campo de datos: Usuario o Adversario");
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
            Boolean encontrado = false;
            if (ConectadosGrid.CurrentCell.Value.ToString() != usuario) {
                if (invitados.Count() == 0) {
                    ConectadosGrid.Rows[fila].DefaultCellStyle.BackColor = Color.Green;
                    invitados.Add(ConectadosGrid.CurrentCell.Value.ToString());
                    numSeleccionados++;
                } else {
                    for (int i = 0; i < invitados.Count(); i++) {
                        if (invitados[i] == ConectadosGrid.CurrentCell.Value.ToString())
                            encontrado = true;
                        if (!encontrado) {
                            ConectadosGrid.Rows[fila].DefaultCellStyle.BackColor = Color.Green;
                            invitados.Add(ConectadosGrid.CurrentCell.Value.ToString());
                            numSeleccionados++;
                        } //else {
                        //    ConectadosGrid.Rows[fila].DefaultCellStyle.BackColor = Color.White;
                        //    invitados.Remove(ConectadosGrid.CurrentCell.Value.ToString());
                        //    numSeleccionados--;
                        //    MessageBox.Show("Se ha eliminado a " + ConectadosGrid.CurrentCell.Value.ToString() + " de la lista de invitados");
                        //}
                    }
                }
            } else
                MessageBox.Show("Eres el anfitrión, no te puedes invitar");
            ConectadosGrid.ClearSelection();
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
                    int tmp = ConectadosGrid.RowCount;
                    for (int i=0; i < tmp; i++)
                        ConectadosGrid.Rows[i].DefaultCellStyle.BackColor = Color.White; //No va
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
            if (!YAdesconectado) {
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
}
