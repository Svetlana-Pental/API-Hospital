using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int CabinetId { get; set; }
        public int SpecializationId { get; set; }
        public int UchastokId { get; set; }
        [JsonIgnore]
        public virtual Cabinet Cabinet { get; set; }
        [JsonIgnore]
        public virtual Specialization Specialization { get; set; }
        [JsonIgnore]
        public virtual Uchastok Uchastok { get; set; }
    }
}