using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Contracts;

namespace Domain.Entities
{
  public class DimActor : IEntity
  {
    public DimActor()
    {
      this.Movies = new HashSet<DimMovie>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public ActorGender Gender { get; set; }

    public virtual ICollection<DimMovie> Movies { get; set; }
  }
}
