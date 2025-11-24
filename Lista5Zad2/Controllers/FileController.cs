using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Lista5Zad2.Controllers
{
    [Route("File")]
    public class FileController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Upload")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(Microsoft.AspNetCore.Http.IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    ModelState.AddModelError("file", "Wybierz plik do przes³ania.");
                    return View("Index");
                }


                await using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var size = ms.Length;
                var name = file.FileName ?? string.Empty;

                var doc = new XDocument(new XDeclaration("1.0", "utf-8", null),
                    new XElement("file",
                        new XElement("name", name),
                        new XElement("size", size)
                    ));

                var xmlString = doc.ToString();
                var xmlBytes = Encoding.UTF8.GetBytes(xmlString);

                return File(xmlBytes, "application/xml", "fileInfo.xml");
            }
            catch (System.Exception)
            {
                ModelState.AddModelError(string.Empty, "Wyst¹pi³ b³¹d podczas przesy³ania pliku.");
                return View("Index");
            }
        }
    }
}
