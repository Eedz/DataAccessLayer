using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ITCLib;

namespace DataAccessLayerTests
{
    [TestClass]
    public class OtherTests
    {
        [TestMethod]
        public void BackupComments()
        {
            DBAction.BackupComments(10);


        }
    }
}
