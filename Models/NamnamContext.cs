using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NamNamAPI.Models;

public partial class NamnamContext : DbContext
{
    public NamnamContext()
    {
    }

    public NamnamContext(DbContextOptions<NamnamContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Cookinginstruction> Cookinginstructions { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Nutritionaldatum> Nutritionaldata { get; set; }

    public virtual DbSet<Preference> Preferences { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeHasIngredient> RecipeHasIngredients { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=nam-nam-bd.mysql.database.azure.com;database=namnam;user=NamNamAdminBD;pwd=Azure2023", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PRIMARY");

            entity.ToTable("category");

            entity.Property(e => e.IdCategory)
                .HasMaxLength(100)
                .HasColumnName("idCategory");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(60)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<Cookinginstruction>(entity =>
        {
            entity.HasKey(e => e.IdCookingInstruction).HasName("PRIMARY");

            entity.ToTable("cookinginstruction");

            entity.HasIndex(e => e.RecipeIdRecipe, "fk_CookingInstruction_Recipe1_idx");

            entity.Property(e => e.IdCookingInstruction)
                .HasMaxLength(100)
                .HasColumnName("idCookingInstruction");
            entity.Property(e => e.Instruction)
                .HasMaxLength(300)
                .HasColumnName("instruction");
            entity.Property(e => e.RecipeIdRecipe)
                .HasMaxLength(100)
                .HasColumnName("Recipe_idRecipe");
            entity.Property(e => e.Step).HasColumnName("step");

            entity.HasOne(d => d.RecipeIdRecipeNavigation).WithMany(p => p.Cookinginstructions)
                .HasForeignKey(d => d.RecipeIdRecipe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_CookingInstruction_Recipe1");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IdIngredient).HasName("PRIMARY");

            entity.ToTable("ingredient");

            entity.Property(e => e.IdIngredient)
                .HasMaxLength(100)
                .HasColumnName("idIngredient");
            entity.Property(e => e.IngredientName)
                .HasMaxLength(60)
                .HasColumnName("ingredientName");
            entity.Property(e => e.Measure)
                .HasMaxLength(3)
                .HasColumnName("measure");
        });

        modelBuilder.Entity<Nutritionaldatum>(entity =>
        {
            entity.HasKey(e => e.IdNutritionalData).HasName("PRIMARY");

            entity.ToTable("nutritionaldata");

            entity.HasIndex(e => e.IngredientIdIngredient, "fk_NutritionalData_Ingredient1_idx");

            entity.Property(e => e.IdNutritionalData)
                .HasMaxLength(100)
                .HasColumnName("idNutritionalData");
            entity.Property(e => e.Calories).HasColumnName("calories");
            entity.Property(e => e.Carbohydrates).HasColumnName("carbohydrates");
            entity.Property(e => e.Fat).HasColumnName("fat");
            entity.Property(e => e.IngredientIdIngredient)
                .HasMaxLength(100)
                .HasColumnName("Ingredient_idIngredient");
            entity.Property(e => e.Protein).HasColumnName("protein");

            entity.HasOne(d => d.IngredientIdIngredientNavigation).WithMany(p => p.Nutritionaldata)
                .HasForeignKey(d => d.IngredientIdIngredient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_NutritionalData_Ingredient1");
        });

        modelBuilder.Entity<Preference>(entity =>
        {
            entity.HasKey(e => e.IdPreference).HasName("PRIMARY");

            entity.ToTable("preference");

            entity.HasIndex(e => e.IdCategory, "idCategory_idx");

            entity.HasIndex(e => e.IdUser, "idUser_idx");

            entity.Property(e => e.IdPreference)
                .HasMaxLength(45)
                .HasColumnName("idPreference");
            entity.Property(e => e.IdCategory)
                .HasMaxLength(45)
                .HasColumnName("idCategory");
            entity.Property(e => e.IdUser)
                .HasMaxLength(45)
                .HasColumnName("idUser");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Preferences)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("idCategory");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Preferences)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("idUser");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.IdRecipe).HasName("PRIMARY");

            entity.ToTable("recipe");

            entity.HasIndex(e => e.UserIdUser, "fk_Recipe_User1_idx");

            entity.HasIndex(e => e.IdMainIngredient, "idMainIngredient_idx");

            entity.Property(e => e.IdRecipe)
                .HasMaxLength(100)
                .HasColumnName("idRecipe");
            entity.Property(e => e.IdMainIngredient)
                .HasMaxLength(100)
                .HasColumnName("idMainIngredient");
            entity.Property(e => e.ImageRecipeUrl)
                .HasMaxLength(200)
                .HasColumnName("imageRecipeURL");
            entity.Property(e => e.Portion).HasColumnName("portion");
            entity.Property(e => e.PreparationTime)
                .HasColumnType("time")
                .HasColumnName("preparationTime");
            entity.Property(e => e.ReceipName)
                .HasMaxLength(100)
                .HasColumnName("receipName");
            entity.Property(e => e.UserIdUser)
                .HasMaxLength(100)
                .HasColumnName("User_idUser");

            entity.HasOne(d => d.IdMainIngredientNavigation).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.IdMainIngredient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("idMainIngredient");

            entity.HasOne(d => d.UserIdUserNavigation).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.UserIdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Recipe_User1");

            entity.HasMany(d => d.CategoryIdCategories).WithMany(p => p.RecipeIdRecipes)
                .UsingEntity<Dictionary<string, object>>(
                    "RecipeHasCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryIdCategory")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_Recipe_has_Category_Category1"),
                    l => l.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipeIdRecipe")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_Recipe_has_Category_Recipe1"),
                    j =>
                    {
                        j.HasKey("RecipeIdRecipe", "CategoryIdCategory")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("recipe_has_category");
                        j.HasIndex(new[] { "CategoryIdCategory" }, "fk_Recipe_has_Category_Category1_idx");
                        j.HasIndex(new[] { "RecipeIdRecipe" }, "fk_Recipe_has_Category_Recipe1_idx");
                        j.IndexerProperty<string>("RecipeIdRecipe")
                            .HasMaxLength(100)
                            .HasColumnName("Recipe_idRecipe");
                        j.IndexerProperty<string>("CategoryIdCategory")
                            .HasMaxLength(100)
                            .HasColumnName("Category_idCategory");
                    });
        });

        modelBuilder.Entity<RecipeHasIngredient>(entity =>
        {
            entity.HasKey(e => new { e.IngredientIdIngredient, e.RecipeIdRecipe })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("recipe_has_ingredient");

            entity.HasIndex(e => e.IngredientIdIngredient, "fk_Ingredient_has_Recipe_Ingredient1_idx");

            entity.HasIndex(e => e.RecipeIdRecipe, "fk_Ingredient_has_Recipe_Recipe1_idx");

            entity.Property(e => e.IngredientIdIngredient)
                .HasMaxLength(100)
                .HasColumnName("Ingredient_idIngredient");
            entity.Property(e => e.RecipeIdRecipe)
                .HasMaxLength(100)
                .HasColumnName("Recipe_idRecipe");
            entity.Property(e => e.Amount).HasColumnName("amount");

            entity.HasOne(d => d.IngredientIdIngredientNavigation).WithMany(p => p.RecipeHasIngredients)
                .HasForeignKey(d => d.IngredientIdIngredient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Ingredient_has_Recipe_Ingredient1");

            entity.HasOne(d => d.RecipeIdRecipeNavigation).WithMany(p => p.RecipeHasIngredients)
                .HasForeignKey(d => d.RecipeIdRecipe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Ingredient_has_Recipe_Recipe1");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.IdReview).HasName("PRIMARY");

            entity.ToTable("review");

            entity.HasIndex(e => e.RecipeIdRecipe, "fk_review_Recipe1_idx");

            entity.HasIndex(e => e.UserIdUser, "fk_review_User1_idx");

            entity.Property(e => e.IdReview)
                .HasMaxLength(100)
                .HasColumnName("idReview");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.RecipeIdRecipe)
                .HasMaxLength(100)
                .HasColumnName("Recipe_idRecipe");
            entity.Property(e => e.Review1)
                .HasMaxLength(300)
                .HasColumnName("review");
            entity.Property(e => e.UserIdUser)
                .HasMaxLength(100)
                .HasColumnName("User_idUser");

            entity.HasOne(d => d.RecipeIdRecipeNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.RecipeIdRecipe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_Recipe1");

            entity.HasOne(d => d.UserIdUserNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserIdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_User1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.Property(e => e.IdUser)
                .HasMaxLength(100)
                .HasColumnName("idUser");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("lastName");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");

            entity.HasMany(d => d.CategoryIdCategories).WithMany(p => p.UserIdUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "UserHasPreferencecategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryIdCategory")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_User_has_Category_Category1"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserIdUser")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_User_has_Category_User1"),
                    j =>
                    {
                        j.HasKey("UserIdUser", "CategoryIdCategory")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("user_has_preferencecategories");
                        j.HasIndex(new[] { "CategoryIdCategory" }, "fk_User_has_Category_Category1_idx");
                        j.HasIndex(new[] { "UserIdUser" }, "fk_User_has_Category_User1_idx");
                        j.IndexerProperty<string>("UserIdUser")
                            .HasMaxLength(100)
                            .HasColumnName("User_idUser");
                        j.IndexerProperty<string>("CategoryIdCategory")
                            .HasMaxLength(100)
                            .HasColumnName("Category_idCategory");
                    });

            entity.HasMany(d => d.RecipeIdRecipes).WithMany(p => p.UserIdUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "Favoriterecipelist",
                    r => r.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipeIdRecipe")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_User_has_Recipe_Recipe2"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserIdUser")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_User_has_Recipe_User2"),
                    j =>
                    {
                        j.HasKey("UserIdUser", "RecipeIdRecipe")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("favoriterecipelist");
                        j.HasIndex(new[] { "RecipeIdRecipe" }, "fk_User_has_Recipe_Recipe2_idx");
                        j.HasIndex(new[] { "UserIdUser" }, "fk_User_has_Recipe_User2_idx");
                        j.IndexerProperty<string>("UserIdUser")
                            .HasMaxLength(100)
                            .HasColumnName("User_idUser");
                        j.IndexerProperty<string>("RecipeIdRecipe")
                            .HasMaxLength(100)
                            .HasColumnName("Recipe_idRecipe");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
