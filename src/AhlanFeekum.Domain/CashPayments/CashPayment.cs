using AhlanFeekum.CashPayments;
using AhlanFeekum.UserProfiles;
using AhlanFeekum.Reservations;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace AhlanFeekum.CashPayments
{
    public abstract class CashPaymentBase : FullAuditedAggregateRoot<Guid>
    {
        public virtual long Amount { get; set; }

        [NotNull]
        public virtual string Currency { get; set; }

        public virtual DateTime PaymentDate { get; set; }

        [CanBeNull]
        public virtual string? Description { get; set; }

        public virtual CashPaymentStatus Status { get; set; }
        public Guid UserProfileId { get; set; }
        public Guid ReservationId { get; set; }

        protected CashPaymentBase()
        {

        }

        public CashPaymentBase(Guid id, Guid userProfileId, Guid reservationId, long amount, string currency, DateTime paymentDate, CashPaymentStatus status, string? description = null)
        {

            Id = id;
            Check.NotNull(currency, nameof(currency));
            Amount = amount;
            Currency = currency;
            PaymentDate = paymentDate;
            Status = status;
            Description = description;
            UserProfileId = userProfileId;
            ReservationId = reservationId;
        }

    }
}