using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ITCLib;

namespace DataAccessLayerTests
{
    [TestClass]
    public class TranslationTests
    {
        [TestMethod]
        public void TranslationsSurveyLang()
        {
            var translations = DBAction.GetSurveyTranslation("KRA3", "Korean");
            Assert.IsNotNull(translations);
            
            Assert.IsTrue(translations.Count == 815);
        }

        
        
    }
}
