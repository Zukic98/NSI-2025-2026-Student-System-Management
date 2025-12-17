using Analytics.Infrastructure; // Potrebno za AddAnalyticsModule

var builder = WebApplication.CreateBuilder(args);

// 1. DODAJ PODRŠKU ZA KONTROLERE (bez ovoga AnalyticsController ne?e raditi)
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. REGISTRUJ SVOJ MODUL (poziva metodu iz ServiceCollectionExtensions)
builder.Services.AddAnalyticsModule();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

// 3. DODAJ RUTIRANJE ZA KONTROLERE
app.MapControllers();

// Opcionalno: Možeš obrisati MapGet("/weatherforecast") ako ti ne treba

app.Run();