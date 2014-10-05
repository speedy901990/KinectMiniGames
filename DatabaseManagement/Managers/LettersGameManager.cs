using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatabaseManagement.Params;

namespace DatabaseManagement.Managers
{
    public class LettersGameManager
    {
        private Game _game;
        private readonly Player _player;
        private readonly Dictionary<String, GameParam> _gameParams = new Dictionary<string, GameParam>();
        private readonly Dictionary<String, GameResult> _gameResults = new Dictionary<string, GameResult>();


        public LettersGameManager()
        {

        }

        public LettersGameManager(Player player)
        {
            _player = player;
            GetGame();
        }

        public void SaveGameResult(LettersGameParams lgp)
        {
            using (var context = new GameModelContainer())
            {
                var date = DateTime.Now;
                var historyParams = new List<HistoryParam>
                {
                    new HistoryParam
                    {
                        GameParam = _gameParams["Level"],
                        Value = lgp.LettersCount.ToString(CultureInfo.InvariantCulture)
                    }
                };

                var historyResults = new List<HistoryResult>
                {
                    new HistoryResult
                    {
                        GameResult = _gameResults["Correct Trials"],
                        Value = lgp.CorrectTrials
                    },
                    new HistoryResult
                    {
                        GameResult = _gameResults["Failures"],
                        Value = lgp.Failures
                    },
                    new HistoryResult
                    {
                        GameResult = _gameResults["Time"],
                        Value = lgp.Time
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
                var game = context.Games.FirstOrDefault(b => b.Name == "LettersGame");
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
