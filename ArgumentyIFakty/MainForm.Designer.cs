namespace ArgumentyIFakty
{
    partial class MainForm
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
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnCreatePDF = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(67, 30);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 23);
            this.btnDownload.TabIndex = 0;
            this.btnDownload.Text = "Download articles";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnCreatePDF
            // 
            this.btnCreatePDF.Location = new System.Drawing.Point(192, 30);
            this.btnCreatePDF.Name = "btnCreatePDF";
            this.btnCreatePDF.Size = new System.Drawing.Size(103, 23);
            this.btnCreatePDF.TabIndex = 1;
            this.btnCreatePDF.Text = "Create PDF files";
            this.btnCreatePDF.UseVisualStyleBackColor = true;
            this.btnCreatePDF.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(380, 99);
            this.Controls.Add(this.btnCreatePDF);
            this.Controls.Add(this.btnDownload);
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load_1);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnCreatePDF;
    }
}