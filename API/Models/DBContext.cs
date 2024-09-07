using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class DBContext : DbContext
    {
        public DbSet<Patient> patients { get; set; }
        public DbSet<Doctor> doctors { get; set; }
    }
}