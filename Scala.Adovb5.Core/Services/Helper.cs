using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scala.Adovb5.Core.Services
{
    internal class Helper
    {
        public static string GetConnectionString()
        {
            //return @"Data Source=(local)\SQLEXPRESS;Initial Catalog=ScalaAdovb5; Integrated security=true;";
            return @"Data Source=HOW-94NWFS3;Initial Catalog=ScalaAdovb5; Integrated security=true;";
        }
        public static string HandleQuotes(string value)
        {
            return value.Trim().Replace("'", "''");
        }
    }
}
