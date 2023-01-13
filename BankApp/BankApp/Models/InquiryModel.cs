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
        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        [Required]
        public int InstallmentsCount { get; set; }       
        [Range(1, float.MaxValue, ErrorMessage = "Please enter valid float Number")]
        [Required]
        public float LoanValue { get; set; }

        [Required]
        public string SubmisionDate { get; set; } = DateTime.UtcNow.ToString("o");

        [Required]
        public string ClientId { get; set; }
    }
}
