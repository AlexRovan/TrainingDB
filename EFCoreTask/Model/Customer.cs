using System.Collections.Generic;

namespace EFCoreTask.Model
{
    public class Customer
    {
        public int Id { get; set; }

        public string Fio { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public virtual List<Order> Orders { get; set; } = new List<Order>();
    }
}
