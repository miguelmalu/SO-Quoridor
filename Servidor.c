#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <mysql.h>

int Registrarse (char usuario[20],char contrasena[20], char respuesta[100],MYSQL *conn,int sock_conn){
	//Funcion para registrarse
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[100];
	sprintf (consulta, "SELECT Jugador.Usuario FROM Jugador WHERE Jugador.Usuario = '%s'", usuario);
	//Consulta: Busca si existe el usuario
	int err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL)
	{
		printf ("El usuario no existe\n");
		err=mysql_query (conn, "SELECT MAX(Jugador.ID) FROM Jugador");
		if (err!=0) {
			printf ("Error al consultar datos de la base %u %s\n",
					mysql_errno(conn), mysql_error(conn));
			exit (1);
			//recogemos el resultado de la consulta
		}
		resultado = mysql_store_result (conn);
		row = mysql_fetch_row (resultado);
		if (row ==  NULL)
			printf ("No se han obtenido datos en la consulta\n");
		else {
			int ID = atoi (row[0]) +1;
			printf("%d\n", ID);
			char IDstr[3];
			//convertimos la ID en un string y lo concatenamos
			sprintf(IDstr, "%d", ID);
			// Ahora construimos el string con el comando SQL
			// para insertar la persona en la base. Ese string es:
			// INSERT INTO Jugador VALUES (ID, 'usuario', 'Contrasena');
			sprintf (consulta, "INSERT INTO Jugador VALUES ('%s','%s','%s')", IDstr, usuario, contrasena);
			printf("%s\n", consulta);
			// Ahora ya podemos realizar la insercion
			err = mysql_query(conn, consulta);
			if (err!=0) {
				printf ("Error al introducir datos la base %u %s\n",
						mysql_errno(conn), mysql_error(conn));
				exit (1);
			}
			else
				strcpy(respuesta, "0");
				return 0;
		}
	}
	else {
		printf("El usuario %s ya existe\n", row[0]);
		strcpy(respuesta, "1");
		return 1;
	}
}
int Entrar (char usuario[20],char contrasena[20], char respuesta[100],MYSQL *conn,int sock_conn){
	//Funcion para entrar
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[100];
	sprintf (consulta, "SELECT Jugador.Usuario FROM Jugador WHERE Jugador.Usuario = '%s' AND Jugador.Contrasena = '%s'", usuario, contrasena);
	//Consulta: Busca si existe el usuario y la contrase�a
	int err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta
		resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL)
	{
		printf ("Datos de acceso inv�lidos\n");
		strcpy(respuesta, "1");
		return 1;
	}
	else {
		printf("%s ha iniciado sesi�n\n", row[0]);
		strcpy(respuesta,"0");
		return 0;
	}
}
void Contrasena (char usuario[20], char respuesta[100],MYSQL *conn,int sock_conn){
	//Procedimiento para devolver la contrasena
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[100];
	sprintf (consulta, "SELECT Jugador.Contrasena FROM Jugador WHERE Jugador.Usuario = '%s'", usuario);
	//Consulta: Buscar la contrase�a a partir del nombre de usuario
	int err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL) {
		printf ("No se han obtenido datos en la consulta\n");
		strcpy(respuesta, "fail");
	}
	else {
		printf("%s\n", row[0]);
		strcpy(respuesta, row[0]);
	}
}
void Jugadores (int partida, char respuesta[100],MYSQL *conn,int sock_conn){
	//Procedimiento para devolver los jugadores de una partida
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[200];
	sprintf (consulta, "SELECT Jugador.Usuario FROM (Jugador, Partida, Jugadores) WHERE Partida.ID = '%d' AND Jugador.ID = Jugadores.ID_J AND Partida.ID = Jugadores.ID_P", partida);
	//Consulta: Buscar los jugadores de una partida a partir del identificador
	int err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL) {
		printf ("No se han obtenido datos en la consulta\n");
		strcpy(respuesta, "fail");
	}
	else {
		strcpy(respuesta, "");
		while (row !=NULL) {
			printf("%s\n", row[0]);
			sprintf(respuesta, "%s%s/", respuesta, row[0]);
			row = mysql_fetch_row (resultado);
		}
	}
}
void Ganador (int partida, char respuesta[100],MYSQL *conn,int sock_conn){
	//Procedimiento para devolver el ganador de una partida
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[100];
	sprintf (consulta, "SELECT Partida.Ganador FROM Partida WHERE Partida.ID = '%d'", partida);
	//Consulta: Buscar el ganador de una partida a partir del identificador
	int err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL) {
		printf ("No se han obtenido datos en la consulta\n");
		strcpy(respuesta, "fail");
	}
	else {
		printf("%s\n", row[0]);
		strcpy(respuesta, row[0]);
	}
}
void Tiempo (int partida, char respuesta[100],MYSQL *conn,int sock_conn){
	//Procedimiento para devolver la duraci�n de una partida
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[100];
	sprintf (consulta, "SELECT Partida.Tiempo FROM Partida WHERE Partida.ID = '%d'", partida);
	//Consulta: Buscar la duraci�n de una partida a partir del identificador
	int err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL) {
		printf ("No se han obtenido datos en la consulta\n");
		strcpy(respuesta, "fail");
	}
	else {
		int tiempo = atoi (row[0]);
		printf("%d\n", tiempo);
		strcpy(respuesta, row[0]);
	}
}
void Rapido (char respuesta[100],MYSQL *conn,int sock_conn){
	//Procedimiento para devolver el ganador mas r�pido de una partida
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	//Consulta: Buscar la jugador que ha ganada m�s r�pido
	int err=mysql_query (conn, "SELECT Partida.Ganador FROM Partida WHERE Partida.Tiempo = (SELECT MIN(Partida.Tiempo) FROM Partida)");
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL) {
		printf ("No se han obtenido datos en la consulta\n");
		strcpy(respuesta, "fail");
	}
	else {
		printf("%s\n", row[0]);
		strcpy(respuesta, row[0]);
	}
}

int main(int argc, char *argv[])
{
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char peticion[512];
	char respuesta[512];
	// INICIALITZACIONS
	// Obrim el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket");
	// Fem el bind al port
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	// asocia el socket a cualquiera de las IP de la m?quina.
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// establecemos el puerto de escucha
	serv_adr.sin_port = htons(9000);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podr? ser superior a 3
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen");

	//MYSQL
	MYSQL *conn;
	int err;
	//Creamos una conexion al servidor MYSQL
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexion
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "BD",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}

	// Bucle infinito
	for (;;){
		printf ("Escuchando\n");

		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		//sock_conn es el socket que usaremos para este cliente

		int terminar =0;
		// Entramos en un bucle para atender todas las peticiones de este cliente
		//hasta que se desconecte
		while (terminar ==0)
		{
			// Ahora recibimos la peticion
			ret=read(sock_conn,peticion, sizeof(peticion));
			printf ("Recibido\n");
			// Tenemos que a�adirle la marca de fin de string
			// para que no escriba lo que hay despues en el buffer
			peticion[ret]='\0';
			printf ("Peticion: %s\n", peticion);
			// vamos a ver que quieren
			char *p = strtok( peticion, "/");
			int codigo =  atoi (p);
			// Ya tenemos el codigo de la peticion
			printf ("Codigo: %d\n", codigo);

			if (codigo !=0)
				p = strtok( NULL, "/");
			char usuario[20];
			char contrasena[20];
			int partida;

			if (codigo ==0) //Peticion de Desconexion
				terminar=1;
			else if (codigo ==1) //Peticion de Registro
			{
				strcpy (usuario, p);
				p = strtok( NULL, "/");
				strcpy (contrasena, p);
				Registrarse(usuario,contrasena,respuesta,conn,sock_conn);
			}
			else if (codigo ==2) //Peticion de Login
			{
				strcpy (usuario, p);
				p = strtok( NULL, "/");
				strcpy (contrasena, p);
				Entrar(usuario,contrasena,respuesta,conn,sock_conn);
			}
			else if (codigo ==3) //Peticion de Contrase�a
			{
				strcpy (usuario, p);
				Contrasena(usuario,respuesta,conn,sock_conn);
			}
			else if (codigo ==4) //Peticion de Jugadores
			{
				partida = atoi (p);
				Jugadores(partida,respuesta,conn,sock_conn);
			}
			else if (codigo ==5) //Peticion de Ganador
			{
				partida = atoi (p);
				Ganador(partida,respuesta,conn,sock_conn);
			}
			else if (codigo ==6) //Peticion de Tiempo
			{
				partida = atoi (p);
				Tiempo(partida,respuesta,conn,sock_conn);
			}
			else if (codigo ==7) //Peticion de ganador m�s r�pido
			{
				Rapido(respuesta,conn,sock_conn);
			}
			else
				strcpy (respuesta,"C�digo de petici�n no v�lido");

			if (codigo !=0)
			{
				// Enviamos respuesta
				write (sock_conn,respuesta, strlen(respuesta));
			}
		}
		//Cerramos MYSQL
		mysql_close (conn);
		exit(0);
		//Se acabo el servicio para este cliente
		close(sock_conn);
	}
}
