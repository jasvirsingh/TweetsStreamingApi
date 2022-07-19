using System.Collections.Generic;

namespace TweetsAccess.Data
{
    public class ListCustomers
    {
        public List<CustomerEntity> Customers { get; set; }

        public ListCustomers()
        {
            Customers = new List<CustomerEntity>();
        }
    }
}
