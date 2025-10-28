namespace WorldSkillsRussia
{
    partial class FormInfoUchastniki
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
            this.labelTitleChamp = new System.Windows.Forms.Label();
            this.labelTitleSkill = new System.Windows.Forms.Label();
            this.pictureBLogo = new System.Windows.Forms.PictureBox();
            this.butBack = new System.Windows.Forms.Button();
            this.butVozrastCenz = new System.Windows.Forms.Button();
            this.butSecure = new System.Windows.Forms.Button();
            this.butZjerib = new System.Windows.Forms.Button();
            this.butOznakomlenKonkursDocum = new System.Windows.Forms.Button();
            this.butOznakomNormatDocum = new System.Windows.Forms.Button();
            this.butOznakomlenMest = new System.Windows.Forms.Button();
            this.butCheckToolBox = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTitleChamp
            // 
            this.labelTitleChamp.AutoSize = true;
            this.labelTitleChamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTitleChamp.Location = new System.Drawing.Point(107, 13);
            this.labelTitleChamp.Name = "labelTitleChamp";
            this.labelTitleChamp.Size = new System.Drawing.Size(218, 20);
            this.labelTitleChamp.TabIndex = 0;
            this.labelTitleChamp.Text = "Наименование чемпионата";
            // 
            // labelTitleSkill
            // 
            this.labelTitleSkill.AutoSize = true;
            this.labelTitleSkill.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTitleSkill.Location = new System.Drawing.Point(107, 43);
            this.labelTitleSkill.Name = "labelTitleSkill";
            this.labelTitleSkill.Size = new System.Drawing.Size(111, 20);
            this.labelTitleSkill.TabIndex = 1;
            this.labelTitleSkill.Text = "Компетенция";
            // 
            // pictureBLogo
            // 
            this.pictureBLogo.BackgroundImage = global::WorldSkillsRussia.Properties.Resources.wsrlogo_Red;
            this.pictureBLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBLogo.Location = new System.Drawing.Point(650, 35);
            this.pictureBLogo.Name = "pictureBLogo";
            this.pictureBLogo.Size = new System.Drawing.Size(138, 70);
            this.pictureBLogo.TabIndex = 2;
            this.pictureBLogo.TabStop = false;
            // 
            // butBack
            // 
            this.butBack.Location = new System.Drawing.Point(13, 13);
            this.butBack.Name = "butBack";
            this.butBack.Size = new System.Drawing.Size(75, 23);
            this.butBack.TabIndex = 3;
            this.butBack.Text = "Назад";
            this.butBack.UseVisualStyleBackColor = true;
            this.butBack.Click += new System.EventHandler(this.butBack_Click);
            // 
            // butVozrastCenz
            // 
            this.butVozrastCenz.Location = new System.Drawing.Point(73, 138);
            this.butVozrastCenz.Name = "butVozrastCenz";
            this.butVozrastCenz.Size = new System.Drawing.Size(292, 66);
            this.butVozrastCenz.TabIndex = 4;
            this.butVozrastCenz.Text = "Регистрация в соответствии возрастному цензу";
            this.butVozrastCenz.UseVisualStyleBackColor = true;
            this.butVozrastCenz.Click += new System.EventHandler(this.butVozrastCenz_Click);
            // 
            // butSecure
            // 
            this.butSecure.Location = new System.Drawing.Point(73, 210);
            this.butSecure.Name = "butSecure";
            this.butSecure.Size = new System.Drawing.Size(292, 66);
            this.butSecure.TabIndex = 5;
            this.butSecure.Text = "Ознакомление участников с техникой безопасности";
            this.butSecure.UseVisualStyleBackColor = true;
            // 
            // butZjerib
            // 
            this.butZjerib.Location = new System.Drawing.Point(73, 282);
            this.butZjerib.Name = "butZjerib";
            this.butZjerib.Size = new System.Drawing.Size(292, 66);
            this.butZjerib.TabIndex = 6;
            this.butZjerib.Text = "Жеребьевка и распределение конкурсных мест";
            this.butZjerib.UseVisualStyleBackColor = true;
            // 
            // butOznakomlenKonkursDocum
            // 
            this.butOznakomlenKonkursDocum.Location = new System.Drawing.Point(417, 282);
            this.butOznakomlenKonkursDocum.Name = "butOznakomlenKonkursDocum";
            this.butOznakomlenKonkursDocum.Size = new System.Drawing.Size(292, 66);
            this.butOznakomlenKonkursDocum.TabIndex = 9;
            this.butOznakomlenKonkursDocum.Text = "Ознакомление с конкурсной документацией";
            this.butOznakomlenKonkursDocum.UseVisualStyleBackColor = true;
            // 
            // butOznakomNormatDocum
            // 
            this.butOznakomNormatDocum.Location = new System.Drawing.Point(417, 210);
            this.butOznakomNormatDocum.Name = "butOznakomNormatDocum";
            this.butOznakomNormatDocum.Size = new System.Drawing.Size(292, 66);
            this.butOznakomNormatDocum.TabIndex = 8;
            this.butOznakomNormatDocum.Text = "Ознакомление с нормативной документацией";
            this.butOznakomNormatDocum.UseVisualStyleBackColor = true;
            // 
            // butOznakomlenMest
            // 
            this.butOznakomlenMest.Location = new System.Drawing.Point(417, 138);
            this.butOznakomlenMest.Name = "butOznakomlenMest";
            this.butOznakomlenMest.Size = new System.Drawing.Size(292, 66);
            this.butOznakomlenMest.TabIndex = 7;
            this.butOznakomlenMest.Text = "Ознакомление с рабочими местами, конкурсной документацией и оборудованием";
            this.butOznakomlenMest.UseVisualStyleBackColor = true;
            // 
            // butCheckToolBox
            // 
            this.butCheckToolBox.Location = new System.Drawing.Point(243, 354);
            this.butCheckToolBox.Name = "butCheckToolBox";
            this.butCheckToolBox.Size = new System.Drawing.Size(292, 66);
            this.butCheckToolBox.TabIndex = 10;
            this.butCheckToolBox.Text = "Проверка Тулбокса";
            this.butCheckToolBox.UseVisualStyleBackColor = true;
            // 
            // FormInfoUchastniki
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.butCheckToolBox);
            this.Controls.Add(this.butOznakomlenKonkursDocum);
            this.Controls.Add(this.butOznakomNormatDocum);
            this.Controls.Add(this.butOznakomlenMest);
            this.Controls.Add(this.butZjerib);
            this.Controls.Add(this.butSecure);
            this.Controls.Add(this.butVozrastCenz);
            this.Controls.Add(this.butBack);
            this.Controls.Add(this.pictureBLogo);
            this.Controls.Add(this.labelTitleSkill);
            this.Controls.Add(this.labelTitleChamp);
            this.Name = "FormInfoUchastniki";
            this.Text = "Участник";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitleChamp;
        private System.Windows.Forms.Label labelTitleSkill;
        private System.Windows.Forms.PictureBox pictureBLogo;
        private System.Windows.Forms.Button butBack;
        private System.Windows.Forms.Button butVozrastCenz;
        private System.Windows.Forms.Button butSecure;
        private System.Windows.Forms.Button butZjerib;
        private System.Windows.Forms.Button butOznakomlenKonkursDocum;
        private System.Windows.Forms.Button butOznakomNormatDocum;
        private System.Windows.Forms.Button butOznakomlenMest;
        private System.Windows.Forms.Button butCheckToolBox;
    }
}