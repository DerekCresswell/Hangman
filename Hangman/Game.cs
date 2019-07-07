
using System;
using System.IO;
using System.Collections.Generic;

namespace Hangman
{
    class Game {

        private string filePath;

        private List<String> words;

        private int numOfChars;
        private char[] guessedChars = new char[26];
        private char[] correctChars;
        private int guesses = 0;

        public Game() {

            Console.WriteLine(Messages.Welcome);

            FindFilePath();

            words = new List<String>();
            SetWordsArray();

            Console.WriteLine(Messages.Rules);

            GetUserWord();

        }

        private void WriteMessage(String messageName) {
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

                String curLine = sr.ReadLine();

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
                GetUserFilePath();
            }

            SetWordsArray();

        }

        private void GetUserFilePath() {
            Console.Write(Messages.EnterFilePath);
            filePath = Console.ReadLine();
        }

        private void GetUserWord() {

            Console.WriteLine(Messages.GetUserLength);

        }

    }
}
