﻿namespace DarazApp.DTOs
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }  // Nullable ParentCategoryId
    }
}
