using System;

namespace Conditus.DynamoDB.MappingExtensions.Attributes
{
    /// <summary>
    /// This attribute specifies that the DynamoDB value of this attributes shall be a composite key
    /// consisting of the value of this property and the values of the composite keys passed in the constructor
    /// 
    /// Warnings:
    ///   - As this key will use the values of other properties in the entity, 
    ///     and that the IPropertyConverter only takes the value of the single property/attribute
    ///     DynamoDBContext won't be able to convert from and to, forcing you to use the low level API
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