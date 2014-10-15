using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatabaseManagement.Params;

namespace DatabaseManagement.Managers
{
    public class TrainOfWordsManager
    {
        private readonly Player _player;

        public TrainOfWordsManager(Player player)
        {
            _player = player;
        }

        public void SaveGameResult(TrainOfWordsParams tParams)
        {
            using (var context = new GameModelContainer())
            {
                var game = context.Games.FirstOrDefault(game1 => game1.Name == "TrainOfWords");

                if (game == null)
                    return;

                var date = DateTime.Now;

                var historyParams = new List<HistoryParam>
                {
                    new HistoryParam
                    {
                        GameParam = game.GameParams.FirstOrDefault(param => param.Name == "Level"),
                        Value = tParams.Level.ToString(CultureInfo.InvariantCulture)
                    }
                };

                var historyResults = new List<HistoryResult>
                {
                    new HistoryResult
                    {
                        GameResult = game.GameResults.FirstOrDefault(param => param.Name == "Correct Trials"),
                        Value = tParams.CorrectTrials
                    },
                    new HistoryResult
                    {
                        GameResult = game.GameResults.FirstOrDefault(param => param.Name == "Failures"),
                        Value = tParams.Failures
                    },
                    new HistoryResult
                    {
                        GameResult = game.GameResults.FirstOrDefault(param => param.Name == "Time"),
                        Value = tParams.Time
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

                if (player == null) 
                    return;
                player.Histories.Add(history);
                context.SaveChanges();
            }
        }
    }
}