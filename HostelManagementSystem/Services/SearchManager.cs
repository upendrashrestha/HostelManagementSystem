using HostelManagementSystem.Data;
using HostelManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HostelManagementSystem.Services
{
    public class SearchManager
    {
        private HMSEntities _hmsDB = null;
        public SearchManager()
        {
            _hmsDB = new HMSEntities();
        }

        public List<SearchStudentResult> SearchStudent(SearchCriteria searchCriteria) {
          
            var searchResults = _hmsDB.SearchStudentFunction(searchCriteria.FirstName, searchCriteria.LastName, searchCriteria.Phone, searchCriteria.Email, searchCriteria.RoomNo).ToList();
            searchResults = searchResults.Where(x => x.Active == (searchCriteria.Inactive == true ? "N" : "Y")).ToList();
            if (searchResults.Count > 0) {
                return searchResults;
            }
            return null;
        }
    }
}