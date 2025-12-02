using AhlanFeekum.CashPayments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace AhlanFeekum.CashPayments
{
    public abstract class CashPaymentManagerBase : DomainService
    {
        protected ICashPaymentRepository _cashPaymentRepository;

        public CashPaymentManagerBase(ICashPaymentRepository cashPaymentRepository)
        {
            _cashPaymentRepository = cashPaymentRepository;
        }

        public virtual async Task<CashPayment> CreateAsync(
        Guid userProfileId, Guid reservationId, long amount, string currency, DateTime paymentDate, CashPaymentStatus status, string? description = null)
        {
            Check.NotNull(userProfileId, nameof(userProfileId));
            Check.NotNull(reservationId, nameof(reservationId));
            Check.NotNullOrWhiteSpace(currency, nameof(currency));
            Check.NotNull(paymentDate, nameof(paymentDate));
            Check.NotNull(status, nameof(status));

            var cashPayment = new CashPayment(
             GuidGenerator.Create(),
             userProfileId, reservationId, amount, currency, paymentDate, status, description
             );

            return await _cashPaymentRepository.InsertAsync(cashPayment);
        }

        public virtual async Task<CashPayment> UpdateAsync(
            Guid id,
            Guid userProfileId, Guid reservationId, long amount, string currency, DateTime paymentDate, CashPaymentStatus status, string? description = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNull(userProfileId, nameof(userProfileId));
            Check.NotNull(reservationId, nameof(reservationId));
            Check.NotNullOrWhiteSpace(currency, nameof(currency));
            Check.NotNull(paymentDate, nameof(paymentDate));
            Check.NotNull(status, nameof(status));

            var cashPayment = await _cashPaymentRepository.GetAsync(id);

            cashPayment.UserProfileId = userProfileId;
            cashPayment.ReservationId = reservationId;
            cashPayment.Amount = amount;
            cashPayment.Currency = currency;
            cashPayment.PaymentDate = paymentDate;
            cashPayment.Status = status;
            cashPayment.Description = description;

            cashPayment.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _cashPaymentRepository.UpdateAsync(cashPayment);
        }

    }
}