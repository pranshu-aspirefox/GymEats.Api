using AutoMapper;
using GymEats.Data;
using GymEats.Data.Entity;
using GymEats.Services.Auth;
using GymEats.Services.Blob;
using GymEats.Services.Common;
using GymEats.Services.Diet;
using GymEats.Services.Mealme;
using GymEats.Services.Nutritionix;
using GymEats.Services.Option;
using GymEats.Services.Question;
using GymEats.Services.Repository.Common;
using GymEats.Services.Seed;
using GymEats.Services.Service;
using GymEats.Services.Suggestic;
using GymEats.Services.Survey;
using GymEats.Services.UserAddress;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace GymEats.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMvc();
            builder.Services.AddControllers();
            builder.Services.AddScoped<SeedData>();

            // Add services for identity
            var configuration = builder.Configuration;
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();

                });

            });
            builder.Services.AddDbContext<GymEatsDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });
            builder.Services.AddIdentity<User, IdentityRole>(
                option =>
                {
                    option.Password.RequireDigit = false;
                    option.Password.RequiredLength = 5;
                    option.Password.RequireLowercase = false;
                    option.Password.RequireNonAlphanumeric = false;
                    option.Password.RequireUppercase = false;
                    option.SignIn.RequireConfirmedEmail = true;
                    option.SignIn.RequireConfirmedEmail = true;
                }
            ).AddEntityFrameworkStores<GymEatsDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<ICommonService, CommonService>();
            builder.Services.AddTransient<IBlobService, BlobService>();
            builder.Services.AddTransient<ISurveyService, SurveyService>();
            builder.Services.AddTransient<IDietService, DietService>();
            builder.Services.AddTransient<INutritionixService, NutritionixService>();
            builder.Services.AddTransient<IGenericRepository<ShoppingList>, GenericRepository<ShoppingList>>();
            builder.Services.AddTransient<IGenericRepository<Question>, GenericRepository<Question>>();
            builder.Services.AddTransient<IGenericRepository<Option>, GenericRepository<Option>>();
            builder.Services.AddTransient<IGenericRepository<Survey>, GenericRepository<Survey>>();
            builder.Services.AddTransient<IGenericRepository<Diet>, GenericRepository<Diet>>();
            builder.Services.AddTransient<IGenericRepository<UserAddress>, GenericRepository<UserAddress>>();
            builder.Services.AddTransient<IGenericRepository<SurveyOption>, GenericRepository<SurveyOption>>();
            builder.Services.AddTransient<IGenericRepository<SurveyQuestionMapping>, GenericRepository<SurveyQuestionMapping>>();
            builder.Services.AddTransient<IGenericRepository<UserDetails>, GenericRepository<UserDetails>>();
            builder.Services.AddTransient<IGenericRepository<SurveyQuestion>, GenericRepository<SurveyQuestion>>();
            builder.Services.AddTransient<IUserShoppingService, UserShoppingService>();
            builder.Services.AddTransient<IQuestionService, QuestionService>();
            builder.Services.AddTransient<IOptionService, OptionService>();
            builder.Services.AddTransient<ISurveyService, SurveyService>();
            builder.Services.AddTransient<IUserAddressService, UserAddressService>();

            builder.Services.AddScoped<SuggesticApiService>();
            builder.Services.AddScoped<IMealmeService, MealmeService>();
            builder.Services.AddScoped<SurveyQuestionMapppingService>();

            builder.Services.AddOptions();
            builder.Services.AddHttpClient();

            //Add services for jwt
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
        
            });
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Gym Eats", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            var seeder = new Seeder(app);

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();

        }
        public class Seeder
        {
            public Seeder(WebApplication app)
            {
                using (var scope = app.Services.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<SeedData>();
                    seeder?.SeedAsync().Wait();
                }
            }
        }
    }

}
