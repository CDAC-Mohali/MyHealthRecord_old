using System;

namespace PHRMS.ViewModels
{
    public class DashboardAnalyticsViewModel
    {

        public float GlucoseLatest { get; set; }

        public float BPLatestSys { get; set; }

        public float BPLatestDia { get; set; }

        public float BloodGlucoseCount { get; set; }

        public float HeartRateCount { get; set; }

        public float BloodPressureCount { get; set; }

        public int LabsCount { get; set; }

        public int AllergiesCount { get; set; }

        public int MedicationsCount { get; set; }

        public int ImmunizationsCount { get; set; }

        public int ProceduresCount { get; set; }

        public int ConditionsCount { get; set; }
        public string LastBPCollectionDate { get; set; }
        public string LastGlucoseCollectionDate { get; set; }

        //Wellness Activity related fields
        public string LastActivityCollectionDate { get; set; }
        public string LastActivityType { get; set; }
        public decimal LastActivityDistance { get; set; }

    }
    public class BPViewModel
    {
        public float? Systolic { get; set; }
        public float? Diastolic { get; set; }
        public float? Pulse { get; set; }
        public string Date { get; set; }
    }
}