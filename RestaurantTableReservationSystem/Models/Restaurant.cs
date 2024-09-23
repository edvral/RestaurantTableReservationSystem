namespace RestaurantTableReservationSystem.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string OpeningHours { get; set; }
        public string Description { get; set; }
        public ICollection<Table> Tables { get; set; }
    }
}
