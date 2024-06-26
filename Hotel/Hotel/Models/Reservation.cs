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
        // Qr Code
        //public string QRCodePath { get; set; }

        // Relations
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int CustomerId { get; set; } 
        public Customer Customer { get; set; }

        //public ICollection<Payment> Payments { get; set; }
        public ICollection<ReservationService> ReservationServices { get; set; }
        public List<Extras>? SelectedExtras { get; set; }

        public decimal TotalCost { get; set; }

        // Total cost calculation method
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
