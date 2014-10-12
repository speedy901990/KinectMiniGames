using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatabaseManagement.Params;

namespace DatabaseManagement.Managers
{
    public class DrawingGameManager
    {
        
        private Player _player;
        private readonly Dictionary<String, GameParam> _gameParams = new Dictionary<string, GameParam>();
        private readonly Dictionary<String, GameResult> _gameResults = new Dictionary<string, GameResult>();

        public DrawingGameManager()
        {

        }


        public DrawingGameManager(Player player)
        {
           
            _player = player;
        }
        public void SaveGameResult(DrawingGameParams bgp)
        {
            using (var context = new GameModelContainer())
            {
                var date = DateTime.Now;

                var game = context.Games.FirstOrDefault(b => b.Name == "DrawingGame");


                if (game == null)
                    return;

                var historyParams = new List<HistoryParam>
                {
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
                        GameResult = game.GameResults.FirstOrDefault(result => result.Name == "Time"),
                        Value = bgp.TimeOfGame
                    },
                    new HistoryResult
                    {
                        GameResult = game.GameResults.FirstOrDefault(result => result.Name == "Time Out"),
                        Value = bgp.TimeOutOfField
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
