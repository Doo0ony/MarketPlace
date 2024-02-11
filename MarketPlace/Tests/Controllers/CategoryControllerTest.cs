using MarketPlace.Controllers;
using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace MarketPlace.Tests.Controllers
{
    [TestFixture]
    public class CategoryControllerTests
    {
        [Test]
        public async Task Index_ReturnsViewWithCategories()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Categories.Add(new Category { CategoryId = 101, Name = "Category 1", CategoryThumbnail = "thumbnail1.jpg" });
                context.Categories.Add(new Category { CategoryId = 102, Name = "Category 2", CategoryThumbnail = "thumbnail2.jpg" });
                context.SaveChanges();

                var controller = new CategoryController(context);

                // Act
                var result = await controller.Index() as ViewResult;
                var model = result.Model as List<Category>;

                // Assert
                Assert.NotNull(result);
                Assert.IsNotNull(model);
                Assert.AreEqual(3, model.Count);
            }
        }

        [Test]
        public async Task Details_WithValidId_ReturnsViewWithCategory()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Categories.Add(new Category { CategoryId = 1, Name = "Category 1", CategoryThumbnail = "thumbnail.jpg" });
                context.SaveChanges();

                var controller = new CategoryController(context);

                // Act
                var result = await controller.Details(1) as ViewResult;
                var model = result.Model as Category;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(model);
                Assert.AreEqual("Category 1", model.Name);
            }
        }


        // You can add more tests for other actions similarly
    }
}
