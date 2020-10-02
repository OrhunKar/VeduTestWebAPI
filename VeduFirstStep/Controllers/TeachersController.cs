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
    public class TeachersController : ApiController
    {
        private DBModelNew db = new DBModelNew();

        // GET: api/Teachers
        public IQueryable<Teacher> GetTeachers()
        {
            return db.Teachers;
        }

        // GET: api/Teachers/5
        [ResponseType(typeof(Teacher))]
        public IHttpActionResult GetTeacher(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(teacher);
        }

        [Route("api/Teachers/Courses")]
        public List<Course> Get()
        {
            using (var ctx = db)
            {
                var courseList = ctx.Database.SqlQuery<Course>(@"SELECT dbo.Course.*
                                                                FROM dbo.Course 
                                                                INNER JOIN dbo.Teacher ON dbo.Course.CID = dbo.Teacher.CourseID 
                                                                Where Tname = 'EnglishTestTeacher'")
                                     .ToList<Course>();
                return courseList;
            }
        }

        [Route("api/Teachers/Students/{CID}")]
        public List<Student> Get(int CID)
        {
            using (var ctx = db)
            {
                var studentList = ctx.Database.SqlQuery<Student>(@"SELECT dbo.Student.*
                                                                FROM dbo.Student 
                                                                INNER JOIN dbo.Course ON dbo.Student.CourseID = dbo.Course.CID
                                                                Where CID = " + CID)
                                     .ToList<Student>();
                return studentList;
            }
        }

        // PUT: api/Teachers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTeacher(int id, Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teacher.TID)
            {
                return BadRequest();
            }

            db.Entry(teacher).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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

        // POST: api/Teachers
        [ResponseType(typeof(Teacher))]
        public IHttpActionResult PostTeacher(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teachers.Add(teacher);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = teacher.TID }, teacher);
        }

        // DELETE: api/Teachers/5
        [ResponseType(typeof(Teacher))]
        public IHttpActionResult DeleteTeacher(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return NotFound();
            }

            db.Teachers.Remove(teacher);
            db.SaveChanges();

            return Ok(teacher);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeacherExists(int id)
        {
            return db.Teachers.Count(e => e.TID == id) > 0;
        }
    }
}