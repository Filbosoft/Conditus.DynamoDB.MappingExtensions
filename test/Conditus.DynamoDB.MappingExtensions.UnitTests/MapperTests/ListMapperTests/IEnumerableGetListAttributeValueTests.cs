using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Conditus.DynamoDB.MappingExtensions.Mappers;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDB.MappingExtensions.UnitTests.MapperTests.ListMapperTests
{
    public class IEnumerableGetListAttributeValueTests
    {
        private class ClassWithNestedList
        {
            public string Name { get; set; }
            public List<NestedItem> NestedItems { get; set; }
        }

        private class NestedItem
        {
            public string Id { get; set; }
            public string StringProp { get; set; }
            public long LongProp { get; set; }
        }

        [Fact]
        public void GetListAttributeValue_WithObjectList_ShouldReturnListAttributeValueWithMapsOfObjects()
        {
            //Given
            var element = new NestedItem
            {
                Id = "e64173da-d545-4b16-9a9c-7aa1b474ce3a",
                StringProp = "stringProp",
                LongProp = 100
            };
            var elementList = new List<NestedItem> { element };

            //When
            var listAttributeValue = elementList.GetListAttributeValue();

            //Then
            var attributeValueList = listAttributeValue.L;
            var expectedElementAttributeMap = new Dictionary<string, AttributeValue>
            {
                {nameof(NestedItem.Id), new AttributeValue{ S = element.Id }},
                {nameof(NestedItem.StringProp), new AttributeValue{ S = element.StringProp }},
                {nameof(NestedItem.LongProp), new AttributeValue{ N = element.LongProp.ToString() }}
            };
            var expectedAttributeMapList = new List<AttributeValue>
            {
                new AttributeValue{M = expectedElementAttributeMap}
            };

            attributeValueList.Should().NotBeNullOrEmpty()
                .And.BeEquivalentTo(expectedAttributeMapList);
        }

        [Fact]
        public void GetListAttributeValue_WithNestedList_ShouldReturnMapAttributeValueWithNestedListAttributeValue()
        {
            //Given
            var nestedElement = new NestedItem
            {
                Id = "e64173da-d545-4b16-9a9c-7aa1b474ce3a",
                StringProp = "stringProp",
                LongProp = 100
            };
            var element = new ClassWithNestedList
            {
                Name = "el",
                NestedItems = new List<NestedItem> { nestedElement }
            };

            //When
            var elementAttributeValueMap = element.GetAttributeValueMap();

            //Then
            var expectedNestedElementMap = new Dictionary<string, AttributeValue>
            {
                {nameof(NestedItem.Id), new AttributeValue{ S = nestedElement.Id }},
                {nameof(NestedItem.StringProp), new AttributeValue{ S = nestedElement.StringProp }},
                {nameof(NestedItem.LongProp), new AttributeValue{ N = nestedElement.LongProp.ToString() }}
            };
            var expectedNestedList = new List<AttributeValue>
            {
                new AttributeValue{ M = expectedNestedElementMap }
            };
            var expectedElementMap = new Dictionary<string, AttributeValue>
            {
                {nameof(ClassWithNestedList.Name), new AttributeValue{S = element.Name}},
                {nameof(ClassWithNestedList.NestedItems), new AttributeValue{L = expectedNestedList}}
            };


            elementAttributeValueMap.Should().NotBeEmpty()
                .And.BeEquivalentTo(expectedElementMap);
        }
    }
}
