using System;
using DatabaseManagement.Managers;
using DatabaseManagement.Params;

namespace DrawingGame
{
    public class ResultSaver
    {
        private MainWindow _mainWindow;

        public ResultSaver(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public DatabaseManagement.Managers.DrawingGameManager DrawingGameManager
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void SaveResults()
        {
            int level = 0;

            if (_mainWindow.Configuration.HandsState == 1 && _mainWindow.Configuration.Difficulty == 1)
                level = 1;
            if (_mainWindow.Configuration.HandsState == 2 && _mainWindow.Configuration.Difficulty == 1)
                level = 2;
            if (_mainWindow.Configuration.HandsState == 1 && _mainWindow.Configuration.Difficulty == 2)
                level = 3;
            if (_mainWindow.Configuration.HandsState == 2 && _mainWindow.Configuration.Difficulty == 2)
                level = 4;

            DrawingGameManager manager = new DrawingGameManager(_mainWindow.Configuration.Player);
            DrawingGameParams gameParams = new DrawingGameParams
            {
                TimeOfGame =(int)Math.Round((double) _mainWindow.StopwatchOfGame.ElapsedMilliseconds/1000),
                TimeOutOfField = (int)Math.Round((double) _mainWindow.StopwatchOfOutOfField.ElapsedMilliseconds / 1000),
                Level = level
            };
            manager.SaveGameResult(gameParams);
        }
    }
}