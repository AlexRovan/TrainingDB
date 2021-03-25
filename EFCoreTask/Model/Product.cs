using System.Collections.Generic;

namespace EFCoreTask.Model
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public virtual List<CategoryProducts> CategoryProduct { get; set; } = new List<CategoryProducts>();

        public virtual List<PositionsOrder> PositionOrder { get; set; } = new List<PositionsOrder>();
    }
}