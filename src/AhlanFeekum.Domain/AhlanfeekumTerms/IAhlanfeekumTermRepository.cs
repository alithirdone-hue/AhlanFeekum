using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace AhlanFeekum.AhlanfeekumTerms
{
    public partial interface IAhlanfeekumTermRepository : IRepository<AhlanfeekumTerm, Guid>
    {

        Task DeleteAllAsync(
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
            CancellationToken cancellationToken = default);
        Task<List<AhlanfeekumTerm>> GetListAsync(
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
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
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
            CancellationToken cancellationToken = default);
    }
}