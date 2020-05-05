using Microsoft.Win32;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;       //observableCollection
using System.Diagnostics;                   //Process.Start
using System.Text.RegularExpressions;       //Regex

namespace SearchFileApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<FileHelper> listFH = new ObservableCollection<FileHelper>();
        SearchingWordsSystem searchingWordsSystem = new SearchingWordsSystem();
        ExceptionWordsSystem exceptionWordsSystem = new ExceptionWordsSystem();
        RegexCreatorPattern rCP = new RegexCreatorPattern();
        Regex regex;

        public MainWindow()
        {
            // https://flask.io/x1DrqfU3E9yA
            InitializeComponent();
            searchingWordsSystem.onAddToListEvent += SearchingWordsSystem_onAddToComboBox;
            searchingWordsSystem.onDeleteElementFromListEvent += SearchingWordsSystem_onDeleteElementFromComboBox;
            exceptionWordsSystem.onAddToListExceptioEvent += AddToListExceptionWords;
            exceptionWordsSystem.onDeleteElementFromListExceptioEvent += DeleteElementFromListException;
            boxOfSearchingWords.Items.Add("[pusto]");
        }

        ~MainWindow()
        {
            searchingWordsSystem.onAddToListEvent -= SearchingWordsSystem_onAddToComboBox;
            searchingWordsSystem.onDeleteElementFromListEvent -= SearchingWordsSystem_onDeleteElementFromComboBox;
            exceptionWordsSystem.onAddToListExceptioEvent -= AddToListExceptionWords;
            exceptionWordsSystem.onDeleteElementFromListExceptioEvent -= DeleteElementFromListException;
        }

        public static bool IsNotSpecialChar(string text)
        {
            Regex regex = new Regex("^([a-zA-Z0-9])*$");
            if (regex.IsMatch(text))
                return true;

            return false;
        }

        //przeszukiwanie pliku
        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            //czy podano ścieżkę -> spawdzanie czy jest tekst w stringu metodą IsNullOrEmpty
            //czy ścieżka istnieje -> Directory.Exists
            if (!string.IsNullOrEmpty(filePath.Text) && Directory.Exists(filePath.Text))
            {
                //czy wpisano rozszerzenie - IsNullOrEmpty
                //czy rozszerzenie jest poprawne - spawdzenie czy ma kropkę z przodu
                if (!string.IsNullOrEmpty(fileNameExtension.Text) && isExtensionFileNameCorrect(fileNameExtension.Text))
                {
                    DirectoryInfo di = new DirectoryInfo(filePath.Text);
                    string extensionFile = string.Format("*{0}", fileNameExtension.Text);

                    //metoda GetFiles przyjmująca parametr rozszerzenie pliku w postaci *.turozszerzenie, sparwdza wszystkie ścieżki
                    FileInfo [] fileInfoArray = di.GetFiles(extensionFile, SearchOption.AllDirectories);
                    
                    ClearListFileHelper();

                    regex = new Regex(rCP.GetUnsearchingPattern());

                    foreach (var fileInfo in fileInfoArray)
                    {
                        if (regex.Match(fileInfo.Name).Success)
                        {
                            listFH.Add(FileHelper.BuilderFileHelper(fileInfo));
                            await Task.Delay(1);
                        }
                    }

                    if (listFH.Count > 0)
                    {
                        listOfPaths.ItemsSource = listFH;
                    }
                    else
                        ShowMessege("Brak wyników");
                    
                }
                else
                {
                    ShowMessegeWarning("Nie podano prawidłowego rozszerzenia pliku!!!");
                }
            }
            else
            {
                ShowMessegeWarning("Nie podano prawidłowej ścieżki!!!");
            }
        }

        private bool isExtensionFileNameCorrect(string fileNameExtension)
        {
            if (fileNameExtension.StartsWith("."))
            {
                return true;
            }
            return false;
        }

        //wybór ścieżki do katalogu
        private void filePath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Tworzymy obiekt okna dialogowego przeglądarki folderów
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            //resultat otwarcia okna dialogowego umieszczmy  w zmiennej i porownojemy czy wcisnięto klawisz ok
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                filePath.Text = folderBrowserDialog.SelectedPath; // przypisanie wybranej ścieżki do pola TextBox
            }
        }

        //usunięcie ścieżki wyszukacych pliku z listy
        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (listOfPaths.SelectedItems.Count > 0)
            {
                while (listOfPaths.SelectedItems.Count > 0)
                {
                    listFH.Remove(listOfPaths.SelectedItem as FileHelper);
                    await Task.Delay(1);
                }
            }
            else
                ShowMessegeError("Nie wybrano wyników do usunięcia");
        }

        //otwieranie pliku
        private void listOfPaths_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listOfPaths.SelectedIndex >= 0)
            {
                string path = listFH[listOfPaths.SelectedIndex].FilePath;
                try
                {
                    Process.Start(path);

                }
                catch (System.ComponentModel.Win32Exception)
                {
                    ShowMessegeError("Nie można uruchomić pliku!!!");
                }

            }
        }

        private void deleteAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (listFH.Count > 0)
            {
                ClearListFileHelper();
            }
            else
            {
                System.Windows.MessageBox.Show("Pole wyników juz jest puste!!!", "UWAGA", 
                    System.Windows.MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        
        private void ClearListFileHelper()
        {
            listFH.Clear();
            FileHelper.ResetIdCount();

        }

        private void addSearchingWordsButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = new AddSearchingWordsWindow();
            window.ShowDialog();
        }
        
        //dodawanie do listy nie wyszukiwanych nazw plików
        private void AddExceptionWordButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textExceptionBox.Text) 
                && !exceptionWordsList.Items.Contains(textExceptionBox.Text))
            {
                if (IsNotSpecialChar(textExceptionBox.Text))
                {
                    ExceptionWordsSystem.Instance.AddToList(textExceptionBox.Text);
                }
                else
                    ShowMessegeError("Niepoprawny znak!!! Nie używaj znaków specjalnych!\nTylko litery i liczby dozwolone.");
            }
            else
            {
                ShowMessegeError("Puste pole albo takie słowo jest już na liście");
            }
        }

        private void AddToListExceptionWords(string text)
        {
            exceptionWordsList.Items.Add(text);
        }

        //usuwanie z listy nie wyszukiwanych nazw plików
        private void DeleteExceptionWordButton_Click(object sender, RoutedEventArgs e)
        {
            if (exceptionWordsList.SelectedIndex >= 0)
                ExceptionWordsSystem.Instance.DeleteFromList(exceptionWordsList.SelectedIndex);
            else
                ShowMessegeWarning("Nie wskazano elementu do usunięcia");
        }

        private void DeleteElementFromListException(int index)
        {
            exceptionWordsList.Items.RemoveAt(index);
        }

        //dodanie szukanych wyrazów do comboBox
        private void SearchingWordsSystem_onAddToComboBox(string textToAdd)
        {
            boxOfSearchingWords.Items.Add(textToAdd);
        }

        //usuwanie z combobox szuanych wyrazów + 1 bo pierwszy element jest niezmienny
        private void SearchingWordsSystem_onDeleteElementFromComboBox(int index)
        {
            boxOfSearchingWords.Items.RemoveAt(index + 1); 
        }

        //wyswietlanie wybranych z combobox nazw z pominięciem reszty
        private void boxOfSearchingWords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (boxOfSearchingWords.SelectedIndex >= 0)
            {
                regex = new Regex(rCP.GetSearchingPattern((string)boxOfSearchingWords.SelectedValue));

                foreach (var item in listFH)
                {
                    if (regex.Match(item.FileName).Success)
                    {
                        item.IsEnabled = true;
                    }
                    else
                    {
                        item.IsEnabled = false;
                    }
                }
            }
        }

        private void ShowMessege(string message)
        {
            System.Windows.MessageBox.Show(message, "UWAGA!", System.Windows.MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Wyswietla ostrzeżenie w MessageBox
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessegeWarning(string message)
        {
            System.Windows.MessageBox.Show(message, "UWAGA!", System.Windows.MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Wyświetla błąd w MessageBox
        /// </summary>
        /// <param name="message">błąd</param>
        private void ShowMessegeError(string message)
        {
            System.Windows.MessageBox.Show(message, "Błąd!", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
