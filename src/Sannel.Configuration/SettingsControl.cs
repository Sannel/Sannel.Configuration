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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace Sannel.Configuration
{
	public sealed class SettingsControl : ContentControl
	{
		private readonly string panelName = "Panel";

		private StackPanel panel;

		public SettingsControl()
		{
			DefaultStyleKey = typeof(SettingsControl);
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			panel = (StackPanel)GetTemplateChild(panelName);

			panel.Children.Clear();

			DataContext = SettingsBase.Current;

			if (DataContext == null)
			{
				return;
			}

			foreach (var (label, propertyName, type) in SettingsBase.Current.GetSettings())
			{
				var binding = new Binding()
				{
					Path = new PropertyPath(propertyName),
					Source = DataContext,
					Mode = BindingMode.TwoWay
				};
				switch (type)
				{
					case SettingsPropertyType.String:
					case SettingsPropertyType.Uri:
					case SettingsPropertyType.Integer:

						var element = new TextBox()
						{
							Header = label,
							Text = SettingsBase.Current.GetValue(propertyName)
						};
						panel.Children.Add(element);

						BindingOperations.SetBinding(element, TextBox.TextProperty, binding);

						if (type == SettingsPropertyType.Uri)
						{
							var scope = new InputScope();
							scope.Names.Add(new InputScopeName()
							{
								NameValue = InputScopeNameValue.Url
							});
							element.InputScope = scope;
						}
						else if(type == SettingsPropertyType.Integer)
						{
							var scope = new InputScope();
							scope.Names.Add(new InputScopeName()
							{
								NameValue = InputScopeNameValue.Number
							});
							element.InputScope = scope;
						}

						break;

					case SettingsPropertyType.Password:
						var pass = new PasswordBox()
						{
							Header = label,
							Password = SettingsBase.Current.GetValue(propertyName)
						};
						panel.Children.Add(pass);

						BindingOperations.SetBinding(pass, PasswordBox.PasswordProperty, binding);

						break;
				}
			}
		}

	}
}
