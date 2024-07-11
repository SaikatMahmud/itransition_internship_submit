using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaper
{
    public static class Error
    {
        public const string NoMovesFound = "Enter your moves in sequence.";
        public const string InvalidMovesLength = "Please enter odd number of moves greater than 3....";
        public const string DuplicateMoves = "You entered duplicate moves! Try again....";
        public const string InvalidUserInput = "! ! ! ! Enter your selection from the list....";
    }
}
