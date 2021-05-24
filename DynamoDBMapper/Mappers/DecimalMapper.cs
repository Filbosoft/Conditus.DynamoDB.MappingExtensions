using Amazon.DynamoDBv2.Model;

namespace DynamoDBMapper.Mappers
{
    public static class DecimalMapper
    {
        public static AttributeValue GetAttributeValue(this decimal value)
            => new AttributeValue { N = value.ToString() };
    }
}