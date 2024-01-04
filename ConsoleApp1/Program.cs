using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

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
        public string StudioCountry { get; set; }
        public string StudioCity { get; set; }
        public GameInfo(string name, string studio, string style, DateTime releaseDate, bool multiplayer, int sales, string studioCountry, string studioCity)
        {
            Name = name;
            Studio = studio;
            Style = style;
            ReleaseDate = releaseDate;
            Multiplayer = multiplayer;
            Sales = sales;
            StudioCountry = studioCountry;
            StudioCity = studioCity;
        }
        public void Print()
        {
            Console.WriteLine($"Game name: {Name}");
            Console.WriteLine($"Game studio: {Studio}");
            Console.WriteLine($"Game style: {Style}");
            Console.WriteLine($"Game realese date: {ReleaseDate}");

        }
    }

    public class GameDbContext : DbContext
    {
        public string ConnectionString = @"Data Source=WIN-0I7PB3TGH35\SQLEXPRESS;Initial Catalog=Game;Integrated Security=True;Encrypt=False";

        public GameDbContext()
        {
        }

        public DbSet<GameInfo> GameInfo { get; set; }

        public void DisplaySingleMultiplayerGamesCount()
        {
            var count = GameInfo.Count(g => !g.Multiplayer);
            Console.WriteLine($"Number of single-player games: {count}");
        }

        public void DisplayMultiplayerGamesCount()
        {
            var count = GameInfo.Count(g => g.Multiplayer);
            Console.WriteLine($"Number of multiplayer games: {count}");
        }

        public void DisplayGameWithMaxSalesByStyle(string style)
        {
            var game = GameInfo.Where(g => g.Style == style).OrderByDescending(g => g.Sales).FirstOrDefault();
            if (game != null)
            {
                Console.WriteLine($"Game with the highest sales in the style '{style}':");
                game.Print();
            }
            else
            {
                Console.WriteLine($"No games found with the style '{style}'.");
            }
        }

        public void DisplayTop5GamesByMaxSalesByStyle(string style)
        {
            var top5Games = GameInfo.Where(g => g.Style == style).OrderByDescending(g => g.Sales).Take(5).ToList();
            Console.WriteLine($"Top 5 games with the highest sales in the style '{style}':");
            foreach (var game in top5Games)
            {
                game.Print();
                Console.WriteLine(" ");
            }
        }

        public void DisplayTop5GamesByMinSalesByStyle(string style)
        {
            var top5Games = GameInfo.Where(g => g.Style == style).OrderBy(g => g.Sales).Take(5).ToList();
            Console.WriteLine($"Top 5 games with the lowest sales in the style '{style}':");
            foreach (var game in top5Games)
            {
                game.Print();
                Console.WriteLine(" ");
            }
        }

        public void DisplayFullGameInfo()
        {
            var games = GameInfo.ToList();
            Console.WriteLine("Full information about games:");
            foreach (var game in games)
            {
                game.Print();
                Console.WriteLine(" ");
            }
        }

        public void DisplayStudiosAndStylesWithMoreGames()
        {
            var studiosAndStyles = GameInfo.GroupBy(g => new { g.Studio, g.Style })
                                           .Where(group => group.Count() > 1)
                                           .Select(group => new
                                           {
                                               Studio = group.Key.Studio,
                                               Style = group.Key.Style,
                                               GamesCount = group.Count()
                                           }).ToList();

            Console.WriteLine("Name of each studio and game style with a higher number of games created in this style:");
            foreach (var item in studiosAndStyles)
            {
                Console.WriteLine($"Studio: {item.Studio}, Style: {item.Style}, Number of games: {item.GamesCount}");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
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

            GameInfo game1 = new GameInfo("Genshin Impact", "miHoYo", "RPG", new DateTime(2023, 11, 08), true, 1000, "Studio 1", "Country1");
            GameInfo game2 = new GameInfo("Subway Surfers", "Imangi Studios", "Endless Runner", new DateTime(2015, 01, 16), true, 900, "Studio2", "Country2");
            GameInfo game3 = new GameInfo("The Legend of Zelda: Tears of the Kingdom", "Nintendo Entertainment Planning & Development", "Puzzle, Action-Adventure, Fighting, Shooter", new DateTime(2023, 03, 12), true, 800, "Studio3", "Country3");
            GameInfo game4 = new GameInfo("Marvel's Spider-Man", "Insomniac Games", "Action-Adventure, Fighting, Platformer", new DateTime(2023, 10, 20), false, 700, "Studio4", "Country4");
            GameInfo game5 = new GameInfo("FC 24", "Electronic Arts, EA Sports, EA Romania, EA Vancouver", "Sports Simulator, Simulator, Gaming Simulation", new DateTime(2022, 09, 27), false, 600, "Studio5", "Country5");

            try
            {
                using (var db = new GameDbContext())
                {
                    db.Add(game1);
                    db.Add(game2);
                    db.Add(game3);
                    db.Add(game4);
                    db.Add(game5);

                    db.SaveChanges();

                    var games = db.GameInfo.ToList();
                    foreach (var game in games)
                    {
                        game.Print();
                        Console.WriteLine(" ");
                    }
                    db.DisplaySingleMultiplayerGamesCount();
                    db.DisplayMultiplayerGamesCount();
                    db.DisplayGameWithMaxSalesByStyle("RPG");
                    db.DisplayTop5GamesByMaxSalesByStyle("RPG");
                    db.DisplayTop5GamesByMinSalesByStyle("RPG");
                    db.DisplayFullGameInfo();
                    db.DisplayStudiosAndStylesWithMoreGames();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
