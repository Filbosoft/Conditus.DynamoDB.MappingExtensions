using System;
using System.Collections;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2.DataModel;

namespace Conditus.DynamoDB.MappingExtensions.Mappers
{
    public static class ListMapper
    {
        public static AttributeValue GetMapAttributeValue(this IEnumerable enumerable)
        {
            var attributeMap = enumerable.GetAttributeValueMap();

            if (attributeMap == null)
                return null;

            return new AttributeValue { M = attributeMap };
        }

        public static Dictionary<string, AttributeValue> GetAttributeValueMap(this IEnumerable enumerable)
        {
            var list = enumerable.Cast<object>()
                .ToList<object>();

            if (list.Count == 0)
                return null;

            var map = new Dictionary<string, AttributeValue>();
            var firstItem = list.FirstOrDefault();
            var idProperty = GetHashProperty(firstItem);

            foreach (var item in list)
            {
                var idValue = idProperty.GetValue(item);
                var itemMap = item.GetAttributeValueMap();

                map.Add(idValue.ToString(), new AttributeValue { M = itemMap });
            }

            return map;
        }

        public static PropertyInfo GetHashProperty(object obj)
        {
            var properties = obj.GetType().GetProperties();
            var hashKey = properties
                .FirstOrDefault(p => p.GetCustomAttribute(typeof(DynamoDBHashKeyAttribute), true) != null);

            if (hashKey == null)
                throw new ArgumentException(
                    "No DynamoDBHashKeyProperty defined on the passed entity",
                    nameof(obj));

            return hashKey;
        }

        public static List<T> ToEntityList<T>(this Dictionary<string, AttributeValue> map)
            where T : new()
        {
            return map.ToEntityList(typeof(T))
                .Cast<T>()
                .ToList();
        }

        public static IList ToEntityList(this Dictionary<string, AttributeValue> map, Type elementType)
        {
            var listType = typeof(List<>).MakeGenericType(elementType);
            var listInstance = Activator.CreateInstance(listType);
            var entityList = (IList)listInstance;

            if (map.Count == 0)
                return entityList;

            var attributeValues = map
                .Select(m => m.Value);

            if (!attributeValues.First().IsMSet)
                throw new NotImplementedException();

            foreach (var attribute in attributeValues)
                entityList.Add(attribute.M.ToEntity(elementType));

            return entityList;
        }
    }
}