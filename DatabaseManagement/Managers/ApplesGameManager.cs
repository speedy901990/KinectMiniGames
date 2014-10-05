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
        private readonly Dictionary<String,GameParam> _gameParams = new Dictionary<string,GameParam>();
        private readonly Dictionary<String,GameResult> _gameResults = new Dictionary<string,GameResult>();

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

                var historyParams = new List<HistoryParam>
                {
                    new HistoryParam
                    {
                        GameParam = _gameParams["Apples"],
                        Value = apg.Apples.ToString(CultureInfo.InvariantCulture)
                    },
                    new HistoryParam
                    {
                        GameParam = _gameParams["Colors"],
                        Value = apg.Colors.ToString(CultureInfo.InvariantCulture)
                    },
                    new HistoryParam
                    {
                        GameParam = _gameParams["Baskets"],
                        Value = apg.Baskets.ToString(CultureInfo.InvariantCulture)
                    }
                };

                var historyResults = new List<HistoryResult>
                {
                    new HistoryResult
                    {
                        GameResult = _gameResults["Correct Trials"],
                        Value = apg.CorrectTrials
                    },
                    new HistoryResult
                    {
                        GameResult = _gameResults["Failures"],
                        Value = apg.Failures
                    },
                    new HistoryResult
                    {
                        GameResult = _gameResults["Time"],
                        Value = apg.Time
                    }
                };

                var history = new History
                {
                    Game = _game,
                    Date = date,
                    HistoryParams = historyParams,
                    HistoryResults = historyResults
                };
                var player = context.Players.FirstOrDefault(player1 => player1.Id == _player.Id);
                if (player != null)
                    player.Histories.Add(history);
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