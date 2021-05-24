using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using DynamoDBMapper.Attributes;
using DynamoDBMapper.Mappers;
using FluentAssertions;
using Xunit;

namespace DynamoDBMapper.Tests
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
    }
}
