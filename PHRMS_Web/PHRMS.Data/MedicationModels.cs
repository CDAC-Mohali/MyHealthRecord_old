using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PHRMS.Data
{

    [Table("MedicationMaster")]
    public class MedicationMaster
    {
        [Key]
        public int Id { get; set; }
        public string MedicineName { get; set; }
        public string RXCUI { get; set; }
        public string RXAUI { get; set; }
    }

    [Table("MedicineRoute")]
    public class MedicineRoute
    {
        [Key]
        public int Id { get; set; }
        public string Route { get; set; }
    }

    [Table("DosageValue")]
    public class DosageValue
    {
        [Key]
        public int Id { get; set; }
        public string DosValue { get; set; }
    }
    [Table("DosageUnit")]
    public class DosageUnit
    {
        [Key]
        public int Id { get; set; }
        public string DosUnit { get; set; }
    }
    [Table("FrequencyTaken")]
    public class FrequencyTaken
    {
        [Key]
        public int Id { get; set; }
        public string Frequency { get; set; }
    }
    [Table("Medication")]
    public class Medication
    {

        [NotMapped]
        public string strFrequency;
        [NotMapped]
        public string strDosValue;
        [NotMapped]
        public string strDosUnit;
        [NotMapped]
        public string strRoute;

        public Medication()
        {
            SourceId = 2;
        }
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int MedicineType { get; set; }
        public bool TakingMedicine { get; set; }
        [Required]
        public DateTime PrescribedDate { get; set; }
        [Required]
        public DateTime DispensedDate { get; set; }
        public string Provider { get; set; }
        public int Route { get; set; }
        public string Strength { get; set; }
        public int DosValue { get; set; }
        public int DosUnit { get; set; }
        public int Frequency { get; set; }
        public string LabelInstructions { get; set; }
        public string Quantity { get; set; }
        public string Refills { get; set; }
        public string DaysSupply { get; set; }
        public string FillingPharmacy { get; set; }
        public string Notes { get; set; }
        public bool HideItem { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SourceId { get; set; }
        public Guid? PrescriptionId { get; set; }
        [NotMapped]
        public string MedicationName { get; internal set; }
    }

    public class Doctor
    {
        [Key]
        [Required]
        public Guid docid { get; set; }
        public string name { get; set; }
        public DateTime date_of_birth { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public string qualification_set { get; set; }
        public bool delete_flag { get; set; }
        public DateTime request_time { get; set; }
        public bool email_flag { get; set; }
        public string password { get; set; }
        public int IsApproved { get; set; }
        public string Gender { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int OTP { get; set; }
      //  public virtual List<Places_of_Practice> Places_of_Practice { get; set; }
    }

    [Table("DocPatientDetails")]
    public class DocPatientDetails
    {
        [Key]
        public long DocPatientId { get; set; }
        public Guid DocId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string AadhaarNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City_Vill_Town { get; set; }
        public string District { get; set; }
        public int? State { get; set; }
        public Guid? PHRMSUserId { get; set; }
    }
    [Table("Advice")]
    public class Advice
    {
        [Key]
        [Required]
        public long Id { get; set; }
        public Guid PrescriptionId { get; set; }
        public int ModuleId { get; set; }
        public int TypeId { get; set; }
        [NotMapped]
        public string Name { get; set; }
    }
    [Table("Appointment_Fees")]
    public class Appointment_Fees
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid DocId { get; set; }
        public DateTime Date { get; set; }
        public short Hours { get; set; }
        public short Mins { get; set; }
        [MaxLength(2)]
        public string meridiem { get; set; }
        public decimal NetFee { get; set; }
        public bool Visited { get; set; }
        //public bool IsApproved { get; set; }
        //public DateTime ApprovedDate { get; set; }
        //public char Gender { get; set; }
        public Guid PrescriptionId { get; set; }

        [ForeignKey("DocId")]
        public virtual Doctor doctors { get; set; }
        [ForeignKey("UserId")]
        public virtual Users users { get; set; }
    }
}
