using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model
{
    public class SearchWordUtil
    {
        public static string[] ParseSearchWords(string search)
        {
            char[] charSeparators = new char[] {' ', '　'};
            return search.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool IsDisplayable(CharacterModel chara, string[] filterWords)
        {
            string chainedAll = chara.ToSearchString();
            for (int i = 0; i < filterWords.Count(); i++)
            {
                if (!chainedAll.Contains(filterWords[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsDisplayable(object obj, string[] filterWords)
        {
            var searchable = obj as ISearchable;

            if (searchable == null)
            {
                return true;
            }

            string chainedAll = searchable.ToSearchString();
            for (int i = 0; i < filterWords.Count(); i++)
            {
                if (!chainedAll.Contains(filterWords[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static void DoFilterMark(string search, ObservableCollection<IMarkable> filterItems)
        {
            if(search == null || search.Count() == 0)
            {
                return;
            }

            var words = ParseSearchWords(search);

            if(words.Count() == 0)
            {
                return ;
            }

            var removeList = new List<IMarkable>();

            foreach (var item in filterItems)
            {
                if(!IsDisplayable(item, words))
                {
                    removeList.Add(item);
                }
            }

            foreach (var removeitem in removeList)
            {
                filterItems.Remove(removeitem);
            }
        }
    }

}
