using AhlanFeekum.Reservations;
using Volo.Abp.Application.Dtos;
using System;

namespace AhlanFeekum.Reservations
{
    public abstract class GetReservationsInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public DateOnly? FromeDateMin { get; set; }
        public DateOnly? FromeDateMax { get; set; }
        public DateOnly? ToDateMin { get; set; }
        public DateOnly? ToDateMax { get; set; }
        public DateTime? CheckInDateMin { get; set; }
        public DateTime? CheckInDateMax { get; set; }
        public DateTime? CheckOutDateMin { get; set; }
        public DateTime? CheckOutDateMax { get; set; }
        public int? NumberOfGuestMin { get; set; }
        public int? NumberOfGuestMax { get; set; }
        public double? PriceMin { get; set; }
        public double? PriceMax { get; set; }
        public double? DiscountMin { get; set; }
        public double? DiscountMax { get; set; }
        public ReservationStatus? ReservationStatus { get; set; }
        public string? Notes { get; set; }
        public Guid? UserProfileId { get; set; }
        public Guid? SitePropertyId { get; set; }

        public GetReservationsInputBase()
        {

        }
    }
}