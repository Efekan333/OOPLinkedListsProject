namespace OOPLinkedListsProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameField.Player? currentPlayer = null;
            GameField.Player? inactivePlayer = null;
            
            
            Console.Write("Geben Sie eine Breite für das Spielfeld ein: ");
            GameField feld = new GameField(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine($"Euer Spielfeld ist {feld.FieldLength} Felder lang");
            Console.Write("Spieler 1: ");
            GameField.Player player1 = new GameField.Player(Console.ReadLine(), feld);
            Console.Write("Spieler 2: ");
            GameField.Player player2 = new GameField.Player(Console.ReadLine(), feld);
            Console.Write("Drücke Enter um zu beginnen");
            Console.ReadLine();
            Console.Clear();
            feld.PrintCurrentState(player1, player2);
            
            int rundenToken = 0;
            while (!(player1.won || player2.won))
            {
                switch (rundenToken)
                {
                    case 0:
                        currentPlayer = player1;
                        inactivePlayer = player2;
                        break;
                    case 1:
                        currentPlayer = player2;
                        inactivePlayer = player1;
                        break;
                }
                
                Console.Write($"{currentPlayer} ist dran!\nDrücke Enter, um zu würfeln");
                Console.ReadLine();
                int diceThrow = GameField.Player.ThrowDice();
                Console.WriteLine($"{currentPlayer} hat eine {diceThrow} gewürfelt");
                
                feld.Update(currentPlayer, inactivePlayer, diceThrow);
            
                if (currentPlayer.won)
                {
                    Console.WriteLine($"{currentPlayer} hat gewonnen!");
                    break;
                }
                
                Console.ReadLine();
            
                feld.PrintCurrentState(player1, player2);
                feld.PrintBoard(player1, player2);
                
                rundenToken = (rundenToken + 1) % 2;
            }  
            
            //testy test
            // GameField testField = new GameField(10);
            //  testField.testGameBoard();
        }
    }
}
