using ECommerceBE.API.Configurations.ColumnWriters;
using ECommerceBE.API.Extensions;
using ECommerceBE.API.Filters;
using ECommerceBE.Application;
using ECommerceBE.Application.Validators.Products;
using ECommerceBE.Infrastructure;
using ECommerceBE.Infrastructure.Filters;
using ECommerceBE.Infrastructure.Services.Storage.Azure;
using ECommerceBE.Persistence;
using ECommerceBE.SignalR;
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
            builder.Services.AddHttpContextAccessor(); // client'tan gele nrequest neticesinde oluşturulan HTTPcontext nesnesine katmanlardaki class'lar üzerinden(business logic) erişebilmemizi sağlayan bir servistir.
            builder.Services.AddPersistenceServices();
            builder.Services.AddInfrastructureServices();
            builder.Services.AddApplicationServices();
            builder.Services.AddSignalRServices();

            //builder.Services.AddStorage();
            //aşağıdaki enum ile olan tanımlamayı genelde kullanmıyoruz(yeni nesneler için koda müdahale olduğundan ötürü), generic yapı kullanılıyor.
            //builder.Services.AddStorage(StorageType.Local);
            //builder.Services.AddStorage<LocalStorage>();
            builder.Services.AddStorage<AzureStorage>();

            builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
                policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                .AllowAnyHeader().AllowAnyMethod().AllowCredentials()
            ));



            //serilog mekanizması
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

            //built-in'deki log mekanızması ile serilog değiştirildi. Artık built-indeki kullanılmıcak.
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


            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
                options.Filters.Add<RolePermissionFilter>();
            })
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
                        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

                        //JWT üzerinde Name claimne karşılık gelen değeri User.Identity.Name propertysinden elde edebiliriz.
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


            //wwwroot dizinine erişebilmek için eklenmeli.
            app.UseStaticFiles();

            //kendisinden sonraki middlewareleri loglatıyor.
            app.UseSerilogRequestLogging();

            app.UseHttpLogging();

            //Global exception handler (Seriallog MW'sinden sonra eklendi. önce eklenirse loglanmıyor!!)
            app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());

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

            app.MapHubs();

            app.Run();
        }
    }
}