using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Contracts;

namespace Domain.Entities
{
  public class DimDirector : IEntity
  {
    public DimDirector()
    {
      this.Movies = new HashSet<DimMovie>();
    }

    public int Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<DimMovie> Movies { get; set; }
  }
}
