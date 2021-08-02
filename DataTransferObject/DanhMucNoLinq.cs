using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    public class DanhMucNoLinq
    {
        public int MaDm { get; set; }
        public string TenDm { get; set; }
        public override string ToString()
        {
            return this.MaDm + ": " + this.TenDm;
        }
    }
}
