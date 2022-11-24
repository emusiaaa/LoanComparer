using System.ComponentModel.DataAnnotations.Schema;

namespace BankApp.Models
{
    [Table("Inquiries")]
    public class InquiryModel
    {

        public int FormId { get; set; }
        public int InstallmentsCount { get; set; }
        public float LoanValue { get; set; }

        public ClientModel? Client { get; set; }

    }
}
