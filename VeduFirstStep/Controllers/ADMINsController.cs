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
using VeduFirstStep.Models;

namespace VeduFirstStep.Controllers
{
    public class ADMINsController : ApiController
    {
        private DBModelNew db = new DBModelNew();

        // GET: api/ADMINs
        public IQueryable<ADMIN> GetADMINs()
        {
            return db.ADMINs;
        }

        // GET: api/ADMINs/5
        [ResponseType(typeof(ADMIN))]
        public IHttpActionResult GetADMIN(int id)
        {
            ADMIN aDMIN = db.ADMINs.Find(id);
            if (aDMIN == null)
            {
                return NotFound();
            }

            return Ok(aDMIN);
        }

        [Route("api/ADMINs/Users/{name}")]
        public String Get(string name)
        {
            using (var ctx = db)
            {
                var studentUserList = ctx.Database.SqlQuery<string>(@"SELECT S.Sname 
                                                                    FROM [dbo].[Student] as S")
                                     .ToList<string>();
                if(studentUserList.Contains(name))
                {
                    return "student";
                }
                var teacherUserList = ctx.Database.SqlQuery<string>(@"SELECT T.Tname 
                                                                    FROM [dbo].[Teacher] as T")
                                     .ToList<string>();
                if (teacherUserList.Contains(name))
                {
                    return "teacher";
                }
                var adminList = ctx.Database.SqlQuery<string>(@"SELECT A.Aname 
                                                               FROM [dbo].[ADMIN] as A")
                                     .ToList<string>();
                if (adminList.Contains(name))
                {
                    return "admin";
                }
                return "notValid";
            }
        }

        // PUT: api/ADMINs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutADMIN(int id, ADMIN aDMIN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aDMIN.AID)
            {
                return BadRequest();
            }

            db.Entry(aDMIN).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ADMINExists(id))
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

        // POST: api/ADMINs
        [ResponseType(typeof(ADMIN))]
        public IHttpActionResult PostADMIN(ADMIN aDMIN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ADMINs.Add(aDMIN);
            
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ADMINExists(aDMIN.AID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aDMIN.AID }, aDMIN);
        }

        // DELETE: api/ADMINs/5
        [ResponseType(typeof(ADMIN))]
        public IHttpActionResult DeleteADMIN(int id)
        {
            ADMIN aDMIN = db.ADMINs.Find(id);
            if (aDMIN == null)
            {
                return NotFound();
            }

            db.ADMINs.Remove(aDMIN);
            db.SaveChanges();

            return Ok(aDMIN);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ADMINExists(int id)
        {
            return db.ADMINs.Count(e => e.AID == id) > 0;
        }
    }
}