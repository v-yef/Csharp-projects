using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionariesApp
{
    internal class Drawer
    {
        // Метод заполнения прямоугольной области заданным цветом.
        // Принимает координаты верхнего левого угла области, длину и высоту обасти, а также цвет заливки.
        public static void fillRectangleArea(int upLeft_x, int upLeft_y, int areaLength, int areaHeight,
                                                ConsoleColor backgroundColor)
        {
            // Зафиксировать начальное положение курсора.
            int[] formerCoord = new int[2] { Console.CursorLeft, Console.CursorTop };
            // Зафиксировать начальный цвет фона.
            ConsoleColor initialBackgroundColor = Console.BackgroundColor;
            // Сменить цвет фона на требуемый.
            Console.BackgroundColor = backgroundColor;
            // Заполнить пространство как двумерный массив пустых символов.
            for (int i = 0; i < areaHeight; i++)
            {
                Console.SetCursorPosition(upLeft_x, upLeft_y + i);
                for (int j = 0; j < areaLength; j++)
                {
                    Console.Write(" ");
                }
            }
            // Вернуть курсор в начальное положение.
            Console.SetCursorPosition(formerCoord[0], formerCoord[1]);
            // Сменить цвет фона на начальный.
            Console.BackgroundColor = initialBackgroundColor;

            return;
        }

        // Метод вывода прямоугольника.
        // Принимает координаты левого верхнего угла прямоугольника, его ширину и высоту, а также символы для отображения
        // сторон прямоугольника по горизонтали, вертикали и каждого угла прямоугольника. Также принимет цвет символов и фона.
        public static void rectangle(int upLeft_x, int upLeft_y, int rectangleLength, int rectangleHeight, char horizontSymb, char verticSymb,
                                                char upperLeftCornerSymb, char upperRightCornerSymb, char lowerLeftCornerSymb, char lowerRightCornerSymb,
                                                ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            // Зафиксировать начальные цвета текста и фона.
            ConsoleColor initialForegroundColor = Console.ForegroundColor;
            ConsoleColor initialBackgroundColor = Console.BackgroundColor;
            // Сменить цвета текста и фона на требуемые.
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;

            // Горизонтальная линия сверху.
            lineHorizontal(upLeft_x, upLeft_y, rectangleLength, horizontSymb, foregroundColor, backgroundColor);
            // Горизонтпльная линия снизу.
            lineHorizontal(upLeft_x, upLeft_y + rectangleHeight, rectangleLength, horizontSymb, foregroundColor, backgroundColor);
            // Вертикальная линия слева.
            lineVertical(upLeft_x, upLeft_y, rectangleHeight, verticSymb, foregroundColor, backgroundColor);
            // Вертикальная линия справа.
            lineVertical(upLeft_x + rectangleLength, upLeft_y, rectangleHeight, verticSymb, foregroundColor, backgroundColor);

            // Левый верхний угол.
            Console.SetCursorPosition(upLeft_x, upLeft_y);
            Console.Write(upperLeftCornerSymb);
            // Правый верхний угол.
            Console.SetCursorPosition(upLeft_x + rectangleLength, upLeft_y);
            Console.Write(upperRightCornerSymb);
            // Левый нижний угол.
            Console.SetCursorPosition(upLeft_x, upLeft_y + rectangleHeight);
            Console.Write(lowerLeftCornerSymb);
            // Правый нижний угол.
            Console.SetCursorPosition(upLeft_x + rectangleLength, upLeft_y + rectangleHeight);
            Console.Write(lowerRightCornerSymb);

            // Сменить цвета текста и фона на начальные.
            Console.ForegroundColor = initialForegroundColor;
            Console.BackgroundColor = initialBackgroundColor;

            return;
        }

        // Метод вывода горизонтальной линии.
        // Принимает координаты начала, длину, символ рисования, а также цвет символа и фона.
        public static void lineHorizontal(int x_begin, int y_begin, int length, char symbol,
                                              ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            // Зафиксировать начальные цвета текста и фона.
            ConsoleColor initialForegroundColor = Console.ForegroundColor;
            ConsoleColor initialBackgroundColor = Console.BackgroundColor;
            // Сменить цвета текста и фона на требуемые.
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            // Вывести горизонтальную линию из заданного символа.
            for (int i = 0; i < length; i++)
            {
                Console.SetCursorPosition(x_begin + i, y_begin);
                Console.Write(symbol);
            }
            // Сменить цвета текста и фона на начальные.
            Console.ForegroundColor = initialForegroundColor;
            Console.BackgroundColor = initialBackgroundColor;

            return;
        }

        // Метод вывода вертикальной линии.
        // Принимает координаты начала, длину, символ рисования, а также цвет символа и фона.
        public static void lineVertical(int x_begin, int y_begin, int length, char symbol,
                                            ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            // Зафиксировать начальные цвета текста и фона.
            ConsoleColor initialForegroundColor = Console.ForegroundColor;
            ConsoleColor initialBackgroundColor = Console.BackgroundColor;
            // Сменить цвета текста и фона на требуемые.
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            // Вывести вертикальную линию из заданного символа.
            for (int i = 0; i < length; i++)
            {
                Console.SetCursorPosition(x_begin, y_begin + i);
                Console.Write(symbol);
            }
            // Сменить цвета текста и фона на начальные.
            Console.ForegroundColor = initialForegroundColor;
            Console.BackgroundColor = initialBackgroundColor;

            return;
        }

        // Метод вывода текста по координатам.
        // Принимает координаты первого символа, текст, его цвет и цвет фона.
        public static void textLabel(int upLeft_x, int upLeft_y, string text,
                                        ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            // Зафиксировать начальные цвета текста и фона.
            ConsoleColor initialForegroundColor = Console.ForegroundColor;
            ConsoleColor initialBackgroundColor = Console.BackgroundColor;
            // Сменить цвета текста и фона на требуемые.
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            // Вывести требуемый текст.
            Console.SetCursorPosition(upLeft_x, upLeft_y);
            Console.Write(text);
            // Сменить цвета текста и фона на начальные.
            Console.ForegroundColor = initialForegroundColor;
            Console.BackgroundColor = initialBackgroundColor;

            return;
        }

        // Метод вычисления X-координаты строки, центрированной по заданной ширине области.
        // Принимает X-координату начала, ширину области и длину самой строки.
        // Возвращает Х-координату, при которой строка будет распологаться в центре области.
        private static int centeredTextX(int xCoord, int areaLen, int textLen) =>
            xCoord + Math.Abs(areaLen - textLen) / 2;

        // Метод вывода оконного меню.
        // Принимает ккординаты верхнего левого угла окна, заголовок угла, массив позиций меню,
        // а также цвет текста и фона окна меню.
        // Возвращает индекс выбранного элемента или -1 при нажатии ESC (выход).
        public static int windowMenu(int upLeft_x, int upLeft_y, string title, string[] items,
                                        ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            int maxItemMenu = 0;
            int maxItemLen = 0;

            foreach (var item in items)
            {
                if (item.Length > maxItemMenu)
                {
                    maxItemMenu = item.Length;
                }
            }

            if (title.Length > maxItemMenu)
            {
                maxItemLen = title.Length;
            }
            else
            {
                maxItemLen = maxItemMenu;
            }

            int fillingLength = maxItemLen + 4;
            int fillingHeight = items.Length + 1;

            Drawer.fillRectangleArea(upLeft_x, upLeft_y, fillingLength, fillingHeight, backgroundColor);

            Drawer.rectangle(upLeft_x, upLeft_y, fillingLength, fillingHeight,
                            '\u2550', '\u2551', '\u2554', '\u2557', '\u255A', '\u255D', foregroundColor, backgroundColor);

            Drawer.textLabel(centeredTextX(upLeft_x, fillingLength, title.Length), upLeft_y, title,
                                foregroundColor, backgroundColor);

            return Menu.VerticalMenu(items, centeredTextX(upLeft_x, fillingLength, maxItemMenu), upLeft_y + 1,
                                        ConsoleColor.Black, ConsoleColor.Gray);
        }
    }
}
