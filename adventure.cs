using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

/*
 Cool ass stuff people could implement:
 > jumping
 > attacking
 > randomly moving monsters
 > smarter moving monsters
*/
namespace asciiadventure {
    public class Game {
        private Random random = new Random();
        private static Boolean Eq(char c1, char c2){
            return c1.ToString().Equals(c2.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private static string Menu() {
            return "WASD to move\nIJKL to attack/interact\nEnter command: ";
        }
        private static void PrintScreen(Screen screen, string message, string menu) {
            Console.Clear();
            Console.WriteLine(screen);
            Console.WriteLine($"\n{message}");
            Console.WriteLine($"\n{menu}");
        }
        public void Run() {
            Console.ForegroundColor = ConsoleColor.Green;

            Screen screen = new Screen(10, 10);
            // add a couple of walls
            for (int i=0; i < 3; i++){
                new Wall(1, 2 + i, screen);
            }
            for (int i=0; i < 4; i++){
                new Wall(3 + i, 4, screen);
            }

            // add a player
            Player player = new Player(0, 0, screen, "Zelda");
            
            // add a treasure
            Treasure treasure = new Treasure(6, 2, screen);

            // add a sword
            Sword sword = new Sword(8,7,screen);

            // add some mobs
            List<Mob> mobs = new List<Mob>();
            mobs.Add(new Mob(9, 9, screen));
            
            // initially print the game board
            PrintScreen(screen, "Welcome!", Menu());
            
            Boolean gameOver = false;

            Boolean hasTreasure = false;

            while (!gameOver) {
                char input = Console.ReadKey(true).KeyChar;

                String message = "";

                int? actionRow = null;
                int? actionCol = null;

                if (Eq(input, 'q')) {
                    break;
                } else if (Eq(input, 'w')) {
                    player.Move(-1, 0);
                } else if (Eq(input, 's')) {
                    player.Move(1, 0);
                } else if (Eq(input, 'a')) {
                    player.Move(0, -1);
                } else if (Eq(input, 'd')) {
                    player.Move(0, 1);
                } else if (Eq(input, 'i')) {
                    message += player.Action(-1, 0) + "\n";
                    actionRow = player.Row-1;
                    actionCol = player.Col;
                } else if (Eq(input, 'k')) {
                    message += player.Action(1, 0) + "\n";
                    actionRow = player.Row+1;
                    actionCol = player.Col;
                } else if (Eq(input, 'j')) {
                    message += player.Action(0, -1) + "\n";
                    actionRow = player.Row;
                    actionCol = player.Col-1;
                } else if (Eq(input, 'l')) {
                    message += player.Action(0, 1) + "\n";
                    actionRow = player.Row;
                    actionCol = player.Col+1;
                } else if (Eq(input, 'v')) {
                    // TODO: handle inventory
                    message = "You have nothing\n";
                } else {
                    message = $"Unknown command: {input}";
                }
                if(message.Equals("Yay, we got the treasure!\n"))
                    hasTreasure = true;
                // OK, now move the mobs
                foreach (Mob mob in mobs){
                    // TODO: Make mobs smarter, so they jump on the player, if it's possible to do so
                    if((actionRow!= null)&&player.Armed){
                        if((mob.Row==actionRow)&&(mob.Col==actionCol)){
                            gameOver = true;
                            mob.Token = "*";
                        }
                    }
                    else{
                        List<Tuple<int, int>> moves = screen.GetLegalMoves(mob.Row, mob.Col);
                        if (moves.Count == 0){
                            continue;
                        }
                        // mobs move randomly
                        var (deltaRow, deltaCol) = moves[random.Next(moves.Count)];
                        if (screen[mob.Row + deltaRow, mob.Col + deltaCol] is Player){
                            if(!hasTreasure){
                                mob.Token = "*";
                                message += "A MOB GOT YOU! GAME OVER\n";
                                gameOver = true;
                            }
                            else{
                                message += "You used the treasure to revive yourself\n";
                                hasTreasure = false;
                            }
                        }
                    mob.Move(deltaRow, deltaCol);
                    }
                }
                PrintScreen(screen, message, Menu());
            }
        }

        public static void Main(string[] args){
            Game game = new Game();
            game.Run();
        }
    }
}