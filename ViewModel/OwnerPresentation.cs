using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceHund.ViewModel
{
    public class OwnerPresentation
    {
        public int Id { get; set; }
        public string Name
        {
            get { return Firstname + " " + Lastname; }
        }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Adress { get; set; }
    }
}
