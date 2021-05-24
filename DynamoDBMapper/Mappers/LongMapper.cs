using Amazon.DynamoDBv2.Model;

namespace DynamoDBMapper.Mappers
{
    public static class LongMapper
    {
        public static AttributeValue GetAttributeValue(this long value)
            => new AttributeValue { N = value.ToString() };
    }
}