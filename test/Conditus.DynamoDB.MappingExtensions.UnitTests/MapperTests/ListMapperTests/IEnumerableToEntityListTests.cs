using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Conditus.DynamoDB.MappingExtensions.Mappers;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDB.MappingExtensions.UnitTests.MapperTests.ListMapperTests
{
    public class IEnumerableToEntityListTests
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
        public void ToEntityList_WithValidList_ShouldReturnListWithEntities()
        {
            //Given
            var element = new NestedItem
            {
                Id = "e64173da-d545-4b16-9a9c-7aa1b474ce3a",
                StringProp = "stringProp",
                LongProp = 100
            };
            var elementAttributeMap = new Dictionary<string, AttributeValue>
            {
                {nameof(NestedItem.Id), new AttributeValue{ S = element.Id }},
                {nameof(NestedItem.StringProp), new AttributeValue{ S = element.StringProp }},
                {nameof(NestedItem.LongProp), new AttributeValue{ N = element.LongProp.ToString() }}
            };
            var attributeMapList = new List<AttributeValue>
            {
                new AttributeValue{M = elementAttributeMap}
            };

            //When
            var entityList = attributeMapList.ToEntityList<NestedItem>();

            //Then
            entityList.Should().NotBeNullOrEmpty()
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
            var element = new ClassWithNestedList
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
            var listMap = new List<AttributeValue>
            {
                new AttributeValue{ M = nestedElementMap }
            };
            var elementMap = new Dictionary<string, AttributeValue>
            {
                {nameof(ClassWithNestedList.Name), new AttributeValue{S = element.Name}},
                {nameof(ClassWithNestedList.NestedItems), new AttributeValue{L = listMap}}
            };

            //When
            var entity = elementMap.ToEntity<ClassWithNestedList>();

            //Then
            entity.Should().NotBeNull()
                .And.BeEquivalentTo(element);
            entity.NestedItems.Should().NotBeNullOrEmpty()
                .And.ContainEquivalentOf(nestedElement);
        }
    }
}
