using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchFileApplication
{
    public delegate void AddToListHandler(string textToAdd);
    public delegate void DeleteElementFromListHandler(int index);

    public class SearchingWordsSystem
    {
        public static SearchingWordsSystem Instace { get; private set; }
        public event AddToListHandler onAddToListEvent;              
        public event DeleteElementFromListHandler onDeleteElementFromListEvent;

        List<string> searchingWordsList;

        public SearchingWordsSystem()
        {
            Instace = this;
            searchingWordsList = new List<string>();
        }

        public void AddToList(string text)
        {
            //dodawać do exceptionsList
            searchingWordsList.Add(text);

            if (onAddToListEvent != null)
                onAddToListEvent.Invoke(text);
        }

        public void DeleteFromList(int index)
        {
            searchingWordsList.RemoveAt(index);

            if (onDeleteElementFromListEvent != null)
                onDeleteElementFromListEvent.Invoke(index);
        }

        public List<string> Load()
        {
            return searchingWordsList;
        }
    }
}
