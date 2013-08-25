using System.Collections.Generic;
using System.Linq;

namespace WordShuffler.Models
{
    public class TraversalStack
    {
        public TraversalStack()
        {
            Path = new List<CharCoordinate>();
        }
        
        public void StepTo(CharCoordinate nextCoordinate)
        {
            Path.Add(nextCoordinate);
        }
        public void StepBack()
        {
            Path.Remove(Path.Last());
        }

        public List<CharCoordinate> Path { get; private set; }

    }
}
