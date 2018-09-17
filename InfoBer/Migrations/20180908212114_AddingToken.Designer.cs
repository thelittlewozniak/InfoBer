﻿// <auto-generated />
using System;
using InfoBer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InfoBer.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20180908212114_AddingToken")]
    partial class AddingToken
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InfoBer.Models.Departement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int?>("SchoolId");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId");

                    b.ToTable("Departements");
                });

            modelBuilder.Entity("InfoBer.Models.Problem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ProblemTitle");

                    b.Property<int?>("TakerId");

                    b.Property<int?>("WriterId");

                    b.Property<bool>("status");

                    b.HasKey("Id");

                    b.HasIndex("TakerId");

                    b.HasIndex("WriterId");

                    b.ToTable("Problems");
                });

            modelBuilder.Entity("InfoBer.Models.School", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adress");

                    b.Property<string>("Director");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("InfoBer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DepartementId");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<double>("Rating");

                    b.Property<int?>("SchoolId");

                    b.Property<string>("Surname");

                    b.Property<string>("Token");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("DepartementId");

                    b.HasIndex("SchoolId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("InfoBer.Models.Departement", b =>
                {
                    b.HasOne("InfoBer.Models.School", "School")
                        .WithMany("Departements")
                        .HasForeignKey("SchoolId");
                });

            modelBuilder.Entity("InfoBer.Models.Problem", b =>
                {
                    b.HasOne("InfoBer.Models.User", "Taker")
                        .WithMany()
                        .HasForeignKey("TakerId");

                    b.HasOne("InfoBer.Models.User", "Writer")
                        .WithMany()
                        .HasForeignKey("WriterId");
                });

            modelBuilder.Entity("InfoBer.Models.User", b =>
                {
                    b.HasOne("InfoBer.Models.Departement", "Departement")
                        .WithMany()
                        .HasForeignKey("DepartementId");

                    b.HasOne("InfoBer.Models.School", "School")
                        .WithMany()
                        .HasForeignKey("SchoolId");
                });
#pragma warning restore 612, 618
        }
    }
}
