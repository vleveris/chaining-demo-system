using ChainingAPI.Models;
using ChainingAPI.Services;
namespace ChainingAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<FactsRulesDatabaseSettings>(
                builder.Configuration.GetSection("ChainingDatabase"));
builder.Services.AddSingleton<KnowledgeBaseService>();
            builder.Services.AddSingleton<ForwardChainingService>();
            builder.Services.AddSingleton<BackwardChainingService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(cors => cors
.AllowAnyMethod()
.AllowAnyHeader()
.SetIsOriginAllowed(origin => true)
.AllowCredentials()
);
            app.MapControllers();

            app.Run();
        }
    }
}