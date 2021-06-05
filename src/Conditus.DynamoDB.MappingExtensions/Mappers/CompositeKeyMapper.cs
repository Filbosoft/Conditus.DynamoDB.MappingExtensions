using System;
using Amazon.DynamoDBv2.Model;
using System.Reflection;
using Conditus.DynamoDB.MappingExtensions.Attributes;
using System.Linq;

using static Conditus.DynamoDB.MappingExtensions.Constants.MappingConstants;

namespace Conditus.DynamoDB.MappingExtensions.Mappers
{
    public static class CompositeKeyMapper
    {
        public static AttributeValue GetCompositeKeyAttributeValue<T>(T entity, string compositeKeyPropertyName)
        {
            var compositeKey = GetCompositeKey(entity, compositeKeyPropertyName);
            return new AttributeValue { S = compositeKey };
        }

        public static string GetCompositeKey(object entity, string compositeKeyPropertyName)
        {
            var entityType = entity.GetType();
            var compositeKeyProperty = entityType.GetProperty(compositeKeyPropertyName);
            var compositeKeyAttribute = compositeKeyProperty.GetCustomAttribute(typeof(DynamoDBCompositeKeyAttribute), false)
                as DynamoDBCompositeKeyAttribute;

            if (compositeKeyAttribute == null)
                throw new ArgumentException(
                    $"{nameof(DynamoDBCompositeKeyAttribute)} declaration is missing from property",
                    nameof(compositeKeyPropertyName));

            var compositeKeyKeys = compositeKeyAttribute.Keys;

            var keyValues = compositeKeyKeys.Select(k =>
            {
                var keyProperty = entityType.GetProperty(k);
                var keyPropertyValue = keyProperty.GetValue(entity);

                return StringMapper.ConvertToDynamoDBStringValue(keyPropertyValue);
            });

            return string.Join(COMPOSITE_KEY_SEPARATOR, keyValues);
        }
    }
}