using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArgumentyIFakty.Core.Services;
using ArgumentyIFakty.Core;
namespace AiFUnitTestProject
{
    [TestClass]
    public class RequestServicesTest
    {
        [TestMethod]
        public void getArticleAuthor()
        {
            var expected = " Дмитрий Романов";
            var htmlText = RequestServices.GetWebText("http://www.aif.ru/realty/utilities/esli_zhilcy_nastaivayut_v_stolice_zarabotali_komissii_po_kapremontu");
            var actual = RequestServices.getArticleAuthor(htmlText);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void getArticleAuthor1()
        {
            var expected = "Юлия Шигарева";
            var htmlText = RequestServices.GetWebText("http://www.aif.ru/culture/person/2529");
            var actual = RequestServices.getArticleAuthor(htmlText);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void getArticleTitle()
        {
            var expected = "Без заголовка";
            var htmlText = RequestServices.GetWebText("http://www.aif.ru/society/2390");
            var actual = RequestServices.getArticleTitle(htmlText);
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void getArticleDate()
        {
            var expected = DateTime.ParseExact("00:00 14/12/1993", "HH:mm dd/MM/yyyy", null);
            var htmlText = RequestServices.GetWebText("http://www.aif.ru/archive/1656036");
            var actual = RequestServices.getArticleDate(htmlText);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void getArticleDate1()
        {
            var expected = DateTime.ParseExact("00:00 19/03/2008", "HH:mm dd/MM/yyyy", null);
            var htmlText = RequestServices.GetWebText("http://www.aif.ru/culture/person/2529");
            var actual = RequestServices.getArticleDate(htmlText);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void getArticleContent()
        {
            var expected = "";
            var htmlText = RequestServices.GetWebText("http://www.aif.ru/society/healthcare/bolet_nelzya_otpravlyat_skorye_k_pacientam_skoro_budet_prosto_ne_na_chto");
            var actual = RequestServices.getArticleContent(htmlText);
            Assert.AreEqual(expected, actual);
        }



        [TestMethod]
        public void getEditionName()
        {
            var expected = @"Еженедельник ""Аргументы и Факты"" № 12 19/03/2008";
            var htmlText = RequestServices.GetWebText("http://www.aif.ru/culture/person/2529");
            var actual = RequestServices.getEditionName(htmlText);
            Assert.AreEqual(expected, actual);
        }

        
        
   
    }
}
