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
    public class PatientsController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/Patients
        public IHttpActionResult GetPatients(int page, int pageSize, string sortBy, bool isAscending)
        {
            var patients = db.patients
                .Include(p => p.Uchastok)
                .Select(p => new
                {
                    Id = p.Id,
                    LastName = p.LastName,
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    Address = p.Address,
                    BirthDate = p.BirthDate,
                    Gender = p.Gender,
                    UchastokName = p.Uchastok.Number
                });

            // Сортировка
            switch (sortBy.ToLower())
            {
                case "id":
                    patients = isAscending ? patients.OrderBy(p => p.Id) : patients.OrderByDescending(p => p.Id);
                    break;
                case "lastname":
                    patients = isAscending ? patients.OrderBy(p => p.LastName) : patients.OrderByDescending(p => p.LastName);
                    break;
                case "firstname":
                    patients = isAscending ? patients.OrderBy(p => p.FirstName) : patients.OrderByDescending(p => p.FirstName);
                    break;
                case "middlename":
                    patients = isAscending ? patients.OrderBy(p => p.MiddleName) : patients.OrderByDescending(p => p.MiddleName);
                    break;
                case "address":
                    patients = isAscending ? patients.OrderBy(p => p.Address) : patients.OrderByDescending(p => p.Address);
                    break;
                case "birthdate":
                    patients = isAscending ? patients.OrderBy(p => p.BirthDate) : patients.OrderByDescending(p => p.BirthDate);
                    break;
                case "gender":
                    patients = isAscending ? patients.OrderBy(p => p.Gender) : patients.OrderByDescending(p => p.Gender);
                    break;
                case "uchastokname":
                    patients = isAscending ? patients.OrderBy(p => p.UchastokName) : patients.OrderByDescending(p => p.UchastokName);
                    break;
                default:
                    patients = patients.OrderBy(p => p.Id);
                    break;
            }

            // Постраничный возврат данных
            var skip = (page - 1) * pageSize;
            var take = pageSize;

            var result = patients.Skip(skip).Take(take).ToList();

            var totalRecords = result.Count(); 
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return Ok(result);
        }

        // GET: api/Patients/5
        [ResponseType(typeof(Patient))]
        public IHttpActionResult GetPatient(int id)
        {
            Patient patient = db.patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        // PUT: api/Patients/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.Id)
            {
                return BadRequest();
            }

            db.Entry(patient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        // POST: api/Patients
        [ResponseType(typeof(Patient))]
        public IHttpActionResult PostPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.patients.Add(patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patient.Id }, patient);
        }

        // DELETE: api/Patients/5
        [ResponseType(typeof(Patient))]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient patient = db.patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.patients.Remove(patient);
            db.SaveChanges();

            return Ok(patient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.patients.Count(e => e.Id == id) > 0;
        }
    }
}