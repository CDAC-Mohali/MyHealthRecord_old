using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PHRMS.ViewModels
{
    public class MedicationViewModel
    {
        public MedicationViewModel()
        {
            SourceId = Source.WebApp;
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int sno { get; set; }
        public int MedicineType { get; set; }
        public string MedicineName { get; set; }
        public bool TakingMedicine { get; set; }
        public string strTakingMedicine { get; set; }
        public DateTime PrescribedDate { get; set; }
        public string strPrescribedDate { get; set; }
        public DateTime DispensedDate { get; set; }
        public string strDispensedDate { get; set; }
        public string Provider { get; set; }
        public int Route { get; set; }
        public string strRoute { get; set; }
        public string Strength { get; set; }
        public int DosValue { get; set; }
        public string strDosValue { get; set; }
        public int DosUnit { get; set; }
        public string strDosUnit { get; set; }
        public int Frequency { get; set; }
        public string strFrequency { get; set; }
        public string LabelInstructions { get; set; }
        public string Notes { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<string> lstFiles { get; set; }
        public List<FileViewModel> lstFileModels { get; set; }
        public int SourceId { get; set; }
        public string MedicationName { get; set; }
    }

    public class MedicationMasterViewModel
    {
        public int Id { get; set; }
        public string MedicineName { get; set; }
        public string RXCUI { get; set; }
        public string RXAUI { get; set; }

    }

    public class MedicineRouteViewModel
    {
        public int Id { get; set; }
        public string Route { get; set; }
    }

    public class DosageValueViewModel
    {
        public int Id { get; set; }
        public string DosValue { get; set; }
    }

    public class DosageUnitViewModel
    {
        public int Id { get; set; }
        public string DosUnit { get; set; }
    }

    public class FrequencyTakenViewModel
    {
        public int Id { get; set; }
        public string Frequency { get; set; }
    }

    
}
