﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PropertyManager.Api.Domain;
using PropertyManager.Api.Infrastructure;
using PropertyManager.Api.Models;
using AutoMapper;

namespace PropertyManager.Api.Controllers
{
    [Authorize]
    public class PropertiesController : ApiController
    {
        private PropertyManagerDataContext db = new PropertyManagerDataContext();

        // GET: api/Properties
        public IEnumerable<PropertyModel> GetProperties()
        {
            return Mapper.Map<IEnumerable<PropertyModel>>(
                db.Properties.Where(p => p.User.UserName == User.Identity.Name)
            );
        }

        // GET: api/Properties/5
        [ResponseType(typeof(PropertyModel))]
        public IHttpActionResult GetProperty(int id)
        {
            // Property property = db.Properties.Find(id);


            // The first or default method: this will return the 1st item in the database that matches the quiry or it will return null
            Property dbProperty = db.Properties.FirstOrDefault(p => p.User.UserName == User.Identity.Name && p.PropertyId == id);

            if (dbProperty == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PropertyModel>(dbProperty));
        }

        // PUT: api/Properties/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProperty(int id, PropertyModel property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != property.PropertyId)
            {
                return BadRequest();
            }

            Property dbProperty = db.Properties.FirstOrDefault(p => p.User.UserName == User.Identity.Name && p.PropertyId == id);

            if(dbProperty == null)
            {
                return BadRequest();
            }

            dbProperty.Update(property);

            db.Entry(dbProperty).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Properties
        [ResponseType(typeof(PropertyModel))]
        public IHttpActionResult PostProperty(PropertyModel property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbProperty = new Property();

            // User in this case is the token, not the user
            dbProperty.User = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            dbProperty.Update(property);

            db.Properties.Add(dbProperty);
            db.SaveChanges();

            property.PropertyId = dbProperty.PropertyId;

            return CreatedAtRoute("DefaultApi", new { id = property.PropertyId }, property);
        }

        // DELETE: api/Properties/5
        [ResponseType(typeof(PropertyModel))]
        public IHttpActionResult DeleteProperty(int id)
        {
            Property property = db.Properties.FirstOrDefault(p => p.User.UserName == User.Identity.Name && p.PropertyId == id);

            if (property == null)
            {
                return NotFound();
            }

            db.Properties.Remove(property);
            db.SaveChanges();

            return Ok(Mapper.Map<PropertyModel>(property));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PropertyExists(int id)
        {
            return db.Properties.Count(e => e.PropertyId == id) > 0;
        }
    }
}