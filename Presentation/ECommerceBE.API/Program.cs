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
            //a�a��daki enum ile olan tan�mlamay� genelde kullanm�yoruz(yeni nesneler i�in koda m�dahale oldu�undan �t�r�), generic yap� kullan�l�yor.
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
                        //olu�turulacak token de�erini kimlerin/hangi originlerin/sitelerin kullan�ca��n� velirledi�imiz de�erdir. -> www.bilmemne.com
                        ValidateAudience = true,

                        //olu�turulacak token de�erini kimin da��tt���n� ifade edece�imiz aland�r. -> www.myapi.com
                        ValidateIssuer = true,

                        //olu�turulacak token de�erinin s�resini kontrol edecek olan do�rulamad�r.
                        ValidateLifetime = true,

                        //�retilecek token de�erinin uygulamam�za ait bir de�er oldu�unu ifade eden security key verisinin do�rulanmas�d�r.
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

            //wwwroot dizinine eri�ebilmek i�in eklenmeli.
            app.UseStaticFiles();

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}