namespace BankApp
{
    public class jsonclass
    {
        public class Loan
        {
            public int value { get; set; }
            public int installmentsNumber { get; set; }
            public PersonalData personalData { get; set; }

            public GovernmentDocument governmentDocument { get; set; }

            public JobDetails jobDetails { get; set; }
        }

        public class PersonalData
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string birthDate { get; set; }
        }
        public class GovernmentDocument
        {
            public int typeId { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string number { get; set; }
        }
        public class JobDetails
        {
            public int typeId { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string jobStartDate { get; set; }
            public string jobEndDate { get; set; }
        }
    }
}
