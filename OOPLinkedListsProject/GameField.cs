using System;

using System.Collections.Generic;
using System.Text;

namespace OOPLinkedListsProject
{
    public class GameField
    {
        private FieldNode? head = null;
        private FieldNode? tail = null;

        public GameField(Player player1, Player player2)
        {
            int length;

            do
            {
                Console.WriteLine("How Long should the Map be? (10 <= input <= 500) ");
                length = Convert.ToInt32(Console.ReadLine());

            } while (length < 10 && length > 500);



            FieldNode firstNode = new FieldNode();

            head = firstNode;
            tail = firstNode;

            for (int i = length - 1; i > 0; i--)
            {
                FieldNode newNode = new FieldNode();
                FillNodeWithInfo(newNode, i);
                head.prev = newNode;
                newNode.next = head;
                head = newNode;
                newNode.prev = null;
            }

            player1.position = player2.position = head;
        }



        private void FillNodeWithInfo(FieldNode node, int pos)
        {
            node.tileNr = pos;

            if(pos % 5 == 0)
            {
                node.Ladder = true;
            }

            else if (pos % 8 == 0)
            {
                node.Snake = true; 
            }
        }

        void FixTileNr(Player currentPlayer, int addedTiles)
        {
            int startTileNr = currentPlayer.position.tileNr;
            int tmp = startTileNr;

            for(int i = 0; i < addedTiles; i++)
            {
                currentPlayer.BackwardMove(this, 1);
                currentPlayer.position.tileNr = --tmp;
            }

            while (currentPlayer.position != head) // should be the head at the end
            {
                currentPlayer.BackwardMove(this, 1);
                currentPlayer.position.tileNr -= 5;
            }

            currentPlayer.ForewardMove(this, startTileNr - 1);

            




            
        }
        



        public void PrintCurrentState(int round, Player p1, Player p2)
        {
            Console.Clear();
            Console.WriteLine($"Current Round: {round}");

            Console.WriteLine($"{p1.name} is currently in Tile: {p1.position.tileNr}"); //TODO IF APPENDING NEW NODES WATHC OUT THAT TILENR IS STILL ACCURATE
            Console.WriteLine($"{p2.name} is currently in Tile: {p2.position.tileNr}");

        }


        public void Update(Player currentPlayer,Player otherPlayer,  int diceThrow )
        {
            currentPlayer.ForewardMove(this ,diceThrow);

            if (currentPlayer.position == tail) //win takes precendence over snake and other stuff. 
            {
                currentPlayer.won = true;
                return;
            }


            if (currentPlayer.position.Snake)
            {
                currentPlayer.BackwardMove(this, 3);
                Console.WriteLine("SNAKE!!");

            }

            else if (currentPlayer.position.Ladder)
            {
                currentPlayer.ForewardMove(this, 3);
                Console.WriteLine("LADDERR!!!");

            }


            if (currentPlayer.position == otherPlayer.position)
            {
                currentPlayer.BackwardMove(this, 1);
                Console.WriteLine("YOU HIT THE OTHER PLAYER!!");

            }

            if (diceThrow == 1)
            {
                ExpandMapEnd(5);
                Console.WriteLine("The Map expands by 5 tiles!");

            }

            if (diceThrow == 6)
            {
                int n = 5;
                Console.WriteLine("You expand the map behind u by 5 tiles!");
                ExpandMapBeforeCurrent(n, currentPlayer);                
                //FixTileNr(currentPlayer, n);

            }

        }

       

        private void ExpandMapEnd(int n)
        {
            FieldNode newNode = new FieldNode();
            for (int i = 0; i < n; i++)
            {
                tail.next = newNode;
                newNode.prev = tail;
                tail = newNode;
                newNode.next = null;
            }
        }
        private void ExpandMapBeforeCurrent(int n, Player currentPlayer) //before means left of it
        {
            //cant be tail, because checks in Update() for it:
            
            
            if(currentPlayer.position == head)
            {
                for (int i = 0; i < n; i++)
                {
                    FieldNode newNode = new FieldNode();
                    head.prev = newNode;
                    newNode.next = head;
                    head = newNode;
                    newNode.prev = null;
                }
                
            }
            else
            {
                for(int i = 0; i < n; i++)
                {
                    FieldNode newNode = new FieldNode();
                    newNode.next = currentPlayer.position;
                    currentPlayer.position.prev.next = newNode;
                    newNode.prev = currentPlayer.position.prev;
                    currentPlayer.position.prev = newNode;

                }
            }




        }


        public class FieldNode
        {
            public bool Snake { get; set; } = false;
            public bool Ladder { get; set; } = false;

            public FieldNode? next = null;
            public FieldNode? prev = null;
            public int tileNr;
        }


        public class Player
        {
            public bool won;
            public string name;
            public FieldNode position;

            public Player(string name)
            {
                this.name = name;
            }


            public void ForewardMove(GameField game, int n)
            {
                while (position!= game.tail)
                {
                    if (n-- == 0) break;

                    position = position.next;

                }
            }

            public void BackwardMove(GameField game, int n)
            {
                while (position != game.head)
                {
                    if (n-- == 0) break;

                    position = position.prev;


                }
            }

            public static int ThrowDice()
            {
                Console.WriteLine("Press enter to throw dice");
                Console.ReadLine();
                Random rd = new Random();
                int diceThrow = rd.Next(6) + 1; 
                return diceThrow;
            }



        }



    }
}
