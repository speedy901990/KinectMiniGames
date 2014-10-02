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
        private readonly Dictionary<String, GameParams> _gameParams = new Dictionary<string, GameParams>();
        private readonly Dictionary<String, GameResults> _gameResults = new Dictionary<string, GameResults>();

        public DrawingGameManager()
        {

        }

        public DrawingGameManager(String playerName)
        {
            GetGame();
            GetPlayer(playerName);
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

                var history = new History
                {
                    Game = _game,
                    Player = _player,
                    Date = date
                };

                history.HistoryParams.Add(new HistoryParams
                {
                    GameParam = _gameParams["level"],
                    Value = bgp.Level.ToString(CultureInfo.InvariantCulture)
                });


                var historyResults = new List<HistoryResult>
                {
                    new HistoryResult
                    {
                        GameResult = _gameResults["Time of Game"],
                        Value = bgp.TimeOfGame
                    },
                    new HistoryResult
                    {
                        GameResult = _gameResults["Time out of field"],
                        Value = bgp.TimeOutOfField
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

        private void GetPlayer(String playerName)
        {
            using (var context = new GameModelContainer())
            {
                var user = context.Players.FirstOrDefault(b => b.Name == playerName);
                _player = user;
            }
        }
    }
}
