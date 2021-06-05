using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using System.Reflection;
using Conditus.DynamoDB.MappingExtensions.Attributes;

namespace Conditus.DynamoDB.MappingExtensions.Mappers
{
    public static class CompositeKeyMapper
    {
        public const char KEY_SEPARATOR = '#';

        public static AttributeValue GetSelfContainingCompositeKeyAttributeValue<T>(T entity, string compositeKeyPropertyName)
        {
            var compositeKey = GetSelfContainingCompositeKey<T>(entity, compositeKeyPropertyName);
            return new AttributeValue { S = compositeKey };
        }

        public static string GetSelfContainingCompositeKey<T>(T entity, string compositeKeyPropertyName)
        {
            var entityType = typeof(T);
            var compositeKey = GetSelfContainingCompositeKey(entityType, entity, compositeKeyPropertyName);

            return compositeKey;
        }

        public static AttributeValue GetSelfContainingCompositeKeyAttributeValue(Type type, object entity, string compositeKeyPropertyName)
        {
            var compositeKey = GetSelfContainingCompositeKey(type, entity, compositeKeyPropertyName);
            return new AttributeValue { S = compositeKey };
        }

        public static string GetSelfContainingCompositeKey(Type type, object entity, string compositeKeyPropertyName)
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

            var resultingKeys = new List<string>();
            var propertyValue = compositeKeyProperty.GetValue(entity);
            var propertyStringValue = StringMapper.ConvertToDynamoDBStringValue(propertyValue);

            resultingKeys.Insert(0, propertyStringValue);
            foreach (var key in compositeKeyKeys)
            {
                var keyProperty = entityType.GetProperty(key);
                var keyPropertyValue = keyProperty.GetValue(entity);
                var keyStringValue = StringMapper.ConvertToDynamoDBStringValue(keyPropertyValue);

                resultingKeys.Add(keyStringValue);
            }

            return string.Join(KEY_SEPARATOR, resultingKeys);
        }

        public static T ToEntity<T>(AttributeValue attributeValue)
        {
            var entityType = typeof(T);
            return (T) ToEntity(attributeValue, entityType);
        }

        public static object ToEntity(AttributeValue attributeValue, Type type)
        {
            var compositeKey = attributeValue.S;
            var keyParts = compositeKey.Split(KEY_SEPARATOR);
            var entity = StringMapper.ConvertToType(keyParts[0], type);

            return entity;
        }
    }
}