using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Contracts;

namespace Domain.Entities
{
  public class FactRating : IEntity
  {
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int? Year { get; set; } // we'll add that here aswell, to lessen loadtime
    public string Country { get; set; } // we'll add that here aswell, to lessen loadtime
    public decimal Rating { get; set; }
    public double Votes { get; set; }

    public virtual DimMovie Movie { get; set; }
  }
}
