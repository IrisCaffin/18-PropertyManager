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
    public class LeasesController : ApiController
    {
        private PropertyManagerDataContext db = new PropertyManagerDataContext();

        // GET: api/Leases
        public IEnumerable<LeaseModel> GetLeases()
        {
            return Mapper.Map<IEnumerable<LeaseModel>>(
                db.Leases.Where(l => l.Property.User.UserName == User.Identity.Name)
            );
        }

        // GET: api/Leases/5
        [ResponseType(typeof(LeaseModel))]
        public IHttpActionResult GetLease(int id)
        {
            Lease dbLease = db.Leases.FirstOrDefault(l => l.Property.User.UserName == User.Identity.Name && l.LeaseId == id);
            if (dbLease == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<LeaseModel>(dbLease));
        }

        // PUT: api/Leases/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLease(int id, LeaseModel lease)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lease.LeaseId)
            {
                return BadRequest();
            }

            Lease dbLease = db.Leases.FirstOrDefault(l => l.Property.User.UserName == User.Identity.Name && l.LeaseId == id);

            if(dbLease == null)
            {
                return BadRequest();
            }

            dbLease.Update(lease);

            db.Entry(dbLease).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaseExists(id))
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

        // POST: api/Leases
        [ResponseType(typeof(LeaseModel))]
        public IHttpActionResult PostLease(LeaseModel lease)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbLease = new Lease();

            // Lease doesn't need the following code in POST because it's not being created directly by the user
            // dbLease.User = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            dbLease.Update(lease);

            db.Leases.Add(dbLease);
            db.SaveChanges();

            lease.LeaseId = dbLease.LeaseId;

            return CreatedAtRoute("DefaultApi", new { id = lease.LeaseId }, lease);
        }

        // DELETE: api/Leases/5
        [ResponseType(typeof(LeaseModel))]
        public IHttpActionResult DeleteLease(int id)
        {
            Lease lease = db.Leases.FirstOrDefault(l => l.Property.User.UserName == User.Identity.Name && l.LeaseId == id);

            if (lease == null)
            {
                return NotFound();
            }

            db.Leases.Remove(lease);
            db.SaveChanges();

            return Ok(Mapper.Map<LeaseModel>(lease));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LeaseExists(int id)
        {
            return db.Leases.Count(e => e.LeaseId == id) > 0;
        }
    }
}