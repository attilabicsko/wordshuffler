using System;
using System.Collections.Generic;
using System.Linq;
using WordShuffler.Models;

namespace Models
{
    public class CharMatrix
    {
        

        public char[,] GetMatrix()
        {
            return _matrix;
        }

        public char GetCharAt(CharCoordinate coordinate)
        {
            return _matrix[coordinate.Row, coordinate.Column];
        }

        public CharMatrix(int size, List<string> words)
        {
            _size = size;
            _words = words;

            _rnd = new Random();
            _matrix = new char[_size, _size];


            //-size to make sure there will be a few random characters too
            var baseWord = GetRadnomWord(CellCount - size);

            WriteWord(baseWord.ToCharArray());
            FillEmptyCellsRandom();
        }

        public CharMatrix(List<string> words, char[,] matrix)
        {
            _size = Convert.ToInt32(Math.Sqrt(matrix.Length));
            _words = words;
            _matrix = matrix;
        }


        public int Size
        {
            get
            {
                return _size;
            }

        }

        public IEnumerable<CharCoordinate> GetAllCoordinates()
        {
            return GetAllCoordinates(Size);
        }

        public static IEnumerable<CharCoordinate> GetAllCoordinates(int size)
        {
            var all = new List<CharCoordinate>();

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    all.Add(new CharCoordinate(i, j));
                }
            }

            return all;
        }

        private string GetRadnomWord(int maxLength, int maxDivergency = 3)
        {
            var longestWord = _words.OrderByDescending(x => x.Length).FirstOrDefault();
            if (longestWord != null)
            {
                var longestWordLength = longestWord.Length;

                if (maxLength > longestWordLength)
                    maxLength = longestWordLength;
            }

            var minLength = maxLength > maxDivergency ? maxLength - maxDivergency : maxLength;

            var longWords = _words.Where(x => x.Length > minLength && x.Length < maxLength);

            if (!longWords.Any()) return longestWord;

            var rnd = new Random();
            int count = longWords.Count();
            var index = rnd.Next(0, count);
            return longWords.ElementAt(index);
        }

        #region fields


        private readonly Random _rnd;
        //private char[] ValidCharacters;
        private char[] _validRandomCharacters = new char[] { 'A', 'Á', 'B', 'C', 'D', 'E', 'É', 'F', 'G', 'H', 'I', 'Í', 'J', 'K', 'L', 'M', 'N', 'O', 'Ó', 'Ö', 'Ő', 'P', 'R', 'S', 'T', 'U', 'Ú', 'Ü', 'Ű', 'V', 'Y', 'Z' };
        private readonly char[,] _matrix;
        private readonly List<string> _words;
        private readonly int _size;
        private int CellCount { get { return _size * _size; } }

        #endregion

        #region private methods

        #region building the matrix

        private void WriteWord(char[] word)
        {
            var charCount = word.Length;

            if (charCount > GetEmptyCells().Count())
                throw new InvalidOperationException("Data longer then empty cells count");


            WriteWord(word, new CharCoordinate[charCount], new List<CharCoordinate>[charCount], -1, false); //Initial conditions

            var valid = word.Length + GetEmptyCells().Count == CellCount;

            //Debug purposes
            if (!valid)
            {
                Console.WriteLine("!!! INVALID !!!");
            }

        }

        private void WriteWord(char[] word, CharCoordinate[] path, List<CharCoordinate>[] triedCoordinatesAtIndex, int lastCoordinateIndex, bool steppedBack, int runCount = 0)
        {
            runCount++;

            if (runCount == Int32.MaxValue) throw new Exception("Túl sokszor futott a dolog!");

            var charCount = word.Length;

            var currentCoordinateIndex = lastCoordinateIndex + 1;

            if (currentCoordinateIndex == 0)
                path[0] = GetRandomEmptyCell();

            var currentIsTrapped = (currentCoordinateIndex > 0) && (IsTrapped(path[currentCoordinateIndex - 1], triedCoordinatesAtIndex[currentCoordinateIndex]));

            if (currentIsTrapped)
            {
                DelCharAt(path[lastCoordinateIndex]);
                path[currentCoordinateIndex] = null;
                triedCoordinatesAtIndex[currentCoordinateIndex] = null;
                currentCoordinateIndex--;

                if (steppedBack)
                {
                    currentCoordinateIndex--;
                }
            }
            else
            {
                var currentCoordinate = currentCoordinateIndex == 0
                    ? path[currentCoordinateIndex]
                    : GetRandomEmptySibling(path[currentCoordinateIndex - 1], except: triedCoordinatesAtIndex[currentCoordinateIndex]);
                path[currentCoordinateIndex] = currentCoordinate;
                triedCoordinatesAtIndex[currentCoordinateIndex] = triedCoordinatesAtIndex[currentCoordinateIndex] ?? new List<CharCoordinate>();
                triedCoordinatesAtIndex[currentCoordinateIndex].Add(currentCoordinate);
                SetCharAt(currentCoordinate, word[currentCoordinateIndex]);
            }

            if (currentCoordinateIndex < charCount - 1 || currentIsTrapped)
            {
                WriteWord(word, path, triedCoordinatesAtIndex, currentCoordinateIndex, currentIsTrapped, runCount);
            }

        }

        private void FillEmptyCellsRandom()
        {
            var emptyCells = GetEmptyCells();

            foreach (var cell in emptyCells)
            {
                SetCharAt(cell, GetRandomCharacter());
            }
        }

        #endregion

        #region character operations

        private char GetRandomCharacter()
        {
            var randomIndex = _rnd.Next(0, _validRandomCharacters.Count());
            return _validRandomCharacters[randomIndex];
        }

        private void DelCharAt(CharCoordinate coordinate)
        {
            _matrix[coordinate.Row, coordinate.Column] = '\0';
        }

        private void SetCharAt(CharCoordinate coordinate, char c)
        {
            _matrix[coordinate.Row, coordinate.Column] = c;
        }

        #endregion

        #region empty sibling search

        private IEnumerable<CharCoordinate> GetEmptySiblings(CharCoordinate coordinate)
        {
            var emptyCells = GetEmptyCells();
            var emptySiblings = emptyCells.Where(x => x.IsSiblingOf(coordinate)).ToList();
            return emptySiblings;
        }

        private CharCoordinate GetRandomEmptySibling(CharCoordinate coordinate, List<CharCoordinate> except = null)
        {

            except = except ?? new List<CharCoordinate>();
            var siblingEmptyCells = GetEmptySiblings(coordinate).Where(x => !except.Contains(x));
            var rnd = new Random();

            var randomIndex = rnd.Next(0, siblingEmptyCells.Count());
            var nextSibling = siblingEmptyCells.ElementAt(randomIndex);

            return nextSibling;
        }

        #endregion

        #region empty cell search

        private List<CharCoordinate> GetEmptyCells()
        {
            var emptyCells = new List<CharCoordinate>();
            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    var current = new CharCoordinate(i, j);

                    if (IsEmpty(current))
                        emptyCells.Add(current);
                }

            }
            return emptyCells;
        }

        private CharCoordinate GetRandomEmptyCell()
        {
            var emptyCells = GetEmptyCells();
            var rnd = new Random();
            var randomIndex = rnd.Next(0, emptyCells.Count);
            return emptyCells.ElementAt(randomIndex);
        }

        #endregion

        #region boolean checkers

        public bool IsTrapped(CharCoordinate coordinate, List<CharCoordinate> previouslyUsedPath = null)
        {
            previouslyUsedPath = previouslyUsedPath ?? new List<CharCoordinate>();
            var empty = GetEmptySiblings(coordinate);
            var trapped = empty.All(x => previouslyUsedPath.Contains(x));

            return trapped;
        }
        public bool HasEmptySibling(CharCoordinate coordinate)
        {
            var emptyCells = GetEmptyCells();
            var siblingEmptyCells = emptyCells.Where(x => x.IsSiblingOf(coordinate));
            return siblingEmptyCells.Any();
        }
        private bool IsEmpty(CharCoordinate coordinate)
        {
            return _matrix[coordinate.Row, coordinate.Column] == '\0';
        }

        #endregion

        #endregion
    }
}
