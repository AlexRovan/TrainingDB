﻿using System.Collections.Generic;

namespace ShopDatabase.Model
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<CategoryProducts> CategoryProduct { get; set; } = new List<CategoryProducts>();
    }
}
