using ECommerceBE.API.Configurations.ColumnWriters;
using ECommerceBE.API.Extensions;
using ECommerceBE.Application;
using ECommerceBE.Application.Validators.Products;
using ECommerceBE.Infrastructure;
using ECommerceBE.Infrastructure.Filters;
using ECommerceBE.Infrastructure.Services.Storage.Azure;
using ECommerceBE.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
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



            //serilog mekanizmasý
            Logger log = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt")
                .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"), "logs",
                    needAutoCreateTable: true,
                    columnOptions: new Dictionary<string, ColumnWriterBase>
                    {
                        {"message", new RenderedMessageColumnWriter()},
                        {"message_template", new MessageTemplateColumnWriter()},
                        {"level", new LevelColumnWriter()},
                        {"time_stamp", new TimestampColumnWriter()},
                        {"exception", new ExceptionColumnWriter()},
                        {"log_event", new LogEventSerializedColumnWriter()},
                        {"user_name", new UsernameColumnWriter()}
                    })
                .WriteTo.Seq(builder.Configuration["Seq:ServerURL"])
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .CreateLogger();

            //built-in'deki log mekanýzmasý ile serilog deðiþtirildi. Artýk built-indeki kullanýlmýcak.
            builder.Host.UseSerilog(log);

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;

            });



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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
                        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

                        //JWT üzerinde Name claimne karþýlýk gelen deðeri User.Identity.Name propertysinden elde edebiliriz.
                        NameClaimType = ClaimTypes.Name
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            //Global exception handler
            app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());

            //wwwroot dizinine eriþebilmek için eklenmeli.
            app.UseStaticFiles();

            //kendisinden sonraki middlewareleri loglatýyor.
            app.UseSerilogRequestLogging();

            app.UseHttpLogging();

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;

                LogContext.PushProperty("user_name", username);

                await next();
            });


            app.MapControllers();

            app.Run();
        }
    }
}