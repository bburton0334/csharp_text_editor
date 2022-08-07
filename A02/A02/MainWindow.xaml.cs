/*
* FILE              : MainWindow.xaml.cs
* PROJECT           : PROG2121 - A02
* PROGRAMMER        : Briana Burton
* FIRST VERSION     : 2021-09-25
* DESCRIPTION       :
*   The functions in this file are used to allow 
*   the user to utilize the abilities of the 
*   MainWindow. The user can type into the textbox
*   and can save their changes, open new, close, 
*   and open and about box. The MainWinow also
*   Includes a character counter which counts the
*   amount of characters currently present in the
*   textbox.
*/

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Win32;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;

namespace A02
{
    /*
    * NAME      : MainWindow
    * PURPOSE   : The MainWindow class holds the methods which allow
    *       the user to properly interact with the notepad MainWindow.
    *       It holds two properties: textChangeStatus which holds the 
    *       status true or false depending on if the text has been 
    *       altered in the textbox. filePath contains the location of
    *       the file.
    */
    public partial class MainWindow : Window
    {
        private bool textChangeStatus = false;   // holds the status of whether or not text has been modified
        private string textFileLocation = null;          // holds the file location

        public MainWindow()
        {
            InitializeComponent();
        }

        //
        // FUNCTION         : FileNew
        // DESCRIPTION      :
        //      This method checks if the text has been altered. If
        //      it has been, it will call the method to prompt the
        //      user to save before calling the WipeTextBox method
        //      to clear the textbox.
        // PARAMETERS       :
        //      object sender
        //      RoutedEventargs
        // RETURNS          : none
        //
        private void FileNew(object sender, RoutedEventArgs e)
        {
            /* checking if text has been changed. If so, user will be
             * prompted to save the file, before starting a new file. */
            if (textChangeStatus == true)
            {
                string result = SaveFilePrompt();
                if (result != "Cancel")
                {
                    WipeTextBox();
                }
            }
            else
            {
                WipeTextBox();
            }
        }

        //
        // FUNCTION         : FileOpen
        // DESCRIPTION      :
        //      This method checks if the text has been altered. If
        //      it has been, it will call the method to prompt the
        //      user to save before calling the DisplayOpenFileDialog
        //      method to open the file.
        // PARAMETERS       :
        //      object sender
        //      RoutedEventargs
        // RETURNS          : none
        //
        private void FileOpen(object sender, RoutedEventArgs e)
        {
            /* checking if text has been changed. If so, user will be
             * prompted to save the file, before opening new file. */
            if (textChangeStatus == true)
            {
                string result = SaveFilePrompt();
                if (result != "Cancel")
                {
                    DisplayOpenFileDialog();
                } 
            }
            else
            {
                DisplayOpenFileDialog();
            }
        }

        //
        // FUNCTION         : FileSaveAs
        // DESCRIPTION      :
        //      This method calls function to save the current file
        // PARAMETERS       :
        //      object sender
        //      RoutedEventargs
        // RETURNS          : none
        //
        private void FileSaveAs(object sender, RoutedEventArgs e)
        {
            DisplaySaveFileDialog();
        }

        //
        // FUNCTION         : DisplayCharCount
        // DESCRIPTION      :
        //      This method calls function to update status bar
        //      indicating the amount of characters present
        // PARAMETERS       :
        //      object sender
        //      KeyEventArgs
        // RETURNS          : none
        //
        private void DisplayCharCount(object sender, KeyEventArgs e)
        {
            CountCharacters();
        }

        //
        // FUNCTION         : CountCharacters
        // DESCRIPTION      :
        //      This method collects the length of content inside
        //      the textbox and displays the total number of character
        //      at the bottom of the window.
        // PARAMETERS       : none
        // RETURNS          : none
        //
        private void CountCharacters()
        {
            int count = 0;  // holds amount of characters

            // converting amount of characters to string
            count = txtBoxContent.Text.Length;
            string outputCounter = count.ToString() + " Characters Present.";

            // displaying updating characters in window
            charCounter.Text = outputCounter;
        }

        //
        // FUNCTION         : FileClose
        // DESCRIPTION      :
        //      This method checks if the text has been altered. If
        //      it has been, it will call the method to prompt the
        //      user to save before closing the window.
        // PARAMETERS       :
        //      object sender
        //      RoutedEventargs
        // RETURNS          : none
        //
        private void FileClose(object sender, RoutedEventArgs e)
        {
            // prompting user to save if text has been altered
            if (textChangeStatus == true)
            {
                string result = SaveFilePrompt();
                if (result != "Cancel")
                {
                    this.Close();   // closing window
                }
            }
            else
            {
                this.Close();   // closing window
            }
        }

        //
        // FUNCTION         : DisplayOpenFileDialog
        // DESCRIPTION      :
        //      This method opens a dialog to allow the user
        //      to open a text file to be loaded into the textbox
        // PARAMETERS       : none
        // RETURNS          : none
        //
        private void DisplayOpenFileDialog()
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            // filters for file path
            openDialog.Filter = "Text Files|*.txt|All|*.*";

            // processing the file if user chose a text file
            if (openDialog.ShowDialog() == true)
            {
                textFileLocation = openDialog.FileName;
                ReadFilePathAndDisplay(textFileLocation);
                textChangeStatus = false;
            }
        }

        //
        // FUNCTION         : ReadFilePathAndDisplay
        // DESCRIPTION      :
        //      This method loads the content of the file 
        //      specified within location and displays the
        //      content with the textbox of MainWindow
        // PARAMETERS       :
        //      string location : holds path of file
        // RETURNS          : none
        //
        private void ReadFilePathAndDisplay(string location)
        {
            StreamReader readFileLocation = new StreamReader(location);
            StringBuilder displayFile = new StringBuilder();

            // reading contents of file as long as content exists
            string fileContent = readFileLocation.ReadLine();
            while (fileContent != null)
            {
                displayFile.Append(fileContent);
                displayFile.Append(Environment.NewLine);
                fileContent = readFileLocation.ReadLine();
            }
            readFileLocation.Close();   // closing window

            // loading content of file into the textbox
            txtBoxContent.Text = displayFile.ToString();

            // calling method to update character count
            CountCharacters();
        }

        //
        // FUNCTION         : SaveFile
        // DESCRIPTION      :
        //      This method collects the data within the textbox
        //      to be allowed to be saved as a text file.
        // PARAMETERS       :
        //      string location : path of file
        // RETURNS          : none
        //
        private void SaveFile(string location)
        {
            StreamWriter content = new StreamWriter(location);
            content.Write(txtBoxContent.Text);
            content.Close();
            textChangeStatus = false;   // file has been saved. Updating status
        }

        //
        // FUNCTION         : DisplaySaveFileDialog
        // DESCRIPTION      :
        //      This method is called to save the contents of the
        //      textbox. The method checks if selected file is 
        //      valid, it will call SaveFile method to update the
        //      textChangeStatus and save file.
        // PARAMETERS       : none
        // RETURNS          : none
        //
        private void DisplaySaveFileDialog()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();

            // filter for file path
            saveDialog.Filter = "Text Document (.txt)|*.txt";

            // saving if path is valid
            bool? saveFileResult = saveDialog.ShowDialog();
            if (saveFileResult == true)
            {
                string tmpFileName = saveDialog.FileName;
                textFileLocation = tmpFileName;
                SaveFile(tmpFileName);
            }
        }

        //
        // FUNCTION         : SaveFilePrompt
        // DESCRIPTION      :
        //      This method contains a window prompt which asks the user
        //      if they would like to save their content before changing
        //      it or closing the program. Returns user choice.
        // PARAMETERS       : none
        // RETURNS          :
        //      string   :  contains answer to window prompt
        //
        private string SaveFilePrompt()
        {
            // prompting user with message and choices 
            MessageBoxResult choice = MessageBox.Show("Would you like to save your changes first?", "Save Changes?", MessageBoxButton.YesNoCancel);
            if (choice == MessageBoxResult.Yes)
            {
                DisplaySaveFileDialog();    // calling method to save file
            }
            else if (choice == MessageBoxResult.Cancel)
            {
                return "Cancel";    // returning cancel answer
            }
            return "None";
        }

        //
        // FUNCTION         : WipeTextBox
        // DESCRIPTION      :
        //      This method clears the current content within the texbox
        //      and updates sets the class properties back to their default
        // PARAMETERS       :
        //      object sender
        //      RoutedEventargs
        // RETURNS          : none
        //
        private void WipeTextBox()
        {
            // setting properties back to their default
            textFileLocation = null;
            textChangeStatus = false;

            // clearing textbox contents
            txtBoxContent.Text = "";

            // calling method to update character count
            CountCharacters();
        }

        //
        // FUNCTION         : DisplayAboutWindow
        // DESCRIPTION      :
        //      This method opens the AboutWindow after the user
        //      selects the "about" option under the help section
        //      within the MainWindow menu. AboutWindow contains
        //      info about the program.
        // PARAMETERS       :
        //      object sender
        //      RoutedEventargs
        // RETURNS          : none
        //
        private void DisplayAboutWindow(object sender, RoutedEventArgs e)
        {
            // opens AboutWindow
            AboutWindow abtWin = new AboutWindow();
            abtWin.WindowStartupLocation = WindowStartupLocation.Manual;
            abtWin.ShowDialog();
        }

        //
        // FUNCTION         : CloseRequest
        // DESCRIPTION      :
        //      This method when the user wishes to exit the program using
        //      the exit in the top window. It will prompt user to save if
        //      text has been altered, before closing the program.
        // PARAMETERS       :
        //      object sender
        //      CancelEventargs
        // RETURNS          : none
        //
        private void CloseRequest(object sender, CancelEventArgs e)
        {
            if (textChangeStatus == true)
            {
                string result = SaveFilePrompt();
                if (result == "Cancel") 
                {
                    e.Cancel = true;
                }
            }
        }

        //
        // FUNCTION         : ContentAltered
        // DESCRIPTION      :
        //      This method changes textChangeStatus if keys have been pressed
        // PARAMETERS       :
        //      object sender
        //      RoutedEventargs
        // RETURNS          : none
        //
        private void ContentAltered(object sender, KeyEventArgs e)
        {
            textChangeStatus = true;
        }

        //
        // FUNCTION         : FocusAltered
        // DESCRIPTION      :
        //      This method changes textChangeStatus if textbox has been interacted with
        // PARAMETERS       :
        //      object sender
        //      RoutedEventargs
        // RETURNS          : none
        //
        private void FocusAltered(object sender, RoutedEventArgs e)
        {
            textChangeStatus = true;
        }
    }
}
