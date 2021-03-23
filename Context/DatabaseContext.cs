using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicCoreApplication.Context
{
	public class DatabaseContext : DbContext
	{
		//it requires to install the using Microsoft.EntityFrameworkCore package
		public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options)
		{

		}

        //define all the database models
        public DbSet<Employee> Employees { get; set; }

        //perform the seeding to insert the default value into the database on apply the migration 
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<Employee>().ToTable("Employees"); //initialize the employee table

            //insert the dummy entry to the tables
            modelBuilder.Entity<Employee>().HasData(new Employee
            {
                EmployeeId = 1,
                FirstName = "Uncle",
                LastName = "Bob",
                Email = "uncle.bob@gmail.com",
                DateOfBirth = new DateTime(1979, 04, 25),
                PhoneNumber = "999-888-7777",
                Password = "Systems@123"
            }, new Employee
            {
                EmployeeId = 2,
                FirstName = "Jan",
                LastName = "Kirsten",
                Email = "jan.kirsten@gmail.com",
                DateOfBirth = new DateTime(1981, 07, 13),
                PhoneNumber = "111-222-3333",
                Password = "Systems@123"
            });
            base.OnModelCreating(modelBuilder);
		}
	}
}
