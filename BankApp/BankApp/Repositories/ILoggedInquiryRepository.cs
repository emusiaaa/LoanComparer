﻿using BankApp.Models;

namespace BankApp.Repositories
{
    public interface ILoggedInquiryRepository
    {
        public InquiryModel Get(int inquiryID);

        public void Add(InquiryModel client);

        public void Update(int clientID, InquiryModel client);

        public void Delete(int clientID);
    }
}