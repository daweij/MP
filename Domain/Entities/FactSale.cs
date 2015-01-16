﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Contracts;

namespace Domain.Entities
{
  public class FactSale : IEntity
  {
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int? YearId { get; set; } // we'll add that here aswell, to lessen loadtime
    public int CountryId { get; set; } // we'll add that here aswell, to lessen loadtime
    public double Revenue { get; set; }

    public virtual DimMovie Movie { get; set; }
    public virtual DimYear Year { get; set; }
    public virtual DimCountry Country { get; set; }
  }
}