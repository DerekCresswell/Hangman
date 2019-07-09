
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
        const int NUMSTAGES = 6;
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

        private int ParseForLength(int maxLength) {

            int toRet;

            if (!Int32.TryParse(Console.ReadLine(), out toRet)) {
                Console.Write(Messages.InvalidEntry);
                ParseForLength();
            }
            if (toRet < 1 || toRet > maxLength) {
                Console.Write(Messages.InvalidEntry);
                ParseForLength();
            }

            return toRet;

        }

        private bool MakeGuess() {

            ExecGuess(ChooseGuess());

            //Needs improving \/
            if (incorrectGuesses < NUMSTAGES) {
                return true;
            } else {
                Console.WriteLine(Messages.LostGame);
                DrawMan(NUMSTAGES);
                return false;
            }
        }

        private char ChooseGuess() {
            //The logic for this needs to be mostly random for first few
            //Then should use posWords to determine ideal choice

            char toGuess = default(char);

            if(!Contains(correctChars, VOWELS)) {

                Random rnd = new Random();

                toGuess = VOWELS[rnd.Next(VOWELS.Length)];
                while (Contains(guessedChars, toGuess)) {

                    toGuess = VOWELS[rnd.Next(VOWELS.Length)];

                }

            }

            //find most common letter in remaining words

            return toGuess;

        }

        private void ExecGuess(char toGuess) {

            guessedChars.Add(toGuess);

            Console.WriteLine(Messages.CompGuess + toGuess + "?");
            Console.Write(Messages.EnterYN);
            char response = CheckValidity('y', 'n');

            if (response == 'y') {
                Console.Write("\nPlease enter the position of the letter " + toGuess + " (a number between 1 and " + numOfChars + ")" + "\nEnter : ");
                GetCharPositions(toGuess);
            } else {
                Console.WriteLine(Messages.FailedGuess);
                incorrectGuesses++;
            }

            guesses++;

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

        private void GetCharPositions(char guessed) {

            int pos = ParseForLength(numOfChars) - 1;

            if (correctChars[pos] == default(char)) {
                correctChars[pos] = guessed;
                DrawGuesses();
                UpdatePosWords(pos, guessed);
            } else {
                Console.Write(Messages.InvalidEntry);
                GetCharPositions(guessed);
            }

            Console.WriteLine(Messages.NextOccurence + guessed + " in the word?");
            Console.Write(Messages.EnterYN);
            char response = CheckValidity('y', 'n');

            if (response == 'y') {
                Console.Write("\nPlease enter the position of the letter " + guessed + " (a number between 1 and " + numOfChars + ")" + "\nEnter : ");
                GetCharPositions(guessed);
            } 
            //Need escape if you accidently put y

        }

        private void UpdatePosWords(int pos, char check) {

            for(int i = 0; i < posWords.Count; i++) {
                if(posWords[i][pos] != check) {
                    posWords.RemoveAt(i);
                    i--;
                }
            }

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

            if(stage < 0 || stage > NUMSTAGES) {
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
