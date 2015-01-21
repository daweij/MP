using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Contracts;

namespace Domain.Entities
{
  public class DimMovie : IEntity
  {
    public DimMovie()
    {
      this.Genres = new HashSet<DimGenre>();
      this.Actors = new HashSet<DimActor>();
      this.Countries = new HashSet<DimCountry>();
      this.Directors = new HashSet<DimDirector>();
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }

    public virtual ICollection<DimGenre> Genres { get; set; }
    public virtual ICollection<DimActor> Actors { get; set; }
    public virtual ICollection<DimCountry> Countries { get; set; }
    public virtual ICollection<DimDirector> Directors { get; set; }

    //public virtual FactSale Sale { get; set; }
    //public virtual FactRating Rating { get; set; }
  }
}
