using BankApp.Models;

namespace BankApp.Services
{
    public static class MailCreator
    {
        public static string ConfirmationOfSubmittingAnInquiry(ClientModel user, InquiryModel inquiry)
        {
            return
                "<h3>Thanks for submitting your form!</h3>" +
                "<p>Here's a little summary: " +
                "</p><p>Loan value: " + inquiry.LoanValue +
                "</p>" +
                "<p>Number of installments: " + inquiry.InstallmentsCount +
                "</p><p>Name: " + user.UserFirstName + " " + user.UserLastName +
                "</p><p>Government ID Type: " + user.ClientGovernmentIDType +
                "</p><p>Government ID Number: " + user.ClientGovernmentIDNumber +
                "</p><p>Job type: " + user.ClientJobType +
                "</p><p>Income level: " + user.ClientIncomeLevel +
                "</p><p>Link to check the status of  inquiry: <a href=\"https://localhost:7280/Home/OfferList2?inquiryID=" + inquiry.Id + "\">" + "here</a>" +
                "</p>";
        }
        public static string ConfirmationOfSubmittingAnInquiryNR(NotRegisteredInquiryModel inquiry)
        {
            return
                 "<h3>Thanks for submitting your form!</h3>" +
                 "<p>Here's a little summary: " +
                 "</p><p>Loan value: " + inquiry.LoanValue +
                 "</p>" +
                 "<p>Number of installments: " + inquiry.InstallmentsCount +
                 "</p><p>Name: " + inquiry.UserFirstName + " " + inquiry.UserLastName +
                 "</p><p>Government ID Type: " + inquiry.ClientGovernmentIDType +
                 "</p><p>Government ID Number: " + inquiry.ClientGovernmentIDNumber +
                 "</p><p>Job type: " + inquiry.ClientJobType +
                 "</p><p>Income level: " + inquiry.ClientIncomeLevel +
                 "</p><p>Link to check the status of  inquiry: <a href=\"https://localhost:7280/Home/OfferList2?inquiryID=" + inquiry.Id + "&isNR=true\">" + "here</a>" +
                 "</p>";
        }
        public static string EmployeeDecision(bool decision)
        {
            string descisionString = decision ? "accepted" : "declined";
            string extra = decision ? "<p>Thanks for choosing our bank! We hope you'll enjoy your money ;)</p>" :
                "<p>We hope that you'll still make ends meet</p>";
            return
                "<p>Your offer was " + descisionString + " by the bank</p>" + extra;
        }
    }
}
