using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using ArbitraryCollectionMgmt.DAL;
using Microsoft.EntityFrameworkCore;
using ArbitraryCollectionMgmt.BLL.Hubs;
using Microsoft.AspNetCore.SignalR;
using ArbitraryCollectionMgmt.Web.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<SalesforceClient>();
builder.Services.AddSingleton<JiraClient>();

builder.Services.AddHttpContextAccessor();

//builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); 

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseCookiePolicy();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<CommentHub>("/hub/commentHub");

app.Run();
