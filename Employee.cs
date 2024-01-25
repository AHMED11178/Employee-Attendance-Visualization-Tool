using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceVisualizer
{
    public class Employee
    {
        private string id;

        public Employee(string id)
        {
            this.id = id;
        }

        public String getID() { 

            return this.id;
        
        }

        public void setID(String id)
        {
            this.id = id;
        }
    }
}
