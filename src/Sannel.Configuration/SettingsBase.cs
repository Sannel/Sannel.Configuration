/* Copyright 2017 Sannel Software, L.L.C.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

	   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.*/
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Windows.Storage;

namespace Sannel.Configuration
{
	public abstract class SettingsBase
	{
		protected readonly ApplicationDataContainer settings;

		public static SettingsBase Current
		{
			get;
			private set;
		}

		protected SettingsBase()
		{
			settings = ApplicationData.Current.LocalSettings;
			Initialize();
		}

		protected void Init(SettingsBase b)
		{
			SettingsBase.Current = b;
		}

		public abstract void Initialize();

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Gets the settings defended for this app
		/// </summary>
		/// <returns></returns>
		public IEnumerable<(string header, string propertyName, SettingsPropertyType type)> GetSettings()
		{
			foreach (var prop in GetType().GetProperties())
			{
				var p = prop.GetCustomAttribute<SettingsPropertyAttribute>();
				if (p != null)
				{
					var header = p.Header;
					var type = p.Type;
					yield return (header, prop.Name, type);
				}
			}
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		public T GetValue<T>([CallerMemberName]string key = null, T defaultValue = default(T))
			where T : struct
		{
			if (settings.Values.ContainsKey(key))
			{
				if (settings.Values[key] is T value)
				{
					return value;
				}
			}
			return defaultValue;
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public string GetValue([CallerMemberName]string key = null, string defaultValue = null)
		{
			if (settings.Values.ContainsKey(key))
			{
				if (settings.Values[key] is string value)
				{
					return value;
				}
			}
			return defaultValue ?? string.Empty;
		}

		/// <summary>
		/// Sets the value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <param name="key">The key.</param>
		protected void SetValue<T>(T value, [CallerMemberName]string key = null)
			where T : struct
		{
			settings.Values[key] = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(key));
		}

		/// <summary>
		/// Sets the value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="key">The key.</param>
		protected void SetValue(string value, [CallerMemberName]string key = null)
		{
			settings.Values[key] = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(key));
		}
	}
}
