using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ProjectEulerSolver.Model
{
    public class ProjectEulerSolverDataContext : DbContext
    {
        public ProjectEulerSolverDataContext() : base()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          //  string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProjectEulerSolver.Factorials3;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
           // optionsBuilder.UseSqlServer(connString);
        }
        public virtual DbSet<FactorialResult> FactorialResults { get; set; }

    }
    public class FactorialResult
    {
        public int FactorialResultId { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int Count3 { get; set; }
        public int Count4 { get; set; }
        public int Count5 { get; set; }
        public int Count6 { get; set; }
        public int Count7 { get; set; }
        public int Count8 { get; set; }
        public int Count9 { get; set; }
        public string FactorialSum { get; set; }
        public int FactorialSumCount1 { get; set; }
        public int FactorialSumCount2 { get; set; }
        public int FactorialSumCount3 { get; set; }
        public int FactorialSumCount4 { get; set; }
        public int FactorialSumCount5 { get; set; }
        public int FactorialSumCount6 { get; set; }
        public int FactorialSumCount7 { get; set; }
        public int FactorialSumCount8 { get; set; }
        public int FactorialSumCount9 { get; set; }
        public bool IsMatch { get; set; }
    }
}
