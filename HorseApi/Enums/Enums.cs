namespace HorseApi.Enums
{
    public enum ReponseErrorType
    {
        USER_NOT_FOUND = 1,
        INVALID_PASSWORD = 2,
        INVALID_REFRESH_TOKEN = 3,
    }

    public enum UserRole
    {
        ADMIN = 1,
        USER = 2
    }
}