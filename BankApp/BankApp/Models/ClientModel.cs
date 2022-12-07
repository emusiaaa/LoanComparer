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
        [Required]
        [DisplayName("Data urodzenia")]
        public string UserBirthDay { get; set; }

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
        [DisplayName("Data rozpoczęcia pracy")]
        public string ClientJobStartDay { get; set; }

        [Required]
        [DisplayName("Data zakończenia pracy")]
        public string ClientJobEndDay { get; set; }
    }

}
