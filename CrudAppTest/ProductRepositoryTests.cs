using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using CrudApp.Controllers;
using CrudApp.Repositories;
using CrudApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

public class ProductRepositoryTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ProductController _controller;

    public ProductRepositoryTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();
        _controller = new ProductController(_mockProductRepository.Object);
    }

    [Theory]
    [InlineData(1, "Test Product", 100)]
    public async Task GetProductById(int id, string name, decimal price)
    {
        // Arrange
        var productDto = new ProductDto { Name = name, Price = price };
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(productDto);

        // Act
        var result = await _controller.GetProductById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result); 
        var returnValue = Assert.IsType<ProductDto>(okResult.Value); 
        Assert.Equal(name, returnValue.Name); 
        Assert.Equal(price, returnValue.Price);
    }

    [Fact]
    public async Task GetProductById_NotFound()
    {
        // Arrange
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((ProductDto)null);

        // Act
        var result = await _controller.GetProductById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result); // NotFound döndürmeli
    }

    [Theory]
    [InlineData("Product 1", 10)]
    [InlineData("Product 2", 20)]
    public async Task GetProducts(string name, decimal price)
    {
        // Arrange
        var productList = new List<ProductDto>
        {
            new ProductDto { Name = name, Price = price }
        };

        _mockProductRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(productList);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result); // Ok döndürmeli
        var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value); 
        Assert.Single(returnValue);
        Assert.Equal(name, returnValue[0].Name);
        Assert.Equal(price, returnValue[0].Price);
    }

    [Fact]
    public async Task AddProduct()
    {
        // Arrange
        var productDto = new ProductDto { Name = "New Product", Price = 30 };

        _mockMapper.Setup(m => m.Map<Product>(It.IsAny<ProductDto>())).Returns(new Product()); // Mapper mock
        _mockProductRepository.Setup(repo => repo.AddAsync(It.IsAny<ProductDto>()))
            .Returns(Task.CompletedTask); 

        // Act
        var result = await _controller.AddProduct(productDto);

        // Assert
        Assert.IsType<OkResult>(result); 
    }

    [Fact]
    public async Task DeleteProduct()
    {
        // Arrange
        _mockProductRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
