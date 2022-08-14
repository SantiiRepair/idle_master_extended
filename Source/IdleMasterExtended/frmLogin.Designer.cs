namespace IdleMasterExtended
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxLogin = new System.Windows.Forms.GroupBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.textBoxSteamGuard = new System.Windows.Forms.TextBox();
            this.checkBoxSteamGuard = new System.Windows.Forms.CheckBox();
            this.checkBoxSaveCreds = new System.Windows.Forms.CheckBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.groupBoxLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxLogin
            // 
            this.groupBoxLogin.Controls.Add(this.checkBoxSaveCreds);
            this.groupBoxLogin.Controls.Add(this.textBoxSteamGuard);
            this.groupBoxLogin.Controls.Add(this.checkBoxSteamGuard);
            this.groupBoxLogin.Controls.Add(this.labelPassword);
            this.groupBoxLogin.Controls.Add(this.textBoxUsername);
            this.groupBoxLogin.Controls.Add(this.textBoxPassword);
            this.groupBoxLogin.Controls.Add(this.labelUsername);
            this.groupBoxLogin.Location = new System.Drawing.Point(12, 12);
            this.groupBoxLogin.Name = "groupBoxLogin";
            this.groupBoxLogin.Size = new System.Drawing.Size(245, 127);
            this.groupBoxLogin.TabIndex = 1;
            this.groupBoxLogin.TabStop = false;
            this.groupBoxLogin.Text = "Steam Credentials";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(70, 49);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(169, 20);
            this.textBoxPassword.TabIndex = 2;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(6, 52);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 13);
            this.labelPassword.TabIndex = 1;
            this.labelPassword.Text = "Password:";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(70, 23);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(169, 20);
            this.textBoxUsername.TabIndex = 3;
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(6, 26);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(58, 13);
            this.labelUsername.TabIndex = 4;
            this.labelUsername.Text = "Username:";
            // 
            // textBoxSteamGuard
            // 
            this.textBoxSteamGuard.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxSteamGuard.Location = new System.Drawing.Point(134, 96);
            this.textBoxSteamGuard.Name = "textBoxSteamGuard";
            this.textBoxSteamGuard.Size = new System.Drawing.Size(105, 20);
            this.textBoxSteamGuard.TabIndex = 6;
            // 
            // checkBoxSteamGuard
            // 
            this.checkBoxSteamGuard.AutoSize = true;
            this.checkBoxSteamGuard.Location = new System.Drawing.Point(9, 98);
            this.checkBoxSteamGuard.Name = "checkBoxSteamGuard";
            this.checkBoxSteamGuard.Size = new System.Drawing.Size(119, 17);
            this.checkBoxSteamGuard.TabIndex = 7;
            this.checkBoxSteamGuard.Text = "Steam Guard Code:";
            this.checkBoxSteamGuard.UseVisualStyleBackColor = true;
            // 
            // checkBoxSaveCreds
            // 
            this.checkBoxSaveCreds.AutoSize = true;
            this.checkBoxSaveCreds.Location = new System.Drawing.Point(9, 75);
            this.checkBoxSaveCreds.Name = "checkBoxSaveCreds";
            this.checkBoxSaveCreds.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBoxSaveCreds.Size = new System.Drawing.Size(195, 17);
            this.checkBoxSaveCreds.TabIndex = 2;
            this.checkBoxSaveCreds.Text = "Remember username and password";
            this.checkBoxSaveCreds.UseVisualStyleBackColor = true;
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(146, 145);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(105, 23);
            this.buttonLogin.TabIndex = 2;
            this.buttonLogin.Text = "Login to Steam";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // btnView
            // 
            this.btnView.Image = global::IdleMasterExtended.Properties.Resources.imgView;
            this.btnView.Location = new System.Drawing.Point(12, 146);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(27, 23);
            this.btnView.TabIndex = 7;
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 181);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.groupBoxLogin);
            this.Name = "frmLogin";
            this.Text = "Login to Steam";
            this.groupBoxLogin.ResumeLayout(false);
            this.groupBoxLogin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxLogin;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.CheckBox checkBoxSaveCreds;
        private System.Windows.Forms.TextBox textBoxSteamGuard;
        private System.Windows.Forms.CheckBox checkBoxSteamGuard;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Button btnView;
    }
}