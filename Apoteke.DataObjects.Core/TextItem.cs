using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apoteke.DataObjects.Core
{
    public class TextItem
    {
        #region Properties
        public int MaxLenght
        {
            get;
            set;
        }

        public int Position
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public TextAlignment Alignment
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public TextItem(int position, int maxLength, string text, TextAlignment alignment)
        {
            Position = position;
            if (maxLength > 0)
                Text = text.Substring(0, maxLength);
            else
                Text = text;
            Alignment = alignment;

            if (!String.IsNullOrEmpty(Text))
                Text = TextItem.CharSetCorrection(Text);
        }

        public TextItem(int position, string text, TextAlignment alignment)
            : this(position, 0, text, alignment)
        { }
        #endregion

        #region Methods
        private static string CharSetCorrection(string oldString)
        {
            // Š
            oldString = oldString.Replace((char)352, (char)91);
            // š
            oldString = oldString.Replace((char)353, (char)123);
            // Č
            oldString = oldString.Replace((char)268, (char)94);
            // č
            oldString = oldString.Replace((char)269, (char)126);
            // Ć
            oldString = oldString.Replace((char)262, (char)93);
            // ć
            oldString = oldString.Replace((char)263, (char)125);
            // Đ
            oldString = oldString.Replace((char)272, (char)92);
            // đ
            oldString = oldString.Replace((char)273, (char)124);
            // Ž
            oldString = oldString.Replace((char)381, (char)166);
            // ž
            oldString = oldString.Replace((char)382, (char)167);

            return oldString;
        }
        #endregion

    }
}
