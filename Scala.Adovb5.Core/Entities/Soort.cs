using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scala.Adovb5.Core.Entities
{
    public class Soort
    {
        public string Id { get; private set; }
        public string Soortnaam { get; set; }
        public Soort()
        {
            Id = Guid.NewGuid().ToString();
        }
        public Soort(string soortnaam)
        {
            Id = Guid.NewGuid().ToString();
            Soortnaam = soortnaam;
        }
        internal Soort(string id, string soortnaam)
        {
            Id = id;
            Soortnaam = soortnaam;
        }
        public override string ToString()
        {
            return Soortnaam;
        }
    }
}
