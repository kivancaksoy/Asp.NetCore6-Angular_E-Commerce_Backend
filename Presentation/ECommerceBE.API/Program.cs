using ECommerceBE.Application;
using ECommerceBE.Application.Validators.Products;
using ECommerceBE.Infrastructure;
using ECommerceBE.Infrastructure.Enums;
using ECommerceBE.Infrastructure.Filters;
using ECommerceBE.Infrastructure.Services.Storage.Azure;
using ECommerceBE.Infrastructure.Services.Storage.Local;
using ECommerceBE.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerceBE.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddPersistenceServices();
            builder.Services.AddInfrastructureServices();
            builder.Services.AddApplicationServices();

            //builder.Services.AddStorage();
            //aþaðýdaki enum ile olan tanýmlamayý genelde kullanmýyoruz(yeni nesneler için koda müdahale olduðundan ötürü), generic yapý kullanýlýyor.
            //builder.Services.AddStorage(StorageType.Local);
            //builder.Services.AddStorage<LocalStorage>();
            builder.Services.AddStorage<AzureStorage>();

            builder.Services.AddCors(options => options.AddDefaultPolicy(policy => 
                policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                .AllowAnyHeader().AllowAnyMethod()
            ));

            builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
                .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication("Admin")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        //oluþturulacak token deðerini kimlerin/hangi originlerin/sitelerin kullanýcaðýný velirlediðimiz deðerdir. -> www.bilmemne.com
                        ValidateAudience = true,

                        //oluþturulacak token deðerini kimin daðýttýðýný ifade edeceðimiz alandýr. -> www.myapi.com
                        ValidateIssuer = true,

                        //oluþturulacak token deðerinin süresini kontrol edecek olan doðrulamadýr.
                        ValidateLifetime = true,

                        //üretilecek token deðerinin uygulamamýza ait bir deðer olduðunu ifade eden security key verisinin doðrulanmasýdýr.
                        ValidateIssuerSigningKey = true,

                        ValidAudience = builder.Configuration["Token:Audince"],
                        ValidIssuer = builder.Configuration["Token:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //wwwroot dizinine eriþebilmek için eklenmeli.
            app.UseStaticFiles();

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}