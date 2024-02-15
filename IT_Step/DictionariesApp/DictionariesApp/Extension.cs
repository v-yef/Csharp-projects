namespace DictionariesApp
{
    internal static class Extension
    {
        // Метод расширения, центрирующий строку по указанной длине.
        // Центрирование происходит путем добавления пробелов в начало и конец строки,
        // тем самым увеличивая общую длину строки до указанной.
        public static string PadCenter(this string sourceString, int length)
        {
            // Условие, при котором действие не имеет смысла.
            if ((sourceString.Length >= length) || (sourceString.Length + 1 == length))
            {
                return sourceString;
            }

            // Сколько символов добавить с обеих концов исходной строки, чтобы уравнять её длину с length.
            int charNumberToAdd = (length - sourceString.Length) / 2;

            string result = "";
            // Добавить пробелы в начале строки.
            for (int i = 0; i < charNumberToAdd; i++)
            {
                result += " ";
            }
            // Добавить исходную строку.
            result += sourceString;
            // Добавить пробелы в конце строки.
            for (int i = 0; i < charNumberToAdd; i++)
            {
                result += " ";
            }

            return result;
        }
    }
}
