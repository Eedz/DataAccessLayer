using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using ITCLib;

namespace DataAccessLayerTests
{
    [TestClass]
    public class AUditTests
    {
  
        [TestMethod]
        public void GetQuestionID_SurveyVarName_Found()
        {
            var qid = DBAction.GetDeletedQID("4CV4", "EB004");

            Assert.IsTrue(qid == "155029");
        }

        [TestMethod]
        public void GetQuestionID_SurveyVarName_NotFound()
        {
            var qid = DBAction.GetDeletedQID("4CV4", "FR309v");

            Assert.IsTrue(qid=="0");
        }

        
    }
}
