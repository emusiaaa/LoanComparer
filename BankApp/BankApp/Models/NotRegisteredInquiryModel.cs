using BankApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace BankApp.Models
{
    [Table("NotRegisteredInquiries")] 
    public class NotRegisteredInquiryModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string SubmissionDate { get; set; } 

        [Required]
        public int InstallmentsCount { get; set; }

        [Required]
        public float LoanValue { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string UserFirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string UserLastName { get; set; }

        [Required]
        [DisplayName("Birth date")]
        public string UserBirthDay { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [DisplayName("ID number")]
        public string ClientGovernmentIDNumber { get; set; }

        [Required]
        [DisplayName("ID type (id/passport)")]
        public string ClientGovernmentIDType { get; set; }

        [Required]
        [DisplayName("Job Type")]
        public string ClientJobType { get; set; }

        [Required]
        [DisplayName("Monthly income")]
        public string ClientIncomeLevel { get; set; }

        [Required]
        [DisplayName("Job start date")]
        public string ClientJobStartDay { get; set; }

        [DisplayName("Job end date")]
        public string ClientJobEndDay { get; set; }

    }
}
