using System;
using System.Collections.Generic;

namespace ShopDatabase.Model
{
    public class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public DateTime? DateBirth { get; set; }

        public virtual List<Order> Orders { get; set; } = new List<Order>();
    }
}
