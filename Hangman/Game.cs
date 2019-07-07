
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
            }
            catch (FileNotFoundException e) {
                Console.WriteLine("Exception : " + e.Message);
                filePath = null;
                FindFilePath();
            } catch (IOException e) {
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

        //Implement alpha parsing
        private void GetUserWord() {

            Console.Write(Messages.GetUserLength);

            numOfChars = 0;
            string length = Console.ReadLine();

            numOfChars = CheckUserLength(length);

            while(numOfChars <= 0) {

                Console.Write(Messages.InvalidEntry);
                length = Console.ReadLine();
                numOfChars = CheckUserLength(length);

            }

        }

        private int CheckUserLength(string length) {

            int toRet = 0;

            try {
                toRet = Int32.Parse(length);
            } catch {
                return -1;
            }

            if (toRet <= 25) {
                return toRet;
            }
            return -1;

        }

    }
}
