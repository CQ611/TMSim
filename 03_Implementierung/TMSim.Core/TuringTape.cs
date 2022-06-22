using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSim.Core
{
    public class TuringTape
    {
        private string content;
        public string Content {
            get { return content; }
            set {
                content = value;
                HeadIndex = 0;
            }
        }
        private char Blank {get; set;}

        public int HeadIndex { get; private set; }

        public TuringTape(string content, char blank)
        {
            this.content = content;
            Blank = blank;
            HeadIndex = 0;
        }


        public char GetCurrentSymbol()
        {
            try
            {
                return content[HeadIndex];
            }
            catch (IndexOutOfRangeException) {
                return Blank;
            }
        }

        public void SetCurrentSymbol(char newSymbol)
        {
            if (content != String.Empty)
            {
                char[] chars = content.ToCharArray();
                chars[HeadIndex] = newSymbol;
                content = new string(chars);
            }
            else
            {
                content = newSymbol.ToString();
            }
        }

        public void MoveLeft()
        {
            if(HeadIndex == 0)
            {
                content = Blank + content;
            }
            else
            {
                HeadIndex--;
            }
        }

        public void MoveRight()
        {
            HeadIndex++;
            if(HeadIndex == content.Length)
            {
                content = content + Blank;
            }
        }
    }
}
