using BankApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace BankApp.Models
{
    
    public class ClientModel: IdentityUser
    {
        [Required(ErrorMessage = "Pole Imię jest wymagane.")]
        [DisplayName("Imię")]
        public string UserFirstName { get; set; }

        [Required(ErrorMessage = "Pole Nazwisko jest wymagane.")]
        [DisplayName("Nazwisko")]
        public string UserLastName { get; set; }

        [DisplayName("Numer dokumentu tożsamości")]
        public string ClientGovernmentIDNumber { get; set; }
        [DisplayName("Rodzaj dokumentu tożsamości (dowód/paszport)")]
        public string ClientGovernmentIDType { get; set; }
        [DisplayName("Zawód")]
        public string ClientJobType { get; set; }
        [DisplayName("Miesięczny przychód")]
        public string ClientIncomeLevel { get; set; }
    }

}
