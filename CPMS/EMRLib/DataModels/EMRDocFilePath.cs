using EMRViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMRLib.DataModels
{
   
    public class EMRDocFilePath
    {
        [Key]
        public int FileId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid RecId { get; set; }
        public string filePath { get; set; }

        public Guid DocId { get; set; }
        public virtual Doctor Doctor { get; set; }

    }
}
