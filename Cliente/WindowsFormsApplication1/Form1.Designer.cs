namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.UsuarioBox = new System.Windows.Forms.TextBox();
            this.Conectar = new System.Windows.Forms.Button();
            this.Consultar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Entrar = new System.Windows.Forms.Button();
            this.Registrarse = new System.Windows.Forms.Button();
            this.ContraseñaBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Contraseña = new System.Windows.Forms.RadioButton();
            this.Jugadores = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.PartidaBox = new System.Windows.Forms.TextBox();
            this.Ganador = new System.Windows.Forms.RadioButton();
            this.Desconectar = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Rápido = new System.Windows.Forms.RadioButton();
            this.Tiempo = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ConectadosGrid = new System.Windows.Forms.DataGridView();
            this.PassCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConectadosGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(33, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Usuario";
            // 
            // UsuarioBox
            // 
            this.UsuarioBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsuarioBox.Location = new System.Drawing.Point(92, 33);
            this.UsuarioBox.Name = "UsuarioBox";
            this.UsuarioBox.Size = new System.Drawing.Size(120, 20);
            this.UsuarioBox.TabIndex = 3;
            // 
            // Conectar
            // 
            this.Conectar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Conectar.Location = new System.Drawing.Point(23, 12);
            this.Conectar.Name = "Conectar";
            this.Conectar.Size = new System.Drawing.Size(109, 28);
            this.Conectar.TabIndex = 4;
            this.Conectar.Text = "Conectar";
            this.Conectar.UseVisualStyleBackColor = true;
            this.Conectar.Click += new System.EventHandler(this.Conectar_Click);
            // 
            // Consultar
            // 
            this.Consultar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Consultar.Location = new System.Drawing.Point(80, 154);
            this.Consultar.Name = "Consultar";
            this.Consultar.Size = new System.Drawing.Size(75, 23);
            this.Consultar.TabIndex = 5;
            this.Consultar.Text = "Consultar";
            this.Consultar.UseVisualStyleBackColor = true;
            this.Consultar.Click += new System.EventHandler(this.Consultar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Silver;
            this.groupBox1.Controls.Add(this.PassCheckBox);
            this.groupBox1.Controls.Add(this.Entrar);
            this.groupBox1.Controls.Add(this.Registrarse);
            this.groupBox1.Controls.Add(this.ContraseñaBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.UsuarioBox);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(23, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 160);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Acceso";
            // 
            // Entrar
            // 
            this.Entrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Entrar.Location = new System.Drawing.Point(116, 128);
            this.Entrar.Name = "Entrar";
            this.Entrar.Size = new System.Drawing.Size(75, 23);
            this.Entrar.TabIndex = 11;
            this.Entrar.Text = "Entrar";
            this.Entrar.UseVisualStyleBackColor = true;
            this.Entrar.Click += new System.EventHandler(this.Entrar_Click);
            // 
            // Registrarse
            // 
            this.Registrarse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Registrarse.Location = new System.Drawing.Point(35, 128);
            this.Registrarse.Name = "Registrarse";
            this.Registrarse.Size = new System.Drawing.Size(75, 23);
            this.Registrarse.TabIndex = 10;
            this.Registrarse.Text = "Registrarse";
            this.Registrarse.UseVisualStyleBackColor = true;
            this.Registrarse.Click += new System.EventHandler(this.Registrarse_Click);
            // 
            // ContraseñaBox
            // 
            this.ContraseñaBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContraseñaBox.Location = new System.Drawing.Point(92, 64);
            this.ContraseñaBox.Name = "ContraseñaBox";
            this.ContraseñaBox.Size = new System.Drawing.Size(120, 20);
            this.ContraseñaBox.TabIndex = 5;
            this.ContraseñaBox.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Contraseña";
            // 
            // Contraseña
            // 
            this.Contraseña.AutoSize = true;
            this.Contraseña.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Contraseña.Location = new System.Drawing.Point(28, 30);
            this.Contraseña.Name = "Contraseña";
            this.Contraseña.Size = new System.Drawing.Size(133, 17);
            this.Contraseña.TabIndex = 7;
            this.Contraseña.TabStop = true;
            this.Contraseña.Text = "Contraseña del usuario";
            this.Contraseña.UseVisualStyleBackColor = true;
            // 
            // Jugadores
            // 
            this.Jugadores.AutoSize = true;
            this.Jugadores.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Jugadores.Location = new System.Drawing.Point(28, 53);
            this.Jugadores.Name = "Jugadores";
            this.Jugadores.Size = new System.Drawing.Size(135, 17);
            this.Jugadores.TabIndex = 7;
            this.Jugadores.TabStop = true;
            this.Jugadores.Text = "Jugadores de la partida";
            this.Jugadores.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label3.Location = new System.Drawing.Point(27, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Partida";
            // 
            // PartidaBox
            // 
            this.PartidaBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PartidaBox.Location = new System.Drawing.Point(73, 27);
            this.PartidaBox.Name = "PartidaBox";
            this.PartidaBox.Size = new System.Drawing.Size(62, 20);
            this.PartidaBox.TabIndex = 9;
            // 
            // Ganador
            // 
            this.Ganador.AutoSize = true;
            this.Ganador.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ganador.Location = new System.Drawing.Point(28, 76);
            this.Ganador.Name = "Ganador";
            this.Ganador.Size = new System.Drawing.Size(127, 17);
            this.Ganador.TabIndex = 8;
            this.Ganador.TabStop = true;
            this.Ganador.Text = "Ganador de la partida";
            this.Ganador.UseVisualStyleBackColor = true;
            // 
            // Desconectar
            // 
            this.Desconectar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Desconectar.Location = new System.Drawing.Point(139, 12);
            this.Desconectar.Name = "Desconectar";
            this.Desconectar.Size = new System.Drawing.Size(109, 28);
            this.Desconectar.TabIndex = 10;
            this.Desconectar.Text = "Desconectar";
            this.Desconectar.UseVisualStyleBackColor = true;
            this.Desconectar.Click += new System.EventHandler(this.Desconectar_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Silver;
            this.groupBox2.Controls.Add(this.Rápido);
            this.groupBox2.Controls.Add(this.Tiempo);
            this.groupBox2.Controls.Add(this.Jugadores);
            this.groupBox2.Controls.Add(this.Contraseña);
            this.groupBox2.Controls.Add(this.Consultar);
            this.groupBox2.Controls.Add(this.Ganador);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(23, 282);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(225, 187);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Petición";
            // 
            // Rápido
            // 
            this.Rápido.AutoSize = true;
            this.Rápido.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Rápido.Location = new System.Drawing.Point(28, 122);
            this.Rápido.Name = "Rápido";
            this.Rápido.Size = new System.Drawing.Size(120, 17);
            this.Rápido.TabIndex = 10;
            this.Rápido.TabStop = true;
            this.Rápido.Text = "Ganador más rápido";
            this.Rápido.UseVisualStyleBackColor = true;
            // 
            // Tiempo
            // 
            this.Tiempo.AutoSize = true;
            this.Tiempo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Tiempo.Location = new System.Drawing.Point(28, 99);
            this.Tiempo.Name = "Tiempo";
            this.Tiempo.Size = new System.Drawing.Size(129, 17);
            this.Tiempo.TabIndex = 9;
            this.Tiempo.TabStop = true;
            this.Tiempo.Text = "Duración de la partida";
            this.Tiempo.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Silver;
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.PartidaBox);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(23, 212);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(225, 64);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Datos";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Silver;
            this.groupBox4.Controls.Add(this.ConectadosGrid);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(254, 46);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(194, 423);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Lista Conectados";
            // 
            // ConectadosGrid
            // 
            this.ConectadosGrid.AllowUserToResizeColumns = false;
            this.ConectadosGrid.AllowUserToResizeRows = false;
            this.ConectadosGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ConectadosGrid.BackgroundColor = System.Drawing.Color.Silver;
            this.ConectadosGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ConectadosGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConectadosGrid.ColumnHeadersVisible = false;
            this.ConectadosGrid.Location = new System.Drawing.Point(15, 33);
            this.ConectadosGrid.Name = "ConectadosGrid";
            this.ConectadosGrid.ReadOnly = true;
            this.ConectadosGrid.RowHeadersVisible = false;
            this.ConectadosGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.ConectadosGrid.Size = new System.Drawing.Size(161, 363);
            this.ConectadosGrid.TabIndex = 11;
            // 
            // PassCheckBox
            // 
            this.PassCheckBox.AutoSize = true;
            this.PassCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PassCheckBox.Location = new System.Drawing.Point(60, 99);
            this.PassCheckBox.Name = "PassCheckBox";
            this.PassCheckBox.Size = new System.Drawing.Size(122, 17);
            this.PassCheckBox.TabIndex = 12;
            this.PassCheckBox.Text = "Mostrar constraseña";
            this.PassCheckBox.UseVisualStyleBackColor = true;
            this.PassCheckBox.CheckedChanged += new System.EventHandler(this.PassCheckBox_Check);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 490);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Desconectar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Conectar);
            this.Name = "Form1";
            this.Text = "Cliente";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ConectadosGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox UsuarioBox;
        private System.Windows.Forms.Button Conectar;
        private System.Windows.Forms.Button Consultar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton Contraseña;
        private System.Windows.Forms.RadioButton Ganador;
        private System.Windows.Forms.RadioButton Jugadores;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PartidaBox;
        private System.Windows.Forms.Button Desconectar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ContraseñaBox;
        private System.Windows.Forms.Button Entrar;
        private System.Windows.Forms.Button Registrarse;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton Tiempo;
        private System.Windows.Forms.RadioButton Rápido;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView ConectadosGrid;
        private System.Windows.Forms.CheckBox PassCheckBox;
    }
}

