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

        public string? EmployeesBankName { get; set; }
    }

    public static class JobTypes
    {
        public static Dictionary<String, int> JobTypesDictionary = new Dictionary<string, int>
        {
            { "Director",  30 },
            { "Agent", 37 },
            { "Administrator", 44 },
            { "Coordinator", 47 },
            { "Specialist", 60 },
            { "Orchestrator", 62 },
            { "Assistant", 64 },
            { "Designer", 69 },
            { "Facilitator", 72 },
            { "Analyst", 79 },
            { "Producer", 80},
            { "Technician", 81},
            { "Manager", 84},
            { "Liaison", 87},
            { "Associate", 88},
            { "Consultant", 89},
            { "Engineer", 92},
            { "Strategist", 93},
            { "Supervisor", 94},
            { "Executive",95},
            { "Planner", 96},
            { "Developer",97},
            { "Officer", 98},
            { "Architect",99},
            { "Representative",100},
        };
    }

    public static class DocumentTypes
    {
        public static Dictionary<String, int> DocumentTypesDictionary = new Dictionary<string, int>
        {
            { "Driver License", 1 },
            { "Passport",  2 },
            { "Government Id", 3 },
        };
    }


}
