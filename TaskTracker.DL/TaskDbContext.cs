using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DL.Models;

namespace TaskTracker.DL
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext()
        {

        }
        public TaskDbContext(DbContextOptions options) : base(options) { }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Activity> Activities { get; set; }

    }
}
