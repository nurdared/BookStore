using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Interfaces
{
    public interface ISlideRepository
    {
        IEnumerable<Slide> Slide { get; }
    }
}
