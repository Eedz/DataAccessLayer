using ITCLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataAccessLayerTests
{
    /// <summary>
    /// Summary description for GetTests
    /// </summary>
    [TestClass]
    public class GetTests
    {
        public GetTests()
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
        public void GetCohorts()
        {
            var cohorts = DBAction.GetCohortInfo();

            Assert.IsTrue(cohorts != null && cohorts.Count > 0);
        }

        [TestMethod]
        public void GetGroups()
        {
            //var groups = DBAction.GetGroupInfo();

            //Assert.IsTrue(groups != null && groups.Count > 0);
        }

        [TestMethod]
        public void GetModes()
        {
            var modes = DBAction.GetModeInfo();

            Assert.IsTrue(modes != null && modes.Count > 0);
        }

        [TestMethod]
        public void GetScreenedProducts()
        {
            var products = DBAction.GetScreenProducts();

            Assert.IsTrue(products != null && products.Count > 0);
        }

        [TestMethod]
        public void GetUserStates()
        {
            var states = DBAction.GetUserStates();

            Assert.IsTrue(states != null && states.Count > 0);
        }

        [TestMethod]
        public void GetRegions()
        {
            var regions = DBAction.GetRegionInfo();

            Assert.IsTrue(regions != null && regions.Count > 0);
        }

        [TestMethod]
        public void GetStudies()
        {
            var studies = DBAction.GetStudyInfo();

            Assert.IsTrue(studies != null && studies.Count > 0);
        }

        [TestMethod]
        public void GetStudies_RegionID()
        {
            var studies = DBAction.GetStudyInfo(1);

            Assert.IsTrue(studies != null && studies.Count > 0);
        }

        [TestMethod]
        public void GetWaves()
        {
            var waves = DBAction.GetWaveInfo();

            Assert.IsTrue(waves != null && waves.Count > 0);
        }

        [TestMethod]
        public void GetWaves_StudyID()
        {
            var studies = DBAction.GetWaves(1);

            Assert.IsTrue(studies != null && studies.Count > 0);
        }

        [TestMethod]
        public void GetPrefixesNew()
        {
            var prefixes = DBAction.GetVariablePrefixes();

            Assert.IsTrue(prefixes != null && prefixes.Count > 0);
        }

        [TestMethod]
        public void GetSurveyProcessingRecords()
        {
            var records = DBAction.GetSurveyProcessingRecords();

            Assert.IsTrue(records != null && records.Count > 0);
        }

        [TestMethod]
        public void GetPraccingIssues()
        {
            var records = DBAction.GetPraccingIssues("4CV4");

            Assert.IsTrue(records != null && records.Count > 0);
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void GetCommentTypes_Survey()
        {
            Survey survey = new Survey("4CV4");
            var records = DBAction.GetCommentTypes(survey); 

            Assert.IsTrue(records != null && records.Count > 0);
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void GetDeletedComments_SurveyVarName()
        {
            var records = DBAction.GetDeletedComments("4CV4", "CV001");

            Assert.IsTrue(records != null && records.Count == 1);
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void DeletedCommentExists_True()
        {
            DeletedQuestion dq = new DeletedQuestion();
            dq.SurveyCode = "JP1";
            dq.VarName = "BR23353";
            
            bool exists = DBAction.DeletedCommentExists(dq, 54860);

            Assert.IsTrue(exists);
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void DeletedCommentExists_False()
        {
            DeletedQuestion dq = new DeletedQuestion();
            dq.SurveyCode = "JP1";
            dq.VarName = "BR23353";

            bool exists = DBAction.DeletedCommentExists(dq, 0);

            Assert.IsFalse(exists);
        }


        [TestMethod]
        [TestCategory("Comments")]
        public void QuestionCommentExists_True()
        {
            SurveyQuestion dq = new SurveyQuestion();
            dq.SurveyCode = "4C6-C";
            dq.VarName.VarName = "SM544";

            bool exists = DBAction.QuestionCommentExists(dq, 23121);

            Assert.IsTrue(exists);
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void QuestionCommentExists_False()
        {
            SurveyQuestion dq = new SurveyQuestion();
            dq.SurveyCode = "4C6-C";
            dq.VarName.VarName = "SM544";

            bool exists = DBAction.QuestionCommentExists(dq, 0);

            Assert.IsFalse(exists);
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void GetQuestionComments_CID()
        {
            var records = DBAction.GetQuesCommentsByCID(23121);

            Assert.IsTrue(records != null && records.Count == 1);
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void GetQuestionComments_Survey()
        {
            //Survey survey = new Survey() { SID = 930 };
            //var records = DBAction.GetQuesComments(survey);

            //Assert.IsTrue(records != null && records.Count == 196);
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void GetQuestionComments_QID()
        {
            //SurveyQuestion question = new SurveyQuestion() { ID = 155500 };
            //var records = DBAction.GetQuesComments(question);

            //Assert.IsTrue(records != null && records.Count == 1);
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void SurveyCommentExists_True()
        {
            Survey dq = new Survey();
            dq.SurveyCode = "4CV1";

            bool exists = DBAction.SurveyCommentExists(dq, 45798);

            Assert.IsTrue(exists);
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void SurveyCommentExists_False()
        {
            Survey dq = new Survey();
            dq.SurveyCode = "4CV1";

            bool exists = DBAction.SurveyCommentExists(dq, 0);

            Assert.IsFalse(exists);
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void GetSurveyComments_CID()
        {
            var records = DBAction.GetSurvCommentsByCID(45784);

            Assert.IsTrue(records != null && records.Count == 1);
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void GetSurveyComments_Survey()
        {
            Survey dq = new Survey();
            dq.SurveyCode = "4CV1";
            dq.SID = 748;

            var records = DBAction.GetSurvComments(dq);

            Assert.IsTrue(records != null && records.Count == 11);
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void WaveCommentExists_True()
        {
            StudyWave dq = new StudyWave("NLD", 2);

            bool exists = DBAction.WaveCommentExists(dq, 62327);

            Assert.IsTrue(exists);
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void WaveCommentExists_False()
        {
            StudyWave dq = new StudyWave("NLD2", 2);

            bool exists = DBAction.WaveCommentExists(dq, 0);

            Assert.IsFalse(exists);
        }
    }
}
