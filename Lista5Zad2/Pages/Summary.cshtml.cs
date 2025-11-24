using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Lista5Zad2.Pages
{
    public class SummaryModel : PageModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int[] Points { get; set; }

        public IActionResult OnGet()
        {
            if (TempData.TryGetValue("StudentModel", out var serialized) && serialized is string json)
            {
                var data = JsonSerializer.Deserialize<JsonElement>(json);
                FirstName = data.GetProperty("FirstName").GetString();
                LastName = data.GetProperty("LastName").GetString();
                var pointsJson = data.GetProperty("Points");
                Points = new int[pointsJson.GetArrayLength()];
                for (int i = 0; i < Points.Length; i++)
                {
                    Points[i] = pointsJson[i].GetInt32();
                }
                return Page();
            }

            return RedirectToPage("Student");
        }
    }
}