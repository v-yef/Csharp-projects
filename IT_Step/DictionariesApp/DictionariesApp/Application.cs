using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionariesApp
{
    internal class Application
    {
        private UserDictionary _currentDictionary = null;

        public Application()
        {
        }

        // Метод проверки наличия словаря с заданным сочетанием языков.
        // Принимает исходный и целевой языки.
        // Возвращает true, если словарь существует, иначе - false.
        private bool isExist(string sourceLanguage, string targetLanguage)
        {

            // Директория хранения словарей.
            string dictionaryPath = Directory.GetCurrentDirectory() + "\\..\\..\\Dictionaries\\";

            DirectoryInfo dir = new DirectoryInfo(dictionaryPath);

            if (dir.Exists == true)
            {

                FileInfo[] files = dir.GetFiles();

                // Проверить все имена файлов на совпадение с входящим названием.
                foreach (var file in files)
                {
                    if (file.Name == sourceLanguage + "-" + targetLanguage + ".xml")
                    {
                        return true;
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(dictionaryPath);
            }

            return false;
        }

        // Метод выбора языка словаря.
        // Принимает координаты левого верхнего угла меню и заголовок меню.
        // Возвращает строку, содержащую выбранный язык.
        private string chooseLanguage(int upLeft_x, int upLeft_y, string title)
        {

            this.mainWindow();

            // Сформировать массив доступных языков из перечисления языков.
            string[] languagesList = Enum.GetNames(typeof(Language));
            // Получить язык через меню выбора.
            int userChoice = Drawer.windowMenu(upLeft_x, upLeft_y, title, languagesList, ConsoleColor.Black, ConsoleColor.Gray);

            if (userChoice >= 0)
            {
                return languagesList[userChoice];
            }
            else
            {
                // Если при выборе была нажата клавиша ESC.
                return null;
            }
        }

        // Метод создания словаря.
        // Запрашивает у Пользователя выбор исходного и целевого языков, создает словарь и соответствующий ему файл.
        private void createNewDictionary()
        {

            string sourceLanguage = this.chooseLanguage(40, 5, " Исходный язык ");

            if (sourceLanguage == null)
            {
                return;
            }

            string targetLanguage = this.chooseLanguage(40, 5, " Целевой язык ");

            if (targetLanguage == null)
            {
                return;
            }

            if (sourceLanguage == targetLanguage)
            {
                Message.ShowError("Недопустимая языковая комбинация!");
                return;
            }

            if (isExist(sourceLanguage, targetLanguage) == true)
            {
                Message.ShowError("Такой словарь уже существует!");
                return;
            }

            this._currentDictionary = new UserDictionary(sourceLanguage, targetLanguage);

            if (this._currentDictionary != null)
            {
                this._currentDictionary.saveToFile(sourceLanguage + "-" + targetLanguage);
                Message.ShowMessage("Словарь создан!");
            }

            return;
        }

        // Метод выхода из программы.
        private void exitProgram() => Environment.Exit(0);

        // Метод отрисовки окна приложения.
        private void mainWindow() => drawWindow(0, 0, 100, 15, ConsoleColor.Black, ConsoleColor.White);

        private void drawWindow(int upLeft_x, int upLeft_y, int windowLength, int windowHeight,
                                ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            // Подложка для окна.
            Drawer.fillRectangleArea(upLeft_x, upLeft_y, windowLength, windowHeight, backgroundColor);

            // Зафиксировать начальные цвета текста и фона.
            ConsoleColor initialForegroundColor = Console.ForegroundColor;
            ConsoleColor initialBackgroundColor = Console.BackgroundColor;
            // Сменить цвета текста и фона на требуемые.
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;

            // Первая горизонтальная линия окна.
            Drawer.lineHorizontal(upLeft_x, upLeft_y, windowLength, '\u2550', foregroundColor, backgroundColor);
            // Вторая горизонтальная линия окна.
            Drawer.lineHorizontal(upLeft_x, upLeft_y + 2, windowLength, '\u2550', foregroundColor, backgroundColor);
            // Третья горизонтальная линия окна.
            Drawer.lineHorizontal(upLeft_x, windowHeight - 2, windowLength, '\u2550', foregroundColor, backgroundColor);
            // Четвертая горизонтальная линия окна.
            Drawer.lineHorizontal(upLeft_x, windowHeight, windowLength, '\u2550', foregroundColor, backgroundColor);
            // Первая вертикальная линия окна.
            Drawer.lineVertical(upLeft_x, upLeft_y, windowHeight, '\u2551', foregroundColor, backgroundColor);
            // Вторая вертикальная линия окна.
            Drawer.lineVertical(windowLength, upLeft_y, windowHeight, '\u2551', foregroundColor, backgroundColor);

            // Левый верхний угол окна.
            Console.SetCursorPosition(upLeft_x, upLeft_y);
            Console.Write('\u2554');
            // Правый верхний угол окна.
            Console.SetCursorPosition(windowLength, upLeft_y);
            Console.Write('\u2557');
            // Левый нижний угол окна.
            Console.SetCursorPosition(upLeft_x, windowHeight);
            Console.Write('\u255A');
            // Правый нижний угол окна.
            Console.SetCursorPosition(upLeft_x + windowLength, windowHeight);
            Console.Write('\u255D');

            // Первый сверху слева угловой разделитель между секциями окна.
            Console.SetCursorPosition(upLeft_x, upLeft_y + 2);
            Console.Write('\u2560');
            // Второй слева центральный угловой разделитель между секциями окна.
            Console.SetCursorPosition(upLeft_x, windowHeight - 2);
            Console.Write('\u2560');

            // Первый сверху справа угловой разделитель между секциями окна.
            Console.SetCursorPosition(windowLength, upLeft_y + 2);
            Console.Write('\u2563');
            // Второй сверху справа угловой разделитель между секциями окна.
            Console.SetCursorPosition(windowLength, windowHeight - 2);
            Console.Write('\u2563');

            // Заголовок окна.
            Console.SetCursorPosition(1, 1);
            Console.WriteLine("\u2591\u2591\u2591\u2592\u2592 СБОРНИК СЛОВАРЕЙ (ver1.0) \u2592\u2592\u2591\u2591\u2591".PadCenter(windowLength - 1));

            // Подсказки внизу окна.
            Drawer.textLabel(upLeft_x + 42, windowHeight - 1, "<\u2191\\\u2193>", ConsoleColor.White, ConsoleColor.Black);
            Drawer.textLabel(upLeft_x + 47, windowHeight - 1, " Вверх\\Вниз", ConsoleColor.Black, ConsoleColor.Gray);

            Drawer.textLabel(upLeft_x + 65, windowHeight - 1, "<ENTER>", ConsoleColor.White, ConsoleColor.Black);
            Drawer.textLabel(upLeft_x + 72, windowHeight - 1, " Выбор ", ConsoleColor.Black, ConsoleColor.Gray);

            Drawer.textLabel(upLeft_x + 85, windowHeight - 1, "<ESC>", ConsoleColor.White, ConsoleColor.Black);
            Drawer.textLabel(upLeft_x + 90, windowHeight - 1, " Выход", ConsoleColor.Black, ConsoleColor.Gray);

            // Сменить цвета текста и фона на начальные.
            Console.ForegroundColor = initialForegroundColor;
            Console.BackgroundColor = initialBackgroundColor;

            return;
        }

        // Метод загрузки словаря.
        // Загружает выбранный словарь и устанавливает его как текущий словарь Приложения.
        private void loadDictionary()
        {

            string dictionaryName = chooseDictionaryToLoad();

            if (dictionaryName == null)
            {
                return;
            }

            // Извлечение языков словаря из его названия.
            string[] languages = dictionaryName.Split('-');

            this._currentDictionary = new UserDictionary(languages[0], languages[1]);

            if (this._currentDictionary != null)
            {
                this._currentDictionary.loadFromFile(dictionaryName);
                this._currentDictionary.mainWindow();
            }

            return;
        }

        // Метод выбора словаря для загрузки.
        // Формирует список доступных словарей и запрашивает выбор Пользователя.
        // Возвращает строку, содержащую название выбранного словаря.
        private string chooseDictionaryToLoad()
        {

            this.mainWindow();

            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\..\\..\\Dictionaries\\");

            if (dir.Exists == false)
            {
                Message.ShowError("Директория загрузки не существует!");
                return null;
            }

            // Создание списка доступных словарей из директории их хранения для внесения их в меню выбора.
            FileInfo[] files = dir.GetFiles();

            if (files.Length == 0)
            {
                Message.ShowError("Директория загрузки пуста!");
                return null;
            }

            string[] fileNames = new string[files.Length];

            for (int i = 0; i < fileNames.Length; i++)
            {
                // Получить только имя файла до точки (без расширения).
                fileNames[i] = files[i].Name.Substring(0, files[i].Name.IndexOf('.'));
            }

            // Получить загружаемый словарь через меню выбора.
            int userChoice = Drawer.windowMenu(39, 5, " Выбор словаря ", fileNames, ConsoleColor.Black, ConsoleColor.Gray);

            if (userChoice >= 0)
            {
                return fileNames[userChoice];
            }
            else
            {
                return null;
            }
        }

        // Метод меню для работы с Приложением.
        public void mainMenu()
        {

            Console.Title = "Dictionaries_C#";

            while (true)
            {

                this.mainWindow();

                int userChoice = Drawer.windowMenu(37, 5, " Главное меню ",
                                                    new string[] { "Загрузить словарь",
                                                                   "Создать новый словарь",
                                                                   "Выход из программы" }, ConsoleColor.Black, ConsoleColor.Gray);
                switch (userChoice)
                {
                    case 0:
                        this.loadDictionary();
                        break;
                    case 1:
                        this.createNewDictionary();
                        break;
                    default:
                        this.exitProgram();
                        break;
                }
            }

            return;
        }
    }
}
