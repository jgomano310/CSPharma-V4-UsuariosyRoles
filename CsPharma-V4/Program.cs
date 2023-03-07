using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CsPharma_V4.Areas.Identity.Data;
using CsPharma_V4.repositorios;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();


builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<CsPharmaV4Context>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("EFCConexion"));
    });
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<LoginContexto>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("EFCConexion"));
    });


//activa los roles
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>()

  .AddEntityFrameworkStores<LoginContexto>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

AddScope();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
void AddScope()
{
    builder.Services.AddScoped<InterfazUsuario, RepoUsuario>();
    builder.Services.AddScoped<InterfazRoles, RepoRoles>();
    builder.Services.AddScoped<UnidadDeTrabajo, UnidadDeTrabajoRepo>();
}
