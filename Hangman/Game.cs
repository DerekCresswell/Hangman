
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

        private bool changeCommonLetters = true;
        private int[] commonLetters;
        private List<char[]> posWords = new List<char[]>();

        private List<string> words = new List<string>();

        //Const
        const int MAXLENGTH = 25;
        const int NUMSTAGES = 6;
        const int ASCIICONST = 97;
        private char[] VOWELS = "aeiouy".ToCharArray();
        private char[] CONSONANTS = "bcfghjklmnpqrstvwxz".ToCharArray();
        private char[] ALPHABET = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        public Game() {

            Console.WriteLine(Messages.Welcome);

            FindFilePath();     

            SetWordsArray();

            Console.WriteLine(Messages.Rules);

            RUNGAME();

            //End game, reset, add new word

        }

        private void RUNGAME() {

            GetUserWord();

            Console.WriteLine(Messages.Begin);

            foreach (string s in words)
            {
                if(s.Length == numOfChars){
                    posWords.Add(s.ToCharArray());
                }
            }

            DrawTurn();

            while (MakeGuess())
            {
                DrawTurn();
                if (WordFound(correctChars))
                {
                    Console.WriteLine(Messages.CompWin);
                    break;
                }
            }

            Clear();

            Console.WriteLine(Messages.PlayAgain);
            Console.Write(Messages.EnterYN);

            char response = CheckValidity('y', 'n');

            if (response == 'y') {
                RUNGAME();
            } else {
                Console.WriteLine(Messages.GoodBye);
            }

        }

        private void Clear() {

            numOfChars = new int();
            guessedChars = new List<char>();
            guesses = 0;
            incorrectGuesses = 0;
            posWords = new List<char[]>();

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

                string line;

                while ((line = sr.ReadLine()) != null) {
                    words.Add(line);
                }

            }

        }

        private void FindFilePath()
        {

            //Check to see if this works in and app, not just VS
            filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\wordBank.txt"));

            if (filePath == null) {
                Console.WriteLine(Messages.InvalidFilePath);
                Console.Write(Messages.EnterFilePath);
                filePath = Console.ReadLine();
            }

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

        private bool WordFound(char[] cArr) {

            foreach(char c in cArr) {
                if(c == default(char)) {
                    return false;
                }
            }

            return true;

        }

        private bool MakeGuess() {

            if(posWords.Count == 1) {
                Console.WriteLine(Messages.GuessWord + posWords[0] + "?");
                Console.Write(Messages.EnterYN);
                char response = CheckValidity('y', 'n');
                if (response == 'y') {
                    Console.WriteLine(Messages.CompWin);
                    return false;
                } else {
                    Console.WriteLine(Messages.GuessWordWrong);
                }
            }

            ExecGuess(ChooseGuess());

            //Needs improving \/
            if (incorrectGuesses < NUMSTAGES) {
                return true;
            } else {
                Console.WriteLine(Messages.LostGame);
                DrawMan(NUMSTAGES);
                LearnNewWord();
                return false;
            }
        }

        private char ChooseGuess() {

            //TEMP
            changeCommonLetters = true;

            if(changeCommonLetters) {
                ChangeCommonLetters();
            }

            char toGuess = default(char);
            int max = 0;

            if (!Contains(correctChars, VOWELS)) {

                foreach(char c in VOWELS) {
                    if (commonLetters[((int)c) - ASCIICONST] > max) {
                        if (!Contains(guessedChars, c)) {
                            max = commonLetters[((int)c) - ASCIICONST];
                            toGuess = c;
                        }
                    }
                }

                return toGuess;

            }

            foreach (char c in ALPHABET) {
                if (commonLetters[((int)c) - ASCIICONST] > max) {
                    if(!Contains(guessedChars, c)) {
                        max = commonLetters[((int)c) - ASCIICONST];
                        toGuess = c;
                    }
                }
            }

            return toGuess != default(char) ? toGuess : getRandomGuess();

        }

        private void ExecGuess(char toGuess) {

            guessedChars.Add(toGuess);

            Console.WriteLine(Messages.CompGuess + toGuess + "?");
            Console.Write(Messages.EnterYN);
            char response = CheckValidity('y', 'n');

            if (response == 'y') {

                if(CharsLeft(correctChars) == 1) {

                    int index = 0;

                    //Error catching needed
                    while(correctChars[index] != default(char)) {
                        index++;
                    }

                    correctChars[index] = toGuess;
                    DrawGuesses();
                    return;

                }


                Console.Write("\nPlease enter the position of the letter " + toGuess + " (a number between 1 and " + numOfChars + ")" + "\nEnter : ");
                GetCharPositions(toGuess);
                changeCommonLetters = true;
            } else {
                Console.WriteLine(Messages.FailedGuess);
                incorrectGuesses++;
            }

            guesses++;

        }

        private int CharsLeft(char[] cArr) {

            int count = 0;

            foreach(char c in cArr) {
                if(c == default(char))
                    count++;
            }

            return count;

        }

        private void ChangeCommonLetters() {

            commonLetters = new int[26];

            foreach(char[] cArr in posWords) {
                for(int i = 0; i < cArr.Length; i++) {
                    commonLetters[((int)cArr[i]) - ASCIICONST]++;
                }
            }

            changeCommonLetters = false;

        }

        private char CheckValidity(char i, char j) {

            char[] curChar = Console.ReadLine().ToLower().ToCharArray();

            if (curChar[0] == i || curChar[0] == j) {
                return curChar[0];
            } else {
                Console.Write(Messages.InvalidEntry);
                return CheckValidity(i, j);
            }

        }

        private void GetCharPositions(char guessed) {

            int pos = ParseForLength(numOfChars) - 1;

            if(correctChars[pos] == default(char)) {
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

            if(response == 'y') {
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

        private char getRandomGuess() {

            Random rand = new Random();
            char toGuess = ALPHABET[rand.Next(0, ALPHABET.Length)];

            if(guessedChars.Count == ALPHABET.Length)
                return default(char);

            while(Contains(guessedChars, toGuess)) {
                toGuess = ALPHABET[rand.Next(0, ALPHABET.Length)];
            }

            return toGuess;

        }

        private void LearnNewWord() {

            Console.WriteLine(Messages.WhatWasWord);
            string actualWord;

            //Check if char in actual was guessed
            while(GuessedMatchesActual(out actualWord)) { }

            if (words.Contains(actualWord)) {
                Console.WriteLine(Messages.AlreadyKnown);
            } else {
                Console.WriteLine(Messages.NewWord);
                AddWordToBank(actualWord);
            }

        }

        private bool GuessedMatchesActual(out string actualWord) {

            Console.Write(Messages.EnterActual);
            actualWord = Console.ReadLine().ToLower();
            char[] actualArr = actualWord.ToCharArray();

            if(actualWord.Length != correctChars.Length) {
                Console.WriteLine(Messages.InvalidEntry);
                return false;
            } else {
                for (int i = 0; i < actualWord.Length; i++) {
                    if (actualArr[i] != correctChars[i] || correctChars[i] == default(char)) {
                        Console.WriteLine(Messages.InvalidEntry);
                        return false;
                    }
                }
            }

            return true;

        }

        private void AddWordToBank(String newWord) {

            //Should always be lower case already
            int bankIndex = 0;
            int charIndex = 0;

            bankIndex = binarySearch(words, newWord, 0);
            while(words[bankIndex][0] == newWord[0]) {
                bankIndex--;
            }
            bankIndex++;
            //find higher bound index?

            //Continue through next letters

        }

        private int binarySearch(List<string> bank, string word, int targetIndex) {

            int lowerBound = 0;
            int higherBound = words.Count;

            while (lowerBound <= higherBound)
            {

                int midPoint = (lowerBound + higherBound) / 2;
                if ((int)bank[midPoint][targetIndex] > (int)word[targetIndex]) {
                    higherBound = midPoint - 1;
                } else if ((int)bank[midPoint][targetIndex] < (int)word[targetIndex]) {
                    lowerBound = midPoint + 1;
                } else {
                    return midPoint;
                }

            }

            return -1;

        }

    }

}
