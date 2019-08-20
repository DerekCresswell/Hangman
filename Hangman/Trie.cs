using System;
using System.Collections.Generic;
using System.Text;

namespace Hangman {

    class Trie {

        private TrieNode rootNode;

        public Trie(){

            rootNode = new TrieNode();

        }

        public void AddWord(string word){

            TrieNode curNode = rootNode;

            foreach(char c in word){

                if(curNode.children[c] != null){
                    curNode = curNode.children[c];
                } else {
                    TrieNode newNode = new TrieNode(c, curNode);
                    curNode.children[c] = newNode;
                    curNode = newNode;
                }

            }

            curNode.completeString = true;

        }

    }

    class TrieNode {

        internal char value;
        internal TrieNode parentNode;
        internal bool completeString;
        internal List<TrieNode> children = new List<TrieNode>();

        internal TrieNode(char value, TrieNode parent){

            this.value = value;
            this.parentNode = parent;

        }

        internal TrieNode() {}

    }

}
