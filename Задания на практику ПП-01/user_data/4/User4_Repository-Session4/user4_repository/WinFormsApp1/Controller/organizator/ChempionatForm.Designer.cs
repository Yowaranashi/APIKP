namespace WinFormsApp1.Controller.organizator
{
    partial class ChempionatForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChempionatForm));
            wellcome = new Label();
            label1 = new Label();
            chempionatOptions = new Button();
            expertUprav = new Button();
            Protokol = new Button();
            chempionat = new Button();
            groupBox1 = new GroupBox();
            exit = new Button();
            analitik = new Button();
            pictureBox1 = new PictureBox();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // wellcome
            // 
            wellcome.AutoSize = true;
            wellcome.Location = new Point(12, 68);
            wellcome.Name = "wellcome";
            wellcome.Size = new Size(60, 15);
            wellcome.TabIndex = 21;
            wellcome.Text = "Wellcome";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 47);
            label1.Name = "label1";
            label1.Size = new Size(219, 21);
            label1.TabIndex = 20;
            label1.Text = "ТЕХНИЧЕСКАЯ ДИРЕКЦИЯ";
            // 
            // chempionatOptions
            // 
            chempionatOptions.Location = new Point(15, 161);
            chempionatOptions.Name = "chempionatOptions";
            chempionatOptions.Size = new Size(125, 43);
            chempionatOptions.TabIndex = 19;
            chempionatOptions.Text = "Настройка чемпионата";
            chempionatOptions.UseVisualStyleBackColor = true;
            chempionatOptions.Click += chempionatOptions_Click;
            // 
            // expertUprav
            // 
            expertUprav.Location = new Point(15, 221);
            expertUprav.Name = "expertUprav";
            expertUprav.Size = new Size(125, 43);
            expertUprav.TabIndex = 18;
            expertUprav.Text = "Список главных экспертов";
            expertUprav.UseVisualStyleBackColor = true;
            // 
            // Protokol
            // 
            Protokol.Location = new Point(15, 280);
            Protokol.Name = "Protokol";
            Protokol.Size = new Size(125, 43);
            Protokol.TabIndex = 17;
            Protokol.Text = "Отчет по сданным ротоколам";
            Protokol.UseVisualStyleBackColor = true;
            // 
            // chempionat
            // 
            chempionat.Location = new Point(15, 102);
            chempionat.Name = "chempionat";
            chempionat.Size = new Size(125, 43);
            chempionat.TabIndex = 16;
            chempionat.Text = "Список участников";
            chempionat.UseVisualStyleBackColor = true;
            chempionat.Click += chempionat_Click;
            // 
            // groupBox1
            // 
            groupBox1.Location = new Point(146, 96);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(733, 352);
            groupBox1.TabIndex = 15;
            groupBox1.TabStop = false;
            // 
            // exit
            // 
            exit.Location = new Point(15, 12);
            exit.Name = "exit";
            exit.Size = new Size(77, 29);
            exit.TabIndex = 14;
            exit.Text = "Выход";
            exit.UseVisualStyleBackColor = true;
            exit.Click += exit_Click;
            // 
            // analitik
            // 
            analitik.Location = new Point(593, 40);
            analitik.Name = "analitik";
            analitik.Size = new Size(125, 43);
            analitik.TabIndex = 22;
            analitik.Text = "Аналитика";
            analitik.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            pictureBox1.ImageLocation = "C:\\Users\\bsn_user4\\source\\repos\\WinFormsApp1\\WinFormsApp1\\DS.png";
            pictureBox1.InitialImage = (Image)resources.GetObject("pictureBox1.InitialImage");
            pictureBox1.Location = new Point(746, 22);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(133, 68);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 23;
            pictureBox1.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(129, 12);
            label3.Name = "label3";
            label3.Size = new Size(167, 15);
            label3.TabIndex = 24;
            label3.Text = "Наименование чемпоионата";
            // 
            // ChempionatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(902, 462);
            Controls.Add(label3);
            Controls.Add(pictureBox1);
            Controls.Add(analitik);
            Controls.Add(wellcome);
            Controls.Add(label1);
            Controls.Add(chempionatOptions);
            Controls.Add(expertUprav);
            Controls.Add(Protokol);
            Controls.Add(chempionat);
            Controls.Add(groupBox1);
            Controls.Add(exit);
            Name = "ChempionatForm";
            Text = "ChempionatForm";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label wellcome;
        private Label label1;
        private Button chempionatOptions;
        private Button expertUprav;
        private Button Protokol;
        private Button chempionat;
        private GroupBox groupBox1;
        private Button exit;
        private Button analitik;
        private PictureBox pictureBox1;
        private Label label3;
    }
}