using AhlanFeekum.UserProfiles;
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

namespace AhlanFeekum.Tickets
{
    public abstract class EfCoreTicketRepositoryBase : EfCoreRepository<AhlanFeekumDbContext, Ticket, Guid>
    {
        public EfCoreTicketRepositoryBase(IDbContextProvider<AhlanFeekumDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        string? firstName = null,
            string? lastName = null,
            string? description = null,
            bool? isFixed = null,
            Guid? userProfileId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();

            query = ApplyFilter(query, filterText, firstName, lastName, description, isFixed, userProfileId);

            var ids = query.Select(x => x.Ticket.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<TicketWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(ticket => new TicketWithNavigationProperties
                {
                    Ticket = ticket,
                    UserProfile = dbContext.Set<UserProfile>().FirstOrDefault(c => c.Id == ticket.UserProfileId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<TicketWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? description = null,
            bool? isFixed = null,
            Guid? userProfileId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, firstName, lastName, description, isFixed, userProfileId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? TicketConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<TicketWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from ticket in (await GetDbSetAsync())
                   join userProfile in (await GetDbContextAsync()).Set<UserProfile>() on ticket.UserProfileId equals userProfile.Id into userProfiles
                   from userProfile in userProfiles.DefaultIfEmpty()
                   select new TicketWithNavigationProperties
                   {
                       Ticket = ticket,
                       UserProfile = userProfile
                   };
        }

        protected virtual IQueryable<TicketWithNavigationProperties> ApplyFilter(
            IQueryable<TicketWithNavigationProperties> query,
            string? filterText,
            string? firstName = null,
            string? lastName = null,
            string? description = null,
            bool? isFixed = null,
            Guid? userProfileId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Ticket.FirstName!.Contains(filterText!) || e.Ticket.LastName!.Contains(filterText!) || e.Ticket.Description!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(firstName), e => e.Ticket.FirstName.Contains(firstName))
                    .WhereIf(!string.IsNullOrWhiteSpace(lastName), e => e.Ticket.LastName.Contains(lastName))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Ticket.Description.Contains(description))
                    .WhereIf(isFixed.HasValue, e => e.Ticket.IsFixed == isFixed)
                    .WhereIf(userProfileId != null && userProfileId != Guid.Empty, e => e.UserProfile != null && e.UserProfile.Id == userProfileId);
        }

        public virtual async Task<List<Ticket>> GetListAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? description = null,
            bool? isFixed = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, firstName, lastName, description, isFixed);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? TicketConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? description = null,
            bool? isFixed = null,
            Guid? userProfileId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, firstName, lastName, description, isFixed, userProfileId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Ticket> ApplyFilter(
            IQueryable<Ticket> query,
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? description = null,
            bool? isFixed = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.FirstName!.Contains(filterText!) || e.LastName!.Contains(filterText!) || e.Description!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(firstName), e => e.FirstName.Contains(firstName))
                    .WhereIf(!string.IsNullOrWhiteSpace(lastName), e => e.LastName.Contains(lastName))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description))
                    .WhereIf(isFixed.HasValue, e => e.IsFixed == isFixed);
        }
    }
}