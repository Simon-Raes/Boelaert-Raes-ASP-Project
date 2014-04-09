using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.BLL
{
    class SearchService
    {

        public Dictionary<String, List<DvdInfo>> search(String searchText)
        {
            Dictionary<String, List<DvdInfo>> results = new Dictionary<String, List<DvdInfo>>();

            results.Add("Title", searchByTitle(searchText));

            results.Add("Author", searchByAuthor(searchText));
            results.Add("Barcode", searchByBarcode(searchText));
            results.Add("Category", searchByCategory(searchText));

            return results;
        }

        //todo
        private List<DvdInfo> searchByTitle(String searchText)
        {
            List<DvdInfo> result = new List<DvdInfo>();

            return result;
        }

        //todo
        private List<DvdInfo> searchByAuthor(String searchText)
        {
            List<DvdInfo> result = new List<DvdInfo>();

            return result;
        }

        //todo
        private List<DvdInfo> searchByBarcode(String searchText)
        {
            List<DvdInfo> result = new List<DvdInfo>();

            return result;
        }

        //todo
        private List<DvdInfo> searchByCategory(String searchText)
        {
            List<DvdInfo> result = new List<DvdInfo>();

            return result;
        }


    }
}
