using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Contracts;

namespace Domain.Entities
{
  public class DimCountry : IEntity
  {
    public DimCountry()
    {
      this.Sales = new HashSet<FactSale>();
      this.Ratings = new HashSet<FactRating>();
    }

    public int Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<FactSale> Sales { get; set; }
    public virtual ICollection<FactRating> Ratings { get; set; }
  }
}
