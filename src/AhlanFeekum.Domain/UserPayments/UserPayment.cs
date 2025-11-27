using AhlanFeekum.UserPayments;
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

namespace AhlanFeekum.UserPayments
{
    public abstract class UserPaymentBase : FullAuditedAggregateRoot<Guid>
    {
        public virtual long Amount { get; set; }

        [NotNull]
        public virtual string Currency { get; set; }

        [CanBeNull]
        public virtual string? Description { get; set; }

        [CanBeNull]
        public virtual string? ReceiptEmail { get; set; }

        public virtual long AmountCapturable { get; set; }

        public virtual long AmountReceived { get; set; }

        [CanBeNull]
        public virtual string? ConfirmationMethod { get; set; }

        public virtual UserPaymentStatus Status { get; set; }

        [NotNull]
        public virtual string StripPaymentId { get; set; }

        [NotNull]
        public virtual string StripClientSecret { get; set; }
        public Guid UserProfileId { get; set; }
        public Guid ReservationId { get; set; }

        protected UserPaymentBase()
        {

        }

        public UserPaymentBase(Guid id, Guid userProfileId, Guid reservationId, long amount, string currency, long amountCapturable, long amountReceived, UserPaymentStatus status, string stripPaymentId, string stripClientSecret, string? description = null, string? receiptEmail = null, string? confirmationMethod = null)
        {

            Id = id;
            Check.NotNull(currency, nameof(currency));
            Check.NotNull(stripPaymentId, nameof(stripPaymentId));
            Check.NotNull(stripClientSecret, nameof(stripClientSecret));
            Amount = amount;
            Currency = currency;
            AmountCapturable = amountCapturable;
            AmountReceived = amountReceived;
            Status = status;
            StripPaymentId = stripPaymentId;
            StripClientSecret = stripClientSecret;
            Description = description;
            ReceiptEmail = receiptEmail;
            ConfirmationMethod = confirmationMethod;
            UserProfileId = userProfileId;
            ReservationId = reservationId;
        }

    }
}