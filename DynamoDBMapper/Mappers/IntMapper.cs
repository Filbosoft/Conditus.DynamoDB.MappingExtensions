using Amazon.DynamoDBv2.Model;

namespace DynamoDBMapper.Mappers
{
    public static class IntMapper
    {
        public static AttributeValue GetAttributeValue(this int value)
            => new AttributeValue { N = value.ToString() };
    }
}