using EMRViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMRLib.DataModels
{
    public enum FileTypes
    {
        ProfilePic = 1, Photo
    }
    public class EMRFiles
    {
        [Key]
        public int FileId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        public FileTypes FileType { get; set; }
        public bool DeleteFlag { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid RecId { get; set; }
    }
}
