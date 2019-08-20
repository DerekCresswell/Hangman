using System;
using System.Collections.Generic;

namespace Hangman
{
    class Core
    {

        static void Main(string[] args) {

            //Game game = new Game();

            
            Trie t = new Trie();

            t.AddWord("list");
            t.AddWord("lists");
            t.AddWord("listen");
            t.AddWord("be");
            t.AddWord("being");

            Console.WriteLine(t.FindWord("lists"));
            Console.WriteLine(t.FindWord("be"));
            Console.WriteLine(t.FindWord("best"));

        }

    }
}
