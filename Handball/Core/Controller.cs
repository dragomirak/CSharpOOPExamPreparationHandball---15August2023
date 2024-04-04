using Handball.Core.Contracts;
using Handball.Models;
using Handball.Models.Contracts;
using Handball.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handball.Core
{
    public class Controller : IController
    {
        private PlayerRepository players;
        private TeamRepository teams;

        public Controller()
        {
            players = new PlayerRepository();
            teams = new TeamRepository();
        }
        public string LeagueStandings()
        {
            var orderedTeams = teams.Models
                .OrderByDescending(t => t.PointsEarned)
                .ThenByDescending(t => t.OverallRating)
                .ThenBy(t => t.Name);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("***League Standings***");
            foreach (var team in orderedTeams)
            {
                sb.AppendLine(team.ToString());
            }

            return sb.ToString().TrimEnd();

        }

        public string NewContract(string playerName, string teamName)
        {
            if (players.ExistsModel(playerName) == false)
            {
                return $"Player with the name {playerName} does not exist in the {nameof(PlayerRepository)}.";
            }

            if (teams.ExistsModel(teamName) == false)
            {
                return $"Team with the name {teamName} does not exist in the {nameof(TeamRepository)}.";
            }

            var player = players.GetModel(playerName);
            var team = teams.GetModel(teamName);
            if (player.Team != null)
            {
                return $"Player {playerName} has already signed with {player.Team}.";
            }

            player.JoinTeam(teamName);
            team.SignContract(player);
            return $"Player {playerName} signed a contract with {teamName}.";
        }

        public string NewGame(string firstTeamName, string secondTeamName)
        {
            var firstTeam = teams.GetModel(firstTeamName);
            var secondTeam = teams.GetModel(secondTeamName);

            if (firstTeam.OverallRating > secondTeam.OverallRating)
            {
                firstTeam.Win();
                secondTeam.Lose();
                return $"Team {firstTeamName} wins the game over {secondTeamName}!";
            }
            else if (firstTeam.OverallRating < secondTeam.OverallRating)
            {
                secondTeam.Win();
                firstTeam.Lose();
                return $"Team {secondTeamName} wins the game over {firstTeamName}!";
            }

            firstTeam.Draw();
            secondTeam.Draw();
            return $"The game between {firstTeamName} and {secondTeamName} ends in a draw!";
        }

        public string NewPlayer(string typeName, string name)
        {
            if (typeName is not ("Goalkeeper" or "CenterBack" or "ForwardWing"))
            {
                return $"{typeName} is invalid position for the application.";
            }

            if (players.ExistsModel(name))
            {
                var player = players.GetModel(name);
                return $"{name} is already added to the {players.GetType().Name} as {player.GetType().Name}.";
            }

            IPlayer currentPlayer = null;
            if (typeName == "Goalkeeper")
            {
                currentPlayer = new Goalkeeper(name);
            }
            else if (typeName == "CenterBack")
            {
                currentPlayer = new CenterBack(name);
            }
            else if (typeName == "ForwardWing")
            {
                currentPlayer = new ForwardWing(name);
            }

            players.AddModel(currentPlayer);
            return $"{name} is filed for the handball league.";
        }

        public string NewTeam(string name)
        {
            if (teams.ExistsModel(name))
            {
                return $"{name} is already added to the {teams.GetType().Name}.";
            }

            var team = new Team(name);
            teams.AddModel(team);
            return $"{name} is successfully added to the {teams.GetType().Name}.";
        }

        public string PlayerStatistics(string teamName)
        {
            var team = teams.GetModel(teamName);
            var orderedTeam = team.Players
                .OrderByDescending(p => p.Rating)
                .ThenBy(p => p.Name);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"***{teamName}***");
            foreach ( var player in orderedTeam)
            {
                sb.AppendLine(player.ToString());
            }

            return sb.ToString().TrimEnd();
        }
    }
}
