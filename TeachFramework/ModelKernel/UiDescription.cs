using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TeachFramework.Exceptions;

namespace TeachFramework
{
    /// <summary>
    /// User interface description
    /// </summary>
    public class UiDescription : ICollection<UiDescriptionItem>
    {
        private readonly Dictionary<string, UiDescriptionItem> _data;
        //--------------------------------------------------------------------------
        /// <summary>
        /// Represents user interface description
        /// </summary>
        public UiDescription()
        {
            _data = new Dictionary<string, UiDescriptionItem>();
        }

        /// <summary>
        /// Adds data item to end of collection(control will be under previous controls)
        /// </summary>
        public void Add(UiDescriptionItem item)
        {
            try
            {
                _data.Add(item.Name, item);
            }
            catch (ArgumentException)
            {
                throw new DuplicateDataException(item.Name);
            }
        }

        /// <summary>
        /// Adds data items to end of collection(controls will be under previous controls)
        /// </summary>
        public void Add(IEnumerable<UiDescriptionItem> description)
        {
            foreach (var descriptionItem in description)
            {
                try
                {
                    _data.Add(descriptionItem.Name, descriptionItem);
                    
                }
                catch (ArgumentException)
                {
                    throw new DuplicateDataException(descriptionItem.Name);
                }
            }
        }

        /// <summary>
        /// Adds data item to end of collection(control will be under previous controls)
        /// - input parameters are data item parameters
        /// </summary>
        public void Add(string type, string name, string text, object value, bool isEditable, bool check)
        {
            try
            {
                _data.Add(name, CreateData(type, name, text, value, isEditable, check, null));
            }
            catch (ArgumentException)
            {
                throw new DuplicateDataException(name);
            }
        }

        /// <summary>
        /// Adds data item to end of collection(control will be under previous controls)
        /// - input parameters are data item parameters
        /// </summary>
        public void Add(string type, string name, string text, object value, bool isEditable, bool check, object settings)
        {
            try
            {
                _data.Add(name, CreateData(type, name, text, value, isEditable, check, settings));
            }
            catch (ArgumentException)
            {
                throw new DuplicateDataException(name);
            }
        }

        /// <summary>
        /// Adds data item to end of collection(control will be under previous controls)
        /// - input parameters are data item parameters;
        /// - isEditable and check will be initialized as false
        /// </summary>
        public void Add(string type, string name, string text, object value)
        {
            try
            {
                _data.Add(name, CreateData(type, name, text, value, false, false, null));
            }
            catch (ArgumentException)
            {
                throw new DuplicateDataException(name);
            }
        }

        /// <summary>
        /// Adds data item to end of collection(control will be under previous controls)
        /// - input parameters are data item parameters;
        /// - check will be initialized as false
        /// </summary>
        public void Add(string type, string name, string text, object value, bool isEditable)
        {
            try
            {
                _data.Add(name, CreateData(type, name, text, value, isEditable, false, null));
            }
            catch (ArgumentException)
            {
                throw new DuplicateDataException(name);
            }
        }

        /// <summary>
        /// Adds data item to end of collection(control will be under previous controls)
        /// - input parameters are data item parameters;
        /// - isEditable and check will be initialized as false
        /// - text will be initialized as empty string
        /// </summary>
        public void Add(string type, string name, object value)
        {
            try
            {
                _data.Add(name, CreateData(type, name, string.Empty, value, false, false, null));
            }
            catch (ArgumentException)
            {
                throw new DuplicateDataException(name);
            }
        }

        /// <summary>
        /// Removes data item from collection by name
        /// </summary>
        public bool Remove(string name)
        {
            return _data.Remove(name);
        }

        /// <summary>
        /// Returns true if collection contains item with such name
        /// </summary>
        public bool Contains(string name)
        {
            return _data.Keys.Contains(name);
        }

        /// <summary>
        /// Gets data item by name
        /// </summary>
        public UiDescriptionItem this[string name]
        {
            get
            {
                try
                {
                    return _data[name];
                }
                catch (KeyNotFoundException)
                {
                    throw new NoSuchDataException(name);
                }
            }
        }

        /// <summary>
        /// Clears collection
        /// </summary>
        public void Clear()
        {
            _data.Clear();
        }

        /// <summary>
        /// Gets data which needs check(key - Name, value - Value)
        /// </summary>
        public IDictionary<string, object> GetDataToCheck()
        {
            var dataForCheck = new Dictionary<string, object>();
            foreach (var dataItem in _data.Values)
                if (dataItem.CheckRequired)
                    dataForCheck.Add(dataItem.Name, dataItem.Value);
            return dataForCheck;
        }

        /// <summary>
        /// Creates data item
        /// </summary>
        public static UiDescriptionItem CreateData(string type, string name, string text, object value, bool isEditable, bool check, object settings)
        {
            return new UiDescriptionItem
            {
                Name = name,
                Text = text,
                ControlType = type,
                Value = value,
                Editable = isEditable,
                CheckRequired = check,
                ControlSettings = settings
            };
        }

        //==========================================================================
        #region [ ICollection Members ]

        /// <summary>
        /// Copies all data items to array
        /// </summary>
        public void CopyTo(Array array, int index)
        {
            foreach (var dataItem in _data)
                array.SetValue(dataItem, index++);
        }

        /// <summary>
        /// Gets item count
        /// </summary>
        public int Count
        {
            get { return _data.Count; }
        }

        /// <summary>
        /// Returns true if collection is thread safe
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Returns object for lock
        /// </summary>
        public object SyncRoot
        {
            get { return this; }
        }

        #endregion

        #region [ ICollection<Data> Members ]

        /// <summary>
        /// Returns true if collection contains such item
        /// </summary>
        public bool Contains(UiDescriptionItem item)
        {
            return Contains(item.Name);
        }

        /// <summary>
        /// Copies all data items to array
        /// </summary>
        public void CopyTo(UiDescriptionItem[] array, int arrayIndex)
        {
            foreach (var dataItem in _data)
                array.SetValue(dataItem, arrayIndex++);
        }

        /// <summary>
        /// Always returns false
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes such data item from collection
        /// </summary>
        public bool Remove(UiDescriptionItem item)
        {
            return _data.Remove(item.Name);
        }

        #endregion

        #region [ IEnumerable Members ]

        /// <summary>
        /// Gets enumerator
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(_data);
        }

        #endregion

        #region [ IEnumerable<Data> Members ]

        /// <summary>
        /// Gets enumerator
        /// </summary>
        IEnumerator<UiDescriptionItem> IEnumerable<UiDescriptionItem>.GetEnumerator()
        {
            return new Enumerator(_data);
        }

        #endregion

        /// <summary>
        /// User interface description enumerator
        /// </summary>
        public class Enumerator : IEnumerator<UiDescriptionItem>
        {
            private readonly Dictionary<string, UiDescriptionItem> _dataReference;
            private int _current;

            /// <summary>
            /// Represents user interface description enumerator
            /// </summary>
            /// <param name="data"></param>
            public Enumerator(Dictionary<string, UiDescriptionItem> data)
            {
                _dataReference = data;
                _current = -1;
            }

            #region [ IEnumerator<Data> Members ]

            /// <summary>
            /// Gets current object
            /// </summary>
            public UiDescriptionItem Current
            {
                get
                {
                    if (_current < 0 || _current >= _dataReference.Count)
                        throw new InvalidOperationException();
                    return _dataReference.ElementAt(_current).Value;
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            /// <summary>
            /// Sets next object
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                if (_current < _dataReference.Count)
                    _current++;
                return _current != _dataReference.Count;
            }

            /// <summary>
            /// Reset enumerator
            /// </summary>
            public void Reset()
            {
                _current = -1;
            }

            #endregion

            #region [ IDisposable Members ]

            /// <summary>
            /// Dispose(){}
            /// </summary>
            public void Dispose() { }

            #endregion
        }
    }
}
