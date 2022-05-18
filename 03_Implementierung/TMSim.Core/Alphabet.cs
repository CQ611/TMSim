using System;
using System.Collections.Generic;
using System.Linq;

namespace TMSim.Core
{
    public class Alphabet
    {
        public List<char> Symbols = new List<char>();

        public Alphabet(string chars) 
        {
            chars.ToList().ForEach(c => { if (!Symbols.Contains(c)) Symbols.Add(c); });

        }
        public bool WordIsContainedIn(string word)
        {
            return !word.ToList().Where(c => !Symbols.Contains(c)).Any();
        }

        public override string ToString() {
            return new string(Symbols.ToArray());
        }
    }
}
