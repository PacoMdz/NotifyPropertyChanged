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
		#region Event
		/// <summary>
		/// Notify all subscribers when a property value changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region NotifyChanges
		/// <summary>
		/// Notify all subscribers that property value of the given name has changed.
		/// </summary>
		/// <param name="propertyName"> Name of the changed property</param>
		protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (string.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException("propertyName", "Property name can not be null.");

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
			{
				PropertyInfo[] properties = GetType().GetProperties();
				propertiesCount = properties.Length;

				if (propertiesCount != 0)
				{
					propertiesNames = new string[propertiesCount];

					for (index = 0; index < propertiesCount; index++)
						propertiesNames[index] = properties[index].Name;
				}
			}

			propertiesCount = propertiesNames?.Length ?? 0;

			if (propertiesCount != 0)
			{
				string propertyName = string.Empty;

				for (index = 0; index < propertiesCount; index++)
				{
					propertyName = propertiesNames[index];

					if (!string.IsNullOrWhiteSpace(propertyName))
						PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
				}
			}
		}
		#endregion

		#region PropertyValues
		/// <summary>
		/// Update the value of the encapsulated property field.
		/// </summary>
		/// <typeparam name="T">Type of the property</typeparam>
		/// <param name="field">Encapsulated property field</param>
		/// <param name="value">New property value</param>
		/// <param name="propertyName">Name od the property</param>
		/// <returns>If property changed</returns>
		protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
		{
			if (string.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException("propertyName", "Property name can not be null.");

			if (!EqualityComparer<T>.Default.Equals(field, value))
			{
				field = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
				return true;
			}
			return false;
		}
		/// <summary>
		/// Update the value of the encapsulated property field, if the property change 
		/// then execute the calback action.
		/// </summary>
		/// <typeparam name="T">Type of the property</typeparam>
		/// <param name="field">Encapsulated property field</param>
		/// <param name="value">New property value</param>
		/// <param name="callback">Action to execute after the change</param>
		/// <param name="propertyName">Name od the property</param>
		/// <returns>If property changed</returns>
		protected bool SetProperty<T>(ref T field, T value, Action callback, [CallerMemberName] string propertyName = "")
		{
			bool propertyChanged = SetProperty(ref field, value, propertyName);

			if (propertyChanged)
				callback?.Invoke();

			return propertyChanged;
		}
		/// <summary>
		/// Update the value of the encapsulated property field, if the property change 
		/// then execute the calback action with the property name.
		/// </summary>
		/// <typeparam name="T">Type of the property</typeparam>
		/// <param name="field">Encapsulated property field</param>
		/// <param name="value">New property value</param>
		/// <param name="callback">Action to execute after the change</param>
		/// <param name="propertyName">Name od the property</param>
		/// <returns>If property changed</returns>
		protected bool SetProperty<T>(ref T field, T value, Action<string> callback, [CallerMemberName] string propertyName = "")
		{
			bool propertyChanged = SetProperty(ref field, value, propertyName);

			if (propertyChanged)
				callback?.Invoke(propertyName);

			return propertyChanged;
		}
		/// <summary>
		/// Gets the value of the property of the given name.
		/// </summary>
		/// <typeparam name="T">Type of the property.</typeparam>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>Value of the property</returns>
		protected T GetProperty<T>([CallerMemberName] string propertyName = "")
		{
			if (string.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException("propertyName", "Property name can not be null.");

			return (T) GetType().GetProperty(propertyName)?.GetValue(this, null);
		}
		#endregion
	}
}
