using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEulerSolver.Problems
{
    public class Problem18 : BaseProblem, IProblem
    {
        public Problem18()
        {
            Number = 18;
            Prompt = "By starting at the top of the triangle below and moving to adjacent numbers on the row below, the maximum total from top to bottom is 23. "

                +    "3"
                +   "7 4"
                +  "2 4 6"
                + "8 5 9 3"

                + "That is, 3 + 7 + 4 + 9 = 23."

                + "Find the maximum total from top to bottom of the triangle below:"

                +               "75;"
                +              "95 64;"
                +             "17 47 82;"
                +            "18 35 87 10;"
                +           "20 04 82 47 65;"
                +          "19 01 23 75 03 34;"
                +         "88 02 77 73 07 63 67;"
                +        "99 65 04 28 06 16 70 92;"
                +       "41 41 26 56 83 40 80 70 33;"
                +      "41 48 72 33 47 32 37 16 94 29;"
                +     "53 71 44 65 25 43 91 52 97 51 14;"
                +    "70 11 33 28 77 73 17 78 39 68 17 57;"
                +   "91 71 52 38 17 14 91 43 58 50 27 29 48;"
                +  "63 66 04 68 89 53 67 30 73 16 69 87 40 31;"
                + "04 62 98 27 23 09 70 98 73 93 38 53 60 04 23;";
            Notes = "As there are only 16384 routes, it is possible to solve this problem by trying every route. However, Problem 67, is the same challenge with a triangle containing one-hundred rows; it cannot be solved by brute force, and requires a clever method!";
            Input =             "75;"
                +              "95 64;"
                +             "17 47 82;"
                +            "18 35 87 10;"
                +           "20 04 82 47 65;"
                +          "19 01 23 75 03 34;"
                +         "88 02 77 73 07 63 67;"
                +        "99 65 04 28 06 16 70 92;"
                +       "41 41 26 56 83 40 80 70 33;"
                +      "41 48 72 33 47 32 37 16 94 29;"
                +     "53 71 44 65 25 43 91 52 97 51 14;"
                +    "70 11 33 28 77 73 17 78 39 68 17 57;"
                +   "91 71 52 38 17 14 91 43 58 50 27 29 48;"
                +  "63 66 04 68 89 53 67 30 73 16 69 87 40 31;"
                + "04 62 98 27 23 09 70 98 73 93 38 53 60 04 23";

        }
        public override void Solve()
        {
            List<string> rows = Input.Split(';').ToList();
            List<List<int>> values = new  List<List<int>>();
            
            foreach(string item in rows)
            {
                values.Add(item.Split(' ').Select(a => int.Parse(a)).ToList());
            }
            for(int j = values.Count - 1; j > 0; j--)
            {
                for(int i = 0; i < values[j].Count - 1; i++)
                {
                    values[j-1][i] += Math.Max(values[j].ToList()[i], values[j].ToList()[i+1]);
                }
            }

            Output = values[0][0].ToString();


        }
    }
}