namespace OOPLinkedListsProject
{
    internal class Program
    {
        static void Main(string[] args)
        {


            GameField.Player player1 = new GameField.Player("x");
            GameField.Player player2 = new GameField.Player("y");
            GameField game = new GameField(player1, player2);

            int diceP1 = -1;
            int diceP2 = -1;
            int round = 1;


            while (!(player1.won || player2.won))
            {

                game.PrintCurrentState(round++, player1, player2);     //change player each loop iteration


                Console.WriteLine("Player One Throws his Dice: ");
                diceP1 = GameField.Player.ThrowDice();
                Console.WriteLine($"Player One got a {diceP1}");

                game.Update(player1, player2, diceP1); //need both players to check if same position. updates player 1
                if (player1.won) break;

                Console.WriteLine("");

                Console.WriteLine("Player Two Throws his Dice: ");
                diceP2 = GameField.Player.ThrowDice();
                Console.WriteLine($"Player Two got a {diceP2}");

                game.Update(player2, player1, diceP2);

                Console.WriteLine("Ready for next Round?");
                Console.ReadLine();
            }

            if (player1.won)
            {
                Console.WriteLine($"Congratiulation {player1.name}!!! YOU WONNN");
            }
            else if (player2.won)
            {
                Console.WriteLine($"Congratiulation {player2.name}!!! YOU WONNN");
            }

        }


        static void test()
        {
            GameField.Player p1 = new GameField.Player("efe");
            GameField.Player p2 = new GameField.Player("kan");

            GameField game = new GameField(p1, p2);

            p1.ForewardMove(game, 20);
            Console.WriteLine(p1.position.tileNr);
            //game.ExpandMapBeforeCurrent(5, p1);
            Console.WriteLine(p1.position.tileNr);
            p1.BackwardMove(game, 6);
            Console.WriteLine(p1.position.tileNr);
        }



    }
}
