namespace DarazApp.Helpers
{

    public static class UserResponseMessages
    {
        public const string UserCreatedSuccess = "User created successfully.";
        public const string UserUpdatedSuccess = "User updated successfully.";
        public const string UserDeletedSuccess = "User deleted successfully.";
        public const string UsersRetrievedSuccess = "Users retrieved successfully.";
        public const string UserNotFound = "User not found with the specified ID.";
        public const string NoUsersFound = "No users found.";
        public const string ErrorOccurred = "An error occurred while processing the request.";

    }
    public static class AuthResponseMessages
    {
        public const string InvalidCredientials = "Invalid email or password.";
        public const string UserLoginSuccess = "User Login successful.";
        public const string EmailConfirmationFailed = "Email confirmation faild";
        public const string PasswordSetSuccess = "Password Set Successfully.";
        public const string PasswordReset = "Password reset token generated and Sent to Email successfully.";
        
    }

    public static class ProductResponseMessages
    {
        public const string ProductCreatedSuccess = "Product created successfully.";
        public const string ProductUpdatedSuccess = "Product updated successfully.";
        public const string ProductDeletedSuccess = "Product deleted successfully.";
        public const string ProductsRetrievedSuccess = "Products retrieved successfully.";
        public const string ProductNotFound = "Product not found with the specified ID.";
        public const string NoProductsFound = "No products found.";
        public const string ErrorOccurred = "An error occurred while processing the request.";
    }
    public static class CategoryResponseMessages
    {
        public const string TopLevelCategoriesFetchedSuccess = "Fetched top-level categories successfully.";
        public const string SubcategoriesFetchedSuccess = "Fetched subcategories successfully.";
        public const string ProductsFetchedSuccess = "Fetched products successfully.";
        public const string CategoryCreatedSuccess = "Category created successfully.";
        public const string CategoryNameRequired = "Category name is required.";
        public const string CategoryAlreadyExists = "Category already exists.";
        public const string CategoryNotExists = "Category Not exists.";
        public const string ErrorOccurred = "An error occurred while processing the request.";
    }

    public static class OrderResponseMessages
    {
        public const string OrderCreatedSuccess = "Order created successfully.";
        public const string OrderRetrievedSuccess = "Order details retrieved successfully.";
        public const string OrdersRetrievedSuccess = "Orders retrieved successfully.";
        public const string NoOrdersFoundForUser = "No orders found for the user.";
        public const string ErrorOccurred = "An error occurred while processing the request.";
        public const string OrderNotFound = "Order not found with the specified ID.";
    }

}

