using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Models
{
    [Table("Offers")]
    public class OfferModel
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public double Percentage { get; set; }
        [Required]
        public double MonthlyInstallment { get; set; }
        [Required]
        public double RequestedValue { get; set; }
        [Required]
        public int RequestedPeriodInMonth { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public string StatusDescription { get; set; }
        [Required]
        public int OfferIdInBank { get; set; }
        [Required]
        public int InquireId { get; set; }
        [Required]
        public string CreateDate { get; set; }
        [Required]
        public string UpdateDate { get; set; }
        public string? ApprovedBy { get; set; }
        [Required]
        public string DocumentLink { get; set; }
        [Required]
        public string DocumentLinkValidDate { get; set; }

        [Required]
        public bool IsOfferAccepted { get; set; }
    }

}
