using VillaApi_Web;
using VillaApi_Web.Services.IServices;
using VillaApi_Web.Services.VillAServices;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//config automapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

//HttpClient
builder.Services.AddHttpClient<IVillaService,VillaService>();
builder.Services.AddScoped<IVillaService,VillaService>();
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

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
