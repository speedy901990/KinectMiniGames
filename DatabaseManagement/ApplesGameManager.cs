using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagement
{
    public class ApplesGameManager
    {
        private Game game;
        private Player player;
        private List<GameParam> gameParams;
        private List<GameResult> gameResults;

        public ApplesGameManager()
        {

        }

        public ApplesGameManager(String playerName)
        {
            this.GetGame();
            this.GetPlayer(playerName);
            this.GetGameParams();
            this.GetGameResults();
        }

        public void SaveGameResult()
        {
            using (var context = new GameModelContext())
            {
                var history = new History
                {
                    Game = this.game,
                    Player = this.player
                };

                foreach (var item in gameParams)
                {

                }
                // Create and save a new Blog 
                //Console.Write("Enter a name for a new Blog: "); 
                //var name = Console.ReadLine(); 

                //var blog = new Blog { Name = name }; 
                //db.Blogs.Add(blog); 
                //db.SaveChanges(); 
            }
        }

        private void GetGame()
        {
            using (var context = new GameModelContext())
            {
                var game = context.Games
                    .Where(b => b.name == "ApplesGame").FirstOrDefault();
                this.game = game as Game;
            }
        }

        private void GetPlayer(String playerName)
        {
            using (var context = new GameModelContext())
            {
                var user = context.Players
                    .Where(b => b.name == playerName).FirstOrDefault();
                this.player = user as Player;
            }
        }

        private void GetGameParams()
        {
            using (var context = new GameModelContext())
            {
                var gameParams = from b in context.GameParams
                                 where b.Game == this.game
                                 select b;
                foreach (var item in gameParams)
                {
                    this.gameParams.Add(item as GameParam);
                }
            }
        }

        private void GetGameResults()
        {
            using (var context = new GameModelContext())
            {
                var gameResults = from b in context.GameResults
                                  where b.Game == this.game
                                  select b;
                //this.gameResults = gameResults.ToList();
                foreach (var item in gameResults)
                {
                    this.gameResults.Add(item as GameResult);
                }
            }
        }
    }
}