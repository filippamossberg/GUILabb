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
using WebHundApi;

namespace WebHundApi.Controllers
{
    public class OwnersController : ApiController
    {
        private HundModel db = new HundModel();

        // GET: api/Owners
        public IQueryable<Owners> GetOwners()
        {
            return db.Owners;
        }

        // GET: api/Owners/5
        [ResponseType(typeof(Owners))]
        public IHttpActionResult GetOwners(int id)
        {
            Owners owners = db.Owners.Find(id);
            if (owners == null)
            {
                return NotFound();
            }

            return Ok(owners);
        }

        // PUT: api/Owners/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOwners(int id, Owners owners)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != owners.Id)
            {
                return BadRequest();
            }

            db.Entry(owners).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnersExists(id))
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

        // POST: api/Owners
        [ResponseType(typeof(Owners))]
        public IHttpActionResult PostOwners(Owners owners)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Owners.Add(owners);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = owners.Id }, owners);
        }

        // DELETE: api/Owners/5
        [ResponseType(typeof(Owners))]
        public IHttpActionResult DeleteOwners(int id)
        {
            Owners owners = db.Owners.Find(id);
            if (owners == null)
            {
                return NotFound();
            }

            db.Owners.Remove(owners);
            db.SaveChanges();

            return Ok(owners);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OwnersExists(int id)
        {
            return db.Owners.Count(e => e.Id == id) > 0;
        }
    }
}