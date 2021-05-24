using Amazon.DynamoDBv2.Model;

namespace DynamoDBMapper.Mappers
{
    public static class StringMapper
    {
        public static AttributeValue GetAttributeValue(this string value)
            => new AttributeValue { S = value };
    }
}