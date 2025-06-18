


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession();  // added by Sudipta on 3.4.25

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}
app.UseRouting();
app.MapControllers(); 

app.UseEndpoints(endpoints =>
{
  endpoints.MapControllers(); // <- Required
  endpoints.MapDefaultControllerRoute();
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // added by Sudipta on 3.4.25
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FileExplorer}/{action=Index}/{id?}");

// If using Minimal APIs



app.Run();
