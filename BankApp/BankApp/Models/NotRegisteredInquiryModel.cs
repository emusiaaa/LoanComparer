using BankApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace BankApp.Models
{
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
        [DisplayName("Imię")]
        public string UserFirstName { get; set; }

        [Required]
        [DisplayName("Nazwisko")]
        public string UserLastName { get; set; }

        [Required]
        [DisplayName("Numer dokumentu tożsamości")]
        public string ClientGovernmentIDNumber { get; set; }

        [Required]
        [DisplayName("Rodzaj dokumentu tożsamości (dowód/paszport)")]
        public string ClientGovernmentIDType { get; set; }

        [Required]
        [DisplayName("Zawód")]
        public string ClientJobType { get; set; }

        [Required]
        [DisplayName("Miesięczny przychód")]
        public string ClientIncomeLevel { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }
    }
}
