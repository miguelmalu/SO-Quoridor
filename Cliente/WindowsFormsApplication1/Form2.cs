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

        int nForm;
        int ID;
        string usuario;
        string jugadores;
        int IDganador;
        bool YAabandonada;

        Socket server;

        delegate void DelegadoParaJugadaMoverFicha(List<PictureBox> fichas, Point anterior, Point destino, int IDganador);
        delegate void DelegadoParaJugadaPonerBarreras(List<PictureBox> PBDispBar, Point anterior, Point destino);
        delegate void DelegadoParaTurnoActual(string turnoAct);
        delegate void DelegadoParaCerrarForm();

        string player1, player2, player3, player4;
        List<string> players = new List<string>();
        int barIncJ1, barIncJ2, barIncJ3, barIncJ4;
        List<int> barDisponibles = new List<int>();
        string turnoAct;
        PictureBox seleccionado = null;
        PictureBox selecBar1 = null;
        PictureBox selecBar2 = null;
        List<PictureBox> fichas = new List<PictureBox>();
        List<PictureBox> barreras = new List<PictureBox>();
        List<PictureBox> PBDispBar = new List<PictureBox>(); //PictureBox disponibles para poner barreras
        Point puntoBarAlrededor = new Point();
        Point puntoL1 = new Point(); //Eje X de la + de una diagonal
        Point puntoL2 = new Point(); //Eje Y de la + de una diagonal
        Point puntoBarPosterior = new Point();
        Point puntoBarLadoDiagDes = new Point();
        Point puntoBarLadoDiagOrg = new Point();
        Point puntoBarL1 = new Point(); //Eje X de la + de una diagonal
        Point puntoBarL2 = new Point(); //Eje Y de la + de una diagonal

        public Form2(int nForm, int ID, string usuario, Socket server, string jugadores)
        {
            InitializeComponent();
            this.nForm = nForm;
            this.server = server;
            this.ID = ID;
            this.usuario = usuario;
            this.jugadores = jugadores;
            cargarFichas();
            cargarPBDispBar();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            UsuarioLbl.Text = usuario;
            turnoAct = player1;
            TurnoLbl.Text = turnoAct;
            int barDisp = barrerasDisponibles();
            BarDispLbl.Text = barDisp.ToString();
            IDParLbl.Text = ID.ToString();
        }

        private void cargarFichas() //Mostar los fichas y los turnos
        {
            //Limpio el mensaje
            string mensaje = jugadores.Split('\0')[0];
            string[] trozos = mensaje.Split(',');
            int numJugadores = Convert.ToInt32(trozos[0]);

            if (numJugadores == 2)
            {
                player1 = trozos[1].Split('\0')[0];
                players.Add(player1);
                fichas.Add(jugador1);
                player2 = trozos[2].Split('\0')[0];
                players.Add(player2);
                fichas.Add(jugador2);
                jugador3.Location = new Point(0, 0);
                jugador3.Visible = false;
                jugador4.Location = new Point(0, 0);
                jugador4.Visible = false;
                barIncJ1 = 10;
                barIncJ2 = 10;
                barDisponibles.Add(barIncJ1);
                barDisponibles.Add(barIncJ2);
            } else if (numJugadores == 3) {
                player1 = trozos[1].Split('\0')[0];
                  players.Add(player1);
                fichas.Add(jugador1);
                player2 = trozos[2].Split('\0')[0];
                  players.Add(player2);
                fichas.Add(jugador2);
                player3 = trozos[3].Split('\0')[0];
                  players.Add(player3);
                fichas.Add(jugador3);
                jugador4.Location = new Point(0, 0);
                jugador4.Visible = false;
                barIncJ1 = 6;
                barIncJ2 = 6;
                barIncJ3 = 6;
                barDisponibles.Add(barIncJ1);
                barDisponibles.Add(barIncJ2);
                barDisponibles.Add(barIncJ3);
            }
            else if (numJugadores == 4) {
                player1 = trozos[1].Split('\0')[0];
                  players.Add(player1);
                fichas.Add(jugador1);
                player2 = trozos[2].Split('\0')[0];
                  players.Add(player2);
                fichas.Add(jugador2);
                player3 = trozos[3].Split('\0')[0];
                  players.Add(player3);
                fichas.Add(jugador3);
                player4 = trozos[4].Split('\0')[0];
                  players.Add(player4);
                fichas.Add(jugador4);
                barIncJ1 = 5;
                barIncJ2 = 5;
                barIncJ3 = 5;
                barIncJ4 = 5;
                barDisponibles.Add(barIncJ1);
                barDisponibles.Add(barIncJ2);
                barDisponibles.Add(barIncJ3);
                barDisponibles.Add(barIncJ4);
            }
        }

        private void cargarPBDispBar()
        {
            PBDispBar.Add(pictureBox82);
            PBDispBar.Add(pictureBox83);
            PBDispBar.Add(pictureBox84);
            PBDispBar.Add(pictureBox85);
            PBDispBar.Add(pictureBox86);
            PBDispBar.Add(pictureBox87);
            PBDispBar.Add(pictureBox88);
            PBDispBar.Add(pictureBox89);
            PBDispBar.Add(pictureBox90);
            PBDispBar.Add(pictureBox91);
            PBDispBar.Add(pictureBox91);
            PBDispBar.Add(pictureBox93);
            PBDispBar.Add(pictureBox94);
            PBDispBar.Add(pictureBox95);
            PBDispBar.Add(pictureBox96);
            PBDispBar.Add(pictureBox97);
            PBDispBar.Add(pictureBox98);
            PBDispBar.Add(pictureBox99);
            PBDispBar.Add(pictureBox100);
            PBDispBar.Add(pictureBox101);
            PBDispBar.Add(pictureBox102);
            PBDispBar.Add(pictureBox103);
            PBDispBar.Add(pictureBox104);
            PBDispBar.Add(pictureBox105);
            PBDispBar.Add(pictureBox106);
            PBDispBar.Add(pictureBox107);
            PBDispBar.Add(pictureBox108);
            PBDispBar.Add(pictureBox109);
            PBDispBar.Add(pictureBox110);
            PBDispBar.Add(pictureBox111);
            PBDispBar.Add(pictureBox112);
            PBDispBar.Add(pictureBox113);
            PBDispBar.Add(pictureBox114);
            PBDispBar.Add(pictureBox115);
            PBDispBar.Add(pictureBox116);
            PBDispBar.Add(pictureBox117);
            PBDispBar.Add(pictureBox118);
            PBDispBar.Add(pictureBox119);
            PBDispBar.Add(pictureBox120);
            PBDispBar.Add(pictureBox121);
            PBDispBar.Add(pictureBox122);
            PBDispBar.Add(pictureBox123);
            PBDispBar.Add(pictureBox124);
            PBDispBar.Add(pictureBox125);
            PBDispBar.Add(pictureBox126);
            PBDispBar.Add(pictureBox127);
            PBDispBar.Add(pictureBox128);
            PBDispBar.Add(pictureBox129);
            PBDispBar.Add(pictureBox130);
            PBDispBar.Add(pictureBox131);
            PBDispBar.Add(pictureBox132);
            PBDispBar.Add(pictureBox133);
            PBDispBar.Add(pictureBox134);
            PBDispBar.Add(pictureBox135);
            PBDispBar.Add(pictureBox136);
            PBDispBar.Add(pictureBox137);
            PBDispBar.Add(pictureBox138);
            PBDispBar.Add(pictureBox139);
            PBDispBar.Add(pictureBox140);
            PBDispBar.Add(pictureBox141);
            PBDispBar.Add(pictureBox142);
            PBDispBar.Add(pictureBox143);
            PBDispBar.Add(pictureBox144);
            PBDispBar.Add(pictureBox145);
            PBDispBar.Add(pictureBox146);
            PBDispBar.Add(pictureBox147);
            PBDispBar.Add(pictureBox148);
            PBDispBar.Add(pictureBox149);
            PBDispBar.Add(pictureBox150);
            PBDispBar.Add(pictureBox161);
            PBDispBar.Add(pictureBox162);
            PBDispBar.Add(pictureBox163);
            PBDispBar.Add(pictureBox164);
            PBDispBar.Add(pictureBox165);
            PBDispBar.Add(pictureBox166);
            PBDispBar.Add(pictureBox167);
            PBDispBar.Add(pictureBox168);
            PBDispBar.Add(pictureBox169);
            PBDispBar.Add(pictureBox170);
            PBDispBar.Add(pictureBox171);
            PBDispBar.Add(pictureBox172);
            PBDispBar.Add(pictureBox173);
            PBDispBar.Add(pictureBox174);
            PBDispBar.Add(pictureBox175);
            PBDispBar.Add(pictureBox176);
            PBDispBar.Add(pictureBox177);
            PBDispBar.Add(pictureBox178);
            PBDispBar.Add(pictureBox179);
            PBDispBar.Add(pictureBox180);
            PBDispBar.Add(pictureBox181);
            PBDispBar.Add(pictureBox182);
            PBDispBar.Add(pictureBox183);
            PBDispBar.Add(pictureBox184);
            PBDispBar.Add(pictureBox185);
            PBDispBar.Add(pictureBox186);
            PBDispBar.Add(pictureBox187);
            PBDispBar.Add(pictureBox188);
            PBDispBar.Add(pictureBox189);
            PBDispBar.Add(pictureBox190);
            PBDispBar.Add(pictureBox191);
            PBDispBar.Add(pictureBox192);
            PBDispBar.Add(pictureBox193);
            PBDispBar.Add(pictureBox194);
            PBDispBar.Add(pictureBox195);
            PBDispBar.Add(pictureBox196);
            PBDispBar.Add(pictureBox197);
            PBDispBar.Add(pictureBox198);
            PBDispBar.Add(pictureBox199);
            PBDispBar.Add(pictureBox200);
            PBDispBar.Add(pictureBox201);
            PBDispBar.Add(pictureBox202);
            PBDispBar.Add(pictureBox203);
            PBDispBar.Add(pictureBox204);
            PBDispBar.Add(pictureBox205);
            PBDispBar.Add(pictureBox206);
            PBDispBar.Add(pictureBox207);
            PBDispBar.Add(pictureBox208);
            PBDispBar.Add(pictureBox209);
            PBDispBar.Add(pictureBox210);
            PBDispBar.Add(pictureBox211);
            PBDispBar.Add(pictureBox212);
            PBDispBar.Add(pictureBox213);
            PBDispBar.Add(pictureBox214);
            PBDispBar.Add(pictureBox215);
            PBDispBar.Add(pictureBox216);
            PBDispBar.Add(pictureBox217);
            PBDispBar.Add(pictureBox218);
            PBDispBar.Add(pictureBox219);
            PBDispBar.Add(pictureBox220);
            PBDispBar.Add(pictureBox221);
            PBDispBar.Add(pictureBox222);
            PBDispBar.Add(pictureBox223);
            PBDispBar.Add(pictureBox224);
            PBDispBar.Add(pictureBox225);
        }

        private void CerrarForm()
        {
            this.Close();
        }

        private void Turno()
        {
            Boolean encontrado = false;
            for (int i = 0; (i < players.Count) && !encontrado; i++)
                if (i == (players.Count - 1))
                    turnoAct = players[0];
                else if (players[i] == turnoAct) {
                    turnoAct = players[i + 1];
                    encontrado = true;
                }
            TurnoLbl.Text = turnoAct;
        }

        private void JugadaMoverFicha(List<PictureBox> fichas, Point anterior, Point destino, int IDganador)
        {
            for (int i = 0; i < fichas.Count; i++)
                if (fichas[i].Location == anterior) {
                    fichas[i].Location = destino;
                    if (IDganador != -1)
                        fichas[i].BackgroundImage = Properties.Resources.Ganador;
                }
        }

        private void JugadaPonerBarreras(List<PictureBox> PBDispBar, Point puntoBar1, Point puntoBar2)
        {
            for (int i = 0; i < PBDispBar.Count; i++) {
                if ((PBDispBar[i].Location == puntoBar1) || (PBDispBar[i].Location == puntoBar2)) {
                    PBDispBar[i].BackColor = Color.Black;
                    PBDispBar[i].BringToFront();
                    barreras.Add(PBDispBar[i]);
                }
            }
        }

        private void TurnoActual(string turnoAct)
        {
            TurnoLbl.Text = turnoAct;
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
            //Notificación de jugada
            //Limpio el mensaje
            string jugada = mensaje.Split('\0')[0];
            string[] trozos = jugada.Split(',');
            turnoAct = trozos[0].Split('\0')[0];
            //MessageBox.Show(turnoAct);
            DelegadoParaTurnoActual delegado3 = new DelegadoParaTurnoActual(TurnoActual);
            TurnoLbl.Invoke(delegado3, new object[] { turnoAct });
            int codigo = Convert.ToInt32(trozos[1]);
            IDganador = Convert.ToInt32(trozos[6]);

            switch (codigo)
            {
                case 1: //Fichas
                    int antX = Convert.ToInt32(trozos[2]);
                    int antY = Convert.ToInt32(trozos[3]);
                    int destX = Convert.ToInt32(trozos[4]);
                    int destY = Convert.ToInt32(trozos[5]);
                    Point anterior = new Point(antX, antY);
                    Point destino = new Point(destX, destY);
                    DelegadoParaJugadaMoverFicha delegado1 = new DelegadoParaJugadaMoverFicha(JugadaMoverFicha);
                    for (int i = 0; i < fichas.Count; i++)
                        fichas[i].Invoke(delegado1, new object[] { fichas, anterior, destino, IDganador });
                    break;
                case 2: //Barreras
                    int Bar1X = Convert.ToInt32(trozos[2]);
                    //MessageBox.Show(trozos[2]);
                    int Bar1Y = Convert.ToInt32(trozos[3]);
                    //MessageBox.Show(trozos[2]);
                    int Bar2X = Convert.ToInt32(trozos[4]);
                    int Bar2Y = Convert.ToInt32(trozos[5]);
                    Point puntoBar1 = new Point(Bar1X, Bar1Y);
                    Point puntoBar2 = new Point(Bar2X, Bar2Y);
                    DelegadoParaJugadaPonerBarreras delegado2 = new DelegadoParaJugadaPonerBarreras(JugadaPonerBarreras);
                    for (int i = 0; i < PBDispBar.Count; i++)
                        PBDispBar[i].Invoke(delegado2, new object[] { PBDispBar, puntoBar1, puntoBar2 });
                    break;
            }

            if (IDganador != -1) {
                MessageBox.Show(turnoAct + " ha ganado la partida");
                YAabandonada = true;
                DelegadoParaCerrarForm delegado4 = new DelegadoParaCerrarForm(CerrarForm);
                Invoke(delegado4);
            }
        }

        public void TomaRespuesta3(string mensaje)
        {
            //Notificación de Mensaje
            MessageBox.Show(mensaje);
        }

        public void TomaRespuesta4(string mensaje)
        {
            //Notificación de Abandono Partida
            MessageBox.Show(mensaje + " ha abandonado la partida " + ID + ", fin del juego");
            YAabandonada = true;
            DelegadoParaCerrarForm delegado4 = new DelegadoParaCerrarForm(CerrarForm);
            Invoke(delegado4);
        }

        public void seleccionJugador(object objeto)
        {
            if (selecBar1 != null)
            {
                selecBar1.BackColor = Color.White;
                selecBar1 = null;
            }
            PictureBox ficha = (PictureBox)objeto;
            seleccionado = ficha;
            seleccionado.BackColor = Color.Lime;
        }

        public void seleccionBarreras(object objeto)
        {
            if (seleccionado != null)
            {
                seleccionado.BackColor = Color.White;
                seleccionado = null;
            }
            PictureBox barrera = (PictureBox)objeto;
            if ((selecBar1 == null) && (barrera.BackColor == Color.White))
            {
                selecBar1 = barrera;
                selecBar1.BackColor = Color.Lime;
            }
            else if ((selecBar2 == null) && (barrera.BackColor == Color.White))
            {
                selecBar2 = barrera;
                selecBar2.BackColor = Color.Lime;
                ponerBarreras();
            }
        }

        private void movimientoFichas(PictureBox cuadro)
        {
            if (seleccionado != null)
            {
                string jug = seleccionado.Name.ToString().Substring(7, 1); //Saca el número de jugador
                int jugador = Convert.ToInt32(jug);
                if (validacionFichas(seleccionado, cuadro, jugador))
                { //Validación movimiento
                    Point anterior = seleccionado.Location;
                    seleccionado.Location = cuadro.Location;
                    Ganador(jugador);
                    seleccionado.BackColor = Color.White;
                    if (IDganador == -1)
                        Turno();
                    else
                        seleccionado.BackgroundImage = Properties.Resources.Ganador;
                    seleccionado = null;

                    try
                    {
                        //Movimiento Ficha + Posible Ganador
                        string mensaje = "11/" + nForm + "/" + ID + "/" + turnoAct + "/1," + anterior.X + "," + anterior.Y + "," + cuadro.Location.X + "," + cuadro.Location.Y + "/" + IDganador;
                        // Enviamos al servidor la consulta
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }
                    catch (Exception)
                    {
                        //Si hay excepcion imprimimos error y salimos del programa con return 
                        MessageBox.Show("Error en enviar la jugada");
                        return;
                    }

                    if (IDganador == jugador) {
                        MessageBox.Show("Has ganado la partida");
                        YAabandonada = true;
                        this.Close();
                    }
                }
            }
        }

        private int barrerasDisponibles()
        {
            Boolean encontrado = false;
            int i;
            for (i = 0; (i < players.Count) && !encontrado; i++)
                if (players[i] == turnoAct)
                    encontrado = true;
            if (!encontrado)
                return -1;
            else
                return barDisponibles[i-1];
        }

        private void actualizarBarrerasDisponibles()
        {
            Boolean encontrado = false;
            int i;
            for (i = 0; (i < players.Count) && !encontrado; i++)
                if (players[i] == turnoAct)
                    encontrado = true;
            if (!encontrado)
                MessageBox.Show("Error al consultar las barreras disponibles");
            else
                barDisponibles[i-1]--;
        }

        private void ponerBarreras()
        {
            if (validacionBarreras())
            {
                selecBar1.BackColor = Color.Black;
                selecBar1.BringToFront();
                barreras.Add(selecBar1);
                selecBar2.BackColor = Color.Black;
                selecBar2.BringToFront();
                barreras.Add(selecBar2);
                actualizarBarrerasDisponibles();
                int barDisp = barrerasDisponibles();
                BarDispLbl.Text = barDisp.ToString();
                Turno();

                try
                {
                    //Poner Barreras
                    string mensaje = "11/" + nForm + "/" + ID + "/" + turnoAct + "/2," + selecBar1.Location.X + "," + selecBar1.Location.Y + "," + selecBar2.Location.X + "," + selecBar2.Location.Y + "/-1";
                    // Enviamos al servidor la consulta
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                catch (Exception)
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    MessageBox.Show("Error en enviar la jugada");
                    return;
                }
            }
            else
            {
                MessageBox.Show("No es posible poner así las barreras");
                selecBar1.BackColor = Color.White;
                selecBar2.BackColor = Color.White;
            }
            selecBar1 = null;
            selecBar2 = null;
        }

        private bool ocupado(Point punto, List<PictureBox> barreras)
        {
            for (int i = 0; i < barreras.Count; i++)
            {
                if (punto == barreras[i].Location)
                    return true;
            }
            return false;
        }

        private void BarAlrededor(Point puntoOrigen, Point puntoDestino)
        {
            //En función del puntoOrigen para no depender de la magnitud del movimiento
            if ((puntoDestino.X > puntoOrigen.X) && (puntoDestino.Y == puntoOrigen.Y))
                puntoBarAlrededor = new Point(puntoOrigen.X + 45, puntoOrigen.Y);
            else if ((puntoDestino.X < puntoOrigen.X) && (puntoDestino.Y == puntoOrigen.Y))
                puntoBarAlrededor = new Point(puntoOrigen.X - 5, puntoOrigen.Y);
            else if ((puntoDestino.X == puntoOrigen.X) && (puntoDestino.Y > puntoOrigen.Y))
                puntoBarAlrededor = new Point(puntoOrigen.X, puntoOrigen.Y + 45);
            else if ((puntoDestino.X == puntoOrigen.X) && (puntoDestino.Y < puntoOrigen.Y))
                puntoBarAlrededor = new Point(puntoOrigen.X, puntoOrigen.Y - 5);
        }

        private void Eles(Point puntoOrigen, Point puntoDestino)
        {
            if ((puntoDestino.X > puntoOrigen.X) && (puntoDestino.Y > puntoOrigen.Y))
            {
                puntoL1 = new Point(puntoDestino.X, puntoDestino.Y - 50);
                puntoL2 = new Point(puntoDestino.X - 50, puntoDestino.Y);
            }
            else if ((puntoDestino.X > puntoOrigen.X) && (puntoDestino.Y < puntoOrigen.Y))
            {
                puntoL1 = new Point(puntoDestino.X, puntoDestino.Y + 50);
                puntoL2 = new Point(puntoDestino.X - 50, puntoDestino.Y);
            }
            else if ((puntoDestino.X < puntoOrigen.X) && (puntoDestino.Y > puntoOrigen.Y))
            {
                puntoL1 = new Point(puntoDestino.X, puntoDestino.Y - 50);
                puntoL2 = new Point(puntoDestino.X + 50, puntoDestino.Y);
            }
            else if ((puntoDestino.X < puntoOrigen.X) && (puntoDestino.Y < puntoOrigen.Y))
            {
                puntoL1 = new Point(puntoDestino.X, puntoDestino.Y + 50);
                puntoL2 = new Point(puntoDestino.X + 50, puntoDestino.Y);
            }
        }

        private void BarPosterior(Point puntoOrigen, Point puntoDestino)
        {
            //En función del puntoDestino para no depender de la magnitud del movimiento
            if ((puntoDestino.X > puntoOrigen.X) && (puntoDestino.Y == puntoOrigen.Y))
                puntoBarPosterior = new Point(puntoDestino.X + 45, puntoDestino.Y);
            else if ((puntoDestino.X < puntoOrigen.X) && (puntoDestino.Y == puntoOrigen.Y))
                puntoBarPosterior = new Point(puntoDestino.X - 5, puntoDestino.Y);
            else if ((puntoDestino.X == puntoOrigen.X) && (puntoDestino.Y > puntoOrigen.Y))
                puntoBarPosterior = new Point(puntoDestino.X, puntoDestino.Y + 45);
            else if ((puntoDestino.X == puntoOrigen.X) && (puntoDestino.Y < puntoOrigen.Y))
                puntoBarPosterior = new Point(puntoDestino.X, puntoDestino.Y - 5);
        }

        private void BarLadoDiag(Point puntoOrigen, Point puntoOponente, Point puntoDestiono)
        {
            if ((puntoOponente.X > puntoDestiono.X) && (puntoOponente.Y == puntoDestiono.Y))
                puntoBarLadoDiagDes = new Point(puntoOponente.X - 5, puntoOponente.Y);
            else if ((puntoOponente.X < puntoDestiono.X) && (puntoOponente.Y == puntoDestiono.Y))
                puntoBarLadoDiagDes = new Point(puntoOponente.X + 45, puntoOponente.Y);
            else if ((puntoOponente.X == puntoDestiono.X) && (puntoOponente.Y > puntoDestiono.Y))
                puntoBarLadoDiagDes = new Point(puntoOponente.X, puntoOponente.Y - 5);
            else if ((puntoOponente.X == puntoDestiono.X) && (puntoOponente.Y < puntoDestiono.Y))
                puntoBarLadoDiagDes = new Point(puntoOponente.X, puntoOponente.Y + 45);
            if ((puntoOponente.X > puntoOrigen.X) && (puntoOponente.Y == puntoOrigen.Y))
                puntoBarLadoDiagOrg = new Point(puntoBarLadoDiagDes.X - 50, puntoBarLadoDiagDes.Y);
            else if ((puntoOponente.X < puntoOrigen.X) && (puntoOponente.Y == puntoOrigen.Y))
                puntoBarLadoDiagOrg = new Point(puntoBarLadoDiagDes.X + 50, puntoBarLadoDiagDes.Y);
            else if ((puntoOponente.X == puntoOrigen.X) && (puntoOponente.Y > puntoOrigen.Y))
                puntoBarLadoDiagOrg = new Point(puntoBarLadoDiagDes.X, puntoBarLadoDiagDes.Y - 50);
            else if ((puntoOponente.X == puntoOrigen.X) && (puntoOponente.Y < puntoOrigen.Y))
                puntoBarLadoDiagOrg = new Point(puntoBarLadoDiagDes.X, puntoBarLadoDiagDes.Y + 50);
        }

        private void BarEles(Point puntoBar1, Point puntoBar2)
        {
            if ((puntoBar2.X > puntoBar1.X) && (puntoBar1.Y == puntoBar2.Y))
            {
                puntoBarL1 = new Point(puntoBar2.X - 5, puntoBar2.Y - 45);
                puntoBarL2 = new Point(puntoBar2.X - 5, puntoBar2.Y + 5);
            }
            else if ((puntoBar2.X < puntoBar1.X) && (puntoBar1.Y == puntoBar2.Y))
            {
                puntoBarL1 = new Point(puntoBar2.X + 45, puntoBar2.Y - 45);
                puntoBarL2 = new Point(puntoBar2.X + 45, puntoBar2.Y + 5);
            }
            else if ((puntoBar1.X == puntoBar2.X) && (puntoBar2.Y > puntoBar1.Y))
            {
                puntoBarL1 = new Point(puntoBar2.X - 45, puntoBar2.Y - 5);
                puntoBarL2 = new Point(puntoBar2.X + 5, puntoBar2.Y - 5);
            }
            else if ((puntoBar1.X == puntoBar2.X) && (puntoBar2.Y < puntoBar1.Y))
            {
                puntoBarL1 = new Point(puntoBar2.X - 45, puntoBar2.Y + 45);
                puntoBarL2 = new Point(puntoBar2.X + 5, puntoBar2.Y + 45);
            }
        }

        private int promedio(int n1, int n2)
        {
            int resultado = n1 + n2;
            resultado = resultado / 2;
            return Math.Abs(resultado);
        }

        private bool validacionFichas(PictureBox origen, PictureBox destino, int jugador)
        {
            //Definimos los oponentes
            List<PictureBox> oponentes = new List<PictureBox>();
            if (jugador != 1)
                oponentes.Add(jugador1);
            if (jugador != 2)
                oponentes.Add(jugador2);
            if ((jugador != 3) && (players.Count == 3))
                oponentes.Add(jugador3);
            if ((jugador != 4) && (players.Count == 4))
                oponentes.Add(jugador4);

            Point puntoOrigen = origen.Location;
            Point puntoDestino = destino.Location;
            int avanceX = Math.Abs(puntoOrigen.X - puntoDestino.X);
            int avanceY = Math.Abs(puntoOrigen.Y - puntoDestino.Y);
            BarAlrededor(puntoOrigen, puntoDestino);
            if (((avanceX == 50) && (avanceY == 0)) || ((avanceY == 50) && (avanceX == 0)))
            {
                //Una posición el horizontal o vertical
                if (!ocupado(puntoBarAlrededor, barreras))
                    return true;
            }
            else if (((avanceX == 100) && (avanceY == 0)) || ((avanceY == 100) && (avanceX == 0)))
            {
                //Se encuentra con otro jugador de cara, intenta doble salto si no hay barrera detrás
                Point puntoMedio = new Point(promedio(puntoDestino.X, puntoOrigen.X), promedio(puntoDestino.Y, puntoOrigen.Y));
                for (int i = 0; i < oponentes.Count; i++)
                {
                    if (oponentes[i].Location == puntoMedio)
                    {
                        BarPosterior(puntoOrigen, oponentes[i].Location);
                        if ((!ocupado(puntoBarPosterior, barreras)) && (!ocupado(puntoBarAlrededor, barreras)))
                            return true;
                    }
                }
            }
            else if ((avanceY == 50) && (avanceX == 50))
            {
                //Solo deja hacer en diagonal si hay una barrera evitando el doble salto
                Eles(puntoOrigen, puntoDestino);
                for (int i = 0; i < oponentes.Count; i++)
                {
                    if ((oponentes[i].Location == puntoL1) || (oponentes[i].Location == puntoL2))
                    {
                        BarPosterior(puntoOrigen, oponentes[i].Location);
                        BarLadoDiag(puntoOrigen, oponentes[i].Location, puntoDestino);
                        if (ocupado(puntoBarPosterior, barreras))
                            if ((!ocupado(puntoBarLadoDiagDes, barreras)) || (!ocupado(puntoBarLadoDiagOrg, barreras)))
                                return true;
                    }
                }
            }
            return false;
        }

        private bool validacionBarreras()
        {
            Point puntoBar1 = selecBar1.Location;
            Point puntoBar2 = selecBar2.Location;
            int distX = Math.Abs(puntoBar1.X - puntoBar2.X);
            int distY = Math.Abs(puntoBar1.Y - puntoBar2.Y);
            int altBar1 = selecBar1.Size.Height;
            int altBar2 = selecBar2.Size.Height;
            BarEles(puntoBar1, puntoBar2);
            if (((distX == 50) && (distY == 0) && (altBar1 == 10)) || ((distY == 50) && (distX == 0) && (altBar1 == 50)))
                if ((!ocupado(puntoBarL1, barreras)) || (!ocupado(puntoBarL2, barreras)))
                    return true;
            return false;
        }

        private void Ganador(int jugador)
        {
            if (jugador == 1 && seleccionado.Location.X == 450)
                IDganador = jugador;
            else if (jugador == 2 && seleccionado.Location.X == 50)
                IDganador = jugador;
            else if (jugador == 3 && seleccionado.Location.Y == 450)
                IDganador = jugador;
            else if (jugador == 4 && seleccionado.Location.Y == 50)
                IDganador = jugador;
            else
                IDganador = -1;
        }

        private void jugador1Click(object sender, MouseEventArgs e)
        {
            if ((turnoAct == player1) && (turnoAct == usuario))
                seleccionJugador(sender);
            else
                MessageBox.Show("No es tu turno");
        }

        private void jugador2Click(object sender, MouseEventArgs e)
        {
            if ((turnoAct == player2) && (turnoAct == usuario))
                seleccionJugador(sender);
            else
                MessageBox.Show("No es tu turno");
        }

        private void jugador3Click(object sender, MouseEventArgs e)
        {
            if ((turnoAct == player3) && (turnoAct == usuario))
                seleccionJugador(sender);
            else
                MessageBox.Show("No es tu turno");
        }

        private void jugador4Click(object sender, MouseEventArgs e)
        {
            if ((turnoAct == player4) && (turnoAct == usuario))
                seleccionJugador(sender);
            else
                MessageBox.Show("No es tu turno");
        }

        private void cuadroClick(object sender, MouseEventArgs e)
        {
            if (turnoAct == usuario)
                movimientoFichas((PictureBox)sender);
            else
                MessageBox.Show("No es tu turno");
        }

        private void barreraClick(object sender, MouseEventArgs e)
        {
            if (turnoAct == usuario) {
                int barDisp = barrerasDisponibles();
                if (barDisp == -1)
                    MessageBox.Show("Error al consultar las barreras disponibles");
                else if (barDisp == 0)
                    MessageBox.Show("No te quedan barreras por colocar");
                else
                    seleccionBarreras(sender);
            } else
                MessageBox.Show("No es tu turno");
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!YAabandonada) {
                try
                {
                    //Abandonar partida
                    string mensaje = "13/" + nForm + "/" + ID + "/" + usuario;
                    //Enviamos al servidor la consulta
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                catch (Exception)
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    MessageBox.Show("Error al salir de la partida");
                    return;
                }
            }
        }
    }
}
