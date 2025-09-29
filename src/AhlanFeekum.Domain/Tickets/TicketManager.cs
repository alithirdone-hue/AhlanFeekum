using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace AhlanFeekum.Tickets
{
    public abstract class TicketManagerBase : DomainService
    {
        protected ITicketRepository _ticketRepository;

        public TicketManagerBase(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public virtual async Task<Ticket> CreateAsync(
        Guid? userProfileId, string firstName, string lastName, string description, bool isFixed)
        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName));
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName));
            Check.NotNullOrWhiteSpace(description, nameof(description));

            var ticket = new Ticket(
             GuidGenerator.Create(),
             userProfileId, firstName, lastName, description, isFixed
             );

            return await _ticketRepository.InsertAsync(ticket);
        }

        public virtual async Task<Ticket> UpdateAsync(
            Guid id,
            Guid? userProfileId, string firstName, string lastName, string description, bool isFixed, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName));
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName));
            Check.NotNullOrWhiteSpace(description, nameof(description));

            var ticket = await _ticketRepository.GetAsync(id);

            ticket.UserProfileId = userProfileId;
            ticket.FirstName = firstName;
            ticket.LastName = lastName;
            ticket.Description = description;
            ticket.IsFixed = isFixed;

            ticket.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _ticketRepository.UpdateAsync(ticket);
        }

    }
}