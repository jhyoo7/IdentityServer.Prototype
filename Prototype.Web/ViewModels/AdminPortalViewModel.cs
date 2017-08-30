using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Web.ViewModels
{
    public class AdminPortalViewModel
    {
        public AdminPortalViewModel(string address)
        {
            Address = address;
        }

        public string Address { get; }
    }
}
