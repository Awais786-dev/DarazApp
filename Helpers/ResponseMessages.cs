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

}
