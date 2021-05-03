using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEulerSolver.Problems
{
    public class Problem19 : BaseProblem, IProblem
    {
        public Problem19()
        {
            Number = 19;
            Prompt = "You are given the following information, but you may prefer to do some research for yourself." +
                        "1 Jan 1900 was a Monday." +
                        "Thirty days has September," +
                        "April, June and November." +
                        "All the rest have thirty-one," +
                        "Saving February alone," +
                        "Which has twenty-eight, rain or shine." +
                        "And on leap years, twenty-nine." +
                        "A leap year occurs on any year evenly divisible by 4, but not on a century unless it is divisible by 400." +
                        "How many Sundays fell on the first of the month during the twentieth century (1 Jan 1901 to 31 Dec 2000)?";
            Notes = "";

        }
        public override void Solve()
        {
            int result = 0;
            for(int year = 1901; year < 2001; year++)
            {
                for(int month = 1; month < 13; month++)
                {
                    result += (Convert.ToDateTime(month.ToString() + "/1/" + year.ToString()).DayOfWeek == DayOfWeek.Sunday) ? 1 : 0;
                }
            }
            Output = result.ToString();
        }
    }
}