using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Web.ViewModels
{
    public class OrderItemViewModel
    {
        public OrderItemViewModel(string address)
        {
            Address = address;
        }

        public string Address { get; }
    }
}
