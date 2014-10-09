using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatabaseManagement.Params;

namespace DatabaseManagement.Managers
{
    public class DrawingGameManager
    {
        private Game _game;
        private Player _player;
        private readonly Dictionary<String, GameParam> _gameParams = new Dictionary<string, GameParam>();
        private readonly Dictionary<String, GameResult> _gameResults = new Dictionary<string, GameResult>();

        public DrawingGameManager()
        {

        }


        public DrawingGameManager(Player player)
        {
            GetGame();
            _player = player;
        }
        public void SaveGameResult(DrawingGameParams bgp)
        {
            using (var context = new GameModelContainer())
            {
                var date = DateTime.Now;
                var historyParams = new List<HistoryParam>
                {
                    new HistoryParam
                    {
                        GameParam = context.GameParams.Find(_gameParams["Level"].Id),
                        Value = bgp.Level.ToString(CultureInfo.InvariantCulture)
                    }
                };

                var historyResults = new List<HistoryResult>
                {
                   
                   new HistoryResult
                    {
                        GameResult = context.GameResults1.Find(_gameResults["Time"].Id),
                        Value = bgp.TimeOfGame
                    },
                    new HistoryResult
                    {
                        GameResult = context.GameResults1.Find(_gameResults["Time Out"].Id),
                        Value = bgp.TimeOutOfField
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
                var game = context.Games.FirstOrDefault(b => b.Name == "DrawingGame");
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
