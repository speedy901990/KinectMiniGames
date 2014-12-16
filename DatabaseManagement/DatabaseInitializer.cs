using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatabaseManagement.Models;
using Newtonsoft.Json;

namespace DatabaseManagement
{
    public class DatabaseInitializer
    {
        private readonly List<Game> _games = new List<Game>();
        private readonly List<GameParam> _gameParams = new List<GameParam>(); 
        private readonly List<GameResult> _gameResults = new List<GameResult>();
        private Player _player;

        public DatabaseInitializer()
        {
            using (var context = new GameModelContainer())
            {
                if (!context.Database.Exists())
                {
                    context.Database.Create();
                    GenerateDefaultPlayer();
                    GenerateGamesFromResource();
                    GenerateGameParamsFromResources();
                    GenerateGameResultsFromResources();
                    context.Players.Add(_player);
                    context.GameParams.AddRange(_gameParams);
                    context.GameResults1.AddRange(_gameResults);
                    context.Games.AddRange(_games);
                    context.SaveChanges();
                }
                context.Database.Initialize(false);
            }
        }

        private void GenerateDefaultPlayer()
        {
            _player = new Player {Name = "Jan", Surname = "Kowalski", Age = 10};
        }

        private void GenerateGamesFromResource()
        {
            var gameResourceSet = Configs.GameList.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry item in gameResourceSet)
            {
                var game = new Game { Name = item.Key as string};
                _games.Add(game);
            }
            Configs.GameList.ResourceManager.ReleaseAllResources();
        }

        private void GenerateGameParamsFromResources()
        {
            var gameParamsResourceSet = Configs.GameParamsList.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry item in gameParamsResourceSet)
            {
                var gameParamModel = JsonConvert.DeserializeObject<GameParamModel>(item.Value.ToString());
                var game = _games.FirstOrDefault(game1 => game1.Name == gameParamModel.Game);
                if (game != null)
                    _gameParams.Add(new GameParam {Name = gameParamModel.Name, Game = game});
            }
            Configs.GameParamsList.ResourceManager.ReleaseAllResources();
        }

        private void GenerateGameResultsFromResources()
        {
            var gameResultsResourceSet = Configs.GameResultsList.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry item in gameResultsResourceSet)
            {
                var gameResultModel = JsonConvert.DeserializeObject<GameResultModel>(item.Value.ToString());
                var game = _games.FirstOrDefault(game1 => game1.Name == gameResultModel.Game);
               if (game != null)
                    _gameResults.Add(new GameResult { Name = gameResultModel.Name, Game = game});
            }
            Configs.GameList.ResourceManager.ReleaseAllResources();
        }
    }
}