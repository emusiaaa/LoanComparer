using BankApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace BankApp.Models
{
    [Table("Users")]
    public class ClientModel: IdentityUser
    {
        [Required(ErrorMessage = "Field FirstName is required.")]
        [DisplayName("First name")]
        public string UserFirstName { get; set; }

        [Required(ErrorMessage = "Field LastName is required.")]
        [DisplayName("Last name")]
        public string UserLastName { get; set; }

        [DisplayName("Government ID Number")]
        public string ClientGovernmentIDNumber { get; set; }
        [DisplayName("Government ID Type")]
        public string ClientGovernmentIDType { get; set; }
        [DisplayName("Job Type")]
        public string ClientJobType { get; set; }
        [DisplayName("Income Level")]
        public string ClientIncomeLevel { get; set; }
        public bool IsBankEmployee { get; set; }
    }

}
