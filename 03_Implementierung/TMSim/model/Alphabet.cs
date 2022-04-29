using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSim.model
{
    class Alphabet
    {
        private List<char> Symbols = new List<char>();
        
        public Alphabet(string chars) 
        {
            foreach(char c in chars){
                if(!Symbols.Contains(c)){
                    Symbols.Add(c);
                }
            }
        }

        public bool wordIsContainedIn(string word)
        {
            foreach(char c in word){
                if(!Symbols.Contains(c)) return false;
            }
            return true;
        }
    }
}
