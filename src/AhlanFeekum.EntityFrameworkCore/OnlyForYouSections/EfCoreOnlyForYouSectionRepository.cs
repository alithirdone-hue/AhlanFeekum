using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using AhlanFeekum.EntityFrameworkCore;

namespace AhlanFeekum.OnlyForYouSections
{
    public abstract class EfCoreOnlyForYouSectionRepositoryBase : EfCoreRepository<AhlanFeekumDbContext, OnlyForYouSection, Guid>
    {
        public EfCoreOnlyForYouSectionRepositoryBase(IDbContextProvider<AhlanFeekumDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        string? firstPhotoExtension = null,
            string? secondPhotoExtension = null,
            string? thirdPhotoExtension = null,
            CancellationToken cancellationToken = default)
        {

            var query = await GetQueryableAsync();

            query = ApplyFilter(query, filterText, firstPhotoExtension, secondPhotoExtension, thirdPhotoExtension);

            var ids = query.Select(x => x.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OnlyForYouSection>> GetListAsync(
            string? filterText = null,
            string? firstPhotoExtension = null,
            string? secondPhotoExtension = null,
            string? thirdPhotoExtension = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, firstPhotoExtension, secondPhotoExtension, thirdPhotoExtension);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? OnlyForYouSectionConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? firstPhotoExtension = null,
            string? secondPhotoExtension = null,
            string? thirdPhotoExtension = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, firstPhotoExtension, secondPhotoExtension, thirdPhotoExtension);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<OnlyForYouSection> ApplyFilter(
            IQueryable<OnlyForYouSection> query,
            string? filterText = null,
            string? firstPhotoExtension = null,
            string? secondPhotoExtension = null,
            string? thirdPhotoExtension = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.FirstPhotoExtension!.Contains(filterText!) || e.SecondPhotoExtension!.Contains(filterText!) || e.ThirdPhotoExtension!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(firstPhotoExtension), e => e.FirstPhotoExtension.Contains(firstPhotoExtension))
                    .WhereIf(!string.IsNullOrWhiteSpace(secondPhotoExtension), e => e.SecondPhotoExtension.Contains(secondPhotoExtension))
                    .WhereIf(!string.IsNullOrWhiteSpace(thirdPhotoExtension), e => e.ThirdPhotoExtension.Contains(thirdPhotoExtension));
        }
    }
}