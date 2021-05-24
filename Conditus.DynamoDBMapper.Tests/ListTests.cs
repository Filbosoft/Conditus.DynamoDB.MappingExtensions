using System.Collections;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Conditus.DynamoDBMapper.Attributes;
using Conditus.DynamoDBMapper.Mappers;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDBMapper.Tests
{
    public class ListTests
    {
        private class ClassWithNestedListMap
        {
            public string Name { get; set; }
            [MapList]
            public List<NestedItem> NestedItems { get; set; }
        }

        private class NestedItem
        {
            [DynamoDBHashKey]
            public string Id { get; set; }
            public string StringProp { get; set; }
            public long LongProp { get; set; }
        }

        [Fact]
        public void NestedList()
        {
            //Given
            var nestedItem = new NestedItem
            {
                Id = "53661f69-08a5-4649-b1f0-f0eab2bf3f80",
                StringProp = "stringProp",
                LongProp = 1000
            };
            var testObject = new ClassWithNestedListMap
            {
                Name = "Test",
                NestedItems = new List<NestedItem>
                {
                    nestedItem
                }
            };

            //When
            var attributeMap = testObject.GetAttributeValueMap();

            //Then
            attributeMap.Should().NotBeEmpty()
                .And.ContainKey("NestedItems");

            var nestedItemsAttributeValue = attributeMap.GetAttributeValue("NestedItems");
            var nestedItemsMap = nestedItemsAttributeValue.M;

            nestedItemsMap.Should().NotBeEmpty()
                .And.ContainKey(nestedItem.Id);
        }

        [Fact]
        public void GetMapAttributeValue_WithEmptyList_ShouldReturnNull()
        {
            //Given
            var obj = new List<object>();

            //When
            var attributeValue = ((IEnumerable)obj).GetMapAttributeValue();

            //Then
            attributeValue.Should().BeNull();
        }

        [Fact]
        public void GetAttributeValueMap_WithEntityWithEmptyList_ShouldReturnMapWithoutValueForTheEmptyProperty()
        {
            //Given
            var testObject = new ClassWithNestedListMap
            {
                Name = "Test",
                NestedItems = new List<NestedItem>()
            };

            //When
            var attributeValueMap = testObject.GetAttributeValueMap();

            //Then
            attributeValueMap.Should().NotContainKey(nameof(ClassWithNestedListMap.NestedItems));
        }
    }
}
