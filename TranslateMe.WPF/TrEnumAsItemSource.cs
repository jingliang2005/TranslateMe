﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xaml;

namespace TranslateMe.WPF
{
    public class TrEnumAsItemSource : MarkupExtension
    {
        private DependencyObject targetObject;
        private DependencyProperty targetProperty;

        public TrEnumAsItemSource()
        { }

        /// <summary>
        /// The type of the enum to convert to a translated itemSource
        /// </summary>
        [ConstructorArgument("enumType")]
        public Type EnumType { get; set; }

        /// <summary>
        /// Specify a string format from the enum value to calculate the TextId for the translation
        /// By Default "EnumType{0}"
        /// </summary>
        public string TextIdStringFormat { get; set; } = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget service = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (service == null)
                return this;

            targetProperty = service.TargetProperty as DependencyProperty;
            targetObject = service.TargetObject as DependencyObject;
            if (targetObject == null || targetProperty == null)
            {
                return this;
            }

            try
            {
                targetObject.GetType().GetProperty("SelectedValuePath")?.SetValue(targetObject, "Data");
                targetObject.GetType().GetProperty("DisplayMemberPath")?.SetValue(targetObject, "TranslatedText");
            }
            catch { }

            return Enum.GetValues(EnumType)
                .Cast<object>()
                .ToList()
                .ConvertAll(e => new TrData()
                {
                    Data = e,
                    TextId = string.Format(TextIdStringFormat ?? EnumType.Name + "{0}", e.ToString()),
                    DefaultText = e.ToString()
                });
        }
    }
}