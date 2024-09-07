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
using API.Models;

namespace API.Controllers
{
    public class DoctorsController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/Doctors
        public IHttpActionResult GetDoctors(int page, int pageSize, string sortBy, bool isAscending)
        {
            var doctors = db.doctors
                .Include(d => d.Cabinet)
                .Include(d => d.Specialization)
                .Include(d => d.Uchastok)
                .Select(d => new
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    CabinetNumber = d.Cabinet.Number,
                    SpecializationName = d.Specialization.Name,
                    UchastokNumber = d.Uchastok.Number
                });

            // Сортировка
            switch (sortBy.ToLower())
            {
                case "id":
                    doctors = isAscending ? doctors.OrderBy(d => d.Id) : doctors.OrderByDescending(d => d.Id);
                    break;
                case "fullname":
                    doctors = isAscending ? doctors.OrderBy(d => d.FullName) : doctors.OrderByDescending(d => d.FullName);
                    break;
                case "cabinetnumber":
                    doctors = isAscending ? doctors.OrderBy(d => d.CabinetNumber) : doctors.OrderByDescending(d => d.CabinetNumber);
                    break;
                case "specializationname":
                    doctors = isAscending ? doctors.OrderBy(d => d.SpecializationName) : doctors.OrderByDescending(d => d.SpecializationName);
                    break;
                case "uchastoknumber":
                    doctors = isAscending ? doctors.OrderBy(d => d.UchastokNumber) : doctors.OrderByDescending(d => d.UchastokNumber);
                    break;
                default:
                    doctors = doctors.OrderBy(d => d.Id);
                    break;
            }

            // Постраничный возврат данных
            var skip = (page - 1) * pageSize;
            var take = pageSize;
            var totalRecords = doctors.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var result = doctors.Skip(skip).Take(take).ToList();
            return Ok(result);
        }

        // GET: api/Doctors/5
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult GetDoctor(int id)
        {
            Doctor doctor = db.doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        // PUT: api/Doctors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDoctor(int id, Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctor.Id)
            {
                return BadRequest();
            }

            db.Entry(doctor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
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

        // POST: api/Doctors
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult PostDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.doctors.Add(doctor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctor.Id }, doctor);
        }

        // DELETE: api/Doctors/5
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult DeleteDoctor(int id)
        {
            Doctor doctor = db.doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            db.doctors.Remove(doctor);
            db.SaveChanges();

            return Ok(doctor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoctorExists(int id)
        {
            return db.doctors.Count(e => e.Id == id) > 0;
        }
    }
}