using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchFileApplication;

namespace UnitTest
{
    [TestClass]
    public class RegexCreatorPatternTest
    {
        ExceptionWordsSystem eWS = new ExceptionWordsSystem();
        RegexCreatorPattern rcp = new RegexCreatorPattern();
        [TestMethod]
        public void TestGetUnsearchingPattern()
        {        
            eWS.AddToList("text1");
            eWS.AddToList("text2");
            string excpectedString = "^(?!.*(text1|text2)).*$";
            string actualResult = rcp.GetUnsearchingPattern();
            Assert.IsTrue(excpectedString.Equals(actualResult));
        }

        [TestMethod]
        public void TestGetSearchingPattern()
        {
            string excpectedString = "^(?=.*(text1)).*$";
            string actualResult = rcp.GetSearchingPattern("text1");
            Assert.IsTrue(excpectedString.Equals(actualResult));
        }
    }
}
