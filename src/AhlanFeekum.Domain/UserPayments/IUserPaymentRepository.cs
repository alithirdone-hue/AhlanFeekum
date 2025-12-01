using AhlanFeekum.UserPayments;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace AhlanFeekum.UserPayments
{
    public partial interface IUserPaymentRepository : IRepository<UserPayment, Guid>
    {

        Task DeleteAllAsync(
            string? filterText = null,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            string? description = null,
            string? receiptEmail = null,
            long? amountCapturableMin = null,
            long? amountCapturableMax = null,
            long? amountReceivedMin = null,
            long? amountReceivedMax = null,
            string? confirmationMethod = null,
            UserPaymentStatus? status = null,
            string? stripPaymentId = null,
            string? stripClientSecret = null,
            DateTime? createdMin = null,
            DateTime? createdMax = null,
            PaymentMethod? paymentMethod = null,
            Guid? userProfileId = null,
            Guid? reservationId = null,
            CancellationToken cancellationToken = default);
        Task<UserPaymentWithNavigationProperties> GetWithNavigationPropertiesAsync(
            Guid id,
            CancellationToken cancellationToken = default
        );

        Task<List<UserPaymentWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            long? amountMin = null,
            long? amountMax = null,
            string? currency = null,
            string? description = null,
            string? receiptEmail = null,
            long? amountCapturableMin = null,
            long? amountCapturableMax = null,
            long? amountReceivedMin = null,
            long? amountReceivedMax = null,
            string? confirmationMethod = null,
            UserPaymentStatus? status = null,
            string? stripPaymentId = null,
            string? stripClientSecret = null,
            DateTime? createdMin = null,
            DateTime? createdMax = null,
            PaymentMethod? paymentMethod = null,
            Guid? userProfileId = null,
            Guid? reservationId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<UserPayment>> GetListAsync(
                    string? filterText = null,
                    long? amountMin = null,
                    long? amountMax = null,
                    string? currency = null,
                    string? description = null,
                    string? receiptEmail = null,
                    long? amountCapturableMin = null,
                    long? amountCapturableMax = null,
                    long? amountReceivedMin = null,
                    long? amountReceivedMax = null,
                    string? confirmationMethod = null,
                    UserPaymentStatus? status = null,
                    string? stripPaymentId = null,
                    string? stripClientSecret = null,
                    DateTime? createdMin = null,
                    DateTime? createdMax = null,
                    PaymentMethod? paymentMethod = null,
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
            string? description = null,
            string? receiptEmail = null,
            long? amountCapturableMin = null,
            long? amountCapturableMax = null,
            long? amountReceivedMin = null,
            long? amountReceivedMax = null,
            string? confirmationMethod = null,
            UserPaymentStatus? status = null,
            string? stripPaymentId = null,
            string? stripClientSecret = null,
            DateTime? createdMin = null,
            DateTime? createdMax = null,
            PaymentMethod? paymentMethod = null,
            Guid? userProfileId = null,
            Guid? reservationId = null,
            CancellationToken cancellationToken = default);
    }
}