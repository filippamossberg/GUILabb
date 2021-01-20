using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceHund.Model
{
    public class Hund
    {
        public List<Owner> Owners { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ras { get; set; }
    }
}
