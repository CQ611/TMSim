using System.Collections.Generic;
using System.Linq;

namespace TMSim.Core
{
    public class TuringAlphabet
    {
        public List<char> Symbols = new List<char>();

        public TuringAlphabet(string chars) 
        {
            chars.ToList().ForEach(c => { if (!Symbols.Contains(c)) Symbols.Add(c); });

        }
        public bool WordIsContainedIn(string word)
        {
            return !word.ToList().Where(c => !Symbols.Contains(c)).Any();
        }
    }
}
