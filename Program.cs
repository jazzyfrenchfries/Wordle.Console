using System;
using System.Collections.Generic;
using System.IO;

namespace WordleGame
{
    class Wordle
    {
        public int score;
        public string WordToGuess;
        public int RemainingMoves;
        public bool GameOver;
        public bool GuessOutcome;
        public string errormsg = "ERROR";
        public string completemsg = "Word successfully guessed!";
        private List<string> WordDictionary;

        public Wordle()
        {
            ResetGame();
            ResetMoves();
            LoadWordDictionary();
            WordToGuess = GenerateRandomWord();
        }

        public string GuessWord(string guess)
        {
            if (guess.Length != WordToGuess.Length)
            {
                return errormsg;
            }

            if (guess == WordToGuess)
            {
                GuessOutcome = true;
                GameOver = true;
                score++;
                return completemsg;
            }

            
            Dictionary<char, int> targetLetterCounts = new Dictionary<char, int>();
            foreach (char c in WordToGuess)
            {
                if (targetLetterCounts.ContainsKey(c))
                {
                    targetLetterCounts[c]++;
                }
                else
                {
                    targetLetterCounts[c] = 1;
                }
            }

           
            char[] feedback = new char[guess.Length];
            bool[] isGreen = new bool[guess.Length];

           
            for (int i = 0; i < guess.Length; i++)
            {
                if (guess[i] == WordToGuess[i])
                {
                    feedback[i] = 'G'; 
                    isGreen[i] = true;
                    targetLetterCounts[guess[i]]--;
                }
                else
                {
                    feedback[i] = ' '; 
                }
            }

            
            for (int i = 0; i < guess.Length; i++)
            {
                if (!isGreen[i]) 
                {
                    char guessedLetter = guess[i];
                    if (targetLetterCounts.ContainsKey(guessedLetter) && targetLetterCounts[guessedLetter] > 0)
                    {
                        feedback[i] = 'Y'; 
                        targetLetterCounts[guessedLetter]--;
                    }
                    else
                    {
                        feedback[i] = 'X'; 
                    }
                }
            }

           
            for (int i = 0; i < feedback.Length; i++)
            {
                if (feedback[i] == 'G')
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                else if (feedback[i] == 'Y')
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                Console.Write(guess[i]);
                Console.ResetColor();
            }
            Console.WriteLine();

            RemainingMoves--;
            if (RemainingMoves == 0)
            {
                GameOver = true;
                return $"You failed! Word to guess was {WordToGuess}";
            }

            return "Keep guessing!";
        }

        public void ResetGame()
        {
            WordToGuess = "";
            GameOver = false;
        }

        public void ResetMoves()
        {
            RemainingMoves = 5;
        }
        
        public string GenerateRandomWord()
        {
            Random random = new Random();
            int index = random.Next(WordDictionary.Count);
            return WordDictionary[index];
        }

        private void LoadWordDictionary()
        {
            WordDictionary = new List<string>();
            try
            {
                WordDictionary = new List<string>(File.ReadAllLines("words.txt"));
            }
            catch (Exception)
            {
                Console.WriteLine(errormsg);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Wordle game = new Wordle();

            Console.WriteLine("Welcome to Wordle!");
            while (!game.GameOver)
            {
                Console.WriteLine($"Remaining moves: {game.RemainingMoves}");
                Console.Write("Enter a guess: ");
                string playerGuess = Console.ReadLine();

                string results = game.GuessWord(playerGuess);
                Console.WriteLine(results);
            }
        }
    }
}
