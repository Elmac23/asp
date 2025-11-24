using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Lista5Zad2.Pages
{
    public class StudentModel : PageModel
    {
        [BindProperty]
        [Required]
        public string FirstName { get; set; }

        [BindProperty]
        [Required]
        public string LastName { get; set; }

        [BindProperty]
        public int[] Points { get; set; }

        public void OnGet()
        {
            if (Points == null || Points.Length != 10)
            {
                Points = new int[10];
            }
        }

        public IActionResult OnPost()
        {
            if (Points == null || Points.Length != 10)
            {
                ModelState.AddModelError("Points", "WprowadŸ punkty dla 10 zadañ.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            TempData["StudentModel"] = JsonSerializer.Serialize(new { FirstName, LastName, Points });
            return RedirectToPage("Summary");
        }
    }
}