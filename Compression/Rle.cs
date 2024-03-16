using System;


namespace Compression
{
    public class Rle
    {
        private string _text;
        private string _otext;

        public Rle()
        { }

        public Rle(string text) 
        {
            _text = text;
            _otext = text;
        }

        public void Decompress()
        {
            try
            {
                string str1 = _text, str = "", ch = "", s = "", symb = "";
                int i,  // текущий символ 
                k = 0,  // колличество
                j;      // ыыыыыыыы

                for (i = 0; i < str1.Length;)
                {

                    ch = str1.Substring(i , 1);
                    k = 0;
                    s = "";
                    if (("0123456789").Contains(ch))
                    {
                        for (j = i; j < str1.Length; j++)
                        {
                            if ("0123456789".Contains(str1.Substring(j , 1)))
                            {
                                // если текущий символ j является цифрой
                                s += str1.Substring(j , 1);
                            }
                            else
                                break;
                        }
                        // буквa
                        symb = str1.Substring(j , 1);
                        i = j + 1;
                    }
                    else
                        i++;

                    if (s.Length != 0)
                    {
                        // декодирование буквы
                        for (j = 0; j < Convert.ToInt32(s); j++)
                            str += symb;
                    }
                    else
                        str += ch;
                }

                _text = str;
            }
            catch
            {
                _text = "Нет данных";
            }

        }

        public void Compress()
        {
            string str1 = _text, str = "", ch = "";
            int i, // номер символа
                k, // счетчик количества повторяющихся символов
                j; // ыыы)

            for (i = 0; i < str1.Length;)
            {
                // от 0 до длины строки
                // получить текущий символ из строки [str1]
                ch = str1.Substring(i , 1);
                k = 0;

                // если последний символ
                if (i == str1.Length - 1)
                {
                    str += ch;
                    break;
                }

                if (str1.Substring(i + 1 , 1) == ch)
                {
                    for (j = i; j < str1.Length; j++)
                    {
                        //если текущий символ равен символу из строки ch
                        if (str1.Substring(j , 1) == ch)
                            k++;
                        else
                            break;
                    }
                    i = j;
                }
                else
                    i++;

                if (k != 0)
                    str += k.ToString() + ch;
                else
                    str += ch;

            }

            _text = str;

        }


        public string Result => _text;
        public string Text 
        {
            get => _otext; 
            set => _text = value;
        }
    }
}
