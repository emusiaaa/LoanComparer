using BankApp.Data;
using BankApp.Models;

namespace BankApp.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly LoansComparerDBContext _context;

        public ClientRepository(LoansComparerDBContext context)
        {
            _context = context;
        }

        public ClientModel Get(int clientID) => _context.Clients.SingleOrDefault(x => x.ClientId == clientID);

        public void Add(ClientModel client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public void Update(int clientID, ClientModel client)
        {
            var result = _context.Clients.SingleOrDefault(x => x.ClientId == clientID);
            if (result != null)
            {
                result.ClientEmail = client.ClientEmail;
                result.ClientIncomeLevel = client.ClientIncomeLevel;
                result.ClientJobType = client.ClientJobType;

                _context.SaveChanges();
            }
        }

        public void Delete(int clientID)
        {
            var result = _context.Clients.SingleOrDefault(x => x.ClientId == clientID);
            if (result != null)
            {
                _context.Clients.Remove(result);
                _context.SaveChanges();
            }
        }
    }
}
