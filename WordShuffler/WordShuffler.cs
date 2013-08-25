using System.Collections.Generic;
using System.Text;
using System.IO;
using WordShuffler.Models;
using Models;

namespace WordShuffler
{
    public class WordShuffler
    {
        private List<string> _words;
        private CharMatrix _matrix;
        private readonly int _size;
        private readonly WordDFA _dfa;

        public WordShuffler(string path, int size)
        {
            _words = LoadWords(path);
            _dfa = new WordDFA(_words);
            _size = size;
        }

        public WordShuffler(Stream wordsStream, int size)
        {
            var sr = new StreamReader(wordsStream);
            _words = LoadWords(sr);
            _dfa = new WordDFA(_words);
            _size = size;
        }

        public ShuffleModel GetNextModel()
        {
            _matrix = new CharMatrix(_size, _words);
            var model = new ShuffleModel(_matrix, _dfa);
            return model;
        }

        #region private methods

        private List<string> LoadWords(string path)
        {
            _words = new List<string>();
            var sr = new StreamReader(path, Encoding.UTF8);
            return LoadWords(sr);
        }

        private List<string> LoadWords(StreamReader sr)
        {
            _words = new List<string>();
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (line != null) _words.Add(line.ToUpper());
            }
            sr.Close();
            sr.Dispose();

            return _words;
        }

        #endregion

    }
}
