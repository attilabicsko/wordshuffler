using System.Collections.Generic;

namespace WordShuffler.Models
{
    public class WordDFA
    {
        public WordDFA(IEnumerable<string> words)
        {
            State = "";
            States = new Dictionary<string, WordDFAState>();
            Build(words);
        }

        public void Reset()
        {
            State = "";
        }

        public string State { get; private set; }

        public bool IsFinal { get; set; }

        public bool StateChange(char newState)
        {
            if (!States.ContainsKey(State)) return false;

            var state = States[State];
            if (state.Transitions.ContainsKey(newState))
            {
                State = State + newState;
                IsFinal = state.Transitions[newState];
                return true;
            }
            return false;
        }

        public void Back()
        {
            State = State.Length > 1 ? State.Substring(0, State.Length - 1) : "";
        }

        private Dictionary<string, WordDFAState> States { get; set; }

        private void Build(IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                var i = 0;
                while (i < word.Length)
                {
                    var currentWord = word.Substring(0, i);
                    var final = i == word.Length - 1;

                    var newChar = word[i];
                    var newState = new WordDFAState(currentWord);
                    newState.AddTransition(newChar, final);

                    if (States.ContainsKey(currentWord))
                    {

                        States[currentWord].AddTransition(newChar, final);
                    }
                    else
                    {
                        States.Add(currentWord, newState);
                    }


                    i++;
                }
            }
        }
    }


    public class WordDFAState
    {

        public WordDFAState(string data)
        {
            Data = data;
            Transitions = new Dictionary<char, bool>();
        }

        public void AddTransition(char value, bool isFinal)
        {
            if (Transitions.ContainsKey(value))
            {
                if (!Transitions[value])
                    Transitions[value] = isFinal;
            }
            else
            {
                Transitions.Add(value, isFinal);
            }
        }

        public string Data { get; set; }
        public Dictionary<char, bool> Transitions { get; set; }
    }






}
