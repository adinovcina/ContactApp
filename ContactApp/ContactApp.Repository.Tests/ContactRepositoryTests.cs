using AutoMapper;
using ContactApp.Data.EF.EfCore;
using ContactApp.Data.EF.Repositories.Contact;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ContactApp.Repository.Tests
{
    public class ContactRepositoryTests
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;
        private readonly ContactRepository _repository;

        public ContactRepositoryTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Models.Contact, Data.EF.Entities.Contact>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "MemoryDatabase")
                .Options;
            _context = new ApplicationContext(options);

            _repository = new ContactRepository(_mapper, _context);
        }

        [Fact]
        public async Task CreateContactAsync_AddsContactToDatabase()
        {
            // Arrange
            var model = new Models.Contact { Name = "John Doe", Email = "johndoe@gmail.com" };

            // Act
            await _repository.CreateContactAsync(model);

            // Assert
            var entity = await _context.Contacts.FirstOrDefaultAsync(c => c.Name == "John Doe");
            Assert.NotNull(entity);
            Assert.Equal("John Doe", entity.Name);
        }

        [Fact]
        public async Task EditContactAsync_WhenCalled_UpdatesContact()
        {
            // Arrange
            var contactId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var entity = new Data.EF.Entities.Contact
            {
                Id = contactId,
                UserId = userId,
                Name = "John",
            };
            await _context.Contacts.AddAsync(entity);
            await _context.SaveChangesAsync();

            var model = _mapper.Map<Models.Contact>(entity);
            model.Name = "Jane";

            // Act
            var result = await _repository.EditContactAsync(model);

            // Assert
            Assert.Equal("Jane", result.Name);
        }

        [Fact]
        public async Task EditContactAsync_WhenContactNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var contactId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var model = new Models.Contact
            {
                Id = contactId,
                UserId = userId,
                Name = "Jane",
            };

            // Act
            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.EditContactAsync(model));
        }

        [Fact]
        public async Task EditContactAsync_WhenUserIsNotOwner_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var contactId = Guid.NewGuid();
            var entity = new Data.EF.Entities.Contact
            {
                Id = contactId,
                UserId = Guid.NewGuid(),
                Name = "John",
            };
            await _context.Contacts.AddAsync(entity);
            await _context.SaveChangesAsync();

            var model = _mapper.Map<Models.Contact>(entity);
            model.UserId = Guid.NewGuid();

            // Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _repository.EditContactAsync(model));
        }

        [Fact]
        public async Task DeleteContactAsync_RemovesContactFromDatabase()
        {
            // Arrange
            var contact = new Data.EF.Entities.Contact
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                UserId = Guid.NewGuid()
            };
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteContactAsync(contact.Id, contact.UserId);

            // Assert
            var entity = await _context.Contacts.FindAsync(contact.Id);
            Assert.Null(entity);
        }

        [Fact]
        public async Task DeleteContactAsync_ThrowsKeyNotFoundException_WhenContactNotFound()
        {
            // Arrange
            var contactId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Act
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _repository.DeleteContactAsync(contactId, userId));

            // Assert
            Assert.Equal("Entity not found", ex.Message);
        }

        [Fact]
        public async Task DeleteContactAsync_ThrowsUnauthorizedAccessException_WhenUserIsNotOwner()
        {
            // Arrange
            var contact = new Data.EF.Entities.Contact
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                UserId = Guid.NewGuid()
            };
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();

            // Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _repository.DeleteContactAsync(contact.Id, Guid.NewGuid()));

            // Assert
            Assert.Equal("User is not owner of contact", ex.Message);
        }
    }
}