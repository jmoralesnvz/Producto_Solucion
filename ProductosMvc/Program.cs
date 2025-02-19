using ProductosMvc.Controllers;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Habilitar Controladores y Vistas (MVC)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// 🔹 Configurar HttpClient con un Timeout de 30 segundos
builder.Services.AddHttpClient<ProductosController>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30); // ⏳ Aumentar tiempo de espera
});

// 🔹 Configurar CORS para permitir llamadas desde la API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// 🔹 Configurar Middleware para MVC y Razor Pages
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    // 🔹 Manejo de excepciones para ver errores en desarrollo
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 🔹 Activar CORS
app.UseCors("AllowAllOrigins");

// 🔹 Mapear Controladores (MVC) y Razor Pages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}"); // ✅ ProductosController.Index() será la ruta predeterminada

app.MapRazorPages();

app.Run();
