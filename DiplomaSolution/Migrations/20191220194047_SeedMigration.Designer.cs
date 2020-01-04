using DiplomaSolution.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomaSolution.Migrations
{
    [DbContext(typeof(CustomerContext))]
    [Migration("20191220194047_SeedMigration")]
    partial class SeedMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099");

            modelBuilder.Entity("DiplomaSolution.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<int?>("FileId");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Customers");

                    b.HasData(
                        new { Id = 10, EmailAddress = "sss", FileId = 1, FirstName = "Yebvhen", LastName = "Havrasiienko", Password = "1223" }
                    );
                });

            modelBuilder.Entity("DiplomaSolution.Models.FormFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("FileData");

                    b.Property<string>("FullName");

                    b.HasKey("Id");

                    b.ToTable("CustomerFiles");
                });
#pragma warning restore 612, 618
        }
    }
}
