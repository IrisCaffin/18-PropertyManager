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
using AutoMapper;
using PropertyManager.Api.Models;

namespace PropertyManager.Api.Controllers
{
    [Authorize]
    public class WorkOrdersController : ApiController
    {
        private PropertyManagerDataContext db = new PropertyManagerDataContext();

        // GET: api/WorkOrders
        public IEnumerable<WorkOrderModel> GetWorkOrders()
        {
            return Mapper.Map<IEnumerable<WorkOrderModel>>(
                db.WorkOrders.Where(wo => wo.Property.User.UserName == User.Identity.Name)
            );
        }

        // GET: api/WorkOrders/5
        [ResponseType(typeof(WorkOrderModel))]
        public IHttpActionResult GetWorkOrder(int id)
        {
            WorkOrder dbWorkOrder = db.WorkOrders.FirstOrDefault(wo => wo.Property.User.UserName == User.Identity.Name && wo.WorkOrderId == id);
            if (dbWorkOrder == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<WorkOrderModel>(dbWorkOrder));
        }

        // PUT: api/WorkOrders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWorkOrder(int id, WorkOrderModel workOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != workOrder.WorkOrderId)
            {
                return BadRequest();
            }

            WorkOrder dbWorkOrder = db.WorkOrders.FirstOrDefault(wo => wo.Property.User.UserName == User.Identity.Name && wo.WorkOrderId == id);

            if (dbWorkOrder == null)
            {
                return BadRequest();
            }

            dbWorkOrder.Update(workOrder);

            db.Entry(workOrder).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkOrderExists(id))
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

        // POST: api/WorkOrders
        [ResponseType(typeof(WorkOrderModel))]
        public IHttpActionResult PostWorkOrder(WorkOrderModel workOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbWorkOrder = new WorkOrder();

            // Lease doesn't need the following code in POST because it's not being created directly by the user
            // dbWorkOrder.User = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            dbWorkOrder.Update(workOrder);

            db.WorkOrders.Add(dbWorkOrder);
            db.SaveChanges();

            workOrder.WorkOrderId = dbWorkOrder.WorkOrderId;

            return CreatedAtRoute("DefaultApi", new { id = workOrder.WorkOrderId }, workOrder);
        }

        // DELETE: api/WorkOrders/5
        [ResponseType(typeof(WorkOrderModel))]
        public IHttpActionResult DeleteWorkOrder(int id)
        {
            WorkOrder workOrder = db.WorkOrders.FirstOrDefault(wo => wo.Property.User.UserName == User.Identity.Name && wo.WorkOrderId == id);

            if (workOrder == null)
            {
                return NotFound();
            }

            db.WorkOrders.Remove(workOrder);
            db.SaveChanges();

            return Ok(Mapper.Map<WorkOrderModel>(workOrder));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WorkOrderExists(int id)
        {
            return db.WorkOrders.Count(e => e.WorkOrderId == id) > 0;
        }
    }
}