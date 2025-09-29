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

namespace AhlanFeekum.AhlanfeekumTerms
{
    public abstract class EfCoreAhlanfeekumTermRepositoryBase : EfCoreRepository<AhlanFeekumDbContext, AhlanfeekumTerm, Guid>
    {
        public EfCoreAhlanfeekumTermRepositoryBase(IDbContextProvider<AhlanFeekumDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        string? termsTitle = null,
            string? termsAnnotation = null,
            string? termsDescription = null,
            string? termsIconExtension = null,
            string? whoAreWeTitle = null,
            string? whoAreWeAnnotation = null,
            string? whoAreWeDescription = null,
            string? whoAreWeIconExtension = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {

            var query = await GetQueryableAsync();

            query = ApplyFilter(query, filterText, termsTitle, termsAnnotation, termsDescription, termsIconExtension, whoAreWeTitle, whoAreWeAnnotation, whoAreWeDescription, whoAreWeIconExtension, isActive);

            var ids = query.Select(x => x.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<AhlanfeekumTerm>> GetListAsync(
            string? filterText = null,
            string? termsTitle = null,
            string? termsAnnotation = null,
            string? termsDescription = null,
            string? termsIconExtension = null,
            string? whoAreWeTitle = null,
            string? whoAreWeAnnotation = null,
            string? whoAreWeDescription = null,
            string? whoAreWeIconExtension = null,
            bool? isActive = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, termsTitle, termsAnnotation, termsDescription, termsIconExtension, whoAreWeTitle, whoAreWeAnnotation, whoAreWeDescription, whoAreWeIconExtension, isActive);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? AhlanfeekumTermConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? termsTitle = null,
            string? termsAnnotation = null,
            string? termsDescription = null,
            string? termsIconExtension = null,
            string? whoAreWeTitle = null,
            string? whoAreWeAnnotation = null,
            string? whoAreWeDescription = null,
            string? whoAreWeIconExtension = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, termsTitle, termsAnnotation, termsDescription, termsIconExtension, whoAreWeTitle, whoAreWeAnnotation, whoAreWeDescription, whoAreWeIconExtension, isActive);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<AhlanfeekumTerm> ApplyFilter(
            IQueryable<AhlanfeekumTerm> query,
            string? filterText = null,
            string? termsTitle = null,
            string? termsAnnotation = null,
            string? termsDescription = null,
            string? termsIconExtension = null,
            string? whoAreWeTitle = null,
            string? whoAreWeAnnotation = null,
            string? whoAreWeDescription = null,
            string? whoAreWeIconExtension = null,
            bool? isActive = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.TermsTitle!.Contains(filterText!) || e.TermsAnnotation!.Contains(filterText!) || e.TermsDescription!.Contains(filterText!) || e.TermsIconExtension!.Contains(filterText!) || e.WhoAreWeTitle!.Contains(filterText!) || e.WhoAreWeAnnotation!.Contains(filterText!) || e.WhoAreWeDescription!.Contains(filterText!) || e.WhoAreWeIconExtension!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(termsTitle), e => e.TermsTitle.Contains(termsTitle))
                    .WhereIf(!string.IsNullOrWhiteSpace(termsAnnotation), e => e.TermsAnnotation.Contains(termsAnnotation))
                    .WhereIf(!string.IsNullOrWhiteSpace(termsDescription), e => e.TermsDescription.Contains(termsDescription))
                    .WhereIf(!string.IsNullOrWhiteSpace(termsIconExtension), e => e.TermsIconExtension.Contains(termsIconExtension))
                    .WhereIf(!string.IsNullOrWhiteSpace(whoAreWeTitle), e => e.WhoAreWeTitle.Contains(whoAreWeTitle))
                    .WhereIf(!string.IsNullOrWhiteSpace(whoAreWeAnnotation), e => e.WhoAreWeAnnotation.Contains(whoAreWeAnnotation))
                    .WhereIf(!string.IsNullOrWhiteSpace(whoAreWeDescription), e => e.WhoAreWeDescription.Contains(whoAreWeDescription))
                    .WhereIf(!string.IsNullOrWhiteSpace(whoAreWeIconExtension), e => e.WhoAreWeIconExtension.Contains(whoAreWeIconExtension))
                    .WhereIf(isActive.HasValue, e => e.IsActive == isActive);
        }
    }
}