// Decompiled with JetBrains decompiler
// Type: MigracionInventarios.Login
// Assembly: VKMIGINV, Version=1.0.0.4, Culture=neutral, PublicKeyToken=null
// MVID: 091BB591-6A6D-4391-A9A1-65EF14350A86
// Assembly location: C:\MigracionInventario\VKMIGINV.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MigracionInventarios
{
    public class Login : Form
    {
        private IContainer components = (IContainer)null;
        private Panel panel1;
        private Button btnCancel;
        private Button btnOk;
        private TextBox txtPassword;
        private TextBox txtUserID;
        private TextBox txtDatabase;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox txtServidor;
        private Label label4;

        public Login()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.doLogin();
        }

        public void doLogin()
        {
            if (string.IsNullOrEmpty(this.txtServidor.Text) || string.IsNullOrEmpty(this.txtPassword.Text) || string.IsNullOrEmpty(this.txtUserID.Text) || string.IsNullOrEmpty(this.txtDatabase.Text))
                return;
            try
            {
                new Conexion(this.txtServidor.Text, this.txtDatabase.Text, this.txtUserID.Text, this.txtPassword.Text).probarConexion();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("Ocurrio un error al realizar el login: " + ex.Message);
                return;
            }
            this.Hide();
            int num1 = (int)new Principal(this.txtServidor.Text, this.txtUserID.Text, this.txtPassword.Text, this.txtDatabase.Text).ShowDialog();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r')
                return;
            this.doLogin();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Login));
            this.panel1 = new Panel();
            this.txtServidor = new TextBox();
            this.label4 = new Label();
            this.btnCancel = new Button();
            this.btnOk = new Button();
            this.txtPassword = new TextBox();
            this.txtUserID = new TextBox();
            this.txtDatabase = new TextBox();
            this.label3 = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            this.panel1.BackColor = Color.Transparent;
            this.panel1.Controls.Add((Control)this.txtServidor);
            this.panel1.Controls.Add((Control)this.label4);
            this.panel1.Controls.Add((Control)this.btnCancel);
            this.panel1.Controls.Add((Control)this.btnOk);
            this.panel1.Controls.Add((Control)this.txtPassword);
            this.panel1.Controls.Add((Control)this.txtUserID);
            this.panel1.Controls.Add((Control)this.txtDatabase);
            this.panel1.Controls.Add((Control)this.label3);
            this.panel1.Controls.Add((Control)this.label2);
            this.panel1.Controls.Add((Control)this.label1);
            this.panel1.Location = new Point(12, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(442, 209);
            this.panel1.TabIndex = 1;
            this.txtServidor.Location = new Point(171, 11);
            this.txtServidor.Name = "txtServidor";
            this.txtServidor.Size = new Size(156, 20);
            this.txtServidor.TabIndex = 2;
            this.txtServidor.Tag = (object)"";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(116, 14);
            this.label4.Name = "label4";
            this.label4.Size = new Size(49, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Servidor:";
            this.btnCancel.Location = new Point(252, 115);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOk.Location = new Point(171, 115);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(75, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Aceptar";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.txtPassword.Location = new Point(171, 89);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new Size(156, 20);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.KeyPress += new KeyPressEventHandler(this.txtPassword_KeyPress);
            this.txtUserID.Location = new Point(171, 63);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new Size(156, 20);
            this.txtUserID.TabIndex = 4;
            this.txtDatabase.Location = new Point(171, 37);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new Size(156, 20);
            this.txtDatabase.TabIndex = 3;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(101, 92);
            this.label3.Name = "label3";
            this.label3.Size = new Size(64, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Contraseña:";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(119, 66);
            this.label2.Name = "label2";
            this.label2.Size = new Size(46, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Usuario:";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(85, 40);
            this.label1.Name = "label1";
            this.label1.Size = new Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Base de Datos:";
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackgroundImage = (Image)componentResourceManager.GetObject("$this.BackgroundImage");
            this.ClientSize = new Size(462, 327);
            this.Controls.Add((Control)this.panel1);
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = nameof(Login);
            this.Text = nameof(Login);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
