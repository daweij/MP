using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Contracts;

namespace Domain.Entities
{
  public class DimDate : IEntity
  {
    public int Id { get; set; }
    public int Year { get; set; }
  }
}
