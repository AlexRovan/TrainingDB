﻿namespace EFCoreTask.Model
{
    public class PositionOrder
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order{ get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int ProductCount { get; set; }

    }
}
