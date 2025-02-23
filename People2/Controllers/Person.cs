using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using People2.Data;
using People2.Models;

namespace People2.Controllers
{
    public class Person : Controller
    {
        private readonly ApplicationDbContext _context;

        public Person(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var persons = await _context.Persons.ToListAsync();
            return View(persons);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PersonMod model)
        {
            if (_context.Persons.Any(p => p.IdNum == model.IdNum))  // Ensure "IDNumber" matches the property
            {
                ModelState.AddModelError("IDNumber", "A person with this ID Number already exists.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                _context.Persons.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var person = _context.Persons.FirstOrDefault(p => p.IdNum == id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person); // Return the current data for the user to edit
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PersonMod model)
        {
            if (ModelState.IsValid)
            {
                var personInDb = _context.Persons.FirstOrDefault(p => p.IdNum == model.IdNum);
                if (personInDb == null)
                {
                    return NotFound();
                }

                personInDb.FName = model.FName;
                personInDb.LName = model.LName;
                // Update other fields if necessary

                _context.SaveChanges(); // Save changes to the database
                return RedirectToAction("Index"); // Redirect back to the person list
            }

            return View(model); // Return with validation errors if needed
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound(); // Return 404 if person not found
            }

            return View(person); // Pass the person to the Delete confirmation view
        }
        [HttpGet]
        public async Task<IActionResult> SearchAsync(string idNum)
        {
            if (string.IsNullOrEmpty(idNum))
            {
                // If no ID is entered, return the full list
                return RedirectToAction("Index");
            }

            // Find persons that match the entered ID number
            var searchResults = await _context.Persons
                .Where(p => p.IdNum.Contains(idNum)) // You can use exact match or partial match based on the needs
                .ToListAsync();

            return View("Index", searchResults); // Pass the filtered results to the Index view
        }
        // Delete the person (Post)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound(); // Return 404 if person not found
            }

            _context.Persons.Remove(person); // Remove the person
            await _context.SaveChangesAsync(); // Save changes to the database
            return RedirectToAction("Index"); // Redirect to the persons list page after deleting
        }


    }
}