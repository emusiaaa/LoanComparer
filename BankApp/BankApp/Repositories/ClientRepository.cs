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

        public ClientModel Get(string clientID) => _context.Clients.SingleOrDefault(x => x.Id == clientID);

        public void Add(ClientModel client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public void Update(string clientID, ClientModel client)
        {
            var result = _context.Clients.SingleOrDefault(x => x.Id == clientID);
            if (result != null)
            {
                result.Email = client.Email;
                result.ClientIncomeLevel = client.ClientIncomeLevel;
                result.ClientJobType = client.ClientJobType;

                _context.SaveChanges();
            }
        }

        public void Delete(string clientID)
        {
            var result = _context.Clients.SingleOrDefault(x => x.Id == clientID);
            if (result != null)
            {
                _context.Clients.Remove(result);
                _context.SaveChanges();
            }
        }
    }
}
