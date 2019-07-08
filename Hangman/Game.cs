
using System;
using System.IO;
using System.Collections.Generic;

namespace Hangman
{
    class Game {

        private string filePath;

        private int numOfChars;
        private List<char> guessedChars = new List<char>();
        private char[] correctChars;
        private int guesses = 0;
        private int incorrectGuesses = 0;

        private List<string> words = new List<string>();

        //Const
        private int MAXLENGTH = 25;
        private char[] VOWELS = "aeiou".ToCharArray();
        private char[] CONSONANTS = "bcfghjklmnpqrstvwxyz".ToCharArray();

        public Game() {

            Console.WriteLine(Messages.Welcome);

            FindFilePath();     

            SetWordsArray();

            Console.WriteLine(Messages.Rules);

            GetUserWord();

            Console.WriteLine(Messages.Begin);

            DrawTurn();

            MakeGuess();

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

            correctChars = new char[numOfChars];

        }

        private void ParseForLength() {

            if(!Int32.TryParse(Console.ReadLine(), out numOfChars)) {
                Console.Write(Messages.InvalidEntry);
                ParseForLength();
            }
            if (numOfChars <= 0 || numOfChars > MAXLENGTH) {
                Console.Write(Messages.InvalidEntry);
                ParseForLength();
            }
        }

        private void MakeGuess() {



        }

        private void DrawTurn() {

            DrawMan(incorrectGuesses);
            DrawGuesses();

        }

        private void DrawMan(int stage) {

            if(stage < 0 || stage > 6) {
                throw new Exception("Invalid Stage Number!");
            }

            Console.WriteLine(Messages.ResourceManager.GetString(new string("StickMan" + stage)));

        }

        private void DrawGuesses() {

            Console.WriteLine();

            for(int i = 0; i < numOfChars; i++) {
                if (correctChars[i] != default(char)) {
                    Console.Write(" " + correctChars[i]);
                } else {
                    Console.Write(" _");
                }
            }

            Console.WriteLine();

        }

    }
}
