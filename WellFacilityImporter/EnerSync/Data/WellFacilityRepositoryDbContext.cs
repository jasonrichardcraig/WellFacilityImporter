using System;
using Microsoft.EntityFrameworkCore;

// Alias namespaces to prevent type collisions
using AlbertaTownshipSystemModels = EnerSync.Models.AlbertaTownshipSystem;
using BusinessAssociateModels = EnerSync.Models.BusinessAssociate;
using FacilityApprovalsDailyModels = EnerSync.Models.FacilityApprovalsDaily;
using FacilityInfrastructureModels = EnerSync.Models.FacilityInfrastructure;
using FacilityLicenceModels = EnerSync.Models.FacilityLicence;
using FacilityOperatorHistoryModels = EnerSync.Models.FacilityOperatorHistory;
using WellFacilityLinkModels = EnerSync.Models.WellFacilityLink;
using WellInfrastructureModels = EnerSync.Models.WellInfrastructure;
using WellLicenceModels = EnerSync.Models.WellLicence;
using WellWikiModels = EnerSync.Models.WellWiki;

namespace EnerSync.Data
{
    public partial class WellFacilityRepositoryDbContext : DbContext
    {
        public WellFacilityRepositoryDbContext()
        {
        }

        public WellFacilityRepositoryDbContext(DbContextOptions<WellFacilityRepositoryDbContext> options)
            : base(options)
        {
        }

        // AlbertaTownshipSystem Entities
        public virtual DbSet<AlbertaTownshipSystemModels.Coordinate> Coordinates { get; set; }
        public virtual DbSet<AlbertaTownshipSystemModels.ElevationAccuracyLookup> ElevationAccuracyLookups { get; set; }
        public virtual DbSet<AlbertaTownshipSystemModels.ElevationMethodLookup> ElevationMethodLookups { get; set; }
        public virtual DbSet<AlbertaTownshipSystemModels.HorizontalClassificationLookup> HorizontalClassificationLookups { get; set; }
        public virtual DbSet<AlbertaTownshipSystemModels.HorizontalDatumLookup> HorizontalDatumLookups { get; set; }
        public virtual DbSet<AlbertaTownshipSystemModels.MeridianLookup> MeridianLookups { get; set; }
        public virtual DbSet<AlbertaTownshipSystemModels.RoadAllowanceCodeLookup> RoadAllowanceCodeLookups { get; set; }
        public virtual DbSet<AlbertaTownshipSystemModels.StationCodeLookup> StationCodeLookups { get; set; }
        public virtual DbSet<AlbertaTownshipSystemModels.StatusCodeLookup> StatusCodeLookups { get; set; }
        public virtual DbSet<AlbertaTownshipSystemModels.VerticalDatumLookup> VerticalDatumLookups { get; set; }

        // BusinessAssociate Entities
        public virtual DbSet<BusinessAssociateModels.BusinessAssociate> BusinessAssociates { get; set; }

        // FacilityApprovalsDaily Entities
        public virtual DbSet<FacilityApprovalsDailyModels.FacilityLicence> FacilityLicences { get; set; }

        // FacilityInfrastructure Entities
        public virtual DbSet<FacilityInfrastructureModels.Facility> Facilities { get; set; }

        // FacilityLicence Entities
        public virtual DbSet<FacilityLicenceModels.Licence> Licences { get; set; }

        // FacilityOperatorHistory Entities
        public virtual DbSet<FacilityOperatorHistoryModels.OperatorHistory> OperatorHistories { get; set; }

        // WellFacilityLink Entities
        public virtual DbSet<WellFacilityLinkModels.LinkedFacility> LinkedFacilities { get; set; }
        public virtual DbSet<WellFacilityLinkModels.WellFacilityLink> WellFacilityLinks { get; set; }

        // WellInfrastructure Entities
        public virtual DbSet<WellInfrastructureModels.CommingledWell> CommingledWells { get; set; }
        public virtual DbSet<WellInfrastructureModels.Well> WellsInfrastructure { get; set; }

        // WellLicence Entities
        public virtual DbSet<WellLicenceModels.Well> WellsLicence { get; set; }

        // WellWiki Entities
        public virtual DbSet<WellWikiModels.Well> WellsWiki { get; set; }
        public virtual DbSet<WellWikiModels.WellDirectionalDrilling> WellDirectionalDrillings { get; set; }
        public virtual DbSet<WellWikiModels.WellHistory> WellHistories { get; set; }
        public virtual DbSet<WellWikiModels.WellPerforationTreatment> WellPerforationTreatments { get; set; }
        public virtual DbSet<WellWikiModels.WellProductionDatum> WellProductionData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#warning To protect potentially sensitive information in your connection string, move it out of source code. Consider using the Name= syntax to read it from configuration.

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=WellFacilityRepository;TrustServerCertificate=True;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // AlbertaTownshipSystem Configuration
            modelBuilder.Entity<AlbertaTownshipSystemModels.Coordinate>(entity =>
            {
                entity.ToTable("Coordinates", "AlbertaTownshipSystem");

                entity.HasIndex(e => new { e.Meridian, e.Range, e.Township, e.Section, e.QuarterSection }, "IX_Coordinates").IsUnique();

                entity.HasIndex(e => new { e.Latitude, e.Longitude }, "IX_Coordinates_1");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CommentField).HasMaxLength(255);
                entity.Property(e => e.ElevationAccuracy)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.ElevationMethod)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.ElevationOrigin)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.HorizontalClassification)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.HorizontalDatum)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.HorizontalMethod)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.HorizontalOrigin)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.QuarterSection).HasMaxLength(10);
                entity.Property(e => e.RoadAllowanceCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.StationCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.StatusCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
                entity.Property(e => e.VerticalDatum)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.ElevationAccuracyNavigation).WithMany(p => p.Coordinates)
                    .HasForeignKey(d => d.ElevationAccuracy)
                    .HasConstraintName("FK__Coordinat__Eleva__5006DFF2");

                entity.HasOne(d => d.ElevationMethodNavigation).WithMany(p => p.Coordinates)
                    .HasForeignKey(d => d.ElevationMethod)
                    .HasConstraintName("FK__Coordinat__Eleva__4F12BBB9");

                entity.HasOne(d => d.HorizontalClassificationNavigation).WithMany(p => p.Coordinates)
                    .HasForeignKey(d => d.HorizontalClassification)
                    .HasConstraintName("FK__Coordinat__Horiz__4C364F0E");

                entity.HasOne(d => d.HorizontalDatumNavigation).WithMany(p => p.Coordinates)
                    .HasForeignKey(d => d.HorizontalDatum)
                    .HasConstraintName("FK__Coordinat__Horiz__4D2A7347");

                entity.HasOne(d => d.MeridianNavigation).WithMany(p => p.Coordinates)
                    .HasForeignKey(d => d.Meridian)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Coordinat__Merid__4959E263");

                entity.HasOne(d => d.RoadAllowanceCodeNavigation).WithMany(p => p.Coordinates)
                    .HasForeignKey(d => d.RoadAllowanceCode)
                    .HasConstraintName("FK__Coordinat__RoadA__4E1E9780");

                entity.HasOne(d => d.StationCodeNavigation).WithMany(p => p.Coordinates)
                    .HasForeignKey(d => d.StationCode)
                    .HasConstraintName("FK__Coordinat__Stati__4A4E069C");

                entity.HasOne(d => d.StatusCodeNavigation).WithMany(p => p.Coordinates)
                    .HasForeignKey(d => d.StatusCode)
                    .HasConstraintName("FK__Coordinat__Statu__4B422AD5");

                entity.HasOne(d => d.VerticalDatumNavigation).WithMany(p => p.Coordinates)
                    .HasForeignKey(d => d.VerticalDatum)
                    .HasConstraintName("FK__Coordinat__Verti__50FB042B");
            });

            modelBuilder.Entity<AlbertaTownshipSystemModels.ElevationAccuracyLookup>(entity =>
            {
                entity.HasKey(e => e.ElevationAccuracy).HasName("PK__Elevatio__EF51D1BC972F2369");

                entity.ToTable("ElevationAccuracyLookup", "AlbertaTownshipSystem");

                entity.Property(e => e.ElevationAccuracy)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Description).HasMaxLength(50);
            });

            modelBuilder.Entity<AlbertaTownshipSystemModels.ElevationMethodLookup>(entity =>
            {
                entity.HasKey(e => e.ElevationMethod).HasName("PK__Elevatio__A502BC84E0B58C04");

                entity.ToTable("ElevationMethodLookup", "AlbertaTownshipSystem");

                entity.Property(e => e.ElevationMethod)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<AlbertaTownshipSystemModels.HorizontalClassificationLookup>(entity =>
            {
                entity.HasKey(e => e.HorizontalClassification).HasName("PK__Horizont__6FA857392013E23C");

                entity.ToTable("HorizontalClassificationLookup", "AlbertaTownshipSystem");

                entity.Property(e => e.HorizontalClassification)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<AlbertaTownshipSystemModels.HorizontalDatumLookup>(entity =>
            {
                entity.HasKey(e => e.HorizontalDatum).HasName("PK__Horizont__B9F72CCA5D341725");

                entity.ToTable("HorizontalDatumLookup", "AlbertaTownshipSystem");

                entity.Property(e => e.HorizontalDatum)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Description).HasMaxLength(100);
            });

            modelBuilder.Entity<AlbertaTownshipSystemModels.MeridianLookup>(entity =>
            {
                entity.HasKey(e => e.MeridianId).HasName("PK__Meridian__78DE87538EEABAFC");

                entity.ToTable("MeridianLookup", "AlbertaTownshipSystem");

                entity.Property(e => e.MeridianId)
                    .ValueGeneratedNever()
                    .HasColumnName("MeridianID");
            });

            modelBuilder.Entity<AlbertaTownshipSystemModels.RoadAllowanceCodeLookup>(entity =>
            {
                entity.HasKey(e => e.RoadAllowanceCode).HasName("PK__RoadAllo__06CBB2717072105A");

                entity.ToTable("RoadAllowanceCodeLookup", "AlbertaTownshipSystem");

                entity.Property(e => e.RoadAllowanceCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<AlbertaTownshipSystemModels.StationCodeLookup>(entity =>
            {
                entity.HasKey(e => e.StationCode).HasName("PK__StationC__D388561925280E56");

                entity.ToTable("StationCodeLookup", "AlbertaTownshipSystem");

                entity.Property(e => e.StationCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Description).HasMaxLength(100);
            });

            modelBuilder.Entity<AlbertaTownshipSystemModels.StatusCodeLookup>(entity =>
            {
                entity.HasKey(e => e.StatusCode).HasName("PK__StatusCo__6A7B44FD89A24268");

                entity.ToTable("StatusCodeLookup", "AlbertaTownshipSystem");

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Description).HasMaxLength(50);
            });

            modelBuilder.Entity<AlbertaTownshipSystemModels.VerticalDatumLookup>(entity =>
            {
                entity.HasKey(e => e.VerticalDatum).HasName("PK__Vertical__9A5746AC4B9BBFD3");

                entity.ToTable("VerticalDatumLookup", "AlbertaTownshipSystem");

                entity.Property(e => e.VerticalDatum)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Description).HasMaxLength(50);
            });

            // BusinessAssociate Configuration
            modelBuilder.Entity<BusinessAssociateModels.BusinessAssociate>(entity =>
            {
                entity.HasKey(e => e.Baidentifier).HasName("PK__Business__1E4232335E5CC3A9");

                entity.ToTable("BusinessAssociate", "BusinessAssociate");

                entity.Property(e => e.Baidentifier)
                    .HasMaxLength(50)
                    .HasColumnName("BAIdentifier");
                entity.Property(e => e.AmalgamatedIntoBaid)
                    .HasMaxLength(50)
                    .HasColumnName("AmalgamatedIntoBAID");
                entity.Property(e => e.AmalgamatedIntoBalegalName)
                    .HasMaxLength(255)
                    .HasColumnName("AmalgamatedIntoBALegalName");
                entity.Property(e => e.BaabbreviatedName)
                    .HasMaxLength(100)
                    .HasColumnName("BAAbbreviatedName");
                entity.Property(e => e.Baaddress)
                    .HasMaxLength(500)
                    .HasColumnName("BAAddress");
                entity.Property(e => e.BaamalgamationEstablishedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("BAAmalgamationEstablishedDate");
                entity.Property(e => e.BacorporateStatus)
                    .HasMaxLength(50)
                    .HasColumnName("BACorporateStatus");
                entity.Property(e => e.BacorporateStatusEffectiveDate)
                    .HasColumnType("datetime")
                    .HasColumnName("BACorporateStatusEffectiveDate");
                entity.Property(e => e.BalegalName)
                    .HasMaxLength(255)
                    .HasColumnName("BALegalName");
                entity.Property(e => e.BalicenceEligibilityType)
                    .HasMaxLength(50)
                    .HasColumnName("BALicenceEligibilityType");
                entity.Property(e => e.BalicenceEligibiltyDesc)
                    .HasMaxLength(100)
                    .HasColumnName("BALicenceEligibiltyDesc");
                entity.Property(e => e.BaphoneNumber)
                    .HasMaxLength(50)
                    .HasColumnName("BAPhoneNumber");
            });

            // FacilityApprovalsDaily Configuration
            modelBuilder.Entity<FacilityApprovalsDailyModels.FacilityLicence>(entity =>
            {
                entity.HasKey(e => e.LicenceNumber).HasName("PK_FacilityLicenceAllAB");

                entity.ToTable("FacilityLicence", "FacilityApprovalsDaily");

                entity.Property(e => e.LicenceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.EnergyDevelopmentCategoryType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Latitude)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LicenceStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LicenceStatusDate)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Longitude)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.MaxH2sconcentration)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MaxH2SConcentration");
                entity.Property(e => e.NonRoutineLicence)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NonRoutineStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RatingLevel)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SurfaceLocation)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            // FacilityInfrastructure Configuration
            modelBuilder.Entity<FacilityInfrastructureModels.Facility>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("Facility", "FacilityInfrastructure");

                entity.Property(e => e.EnergyDevelopmentCategoryId)
                    .HasMaxLength(50)
                    .HasColumnName("EnergyDevelopmentCategoryID");
                entity.Property(e => e.EnergyDevelopmentCategoryType).HasMaxLength(50);
                entity.Property(e => e.ExperimentalConfidential)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.FacilityId)
                    .HasMaxLength(50)
                    .HasColumnName("FacilityID");
                entity.Property(e => e.FacilityIdentifier).HasMaxLength(50);
                entity.Property(e => e.FacilityLegalSubdivision).HasMaxLength(50);
                entity.Property(e => e.FacilityLicenceStatus).HasMaxLength(50);
                entity.Property(e => e.FacilityLocation).HasMaxLength(100);
                entity.Property(e => e.FacilityName).HasMaxLength(255);
                entity.Property(e => e.FacilityOperationalStatus).HasMaxLength(50);
                entity.Property(e => e.FacilityOperationalStatusDate).HasColumnType("datetime");
                entity.Property(e => e.FacilityProvinceState).HasMaxLength(50);
                entity.Property(e => e.FacilityStartDate).HasColumnType("datetime");
                entity.Property(e => e.FacilitySubType).HasMaxLength(50);
                entity.Property(e => e.FacilitySubTypeDesc).HasMaxLength(255);
                entity.Property(e => e.FacilityType).HasMaxLength(50);
                entity.Property(e => e.LicenceIssueDate).HasColumnType("datetime");
                entity.Property(e => e.LicenceNumber).HasMaxLength(50);
                entity.Property(e => e.LicenceType).HasMaxLength(50);
                entity.Property(e => e.LicenseeBaid)
                    .HasMaxLength(50)
                    .HasColumnName("LicenseeBAID");
                entity.Property(e => e.LicenseeName).HasMaxLength(255);
                entity.Property(e => e.MeterStationPipelineLink).HasMaxLength(255);
                entity.Property(e => e.MpfacilityIdentifier)
                    .HasMaxLength(50)
                    .HasColumnName("MPFacilityIdentifier");
                entity.Property(e => e.MpfacilityProvinceState)
                    .HasMaxLength(50)
                    .HasColumnName("MPFacilityProvinceState");
                entity.Property(e => e.MpfacilityType)
                    .HasMaxLength(50)
                    .HasColumnName("MPFacilityType");
                entity.Property(e => e.OperatorBaid)
                    .HasMaxLength(50)
                    .HasColumnName("OperatorBAID");
                entity.Property(e => e.OperatorName).HasMaxLength(255);
                entity.Property(e => e.OperatorStartDate).HasColumnType("datetime");
                entity.Property(e => e.OrphanWellFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.TerminalPipelineLink).HasMaxLength(255);
                entity.Property(e => e.TierAggregateId)
                    .HasMaxLength(50)
                    .HasColumnName("TierAggregateID");
                entity.Property(e => e.TierAggregatePr)
                    .HasMaxLength(255)
                    .HasColumnName("TierAggregatePR");
                entity.Property(e => e.TpfacilityIdentifier)
                    .HasMaxLength(50)
                    .HasColumnName("TPFacilityIdentifier");
                entity.Property(e => e.TpfacilityProvinceState)
                    .HasMaxLength(50)
                    .HasColumnName("TPFacilityProvinceState");
                entity.Property(e => e.TpfacilityType)
                    .HasMaxLength(50)
                    .HasColumnName("TPFacilityType");
            });

            // FacilityLicence Configuration
            modelBuilder.Entity<FacilityLicenceModels.Licence>(entity =>
            {
                entity.HasKey(e => e.LicenceNumber).HasName("PK__Licence__1EF80591344ED8E4");

                entity.ToTable("Licence", "FacilityLicence");

                entity.Property(e => e.LicenceNumber).HasMaxLength(50);
                entity.Property(e => e.EnergyDevelopmentCategoryType).HasMaxLength(100);
                entity.Property(e => e.LicenceLegalSubdivision).HasMaxLength(50);
                entity.Property(e => e.LicenceLocation).HasMaxLength(100);
                entity.Property(e => e.LicenceStatus).HasMaxLength(50);
                entity.Property(e => e.LicenceStatusDate).HasColumnType("datetime");
                entity.Property(e => e.LicenceType).HasMaxLength(50);
                entity.Property(e => e.Licensee).HasMaxLength(50);
                entity.Property(e => e.LicenseeName).HasMaxLength(255);
                entity.Property(e => e.OrphanWellFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            // FacilityOperatorHistory Configuration
            modelBuilder.Entity<FacilityOperatorHistoryModels.Facility>(entity =>
            {
                entity.HasKey(e => e.FacilityId).HasName("PK__Facility__5FB08B9476E97673");

                entity.ToTable("Facility", "FacilityOperatorHistory");

                entity.Property(e => e.FacilityId)
                    .HasMaxLength(50)
                    .HasColumnName("FacilityID");
                entity.Property(e => e.FacilityIdentifier).HasMaxLength(50);
                entity.Property(e => e.FacilityName).HasMaxLength(255);
                entity.Property(e => e.FacilityProvinceState).HasMaxLength(50);
                entity.Property(e => e.FacilitySubType).HasMaxLength(50);
                entity.Property(e => e.FacilitySubTypeDesc).HasMaxLength(255);
                entity.Property(e => e.FacilityType).HasMaxLength(50);
            });

            modelBuilder.Entity<FacilityOperatorHistoryModels.OperatorHistory>(entity =>
            {
                entity.HasKey(e => new { e.FacilityId, e.OperatorBaid, e.StartDate }).HasName("PK__Operator__9C1CC11F8CB2E43D");

                entity.ToTable("OperatorHistory", "FacilityOperatorHistory");

                entity.Property(e => e.FacilityId)
                    .HasMaxLength(50)
                    .HasColumnName("FacilityID");
                entity.Property(e => e.OperatorBaid)
                    .HasMaxLength(50)
                    .HasColumnName("OperatorBAID");
                entity.Property(e => e.StartDate).HasMaxLength(7);
                entity.Property(e => e.EndDate).HasMaxLength(7);
                entity.Property(e => e.OperatorName).HasMaxLength(255);

                entity.HasOne(d => d.Facility).WithMany(p => p.OperatorHistories)
                    .HasForeignKey(d => d.FacilityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OperatorH__Facil__778AC167");
            });

            // WellFacilityLink Configuration
            modelBuilder.Entity<WellFacilityLinkModels.LinkedFacility>(entity =>
            {
                entity.HasKey(e => new { e.LinkedFacilityId, e.WellId }).HasName("PK__LinkedFa__98B4E4C3B22D0B40");

                entity.ToTable("LinkedFacility", "WellFacilityLink");

                entity.Property(e => e.LinkedFacilityId)
                    .HasMaxLength(50)
                    .HasColumnName("LinkedFacilityID");
                entity.Property(e => e.WellId)
                    .HasMaxLength(50)
                    .HasColumnName("WellID");
                entity.Property(e => e.LinkedFacilityIdentifier).HasMaxLength(50);
                entity.Property(e => e.LinkedFacilityName).HasMaxLength(100);
                entity.Property(e => e.LinkedFacilityOperatorBaid)
                    .HasMaxLength(50)
                    .HasColumnName("LinkedFacilityOperatorBAID");
                entity.Property(e => e.LinkedFacilityOperatorName).HasMaxLength(100);
                entity.Property(e => e.LinkedFacilityProvinceState).HasMaxLength(10);
                entity.Property(e => e.LinkedFacilitySubType).HasMaxLength(50);
                entity.Property(e => e.LinkedFacilitySubTypeDesc).HasMaxLength(100);
                entity.Property(e => e.LinkedFacilityType).HasMaxLength(10);

                entity.HasOne(d => d.Well).WithMany(p => p.LinkedFacilities)
                    .HasForeignKey(d => d.WellId)
                    .HasConstraintName("FK__LinkedFac__WellI__5FB337D6");
            });

            modelBuilder.Entity<WellFacilityLinkModels.WellFacilityLink>(entity =>
            {
                entity.HasKey(e => e.WellId).HasName("PK__WellFaci__E955CC1C74AB5B12");

                entity.ToTable("WellFacilityLink", "WellFacilityLink");

                entity.Property(e => e.WellId)
                    .HasMaxLength(50)
                    .HasColumnName("WellID");
                entity.Property(e => e.WellIdentifier).HasMaxLength(50);
                entity.Property(e => e.WellLegalSubdivision).HasMaxLength(10);
                entity.Property(e => e.WellLocationException).HasMaxLength(50);
                entity.Property(e => e.WellName).HasMaxLength(100);
                entity.Property(e => e.WellProvinceState).HasMaxLength(10);
                entity.Property(e => e.WellStatusFluid).HasMaxLength(50);
                entity.Property(e => e.WellStatusFluidCode).HasMaxLength(10);
                entity.Property(e => e.WellStatusMode).HasMaxLength(50);
                entity.Property(e => e.WellStatusModeCode).HasMaxLength(10);
                entity.Property(e => e.WellStatusStructure).HasMaxLength(50);
                entity.Property(e => e.WellStatusStructureCode).HasMaxLength(10);
                entity.Property(e => e.WellStatusType).HasMaxLength(50);
                entity.Property(e => e.WellStatusTypeCode).HasMaxLength(10);
                entity.Property(e => e.WellType).HasMaxLength(10);
            });

            // WellInfrastructure Configuration
            modelBuilder.Entity<WellInfrastructureModels.CommingledWell>(entity =>
            {
                entity.HasKey(e => new { e.ComminglingProcessApprovalNumber, e.WellId });

                entity.ToTable("CommingledWell", "WellInfrastructure");

                entity.Property(e => e.ComminglingProcessApprovalNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.WellId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("WellID");
                entity.Property(e => e.CommingledReportingWellId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CommingledReportingWellID");
                entity.Property(e => e.CommingledReportingWellIdentifier)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CommingledReportingWellProvinceState)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CommingledReportingWellType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ComminglingProcess)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Well).WithMany(p => p.CommingledWells)
                    .HasForeignKey(d => d.WellId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Commingle__WellI__4AB81AF0");
            });

            modelBuilder.Entity<WellInfrastructureModels.LinkedFacility>(entity =>
            {
                entity.HasKey(e => new { e.LinkedFacilityId, e.WellId });

                entity.ToTable("LinkedFacility", "WellInfrastructure");

                entity.Property(e => e.LinkedFacilityId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LinkedFacilityID");
                entity.Property(e => e.WellId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("WellID");
                entity.Property(e => e.LinkedFacilityIdentifier)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LinkedFacilityName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.LinkedFacilityOperatorBaid)
                    .HasMaxLength(50)
                    .HasColumnName("LinkedFacilityOperatorBAID");
                entity.Property(e => e.LinkedFacilityOperatorLegalName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.LinkedFacilityProvinceState)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LinkedFacilitySubType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LinkedFacilitySubTypeDesc)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.LinkedFacilityType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Well).WithMany(p => p.LinkedFacilities)
                    .HasForeignKey(d => d.WellId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LinkedFac__WellI__48CFD27E");
            });

            modelBuilder.Entity<WellInfrastructureModels.Well>(entity =>
            {
                entity.HasKey(e => e.WellId).HasName("PK__Well__E955CC1C7C2258E4");

                entity.ToTable("Well", "WellInfrastructure");

                entity.Property(e => e.WellId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("WellID");
                entity.Property(e => e.AllowableType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Area)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.AreaName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.ConfidentialType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ExperimentalConfidentialIndicator)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Field)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.FieldName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.FinalTotalDepth).HasColumnType("decimal(10, 4)");
                entity.Property(e => e.HorizontalDrill)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LicenceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LicenceStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LicenceType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LicenseeId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LicenseeID");
                entity.Property(e => e.LicenseeName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.MaxTrueVerticalDepth).HasColumnType("decimal(10, 4)");
                entity.Property(e => e.OrphanWellFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.PoolDeposit)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.PoolDepositDensity).HasColumnType("decimal(10, 4)");
                entity.Property(e => e.PoolDepositName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.PreviousWellId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PreviousWellID");
                entity.Property(e => e.RecoveryMechanismType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.VolumetricGasWellLiquidType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.WellIdentifier)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.WellLegalSubdivision)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.WellLocationException)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.WellName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.WellProvinceState)
                    .HasMaxLength(2)
                    .IsUnicode(false);
                entity.Property(e => e.WellStatusFluid)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.WellStatusFluidCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.WellStatusMode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.WellStatusModeCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.WellStatusStructure)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.WellStatusStructureCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.WellStatusType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.WellStatusTypeCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.WellType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            // WellLicence Configuration
            modelBuilder.Entity<WellLicenceModels.Well>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("Well", "WellLicence");

                entity.Property(e => e.Aerclass)
                    .HasMaxLength(100)
                    .HasColumnName("AERClass");
                entity.Property(e => e.DrillingOperationType).HasMaxLength(100);
                entity.Property(e => e.HeadLessor).HasMaxLength(100);
                entity.Property(e => e.LicenceIssueDate).HasColumnType("datetime");
                entity.Property(e => e.LicenceLegalSubdivision).HasMaxLength(50);
                entity.Property(e => e.LicenceLocation).HasMaxLength(100);
                entity.Property(e => e.LicenceNumber).HasMaxLength(50);
                entity.Property(e => e.LicenceStatus).HasMaxLength(50);
                entity.Property(e => e.LicenceStatusDate).HasColumnType("datetime");
                entity.Property(e => e.LicenceType).HasMaxLength(50);
                entity.Property(e => e.LicenseeId)
                    .HasMaxLength(50)
                    .HasColumnName("LicenseeID");
                entity.Property(e => e.LicenseeName).HasMaxLength(255);
                entity.Property(e => e.OrphanWellFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.ProjectedFormation).HasMaxLength(100);
                entity.Property(e => e.ProjectedTotalDepth).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TargetPool).HasMaxLength(100);
                entity.Property(e => e.TerminatingFormation).HasMaxLength(100);
                entity.Property(e => e.WellCompletionType).HasMaxLength(100);
                entity.Property(e => e.WellLicenceType).HasMaxLength(100);
                entity.Property(e => e.WellPurpose).HasMaxLength(255);
                entity.Property(e => e.WellSubstance).HasMaxLength(100);
            });

            // WellWiki Configuration
            modelBuilder.Entity<WellWikiModels.Well>(entity =>
            {
                entity.ToTable("Well", "WellWiki");

                entity.Property(e => e.WellId)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("WellID");
                entity.Property(e => e.Country)
                    .HasMaxLength(128)
                    .IsUnicode(false);
                entity.Property(e => e.County)
                    .HasMaxLength(128)
                    .IsUnicode(false);
                entity.Property(e => e.DateTimeCreated)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.LicenseNumber)
                    .HasMaxLength(128)
                    .IsUnicode(false);
                entity.Property(e => e.LicenseStatus)
                    .HasMaxLength(128)
                    .IsUnicode(false);
                entity.Property(e => e.Location)
                    .HasMaxLength(128)
                    .IsUnicode(false);
                entity.Property(e => e.LocationAlias)
                    .HasMaxLength(128)
                    .IsUnicode(false);
                entity.Property(e => e.LocationAlternateAlias)
                    .HasMaxLength(128)
                    .IsUnicode(false);
                entity.Property(e => e.OperatorName)
                    .HasMaxLength(128)
                    .IsUnicode(false);
                entity.Property(e => e.Province)
                    .HasMaxLength(128)
                    .IsUnicode(false);
                entity.Property(e => e.WellName)
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WellWikiModels.WellDirectionalDrilling>(entity =>
            {
                entity.ToTable("WellDirectionalDrilling", "WellWiki");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Reason)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.WellId)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("WellID");

                entity.HasOne(d => d.Well).WithMany(p => p.WellDirectionalDrillings)
                    .HasForeignKey(d => d.WellId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WellDirectionalDrilling_Well");
            });

            modelBuilder.Entity<WellWikiModels.WellHistory>(entity =>
            {
                entity.ToTable("WellHistory", "WellWiki");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Event)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.WellId)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("WellID");

                entity.HasOne(d => d.Well).WithMany(p => p.WellHistories)
                    .HasForeignKey(d => d.WellId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WellHistory_Well");
            });

            modelBuilder.Entity<WellWikiModels.WellPerforationTreatment>(entity =>
            {
                entity.ToTable("WellPerforationTreatments", "WellWiki");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.PerforationType)
                    .HasMaxLength(128)
                    .IsUnicode(false);
                entity.Property(e => e.WellId)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("WellID");

                entity.HasOne(d => d.Well).WithMany(p => p.WellPerforationTreatments)
                    .HasForeignKey(d => d.WellId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WellPerforationTreatments_Well");
            });

            modelBuilder.Entity<WellWikiModels.WellProductionDatum>(entity =>
            {
                entity.ToTable("WellProductionData", "WellWiki");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.WellId)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("WellID");

                entity.HasOne(d => d.Well).WithMany(p => p.WellProductionData)
                    .HasForeignKey(d => d.WellId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WellProductionData_Well");
            });

            // WellInfrastructure Wells Configuration (to differentiate from other Wells)
            modelBuilder.Entity<WellInfrastructureModels.Well>().ToTable("Well", "WellInfrastructure");

            // WellLicence Wells Configuration (to differentiate from other Wells)
            modelBuilder.Entity<WellLicenceModels.Well>().ToTable("Well", "WellLicence");

            // WellWiki Wells Configuration (to differentiate from other Wells)
            modelBuilder.Entity<WellWikiModels.Well>().ToTable("Well", "WellWiki");

            // Additional configurations can be added here following the same pattern

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
