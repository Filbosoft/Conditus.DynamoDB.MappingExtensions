using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;
using Conditus.DynamoDB.MappingExtensions.Attributes;

namespace Conditus.DynamoDB.MappingExtensions.Mappers
{
    public static class ObjectMapper
    {
        public static AttributeValue GetAttributeValue(this object obj)
            => new AttributeValue { S = obj.ToString() };

        public static AttributeValue GetAttributeValue(this object obj, Type type)
        {
            if (obj == null) return null;

            if (type == typeof(string))
                return ((string)obj).GetAttributeValue();

            if (type == typeof(int))
                return ((int)obj).GetAttributeValue();

            if (type == typeof(long))
                return ((long)obj).GetAttributeValue();

            if (type == typeof(decimal))
                return ((decimal)obj).GetAttributeValue();

            if (type == typeof(DateTime) || type == typeof(DateTime?))
                return ((DateTime)obj).GetAttributeValue();

            if (type.IsEnum)
                return ((Enum)obj).GetAttributeValue();

            return new AttributeValue { M = GetAttributeValueMap(obj) };
        }

        public static AttributeValue GetMapAttributeValue(this object obj)
            => new AttributeValue { M = GetAttributeValueMap(obj) };

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

        public static AttributeValue GetPropertyAttributeValue(PropertyInfo propertyInfo, object obj)
        {
            if (AttributeChecker.IsCompositeKey(propertyInfo))
                return CompositeKeyMapper.GetCompositeKeyAttributeValue(obj, propertyInfo.Name);

            var value = propertyInfo.GetValue(obj);
            if (value == null) return null;

            if (AttributeChecker.IsSelfContainingCompositeKey(propertyInfo))
                return SelfContainingCompositeKeyMapper.GetSelfContainingCompositeKeyAttributeValue(obj, propertyInfo.Name);

            var type = propertyInfo.PropertyType;

            if (type == typeof(string))
                return ((string)value).GetAttributeValue();

            if (type == typeof(int))
                return ((int)value).GetAttributeValue();

            if (type == typeof(long))
                return ((long)value).GetAttributeValue();

            if (type == typeof(decimal))
                return ((decimal)value).GetAttributeValue();

            if (type == typeof(DateTime) || type == typeof(DateTime?))
                return ((DateTime)value).GetAttributeValue();

            if (type.IsEnum)
                return ((Enum)value).GetAttributeValue();

            if (value is IEnumerable)
                return GetEnumerableAttributeValue(propertyInfo, value);

            return new AttributeValue { M = GetAttributeValueMap(value) };
        }

        private static AttributeValue GetEnumerableAttributeValue(PropertyInfo propertyInfo, object propertyValue)
        {
            var enumerablePropertyValue = (IEnumerable)propertyValue;

            if (AttributeChecker.IsMapList(propertyInfo))
                return enumerablePropertyValue.GetListMapMapAttributeValue();

            var listAttributeValue = enumerablePropertyValue.GetListAttributeValue();

            return listAttributeValue;
        }
    }
}