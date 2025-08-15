using System.Collections.Generic;

namespace eUseControl.Web.Models.Product
{
    public class AddProductViewModel
    {
        public Product Product { get; set; }

        public List<string> Categories { get; set; }
    }
}