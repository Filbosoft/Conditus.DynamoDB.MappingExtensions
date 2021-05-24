using System;
using System.Collections;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2.DataModel;

namespace Conditus.DynamoDBMapper.Mappers
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
            var idProperty = GetIdProperty(firstItem);

            foreach (var item in list)
            {
                var idValue = idProperty.GetValue(item);
                var itemMap = item.GetAttributeValueMap();

                map.Add(idValue.ToString(), new AttributeValue { M = itemMap });
            }

            return map;
        }

        public static PropertyInfo GetIdProperty(object obj)
        {
            var properties = obj.GetType().GetProperties();
            var hashKey = properties
                .FirstOrDefault(p => p.GetCustomAttribute(typeof(DynamoDBHashKeyAttribute), true) != null);

            if (hashKey != null)
                return hashKey;

            var idProperty = properties
                .FirstOrDefault(p => p.Name.ToUpper().Equals("ID"));

            if (idProperty == null)
                throw new ArgumentOutOfRangeException("obj", "The object did not have a hashkey or an id property");

            return idProperty;
        }
    }
}