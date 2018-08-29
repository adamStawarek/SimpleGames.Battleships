﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using SimpleGame.Annotations;

namespace SimpleGame
{
    public abstract class Player
    {
        public static bool IsGameOver;              
        public Players PlayerType { get; set; }
        public ObservableCollection<Ship> Ships { get; }
        public List<int> AlreadyAttackedFields { get; }
        public event EventHandler Lost;
        public ImageSource Flag { get; set; }

        protected Player(Players playerType)
        {
            PlayerType = playerType;
            AlreadyAttackedFields=new List<int>();
            Ships=new ObservableCollection<Ship>();
           
            foreach (var ship in Ships)
            {
                ship.Destroyed += CheckIfGameOver;
            }
        }

        public void AddShips(IEnumerable<Ship> ships)
        {            
            ships.ToList().ForEach(s=>Ships.Add(s));
        }

        private void CheckIfGameOver(object sender, EventArgs e)
        {
            if (!Ships.All(s => s.IsDestroyed)) return;
            MessageBox.Show($"{this.PlayerType} has lost!!!");
            IsGameOver = true;
            OnLost();
        }      

        public void OnLost()
        {
            var handler = Lost;
            handler?.Invoke(this, EventArgs.Empty);
        }

        [CanBeNull]
        public Ship GetShipByField(int i)
        {
            foreach (var ship in Ships)
            {
                if (ship.Fields.Contains(i))
                    return ship;
            }

            return null;
        }

        public bool Attack(Player enemy, int field)
        {            
            var ship = enemy.GetShipByField(field);
            ship?.UnderAttack(field);
            AlreadyAttackedFields.Add(field);
            return ship != null;
        }
    }
}