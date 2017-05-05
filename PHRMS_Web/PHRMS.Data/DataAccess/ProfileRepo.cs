using AutoMapper;
using PHRMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PHRMS.Data.DataAccess
{
    public partial class PHRMSRepo
    {


        //Insert Method for Personal Information Table of Profile Section

        public async Task<PersonalViewModel> PersonalInfoInsert(PersonalViewModel oProfileInsert)
        {
            try
            {
                Mapper.CreateMap<PersonalViewModel, PersonalInformation>();
                PersonalInformation oPersonalInformation = Mapper.Map<PersonalInformation>(oProfileInsert);
                oPersonalInformation.Id = Guid.NewGuid();
                _db.PersonalInformation.Add(oPersonalInformation);
                int res = await _db.SaveChangesAsync();
                if (res > 0)
                {
                    oProfileInsert.Id = oPersonalInformation.Id;
                }
                oProfileInsert.StatusCode = res;
            }
            catch (Exception)
            {

                throw;
            }
            return oProfileInsert;
        }

        public async Task<PersonalViewModel> PersonalInfoUpdate(PersonalViewModel oProfileInsert)
        {
            try
            {
                var record = _db.PersonalInformation.FirstOrDefault(m => m.UserId.Equals(oProfileInsert.UserId));

                if (record != null)
                {
                    record.FirstName = oProfileInsert.FirstName;
                    record.LastName = oProfileInsert.LastName;
                    record.Uhid = oProfileInsert.Uhid;
                    record.AddressLine1 = oProfileInsert.AddressLine1;
                    record.AddressLine2 = oProfileInsert.AddressLine2;
                    record.BloodType = oProfileInsert.BloodType;
                    record.Cell_Phone = oProfileInsert.Cell_Phone;
                    record.City_Vill_Town = oProfileInsert.City_Vill_Town;
                    record.DiffAbled = oProfileInsert.DiffAbled;
                    record.DAbilityType = oProfileInsert.DiffAbledType;
                    record.District = oProfileInsert.District;
                    record.DOB = oProfileInsert.DOB;
                    record.Email = oProfileInsert.Email;
                    record.Ethinicity = oProfileInsert.Ethinicity;
                    record.Gender = oProfileInsert.Gender;
                    record.Home_Phone = oProfileInsert.Home_Phone;
                    record.Pin = oProfileInsert.Pin;
                    record.State = oProfileInsert.State;
                    record.Work_Phone = oProfileInsert.Work_Phone;
                    int res = await _db.SaveChangesAsync();
                    UpdateNameInReg(oProfileInsert.UserId, oProfileInsert.FirstName, oProfileInsert.LastName);
                    oProfileInsert.StatusCode = res == 0 ? 1 : res;
                }
                else
                    oProfileInsert = await PersonalInfoInsert(oProfileInsert);
            }
            catch (Exception)
            {

                throw;
            }
            return oProfileInsert;
        }

        public void UpdateNameInReg(Guid userId, string strFirstName, string strLastName)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(m => m.UserId.Equals(userId));
                if (user != null)
                {
                    user.FirstName = strFirstName;
                    user.LastName = strLastName;
                    _db.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Insert Method for Emergency Information Table of Profile Section

        public async Task<EmergencyViewModel> EmergencyInsert(EmergencyViewModel oEmergencyInsert)
        {
            try
            {
                Mapper.CreateMap<EmergencyViewModel, EmergencyInformation>();
                EmergencyInformation oEmergencyInformation = Mapper.Map<EmergencyInformation>(oEmergencyInsert);
                oEmergencyInformation.Id = Guid.NewGuid();
                _db.EmergencyInformation.Add(oEmergencyInformation);
                int res = await _db.SaveChangesAsync();
                if (res > 0)
                {
                    oEmergencyInsert.Id = oEmergencyInformation.Id;
                }
                oEmergencyInsert.StatusCode = res;
                oEmergencyInformation = null;
            }
            catch (Exception)
            {

                throw;
            }
            return oEmergencyInsert;

        }

        public async Task<EmergencyViewModel> EmergencyInfoUpdate(EmergencyViewModel oEmergencyInsert)
        {
            try
            {
                //var record = _db.EmergencyInformation.FirstOrDefault(m => m.UserId.Equals(oEmergencyInsert.UserId));
                var record = _db.EmergencyInformation.Where(m => m.UserId.Equals(oEmergencyInsert.UserId)).FirstOrDefault();

                if (record != null)
                {
                    record.Primary_Emergency_Contact = oEmergencyInsert.Primary_Emergency_Contact;
                    record.PC_Relationship = oEmergencyInsert.PC_Relationship;
                    record.PC_AddressLine1 = oEmergencyInsert.PC_AddressLine1;
                    record.PC_AddressLine2 = oEmergencyInsert.PC_AddressLine2;
                    record.PC_City_Vill_Town = oEmergencyInsert.PC_City_Vill_Town;
                    record.PC_District = oEmergencyInsert.PC_District;
                    record.PC_State = oEmergencyInsert.PC_State;
                    record.PC_Pin = oEmergencyInsert.PC_Pin;
                    record.PC_Phone1 = oEmergencyInsert.PC_Phone1;
                    record.PC_Phone2 = oEmergencyInsert.PC_Phone2;
                    record.Secondary_Emergency_Contact = oEmergencyInsert.Secondary_Emergency_Contact;
                    record.SC_Relationship = oEmergencyInsert.SC_Relationship;
                    record.SC_AddressLine1 = oEmergencyInsert.SC_AddressLine1;
                    record.SC_AddressLine2 = oEmergencyInsert.SC_AddressLine2;
                    record.SC_City_Vill_Town = oEmergencyInsert.SC_City_Vill_Town;
                    record.SC_District = oEmergencyInsert.SC_District;
                    record.SC_State = oEmergencyInsert.SC_State;
                    record.SC_Pin = oEmergencyInsert.SC_Pin;
                    record.SC_Phone1 = oEmergencyInsert.SC_Phone1;
                    record.SC_Phone2 = oEmergencyInsert.SC_Phone2;

                    int res = await _db.SaveChangesAsync();
                    oEmergencyInsert.StatusCode = res == 0 ? 1 : res;
                }
                else
                    oEmergencyInsert = await EmergencyInsert(oEmergencyInsert);
            }
            catch (Exception ex)
            {

                throw;
            }
            return oEmergencyInsert;
        }

        //Insert Method for Employer Information Table of Profile Section

        public async Task<EmployerViewModel> EmployerInsert(EmployerViewModel oEmployerInsert)
        {

            try
            {
                Mapper.CreateMap<EmployerViewModel, EmployerInformation>();
                EmployerInformation oEmployerInformation = Mapper.Map<EmployerInformation>(oEmployerInsert);
                oEmployerInformation.Id = Guid.NewGuid();
                _db.EmployerInformation.Add(oEmployerInformation);
                int res = await _db.SaveChangesAsync();
                if (res > 0)
                {
                    oEmployerInsert.Id = oEmployerInformation.Id;
                }
                oEmployerInsert.StatusCode = res;
            }
            catch (Exception)
            {

                throw;
            }
            return oEmployerInsert;
        }


        public async Task<EmployerViewModel> EmployerInfoUpdate(EmployerViewModel oEmployerViewModel)
        {
            try
            {
                var record = _db.EmployerInformation.FirstOrDefault(m => m.UserId.Equals(oEmployerViewModel.UserId));

                if (record != null)
                {
                    record.EmployerName = oEmployerViewModel.EmployerName;
                    record.EmpAddressLine1 = oEmployerViewModel.EmpAddressLine1;
                    record.EmpAddressLine2 = oEmployerViewModel.EmpAddressLine2;
                    record.EmpCity_Vill_Town = oEmployerViewModel.EmpCity_Vill_Town;
                    record.EmpDistrict = oEmployerViewModel.EmpDistrict;
                    record.EmpState = oEmployerViewModel.EmpState;
                    record.EmpPin = oEmployerViewModel.EmpPin;
                    record.EmployerPhone = oEmployerViewModel.EmployerPhone;
                    record.EmployerOccupation = oEmployerViewModel.EmployerOccupation;
                    record.CUG = oEmployerViewModel.CUG;

                    int res = await _db.SaveChangesAsync();
                    oEmployerViewModel.StatusCode = res == 0 ? 1 : res;
                }
                else
                    oEmployerViewModel = await EmployerInsert(oEmployerViewModel);
            }
            catch (Exception)
            {

                throw;
            }
            return oEmployerViewModel;
        }

        //Insert Method for Insurance Information Table of Profile Section

        public async Task<InsuranceViewModel> InsuranceInsert(InsuranceViewModel oInsuranceViewModel)
        {

            try
            {
                Mapper.CreateMap<InsuranceViewModel, InsuranceInformation>();
                InsuranceInformation oInsuranceInformation = Mapper.Map<InsuranceInformation>(oInsuranceViewModel);
                oInsuranceInformation.Id = Guid.NewGuid();
                _db.InsuranceInformation.Add(oInsuranceInformation);
                int res = await _db.SaveChangesAsync();
                if (res > 0)
                {
                    oInsuranceViewModel.Id = oInsuranceInformation.Id;
                }
                oInsuranceViewModel.StatusCode = res;
                oInsuranceInformation = null;
            }
            catch (Exception)
            {

                throw;
            }
            return oInsuranceViewModel;
        }

        public async Task<InsuranceViewModel> InsuranceInfoUpdate(InsuranceViewModel oInsuranceViewModel)
        {
            try
            {
                var record = _db.InsuranceInformation.FirstOrDefault(m => m.UserId.Equals(oInsuranceViewModel.UserId));

                if (record != null)
                {
                    record.Insu_Org_Name = oInsuranceViewModel.Insu_Org_Name;
                    record.Insu_Org_Phone = oInsuranceViewModel.Insu_Org_Phone;
                    record.Insu_Org_Grp_Num = oInsuranceViewModel.Insu_Org_Grp_Num;
                    record.ValidTill = oInsuranceViewModel.ValidTill;

                    int res = await _db.SaveChangesAsync();
                    oInsuranceViewModel.StatusCode = res == 0 ? 1 : res;
                    record = null;
                }
                else
                    oInsuranceViewModel = await InsuranceInsert(oInsuranceViewModel);
            }
            catch (Exception)
            {

                throw;
            }
            return oInsuranceViewModel;
        }

        //Insert Method for Legal Information Table of Profile Section

        public async Task<LegalViewModel> LegalInsert(LegalViewModel oLegalViewModel)
        {
            try
            {
                Mapper.CreateMap<LegalViewModel, LegalInformation>();
                LegalInformation oLegalInformation = Mapper.Map<LegalInformation>(oLegalViewModel);
                oLegalInformation.Id = Guid.NewGuid();
                _db.LegalInformation.Add(oLegalInformation);
                int res = await _db.SaveChangesAsync();
                if (res > 0)
                {
                    oLegalViewModel.Id = oLegalInformation.Id;
                }
                oLegalViewModel.StatusCode = res;
            }
            catch (Exception)
            {

                throw;
            }
            return oLegalViewModel;
        }

        public async Task<LegalViewModel> LegalInfoUpdate(LegalViewModel oLegalViewModel)
        {
            try
            {
                var record = _db.LegalInformation.FirstOrDefault(m => m.UserId.Equals(oLegalViewModel.UserId));

                if (record != null)
                {
                    record.Power_Attorney = oLegalViewModel.Power_Attorney;
                    record.PA_ContactPerson = oLegalViewModel.PA_ContactPerson;
                    record.PA_DateInit = oLegalViewModel.PA_DateInit;
                    record.PA_PhoneNo = oLegalViewModel.PA_PhoneNo;
                    record.Care_Directive = oLegalViewModel.Care_Directive;
                    record.CD_ContactPerson = oLegalViewModel.PA_PhoneNo;
                    record.CD_DateInit = oLegalViewModel.CD_DateInit;
                    record.CD_PhoneNo = oLegalViewModel.PA_PhoneNo;
                    record.Living_Will = oLegalViewModel.Living_Will;
                    record.LW_ContactPerson = oLegalViewModel.PA_PhoneNo;
                    record.LW_DateInit = oLegalViewModel.LW_DateInit;
                    record.LW_PhoneNo = oLegalViewModel.LW_PhoneNo;

                    int res = await _db.SaveChangesAsync();
                    oLegalViewModel.StatusCode = res == 0 ? 1 : res;
                    record = null;
                }
                else
                    oLegalViewModel = await LegalInsert(oLegalViewModel);
            }
            catch (Exception)
            {

                throw;
            }
            return oLegalViewModel;
        }

        //Insert Method for Preferences Table of Profile Section

        public async Task<PreferencesViewModel> PreferencesInsert(PreferencesViewModel oPreferencesInsert)
        {

            try
            {
                Mapper.CreateMap<PreferencesViewModel, Preferences>();
                Preferences oPreferences = Mapper.Map<Preferences>(oPreferencesInsert);
                oPreferences.Id = Guid.NewGuid();
                _db.Preferences.Add(oPreferences);
                int res = await _db.SaveChangesAsync();
                if (res > 0)
                {
                    oPreferencesInsert.Id = oPreferences.Id;
                }
                oPreferencesInsert.StatusCode = res;
            }
            catch (Exception)
            {

                throw;
            }
            return oPreferencesInsert;
        }

        public async Task<PreferencesViewModel> PreferencesInfoUpdate(PreferencesViewModel oPreferencesViewModel)
        {
            try
            {
                var record = _db.Preferences.FirstOrDefault(m => m.UserId.Equals(oPreferencesViewModel.UserId));

                if (record != null)
                {
                    record.Pref_Hosp = oPreferencesViewModel.Pref_Hosp;
                    record.Prim_Care_Prov = oPreferencesViewModel.Prim_Care_Prov;
                    record.Special_Needs = oPreferencesViewModel.Special_Needs;

                    int res = await _db.SaveChangesAsync();
                    if (res > 0)
                    {
                        oPreferencesViewModel.Id = record.Id;
                    }
                    oPreferencesViewModel.StatusCode = res == 0 ? 1 : res;
                    record = null;
                }
                else
                    oPreferencesViewModel = await PreferencesInsert(oPreferencesViewModel);
            }
            catch (Exception)
            {

                throw;
            }
            return oPreferencesViewModel;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////



        public PersonalViewModel GetPersonalInformation(Guid userId)
        {
            PersonalViewModel oPersonalViewModel = null;
            try
            {
                var record = _db.PersonalInformation.FirstOrDefault(m => m.UserId.Equals(userId));
                if (record != null)
                {
                    Mapper.CreateMap<PersonalInformation, PersonalViewModel>();
                    oPersonalViewModel = Mapper.Map<PersonalViewModel>(record);
                    oPersonalViewModel.DiffAbledType = record.DAbilityType;
                    var user = GetInitialPersonalDetails(userId);
                    if (oPersonalViewModel.FirstName == "" || oPersonalViewModel.FirstName == null)
                        oPersonalViewModel.FirstName = user.FirstName;
                    if (oPersonalViewModel.LastName == "" || oPersonalViewModel.LastName == null)
                        oPersonalViewModel.LastName = user.LastName;
                    if (oPersonalViewModel.Uhid == "" || oPersonalViewModel.Uhid == null)
                        oPersonalViewModel.Uhid = user.Uhid;
                    if (oPersonalViewModel.Cell_Phone == "" || oPersonalViewModel.Cell_Phone == null)
                        oPersonalViewModel.Cell_Phone = user.Cell_Phone;
                    if (oPersonalViewModel.Email == "" || oPersonalViewModel.Email == null)
                        oPersonalViewModel.Email = user.Email;

                    record = null;
                }
                else
                    oPersonalViewModel = GetInitialPersonalDetails(userId);
            }
            catch (Exception)
            {

            }
            return oPersonalViewModel;
        }

        public PersonalViewModel GetInitialPersonalDetails(Guid userId)
        {
            PersonalViewModel oPersonalViewModel = null;
            try
            {
                var user = _db.Users.FirstOrDefault(m => m.UserId.Equals(userId));
                if (user != null)
                {
                    oPersonalViewModel = new PersonalViewModel();
                    oPersonalViewModel.FirstName = user.FirstName;
                    oPersonalViewModel.LastName = user.LastName;
                    oPersonalViewModel.Uhid = user.AadhaarNo;
                    oPersonalViewModel.Cell_Phone = user.MobileNo;
                    oPersonalViewModel.Email = user.Email;
                    user = null;
                }
            }
            catch (Exception)
            {

            }
            return oPersonalViewModel;
        }

        public EmergencyViewModel GetEmergencyInformation(Guid userId)
        {
            EmergencyViewModel oEmergencyViewModel = null;
            try
            {
                var record = _db.EmergencyInformation.FirstOrDefault(m => m.UserId.Equals(userId));
                if (record != null)
                {
                    Mapper.CreateMap<EmergencyInformation, EmergencyViewModel>();
                    oEmergencyViewModel = Mapper.Map<EmergencyViewModel>(record);
                }
            }
            catch (Exception)
            {

            }
            return oEmergencyViewModel;
        }

        public EmployerViewModel GetInitialEmployerDetails(Guid userId)
        {
            EmployerViewModel oEmployerViewModel = null;
            try
            {
                oEmployerViewModel = new EmployerViewModel();
                var user = _db.Users.FirstOrDefault(m => m.UserId.Equals(userId));
                if (user != null)
                {
                    oEmployerViewModel.CUG = user.CUG;
                    user = null;
                }

            }
            catch (Exception)
            {

            }
            return oEmployerViewModel;
        }

        public EmployerViewModel GetEmployerInformation(Guid userId)
        {
            EmployerViewModel oEmployerViewModel = null;
            try
            {
                var record = _db.EmployerInformation.FirstOrDefault(m => m.UserId.Equals(userId));
                if (record != null)
                {
                    Mapper.CreateMap<EmployerInformation, EmployerViewModel>();
                    oEmployerViewModel = Mapper.Map<EmployerViewModel>(record);
                }
                else
                    oEmployerViewModel = GetInitialEmployerDetails(userId);
            }
            catch (Exception)
            {

            }
            return oEmployerViewModel;
        }

        public InsuranceViewModel GetInsuranceInformation(Guid userId)
        {
            InsuranceViewModel oInsuranceViewModel = null;
            try
            {
                var record = _db.InsuranceInformation.FirstOrDefault(m => m.UserId.Equals(userId));

                if (record != null)
                {
                    Mapper.CreateMap<InsuranceInformation, InsuranceViewModel>();
                    oInsuranceViewModel = Mapper.Map<InsuranceViewModel>(record);
                }

            }
            catch (Exception)
            {

            }
            return oInsuranceViewModel;
        }

        public LegalViewModel GetLegalInformation(Guid userId)
        {
            LegalViewModel oLegalViewModel = null;
            try
            {
                var record = _db.LegalInformation.FirstOrDefault(m => m.UserId.Equals(userId));
                if (record != null)
                {
                    Mapper.CreateMap<LegalInformation, LegalViewModel>();
                    oLegalViewModel = Mapper.Map<LegalViewModel>(record);
                }

            }
            catch (Exception)
            {

            }
            return oLegalViewModel;
        }

        public PreferencesViewModel GetPreferences(Guid userId)
        {
            PreferencesViewModel oPreferencesViewModel = null;
            try
            {
                var record = _db.Preferences.FirstOrDefault(m => m.UserId.Equals(userId));
                if (record != null)
                {
                    Mapper.CreateMap<Preferences, PreferencesViewModel>();
                    oPreferencesViewModel = Mapper.Map<PreferencesViewModel>(record);
                }

            }
            catch (Exception)
            {

            }
            return oPreferencesViewModel;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public PersonalViewModel GetPatientInformation(Guid userId)
        {
            PersonalViewModel oPersonalViewModel = null;
            try
            {
                var record = _db.PersonalInformation.FirstOrDefault(m => m.UserId.Equals(userId));
                if (record != null)
                {
                    Mapper.CreateMap<PersonalInformation, PersonalViewModel>();
                    oPersonalViewModel = Mapper.Map<PersonalViewModel>(record);
                    oPersonalViewModel.DiffAbledType = record.DAbilityType;
                    var user = GetInitialPersonalDetails(userId);
                    if (oPersonalViewModel.FirstName == "" || oPersonalViewModel.FirstName == null)
                        oPersonalViewModel.FirstName = user.FirstName;
                    if (oPersonalViewModel.LastName == "" || oPersonalViewModel.LastName == null)
                        oPersonalViewModel.LastName = user.LastName;
                    if (oPersonalViewModel.Uhid == "" || oPersonalViewModel.Uhid == null)
                        oPersonalViewModel.Uhid = user.Uhid;
                    if (oPersonalViewModel.Cell_Phone == "" || oPersonalViewModel.Cell_Phone == null)
                        oPersonalViewModel.Cell_Phone = user.Cell_Phone;
                    if (oPersonalViewModel.Email == "" || oPersonalViewModel.Email == null)
                        oPersonalViewModel.Email = user.Email;
                    if (oPersonalViewModel.Pin == "" || oPersonalViewModel.Pin == null)
                        oPersonalViewModel.Pin = user.Pin;

                    oPersonalViewModel.strDOB = oPersonalViewModel.DOB.ToString("dd/MM/yyyy");
                   
                     var stateinfo=_db.States.Where(x=>x.Id== oPersonalViewModel.State).FirstOrDefault();
                    oPersonalViewModel.strState = stateinfo.Name;
                 
                    record = null;
                }
                else
                    oPersonalViewModel = GetInitialPersonalDetails(userId);
            }
            catch (Exception)
            {

            }
            return oPersonalViewModel;
        }

    }
}
