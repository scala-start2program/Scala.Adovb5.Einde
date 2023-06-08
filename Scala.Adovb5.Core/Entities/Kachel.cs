using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scala.Adovb5.Core.Entities
{
    public class Kachel
    {
        public string Id { get; private set; }
        public string SoortId { get; set; }
        public string Merk { get; set; }
        public string Serie { get; set; }
        public decimal Prijs { get; set; }
        public Kachel()
        {
            Id = Guid.NewGuid().ToString();
        }
        public Kachel(string soortId, string merk, string serie, decimal prijs)
        {
            Id = Guid.NewGuid().ToString();
            SoortId = soortId;
            Merk = merk;
            Serie = serie;
            Prijs = prijs;
        }
        internal Kachel(string id, string soortId, string merk, string serie, decimal prijs)
        {
            Id = id;
            SoortId = soortId;
            Merk = merk;
            Serie = serie;
            Prijs = prijs;
        }
        public override string ToString()
        {
            return $"{Merk} - {Serie}";
        }
    }
}
