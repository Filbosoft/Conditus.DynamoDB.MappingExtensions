using Conditus.DynamoDB.MappingExtensions.Attributes;

namespace Conditus.DynamoDB.MappingExtensions.UnitTests.MapperTests.CompositeKeyMapperTests.TestClasses
{
    public class ClassWithSelfContainingCompositeStringKey
    {
        [DynamoDBSelfContainingCompositeKey(nameof(KeyProperty1), nameof(KeyProperty2))]
        public string SelfContainingCompositeKey { get; set; }
        public string KeyProperty1 { get; set; }
        public string KeyProperty2 { get; set; }        
    }
}