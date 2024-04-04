using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handball.Models
{
    public class CenterBack : Player
    {
        private const double InitialRating = 4;
        public CenterBack(string name)
            : base(name, InitialRating)
        {
        }

        public override void DecreaseRating()
        {
            Rating--;
            if (Rating < 1)
            {
                Rating = 1;
            }
        }

        public override void IncreaseRating()
        {
            Rating++;
            if (Rating > 10)
            {
                Rating = 10;
            }
        }
    }
}

