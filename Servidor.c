#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <mysql.h>
#include <pthread.h>
#include <my_global.h> //Solo para BD en Entorno Producción

//ESTRUCTURAS PARA LA LISTA DE CONECTADOS
typedef struct {
	char nombre [20];
	int socket;
} Conectado;

typedef struct {
	Conectado conectados [100];
	int num;
} ListaConectados;

ListaConectados lista;

//FUNCIONES PARA LA LISTA DE CONECTADOS
int Pon (ListaConectados *lista, char nombre[20], int socket) {
	//Añade nuevos conectados.
	//Retorna 0 si OK, 1 si la lista estaba llena.
	if (lista->num == 100)
		return -1;
	else {
		strcpy (lista->conectados[lista->num].nombre, nombre);
		lista->conectados[lista->num].socket = socket;
		lista->num++;
		return 0;
	}
}

int DamePosicion (ListaConectados *lista, char nombre[20]) {
	//Devuelve la posición o -1 si no está en la lista
	int i=0;
	int encontrado=0;
	while ((i < lista->num) && !encontrado) {
		if (strcmp (lista->conectados[i].nombre, nombre) ==0)
			encontrado =1;
		if (!encontrado)
			i++;
	}
	if (encontrado)
		return i;
	else
		return -1;
}

int DamePosicionPorSocket (ListaConectados *lista, int socket) {
	//Devuelve la posición o -1 si no está en la lista
	int i=0;
	int encontrado=0;
	while ((i < lista->num) && !encontrado) {
		if (lista->conectados[i].socket == socket)
			encontrado =1;
		if (!encontrado)
			i++;
	}
	if (encontrado)
		return i;
	else
		return -1;
}

int Elimina (ListaConectados *lista, char nombre[20]) {
	//Retorna 0 si elimina, -1 si no está en la lista
	int pos = DamePosicion (lista, nombre);
	if (pos == -1)
		return -1;
	else {
		for (int i=pos; i < lista->num-1; i++)
			lista->conectados[i] = lista->conectados[i+1];
		lista->num--;
		return 0;
	}
}

int EliminaPorSocket (ListaConectados *lista, int socket) {
	//Retorna 0 si elimina, -1 si no está en la lista
	int pos = DamePosicionPorSocket (lista, socket);
	if (pos == -1)
		return -1;
	else {
		for (int i=pos; i < lista->num-1; i++)
			lista->conectados[i] = lista->conectados[i+1];
		lista->num--;
		return 0;
	}
}

void DameConectados (ListaConectados *lista, char conectados[300]) {
	//Pone en conectados el nombre de todos los conectados separados por /
	//Empieza con el número de conectados, Ejemplo: "3/Juan/MAria/Pedro"
	sprintf (conectados, "%d", lista->num);
	for (int i=0; i < lista->num; i++)
		sprintf (conectados, "%s/%s", conectados, lista->conectados[i].nombre);
}

int i=0;
int sockets[100];

//Estructura necesaria para acceso excluyente
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;

void *AtenderCliente (void *socket) {
	int sock_conn = * (int *) socket;
	printf ("Socket: %d\n", sock_conn);
	
	char peticion[512];
	char respuesta[512];
	int ret;
	
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
	//conn = mysql_real_connect (conn, "localhost","root", "mysql", "M3_BD",0, NULL, 0); //Entorno Desarrollo (también quitar librería)
	conn = mysql_real_connect (conn, "shiva2.upc.es","root", "mysql", "M3_BD",0, NULL, 0); //Entorno Producción
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	int terminar =0;
	// Entramos en un bucle para atender todas las peticiones de este cliente
	//hasta que se desconecte
	while (terminar ==0)
	{
		// Ahora recibimos la peticion
		ret=read(sock_conn,peticion, sizeof(peticion));
		printf ("Recibido\n");
		// Tenemos que añadirle la marca de fin de string 
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
		
		if (codigo ==0) {
			//Peticion de Desconexion
			Desconectar(sock_conn);
			terminar=1;
		}
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
		else if (codigo ==3) //Peticion de Contraseña
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
		else if (codigo ==7) //Peticion de ganador más rápido
		{
			Rapido(respuesta,conn,sock_conn);
		}
		else
				 strcpy (respuesta,"Código de petición no válido");
		
		if (codigo !=0)
		{
			// Enviamos respuesta
			write (sock_conn,respuesta, strlen(respuesta));
		}
		
		if ((codigo ==2)||(codigo==0)) //Notificación de Lista Conectados
		{
			Conectados (respuesta);
		}
	}
	//Cerramos MYSQL
	mysql_close (conn);
	//Se acabo el servicio para este cliente
	close(sock_conn); 
}

int Desconectar (int socket) {
	//Borramos el cliente de la lista y cerramos conexion
	pthread_mutex_lock(&mutex); //No me interrumpas ahora
	int res = EliminaPorSocket(&lista,socket);
	pthread_mutex_unlock(&mutex); //ya puedes interrumpirme
	if (res == 0)
		printf ("Se ha eliminado el usuario\n");
	else
		printf ("No se había iniciado sesión\n");
	return 0;
}

int Registrarse (char usuario[20],char contrasena[20], char respuesta[100],MYSQL *conn,int sock_conn){
	//Funcion para registrarse
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	//Limitamos la longitud de carácteres del nombre de usuario para que no peten otras consultas
	if (strlen(usuario) > 20) {
		printf("El nombre de usuario es muy largo\n");
		strcpy(respuesta, "1/-2");
		return -2;
	}
	else {
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
			if (row == NULL)
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
				else {
					printf("%s se ha registrado correctamente\n", usuario);
					strcpy(respuesta, "1/0");
					return 0;
				}
			}
		}
		else {
			printf("El usuario %s ya existe\n", row[0]);
			strcpy(respuesta, "1/-1");
			return -1;
		}	
	}
	printf("%s\n", respuesta);
}
int Entrar (char usuario[20],char contrasena[20], char respuesta[100],MYSQL *conn,int sock_conn) {
	//Funcion para entrar
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[200];
	sprintf (consulta, "SELECT Jugador.Usuario FROM Jugador WHERE Jugador.Usuario = '%s' AND Jugador.Contrasena = '%s'", usuario, contrasena);
	//Consulta: Busca si existe el usuario y la contraseña
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
		printf ("Datos de acceso inválidos\n");
		strcpy(respuesta, "2/-3");
		return -3;
	}
	else {
		int pos = DamePosicionPorSocket (&lista, sock_conn);
		if (pos != -1) {
			printf("En este cliente ya se había iniciado sesión con otro usuario.\n");
			strcpy(respuesta, "2/-2");
			return -2;
		}
		else {
			pos = DamePosicion (&lista, usuario);
			if (pos == -1) {
				pthread_mutex_lock(&mutex);
				int pon = Pon (&lista,usuario,sock_conn);
				pthread_mutex_unlock(&mutex);
				if (pon == -1) {
					printf("No se ha podido iniciar sesión, la lista de conectados está llena y no se ha podido añadir.\n");
					strcpy(respuesta, "2/-1");
					return -1;
				}
				else{
					printf("%s ha iniciado sesión y se ha añadido a la lista de conectados\n", row[0]);
					strcpy(respuesta, "2/0");
					return 0;
				}
			}
			else {
				if (lista.conectados[pos].socket == sock_conn) {
					printf("%s ya había iniciado sesión en este cliente y está en la lista de conectados\n", row[0]);
					strcpy(respuesta, "2/1");
					return 1;
				}
				else {
					printf("%s ya había iniciado sesión en otro cliente y está en la lista de conectados\n", row[0]);
					strcpy(respuesta, "2/2");
				}
			}
		}
	}
	printf("%s\n", respuesta);
}
void Contrasena (char usuario[20], char respuesta[100],MYSQL *conn,int sock_conn){
	//Procedimiento para devolver la contrasena
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[100];
	sprintf (consulta, "SELECT Jugador.Contrasena FROM Jugador WHERE Jugador.Usuario = '%s'", usuario);
	//Consulta: Buscar la contraseña a partir del nombre de usuario
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
		strcpy(respuesta, "3/fail");
	}
	else {
		sprintf(respuesta, "3/%s", row[0]);
		printf("%s\n", respuesta);
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
		strcpy(respuesta, "4/fail");
	}
	else {
		strcpy(respuesta, "4/");
		while (row !=NULL) {
			printf("%s\n", row[0]);
			sprintf(respuesta, "%s%s/", respuesta, row[0]);
			row = mysql_fetch_row (resultado);
		}
		printf("%s\n", respuesta);
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
		strcpy(respuesta, "5/fail");
	}
	else {
		sprintf(respuesta, "5/%s", row[0]);
		printf("%s\n", respuesta);
	}
}
void Tiempo (int partida, char respuesta[100],MYSQL *conn,int sock_conn){
	//Procedimiento para devolver la duración de una partida
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[100];
	sprintf (consulta, "SELECT Partida.Tiempo FROM Partida WHERE Partida.ID = '%d'", partida);
	//Consulta: Buscar la duración de una partida a partir del identificador
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
		strcpy(respuesta, "6/fail");
	}
	else {
		int tiempo = atoi (row[0]);
		sprintf(respuesta, "6/%s", row[0]);
		printf("%s\n", respuesta);
	}
}
void Rapido (char respuesta[100],MYSQL *conn,int sock_conn){
	//Procedimiento para devolver el ganador mas rápido de una partida
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	//Consulta: Buscar la jugador que ha ganada más rápido
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
		strcpy(respuesta, "7/fail");
	}
	else {
		sprintf(respuesta, "7/%s", row[0]);
		printf("%s\n", respuesta);
	}
}

void Conectados (char respuesta[512]) {
	//Enviamos la Lista de Conectados como notificación
	DameConectados(&lista, respuesta);
	char notificacion[512];
	sprintf (notificacion, "8/%s", respuesta);
	printf("Conectados: %s\n",notificacion);
	for (int j=0; j < lista.num; j++)
		write (lista.conectados[j].socket, notificacion, strlen(notificacion));
}

int main(int argc, char *argv[]) {
	int sock_conn, sock_listen;
	struct sockaddr_in serv_adr;

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
	//serv_adr.sin_port = htons(9080); //Entorno Desarrollo
	serv_adr.sin_port = htons(50058); //Entorno Producción
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podr ser superior a 3
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen");
	
	pthread_t thread;
	// Bucle infinito
	for (;;){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		
		sockets[i] =sock_conn;
		//sock_conn es el socket que usaremos para este cliente
		
		// Crear thead y decirle lo que tiene que hacer
		pthread_create (&thread, NULL, AtenderCliente,&sockets[i]);
		i=i+1;
	}	
}
