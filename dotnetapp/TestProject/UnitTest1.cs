using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using dotnetapp.Controllers;
using dotnetapp.Models;
using System.Reflection;

namespace dotnetapp.Tests
{
    [TestFixture]
    public class DeliveryControllerTests
    {
        private DbContextOptions<DeliveryBoyDbContext> _dbContextOptions;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DeliveryBoyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var dbContext = new DeliveryBoyDbContext(_dbContextOptions))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
            using (var dbContext = new DeliveryBoyDbContext(_dbContextOptions))
            {
                // Add test data to the in-memory database
                var delivery = new Delivery
                {
                    DeliveryId = 1,
                    EstablishmentDate = DateTime.Parse("2023-08-30"),
                    OrderId = 1,
                    Status = "Pending"
                };
                dbContext.Deliveries.Add(delivery);
                dbContext.SaveChanges();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var dbContext = new DeliveryBoyDbContext(_dbContextOptions))
            {
                // Clear the in-memory database after each test
                dbContext.Database.EnsureDeleted();
            }
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            using (var dbContext = new DeliveryBoyDbContext(_dbContextOptions))
            {
                // Arrange
                var controller = new DeliveryController(dbContext);

                // Act
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNull(result.ViewName); // Updated assertion
            }
        }

        [Test]
        public void CheckOrders_ReturnsViewResultWithViewModel()
        {
            using (var dbContext = new DeliveryBoyDbContext(_dbContextOptions))
            {
                // Arrange
                // Add test data to dbContext

                var controller = new DeliveryController(dbContext);

                // Act
                var result = controller.CheckOrders() as ViewResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNull(result.ViewName); // Updated assertion

                var viewModel = result.Model as OrderViewModel;
                Assert.IsNotNull(viewModel);
                // Add more assertions on the viewModel and its properties
            }
        }
        [Test]
        public void OrderForm_ValidInput_CreatesOrderAndDelivery()
        {
            using (var dbContext = new DeliveryBoyDbContext(_dbContextOptions))
            {
                // Arrange
                var controller = new OrderController(dbContext);

                // Act
                var result = controller.OrderForm("John", "1234567890", "123 Main St", 100) as ViewResult;

                // Assert
                Assert.IsNotNull(result);

                // Check if the order and delivery have been created in the database
                var order = dbContext.Orders.FirstOrDefault();
                var delivery = dbContext.Deliveries.FirstOrDefault();

                Assert.IsNotNull(order);
                Assert.IsNotNull(delivery);
                Assert.AreEqual("John", order.CustomerName);
                Assert.AreEqual("1234567890", order.ContactNumber);
                Assert.AreEqual("123 Main St", order.Location);
                Assert.AreEqual(100, order.Amount);
                Assert.AreEqual(order.OrderId, delivery.OrderId);
                Assert.AreEqual("Pending", delivery.Status);
            }
        }
        [Test]
        public void OrderClassExists()
        {
            // Arrange
            Type orderType = typeof(Order);

            // Act & Assert
            Assert.IsNotNull(orderType, "Order class not found.");
        }

        [Test]
        public void DeliveryClassExists()
        {
            // Arrange
            Type deliveryType = typeof(Delivery);

            // Act & Assert
            Assert.IsNotNull(deliveryType, "Delivery class not found.");
        }
        [Test]
        public void DeliveryBoyDbContextContainsDbSetOrderProperty()
        {
            using (var dbContext = new DeliveryBoyDbContext(_dbContextOptions))
            {
                var propertyInfo = dbContext.GetType().GetProperty("Orders");

                Assert.IsNotNull(propertyInfo);
                Assert.AreEqual(typeof(DbSet<Order>), propertyInfo.PropertyType);
            }
        }

        [Test]
        public void DeliveryBoyDbContextContainsDbSetDeliveryProperty()
        {
            using (var dbContext = new DeliveryBoyDbContext(_dbContextOptions))
            {
                var propertyInfo = dbContext.GetType().GetProperty("Deliveries");

                Assert.IsNotNull(propertyInfo);
                Assert.AreEqual(typeof(DbSet<Delivery>), propertyInfo.PropertyType);
            }
        }
        [Test]
        public void Order_Properties_OrderId_ReturnExpectedDataTypes()
        {
            Order order = new Order();
            PropertyInfo propertyInfo = order.GetType().GetProperty("OrderId");

            Assert.IsNotNull(propertyInfo, "OrderId property not found.");
            Assert.AreEqual(typeof(int), propertyInfo.PropertyType, "OrderId property type is not int.");
        }

        [Test]
        public void Order_Properties_CustomerName_ReturnExpectedDataTypes()
        {
            Order order = new Order();
            PropertyInfo propertyInfo = order.GetType().GetProperty("CustomerName");

            Assert.IsNotNull(propertyInfo, "CustomerName property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "CustomerName property type is not string.");
        }

        [Test]
        public void Order_Properties_ContactNumber_ReturnExpectedDataTypes()
        {
            Order order = new Order();
            PropertyInfo propertyInfo = order.GetType().GetProperty("ContactNumber");

            Assert.IsNotNull(propertyInfo, "ContactNumber property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "ContactNumber property type is not string.");
        }

        [Test]
        public void Order_Properties_Location_ReturnExpectedDataTypes()
        {
            Order order = new Order();
            PropertyInfo propertyInfo = order.GetType().GetProperty("Location");

            Assert.IsNotNull(propertyInfo, "Location property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "Location property type is not string.");
        }

        [Test]
        public void Order_Properties_Amount_ReturnExpectedDataTypes()
        {
            Order order = new Order();
            PropertyInfo propertyInfo = order.GetType().GetProperty("Amount");

            Assert.IsNotNull(propertyInfo, "Amount property not found.");
            Assert.AreEqual(typeof(int), propertyInfo.PropertyType, "Amount property type is not int.");
        }

        [Test]
        public void Order_Properties_Delivery_ReturnExpectedDataTypes()
        {
            Order order = new Order();
            PropertyInfo propertyInfo = order.GetType().GetProperty("delivery");

            Assert.IsNotNull(propertyInfo, "delivery property not found.");
            Assert.AreEqual(typeof(Delivery), propertyInfo.PropertyType, "delivery property type is not Delivery.");
        }
        [Test]
        public void Delivery_Properties_DeliveryId_ReturnExpectedDataTypes()
        {
            Delivery delivery = new Delivery();
            PropertyInfo propertyInfo = delivery.GetType().GetProperty("DeliveryId");

            Assert.IsNotNull(propertyInfo, "DeliveryId property not found.");
            Assert.AreEqual(typeof(int), propertyInfo.PropertyType, "DeliveryId property type is not int.");
        }

        [Test]
        public void Delivery_Properties_EstablishmentDate_ReturnExpectedDataTypes()
        {
            Delivery delivery = new Delivery();
            PropertyInfo propertyInfo = delivery.GetType().GetProperty("EstablishmentDate");

            Assert.IsNotNull(propertyInfo, "EstablishmentDate property not found.");
            Assert.AreEqual(typeof(DateTime), propertyInfo.PropertyType, "EstablishmentDate property type is not DateTime.");
        }

        [Test]
        public void Delivery_Properties_OrderId_ReturnExpectedDataTypes()
        {
            Delivery delivery = new Delivery();
            PropertyInfo propertyInfo = delivery.GetType().GetProperty("OrderId");

            Assert.IsNotNull(propertyInfo, "OrderId property not found.");
            Assert.AreEqual(typeof(int), propertyInfo.PropertyType, "OrderId property type is not int.");
        }

        [Test]
        public void Delivery_Properties_Status_ReturnExpectedDataTypes()
        {
            Delivery delivery = new Delivery();
            PropertyInfo propertyInfo = delivery.GetType().GetProperty("Status");

            Assert.IsNotNull(propertyInfo, "Status property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "Status property type is not string.");
        }
        [Test]
        public void Order_Delivery_ReturnsExpectedValue()
        {
            // Arrange
            Delivery expectedDelivery = new Delivery { DeliveryId = 3 };
            Order order = new Order { delivery = expectedDelivery };

            // Assert
            Assert.AreEqual(expectedDelivery, order.delivery);
        }

        [Test]
        public void Delivery_Order_ReturnsExpectedValue()
        {
            // Arrange
            Order expectedOrder = new Order { OrderId = 4 };
            Delivery delivery = new Delivery { OrderId = expectedOrder.OrderId };

            // Assert
            Assert.AreEqual(expectedOrder.OrderId, delivery.OrderId);
        }
        [Test]
        public void OrderViewModel_Properties_InitializedCorrectly()
        {
            // Arrange
            var viewModel = new OrderViewModel();

            // Act & Assert
            Assert.IsNull(viewModel.Orders);
            Assert.AreEqual(0, viewModel.PendingCount);
            Assert.AreEqual(0, viewModel.DeliveredCount);
            Assert.AreEqual(0, viewModel.OutForDeliveryCount);
            Assert.AreEqual(0, viewModel.InTransitCount);
        }

        [Test]
        public void OrderViewModel_Properties_SetAndGetCorrectly()
        {
            // Arrange
            var viewModel = new OrderViewModel();

            // Act
            viewModel.Orders = new List<Order>
            {
                new Order(),
                new Order()
            };
            viewModel.PendingCount = 2;
            viewModel.DeliveredCount = 1;
            viewModel.OutForDeliveryCount = 0;
            viewModel.InTransitCount = 1;

            // Assert
            Assert.IsNotNull(viewModel.Orders);
            Assert.AreEqual(2, viewModel.Orders.Count);
            Assert.AreEqual(2, viewModel.PendingCount);
            Assert.AreEqual(1, viewModel.DeliveredCount);
            Assert.AreEqual(0, viewModel.OutForDeliveryCount);
            Assert.AreEqual(1, viewModel.InTransitCount);
        }
        [Test]
        public void OrderViewModelClassExists()
        {
            // Arrange
            Type orderViewModelType = typeof(OrderViewModel);

            // Act & Assert
            Assert.IsNotNull(orderViewModelType, "OrderViewModel class not found.");
        }
        [Test]
        public void OrderViewModel_Properties_PendingCount_ReturnExpectedDataTypes()
        {
            OrderViewModel orderViewModel = new OrderViewModel();
            PropertyInfo propertyInfo = orderViewModel.GetType().GetProperty("PendingCount");

            Assert.IsNotNull(propertyInfo, "PendingCount property not found.");
            Assert.AreEqual(typeof(int), propertyInfo.PropertyType, "PendingCount property type is not int.");
        }

        [Test]
        public void OrderViewModel_Properties_DeliveredCount_ReturnExpectedDataTypes()
        {
            OrderViewModel orderViewModel = new OrderViewModel();
            PropertyInfo propertyInfo = orderViewModel.GetType().GetProperty("DeliveredCount");

            Assert.IsNotNull(propertyInfo, "DeliveredCount property not found.");
            Assert.AreEqual(typeof(int), propertyInfo.PropertyType, "DeliveredCount property type is not int.");
        }

        [Test]
        public void OrderViewModel_Properties_OutForDeliveryCount_ReturnExpectedDataTypes()
        {
            OrderViewModel orderViewModel = new OrderViewModel();
            PropertyInfo propertyInfo = orderViewModel.GetType().GetProperty("OutForDeliveryCount");

            Assert.IsNotNull(propertyInfo, "OutForDeliveryCount property not found.");
            Assert.AreEqual(typeof(int), propertyInfo.PropertyType, "OutForDeliveryCount property type is not int.");
        }

        [Test]
        public void OrderViewModel_Properties_InTransitCount_ReturnExpectedDataTypes()
        {
            OrderViewModel orderViewModel = new OrderViewModel();
            PropertyInfo propertyInfo = orderViewModel.GetType().GetProperty("InTransitCount");

            Assert.IsNotNull(propertyInfo, "InTransitCount property not found.");
            Assert.AreEqual(typeof(int), propertyInfo.PropertyType, "InTransitCount property type is not int.");
        }
        [Test]
        public void CheckOrders_ReturnsExpectedViewModelWithCounts()
        {
            // Arrange
            using (var dbContext = new DeliveryBoyDbContext(_dbContextOptions))
            {
                var orders = new List<Order>
                {
                    new Order { CustomerName="c",ContactNumber ="12345",Location ="Chennai",Amount=100,delivery = new Delivery {EstablishmentDate = DateTime.Now,OrderId=1, Status = "Pending" } },
                    new Order { CustomerName="c",ContactNumber ="12345",Location ="Chennai",Amount=100,delivery = new Delivery { EstablishmentDate = DateTime.Now,OrderId=1,Status = "Delivered" } },
                    new Order { CustomerName="c",ContactNumber ="12345",Location ="Chennai",Amount=100,delivery = new Delivery { EstablishmentDate = DateTime.Now,OrderId=1,Status = "Out for Delivery" } },
                    new Order { CustomerName="c",ContactNumber ="12345",Location ="Chennai",Amount=100,delivery = new Delivery { EstablishmentDate = DateTime.Now,OrderId=1,Status = "In Transit" } }
                };
                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();

                var controller = new DeliveryController(dbContext);

                // Act
                var result = controller.CheckOrders() as ViewResult;
                var viewModel = result.Model as OrderViewModel;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(viewModel);

                Assert.AreEqual(1, viewModel.PendingCount);
                Assert.AreEqual(1, viewModel.DeliveredCount);
                Assert.AreEqual(1, viewModel.OutForDeliveryCount);
                Assert.AreEqual(1, viewModel.InTransitCount);
            }
        }
        [Test]
        public void UpdateStatus_InvalidOrder_ReturnsJsonFailure()
        {
            using (var dbContext = new DeliveryBoyDbContext(_dbContextOptions))
            {
                var controller = new DeliveryController(dbContext);

                var result = controller.UpdateStatus(999, "Delivered") as JsonResult;

                Assert.IsNotNull(result);
                Assert.IsFalse(result.Value is IDictionary<string, object> data && (bool)data["success"]);
            }
        }

    }
}