using BankApp.Models;

namespace BankApp.Repositories
{
    public interface IClientRepository
    {
        public ClientModel Get(int clientID);
        
        public void Add(ClientModel client);

        public void Update(int clientID, ClientModel client);

        public void Delete(int clientID);
    }
}
