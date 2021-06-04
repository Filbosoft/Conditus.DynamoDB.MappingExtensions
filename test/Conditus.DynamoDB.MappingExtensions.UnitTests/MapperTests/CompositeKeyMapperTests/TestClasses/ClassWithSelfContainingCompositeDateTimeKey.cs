using System;
using Conditus.DynamoDB.MappingExtensions.Attributes;

namespace Conditus.DynamoDB.MappingExtensions.UnitTests.MapperTests.CompositeKeyMapperTests.TestClasses
{
    public class ClassWithSelfContainingCompositeDateTimeKey
    {
        [DynamoDBSelfContainingCompositeKey(nameof(KeyProperty1), nameof(KeyProperty2))]
        public DateTime SelfContainingCompositeKey { get; set; }
        public string KeyProperty1 { get; set; }
        public string KeyProperty2 { get; set; }        
    }
}