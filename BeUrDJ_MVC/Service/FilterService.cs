using BeUrDJ_MVC.Models;
using BeUrDJ_MVC.Models.ViewModels;
using BeUrDJ_MVC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeUrDJ_MVC.Service
{
    public class FilterService
    {
        private FilterRepository _filterRepository = new FilterRepository();

        public FilterService()
        {

        }
        public FiltersModel UpdateFilters(FiltersModel updateFilter)
        {
            _filterRepository.UpdateFilter(updateFilter);
            return GetFilter(updateFilter.sessionId);
        }
        public FiltersModel GetFilter(int? sessionId)
        {
            return _filterRepository.GetFilter(sessionId);
        }
        public SearchViewModel FilterSongs(Tracks tracks, string tokenId, int? sessionId)
        {
            SearchViewModel sessionSearch = new SearchViewModel();
            sessionSearch.Filters = GetFilter(sessionId);
            sessionSearch.Songs = _filterRepository.FilterSongs(tracks, sessionSearch.Filters, tokenId);
            return sessionSearch;
        }
        public void CreateFilters(int? sessionId)
        {
            _filterRepository.CreateFilters(sessionId);
        }
    }
}