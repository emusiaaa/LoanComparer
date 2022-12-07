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
    public class ClientModel : IdentityUser
    {
        [Required(ErrorMessage = "Field FirstName is required.")]
        [DisplayName("First name")]
        public string UserFirstName { get; set; }

        [Required(ErrorMessage = "Field LastName is required.")]
        [DisplayName("Last name")]
        public string UserLastName { get; set; }
        [Required]
        [DisplayName("Birth date")]
        public string UserBirthDay { get; set; }

        [Required]
        [DisplayName("ID Number")]
        public string ClientGovernmentIDNumber { get; set; }

        [Required]
        [DisplayName("ID type (id/passport)")]
        public string ClientGovernmentIDType { get; set; }

        [Required]
        [DisplayName("Job type")]
        public string ClientJobType { get; set; }

        [Required]
        [DisplayName("Monthly income")]
        public string ClientIncomeLevel { get; set; }

        [Required]
        [DisplayName("Job start date")]
        public string ClientJobStartDay { get; set; }

        [DisplayName("Job end date")]
        public string ClientJobEndDay { get; set; }

        public bool IsBankEmployee { get; set; }
    }

    public enum GovIDType
    {
        Passport,
        DriversLicense,
        GovernmentID
    }

    public enum JobType
    {
        Director,
        Agent,
        Administrator,
        Coordinator,
        Specialist,
        Orchestrator,
        Assistant,
        Designer,
        Facilitator,
        Analyst,
        Producer,
        Technician,
        Manager,
        Liaison,
        Associate,
        Consultant,
        Engineer,
        Strategist,
        Supervisor,
        Executive,
        Developer,
        Officer,
        Planner,
        Architect,
        Representative
    }
}
