# ProjectEulerSolver

Solutions to [Project Euler](https://projecteuler.net/) problems 1–79 in C# (.NET 8), with a small
reusable number-theory toolkit, a console runner, and a test suite that checks every answer.

This started as a weekend project many years ago. It has since been cleaned up so that every problem
is a small, self-contained class, the shared maths lives in one place, and the whole set solves in a
couple of seconds.

## Layout

```
src/ProjectEulerSolver/          class library: one class per problem plus shared tools
  Problems/Problem001.cs ...     each problem: Number, Title, Solve()
  Tools/                         Primes, Digits, NumberTheory, Figurate, Combinatorics,
                                 ContinuedFractions, NumberWords, Resources
  Resources/                     puzzle data files embedded in the assembly
src/ProjectEulerSolver.Cli/      console runner
tests/ProjectEulerSolver.Tests/  xUnit tests: accepted answers for every problem, plus unit tests for the tools
```

## Running

Solve a problem (or several):

```bash
dotnet run --project src/ProjectEulerSolver.Cli -- 42
```

Solve everything with timings:

```bash
dotnet run --project src/ProjectEulerSolver.Cli -c Release -- --all
```

Run the tests:

```bash
dotnet test
```

## Adding a problem

1. Create `src/ProjectEulerSolver/Problems/ProblemNNN.cs` deriving from `Problem` and implement
   `Number`, `Title` and `Solve()`. The catalog discovers it by reflection.
2. Put any data file from projecteuler.net in `src/ProjectEulerSolver/Resources/` (it is embedded
   automatically) and read it with `Resources.ReadLines(...)`.
3. Add the accepted answer to `AnswerTests.Expected`.

## Data files

Every puzzle input (names, words, the poker hands, the keylog, the cipher text, both triangles, the
20×20 grid and the 100 fifty-digit numbers) is embedded in the library from
`src/ProjectEulerSolver/Resources/`, so nothing needs to be downloaded to run or test the solutions.

Problem 59 uses the original `cipher.txt`; Project Euler replaced that file in 2019, so the recorded
answer (107359) differs from the one the site now accepts (129448).

## Notes

- Project Euler asks that solutions beyond problem 100 are not published; everything here is well
  within that range.
- Problem statements are not reproduced; each class links to the problem via `Problem.Url`.
- Problem text and data files are © Project Euler, licensed under
  [CC BY-NC-SA 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0/).

## License

[MIT](LICENSE)
