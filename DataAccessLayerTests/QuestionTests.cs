using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ITCLib;

namespace DataAccessLayerTests
{
    /// <summary>
    /// Summary description for QuestionTests
    /// </summary>
    [TestClass]
    public class QuestionTests
    {
        public QuestionTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetSurveyQuestion_ID()
        {
            var results = DBAction.GetSurveyQuestion(1000);

            Assert.IsTrue(results != null);
        }

        [TestMethod]
        public void GetCompleteSurvey_SID()
        {
            var results = DBAction.GetCompleteSurvey(new Survey() { SID = 899 });

            Assert.IsTrue(results != null);
        }

        [TestMethod]
        public void GetQuestions_VarName()
        {
            var results = DBAction.GetVarNameQuestions("FR309v");

            Assert.IsTrue(results != null);
        }

        [TestMethod]
        public void GetQuestionUsage_VarName()
        {
            VariableName varname = new VariableName("FR309v");
            var results = DBAction.GetVarNameQuestions(varname);

            Assert.IsTrue(results != null);
        }

        [TestMethod]
        public void GetQuestions_RefVarName()
        {
            var results = DBAction.GetRefVarNameQuestions("FR309v");

            Assert.IsTrue(results != null);
        }

        [TestMethod]
        public void GetQuestions_RefVarName_Pattern()
        {
            var results = DBAction.GetRefVarNameQuestionsGlob("FR309v", "4CV%");

            Assert.IsTrue(results != null);
        }

        [TestMethod]
        public void GetQuestions_Corrected_Survey()
        {
            Survey s = new Survey() { SID = 529, SurveyCode = "UY4-CS" };
            var results = DBAction.GetCorrectedWordings(s);

            Assert.IsTrue(results != null);
        }

        [TestMethod]
        public void GetQuestionID_Survey_VarName_Exists()
        {
            var results = DBAction.GetQuestionID("4CV4", "FR309v");

            Assert.IsTrue(results != 0);
        }

        [TestMethod]
        public void GetQuestionID_Survey_VarName_NotExists()
        {
            var results = DBAction.GetQuestionID("4CV4", "00");

            Assert.IsTrue(results == 0);
        }

        [TestMethod]
        public void GetQuestionIDRef_Survey_VarName_Exists()
        {
            var results = DBAction.GetQuestionIDRef("4CV4", "FR309v");

            Assert.IsTrue(results != 0);
        }

        [TestMethod]
        public void GetQuestionIDRef_Survey_VarName_NotExists()
        {
            var results = DBAction.GetQuestionIDRef("4CV4", "00");

            Assert.IsTrue(results == 0);
        }

        [TestMethod]
        public void GetDeletedQuestions_Survey()
        {
            List<DeletedQuestion> results = DBAction.GetDeletedQuestions("MYS1");

            Assert.IsTrue(results !=null && results.Count> 0 && results.Any(x=>x.DeleteNotes.Count>0));
        }

        [TestMethod]
        public void VarNameIsUsed_Exists()
        {
            bool result = DBAction.VarNameIsUsed("FR309v");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void VarNameIsUsed_NotExists()
        {
            bool result = DBAction.VarNameIsUsed("00");

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("RTF")]
        public void InterpretBulletTag()
        {
            SurveyQuestion question = DBAction.GetSurveyQuestion(166766);

            string targetRTF = (@"<strong>Ask if:</strong><br><strong>[bullet] [Has tried to quit since LSD or in last 24M] AND </strong>has used e-cigs<br><strong>[bullet] Recent cigarette quitter AND </strong>has used e-cigs");
            //string actualRTF = (question.FilterDescriptionRTF);
            //Assert.AreEqual(targetRTF, actualRTF);
        }
    }
}
