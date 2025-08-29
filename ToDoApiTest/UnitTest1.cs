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
            // InMemory veritaban� kullan�yoruz
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TodoTestDb")
                .Options;

            _context = new AppDbContext(options);
            _controller = new TodoController(_context);
        }

        // Veritaban�nda hi� Todo yokken, GetTodos() �a�r�ld���nda bo� liste d�n�p d�nmedi�i test ediliyor.
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

        // Yeni bir Todo olu�turma iste�i g�nderdi�imizde, bu Todo�nun veritaban�na eklendi�ini ve do�ru response d�nd���n� test ediyoruz.
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
