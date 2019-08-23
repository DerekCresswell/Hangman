﻿using System;
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

                int indexOfC = IndexOf(curNode.children, c);

                if(indexOfC != -1){
                    curNode = curNode.children[indexOfC];
                } else {
                    TrieNode newNode = new TrieNode(c, curNode);
                    curNode.children.Add(newNode);
                    curNode = newNode;
                }

            }

            curNode.completeString = true;

        }

        public bool FindWord(string word){

            TrieNode curNode = rootNode;

            foreach(char c in word){

                int indexOfC = IndexOf(curNode.children, c);

                if(indexOfC != -1){
                    curNode = curNode.children[indexOfC];
                } else {
                    return false;
                }

            }

            return true;

        }

        public void RemoveWord(string word){

            TrieNode curNode = rootNode;
            List<TrieNode> path = new List<TrieNode>();

            foreach(char c in word){

                int indexOfC = IndexOf(curNode.children, c);

                if(indexOfC != -1){
                    curNode = curNode.children[indexOfC];
                    path.Insert(0, curNode);
                } else {
                    return;
                }

            }

            bool flagFirst = false;

            for(int i = 1; i < path.Count; i++){

                TrieNode parent = path[i];
                TrieNode child = path[i - 1];

                if(child.completeString || child.children.Count != 0){
                    if(!flagFirst){
                        flagFirst = true;
                        parent.children.Remove(child);
                    } else {
                        return;
                    }
                } else {
                    parent.children.Remove(child);
                }

            }

        }

        //Temp, Hopefully
        private int IndexOf(List<TrieNode> tArr, char check) {

            for(int i = 0; i < tArr.Count; i++){
                if(tArr[i].value == check)
                    return i;
            }

            return -1;

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

        internal TrieNode(){}

    }

}
