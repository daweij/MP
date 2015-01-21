using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
  public class ChartVM
  {
    public ChartVM()
    {
      this.data = new List<double[]>();
    }

    public string label { get; set; }
    public List <double[]> data { get; set; }
  }

  public class SimpleChartVM
  {
    public string label { get; set; }
    public double data { get; set; }
  }
}
