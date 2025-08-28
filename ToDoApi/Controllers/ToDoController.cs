using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using ToDoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            return await _context.TodoItems.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo(TodoCreateDto dto)
        {
            var todo = new Todo
            {
                Title = dto.Title,
                IsCompleted = dto.IsCompleted,
                CreatedAt = DateTime.UtcNow
            };

            _context.TodoItems.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodos), new { id = todo.Id }, todo);
        }

    }
}
