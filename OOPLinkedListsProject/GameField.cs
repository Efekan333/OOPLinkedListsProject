using System;

using System.Collections.Generic;
using System.Text;

namespace OOPLinkedListsProject
{
    public class GameField
    {
        private static int nextTileNr = 0; //testzwecke
        internal FieldNode? head = null;
        internal FieldNode? tail = null;
        private int FieldLength { get; set; }
        private int RoundCount { get; set; } //implementieren
        public enum NodeType {none , snake, ladder}
        
        public class FieldNode
        {
            //public bool Snake { get; set; } = false; //ALTE BOOLS
            //public bool Ladder { get; set; } = false; //ALTE BOOLS
            public NodeType nodeType = NodeType.none;
            public FieldNode? next = null;
            public FieldNode? prev = null;
            private GameField parent;
            public int tileNr; //testzwecke
            
            public FieldNode(FieldNode prev, FieldNode next)
            {
                this.prev = prev;
                this.next = next;
                tileNr = nextTileNr; //testzwecke
                nextTileNr++; //testzwecke
            }
        }


        public class Player
        {
            public bool won;
            public string name;
            private GameField parent;
            public FieldNode position;
            public int throws = 0;

            public Player(string name, GameField parent)
            {
                this.name = name;
                this.parent = parent;
                this.position = parent.head;
            }


            public void ForewardMove(GameField game, int n)
            {
                while (position!= game.tail)
                {
                    if (n-- == 0) break;
                    position = position.next;
                }

                if (position == game.tail)
                {
                    won = true;
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
                Random rd = new Random();
                int diceThrow = rd.Next(6) + 1; 
                return diceThrow;
            }
            
            public override string ToString()
            {
                return name;
            }
        }

        public GameField(int length)
        {
            FieldLength = length * length; //quadratisches Spielfeld

            for (int i = 1; i <= FieldLength; i++)
            {
                InsertNode(i);
            }
        }
        
        private void InsertNode(int i)
        {
            FieldNode newNode = new FieldNode(null, head);
            if (head == null)
            {
                tail = newNode;
            }
            else
            {
                head.prev = newNode;
            }
            head = newNode;
            
            if(i % 5 == 0)
            {
                newNode.nodeType = NodeType.ladder;
            }

            else if (i % 8 == 0)
            {
                newNode.nodeType = NodeType.snake; 
            }
        }

        void FixTail(FieldNode oldTail, int addedTiles)
        {
            FieldNode nextNode = oldTail.next;
            for(int i = 1; i <= addedTiles; i++)
            {
                nextNode.tileNr = oldTail.tileNr + i;
                nextNode = nextNode.next;
            }
        }

        void FixTileNr(Player currentPlayer, int addedTiles)
        {
            FieldNode currentPosition = currentPlayer.position;
            for (int i = currentPosition.tileNr; i < FieldLength; i++) //Teile rechts fixen
            {
                
                currentPosition.tileNr += 5;
                currentPosition = currentPosition.next;
            }
            currentPosition = currentPlayer.position.prev;
            int id = currentPlayer.position.tileNr;
            for (int i = 1; i <= 5; i++) //Teile links fixen
            {
                currentPosition.tileNr = id - i;
                currentPosition = currentPosition.prev;
            }
        }
        
        public void PrintCurrentState(Player p1, Player p2)
        {
            Console.Clear();
            Console.WriteLine($"Current Round: {RoundCount++}");

            Console.WriteLine($"{p1.name} is currently in Tile: {p1.position.tileNr}"); //TODO IF APPENDING NEW NODES WATHC OUT THAT TILENR IS STILL ACCURATE
            Console.WriteLine($"{p2.name} is currently in Tile: {p2.position.tileNr}");
            
            // Vorrübergehender Printstate mit tileNr, wird mit graphischer Darstellung ohne ID ersetzt
        }


        public void Update(Player currentPlayer,Player otherPlayer,  int diceThrow )
        {
            
            currentPlayer.ForewardMove(this ,diceThrow);

            if (currentPlayer.position == tail) //win takes precendence over snake and other stuff. 
            {
                currentPlayer.won = true;
                return;
            }
            
            if (currentPlayer.position.nodeType == NodeType.snake)
            {
                currentPlayer.BackwardMove(this, 3);
                Console.WriteLine("SNAKE!!");
            }
            
            else if (currentPlayer.position.nodeType == NodeType.ladder)
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
                FieldNode oldTail = tail;
                ExpandMapEnd(5);
                //FixTail(oldTail ,5);
                Console.WriteLine("The Map expands by 5 tiles!");
            }

            if (diceThrow == 6)
            {
                int n = 5;
                Console.WriteLine("You expand the map behind you by 5 tiles!");
                ExpandMapBeforeCurrent(n, currentPlayer);                
                //FixTileNr(currentPlayer, n);
            }

        }

       

        //Expand Maps Funktionen
        private void ExpandMapEnd(int n)
        {
            for (int i = 0; i < n; i++)
            {
                FieldNode newNode = new FieldNode(tail, null);
                tail.next = newNode;
                tail = newNode;
            }
            FieldLength += 5;
        }

        private void ExpandMapBeforeCurrent(int n, Player currentPlayer) //before means left of it
        {
            
            //cant be tail, because checks in Update() for it:
            if(currentPlayer.position == head)
            {
                InsertNode(n);
            }
            
            else
            {
                for(int i = 0; i < n; i++)
                {
                    FieldNode newNode = new FieldNode(currentPlayer.position.prev, currentPlayer.position);
                    currentPlayer.position.prev.next = newNode;
                    currentPlayer.position.prev = newNode;
                }
            }
            
        }

        public void testGameBoard()
        {
            FieldNode? start = head;
            while (start != null)
            {
                Console.WriteLine($"{start.tileNr} {start.nodeType}");
                start = start.next;
            }
        }
    }
}
