using AutoMapper;
using PropertyManager.Api.Domain;
using PropertyManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PropertyManager.Api
{
    public static class WebApiConfig
    {
        // Register method below is being called in Startup.cs
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");

            config.EnableCors(cors);
            
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // API to return JSON instead of XML
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            CreateMaps();
        }

        public static void CreateMaps()
        {
            Mapper.CreateMap<Address, AddressModel>();
            Mapper.CreateMap<Property, PropertyModel>();
            Mapper.CreateMap<Tenant, TenantModel>();
            Mapper.CreateMap<Lease, LeaseModel>();
            Mapper.CreateMap<WorkOrder, WorkOrderModel>();

        }
    }
}
