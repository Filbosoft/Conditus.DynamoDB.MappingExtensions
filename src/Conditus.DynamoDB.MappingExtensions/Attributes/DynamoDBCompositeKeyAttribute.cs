using System;

namespace Conditus.DynamoDB.MappingExtensions.Attributes
{
    /// <summary>
    /// This attribute specifies that the property is a composite key
    /// consisting of the keys provided.
    /// </summary>
    public class DynamoDBCompositeKeyAttribute : Attribute
    {
        public string[] Keys { get; }
        public DynamoDBCompositeKeyAttribute(params string[] keys)
        {
            Keys = keys;
        }
    }
}