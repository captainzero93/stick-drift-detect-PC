namespace StickDrift
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();

            // Initialize status label
            this.statusLabel = new System.Windows.Forms.Label();
            this.statusLabel.Location = new System.Drawing.Point(10, 10);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(580, 40);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "Connect a controller and press Start to begin drift check";

            // Initialize start button
            this.startButton = new System.Windows.Forms.Button();
            this.startButton.Location = new System.Drawing.Point(10, 60);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(100, 30);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start Check";
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);

            // Initialize visualizer panel
            this.visualizer = new System.Windows.Forms.Panel();
            this.visualizer.Location = new System.Drawing.Point(10, 100);
            this.visualizer.Name = "visualizer";
            this.visualizer.Size = new System.Drawing.Size(580, 290);
            this.visualizer.TabIndex = 2;
            this.visualizer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.visualizer.BackColor = System.Drawing.Color.White;
            this.visualizer.Paint += new System.Windows.Forms.PaintEventHandler(this.Visualizer_Paint);

            // Initialize update timer
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.updateTimer.Interval = 16;
            this.updateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);

            // Form settings
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.visualizer);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.statusLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Controller Drift Checker";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Panel visualizer;
        private System.Windows.Forms.Timer updateTimer;
    }
}