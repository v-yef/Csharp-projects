namespace DictionariesApp
{
    internal static class Menu
    {
        // Метод вывода вертикального меню по координатам.
        // Принимает массив позиций меню, координаты левого верхнего угла меню, а также цвет текста и фона.
        // Возвращает индекс выбранного элемента или -1 при нажатии ESC (выход).
        public static int VerticalMenu(string[] elements, int upLeft_x, int upLeft_y,
            ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {

            int maxLen = 0;

            foreach (var item in elements)
            {
                if (item.Length > maxLen)
                {
                    maxLen = item.Length;
                }
            }

            // Зафиксировать начальные цвета текста и фона.
            ConsoleColor initialForegroundColor = Console.ForegroundColor;
            ConsoleColor initialBackgroundColor = Console.BackgroundColor;

            Console.BackgroundColor = foregroundColor;
            Console.ForegroundColor = backgroundColor;

            // Вернуть курсор в начальное положение для корректной привязки вывода.
            Console.SetCursorPosition(0, 0);

            int x = Console.CursorLeft;
            int y = Console.CursorTop;

            int pos = 0;

            while (true)
            {
                for (int i = 0; i < elements.Length; i++)
                {

                    Console.CursorVisible = false;
                    Console.SetCursorPosition(x + upLeft_x, y + i + upLeft_y);

                    if (i == pos)
                    {
                        Console.BackgroundColor = foregroundColor;
                        Console.ForegroundColor = backgroundColor;
                    }
                    else
                    {
                        Console.BackgroundColor = backgroundColor;
                        Console.ForegroundColor = foregroundColor;
                    }

                    Console.Write(elements[i].PadCenter(maxLen));
                }

                // Перевод курсора в незадействованную область, чтоб не засорять меню.
                Console.SetCursorPosition(0, 20);

                // Сменить цвета текста и фона на начальные.
                Console.ForegroundColor = initialForegroundColor;
                Console.BackgroundColor = initialBackgroundColor;

                ConsoleKey consoleKey = Console.ReadKey().Key;

                switch (consoleKey)
                {

                    case ConsoleKey.Enter:
                        return pos;

                    case ConsoleKey.Escape:
                        return -1;

                    case ConsoleKey.UpArrow:
                        if (pos > 0)
                        {
                            pos--;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (pos < elements.Length - 1)
                        {
                            pos++;
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
