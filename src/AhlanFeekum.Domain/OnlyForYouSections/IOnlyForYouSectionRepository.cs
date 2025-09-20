using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace AhlanFeekum.OnlyForYouSections
{
    public partial interface IOnlyForYouSectionRepository : IRepository<OnlyForYouSection, Guid>
    {

        Task DeleteAllAsync(
            string? filterText = null,
            string? firstPhotoExtension = null,
            string? secondPhotoExtension = null,
            string? thirdPhotoExtension = null,
            CancellationToken cancellationToken = default);
        Task<List<OnlyForYouSection>> GetListAsync(
                    string? filterText = null,
                    string? firstPhotoExtension = null,
                    string? secondPhotoExtension = null,
                    string? thirdPhotoExtension = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? firstPhotoExtension = null,
            string? secondPhotoExtension = null,
            string? thirdPhotoExtension = null,
            CancellationToken cancellationToken = default);
    }
}