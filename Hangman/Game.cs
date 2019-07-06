
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

            //All messages are temporary, store in data bank later, search Message
            Console.WriteLine("Welcome"); //Message

            FindFilePath();

            words = new List<String>();
            SetWordsArray();

            

        }

        private void SetWordsArray() {
            
            try
            {

                UseStreamReader();

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("Exception : " + e.Message);
                filePath = null;
                FindFilePath();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Exception : " + e.Message);
                filePath = null;
                FindFilePath();
            } catch (IOException e) {
            } catch (Exception e)
            {
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
                Console.WriteLine("Invalid filepath"); //Message
                GetUserFilePath();
            }

            SetWordsArray();

        }

        private void GetUserFilePath()
        {
            Console.Write("\nPlease Enter the filepath to your word bank\nPath : "); //Message
            filePath = Console.ReadLine();
        }

    }
}
