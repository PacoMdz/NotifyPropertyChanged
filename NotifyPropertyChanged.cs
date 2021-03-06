using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System;

namespace Mx.Models.NotifyProperties
{
	/// <summary>
	/// Implements the INotifyPropertyChanged interface.
	/// </summary>
	public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        #region Private / Protected Properties Fields
        private readonly string[] _propertiesNames;
        #endregion

        #region Private / Protected Properties
        /// <summary>
        /// Cantains the names of all properties in the object.
        /// </summary>
        protected IReadOnlyList<string> PropertiesNames
        {
            get { return _propertiesNames; }
        }
        #endregion

        #region Private / Protected Methods
        /// <summary>
        /// Check if the property exists in the object.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>Exist property</returns>
        protected bool PropertyExists(string propertyName)
        {
            return _propertiesNames.Length != 0 ? 
                Array.BinarySearch(_propertiesNames, propertyName) != -1 : 
                false;
        }
        #endregion

        #region Constructor
        public NotifyPropertyChanged()
        {
            PropertyInfo[] properties = GetType().GetProperties();
            int propertiesCount = properties.Length;

            if (propertiesCount != 0)
            {
                _propertiesNames = new string[propertiesCount];

                for (int index = 0; index < propertiesCount; index++)
                    _propertiesNames[index] = properties[index].Name;

                if (_propertiesNames.Length != 1)
                    Array.Sort(_propertiesNames);
            }
            else
            {
                _propertiesNames = Array.Empty<string>();
            }
        }
        #endregion

        #region NotifyChanges
        /// <summary>
        /// Notify all subscribers that property value of the given name has changed.
        /// </summary>
        /// <param name="propertyName"> Name of the changed property</param>
        protected void TriggerPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Notify all subscribers that property value of the given name has changed.
        /// </summary>
        /// <param name="propertyName"> Name of the changed property</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException("propertyName", "Property name can not be null.");

            if (!PropertyExists(propertyName))
                throw new MissingMemberException("NotifyPropertyChanged", propertyName);

            TriggerPropertyChanged(propertyName);
        }
        /// <summary>
        /// Notify all subscribers that properties of the given names have changed, 
        /// if no names are given all properties are raised.
        /// </summary>
        /// <param name="propertiesNames">Names of the changed properties.</param>
        protected void RaisePropertiesChanged(params string[] propertiesNames)
        {
            int propertiesCount, index = 0;

            if (propertiesNames == null)
                propertiesNames = _propertiesNames;

            propertiesCount = propertiesNames.Length;

            if (propertiesCount != 0)
            {
                string propertyName = string.Empty;

                for (index = 0; index < propertiesCount; index++)
                {
                    propertyName = propertiesNames[index];

                    if (!string.IsNullOrWhiteSpace(propertyName) && PropertyExists(propertyName))
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
        #endregion

        #region PropertyValues
        /// <summary>
        /// Update the value of the encapsulated property field without validations.
        /// </summary>
        /// <typeparam name="TValue">Type of the property</typeparam>
        /// <param name="field">Encapsulated property field</param>
        /// <param name="value">New property value</param>
        /// <param name="propertyName">Name od the property</param>
        /// <returns>If property changed</returns>
        protected bool SetProperty<TValue>(ref TValue field, TValue value, [CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<TValue>.Default.Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
        /// <summary>
        /// Update the value of the encapsulated property field without validations, 
        /// if the property change then execute the calback action.
        /// </summary>
        /// <typeparam name="TValue">Type of the property</typeparam>
        /// <param name="field">Encapsulated property field</param>
        /// <param name="value">New property value</param>
        /// <param name="callback">Action to execute after the change</param>
        /// <param name="propertyName">Name od the property</param>
        /// <returns>If property changed</returns>
        protected bool SetProperty<TValue>(ref TValue field, TValue value, Action callback, [CallerMemberName] string propertyName = "")
        {
            bool propertyChanged = SetProperty(ref field, value, propertyName);

            if (propertyChanged)
                callback();

            return propertyChanged;
        }
        /// <summary>
        /// Update the value of the encapsulated property field  without validations, 
        /// if the property change then execute the calback action with the property name.
        /// </summary>
        /// <typeparam name="TValue">Type of the property</typeparam>
        /// <param name="field">Encapsulated property field</param>
        /// <param name="value">New property value</param>
        /// <param name="callback">Action to execute after the change</param>
        /// <param name="propertyName">Name od the property</param>
        /// <returns>If property changed</returns>
        protected bool SetProperty<TValue>(ref TValue field, TValue value, Action<string> callback, [CallerMemberName] string propertyName = "")
        {
            bool propertyChanged = SetProperty(ref field, value, propertyName);

            if (propertyChanged)
                callback(propertyName);

            return propertyChanged;
        }

        /// <summary>
        /// Update the value of the encapsulated property field.
        /// </summary>
        /// <typeparam name="TValue">Type of the property</typeparam>
        /// <param name="field">Encapsulated property field</param>
        /// <param name="value">New property value</param>
        /// <param name="propertyName">Name od the property</param>
        /// <returns>If property changed</returns>
        protected bool UpdateProperty<TValue>(ref TValue field, TValue value, [CallerMemberName] string propertyName = "")
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException("propertyName", "Property name can not be null.");

            if (!PropertyExists(propertyName))
                throw new MissingMemberException("NotifyPropertyChanged", propertyName);

            return SetProperty(ref field, value, propertyName);
        }
        /// <summary>
        /// Update the value of the encapsulated property field, if the property change 
        /// then execute the calback action.
        /// </summary>
        /// <typeparam name="TValue">Type of the property</typeparam>
        /// <param name="field">Encapsulated property field</param>
        /// <param name="value">New property value</param>
        /// <param name="callback">Action to execute after the change</param>
        /// <param name="propertyName">Name od the property</param>
        /// <returns>If property changed</returns>
        protected bool UpdateProperty<TValue>(ref TValue field, TValue value, Action callback, [CallerMemberName] string propertyName = "")
        {
            bool propertyChanged = UpdateProperty(ref field, value, propertyName);

            if (propertyChanged)
                callback?.Invoke();

            return propertyChanged;
        }
        /// <summary>
        /// Update the value of the encapsulated property field, if the property change 
        /// then execute the calback action with the property name.
        /// </summary>
        /// <typeparam name="TValue">Type of the property</typeparam>
        /// <param name="field">Encapsulated property field</param>
        /// <param name="value">New property value</param>
        /// <param name="callback">Action to execute after the change</param>
        /// <param name="propertyName">Name od the property</param>
        /// <returns>If property changed</returns>
        protected bool UpdateProperty<TValue>(ref TValue field, TValue value, Action<string> callback, [CallerMemberName] string propertyName = "")
        {
            bool propertyChanged = UpdateProperty(ref field, value, propertyName);

            if (propertyChanged)
                callback?.Invoke(propertyName);

            return propertyChanged;
        }

        /// <summary>
        /// Gets the value of the property of the given name.
        /// </summary>
        /// <typeparam name="TValue">Type of the property.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Value of the property</returns>
        protected TValue GetProperty<TValue>([CallerMemberName] string propertyName = "")
        {
            return (TValue) GetType().GetProperty(propertyName)?.GetValue(this, null);
        }
        /// <summary>
        /// Gets the value of the property of the given name.
        /// </summary>
        /// <typeparam name="TValue">Type of the property.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Value of the property</returns>
        protected TValue SeekProperty<TValue>([CallerMemberName] string propertyName = "")
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException("propertyName", "Property name can not be null.");

            if (!PropertyExists(propertyName))
                throw new MissingMemberException("NotifyPropertyChanged", propertyName);

            return GetProperty<TValue>(propertyName);
        }
        #endregion

        #region Event
        /// <summary>
        /// Notify all subscribers when a property value changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
