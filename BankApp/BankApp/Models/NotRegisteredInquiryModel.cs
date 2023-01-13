using BankApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
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
        public string SubmissionDate { get; set; } = DateTime.UtcNow.ToString("o");

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid int Number")]
        [Required]
        public int InstallmentsCount { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Please enter valid float Number")]
        [Required]
        public float LoanValue { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        [DisplayName("First Name")]
        public string UserFirstName { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
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
        public string ClientJobEndDay { get; set; } = DateTime.UtcNow.ToString("o");

    }
}
