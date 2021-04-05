using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SearchFileApplication
{
    /// <summary>
    /// Interaction logic for AddSearchingWordsWindow.xaml
    /// </summary>
    public partial class AddSearchingWordsWindow : Window
    {
        public AddSearchingWordsWindow()
        {
            InitializeComponent();
            SearchingWordsSystem.Instace.Load().ForEach(x=>AddToList(x));
            SearchingWordsSystem.Instace.onAddToListEvent += AddToList;
            SearchingWordsSystem.Instace.onDeleteElementFromListEvent += DeleteFromList;
        }

        ~AddSearchingWordsWindow()
        {
            SearchingWordsSystem.Instace.onAddToListEvent -= AddToList;
            SearchingWordsSystem.Instace.onDeleteElementFromListEvent -= DeleteFromList;
        }
        
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxAddSearchingWords.Text)
                && !listOfSearchingWords.Items.Contains(textBoxAddSearchingWords.Text))
            {
                if (MainWindow.IsNotSpecialChar(textBoxAddSearchingWords.Text))
                {
                    SearchingWordsSystem.Instace.AddToList(textBoxAddSearchingWords.Text);
                }
                else
                {
                    ShowError("Niepoprawny znak!!! Nie używaj znaków specjalnych!\nTylko litery i liczby dozwolone.");
                }
                
            }
            else
                ShowError("Puste pole albo takie słowo jest już na liście");
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (listOfSearchingWords.SelectedIndex >= 0)
            {
                SearchingWordsSystem.Instace.DeleteFromList(listOfSearchingWords.SelectedIndex);
            }
            else
            {
                ShowError("Nie zaznaczony element do usunięcia!!!");
            }
        }

        private void AddToList(string textToAdd)
        {
            listOfSearchingWords.Items.Add(textToAdd);
        }

        private void DeleteFromList(int index)
        {
            listOfSearchingWords.Items.RemoveAt(index);
        }

        private void ShowError(string message)
        {
            System.Windows.MessageBox.Show(message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
