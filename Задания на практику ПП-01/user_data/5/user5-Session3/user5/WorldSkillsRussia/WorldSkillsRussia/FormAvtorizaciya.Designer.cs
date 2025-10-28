namespace WorldSkillsRussia
{
    partial class FormAvtorizaciya
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelTitleAvtoriz = new System.Windows.Forms.Label();
            this.labelLogin = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.tBLogin = new System.Windows.Forms.TextBox();
            this.tBPassword = new System.Windows.Forms.TextBox();
            this.checkBRemMe = new System.Windows.Forms.CheckBox();
            this.butLogin = new System.Windows.Forms.Button();
            this.pictureBLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTitleAvtoriz
            // 
            this.labelTitleAvtoriz.AutoSize = true;
            this.labelTitleAvtoriz.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTitleAvtoriz.Location = new System.Drawing.Point(329, 110);
            this.labelTitleAvtoriz.Name = "labelTitleAvtoriz";
            this.labelTitleAvtoriz.Size = new System.Drawing.Size(140, 24);
            this.labelTitleAvtoriz.TabIndex = 0;
            this.labelTitleAvtoriz.Text = "Авторизация";
            // 
            // labelLogin
            // 
            this.labelLogin.AutoSize = true;
            this.labelLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLogin.Location = new System.Drawing.Point(232, 163);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(55, 20);
            this.labelLogin.TabIndex = 1;
            this.labelLogin.Text = "Логин";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPassword.Location = new System.Drawing.Point(220, 205);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(67, 20);
            this.labelPassword.TabIndex = 2;
            this.labelPassword.Text = "Пароль";
            // 
            // tBLogin
            // 
            this.tBLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tBLogin.Location = new System.Drawing.Point(312, 158);
            this.tBLogin.MaxLength = 5;
            this.tBLogin.Name = "tBLogin";
            this.tBLogin.Size = new System.Drawing.Size(204, 26);
            this.tBLogin.TabIndex = 3;
            // 
            // tBPassword
            // 
            this.tBPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tBPassword.Location = new System.Drawing.Point(312, 202);
            this.tBPassword.MaxLength = 255;
            this.tBPassword.Name = "tBPassword";
            this.tBPassword.Size = new System.Drawing.Size(204, 26);
            this.tBPassword.TabIndex = 3;
            // 
            // checkBRemMe
            // 
            this.checkBRemMe.AutoSize = true;
            this.checkBRemMe.Checked = true;
            this.checkBRemMe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBRemMe.Location = new System.Drawing.Point(312, 251);
            this.checkBRemMe.Name = "checkBRemMe";
            this.checkBRemMe.Size = new System.Drawing.Size(111, 17);
            this.checkBRemMe.TabIndex = 4;
            this.checkBRemMe.Text = "Запомните меня";
            this.checkBRemMe.UseVisualStyleBackColor = true;
            // 
            // butLogin
            // 
            this.butLogin.BackColor = System.Drawing.Color.Tomato;
            this.butLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butLogin.ForeColor = System.Drawing.SystemColors.Desktop;
            this.butLogin.Location = new System.Drawing.Point(344, 297);
            this.butLogin.Name = "butLogin";
            this.butLogin.Size = new System.Drawing.Size(143, 53);
            this.butLogin.TabIndex = 5;
            this.butLogin.Text = "Логин";
            this.butLogin.UseVisualStyleBackColor = false;
            this.butLogin.Click += new System.EventHandler(this.butLogin_Click);
            // 
            // pictureBLogo
            // 
            this.pictureBLogo.BackgroundImage = global::WorldSkillsRussia.Properties.Resources.wsrlogo_Red;
            this.pictureBLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBLogo.Location = new System.Drawing.Point(590, 12);
            this.pictureBLogo.Name = "pictureBLogo";
            this.pictureBLogo.Size = new System.Drawing.Size(198, 81);
            this.pictureBLogo.TabIndex = 21;
            this.pictureBLogo.TabStop = false;
            // 
            // FormAvtorizaciya
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBLogo);
            this.Controls.Add(this.butLogin);
            this.Controls.Add(this.checkBRemMe);
            this.Controls.Add(this.tBPassword);
            this.Controls.Add(this.tBLogin);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelLogin);
            this.Controls.Add(this.labelTitleAvtoriz);
            this.Name = "FormAvtorizaciya";
            this.Text = "Авторизация";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitleAvtoriz;
        private System.Windows.Forms.Label labelLogin;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox tBLogin;
        private System.Windows.Forms.TextBox tBPassword;
        private System.Windows.Forms.CheckBox checkBRemMe;
        private System.Windows.Forms.Button butLogin;
        private System.Windows.Forms.PictureBox pictureBLogo;
    }
}

