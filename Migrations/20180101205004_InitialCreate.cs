using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EulerSolver.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FactorialResults",
                columns: table => new
                {
                    FactorialResultId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Count1 = table.Column<int>(nullable: false),
                    Count2 = table.Column<int>(nullable: false),
                    Count3 = table.Column<int>(nullable: false),
                    Count4 = table.Column<int>(nullable: false),
                    Count5 = table.Column<int>(nullable: false),
                    Count6 = table.Column<int>(nullable: false),
                    Count7 = table.Column<int>(nullable: false),
                    Count8 = table.Column<int>(nullable: false),
                    Count9 = table.Column<int>(nullable: false),
                    FactorialSum = table.Column<string>(nullable: true),
                    FactorialSumCount1 = table.Column<int>(nullable: false),
                    FactorialSumCount2 = table.Column<int>(nullable: false),
                    FactorialSumCount3 = table.Column<int>(nullable: false),
                    FactorialSumCount4 = table.Column<int>(nullable: false),
                    FactorialSumCount5 = table.Column<int>(nullable: false),
                    FactorialSumCount6 = table.Column<int>(nullable: false),
                    FactorialSumCount7 = table.Column<int>(nullable: false),
                    FactorialSumCount8 = table.Column<int>(nullable: false),
                    FactorialSumCount9 = table.Column<int>(nullable: false),
                    IsMatch = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactorialResults", x => x.FactorialResultId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FactorialResults");
        }
    }
}
