using Com.Scm.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace Com.Scm;

public static class SwaggerExtension
{
    public static void SwaggerSetup(this IServiceCollection services, SwaggerConfig config)
    {
        if (config == null)
        {
            return;
        }

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(s =>
        {
            foreach (var doc in config.ApiDocs)
            {
                s.SwaggerDoc(doc.Group, new OpenApiInfo
                {
                    Version = doc.Version,
                    Title = doc.Title,
                    Description = doc.Description,
                });
            }

            s.OrderActionsBy(o => o.RelativePath);

            //Add Xml
            foreach (var file in config.DllXmls)
            {
                var path = Path.Combine(AppContext.BaseDirectory, file);
                if (File.Exists(path))
                {
                    s.IncludeXmlComments(path, true);
                }
            }

            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "accessToken",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            //s.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = "Bearer"
            //            }
            //        },
            //        new string[] {}
            //    }
            //});
        });
    }

    public static void UseSwaggerSetup(this IApplicationBuilder app, SwaggerConfig config)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }
        if (config == null)
        {
            return;
        }

        app.UseSwagger();
        // Swagger UI
        app.UseSwaggerUI(c =>
        {
            foreach (var doc in config.ApiDocs)
            {
                c.SwaggerEndpoint($"/swagger/{doc.Group}/swagger.json", doc.Title);
            }
        });
        // FytApiUI
        //app.UseFytApiUI(c =>
        //{
        //    foreach (var doc in config.ApiDocs)
        //    {
        //        c.SwaggerEndpoint($"/swagger/{doc.Group}/swagger.json", doc.Title, doc.Version);
        //    }
        //});
    }
}