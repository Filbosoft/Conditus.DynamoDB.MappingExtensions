using System;

namespace Conditus.DynamoDB.MappingExtensions.Attributes
{
    /// <summary>
    /// This attribute specifies that the DynamoDB value of this attributes shall be a composite key
    /// consisting of the value of this property and the values of the composite keys passed in the constructor
    /// </summary>
    public class DynamoDBSelfContainingCompositeKeyAttribute : Attribute
    {
        public string[] Keys { get; }
        public DynamoDBSelfContainingCompositeKeyAttribute(params string[] keys)
        {
            Keys = keys;
        }
    }
}