# ProjectEulerSolver

Solutions to [Project Euler](https://projecteuler.net/) problems 1–79 in C# (.NET 8), with a small
reusable number-theory toolkit, a console runner, and a test suite that checks every answer.

This started as a weekend project many years ago. It has since been cleaned up so that every problem
is a small class, the shared maths lives in one place, and the whole set solves in a couple of
seconds.

## History

The original code, visible in the git history before the 2026 rewrite, looks odd at first glance:
every problem file carried its own copy of the prime sieve, GCD, digit helpers and so on, plus
file-based logging. That was deliberate. The solutions were also submitted to HackerRank's Project
Euler+ contest, whose editor accepts a single pasted source file and is a poor place to debug. Keeping
each problem fully self-contained meant it could be worked on in Visual Studio or Rider and pasted
across unchanged, with no risk of introducing errors while hand-merging helpers into one file.
Duplication was the price of that workflow. Now that HackerRank is no longer the target, the shared
code lives in `Tools/` and each problem is just the part that is specific to it.

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

Requires the .NET 8 SDK (`global.json` pins the 8.0 feature band; any 8.0.1xx or later 8.0 SDK works).

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

Every puzzle input is embedded in the library from `src/ProjectEulerSolver/Resources/` (one file per
problem that needs one), so nothing has to be downloaded to run or test the solutions.

Problem 59 uses the cipher text that was inlined in the original code, stored as
`0059_cipher_original.txt`. Project Euler replaced that file in 2019, so the recorded answer (107359)
differs from the one the site now accepts (129448); the current `0059_cipher.txt` from the site is a
different puzzle input and should not be dropped in under the old name.

## Notes

- Project Euler asks that solutions beyond problem 100 are not published; everything here is well
  within that range.
- Problem statements are not reproduced; each class links to the problem via `Problem.Url`.
- Problem text and data files are © Project Euler, licensed under
  [CC BY-NC-SA 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0/).

## License

[MIT](LICENSE)
