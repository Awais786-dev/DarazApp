﻿namespace DarazApp.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }  // Null for top-level categories

        // Navigation property to self-reference parent category
        public Category ParentCategory { get; set; }

        // Navigation property for subcategories
        public ICollection<Category> SubCategories { get; set; }

        // Navigation property for Products under this category
        public ICollection<Product> Products { get; set; }

    }
}
