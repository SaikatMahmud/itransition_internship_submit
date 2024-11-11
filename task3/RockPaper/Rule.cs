using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaper
{
    public static class Rule
    {
        public static string GetResultString(int a, int b, int n)
        {
            int p = n / 2;
            int result = ((a - b + p + n) % n - p);
            var sign = result == 0 ? 0 : result / Math.Abs(result);
            if (sign == 0) return "Draw";
            else if (sign == 1) return "Win";
            else return "Lose";
        }
    }
}
