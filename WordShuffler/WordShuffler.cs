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
        private int _size;
        private WordDFA _dfa;


        private WordShuffler(int size)
        {
            setSize(size);
        }

        public WordShuffler(string path, int size) : this(size)
        {
            setWords(LoadWords(path));            
        }

        public WordShuffler(Stream wordsStream, int size): this(size)
        {
            var sr = new StreamReader(wordsStream);
            setWords(LoadWords(sr));
        }

        public WordShuffler(List<string> words, int size) : this(size)
        {
            setWords(words);
        }

        public void setSize(int size)
        {
            _size = size;
        }

        private void setWords(List<string> words)
        {
            _words = words;
            _dfa = new WordDFA(_words);
        }

        public ShuffleModel GetNextModel()
        {
            _matrix = new CharMatrix(_size, _words);
            var model = new ShuffleModel(_matrix, _dfa);
            return model;
        }

        public ShuffleModel GetNextModel(char[,] matrix)
        {
            _matrix = new CharMatrix(_words, matrix);
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
