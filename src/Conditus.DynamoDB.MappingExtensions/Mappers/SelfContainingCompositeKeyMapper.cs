using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.Model;
using System.Reflection;
using Conditus.DynamoDB.MappingExtensions.Attributes;

using static Conditus.DynamoDB.MappingExtensions.Constants.MappingConstants;

namespace Conditus.DynamoDB.MappingExtensions.Mappers
{
    public static class SelfContainingCompositeKeyMapper
    {
        public static AttributeValue GetSelfContainingCompositeKeyAttributeValue<T>(T entity, string compositeKeyPropertyName)
        {
            var compositeKey = GetSelfContainingCompositeKey(entity, compositeKeyPropertyName);
            return new AttributeValue { S = compositeKey };
        }

        public static string GetSelfContainingCompositeKey(object entity, string compositeKeyPropertyName)
        {
            var entityType = entity.GetType();
            var compositeKeyProperty = entityType.GetProperty(compositeKeyPropertyName);
            var selfContainingCompositeKeyAttribute = compositeKeyProperty.GetCustomAttribute(typeof(DynamoDBSelfContainingCompositeKeyAttribute), false)
                as DynamoDBSelfContainingCompositeKeyAttribute;

            if (selfContainingCompositeKeyAttribute == null)
                throw new ArgumentException(
                    $"{nameof(DynamoDBSelfContainingCompositeKeyAttribute)} declaration is missing from property",
                    nameof(compositeKeyPropertyName));

            var compositeKeyKeys = selfContainingCompositeKeyAttribute.Keys;
            var keyProperties = new List<PropertyInfo> { compositeKeyProperty };
            foreach (var key in compositeKeyKeys)
                keyProperties.Add(entityType.GetProperty(key));

            var keyValues = keyProperties.Select(kp =>
            {
                var keyPropertyValue = kp.GetValue(entity);

                return StringMapper.ConvertToDynamoDBStringValue(keyPropertyValue);
            });

            return string.Join(COMPOSITE_KEY_SEPARATOR, keyValues);
        }

        public static T SelfContainingCompositeKeyToEntity<T>(AttributeValue attributeValue)
        {
            var entityType = typeof(T);
            return (T)SelfContainingCompositeKeyToEntity(attributeValue, entityType);
        }

        public static object SelfContainingCompositeKeyToEntity(AttributeValue attributeValue, Type type)
        {
            var compositeKey = attributeValue.S;
            var keyParts = compositeKey.Split(COMPOSITE_KEY_SEPARATOR);
            var entity = StringMapper.ConvertToType(keyParts[0], type);

            return entity;
        }

        public static AttributeValue GetSelfContainingCompositeKeyQueryAttributeValue(this object value)
        {
            var stringValue = StringMapper.ConvertToDynamoDBStringValue(value);
            var query = stringValue + COMPOSITE_KEY_SEPARATOR;

            return new AttributeValue { S = query };
        }
    }
}