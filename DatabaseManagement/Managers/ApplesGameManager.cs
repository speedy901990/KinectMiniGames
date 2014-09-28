using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatabaseManagement.Params;

namespace DatabaseManagement.Managers
{
    public class ApplesGameManager
    {
        private Game _game;
        private Player _player;
        private readonly Dictionary<String,GameParams> _gameParams = new Dictionary<string,GameParams>();
        private readonly Dictionary<String,GameResults> _gameResults = new Dictionary<string,GameResults>();

        public ApplesGameManager()
        {

        }

        public ApplesGameManager(String playerName)
        {
            GetGame();
            GetPlayer(playerName);
        }

        public ApplesGameManager(Player player)
        {
            GetGame();
            GetPlayer(player);
        }

        public void SaveGameResult(ApplesGameParams apg)
        {
            using (var context = new GameModelContainer())
            {
                var date = DateTime.Now;

                var history = new History
                {
                    Game = _game,
                    Player = _player,
                    Date = date
                };

                var historyParams = new List<HistoryParams>
                {
                    new HistoryParams
                    {
                        GameParam = _gameParams["apples"],
                        Value = apg.Apples.ToString(CultureInfo.InvariantCulture)
                    },
                    new HistoryParams
                    {
                        GameParam = _gameParams["colors"],
                        Value = apg.Colors.ToString(CultureInfo.InvariantCulture)
                    },
                    new HistoryParams
                    {
                        GameParam = _gameParams["baskets"],
                        Value = apg.Baskets.ToString(CultureInfo.InvariantCulture)
                    }
                };
                foreach (var item in historyParams)
                {
                    history.HistoryParams.Add(item);
                }

                var historyResults = new List<HistoryResult>
                {
                    new HistoryResult
                    {
                        GameResult = _gameResults["correctTrials"],
                        Value = apg.CorrectTrials
                    },
                    new HistoryResult
                    {
                        GameResult = _gameResults["failures"],
                        Value = apg.Failures
                    },
                    new HistoryResult
                    {
                        GameResult = _gameResults["time"],
                        Value = apg.Time
                    }
                };
                foreach (var item in historyResults)
                {
                    history.HistoryResults.Add(item);
                }

                context.Histories.Add(history);
                context.SaveChanges();
            }
        }

        private void GetGame()
        {
            using (var context = new GameModelContainer())
            {
                var game = context.Games.FirstOrDefault(b => b.Name == "ApplesGame");
                _game = game;

                if (_game == null) 
                    return;
                foreach (var item in _game.GameParams)
                {
                    _gameParams.Add(item.Name, item);
                }
                foreach (var item in _game.GameResults)
                {
                    _gameResults.Add(item.Name, item);
                }
            }
        }

        private void GetPlayer(String playerName)
        {
            using (var context = new GameModelContainer())
            {
                var user = context.Players.FirstOrDefault(b => b.Name == playerName);
                _player = user;
            }
        }

        private void GetPlayer(Player player)
        {
            _player = player;
        }
    }
}