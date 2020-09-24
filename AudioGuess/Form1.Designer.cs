namespace AudioGuess
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.folderPathLabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.a1 = new System.Windows.Forms.Button();
            this.play = new System.Windows.Forms.Button();
            this.a2 = new System.Windows.Forms.Button();
            this.a3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Выбрать папку";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.selectFilesButton_Click);
            // 
            // folderPathLabel
            // 
            this.folderPathLabel.AutoSize = true;
            this.folderPathLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.folderPathLabel.Location = new System.Drawing.Point(12, 69);
            this.folderPathLabel.Name = "folderPathLabel";
            this.folderPathLabel.Size = new System.Drawing.Size(0, 13);
            this.folderPathLabel.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 43);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(123, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Старт";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // a1
            // 
            this.a1.Location = new System.Drawing.Point(1, 334);
            this.a1.Name = "a1";
            this.a1.Size = new System.Drawing.Size(100, 80);
            this.a1.TabIndex = 3;
            this.a1.Text = "Вариант ответа";
            this.a1.UseVisualStyleBackColor = true;
            this.a1.Click += new System.EventHandler(this.OnChooseButtonPressed);
            // 
            // play
            // 
            this.play.Location = new System.Drawing.Point(95, 187);
            this.play.Name = "play";
            this.play.Size = new System.Drawing.Size(121, 44);
            this.play.TabIndex = 6;
            this.play.Text = "Воспроизвести";
            this.play.UseVisualStyleBackColor = true;
            this.play.Click += new System.EventHandler(this.play_Click);
            // 
            // a2
            // 
            this.a2.Location = new System.Drawing.Point(105, 334);
            this.a2.Name = "a2";
            this.a2.Size = new System.Drawing.Size(100, 80);
            this.a2.TabIndex = 7;
            this.a2.Text = "Вариант ответа";
            this.a2.UseVisualStyleBackColor = true;
            this.a2.Click += new System.EventHandler(this.OnChooseButtonPressed);
            // 
            // a3
            // 
            this.a3.Location = new System.Drawing.Point(211, 334);
            this.a3.Name = "a3";
            this.a3.Size = new System.Drawing.Size(100, 80);
            this.a3.TabIndex = 8;
            this.a3.Text = "Вариант ответа";
            this.a3.UseVisualStyleBackColor = true;
            this.a3.Click += new System.EventHandler(this.OnChooseButtonPressed);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 450);
            this.Controls.Add(this.a3);
            this.Controls.Add(this.a2);
            this.Controls.Add(this.play);
            this.Controls.Add(this.a1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.folderPathLabel);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClosedForm);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label folderPathLabel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button a1;
        private System.Windows.Forms.Button play;
        private System.Windows.Forms.Button a2;
        private System.Windows.Forms.Button a3;
    }
}

