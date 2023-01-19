using BankApp.Models;

namespace BankApp.Services
{
    public class InquiryServer: IInquiryServer
    {
        public jsonclass.Loan CreateNRInquiry(NotRegisteredInquiryModel inquiry)
        {
            DateTime dt = DateTime.UtcNow;
            inquiry.SubmissionDate = dt.ToString("o");
            inquiry.ClientJobEndDay = dt.ToString("o");
            inquiry.UserBirthDay = DateTimeOffset.Parse(inquiry.UserBirthDay).UtcDateTime.ToString("o");
            inquiry.ClientJobStartDay = DateTimeOffset.Parse(inquiry.ClientJobStartDay).UtcDateTime.ToString("o");

            var j =  new jsonclass.Loan
            {
                value = (int)inquiry.LoanValue,
                installmentsNumber = inquiry.InstallmentsCount,
                personalData = new jsonclass.PersonalData
                {
                    firstName = inquiry.UserFirstName,
                    lastName = inquiry.UserLastName,
                    birthDate = inquiry.UserBirthDay,
                },
                governmentDocument = new jsonclass.GovernmentDocument
                {
                    typeId = BankApp.Models.DocumentTypes.DocumentTypesDictionary[inquiry.ClientGovernmentIDType],
                    name = inquiry.ClientGovernmentIDType,
                    description = inquiry.ClientGovernmentIDType,
                    number = inquiry.ClientGovernmentIDNumber
                },
                jobDetails = new jsonclass.JobDetails
                {
                    typeId = BankApp.Models.JobTypes.JobTypesDictionary[inquiry.ClientJobType],
                    name = inquiry.ClientJobType,
                    description = inquiry.ClientJobType,
                    jobStartDate = inquiry.ClientJobStartDay,
                    jobEndDate = inquiry.ClientJobEndDay,
                },
            };
            return j;
        }
        public jsonclass.Loan CreateInquiry(InquiryModel inquiry, ClientModel user)
        {
            DateTime dt = DateTime.UtcNow;
            inquiry.ClientId = user.Id;
            inquiry.SubmisionDate = dt.ToString("o");
            return new jsonclass.Loan
            {
                value = (int)inquiry.LoanValue,
                installmentsNumber = inquiry.InstallmentsCount,
                personalData = new jsonclass.PersonalData
                {
                    firstName = user.UserFirstName,
                    lastName = user.UserLastName,
                    birthDate = DateTimeOffset.Parse(user.UserBirthDay).UtcDateTime.ToString("o"),
                    // birthDate = inquiry.UserBirthDay,
                },
                governmentDocument = new jsonclass.GovernmentDocument
                {
                    typeId = BankApp.Models.DocumentTypes.DocumentTypesDictionary[user.ClientGovernmentIDType],
                    name = user.ClientGovernmentIDType,
                    description = user.ClientGovernmentIDType,
                    number = user.ClientGovernmentIDNumber
                },
                jobDetails = new jsonclass.JobDetails
                {
                    typeId = BankApp.Models.JobTypes.JobTypesDictionary[user.ClientJobType],
                    name = user.ClientJobType,
                    description = user.ClientJobType,
                    jobStartDate = DateTimeOffset.Parse(user.ClientJobStartDay).UtcDateTime.ToString("o"),
                    jobEndDate = DateTimeOffset.Parse(user.ClientJobEndDay).UtcDateTime.ToString("o"),
                },
            };
        }
    }
}
