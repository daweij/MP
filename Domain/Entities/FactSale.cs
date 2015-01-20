using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Contracts;

namespace Domain.Entities
{
  public class FactSale : IEntity
  {
    public FactSale()
    {
      this.Genres = new HashSet<DimGenre>();
      this.Countries = new HashSet<DimCountry>();
    }

    public int Id { get; set; }
    public int MovieId { get; set; }
    public double? Budget { get; set; }
    public double? Revenue { get; set; }
    public double? RevenueUSA { get; set; }
    public double? RevenueNonUSA { get; set; }

    public double? Profit()
    {
      return this.Revenue;
    }

    public virtual DimMovie Movie { get; set; }
    public virtual ICollection<DimGenre> Genres { get; set; }
    public virtual ICollection<DimCountry> Countries { get; set; }
  }
}
