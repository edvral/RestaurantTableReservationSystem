namespace RestaurantTableReservationSystem.DTOs
{
    public class ReservationDTO
    {
        public string GuestName { get; set; }
        public string GuestPhoneNumber { get; set; }
        public DateTime ReservationStart { get; set; }
        public DateTime ReservationEnd { get; set; }
        public int NumberOfGuests { get; set; }
        public string SpecialRequests { get; set; }
    }
}
