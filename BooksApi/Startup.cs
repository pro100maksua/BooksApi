using BooksApi.Data;
using BooksApi.Data.Interfaces;
using BooksApi.Entities;
using BooksApi.Logic.Interfaces;
using BooksApi.Logic.Services;
using BooksApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace BooksApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(c =>
                {
                    c.RegisterValidatorsFromAssemblyContaining<Startup>();
                    c.LocalizationEnabled = false;
                });

            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Books"));

            services.AddTransient<IBooksService, BooksService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IRepository<Book>, Repository<Book>>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "BooksApi", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BooksApi"));

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
