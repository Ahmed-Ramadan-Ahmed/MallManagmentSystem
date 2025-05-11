using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MallManagmentSystem.Services;
using MallManagmentSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using MallManagmentSystem.Data;

namespace MallManagmentSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Configure JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add HttpClient services
            builder.Services.AddHttpClient();

            // Add DbContext configuration
            builder.Services.AddDbContext<MallDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register messaging services
            builder.Services.AddKeyedScoped<IMessagingService, WhatsAppService>("WhatsApp");
            builder.Services.AddKeyedScoped<IMessagingService, SMSService>("SMS");
            // Register rent invoice service
            builder.Services.AddScoped<IRentInvoiceService, RentInvoiceService>();
            builder.Services.AddScoped<IPrintService, PrintService>();
            // Register notification service
            builder.Services.AddScoped<INotificationService, NotificationService>();
            // Register auth service
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IStatisticsService, StatisticsService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Add authentication middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
