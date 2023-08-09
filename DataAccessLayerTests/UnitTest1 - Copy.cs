using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using ITCLib;

namespace DataAccessLayerTests
{
    [TestClass]
    public class WordingTests
    {
  
        [TestMethod]
        public void GetSurveyPreP()
        {
            var wordings = DBAction.GetSurveyPreP("FR309v", "4CV4");

            Assert.IsTrue(wordings.Count > 0);
        }

        [TestMethod]
        public void GetSurveyPreI()
        {
            var wordings = DBAction.GetSurveyPreI("EA845", "4CV4");

            Assert.IsTrue(wordings.Count > 0);
        }

        [TestMethod]
        public void GetSurveyPreA()
        {
            var wordings = DBAction.GetSurveyPreA("FR309v", "4CV4");

            Assert.IsTrue(wordings.Count > 0);
        }

        [TestMethod]
        public void GetSurveyLitQ()
        {
            var wordings = DBAction.GetSurveyLitQ("FR309v", "4CV4");

            Assert.IsTrue(wordings.Count > 0);
        }

        [TestMethod]
        public void GetSurveyPstI()
        {
            var wordings = DBAction.GetSurveyPstI("OwnerID", "4CV4");

            Assert.IsTrue(wordings.Count > 0);
        }

        [TestMethod]
        public void GetSurveyPstP()
        {
            var wordings = DBAction.GetSurveyPstP("OwnerID", "4CV4");

            Assert.IsTrue(wordings.Count > 0);
        }
    }
}
