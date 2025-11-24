using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Lista5Zad2.Models
{
    public class HelpersDemo
    {
        [Display(Name = "Akceptujê regulamin")]
        public bool AcceptTerms { get; set; }

        [Display(Name = "Wybór z listy")]
        public string SelectedOption { get; set; }

        public List<SelectListItem> Options { get; set; }

        [Display(Name = "Has³o")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Wybór radiowy")]
        public string RadioChoice { get; set; }

        [Display(Name = "Imiê")]
        public string Name { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        // For Html.Raw demo
        public string RawHtml { get; set; }
    }
}