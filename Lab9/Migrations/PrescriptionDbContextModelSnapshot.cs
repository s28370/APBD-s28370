﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APBD9.Migrations
{
    [DbContext(typeof(PrescriptionDbContext))]
    partial class PrescriptionDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.19")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("APBD9.Models.Doctor", b =>
                {
                    b.Property<int>("IdDoctor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDoctor"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdDoctor");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("APBD9.Models.Medicament", b =>
                {
                    b.Property<int>("IdMedicament")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdMedicament"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdMedicament");

                    b.ToTable("Medicaments");
                });

            modelBuilder.Entity("APBD9.Models.Patient", b =>
                {
                    b.Property<int>("IdPatient")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPatient"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdPatient");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("APBD9.Models.Prescription", b =>
                {
                    b.Property<int>("IdPrescription")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPrescription"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoctorIdDoctor")
                        .HasColumnType("int");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PatientIdPatient")
                        .HasColumnType("int");

                    b.HasKey("IdPrescription");

                    b.HasIndex("DoctorIdDoctor");

                    b.HasIndex("PatientIdPatient");

                    b.ToTable("Prescriptions");
                });

            modelBuilder.Entity("APBD9.Models.PrescriptionMedicament", b =>
                {
                    b.Property<int>("IdPrescription")
                        .HasColumnType("int");

                    b.Property<int>("IdMedicament")
                        .HasColumnType("int");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Dose")
                        .HasColumnType("int");

                    b.HasKey("IdPrescription", "IdMedicament");

                    b.HasIndex("IdMedicament");

                    b.ToTable("PrescriptionMedicaments");
                });

            modelBuilder.Entity("APBD9.Models.Prescription", b =>
                {
                    b.HasOne("APBD9.Models.Doctor", "Doctor")
                        .WithMany()
                        .HasForeignKey("DoctorIdDoctor")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APBD9.Models.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientIdPatient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("APBD9.Models.PrescriptionMedicament", b =>
                {
                    b.HasOne("APBD9.Models.Medicament", "Medicament")
                        .WithMany("PrescriptionMedicaments")
                        .HasForeignKey("IdMedicament")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APBD9.Models.Prescription", "Prescription")
                        .WithMany("PrescriptionMedicaments")
                        .HasForeignKey("IdPrescription")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicament");

                    b.Navigation("Prescription");
                });

            modelBuilder.Entity("APBD9.Models.Medicament", b =>
                {
                    b.Navigation("PrescriptionMedicaments");
                });

            modelBuilder.Entity("APBD9.Models.Prescription", b =>
                {
                    b.Navigation("PrescriptionMedicaments");
                });
#pragma warning restore 612, 618
        }
    }
}
