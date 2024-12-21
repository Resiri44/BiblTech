using System;
using System.Globalization;

namespace ValidationLibrary
{
    public static class InputValidator
    {
        // Проверка на числовое значение (int)
        public static bool ValidateInt(string input)
        {
            return int.TryParse(input?.Trim(), out _);
        }

        // Проверка на числовое значение (decimal)
        

        public static bool ValidateDecimal(string input)
        {
        return decimal.TryParse(input?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }

         // Проверка на корректную дату
        public static bool ValidateDate(string input)
        {
            return DateTime.TryParse(input?.Trim(), out _);
        }

        // Проверка на пустую строку
        public static bool ValidateNotEmpty(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }
    }
}
