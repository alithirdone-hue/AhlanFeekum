using AhlanFeekum.CashPayments;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace AhlanFeekum.CashPayments
{
    public partial interface ICashPaymentRepository : IRepository<CashPayment, Guid>
    {

        Task DeleteAllAsync(
            string? filterText = null,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            DateTime? paymentDateMin = null,
            DateTime? paymentDateMax = null,
            string? description = null,
            CashPaymentStatus? status = null,
            Guid? userProfileId = null,
            Guid? reservationId = null,
            CancellationToken cancellationToken = default);
        Task<CashPaymentWithNavigationProperties> GetWithNavigationPropertiesAsync(
            Guid id,
            CancellationToken cancellationToken = default
        );

        Task<List<CashPaymentWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            DateTime? paymentDateMin = null,
            DateTime? paymentDateMax = null,
            string? description = null,
            CashPaymentStatus? status = null,
            Guid? userProfileId = null,
            Guid? reservationId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<CashPayment>> GetListAsync(
                    string? filterText = null,
                    long? amountMin = null,
                    long? amountMax = null,
                    string? currency = null,
                    DateTime? paymentDateMin = null,
                    DateTime? paymentDateMax = null,
                    string? description = null,
                    CashPaymentStatus? status = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            DateTime? paymentDateMin = null,
            DateTime? paymentDateMax = null,
            string? description = null,
            CashPaymentStatus? status = null,
            Guid? userProfileId = null,
            Guid? reservationId = null,
            CancellationToken cancellationToken = default);
    }
}