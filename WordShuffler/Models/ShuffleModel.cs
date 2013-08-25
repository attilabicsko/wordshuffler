using System.Collections.Generic;
using System.Linq;
using Models;

namespace WordShuffler.Models
{
    public class ShuffleModel
    {
        public ShuffleModel(CharMatrix matrix, WordDFA dfa)
        {
            _dfa = dfa;
            MatrixWords = new List<MatrixWord>();
            Matrix = matrix;
            FindAllWords();
        }

        private readonly WordDFA _dfa;
        private void FindAllWords()
        {
            var allCoordinates = CharMatrix.GetAllCoordinates(Matrix.Size);

            foreach (var coord in allCoordinates)
            {
                FindWords(coord);
            }
        }

        private void FindWords(CharCoordinate currentCoordinate, TraversalStack stack = null)
        {
            stack = stack ?? new TraversalStack();

            if (stack.Path.Contains(currentCoordinate))
                return;

            if (!_dfa.StateChange(Matrix.GetCharAt(currentCoordinate)))
                return;

            stack.StepTo(currentCoordinate);

            if (_dfa.IsFinal)
            {
                var currentPath = new CharCoordinate[stack.Path.Count];
                stack.Path.CopyTo(currentPath);
                MatrixWords.Add(new MatrixWord(_dfa.State, currentPath ));
            }

            foreach (var coordinate in currentCoordinate.FindSiblings(Matrix.Size).Where(x => !stack.Path.Contains(x)))
            {
                FindWords(coordinate, stack);
            }


            _dfa.Back();
            stack.StepBack();
        }


        public IEnumerable<string> Words { get { return MatrixWords.Select(x => x.Word).Distinct().OrderBy(x => x.Length); } }
        public List<MatrixWord> MatrixWords { get; set; }
        public CharMatrix Matrix { get; set; }
    }
}


