using System;
using System.Collections.Generic;
using System.Linq;
using TeachFramework;
using TeachFramework.MathProgramming;

namespace DualSimplexMethodTeaching
{
    public class DualSimplexMethodTeaching : AbstractDynamicModel
    {
        private const string DescriptionLabel = "Двоїстий симплекс-метод";

        private const string SolveLabel = "Почати розв’язок задачі двоїстим симплекс-методом";
        private const string NormilizeLabel = "Привести задачу до виду, зручного для застосування двоїстого симплекс-методу";

        private readonly Dictionary<NormalizeActionTypes, string> _normalizeActionsLabels = new Dictionary<NormalizeActionTypes, string>
        {
            {NormalizeActionTypes.TargetChanging,           "Змінити напрям ф-ції цілі."},
            {NormalizeActionTypes.MoreThanChanging,         "Змінити обмеження виду \">=\" на \"<=\"."},
            {NormalizeActionTypes.LessThanChanging,         "Змінити обмеження виду \"<=\" на \"=\"додавши додаткові змінні."},
            {NormalizeActionTypes.BasisVariablesAdding,     "Додати штучні змінні у обмеження без базисних змінних."},
            {NormalizeActionTypes.WithoutZeroLimChanging,   "Не на всі змінні накладені умови невід’ємності. " +
                                                            "Ввести заміни типу x = x' - x''"},
            {NormalizeActionTypes.NormalazingEnding,        "Задача має вигляд, зручний для застосування двоїстого симплекс-методу. " +
                                                            "Почати розв’язок"}
        };

        private const string ResultEnteringLabel = "Ввести результат.";
        private const string SolvingElementChoosingLabel = "План припускає покращення. Вибрати розв’язуючий елемент.";

        //------------------------------------------------------------------------------------
        private LppForSimplexMethod CurrentProblem { get; set; }
        private SimplexTable CurrentSimplexTable { get; set; }
        private readonly DualSimplexMethod _dualSimplex = new DualSimplexMethod();

        private enum NormalizeActionTypes
        {
            TargetChanging,
            MoreThanChanging,
            LessThanChanging,
            BasisVariablesAdding,
            WithoutZeroLimChanging,
            NormalazingEnding
        }

        private Dictionary<NormalizeActionTypes, Step> _normalizeActionsSteps;
        //-------------------------------------------------------------------------------------
        public DualSimplexMethodTeaching()
        {
            Description = DescriptionLabel;
            SetStartStep(InitialStep);
            IsEnd = false;

            SetNormalizeActionsSteps();
        }
        //=====================================================================================
        #region [ Befor normalizing choice steps ]

        private UiDescription InitialStep(UiDescription userData)
        {
            SetNextStep(SolveOrNormalizeChoiceStep);

            return new UiDescription
                       {
                           {"Label", "lab1", "", "Введіть задачу лінійного програмування:"},
                           {"LppView", "LPP", "", null, true},
                       };
        }

        private UiDescription SolveOrNormalizeChoiceStep(UiDescription userData)
        {
            CurrentProblem = new LppForSimplexMethod(((LinearProgrammingProblem)userData["LPP"].Value));

            //CurrentProblem = _simplex.Normalize(CurrentProblem);
            //CurrentSimplexTable = _simplex.MakeFirstSimplexTable(CurrentProblem);
            //CurrentSimplexTable = _simplex.Solve(CurrentSimplexTable);
            //return NormalizedProblemResultEnteringStep(userData);

            var nextStep = GetSolveOrNormalizeRightVariant();
            SetNextStep(nextStep == SolveLabel ? (Step)MakeFirstSimplexTableStep : NormalizeActionChoiceStep);
            return new UiDescription
                       {
                           {"LppView", "LPP", "", CurrentProblem, false, false},
                           {"Label", "lab", null, "Виберіть наступний крок:", false, false},
                           {"StepsContainer", "NextSteps", GetSolveOrNormalizeVariants(), new StepVariants(new[] {nextStep}), true, true}
                       };
        }

        #endregion

        #region [ Normalizing steps ]

        private UiDescription NormalizeActionChoiceStep(UiDescription userData)
        {
            var nextStep = GetNextStepOfNormalizing();
            SetNextStep(_normalizeActionsSteps[nextStep]);

            return new UiDescription
                       {
                           {"LppView", "LPP", "", CurrentProblem},
                           {"Label", "lab", null, "Виберіть наступний крок:"},
                           {"StepsContainer", "NextSteps", GetAllNormalizeActionsLabels(), 
                                      new StepVariants(_normalizeActionsLabels[nextStep].Split(new []{'|'})), true, true}
                       };
        }

        private UiDescription TargetChangingStep(UiDescription userData)
        {
            var value = CurrentProblem;
            CurrentProblem = _dualSimplex.TargetToMinimize(CurrentProblem);

            SetNextStep(NormalizeActionChoiceStep);

            return new UiDescription
                       {
                           {"LppView", "LPP", "", value},
                           {"Label", "lab", null, "Enter changed target function:"},
                           {"TargetFunctionBox", "TargetFunc", "", CurrentProblem.TargetFunction, true, true}
                       };
        }

        private UiDescription MoreThanConstraintsChangingStep(UiDescription userData)
        {
            var prevProblem = CurrentProblem;
            CurrentProblem = _dualSimplex.ConstraintsToLessThanForm(CurrentProblem);

            SetNextStep(NormalizeActionChoiceStep);

            return new UiDescription
                       {
                           {"LppView", "LPP", "", prevProblem},
                           {"Label", "lab", null, "Введіть змінену задачу:"},
                           {"LppView", "ChangedLPP", "", CurrentProblem, true, true}
                       };
        }

        private UiDescription LessThanConstraintsChangingStep(UiDescription userData)
        {
            var prevProblem = CurrentProblem;
            CurrentProblem = _dualSimplex.ChangeLessThanConstraints(CurrentProblem);

            SetNextStep(NormalizeActionChoiceStep);

            return new UiDescription
                       {
                           {"LppView", "LPP", "", prevProblem},
                           {"Label", "lab", null, "Введіть змінену задачу:"},
                           {"LppView", "ChangedLPP", "", CurrentProblem, true, true}
                       };
        }

        private UiDescription BasisVariablesAddingStep(UiDescription userData)
        {
            var prevProblem = CurrentProblem;
            CurrentProblem = _dualSimplex.ChangeEqualConstraints(CurrentProblem);

            SetNextStep(NormalizeActionChoiceStep);

            return new UiDescription
                       {
                           {"LppView", "LPP", "", prevProblem},
                           {"Label", "lab", null, "Введіть змінену задачу:"},
                           {"LppView", "ChangedLPP", "", CurrentProblem, true, true}
                       };
        }

        private UiDescription VariablesWithoutZeroConstraintsChangingStep(UiDescription userData)
        {
            var prevProblem = CurrentProblem;
            CurrentProblem = _dualSimplex.ReplaceVariablesWithoutZeroConstraints(CurrentProblem);

            SetNextStep(NormalizeActionChoiceStep);

            return new UiDescription
                       {
                           {"LppView", "LPP", "", prevProblem},
                           {"Label", "lab", null, "Enter changed LPP:"},
                           {"LppView", "ChangedLPP", "", CurrentProblem, true, true}
                       };
        }

        #endregion

        #region [ Solving steps ]

        private UiDescription MakeFirstSimplexTableStep(UiDescription userData)
        {
            CurrentSimplexTable = _dualSimplex.MakeFirstSimplexTable(CurrentProblem);
            CurrentSimplexTable = _dualSimplex.CalculateRatings(CurrentSimplexTable);

            SetNextStep(IsEndStep);

            var tableSettings = new SimplexTableViewSettings
            {
                RowCount = CurrentSimplexTable.RowsCount,
                Variables = new List<string>(CurrentSimplexTable.Variables)
            };

            return new UiDescription
                       {
                           {"LppView", "normalizedLpp", CurrentProblem},
                           {"Label", "lab1", "Заповніть першу симплекс-таблицю та підрахуйте оцінки."},
                           {"SimplexTableView", "table", "", CurrentSimplexTable, true, true, tableSettings}
                       };
        }

        private UiDescription IsEndStep(UiDescription userData)
        {
            StepVariants nextStepRightVariant;

            if (_dualSimplex.IsEnd(CurrentSimplexTable))
            {
                nextStepRightVariant = new StepVariants(new[] { ResultEnteringLabel });
                SetNextStep(NormalizedProblemResultEnteringStep);
            }
            else
            {
                nextStepRightVariant = new StepVariants(new[] { SolvingElementChoosingLabel });
                SetNextStep(SolvingElementChoosingStep);
            }

            return new UiDescription
                       {
                           {"SimplexTableView", "table", "", CurrentSimplexTable},
                           {"Label", "lab1", "Подальші дії?"},
                           {
                               "StepsContainer", "variants", ResultEnteringLabel + "|" + SolvingElementChoosingLabel,
                               nextStepRightVariant, true, true
                               }
                       };
        }

        private UiDescription SolvingElementChoosingStep(UiDescription userData)
        {
            SetNextStep(MakeNextSimplexTableStep);

            var solvingElement = _dualSimplex.GetSolvingElement(CurrentSimplexTable);

            var varsString = string.Empty;
            var variableCount = CurrentSimplexTable.Variables.Count;
            for (var i = 0; i < variableCount; i++)
                varsString += CurrentSimplexTable.Variables.ElementAt(i) + (i == variableCount - 1 ? "" : "|");

            var cellVariable = new StepVariants(new[] { CurrentSimplexTable.GetVariable(solvingElement.CellIndex) });
            var rowVariable = new StepVariants(new[] { CurrentSimplexTable.GetBasisVariableLabel(solvingElement.RowIndex) });

            return new UiDescription
                       {
                           {"SimplexTableView", "table", "", CurrentSimplexTable},
                           {"Label", "lab2", "Виберіть змінну, яка виводиться з базису:"},
                           {"StepsContainer", "rowVar", varsString, rowVariable, true, true},
                           {"Label", "lab1", "Виберіть змінну, яка вводиться в базис:"},
                           {"StepsContainer", "cellVar", varsString, cellVariable, true, true}
                       };
        }

        private UiDescription MakeNextSimplexTableStep(UiDescription userData)
        {
            SetNextStep(IsEndStep);

            var prevTable = CurrentSimplexTable;

            var solvingElement = _dualSimplex.GetSolvingElement(CurrentSimplexTable);
            var cellVariable = new StepVariants(new[] { CurrentSimplexTable.GetVariable(solvingElement.CellIndex) });
            var rowVariable = new StepVariants(new[] { CurrentSimplexTable.GetBasisVariableLabel(solvingElement.RowIndex) });

            CurrentSimplexTable = _dualSimplex.NextSimplexTable(CurrentSimplexTable, solvingElement);

            var tableSettings = new SimplexTableViewSettings
            {
                RowCount = CurrentSimplexTable.RowsCount,
                Variables = new List<string>(CurrentSimplexTable.Variables)
            };

            return new UiDescription
                       {
                           {"TargetFunctionBox", "targetFunction", CurrentProblem.TargetFunction},
                           {"SimplexTableView", "prevTable", prevTable},
                           {
                               "Label", "lab1", 
                               "Нова симплекс-таблиця(" + rowVariable.Variants.ElementAt(0) + " виводиться з базису, " + 
                                    cellVariable.Variants.ElementAt(0) + " - вводиться)."},
                           {"SimplexTableView", "table", "", CurrentSimplexTable, true, true, tableSettings}
                       };
        }

        private UiDescription NormalizedProblemResultEnteringStep(UiDescription userData)
        {
            SetNextStep(InitialProblemResultEnteringStep);

            var result = _dualSimplex.GetNormalizedProblemResult(CurrentSimplexTable, CurrentProblem);

            return new UiDescription
                       {
                           {"Label", "lab1", "Ф-ція цілі допоміжної задачі:"},
                           {"TargetFunctionBox", "targetFunction", CurrentProblem.TargetFunction},
                           {"SimplexTableView", "table", CurrentSimplexTable},
                           {"Label", "lab2", "Введіть результат розв’язку допоміжної задачі:"},
                           {"LppResultView", "result", "", result, true, true, CurrentProblem.TargetFunctionArguments}
                       };
        }

        private UiDescription InitialProblemResultEnteringStep(UiDescription userData)
        {
            SetNextStep(InitialStep);
            IsEnd = true;

            var prevResult = _dualSimplex.GetNormalizedProblemResult(CurrentSimplexTable, CurrentProblem);
            var result = _dualSimplex.GetInitialProblemResult(CurrentSimplexTable, CurrentProblem);

            var data = new UiDescription
                           {
                               {"Label", "lab1", "Ф-ція цілі вихідної задачі:"},
                               {"TargetFunctionBox", "targetFunction", CurrentProblem.InitialProblem.TargetFunction},
                               {"Label", "lab2", "Результат розв’язку допоміжної задачі:"},
                               {"LppResultView", "prevResult", prevResult},
                               {"Label", "lab3", "Заміни введені у допоміжну задачу:"}
                           };
            foreach (var replacement in CurrentProblem.Replacements)
                data.Add("Label", replacement.Key + "Lab", replacement.Key + " = " +
                    replacement.Value.Key + " - " + replacement.Value.Value);

            data.Add("Label", "lab4", "Введіть результат розв’язку вихідної задачі:");
            data.Add("LppResultView", "result", "", result, true, true,
                     CurrentProblem.InitialProblem.TargetFunction.Arguments);
            return data;
        }

        #endregion

        #region [ For choosing next step of normalizing ]

        private static string GetSolveOrNormalizeVariants()
        {
            return SolveLabel + "|" + NormilizeLabel;
        }
        private string GetSolveOrNormalizeRightVariant()
        {
            return _dualSimplex.IsNormalized(CurrentProblem)
                ? SolveLabel
                : NormilizeLabel;
        }

        private void SetNormalizeActionsSteps()
        {
            _normalizeActionsSteps = new Dictionary<NormalizeActionTypes, Step>
             {
                 {NormalizeActionTypes.TargetChanging,          TargetChangingStep},
                 {NormalizeActionTypes.MoreThanChanging,        MoreThanConstraintsChangingStep},
                 {NormalizeActionTypes.LessThanChanging,        LessThanConstraintsChangingStep},
                 {NormalizeActionTypes.BasisVariablesAdding,    BasisVariablesAddingStep},
                 {NormalizeActionTypes.WithoutZeroLimChanging,  VariablesWithoutZeroConstraintsChangingStep},
                 {NormalizeActionTypes.NormalazingEnding,       MakeFirstSimplexTableStep}
             };
        }
        private NormalizeActionTypes GetNextStepOfNormalizing()
        {
            //1.
            if (!_dualSimplex.IsTargetFunctionMinimized(CurrentProblem))
                return NormalizeActionTypes.TargetChanging;
            //2.
            if (_dualSimplex.ContainsMoreThanConstraints(CurrentProblem))
                return NormalizeActionTypes.MoreThanChanging;
            //3.
            if (_dualSimplex.DoProblemHaveLessThanConstraint(CurrentProblem))
                return NormalizeActionTypes.LessThanChanging;
            //4.
            if (!_dualSimplex.DoAllConstraintsHaveBasisVariable(CurrentProblem))
                return NormalizeActionTypes.BasisVariablesAdding;
            //5.
            return !_dualSimplex.DoAllVariablesHaveZeroConstraint(CurrentProblem)
                ? NormalizeActionTypes.WithoutZeroLimChanging
                : NormalizeActionTypes.NormalazingEnding;
        }
        private string GetAllNormalizeActionsLabels()
        {
            var actions = new List<string>(_normalizeActionsLabels.Values);

            var r = new Random();
            var index = r.Next(actions.Count - 1);
            var steps = actions.ElementAt(index);
            actions.RemoveAt(index);
            while (actions.Count != 0)
            {
                index = r.Next(actions.Count - 1);
                steps += "|" + actions.ElementAt(index);
                actions.RemoveAt(index);
            }
            return steps;
        }

        #endregion
    }
}
