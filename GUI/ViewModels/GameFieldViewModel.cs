using Caliburn.Micro;
using GUI.Models;
using Ninject;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace GUI.ViewModels
{
    public class GameFieldViewModel : ViewModelBase
    {
        private readonly bool _againstBot;

        private int _currentPlayer;
        private IObservableCollection<GameFieldBoxModel> _gameFields;
        private bool _isEnabled;

        public IObservableCollection<GameFieldBoxModel> GameFields
        {
            get => _gameFields;
            set
            {
                _gameFields = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                NotifyOfPropertyChange();
            }
        }

        public GameFieldViewModel(bool againstBot)
        {
            LoadField();
            _againstBot = againstBot;
        }

        private void LoadField()
        {
            GameFields = new BindableCollection<GameFieldBoxModel>();

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    var gameField = _kernel.Get<GameFieldBoxModel>();
                    gameField.Column = x;
                    gameField.Row = y;
                    gameField.Owner = -1;

                    GameFields.Add(gameField);
                }
            }

            _currentPlayer = 0;
            IsEnabled = true;
        }

        public void GameFieldClicked(GameFieldBoxModel gameField)
        {
            if (gameField.HasStone)
                return;

            var column = gameField.Column;
            var row = gameField.Row;

            if (GameFields.Where(x => x.Column == column).All(x => x.HasStone))
                return;

            IsEnabled = false;

            GameFieldBoxModel targetGameField = gameField;

            while (!targetGameField?.HasStone ?? false)
            {
                row++;
                targetGameField = GameFields.FirstOrDefault(x => x.Column == column && x.Row == row);
            }

            row--;
            targetGameField = GameFields.FirstOrDefault(x => x.Column == column && x.Row == row);

            targetGameField.StoneColor = GetPlayerBrush();
            targetGameField.Owner = _currentPlayer;
            targetGameField.HasStone = true;

            if (GetVerticalCount(targetGameField) >= 3)
            {
                ShowWinner();
                return;
            }
            else if (GetHorizontalCount(targetGameField) >= 3)
            {
                ShowWinner();
                return;
            }
            else if (GetDiagonal1Count(targetGameField) >= 3)
            {
                ShowWinner();
                return;
            }
            else if (GetDiagonal2Count(targetGameField) >= 3)
            {
                ShowWinner();
                return;
            }

            ChangePlayer();

            if (_againstBot && _currentPlayer == 1)
            {
                Thread thread = new Thread(() => RunBot());
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            else
            {
                IsEnabled = true;
            }
        }

        private void RunBot()
        {
            Thread.Sleep(1000);

            Random random = new Random((int)DateTime.UtcNow.Ticks);
            var rnd = random.Next(0, 6);

            if (GameFields.Where(x => x.Column == rnd).Any(x => !x.HasStone))
            {
                GameFieldClicked(GameFields.First(x => x.Column == rnd && x.Row == 0));
                IsEnabled = true;
            }
            else
            {
                RunBot();
            }
        }

        private void ShowWinner()
        {
            MessageBox.Show($"Player {_currentPlayer} wins!");
            LoadField();
        }

        private int GetDiagonal2Count(GameFieldBoxModel gameField)
        {
            if (gameField == null)
                return 0;

            return GetDiagonal21Count(gameField) + GetDiagonal22Count(gameField);
        }

        private int GetDiagonal21Count(GameFieldBoxModel gameField)
        {
            if (gameField == null)
                return 0;

            var row1 = gameField.Row + 1;
            var column1 = gameField.Column - 1;
            var rightField = GameFields.FirstOrDefault(x => x.Column == column1 && x.Row == row1);

            if (rightField == null)
                return 0;

            if (rightField.HasStone && rightField.Owner == _currentPlayer)
                return GetDiagonal21Count(rightField) + 1;

            return 0;
        }

        private int GetDiagonal22Count(GameFieldBoxModel gameField)
        {
            if (gameField == null)
                return 0;

            var row2 = gameField.Row - 1;
            var column2 = gameField.Column + 1;
            var leftField = GameFields.FirstOrDefault(x => x.Column == column2 && x.Row == row2);

            if (leftField == null)
                return 0;

            if (leftField.HasStone && leftField.Owner == _currentPlayer)
                return GetDiagonal22Count(leftField) + 1;

            return 0;
        }

        private int GetDiagonal1Count(GameFieldBoxModel gameField)
        {
            if (gameField == null)
                return 0;

            return GetDiagonal11Count(gameField) + GetDiagonal12Count(gameField);
        }

        private int GetDiagonal11Count(GameFieldBoxModel gameField)
        {
            if (gameField == null)
                return 0;

            var row1 = gameField.Row + 1;
            var column1 = gameField.Column + 1;
            var rightField = GameFields.FirstOrDefault(x => x.Column == column1 && x.Row == row1);

            if (rightField == null)
                return 0;

            if (rightField.HasStone && rightField.Owner == _currentPlayer)
                return GetDiagonal11Count(rightField) + 1;

            return 0;
        }

        private int GetDiagonal12Count(GameFieldBoxModel gameField)
        {
            if (gameField == null)
                return 0;

            var row2 = gameField.Row - 1;
            var column2 = gameField.Column - 1;
            var leftField = GameFields.FirstOrDefault(x => x.Column == column2 && x.Row == row2);

            if (leftField == null)
                return 0;

            if (leftField.HasStone && leftField.Owner == _currentPlayer)
                return GetDiagonal12Count(leftField) + 1;

            return 0;
        }

        private int GetHorizontalCount(GameFieldBoxModel gameField)
        {
            if (gameField == null)
                return 0;

            return GetHorizontal1Count(gameField) + GetHorizontal2Count(gameField);
        }

        private int GetHorizontal1Count(GameFieldBoxModel gameField)
        {
            if (gameField == null)
                return 0;

            var row = gameField.Row;
            var column1 = gameField.Column + 1;
            var rightField = GameFields.FirstOrDefault(x => x.Column == column1 && x.Row == row);
            
            if (rightField == null)
                return 0;

            if (rightField.HasStone && rightField.Owner == _currentPlayer)
                return GetHorizontal1Count(rightField) + 1;

            return 0;
        }

        private int GetHorizontal2Count(GameFieldBoxModel gameField)
        {
            if (gameField == null)
                return 0;

            var row = gameField.Row;
            var column2 = gameField.Column - 1;
            var leftField = GameFields.FirstOrDefault(x => x.Column == column2 && x.Row == row);

            if (leftField == null)
                return 0;

            if (leftField.HasStone && leftField.Owner == _currentPlayer)
                return GetHorizontal2Count(leftField) + 1;

            return 0;
        }

        private int GetVerticalCount(GameFieldBoxModel gameField)
        {
            if (gameField == null)
                return 0;

            return GetVertical1Count(gameField) + GetVertical2Count(gameField);
        }

        private int GetVertical1Count(GameFieldBoxModel gameField)
        {
            if (gameField == null)
                return 0;

            var column = gameField.Column;
            var row1 = gameField.Row + 1;
            var rightField = GameFields.FirstOrDefault(x => x.Column == column && x.Row == row1);

            if (rightField == null)
                return 0;

            if (rightField.HasStone && rightField.Owner == _currentPlayer)
                return GetVertical1Count(rightField) + 1;

            return 0;
        }

        private int GetVertical2Count(GameFieldBoxModel gameField)
        {
            if (gameField == null)
                return 0;

            var column = gameField.Column;
            var row2 = gameField.Row - 1;
            var leftField = GameFields.FirstOrDefault(x => x.Column == column && x.Row == row2);
            
            if (leftField == null)
                return 0;

            if (leftField.HasStone && leftField.Owner == _currentPlayer)
                return GetVertical2Count(leftField) + 1;

            return 0;
        }

        private Brush GetPlayerBrush()
        {
            if (_currentPlayer == 0)
                return Brushes.Red;

            return Brushes.Yellow;
        }

        private void ChangePlayer()
        {
            if (_currentPlayer == 0)
            {
                _currentPlayer = 1;
            }
            else
            {
                _currentPlayer = 0;
            }
        }
    }
}
