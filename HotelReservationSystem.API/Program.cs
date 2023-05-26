using HotelReservationSystem.API.Data;
using HotelReservationSystem.API.Interfaces;
using HotelReservationSystem.API.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IHotelCustomerRepository, HotelCustomerRepository>();
builder.Services.AddTransient<IHotelReservationRepository, HotelReservationRepository>();
builder.Services.AddTransient<IHotelReviewsRepository, HotelReviewsRepository>();
builder.Services.AddTransient<IHotelRoomRepository, HotelRoomRepository>();

builder.Services.AddDbContext<HotelContext>(opt =>
{
    var conStrBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("myDb"));

    conStrBuilder.UserID = builder.Configuration.GetConnectionString("userid");

    conStrBuilder.Password = builder.Configuration.GetConnectionString("password");

    opt.UseSqlServer(conStrBuilder.ConnectionString);
}
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
