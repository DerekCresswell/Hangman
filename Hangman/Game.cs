
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

        private List<char[]> posWords = new List<char[]>();

        //Const
        const int MAXLENGTH = 25;
        private char[] VOWELS = "aeiou".ToCharArray();
        private char[] CONSONANTS = "bcfghjklmnpqrstvwxyz".ToCharArray();

        public Game() {

            Console.WriteLine(Messages.Welcome);

            FindFilePath();     

            SetWordsArray();

            Console.WriteLine(Messages.Rules);

            GetUserWord();

            Console.WriteLine(Messages.Begin);

            foreach(string s in words) {
                if(s.Length == numOfChars) {
                    posWords.Add(s.ToCharArray());
                }
            }

            DrawTurn();

            while (MakeGuess()) {
                DrawTurn();
            }

            //End game, reset, add new word

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

        private bool MakeGuess() {

            if(!Contains(correctChars, VOWELS)) {

                Random rnd = new Random();

                char toGuess = VOWELS[rnd.Next(VOWELS.Length + 1)];
                while (Contains(guessedChars, toGuess)) {

                    toGuess = VOWELS[rnd.Next(VOWELS.Length + 1)];

                }
                guessedChars.Add(toGuess);

                Console.WriteLine(Messages.CompGuess + toGuess + "?");
                Console.Write("\nPlease Enter Y or N\nEnter : "); //Messages
                char response = CheckValidity('y', 'n');

                if (response == 'y') {
                    //GetCharPositions();
                } else {
                    Console.WriteLine(Messages.FailedGuess);
                    incorrectGuesses++;
                }

                guesses++;

            }

            //temp
            return false;
        }

        private char CheckValidity(char i, char j) {

            char[] curChar = Console.ReadLine().ToLower().ToCharArray();

            /*
            if (curChar.Length != 1) {
                Console.Write(Messages.InvalidEntry);
                return CheckValidity(i, j);
            } else if(curChar[0] != i || curChar[0] != j) {
                Console.Write(Messages.InvalidEntry);
                return CheckValidity(i, j);
            }
            */

            if (curChar[0] == i || curChar[0] == j) {
                return curChar[0];
            } else {
                Console.Write(Messages.InvalidEntry);
                return CheckValidity(i, j);
            }

            //return curChar[0];

        }

        private bool Contains(char[] cArr, char[] check) {

            foreach (char c1 in cArr) {
                foreach (char c2 in check) {
                    if (c1 == c2) {
                        return true;
                    }
                }
            }

            return false;

        }

        private bool Contains(List<char> cArr, char check) {

            foreach(char c in cArr) {
                if(c == check) {
                    return true;
                }
            }

            return false;

        }

        private void DrawTurn() {
            try {
                DrawMan(incorrectGuesses);
            } catch {
                //End game
            }
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

            Console.WriteLine("\n");

        }

    }
}
