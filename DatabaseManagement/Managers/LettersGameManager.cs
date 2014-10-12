using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatabaseManagement.Params;

namespace DatabaseManagement.Managers
{
    public class LettersGameManager
    {
        private readonly Player _player;

        public LettersGameManager()
        {

        }

        public LettersGameManager(Player player)
        {
            _player = player;
        }

        public void SaveGameResult(LettersGameParams lgp)
        {
            using (var context = new GameModelContainer())
            {
                var game = context.Games.FirstOrDefault(b => b.Name == "LettersGame");

                if (game == null)
                    return;

                var date = DateTime.Now;
                var historyParams = new List<HistoryParam>
                {
                    new HistoryParam
                    {
                        GameParam = game.GameParams.FirstOrDefault(param => param.Name == "Level"),
                        Value = lgp.Level.ToString(CultureInfo.InvariantCulture)
                    }
                };
                
                var historyResults = new List<HistoryResult>
                {
                    new HistoryResult
                    {
                        GameResult = game.GameResults.FirstOrDefault(param => param.Name == "Correct Trials"),
                        Value = lgp.CorrectTrials
                    },
                    new HistoryResult
                    {
                        GameResult = game.GameResults.FirstOrDefault(param => param.Name == "Failures"),
                        Value = lgp.Failures
                    },
                    new HistoryResult
                    {
                        GameResult = game.GameResults.FirstOrDefault(param => param.Name == "Time"),
                        Value = lgp.Time
                    }
                };

                var history = new History
                {
                    Game = game,
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
    }
}
