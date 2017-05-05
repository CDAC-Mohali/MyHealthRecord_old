using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PHRMS.ViewModels
{
    public class ProcedureMasterViewModel
    {
        public int Id { get; set; }
        public string ProcedureName { get; set; }
    }

    public class ProceduresViewModel
    {
        public ProceduresViewModel()
        {
            SourceId = Source.WebApp;
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int ProcedureType { get; set; }
        public string ProcedureName { get; set; }
        public DateTime StartDate { get; set; }
        public string strStartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string strEndDate { get; set; }
        public string Comments { get; set; }
        public string SurgeonName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int sno { get; set; }
        public bool DeleteFlag { get; set; }
        public List<string> lstFiles { get; set; }
        public List<FileViewModel> lstFileModels { get; set; }
        public int SourceId { get; set; }
    }
}
