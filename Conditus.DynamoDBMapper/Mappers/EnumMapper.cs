using System;
using Amazon.DynamoDBv2.Model;

namespace Conditus.DynamoDBMapper.Mappers
{
    public static class EnumMapper
    {
        public static AttributeValue GetAttributeValue(this Enum enumValue)
        {
            var numericValue = Convert.ToUInt32(enumValue);
            var value = new AttributeValue { N = numericValue.ToString() };

            return value;
        }
    }
}