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
  
        public TimeSpan VarNameListSpeedDapper()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var dapper = DBAction.GetAllVarNamesD();
            stopwatch.Stop();

            return stopwatch.Elapsed;

        }

        public TimeSpan VarNameListSpeedNotDapper()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var notDapper = DBAction.GetAllVarNames();
            stopwatch.Stop();

            return stopwatch.Elapsed;



        }

        [TestMethod]
        public void VarNameListSpeed()
        {
            
            
            var dapper = VarNameListSpeedDapper();

            
        }
    }
}
