using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Models
{

    [Table("OffersSummary")]
    public class OffersSummaryModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InquiryIdInOurDb { get; set; }

        [Required]
        public bool IsNRInquiry { get; set; }

        [Required]
        public string BankName { get; set; }

        [Required]
        public long OfferIdInOurDb { get; set; }

        public string? ClientId { get; set; }
    }

}
