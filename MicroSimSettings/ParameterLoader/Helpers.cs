using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MicroSimSettings
{
    public static class Helpers
    {
        static Regex r_betu = new Regex("[a-zA-ZÁÍŰŐÜÖÚÓÉáéöőüűúóí]");
        static Regex r_egyeb = new Regex("[0-9_=]");
        public static string StringHelper(string s)
        {
            string help = "";
            bool nagybetu = true;
            for (int i = 0; i < s.Length; i++)
            {
                if (r_betu.IsMatch(s[i].ToString()))
                {
                    if (nagybetu)
                    {
                        help += s[i].ToString().ToUpper();
                        nagybetu = false;
                    }
                    else
                    {
                        help += s[i].ToString();
                    }
                }
                else if (r_egyeb.IsMatch(s[i].ToString()))
                {
                    help += s[i].ToString();
                    nagybetu = true;
                }
                else
                {
                    nagybetu = true;
                }
            }
            return help;
        }

        public static string FirstCharToLower(string str)
        {
            string result;
            string s = str[0].ToString().ToLower();
            result = s + str.Substring(1, str.Length - 1);
            return result;
        }


        public static string ListToComaSeparatedString(List<string> list)
        {
            string result = "";
            for (int i = 0; i < list.Count-1; i++)
			{
                result += list[i] + ",";
			}
            result += list[list.Count - 1];
            return result;
        }


        public static List<string> GenerateColumnList() //A-ZZ oszlopok stringje előállítása sorban -> lst_columnlist
        {
            List<string> lst_columnlist = new List<string>();
            char x = 'A';
            do
            {
                lst_columnlist.Add(x.ToString());
                x++;
            } while (x != 'Z' + 1);

            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    lst_columnlist.Add(lst_columnlist[i] + lst_columnlist[j]);
                }
            }
            return lst_columnlist;
        }

        public static int DimensionChange(List<int> lst_1, List<int> lst_2)
        {
            if (lst_1.Count != lst_2.Count) return -2; //ezt bizony még jobban ki kell dolgozni
            for (int i = 0; i < lst_1.Count; i++)
            {
                if (lst_1[i] == lst_2[i]) continue;
                else return lst_1.Count-i-1;
            }
            return -1;
        }
    }
}
