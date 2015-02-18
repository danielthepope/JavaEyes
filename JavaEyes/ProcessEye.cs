using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaEyes
{
    class ProcessEye
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private long _ram;
        public long Ram { get { return _ram; } set { _ram = value; } }
        public string RamString
        { 
            get 
            {
                if (_ram >= 10485760) return String.Format("{0:n1} MB", _ram / 1048576.0);
                else return String.Format("{0:n0} KB", Ram / 1024.0);
            }
        }

        public ProcessEye(Process p)
        {
            Id = p.Id;
            Name = p.ProcessName;
            Ram = p.WorkingSet64;
        }
    }
}
