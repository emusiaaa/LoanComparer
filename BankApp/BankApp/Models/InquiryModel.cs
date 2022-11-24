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
        public int InstallmentsCount { get; set; }
        public float LoanValue { get; set; }

        public string ClientId { get; set; }
        public string SubmisionDate { get; set; }
    }
}
