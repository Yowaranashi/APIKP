namespace WorldSkillsRussia
{
    partial class FormExpert
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
            this.butQuit = new System.Windows.Forms.Button();
            this.labelInfoAboutPerson = new System.Windows.Forms.Label();
            this.labelTitleChamp = new System.Windows.Forms.Label();
            this.labelTitleComp = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelExpert = new System.Windows.Forms.Panel();
            this.panelUch = new System.Windows.Forms.Panel();
            this.butDelUch = new System.Windows.Forms.Button();
            this.butEditUch = new System.Windows.Forms.Button();
            this.butAddUch = new System.Windows.Forms.Button();
            this.butSaveUch = new System.Windows.Forms.Button();
            this.dataGVUchastniki = new System.Windows.Forms.DataGridView();
            this.butSaveExpert = new System.Windows.Forms.Button();
            this.butDelExpert = new System.Windows.Forms.Button();
            this.butEditExpert = new System.Windows.Forms.Button();
            this.butAddExpert = new System.Windows.Forms.Button();
            this.butBlockExpert = new System.Windows.Forms.Button();
            this.dataGVExpert = new System.Windows.Forms.DataGridView();
            this.butListUchastnic = new System.Windows.Forms.Button();
            this.butListExpert = new System.Windows.Forms.Button();
            this.butProtocol = new System.Windows.Forms.Button();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panelExpert.SuspendLayout();
            this.panelUch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGVUchastniki)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGVExpert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // butQuit
            // 
            this.butQuit.Location = new System.Drawing.Point(13, 13);
            this.butQuit.Name = "butQuit";
            this.butQuit.Size = new System.Drawing.Size(75, 23);
            this.butQuit.TabIndex = 0;
            this.butQuit.Text = "Выход";
            this.butQuit.UseVisualStyleBackColor = true;
            this.butQuit.Click += new System.EventHandler(this.butQuit_Click);
            // 
            // labelInfoAboutPerson
            // 
            this.labelInfoAboutPerson.AutoSize = true;
            this.labelInfoAboutPerson.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelInfoAboutPerson.Location = new System.Drawing.Point(12, 66);
            this.labelInfoAboutPerson.Name = "labelInfoAboutPerson";
            this.labelInfoAboutPerson.Size = new System.Drawing.Size(29, 16);
            this.labelInfoAboutPerson.TabIndex = 1;
            this.labelInfoAboutPerson.Text = "FIO";
            // 
            // labelTitleChamp
            // 
            this.labelTitleChamp.AutoSize = true;
            this.labelTitleChamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTitleChamp.Location = new System.Drawing.Point(170, 13);
            this.labelTitleChamp.Name = "labelTitleChamp";
            this.labelTitleChamp.Size = new System.Drawing.Size(190, 16);
            this.labelTitleChamp.TabIndex = 2;
            this.labelTitleChamp.Text = "Наименование чемпионата";
            // 
            // labelTitleComp
            // 
            this.labelTitleComp.AutoSize = true;
            this.labelTitleComp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTitleComp.Location = new System.Drawing.Point(170, 38);
            this.labelTitleComp.Name = "labelTitleComp";
            this.labelTitleComp.Size = new System.Drawing.Size(95, 16);
            this.labelTitleComp.TabIndex = 3;
            this.labelTitleComp.Text = "Компетенция";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelUch);
            this.panel1.Controls.Add(this.panelExpert);
            this.panel1.Location = new System.Drawing.Point(173, 84);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(629, 366);
            this.panel1.TabIndex = 4;
            // 
            // panelExpert
            // 
            this.panelExpert.Controls.Add(this.butSaveExpert);
            this.panelExpert.Controls.Add(this.butDelExpert);
            this.panelExpert.Controls.Add(this.butEditExpert);
            this.panelExpert.Controls.Add(this.butAddExpert);
            this.panelExpert.Controls.Add(this.butBlockExpert);
            this.panelExpert.Controls.Add(this.dataGVExpert);
            this.panelExpert.Location = new System.Drawing.Point(3, 3);
            this.panelExpert.Name = "panelExpert";
            this.panelExpert.Size = new System.Drawing.Size(609, 341);
            this.panelExpert.TabIndex = 1;
            // 
            // panelUch
            // 
            this.panelUch.Controls.Add(this.butDelUch);
            this.panelUch.Controls.Add(this.butEditUch);
            this.panelUch.Controls.Add(this.butAddUch);
            this.panelUch.Controls.Add(this.butSaveUch);
            this.panelUch.Controls.Add(this.dataGVUchastniki);
            this.panelUch.Location = new System.Drawing.Point(3, 3);
            this.panelUch.Name = "panelUch";
            this.panelUch.Size = new System.Drawing.Size(620, 341);
            this.panelUch.TabIndex = 0;
            // 
            // butDelUch
            // 
            this.butDelUch.Location = new System.Drawing.Point(515, 296);
            this.butDelUch.Name = "butDelUch";
            this.butDelUch.Size = new System.Drawing.Size(75, 23);
            this.butDelUch.TabIndex = 4;
            this.butDelUch.Text = "Удалить";
            this.butDelUch.UseVisualStyleBackColor = true;
            // 
            // butEditUch
            // 
            this.butEditUch.Location = new System.Drawing.Point(109, 296);
            this.butEditUch.Name = "butEditUch";
            this.butEditUch.Size = new System.Drawing.Size(75, 23);
            this.butEditUch.TabIndex = 3;
            this.butEditUch.Text = "Изменить";
            this.butEditUch.UseVisualStyleBackColor = true;
            // 
            // butAddUch
            // 
            this.butAddUch.Location = new System.Drawing.Point(24, 296);
            this.butAddUch.Name = "butAddUch";
            this.butAddUch.Size = new System.Drawing.Size(75, 23);
            this.butAddUch.TabIndex = 2;
            this.butAddUch.Text = "Добавить";
            this.butAddUch.UseVisualStyleBackColor = true;
            // 
            // butSaveUch
            // 
            this.butSaveUch.Location = new System.Drawing.Point(442, 4);
            this.butSaveUch.Name = "butSaveUch";
            this.butSaveUch.Size = new System.Drawing.Size(134, 34);
            this.butSaveUch.TabIndex = 1;
            this.butSaveUch.Text = "Зафиксировать список";
            this.butSaveUch.UseVisualStyleBackColor = true;
            // 
            // dataGVUchastniki
            // 
            this.dataGVUchastniki.AllowUserToAddRows = false;
            this.dataGVUchastniki.AllowUserToDeleteRows = false;
            this.dataGVUchastniki.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGVUchastniki.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGVUchastniki.Location = new System.Drawing.Point(4, 44);
            this.dataGVUchastniki.Name = "dataGVUchastniki";
            this.dataGVUchastniki.Size = new System.Drawing.Size(586, 246);
            this.dataGVUchastniki.TabIndex = 0;
            // 
            // butSaveExpert
            // 
            this.butSaveExpert.Location = new System.Drawing.Point(287, 315);
            this.butSaveExpert.Name = "butSaveExpert";
            this.butSaveExpert.Size = new System.Drawing.Size(75, 23);
            this.butSaveExpert.TabIndex = 5;
            this.butSaveExpert.Text = "Сохранить";
            this.butSaveExpert.UseVisualStyleBackColor = true;
            // 
            // butDelExpert
            // 
            this.butDelExpert.Location = new System.Drawing.Point(515, 296);
            this.butDelExpert.Name = "butDelExpert";
            this.butDelExpert.Size = new System.Drawing.Size(75, 23);
            this.butDelExpert.TabIndex = 4;
            this.butDelExpert.Text = "Удалить";
            this.butDelExpert.UseVisualStyleBackColor = true;
            // 
            // butEditExpert
            // 
            this.butEditExpert.Location = new System.Drawing.Point(109, 296);
            this.butEditExpert.Name = "butEditExpert";
            this.butEditExpert.Size = new System.Drawing.Size(75, 23);
            this.butEditExpert.TabIndex = 3;
            this.butEditExpert.Text = "Изменить";
            this.butEditExpert.UseVisualStyleBackColor = true;
            // 
            // butAddExpert
            // 
            this.butAddExpert.Location = new System.Drawing.Point(24, 296);
            this.butAddExpert.Name = "butAddExpert";
            this.butAddExpert.Size = new System.Drawing.Size(75, 23);
            this.butAddExpert.TabIndex = 2;
            this.butAddExpert.Text = "Добавить";
            this.butAddExpert.UseVisualStyleBackColor = true;
            // 
            // butBlockExpert
            // 
            this.butBlockExpert.Location = new System.Drawing.Point(442, 4);
            this.butBlockExpert.Name = "butBlockExpert";
            this.butBlockExpert.Size = new System.Drawing.Size(134, 34);
            this.butBlockExpert.TabIndex = 1;
            this.butBlockExpert.Text = "Зафиксировать список";
            this.butBlockExpert.UseVisualStyleBackColor = true;
            // 
            // dataGVExpert
            // 
            this.dataGVExpert.AllowUserToAddRows = false;
            this.dataGVExpert.AllowUserToDeleteRows = false;
            this.dataGVExpert.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGVExpert.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGVExpert.Location = new System.Drawing.Point(4, 44);
            this.dataGVExpert.Name = "dataGVExpert";
            this.dataGVExpert.Size = new System.Drawing.Size(586, 246);
            this.dataGVExpert.TabIndex = 0;
            // 
            // butListUchastnic
            // 
            this.butListUchastnic.Location = new System.Drawing.Point(15, 168);
            this.butListUchastnic.Name = "butListUchastnic";
            this.butListUchastnic.Size = new System.Drawing.Size(143, 26);
            this.butListUchastnic.TabIndex = 5;
            this.butListUchastnic.Text = "Список участников";
            this.butListUchastnic.UseVisualStyleBackColor = true;
            this.butListUchastnic.Click += new System.EventHandler(this.butListUchastnic_Click);
            // 
            // butListExpert
            // 
            this.butListExpert.Location = new System.Drawing.Point(15, 226);
            this.butListExpert.Name = "butListExpert";
            this.butListExpert.Size = new System.Drawing.Size(143, 26);
            this.butListExpert.TabIndex = 6;
            this.butListExpert.Text = "Список экспертов";
            this.butListExpert.UseVisualStyleBackColor = true;
            this.butListExpert.Click += new System.EventHandler(this.butListExpert_Click);
            // 
            // butProtocol
            // 
            this.butProtocol.Location = new System.Drawing.Point(15, 286);
            this.butProtocol.Name = "butProtocol";
            this.butProtocol.Size = new System.Drawing.Size(143, 26);
            this.butProtocol.TabIndex = 7;
            this.butProtocol.Text = "Протоколы";
            this.butProtocol.UseVisualStyleBackColor = true;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackgroundImage = global::WorldSkillsRussia.Properties.Resources.wsrlogo_Red;
            this.pictureBoxLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxLogo.Location = new System.Drawing.Point(808, 84);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(125, 110);
            this.pictureBoxLogo.TabIndex = 8;
            this.pictureBoxLogo.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(357, 453);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Код доступа на сегодня:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(529, 453);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "JuRDBee";
            // 
            // FormExpert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 479);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.butProtocol);
            this.Controls.Add(this.butListExpert);
            this.Controls.Add(this.butListUchastnic);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelTitleComp);
            this.Controls.Add(this.labelTitleChamp);
            this.Controls.Add(this.labelInfoAboutPerson);
            this.Controls.Add(this.butQuit);
            this.Name = "FormExpert";
            this.Text = "Эксперт";
            this.panel1.ResumeLayout(false);
            this.panelExpert.ResumeLayout(false);
            this.panelUch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGVUchastniki)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGVExpert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butQuit;
        private System.Windows.Forms.Label labelInfoAboutPerson;
        private System.Windows.Forms.Label labelTitleChamp;
        private System.Windows.Forms.Label labelTitleComp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button butListUchastnic;
        private System.Windows.Forms.Button butListExpert;
        private System.Windows.Forms.Button butProtocol;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Panel panelUch;
        private System.Windows.Forms.DataGridView dataGVUchastniki;
        private System.Windows.Forms.Button butSaveUch;
        private System.Windows.Forms.Button butEditUch;
        private System.Windows.Forms.Button butAddUch;
        private System.Windows.Forms.Button butDelUch;
        private System.Windows.Forms.Panel panelExpert;
        private System.Windows.Forms.Button butSaveExpert;
        private System.Windows.Forms.Button butDelExpert;
        private System.Windows.Forms.Button butEditExpert;
        private System.Windows.Forms.Button butAddExpert;
        private System.Windows.Forms.Button butBlockExpert;
        private System.Windows.Forms.DataGridView dataGVExpert;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}