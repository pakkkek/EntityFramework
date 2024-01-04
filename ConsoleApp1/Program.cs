using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ConsoleApp1
{
    public class GameInfo
    {
        [Key]
        public int GameId { get; set; }
        public string Name { get; set; }
        public string Studio { get; set; }
        public string Style { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool Multiplayer { get; set; }
        public int Sales { get; set; }

        public GameInfo(string name, string studio, string style, DateTime releaseDate, bool multiplayer, int sales)
        {
            Name = name;
            Studio = studio;
            Style = style;
            ReleaseDate = releaseDate;
            Multiplayer = multiplayer;
            Sales = sales;
        }
        public void Print()
        {
            Console.WriteLine($"Game name: {Name}");
            Console.WriteLine($"Game studio: {Studio}");
            Console.WriteLine($"Game style: {Style}");
            Console.WriteLine($"Game realease date: {ReleaseDate}");

        }
    }

    public class GameDbContext : DbContext
    {
        public string connectionString = @"Data Source=WIN-0I7PB3TGH35\SQLEXPRESS;Initial Catalog=Game;Integrated Security=True;Encrypt=False";

        public GameDbContext()
        {
        }

        public DbSet<GameInfo> GameInfo { get; set; }
        public GameInfo FindGameByName(string name)
        {
            var game = this.GameInfo.Where(x => x.Name == name).FirstOrDefault();
            return game;
        }

        public GameInfo FindGamesByStudioName(string studioName)
        {
            var game = this.GameInfo.Where(x => x.Studio == studioName).FirstOrDefault();
            return game;
        }

        public GameInfo FindGameByStudioNameAndGameName(string studioName, string gameName)
        {
            var game = this.GameInfo.Where(x => x.Studio == studioName && x.Name == gameName).FirstOrDefault();
            return game;
        }
        public GameInfo FindGamesByStyle(string style)
        {
            var game = this.GameInfo.Where(x => x.Style == style).FirstOrDefault();
            return game;
        }
        public GameInfo FindGamesByReleaseYear(int releaseYear)
        {
            var game = this.GameInfo.Where(x => x.ReleaseDate.Year == releaseYear).FirstOrDefault();
            return game;
        }

        public GameInfo FindSinglePlayerGames()
        {
            var game = this.GameInfo.Where(x => x.Multiplayer == false).FirstOrDefault();
            return game;
        }
        public GameInfo FindMultiplayerGames()
        {
            var game = this.GameInfo.Where(x => x.Multiplayer == true).FirstOrDefault();
            return game;
        }
        public GameInfo FindGameWithMaxSales()
        {
            var game = this.GameInfo.Where(x => x.Sales == this.GameInfo.Max(x => x.Sales)).FirstOrDefault();
            return game;
        }

        public GameInfo FindGameWithMinSales()
        {
            var game = this.GameInfo.Where(x => x.Sales == this.GameInfo.Min(x => x.Sales)).FirstOrDefault();
            return game;
        }
        public IEnumerable<GameInfo> FindTop3PopularGames()
        {
            return this.GameInfo.OrderByDescending(x => x.Sales).Take(3);
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo culture = new CultureInfo("uk-UA");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Console.OutputEncoding = Encoding.UTF8;

            GameInfo game1 = new GameInfo("Genshin Impact", "miHoYo", "RPG", new DateTime(2023, 11, 08), true, 1000);
            GameInfo game2 = new GameInfo("Subway Surfers", "Imangi Studios", "Endless runner", new DateTime(2015, 01, 16), true, 900);
            GameInfo game3 = new GameInfo("The Legend of Zelda: Tears of the Kingdom", "Nintendo Entertainment Planning & Development", " Puzzle, Action-adventure, Fighting, Shooter", new DateTime(2023, 03, 12), true, 800);
            GameInfo game4 = new GameInfo("Marvel's Spider-Man", " Insomniac Games", "Action adventure, Fighting, Platformer", new DateTime(2023, 10, 20), false, 700);
            GameInfo game5 = new GameInfo("FC 24", "Electronic Arts, EA Sports, EA Romania, EA Vancouver", "Sports simulator, Simulator, Game simulation", new DateTime(2022, 09, 27), false, 600);

            try
            {
                using (var db = new GameDbContext())
                {

                    db.SaveChanges();

                    var games = db.GameInfo.ToList();

                    foreach (var game in games)
                    {
                        game.Print();
                        Console.WriteLine(" ");
                    }

                    Console.WriteLine("1 - Task 1");
                    Console.WriteLine("2 - Task 2");
                    Console.WriteLine("3 - Task 3");
                    Console.WriteLine("");
                    int num;
                    do
                    {
                        Console.WriteLine("Enter number:");
                        num = Convert.ToInt32(Console.ReadLine());
                        switch (num)
                        {
                            case 1:
                                {

                                    int num1;
                                    do
                                    {
                                        Console.WriteLine("1 - Search for information by game name.");
                                        Console.WriteLine("2 - Search games by studio name.");
                                        Console.WriteLine("3 - Search for information by studio and game name.");
                                        Console.WriteLine("4 - Search games by style.");
                                        Console.WriteLine("5 - Search games by year of release.");

                                        Console.WriteLine("");
                                        Console.WriteLine("Enter number:");
                                        num1 = Convert.ToInt32(Console.ReadLine());
                                        switch (num1)
                                        {
                                            case 0:
                                                break;

                                            case 1:
                                                {
                                                    Console.WriteLine("Enter game name -->");
                                                    string nameGame = Console.ReadLine();
                                                    Console.WriteLine(" ");
                                                    var game = db.FindGameByName(nameGame);

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                break;
                                            case 2:
                                                {
                                                    Console.WriteLine("Enter game studio -->");
                                                    string studioName = Console.ReadLine();
                                                    Console.WriteLine(" ");
                                                    var game = db.FindGamesByStudioName(studioName);

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                break;
                                            case 3:
                                                {
                                                    Console.WriteLine("Enter game name -->");
                                                    string name = Console.ReadLine();
                                                    Console.WriteLine(" ");
                                                    Console.WriteLine("Enter game studio -->");
                                                    string studioName = Console.ReadLine();
                                                    Console.WriteLine(" ");
                                                    var game = db.FindGameByStudioNameAndGameName(name, studioName);

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                break;
                                            case 4:
                                                {
                                                    Console.WriteLine("Enter game style -->");
                                                    string style = Console.ReadLine();
                                                    Console.WriteLine(" ");
                                                    var game = db.FindGamesByStyle(style);

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                break;
                                            case 5:
                                                {
                                                    Console.WriteLine("Enter game realese date -->");
                                                    int releaseYear = Convert.ToInt32(Console.ReadLine()); ;
                                                    Console.WriteLine(" ");
                                                    var game = db.FindGamesByReleaseYear(releaseYear);

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                break;
                                        }
                                    } while (num1 != 0);
                                }
                                break;
                            case 2:
                                {
                                    int num2;
                                    do
                                    {

                                        Console.WriteLine(" 1 - Display information about all single-player games. ");
                                        Console.WriteLine(" 2 - Display information about all multiplayer games. ");
                                        Console.WriteLine(" 3 - Show the game with the maximum number of games sold. ");
                                        Console.WriteLine(" 4 - Show the game with the minimum number of games sold. ");
                                        Console.WriteLine(" 5 - Displaying the Top 3 most popular games. ");
                                        Console.WriteLine(" 6 - Displaying the Top 3 most unpopular games. ");

                                        Console.WriteLine("");
                                        Console.WriteLine("Enter number:");
                                        num2 = Convert.ToInt32(Console.ReadLine());
                                        switch (num2)
                                        {
                                            case 0:
                                                break;

                                            case 1:
                                                {
                                                    Console.WriteLine("All single player games");
                                                    Console.WriteLine(" ");
                                                    var game = db.FindSinglePlayerGames();

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                break;
                                            case 2:
                                                {
                                                    Console.WriteLine("All multiplayer games");
                                                    Console.WriteLine(" ");
                                                    var game = db.FindMultiplayerGames();

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                        Console.WriteLine("");
                                                    }
                                                }
                                                break;
                                            case 3:
                                                {
                                                    Console.WriteLine("The maximum number of games sold");
                                                    Console.WriteLine(" ");
                                                    var game = db.FindGameWithMaxSales();

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                    }


                                                }
                                                break;
                                            case 4:
                                                {
                                                    Console.WriteLine("Minimum number of games sold");
                                                    Console.WriteLine(" ");
                                                    var game = db.FindGameWithMinSales();

                                                    if (game != null)
                                                    {
                                                        game.Print();
                                                    }

                                                }
                                                break;
                                            case 5:
                                                {
                                                    Console.WriteLine("Top 3 most popular games");
                                                    Console.WriteLine(" ");
                                                    var games5 = db.FindTop3PopularGames();

                                                    foreach (var game in games5)
                                                    {
                                                        game.Print();
                                                        Console.WriteLine(" ");
                                                    }

                                                }
                                                break;
                                            case 6:
                                                {
                                                }
                                                break;
                                        }
                                    } while (num2 != 0);
                                }
                                break;
                            case 3:
                                {

                                    int num3;
                                    do
                                    {
                                        Console.WriteLine("1 - Adding a new game. Before adding it is necessary");
                                        Console.WriteLine("check if such a game already exists.");
                                        Console.WriteLine("2 - Changing data in an existing game. The user can change any parameter of the game.");
                                        Console.WriteLine("3 - Delete the game. The search for the game to delete is carried out by");
                                        Console.WriteLine("by the name of the game and the studio. Before uninstalling the game, the app has");
                                        Console.WriteLine("ask the user if the game should be uninstalled.");

                                        Console.WriteLine("");
                                        Console.WriteLine("Enter number:");
                                        num3 = Convert.ToInt32(Console.ReadLine());
                                        switch (num3)
                                        {
                                            case 0:
                                                break;

                                            case 1:
                                                {
                                                    Console.WriteLine("name:");
                                                    string name = Console.ReadLine();
                                                    Console.WriteLine("studio:");
                                                    string studio = Console.ReadLine();
                                                    Console.WriteLine("style:");
                                                    string style = Console.ReadLine();
                                                    Console.WriteLine("releaseDate:");
                                                    DateTime releaseDate = DateTime.Parse(Console.ReadLine());
                                                    Console.WriteLine("multiplayer:");
                                                    bool multiplayer = bool.Parse(Console.ReadLine());
                                                    Console.WriteLine("sales:");
                                                    int sales = int.Parse(Console.ReadLine());

                                                    var game = db.GameInfo.Where(x => x.Name == name && x.Studio == studio).FirstOrDefault();

                                                    if (game == null)
                                                    {
                                                        game = new GameInfo(name, studio, style, releaseDate, multiplayer, sales);
                                                        db.GameInfo.Add(game);
                                                        db.SaveChanges();
                                                    }

                                                }
                                                break;
                                            case 2:
                                                {
                                                    Console.WriteLine("Enter the name of the game to search:");
                                                    string name = Console.ReadLine();
                                                    Console.WriteLine("Enter game studio to search:");
                                                    string studio = Console.ReadLine();

                                                    var game = db.GameInfo.Where(x => x.Name == name && x.Studio == studio).FirstOrDefault();

                                                    if (game != null)
                                                    {

                                                        Console.WriteLine("name:");
                                                        string newName = Console.ReadLine();
                                                        Console.WriteLine("studio:");
                                                        string newStudio = Console.ReadLine();
                                                        Console.WriteLine("style:");
                                                        string newStyle = Console.ReadLine();
                                                        Console.WriteLine("releaseDate:");
                                                        DateTime newReleaseDate = DateTime.Parse(Console.ReadLine());
                                                        Console.WriteLine("multiplayer:");
                                                        bool newMultiplayer = bool.Parse(Console.ReadLine());
                                                        Console.WriteLine("sales:");
                                                        int newSales = int.Parse(Console.ReadLine());

                                                        game.Name = newName;
                                                        game.Studio = newStudio;
                                                        game.Style = newStyle;
                                                        game.ReleaseDate = newReleaseDate;
                                                        game.Multiplayer = newMultiplayer;
                                                        game.Sales = newSales;

                                                        db.SaveChanges();
                                                    }

                                                }
                                                break;
                                            case 3:
                                                {
                                                    Console.WriteLine("Enter the name of the game to search:");
                                                    string name = Console.ReadLine();
                                                    Console.WriteLine("Enter game studio to search:");
                                                    string studio = Console.ReadLine();

                                                    var game = db.GameInfo.Where(x => x.Name == name && x.Studio == studio).FirstOrDefault();

                                                    if (game != null)
                                                    {
                                                        Console.WriteLine("Are you sure you want to remove game {0} from {1}  (yes)?", name, studio);
                                                        var response = Console.ReadLine();

                                                        if (response == "yes")
                                                        {
                                                            db.GameInfo.Remove(game);
                                                            db.SaveChanges();
                                                        }
                                                    }

                                                }
                                                break;
                                        }
                                    } while (num3 != 0);
                                }
                                break;

                        }

                    } while (num != 0);
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }
    }
}
