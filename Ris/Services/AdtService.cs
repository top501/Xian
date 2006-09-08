using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Healthcare;
using ClearCanvas.Common;
using ClearCanvas.Healthcare.Brokers;
using ClearCanvas.Enterprise;

namespace ClearCanvas.Ris.Services
{

    [ExtensionPoint()]
    public class PatientReconciliationStrategyExtensionPoint : ExtensionPoint<IPatientReconciliationStrategy>
    {
    }

    [ExtensionOf(typeof(ClearCanvas.Enterprise.ServiceLayerExtensionPoint))]
    public class AdtService : HealthcareServiceLayer, IAdtService
    {
        private IExtensionPoint _strategyExtensionPoint;

        public AdtService()
            :this(new PatientReconciliationStrategyExtensionPoint())
        {
        }

        internal AdtService(IExtensionPoint strategyExtensionPoint)
        {
            _strategyExtensionPoint = strategyExtensionPoint;
        }

        #region IAdtService Members

        [ReadOperation]
        public IList<PatientProfile> ListPatientProfiles(PatientProfileSearchCriteria criteria)
        {
            return GetPatientProfileBroker().Find(criteria);
        }

        [ReadOperation]
        public IList<PatientProfileMatch> FindPatientReconciliationMatches(PatientProfile patientProfile)
        {
            IPatientReconciliationStrategy strategy = (IPatientReconciliationStrategy)_strategyExtensionPoint.CreateExtension();

            return strategy.FindReconciliationMatches(patientProfile, GetPatientProfileBroker());
        }

        [ReadOperation]
        public IList<PatientProfile> ListReconciledPatientProfiles(PatientProfile patientProfile)
        {
            Patient patient = patientProfile.Patient;

            // ensure that the profiles collection is loaded
            GetPatientBroker().LoadRelated(patient, patient.Profiles);

            // exclude the reference profile from the list of returned profiles
            IList<PatientProfile> reconciledProfiles = new List<PatientProfile>();
            foreach (PatientProfile profile in patient.Profiles)
            {
                if(!profile.MRN.Equals(patientProfile.MRN))
                {
                    reconciledProfiles.Add(profile);
                }
            }
            return reconciledProfiles;
        }

        [UpdateOperation]
        public void ReconcilePatients(PatientProfile toBeKept, PatientProfile toBeReconciled)
        {
            if( toBeKept == null )
            {
                throw new PatientReconciliationException("Patient to be kept is null");
            }
            if (toBeKept == toBeReconciled)
            {
                throw new PatientReconciliationException("Patients are the same");
            }
            DoReconciliation(toBeKept.Patient, toBeReconciled);
        }

        [UpdateOperation]
        public void ReconcilePatients(Patient patient, PatientProfile toBeReconciled)
        {
            DoReconciliation(patient, toBeReconciled);
        }

        #endregion

        private void DoReconciliation(Patient patient, PatientProfile toBeReconciled)
        {
            PatientIdentifier mrnToBeReconciled = toBeReconciled.MRN;
            if (mrnToBeReconciled != null &&
                PatientHasProfileForSite(patient, mrnToBeReconciled.AssigningAuthority) == true)
            {
                throw new PatientReconciliationException("Patient already has identifier for site " + mrnToBeReconciled.AssigningAuthority);
            }

            // perform some additional validation on the profile?
            patient.AddProfile(toBeReconciled);

            GetPatientProfileBroker().Store(patient);
        }

        private static bool PatientHasProfileForSite(Patient patient, string site)
        {
            foreach (PatientProfile profile in patient.Profiles)
            {
                PatientIdentifier id = profile.MRN;
                if (id != null && 
                    id.AssigningAuthority == site)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
