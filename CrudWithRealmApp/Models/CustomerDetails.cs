using System;
using Realms;

namespace CrudWithRealmApp.Models
{
    public class CustomerDetails : RealmObject 
    {
        public CustomerDetails()
        {
        }

        [PrimaryKey]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerAge { get; set; }
    }
}
