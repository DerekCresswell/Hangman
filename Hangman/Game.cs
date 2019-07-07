
using System;
using System.IO;
using System.Collections.Generic;

namespace Hangman
{
    class Game {

        private string filePath;

        private List<string> words;

        private int numOfChars;
        private List<char> guessedChars;
        private char[] correctChars;
        private int guesses = 0;

        //Const
        private int MAXLENGTH = 25;

        public Game() {

            Console.WriteLine(Messages.Welcome);

            FindFilePath();

            words = new List<string>();
            guessedChars = new List<char>();

            SetWordsArray();

            Console.WriteLine(Messages.Rules);

            GetUserWord();

        }

        private void WriteMessage(string messageName) {
            Console.WriteLine(Messages.ResourceManager.GetString(messageName));
        }

        private void SetWordsArray() {
            
            try {

                UseStreamReader();

            } catch (ArgumentNullException e) {
                Console.WriteLine("Exception : " + e.Message);
                filePath = null;
                FindFilePath();
            } catch (FileNotFoundException e) {
                Console.WriteLine("Exception : " + e.Message);
                filePath = null;
                FindFilePath();
            } catch (IOException) {
            } catch (Exception e) {
                Console.WriteLine("Exception : " + e.Message);
            }

        }

        private void UseStreamReader() {

            using (StreamReader sr = new StreamReader(filePath)) {

                string curLine = sr.ReadLine();

                while (curLine != null) {
                    words.Add(curLine);
                    curLine = sr.ReadLine();
                }

            }

        }

        private void FindFilePath()
        {

            //Implement directory usage

            if (filePath == null) {
                Console.WriteLine(Messages.InvalidFilePath);
                Console.Write(Messages.EnterFilePath);
                filePath = Console.ReadLine();
            }

            SetWordsArray();

        }

        private void GetUserWord() {

            //Length
            Console.Write(Messages.GetUserLength);

            //Alpha parsing still required, enums?
            ParseForLength();
            
        }

        private void ParseForLength() {

            if(!Int32.TryParse(Console.ReadLine(), out numOfChars)) {
                Console.Write(Messages.InvalidEntry);
                ParseForLength();
            } if(numOfChars <= 0 || numOfChars > 25) {
                Console.Write(Messages.InvalidEntry);
                ParseForLength();
            }
        }

    }
}
