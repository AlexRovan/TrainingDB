using System.Collections.Generic;

namespace EFCoreTask.Model
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public virtual List<CategoryProduct> CategoryProduct { get; set; } = new List<CategoryProduct>();

        public virtual List<PositionOrder> PositionOrder { get; set; } = new List<PositionOrder>();
    }
}