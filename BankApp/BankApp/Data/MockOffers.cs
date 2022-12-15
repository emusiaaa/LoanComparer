using Newtonsoft.Json;
using NuGet.Protocol;

namespace BankApp.Data
{
    public class MockOffers
    {
        public long id { get; set; }
        public double percentage { get; set; }

        public double monthlyInstallment { get; set; }

        public double requestedValue { get; set; }

        public int requestedPeriodInMonth { get; set; }

        public int statusId { get; set; }

        public string statusDescription { get; set; }

        public int inquireId { get; set; }

        public string createDate { get; set; }

        public string updateDate { get; set; }

        public int? approvedBy { get; set; }

        public string documentLink { get; set; }

        public string documentLinkValidDate { get; set; }

        public MockOffers() { }

        public static string GenerateMockOffer()
        {
            var randomSeed = new Random();

            var offer = new MockOffers();
            offer.id = randomSeed.Next(100);
            offer.percentage = randomSeed.Next(30) / 100.0;
            offer.requestedValue = randomSeed.Next(10000);
            offer.requestedPeriodInMonth = randomSeed.Next(24);
            offer.monthlyInstallment = offer.requestedValue * (1.0 + offer.percentage) / offer.requestedPeriodInMonth;
            offer.statusId = 1;
            offer.statusDescription = "Created";
            offer.inquireId = 31;

            var month = randomSeed.Next(11);

            offer.createDate = new DateTime(2022, month, 20, 19, 34, 56, DateTimeKind.Utc).ToString("o");
            offer.updateDate = new DateTime(2022, month+1, 2, 8, 50, 14, DateTimeKind.Utc).ToString("o");
            offer.approvedBy = null;
            offer.documentLink = "https://mock-offer-link.com";
            offer.documentLinkValidDate = new DateTime(2023, (month+7)%12, 2, 8, 50, 14, DateTimeKind.Utc).ToString("o");

            return JsonConvert.SerializeObject(offer);
        }
    }
}
