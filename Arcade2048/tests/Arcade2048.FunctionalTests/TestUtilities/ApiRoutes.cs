namespace Arcade2048.FunctionalTests.TestUtilities;
public class ApiRoutes
{
    public const string Base = "api";
    public const string Health = Base + "/health";

    // new api route marker - do not delete

    public static class RolePermissions
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/rolePermissions";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/rolePermissions/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/rolePermissions/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/rolePermissions/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/rolePermissions/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/rolePermissions";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/rolePermissions/batch";
    }

    public static class Permissions
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/permissions";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/permissions/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/permissions/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/permissions/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/permissions/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/permissions";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/permissions/batch";
    }

    public static class Roles
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/roles";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/roles/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/roles/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/roles/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/roles/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/roles";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/roles/batch";
    }

    public static class LeaderboardEntries
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/leaderboardEntries";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/leaderboardEntries/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/leaderboardEntries/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/leaderboardEntries/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/leaderboardEntries/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/leaderboardEntries";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/leaderboardEntries/batch";
    }

    public static class Scores
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/scores";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/scores/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/scores/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/scores/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/scores/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/scores";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/scores/batch";
    }

    public static class ExternalLogins
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/externalLogins";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/externalLogins/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/externalLogins/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/externalLogins/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/externalLogins/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/externalLogins";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/externalLogins/batch";
    }

    public static class Users
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/users";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/users/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/users/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/users/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/users/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/users";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/users/batch";
    }
}
