using ContactApp.Api.Controllers.Contact;
using ContactApp.Api.Controllers.Contact.ControllerService;
using ContactApp.Api.Controllers.Contact.Models;
using ContactApp.Models.ApiModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace ContactApp.Api.Tests
{
    public class ContactControllerTests
    {
        private readonly ContactDto _createContactDto = new()
        {
            Name = "Adin",
            PhoneNumber = "062-333-222",
            Email = "example@gmail.com",
            Address = "Mostar"
        };

        [Fact]
        public async Task CreateContact_ReturnsOkResult()
        {
            // Arrange
            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
                }))
            };

            var contactService = new Mock<IContactControllerService>();
            contactService.Setup(x => x.CreateContactAsync(It.IsAny<ContactDto>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            var controller = new ContactController(contactService.Object);
            controller.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await controller.CreateContact(_createContactDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var ContactApiResponse = Assert.IsType<ContactApiResponse>(okResult.Value);
            Assert.Equal("New contact successfully created", ContactApiResponse.Message);
        }
    }
}