using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ITCLib
{
    public interface IObjectDataPageRetriever<T>
    {
        List<T> SupplyPageOfData(int lowerPageBoundary, int rowsPerPage);
    }

    public class ObjectDataRetriever<T> : IObjectDataPageRetriever<T>
    {
        private List<T> ObjectList;
       

        public ObjectDataRetriever(List<T> list)
        {
            ObjectList = list;
        }

        private int rowCountValue = -1;

        public int RowCount
        {
            get
            {
                // Return the existing value if it has already been determined.
                if (rowCountValue != -1)
                {
                    return rowCountValue;
                }

                // Retrieve the row count from the database.

                rowCountValue = ObjectList.Count;
                return rowCountValue;
            }
        }

        // Declare variables to be reused by the SupplyPageOfData method.

        public List<T> SupplyPageOfData(int lowerPageBoundary, int rowsPerPage)
        {
            // Retrieve the specified number of rows from the database, starting
            // with the row specified by the lowerPageBoundary parameter.
          
            List<T> list = new List<T>();
            list = ObjectList.Skip(lowerPageBoundary).Take(rowsPerPage).ToList();
            
            return list;
        }
    }
}
