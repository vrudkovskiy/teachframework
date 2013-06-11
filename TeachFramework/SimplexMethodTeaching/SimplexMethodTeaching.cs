using System;
using System.Collections.Generic;
using System.Linq;

using TeachFramework;
using TeachFramework.MathProgramming;

namespace SimplexMethodTeaching
{
    public class SimplexMethodTeaching : AbstractDynamicModel
    {
        private LppForSimplexMethod CurrentProblem { get; set; }
        private SimplexTable CurrentSimplexTable { get; set; }
        private Dictionary<NormalizeActionTypes, string> _normalizeActionsLabels;
        private Dictionary<NormalizeActionTypes, Step> _normalizeActionsSteps;
        private readonly SimplexMethod _simplex = new SimplexMethod();
        private enum NormalizeActionTypes
        {
            TargetChanging,
            FreeCoefficientsChanging,
            MoreThanZeroChanging,
            FreeCoefOrMoreThanZeroChanging,
            LessThanChanging,
            MoreThanAndEqualChanging,
            MoreThanOnlyChanging,
            EqualOnlyChanging,
            WithoutZeroLimChanging,
            NormalazingEnding
        }
        private const string SolveLabel = "Почати розв’язок задачі симплекс-методом";
        private const string NormilizeLabel = "Привести задачу до виду, зручного для застосування симплекс-методу";
        private static string SolveOrNormalizeVariants
        {
            get { return SolveLabel + "|" + NormilizeLabel; }
        }

        private Dictionary<SolveActionTypes, string> _solveActionsLabels;
        private Dictionary<SolveActionTypes, Step> _solveActionsSteps;

        private enum SolveActionTypes
        {
            ResultEntering,
            SolvingElementChoosing
        }

        private const string DescriptionLabel = "Симплекс-метод";
        //------------------------------------------------------------------------------------
        public SimplexMethodTeaching()
        {
            Description = DescriptionLabel;
            SetStartStep(InitialStep);
            IsEnd = false;
            SetNormalizeActionsLabels();
            SetNormalizeActionsSteps();
            SetSolveActionsLabels();
            SetSolveActionsSteps();
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

            var nextStep = SolveOrNormalizing();
            SetNextStep(nextStep == SolveLabel ? (Step)MakeFirstSimplexTableStep : NormalizeActionChoiceStep);
            return new UiDescription
                       {
                           {"LppView", "LPP", "", CurrentProblem, false, false},
                           {"Label", "lab", null, "Виберіть наступний крок:", false, false},
                           {"StepsContainer", "NextSteps", SolveOrNormalizeVariants, new StepVariants(new[] {nextStep}), true, true}
                       };
        }

        #endregion

        #region [ Normalizing steps ]

        private UiDescription NormalizeActionChoiceStep(UiDescription userData)
        {
            var nextStep = NextStepOfNormalizing();
            SetNextStepOfNormalizing(nextStep);

            return new UiDescription
                       {
                           {"LppView", "LPP", "", CurrentProblem},
                           {"Label", "lab", null, "Виберіть наступний крок:"},
                           {"StepsContainer", "NextSteps", GetAllNormalizeActions(), 
                                      new StepVariants(GetPosibleNormalizeActions(nextStep).Split(new[] {'|'})), true, true}
                       };
        }

        private UiDescription TargetChanging(UiDescription userData)
        {
            var value = CurrentProblem;
            CurrentProblem = _simplex.TargetToMinimize(CurrentProblem);

            SetNextStep(NormalizeActionChoiceStep);

            return new UiDescription
                       {
                           {"LppView", "LPP", "", value},
                           {"Label", "lab", null, "Enter changed target function:"},
                           {"TargetFunctionBox", "TargetFunc", "", CurrentProblem.TargetFunction, true, true}
                       };
        }

        private UiDescription BeforLessThanChoice(UiDescription userData)
        {
            foreach (var label in _normalizeActionsLabels)
                if (label.Value == ((StepVariants)userData["NextSteps"].Value).Variants.ElementAt(0))
                    return _normalizeActionsSteps[label.Key](userData);
            return null;
        }

        private UiDescription FreeCoeffChanging(UiDescription userData)
        {
            var value = CurrentProblem;
            CurrentProblem = _simplex.MakeFreeCoefficientsNonNegative(CurrentProblem);

            SetNextStep(NormalizeActionChoiceStep);

            return new UiDescription
                       {
                           {"LppView", "LPP", "", value},
                           {"Label", "lab", null, "Enter changed limitations system:"},
                           {"LimitationsArea", "LimSystem", "", CurrentProblem.GetAllConstraints(), true, true}
                       };
        }

        private UiDescription MoreThanOrEqualsZeroLimitationsChanging(UiDescription userData)
        {
            var value = CurrentProblem;
            CurrentProblem = _simplex.ChangeMoreThanZeroConstraints(CurrentProblem);

            SetNextStep(NormalizeActionChoiceStep);

            return new UiDescription
                       {
                           {"LppView", "LPP", "", value},
                           {"Label", "lab", null, "Enter changed limitations system:"},
                           {"LimitationsArea", "LimSystem", "", CurrentProblem.GetAllConstraints(), true, true}
                       };
        }

        private UiDescription LessThanOrEqualsLimitationsChanging(UiDescription userData)
        {
            var prevProblem = CurrentProblem;
            CurrentProblem = _simplex.ChangeLessThanConstraints(CurrentProblem);

            SetNextStep(NormalizeActionChoiceStep);

            return new UiDescription
                       {
                           {"LppView", "LPP", "", prevProblem},
                           {"Label", "lab", null, "Введіть змінену задачу:"},
                           {"LppView", "ChangedLPP", "", CurrentProblem, true, true}
                       };
        }

        private UiDescription MoreOrEqualsAndEqualsLimitationsChanging(UiDescription userData)
        {
            var prevProblem = CurrentProblem;
            CurrentProblem = _simplex.ChangeMoreThanAndEqualConstraints(CurrentProblem);

            SetNextStep(NormalizeActionChoiceStep);

            return new UiDescription
                       {
                           {"LppView", "LPP", "", prevProblem},
                           {"Label", "lab", null, "Введіть змінену задачу:"},
                           {"LppView", "ChangedLPP", "", CurrentProblem, true, true}
                       };
        }

        private UiDescription VariablesWithoutZeroLimitationsChanging(UiDescription userData)
        {
            var prevProblem = CurrentProblem;
            CurrentProblem = _simplex.ReplaceVariablesWithoutZeroConstraints(CurrentProblem);

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
            CurrentSimplexTable = _simplex.MakeFirstSimplexTable(CurrentProblem);
            CurrentSimplexTable = _simplex.CalculateRatings(CurrentSimplexTable);

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

            SetNextStep(_solveActionsSteps[IsEndRightVariant]);

            var nextStepRightVariant = new StepVariants(new[] { _solveActionsLabels[IsEndRightVariant] });

            return new UiDescription
                       {
                           {"SimplexTableView", "table", "", CurrentSimplexTable},
                           {"Label", "lab1", "Подальші дії?"},
                           {"StepsContainer", "variants", IsEndAllVariants, nextStepRightVariant, true, true}
                       };
        }

        private UiDescription SolvingElementChoosingStep(UiDescription userData)
        {
            SetNextStep(MakeNextSimplexTableStep);

            var solvingElement = _simplex.GetSolvingElement(CurrentSimplexTable);
            var varsString = string.Empty;
            var variableCount = CurrentSimplexTable.Variables.Count;
            for (var i = 0; i < variableCount; i++)
                varsString += CurrentSimplexTable.Variables.ElementAt(i) + (i == variableCount - 1 ? "" : "|");
            var cellVariable = new StepVariants(new[] { CurrentSimplexTable.GetVariable(solvingElement.CellIndex) });
            var rowVariable = new StepVariants(new[] { CurrentSimplexTable.GetBasisVariableLabel(solvingElement.RowIndex) });

            return new UiDescription
                       {
                           {"SimplexTableView", "table", "", CurrentSimplexTable},
                           {"Label", "lab1", "Виберіть змінну, яка вводиться в базис:"},
                           {"StepsContainer", "cellVar", varsString, cellVariable, true, true},
                           {"Label", "lab2", "Виберіть змінну, яка виводиться з базису:"},
                           {"StepsContainer", "rowVar", varsString, rowVariable, true, true}
                       };
        }

        private UiDescription MakeNextSimplexTableStep(UiDescription userData)
        {
            SetNextStep(IsEndStep);

            var prevTable = CurrentSimplexTable;

            var solvingElement = _simplex.GetSolvingElement(CurrentSimplexTable);
            var cellVariable = new StepVariants(new[] { CurrentSimplexTable.GetVariable(solvingElement.CellIndex) });
            var rowVariable = new StepVariants(new[] { CurrentSimplexTable.GetBasisVariableLabel(solvingElement.RowIndex) });

            CurrentSimplexTable = _simplex.NextSimplexTable(CurrentSimplexTable, solvingElement);

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
                               "Label", "lab2", 
                               "Нова симплекс-таблиця(" + cellVariable.Variants.ElementAt(0) + " вводиться в базис, " + 
                                    rowVariable.Variants.ElementAt(0) + " - виводиться)."},
                           {"SimplexTableView", "table", "", CurrentSimplexTable, true, true, tableSettings}
                       };
        }

        private UiDescription NormalizedProblemResultEnteringStep(UiDescription userData)
        {
            SetNextStep(InitialProblemResultEnteringStep);

            var result = _simplex.GetNormalizedProblemResult(CurrentSimplexTable, CurrentProblem);

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

            var prevResult = _simplex.GetNormalizedProblemResult(CurrentSimplexTable, CurrentProblem);
            var result = _simplex.GetInitialProblemResult(CurrentSimplexTable, CurrentProblem);

            var data = new UiDescription
                           {
                               {"Label", "lab1", "Ф-ція цілі допоміжної задачі:"},
                               {"TargetFunctionBox", "targetFunction", CurrentProblem.TargetFunction},
                               {"Label", "lab2", "Результат розв’язку допоміжної задачі:"},
                               {"LppResultView", "prevResult", prevResult},
                               {"Label", "lab3", "Ф-ція цілі вихідної задачі:"},
                               {"TargetFunctionBox", "initialProblemTF", CurrentProblem.InitialProblem.TargetFunction},
                               {"Label", "lab4", "Заміни введені у допоміжну задачу:"}
                           };
            foreach (var replacement in CurrentProblem.Replacements)
                data.Add("Label", replacement.Key + "Lab", replacement.Key + " = " +
                    replacement.Value.Key + " - " + replacement.Value.Value);

            data.Add("Label", "lab5", "Введіть результат розв’язку вихідної задачі:");
            data.Add("LppResultView", "result", "", result, true, true,
                     CurrentProblem.InitialProblem.TargetFunction.Arguments);
            return data;
        }

        #endregion

        #region [ For choosing next step of normalizing ]

        private void SetNormalizeActionsLabels()
        {
            _normalizeActionsLabels = new Dictionary<NormalizeActionTypes, string>
             {
                 {NormalizeActionTypes.TargetChanging,           "Змінити напрям ф-ції цілі"},
                 {NormalizeActionTypes.FreeCoefficientsChanging, "Позбавитись від’ємних вільних членів"},
                 {NormalizeActionTypes.MoreThanZeroChanging,     "Змінити знаки нерівностей виду \">= 0\""},
                 {NormalizeActionTypes.LessThanChanging,         "Змінити обмеження виду \"<=\""},
                 {NormalizeActionTypes.MoreThanAndEqualChanging, "Залишились обмеження виду \">=\" i \"=\". Виконати відповідні перетворення"},
                 {NormalizeActionTypes.MoreThanOnlyChanging,     "Залишились обмеження виду \">=\". Виконати відповідні перетворення"},
                 {NormalizeActionTypes.EqualOnlyChanging,        "Залишились обмеження виду \"=\". Виконати відповідні перетворення"},
                 {NormalizeActionTypes.WithoutZeroLimChanging,   "Не на всі змінні накладені умови невід’ємності. Ввести заміни типу x = x' - x''"},
                 {NormalizeActionTypes.NormalazingEnding,        "Задача має вигляд, зручний для застосування симплекс-методу. Почати розв’язок"}
             };
        }

        private void SetNormalizeActionsSteps()
        {
            _normalizeActionsSteps = new Dictionary<NormalizeActionTypes, Step>
             {
                 {NormalizeActionTypes.TargetChanging,           TargetChanging},
                 {NormalizeActionTypes.FreeCoefficientsChanging, FreeCoeffChanging},
                 {NormalizeActionTypes.MoreThanZeroChanging,     MoreThanOrEqualsZeroLimitationsChanging},
                 {NormalizeActionTypes.LessThanChanging,         LessThanOrEqualsLimitationsChanging},
                 {NormalizeActionTypes.MoreThanAndEqualChanging, MoreOrEqualsAndEqualsLimitationsChanging},
                 {NormalizeActionTypes.MoreThanOnlyChanging,     MoreOrEqualsAndEqualsLimitationsChanging},
                 {NormalizeActionTypes.EqualOnlyChanging,        MoreOrEqualsAndEqualsLimitationsChanging},
                 {NormalizeActionTypes.WithoutZeroLimChanging,   VariablesWithoutZeroLimitationsChanging},
                 {NormalizeActionTypes.NormalazingEnding,        MakeFirstSimplexTableStep}
             };
        }

        private string GetAllNormalizeActions()
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

        private string GetPosibleNormalizeActions(NormalizeActionTypes nextStep)
        {
            if (nextStep == NormalizeActionTypes.FreeCoefOrMoreThanZeroChanging)
                return _normalizeActionsLabels[NormalizeActionTypes.FreeCoefficientsChanging] + "|" +
                    _normalizeActionsLabels[NormalizeActionTypes.MoreThanZeroChanging];
            return _normalizeActionsLabels[nextStep];
        }

        private void SetNextStepOfNormalizing(NormalizeActionTypes nextStep)
        {
            if (nextStep == NormalizeActionTypes.FreeCoefOrMoreThanZeroChanging)
            {
                SetNextStep(BeforLessThanChoice);
                return;
            }
            SetNextStep(_normalizeActionsSteps[nextStep]);
        }

        private string SolveOrNormalizing()
        {
            return _simplex.IsNormalized(CurrentProblem)
                ? SolveLabel
                : NormilizeLabel;
        }

        private NormalizeActionTypes NextStepOfNormalizing()
        {
            //1.
            if (!_simplex.IsTargetFunctionMinimized(CurrentProblem))
                return NormalizeActionTypes.TargetChanging;
            //2.
            var freeCoef = !_simplex.AreFreeCoefficientsNonNegative(CurrentProblem);
            var moreZero = _simplex.DoProblemHaveMoreThanZeroConstraint(CurrentProblem);
            if (freeCoef && moreZero) return NormalizeActionTypes.FreeCoefOrMoreThanZeroChanging;
            if (freeCoef) return NormalizeActionTypes.FreeCoefficientsChanging;
            if (moreZero) return NormalizeActionTypes.MoreThanZeroChanging;
            //3.
            if (_simplex.DoProblemHaveLessThanConstraint(CurrentProblem))
                return NormalizeActionTypes.LessThanChanging;
            //4. 
            var equations = _simplex.ContainsEqualConstraints(CurrentProblem);
            var moreEqual = _simplex.ContainsMoreThanConstraints(CurrentProblem);
            if (moreEqual && equations)
                return NormalizeActionTypes.MoreThanAndEqualChanging;
            if (moreEqual)
                return NormalizeActionTypes.MoreThanOnlyChanging;
            if (!_simplex.DoAllConstraintsHaveBasisVariable(CurrentProblem))
                return NormalizeActionTypes.EqualOnlyChanging;
            //5.
            return !_simplex.DoAllVariablesHaveZeroConstraint(CurrentProblem)
                ? NormalizeActionTypes.WithoutZeroLimChanging
                : NormalizeActionTypes.NormalazingEnding;
        }

        #endregion

        #region [ For 'is end' checking ]

        private void SetSolveActionsLabels()
        {
            _solveActionsLabels = new Dictionary<SolveActionTypes, string>
                                      {
                                          {SolveActionTypes.ResultEntering, "Ввести результат."},
                                          {
                                              SolveActionTypes.SolvingElementChoosing,
                                              "План припускає покращення. Вибрати розв’язуючий елемент."
                                              }
                                      };
        }

        private void SetSolveActionsSteps()
        {
            _solveActionsSteps = new Dictionary<SolveActionTypes, Step>
                                     {
                                         {SolveActionTypes.ResultEntering, NormalizedProblemResultEnteringStep},
                                         {SolveActionTypes.SolvingElementChoosing, SolvingElementChoosingStep}
                                     };
        }

        private string IsEndAllVariants
        {
            get
            {
                return _solveActionsLabels[SolveActionTypes.ResultEntering] + "|" +
                       _solveActionsLabels[SolveActionTypes.SolvingElementChoosing];
            }
        }

        private SolveActionTypes IsEndRightVariant
        {
            get
            {
                return _simplex.IsEnd(CurrentSimplexTable)
                    ? SolveActionTypes.ResultEntering
                    : SolveActionTypes.SolvingElementChoosing;
            }
        }

        #endregion
    }
}
