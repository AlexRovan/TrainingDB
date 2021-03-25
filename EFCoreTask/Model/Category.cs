using System.Collections.Generic;

namespace EFCoreTask.Model
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<CategoryProducts> CategoryProduct { get; set; } = new List<CategoryProducts>();
    }
}
