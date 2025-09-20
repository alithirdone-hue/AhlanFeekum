using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace AhlanFeekum.Statuses
{
    public abstract class StatusManagerBase : DomainService
    {
        protected IStatusRepository _statusRepository;

        public StatusManagerBase(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public virtual async Task<Status> CreateAsync(
        string name, int order, bool isActive)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var status = new Status(
             GuidGenerator.Create(),
             name, order, isActive
             );

            return await _statusRepository.InsertAsync(status);
        }

        public virtual async Task<Status> UpdateAsync(
            Guid id,
            string name, int order, bool isActive, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var status = await _statusRepository.GetAsync(id);

            status.Name = name;
            status.Order = order;
            status.IsActive = isActive;

            status.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _statusRepository.UpdateAsync(status);
        }

    }
}