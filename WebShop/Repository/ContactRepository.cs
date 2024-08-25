using WebShop.Interface;
using WebShop.Models;
using WebShop.Data;

namespace WebShop.Repository
{
    public class ContactRepository : IContactRepository
    {
        public WebDbContext _dbContext;

        public ContactRepository(WebDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddContact(Contact contact)
        {
            _dbContext.Contacts.Add(contact);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Contact> GetAllContacts()
        {
            return _dbContext.Contacts.ToList();
        }
        //public IEnumerable<EmailSubscribe> GetAllSubscriber()
        //{
        //    return _dbContext.EmailSubscriptions.ToList();
        //}

    }
}
