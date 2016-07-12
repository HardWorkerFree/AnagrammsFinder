using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Anagramms
{
    public sealed class Anagramms
    {
        #region Private_variables

        /// <summary>
        /// Словарь анаграмм - для удобства поиска существующих анаграмм, либо добавления новых.
        /// </summary>
        private Dictionary<string, string> _anagrammsDictionary;

        /// <summary>
        /// Имя файла источника (код написан для файлов с кодировкой UTF-8).
        /// </summary>
        private string _sourceFileName;

        /// <summary>
        /// Имя файла с результатом.
        /// </summary>
        private string _outputFileName;

        /// <summary>
        /// Паттерн регулярного выражения, с помощью которого будет осуществляться поиск слов в тексте.
        /// </summary>
        private string _regexPattern;

        /// <summary>
        /// Регулярное выражение на основе определенного паттерна.
        /// </summary>                                            
        private Regex _regex;

        #endregion Private_variables

        public Anagramms()
        {
            _anagrammsDictionary = new Dictionary<string, string>();
            _sourceFileName = @"anagramms.txt";
            _outputFileName = @"result.txt";
            _regexPattern = @"\b[а-я]+\b";
            _regex = new Regex(_regexPattern, RegexOptions.IgnoreCase);

            //сделать два метода, один для файлов по умолчанию, в другом дать пользователю право выбора откуда брать и куда сохранять результат.
        }

        #region Private_methods

        /// <summary>
        /// Осуществляет поиск анаграмм в файле.
        /// </summary>
        /// <param name="fileName">Имя файла источника (код написан для файлов с кодировкой UTF-8).</param>
        private void FindAnagrammsInFile(string fileName)
        {
            string sourceFileName = fileName;

            try
            {
                using (StreamReader streamReader = new StreamReader(sourceFileName))
                {
                    string textString = "";

                    do
                    {
                        textString = streamReader.ReadLine();                            //Файл читается построчно 
                        FindWordsAndCreateAnagramms(textString);                         //В выбранной строке осуществляется поиск слов

                    } while (!streamReader.EndOfStream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка чтения файла. " + ex.Message);
            }
        }

        /// <summary>
        /// Выполняет поиск всех слов в строке и создает анаграммы.
        /// </summary>
        /// <param name="stringOfText">Строка, в которой необходимо осуществить поиск.</param>
        private void FindWordsAndCreateAnagramms(string stringOfText)
        {
            string textString = stringOfText;
            string word = "";

            MatchCollection regexMatches = this._regex.Matches(textString);             //Поиск всех слов в строке текста
            if (regexMatches.Count > 0)                                                 //Если есть слова в строке - то создать анаграмму для каждого слова
            {
                foreach (Match match in regexMatches)
                {
                    word = match.Value.ToLower();                                       //Перевод всех букв слова в нижний регистр
                    CreateAnagramm(word);
                }
            }
        }

        /// <summary>
        /// Создает новый элемент с ключом-анаграммой в словаре анаграмм либо добавляет слово, если данная анаграмма уже существует.
        /// </summary>
        /// <param name="word">Слово, из которого необходимо создать анаграмму.</param>
        private void CreateAnagramm(string word)
        {
            string anagrammWord = word;
            string anagrammKey;

            anagrammKey = AlphabetizeLetters(anagrammWord);                              //Для создания ключа-анаграммы необходимо отсортировать все буквы в слове по алфавиту
            if (!this._anagrammsDictionary.ContainsKey(anagrammKey))
            {
                CreateAnagrammInDictionary(anagrammKey);
                AddWordToAnagrammsDictionary(anagrammKey, anagrammWord);
            }
            else
            {
                AddWordToAnagrammsDictionary(anagrammKey, anagrammWord);
            }
        }

        /// <summary>
        /// Сортирует буквы в слове по алфавиту.
        /// </summary>
        /// <param name="word">слово, которое необходимо отсортировать.</param>
        /// <returns>Возвращает слово с отсортированными по алфавиту буквами.</returns>
        private static string AlphabetizeLetters(string word)
        {
            string ABCword = word;

            char[] alphabetLetters = ABCword.ToCharArray();
            Array.Sort(alphabetLetters);
            
            return new string(alphabetLetters);
        }

        /// <summary>
        /// Создает пустой элемент в словаре анаграмм по ключу-анаграмме.
        /// </summary>
        /// <param name="key">Анаграмма-ключ.</param>
        private void CreateAnagrammInDictionary(string key)
        {
            string anagrammKey = key;

            this._anagrammsDictionary.Add(anagrammKey, "");
        }

        /// <summary>
        /// Добавляет слово в словарь анаграмм, если данного слова там ранее не существовало.
        /// </summary>
        /// <param name="key">Анаграмма-ключ.</param>
        /// <param name="word">Слово, которое необходимо добавить.</param>
        private void AddWordToAnagrammsDictionary(string key, string word)
        {
            string anagrammKey = key;
            string anagrammWord = word;

            if (!this._anagrammsDictionary[anagrammKey].Contains(anagrammWord))
            {
                this._anagrammsDictionary[anagrammKey] += anagrammWord + " ";
            }
        }

        /// <summary>
        /// Сохраняет словарь анаграмм в файл построчно, исключив при этом слова, не имеющие анаграмм.
        /// </summary>
        /// <param name="fileName">Имя файла, в который необходимо сохранить результат.</param>
        private void SaveAnagrammsToFile(string fileName)
        {
            string outputFileName = fileName;

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(outputFileName, false))
                {
                    foreach (string anagramm in _anagrammsDictionary.Values)
                    {
                        MatchCollection matches = this._regex.Matches(anagramm);
                        if (matches.Count != 1)
                        {
                            streamWriter.WriteLine(anagramm);                                   //Запись в файл осуществляется построчно
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Ошибка записи в файл. " + ex.Message);
            }
        }

        #endregion Private_methods

        #region Public_methods

        /// <summary>
        /// Создает файл анаграмм (result.txt) из стандартного файла (anagramms.txt), находящегося в папке с программой. 
        /// </summary>
        public void CreateFileOfAnagrammsFromFile()
        {
            this.FindAnagrammsInFile(_sourceFileName);
            this.SaveAnagrammsToFile(_outputFileName);
        }

        /// <summary>
        /// Создает файл анаграмм из входного файла с кодировкой UTF-8.
        /// </summary>
        /// <param name="inputFileName">Имя входного файла</param>
        /// <param name="outputFileName">Имя выходного файла</param>
        public void CreateFileOfAnagrammsFromFile(string inputFileName, string outputFileName)
        {
            this.FindAnagrammsInFile(inputFileName);
            this.SaveAnagrammsToFile(outputFileName);
        }

        #endregion Public_methods
    }
}
