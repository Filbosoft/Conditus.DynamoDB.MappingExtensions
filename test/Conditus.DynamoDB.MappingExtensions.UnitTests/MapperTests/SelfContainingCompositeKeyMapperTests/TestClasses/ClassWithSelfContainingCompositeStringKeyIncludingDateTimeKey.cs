using System;
using Conditus.DynamoDB.MappingExtensions.Attributes;

namespace Conditus.DynamoDB.MappingExtensions.UnitTests.MapperTests.CompositeKeyMapperTests.TestClasses
{
    public class ClassWithSelfContainingCompositeStringKeyIncludingDateTimeKey
    {
        [DynamoDBSelfContainingCompositeKey(nameof(DateTimeKeyProperty1), nameof(KeyProperty2))]
        public string SelfContainingCompositeKey { get; set; }
        public DateTime DateTimeKeyProperty1 { get; set; }
        public string KeyProperty2 { get; set; }        
    }
}