using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatabaseManagement.Params;

namespace DatabaseManagement.Managers
{
    public class BubblesGameManager
    {
        private readonly Player _player;

        public BubblesGameManager(Player player)
        {
            _player = player;
        }

        public void SaveGameResult(BubblesGameParams bgp)
        {
            using (var context = new GameModelContainer())
            {
                var date = DateTime.Now;

                var game = context.Games.FirstOrDefault(b => b.Name == "BubblesGame");

                if (game == null)
                    return;
                
                var historyParams = new List<HistoryParam>
                {
                    new HistoryParam
                    {
                        GameParam = game.GameParams.FirstOrDefault(param => param.Name == "Appearance Frequency"),
                        Value = bgp.AppearanceFrequency.ToString(CultureInfo.InvariantCulture)
                    },
                    new HistoryParam
                    {
                        GameParam = game.GameParams.FirstOrDefault(param => param.Name == "Bubbles"),
                        Value = bgp.Bubbles.ToString(CultureInfo.InvariantCulture)
                    },
                    new HistoryParam
                    {
                        GameParam = game.GameParams.FirstOrDefault(param => param.Name == "Bubbles Size"),
                        Value = bgp.BubblesSize.ToString(CultureInfo.InvariantCulture)
                    },
                    new HistoryParam
                    {
                        GameParam = game.GameParams.FirstOrDefault(param => param.Name == "Fall Speed"),
                        Value = bgp.FallSpeed.ToString(CultureInfo.InvariantCulture)
                    },
                    new HistoryParam
                    {
                        GameParam = game.GameParams.FirstOrDefault(param => param.Name == "Level"),
                        Value = bgp.Level.ToString(CultureInfo.InvariantCulture)
                    }
                };

                var historyResults = new List<HistoryResult>
                {
                    new HistoryResult
                    {
                        GameResult = game.GameResults.FirstOrDefault(result => result.Name == "Success"),
                        Value = bgp.Success
                    },
                    new HistoryResult
                    {
                        GameResult = game.GameResults.FirstOrDefault(result => result.Name == "Time"),
                        Value = bgp.Time
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
