﻿namespace PropertyManager.Api.Models
{
    public class PropertyModel
    {
        public int PropertyId { get; set; }
        public int AddressId { get; set; }
        public string PropertyName { get; set; }
        public int? SquareFeet { get; set; }
        public int? NumberOfBedrooms { get; set; }
        public float? NumberOfBathrooms { get; set; }
        public int? NumberOfVehicles { get; set; }

        public AddressModel Address { get; set; }
        public bool HasOutdoorSpace { get; set; }
    }
}