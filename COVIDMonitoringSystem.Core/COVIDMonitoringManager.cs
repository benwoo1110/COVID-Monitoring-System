//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.SafeEntryMgr;
using COVIDMonitoringSystem.Core.TravelEntryMgr;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.Core
{
    public class COVIDMonitoringManager
    {
        public List<BusinessLocation> BusinessLocationList { get; private set; }
        public List<SHNFacility> SHNFacilitiesList { get; private set; }
        public List<Person> PersonList { get; private set; }
        public HashSet<string> ValidCountries { get; private set; }
        public HashSet<string> ValidCollectionLocation { get; private set; }

        public COVIDMonitoringManager()
        {
            LoadValidInputSetData();
            LoadBusinessLocationData();
            LoadSHNFacilityData();
            LoadPersonData();
        }

        private void LoadValidInputSetData()
        {
            ValidCollectionLocation = CoreHelper.ReadSingleColumnCsv("resources/CollectionLocation.csv");
            ValidCountries = CoreHelper.ReadSingleColumnCsv("resources/Countries.csv");
        }

        /// <summary>
        /// Reads CSV file "BusinessLocation.csv" and adds the data into a list BusinessLocationList
        /// </summary>
        private void LoadBusinessLocationData()
        {
            var locationCsvData = CoreHelper.ReadCsv("resources/BusinessLocation.csv");
            BusinessLocationList = new List<BusinessLocation>();
            foreach (var entry in locationCsvData)
            {
                var newLocation = new BusinessLocation(entry["businessname"], entry["branchcode"], Convert.ToInt32(entry["maximumcapacity"]));
                BusinessLocationList.Add(newLocation);
            }
        }

        /// <summary>
        /// Calls a web API and reads the JSON file and adds the data into a list SHNFacilitiesList
        /// </summary>
        private void LoadSHNFacilityData()
        {
            SHNFacilitiesList = (List<SHNFacility>) CoreHelper.FetchFromWeb<List<SHNFacility>>(
                "https://covidmonitoringapiprg2.azurewebsites.net",
                "/facility"
            ) ?? new List<SHNFacility>();
        }

        /// <summary>
        /// Reads CSV file "Person.csv" and adds the data into a list PersonList, calls function ParseResidentEntry or ParseVisitorEntry depending on type of person in data
        /// </summary>
        private void LoadPersonData()
        {
            var personCsvData = CoreHelper.ReadCsv("resources/Person.csv");
            PersonList = new List<Person>();

            foreach (var entry in personCsvData)
            {
                var newPerson = entry["type"] == "resident"
                    ? ParseResidentEntry(entry)
                    : ParseVisitorEntry(entry);

                if (!string.IsNullOrEmpty(entry["travelEntryLastCountry"]))
                {
                    newPerson.AddTravelEntry(ParseTravelEntry(entry, newPerson));
                }

                PersonList.Add(newPerson);
            }
        }

        /// <summary>
        /// Creates new Resident if person in CSV file is a resident with its appropriate attributes, calls ParseTraceTogetherToken if resident has a TraceTogether token
        /// </summary>
        /// <param name="entry">Each entry of person details in CSV file</param>
        /// <returns>New Resident object</returns>
        private Person ParseResidentEntry(IReadOnlyDictionary<string, string> entry)
        {
            var newResident = new Resident(
                entry["name"], 
                entry["address"], 
                Convert.ToDateTime(entry["lastLeftCountry"])
            );

            if (!string.IsNullOrWhiteSpace(entry["tokenSerial"]))
            {
                newResident.Token = ParseTraceTogetherToken(entry);
            }
            
            return newResident;
        }

        /// <summary>
        /// Creates new TraceTogetherToken object if the Resident in the CSV file has a token
        /// </summary>
        /// <param name="entry">Each entry of person details in CSV file</param>
        /// <returns>New TraceTogetherToken object</returns>
        private TraceTogetherToken ParseTraceTogetherToken(IReadOnlyDictionary<string, string> entry)
        {
            return new TraceTogetherToken(
                entry["tokenSerial"], 
                entry["tokenCollectionLocation"], 
                Convert.ToDateTime(entry["tokenExpiryDate"])
            );
        }

        /// <summary>
        /// Creates new Visitor if person in CSV file is a visitor with its appropriate attributes
        /// </summary>
        /// <param name="entry">Each entry of person details in CSV file</param>
        /// <returns>New Visitor object</returns>
        private Person ParseVisitorEntry(IReadOnlyDictionary<string, string> entry)
        {
            return new Visitor(entry["name"], entry["passportNo"], entry["nationality"]);
        }

        /// <summary>
        /// Creates a new TravelEntry for each person in CSV file
        /// </summary>
        /// <param name="entry">Each entry of person details in CSV file</param>
        /// <param name="newPerson">The new Person object created in function LoadPersonData()</param>
        /// <returns></returns>
        private TravelEntry ParseTravelEntry(IReadOnlyDictionary<string, string> entry, Person newPerson)
        {
            return new TravelEntry(
                newPerson,
                entry["travelEntryLastCountry"],
                CoreHelper.ParseEnum<TravelEntryMode>(entry["travelEntryMode"]),
                Convert.ToDateTime(entry["travelEntryDate"]),
                Convert.ToDateTime(entry["travelShnEndDate"]),
                FindSHNFacility(entry["facilityName"]),
                Convert.ToBoolean(entry["travelIsPaid"])
            );
        }

        /// <summary>
        /// Calls a web API and reads the JSON file and adds the data into a dictionary
        /// </summary>
        /// <param name="country">Input name of country that user wants to check for COVID data</param>
        /// <returns>Dictionary object consisting of country name and country COVID data</returns>
        public Dictionary<string, object> LoadCountryCovidData(string country)
        {
            return CoreHelper.FetchFromWeb<Dictionary<string, object>>(
                "https://disease.sh", 
                $"/v3/covid-19/countries/{country}"
            );
        }

        /// <summary>
        /// Generates SHN Status Report into a new CSV for each person who is serving SHN
        /// </summary>
        /// <param name="dateTime">Input DateTime of the date to generate SHN status report for</param>
        /// <returns>New CSV file with SHN status report information</returns>
        public FileCreateResult GenerateSHNStatusReportFile(DateTime dateTime)
        {
            var csvData = new List<Dictionary<string, string>>();

            PersonList.ForEach(person => person.TravelEntryList.ForEach(entry =>
            {
                if (entry.IsWithinQuarantineTime(dateTime))
                {
                    csvData.Add(GenerateSingleTravelEntryReport(entry));
                }
            }));

            return CoreHelper.WriteCsv(
                $"SHNStatusReport_{dateTime:yyyy-MM-dd_HH-mm-ss}.csv",
                new []{ "Type", "Name", "End Date", "Facility Name" },
                csvData
            );
        }

        /// <summary>
        /// Generates Contact Tracing Report into a new CSV file
        /// </summary>
        /// <param name="location">Input store name to generate contact tracing details for</param>
        /// <param name="start">Input the start of the time range to perform contact tracing</param>
        /// <param name="end">Input the end of the time range to perform contact tracing</param>
        /// <returns>New CSV file with contact tracing information</returns>
        public FileCreateResult GenerateContactTracingReportFile(BusinessLocation location, DateTime start, DateTime end)
        {
            var csvData = new List<Dictionary<string, string>>();

            foreach (var p in PersonList)
            {
                foreach (var i in p.SafeEntryList)
                {
                    if (i.Location == location && i.CheckIn >= start && i.CheckIn <= end)
                    {
                        csvData.Add(GenerateSingleSafeEntryDetails(i, p));
                    }
                }
            }
            return CoreHelper.WriteCsv(
                $"ContactTracingReport.csv",
                new[] { "Name", "Check In Date and Time", "Check Out Date and Time" },
                csvData
            );
        }

        /// <summary>
        /// Generates a single TravelEntry record as a dictionary
        /// </summary>
        /// <param name="entry">The TravelEntry object to generate record for</param>
        /// <returns>Dictionary containing TravelEntry record information</returns>
        private Dictionary<string, string> GenerateSingleTravelEntryReport(TravelEntry entry)
        {
            return new Dictionary<string, string>
            {
                {"Type", ReflectHelper.GetTypeName(entry.TravelPerson)},
                {"Name", entry.TravelPerson.Name},
                {"End Date", entry.ShnEndDate.ToString(CultureInfo.InvariantCulture)},
                {"Facility Name", entry.GetFacilityName()},
            };
        }

        /// <summary>
        /// Generate SafeEntry details of a single person
        /// </summary>
        /// <param name="details">SafeEntry object to pass in</param>
        /// <param name="info">Target to generate SafeEntry details for</param>
        /// <returns></returns>
        private Dictionary<string, string> GenerateSingleSafeEntryDetails(SafeEntry details, Person info)
        {
            return new Dictionary<string, string>
            {
                {"Name", info.Name},
                {"Check In Date and Time", Convert.ToString(details.CheckIn)},
                {"Check Out Date and Time", (details.CheckOut.Equals(DateTime.MinValue)) 
                    ? "-" 
                    : Convert.ToString(details.CheckOut)}
            };
        }

        /// <summary>
        /// Adds a new business location to BusinessLocationList
        /// </summary>
        /// <param name="business">Input business location</param>
        public void AddBusinessLocation(BusinessLocation business)
        {
            BusinessLocationList.Add(business);
        }

        /// <summary>
        /// Adds a new SHN facility to SHNFacilitiesList
        /// </summary>
        /// <param name="facility">Input SHN facility</param>
        public void AddSHNFacility(SHNFacility facility)
        {
            SHNFacilitiesList.Add(facility);
        }

        /// <summary>
        /// Adds a new Person to PersonList
        /// </summary>
        /// <param name="person">Input Person</param>
        public void AddPerson(Person person)
        {
            PersonList.Add(person);
        }

        /// <summary>
        /// Searches for input in BusinessLocationList
        /// </summary>
        /// <param name="name">Name of business location to search for</param>
        /// <returns>Name of business location if it is found</returns>
        public BusinessLocation FindBusinessLocation(string name)
        {
            return BusinessLocationList.Find(businessLocation => businessLocation.BusinessName.ToLower().Equals(name.ToLower()));
        }

        /// <summary>
        /// Searches for input in SHNFacilitiesList
        /// </summary>
        /// <param name="name">Name of SHN facility to search for</param>
        /// <returns>Name of SHN facility if it is found</returns>
        public SHNFacility FindSHNFacility(string name)
        {
            return SHNFacilitiesList.Find(shnFacility => shnFacility.FacilityName.Equals(name));
        }

        /// <summary>
        /// Searches for input in PersonList
        /// </summary>
        /// <param name="name">Name of Person to search for</param>
        /// <returns>Name of Person if it is found</returns>
        public Person FindPerson(string name)
        {
            return PersonList.Find(person => person.Name.Equals(name));
        }

        /// <summary>
        /// Searches if a certain person type has the input name
        /// </summary>
        /// <typeparam name="T">Type of Person to search for</typeparam>
        /// <param name="name">Name of Person to search for</param>
        /// <returns>Name of Resident/Visitor if it is found</returns>
        public T FindPersonOfType<T>(string name) where T : Person
        {
            return GetAllPersonOfType<T>().Find(person => person.Name.ToLower().Equals(name.ToLower()));
        }

        /// <summary>
        /// Gets all Persons of a certain type in a list
        /// </summary>
        /// <typeparam name="T">Type of Person to search for</typeparam>
        /// <returns>A list with all the persons of the selected type</returns>
        public List<T> GetAllPersonOfType<T>() where T : Person
        {
            return PersonList.FindAll(p => p is T).ConvertAll(p => (T) p);
        }
    }
}