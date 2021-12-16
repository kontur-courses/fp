﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using TagsCloudContainer;

namespace TagsCloudApp.Parsers
{
    public class EnumParser : IEnumParser
    {
        public Result<T> TryParse<T>(string value) where T : struct, Enum
        {
            if (Enum.TryParse<T>(value, true, out var enumValue))
                return Result.Ok(enumValue);

            var valuesDescription = GetEnumValuesDescription(typeof(T));
            return Result.Fail<T>(
                $"Invalid value: {value}. Available values:\n"
                + string.Join(Environment.NewLine, valuesDescription));
        }

        private static IEnumerable<string> GetEnumValuesDescription(Type type)
        {
            foreach (var enumName in type.GetEnumNames())
            {
                var enumInfo = type.GetMember(enumName)[0];
                var attributes = enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                yield return attributes.Length > 0
                    ? enumName + ": " + ((DescriptionAttribute)attributes[0]).Description
                    : enumName;
            }
        }
    }
}