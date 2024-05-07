using MeuTodo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MeuTodo.Data {
    // DbContext representação do nosso banco
    public class AppDbContext : DbContext {

        // REPRESENTA NOSSA TABELA DE TAREFAS DO BANCO (Todo será a representação)
        protected readonly IConfiguration Configuration;
        
        public AppDbContext(IConfiguration configuration) {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<Todo> Todos { get; set; }

    }
}
