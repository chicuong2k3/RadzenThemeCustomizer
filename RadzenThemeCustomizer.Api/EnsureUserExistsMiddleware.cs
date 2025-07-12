using Microsoft.EntityFrameworkCore;

namespace RadzenThemeCustomizer.Api;

public class EnsureUserExistsMiddleware
{
    private readonly RequestDelegate _next;

    public EnsureUserExistsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, RadzenThemeCustomizerDbContext dbContext)
    {
        var userId = context.GetUserId();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            user = new User
            {
                Id = userId
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }

        await _next(context);
    }
}