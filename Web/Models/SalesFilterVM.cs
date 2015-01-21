using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
  public class SalesFilterVM
  {
    public SalesFilterVM()
    {
      this.AvailibleCountries = new List<DimCountry>();
      this.AvailibleGenres = new List<DimGenre>();
      this.Sales = new List<FactSale>();

      //this.SelectedCountries = new List<int>();
      //this.SelectedGenres = new List<int>();
    }

    [MaxLength(32)]
    [Display(Name = "Term")]
    public string SearchTerm { get; set; }

    public IEnumerable<DimCountry> AvailibleCountries { get; set; }
    public IEnumerable<DimGenre> AvailibleGenres { get; set; }
    public IEnumerable<FactSale> Sales { get; set; }

    [Required]
    [Display(Name = "Select countries")]
    public List<int> SelectedCountries { get; set; }

    [Required]
    [Display(Name = "Select genres")]
    public List<int> SelectedGenres { get; set; }

  }
}