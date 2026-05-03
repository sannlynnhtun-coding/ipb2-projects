using IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JSON to ignore cycles (Important for Results -> Student relationship)
builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ==========================================
// STUDENT ENDPOINTS (Soft Delete)
// ==========================================
var students = app.MapGroup("/api/students");

students.MapGet("/", async (AppDbContext db) =>
    await db.Students.Where(s => s.IsDelete != true).ToListAsync());

students.MapGet("/{id}", async (int id, AppDbContext db) =>
    await db.Students.FirstOrDefaultAsync(s => s.Id == id && s.IsDelete != true)
        is Student s ? Results.Ok(s) : Results.NotFound());

students.MapPost("/", async (Student student, AppDbContext db) => {
    student.IsDelete = false;
    db.Students.Add(student);
    await db.SaveChangesAsync();
    return Results.Created($"/api/students/{student.Id}", student);
});

students.MapDelete("/{id}", async (int id, AppDbContext db) => {
    var student = await db.Students.FindAsync(id);
    if (student is null) return Results.NotFound();

    student.IsDelete = true;
    await db.SaveChangesAsync();
    return Results.Ok("Student soft-deleted.");
});

// ==========================================
// SUBJECT ENDPOINTS (Search & Soft Delete)
// ==========================================
var subjects = app.MapGroup("/api/subjects");

subjects.MapGet("/search/{name}", async (string name, AppDbContext db) =>
    await db.Subjects.FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower() && !s.IsDelete)
        is Subject s ? Results.Ok(s) : Results.NotFound());

subjects.MapDelete("/{id}", async (int id, AppDbContext db) => {
    var subject = await db.Subjects.FindAsync(id);
    if (subject is null || subject.IsDelete) return Results.NotFound();

    subject.IsDelete = true;
    await db.SaveChangesAsync();
    return Results.Ok("Subject soft-deleted.");
});

// ==========================================
// RESULT ENDPOINTS (Pagination & Auto-Grade)
// ==========================================
var results = app.MapGroup("/api/results");

// List with Pagination
results.MapGet("/list", async (AppDbContext db, int pageNo = 1, int pageSize = 10) => {
    var query = db.Results.Include(r => r.Student).Include(r => r.Subject).AsNoTracking();
    var totalRecords = await query.CountAsync();
    var data = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

    return Results.Ok(new { TotalCount = totalRecords, Data = data });
});

// Create with Auto-Grade Logic
results.MapPost("/", async (Result result, AppDbContext db) => {
    result.Grade = result.Marks switch
    {
        >= 90 => "A+",
        >= 80 => "A",
        >= 70 => "B",
        >= 60 => "C",
        >= 50 => "D",
        _ => "F"
    };

    db.Results.Add(result);
    await db.SaveChangesAsync();
    return Results.Created($"/api/results/{result.Id}", result);
});

app.Run();