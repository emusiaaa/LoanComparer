using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BankApp.Models
{
    public class AllInquiryViewModel
    {
        public string SubmissionDate { get; set; }

        public int InstallmentsCount { get; set; }

        public float LoanValue { get; set; }
        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string UserBirthDay { get; set; }

        public string Email { get; set; }

        public string ClientGovernmentIDNumber { get; set; }

        public string ClientGovernmentIDType { get; set; }

        public string ClientJobType { get; set; }

        public string ClientIncomeLevel { get; set; }
        public string ClientJobStartDay { get; set; }
        public string ClientJobEndDay { get; set; }
    }
}
