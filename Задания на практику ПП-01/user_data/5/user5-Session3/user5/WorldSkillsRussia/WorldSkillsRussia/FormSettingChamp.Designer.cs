namespace WorldSkillsRussia
{
    partial class FormSettingChamp
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelMainSetting = new System.Windows.Forms.Panel();
            this.checkBRoleZam = new System.Windows.Forms.CheckBox();
            this.checkBRoleExpert = new System.Windows.Forms.CheckBox();
            this.butMainSetting = new System.Windows.Forms.Button();
            this.butListProtocol = new System.Windows.Forms.Button();
            this.butListExtraRight = new System.Windows.Forms.Button();
            this.butSave = new System.Windows.Forms.Button();
            this.pictureBLogo = new System.Windows.Forms.PictureBox();
            this.panelMain.SuspendLayout();
            this.panelMainSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelMainSetting);
            this.panelMain.Location = new System.Drawing.Point(1, 51);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(800, 232);
            this.panelMain.TabIndex = 0;
            // 
            // panelMainSetting
            // 
            this.panelMainSetting.Controls.Add(this.checkBRoleZam);
            this.panelMainSetting.Controls.Add(this.checkBRoleExpert);
            this.panelMainSetting.Location = new System.Drawing.Point(11, 111);
            this.panelMainSetting.Name = "panelMainSetting";
            this.panelMainSetting.Size = new System.Drawing.Size(421, 100);
            this.panelMainSetting.TabIndex = 0;
            // 
            // checkBRoleZam
            // 
            this.checkBRoleZam.AutoSize = true;
            this.checkBRoleZam.Location = new System.Drawing.Point(22, 44);
            this.checkBRoleZam.Name = "checkBRoleZam";
            this.checkBRoleZam.Size = new System.Drawing.Size(330, 17);
            this.checkBRoleZam.TabIndex = 1;
            this.checkBRoleZam.Text = "Зам главного эксперта может принимать участие в оценке";
            this.checkBRoleZam.UseVisualStyleBackColor = true;
            // 
            // checkBRoleExpert
            // 
            this.checkBRoleExpert.AutoSize = true;
            this.checkBRoleExpert.Checked = true;
            this.checkBRoleExpert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBRoleExpert.Location = new System.Drawing.Point(22, 21);
            this.checkBRoleExpert.Name = "checkBRoleExpert";
            this.checkBRoleExpert.Size = new System.Drawing.Size(303, 17);
            this.checkBRoleExpert.TabIndex = 0;
            this.checkBRoleExpert.Text = "Главный эксперт может принимать участие в оцентке";
            this.checkBRoleExpert.UseVisualStyleBackColor = true;
            // 
            // butMainSetting
            // 
            this.butMainSetting.Location = new System.Drawing.Point(71, 12);
            this.butMainSetting.Name = "butMainSetting";
            this.butMainSetting.Size = new System.Drawing.Size(174, 38);
            this.butMainSetting.TabIndex = 1;
            this.butMainSetting.Text = "Основные настройки";
            this.butMainSetting.UseVisualStyleBackColor = true;
            // 
            // butListProtocol
            // 
            this.butListProtocol.Location = new System.Drawing.Point(312, 12);
            this.butListProtocol.Name = "butListProtocol";
            this.butListProtocol.Size = new System.Drawing.Size(174, 38);
            this.butListProtocol.TabIndex = 2;
            this.butListProtocol.Text = "Список протоколов";
            this.butListProtocol.UseVisualStyleBackColor = true;
            this.butListProtocol.Click += new System.EventHandler(this.butListProtocol_Click);
            // 
            // butListExtraRight
            // 
            this.butListExtraRight.Location = new System.Drawing.Point(554, 12);
            this.butListExtraRight.Name = "butListExtraRight";
            this.butListExtraRight.Size = new System.Drawing.Size(174, 38);
            this.butListExtraRight.TabIndex = 3;
            this.butListExtraRight.Text = "Список особых полномичий экспертов";
            this.butListExtraRight.UseVisualStyleBackColor = true;
            // 
            // butSave
            // 
            this.butSave.Location = new System.Drawing.Point(330, 289);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(146, 37);
            this.butSave.TabIndex = 4;
            this.butSave.Text = "Сохранить";
            this.butSave.UseVisualStyleBackColor = true;
            // 
            // pictureBLogo
            // 
            this.pictureBLogo.BackgroundImage = global::WorldSkillsRussia.Properties.Resources.wsrlogo_Red;
            this.pictureBLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBLogo.Location = new System.Drawing.Point(634, 289);
            this.pictureBLogo.Name = "pictureBLogo";
            this.pictureBLogo.Size = new System.Drawing.Size(154, 66);
            this.pictureBLogo.TabIndex = 21;
            this.pictureBLogo.TabStop = false;
            // 
            // FormSettingChamp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 362);
            this.Controls.Add(this.pictureBLogo);
            this.Controls.Add(this.butSave);
            this.Controls.Add(this.butListProtocol);
            this.Controls.Add(this.butMainSetting);
            this.Controls.Add(this.butListExtraRight);
            this.Controls.Add(this.panelMain);
            this.Name = "FormSettingChamp";
            this.Text = "Настройки чемпионата";
            this.panelMain.ResumeLayout(false);
            this.panelMainSetting.ResumeLayout(false);
            this.panelMainSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button butMainSetting;
        private System.Windows.Forms.Button butListProtocol;
        private System.Windows.Forms.Button butListExtraRight;
        private System.Windows.Forms.Panel panelMainSetting;
        private System.Windows.Forms.CheckBox checkBRoleZam;
        private System.Windows.Forms.CheckBox checkBRoleExpert;
        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.PictureBox pictureBLogo;
    }
}