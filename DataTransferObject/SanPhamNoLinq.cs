using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    public class SanPhamNoLinq
    {
        public int MaSp { get; set; }
        public string TenSp { get; set; }
        public int DonGia { get; set; }
        public int MaDm { get; set; }
        
        public override string ToString()
        {
            return this.MaSp + ": " + this.TenSp + " - " + this.DonGia + " - " + this.MaDm;
        }

    }
}
