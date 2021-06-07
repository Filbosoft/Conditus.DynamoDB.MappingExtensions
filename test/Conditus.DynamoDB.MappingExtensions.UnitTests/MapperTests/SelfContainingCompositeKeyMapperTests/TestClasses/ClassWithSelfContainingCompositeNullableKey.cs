using System;
using Conditus.DynamoDB.MappingExtensions.Attributes;

namespace Conditus.DynamoDB.MappingExtensions.UnitTests.MapperTests.CompositeKeyMapperTests.TestClasses
{
    public class ClassWithSelfContainingCompositeNullableKeys
    {
        [DynamoDBSelfContainingCompositeKey(nameof(DateTimeKeyProperty1), nameof(IntProperty1))]
        public string SelfContainingCompositeKey { get; set; }
        public DateTime? DateTimeKeyProperty1 { get; set; }
        public int? IntProperty1 { get; set; }        
    }
}