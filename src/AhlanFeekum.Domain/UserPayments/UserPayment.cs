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

        [CanBeNull]
        public virtual string? Currency { get; set; }

        [CanBeNull]
        public virtual string? Description { get; set; }

        [CanBeNull]
        public virtual string? ReceiptEmail { get; set; }

        public virtual long AmountCapturable { get; set; }

        public virtual long AmountReceived { get; set; }

        [CanBeNull]
        public virtual string? ConfirmationMethod { get; set; }

        public virtual UserPaymentStatus Status { get; set; }

        [CanBeNull]
        public virtual string? StripPaymentId { get; set; }

        [CanBeNull]
        public virtual string? StripClientSecret { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }
        public Guid UserProfileId { get; set; }
        public Guid ReservationId { get; set; }

        protected UserPaymentBase()
        {

        }

        public UserPaymentBase(Guid id, Guid userProfileId, Guid reservationId, long amount, long amountCapturable, long amountReceived, UserPaymentStatus status, DateTime created, PaymentMethod paymentMethod, string? currency = null, string? description = null, string? receiptEmail = null, string? confirmationMethod = null, string? stripPaymentId = null, string? stripClientSecret = null)
        {

            Id = id;
            Amount = amount;
            AmountCapturable = amountCapturable;
            AmountReceived = amountReceived;
            Status = status;
            Created = created;
            PaymentMethod = paymentMethod;
            Currency = currency;
            Description = description;
            ReceiptEmail = receiptEmail;
            ConfirmationMethod = confirmationMethod;
            StripPaymentId = stripPaymentId;
            StripClientSecret = stripClientSecret;
            UserProfileId = userProfileId;
            ReservationId = reservationId;
        }

    }
}