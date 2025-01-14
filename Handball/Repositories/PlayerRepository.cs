﻿using Handball.Models.Contracts;
using Handball.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handball.Repositories
{
    public class PlayerRepository : IRepository<IPlayer>
    {
        private List<IPlayer> players;
        public PlayerRepository()
        {
            players = new List<IPlayer>();
        }
        public IReadOnlyCollection<IPlayer> Models => players.AsReadOnly();

        public void AddModel(IPlayer model)
        {
            players.Add(model);
        }

        public bool ExistsModel(string name)
        {
            return players.Any(p => p.Name == name);
        }

        public IPlayer GetModel(string name)
        {
            return players.FirstOrDefault(p => p.Name == name);
        }

        public bool RemoveModel(string name)
        {
            var player = players.FirstOrDefault(p => p.Name == name);
            if (player != null)
            {
                players.Remove(player);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
