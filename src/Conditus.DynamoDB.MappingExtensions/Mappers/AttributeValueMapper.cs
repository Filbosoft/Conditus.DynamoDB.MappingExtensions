using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2.Model;
using Conditus.DynamoDB.MappingExtensions.Attributes;

namespace Conditus.DynamoDB.MappingExtensions.Mappers
{
    public static class AttributeValueMapper
    {
        public static T ToEntity<T>(this Dictionary<string, AttributeValue> attributeMap)
            where T : new()
        {
            return (T)attributeMap.ToEntity(typeof(T));
        }

        public static object ToEntity(this Dictionary<string, AttributeValue> attributeMap, Type type)
        {
            var entity = Activator.CreateInstance(type);

            foreach (var propertyInfo in entity.GetType().GetProperties())
            {
                var entityProperty = entity.GetType().GetProperty(propertyInfo.Name);
                if (entityProperty == null || !entityProperty.CanWrite) continue;

                var attributeValue = attributeMap.GetAttributeValue(propertyInfo.Name);
                if (attributeValue == null) continue;

                var propertyValue = attributeValue.GetPropertyValue(propertyInfo);

                if (propertyValue == null) 
                    continue;

                entityProperty.SetValue(entity, propertyValue);
            }

            return entity;
        }

        public static AttributeValue GetAttributeValue(this Dictionary<string, AttributeValue> attributeMap, string key)
        {
            AttributeValue attributeValue;
            attributeMap.TryGetValue(key, out attributeValue);

            return attributeValue;
        }

        private static object GetPropertyValue(this AttributeValue attributeValue, PropertyInfo propertyInfo)
        {
            var propertyType = propertyInfo.PropertyType;

            if (propertyType == typeof(string))
                return attributeValue.S;

            if (propertyType == typeof(int))
                return Convert.ToInt32(attributeValue.N);

            if (propertyType == typeof(long))
                return Convert.ToInt64(attributeValue.N);

            if (propertyType == typeof(decimal))
                return Convert.ToDecimal(attributeValue.N);

            if (propertyType.IsEnum)
                return Enum.ToObject(propertyType, int.Parse(attributeValue.N));

            if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                return DateTimeMapper.FromUtcUnixTimeMilliseconds(attributeValue.N);

            if (propertyInfo.GetCustomAttribute(typeof(DynamoDBMapListAttribute)) != null)
                return ListMapper.ToEntityList(
                    attributeValue.M,
                    propertyType.GetGenericArguments().First());

            if (attributeValue.IsMSet)
                return attributeValue.M.ToEntity(propertyType);

            return null;
        }
    }
}