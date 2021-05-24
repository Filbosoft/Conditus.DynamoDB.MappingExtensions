using Amazon.DynamoDBv2.Model;

namespace DynamoDBMapper.Mappers
{
    public interface IDynamoDBMapper<T>
    {
        AttributeValue GetAttributeValue(T value);
    }
}