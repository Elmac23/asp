using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Lista6.Validators
{
    public class PeselAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var s = value as string;
            if (string.IsNullOrEmpty(s)) return true;
            return Regex.IsMatch(s, "^\\d{11}$");
        }
    }

    public class LettersDigitsWhiteAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var s = value as string;
            if (string.IsNullOrEmpty(s)) return true;
            var pattern = "^[A-Za-z0-9\\s¹æê³ñóœ¿Ÿ¥ÆÊ£ÑÓŒ¯]*$";
            return Regex.IsMatch(s, pattern);
        }
    }
}
