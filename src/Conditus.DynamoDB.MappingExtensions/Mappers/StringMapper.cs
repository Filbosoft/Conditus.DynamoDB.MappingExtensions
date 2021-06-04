using System;
using Amazon.DynamoDBv2.Model;

namespace Conditus.DynamoDB.MappingExtensions.Mappers
{
    public static class StringMapper
    {
        public static AttributeValue GetAttributeValue(this string value)
            => new AttributeValue { S = value };

        public static T ConvertToScalarType<T>(string value)
        {
            var type = typeof(T);

            return (T)ConvertToScalarType(value, type);
        }

        public static object ConvertToScalarType(string value, Type type)
        {
            if (type == typeof(string))
                return Convert.ChangeType(value, type);
            
            if (type == typeof(int))
                return Convert.ToInt32(value);

            if (type == typeof(long))
                return Convert.ToInt64(value);

            if (type == typeof(decimal))
                return Convert.ToInt64(value);

            if (type.IsEnum)
                return Enum.Parse(type, value);

            if (type == typeof(DateTime) || type == typeof(DateTime?))
                return DateTimeMapper.FromUtcUnixTimeMilliseconds(value);

            throw new NotImplementedException("Conversion to the provided value isn't supported");
        }

        public static string ConvertToString(object value, Type type)
        {
            if (type == typeof(string)
                || type == typeof(int)
                || type == typeof(long)
                || type == typeof(decimal)
                || type.IsEnum)
                return value.ToString();

            if (type == typeof(DateTime) || type == typeof(DateTime?))
                return DateTimeMapper.ToUtcUnixTimeMilliseconds((DateTime)value).ToString();

            throw new NotImplementedException("Conversion to the provided value isn't supported");
        }
    }
}