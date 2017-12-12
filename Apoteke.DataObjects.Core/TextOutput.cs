using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apoteke.DataObjects.Core
{
    public class TextOutput
    {
        private int _currentLineNumber = 0;
        private string _currentLine = null;
        private StringBuilder _sb = null;
        private char _splitChar = '-';
        
        public Char SplitChar
        {
            get{return _splitChar;}
            set { _splitChar = value; }
        }
        public int LineLenght
        {
            get;
            set;
        }

        public string CurrentLine
        {
            get { return _currentLine; }
        }

        public int CurrentLineNumber
        {
            get { return _currentLineNumber; }
        }

        public int LineCount
        {
            get
            {
                if (_sb != null)
                    return _sb.Length % 40;
                return 0;
            }
        }
        public TextOutput(int lineLenght)
            : this(lineLenght, null)
        { }
        

        public TextOutput(int lineLenght, string initialString )
        {
            _sb = new StringBuilder(initialString);
            LineLenght = lineLenght;
            NewLine();
           
        }

        public void Add(TextItem item)
        {
            if (item.Position > LineLenght || item.Position < 0)
                throw new ArgumentOutOfRangeException("Position");

            int startPos = 0;
            switch (item.Alignment)
            {
                case TextAlignment.Left:
                    startPos = item.Position;
                    break;
                case TextAlignment.Right:
                    startPos = item.Position - item.Text.Length;
                    break;
            }

            if (startPos < 0 || startPos > LineLenght)
                throw new ApplicationException("Item starting position is out of range.");

            if (startPos + item.Text.Length > LineLenght)
                item.Text = item.Text.Remove(item.Text.Length - (startPos + item.Text.Length - LineLenght));
                
            _currentLine = _currentLine.Remove(startPos, item.Text.Length);
            _currentLine = _currentLine.Insert(startPos, item.Text);
            
        }

        public void NewLine()
        {            
            _currentLineNumber++;
            StringBuilder lineSb = new StringBuilder();
            for (int i = 0; i < LineLenght; i++)
                lineSb.Append(' ');
            _currentLine = lineSb.ToString();
        }

        public void Add( params TextItem[] items)
        {
            foreach (TextItem item in items)
                Add(item);
        }

        public string GetOutput()
        {
            if (_sb != null)
               return _sb.ToString();

            return null;
        }

        public void CommitLine()
        {
            _sb.AppendLine(_currentLine);
        }

        public void AddSplitLine()
        {
            NewLine();
            _currentLine = _currentLine.Replace(' ', _splitChar);
            CommitLine();
        }

        public void AddBlankLine()
        {
            NewLine();
            CommitLine();
        }
    }
}
