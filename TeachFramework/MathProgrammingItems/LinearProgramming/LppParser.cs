using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using TeachFramework.MathTypes;

namespace TeachFramework.MathProgramming
{
    /// <summary>
    /// Class for LPP parsing
    /// </summary>
    public static class LppParser
    {
        private static readonly Regex TfRegex = new Regex(TargetFunctionPattern);
        private static readonly Regex LimSysRegex = new Regex(ConstraintsPattern, RegexOptions.Compiled);
        //-------------------------------------------------------------------
        private static string TargetFunctionPattern
        {
            get
            {
                return @"^(?<tfBeginning>f[ ]*\([ ]*x[ ]*\)[ ]*=[ ]*)" +
                       @"(?:(?<var>[ ]*[-+]{0,1}[ ]*(?:(?:M)|(?:[0-9]*)|(?:[0-9]+/[0-9]+))[ ]*[*]*[ ]*x[0-9]+[']{0,2})+)" +
                       @"(?:[ ]*(?:->)[ ]*(?<target>(?:min)|(?:max)))$";
            }
        }
        private static string ConstraintsPattern
        {
            get
            {
                return
                    @"^(?:(?<constraint>(?:(?>(?<variable>(?>[ ]*)[-+]{0,1}[ ]*(?:(?:[0-9]*)|(?:[0-9]+/[0-9]+))(?>[ ]*)(?>[*]*)(?>[ ]*)[x|S](?>[0-9]+)[']{0,2})+))" +
                    @"[ ]*(?<sign>(?:=)|(?:>=)|(?:<=)|(?:<)|(?:>))[ ]*" +
                    @"(?<freeCoefficient>[ ]*[-+]{0,1}[ ]*(?:(?:[0-9]+)|(?:[0-9]+/[0-9]+)))[ ]*[\n]*)+)$";
            }
        }
        //-------------------------------------------------------------------
        /// <summary>
        /// Gets error description or empty string if last parsing was succeed
        /// </summary>
        public static string Error { get; private set; }
        //===================================================================

        /// <summary>
        /// Parses target function
        /// </summary>
        /// <param name="targetFunctionDescription">Target function description</param>
        public static TargetFunction ParseTargetFunction(string targetFunctionDescription)
        {
            var tf = TfRegex.Match(targetFunctionDescription.Trim());
            Error = (!tf.Success) ? "Target function is incorrectly entered" : string.Empty;
            if (!tf.Success)
                return null;
            var description = new List<string> { "f(x) =" };
            foreach (Capture var in tf.Groups["var"].Captures)
                description.Add(var.Value.Replace(" ", "").Replace("*", ""));
            description.Add("->");
            description.Add(tf.Groups["target"].Value.Replace(" ", ""));
            return ParseTargetFunction(description);
        }

        /// <summary>
        /// Validates target function
        /// </summary>
        /// <param name="targetFunctionDescription">Target function description</param>
        public static bool IsValidTargetFunction(string targetFunctionDescription)
        {
            return TfRegex.IsMatch(targetFunctionDescription.Trim());
        }

        /// <summary>
        /// Parses Constraints system
        /// </summary>
        /// <param name="constraintsDescription">Constraints description</param>
        public static List<Constraint> ParseConstraints(string constraintsDescription)
        {
            if (!Regex.IsMatch(constraintsDescription.Trim(), ConstraintsPattern))
            {
                Error = "Constraints system is incorrectly entered";
                return null;
            }
            var limSys = LimSysRegex.Match(constraintsDescription.Trim());
            var description = new List<string>();
            foreach (Capture lim in limSys.Groups["constraint"].Captures)
            {
                var limDescription = LimSysRegex.Match(lim.Value);
                foreach (Capture var in limDescription.Groups["variable"].Captures)
                    description.Add(var.Value);
                description.Add(limDescription.Groups["sign"].Value);
                description.Add(limDescription.Groups["freeCoefficient"].Value);
            }
            return ParseConstraints(description);
        }

        /// <summary>
        /// Validates Constraints system
        /// </summary>
        /// <param name="constraintsDescription">Constraints description</param>
        public static bool IsValidConstraints(string constraintsDescription)
        {
            return LimSysRegex.IsMatch(constraintsDescription.Trim());
        }

        /// <summary>
        /// Parses target function
        /// </summary>
        /// <param name="problemDescription">LPP or just target function description</param>
        public static TargetFunction ParseTargetFunction(IList<string> problemDescription)
        {
            var flag = false;
            var formula = new List<Variable>();
            var target = string.Empty;
            var mLabels = new List<string>();
            for (var i = 0; i < problemDescription.Count; i++)
            {
                if (problemDescription[i].Replace(" ", "") == "->")
                {
                    target = problemDescription[i + 1].Replace(" ", "");
                    break;
                }

                if (flag)
                {
                    if (problemDescription[i].Contains("M"))
                    {
                        mLabels.Add(problemDescription[i].Substring(problemDescription[i].IndexOf("M", StringComparison.Ordinal) + 1));
                        continue;
                    }
                    try
                    {
                        formula.Add(CreateVariable(problemDescription[i]));
                    }
                    catch (FormatException)
                    {
                        return null;
                    }
                }

                if (problemDescription[i].Replace(" ", "") == "f(x)=")
                    flag = true;
            }
            var tf = new TargetFunction(formula, target);
            foreach (var label in mLabels)
                tf.AddVariableWithMaxCoefficient(label);
            return tf;
        }

        /// <summary>
        /// Parses Constraints system
        /// </summary>
        /// <param name="problemDescription">LPP or just Constraints description</param>
        public static List<Constraint> ParseConstraints(IList<string> problemDescription)
        {
            var constraints = new List<Constraint>();
            var formula = new List<Variable>();
            var flag = true;
            for (var i = 0; i < problemDescription.Count; i++)
            {
                problemDescription[i] = problemDescription[i].Replace(" ", "");
                if (problemDescription[i] == "f(x)=")
                {
                    flag = false;
                    continue;
                }
                if (problemDescription[i] == "->")
                {
                    flag = true;
                    i += 2;
                }
                if (!flag) continue;
                try
                {
                    if (problemDescription[i][0] == '<' || problemDescription[i][0] == '>' || problemDescription[i][0] == '=')
                    {
                        constraints.Add(new Constraint(formula, problemDescription[i], CreateFraction(problemDescription[i + 1])));
                        i++;
                        formula.Clear();
                        continue;
                    }
                    formula.Add(CreateVariable(problemDescription[i]));
                }
                catch (FormatException)
                {
                    return null;
                }
            }
            return constraints;
        }

        private static Variable CreateVariable(string varString)
        {
            varString = varString.Replace(" ", "");
            var coefString = string.Empty;
            var label = string.Empty;
            for (var i = 0; i < varString.Length; i++)
            {
                if (char.IsLetter(varString[i]))
                {
                    label = varString.Substring(i);
                    break;
                }
                coefString += varString[i];
            }
            var coefficient = CreateFraction(coefString);
            return new Variable(coefficient, label);
        }
        private static Fraction CreateFraction(string fractionString)
        {
            var coefString = fractionString.Replace(" ", "");
            if (coefString == "-" || coefString == "+" || coefString == string.Empty)
                coefString += "1";
            if (coefString.IndexOf('/') == -1)
                coefString += "/1";
            return new Fraction(coefString);
        }
    }
}
