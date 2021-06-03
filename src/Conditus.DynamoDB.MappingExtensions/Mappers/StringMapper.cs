using Amazon.DynamoDBv2.Model;

namespace Conditus.DynamoDB.MappingExtensions.Mappers
{
    public static class StringMapper
    {
        public static AttributeValue GetAttributeValue(this string value)
            => new AttributeValue { S = value };
    }
}