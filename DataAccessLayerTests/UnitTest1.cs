using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using ITCLib;

namespace DataAccessLayerTests
{
    [TestClass]
    public class VarNameTests
    {
  
        [TestMethod]
        public void RefVarNameList()
        {
            var refVarNames = DBAction.GetAllRefVarNames();

            Assert.IsTrue(refVarNames.Count > 0);
        }

        [TestMethod]
        public void VarNameList()
        {
            var refVarNames = DBAction.GetAllVarNames();

            Assert.IsTrue(refVarNames.Count > 0);
        }

        [TestMethod]
        public void CanonVars()
        {
            var refVarNames = DBAction.GetAllCanonVars();

            Assert.IsTrue(refVarNames.Count > 0);
        }

        [TestMethod]
        public void SingleVarName()
        {
            var refVarName = DBAction.GetVariable("BI104");

            Assert.IsTrue(refVarName != null);
        }

        [TestMethod]
        public void VarNamesByRef()
        {
            var refVarNames = DBAction.GetVarNamesByRef("BI104");

            Assert.IsTrue(refVarNames.Count > 0);
        }
    }
}
