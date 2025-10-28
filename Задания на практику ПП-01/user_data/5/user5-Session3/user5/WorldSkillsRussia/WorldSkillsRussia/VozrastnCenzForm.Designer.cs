namespace WorldSkillsRussia
{
    partial class VozrastnCenzForm
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
            this.butBack = new System.Windows.Forms.Button();
            this.labelTitleSkill = new System.Windows.Forms.Label();
            this.labelTitleChamp = new System.Windows.Forms.Label();
            this.pictureBLogo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // butBack
            // 
            this.butBack.Location = new System.Drawing.Point(12, 12);
            this.butBack.Name = "butBack";
            this.butBack.Size = new System.Drawing.Size(75, 23);
            this.butBack.TabIndex = 4;
            this.butBack.Text = "Назад";
            this.butBack.UseVisualStyleBackColor = true;
            this.butBack.Click += new System.EventHandler(this.butBack_Click);
            // 
            // labelTitleSkill
            // 
            this.labelTitleSkill.AutoSize = true;
            this.labelTitleSkill.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTitleSkill.Location = new System.Drawing.Point(179, 45);
            this.labelTitleSkill.Name = "labelTitleSkill";
            this.labelTitleSkill.Size = new System.Drawing.Size(111, 20);
            this.labelTitleSkill.TabIndex = 6;
            this.labelTitleSkill.Text = "Компетенция";
            // 
            // labelTitleChamp
            // 
            this.labelTitleChamp.AutoSize = true;
            this.labelTitleChamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTitleChamp.Location = new System.Drawing.Point(179, 15);
            this.labelTitleChamp.Name = "labelTitleChamp";
            this.labelTitleChamp.Size = new System.Drawing.Size(218, 20);
            this.labelTitleChamp.TabIndex = 5;
            this.labelTitleChamp.Text = "Наименование чемпионата";
            // 
            // pictureBLogo
            // 
            this.pictureBLogo.BackgroundImage = global::WorldSkillsRussia.Properties.Resources.wsrlogo_Red;
            this.pictureBLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBLogo.Location = new System.Drawing.Point(650, 45);
            this.pictureBLogo.Name = "pictureBLogo";
            this.pictureBLogo.Size = new System.Drawing.Size(138, 70);
            this.pictureBLogo.TabIndex = 7;
            this.pictureBLogo.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(84, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(655, 32);
            this.label1.TabIndex = 8;
            this.label1.Text = "Протокол чемпионата по стандартам WorldSkills Russia об ознакомлении участников с" +
    " правилами\r\nтехники безопасности и охраны труда";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 168);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(776, 270);
            this.dataGridView1.TabIndex = 9;
            // 
            // VozrastnCenzForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBLogo);
            this.Controls.Add(this.labelTitleSkill);
            this.Controls.Add(this.labelTitleChamp);
            this.Controls.Add(this.butBack);
            this.Name = "VozrastnCenzForm";
            this.Text = "Возрастной ценз";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butBack;
        private System.Windows.Forms.Label labelTitleSkill;
        private System.Windows.Forms.Label labelTitleChamp;
        private System.Windows.Forms.PictureBox pictureBLogo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}