using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ScrumPoker.Data;
using ScrumPoker.Services;
using ScrumPoker.SignalR;

namespace ScrumPoker
{
  /// <summary>
  /// ������������ ����������.
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// ����������� ������ Startup.
    /// </summary>
    /// <param name="configuration">������� ���������� ������������.</param>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    /// <summary>
    /// ������� ���������� ������������.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// ������������ ��������.
    /// </summary>
    /// <param name="services">��������� ��������.</param>
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      string connection = Configuration.GetConnectionString("DefaultConnection");
      services.AddDbContext<ModelContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);
      services.AddSingleton<UserService>();
      services.AddSingleton<RoomService>();
      services.AddSingleton<DeckService>();
      services.AddSingleton<RoundService>();
      services.AddSingleton<RoundCardService>();

      services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", options =>
       {
         options.LoginPath = "/user/auf";

         options.Cookie = new CookieBuilder
         {
           Name = "HUI",
           HttpOnly = true,
           Path = "/",
           SameSite = SameSiteMode.Lax,
           SecurePolicy = CookieSecurePolicy.None
         };
         options.SlidingExpiration = true;
       });



      services.AddAuthorization();
      services.AddSignalR();

    }
    /// <summary>
    /// ������������ HTTP.
    /// </summary>
    /// <param name="app">������� ���������� �������.</param>
    /// <param name="env">�������� ���������� ���������.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();



        app.UseHttpsRedirection();

        app.UseRouting();


        app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials());
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCookiePolicy(new CookiePolicyOptions
        {
          HttpOnly = HttpOnlyPolicy.None,
          Secure = CookieSecurePolicy.None
        });

        app.UseEndpoints(endpoints =>
        {
          endpoints.MapControllers();
          endpoints.MapHub<RoomsHub>("/roomsHub");
        });
      }
    }
  }
}



