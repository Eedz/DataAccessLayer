using ITCLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataAccessLayerTests
{
    /// <summary>
    /// Summary description for InsertTests
    /// </summary>
    [TestClass]
    public class InsertTests
    {
        public InsertTests()
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
        public void InsertSurveyAsCopy()
        {
            string source = "4CV5";
            string destination = "4CV6";

            DBAction.CopySurvey(source, destination);
        }

        [TestMethod]
        public void InsertNote()
        {
            Note newNote = new Note();
            newNote.NoteText = "test new note";

            DBAction.InsertNote(newNote);

            Assert.IsTrue(newNote.ID > 0);
        }

        [TestMethod]
        public void InsertQuestion()
        {
            SurveyQuestion newQ = new SurveyQuestion("AA000", "001");
            newQ.SurveyCode = "4CV5";

            DBAction.InsertQuestion("4CV5", newQ);

            Assert.IsTrue(newQ.ID > 0);
        }

        [TestMethod]
        public void InsertVariable()
        {
            VariableName newV = new VariableName("AA000");
            newV.VarLabel = "Value";

            DBAction.InsertVariable(newV);

            Assert.IsTrue(DBAction.GetVariable("AA000")!=null);
        }

        [TestMethod]
        public void InsertDomainLabel()
        {
            DomainLabel label = new DomainLabel();
            label.LabelText = "dsfdf";

            DBAction.InsertDomainLabel(label);

            Assert.IsTrue(label.ID>0);
        }

        [TestMethod]
        public void InsertTopicLabel()
        {
            TopicLabel label = new TopicLabel();
            label.LabelText = "new topic test";

            DBAction.InsertTopicLabel(label);

            Assert.IsTrue(label.ID > 0);
        }

        [TestMethod]
        public void InsertContentLabel()
        {
            ContentLabel label = new ContentLabel();
            label.LabelText = "new content test";

            DBAction.InsertContentLabel(label);

            Assert.IsTrue(label.ID > 0);
        }

        [TestMethod]
        public void InsertProductLabel()
        {
            ProductLabel label = new ProductLabel();
            label.LabelText = "new product test";

            DBAction.InsertProductLabel(label);

            Assert.IsTrue(label.ID > 0);
        }

        [TestMethod]
        public void InsertKeyword()
        {
            Keyword label = new Keyword();
            label.LabelText = "new keyword test";

            DBAction.InsertKeyword(label);

            Assert.IsTrue(label.ID > 0);
        }

        [TestMethod]
        public void InsertRegion()
        {
            Region region = new Region();
            region.RegionName = "The Moon";
            region.TempVarPrefix = "MN";

            DBAction.InsertRegion(region);

            Assert.IsTrue(region.ID > 0);
        }

        [TestMethod]
        public void InsertStudy()
        {
            Study record = new Study();
            record.StudyName = "The Moon";
            record.CountryName = "Lunaria";
            record.ISO_Code = "MN";
            record.Cohort = 1;


            DBAction.InsertCountry(record);

            Assert.IsTrue(record.ID > 0);
        }

        [TestMethod]
        public void InsertWave()
        {
            StudyWave record = new StudyWave();
            record.StudyID = 246;
            record.Wave = 2;
            record.EnglishRouting = true;
            record.Countries = "Lunaria";


            DBAction.InsertStudyWave(record);

            Assert.IsTrue(record.ID > 0);
        }

        [TestMethod]
        public void InsertWaveStudyDoesntExist()
        {
            StudyWave record = new StudyWave();
            record.StudyID = -1;
            record.Wave = 2;
            record.EnglishRouting = true;
            record.Countries = "Lunaria";


            DBAction.InsertStudyWave(record);

            Assert.IsTrue(record.ID == 0);
        }

        [TestMethod]
        public void InsertWordingPreP()
        {
            Wording record = new Wording();
            //record.FieldName = "PreP";
            record.WordingText = "PreP Test";

            DBAction.InsertWording(record);

            Assert.IsTrue(record.WordID > 0);
        }

        [TestMethod]
        public void InsertResponseSetRO()
        {
            ResponseSet record = new ResponseSet();
           // record.FieldName = "RespOptions";
            record.RespList = "1   Test\r\n2    No";
            record.RespSetName = "TestNew";

            DBAction.InsertResponseSet(record);

            Assert.IsTrue(DBAction.GetResponseSets("RespOptions").Where(x=>x.RespSetName.Equals("TestNew")).FirstOrDefault() !=null );
        }

        [TestMethod]
        public void InsertDraft()
        {
           // SurveyDraftRecord record = new SurveyDraftRecord();
          // // record.SurvID = 791;
          //  record.DraftDate = DateTime.Today;
           // record.DraftTitle = "TestNew Draft";

           // DBAction.InsertSurveyDraft(record);

           // Assert.IsTrue(record.ID>0);
        }

        [TestMethod]
        public void InsertDraftInfo()
        {
            int draftID = 50;
            int extraFieldNum = 1;
            string extraFieldLabel = "Label";
            
            int newID = DBAction.InsertSurveyDraftExtraInfo(draftID, extraFieldNum, extraFieldLabel);

            Assert.IsTrue(newID == 0);
        }

        [TestMethod]
        public void InsertDraftQuestion()
        {
           // DraftQuestionRecord question = new DraftQuestionRecord();
          //  question.DraftID = 50;
          //  question.VarName = "TEST";

            //int newID = DBAction.InsertDraftQuestion(question);

            //Assert.IsTrue(newID == 0);
        }

        [TestMethod]
        public void InsertTranslation()
        {
            Translation question = new Translation();
            question.QID = 165875;
            question.TranslationText = "TEST";
            question.LanguageName = new Language() { ID = 5, LanguageName = "French" };

            DBAction.InsertTranslation(question);
            
            Assert.IsTrue(question.ID > 0);
        }

        [TestMethod]
        public void InsertLanguage()
        {
            Language language = new Language();
            language.LanguageName = "TestLanguage";
            language.RTL = false;
            language.Abbrev = "TL";
            language.PreferredFont = "Arial";
            language.NonLatin = false;
     

            DBAction.InsertLanguage(language);

            Assert.IsTrue(language.ID > 0);
        }

        [TestMethod]
        public void InsertSurveyLanguage()
        {
            SurveyLanguage language = new SurveyLanguage();
            language.SurvID = 1;
            language.SurvLanguage = new Language() { ID = 5 };
            
            DBAction.InsertSurveyLanguage(language);

            Assert.IsTrue(language.ID > 0);
        }
    }
}
