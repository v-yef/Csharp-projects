namespace DictionariesApp
{
    internal static class Message
    {
        public static void ShowMessage(string message) =>
           MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void ShowWarning(string message) =>
            MessageBox.Show(message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static void ShowError(string message) =>
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
