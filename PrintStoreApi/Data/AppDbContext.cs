using Microsoft.EntityFrameworkCore;

using PrintStoreApi.Core.Entities.Auth;
using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Entities.User;


namespace PrintStoreApi.Data;

public class AppDbContext: DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

	public DbSet<UserDB> Users { get; set; }
	public DbSet<Role> Roles { get; set; }
	public DbSet<RevokedToken> RevokedTokens { get; set; }
	public DbSet<BaseProduct> BaseProducts { get; set; }
	public DbSet<BaseVariant> BaseVariants { get; set; }
	public DbSet<StoreProduct> StoreProducts { get; set; }
	public DbSet<StoreVariant> StoreVariants { get; set; }
	public DbSet<VariantFile> StoreVariantFiles { get; set; }
	public DbSet<ProductCategory> ProductCategories { get; set; }
	public DbSet<CustomizableProduct> CustomizableProducts { get; set; }
	public DbSet<CustomizableVarint> CustomizableVarints { get; set; }
	public DbSet<ProductMaterial> Materials { get; set; }
	public DbSet<AvailableRegion> AvailableRegions { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Configure relationships
		modelBuilder.Entity<BaseVariant>()
			.HasOne(bv => bv.BaseProduct)
			.WithMany(bp => bp.BaseVariants)
			.HasForeignKey(bv => bv.BaseProductId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<StoreProduct>()
			.HasOne(sp => sp.BaseProduct)
			.WithMany(bp => bp.StoreProducts)
			.HasForeignKey(sp => sp.BaseProductId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<StoreVariant>()
			.HasOne(sv => sv.BaseProduct)
			.WithMany(bp => bp.StoreVariants)
			.HasForeignKey(sv => sv.BaseProductId)
			.OnDelete(DeleteBehavior.NoAction);

		modelBuilder.Entity<StoreVariant>()
			.HasOne(sv => sv.StoreProduct)
			.WithMany(sp => sp.StoreVariants)
			.HasForeignKey(sv => sv.StoreProductId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<StoreVariant>()
			.HasOne(sv => sv.BaseVariant)
			.WithMany(bv => bv.StoreVariants)
			.HasForeignKey(sv => sv.BaseVariantId)
			.OnDelete(DeleteBehavior.NoAction);

		modelBuilder.Entity<VariantFile>()
			.HasOne(sv => sv.storeVariant)
			.WithMany(bv => bv.Files)
			.HasForeignKey(sv => sv.VariantId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<ProductCategory>()
			.HasIndex(pc => pc.PrintfulId)
			.IsUnique();

		modelBuilder.Entity<ProductCategory>()
			.HasOne(c => c.ParentCategory)
			.WithMany(c => c.SubCategories)
			.HasForeignKey(c => c.ParentId)
			.OnDelete(DeleteBehavior.SetNull);

		modelBuilder.Entity<StoreVariant>()
			.HasOne(sv => sv.ProductCategory)
			.WithMany(sp => sp.StoreVariants)
			.HasPrincipalKey(pc => pc.PrintfulId)
			.HasForeignKey(sv => sv.CategoryId)
			.OnDelete(DeleteBehavior.SetNull);

		modelBuilder.Entity<BaseProduct>()
			.HasOne(sv => sv.ProductCategory)
			.WithMany(sp => sp.BaseProducts)
			.HasPrincipalKey(pc => pc.PrintfulId)
			.HasForeignKey(sv => sv.CategoryId)
			.OnDelete(DeleteBehavior.SetNull);

		modelBuilder.Entity<CustomizableVarint>()
			.HasOne(bv => bv.CustomizableProduct)
			.WithMany(bp => bp.CustomizableVarints)
			.HasForeignKey(bv => bv.BaseProductId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<ProductMaterial>()
			.HasOne(bv => bv.CustomizableVarint)
			.WithMany(bp => bp.ProductMaterials)
			.HasForeignKey(bv => bv.VariantId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<AvailableRegion>()
			.HasOne(bv => bv.CustomizableVarint)
			.WithMany(bp => bp.AvailableRegions)
			.HasForeignKey(bv => bv.VariantId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<CustomizableProduct>()
			.HasOne(sv => sv.ProductCategory)
			.WithMany(sp => sp.CustomizableProduct)
			.HasPrincipalKey(pc => pc.PrintfulId)
			.HasForeignKey(sv => sv.CategoryId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}