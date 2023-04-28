﻿using ShopBackend.Controllers;
using ShopBackend.Models;
using ShopBackend.Dtos;
using Microsoft.AspNetCore.Mvc;


namespace ShopBackend.Test.ControllersTest
{
    /**
     * @author: Sheba
     * @date: 01-04-2023
     */
    public class TestProductsController
    {
        /*
        private readonly List<Product> productList;

        public TestProductsController()
        {
            productList = new List<Product>
            {
                new Product{Id="1", Name="Water", Price=10, Currency="DKK", RebateQuantity=2, RebatePercent=10, UpsellProductId="10"},
                new Product{Id="2", Name="Soda", Price=15, Currency="DKK", RebateQuantity=2, RebatePercent=10, UpsellProductId="12"},
            };
        }

        private ProductsController Controller()
        {
            //Mutual Arrange
            var mock = MockIRepositories.GetProductRepository(productList);
            var productController = new ProductsController(mock.Object);
            return productController;
        }

        [Fact]
        public async Task GetAllProducts_onOk()
        {
            //Act
            var actionResult = await Controller().Get();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            IEnumerable<ProductDto> list = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal(2, list.Count());
        }


        [Fact]
        public async Task GetAllProducts_onNotFound()
        {
            //Arrange
            var emptyProductList = new List<Product>();
            var mock = MockIRepositories.GetProductRepository(emptyProductList);
            var Controller = new ProductsController(mock.Object);


            //Act
            var actionResult = await Controller.Get();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("The specified products does not exist!", msg);
        }

        [Theory]
        [InlineData("1")]
        public async Task GetProductById_onOk(string productId)
        {
            //Act
            var actionResult = await Controller().Get(productId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            var product = Assert.IsType<ProductDto>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal(productList[0].Id, product.Id);
        }

        [Theory]
        [InlineData("3")]
        public async Task GetProductById_onNotFound(string productId)
        {
            //Act
            var actionResult = await Controller().Get(productId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("The specified product does not exist!", msg);
        }

        [Fact]
        public async Task UpdateProduct_onOk()
        {
            //Arrange 
            var targetProduct = new ProductDto { Id = "2", Name = "Juice", Price = 20, Currency = "DKK", RebateQuantity = 2, RebatePercent = 10, UpsellProductId = "8" };

            //Act
            var actionResult = await Controller().Update(targetProduct);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal("Product updated successfully!", msg);
        }


        [Fact]
        public async Task UpdateProduct_onNotFound()
        {
            //Arrange
            var targetProduct = new ProductDto { Id = "5", Name = "Icecream", Price = 5, Currency = "DKK", RebateQuantity = 2, RebatePercent = 10, UpsellProductId = "6" };

            //Act
            var actionResult = await Controller().Update(targetProduct);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("Product does not exist!", msg);
        }

        [Fact]
        public async Task UpdateProduct_onBadRequest_NullId()
        {
            //Arrange
            var targetProduct = new ProductDto { Id = null, Name = "Juice", Price = 20, Currency = "DKK", RebateQuantity = 2, RebatePercent = 10 };

            //Act
            var actionResult = await Controller().Update(targetProduct);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("Product ID is required to update the product!", msg);
        }

        [Fact]
        public async Task NewProduct_onBadRequest_IdExist()
        {
            //Arrange 
            var newProduct = new ProductDto { Id = "2", Name = "Juice", Price = 20, Currency = "DKK", RebateQuantity = 2, RebatePercent = 10, UpsellProductId = "8" };

            //Act
            var actionResult = await Controller().Create(newProduct);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("This product id is already in use!", msg);
        }

        [Fact]
        public async Task NewProduct_onBadRequest_NullId()
        {
            //Arrange
            var newProduct = new ProductDto { Id = null, Name = "Banana", Price = 2, Currency = "DKK", RebateQuantity = 10, RebatePercent = 10, UpsellProductId = "20" };

            //Act
            var actionResult = await Controller().Create(newProduct);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("Product ID is required to register the product!", msg);
        }

        [Theory]
        [InlineData("1")]
        public async Task DeleteProduct_onOk(string productId)
        {
            //Act
            var actionResult = await Controller().Delete(productId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal("Product has been deleted!", msg);
            Assert.Single(productList);
        }

        [Theory]
        [InlineData(null)]
        public async Task DeleteProduct_onBadRequest_NullId(string productId)
        {
            //Act
            var actionResult = await Controller().Delete(productId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("Product ID is required to delete the product!", msg);
            Assert.Equal(2, productList.Count);
        }

        [Theory]
        [InlineData("6")]
        public async Task DeleteProduct_onNotFound(string productId)
        {
            //Act
            var actionResult = await Controller().Delete(productId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("Product could not be deleted!", msg);
            Assert.Equal(2, productList.Count);
        }
         */
    }


}
