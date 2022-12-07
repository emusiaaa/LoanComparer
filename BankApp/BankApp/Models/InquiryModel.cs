using BankApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace BankApp.Models
{
    [Table("Inquiries")]
    public class InquiryModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int InstallmentsCount { get; set; }
        [Required]
        public float LoanValue { get; set; }

        [Required]
        public string SubmisionDate { get; set; }

        [Required]
        public string ClientId { get; set; }
    }
}
