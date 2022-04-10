using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PICarHost.Server
{
    public class MotorCommand
    {
        public String CommandID { get; set; }
        public String Command { get; set; }
        public double Parameter { get; set; }
    }
}
