namespace RestaurantTableReservationSystem.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int TableId { get; set; }
        public string GuestName { get; set; }
        public string GuestPhoneNumber { get; set; }
        public DateTime ReservationStart { get; set; }
        public DateTime ReservationEnd { get; set; }
        public int NumberOfGuests { get; set; }
        public string SpecialRequests { get; set; }
        public Table Table { get; set; }
    }
}
