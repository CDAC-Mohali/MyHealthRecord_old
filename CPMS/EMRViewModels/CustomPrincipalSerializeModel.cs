using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMRViewModels
{
    [Serializable()]
    public class CustomPrincipalSerializeModel
    {
        public CustomPrincipalSerializeModel()
        {
            IsDetailActive = false;
        }
        public Guid DocId { get; set; }
        public Guid PhrmsUserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string StrDOB { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public bool IsDetailActive { get; set; }
        public string DoctorName { get; set; }
        public string DoctorPhone { get; set; }
        public string ImgPath { get; set; }
        public long DocPatientId { get; set; }
    }
}