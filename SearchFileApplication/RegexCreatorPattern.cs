using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchFileApplication
{
    public class RegexCreatorPattern
    {
        enum KIND { Unserching, Searching };

        string pattern;
        List<string> exceptionWordsPatternList;

        public RegexCreatorPattern()
        {
            exceptionWordsPatternList = new List<string>();
            pattern = "";

            ExceptionWordsSystem.Instance.onAddToListExceptioEvent += AddToPatternException_onAddToListExceptioEvent;
            ExceptionWordsSystem.Instance.onDeleteElementFromListExceptioEvent += DeleteFromPatternException_onDeleteElementFromListExceptioEvent;
        }

        ~RegexCreatorPattern()
        {
            ExceptionWordsSystem.Instance.onAddToListExceptioEvent -= AddToPatternException_onAddToListExceptioEvent;
            ExceptionWordsSystem.Instance.onDeleteElementFromListExceptioEvent -= DeleteFromPatternException_onDeleteElementFromListExceptioEvent;
        }

        public string GetUnsearchingPattern()
        {
            pattern = GetPattern(exceptionWordsPatternList, KIND.Unserching);
            return pattern;
        }

        public string GetSearchingPattern(string word)
        {
            List<string> words = new List<string>();
            words.Add(word);
            pattern = GetPattern(words, KIND.Searching);
            return pattern;
        }

        private string GetPattern(List<string> list, KIND kIND)
        {
            string text;
            pattern = ClearPattern();

            if (list.Count > 0)
            {
                text = string.Join("|", list);

                if (kIND.Equals(KIND.Unserching))
                {
                    pattern = string.Format("^(?!.*({0})).*$", text);
                }
                else if (kIND.Equals(KIND.Searching))
                {
                    pattern = string.Format("^(?=.*({0})).*$", text);
                }
                
            }

            return pattern;
        }

        private string ClearPattern()
        {
            return "";
        }

        private void AddToPatternException_onAddToListExceptioEvent(string textToAdd)
        {
            exceptionWordsPatternList.Add(textToAdd);
        }

        private void DeleteFromPatternException_onDeleteElementFromListExceptioEvent(int index)
        {
            exceptionWordsPatternList.RemoveAt(index);
        }

    }
}
