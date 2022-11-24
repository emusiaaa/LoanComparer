using BankApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace BankApp.Models
{
    [Table("Forms")]
    public class FormModel
    {
        
        [Key]
        public int FormId { get; set; }
        public int InstallmentsCount { get; set; }
        public float LoanValue { get; set; }

        public ClientModel? Client { get; set; }
        
    }

}
