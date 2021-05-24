using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;

namespace Conditus.DynamoDBMapper.Mappers
{
    public static class AttributeValueMapper
    {
        public static T GetEntity<T>(this Dictionary<string, AttributeValue> attributeMap)
            where T : new()
        {
            var entity = new T();

            foreach (var property in entity.GetType().GetProperties())
            {
                var entityProperty = entity.GetType().GetProperty(property.Name);
                if (entityProperty == null || !entityProperty.CanWrite) continue;

                var attributeValue = attributeMap.GetAttributeValue(property.Name);
                if (attributeValue == null) continue;

                object propertyValue = attributeValue.GetPropertyValue(property.PropertyType);

                if(propertyValue != null) entityProperty.SetValue(entity, propertyValue);
            }

            return entity;
        }

        public static AttributeValue GetAttributeValue(this Dictionary<string, AttributeValue> attributeMap, string key)
        {
            AttributeValue attributeValue;
            attributeMap.TryGetValue(key, out attributeValue);

            return attributeValue;
        }

        private static object GetPropertyValue(this AttributeValue attributeValue, Type propertyType)
        {
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
            
            if (propertyType == typeof(object))
                return Convert.ChangeType(attributeValue.M.GetEntity<object>(), propertyType);

            return null;
        }
    }
}