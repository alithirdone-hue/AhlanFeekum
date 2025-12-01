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
        Guid userProfileId, Guid reservationId, long amount, long amountCapturable, long amountReceived, UserPaymentStatus status, DateTime created, PaymentMethod paymentMethod, string? currency = null, string? description = null, string? receiptEmail = null, string? confirmationMethod = null, string? stripPaymentId = null, string? stripClientSecret = null)
        {
            Check.NotNull(userProfileId, nameof(userProfileId));
            Check.NotNull(reservationId, nameof(reservationId));
            Check.NotNull(status, nameof(status));
            Check.NotNull(created, nameof(created));
            Check.NotNull(paymentMethod, nameof(paymentMethod));

            var userPayment = new UserPayment(
             GuidGenerator.Create(),
             userProfileId, reservationId, amount, amountCapturable, amountReceived, status, created, paymentMethod, currency, description, receiptEmail, confirmationMethod, stripPaymentId, stripClientSecret
             );

            return await _userPaymentRepository.InsertAsync(userPayment);
        }

        public virtual async Task<UserPayment> UpdateAsync(
            Guid id,
            Guid userProfileId, Guid reservationId, long amount, long amountCapturable, long amountReceived, UserPaymentStatus status, DateTime created, PaymentMethod paymentMethod, string? currency = null, string? description = null, string? receiptEmail = null, string? confirmationMethod = null, string? stripPaymentId = null, string? stripClientSecret = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNull(userProfileId, nameof(userProfileId));
            Check.NotNull(reservationId, nameof(reservationId));
            Check.NotNull(status, nameof(status));
            Check.NotNull(created, nameof(created));
            Check.NotNull(paymentMethod, nameof(paymentMethod));

            var userPayment = await _userPaymentRepository.GetAsync(id);

            userPayment.UserProfileId = userProfileId;
            userPayment.ReservationId = reservationId;
            userPayment.Amount = amount;
            userPayment.AmountCapturable = amountCapturable;
            userPayment.AmountReceived = amountReceived;
            userPayment.Status = status;
            userPayment.Created = created;
            userPayment.PaymentMethod = paymentMethod;
            userPayment.Currency = currency;
            userPayment.Description = description;
            userPayment.ReceiptEmail = receiptEmail;
            userPayment.ConfirmationMethod = confirmationMethod;
            userPayment.StripPaymentId = stripPaymentId;
            userPayment.StripClientSecret = stripClientSecret;

            userPayment.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _userPaymentRepository.UpdateAsync(userPayment);
        }

    }
}