using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ITCLib
{
    
    public class ObjectCache<T>
    {
        private static int RowsPerPage;

        // Represents one page of data.
        public struct ObjectDataPage<T>
        {
            public List<T> list;
            private int lowestIndexValue;
            private int highestIndexValue;

            public ObjectDataPage(List<T> list, int rowIndex)
            {
                this.list = list;
                lowestIndexValue = MapToLowerBoundary(rowIndex);
                highestIndexValue = MapToUpperBoundary(rowIndex);
                System.Diagnostics.Debug.Assert(lowestIndexValue >= 0);
                System.Diagnostics.Debug.Assert(highestIndexValue >= 0);
            }

            public int LowestIndex
            {
                get
                {
                    return lowestIndexValue;
                }
            }

            public int HighestIndex
            {
                get
                {
                    return highestIndexValue;
                }
            }

            public static int MapToLowerBoundary(int rowIndex)
            {
                // Return the lowest index of a page containing the given index.
                return (rowIndex / RowsPerPage) * RowsPerPage;
            }

            private static int MapToUpperBoundary(int rowIndex)
            {
                // Return the highest index of a page containing the given index.
                return MapToLowerBoundary(rowIndex) + RowsPerPage - 1;
            }
        }

        private ObjectDataPage<T>[] cachePages;
        private IObjectDataPageRetriever<T> dataSupply;

        public ObjectCache(IObjectDataPageRetriever<T> dataSupplier, int rowsPerPage)
        {
            dataSupply = dataSupplier;
            RowsPerPage = rowsPerPage;
            LoadFirstTwoPages();
        }

        public T RetrieveRow(int rowIndex)
        {
            T row = default(T);
            if (IfPageCached_ThenSetElement(rowIndex, ref row))
            {
                return row;
            }
            else
            {
                return RetrieveData_CacheIt_ThenReturnElement(rowIndex);
            }
        }

        // Sets the value of the element parameter if the value is in the cache.
        private bool IfPageCached_ThenSetElement(int rowIndex, ref T element)
        {
            if (IsRowCachedInPage(0, rowIndex))
            {
                element = cachePages[0].list[rowIndex % RowsPerPage];
                return true;
            }
            else if (IsRowCachedInPage(1, rowIndex))
            {
                element = cachePages[1].list[rowIndex % RowsPerPage];
                return true;
            }

            return false;
        }

        private void LoadFirstTwoPages()
        {
            cachePages = new ObjectDataPage<T>[]{
                new ObjectDataPage<T>(dataSupply.SupplyPageOfData(ObjectDataPage<T>.MapToLowerBoundary(0), RowsPerPage), 0),
                new ObjectDataPage<T>(dataSupply.SupplyPageOfData(ObjectDataPage<T>.MapToLowerBoundary(RowsPerPage), RowsPerPage), RowsPerPage)
            };
        }

        private T RetrieveData_CacheIt_ThenReturnElement(int rowIndex)
        {
            // Retrieve a page worth of data containing the requested value.
            List<T> table = dataSupply.SupplyPageOfData(ObjectDataPage<T>.MapToLowerBoundary(rowIndex), RowsPerPage);

            // Replace the cached page furthest from the requested cell
            // with a new page containing the newly retrieved data.
            cachePages[GetIndexToUnusedPage(rowIndex)] = new ObjectDataPage<T>(table, rowIndex);

            return RetrieveRow(rowIndex);
        }

        // Returns the index of the cached page most distant from the given index
        // and therefore least likely to be reused.
        private int GetIndexToUnusedPage(int rowIndex)
        {
            if (rowIndex > cachePages[0].HighestIndex && rowIndex > cachePages[1].HighestIndex)
            {
                int offsetFromPage0 = rowIndex - cachePages[0].HighestIndex;
                int offsetFromPage1 = rowIndex - cachePages[1].HighestIndex;
                if (offsetFromPage0 < offsetFromPage1)
                {
                    return 1;
                }
                return 0;
            }
            else
            {
                int offsetFromPage0 = cachePages[0].LowestIndex - rowIndex;
                int offsetFromPage1 = cachePages[1].LowestIndex - rowIndex;
                if (offsetFromPage0 < offsetFromPage1)
                {
                    return 1;
                }
                return 0;
            }
        }

        // Returns a value indicating whether the given row index is contained
        // in the given DataPage.
        private bool IsRowCachedInPage(int pageNumber, int rowIndex)
        {
            return rowIndex <= cachePages[pageNumber].HighestIndex && rowIndex >= cachePages[pageNumber].LowestIndex;
        }
    }
}
