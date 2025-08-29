using Microsoft.AspNetCore.Mvc;
using ToDoApi.Data;
using ToDoApi.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using TodoApi.Controllers;
using TodoApi.Data;
using TodoApi.Models;

namespace ToDoApiTest
{
    public class ToDoControllerTests
    {
        private readonly AppDbContext _context;
        private readonly TodoController _controller;

        public ToDoControllerTests()
        {
            // InMemory veritabaný kullanýyoruz
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TodoTestDb")
                .Options;

            _context = new AppDbContext(options);
            _controller = new TodoController(_context);
        }

        // Veritabanýnda hiç Todo yokken, GetTodos() çaðrýldýðýnda boþ liste dönüp dönmediði test ediliyor.
        [Fact]
        public async Task GetTodos_ReturnsEmpty_WhenNoTodos()
        {
            // Act
            var result = await _controller.GetTodos();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Todo>>>(result);
            var todos = Assert.IsAssignableFrom<IEnumerable<Todo>>(actionResult.Value);
            Assert.Empty(todos);
        }

        // Yeni bir Todo oluþturma isteði gönderdiðimizde, bu Todo’nun veritabanýna eklendiðini ve doðru response döndüðünü test ediyoruz.
        [Fact]
        public async Task Create_AddsNewTodo()
        {
            // Arrange
            var todoDto = new TodoCreateDto { Title = "Test Todo", IsCompleted = false };

            // Act
            var result = await _controller.CreateTodo(todoDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdTodo = Assert.IsType<Todo>(createdAtActionResult.Value);

            Assert.Equal("Test Todo", createdTodo.Title);
            Assert.False(createdTodo.IsCompleted);
        }

    }
}
