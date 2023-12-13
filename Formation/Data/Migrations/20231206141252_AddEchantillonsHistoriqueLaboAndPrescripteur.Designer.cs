﻿// <auto-generated />
using System;
using Formation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Formation.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231206141252_AddEchantillonsHistoriqueLaboAndPrescripteur")]
    partial class AddEchantillonsHistoriqueLaboAndPrescripteur
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Formation.Data.Entities.EchantillonEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateDeCollecte")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LaboratoireId")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PrescripteurId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LaboratoireId");

                    b.HasIndex("PrescripteurId");

                    b.ToTable("Echantillons");
                });

            modelBuilder.Entity("Formation.Data.Entities.EchantillonHistorique", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("EchantillonId")
                        .HasColumnType("int");

                    b.Property<int>("LaboratoireId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Observations")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Statut")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EchantillonId");

                    b.HasIndex("LaboratoireId");

                    b.ToTable("EchantillonHistoriques");
                });

            modelBuilder.Entity("Formation.Data.Entities.Laboratoire", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adresse")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Laboratoires");
                });

            modelBuilder.Entity("Formation.Data.Entities.Prescripteur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adresse")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Denomination")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Prescripteurs");
                });

            modelBuilder.Entity("Formation.Data.Entities.EchantillonEntity", b =>
                {
                    b.HasOne("Formation.Data.Entities.Laboratoire", "Laboratoire")
                        .WithMany("echantillons")
                        .HasForeignKey("LaboratoireId");

                    b.HasOne("Formation.Data.Entities.Prescripteur", "Prescripteur")
                        .WithMany("Echantillons")
                        .HasForeignKey("PrescripteurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Laboratoire");

                    b.Navigation("Prescripteur");
                });

            modelBuilder.Entity("Formation.Data.Entities.EchantillonHistorique", b =>
                {
                    b.HasOne("Formation.Data.Entities.EchantillonEntity", "Echantillon")
                        .WithMany("echantillonHistoriques")
                        .HasForeignKey("EchantillonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Formation.Data.Entities.Laboratoire", "Laboratoire")
                        .WithMany()
                        .HasForeignKey("LaboratoireId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Echantillon");

                    b.Navigation("Laboratoire");
                });

            modelBuilder.Entity("Formation.Data.Entities.EchantillonEntity", b =>
                {
                    b.Navigation("echantillonHistoriques");
                });

            modelBuilder.Entity("Formation.Data.Entities.Laboratoire", b =>
                {
                    b.Navigation("echantillons");
                });

            modelBuilder.Entity("Formation.Data.Entities.Prescripteur", b =>
                {
                    b.Navigation("Echantillons");
                });
#pragma warning restore 612, 618
        }
    }
}