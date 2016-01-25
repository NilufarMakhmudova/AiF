using ArgumentyIFakty.Core;
using ArgumentyIFakty.Core.Services;
using ArgumentyIFakty.DataAccess;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArgumentyIFakty
{
    public partial class MainForm : Form
    {
        static IArticleService _articleService;
       


        static IArticleService articleService
        {
            get
            {
                if (_articleService == null)
                    _articleService = new ArticleEFService();
                return _articleService;
            }
        }
        
        public MainForm()
        {
            InitializeComponent();
        }
      
        private void MainForm_Load_1(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dBDataSet.Article' table. You can move, or remove it, as needed.
           

        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                LogServices.WriteProgressLog("Download button clicked");
                RequestServices.GetNewspapers(30500);
                LogServices.WriteProgressLog("All articles fetched");
                MessageBox.Show("Finished downloading");
            }
            catch (Exception)
            {

                LogServices.WriteErrorLog("Some error occurred at " + DateTime.Now.ToString());
                MessageBox.Show("Some error occurred at " + DateTime.Now.ToString());
            }
            //foreach (Article a in articles)
            //{
            //    LogServices.WriteProgressLog("Trying to insert article to database");
            //    articleService.Insert(a);
            //    LogServices.WriteProgressLog("Successfully inserted to database");
            //}
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PDFServices.CreatePDFDocuments();
        }
    }
}
