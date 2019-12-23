﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
