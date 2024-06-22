using Hotel.Enums;
using Hotel.Models.Base;
using System;
using System.Collections.Generic;

namespace Hotel.Models
{
    public class Reservation : BaseEntity
    {
        public DateTime CheckOutDate { get; set; } = DateTime.Now;
        public DateTime CheckInDate { get; set; } = DateTime.Now;

        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }

        // Relation 
        public Room Room { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Payment> Payments { get; set; }
        public ICollection<ReservationService> ReservationServices { get; set; }

        public List<Extras> SelectedExtras { get; set; }

        // Property to store total cost
        public decimal TotalCost { get; set; }

        // Total cost calculation method
        public void CalculateTotalCost(Dictionary<Extras, decimal> extrasPrices)
        {
            decimal totalCost = 0;

            // Calculate extras cost
            if (SelectedExtras != null)
            {
                foreach (var extra in SelectedExtras)
                {
                    if (extrasPrices.ContainsKey(extra))
                    {
                        totalCost += extrasPrices[extra];
                    }
                }
            }

            // Add room price to total cost
            if (Room != null)
            {
                totalCost += Room.Price;
            }

            // Assign calculated total cost to property
            TotalCost = totalCost;
        }
    }
}
