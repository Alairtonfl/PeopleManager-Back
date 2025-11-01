using System.Text.RegularExpressions;

namespace PeopleManager.Application.Utilities
{
    public static class Utilitie
    {
        public static bool EmailIsValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

            return regex.IsMatch(email);
        }

        public static bool PasswordIsValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Password must be at least 8 characters long, contain at least one digit, one uppercase letter, and one lowercase letter
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", RegexOptions.Compiled);

            return regex.IsMatch(password);
        }

        public static string ToPascalCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = Regex.Replace(input, @"[_\s]+", " ");

            return Regex.Replace(input, @"\b\w+\b", match =>
            {
                string word = match.Value.ToLower();
                return char.ToUpper(word[0]) + word.Substring(1);
            });
        }

        public static bool IsCpfValid(string? cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = Regex.Replace(cpf, @"\D", "");

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            string baseCpf = cpf[..9];
            string firstCheckDigit = CalculateCheckDigit(baseCpf, new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 });
            string secondCheckDigit = CalculateCheckDigit(baseCpf + firstCheckDigit, new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 });

            return cpf.EndsWith(firstCheckDigit + secondCheckDigit);
        }

        private static string CalculateCheckDigit(string baseCpf, int[] multipliers)
        {
            int sum = baseCpf
                .Select((digit, index) => int.Parse(digit.ToString()) * multipliers[index])
                .Sum();

            int remainder = sum % 11;
            return (remainder < 2 ? 0 : 11 - remainder).ToString();
        }
    }
}
