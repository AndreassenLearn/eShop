﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceLayer.LocomotiveService
{
    public static class LocomotiveSearch
    {
        /// <summary>
        /// Order locomotives.
        /// </summary>
        /// <param name="locomotives">Locomotives to order.</param>
        /// <param name="orderByOptions">What property to order by.</param>
        /// <returns><see cref="IQueryable"/> of <see cref="ListLocomotiveDto"/>.</returns>
        public static IQueryable<ListLocomotiveDto> OrderLocomotivesBy(this IQueryable<ListLocomotiveDto> locomotives, OrderByOptions orderByOptions) => orderByOptions switch
        {
            OrderByOptions.ByNameAsc => locomotives.OrderBy(l => l.Name),
            OrderByOptions.ByNameDesc => locomotives.OrderByDescending(l => l.Name),
            OrderByOptions.ByPriceAsc => locomotives.OrderBy(l => l.Price),
            OrderByOptions.ByPriceDesc => locomotives.OrderByDescending(l => l.Price),
            OrderByOptions.ByRailwayCompanyAsc => locomotives.OrderBy(l => l.RailwayCompanyName),
            OrderByOptions.ByRailwayCompanyDesc => locomotives.OrderByDescending(l => l.RailwayCompanyName),
            _ => throw new ArgumentOutOfRangeException(nameof(orderByOptions), orderByOptions, null),
        };

        /// <summary>
        /// Search for locomotives using a search string. Effectively filters out non-matching objects.
        /// Whitespaces in <paramref name="searchString"/> is used to separate multiple search parameters, ie. "BR 218 DB" will search for both "BR", "218", and "DB".
        /// </summary>
        /// <param name="locomotives">Locomotives to search in.</param>
        /// <param name="searchString">Search string.</param>
        /// <returns><see cref="IQueryable"/> of <see cref="ListLocomotiveDto"/>.</returns>
        public static IQueryable<ListLocomotiveDto> SearchFor(this IQueryable<ListLocomotiveDto> locomotives, string searchString)
        {
            string[] searchParams = searchString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            return locomotives
                .Where(l => string.IsNullOrEmpty(searchString)
                || searchParams.Any(sp => l.Name.Contains(sp))
                || searchParams.Any(sp => l.RailwayCompanyName.Contains(sp))
                );
        }

        /// <summary>
        /// Filter locomotives.
        /// </summary>
        /// <param name="locomotives">Locomotives to filter.</param>
        /// <param name="filterOptions">What filters to filter by.</param>
        /// <returns><see cref="IQueryable"/> of <see cref="ListLocomotiveDto"/>.</returns>
        public static IQueryable<ListLocomotiveDto> Filter(this IQueryable<ListLocomotiveDto> locomotives, FilterOptions filterOptions) => locomotives
            .Where(l => (!filterOptions.Tags.Any() || filterOptions.Tags.Contains(l.Tag)));
    }
}