using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatabaseManagement.Params;

namespace DatabaseManagement.Managers
{
    public class BubblesGameManager
    {
        private Game _game;
        private readonly Player _player;
        private readonly Dictionary<String,GameParam> _gameParams = new Dictionary<string,GameParam>();
        private readonly Dictionary<String,GameResult> _gameResults = new Dictionary<string,GameResult>();
        
        public BubblesGameManager(Player player)
        {
            GetGame();
            _player = player;
        }

        public void SaveGameResult(BubblesGameParams bgp)
        {
            using (var context = new GameModelContainer())
            {
                var date = DateTime.Now;

                
                //TODO poprawic bgp i value w kazdym z history params
                var historyParams = new List<HistoryParam>
                {
                    new HistoryParam
                    {
                        GameParam = _gameParams["Appearance Frequency"],
                        Value = bgp.Level.ToString(CultureInfo.InvariantCulture)
                    },
                    new HistoryParam
                    {
                        GameParam = _gameParams["Bubbles"],
                        Value = bgp.Level.ToString(CultureInfo.InvariantCulture)
                    },
                    new HistoryParam
                    {
                        GameParam = _gameParams["Bubbles Size"],
                        Value = bgp.Level.ToString(CultureInfo.InvariantCulture)
                    },
                    new HistoryParam
                    {
                        GameParam = _gameParams["Fall Speed"],
                        Value = bgp.Level.ToString(CultureInfo.InvariantCulture)
                    }
                };

                var historyResults = new List<HistoryResult>
                {
                    new HistoryResult
                    {
                        GameResult = _gameResults["Success"],
                        Value = bgp.Success
                    },
                    new HistoryResult
                    {
                        GameResult = _gameResults["Time"],
                        Value = bgp.Time
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
                var game = context.Games.FirstOrDefault(b => b.Name == "BubblesGame");
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
    }
}
