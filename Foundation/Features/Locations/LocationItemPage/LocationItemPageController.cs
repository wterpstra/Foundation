﻿using EPiServer;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Web.Mvc;
using Foundation.Cms.Personalization;
using Foundation.Find.Cms.Locations.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Locations.LocationItemPage
{
    public class LocationItemPageController : PageController<Find.Cms.Models.Pages.LocationItemPage>
    {
        private readonly IContentRepository _contentRepository;
        private readonly ICmsTrackingService _trackingService;

        public LocationItemPageController(IContentRepository contentRepository,
            ICmsTrackingService trackingService)
        {
            _contentRepository = contentRepository;
            _trackingService = trackingService;
        }

        public async Task<ActionResult> Index(Find.Cms.Models.Pages.LocationItemPage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
            var model = new LocationViewModel(currentPage);
            if (!ContentReference.IsNullOrEmpty(currentPage.Image))
            {
                model.Image = _contentRepository.Get<ImageData>(currentPage.Image);
            }

            model.LocationNavigation.ContinentLocations = SearchClient.Instance
                .Search<Find.Cms.Models.Pages.LocationItemPage>()
                .Filter(x => x.Continent.Match(currentPage.Continent))
                .OrderBy(x => x.PageName)
                .FilterForVisitor()
                .Take(100)
                .StaticallyCacheFor(new System.TimeSpan(0, 10, 0))
                .GetContentResult();

            model.LocationNavigation.CloseBy = SearchClient.Instance
                .Search<Find.Cms.Models.Pages.LocationItemPage>()
                .Filter(x => x.Continent.Match(currentPage.Continent)
                             & !x.PageLink.Match(currentPage.PageLink))
                .FilterForVisitor()
                .OrderBy(x => x.Coordinates)
                .DistanceFrom(currentPage.Coordinates)
                .Take(5)
                .StaticallyCacheFor(new System.TimeSpan(0, 10, 0))
                .GetContentResult();

            var editingHints = ViewData.GetEditHints<LocationViewModel, Find.Cms.Models.Pages.LocationItemPage>();
            editingHints.AddFullRefreshFor(p => p.Image);
            editingHints.AddFullRefreshFor(p => p.Tags);

            return View(model);
        }

        private IEnumerable<Find.Cms.Models.Pages.LocationItemPage> GetRelatedDestinations(Find.Cms.Models.Pages.LocationItemPage currentPage)
        {
            IQueriedSearch<Find.Cms.Models.Pages.LocationItemPage> query = SearchClient.Instance
                .Search<Find.Cms.Models.Pages.LocationItemPage>()
                .MoreLike(SearchTextFly(currentPage))
                .BoostMatching(x =>
                    x.Country.Match(currentPage.Country ?? ""), 2)
                .BoostMatching(x =>
                    x.Continent.Match(currentPage.Continent ?? ""), 1.5)
                .BoostMatching(x =>
                    x.Coordinates
                        .WithinDistanceFrom(currentPage.Coordinates ?? new GeoLocation(0, 0),
                            1000.Kilometers()), 2.5);

            query = currentPage.Category.Aggregate(query,
                (current, category) =>
                    current.BoostMatching(x => x.InCategory(category), 1.5));

            return query
                .Filter(x => !x.PageLink.Match(currentPage.PageLink))
                .FilterForVisitor()
                .Take(3)
                .GetPagesResult();
        }

        public virtual string SearchTextFly(Find.Cms.Models.Pages.LocationItemPage currentPage)
        {
            var searchText = "";

            return searchText;
        }

    }
}