using Market.Logic.Commands;
using Market.Logic.Models;
using Market.Logic.Queries;
using Market.Logic.Transport.Deserializers;

namespace Market.Logic.Tests.Transport.Deserializers;

public class ImportQueryResultDeserializerTests
{
    [Fact(DisplayName = $"The {nameof(ImportQueryResultDeserializer)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new ImportQueryResultDeserializer());

        // Assert
        exception.Should().BeNull();
    }

    [Theory(DisplayName = $"The {nameof(ImportQueryResultDeserializer)} can deserialize.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(DeserializeParameters))]
    public void CanDeserialize(string rawSource, QueryResult<IReadOnlyCollection<Link>> expectedResult)
    {
        // Arrange
        var deserializer = new ImportQueryResultDeserializer();

        // Act
        var result = deserializer.Deserialize(rawSource);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory(DisplayName = $"The {nameof(ImportQueryResultDeserializer)} can't deserialize bad string.")]
    [Trait("Category", "Unit")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("\r\n   \t ")]
    public void CanNotDeserializeBadString(string source)
    {
        // Arrange
        var deserializer = new ImportQueryResultDeserializer();

        // Act
        var exception = Record.Exception(() =>
            _ = deserializer.Deserialize(source));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    public readonly static TheoryData<string, QueryResult<IReadOnlyCollection<Link>>> DeserializeParameters = new()
    {
        {
            /*lang=json,strict*/
            @"
                {
                    ""id"":
                    {
                        ""value"": ""some id""
                    },
                    ""errorMessage"": null,
                    ""result"":
                    [
                        {
                            ""internalID"":
                            {
                                ""value"": 1
                            },
                            ""externalID"":
                            {
                                ""value"": 2,
                                ""provider"": 1
                            }
                        },
                        {
                            ""internalID"":
                            {
                                ""value"": 1
                            },
                            ""externalID"":
                            {
                                ""value"": 3,
                                ""provider"": 2
                            }
                        }
                    ]
                }
            ",
            new QueryResult<IReadOnlyCollection<Link>>(
                new("some id"),
                new Link[]
                {
                    new Link(internalID: new(1),
                        externalID: new(2),
                        providerID: new(1)),

                    new Link(internalID: new(1),
                        externalID: new(3),
                        providerID: new(2)),
                },
                errorMessge: null)
        }
    };
}