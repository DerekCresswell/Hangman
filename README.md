# Hangman
A console version of Hangman written in C# using Visual Studio.

One of the main goals of this project (other than having a friend to play games with) was to familiarize with standalone C# applications as well as using recursion in my code. I've found it somewhat difficult to find good places for recursion in other projects. The nature of verifying input from the user and having multiple letters in a single word setup a great enviroment to use recursive functions.

# Future Plans

* Learning, the program will learn new words and ideas based off of your words.
* Command line access, allow this program to be called and played in the command line without needing a standalone app.
* Implement a Trie structure to improve the storage and guesing of words.

# Current Issues

* When Hangman guesses the word it will guess "System.char[]", cause unknown.
* When a new word is learned even with valid word entry an error line is still printed, cause unknown
