﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.Configuration.Test
{
	public class AppSettings : SettingsBase
	{
		public override void Initialize()
		{
			Init(this);
		}

		[SettingsProperty("String Property", SettingsPropertyType.String)]
		public string StringProp
		{
			get => GetValue();
			set => SetValue(value);
		}

		[SettingsProperty("Uri Property", SettingsPropertyType.Uri)]
		public string UriProp
		{
			get => GetValue();
			set => CheckAndSetUri(value, "Uri Property is not a valid Url");
		}

		[SettingsProperty("Password Property", SettingsPropertyType.Password)]
		public string PasswordProp
		{
			get => GetValue();
			set => SetValue(value);
		}

		[SettingsProperty("Integer Property", SettingsPropertyType.Integer)]
		public int IntegerProp
		{
			get => GetValue<int>();
			set => SetValue(value);
		}

	}
}
