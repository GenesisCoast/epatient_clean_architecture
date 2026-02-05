using System;
using System.Collections.Generic;
using System.Text;
using ePatient.Domain.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ePatient.Infrastructure.Persistence.Patients;

/// <summary>
/// Entity type configuration for <see cref="Patient"/>.
/// </summary>
internal class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.DateOfBirth)
            .IsRequired();

        builder.Property(e => e.MedicalRecordNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(e => e.MedicalRecordNumber)
            .IsUnique();

        builder.Property(e => e.Email)
            .HasMaxLength(255);

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20);
    }
}
