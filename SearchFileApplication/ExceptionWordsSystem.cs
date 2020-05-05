using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchFileApplication
{
    public delegate void AddToListExceptionHandler(string textToAdd);
    public delegate void DeleteElementFromListExceptionHandler(int index);

    public class ExceptionWordsSystem
    {
        public static ExceptionWordsSystem Instance { get; private set; }
        public event AddToListExceptionHandler onAddToListExceptioEvent;
        public event DeleteElementFromListExceptionHandler onDeleteElementFromListExceptioEvent;

        List<string> exceptionWordsList;

        public ExceptionWordsSystem()
        {
            Instance = this;
            exceptionWordsList = new List<string>();
        }

        public void AddToList(string text)
        {
            exceptionWordsList.Add(text);

            if (onAddToListExceptioEvent != null)
            {
                onAddToListExceptioEvent.Invoke(text);
            }
        }

        public void DeleteFromList(int index)
        {
            exceptionWordsList.RemoveAt(index);

            if (onDeleteElementFromListExceptioEvent != null)
            {
                onDeleteElementFromListExceptioEvent.Invoke(index);
            }
        }
    }
}
