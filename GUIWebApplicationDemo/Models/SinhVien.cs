using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GUIWebApplicationDemo.Models
{
    public class SinhVien
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public override string ToString()
        {
            return this.Account + ": " + this.Password;
        }
    }
}