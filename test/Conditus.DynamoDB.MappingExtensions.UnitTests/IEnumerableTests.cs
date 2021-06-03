using System.Collections;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Conditus.DynamoDB.MappingExtensions.Attributes;
using Conditus.DynamoDB.MappingExtensions.Mappers;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDB.MappingExtensions.UnitTests
{
    public class IEnumerableTests
    {
        private class ClassWithNestedIEnumerable
        {
            public string Name { get; set; }
            [MapList]
            public IEnumerable<NestedItem> NestedItems { get; set; }
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
            var testObject = new ClassWithNestedIEnumerable
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
            var testObject = new ClassWithNestedIEnumerable
            {
                Name = "Test",
                NestedItems = new List<NestedItem>()
            };

            //When
            var attributeValueMap = testObject.GetAttributeValueMap();

            //Then
            attributeValueMap.Should().NotContainKey(nameof(ClassWithNestedIEnumerable.NestedItems));
        }

        [Fact]
        public void ToEntityList_WithValidList_ShouldReturnListWithEntities()
        {
            //Given
            var element = new NestedItem
            {
                Id = "e64173da-d545-4b16-9a9c-7aa1b474ce3a",
                StringProp = "stringProp",
                LongProp = 100
            };
            var listElement = new Dictionary<string, AttributeValue>
            {
                {nameof(NestedItem.Id), new AttributeValue{ S = element.Id }},
                {nameof(NestedItem.StringProp), new AttributeValue{ S = element.StringProp }},
                {nameof(NestedItem.LongProp), new AttributeValue{ N = element.LongProp.ToString() }}
            };
            var listMap = new Dictionary<string, AttributeValue>
            {
                {element.Id, new AttributeValue{ M = listElement }}
            };

            //When
            var list = listMap.ToEntityList<NestedItem>();

            //Then
            list.Should().NotBeNullOrEmpty()
                .And.ContainEquivalentOf(element);
        }

        [Fact]
        public void ToEntity_WithNestedMapList_ShouldReturnEntityWithNestedList()
        {
            //Given
            var nestedElement = new NestedItem
            {
                Id = "e64173da-d545-4b16-9a9c-7aa1b474ce3a",
                StringProp = "stringProp",
                LongProp = 100
            };
            var element = new ClassWithNestedIEnumerable
            {
                Name = "el",
                NestedItems = new List<NestedItem> { nestedElement }
            };

            var nestedElementMap = new Dictionary<string, AttributeValue>
            {
                {nameof(NestedItem.Id), new AttributeValue{ S = nestedElement.Id }},
                {nameof(NestedItem.StringProp), new AttributeValue{ S = nestedElement.StringProp }},
                {nameof(NestedItem.LongProp), new AttributeValue{ N = nestedElement.LongProp.ToString() }}
            };
            var listMap = new Dictionary<string, AttributeValue>
            {
                {nestedElement.Id, new AttributeValue{ M = nestedElementMap }}
            };
            var elementMap = new Dictionary<string, AttributeValue>
            {
                {nameof(ClassWithNestedIEnumerable.Name), new AttributeValue{S = element.Name}},
                {nameof(ClassWithNestedIEnumerable.NestedItems), new AttributeValue{M = listMap}}
            };

            //When
            var entity = elementMap.ToEntity<ClassWithNestedIEnumerable>();

            //Then
            entity.Should().NotBeNull()
                .And.BeEquivalentTo(element);
            entity.NestedItems.Should().NotBeNullOrEmpty()
                .And.ContainEquivalentOf(nestedElement);
        }
    }
}
