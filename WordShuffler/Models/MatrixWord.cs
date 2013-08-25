namespace WordShuffler.Models
{
    public class MatrixWord
    {

        public MatrixWord(string word, CharCoordinate[] path)
        {
            Word = word;
            Path = path;
        }

        public string Word { get; private set; }
        public CharCoordinate[] Path { get; private set; }
    }
}
