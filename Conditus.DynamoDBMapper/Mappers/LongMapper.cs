using Amazon.DynamoDBv2.Model;

namespace Conditus.DynamoDBMapper.Mappers
{
    public static class LongMapper
    {
        public static AttributeValue GetAttributeValue(this long value)
            => new AttributeValue { N = value.ToString() };
        public static AttributeValue GetAttributeValue(this long? value)
            => new AttributeValue { N = value.ToString() };
    }
}