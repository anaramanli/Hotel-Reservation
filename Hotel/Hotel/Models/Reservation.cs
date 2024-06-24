using Hotel.Enums;
using Hotel.Models.Base;
using Stripe;
using System;
using System.Collections.Generic;

namespace Hotel.Models
{
    public class Reservation : BaseEntity
    {
        // Properties
        public DateTime CheckOutDate { get; set; } = DateTime.Now;
        public DateTime CheckInDate { get; set; } = DateTime.Now;

        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Message { get; set; }

        // Relations
        public Room Room { get; set; } 
        public Customer Customer { get; set; }

        public ICollection<Payment> Payments { get; set; }  // Collection of Payments
        public ICollection<ReservationService> ReservationServices { get; set; }  // Collection of Reservation Services

        public List<Extras> SelectedExtras { get; set; }  // List of selected Extras

        // Property to store total cost
        public decimal TotalCost { get; set; }

        // Total cost calculation method
        public void CalculateTotalCost(Dictionary<Extras, decimal> extrasPrices)
        {
            decimal totalCost = Room.Price;  // Initialize with base price of the room

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

            // Add room price to total cost (if Room is not null)
            if (Room != null)
            {
                totalCost += Room.Price;
            }

            // Assign calculated total cost to TotalCost property
            TotalCost = totalCost;
        }
    }
}
