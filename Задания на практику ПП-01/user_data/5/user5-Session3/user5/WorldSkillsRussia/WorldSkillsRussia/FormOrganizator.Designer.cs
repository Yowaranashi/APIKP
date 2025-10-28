namespace WorldSkillsRussia
{
    partial class FormOrganizator
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
            this.butChampionate = new System.Windows.Forms.Button();
            this.butOptionChampionate = new System.Windows.Forms.Button();
            this.butManageExperts = new System.Windows.Forms.Button();
            this.butProtocols = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelInfoAboutPerson = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelChampionate = new System.Windows.Forms.Panel();
            this.butAddCompetition = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUDSq = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUDUchastnikov = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUDExperts = new System.Windows.Forms.NumericUpDown();
            this.comboBMainExpeyt = new System.Windows.Forms.ComboBox();
            this.comboBCompetition = new System.Windows.Forms.ComboBox();
            this.tBNameChamp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTPChampEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTPChampStart = new System.Windows.Forms.DateTimePicker();
            this.labelDateChamp = new System.Windows.Forms.Label();
            this.butChangeChamp = new System.Windows.Forms.Button();
            this.butAddChamp = new System.Windows.Forms.Button();
            this.comboBTitleChamp = new System.Windows.Forms.ComboBox();
            this.pictureBLogo = new System.Windows.Forms.PictureBox();
            this.panelContent.SuspendLayout();
            this.panelChampionate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUDSq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUDUchastnikov)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUDExperts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // butQuit
            // 
            this.butQuit.Location = new System.Drawing.Point(13, 13);
            this.butQuit.Name = "butQuit";
            this.butQuit.Size = new System.Drawing.Size(75, 28);
            this.butQuit.TabIndex = 0;
            this.butQuit.Text = "Выход";
            this.butQuit.UseVisualStyleBackColor = true;
            this.butQuit.Click += new System.EventHandler(this.butQuit_Click);
            // 
            // butChampionate
            // 
            this.butChampionate.Location = new System.Drawing.Point(36, 98);
            this.butChampionate.Name = "butChampionate";
            this.butChampionate.Size = new System.Drawing.Size(107, 41);
            this.butChampionate.TabIndex = 1;
            this.butChampionate.Text = "Список участников";
            this.butChampionate.UseVisualStyleBackColor = true;
            this.butChampionate.Click += new System.EventHandler(this.butChampionate_Click);
            // 
            // butOptionChampionate
            // 
            this.butOptionChampionate.Location = new System.Drawing.Point(36, 145);
            this.butOptionChampionate.Name = "butOptionChampionate";
            this.butOptionChampionate.Size = new System.Drawing.Size(107, 41);
            this.butOptionChampionate.TabIndex = 2;
            this.butOptionChampionate.Text = "Настройка чемпионата";
            this.butOptionChampionate.UseVisualStyleBackColor = true;
            this.butOptionChampionate.Click += new System.EventHandler(this.butOptionChampionate_Click);
            // 
            // butManageExperts
            // 
            this.butManageExperts.Location = new System.Drawing.Point(35, 192);
            this.butManageExperts.Name = "butManageExperts";
            this.butManageExperts.Size = new System.Drawing.Size(107, 41);
            this.butManageExperts.TabIndex = 3;
            this.butManageExperts.Text = "Управление экспертами";
            this.butManageExperts.UseVisualStyleBackColor = true;
            // 
            // butProtocols
            // 
            this.butProtocols.Location = new System.Drawing.Point(36, 239);
            this.butProtocols.Name = "butProtocols";
            this.butProtocols.Size = new System.Drawing.Size(107, 41);
            this.butProtocols.TabIndex = 4;
            this.butProtocols.Text = "Протоколы";
            this.butProtocols.UseVisualStyleBackColor = true;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTitle.Location = new System.Drawing.Point(183, 16);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(314, 25);
            this.labelTitle.TabIndex = 6;
            this.labelTitle.Text = "ТЕХНИЧЕСКАЯ ДИРЕКЦИЯ";
            // 
            // labelInfoAboutPerson
            // 
            this.labelInfoAboutPerson.AutoSize = true;
            this.labelInfoAboutPerson.Location = new System.Drawing.Point(188, 45);
            this.labelInfoAboutPerson.Name = "labelInfoAboutPerson";
            this.labelInfoAboutPerson.Size = new System.Drawing.Size(181, 13);
            this.labelInfoAboutPerson.TabIndex = 7;
            this.labelInfoAboutPerson.Text = "Добрый день, ФИО организатора";
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.panelChampionate);
            this.panelContent.Location = new System.Drawing.Point(188, 78);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(600, 360);
            this.panelContent.TabIndex = 10;
            // 
            // panelChampionate
            // 
            this.panelChampionate.Controls.Add(this.butAddCompetition);
            this.panelChampionate.Controls.Add(this.label8);
            this.panelChampionate.Controls.Add(this.label7);
            this.panelChampionate.Controls.Add(this.label6);
            this.panelChampionate.Controls.Add(this.numericUDSq);
            this.panelChampionate.Controls.Add(this.label5);
            this.panelChampionate.Controls.Add(this.numericUDUchastnikov);
            this.panelChampionate.Controls.Add(this.label4);
            this.panelChampionate.Controls.Add(this.numericUDExperts);
            this.panelChampionate.Controls.Add(this.comboBMainExpeyt);
            this.panelChampionate.Controls.Add(this.comboBCompetition);
            this.panelChampionate.Controls.Add(this.tBNameChamp);
            this.panelChampionate.Controls.Add(this.label3);
            this.panelChampionate.Controls.Add(this.label2);
            this.panelChampionate.Controls.Add(this.label1);
            this.panelChampionate.Controls.Add(this.dateTPChampEnd);
            this.panelChampionate.Controls.Add(this.dateTPChampStart);
            this.panelChampionate.Controls.Add(this.labelDateChamp);
            this.panelChampionate.Controls.Add(this.butChangeChamp);
            this.panelChampionate.Controls.Add(this.butAddChamp);
            this.panelChampionate.Location = new System.Drawing.Point(3, 29);
            this.panelChampionate.Name = "panelChampionate";
            this.panelChampionate.Size = new System.Drawing.Size(594, 294);
            this.panelChampionate.TabIndex = 11;
            // 
            // butAddCompetition
            // 
            this.butAddCompetition.Location = new System.Drawing.Point(6, 200);
            this.butAddCompetition.Name = "butAddCompetition";
            this.butAddCompetition.Size = new System.Drawing.Size(29, 23);
            this.butAddCompetition.TabIndex = 28;
            this.butAddCompetition.Text = "+";
            this.butAddCompetition.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(467, 270);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(13, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(292, 270);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(169, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Из общей площади S, свободно";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(426, 183);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Площадь";
            // 
            // numericUDSq
            // 
            this.numericUDSq.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUDSq.Location = new System.Drawing.Point(486, 181);
            this.numericUDSq.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUDSq.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUDSq.Name = "numericUDSq";
            this.numericUDSq.Size = new System.Drawing.Size(68, 20);
            this.numericUDSq.TabIndex = 24;
            this.numericUDSq.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(205, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Количество участников";
            // 
            // numericUDUchastnikov
            // 
            this.numericUDUchastnikov.Location = new System.Drawing.Point(337, 181);
            this.numericUDUchastnikov.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUDUchastnikov.Name = "numericUDUchastnikov";
            this.numericUDUchastnikov.Size = new System.Drawing.Size(68, 20);
            this.numericUDUchastnikov.TabIndex = 22;
            this.numericUDUchastnikov.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 183);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Количество экспертов";
            // 
            // numericUDExperts
            // 
            this.numericUDExperts.Location = new System.Drawing.Point(131, 181);
            this.numericUDExperts.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUDExperts.Name = "numericUDExperts";
            this.numericUDExperts.Size = new System.Drawing.Size(68, 20);
            this.numericUDExperts.TabIndex = 20;
            this.numericUDExperts.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // comboBMainExpeyt
            // 
            this.comboBMainExpeyt.FormattingEnabled = true;
            this.comboBMainExpeyt.Location = new System.Drawing.Point(238, 154);
            this.comboBMainExpeyt.Name = "comboBMainExpeyt";
            this.comboBMainExpeyt.Size = new System.Drawing.Size(177, 21);
            this.comboBMainExpeyt.TabIndex = 19;
            this.comboBMainExpeyt.Text = "Главный эксперт";
            // 
            // comboBCompetition
            // 
            this.comboBCompetition.FormattingEnabled = true;
            this.comboBCompetition.Location = new System.Drawing.Point(3, 154);
            this.comboBCompetition.Name = "comboBCompetition";
            this.comboBCompetition.Size = new System.Drawing.Size(229, 21);
            this.comboBCompetition.TabIndex = 18;
            this.comboBCompetition.Text = "Компетенция";
            // 
            // tBNameChamp
            // 
            this.tBNameChamp.Location = new System.Drawing.Point(95, 128);
            this.tBNameChamp.Name = "tBNameChamp";
            this.tBNameChamp.Size = new System.Drawing.Size(320, 20);
            this.tBNameChamp.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Наименование ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Конец";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Начало";
            // 
            // dateTPChampEnd
            // 
            this.dateTPChampEnd.Location = new System.Drawing.Point(93, 95);
            this.dateTPChampEnd.Name = "dateTPChampEnd";
            this.dateTPChampEnd.Size = new System.Drawing.Size(139, 20);
            this.dateTPChampEnd.TabIndex = 13;
            // 
            // dateTPChampStart
            // 
            this.dateTPChampStart.Location = new System.Drawing.Point(93, 68);
            this.dateTPChampStart.Name = "dateTPChampStart";
            this.dateTPChampStart.Size = new System.Drawing.Size(139, 20);
            this.dateTPChampStart.TabIndex = 12;
            // 
            // labelDateChamp
            // 
            this.labelDateChamp.AutoSize = true;
            this.labelDateChamp.Location = new System.Drawing.Point(3, 47);
            this.labelDateChamp.Name = "labelDateChamp";
            this.labelDateChamp.Size = new System.Drawing.Size(98, 13);
            this.labelDateChamp.TabIndex = 11;
            this.labelDateChamp.Text = "Даты чемпионата";
            // 
            // butChangeChamp
            // 
            this.butChangeChamp.Location = new System.Drawing.Point(93, 3);
            this.butChangeChamp.Name = "butChangeChamp";
            this.butChangeChamp.Size = new System.Drawing.Size(84, 29);
            this.butChangeChamp.TabIndex = 10;
            this.butChangeChamp.Text = "Изменить";
            this.butChangeChamp.UseVisualStyleBackColor = true;
            // 
            // butAddChamp
            // 
            this.butAddChamp.Location = new System.Drawing.Point(3, 3);
            this.butAddChamp.Name = "butAddChamp";
            this.butAddChamp.Size = new System.Drawing.Size(84, 29);
            this.butAddChamp.TabIndex = 9;
            this.butAddChamp.Text = "Добавить";
            this.butAddChamp.UseVisualStyleBackColor = true;
            // 
            // comboBTitleChamp
            // 
            this.comboBTitleChamp.FormattingEnabled = true;
            this.comboBTitleChamp.Location = new System.Drawing.Point(12, 47);
            this.comboBTitleChamp.Name = "comboBTitleChamp";
            this.comboBTitleChamp.Size = new System.Drawing.Size(170, 21);
            this.comboBTitleChamp.TabIndex = 19;
            this.comboBTitleChamp.Text = "Чемпионат";
            this.comboBTitleChamp.SelectedIndexChanged += new System.EventHandler(this.comboBTitleChamp_SelectedIndexChanged);
            // 
            // pictureBLogo
            // 
            this.pictureBLogo.BackgroundImage = global::WorldSkillsRussia.Properties.Resources.wsrlogo_Red;
            this.pictureBLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBLogo.Location = new System.Drawing.Point(620, 2);
            this.pictureBLogo.Name = "pictureBLogo";
            this.pictureBLogo.Size = new System.Drawing.Size(168, 70);
            this.pictureBLogo.TabIndex = 20;
            this.pictureBLogo.TabStop = false;
            // 
            // FormOrganizator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBLogo);
            this.Controls.Add(this.comboBTitleChamp);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.labelInfoAboutPerson);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.butProtocols);
            this.Controls.Add(this.butManageExperts);
            this.Controls.Add(this.butOptionChampionate);
            this.Controls.Add(this.butChampionate);
            this.Controls.Add(this.butQuit);
            this.Name = "FormOrganizator";
            this.Text = "Организатор";
            this.Load += new System.EventHandler(this.FormOrganizator_Load);
            this.panelContent.ResumeLayout(false);
            this.panelChampionate.ResumeLayout(false);
            this.panelChampionate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUDSq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUDUchastnikov)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUDExperts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butQuit;
        private System.Windows.Forms.Button butChampionate;
        private System.Windows.Forms.Button butOptionChampionate;
        private System.Windows.Forms.Button butManageExperts;
        private System.Windows.Forms.Button butProtocols;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelInfoAboutPerson;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Button butChangeChamp;
        private System.Windows.Forms.Button butAddChamp;
        private System.Windows.Forms.Panel panelChampionate;
        private System.Windows.Forms.DateTimePicker dateTPChampStart;
        private System.Windows.Forms.Label labelDateChamp;
        private System.Windows.Forms.DateTimePicker dateTPChampEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBNameChamp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBMainExpeyt;
        private System.Windows.Forms.ComboBox comboBCompetition;
        private System.Windows.Forms.NumericUpDown numericUDExperts;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUDUchastnikov;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUDSq;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBTitleChamp;
        private System.Windows.Forms.Button butAddCompetition;
        private System.Windows.Forms.PictureBox pictureBLogo;
    }
}