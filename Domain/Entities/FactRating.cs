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
    public FactRating()
    {
      this.Genres = new HashSet<DimGenre>();
      this.Countries = new HashSet<DimCountry>();
    }

    public int Id { get; set; }
    public int MovieId { get; set; }
    public double Rating { get; set; }
    public int Votes { get; set; }

    public virtual DimMovie Movie { get; set; }
    public virtual ICollection<DimGenre> Genres { get; set; }
    public virtual ICollection<DimCountry> Countries { get; set; }
  }
}
