using System.Collections.Generic;
using System.Linq;
using TeachFramework;
using TeachFramework.MathProgramming;
using TeachFramework.MathTypes;

namespace GomorySecondMethodTeaching
{
    public class GomorySecondTeaching : AbstractDynamicModel
    {
        private const string DescriptionLabel = "Другий алгоритм Гоморі";

        private const string InitialProblemEnteringLabel = "Введіть задачу дискретного програмування:";

        private const string IsNotSuitableProblemLabel = "Дану задачу не можна розв’язати другим алгоритмом Гоморі";

        private const string WeakeneProblemLabel = "Послаблена задача:";
        private const string FollowingActionsLabel = "Наступні дії:";
        private const string NormalizeWeakenedLabel = "Привести послаблену задачу до виду, зручного для застосування симплекс-методу";
        private const string SolveWeakenedLabel = "Розв’язати послаблену задачу симплекс-методом";

        private const string WeakenedProblemResultLabel = "Розв’язок послабленої задачі:";
        private const string InitialProblemResultLabel = "Розв’язок вихідної задачі:";
        private const string IsEndLabel = "Чи є розв’язок даної задачі розв’язком вихідної?";
        private const string RightResulLabel = "Так.";
        private const string NeedCutoffLabel = "Ні. Необхідно додати відсічення.";

        private readonly GomorySecond _gomorySecond = new GomorySecond();
        private ProblemForGomory InitialProblem { get; set; }
        private LppForSimplexMethod _weakenedNormalizedProblem;
        private SimplexTable _currentSimplexTable;

        //---------------------------------------------------------------------------------
        public GomorySecondTeaching()
        {
            Description = DescriptionLabel;
            SetStartStep(InitialStep);
            IsEnd = false;
        }
        //=================================================================================

        private UiDescription InitialStep(UiDescription userData)
        {
            SetNextStep(InputProblemAnalyzingStep);

            return new UiDescription
                       {
                           {"Label", "lab1", "", InitialProblemEnteringLabel},
                           {"DppView", "DPP", "", null, true},
                       };
        }

        private UiDescription InputProblemAnalyzingStep(UiDescription userData)
        {
            if (!_gomorySecond.IsSuitable((DiscreteProgrammingProblem)userData["DPP"].Value))
                return IsNotSuitableMessagingStep(userData);

            InitialProblem = new ProblemForGomory((DiscreteProgrammingProblem)userData["DPP"].Value);

            return AfterInitActionChoosingStep();
        }

        private UiDescription IsNotSuitableMessagingStep(UiDescription userData)
        {
            IsEnd = true;
            SetNextStep(InitialStep);
            return new UiDescription
                       {
                           {"Label", "lab1", IsNotSuitableProblemLabel},
                           {"DppView", "DPP", "", userData["DPP"].Value, false}
                       };
        }

        private UiDescription AfterInitActionChoosingStep()
        {
            var nextAction = _gomorySecond.IsWeakenedProblemNormalized(InitialProblem)
                               ? SolveWeakenedLabel
                               : NormalizeWeakenedLabel;

            SetNextStep(nextAction == SolveWeakenedLabel ? (Step)FirstWeakenedProblemSolvingStep : FirstWeakenedProblemNormalizingStep);

            return new UiDescription
                       {
                           {"Label", "lab1", WeakeneProblemLabel},
                           {"LppView", "WP", InitialProblem},
                           {"Label", "lab2", FollowingActionsLabel},
                           {
                               "StepsContainer", "solveOrNormalize", SolveWeakenedLabel + "|" + NormalizeWeakenedLabel,
                               new StepVariants(nextAction.Split(new[] {'|'})), true, true
                               }
                       };
        }

        private UiDescription FirstWeakenedProblemNormalizingStep(UiDescription userData)
        {
            SetNextStep(FirstWeakenedProblemSolvingStep);

            _gomorySecond.SolveInitialWeekenedProblem(InitialProblem, out _weakenedNormalizedProblem, out _currentSimplexTable);

            return new UiDescription
                       {
                           {"Label", "lab1", "Вихідна послаблена задача:"},
                           {"LppView", "WP", InitialProblem},
                           {"Label", "lab2", "Приведена до виду, зручного до використання симплекс-методу:"},
                           {"LppView", "WNP", "", _weakenedNormalizedProblem, true, true} 
                       };
        }

        private UiDescription FirstWeakenedProblemSolvingStep(UiDescription userData)
        {
            SetNextStep(IsEndStep);

            var result = _gomorySecond.SolveInitialWeekenedProblem(InitialProblem, out _weakenedNormalizedProblem,
                                                                  out _currentSimplexTable);

            var tableSettings = new SimplexTableViewSettings
            {
                RowCount = _currentSimplexTable.RowsCount,
                Variables = new List<string>(_currentSimplexTable.Variables)
            };

            return new UiDescription
                       {
                           {"Label", "lab1", "Приведена до виду, зручного до використання симплекс-методу:"},
                           {"LppView", "WNP", _weakenedNormalizedProblem},
                           {"Label", "lab2", "Розв’яжіть послаблену задачу та введіть останню симплекс-таблицю і результат:"},
                           {"SimplexTableView", "table", "", _currentSimplexTable, true, true, tableSettings},
                           {"LppResultView", "result", "", result, true, true, _weakenedNormalizedProblem.TargetFunctionArguments}
                       };
        }

        private UiDescription IsEndStep(UiDescription userData)
        {
            var nextAction = _gomorySecond.IsEnd((LppResult)userData["result"].Value, InitialProblem)
                                 ? RightResulLabel
                                 : NeedCutoffLabel;

            SetNextStep(nextAction == RightResulLabel ? (Step)ResultEnteringStep : CutoffMakingStep);

            return new UiDescription
                       {
                           {"Label", "lab1", "Вихідна задача:"},
                           {"DppView", "initialProblem", InitialProblem.DiscreteProblem},
                           {"Label", "lab2", WeakenedProblemResultLabel},
                           {
                               "LppResultView", "result", "", userData["result"].Value, false, false,
                               _currentSimplexTable.Variables
                               },
                           {"Label", "lab3", IsEndLabel},
                           {
                               "StepsContainer", "isEnd", RightResulLabel + "|" + NeedCutoffLabel,
                               new StepVariants(nextAction.Split(new[] {'|'})), true, true
                               }
                       };
        }

        private UiDescription ResultEnteringStep(UiDescription userData)
        {
            var initialProblemResult = _gomorySecond.GetInitialProblemResult(_currentSimplexTable, _weakenedNormalizedProblem);

            SetNextStep(InitialStep);
            IsEnd = true;

            return new UiDescription
                       {
                           {"Label", "lab1", "Ф-ція цілі послабленої задаяі:"},
                           {"TargetFunctionBox", "tfWeakened", _weakenedNormalizedProblem.TargetFunction},
                           {"Label", "lab2", WeakenedProblemResultLabel},
                           {
                               "LppResultView", "weakenedProblemResult", "", userData["result"].Value, false, false,
                               _currentSimplexTable.Variables
                               },
                           {"Label", "lab3", "Ф-ція цілі вихідної задаяі:"},
                           {"TargetFunctionBox", "tfInitial", _weakenedNormalizedProblem.TargetFunction},
                           {"Label", "lab4", InitialProblemResultLabel},
                           {
                               "LppResultView", "initialProblemResult", "", initialProblemResult, true, true,
                               InitialProblem.TargetFunctionArguments
                               }
                       };
        }

        private UiDescription CutoffMakingStep(UiDescription userData)
        {
            SetNextStep(CutoffAddingStep);

            var creativeVariable = _gomorySecond.GetCreativeVariable(_currentSimplexTable, InitialProblem);

            var allVariablesString = string.Empty;
            var variableCount = _weakenedNormalizedProblem.TargetFunctionArguments.Count;
            for (var i = 0; i < _weakenedNormalizedProblem.TargetFunctionArguments.Count; i++)
                allVariablesString += _weakenedNormalizedProblem.TargetFunctionArguments.ElementAt(i) +
                                      (i == variableCount - 1 ? "" : "|");

            var sLabel = _gomorySecond.GetFreeBasisVariableLabel(_currentSimplexTable);
            var cutoff = _gomorySecond.MakeCutoff(_currentSimplexTable, creativeVariable, InitialProblem);

            return new UiDescription
                       {
                           {"Label", "lab1", "Остання симплекс-таблиця:"},
                           {"SimplexTableView", "table", _currentSimplexTable},
                           {"Label", "lab2", "Виберіть змінну, що відповідає твірному рядку:"},
                           {
                               "StepsContainer", "creativeVar", allVariablesString,
                               new StepVariants(creativeVariable.Split(new[] {'|'})), true, true
                               },
                           {"Label", "lab3", "Введіть відсічення(використайте змінну " + sLabel + "):"},
                           {"LimitationsArea", "limSystem", "", new List<Constraint>{CutoffConstraint(cutoff, sLabel)}, true, true}
                       };
        }

        private UiDescription CutoffAddingStep(UiDescription userData)
        {
            SetNextStep(NextWeakenedProblemSolvingStep);

            var sLabel = _gomorySecond.GetFreeBasisVariableLabel(_currentSimplexTable);
            var cutoff = _gomorySecond.MakeCutoff(_currentSimplexTable,
                                                 ((StepVariants)userData["creativeVar"].Value).Variants.ElementAt(0),
                                                 InitialProblem);
            var prevSimplexTable = _currentSimplexTable;
            _currentSimplexTable = _gomorySecond.AddCutoff(_currentSimplexTable, sLabel, cutoff);

            var tableSettings = new SimplexTableViewSettings
            {
                RowCount = _currentSimplexTable.RowsCount,
                Variables = new List<string>(_currentSimplexTable.Variables)
            };

            return new UiDescription
                       {
                           {"SimplexTableView", "prevTable", prevSimplexTable},
                           {"Label", "lab1", "Відсічення:"},
                           {"LimitationsArea", "limSystem", userData["limSystem"].Value},
                           {"Label", "lab2", "Нова симплекс-таблиця:"},
                           {"SimplexTableView", "table", "", _currentSimplexTable, true, true, tableSettings}
                       };
        }

        private UiDescription NextWeakenedProblemSolvingStep(UiDescription userData)
        {
            SetNextStep(IsEndStep);

            var prevTable = _currentSimplexTable;

            var result = _gomorySecond.SolveWeekenedProblemWithCutoff(_weakenedNormalizedProblem, _currentSimplexTable,
                                                                  out _currentSimplexTable);

            var tableSettings = new SimplexTableViewSettings
            {
                RowCount = _currentSimplexTable.RowsCount,
                Variables = new List<string>(_currentSimplexTable.Variables)
            };

            return new UiDescription
                       {
                           {"Label", "lab1", "Ф-ція цілі послабленої задачі:"},
                           {"TargetFunctionBox", "tf", _weakenedNormalizedProblem.TargetFunction},
                           {"Label", "lab2", "Перша симплекс-таблиця:"},
                           {"SimplexTableView", "firstTable", "", prevTable, false, false, tableSettings},
                           {"Label", "lab3", "Розв’яжіть послаблену задачу двоїстим симплекс-методом" +
                                             " та введіть останню симплекс-таблицю і результат:"},
                           {"SimplexTableView", "lastTable", "", _currentSimplexTable, true, true, tableSettings},
                           {"LppResultView", "result", "", result, true, true, _currentSimplexTable.Variables}
                       };
        }

        private Constraint CutoffConstraint(KeyValuePair<Fraction[], Fraction> cutoff, string cutoffVariable)
        {
            var leftSide = new List<Variable>();
            var rightSide = new Fraction(cutoff.Value);

            leftSide.Add(new Variable(new Fraction("1/1"), cutoffVariable));
            foreach (var variable in _currentSimplexTable.Variables)
            {
                var index = _currentSimplexTable.IndexOf(variable);
                leftSide.Add(new Variable(cutoff.Key[index], variable));
            }

            return new Constraint(leftSide, "=", rightSide);
        }
    }
}

