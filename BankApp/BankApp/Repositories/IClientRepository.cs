﻿using BankApp.Models;

namespace BankApp.Repositories
{
    public interface IClientRepository
    {
        public ClientModel Get(string clientID);

        public string GetRandomClientsEmail(int howManyClientsToSkip);

        public void Add(ClientModel client);

        public void Update(string clientID, ClientModel client);

        public void Delete(string clientID);
    }
}
