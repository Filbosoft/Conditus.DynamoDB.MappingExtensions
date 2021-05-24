using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Amazon.DynamoDBv2.Model;
using DynamoDBMapper.Attributes;

namespace DynamoDBMapper.Mappers
{
    public static class ObjectMapper
    {
        public static AttributeValue GetAttributeValue(this object obj)
            => new AttributeValue { S = obj.ToString() };

        public static Dictionary<string, AttributeValue> GetAttributeValueMap(this object obj)
        {
            var map = new Dictionary<string, AttributeValue>();

            foreach (var propertyInfo in obj.GetType().GetProperties())
            {
                AttributeValue mapValue = GetPropertyAttributeValue(propertyInfo, obj);
                if (mapValue == null) continue;

                map.Add(propertyInfo.Name, mapValue);
            }

            return map;
        }

        private static AttributeValue GetPropertyAttributeValue(PropertyInfo propertyInfo, object obj)
        {
            var type = propertyInfo.PropertyType;
            var value = propertyInfo.GetValue(obj);

            if (value == null) return null;

            if (type == typeof(string))
                return ((string)value).GetAttributeValue();

            if (type == typeof(int))
                return ((int)value).GetAttributeValue();

            if (type == typeof(long))
                return ((long)value).GetAttributeValue();

            if (type == typeof(decimal))
                return ((decimal)value).GetAttributeValue();

            if (type == typeof(DateTime))
                return ((DateTime)value).GetAttributeValue();

            if (type.IsEnum)
                return ((Enum)value).GetAttributeValue();

            if (value is IEnumerable)
                return GetEnumerableAttributeValue(propertyInfo, value);

            return new AttributeValue { M = GetAttributeValueMap(value) };
        }

        private static AttributeValue GetEnumerableAttributeValue(PropertyInfo propertyInfo, object propertyValue)
        {
            var enumerablePropertyValue = (IEnumerable) propertyValue;

            if (IsMapList(propertyInfo))
                return enumerablePropertyValue.GetMapAttributeValue();

            return new AttributeValue{S = JsonSerializer.Serialize(propertyValue)};
        }

        private static bool IsMapList(PropertyInfo propertyInfo)
            => propertyInfo.GetCustomAttribute(typeof(MapListAttribute), false) != null;
    }
}