using System;
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
    public class TenantsController : ApiController
    {
        private PropertyManagerDataContext db = new PropertyManagerDataContext();

        // GET: api/Tenants
        public IEnumerable<TenantModel> GetTenants()
        {
            return Mapper.Map<IEnumerable<TenantModel>>(
                db.Tenants.Where(t => t.User.UserName == User.Identity.Name)
            );
        }

        // GET: api/Tenants/5
        [ResponseType(typeof(Tenant))]
        public IHttpActionResult GetTenant(int id)
        {
            Tenant dbTenant = db.Tenants.FirstOrDefault(t => t.User.UserName == User.Identity.Name && t.TenantId == id);

            if (dbTenant == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<TenantModel>(dbTenant));
        }

        // PUT: api/Tenants/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTenant(int id, TenantModel tenant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tenant.TenantId)
            {
                return BadRequest();
            }

            Tenant dbTenant = db.Tenants.FirstOrDefault(t => t.User.UserName == User.Identity.Name && t.TenantId == id);

            if(dbTenant == null)
            {
                return BadRequest();
            }

            dbTenant.Update(tenant);
             
            db.Entry(tenant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenantExists(id))
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

        // POST: api/Tenants
        [ResponseType(typeof(TenantModel))]
        public IHttpActionResult PostTenant(TenantModel tenant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbTenant = new Tenant();

            dbTenant.User = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            dbTenant.Update(tenant);

            db.Tenants.Add(dbTenant);
            db.SaveChanges();

            tenant.TenantId = dbTenant.TenantId;

            return CreatedAtRoute("DefaultApi", new { id = tenant.TenantId }, tenant);
        }

        // DELETE: api/Tenants/5
        [ResponseType(typeof(Tenant))]
        public IHttpActionResult DeleteTenant(int id)
        {
            Tenant tenant = db.Tenants.FirstOrDefault(t => t.User.UserName == User.Identity.Name && t.TenantId == id);

            if (tenant == null)
            {
                return NotFound();
            }

            db.Tenants.Remove(tenant);
            db.SaveChanges();

            return Ok(Mapper.Map<TenantModel>(tenant));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TenantExists(int id)
        {
            return db.Tenants.Count(e => e.TenantId == id) > 0;
        }
    }
}