using ECommerceBE.Application;
using ECommerceBE.Application.Validators.Products;
using ECommerceBE.Infrastructure;
using ECommerceBE.Infrastructure.Filters;
using ECommerceBE.Infrastructure.Services.Storage.Azure;
using ECommerceBE.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            //aşağıdaki enum ile olan tanımlamayı genelde kullanmıyoruz(yeni nesneler için koda müdahale olduğundan ötürü), generic yapı kullanılıyor.
            //builder.Services.AddStorage(StorageType.Local);
            //builder.Services.AddStorage<LocalStorage>();
            builder.Services.AddStorage<AzureStorage>();

            builder.Services.AddCors(options => options.AddDefaultPolicy(policy => 
                policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                .AllowAnyHeader().AllowAnyMethod()
            ));

            //builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
            //    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
            //    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);


            builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
            builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<CreateProductValidator>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Admin", options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        //oluşturulacak token değerini kimlerin/hangi originlerin/sitelerin kullanıcağını velirlediğimiz değerdir. -> www.bilmemne.com
                        ValidateAudience = true,

                        //oluşturulacak token değerini kimin dağıttığını ifade edeceğimiz alandır. -> www.myapi.com
                        ValidateIssuer = true,

                        //oluşturulacak token değerinin süresini kontrol edecek olan doğrulamadır.
                        ValidateLifetime = true,

                        //üretilecek token değerinin uygulamamıza ait bir değer olduğunu ifade eden security key verisinin doğrulanmasıdır.
                        ValidateIssuerSigningKey = true,

                        ValidAudience = builder.Configuration["Token:Audince"],
                        ValidIssuer = builder.Configuration["Token:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
                        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //wwwroot dizinine erişebilmek için eklenmeli.
            app.UseStaticFiles();

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}