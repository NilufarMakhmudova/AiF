using ArgumentyIFakty.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiFUnitTestProject
{
    [TestClass]
   public  class QATest
    {
        [TestMethod]
        public void getQATitle() {
            var expected = "Если кишечник газанул";
            var htmlText = RequestServices.GetWebText("http://www.aif.ru/health/life/esli_kishechnik_gazanul");
            var actual = QAServices.getQATitle(htmlText);
            Assert.AreEqual(expected, actual);       
        
        }

        [TestMethod]        
        public void getQAEditionName()
        {
            var expected = @"Еженедельник ""Аргументы и Факты"" № 51 19/12/2007";
            var htmlText = RequestServices.GetWebText("http://www.aif.ru/money/business/1500");
            var actual = QAServices.getQAEditionName(htmlText);
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]        
        public void getQAContent()
        {
            var expected = "Если кишечник газанул";
            var htmlText = RequestServices.GetWebText("http://www.aif.ru/health/life/esli_kishechnik_gazanul");
            var actual = QAServices.getQAContent(htmlText);
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]        
        public void getQADate()
        {
            var expected = DateTime.ParseExact("16:20 20/12/2007", "HH:mm dd/MM/yyyy", null);
            var htmlText = RequestServices.GetWebText("http://www.aif.ru/health/life/esli_kishechnik_gazanul");
            var actual = QAServices.getQADate(htmlText);
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]       

        public void getQAAuthor()
        {
            var expected = "Автор не известен";
            var htmlText = RequestServices.GetWebText("http://www.aif.ru/money/business/1500");
            var actual = QAServices.getQAAuthor(htmlText);
            Assert.AreEqual(expected, actual);

        }
    
    }
}
