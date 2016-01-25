using ArgumentyIFakty.DataAccess;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentyIFakty.Core
{
    public static class PDFServices
    {
        static IArticlesWithMigraService _migraService;
        static IArticlesWithMigraService migraService
        {
            get
            {
                if (_migraService == null)
                    _migraService = new MigraArticleService();
                return _migraService;
            }
        }

        public static void CreatePDFDocument(ArticlesWithWordMigra article) {
             // Create a MigraDoc document
            Document document = PDFServices.CreateDocument(article);         
            //string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
            MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = document;       
            renderer.RenderDocument();
            var extension = ".pdf";
            // Save the document...
            string filename = article.DatePublished.ToString("yyyy-MM-dd")+" " + article.Author;
            var fileCreated = false;
            var FileNameCounter = 0;
           
            while (!fileCreated)
            {

                DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
                FileInfo[] PDFFiles = di.GetFiles(filename + extension);
                if (PDFFiles.Length == 0)
                {
                    renderer.PdfDocument.Save(filename+ extension);
                    fileCreated = true;
                }
                else
                {
                    FileNameCounter = FileNameCounter + 1;
                    filename = filename+'('+ FileNameCounter+')';
                }
            }
            //// ...and start a viewer.
            //Process.Start(filename);
        }
        
        public static Document CreateDocument(ArticlesWithWordMigra article)
        {
            // Create a new MigraDoc document
            Document document = new Document();
            document.Info.Title = article.Title;            
            document.Info.Author = "Nilufar Makhmudova";
            DefineStyles(document);         
            DefineContentSection(document, article);
            DefineParagraphs(document, article); 
            return document;
        }
        public static void DefineParagraphs(Document document, ArticlesWithWordMigra article)
        {
            Paragraph paragraphTitle = document.LastSection.AddParagraph(article.Title, "Heading1");
            Paragraph paragraph = document.LastSection.AddParagraph(article.Context, "Normal");
            paragraphTitle.AddBookmark("Paragraphs Title");
            paragraph.AddBookmark("Paragraphs");
        }

        /// <summary>
        /// Defines page setup, headers, and footers.
        /// </summary>
        static void DefineContentSection(Document document, ArticlesWithWordMigra article)
        {
            Section section = document.AddSection();
            section.PageSetup.OddAndEvenPagesHeaderFooter = false;
            section.PageSetup.StartingNumber = 1;

            HeaderFooter header = section.Headers.Primary;
            header.AddParagraph(article.EditionName +"\t"+ article.Url);
                    
            // Create a paragraph with centered page number. See definition of style "Footer".
            Paragraph paragraph = new Paragraph();
            paragraph.AddTab();
            paragraph.AddPageField();

            // Add paragraph to footer for odd pages.
            section.Footers.Primary.Add(paragraph);
            
        }
        /// <summary>
        /// Defines the styles used in the document.
        /// </summary>
        public static void DefineStyles(Document document)
        {
            // Get the predefined style Normal.
            Style style = document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Times New Roman";

            // Heading1 to Heading9 are predefined styles with an outline level. An outline level
            // other than OutlineLevel.BodyText automatically creates the outline (or bookmarks) 
            // in PDF.

            style = document.Styles["Heading1"];
            style.Font.Name = "Times New Roman";
            style.Font.Size = 14;
            style.Font.Bold = true;
            style.Font.Color = Colors.DarkBlue;
            style.ParagraphFormat.PageBreakBefore = true;
            style.ParagraphFormat.SpaceAfter = 6;

            style = document.Styles["Heading2"];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 6;          

            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

          
        }
             

        public static void CreatePDFDocuments()
        {
            var articles = migraService.GetAll();
            foreach (ArticlesWithWordMigra a in articles)
            {
                CreatePDFDocument(a);
            }
        }
    }
}