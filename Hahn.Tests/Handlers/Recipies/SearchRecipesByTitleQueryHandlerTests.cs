using AutoMapper;
using Hahn.Application.Queries.Recipies;
using Hahn.Application.Queries.Recipies.Handlers;
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Domain.Entities;
using Hahn.Infra.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace Hahn.Tests.Handlers.Recipies;

public class SearchRecipiesByTitleQueryHandlerTests
{
    private readonly Mock<IRecipeRepository> _mockRepo;
    private readonly IMapper _mapper;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly SearchRecipiesByTitleQueryHandler _handler;
    private readonly Mock<ILogger<SearchRecipiesByTitleQueryHandler>> _logger;

    public SearchRecipiesByTitleQueryHandlerTests()
    {
        _mockRepo = new Mock<IRecipeRepository>();

        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });
        _mapper = mappingConfig.CreateMapper();

        _mockHttpClientFactory = new Mock<IHttpClientFactory>();

        // Setup HttpClient mock
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
           .Protected()
           // Setup the PROTECTED method to mock
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           )
           // Prepare the expected response of the external API
           .ReturnsAsync(new HttpResponseMessage()
           {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(JsonSerializer.Serialize(new TheMealDbApiResponse
               {
                   Meals = new List<Meal>
                   {
                       new Meal
                       {
                           IdMeal = Guid.NewGuid().ToString(),
                           StrMeal = "Test Meal",
                           StrInstructions = "Test Instructions",
                           StrIngredient1 = "Ingredient1",
                           StrMeasure1 = "1 cup",
                           // Initialize other ingredients and measures as needed
                       }
                   }
               })),
           })
           .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/")
        };

        _mockHttpClientFactory.Setup(_ => _.CreateClient("TheMealDb")).Returns(httpClient);

        _handler = new SearchRecipiesByTitleQueryHandler(_mockRepo.Object, _mapper, _mockHttpClientFactory.Object,_logger.Object);
    }

    [Fact]
    public async Task Handle_ReturnsLocalRecipies_WhenFound()
    {
        // Arrange
        var query = new SearchRecipiesByTitleQuery("Local Recipe");
        var localRecipies = new List<FoodRecipies>
        {
             "Local Recipe", "Local Ingredients", Instructions = "Local Instructions" 
        };
        _mockRepo.Setup(repo => repo.SearchByTitleAsync("Local Recipe")).ReturnsAsync(localRecipies);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal("Local Recipe", ((List<FoodRecipeDto>)result)[0].Title);
        _mockRepo.Verify(repo => repo.SearchByTitleAsync("Local Recipe"), Times.Once);
        _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<FoodRecipies>()), Times.Never);
    }

    [Fact]
    public async Task Handle_SearchesExternalApi_WhenLocalNotFound()
    {
        // Arrange
        var query = new SearchRecipiesByTitleQuery("External Recipe");
        _mockRepo.Setup(repo => repo.SearchByTitleAsync("External Recipe")).ReturnsAsync(new List<FoodRecipies>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal("Test Meal", ((List<FoodRecipeDto>)result)[0].Title);
        _mockRepo.Verify(repo => repo.SearchByTitleAsync("External Recipe"), Times.Once);
        _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<FoodRecipies>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsEmpty_WhenExternalApiNoResult()
    {
        // Arrange
        var query = new SearchRecipiesByTitleQuery("Nonexistent Recipe");
        _mockRepo.Setup(repo => repo.SearchByTitleAsync("Nonexistent Recipe")).ReturnsAsync(new List<FoodRecipies>());

        // Setup HttpClient to return no meals
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("search.php?s=Nonexistent%20Recipe")),
              ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(new HttpResponseMessage()
           {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(JsonSerializer.Serialize(new TheMealDbApiResponse
               {
                   Meals = null
               })),
           })
           .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/")
        };

        _mockHttpClientFactory.Setup(_ => _.CreateClient("TheMealDb")).Returns(httpClient);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
        _mockRepo.Verify(repo => repo.SearchByTitleAsync("Nonexistent Recipe"), Times.Once);
        _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<FoodRecipies>()), Times.Never);
    }
}