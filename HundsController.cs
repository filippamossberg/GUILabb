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
    public class HundsController : ApiController
    {
        private HundModel db = new HundModel();

        // GET: api/Hunds
        public IQueryable<Hunds> GetHunds()
        {
            return db.Hunds;
        }

        // GET: api/Hunds/5
        [ResponseType(typeof(Hunds))]
        public IHttpActionResult GetHunds(int id)
        {
            Hunds hunds = db.Hunds.Find(id);
            if (hunds == null)
            {
                return NotFound();
            }

            return Ok(hunds);
        }

        // PUT: api/Hunds/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHunds(int id, Hunds hunds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hunds.Id)
            {
                return BadRequest();
            }

            //dbHund är den korrekta entity från databasen, ska inte använda hunds.
            var dbHund = db.Hunds.Find(hunds.Id);
            dbHund.Name = hunds.Name;
            dbHund.Ras = hunds.Ras;

            db.Entry(dbHund).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HundsExists(id))
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
        // POST: api/Hunds
        [ResponseType(typeof(Hunds))]
        public IHttpActionResult PostHunds(Hunds hunds)
        {
            //välj rätt owner från Databasen om den finns
            List<Owners> hundOwners = new List<Owners>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var owner in hunds.Owners)
            {
                if (owner.Id == 0)
                {
                    hundOwners.Add(owner);
                }
                else
                {
                    var dbOwner = db.Owners.Find(owner.Id);
                    hundOwners.Add(dbOwner); 
                }
            }
            hunds.Owners = hundOwners;
            db.Hunds.Add(hunds);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = hunds.Id }, hunds);
        }

        // DELETE: api/Hunds/5
        [ResponseType(typeof(Hunds))]
        public IHttpActionResult DeleteHunds(int id)
        {
            Hunds hunds = db.Hunds.Find(id);

            if (hunds == null)
            {
                return NotFound();
            }

            db.Hunds.Remove(hunds);
            db.SaveChanges();

            return Ok(hunds);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HundsExists(int id)
        {
            return db.Hunds.Count(e => e.Id == id) > 0;
        }
    }
}