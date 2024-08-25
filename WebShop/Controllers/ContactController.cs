using Microsoft.AspNetCore.Mvc;
using WebShop.Interface;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;

        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitContact(Contact contact, bool disableSubmit = false)
        {
            if (disableSubmit)
            {

                return View("Index");
            }

            if (ModelState.IsValid)
            {

                try
                {

                    _contactRepository.AddContact(contact);

                    TempData["success"] = "Your contact information has been submitted successfully.";

                    return RedirectToAction("Index","Home");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("disableSubmit", "An error occurred while processing your request.");
                    return View("Index", contact);
                }
            }
            else
            {

                if (string.IsNullOrWhiteSpace(contact.Name))
                {
                    ModelState.AddModelError("Name", "The Name field is required.");
                }

                if (string.IsNullOrWhiteSpace(contact.Email))
                {
                    ModelState.AddModelError("Email", "The Email field is required.");
                }

                if (string.IsNullOrWhiteSpace(contact.Message))
                {
                    ModelState.AddModelError("Message", "The Message field is required.");
                }

                return View("Index", contact);
            }

        }
    }
}
