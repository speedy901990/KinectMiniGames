using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManagement.Params;

namespace DatabaseManagement.Managers
{
    public class LettersGameManager
    {
        private Game _game;
        private readonly Player _player;
        private readonly Dictionary<String, GameParams> _gameParams = new Dictionary<string, GameParams>();
        private readonly Dictionary<String, GameResults> _gameResults = new Dictionary<string, GameResults>();


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
                        GameParam = _gameParams["lettersCount"],
                        Value = lgp.LettersCount.ToString(CultureInfo.InvariantCulture)
                    },
                    new HistoryParams
                    {
                        GameParam = _gameParams["level"],
                        Value = lgp.Level.ToString(CultureInfo.InvariantCulture)
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
                        Value = lgp.CorrectTrials
                    },
                    new HistoryResult
                    {
                        GameResult = _gameResults["failures"],
                        Value = lgp.Failures
                    },
                    new HistoryResult
                    {
                        GameResult = _gameResults["time"],
                        Value = lgp.Time
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
