using PropertyManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyManager.Api.Domain
{
    public class Property
    {
        public int PropertyId { get; set; }
        public string UserId { get; set; }

        public int AddressId { get; set; }
        public string PropertyName { get; set; }
        public int? SquareFeet { get; set; }
        public int? NumberOfBedrooms { get; set; }
        public float? NumberOfBathrooms { get; set; }
        public int? NumberOfVehicles { get; set; }
        public bool HasOutdoorSpace { get; set; }

        public virtual Address Address { get; set; }

        public virtual PropertyManagerUser User { get; set; }

        public virtual ICollection<Lease> Leases { get; set; }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }

        public void Update(PropertyModel property)
        {
            PropertyName = property.PropertyName;
            SquareFeet = property.SquareFeet;
            NumberOfBedrooms = property.NumberOfBedrooms;
            NumberOfBathrooms = property.NumberOfBathrooms;
            NumberOfVehicles = property.NumberOfVehicles;
            HasOutdoorSpace = property.HasOutdoorSpace;
            Address.Update(property.Address);
        }
    }
}