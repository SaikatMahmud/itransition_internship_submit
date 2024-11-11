using DrawingBoard.Models;
using Microsoft.EntityFrameworkCore;

namespace DrawingBoard
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Board> Boards { get; set; }

    }
}
