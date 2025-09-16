var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

#region Dependency Injection

builder.Services.AddDomainModuleDependencies()
        .AddInfrastructureModuleDependencies()
        .AddServicesModuleDependencies()
        .AddApiModuleDependencies(builder.Configuration);

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

app.UseSwagger();
app.UseSwaggerUI();



app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
