using MarketPlace.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace MarketPlace.Tests.Controllers
{
    public class AppRolesControllerTests
    {
        [Test]
        public async Task Index_ReturnsViewWithRoles()
        {
            // Arrange
            var roles = new List<IdentityRole>
            {
                new IdentityRole("Admin"),
                new IdentityRole("User")
            };

            var mockStore = new Mock<IQueryableRoleStore<IdentityRole>>();
            var mockRoleManager = new Mock<RoleManager<IdentityRole>>(mockStore.Object, null, null, null, null);

            mockRoleManager.Setup(m => m.Roles).Returns(roles.AsQueryable());

            var controller = new AppRolesController(mockRoleManager.Object);

            // Act
            var result = controller.Index() as ViewResult;
            var model = result?.Model as IEnumerable<IdentityRole>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count());
        }


        [Test]
        public async Task Create_Post_ReturnsRedirectToIndex()
        {
            // Arrange
            var role = new IdentityRole("TestRole");

            var mockStore = new Mock<IQueryableRoleStore<IdentityRole>>();
            var mockRoleManager = new Mock<RoleManager<IdentityRole>>(mockStore.Object, null, null, null, null);

            mockRoleManager.Setup(m => m.RoleExistsAsync(role.Name)).ReturnsAsync(false);
            mockRoleManager.Setup(m => m.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

            var controller = new AppRolesController(mockRoleManager.Object);

            // Act
            var result = await controller.Create(role) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }
    }
}
