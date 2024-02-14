using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace DictionariesApp
{
    // Перечень доступных языков.
    enum Language
    {
        ENGLISH, RUSSIAN, UKRAINIAN
    }

    internal class UserDictionary
    {
        // Исходный язык.
        private string _sourceLanguage;

        // Целевой язык.
        private string _targetLanguage;

        // Контейнер для информации в словаре.
        private SortedDictionary<string, List<string>> _dictionary;

        // Текущее успешно найденное слово в словаре.
        private string _currentWord;

        // Делегат для метода расчета координаты курсора.
        private delegate int CursorCoordFunc(int n, int a);

        // Конструктор с 2мя параметрами, принимает исходный и целевой языки.
        public UserDictionary(string sourceLanguage, string targetLanguage)
        {

            this._sourceLanguage = sourceLanguage;
            this._targetLanguage = targetLanguage;
            this._dictionary = new SortedDictionary<string, List<string>>();
            this._currentWord = "";
        }

        // Метод добавления записи в словарь.
        // Запись представляет собой пару "строка на исходном языке - список строк перевода на целевом языке".
        // Добавление в словарь возможно только при корректных вводах исходного и целевого выражений.
        private void addEntry()
        {

            string sourceString = this.sourceStringInput();

            if (sourceString == null)
            {
                return;
            }

            if (this._dictionary.ContainsKey(sourceString) == true)
            {
                Message.ShowError("Такое слово уже есть в словаре!");
                return;
            }

            List<string> targetStrings = this.targetStringsInput();

            if (targetStrings == null)
            {
                return;
            }

            try
            {
                this._dictionary.Add(sourceString, targetStrings);
            }
            catch (Exception exception)
            {// Никогда не возникает, т.к. возможные причины отброшены ранее.
                Message.ShowError(exception.Message);
            }

            this.saveToFile(this._sourceLanguage + "-" + this._targetLanguage);

            Message.ShowMessage("Запись добавлена в словарь!");

            return;
        }

        // Метод проверки ввода.
        // Принимает проверяемую строку. 
        // Допускается ввод букв английского, русского и украиского алфавитов из 1-3 слов через пробелы и/или дефисы.
        // Смесь алфавитов не допускается. Слова могут начинаться с заглавной буквы.
        // Возвращает true при корректном воде, иначе возвращает false.
        private bool isInputGood(string inputString)
        {

            Regex regex = new Regex(@"^([A-Z]?[a-z]+((\s|\-)?[A-Z]?[a-z]+)*)|([А-ЯЁЇІЄҐ]?[а-яёїієґ']+((\s|\-)?[А-ЯЁЇІЄҐ]?[а-яёїієґ']+)*)", RegexOptions.Compiled);

            if (regex.IsMatch(inputString) == true)
            {
                return true;
            }
            else
            {
                Message.ShowError("Ввод некорректных символов!");
                return false;
            }
        }

        // Метод получения исходной строки от Пользователя.
        // Запрашивает ввод через окно ввода из VisualBasic.
        // Проверяет ввод, возвращая введенную строку при корректном вводе и null при некорректном или пустом вводе.
        private string sourceStringInput()
        {

            string userInput =
                Microsoft.VisualBasic.Interaction.InputBox("Введите исходное слово/сочетание :", "Ввод исходной строки");

            if (String.IsNullOrEmpty(userInput) == true)
            {
                return null;
            }

            if (isInputGood(userInput) == true)
            {
                return userInput;
            }
            else
            {
                return null;
            }
        }

        // Метод получения перевода от Пользователя.
        // Запрашивает ввод через окно ввода из VisualBasic. Допустимо ввести несколько переводов через ; без пробела.
        // Введённая строка разбивается по разделителю ;, формируя список строк. Каждая строка в списке проверяется на корректность ввода.
        // Возвращает сформированный список строк перевода при корректном вводе и null при некорректном или пустом вводе.
        private List<string> targetStringsInput()
        {

            string userInput =
                Microsoft.VisualBasic.Interaction.InputBox("Введите перевод слова/сочетания (через ; без пробела если несколько) :", "Ввод перевода");

            if (String.IsNullOrEmpty(userInput))
            {
                return null;
            }

            List<string> targetStrings = userInput.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (string targetWord in targetStrings)
            {
                if (isInputGood(targetWord) == false)
                {
                    return null;
                }
            }

            targetStrings.Sort();

            return targetStrings;
        }

        // Метод сохранения словаря в файл формата XML.
        #region Структура файла XML
        //-<words>
        //      -<word>
        //          <source>WORD_1</source>
        //          <target>TRANSLATION_1</target>
        //          <target>TRANSLATION_2</target>
        //          <target>TRANSLATION_3</target>
        //      </word>
        //      -<word>
        //          <source>WORD_2</source>
        //          <target>TRANSLATION_1</target>
        //      </word>
        //</words>
        #endregion
        public void saveToFile(string dictionaryName)
        {

            if (this._dictionary == null)
            {
                return;
            }

            XmlDocument xmlDoc = new XmlDocument();

            // Корневой узел.
            XmlNode rootNode = xmlDoc.CreateElement("words");
            xmlDoc.AppendChild(rootNode);

            if (this._dictionary.Count != 0)
            {

                foreach (KeyValuePair<string, List<string>> word in this._dictionary)
                {

                    // Узел записи в словаре.
                    XmlNode wordEntry = xmlDoc.CreateElement("word");

                    // Узел исходного слова.
                    XmlNode sourceWord = xmlDoc.CreateElement("source");
                    sourceWord.InnerText = word.Key;
                    wordEntry.AppendChild(sourceWord);

                    foreach (string translation in word.Value)
                    {

                        // Узел перевода слова.
                        XmlNode targetWord = xmlDoc.CreateElement("target");
                        targetWord.InnerText = translation;

                        wordEntry.AppendChild(targetWord);
                    }

                    rootNode.AppendChild(wordEntry);
                }
            }

            xmlDoc.Save(Directory.GetCurrentDirectory() + "\\..\\..\\Dictionaries\\" + dictionaryName + ".xml");

            return;
        }

        // Метод загрузки словаря из файла формата XML.
        #region Структура файла XML
        //-<words>
        //      -<word>
        //          <source>WORD_1</source>
        //          <target>TRANSLATION_1</target>
        //          <target>TRANSLATION_2</target>
        //          <target>TRANSLATION_3</target>
        //      </word>
        //      -<word>
        //          <source>WORD_2</source>
        //          <target>TRANSLATION_1</target>
        //      </word>
        //</words>
        #endregion
        public void loadFromFile(string dictionaryName)
        {

            if (this._dictionary == null)
            {
                return;
            }

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(Directory.GetCurrentDirectory() + "\\..\\..\\Dictionaries\\" + dictionaryName + ".xml");

            if (xmlDoc.DocumentElement == null)
            {
                return;
            }

            // Получить список узлов записей в словаре.
            XmlNodeList wordNodesList = xmlDoc.GetElementsByTagName("word");

            foreach (XmlNode wordNode in wordNodesList)
            {

                string sourceWord = "";

                List<string> targetWordsList = new List<string>();

                try
                {
                    XmlNodeList nodesList = wordNode.ChildNodes;

                    for (int i = 0; i < nodesList.Count; i++)
                    {
                        // Первый элемент списка - исходное слово.
                        if (i == 0)
                        {
                            sourceWord = nodesList[i].InnerText;
                            continue;
                        }
                        // Последующие элементы списка - переводы слова.
                        string targetWord = nodesList[i].InnerText;
                        targetWordsList.Add(targetWord);
                    }
                }
                catch (Exception e)
                {
                    Message.ShowError("Ошибка чтения базы переводов!");
                }

                try
                {
                    this._dictionary.Add(sourceWord, targetWordsList);
                }
                catch (Exception exception)
                {
                    Message.ShowError(exception.Message);
                }
            }

            return;
        }

        // Метод поиска перевода в словаре.
        // Принимает исходное слово.
        // Находит варианты перевода исходного слова в словаре и отображает их в области целевого языка.
        // Если соответствующая запись не найдена, информирует об этом.
        private void findEntry(string sourceWord)
        {

            bool isEntryFound = this._dictionary.TryGetValue(sourceWord, out List<string> targetWords);

            if (isEntryFound == true)
            {
                // Зафиксировать найденное слово для дальнейшей работы.
                this._currentWord = sourceWord;
                // Отобразить переводы найденного слова.
                this.showTargetStrings(targetWords);
            }
            else
            {
                Message.ShowMessage("Такое слово/сочетание в словаре отсутствует!");
                this._currentWord = null;
                this.clearTargetWindow();
                this.showSourceString(sourceWord);
            }

            return;
        }

        // Метод вывода переводов.
        // Принимает список переводов для данного слова.
        // Вывод переводов происходит в предназначенной для этого области.
        // Если длина перевода больше ширины области, вывод переносится на новую строку данной области.
        private void showTargetStrings(List<string> targetWords)
        {

            if (String.IsNullOrEmpty(this._currentWord) == true)
            {
                Message.ShowWarning("Введите исходное слово!");
                return;
            }

            // Зафиксировать исходную позицию курсора.
            int[] formerCoord = new int[2] { Console.CursorLeft, Console.CursorTop };

            this.clearTargetWindow();

            Console.SetCursorPosition(51, 5);

            int yPos = Console.CursorTop;

            int outputLines = 0;

            // Выводить переводы слова. Если перевод не влезает по ширине в область вывода, переносить вывод на следующую строку.
            // Переводы выводятся с новой строки с разделением ;.
            foreach (string item in targetWords)
            {

                for (int i = 0; i < item.Length; i++)
                {

                    if (Console.CursorLeft > 98)
                    {

                        outputLines++;

                        // Если суммарный перевод слова не помещается в окне вывода, прекратить вывод.
                        if (outputLines > 5)
                        {
                            Message.ShowWarning("Перевод данного слова не вмещается в область вывода!");
                            Console.SetCursorPosition(formerCoord[0], formerCoord[1]);
                            return;
                        }
                        // Перевод курсора на новую строку.
                        Console.SetCursorPosition(51, ++yPos);
                    }

                    Console.Write(item[i]);
                }

                Console.Write(";");
                Console.SetCursorPosition(51, ++yPos);

                outputLines++;
            }

            // Вернуть курсор в исходную позицию.
            Console.SetCursorPosition(formerCoord[0], formerCoord[1]);

            return;
        }

        // Метод вывода исходной строки.
        // Выводить переводы слова. Если перевод не влезает по ширине в область вывода, переносить вывод на следующую строку.
        private void showSourceString(string sourceString)
        {

            if (String.IsNullOrEmpty(this._currentWord) == true)
            {
                return;
            }

            Console.SetCursorPosition(1, 5);

            int yPos = Console.CursorTop;

            for (int i = 0; i < sourceString.Length; i++)
            {

                if (Console.CursorLeft > 48)
                {
                    Console.SetCursorPosition(1, ++yPos);
                }

                Console.Write(sourceString[i]);
            }

            return;
        }

        // Метод добавления строки перевода в словарь.
        // Добавляет новый список переводов к имеемому списку. Если встречается одинаковый перевод, не добавляет его.
        private void addTargetString()
        {

            if (String.IsNullOrEmpty(this._currentWord) == true)
            {
                Message.ShowWarning("Введите исходное слово!");
                return;
            }

            List<string> additionalTargetStrings = targetStringsInput();

            if (additionalTargetStrings == null)
            {
                return;
            }

            foreach (string targetString in additionalTargetStrings)
            {

                if (this._dictionary[this._currentWord].Contains(targetString) == false)
                {
                    this._dictionary[this._currentWord].Add(targetString);
                }
                else
                {
                    Message.ShowWarning("Обнаруженный дубликат перевода в словарь не добавлен!");
                }
            }

            this._dictionary[this._currentWord].Sort();

            this.refreshTargetWindow();

            this.saveToFile(this._sourceLanguage + "-" + this._targetLanguage);

            return;
        }

        // Метод редактирования строки на исходном языке.
        // Для редактирования исходной строки используется окно ввода VisualBasic, в котором вводом по умолчанию является имеемая исходная строка,
        // редактируемая непосредственно в этом окне. Результат редактирования проверяется на корректность.
        private void editSourceString()
        {

            if (String.IsNullOrEmpty(this._currentWord) == true)
            {
                Message.ShowWarning("Введите исходное слово!");
                return;
            }

            string userInput =
                Microsoft.VisualBasic.Interaction.InputBox("Отредактируйте исходное слово/сочетание :", "Редактирование исходной строки", this._currentWord);

            if (userInput == "")
            {
                Message.ShowError("Невозможно заменить слово на пустую строку!");
                return;
            }

            if (isInputGood(userInput) == true)
            {
                // Получить список переводов по данному ключу и сохранить его под новым ключом,
                // после чего удалить прежнюю запись и обновить область ввода.
                this._dictionary.TryGetValue(this._currentWord, out List<string> targetWords);
                this._dictionary.Add(userInput, targetWords);
                this._dictionary.Remove(this._currentWord);
                this.saveToFile(this._sourceLanguage + "-" + this._targetLanguage);

                this._currentWord = userInput;

                this.refreshSourceWindow();
            }

            return;
        }

        // Метод редактирования перевода.
        // Для редактирования переводов используется окно ввода VisualBasic, в котором вводом по умолчанию является имеемый перевод,
        // редактируемый непосредственно в этом окне. Результат редактирования проверяется на корректность.
        private void editTargetStrings()
        {

            if (String.IsNullOrEmpty(this._currentWord) == true)
            {
                Message.ShowWarning("Введите исходное слово!");
                return;
            }

            this._dictionary.TryGetValue(this._currentWord, out List<string> targetStrings_Old);

            // Список переводов формирует строку, в которой переводы разделены ;.
            string result = "";

            foreach (var item in targetStrings_Old)
            {
                result += item + ";";
            }

            string userInput =
             Microsoft.VisualBasic.Interaction.InputBox("Отредактируйте перевод слова/сочетания (через ; без пробела если несколько) :", "Ввод перевода", result);

            if (String.IsNullOrEmpty(userInput))
            {
                Message.ShowError("Невозможно удалить все переводы!");

                return;
            }

            // После ввода строка разбивается и формируется новый список переводов.
            List<string> targetStrings_New = userInput.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (string targetWord in targetStrings_New)
            {

                if (isInputGood(targetWord) == false)
                {
                    return;
                }
            }

            targetStrings_New.Sort();

            // Новый список переводов сохраняется как значение по текущему ключу.
            this._dictionary[this._currentWord] = targetStrings_New;

            Message.ShowMessage("Перевод обновлён!");

            this.refreshTargetWindow();

            this.saveToFile(this._sourceLanguage + "-" + this._targetLanguage);

            return;
        }

        // Метод удаления записи из словаря.
        // Удаляет текущее обрабатываемое исходное слово и все его переводы.
        private void removeEntry()
        {

            if (String.IsNullOrEmpty(this._currentWord) == true)
            {
                Message.ShowWarning("Введите исходное слово!");
                return;
            }

            this._dictionary.Remove(this._currentWord);

            this._currentWord = null;

            this.clearSourceWindow();
            this.clearTargetWindow();

            this.saveToFile(this._sourceLanguage + "-" + this._targetLanguage);

            Message.ShowMessage("Запись удалена из словаря!");

            Console.SetCursorPosition(1, 5);

            return;
        }

        // Метод сохранения текущей записи в файл.
        private void saveEntryToFile()
        {

            if (String.IsNullOrEmpty(this._currentWord) == true)
            {
                Message.ShowWarning("Введите исходное слово!");
                return;
            }

            // Директория для выходного файла.
            string savePath = Directory.GetCurrentDirectory() + "\\..\\..\\Output";

            if (Directory.Exists(savePath) == false)
            {
                Directory.CreateDirectory(savePath);
            }

            using (FileStream fs = new FileStream(savePath + "\\output.txt", FileMode.Append))
            {

                using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                {

                    sw.WriteLine("Исходное слово : ");
                    sw.WriteLine(this._currentWord);
                    sw.WriteLine();

                    this._dictionary.TryGetValue(this._currentWord, out List<string> writeList);

                    sw.WriteLine("Переводы слова : ");
                    foreach (var targetString in writeList)
                    {
                        sw.WriteLine(targetString);
                    }

                    sw.WriteLine();
                    sw.WriteLine(">====================<");
                    sw.WriteLine();

                    Message.ShowMessage("Запись выведена в файл!");
                }
            }

            return;
        }

        // Метод очистки окна исходного языка.
        private void clearSourceWindow()
        {

            Drawer.fillRectangleArea(1, 5, 49, 6, ConsoleColor.White);
            Console.SetCursorPosition(1, 5);

            return;
        }

        // Метод очистки окна целевого языка.
        private void clearTargetWindow() => Drawer.fillRectangleArea(51, 5, 49, 6, ConsoleColor.White);

        // Метод обновления окна исходного языка.
        private void refreshSourceWindow()
        {

            this.clearSourceWindow();
            this.showSourceString(this._currentWord);

            return;
        }

        // Метод обновления окна целевого языка.
        private void refreshTargetWindow()
        {

            this._dictionary.TryGetValue(this._currentWord, out List<string> targetWords);
            this.showTargetStrings(targetWords);

            return;
        }

        // Метод создания и обработки окна словаря.
        public void mainWindow()
        {

            this.drawWindow(0, 0, 100, 15, ConsoleColor.Black, ConsoleColor.White);
            this.processWindowInput(47, ConsoleColor.White, ConsoleColor.Black);

            return;
        }

        // Метод обработки ввода в окне словаря.
        private void processWindowInput(int maxLineLength, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;

            Console.SetCursorPosition(1, 5);
            Console.CursorVisible = true;

            string inputResult = "";
            int inputLines = 0;

            int x = Console.CursorLeft;
            int y = Console.CursorTop;

            // Выражение для расчета Х-координаты курсора.
            CursorCoordFunc xCoord = (int lines, int length) =>
            {

                if (inputLines == 0)
                    return inputResult.Length + 1;
                else
                    return inputResult.Length - inputLines * maxLineLength + 1;
            };

            while (true)
            {

                var k = Console.ReadKey(true);

                switch (k.Key)
                {

                    case ConsoleKey.Enter:
                        this.findEntry(inputResult);

                        if (String.IsNullOrEmpty(inputResult) == true)
                        {
                            Console.SetCursorPosition(1, 5);
                        }
                        else
                        {
                            Console.SetCursorPosition(xCoord(inputLines, inputResult.Length), 5 + inputLines);
                        }
                        break;

                    case ConsoleKey.Backspace:
                        if (inputResult.Length > 0)
                        {

                            inputResult = inputResult.Remove(inputResult.Length - 1, 1);

                            if (Console.CursorLeft > 1)
                            {
                                Console.Write($"{k.KeyChar} {k.KeyChar}");
                            }

                            if ((Console.CursorLeft == 1) && (inputLines > 0))
                            {

                                if (y > 5)
                                {
                                    Console.SetCursorPosition(maxLineLength + 1, --y);
                                }

                                if ((Console.CursorLeft > 1) && (inputLines > 0))
                                {
                                    Console.Write($"{k.KeyChar} {k.KeyChar}");
                                }

                                inputLines--;
                            }
                        }
                        break;

                    case ConsoleKey.D1:
                        this.editSourceString();
                        break;

                    case ConsoleKey.D2:
                        this.clearSourceWindow();
                        this._currentWord = null;
                        inputResult = "";
                        inputLines = 0;
                        break;

                    case ConsoleKey.D3:
                        this.editTargetStrings();
                        break;

                    case ConsoleKey.D4:
                        this.addTargetString();
                        break;

                    case ConsoleKey.D5:
                        this.addEntry();
                        break;

                    case ConsoleKey.D6:
                        this.saveEntryToFile();
                        break;

                    case ConsoleKey.Delete:
                        this.removeEntry();
                        inputResult = "";
                        inputLines = 0;
                        break;

                    case ConsoleKey.Escape:
                        return;

                    default:
                        // В окне допускается вводить только буквы, пробелы и дефисы.
                        if (char.IsLetter(k.KeyChar) || k.KeyChar == '-' || k.KeyChar == ' ')
                        {

                            if (inputLines < 6)
                            {

                                inputResult += k.KeyChar;
                                Console.Write(k.KeyChar);

                                if (Console.CursorLeft == maxLineLength + 1)
                                {

                                    if (inputLines < 5)
                                    {
                                        Console.SetCursorPosition(x, ++y);
                                    }
                                    inputLines++;
                                }
                            }
                        }
                        break;
                }
            }
        }

        // Метод отрисовки окна словаря.
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
            Drawer.lineHorizontal(upLeft_x, upLeft_y + 4, windowLength, '\u2500', foregroundColor, backgroundColor);
            // Четвертая горизонтальная линия окна.
            Drawer.lineHorizontal(upLeft_x, windowHeight - 2, windowLength, '\u2550', foregroundColor, backgroundColor);
            // Пятая горизонтальная линия окна.
            Drawer.lineHorizontal(upLeft_x, windowHeight - 4, windowLength, '\u2550', foregroundColor, backgroundColor);
            // Шестая горизонтальная линия окна.
            Drawer.lineHorizontal(upLeft_x, windowHeight, windowLength, '\u2550', foregroundColor, backgroundColor);
            // Первая вертикальная линия окна.
            Drawer.lineVertical(upLeft_x, upLeft_y, windowHeight, '\u2551', foregroundColor, backgroundColor);
            // Вторая вертикальная линия окна.
            Drawer.lineVertical(windowLength / 2, upLeft_y + 3, windowHeight - 4, '\u2551', foregroundColor, backgroundColor);
            // Третья вертикальная линия окна.
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
            // Второй сверху слева угловой разделитель между секциями окна.
            Console.SetCursorPosition(upLeft_x, upLeft_y + 4);
            Console.Write('\u255F');
            // Третий сверху слева угловой разделитель между секциями окна.
            Console.SetCursorPosition(upLeft_x, windowHeight - 2);
            Console.Write('\u2560');
            // Четвертый сверху слева угловой разделитель между секциями окна.
            Console.SetCursorPosition(upLeft_x, windowHeight - 4);
            Console.Write('\u2560');

            // Первый сверху центральный угловой разделитель между секциями окна.
            Console.SetCursorPosition(windowLength / 2, upLeft_y + 2);
            Console.Write('\u2566');
            // Второй сверху центральный угловой разделитель между секциями окна.
            Console.SetCursorPosition(windowLength / 2, upLeft_y + 4);
            Console.Write('\u256B');
            // Третий сверху центральный угловой разделитель между секциями окна.
            Console.SetCursorPosition(windowLength / 2, windowHeight - 2);
            Console.Write('\u2569');
            // Четвертый сверху центральный угловой разделитель между секциями окна.
            Console.SetCursorPosition(windowLength / 2, windowHeight - 4);
            Console.Write('\u256C');

            // Первый сверху справа угловой разделитель между секциями окна.
            Console.SetCursorPosition(windowLength, upLeft_y + 2);
            Console.Write('\u2563');
            // Второй сверху справа угловой разделитель между секциями окна.
            Console.SetCursorPosition(windowLength, upLeft_y + 4);
            Console.Write('\u2562');
            // Третий сверху справа угловой разделитель между секциями окна.
            Console.SetCursorPosition(windowLength, windowHeight - 2);
            Console.Write('\u2563');
            // Четвертый сверху справа угловой разделитель между секциями окна.
            Console.SetCursorPosition(windowLength, windowHeight - 4);
            Console.Write('\u2563');

            // Заголовок окна.
            Console.SetCursorPosition(1, 1);
            Console.WriteLine("\u2591\u2591\u2591\u2592\u2592 СЛОВАРЬ (ver1.0) \u2592\u2592\u2591\u2591\u2591".PadCenter(windowLength - 1));

            Console.SetCursorPosition(1, 3);
            Console.WriteLine(("<<< " + this._sourceLanguage + " >>>").PadCenter(49));

            Console.SetCursorPosition(51, 3);
            Console.WriteLine(("<<< " + this._targetLanguage + " >>>").PadCenter(49));

            // Подсказки внизу окна.
            Drawer.textLabel(upLeft_x + 3, windowHeight - 3, "<1>", ConsoleColor.White, ConsoleColor.Black);
            Drawer.textLabel(upLeft_x + 6, windowHeight - 3, " Редакт. исх. слово", ConsoleColor.Black, ConsoleColor.Gray);

            Drawer.textLabel(upLeft_x + 30, windowHeight - 3, "<2>", ConsoleColor.White, ConsoleColor.Black);
            Drawer.textLabel(upLeft_x + 33, windowHeight - 3, " Очистить ввод", ConsoleColor.Black, ConsoleColor.Gray);

            Drawer.textLabel(upLeft_x + 53, windowHeight - 3, "<3>", ConsoleColor.White, ConsoleColor.Black);
            Drawer.textLabel(upLeft_x + 56, windowHeight - 3, " Редактировать перевод", ConsoleColor.Black, ConsoleColor.Gray);

            Drawer.textLabel(upLeft_x + 80, windowHeight - 3, "<4>", ConsoleColor.White, ConsoleColor.Black);
            Drawer.textLabel(upLeft_x + 83, windowHeight - 3, " Добавить перевод", ConsoleColor.Black, ConsoleColor.Gray);

            Drawer.textLabel(upLeft_x + 3, windowHeight - 1, "<5>", ConsoleColor.White, ConsoleColor.Black);
            Drawer.textLabel(upLeft_x + 6, windowHeight - 1, " Добавить запись в словарь", ConsoleColor.Black, ConsoleColor.Gray);

            Drawer.textLabel(upLeft_x + 35, windowHeight - 1, "<6>", ConsoleColor.White, ConsoleColor.Black);
            Drawer.textLabel(upLeft_x + 38, windowHeight - 1, " Сохранить запись в файл", ConsoleColor.Black, ConsoleColor.Gray);

            Drawer.textLabel(upLeft_x + 65, windowHeight - 1, "<DEL>", ConsoleColor.White, ConsoleColor.Black);
            Drawer.textLabel(upLeft_x + 70, windowHeight - 1, " Удалить запись", ConsoleColor.Black, ConsoleColor.Gray);

            Drawer.textLabel(upLeft_x + 88, windowHeight - 1, "<ESC>", ConsoleColor.White, ConsoleColor.Black);
            Drawer.textLabel(upLeft_x + 93, windowHeight - 1, " Выход", ConsoleColor.Black, ConsoleColor.Gray);

            // Сменить цвета текста и фона на начальные.
            Console.ForegroundColor = initialForegroundColor;
            Console.BackgroundColor = initialBackgroundColor;

            return;
        }
    }
}
