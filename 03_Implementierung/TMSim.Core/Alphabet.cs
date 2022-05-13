using System.Collections.Generic;

namespace TMSim.Core
{
    public class Alphabet
    {
        private List<char> Symbols = new List<char>();
        
        public Alphabet(string chars) 
        {
            foreach(char c in chars)
            {
                if(!Symbols.Contains(c))
                {
                    Symbols.Add(c);
                }
            }
        }

        public bool WordIsContainedIn(string word)
        {
            foreach(char c in word)
            {
                if(!Symbols.Contains(c)) 
                    return false;
            }
            return true;
        }

        public override string ToString() {
            return new string(Symbols.ToArray());
        }
    }
}
