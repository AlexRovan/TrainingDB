﻿using System.Collections.Generic;

namespace EFCoreTask.Model
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public virtual List<Category> Categories { get; set; } = new List<Category>();

        public virtual List<Order> Orders { get; set; } = new List<Order>();
    }
}
