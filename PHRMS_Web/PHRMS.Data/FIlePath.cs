using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PHRMS.Data
{
    public enum FileType
    {
        ProfilePic = 1, Photo, ePrescription, LabReport, Medication, Procedure, Feedback
    }
    public class FilePath
    {
        public int FilePathId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public bool DeleteFlag { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Users User { get; set; }
        public Guid RecId { get; set; }
    }
}
