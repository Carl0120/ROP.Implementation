namespace ROP.Implementation.Result;

public record ResultCode(int Id, string Name)
{
    public static readonly ResultCode Ok = new(200, "OK");
    public static readonly ResultCode BadRequest = new(400, "Bad Request");
    public static readonly ResultCode Unauthorized = new(401, "Unauthorized");
    public static readonly ResultCode NotFound = new(404, "Not Found");
    public static readonly ResultCode Forbidden = new(403, "Forbidden");
    public static readonly ResultCode InternalServerError = new(500, "Internal Server Error");
    public static readonly ResultCode NotImplemented = new(501, "Not Implemented");
}