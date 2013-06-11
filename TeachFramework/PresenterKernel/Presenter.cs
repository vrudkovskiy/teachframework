using System;
using System.Collections;
using System.Collections.Generic;

using TeachFramework.Exceptions;
using TeachFramework.Interfaces;

namespace TeachFramework
{
    /// <summary>
    /// Presenter for teach programs
    /// </summary>
    public class Presenter
    {
        private const int ModelIsNotChoosen = -1;
        private IView _view;
        private IViewBuilder _viewBuilder;
        private Dictionary<int, IModel> _models;
        private readonly List<UiDescriptionItem> _dataToCheck = new List<UiDescriptionItem>();
        private Dictionary<string, int> _modelsDescriptions;
        private int _currentModel;
        private const string PresenterName = "Навчаючі програми";
        private const string InvalidDataMessage = "Дані введені неправильно";
        private const string ModelIsNotChosenMessage = "Вберіть програму, будь-ласка";
        private const string SuccessMessage = "Правильно";
        private const string FailMessage = "Неправильно. Спобуйте ще.";
        private const string ModelEndedMessage = "Програма закінчила роботу";
        private const string InvalidModelMessageFirstPart = "У плагіні '";
        private const string InvalidModelMessageSecondPart = "' виникла помилка:";
        private const string UnknownErrorMessage = "Виникла невідома помилка. Помилка у поточному навчаюсому модулі." +
                                                   " Прочитайте інформацію про неї нижче. ";
        private readonly StartPageDesigner _startPageDesigner = new StartPageDesigner();
        //------------------------------------------------------------------------------
        /// <summary>
        /// Gets name of current model
        /// </summary>
        public string CurrentModelName
        {
            get { return _models[_currentModel].GetDescription(); }
        }

        /// <summary>
        /// Gets choosen model or null
        /// </summary>
        public IModel CurrentModel
        {
            get { return _currentModel == ModelIsNotChoosen ? null : _models[_currentModel]; }
        }
        //==============================================================================
        /// <summary>
        /// Represents Presenter
        /// </summary>
        public Presenter(){}

        /// <summary>
        /// Represents Presenter
        /// </summary>
        public Presenter(IViewBuilder viewBuilder, IEnumerable<IModel> models)
        {
            SetModels(models);
            RefreshPresenter(viewBuilder);
            BuildStartPage(PresenterName);
        }

        /// <summary>
        /// Sets presenter's settings
        /// </summary>
        public void RefreshPresenter(IViewBuilder viewBuilder)
        {
            _viewBuilder = viewBuilder;
            _view = viewBuilder.Build(null);
            _view.SubmitButtonPressed += ViewSubmitBtnPressed;
            _view.AutoButtonPressed += ViewAutoBtnPressed;
            _view.ResetButtonPressed += ViewResetBtnPressed;
        }

        private void ViewSubmitBtnPressed(object sender, EventArgs e)
        {
            #region [ View validating ]
            
            if (!_view.IsValid())
            {
                _view.ShowMessage(InvalidDataMessage);
                return;
            }
            
            #endregion

            var data = _view.GetUserData();

            #region [ Verifying if model is choosen ]

            if (_currentModel == ModelIsNotChoosen)
            {
                SetCurrentModel(data);
                data.Clear();
            }
            if (_currentModel == ModelIsNotChoosen)
            {
                _view.ShowMessage(ModelIsNotChosenMessage);
                return;
            }

            #endregion

            #region [ Input data checking ]

            bool correct;
            try
            {
                correct = CheckData(data);
            }
            catch (NotMatchedValuesException exception)
            {
                _view.ShowMessage(exception.Message);
                _models[_currentModel].Reset();
                BuildStartPage(PresenterName);
                return;
            }

            if (!correct)
            {
                _view.ShowMessage(FailMessage);
                return;
            }
            if (_dataToCheck.Count != 0)
            {
                _view.ShowMessage(SuccessMessage);
                _dataToCheck.Clear();
            }

            #endregion

            var model = _models[_currentModel];

            #region [ IsEnd of model checking ]

            if (model.IsEnd)
            {
                _view.ShowMessage(ModelEndedMessage);
                BuildStartPage(PresenterName);
                model.Reset();
                return;
            }

            #endregion

            #region [ ExecuteStep ]

            try
            {
                data = GetNextStepData(data);
            }
            catch (InvalidModelException exception)
            {
                var requestStr = InvalidModelMessageFirstPart + 
                                    CurrentModelName + 
                                    InvalidModelMessageSecondPart +
                                    Environment.NewLine +
                                    " - " + 
                                    exception.Message;
                _view.ShowMessage(requestStr);
                _models[_currentModel].Reset();
                BuildStartPage(PresenterName);
                return;
            }
            catch (DuplicateDataException exception)
            {
                _view.ShowMessage(exception.Message);
                _models[_currentModel].Reset();
                BuildStartPage(PresenterName);
                return;
            }
            catch (NoSuchDataException exception)
            {
                _view.ShowMessage(exception.Message);
                _models[_currentModel].Reset();
                BuildStartPage(PresenterName);
                return;
            }
            catch (Exception exception)
            {
                _view.ShowMessage(UnknownErrorMessage + Environment.NewLine + Environment.NewLine + exception.Message);
                _models[_currentModel].Reset();
                BuildStartPage(PresenterName);
                return;
            }

            #endregion

            #region [ SetDataToCheck ]

            try
            {
                SetDataToCheck(data);
            }
            catch (NotComparableDataValueException exception)
            {
                _view.ShowMessage(exception.Message);
                _models[_currentModel].Reset();
                BuildStartPage(PresenterName);
                return;
            }

            #endregion

            #region [ View building ]

            try
            {
                _view = _viewBuilder.Build(data);
            }
            catch(NotExistingControlException exception)
            {
                _view.ShowMessage(exception.Message);
                _models[_currentModel].Reset();
                BuildStartPage(PresenterName);
            }
            catch(InvalidControlDataValueException exception)
            {
                _view.ShowMessage(exception.Message);
                _models[_currentModel].Reset();
                BuildStartPage(PresenterName);
            }

            #endregion
        }

        private void ViewAutoBtnPressed(object sender, EventArgs e)
        {
            _view.ShowCorrectData(_dataToCheck);
            _view.ResetErrors();
        }

        private void ViewResetBtnPressed(object sender, EventArgs e)
        {
            if(_currentModel != ModelIsNotChoosen)
                _models[_currentModel].Reset();
            BuildStartPage(PresenterName);
        }

        protected virtual UiDescription GetNextStepData(UiDescription previousStepData)
        {
            return _models[_currentModel].ExecuteStep(previousStepData);
        }

        #region [ Set and check methods ]

        private void SetCurrentModel(IEnumerable<UiDescriptionItem> data)
        {
            foreach (var dataItem in data)
            {
                if (((IComparable)dataItem.Value).CompareTo(true) != 0) continue;
                _currentModel = _modelsDescriptions[dataItem.Name];
                _view.ViewName = dataItem.Name;
                break;
            }
        }

        private void SetDataToCheck(IEnumerable<UiDescriptionItem> data)
        {
            var notCompareableValues = new List<object>();
            _dataToCheck.Clear();
            foreach (var dataItem in data)
            {
                if (!(dataItem.Value is IComparable) && dataItem.CheckRequired)
                {
                    var value = dataItem.Value as IEnumerable;
                    if (value != null)
                    {
                        var enumerator = (value).GetEnumerator();
                        enumerator.MoveNext();
                        if (!(enumerator.Current is IComparable))
                            notCompareableValues.Add(dataItem.Value);
                    }
                    else
                        notCompareableValues.Add(dataItem.Value);
                }
                if (dataItem.CheckRequired)
                    _dataToCheck.Add(new UiDescriptionItem
                    {
                        Name = dataItem.Name,
                        ControlType = dataItem.ControlType,
                        CheckRequired = dataItem.CheckRequired,
                        Editable = dataItem.Editable,
                        Value = dataItem.Value
                    });
            }
            if (notCompareableValues.Count != 0)
                throw new NotComparableDataValueException(notCompareableValues);
        }

        private bool CheckData(UiDescription data)
        {
            var notMatchedValues = new List<string>();
            foreach (var checkDataElem in _dataToCheck)
                foreach (var userDataElem in data)
                {
                    if (checkDataElem.Name != userDataElem.Name) continue;
                    var isEqual = true;
                    try
                    {
                        isEqual = checkDataElem.Value is IEnumerable
                                   ? CompareCollections(checkDataElem.Value, userDataElem.Value)
                                   : ((IComparable)checkDataElem.Value).CompareTo(userDataElem.Value) == 0;
                    }
                    catch (ArgumentException)
                    {
                        notMatchedValues.Add(checkDataElem.Name);
                    }
                    if (isEqual == false) return false;
                }
            if (notMatchedValues.Count != 0)
                throw new NotMatchedValuesException(notMatchedValues);
            return true;
        }

        private static bool CompareCollections(object first, object second)
        {
            var firstCollection = (IEnumerable)first;
            var secondCollection = (IEnumerable)second;
            foreach (var objectInFirst in firstCollection)
            {
                var flag = false;
                foreach (var objectInSecond in secondCollection)
                {
                    if (((IComparable)objectInFirst).CompareTo(objectInSecond) != 0) continue;
                    flag = true;
                    break;
                }
                if (!flag) return false;
            }
            return true;
        }

        private void SetModels(IEnumerable<IModel> models)
        {
            _models = new Dictionary<int, IModel>();
            _modelsDescriptions = new Dictionary<string, int>();
            var i = 0;
            foreach (var model in models)
            {
                if (_modelsDescriptions.ContainsKey(model.GetDescription())) continue;
                _models.Add(i, model);
                _modelsDescriptions.Add(model.GetDescription(), i++);
            }
        }

        private void BuildStartPage(string viewName)
        {
            _currentModel = ModelIsNotChoosen;
            _view.ViewName = viewName;
            _view = _viewBuilder.Build(_startPageDesigner.Design(_models.Values));
        }

        #endregion
    }
}
