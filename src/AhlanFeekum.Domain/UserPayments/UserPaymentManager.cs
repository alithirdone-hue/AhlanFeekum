using AhlanFeekum.UserPayments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace AhlanFeekum.UserPayments
{
    public abstract class UserPaymentManagerBase : DomainService
    {
        protected IUserPaymentRepository _userPaymentRepository;

        public UserPaymentManagerBase(IUserPaymentRepository userPaymentRepository)
        {
            _userPaymentRepository = userPaymentRepository;
        }

        public virtual async Task<UserPayment> CreateAsync(
        Guid userProfileId, Guid reservationId, long amount, string currency, long amountCapturable, long amountReceived, UserPaymentStatus status, string stripPaymentId, string stripClientSecret, string created, string? description = null, string? receiptEmail = null, string? confirmationMethod = null)
        {
            Check.NotNull(userProfileId, nameof(userProfileId));
            Check.NotNull(reservationId, nameof(reservationId));
            Check.NotNullOrWhiteSpace(currency, nameof(currency));
            Check.NotNull(status, nameof(status));
            Check.NotNullOrWhiteSpace(stripPaymentId, nameof(stripPaymentId));
            Check.NotNullOrWhiteSpace(stripClientSecret, nameof(stripClientSecret));
            Check.NotNullOrWhiteSpace(created, nameof(created));

            var userPayment = new UserPayment(
             GuidGenerator.Create(),
             userProfileId, reservationId, amount, currency, amountCapturable, amountReceived, status, stripPaymentId, stripClientSecret, created, description, receiptEmail, confirmationMethod
             );

            return await _userPaymentRepository.InsertAsync(userPayment);
        }

        public virtual async Task<UserPayment> UpdateAsync(
            Guid id,
            Guid userProfileId, Guid reservationId, long amount, string currency, long amountCapturable, long amountReceived, UserPaymentStatus status, string stripPaymentId, string stripClientSecret, string created, string? description = null, string? receiptEmail = null, string? confirmationMethod = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNull(userProfileId, nameof(userProfileId));
            Check.NotNull(reservationId, nameof(reservationId));
            Check.NotNullOrWhiteSpace(currency, nameof(currency));
            Check.NotNull(status, nameof(status));
            Check.NotNullOrWhiteSpace(stripPaymentId, nameof(stripPaymentId));
            Check.NotNullOrWhiteSpace(stripClientSecret, nameof(stripClientSecret));
            Check.NotNullOrWhiteSpace(created, nameof(created));

            var userPayment = await _userPaymentRepository.GetAsync(id);

            userPayment.UserProfileId = userProfileId;
            userPayment.ReservationId = reservationId;
            userPayment.Amount = amount;
            userPayment.Currency = currency;
            userPayment.AmountCapturable = amountCapturable;
            userPayment.AmountReceived = amountReceived;
            userPayment.Status = status;
            userPayment.StripPaymentId = stripPaymentId;
            userPayment.StripClientSecret = stripClientSecret;
            userPayment.Created = created;
            userPayment.Description = description;
            userPayment.ReceiptEmail = receiptEmail;
            userPayment.ConfirmationMethod = confirmationMethod;

            userPayment.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _userPaymentRepository.UpdateAsync(userPayment);
        }

    }
}