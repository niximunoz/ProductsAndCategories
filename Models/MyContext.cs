#pragma warning disable CS8618

using Microsoft.EntityFrameworkCore;
namespace ProductsAndCategories.Models;


public class MyContext : DbContext 
{   
    public DbSet<Product> Products { get; set; } 
    public DbSet<Category> Categories { get; set; } 
    public DbSet<Association> Associations { get; set; } 

    
    public MyContext(DbContextOptions options) : base(options) { }    


    
}
