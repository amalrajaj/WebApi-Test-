namespace WebApiProject.Startup
{
    public static partial class MiddilewareInitializer
    {
        public static WebApplication ConfigureMiddleware(this WebApplication app)
        {
            ConfigureSwagger(app);
            ConfigureOthers(app);
            app.UseHttpsRedirection();
            return app;

        }

        private static void ConfigureSwagger(WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        private static void ConfigureOthers(WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());       
            app.MapControllers();
        }
}
}
