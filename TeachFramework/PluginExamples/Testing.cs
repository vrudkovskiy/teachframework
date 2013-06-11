using System.Collections.Generic;
using TeachFramework;

namespace PluginExamples
{
    public class Testing : AbstractModel
    {
        public Testing()
        {
            Description = "Testing";
            Steps.Add(0, Question0);
            Steps.Add(1, Question1);
            Steps.Add(2, Question2);
            Steps.Add(3, Question3);
            Steps.Add(4, Question4);
        }
        //===================================================================================
        private UiDescription Question0(UiDescription userData)
        {
            IsEnd = false;
            const string question = "Сколько будет 2 + 2 * 2";
            const string rightAnswer = "6";
            var wrongAnswers = new List<string> { "8", "4", "Невозможно посчитать" };

            return Question(question, rightAnswer, wrongAnswers);
        }

        private UiDescription Question1(UiDescription userData)
        {
            const string question = "Какой класс является базовым для всех классов в С#?";
            const string rightAnswer = "Object";
            var wrongAnswers = new List<string> { "Int16", "Type", "Нету общего базового класса" };

            return Question(question, rightAnswer, wrongAnswers);
        }

        private UiDescription Question2(UiDescription userData)
        {
            const string question = "Что делает оператор % в С#?";
            const string rightAnswer = "Возвращает остаток от деления";
            var wrongAnswers = new List<string>
                                   {
                                       "Возвращает процентное соотношение двух операндов",
                                       "Форматирует значения разных типов в строку",
                                       "Переводит дробное число в проценты"
                                   };

            return Question(question, rightAnswer, wrongAnswers);
        }

        private UiDescription Question3(UiDescription userData)
        {
            const string question = "Сколько родительских классов может иметь производный класс?";
            const string rightAnswer = "Всегда один";
            var wrongAnswers = new List<string> { "Не больше одного", "Не больше двух", "Любое количество" };

            return Question(question, rightAnswer, wrongAnswers);
        }

        private UiDescription Question4(UiDescription userData)
        {
            const string question = "Are you fell in love with this test";
            const string rightAnswer = "Yes";
            var wrongAnswers = new List<string> { "No" };

            return Question(question, rightAnswer, wrongAnswers);
        }

        private static UiDescription Question(string question, string rightAnswer, IEnumerable<string> wrongAnsers)
        {
            var data = new UiDescription();
            var tmp = new UiDescription();

            var qData = new UiDescriptionItem
            {
                Name = "question",
                ControlType = "Label",
                Value = question
            };
            data.Add(qData);

            var rData = new UiDescriptionItem
            {
                Name = rightAnswer,
                Text = rightAnswer,
                ControlType = "DataRadioButton",
                Value = true,
                CheckRequired = true
            };
            data.Add(rData);

            foreach (var wAnswer in wrongAnsers)
            {
                var wData = new UiDescriptionItem
                {
                    Name = wAnswer,
                    Text = wAnswer,
                    ControlType = "DataRadioButton",
                    Value = false,
                    CheckRequired = true
                };
                tmp.Add(wData);
            }
            foreach (var dataItem in tmp)
                data.Add(dataItem);
            return data;
        }
    }
}
