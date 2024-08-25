using WebShop.Models;

namespace WebShop.Interface
{
    public interface IContactRepository
    {
        public void AddContact(Contact contact);
        IEnumerable<Contact> GetAllContacts();
        //IEnumerable<EmailSubscribe> GetAllSubscriber();
    }
}
