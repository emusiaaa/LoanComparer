using BankApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;


namespace BankApp.Models
{
    public class InquiryModel
    {
        //[Key]
        public int InquiryId { get; set; }
        //[DisplayName("Imię")]
        //[Required(ErrorMessage = "Pole Imię jest wymagane.")]
        public string ClientFirstName { get; set; }
       // [DisplayName("Nazwisko")]
        //[Required(ErrorMessage = "Pole Nazwisko jest wymagane.")]
        public string ClientLastName { get; set; }
        //[DisplayName("Email")]
        public string ClientEmail { get; set; }
        //[DisplayName("Numer dokumentu tożsamości")]
        public string ClientGovernmentIDNumber { get; set; }
        //[DisplayName("Rodzaj dokumentu tożsamości (dowód/paszport)")]
        public string ClientGovernmentIDType { get; set; }
        //[DisplayName("Zawód")]
        public string ClientJobType { get; set; }
        //[DisplayName("Miesięczny przychód")]
        public string ClientIncomeLevel { get; set; }
    }
}
