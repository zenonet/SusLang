using System.Collections;
using System.Collections.Generic;

namespace SusLang.Tokens
{
    public class TokenList : IEnumerable<Token>
    {
        private readonly List<Token> _list = new();
        public int index;

        #region Conversions and syntactic sugar

        public Token this[int i]
        {
            get => _list[i];
            set => _list[i] = value;
        }

        public IEnumerator<Token> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        public TokenList()
        {
        }

        private TokenList(List<Token> list) => _list = list;

        public static implicit operator List<Token>(TokenList list) => list._list;
        public static implicit operator TokenList(List<Token> list) => new (list);

        #endregion

        public Token current => _list[index];

        public void Skip(int amount = 1) => index += amount;

        private bool _everUsedNext;
        public Token Next()
        {
            //I did this so that you can use Next() as the first thing you do and
            //it will still output the first element of the list
            //
            //Now that I am thinking about this: this will probably get very confusing at some point...
            if(_everUsedNext || index > 0)
                Skip();
            
            _everUsedNext = true;
            return current;
        }
    }
}