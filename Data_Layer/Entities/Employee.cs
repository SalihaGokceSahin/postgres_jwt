using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Entities
{
    public class Employee:BaseEntity
    {
        public string NameSurname { get; set; }
        public string Title { get; set; }
    }
}
