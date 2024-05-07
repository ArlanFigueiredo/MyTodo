using MeuTodo.Data;
using MeuTodo.Models;
using MeuTodo.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MeuTodo.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase {

        [HttpGet("/todos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context) {
            var todos = await context.Todos.AsNoTracking().ToListAsync();
            return Ok(todos);
        }

        //
        //
        //

        [HttpGet("/todos/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id) {
            var todos = await context.Todos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return todos == null ? NotFound("Usuario não encontrado.") : Ok();
        }

        //
        //
        //

        [HttpPost("/todos")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] CreateTodoViewModel model) {

            if(!ModelState.IsValid) {
                return BadRequest();
            }

            var todo = new Todo() {
                Date = DateTime.Now.ToLocalTime(),
                Done = true,
                Title = model.Title,
            };
            try {
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
                return Created($"/todos/{todo.Id}", todo);
            }catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        //
        //
        //

        [HttpPut("/todos/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] CreateTodoViewModel model, [FromRoute] int id) {

            if (!ModelState.IsValid) {
                return BadRequest("Usuario não encontrado.");
            }

            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if(todo == null) { 
                return NotFound("Usuario não encontrado");
            }
            
            try {
                todo.Title = model.Title;
                context.Todos.Update(todo);
                await context.SaveChangesAsync();
                return Ok(todo);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        //
        //
        //


        [HttpDelete("/todos/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, [FromRoute] int id) {

           

            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (todo == null) {
                return NotFound("Usuario não encontrado");
            }

            try {

                context.Todos.Remove(todo);
                await context.SaveChangesAsync();
                return Ok("Usuario atualizado com sucesso!");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
