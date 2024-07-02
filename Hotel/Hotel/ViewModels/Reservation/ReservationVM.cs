using Hotel.Enums; 

namespace Hotel.ViewModels.Reservation
{
    public class ReservationVM
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Models.Room? Room { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CheckInDate { get; set; } = DateTime.Now;
        public DateTime CheckOutDate { get; set; } = DateTime.Now.AddDays(1);
        public string? Message { get; set; }
        public List<Extras> SelectedExtras { get; set; }
        public decimal TotalCost { get; set; }

        public bool IsDeleted { get; set; }
        public Models.Reservation? Reservation { get; set; }
        public void CalculateTotalCost(Dictionary<Extras, decimal> extrasPrices)
        {
            int numberOfDays = (CheckOutDate - CheckInDate).Days;

            TotalCost = Room.Price * numberOfDays;

            if (SelectedExtras != null && extrasPrices != null)
            {
                foreach (var extra in SelectedExtras)
                {
                    if (extrasPrices.ContainsKey(extra))
                    {
                        TotalCost += extrasPrices[extra];
                    }
                }
            }
        }
    }
}
