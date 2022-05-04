using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSim.model
{
    class TuringBand
    {
        public string Content { get; set; }
        private char Blank {get; set;}

        public int HeadIndex { get; private set; }

        public TuringBand(string content, char blank){
            this.Content = content;
            this.Blank = blank;
        }


        public char GetCurrentSymbol()
        {
            return Content[HeadIndex];
        }

        public void SetCurrentSymbol(char newSymbol){//need to be added in "Schnittstellenuebersicht"
            char[] chars = Content.ToCharArray();
            chars[HeadIndex] = newSymbol;
            Content = new string(chars);
        }

        public void MoveLeft()
        {
            if(HeadIndex == 0){
                Content = Blank + Content;
            }else{
                HeadIndex--;
            }
        }

        public void MoveRight()
        {
            HeadIndex++;
            if(HeadIndex == Content.Length){
                Content = Content + Blank;
            }
        }
    }
}
